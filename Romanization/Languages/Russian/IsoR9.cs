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
		/// The ISO Recommendation No. 9 (ISO/R 9:1968) system of romanization, specialized for Russian.<br />
		/// This transliteration table is designed to cover Bulgarian, Russian, Belarusian, Ukrainian, Serbo-Croatian
		/// and Macedonian in general, with regional specializations for certain languages.<br />
		/// This is largely superceded by ISO 9 (GOST 7.79-2000(A)).<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/ISO_9#ISO/R_9'>https://en.wikipedia.org/wiki/ISO_9#ISO/R_9</a>
		/// </summary>
		public sealed class IsoR9 : IMultiInCultureSystem
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

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.
			/// </summary>
			public IsoR9() : this(CultureInfo.GetCultureInfo(DefaultNativeCulture)) {}

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.<br />
			/// Supports providing a specific <paramref name="nativeCulture"/> to process with, as long as the country
			/// code is <c>ru</c>.
			/// </summary>
			/// <param name="nativeCulture">The culture to romanize from.</param>
			/// <exception cref="IrrelevantCultureException"><paramref name="nativeCulture"/> is irrelevant to the
			/// language/region.</exception>
			public IsoR9(CultureInfo nativeCulture)
			{
				if (nativeCulture.TwoLetterISOLanguageName.ToLowerInvariant() != "ru")
					throw new IrrelevantCultureException(nativeCulture.DisplayName, nameof(nativeCulture));

				NativeCulture = nativeCulture;

				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/ISO_9#ISO/R_9

				RomanizationTable =
					new ReplacementChart(StringComparer.Create(NativeCulture, CompareOptions.IgnoreCase))
					{
						{"а",  "a"},
						{"б",  "b"},
						{"в",  "v"},
						{"г",  "g"},
						{"д",  "d"},
						{"ѓ",  "ǵ"},
						{"ђ",  "đ"},
						{"е",  "e"},
						{"ё",  "ë"},
						{"є", "je"},
						{"ж",  "ž"},
						{"з",  "z"},
						{"ѕ", "dz"},
						{"и",  "i"},
						{"і",  "i"},
						{"ї",  "ï"},
						{"й",  "j"},
						{"к",  "k"},
						{"л",  "l"},
						{"љ", "lj"},
						{"м",  "m"},
						{"н",  "n"},
						{"њ", "nj"},
						{"о",  "o"},
						{"п",  "p"},
						{"р",  "r"},
						{"с",  "s"},
						{"т",  "t"},
						{"ќ",  "ḱ"},
						{"ћ",  "ć"},
						{"у",  "u"},
						{"ў",  "ŭ"},
						{"ф",  "f"},
						{"х",  "h"}, // RU specialization
						{"ц",  "c"},
						{"ч",  "č"},
						{"џ", "dž"},
						{"ш",  "š"},
						{"щ", "šč"},
						{"ъ",  "ʺ"},
						{"ы",  "y"},
						{"ь",  "ʹ"},
						{"ѣ",  "ě"},
						{"э",  "ė"},
						{"ю", "ju"},
						{"я", "ja"},
						{"’",  "ʺ"},
						{"ѫ",  "ʺ̣"},
						{"ѳ",  "ḟ"},
						{"ѵ",  "ẏ"}
					};

				#endregion
			}

			/// <summary>
			/// Performs romanization according to ISO/R 9:1968 on the given text, with regional specializations applied for Russian.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			[Pure]
			public string Process(string text)
				=> CulturalOperations.RunWithCulture(NativeCulture,
					() => text.LanguageWidePreparation().ReplaceFromChartCaseAware(RomanizationTable));
		}
	}
}
