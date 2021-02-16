using System.Text.RegularExpressions;

namespace Romanization.Internal
{
	internal class EncapsulatingCharsSub : ISub
	{
		private readonly Regex  _findRegex;
		private readonly string _substitution;

		public EncapsulatingCharsSub(string startCombination, string endCombination, string startSub, string endSub,
			RegexOptions options = RegexOptions.None)
		{
			_findRegex =
				new Regex(
					$"{startCombination}(.+){endCombination}",
					RegexOptions.Compiled | options);
			_substitution  = $"{startSub}${{1}}{endSub}";
		}

		public string Replace(string text)
			=> _findRegex.Replace(text, _substitution);
	}
}
