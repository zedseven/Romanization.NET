using Romanization.LanguageAgnostic;
using System.Collections.Generic;

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
		/// The GOST 7.79-2000(A) romanization system of Russian.<br />
		/// This is System A of the GOST 7.79-2000 system with 1 Cyrillic to 1 Latin char, with diacritics.<br />
		/// Identical to ISO 9:1995 (different to ISO/R 9:1968).<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/GOST_7.79-2000'>https://en.wikipedia.org/wiki/GOST_7.79-2000</a>
		/// </summary>
		public sealed class Gost7792000A : IRomanizationSystem
		{
			/// <inheritdoc />
			public bool TransliterationSystem => true;

			// System-Specific Constants
			private readonly Dictionary<string, string> RomanizationTable = new Dictionary<string, string>();

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.
			/// </summary>
			public Gost7792000A()
			{
				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/Romanization_of_Russian and https://en.wikipedia.org/wiki/GOST_7.79-2000

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
				RomanizationTable["Е"] = "E";
				RomanizationTable["е"] = "e";
				RomanizationTable["Ё"] = "Ë";
				RomanizationTable["ё"] = "ë";
				RomanizationTable["Ж"] = "Ž";
				RomanizationTable["ж"] = "ž";
				RomanizationTable["З"] = "Z";
				RomanizationTable["з"] = "z";
				RomanizationTable["И"] = "I";
				RomanizationTable["и"] = "i";
				RomanizationTable["Й"] = "J";
				RomanizationTable["й"] = "j";
				RomanizationTable["К"] = "K";
				RomanizationTable["к"] = "k";
				RomanizationTable["Л"] = "L";
				RomanizationTable["л"] = "l";
				RomanizationTable["М"] = "M";
				RomanizationTable["м"] = "m";
				RomanizationTable["Н"] = "N";
				RomanizationTable["н"] = "n";
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
				RomanizationTable["У"] = "U";
				RomanizationTable["у"] = "u";
				RomanizationTable["Ф"] = "F";
				RomanizationTable["ф"] = "f";
				RomanizationTable["Х"] = "H";
				RomanizationTable["х"] = "h";
				RomanizationTable["Ц"] = "C";
				RomanizationTable["ц"] = "c";
				RomanizationTable["Ч"] = "Č";
				RomanizationTable["ч"] = "č";
				RomanizationTable["Ш"] = "Š";
				RomanizationTable["ш"] = "š";
				RomanizationTable["Щ"] = "Ŝ";
				RomanizationTable["щ"] = "ŝ";
				RomanizationTable["Ъ"] = "ʺ";
				RomanizationTable["ъ"] = "ʺ";
				RomanizationTable["Ы"] = "Y";
				RomanizationTable["ы"] = "y";
				RomanizationTable["Ь"] = "ʹ";
				RomanizationTable["ь"] = "ʹ";
				RomanizationTable["Э"] = "È";
				RomanizationTable["э"] = "è";
				RomanizationTable["Ю"] = "Û";
				RomanizationTable["ю"] = "û";
				RomanizationTable["Я"] = "Â";
				RomanizationTable["я"] = "â";
				RomanizationTable["І"] = "Ì";
				RomanizationTable["і"] = "ì";
				RomanizationTable["Ѳ"] = "F̀";
				RomanizationTable["ѳ"] = "f̀";
				RomanizationTable["Ѣ"] = "Ě";
				RomanizationTable["ѣ"] = "ě";
				RomanizationTable["Ѵ"] = "Ỳ";
				RomanizationTable["ѵ"] = "ỳ";
				RomanizationTable["Ѕ"] = "Ẑ";
				RomanizationTable["ѕ"] = "ẑ";
				RomanizationTable["Ѫ"] = "Ǎ";
				RomanizationTable["ѫ"] = "ǎ";

				#endregion
			}

			/// <summary>
			/// Performs GOST 7.79-2000(A) romanization on Russian text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			public string Process(string text)
				=> text.ReplaceFromChart(RomanizationTable);
		}
	}
}
