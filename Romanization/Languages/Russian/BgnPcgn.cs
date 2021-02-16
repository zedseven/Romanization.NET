using Romanization.Internal;
using System;
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
		/// It was developed by the Unites States Board on Geographic Names and the Permanent Committee on Geographical
		/// Names for British Official Use, and is
		/// designed to be easier for anglophones to pronounce.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/BGN/PCGN_romanization_of_Russian'>https://en.wikipedia.org/wiki/BGN/PCGN_romanization_of_Russian</a>
		/// </summary>
		public sealed class BgnPcgn : IMultiInCultureSystem
		{
			/// <inheritdoc />
			public SystemType Type => SystemType.Transliteration;

			/// <summary>
			/// The default culture this system will use.
			/// </summary>
			public const string DefaultNativeCulture = "ru-RU";

			/// <inheritdoc />
			public CultureInfo NativeCulture { get; }

			// System-Specific Constants
			private readonly ReplacementChart RomanizationTable;
			private readonly ReplacementChart DigraphTable;

			private readonly CharSubCased YeProvisionSub = new(
				$"(^|\\b|[{RussianVowels}ЙйЪъЬь])Е", $"(^|\\b|[{RussianVowels}ЙйЪъЬь])е",
				 "${1}Ye",                            "${1}ye");

			private readonly CharSubCased YoProvisionSub = new(
				$"(^|\\b|[{RussianVowels}ЙйЪъЬь])Ё", $"(^|\\b|[{RussianVowels}ЙйЪъЬь])ё",
				 "${1}Yё",                            "${1}yё");

			private readonly CharSubCased IDigraphSub = new(
				"Й([АаУуЫыЭэ])", "й([АаУуЫыЭэ])",
				"Y·${1}",        "y·${1}");

			private readonly CharSubCased YeryExceptionDigraphSub = new(
				"Ы([АаУуЫыЭэ])", "ы([АаУуЫыЭэ])",
				"Y·${1}",        "y·${1}");

			private readonly CharSubCased YeryVowelsDigraphSub = new(
				$"([{RussianVowels}])Ы", $"([{RussianVowels}])ы",
				 "${1}·Y",                "${1}·y");

			private readonly CharSubCased EConsonantsDigraphSub = new(
				$"([{RussianConsonants.WithoutChars("Йй")}])Э", $"([{RussianConsonants.WithoutChars("Йй")}])э",
				 "${1}·E",                                       "${1}·e");

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.
			/// </summary>
			public BgnPcgn() : this(CultureInfo.GetCultureInfo(DefaultNativeCulture)) {}

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.<br />
			/// Supports providing a specific <paramref name="nativeCulture"/> to process with, as long as the country
			/// code is <c>ru</c>.
			/// </summary>
			/// <param name="nativeCulture">The culture to romanize from.</param>
			/// <exception cref="IrrelevantCultureException"><paramref name="nativeCulture"/> is irrelevant to the
			/// language/region.</exception>
			public BgnPcgn(CultureInfo nativeCulture)
			{
				if (nativeCulture.TwoLetterISOLanguageName.ToLowerInvariant() != "ru")
					throw new IrrelevantCultureException(nativeCulture.DisplayName, nameof(nativeCulture));

				NativeCulture = nativeCulture;

				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/BGN/PCGN_romanization_of_Russian

				// Main characters (2021)
				RomanizationTable =
					new ReplacementChart(StringComparer.Create(NativeCulture, CompareOptions.IgnoreCase))
					{
						{"а",    "a"},
						{"б",    "b"},
						{"в",    "v"},
						{"г",    "g"},
						{"д",    "d"},
						{"е",    "e"}, // has special provisions
						{"ё",    "ë"}, // has special provisions
						{"ж",   "zh"},
						{"з",    "z"},
						{"и",    "i"},
						{"й",    "y"}, // has special provisions
						{"к",    "k"},
						{"л",    "l"},
						{"м",    "m"},
						{"н",    "n"},
						{"о",    "o"},
						{"п",    "p"},
						{"р",    "r"},
						{"с",    "s"},
						{"т",    "t"},
						{"у",    "u"},
						{"ф",    "f"},
						{"х",   "kh"},
						{"ц",   "ts"},
						{"ч",   "ch"},
						{"ш",   "sh"},
						{"щ", "shch"},
						{"ъ",    "ʺ"},
						{"ы",    "y"}, // has special provisions
						{"ь",    "ʹ"},
						{"э",    "e"}, // has special provisions
						{"ю",   "yu"},
						{"я",   "ya"}
					};

				// Digraphs specific to this system
				DigraphTable = new ReplacementChart(StringComparer.Create(NativeCulture, CompareOptions.IgnoreCase))
				{
					{"тс",   "t·s"},
					{"шч", "sh·ch"}
				};

				#endregion
			}

			/// <summary>
			/// Performs romanization according to the BGN/PCGN system on the given text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <param name="useDigraphs">Whether or not to insert special digraph combinations with interpunct
			/// characters (eg. <c>шч</c> -> <c>sh·ch</c>).</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			[Pure]
			public string Process(string text, bool useDigraphs)
				=> CulturalOperations.RunWithCulture(NativeCulture, () =>
				{
					text = text.LanguageWidePreparation();

					// Digraphs first, if they're to be inserted
					if (useDigraphs)
						text = text.ReplaceFromChartCaseAware(DigraphTable)
							.ReplaceMany(IDigraphSub,
								YeryExceptionDigraphSub, YeryVowelsDigraphSub,
								EConsonantsDigraphSub);

					// Then single characters
					return text
						.ReplaceMany(YeProvisionSub, YoProvisionSub)
						.ReplaceFromChartCaseAware(RomanizationTable);
				});

			/// <summary>
			/// Performs romanization according to the BGN/PCGN system on the given text, using digraphs.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			[Pure]
			public string Process(string text)
				=> Process(text, true);
		}
	}
}
