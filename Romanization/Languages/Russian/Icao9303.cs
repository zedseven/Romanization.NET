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
		/// The system from ICAO Doc 9303 "Machine Readable Travel Documents, Part 3".<br />
		/// This is the standard for modern Russian passports, in 2021.<br />
		/// For more information, visit:
		/// <a href='https://www.icao.int/publications/Documents/9303_p3_cons_en.pdf'>https://www.icao.int/publications/Documents/9303_p3_cons_en.pdf</a>
		/// </summary>
		public sealed class Icao9303 : IMultiInCultureSystem
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
			public Icao9303() : this(CultureInfo.GetCultureInfo(DefaultNativeCulture)) {}

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.<br />
			/// Supports providing a specific <paramref name="nativeCulture"/> to process with, as long as the country
			/// code is <c>ru</c>.
			/// </summary>
			/// <param name="nativeCulture">The culture to romanize from.</param>
			/// <exception cref="IrrelevantCultureException"><paramref name="nativeCulture"/> is irrelevant to the
			/// language/region.</exception>
			public Icao9303(CultureInfo nativeCulture)
			{
				if (nativeCulture.TwoLetterISOLanguageName.ToLowerInvariant() != "ru")
					throw new IrrelevantCultureException(nativeCulture.DisplayName, nameof(nativeCulture));

				NativeCulture = nativeCulture;

				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/Scientific_transliteration_of_Cyrillic

				RomanizationTable =
					new ReplacementChart(StringComparer.Create(NativeCulture, CompareOptions.IgnoreCase))
					{
						// Main characters (2021)
						{"а",    "a"},
						{"б",    "b"},
						{"в",    "v"},
						{"г",    "g"},
						{"д",    "d"},
						{"е",    "e"},
						{"ё",    "e"},
						{"ж",   "zh"},
						{"з",    "z"},
						{"и",    "i"},
						{"й",    "i"},
						{"і",    "i"},
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
						{"ъ",   "ie"},
						{"ы",    "y"},
						{"ь",     ""},
						{"э",    "e"},
						{"ю",   "iu"},
						{"я",   "ia"}
					};

				#endregion
			}

			/// <summary>
			/// Performs romanization according to ICAO Doc 9303 on the given text.
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
