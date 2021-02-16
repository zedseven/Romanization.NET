using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Romanization.Internal
{
	// TODO: A lot of these functions need to be either reworked or renamed, and all of them need documentation
	internal static class ReplaceOperations
	{
		[Pure]
		public static string ReplaceMany(this string text, params ISub?[] subs)
			=> subs.Aggregate(text, (str, sub) => sub?.Replace(str) ?? str);

		[Pure]
		public static string ReplaceFromChart(this string text, ReplacementChart chart)
			=> ReplaceFromChartCore(text, chart, false);

		[Pure]
		public static string ReplaceFromChartCaseAware(this string text, ReplacementChart chart)
			=> ReplaceFromChartCore(text, chart, true);

		[Pure]
		private static string ReplaceFromChartCore(string text, ReplacementChart chart, bool caseAware)
		{
			if (chart.Count <= 0)
				return text;
			StringBuilder result = new(text.Length);
			for (int i = 0; i < text.Length;)
			{
				int movedChars = 0;
				for (int j = text.Length - i < chart.LongestKeyLength ? text.Length - i : chart.LongestKeyLength; j >= 1; j--)
				{
					if (!chart.TryGetValue(text.Substring(i, j), out string? replacement))
						continue;
					if (!string.IsNullOrEmpty(replacement))
						if (caseAware)
							result.AppendReplacementWithCasing(text, i, j, replacement);
						else
							result.Append(replacement);
					movedChars = j;
					break;
				}
				if (movedChars <= 0)
				{
					result.Append(text[i]);
					movedChars = 1;
				}
				i += movedChars;
			}
			return result.ToString();
		}

		private static void AppendReplacementWithCasing(this StringBuilder result, string str, int foundAt,
			int oldValueLength, string newValue)
		{
			// The logic for this is a bit messy, but the additional checks in lastCharUpper are to determine if the
			// replacee should be counted as all-caps
			// This is to cover edge cases where str is something like ABCDRGHI, where R would otherwise be replaced
			// with Ef instead of EF
			// TODO: Potentially add support for checks past things like vowel ties (k͡s) - this will likely involve splitting into surrogate pairs, which may be expensive
			bool firstCharUpper = char.IsUpper(str[foundAt]);
			if (newValue.Length > 1)
			{
				bool lastCharUpper = char.IsUpper(str[foundAt + oldValueLength - 1]) &&
				                     (oldValueLength > 1 ||
				                      (str.Length > foundAt + 1 && char.IsUpper(str[foundAt + 1])) ||
				                      (foundAt > 0 && char.IsUpper(str[foundAt - 1])));
				switch (firstCharUpper)
				{
					// example
					case false when !lastCharUpper:
						result.Append(newValue.ToLower(CultureInfo.CurrentCulture));
						break;
					// EXAMPLE
					case true when lastCharUpper:
						result.Append(newValue.ToUpper(CultureInfo.CurrentCulture));
						break;
					// Example
					case true when !lastCharUpper:
						result.Append(char.ToUpper(newValue[0], CultureInfo.CurrentCulture));
						result.Append(newValue[1..].ToLower(CultureInfo.CurrentCulture));
						break;
					// examplE
					default:
						result.Append(newValue[..^1].ToLower(CultureInfo.CurrentCulture));
						result.Append(char.ToUpper(newValue[^1], CultureInfo.CurrentCulture));
						break;
				}
			}
			else
				result.Append(firstCharUpper
					? newValue.ToUpper(CultureInfo.CurrentCulture)
					: newValue.ToLower(CultureInfo.CurrentCulture));
		}

		// TODO: This needs to be removed and replaced with less assumptive methods
		/// <summary>
		/// Remove common alternative characters, such as the ideographic full-stop (replaced with a period).
		/// </summary>
		/// <param name="text">The text to replace in.</param>
		/// <returns>The original text with common alternate characters replaced.</returns>
		[Pure]
		internal static string ReplaceCommonAlternates(this string text)
			=> text.Replace(Constants.IdeographicFullStop, '.')
				.Replace(Constants.Interpunct, ' ');

		[Pure]
		internal static string WithoutChars(this string str, string withoutChars)
			=> str.ReplaceMultipleChars(withoutChars, null);

		[Pure]
		internal static string ReplaceMultipleChars(this string str, IEnumerable<char> chars, char? replacement)
		{
			StringBuilder result = new(str.Length);
			IEnumerable<char> charEnumerable = chars as char[] ?? chars.ToArray();
			for (int i = 0; i < str.Length; i++)
			{
				bool replaced = false;
				foreach (char c in charEnumerable)
					if (str[i] == c)
					{
						result.Append(replacement);
						replaced = true;
						break;
					}
				if (!replaced)
					result.Append(str[i]);
			}

			return result.ToString();
		}
	}
}
