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
		/// The general road sign romanization system of Russian.<br />
		/// This consists of Russian GOST R 52290-2004 (tables Г.4, Г.5) as well as GOST 10807-78 (tables 17, 18),
		/// historically.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Romanization_of_Russian#Street_and_road_signs'>https://en.wikipedia.org/wiki/Romanization_of_Russian#Street_and_road_signs</a>
		/// </summary>
		public sealed class RoadSigns : IMultiInCultureSystem
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

			private readonly CharSubCased YeVowelsSub = new(
				$"(^|\\b|[{RussianVowels}ЪъЬь])Е", $"(^|\\b|[{RussianVowels}ЪъЬь])е",
				 "${1}Ye",                          "${1}ye");

			private readonly CharSubCased YoVowelsSub = new(
				$"(^|\\b|[{RussianVowels}ЪъЬь])Ё", $"(^|\\b|[{RussianVowels}ЪъЬь])ё",
				 "${1}Yo",                          "${1}yo");

			private readonly CharSubCased YoExceptionsSub = new(
				"(^|\\b|[ЧчШшЩщЖж])Ё", "(^|\\b|[ЧчШшЩщЖж])ё",
				"${1}E",               "${1}e");

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.
			/// </summary>
			public RoadSigns() : this(CultureInfo.GetCultureInfo(DefaultNativeCulture)) {}

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.<br />
			/// Supports providing a specific <paramref name="nativeCulture"/> to process with, as long as the country
			/// code is <c>ru</c>.
			/// </summary>
			/// <param name="nativeCulture">The culture to romanize from.</param>
			/// <exception cref="IrrelevantCultureException"><paramref name="nativeCulture"/> is irrelevant to the
			/// language/region.</exception>
			public RoadSigns(CultureInfo nativeCulture)
			{
				if (nativeCulture.TwoLetterISOLanguageName.ToLowerInvariant() != "ru")
					throw new IrrelevantCultureException(nativeCulture.DisplayName, nameof(nativeCulture));

				NativeCulture = nativeCulture;

				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/Romanization_of_Russian

				RomanizationTable =
					new ReplacementChart(StringComparer.Create(NativeCulture, CompareOptions.IgnoreCase))
					{
						{"а",    "a"},
						{"б",    "b"},
						{"в",    "v"},
						{"г",    "g"},
						{"д",    "d"},
						{"е",    "e"},
						{"ё",   "ye"},
						{"ж",   "zh"},
						{"з",    "z"},
						{"и",    "i"},
						{"й",    "y"},
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
						{"ъ",    "ʹ"},
						{"ы",    "y"},
						{"ь",    "ʹ"},
						{"э",    "e"},
						{"ю",   "yu"},
						{"я",   "ya"}
					};

				#endregion
			}

			/// <summary>
			/// Performs general road sign romanization on Russian text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			[Pure]
			public string Process(string text)
				=> CulturalOperations.RunWithCulture(NativeCulture, () => text.LanguageWidePreparation()
					// Render ye (Е) and yo (Ё) in different forms depending on what preceeds them
					.ReplaceMany(YeVowelsSub, YoVowelsSub, YoExceptionsSub)
					// Do remaining romanization replacements
					.ReplaceFromChartCaseAware(RomanizationTable));
		}
	}
}
