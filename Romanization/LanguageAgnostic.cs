using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace Romanization
{
	/// <summary>
	/// A global class for language-agnostic functions and constants (things that are independent of specific languages).
	/// </summary>
	internal static class LanguageAgnostic
	{
		// General Constants
		public const string Vowels = "aeiouy";
		public const string Consonants = "bcdfghjklmnpqrstvwxz";
		public const string Punctuation = @"\.?!";
		public const char IdeographicFullStop = '。';

		// Replacement Characters
		public const string MacronA = "ā";
		public const string MacronE = "ē";
		public const string MacronI = "ī";
		public const string MacronO = "ō";
		public const string MacronU = "ū";

		// Regex Constants
		private static readonly Lazy<Regex> LanguageBoundaryRegex = new Lazy<Regex>(() => new Regex(
			$"(?:([{LanguageBoundaryChars}{Punctuation}])([^ {LanguageBoundaryChars}{Punctuation}])|([^ {LanguageBoundaryChars}{Punctuation}])([{LanguageBoundaryChars}]))",
			RegexOptions.Compiled | RegexOptions.IgnoreCase));
		private const string LanguageBoundaryChars = @"a-z";
		private const string LanguageBoundarySubstitution = "${1}${3} ${2}${4}";

		/// <summary>
		/// Remove common alternative characters, such as the ideographic full-stop (replaced with a period).
		/// </summary>
		/// <param name="text">The text to replace in.</param>
		/// <returns>The original text with common alternate characters replaced.</returns>
		[Pure]
		public static string ReplaceCommonAlternates(string text)
			=> text.Replace(IdeographicFullStop, '.');

		/// <summary>
		/// Insert spaces at boundaries between Latin and non-Latin characters (ie. `ニンテンドーDSiブラウザー` -> `ニンテンドー DSi ブラウザー`).
		/// </summary>
		/// <param name="text">The text to insert spaces in.</param>
		/// <returns>The text with spaces inserted at language boundaries.</returns>
		[Pure]
		public static string SeparateLanguageBoundaries(string text)
			=> LanguageBoundaryRegex.Value.Replace(text, LanguageBoundarySubstitution);
	}
}
