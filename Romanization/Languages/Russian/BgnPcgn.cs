using Romanization.LanguageAgnostic;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;

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
		public sealed class BgnPcgn : IMultiCulturalRomanizationSystem
		{
			/// <inheritdoc />
			public SystemType Type => SystemType.Transliteration;

			/// <inheritdoc />
			public CultureInfo DefaultCulture => CultureInfo.GetCultureInfo("ru-RU");

			// System-Specific Constants
			private readonly Dictionary<string, string> RomanizationTable = new Dictionary<string, string>();
			private readonly Dictionary<string, string> DigraphTable = new Dictionary<string, string>();

			private readonly CharSubCased YeProvisionSub = new CharSubCased(
				$"(^|\\b|[{RussianVowels}ЙйЪъЬь])Е", $"(^|\\b|[{RussianVowels}ЙйЪъЬь])е",
				"${1}Ye", "${1}ye");

			private readonly CharSubCased YoProvisionSub = new CharSubCased(
				$"(^|\\b|[{RussianVowels}ЙйЪъЬь])Ё", $"(^|\\b|[{RussianVowels}ЙйЪъЬь])ё",
				"${1}Yё", "${1}yё");

			private readonly CharSubCased IDigraphSub = new CharSubCased(
				"Й([АаУуЫыЭэ])", "й([АаУуЫыЭэ])",
				"Y·${1}", "y·${1}");

			private readonly CharSubCased YeryExceptionDigraphSub = new CharSubCased(
				"Ы([АаУуЫыЭэ])", "ы([АаУуЫыЭэ])",
				"Y·${1}", "y·${1}");

			private readonly CharSubCased YeryVowelsDigraphSub = new CharSubCased(
				$"([{RussianVowels}])Ы", $"([{RussianVowels}])ы",
				"${1}·Y", "${1}·y");

			private readonly CharSubCased EConsonantsDigraphSub = new CharSubCased(
				$"([{RussianConsonants.WithoutChars("Йй")}])Э", $"([{RussianConsonants.WithoutChars("Йй")}])э",
					"${1}·E", "${1}·e");

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.
			/// </summary>
			public BgnPcgn()
			{
				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/BGN/PCGN_romanization_of_Russian

				// Main characters (2021)
				RomanizationTable["а"] = "a";
				RomanizationTable["б"] = "b";
				RomanizationTable["в"] = "v";
				RomanizationTable["г"] = "g";
				RomanizationTable["д"] = "d";
				RomanizationTable["е"] = "e"; // has special provisions
				RomanizationTable["ё"] = "ë"; // has special provisions
				RomanizationTable["ж"] = "zh";
				RomanizationTable["з"] = "z";
				RomanizationTable["и"] = "i";
				RomanizationTable["й"] = "y"; // has special provisions
				RomanizationTable["к"] = "k";
				RomanizationTable["л"] = "l";
				RomanizationTable["м"] = "m";
				RomanizationTable["н"] = "n";
				RomanizationTable["о"] = "o";
				RomanizationTable["п"] = "p";
				RomanizationTable["р"] = "r";
				RomanizationTable["с"] = "s";
				RomanizationTable["т"] = "t";
				RomanizationTable["у"] = "u";
				RomanizationTable["ф"] = "f";
				RomanizationTable["х"] = "kh";
				RomanizationTable["ц"] = "ts";
				RomanizationTable["ч"] = "ch";
				RomanizationTable["ш"] = "sh";
				RomanizationTable["щ"] = "shch";
				RomanizationTable["ъ"] = "ʺ";
				RomanizationTable["ы"] = "y"; // has special provisions
				RomanizationTable["ь"] = "ʹ";
				RomanizationTable["э"] = "e"; // has special provisions
				RomanizationTable["ю"] = "yu";
				RomanizationTable["я"] = "ya";

				// Digraphs specific to this system
				DigraphTable["тс"] = "t·s";
				DigraphTable["шч"] = "sh·ch";

				#endregion
			}

			/// <summary>
			/// Performs romanization according to the BGN/PCGN system on the given text.<br />
			/// Supports providing a specific <paramref name="culture"/> to process with, as long as the country code is <c>ru</c>.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <param name="useDigraphs">Whether or not to insert special digraph combinations with interpunct characters (eg. <c>шч</c> -> <c>sh·ch</c>).</param>
			/// <param name="culture">The culture to use.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			/// <exception cref="IrrelevantCultureException"><paramref name="culture"/> is irrelevant to the language/region.</exception>
			[Pure]
			public string Process(string text, bool useDigraphs, CultureInfo culture)
			{
				if (culture.TwoLetterISOLanguageName.ToLowerInvariant() != "ru")
					throw new IrrelevantCultureException(culture.DisplayName, nameof(culture));

				return Utilities.RunWithCulture(culture, () =>
				{
					text = text.LanguageWidePreparation();

					// Digraphs first, if they're to be inserted
					if (useDigraphs)
						text = text.ReplaceFromChartWithSameCase(DigraphTable)
							.ReplaceMany(IDigraphSub,
								YeryExceptionDigraphSub, YeryVowelsDigraphSub,
								EConsonantsDigraphSub);

					// Then single characters
					return text
						.ReplaceMany(YeProvisionSub, YoProvisionSub)
						.ReplaceFromChartWithSameCase(RomanizationTable);
				});
			}

			/// <summary>
			/// Performs romanization according to the BGN/PCGN system on the given text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <param name="useDigraphs">Whether or not to insert special digraph combinations with interpunct characters (eg. <c>шч</c> -> <c>sh·ch</c>).</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			[Pure]
			public string Process(string text, bool useDigraphs)
				=> Process(text, useDigraphs, DefaultCulture);

			/// <summary>
			/// Performs romanization according to the BGN/PCGN system on the given text.<br />
			/// Supports providing a specific <paramref name="nativeCulture"/> to process with, as long as the country code is <c>ru</c>.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <param name="nativeCulture">The culture to use.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			/// <exception cref="IrrelevantCultureException"><paramref name="nativeCulture"/> is irrelevant to the language/region.</exception>
			[Pure]
			public string Process(string text, CultureInfo nativeCulture)
				=> Process(text, true, nativeCulture);

			/// <summary>
			/// Performs romanization according to the BGN/PCGN system on the given text, using digraphs.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			[Pure]
			public string Process(string text)
				=> Process(text, true, DefaultCulture);
		}
	}
}
