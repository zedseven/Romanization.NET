using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Romanization
{
	internal static class Utilities
	{
		// Constants
		public static readonly string AssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		public static readonly string LanguageCharacterMapsPath = Path.Combine(AssemblyPath, "LanguageCharacterMaps");

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
		/// <exception cref="T:Romanization.Utilities.CannotReadStreamException">The provided stream cannot be read.</exception>
		/// <exception cref="T:Romanization.Utilities.CsvLoadingException">Unable to load the CSV file.</exception>
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
		/// <exception cref="T:Romanization.Utilities.CannotReadStreamException">The provided stream cannot be read.</exception>
		/// <exception cref="T:Romanization.Utilities.CsvLoadingException">Unable to load the CSV file.</exception>
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

		private static string WithoutQuotes(this string str)
			=> str.Length >= 2 && str[0] == '"' && str[^1] == '"' ? str[1..^1] : str;

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
	}
}
