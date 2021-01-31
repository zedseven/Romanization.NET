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
		/// The BGN/PCGN system of romanization for Russian.<br />
		/// It was developed by the Unites States Board on Geographic Names and the Permanent Committee on Geographical Names for British Official Use, and is
		/// designed to be easier for anglophones to pronounce.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/BGN/PCGN_romanization_of_Russian'>https://en.wikipedia.org/wiki/BGN/PCGN_romanization_of_Russian</a>
		/// </summary>
		public sealed class BgnPcgn : IRomanizationSystem
		{
			/// <inheritdoc />
			public bool TransliterationSystem => true;

			// System-Specific Constants
			private readonly Dictionary<string, string> RomanizationTable = new Dictionary<string, string>();
			private readonly Dictionary<string, string> DigraphTable = new Dictionary<string, string>();

			private readonly CharSubCased YeProvisionSub = new CharSubCased(
				$"(^|\\b|[{RussianVowels}ЙйЪъЬь])Е", $"(^|\\b|[{RussianVowels}ЙйЪъЬь])е",
				"${1}Ye", "${1}ye");

			private readonly CharSubCased YoProvisionSub = new CharSubCased(
				$"(^|\\b|[{RussianVowels}ЙйЪъЬь])Ё", $"(^|\\b|[{RussianVowels}ЙйЪъЬь])ё",
				"${1}Yё", "${1}yё");

			private readonly CharSubCased IDigraphSub = new CharSubCased(
				"Й([АаУуЫыЭэ])", "й([АаУуЫыЭэ])",
				"Y·${1}", "y·${1}");

			private readonly CharSubCased YeryExceptionDigraphSub = new CharSubCased(
				"Ы([АаУуЫыЭэ])", "ы([АаУуЫыЭэ])",
				"Y·${1}", "y·${1}");

			private readonly CharSubCased YeryVowelsDigraphSub = new CharSubCased(
				$"([{RussianVowels}])Ы", $"([{RussianVowels}])ы",
				"${1}·Y", "${1}·y");

			private readonly CharSubCased EConsonantsDigraphSub = new CharSubCased(
				$"([{RussianConsonants.WithoutChars("Йй")}])Э", $"([{RussianConsonants.WithoutChars("Йй")}])э",
					"${1}·E", "${1}·e");

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.
			/// </summary>
			public BgnPcgn()
			{
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
				RomanizationTable["Е"] = "E"; // has special provisions
				RomanizationTable["е"] = "e"; // has special provisions
				RomanizationTable["Ё"] = "Ë"; // has special provisions
				RomanizationTable["ё"] = "ë"; // has special provisions
				RomanizationTable["Ж"] = "Zh";
				RomanizationTable["ж"] = "zh";
				RomanizationTable["З"] = "Z";
				RomanizationTable["з"] = "z";
				RomanizationTable["И"] = "I";
				RomanizationTable["и"] = "i";
				RomanizationTable["Й"] = "Y"; // has special provisions
				RomanizationTable["й"] = "y"; // has special provisions
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
				RomanizationTable["Ы"] = "Y"; // has special provisions
				RomanizationTable["ы"] = "y"; // has special provisions
				RomanizationTable["Ь"] = "ʹ";
				RomanizationTable["ь"] = "ʹ";
				RomanizationTable["Э"] = "E"; // has special provisions
				RomanizationTable["э"] = "e"; // has special provisions
				RomanizationTable["Ю"] = "Yu";
				RomanizationTable["ю"] = "yu";
				RomanizationTable["Я"] = "Ya";
				RomanizationTable["я"] = "ya";

				// Digraphs specific to this system
				DigraphTable["Тс"] = "T·s";
				DigraphTable["тс"] = "t·s";
				DigraphTable["Шч"] = "Sh·ch";
				DigraphTable["шч"] = "sh·ch";

				#endregion
			}

			/// <summary>
			/// Performs romanization according to the BGN/PCGN system on the given text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <param name="useDigraphs">Whether or not to insert special digraph combinations with interpunct characters (eg. <code>шч</code> -> <code>sh·ch</code>).</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			public string Process(string text, bool useDigraphs)
			{
				// Digraphs first, if they're to be inserted
				if (useDigraphs)
					text = text.ReplaceFromChart(DigraphTable)
						.ReplaceMany(IDigraphSub, YeryExceptionDigraphSub, YeryVowelsDigraphSub, EConsonantsDigraphSub);

				// Then single characters
				return text
					.ReplaceMany(YeProvisionSub, YoProvisionSub)
					.ReplaceFromChart(RomanizationTable);
			}

			/// <summary>
			/// Performs romanization according to the BGN/PCGN system on the given text, using digraphs.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			public string Process(string text)
				=> Process(text, true);
		}
	}
}
