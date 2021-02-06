using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Romanization.LanguageAgnostic
{
	internal interface ISub
	{
		public string Replace(string text);
	}

	internal class CharSub : ISub
	{
		private readonly Regex  _findRegex;
		private readonly string _substitution;

		public CharSub(string pattern, string substitution, bool ignoreCase = true)
		{
			_findRegex    = new Regex(pattern, ignoreCase ? RegexOptions.Compiled | RegexOptions.IgnoreCase : RegexOptions.Compiled);
			_substitution = substitution;
		}

		public CharSub(string pattern, string substitution, RegexOptions options)
		{
			_findRegex    = new Regex(pattern, RegexOptions.Compiled | options);
			_substitution = substitution;
		}

		public string Replace(string text)
			=> _findRegex.Replace(text, _substitution);
	}

	internal class CharSubCased : ISub
	{
		private readonly Regex  _findRegexUpper;
		private readonly Regex  _findRegexLower;
		private readonly string _substitutionUpper;
		private readonly string _substitutionLower;

		public CharSubCased(string patternUpper, string patternLower, string substitutionUpper, string substitutionLower)
		{
			_findRegexUpper    = new Regex(patternUpper, RegexOptions.Compiled);
			_findRegexLower    = new Regex(patternLower, RegexOptions.Compiled);
			_substitutionUpper = substitutionUpper;
			_substitutionLower = substitutionLower;
		}

		public CharSubCased(string patternUpper, string patternLower, string substitutionUpper, string substitutionLower, RegexOptions options)
		{
			_findRegexUpper    = new Regex(patternUpper, RegexOptions.Compiled | options);
			_findRegexLower    = new Regex(patternLower, RegexOptions.Compiled | options);
			_substitutionUpper = substitutionUpper;
			_substitutionLower = substitutionLower;
		}

		public string Replace(string text)
			=> _findRegexLower.Replace(_findRegexUpper.Replace(text, _substitutionUpper), _substitutionLower);
	}

	/// <summary>
	/// Effectively a <see cref="Regex"/> implementation of <see cref="Utilities.ReplaceWithSameCase"/>.<br />
	/// This could be implemented with a smarter case-checking algorithm (case checks are simply based on
	/// first &amp; last characters of the entire <see cref="Match"/>), but it's use here is only for small
	/// matches where this shouldn't be an issue, and so it isn't worth the implementation &amp; speed cost.<br />
	/// Substitutions should be in the format <c>blah$1blah$0blah$2</c>, and up to 10 captures are supported (0-9).<br />
	/// The input pattern is always matched with <see cref="RegexOptions.IgnoreCase"/> set.
	/// </summary>
	/// <remarks>This was originally created to be able to handle aspirated rho (ρ) with proper casing in Greek.</remarks>
	internal class CaseAwareSub : ISub
	{
		private readonly Regex _findRegex;
		private readonly Substitution _substitution;
		private readonly bool _countCapturesInCasing;

		public CaseAwareSub(string pattern, string substitution, bool countCapturesInCasing = false)
		{
			_findRegex    = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			_substitution = new Substitution(substitution);
			_countCapturesInCasing = countCapturesInCasing;
		}

		public CaseAwareSub(string pattern, string substitution, RegexOptions options, bool countCapturesInCasing = false)
		{
			_findRegex    = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | options);
			_substitution = new Substitution(substitution);
			_countCapturesInCasing = countCapturesInCasing;
		}

		public string Replace(string text)
		{
			StringBuilder result = new StringBuilder(text.Length);

			bool foundMatch = false;
			int startIndex = 0;
			Match match = _findRegex.Match(text);
			while (match.Success)
			{
				foundMatch = true;
				result.Append(text, startIndex, match.Index - startIndex);

				// Handle replacement
				Substitution.CasingMode casingMode = Substitution.CasingMode.LowerCase;
				// Start by looking for the first character not in a capture group in the match
				int searchStart = match.Index;
				int firstCharIndex = -1;
				int lastCharIndex = -1;
				if (!_countCapturesInCasing)
					for (int i = 1; i < match.Groups.Count; i++)
					{
						if (match.Groups[i].Index > searchStart)
						{
							firstCharIndex = searchStart;
							break;
						}
						searchStart += match.Groups[i].Length;
					}
				if (firstCharIndex < 0 && searchStart < match.Index + match.Length)
					firstCharIndex = searchStart;

				// If at least one character exists in the match that isn't in a capture group,
				// search for the last character next
				if (firstCharIndex > -1)
				{
					searchStart = match.Index + match.Length - 1;
					if (!_countCapturesInCasing)
						for (int i = match.Groups.Count - 1; i >= 1; i--)
						{
							if (match.Groups[i].Index + match.Groups[i].Length <= searchStart)
							{
								lastCharIndex = searchStart;
								break;
							}

							searchStart -= match.Groups[i].Length;
						}
					if (lastCharIndex < 0 && searchStart >= match.Index)
						lastCharIndex = searchStart;

					// Now that the positions of the first and last characters are known, determine
					// the casing format to use for the replacement
					bool firstCharUpper = char.IsUpper(text[firstCharIndex]);
					bool lastCharUpper = char.IsUpper(text[lastCharIndex]) &&
					                     (firstCharIndex != lastCharIndex ||
					                      (text.Length > match.Index + match.Length && char.IsUpper(text[match.Index + match.Length])) ||
					                      (match.Index > 0 && char.IsUpper(text[match.Index - 1])));
					if (!firstCharUpper && !lastCharUpper)
						casingMode = Substitution.CasingMode.LowerCase;
					else if (firstCharUpper && lastCharUpper)
						casingMode = Substitution.CasingMode.UpperCase;
					else if (firstCharUpper && !lastCharUpper)
						casingMode = Substitution.CasingMode.TitleCase;
					else
						casingMode = Substitution.CasingMode.ReverseTitleCase;
				}

				// Build the substitute string and add it to the result
				result.Append(_substitution.Generate(casingMode,
					match.Groups.Values.Skip(1).Select(g => g.Value).ToArray()));

				startIndex = match.Index + match.Length;

				match = match.NextMatch();
			}

			// Append any remaining parts of the original text
			if (startIndex < text.Length)
				result.Append(text, startIndex, text.Length - startIndex);

			return foundMatch ? result.ToString() : text;
		}

		private class Substitution
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

			public class CaptureCountMismatchException : ArgumentException
			{
				internal CaptureCountMismatchException() { }
				internal CaptureCountMismatchException(string message) : base(message) { }
				internal CaptureCountMismatchException(string message, Exception inner) : base(message, inner) { }
				internal CaptureCountMismatchException(string message, string paramName) : base(message, paramName) { }
				internal CaptureCountMismatchException(string message, string paramName, Exception inner) : base(message, paramName, inner) { }
			}

			public Substitution(string sub)
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
}
