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
		/// The GOST 16876-71(2) romanization system of Russian.<br />
		/// This is Table 2 of the GOST 16876-71 system with 1 Cyrillic to potentially many Latin chars, without diacritics.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/GOST_16876-71'>https://en.wikipedia.org/wiki/GOST_16876-71</a>
		/// </summary>
		public static readonly Lazy<Gost16876712System> Gost16876712 = new Lazy<Gost16876712System>(() => new Gost16876712System());

		/// <summary>
		/// The GOST 16876-71(2) romanization system of Russian.<br />
		/// This is Table 2 of the GOST 16876-71 system with 1 Cyrillic to potentially many Latin chars, without diacritics.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/GOST_16876-71'>https://en.wikipedia.org/wiki/GOST_16876-71</a>
		/// </summary>
		public sealed class Gost16876712System : IRomanizationSystem
		{
			/// <inheritdoc />
			public bool TransliterationSystem => true;

			// System-Specific Constants
			private static readonly Dictionary<string, string> RomanizationTable = new Dictionary<string, string>();

			internal Gost16876712System()
			{
				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/Romanization_of_Russian and https://en.wikipedia.org/wiki/GOST_16876-71

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
				RomanizationTable["Ё"] = "Jo";
				RomanizationTable["ё"] = "jo";
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
				RomanizationTable["Х"] = "Kh";
				RomanizationTable["х"] = "kh";
				RomanizationTable["Ц"] = "C";
				RomanizationTable["ц"] = "c";
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
				RomanizationTable["Ь"] = "ʹ";
				RomanizationTable["ь"] = "ʹ";
				RomanizationTable["Э"] = "Eh";
				RomanizationTable["э"] = "eh";
				RomanizationTable["Ю"] = "Ju";
				RomanizationTable["ю"] = "ju";
				RomanizationTable["Я"] = "Ja";
				RomanizationTable["я"] = "ja";

				#endregion
			}

			/// <summary>
			/// Performs GOST 16876-71(2) romanization on Russian text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			public string Process(string text)
				=> text.ReplaceFromChart(RomanizationTable);
		}
	}
}
