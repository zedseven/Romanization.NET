using System.Text.RegularExpressions;

namespace Romanization.LanguageAgnostic
{
	internal interface ISub
	{
		public string Replace(string text);
	}

	internal class CharSub : ISub
	{
		private readonly Regex  _findRegex;
		private readonly string _substitution;

		public CharSub(string pattern, string substitution, bool ignoreCase = true)
		{
			_findRegex    = new Regex(pattern, ignoreCase ? RegexOptions.Compiled | RegexOptions.IgnoreCase : RegexOptions.Compiled);
			_substitution = substitution;
		}

		public string Replace(string text)
			=> _findRegex.Replace(text, _substitution);
	}

	internal class CharSubCased : ISub
	{
		private readonly Regex  _findRegexUpper;
		private readonly Regex  _findRegexLower;
		private readonly string _substitutionUpper;
		private readonly string _substitutionLower;

		public CharSubCased(string patternUpper, string patternLower, string substitutionUpper, string substitutionLower)
		{
			_findRegexUpper    = new Regex(patternUpper, RegexOptions.Compiled);
			_findRegexLower    = new Regex(patternLower, RegexOptions.Compiled);
			_substitutionUpper = substitutionUpper;
			_substitutionLower = substitutionLower;
		}

		public string Replace(string text)
			=> _findRegexLower.Replace(_findRegexUpper.Replace(text, _substitutionUpper), _substitutionLower);
	}
}
