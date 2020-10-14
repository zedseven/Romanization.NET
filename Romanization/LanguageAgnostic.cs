using System;
using System.Text.RegularExpressions;

namespace Romanization
{
	/// <summary>
	/// A global class for language-agnostic functions and constants.
	/// </summary>
	internal static class LanguageAgnostic
	{
		// General Constants
		public const string Vowels = "aeiouy";
		public const string Consonants = "bcdfghjklmnpqrstvwxz";
		public const char IdeographicFullStop = '。';

		// Replacement Characters
		public const string MacronA = "ā";
		public const string MacronE = "ē";
		public const string MacronI = "ī";
		public const string MacronO = "ō";
		public const string MacronU = "ū";

		// Regex Constants
		private static readonly Lazy<Regex> LanguageBoundaryRegex = new Lazy<Regex>(new Regex(
			$"(?:([{LanguageBoundaryChars}])([^ {LanguageBoundaryChars}])|([^ {LanguageBoundaryChars}])([{LanguageBoundaryChars}]))",
			RegexOptions.IgnoreCase | RegexOptions.Compiled));
		private const string LanguageBoundaryChars = @"a-z\.\t\-?!";
		private const string LanguageBoundarySubstitution = "${1}${3} ${2}${4}";

		/// <summary>
		/// Remove common alternative characters, such as the ideographic full-stop (replaced with a period).
		/// </summary>
		/// <param name="text">The text to replace in.</param>
		/// <returns>The original text with common alternate characters replaced.</returns>
		public static string RemoveCommonAlternates(string text)
			=> text.Replace(IdeographicFullStop, '.');

		/// <summary>
		/// Insert spaces at boundaries between Latin and non-Latin characters (ie. `ニンテンドーDSiブラウザー` -> `ニンテンドー DSi ブラウザー`).
		/// </summary>
		/// <param name="text">The text to insert spaces in.</param>
		/// <returns>The text with spaces inserted at language boundaries.</returns>
		public static string SeparateLanguageBoundaries(string text)
			=> LanguageBoundaryRegex.Value.Replace(text, LanguageBoundarySubstitution);
	}
}
