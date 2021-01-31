using System;
using System.Collections.Generic;
using System.Linq;
using Romanization.LanguageAgnostic;

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming

namespace Romanization
{
	public static partial class Russian
	{
		/// <summary>
		/// The ISO Recommendation No. 9 (ISO/R 9:1968) system of romanization, specialized for Russian.<br />
		/// This transliteration table is designed to cover Bulgarian, Russian, Belarusian, Ukrainian, Serbo-Croatian and Macedonian in general, with regional specializations for certain languages.<br />
		/// This is largely superceded by ISO 9 (GOST 7.79-2000(A)).<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/ISO_9#ISO/R_9'>https://en.wikipedia.org/wiki/ISO_9#ISO/R_9</a>
		/// </summary>
		public static readonly Lazy<IsoR9System> IsoR9 = new Lazy<IsoR9System>(() => new IsoR9System());

		/// <summary>
		/// The ISO Recommendation No. 9 (ISO/R 9:1968) system of romanization, specialized for Russian.<br />
		/// This transliteration table is designed to cover Bulgarian, Russian, Belarusian, Ukrainian, Serbo-Croatian and Macedonian in general, with regional specializations for certain languages.<br />
		/// This is largely superceded by ISO 9 (GOST 7.79-2000(A)).<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/ISO_9#ISO/R_9'>https://en.wikipedia.org/wiki/ISO_9#ISO/R_9</a>
		/// </summary>
		public sealed class IsoR9System : IRomanizationSystem
		{
			/// <inheritdoc />
			public bool TransliterationSystem => true;

			// System-Specific Constants
			private static readonly Dictionary<string, string> RomanizationTable = new Dictionary<string, string>();

			internal IsoR9System()
			{
				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/ISO_9#ISO/R_9

				RomanizationTable["А"] = "A";
				RomanizationTable["а"] = "a";
				RomanizationTable["Б"] = "B";
				RomanizationTable["б"] = "b";
				RomanizationTable["В"] = "V";
				RomanizationTable["в"] = "v";
				RomanizationTable["Г"] = "G";
				RomanizationTable["г"] = "g";
				RomanizationTable["Д"] = "D";
				RomanizationTable["д"] = "d";
				RomanizationTable["Ѓ"] = "Ǵ";
				RomanizationTable["ѓ"] = "ǵ";
				RomanizationTable["Ђ"] = "Đ";
				RomanizationTable["ђ"] = "đ";
				RomanizationTable["Е"] = "E";
				RomanizationTable["е"] = "e";
				RomanizationTable["Ё"] = "Ë";
				RomanizationTable["ё"] = "ë";
				RomanizationTable["Є"] = "Je";
				RomanizationTable["є"] = "je";
				RomanizationTable["Ж"] = "Ž";
				RomanizationTable["ж"] = "ž";
				RomanizationTable["З"] = "Z";
				RomanizationTable["з"] = "z";
				RomanizationTable["Ѕ"] = "Dz";
				RomanizationTable["ѕ"] = "dz";
				RomanizationTable["И"] = "I";
				RomanizationTable["и"] = "i";
				RomanizationTable["I"] = "I";
				RomanizationTable["і"] = "i";
				RomanizationTable["Ї"] = "Ï";
				RomanizationTable["ї"] = "ï";
				RomanizationTable["Й"] = "J";
				RomanizationTable["й"] = "j";
				RomanizationTable["К"] = "K";
				RomanizationTable["к"] = "k";
				RomanizationTable["Л"] = "L";
				RomanizationTable["л"] = "l";
				RomanizationTable["Љ"] = "Lj";
				RomanizationTable["љ"] = "lj";
				RomanizationTable["М"] = "M";
				RomanizationTable["м"] = "m";
				RomanizationTable["Н"] = "N";
				RomanizationTable["н"] = "n";
				RomanizationTable["Њ"] = "Nj";
				RomanizationTable["њ"] = "nj";
				RomanizationTable["О"] = "O";
				RomanizationTable["о"] = "o";
				RomanizationTable["П"] = "P";
				RomanizationTable["п"] = "p";
				RomanizationTable["Р"] = "R";
				RomanizationTable["р"] = "r";
				RomanizationTable["С"] = "S";
				RomanizationTable["с"] = "s";
				RomanizationTable["Т"] = "T";
				RomanizationTable["т"] = "t";
				RomanizationTable["Ќ"] = "Ḱ";
				RomanizationTable["ќ"] = "ḱ";
				RomanizationTable["Ћ"] = "Ć";
				RomanizationTable["ћ"] = "ć";
				RomanizationTable["У"] = "U";
				RomanizationTable["у"] = "u";
				RomanizationTable["Ў"] = "Ŭ";
				RomanizationTable["ў"] = "ŭ";
				RomanizationTable["Ф"] = "F";
				RomanizationTable["ф"] = "f";
				RomanizationTable["Х"] = "H"; // RU specialization
				RomanizationTable["х"] = "h"; // RU specialization
				RomanizationTable["Ц"] = "C";
				RomanizationTable["ц"] = "c";
				RomanizationTable["Ч"] = "Č";
				RomanizationTable["ч"] = "č";
				RomanizationTable["Џ"] = "Dž";
				RomanizationTable["џ"] = "dž";
				RomanizationTable["Ш"] = "Š";
				RomanizationTable["ш"] = "š";
				RomanizationTable["Щ"] = "Šč";
				RomanizationTable["щ"] = "šč";
				RomanizationTable["Ъ"] = "ʺ";
				RomanizationTable["ъ"] = "ʺ";
				RomanizationTable["Ы"] = "Y";
				RomanizationTable["ы"] = "y";
				RomanizationTable["Ь"] = "ʹ";
				RomanizationTable["ь"] = "ʹ";
				RomanizationTable["Ѣ"] = "Ě";
				RomanizationTable["ѣ"] = "ě";
				RomanizationTable["Э"] = "Ė";
				RomanizationTable["э"] = "ė";
				RomanizationTable["Ю"] = "Ju";
				RomanizationTable["ю"] = "ju";
				RomanizationTable["Я"] = "Ja";
				RomanizationTable["я"] = "ja";
				RomanizationTable["’"] = "ʺ";
				RomanizationTable["Ѫ"] = "ʺ̣";
				RomanizationTable["ѫ"] = "ʺ̣";
				RomanizationTable["Ѳ"] = "Ḟ";
				RomanizationTable["ѳ"] = "ḟ";
				RomanizationTable["Ѵ"] = "Ẏ";
				RomanizationTable["ѵ"] = "ẏ";

				#endregion
			}

			/// <summary>
			/// Performs romanization according to ISO/R 9:1968 on the given text, with regional specializations applied for Russian.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			public string Process(string text)
				=> text.ReplaceFromChart(RomanizationTable);
		}
	}
}
