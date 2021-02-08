using System.Text.RegularExpressions;

namespace Romanization.Internal
{
	internal class CharSubCased : ISub
	{
		private readonly Regex  _findRegexUpper;
		private readonly Regex  _findRegexLower;
		private readonly string _substitutionUpper;
		private readonly string _substitutionLower;

		public CharSubCased(string patternUpper, string patternLower, string substitutionUpper, string substitutionLower)
		{
			_findRegexUpper	= new Regex(patternUpper, RegexOptions.Compiled);
			_findRegexLower	= new Regex(patternLower, RegexOptions.Compiled);
			_substitutionUpper = substitutionUpper;
			_substitutionLower = substitutionLower;
		}

		public CharSubCased(string patternUpper, string patternLower, string substitutionUpper, string substitutionLower, RegexOptions options)
		{
			_findRegexUpper	= new Regex(patternUpper, RegexOptions.Compiled | options);
			_findRegexLower	= new Regex(patternLower, RegexOptions.Compiled | options);
			_substitutionUpper = substitutionUpper;
			_substitutionLower = substitutionLower;
		}

		public string Replace(string text)
			=> _findRegexLower.Replace(_findRegexUpper.Replace(text, _substitutionUpper), _substitutionLower);
	}
}
