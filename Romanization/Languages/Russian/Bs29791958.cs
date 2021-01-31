using Romanization.LanguageAgnostic;
using System;
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
		/// The British Standard 2979:1958 system of romanization for Russian.<br />
		/// It is the main system of Oxford University Press, and was used by the British Library up until 1975. ALA-LC is now used instead.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Romanization_of_Russian#British_Standard'>https://en.wikipedia.org/wiki/Romanization_of_Russian#British_Standard</a>
		/// </summary>
		public static readonly Lazy<Bs29791958System> Bs29791958 = new Lazy<Bs29791958System>(() => new Bs29791958System());

		/// <summary>
		/// The British Standard 2979:1958 system of romanization for Russian.<br />
		/// It is the main system of Oxford University Press, and was used by the British Library up until 1975. ALA-LC is now used instead.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Romanization_of_Russian#British_Standard'>https://en.wikipedia.org/wiki/Romanization_of_Russian#British_Standard</a>
		/// </summary>
		public sealed class Bs29791958System : IRomanizationSystem
		{
			/// <inheritdoc />
			public bool TransliterationSystem => true;

			// System-Specific Constants
			private static readonly Dictionary<string, string> RomanizationTable = new Dictionary<string, string>();
			private static readonly Dictionary<string, string> DigraphTable = new Dictionary<string, string>();

			private static CharSub HardSignSub;

			internal Bs29791958System()
			{
				HardSignSub = new CharSub("[Ъъ]\\b", "");

				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/BGN/PCGN_romanization_of_Russian

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
				RomanizationTable["Ж"] = "Zh";
				RomanizationTable["ж"] = "zh";
				RomanizationTable["З"] = "Z";
				RomanizationTable["з"] = "z";
				RomanizationTable["И"] = "I";
				RomanizationTable["и"] = "i";
				RomanizationTable["Й"] = "Ĭ";
				RomanizationTable["й"] = "ĭ";
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
				RomanizationTable["Х"] = "Kh";
				RomanizationTable["х"] = "kh";
				RomanizationTable["Ц"] = "Ts";
				RomanizationTable["ц"] = "ts";
				RomanizationTable["Ч"] = "Ch";
				RomanizationTable["ч"] = "ch";
				RomanizationTable["Ш"] = "Sh";
				RomanizationTable["ш"] = "sh";
				RomanizationTable["Щ"] = "Shch";
				RomanizationTable["щ"] = "shch";
				RomanizationTable["Ъ"] = "ʺ";
				RomanizationTable["ъ"] = "ʺ";
				RomanizationTable["Ы"] = "Ȳ";
				RomanizationTable["ы"] = "ȳ";
				RomanizationTable["Ь"] = "ʹ";
				RomanizationTable["ь"] = "ʹ";
				RomanizationTable["Э"] = "É";
				RomanizationTable["э"] = "é";
				RomanizationTable["Ю"] = "Yu";
				RomanizationTable["ю"] = "yu";
				RomanizationTable["Я"] = "Ya";
				RomanizationTable["я"] = "ya";

				// Letters eliminated in the orthographic reform of 1918
				RomanizationTable["І"] = "Ī";
				RomanizationTable["і"] = "ī";
				RomanizationTable["Ѳ"] = "Ḟ";
				RomanizationTable["ѳ"] = "ḟ";
				RomanizationTable["Ѣ"] = "Ê";
				RomanizationTable["ѣ"] = "ê";
				RomanizationTable["Ѵ"] = "Y̆";
				RomanizationTable["ѵ"] = "y̆";

				// Digraphs specific to this system
				DigraphTable["Тс"] = "T-s";
				DigraphTable["тс"] = "t-s";

				#endregion
			}

			/// <summary>
			/// Performs romanization according to the British Standard 2979:1958 on the given text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			public string Process(string text)
				=> text
					.ReplaceFromChart(DigraphTable)
					.ReplaceMany(HardSignSub)
					.ReplaceFromChart(RomanizationTable);
		}
	}
}
