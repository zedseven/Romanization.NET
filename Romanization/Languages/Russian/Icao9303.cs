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
		/// The system from ICAO Doc 9303 "Machine Readable Travel Documents, Part 3".<br />
		/// This is the standard for modern Russian passports, in 2021.<br />
		/// For more information, visit:
		/// <a href='https://www.icao.int/publications/Documents/9303_p3_cons_en.pdf'>https://www.icao.int/publications/Documents/9303_p3_cons_en.pdf</a>
		/// </summary>
		public static readonly Lazy<Icao9303System> Icao9303 = new Lazy<Icao9303System>(() => new Icao9303System());

		/// <summary>
		/// The system from ICAO Doc 9303 "Machine Readable Travel Documents, Part 3".<br />
		/// This is the standard for modern Russian passports, in 2021.<br />
		/// For more information, visit:
		/// <a href='https://www.icao.int/publications/Documents/9303_p3_cons_en.pdf'>https://www.icao.int/publications/Documents/9303_p3_cons_en.pdf</a>
		/// </summary>
		public sealed class Icao9303System : IRomanizationSystem
		{
			/// <inheritdoc />
			public bool TransliterationSystem => true;

			// System-Specific Constants
			private static readonly Dictionary<string, string> RomanizationTable = new Dictionary<string, string>();

			internal Icao9303System()
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
				RomanizationTable["Ё"] = "E";
				RomanizationTable["ё"] = "e";
				RomanizationTable["Ж"] = "Zh";
				RomanizationTable["ж"] = "zh";
				RomanizationTable["З"] = "Z";
				RomanizationTable["з"] = "z";
				RomanizationTable["И"] = "I";
				RomanizationTable["и"] = "i";
				RomanizationTable["Й"] = "I";
				RomanizationTable["й"] = "i";
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
				RomanizationTable["Ъ"] = "Ie";
				RomanizationTable["ъ"] = "ie";
				RomanizationTable["Ы"] = "Y";
				RomanizationTable["ы"] = "y";
				RomanizationTable["Ь"] = "";
				RomanizationTable["ь"] = "";
				RomanizationTable["Э"] = "E";
				RomanizationTable["э"] = "e";
				RomanizationTable["Ю"] = "Iu";
				RomanizationTable["ю"] = "iu";
				RomanizationTable["Я"] = "Ia";
				RomanizationTable["я"] = "ia";

				#endregion
			}

			/// <summary>
			/// Performs romanization according to ICAO Doc 9303 on the given text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			public string Process(string text)
				=> text.ReplaceFromChart(RomanizationTable);
		}
	}
}
