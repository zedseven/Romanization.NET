using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Romanization.Internal
{
	/// <summary>
	/// Effectively a <see cref="Regex"/> implementation of <see cref="ReplaceOperations.ReplaceWithSameCase"/>.<br />
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
		private readonly DynamicReplacement _dynamicReplacement;
		private readonly bool _countCapturesInCasing;

		public CaseAwareSub(string pattern, string dynamicReplacement, bool countCapturesInCasing = false)
		{
			_findRegex    = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			_dynamicReplacement = new DynamicReplacement(dynamicReplacement);
			_countCapturesInCasing = countCapturesInCasing;
		}

		public CaseAwareSub(string pattern, string dynamicReplacement, RegexOptions options, bool countCapturesInCasing = false)
		{
			_findRegex    = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | options);
			_dynamicReplacement = new DynamicReplacement(dynamicReplacement);
			_countCapturesInCasing = countCapturesInCasing;
		}

		public string Replace(string text)
		{
			StringBuilder result = new(text.Length);

			bool foundMatch = false;
			int startIndex = 0;
			Match match = _findRegex.Match(text);
			while (match.Success)
			{
				foundMatch = true;
				result.Append(text, startIndex, match.Index - startIndex);

				// Handle replacement
				DynamicReplacement.CasingMode casingMode = DynamicReplacement.CasingMode.LowerCase;
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
						casingMode = DynamicReplacement.CasingMode.LowerCase;
					else if (firstCharUpper && lastCharUpper)
						casingMode = DynamicReplacement.CasingMode.UpperCase;
					else if (firstCharUpper && !lastCharUpper)
						casingMode = DynamicReplacement.CasingMode.TitleCase;
					else
						casingMode = DynamicReplacement.CasingMode.ReverseTitleCase;
				}

				// Build the substitute string and add it to the result
				result.Append(_dynamicReplacement.Generate(casingMode,
					match.Groups.Values.Skip(1).Select(g => g.Value).ToArray()));

				startIndex = match.Index + match.Length;

				match = match.NextMatch();
			}

			// Append any remaining parts of the original text
			if (startIndex < text.Length)
				result.Append(text, startIndex, text.Length - startIndex);

			return foundMatch ? result.ToString() : text;
		}
	}
}
