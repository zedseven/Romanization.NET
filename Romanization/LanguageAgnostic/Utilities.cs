using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

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

		[Pure]
		public static string ReplaceMany(this string text, params ISub[] subs)
			=> subs.Aggregate(text, (str, sub) => sub.Replace(str));

		[Pure]
		public static string ReplaceFromChart(this string text, Dictionary<string, string> chart)
			=> chart.Keys.Aggregate(text, (current, key)
				=> current.Replace(key, chart[key]));

		[Pure]
		public static string ReplaceFromChart(this string text, Dictionary<char, char> chart)
			=> chart.Keys.Aggregate(text, (current, key)
				=> current.Replace(key, chart[key]));

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
		/// Insert spaces at boundaries between Latin and non-Latin characters (ie. <code>ニンテンドーDSiブラウザー</code> -> <code>ニンテンドー DSi ブラウザー</code>).
		/// </summary>
		/// <param name="text">The text to insert spaces in.</param>
		/// <returns>The text with spaces inserted at language boundaries.</returns>
		[Pure]
		internal static string SeparateLanguageBoundaries(this string text)
			=> LanguageBoundarySubstitution.Value.Replace(text);

		[Pure]
		internal static string WithoutChars(this string charset, string withoutChars)
			=> withoutChars.Aggregate(charset, (set, withoutChar) => set.Replace($"{withoutChar}", ""));
	}
}
