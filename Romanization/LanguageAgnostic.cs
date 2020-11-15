using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

// ReSharper disable CommentTypo

namespace Romanization
{
	/// <summary>
	/// A global class for language-agnostic functions and constants (things that are independent of specific languages).
	/// </summary>
	public static class LanguageAgnostic
	{
		// General Constants
		internal const string Vowels            = "aeiouy";
		internal const string Consonants        = "bcdfghjklmnpqrstvwxz";
		internal const string Punctuation       = @"\.?!";
		internal const char IdeographicFullStop = '。';
		internal const char Interpunct          = '・';

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
		private const string LanguageBoundaryChars                = @"a-z";
		private const string LanguageBoundarySubstitution         = "${1}${3} ${2}${4}";

		/// <summary>
		/// A string of characters with all possible readings (pronunciations) for each character.
		/// </summary>
		/// <typeparam name="TType">The reading type enum to use, which contains all supported readings for a given language or system.<br />For example, <see cref="Japanese.KanjiReadingsSystem.ReadingTypes"/>.</typeparam>
		public class ReadingsString<TType>
			where TType : Enum
		{
			/// <summary>
			/// The characters of the string.<br />
			/// Each one stores both the character itself (not necessarily equivalent to a char, as some Hànzì characters are double-wide), and all known readings (pronunciations).
			/// </summary>
			public readonly ReadingCharacter<TType>[] Characters;

			internal ReadingsString(ReadingCharacter<TType>[] characters)
			{
				Characters = characters;
			}

			// TODO: Add additional ToString() implementations that do display reading types.
			/// <summary>
			/// Returns a string that displays all readings of each character.<br />
			/// For characters with 0 readings, they are displayed simply as themselves.<br />
			/// For characters with 1 reading, they are displayed as their only reading.<br />
			/// For characters with more than 1 reading, they are displayed as a space-delimited list of all readings in order, within square brackets.<br />
			/// Example: <code>"xiàndài [hàn tān][yǔ yù] [pín bīn][shuài lǜ lüe l̈ù] cí[diǎn tiǎn]."</code><br />
			/// Note that this does not display the source of each reading.
			/// </summary>
			/// <returns>A string with all known readings of each character.</returns>
			public override string ToString()
				=> Characters.Aggregate("", (current, character) => current + character.FlattenReadings());
		}

		/// <summary>
		/// A character with all possible readings (pronunciations).
		/// </summary>
		/// <typeparam name="TType">The reading type enum to use, which contains all supported readings for a given language or system.<br />For example, <see cref="Japanese.KanjiReadingsSystem.ReadingTypes"/>.</typeparam>
		public class ReadingCharacter<TType>
			where TType : Enum
		{
			/// <summary>
			/// The actual character value.<br />Note that this is not necessarily one char in length - some Hànzì characters go outside the Basic Multilingual Plane (BMP), and as such take up 32 bits (two 16-bit chars).
			/// </summary>
			public readonly string Character;
			/// <summary>
			/// The collection of known readings for the character, in order as specified in the function used to generate this object.
			/// </summary>
			public readonly Reading<TType>[] Readings;

			internal ReadingCharacter(string character, IEnumerable<Reading<TType>> readings)
			{
				Character = character;
				Readings = readings.ToArray();
			}

			/// <summary>
			/// Returns a string that represents the current object.<br />
			/// The format is: <code>'&lt;char&gt;' [&lt;readings&gt;]</code>
			/// </summary>
			/// <returns>A string with the character and all known readings.</returns>
			public override string ToString()
				=> $"'{Character}' {FlattenReadings()}";

			/// <summary>
			/// Returns a string starting and ending with square brackets, containing all readings in the order they appear in <see cref="Readings"/>.<br />
			/// If the character has no known readings, the character itself is returned instead.<br />
			/// Example: <code>[shuài lǜ lüe l̈ù]</code><br />
			/// Note this does not output the source of each reading.
			/// </summary>
			/// <returns>A string representation of all readings of the character, or the character itself if there are none.</returns>
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
		/// <typeparam name="TType">The reading type enum to use, which contains all supported readings for a given language or system.<br />For example, <see cref="Japanese.KanjiReadingsSystem.ReadingTypes"/>.</typeparam>
		public class Reading<TType>
			where TType : Enum
		{
			/// <summary>
			/// The type of reading it is. For example, it could be <see cref="Japanese.KanjiReadingsSystem.ReadingTypes.Kunyomi"/>.
			/// </summary>
			public readonly TType Type;
			/// <summary>
			/// The reading itself - a romanized string representing how a character should be pronounced.
			/// </summary>
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
			=> text.Replace(IdeographicFullStop, '.')
				.Replace(Interpunct, ' ');

		/// <summary>
		/// Insert spaces at boundaries between Latin and non-Latin characters (ie. <code>ニンテンドーDSiブラウザー</code> -> <code>ニンテンドー DSi ブラウザー</code>).
		/// </summary>
		/// <param name="text">The text to insert spaces in.</param>
		/// <returns>The text with spaces inserted at language boundaries.</returns>
		[Pure]
		internal static string SeparateLanguageBoundaries(string text)
			=> LanguageBoundaryRegex.Value.Replace(text, LanguageBoundarySubstitution);
	}
}
