using System.Text.RegularExpressions;

namespace Romanization.Internal
{
	internal class RepeatedCombiningCharsSub : ISub
	{
		private readonly Regex  _findRegex;
		private readonly string _substitution;
		private readonly string _combiningChar;

		public RepeatedCombiningCharsSub(char combiningChar, string startSub, string endSub, int minCount = 1, RegexOptions options = RegexOptions.None)
		{
			_findRegex =
				new Regex(
					$"((?:.[{Constants.MostCombiningChars}]*?{combiningChar}[{Constants.MostCombiningChars}]*){{{minCount},}})",
					RegexOptions.Compiled | options);
			_substitution  = $"{startSub}${{1}}{endSub}";
			_combiningChar = combiningChar.ToString();
		}

		public string Replace(string text)
			=> _findRegex.Replace(text, _substitution).Replace(_combiningChar, null);
	}
}
