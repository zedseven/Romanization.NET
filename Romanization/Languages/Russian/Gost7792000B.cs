using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
		/// The GOST 7.79-2000(B) romanization system of Russian.<br />
		/// This is System B of the GOST 7.79-2000 system with 1 Cyrillic to potentially many Latin chars, without diacritics.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/GOST_7.79-2000'>https://en.wikipedia.org/wiki/GOST_7.79-2000</a>
		/// </summary>
		public static readonly Lazy<Gost7792000BSystem> Gost7792000B = new Lazy<Gost7792000BSystem>(() => new Gost7792000BSystem());

		/// <summary>
		/// The GOST 7.79-2000(B) romanization system of Russian.<br />
		/// This is System B of the GOST 7.79-2000 system with 1 Cyrillic to potentially many Latin chars, without diacritics.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/GOST_7.79-2000'>https://en.wikipedia.org/wiki/GOST_7.79-2000</a>
		/// </summary>
		public sealed class Gost7792000BSystem : IRomanizationSystem
		{
			/// <inheritdoc />
			public bool TransliterationSystem => true;

			// System-Specific Constants
			private static readonly Dictionary<string, string> RomanizationTable = new Dictionary<string, string>();

			private static LanguageAgnostic.CharSubCased TseVowelsSub;
			private static LanguageAgnostic.CharSubCased TseConsonantsSub;

			internal Gost7792000BSystem()
			{
				TseVowelsSub = new LanguageAgnostic.CharSubCased("Ц([eijy])", "ц([eijy])", "C${1}", "c${1}");
				TseConsonantsSub = new LanguageAgnostic.CharSubCased("Ц([abcdfghklmnopqrstuvwxz])",
					"ц([abcdfghklmnopqrstuvwxz])", "Cz${1}", "cz${1}");

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
				RomanizationTable["Ё"] = "Yo";
				RomanizationTable["ё"] = "yo";
				RomanizationTable["Ж"] = "Zh";
				RomanizationTable["ж"] = "zh";
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
				RomanizationTable["Х"] = "X";
				RomanizationTable["х"] = "x";
				//RomanizationTable["Ц"] = "Cz";
				//RomanizationTable["ц"] = "cz";
				RomanizationTable["Ч"] = "Ch";
				RomanizationTable["ч"] = "ch";
				RomanizationTable["Ш"] = "Sh";
				RomanizationTable["ш"] = "sh";
				RomanizationTable["Щ"] = "Shh";
				RomanizationTable["щ"] = "shh";
				RomanizationTable["Ъ"] = "ʺ";
				RomanizationTable["ъ"] = "ʺ";
				RomanizationTable["Ы"] = "Y";
				RomanizationTable["ы"] = "y";
				RomanizationTable["Ь"] = "";
				RomanizationTable["ь"] = "ʹ";
				RomanizationTable["Э"] = "E`";
				RomanizationTable["э"] = "e`";
				RomanizationTable["Ю"] = "Yu";
				RomanizationTable["ю"] = "yu";
				RomanizationTable["Я"] = "Ya";
				RomanizationTable["я"] = "ya";
				RomanizationTable["Ѣ"] = "Ye";
				RomanizationTable["ѣ"] = "ye";
				RomanizationTable["І"] = "I";
				RomanizationTable["і"] = "i";
				RomanizationTable["Ѳ"] = "Fh";
				RomanizationTable["ѳ"] = "fh";
				RomanizationTable["Ѵ"] = "Yh";
				RomanizationTable["ѵ"] = "yh";
				RomanizationTable["Ѕ"] = "Js";
				RomanizationTable["ѕ"] = "js";

				#endregion
			}

			/// <summary>
			/// Performs GOST 7.79-2000(B) romanization on Russian text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			public string Process(string text)
				=> text
					// Do romanization replacements
					.ReplaceFromChart(RomanizationTable)
					// Render tse (Ц/ц) as "c" if in front of e, i, j, or y, and as "cz" otherwise
					.ReplaceMany(TseVowelsSub, TseConsonantsSub);
		}
	}
}
