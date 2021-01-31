using System.Text.RegularExpressions;

// ReSharper disable CommentTypo

namespace Romanization.LanguageAgnostic
{
	/// <summary>
	/// A global class for language-agnostic functions and constants (things that are independent of specific languages).
	/// </summary>
	internal static class Constants
	{
		// General Constants
		public const string Vowels              = "aeiouy";
		public const string Consonants          = "bcdfghjklmnpqrstvwxz";
		public const string Punctuation         = @"\.?!";
		public const char   IdeographicFullStop = '。';
		public const char   Interpunct          = '・';

		// Replacement Characters
		public const string MacronA = "ā";
		public const string MacronE = "ē";
		public const string MacronI = "ī";
		public const string MacronO = "ō";
		public const string MacronU = "ū";
	}
}
