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
		/// The general road sign romanization system of Russian.<br />
		/// This consists of Russian GOST R 52290-2004 (tables Г.4, Г.5) as well as GOST 10807-78 (tables 17, 18), historically.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Romanization_of_Russian#Street_and_road_signs'>https://en.wikipedia.org/wiki/Romanization_of_Russian#Street_and_road_signs</a>
		/// </summary>
		public static readonly Lazy<RoadSignsSystem> RoadSigns = new Lazy<RoadSignsSystem>(() => new RoadSignsSystem());

		/// <summary>
		/// The general road sign romanization system of Russian.<br />
		/// This consists of Russian GOST R 52290-2004 (tables Г.4, Г.5) as well as GOST 10807-78 (tables 17, 18), historically.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Romanization_of_Russian#Street_and_road_signs'>https://en.wikipedia.org/wiki/Romanization_of_Russian#Street_and_road_signs</a>
		/// </summary>
		public sealed class RoadSignsSystem : IRomanizationSystem
		{
			/// <inheritdoc />
			public bool TransliterationSystem => true;

			// System-Specific Constants
			private static readonly Dictionary<string, string> RomanizationTable = new Dictionary<string, string>();

			private static CharSubCased YeVowelsSub;
			private static CharSubCased YoVowelsSub;
			private static CharSubCased YoExceptionsSub;

			internal RoadSignsSystem()
			{
				YeVowelsSub = new CharSubCased("(^|\\b|[ИиЫыЭэЕеАаЯяОоЁёУуЮюЪъЬь])Е",
					"(^|\\b|[ИиЫыЭэЕеАаЯяОоЁёУуЮюЪъЬь])е", "${1}Ye", "${1}ye");
				YoVowelsSub = new CharSubCased("(^|\\b|[ИиЫыЭэЕеАаЯяОоЁёУуЮюЪъЬь])Ё",
					"(^|\\b|[ИиЫыЭэЕеАаЯяОоЁёУуЮюЪъЬь])ё", "${1}Yo", "${1}yo");
				YoExceptionsSub = new CharSubCased("(^|\\b|[ЧчШшЩщЖж])Ё", "(^|\\b|[ЧчШшЩщЖж])ё", "${1}E", "${1}e");

				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/Romanization_of_Russian

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
				RomanizationTable["Ё"] = "Ye";
				RomanizationTable["ё"] = "ye";
				RomanizationTable["Ж"] = "Zh";
				RomanizationTable["ж"] = "zh";
				RomanizationTable["З"] = "Z";
				RomanizationTable["з"] = "z";
				RomanizationTable["И"] = "I";
				RomanizationTable["и"] = "i";
				RomanizationTable["Й"] = "Y";
				RomanizationTable["й"] = "y";
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
				RomanizationTable["Ъ"] = "ʹ";
				RomanizationTable["ъ"] = "ʹ";
				RomanizationTable["Ы"] = "Y";
				RomanizationTable["ы"] = "y";
				RomanizationTable["Ь"] = "ʹ";
				RomanizationTable["ь"] = "ʹ";
				RomanizationTable["Э"] = "E";
				RomanizationTable["э"] = "e";
				RomanizationTable["Ю"] = "Yu";
				RomanizationTable["ю"] = "yu";
				RomanizationTable["Я"] = "Ya";
				RomanizationTable["я"] = "ya";

				#endregion
			}

			/// <summary>
			/// Performs general road sign romanization on Russian text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			public string Process(string text)
				=> text
					// Render ye (Е) and yo (Ё) in different forms depending on what preceeds them
					.ReplaceMany(YeVowelsSub, YoVowelsSub, YoExceptionsSub)
					// Do remaining romanization replacements
					.ReplaceFromChart(RomanizationTable);
		}
	}
}
