// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming

using Romanization.LanguageAgnostic;

namespace Romanization
{
	/// <summary>
	/// The class for romanizing Russian text.<br />
	/// The reason for the abundance of systems is because there is no single, international, modern standard like there is for many other languages.<br />
	/// Note that dictionary/learning-material Russian can include acute diacritics for marking stress. These are ignored by all systems here, and<br />
	/// the diacritic will remain on the romanized version.
	/// See <a href='https://en.wikipedia.org/wiki/Russian_alphabet#Diacritics'>https://en.wikipedia.org/wiki/Russian_alphabet#Diacritics</a> for more info.
	/// </summary>
	public static partial class Russian
	{
		private const string RussianVowels = "ИиЫыЭэЕеАаЯяОоЁёУуЮю"; // https://en.wikipedia.org/wiki/Russian_phonology#Vowels
		private const string RussianConsonants = "БбВвГгДдЖжЗзЙйКкЛлМмНнПпРрСсТтФфХхЦцЧчШшЩщ"; // https://russianlanguage.org.uk/russian-language-consonants/

		private static string LanguageWidePreparation(this string text)
			=> text
					// Normalize Unicode
					.UnicodeNormalize()
					// Remove acute accents, as some formal/educational Russian includes them
					.Replace("\u0301", "");
	}
}
