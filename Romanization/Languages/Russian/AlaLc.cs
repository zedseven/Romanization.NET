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
		/// The ALA-LC (American Library Association and Library of Congress) Russian romanization system.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/ALA-LC_romanization_for_Russian'>https://en.wikipedia.org/wiki/ALA-LC_romanization_for_Russian</a>
		/// </summary>
		public sealed class AlaLc : IRomanizationSystem
		{
			/// <inheritdoc />
			public bool TransliterationSystem => true;

			// System-Specific Constants
			private readonly Dictionary<string, string> RomanizationTable = new Dictionary<string, string>();

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.
			/// </summary>
			public AlaLc()
			{
				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/ALA-LC_romanization_for_Russian

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
				RomanizationTable["Ц"] = "T͡s";
				RomanizationTable["ц"] = "t͡s";
				RomanizationTable["Ч"] = "Ch";
				RomanizationTable["ч"] = "ch";
				RomanizationTable["Ш"] = "Sh";
				RomanizationTable["ш"] = "sh";
				RomanizationTable["Щ"] = "Shch";
				RomanizationTable["щ"] = "shch";
				RomanizationTable["Ъ"] = "ʺ";
				RomanizationTable["ъ"] = "ʺ";
				RomanizationTable["Ы"] = "Y";
				RomanizationTable["ы"] = "y";
				RomanizationTable["Ь"] = "ʹ";
				RomanizationTable["ь"] = "ʹ";
				RomanizationTable["Э"] = "Ė";
				RomanizationTable["э"] = "ė";
				RomanizationTable["Ю"] = "I͡u";
				RomanizationTable["ю"] = "i͡u";
				RomanizationTable["Я"] = "I͡a";
				RomanizationTable["я"] = "i͡a";

				// Letters eliminated in the orthographic reform of 1918
				RomanizationTable["І"] = "І̄";
				RomanizationTable["і"] = "ī";
				RomanizationTable["Ѣ"] = "I͡e";
				RomanizationTable["ѣ"] = "i͡e";
				RomanizationTable["Ѳ"] = "Ḟ";
				RomanizationTable["ѳ"] = "ḟ";
				RomanizationTable["Ѵ"] = "Ẏ";
				RomanizationTable["ѵ"] = "ẏ";

				// Pre-18th century letters
				RomanizationTable["Є"] = "Ē";
				RomanizationTable["є"] = "ē";
				RomanizationTable["Ѥ"] = "I͡e";
				RomanizationTable["ѥ"] = "i͡e";
				RomanizationTable["Ѕ"] = "Ż";
				RomanizationTable["ѕ"] = "ż";
				RomanizationTable["Ꙋ"] = "Ū";
				RomanizationTable["ꙋ"] = "ū";
				RomanizationTable["Ѿ"] = "Ō͡t";
				RomanizationTable["ѿ"] = "ō͡t";
				RomanizationTable["Ѡ"] = "Ō";
				RomanizationTable["ѡ"] = "ō";
				RomanizationTable["Ѧ"] = "Ę";
				RomanizationTable["ѧ"] = "ę";
				RomanizationTable["Ѯ"] = "K͡s";
				RomanizationTable["ѯ"] = "k͡s";
				RomanizationTable["Ѱ"] = "P͡s";
				RomanizationTable["ѱ"] = "p͡s";
				RomanizationTable["Ѫ"] = "Ǫ";
				RomanizationTable["ѫ"] = "ǫ";
				RomanizationTable["Ѩ"] = "I͡ę";
				RomanizationTable["ѩ"] = "i͡ę";
				RomanizationTable["Ѭ"] = "I͡ǫ";
				RomanizationTable["ѭ"] = "i͡ǫ";

				#endregion
			}

			/// <summary>
			/// Performs ALA-LC Russian romanization on the given text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			public string Process(string text)
				=> text.ReplaceFromChart(RomanizationTable);
		}
	}
}
