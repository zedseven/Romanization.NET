using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;

namespace Romanization.Internal
{
	/// <summary>
	/// For systems whose conversion charts are too large to reasonably pack in-DLL, they're stored in separate CSV
	/// files and loaded in upon construction of the system.<br />
	/// A good example of this is <see cref="Chinese.HanyuPinyin"/> - the Hànyǔ Pīnyīn chart alone has over 30000
	/// characters.
	/// </summary>
	internal static class CsvLoader
	{
		/// <summary>
		/// Represents an error that occurred during loading of a library CSV file.
		/// </summary>
		internal class CsvLoadingException : Exception
		{
			internal CsvLoadingException() {}
			internal CsvLoadingException(string message) : base(message) {}
			internal CsvLoadingException(string message, Exception inner) : base(message, inner) {}
		}

		private static readonly string LanguageCharacterMapsPath = Path.Combine(Constants.AssemblyPath,
			"contentFiles", "any", "any", "LanguageCharacterMaps");

		/// <summary>
		/// Loads a language character map file into a dictionary, using the provided mapping functions to map CSV
		/// entries to dict keys &amp; values.
		/// </summary>
		/// <typeparam name="TKey">The type of the dictionary keys. Not null.</typeparam>
		/// <typeparam name="TVal">The type of the dictionary values.</typeparam>
		/// <param name="fileName">The file name of the language character map file.</param>
		/// <param name="dict">The dictionary to load into.</param>
		/// <param name="keyMapper">The function that maps CSV entry first values to dictionary
		/// <typeparamref name="TKey"/> values.</param>
		/// <param name="valueMapper">The function that maps CSV entry second values to dictionary
		/// <typeparamref name="TVal"/> values.</param>
		/// <exception cref="T:Romanization.Internal.CannotReadStreamException">The provided stream cannot be
		/// read.</exception>
		/// <exception cref="T:Romanization.Internal.CsvLoadingException">Unable to load the CSV file.</exception>
		public static void LoadCharacterMap<TKey, TVal>(string fileName, IDictionary<TKey, TVal> dict,
			Func<string, TKey> keyMapper, Func<string, TVal> valueMapper)
			where TKey : notnull
		{
			using FileStream csvStream = File.OpenRead(Path.Combine(LanguageCharacterMapsPath, fileName));
			csvStream.LoadCsvIntoDictionary(dict, keyMapper, valueMapper);
		}

		/// <summary>
		/// Loads a language character map file into a replacement chart, using the provided mapping functions to map
		/// CSV entries to chart keys &amp; values.
		/// </summary>
		/// <param name="fileName">The file name of the language character map file.</param>
		/// <param name="chart">The chart to load into.</param>
		/// <param name="keyMapper">The function that maps CSV entry first values to chart keys.</param>
		/// <param name="valueMapper">The function that maps CSV entry second values to chart values.</param>
		/// <exception cref="T:Romanization.Internal.CannotReadStreamException">The provided stream cannot be
		/// read.</exception>
		/// <exception cref="T:Romanization.Internal.CsvLoadingException">Unable to load the CSV file.</exception>
		public static void LoadCharacterMap(string fileName, ReplacementChart chart,
			Func<string, string> keyMapper, Func<string, string> valueMapper)
		{
			using FileStream csvStream = File.OpenRead(Path.Combine(LanguageCharacterMapsPath, fileName));
			csvStream.LoadCsvIntoDictionary(chart, keyMapper, valueMapper);
			chart.RecalculateLongestKeyLength();
		}

		/// <summary>
		/// Loads a CSV file stream into a dictionary, using the provided mapping functions to map CSV entries to dict
		/// keys &amp; values.<br />
		/// Note that this is a non-standard CSV parsing implementation - it only looks for the first comma.
		/// </summary>
		/// <typeparam name="TKey">The type of the dictionary keys. Not null.</typeparam>
		/// <typeparam name="TVal">The type of the dictionary values.</typeparam>
		/// <param name="stream">The CSV file stream.</param>
		/// <param name="dict">The dictionary to load into.</param>
		/// <param name="keyMapper">The function that maps CSV entry first values to dictionary
		/// <typeparamref name="TKey"/> values.</param>
		/// <param name="valueMapper">The function that maps CSV entry second values to dictionary
		/// <typeparamref name="TVal"/> values.</param>
		/// <exception cref="T:Romanization.Internal.CannotReadStreamException">The provided stream cannot be
		/// read.</exception>
		/// <exception cref="T:Romanization.Internal.CsvLoadingException">Unable to load the CSV file.</exception>
		public static void LoadCsvIntoDictionary<TKey, TVal>(this FileStream stream, IDictionary<TKey, TVal> dict,
			Func<string, TKey> keyMapper, Func<string, TVal> valueMapper)
			where TKey : notnull
		{
			if (!stream.CanRead)
				throw new CannotReadStreamException("The provided stream cannot be read.", nameof(stream));

			try
			{
				using StreamReader reader = new(stream);

				// Discard the first line, since it's simply the heading
				reader.ReadLine();

				while (!reader.EndOfStream)
				{
					string? line = reader.ReadLine();
					if (string.IsNullOrWhiteSpace(line))
						continue;

					int commaIndex = line.IndexOfIgnoreEscaped(',', '"');
					if (commaIndex < 0)
						continue;

					dict.Add(keyMapper(line[..commaIndex].ProcessQuotes()),
						valueMapper(line[(commaIndex + 1)..].ProcessQuotes()));
				}
			}
			catch (Exception e)
			{
				throw new CsvLoadingException("Unable to load the CSV file.", e);
			}
		}

		[Pure]
		private static string ProcessQuotes(this string str)
		{
			if (str.Length >= 2 && str[0] == '"' && str[^1] == '"')
				str = str[1..^1];
			return str.Replace("\"\"", "\"");
		}

		[Pure]
		private static int IndexOfIgnoreEscaped(this string str, char needle, char escapeChar)
		{
			bool escaped = false;
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == escapeChar)
					escaped = !escaped;
				else if (str[i] == needle && !escaped)
					return i;
			}
			return -1;
		}
	}
}
