using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Romanization.LanguageAgnostic
{
	internal static class Utilities
	{
		// Constants
		public static readonly string AssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		public static readonly string LanguageCharacterMapsPath = Path.Combine(AssemblyPath, "contentFiles", "any", "any", "LanguageCharacterMaps");

		// Exceptions
		/// <summary>
		/// Represents the error when the stream provided to <see cref="LoadCsvIntoDictionary{TKey,TVal}"/> cannot be read.
		/// </summary>
		public class CannotReadStreamException : Exception
		{
			public CannotReadStreamException() {}
			public CannotReadStreamException(string message) : base(message) {}
			public CannotReadStreamException(string message, Exception inner) : base(message, inner) {}
		}

		/// <summary>
		/// Represents an error that occurred during loading of a library CSV file.
		/// </summary>
		public class CsvLoadingException : Exception
		{
			public CsvLoadingException() {}
			public CsvLoadingException(string message) : base(message) {}
			public CsvLoadingException(string message, Exception inner) : base(message, inner) {}
		}

		/// <summary>
		/// Loads a language character map file into a dictionary, using the provided mapping functions to map CSV entries to dict keys &amp; values.
		/// </summary>
		/// <typeparam name="TKey">The type of the dictionary keys.</typeparam>
		/// <typeparam name="TVal">The type of the dictionary values.</typeparam>
		/// <param name="fileName">The file name of the language character map file.</param>
		/// <param name="dict">The dictionary to load into.</param>
		/// <param name="keyMapper">The function that maps CSV entry first values to dictionary <typeparamref name="TKey"/> values.</param>
		/// <param name="valueMapper">The function that maps CSV entry second values to dictionary <typeparamref name="TVal"/> values.</param>
		/// <exception cref="T:Romanization.LanguageAgnostic.Utilities.CannotReadStreamException">The provided stream cannot be read.</exception>
		/// <exception cref="T:Romanization.LanguageAgnostic.Utilities.CsvLoadingException">Unable to load the CSV file.</exception>
		public static void LoadCharacterMap<TKey, TVal>(string fileName, IDictionary<TKey, TVal> dict, Func<string, TKey> keyMapper, Func<string, TVal> valueMapper)
		{
			using FileStream csvStream = File.OpenRead(Path.Combine(LanguageCharacterMapsPath, fileName));
			csvStream.LoadCsvIntoDictionary(dict, keyMapper, valueMapper);
		}

		/// <summary>
		/// Loads a CSV file stream into a dictionary, using the provided mapping functions to map CSV entries to dict keys &amp; values.
		/// </summary>
		/// <typeparam name="TKey">The type of the dictionary keys.</typeparam>
		/// <typeparam name="TVal">The type of the dictionary values.</typeparam>
		/// <param name="stream">The CSV file stream.</param>
		/// <param name="dict">The dictionary to load into.</param>
		/// <param name="keyMapper">The function that maps CSV entry first values to dictionary <typeparamref name="TKey"/> values.</param>
		/// <param name="valueMapper">The function that maps CSV entry second values to dictionary <typeparamref name="TVal"/> values.</param>
		/// <exception cref="T:Romanization.LanguageAgnostic.Utilities.CannotReadStreamException">The provided stream cannot be read.</exception>
		/// <exception cref="T:Romanization.LanguageAgnostic.Utilities.CsvLoadingException">Unable to load the CSV file.</exception>
		public static void LoadCsvIntoDictionary<TKey, TVal>(this FileStream stream, IDictionary<TKey, TVal> dict, Func<string, TKey> keyMapper, Func<string, TVal> valueMapper)
		{
			if (!stream.CanRead)
				throw new CannotReadStreamException("The provided stream cannot be read.");

			try
			{
				using StreamReader reader = new StreamReader(stream);

				// Discard the first line, since it's simply the heading
				_ = reader.ReadLine();

				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine();
					if (string.IsNullOrWhiteSpace(line))
						continue;

					int commaIndex = line.IndexOf(',');
					if (commaIndex < 0)
						continue;

					dict[keyMapper(line.Substring(0, commaIndex))] =
						valueMapper(line.Substring(commaIndex + 1));
				}
			}
			catch (Exception e)
			{
				throw new CsvLoadingException("Unable to load the CSV file.", e);
			}
		}

		[Pure]
		private static string WithoutQuotes(this string str)
			=> str.Length >= 2 && str[0] == '"' && str[^1] == '"' ? str[1..^1] : str;

		[Pure]
		public static string[] SplitIntoSurrogatePairs(this string str)
		{
			List<string> retList = new List<string>(str.Length);
			for (int i = 0; i < str.Length; i++)
			{
				if (!char.IsSurrogatePair(str, i))
				{
					retList.Add(str[i].ToString());
					continue;
				}

				retList.Add(str[i].ToString() + str[++i]);
			}

			return retList.ToArray();
		}

		/// <summary>
		/// Replaces all instances of <paramref name="oldValue"/> with <paramref name="newValue"/> in <paramref name="str"/>,
		/// keeping the general casing the same while paying attention to the context (characters surrounding the replacement).<br />
		/// Examples of casing:<br />
		/// <list type="bullet">
		/// <item><description><c>C</c> → <c>Dd</c></description></item>
		/// <item><description><c>c</c> → <c>dd</c></description></item>
		/// <item><description>ABCD<c>R</c>GHI → ABCD<c>EF</c>GHI</description></item>
		/// <item><description>abcd<c>R</c>ghi → abcd<c>Ef</c>ghi</description></item>
		/// <item><description>abcd<c>r</c>ghi → abcd<c>ef</c>ghi</description></item>
		/// <item><description><c>AA</c> → <c>BBB</c></description></item>
		/// <item><description><c>Aa</c> → <c>Bbb</c></description></item>
		/// <item><description><c>aA</c> → <c>bbB</c></description></item>
		/// <item><description><c>aa</c> → <c>bbb</c></description></item>
		/// </list>
		/// NOTE: This function does all comparison and searching with the <see cref="CultureInfo.CurrentCulture"/> of <see cref="CultureInfo"/>.
		/// </summary>
		/// <param name="str">The string to replace in.</param>
		/// <param name="oldValue">The value to search for.</param>
		/// <param name="newValue">The value to replace with.</param>
		/// <param name="strComp">The string comparison type. Defaults to <see cref="StringComparison.CurrentCultureIgnoreCase"/>.</param>
		/// <returns>A string with all instances of <paramref name="oldValue"/> replaced with <paramref name="newValue"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="str"/> or <paramref name="oldValue"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException"><paramref name="oldValue"/> is of length <c>0</c>.</exception>
		/// <remarks>The original code for this is from https://stackoverflow.com/a/45756981 </remarks>
		[Pure]
		public static string ReplaceWithSameCase(this string str, string oldValue, string newValue, StringComparison strComp = StringComparison.CurrentCultureIgnoreCase)
		{
			// Check inputs.
			if (str == null)
				throw new ArgumentNullException(nameof(str));
			if (str.Length == 0)
				return str;
			if (oldValue == null)
				throw new ArgumentNullException(nameof(oldValue));
			if (oldValue.Length == 0)
				throw new ArgumentException("String cannot be of zero length.");

			// Prepare string builder for storing the processed string.
			StringBuilder resultStringBuilder = new StringBuilder(str.Length);

			// Analyze the replacement: replace or remove.
			bool isReplacementNullOrEmpty = string.IsNullOrEmpty(newValue);

			// Replace all values.
			const int valueNotFound = -1;
			int foundAt;
			int startSearchFromIndex = 0;
			while ((foundAt = str.IndexOf(oldValue, startSearchFromIndex, strComp)) != valueNotFound)
			{
				// Append all characters until the found replacement.
				int charsUntilReplacement = foundAt - startSearchFromIndex;
				if (charsUntilReplacement != 0)
					resultStringBuilder.Append(str, startSearchFromIndex, charsUntilReplacement);

				// Process the replacement.
				if (!isReplacementNullOrEmpty)
				{
					// The logic for this is a bit messy, but the additional checks in lastCharUpper are to determine if the replacee should be counted as all-caps
					// This is to cover edge cases where str is something like ABCDRGHI, where R would otherwise be replaced with Ef instead of EF
					// TODO: Potentially add support for checks past things like vowel ties (k͡s) - this will likely involve splitting into surrogate pairs, which may be expensive
					bool firstCharUpper = char.IsUpper(str[foundAt]);
					if (newValue.Length > 1)
					{
						bool lastCharUpper = char.IsUpper(str[foundAt + oldValue.Length - 1]) &&
						                     (oldValue.Length > 1 ||
						                      (str.Length > foundAt + 1 && char.IsUpper(str[foundAt + 1])) ||
						                      (foundAt > 0 && char.IsUpper(str[foundAt - 1])));
						// example
						if (!firstCharUpper && !lastCharUpper)
							resultStringBuilder.Append(newValue.ToLower(CultureInfo.CurrentCulture));
						// EXAMPLE
						else if (firstCharUpper && lastCharUpper)
							resultStringBuilder.Append(newValue.ToUpper(CultureInfo.CurrentCulture));
						// Example
						else if (firstCharUpper && !lastCharUpper)
						{
							resultStringBuilder.Append(char.ToUpper(newValue[0], CultureInfo.CurrentCulture));
							resultStringBuilder.Append(newValue.Substring(1).ToLower(CultureInfo.CurrentCulture));
						}
						// examplE
						else
						{
							resultStringBuilder.Append(newValue.Substring(0, newValue.Length - 1)
								.ToLower(CultureInfo.CurrentCulture));
							resultStringBuilder.Append(char.ToUpper(newValue[^1], CultureInfo.CurrentCulture));
						}
					}
					else
						resultStringBuilder.Append(firstCharUpper
							? newValue.ToUpper(CultureInfo.CurrentCulture)
							: newValue.ToLower(CultureInfo.CurrentCulture));
				}

				// Prepare start index for the next search.
				// This needed to prevent infinite loop, otherwise method always start search
				// from the start of the string. For example: if an oldValue == "EXAMPLE", newValue == "example"
				// and comparisonType == "any ignore case" will conquer to replacing:
				// "EXAMPLE" to "example" to "example" to "example" … infinite loop.
				startSearchFromIndex = foundAt + oldValue.Length;
				if (startSearchFromIndex == str.Length)
				{
					// It is end of the input string: no more space for the next search.
					// The input string ends with a value that has already been replaced.
					// Therefore, the string builder with the result is complete and no further action is required.
					return resultStringBuilder.ToString();
				}
			}

			// Append the last part to the result.
			int charsUntilStringEnd = str.Length - startSearchFromIndex;
			resultStringBuilder.Append(str, startSearchFromIndex, charsUntilStringEnd);

			return resultStringBuilder.ToString();
		}

		[Pure]
		public static string UnicodeNormalize(this string str)
			=> str.Normalize(NormalizationForm.FormD);

		[Pure]
		public static string ReplaceMany(this string text, params ISub[] subs)
			=> subs.Aggregate(text, (str, sub) => sub.Replace(str));

		[Pure]
		public static string ReplaceFromChart(this string text, Dictionary<string, string> chart, StringComparison strComp = StringComparison.CurrentCulture)
			=> chart.Keys.Aggregate(text, (current, key)
				=> current.Replace(key, chart[key], strComp));

		[Pure]
		public static string ReplaceFromChartWithSameCase(this string text, Dictionary<string, string> chart, StringComparison strComp = StringComparison.CurrentCultureIgnoreCase)
			=> chart.Keys.Aggregate(text, (current, key)
				=> current.ReplaceWithSameCase(key, chart[key], strComp));

		private const string LanguageBoundaryChars = @"a-z";
		private static readonly Lazy<CharSub> LanguageBoundarySubstitution = new Lazy<CharSub>(() =>
			new CharSub(
				$"(?:([{LanguageBoundaryChars}{Constants.Punctuation}])([^ {LanguageBoundaryChars}{Constants.Punctuation}])|([^ {LanguageBoundaryChars}{Constants.Punctuation}])([{LanguageBoundaryChars}]))",
				"${1}${3} ${2}${4}"));

		/// <summary>
		/// Remove common alternative characters, such as the ideographic full-stop (replaced with a period).
		/// </summary>
		/// <param name="text">The text to replace in.</param>
		/// <returns>The original text with common alternate characters replaced.</returns>
		[Pure]
		internal static string ReplaceCommonAlternates(this string text)
			=> text.Replace(Constants.IdeographicFullStop, '.')
				.Replace(Constants.Interpunct, ' ');

		/// <summary>
		/// Insert spaces at boundaries between Latin and non-Latin characters (ie. <c>ニンテンドーDSiブラウザー</c> -> <c>ニンテンドー DSi ブラウザー</c>).
		/// </summary>
		/// <param name="text">The text to insert spaces in.</param>
		/// <returns>The text with spaces inserted at language boundaries.</returns>
		[Pure]
		internal static string SeparateLanguageBoundaries(this string text)
			=> LanguageBoundarySubstitution.Value.Replace(text);

		[Pure]
		internal static string WithoutChars(this string charset, string withoutChars, StringComparison strComp = StringComparison.Ordinal)
			=> withoutChars.Aggregate(charset, (set, withoutChar) => set.Replace($"{withoutChar}", "", strComp));

		[Pure]
		internal static bool ContainsAnyOf(this string text, IEnumerable<char> set)
		{
			for (int i = 0; i < text.Length; i++)
				foreach (char entry in set)
				{
					if (text[i] == entry)
						return true;
				}

			return false;
		}

		[Pure]
		internal static string ReplaceMultipleChars(this string str, IEnumerable<char> chars, char replacement)
		{
			StringBuilder result = new StringBuilder(str.Length);
			foreach (char s in str)
			{
				bool replaced = false;
				foreach (char c in chars)
					if (s == c)
					{
						result.Append(replacement);
						replaced = true;
						break;
					}
				if (!replaced)
					result.Append(s);
			}

			return result.ToString();
		}

		[Pure]
		internal static TResult? DetermineResultFromString<TResult>(this string text,
			params (TResult res, string[] set)[] possibleKinds)
			where TResult : struct
		{
			foreach ((TResult res, string[] set) possibleKind in possibleKinds)
				if (possibleKind.set.Any(text.Contains))
					return possibleKind.res;
			return null;
		}

		/// <summary>
		/// Runs <paramref name="func"/> with a specified <paramref name="culture"/>, then reverts the thread culture
		/// back to what it was before.
		/// </summary>
		/// <param name="culture">The culture to run with.</param>
		/// <param name="func">The function to run. Note that this takes a culture as an argument: this is the culture
		/// the thread was using before.</param>
		/// <typeparam name="TRes">The type of the result of running <paramref name="func"/>.</typeparam>
		/// <returns>The result of <paramref name="func"/>.</returns>
		public static TRes RunWithCulture<TRes>(CultureInfo culture, Func<CultureInfo, TRes> func)
		{
			CultureInfo previousCulture = CultureInfo.CurrentCulture;
			if (Equals(previousCulture, culture))
				return func(previousCulture);

			TRes res;
			try
			{
				CultureInfo.CurrentCulture = culture;
				res = func(previousCulture);
			}
			finally
			{
				CultureInfo.CurrentCulture = previousCulture;
			}
			return res;
		}

		/// <summary>
		/// Runs <paramref name="func"/>, then reverts the thread culture back to what it was before.
		/// </summary>
		/// <param name="culture">The culture to run with.</param>
		/// <param name="func">The function to run.</param>
		/// <typeparam name="TRes">The type of the result of running <paramref name="func"/>.</typeparam>
		/// <returns>The result of <paramref name="func"/>.</returns>
		public static TRes RunWithCulture<TRes>(CultureInfo culture, Func<TRes> func)
		{
			CultureInfo previousCulture = CultureInfo.CurrentCulture;
			if (Equals(previousCulture, culture))
				return func();

			TRes res;
			try
			{
				CultureInfo.CurrentCulture = culture;
				res = func();
			}
			finally
			{
				CultureInfo.CurrentCulture = previousCulture;
			}
			return res;
		}

		/// <summary>
		/// Runs <paramref name="action"/>, then reverts the thread culture back to what it was before.
		/// </summary>
		/// <param name="culture">The culture to run with.</param>
		/// <param name="action">The action to run.</param>
		public static void RunWithCulture(CultureInfo culture, Action action)
		{
			CultureInfo previousCulture = CultureInfo.CurrentCulture;
			if (Equals(previousCulture, culture))
			{
				action();
				return;
			}

			try
			{
				CultureInfo.CurrentCulture = culture;
				action();
			}
			finally
			{
				CultureInfo.CurrentCulture = previousCulture;
			}
		}

		/// <summary>
		/// Converts an integer into Roman numerals.<br />
		/// <paramref name="num"/> must be non-negative, and only numbers up to <c>3999</c> are expected - beyond that
		/// there is no standard, and roman numerals shouldn't be necessary.
		/// </summary>
		/// <param name="num">The number to convert.</param>
		/// <returns>The value of <paramref name="num"/>, encoded in Roman numerals.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="num"/> is negative.</exception>
		[Pure]
		public static string ToRomanNumerals(this decimal num)
		{
			if (num < 0)
				throw new ArgumentOutOfRangeException(nameof(num), num, "Must be a non-negative value");

			// Zero doesn't have an actual value in the system, but N was commonly used in it's place and this is better
			// than returning an empty string
			if (num == 0)
				return "N";

			// Value maps
			const decimal closenessMargin = (decimal) 1/20736; // 1 order of magnitude (12^4) smaller than the smallest value
			Dictionary<int, char> valueRepresentations = new Dictionary<int, char>
			{
				{ 1000, 'M' },
				{  500, 'D' },
				{  100, 'C' },
				{   50, 'L' },
				{   10, 'X' },
				{    5, 'V' },
				{    1, 'I' }
			};
			Dictionary<int, int> subtractiveSteps = new Dictionary<int, int>
			{
				{ 1000, 100 },
				{  500, 100 },
				{  100,  10 },
				{   50,  10 },
				{   10,   1 },
				{    5,   1 }
			};
			Dictionary<decimal, char> fractionRepresentations = new Dictionary<decimal, char>
			{
				{ (decimal)   6/12, 'S' },
				{ (decimal)   5/12, '⁙' },
				{ (decimal)   4/12, '∷' },
				{ (decimal)   3/12, '∴' },
				{ (decimal)   2/12, ':' },
				{ (decimal)   1/12, '·' },
				{ (decimal)   1/24, 'Є' },
				{ (decimal)   1/48, 'Ɔ' },
				{ (decimal)   1/72, 'Ƨ' },
				{ (decimal)  1/288, '℈' },
				{ (decimal) 1/1728, '»' }
			};
			char[] dots = { '·', ':', '∴', '∷', '⁙' };

			StringBuilder result = new StringBuilder(5);
			int numToAdd;

			// Whole numbers
			foreach ((int value, char representation) in valueRepresentations)
			{
				if ((numToAdd = (int) (num / value)) >= 1)
				{
					num -= numToAdd * value;
					for (int i = 0; i < numToAdd; i++)
						result.Append(representation);
				}

				// Check if the remaining value is close to another instance of the current numeral since
				// it can be written more concisely with subtractive notation (ie. IV instead of IIII)
				if (!subtractiveSteps.TryGetValue(value, out int step) || (int) ((float) (num + step) / value) < 1)
					continue;

				num -= value - step;
				result.Append(valueRepresentations[step]);
				result.Append(representation);
			}

			// Fractions, to the nearest value the Roman system supports
			if (num == 0)
				return result.ToString();
			foreach ((decimal value, char representation) in fractionRepresentations)
			{
				if ((numToAdd = (int) ((num + closenessMargin) / value)) < 1)
					continue;
				num -= numToAdd * value;
				for (int i = 0; i < numToAdd; i++)
					result.Append(representation);
			}
			// Place dots after symbols, as that looks cleaner and is closer to Є·
			int length = result.Length;
			for (int i = 0; i < length; i++)
			{
				if (!dots.Contains(result[i]))
					continue;
				result.Append(result[i]);
				result.Remove(i, 1);
				i--;
				length--;
			}

			return result.ToString();
		}

		/// <summary>
		/// Converts an integer into Arabic numerals for display.
		/// </summary>
		/// <param name="num">The number to convert.</param>
		/// <param name="culture">The culture to use for display.</param>
		/// <param name="decimalPlaces">The number of decimal places to round to.</param>
		/// <returns>The value of <paramref name="num"/>, encoded in Arabic numerals.</returns>
		public static string ToArabicNumerals(this decimal num, CultureInfo culture, int decimalPlaces = 2)
		{
			num = decimal.Round(num, decimalPlaces, MidpointRounding.AwayFromZero);
			return num.ToString(culture);
		}

		public static string ToRomanNumerals(this int num)
			=> ((decimal) num).ToRomanNumerals();
	}
}
