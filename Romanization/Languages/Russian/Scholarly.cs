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
		/// The International Scholarly System of romanization for Russian.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Scientific_transliteration_of_Cyrillic'>https://en.wikipedia.org/wiki/Scientific_transliteration_of_Cyrillic</a>
		/// </summary>
		public sealed class Scholarly : IRomanizationSystem
		{
			/// <inheritdoc />
			public bool TransliterationSystem => true;

			// System-Specific Constants
			private readonly Dictionary<string, string> RomanizationTable = new Dictionary<string, string>();

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.
			/// </summary>
			public Scholarly()
			{
				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/Scientific_transliteration_of_Cyrillic

				// Main characters (2021)
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
				RomanizationTable["I"] = "I";
				RomanizationTable["і"] = "i";
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
				RomanizationTable["Х"] = "X";
				RomanizationTable["х"] = "x";
				RomanizationTable["Ц"] = "C";
				RomanizationTable["ц"] = "c";
				RomanizationTable["Ч"] = "Č";
				RomanizationTable["ч"] = "č";
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
				RomanizationTable["Э"] = "È";
				RomanizationTable["э"] = "è";
				RomanizationTable["Ю"] = "Ju";
				RomanizationTable["ю"] = "ju";
				RomanizationTable["Я"] = "Ja";
				RomanizationTable["я"] = "ja";

				// Letters eliminated in the orthographic reform of 1918
				RomanizationTable["І"] = "I";
				RomanizationTable["і"] = "i";
				RomanizationTable["Ѳ"] = "F";
				RomanizationTable["ѳ"] = "f";
				RomanizationTable["Ѣ"] = "Ě";
				RomanizationTable["ѣ"] = "ě";
				RomanizationTable["Ѵ"] = "I";
				RomanizationTable["ѵ"] = "i";

				// Pre-18th century letters
				RomanizationTable["Є"] = "E";
				RomanizationTable["є"] = "e";
				RomanizationTable["Ѥ"] = "Je";
				RomanizationTable["ѥ"] = "je";
				RomanizationTable["Ѕ"] = "Dz";
				RomanizationTable["ѕ"] = "dz";
				RomanizationTable["Ꙋ"] = "U";
				RomanizationTable["ꙋ"] = "u";
				RomanizationTable["Ѡ"] = "Ô";
				RomanizationTable["ѡ"] = "ô";
				RomanizationTable["Ѿ"] = "Ôt";
				RomanizationTable["ѿ"] = "ôt";
				RomanizationTable["Ѫ"] = "Ǫ";
				RomanizationTable["ѫ"] = "ǫ";
				RomanizationTable["Ѧ"] = "Ę";
				RomanizationTable["ѧ"] = "ę";
				RomanizationTable["Ѭ"] = "Jǫ";
				RomanizationTable["ѭ"] = "jǫ";
				RomanizationTable["Ѩ"] = "Ję";
				RomanizationTable["ѩ"] = "ję";
				RomanizationTable["Ѯ"] = "Ks";
				RomanizationTable["ѯ"] = "ks";
				RomanizationTable["Ѱ"] = "Ps";
				RomanizationTable["ѱ"] = "ps";

				#endregion
			}

			/// <summary>
			/// Performs romanization according to the International Scholarly System on the given text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			public string Process(string text)
				=> text.ReplaceFromChart(RomanizationTable);
		}
	}
}
