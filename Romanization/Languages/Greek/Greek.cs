// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming

using Romanization.Internal;

namespace Romanization
{
	/// <summary>
	/// The class for romanizing Greek text.<br />
	/// Because there are separate systems for Ancient and Modern Greek, they are seperated further into
	/// <see cref="Ancient"/> and <see cref="Modern"/> classes.
	/// </summary>
	public static partial class Greek
	{
		// Largely sourced from http://www.eki.ee/wgrs/rom1_el.htm - some of this information was surprisingly difficult
		// to find.
		private const string GreekAllVowels		   = "ΑαΕεΗηΙιΟοΩωΥυ";
		private const string GreekFrontSingleVowels   = "ΕεΗηΙιΥυ";
		private const string GreekFrontDoubleVowels   = "αι|ει|οι|υι";
		private const string GreekVowelDiphthongs	 = "αι|ει|οι|υι|αυ|ευ|ηυ|ου|ωυ";
		private const string GreekAllConsonants	   = "ΒβΓγΔδΖζΘθΚκΛλΜμΝνΞξΠπΡρΣσςΤτΦφΧχΨψ";
		private const string GreekVoicedConsonants	= "ΒβΓγΔδΖζΛλΜμΝνΡρ";
		private const string GreekVoicelessConsonants = "ΘθΚκΞξΠπΣσςΤτΦφΧχΨψ";

		private static string LanguageWidePreparation(this string text)
			=> text
				// Normalize Unicode
				.UnicodeNormalize();
	}
}
