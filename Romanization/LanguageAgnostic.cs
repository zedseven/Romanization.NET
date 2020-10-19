using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

namespace Romanization
{
	/// <summary>
	/// A global class for language-agnostic functions and constants (things that are independent of specific languages).
	/// </summary>
	public static class LanguageAgnostic
	{
		// General Constants
		internal const string Vowels = "aeiouy";
		internal const string Consonants = "bcdfghjklmnpqrstvwxz";
		internal const string Punctuation = @"\.?!";
		internal const char IdeographicFullStop = '。';

		// Replacement Characters
		internal const string MacronA = "ā";
		internal const string MacronE = "ē";
		internal const string MacronI = "ī";
		internal const string MacronO = "ō";
		internal const string MacronU = "ū";

		// Regex Constants
		private static readonly Lazy<Regex> LanguageBoundaryRegex = new Lazy<Regex>(() => new Regex(
			$"(?:([{LanguageBoundaryChars}{Punctuation}])([^ {LanguageBoundaryChars}{Punctuation}])|([^ {LanguageBoundaryChars}{Punctuation}])([{LanguageBoundaryChars}]))",
			RegexOptions.Compiled | RegexOptions.IgnoreCase));
		private const string LanguageBoundaryChars = @"a-z";
		private const string LanguageBoundarySubstitution = "${1}${3} ${2}${4}";

		/// <summary>
		/// A string of characters with all possible readings (pronunciations) for each character.
		/// </summary>
		/// <typeparam name="TType">The type of reading.</typeparam>
		public class ReadingsString<TType>
			where TType : Enum
		{
			public readonly ReadingCharacter<TType>[] Characters;

			internal ReadingsString(ReadingCharacter<TType>[] characters)
			{
				Characters = characters;
			}

			public override string ToString()
				=> Characters.Aggregate("", (current, character) => current + character.FlattenReadings());
		}

		/// <summary>
		/// A character with all possible readings (pronunciations).
		/// </summary>
		/// <typeparam name="TType">The type of reading.</typeparam>
		public class ReadingCharacter<TType>
			where TType : Enum
		{
			public readonly string Character;
			public readonly Reading<TType>[] Readings;

			internal ReadingCharacter(string character, IEnumerable<Reading<TType>> readings)
			{
				Character = character;
				Readings = readings.ToArray();
			}

			public override string ToString()
				=> $"'{Character}' {FlattenReadings()}";

			public string FlattenReadings()
			{
				string[] readings = Readings.Select(r => r.Value).Distinct().ToArray();
				if (readings.Length > 1)
					return $"[{string.Join(" ", readings)}]";
				return readings.Length == 1 ? readings[0] : Character;
			}
		}

		/// <summary>
		/// A reading (pronunciation) of a character.
		/// </summary>
		/// <typeparam name="TType">The type of reading.</typeparam>
		public class Reading<TType>
			where TType : Enum
		{
			public readonly TType Type;
			public readonly string Value;

			internal Reading(TType type, string value)
			{
				Type = type;
				Value = value;
			}
		}

		/// <summary>
		/// Remove common alternative characters, such as the ideographic full-stop (replaced with a period).
		/// </summary>
		/// <param name="text">The text to replace in.</param>
		/// <returns>The original text with common alternate characters replaced.</returns>
		[Pure]
		internal static string ReplaceCommonAlternates(string text)
			=> text.Replace(IdeographicFullStop, '.');

		/// <summary>
		/// Insert spaces at boundaries between Latin and non-Latin characters (ie. `ニンテンドーDSiブラウザー` -> `ニンテンドー DSi ブラウザー`).
		/// </summary>
		/// <param name="text">The text to insert spaces in.</param>
		/// <returns>The text with spaces inserted at language boundaries.</returns>
		[Pure]
		internal static string SeparateLanguageBoundaries(string text)
			=> LanguageBoundaryRegex.Value.Replace(text, LanguageBoundarySubstitution);
	}
}
