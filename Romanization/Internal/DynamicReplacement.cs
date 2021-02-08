using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Romanization.Internal
{
	internal class DynamicReplacement
	{
		public readonly int ExpectedCaptureCount;

		private readonly Part[] _parts;
		private readonly int _verbatimPartCount;
		private readonly int _verbatimLength;

		private abstract class Part { }

		private class VerbatimPart : Part
		{
			public readonly string Text;

			public VerbatimPart(string text)
				=> Text = text;

			public override string ToString()
				=> Text;
		}

		private class SubstitutionPart : Part
		{
			public readonly int CaptureIndex;

			public SubstitutionPart(int captureIndex)
				=> CaptureIndex = captureIndex;

			public override string ToString()
				=> $"${CaptureIndex}";
		}

		public DynamicReplacement(string sub)
		{
			// Build a parts list by parsing the input string
			List<Part> parts = new List<Part>();
			int startIndex = 0;
			int foundAt;
			while ((foundAt = sub.IndexOf('$', startIndex)) != -1)
			{
				int ciIndex = foundAt + 1;
				parts.Add(new VerbatimPart(sub.Substring(startIndex, foundAt - startIndex)));
				if (int.TryParse($"{sub[ciIndex]}", out int captureIndex))
				{
					parts.Add(new SubstitutionPart(captureIndex));
					startIndex = ciIndex + 1;
				}
				else
					startIndex = ciIndex;
			}
			if (startIndex < sub.Length)
				parts.Add(new VerbatimPart(sub.Substring(startIndex, sub.Length - startIndex)));

			// Clean the parts list
			for (int i = parts.Count - 1; i >= 1; i--)
			{
				if (parts[i].GetType() != typeof(VerbatimPart))
				{
					ExpectedCaptureCount++;
					continue;
				}
				string currentPartText = ((VerbatimPart) parts[i]).Text;
				// If empty, remove the part
				if (currentPartText.Length <= 0)
				{
					parts.RemoveAt(i);
					continue;
				}
				// If the next part isn't also a verbatim part, then this one is good and can be counted
				if (parts[i - 1].GetType() != typeof(VerbatimPart))
				{
					_verbatimLength += currentPartText.Length;
					_verbatimPartCount++;
					continue;
				}
				// Otherwise we combine the current part with the next one
				parts[i - 1] = new VerbatimPart(((VerbatimPart) parts[i - 1]).Text + currentPartText);
				parts.RemoveAt(i);
			}
			if (parts.Count > 0 && parts[0].GetType() == typeof(VerbatimPart) && ((VerbatimPart) parts[0]).Text.Length <= 0)
				parts.RemoveAt(0);

			_parts = parts.ToArray();
		}

		public enum CasingMode
		{
			LowerCase,
			UpperCase,
			TitleCase,
			ReverseTitleCase
		}

		public string Generate(CasingMode casingMode, IList<string> captures)
		{
			if (captures.Count != ExpectedCaptureCount)
				throw new CaptureCountMismatchException(
					$"Number of captures provided ({captures.Count}) does not match the expected number ({ExpectedCaptureCount}).",
					nameof(captures));

			StringBuilder sb = new StringBuilder(_verbatimLength * 2);
			for (int i = 0, verbatimSoFar = 0; i < _parts.Length; i++)
			{
				Part part = _parts[i];
				if (part.GetType() == typeof(VerbatimPart))
				{
					string text = ((VerbatimPart) part).Text;
					if (casingMode == CasingMode.TitleCase && verbatimSoFar <= 0)
					{
						sb.Append(char.ToUpper(text[0], CultureInfo.CurrentCulture));
						sb.Append(text.Substring(1).ToLower(CultureInfo.CurrentCulture));
					}
					else if (casingMode == CasingMode.ReverseTitleCase && verbatimSoFar >= _verbatimPartCount - 1)
					{
						sb.Append(text.Substring(0, text.Length - 1).ToLower(CultureInfo.CurrentCulture));
						sb.Append(char.ToUpper(text[^1], CultureInfo.CurrentCulture));
					}
					else if (casingMode == CasingMode.UpperCase)
						sb.Append(text.ToUpper(CultureInfo.CurrentCulture));
					else
						sb.Append(text.ToLower(CultureInfo.CurrentCulture));
					verbatimSoFar++;
					continue;
				}

				sb.Append(captures[((SubstitutionPart) part).CaptureIndex]);
			}

			return sb.ToString();
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (Part part in _parts)
				sb.Append(part);
			return sb.ToString();
		}
	}
}
