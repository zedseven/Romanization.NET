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
		/// The GOST 7.79-2000(B) romanization system of Russian.<br />
		/// This is System B of the GOST 7.79-2000 system with 1 Cyrillic to potentially many Latin chars, without
		/// diacritics.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/GOST_7.79-2000'>https://en.wikipedia.org/wiki/GOST_7.79-2000</a>
		/// </summary>
		public sealed class Gost7792000B : IMultiInCultureSystem
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
			private readonly ReplacementChart CasedTable;

			private readonly CharSubCased TseVowelsSub = new(
				"Ц([eijy])", "ц([eijy])",
				"C${1}",     "c${1}");

			// The reason this is done as opposed to having the consonant value in the chart is because the vowel exception is based on latin vowels
			private readonly CharSubCased TseConsonantsSub = new(
				"Ц([abcdfghklmnopqrstuvwxz])", "ц([abcdfghklmnopqrstuvwxz])",
				"Cz${1}",                      "cz${1}");

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.
			/// </summary>
			public Gost7792000B() : this(CultureInfo.GetCultureInfo(DefaultNativeCulture)) {}

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.<br />
			/// Supports providing a specific <paramref name="nativeCulture"/> to process with, as long as the country
			/// code is <c>ru</c>.
			/// </summary>
			/// <param name="nativeCulture">The culture to romanize from.</param>
			/// <exception cref="IrrelevantCultureException"><paramref name="nativeCulture"/> is irrelevant to the language/region.</exception>
			public Gost7792000B(CultureInfo nativeCulture)
			{
				if (nativeCulture.TwoLetterISOLanguageName.ToLowerInvariant() != "ru")
					throw new IrrelevantCultureException(nativeCulture.DisplayName, nameof(nativeCulture));

				NativeCulture = nativeCulture;

				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/Romanization_of_Russian and https://en.wikipedia.org/wiki/GOST_7.79-2000

				RomanizationTable =
					new ReplacementChart(StringComparer.Create(NativeCulture, CompareOptions.IgnoreCase))
					{
						{"а",   "a"},
						{"б",   "b"},
						{"в",   "v"},
						{"г",   "g"},
						{"д",   "d"},
						{"е",   "e"},
						{"ё",  "yo"},
						{"ж",  "zh"},
						{"з",   "z"},
						{"и",   "i"},
						{"й",   "j"},
						{"к",   "k"},
						{"л",   "l"},
						{"м",   "m"},
						{"н",   "n"},
						{"о",   "o"},
						{"п",   "p"},
						{"р",   "r"},
						{"с",   "s"},
						{"т",   "t"},
						{"у",   "u"},
						{"ф",   "f"},
						{"х",   "x"},
						//{"ц",  "cz"},
						{"ч",  "ch"},
						{"ш",  "sh"},
						{"щ", "shh"},
						{"ъ",   "ʺ"},
						{"ы",   "y"},
						{"э",  "e`"},
						{"ю",  "yu"},
						{"я",  "ya"},
						{"ѣ",  "ye"},
						{"і",   "i"},
						{"ѳ",  "fh"},
						{"ѵ",  "yh"},
						{"ѕ",  "js"}
					};

				CasedTable = new ReplacementChart(StringComparer.Create(NativeCulture, CompareOptions.None))
				{
					{"Ь",  ""},
					{"ь", "ʹ"}
				};

				#endregion
			}

			/// <summary>
			/// Performs GOST 7.79-2000(B) romanization on Russian text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			[Pure]
			public string Process(string text)
				=> CulturalOperations.RunWithCulture(NativeCulture, () => text.LanguageWidePreparation()
					// Do romanization replacements
					.ReplaceFromChartCaseAware(RomanizationTable)
					// Do cased romanization replacements
					.ReplaceFromChartCaseAware(CasedTable)
					// Render tse (Ц/ц) as "c" if in front of e, i, j, or y, and as "cz" otherwise
					.ReplaceMany(TseVowelsSub, TseConsonantsSub));
		}
	}
}
