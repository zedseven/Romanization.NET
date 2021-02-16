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
		/// The GOST 7.79-2000(A) romanization system of Russian.<br />
		/// This is System A of the GOST 7.79-2000 system with 1 Cyrillic to 1 Latin char, with diacritics.<br />
		/// Identical to ISO 9:1995 (different to ISO/R 9:1968).<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/GOST_7.79-2000'>https://en.wikipedia.org/wiki/GOST_7.79-2000</a>
		/// </summary>
		public sealed class Gost7792000A : IMultiInCultureSystem
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
			public Gost7792000A() : this(CultureInfo.GetCultureInfo(DefaultNativeCulture)) {}

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.<br />
			/// Supports providing a specific <paramref name="nativeCulture"/> to process with, as long as the country
			/// code is <c>ru</c>.
			/// </summary>
			/// <param name="nativeCulture">The culture to romanize from.</param>
			/// <exception cref="IrrelevantCultureException"><paramref name="nativeCulture"/> is irrelevant to the
			/// language/region.</exception>
			public Gost7792000A(CultureInfo nativeCulture)
			{
				if (nativeCulture.TwoLetterISOLanguageName.ToLowerInvariant() != "ru")
					throw new IrrelevantCultureException(nativeCulture.DisplayName, nameof(nativeCulture));

				NativeCulture = nativeCulture;

				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/Romanization_of_Russian and https://en.wikipedia.org/wiki/GOST_7.79-2000

				RomanizationTable =
					new ReplacementChart(StringComparer.Create(NativeCulture, CompareOptions.IgnoreCase))
					{
						{"а", "a"},
						{"б", "b"},
						{"в", "v"},
						{"г", "g"},
						{"д", "d"},
						{"е", "e"},
						{"ё", "ë"},
						{"ж", "ž"},
						{"з", "z"},
						{"и", "i"},
						{"й", "j"},
						{"к", "k"},
						{"л", "l"},
						{"м", "m"},
						{"н", "n"},
						{"о", "o"},
						{"п", "p"},
						{"р", "r"},
						{"с", "s"},
						{"т", "t"},
						{"у", "u"},
						{"ф", "f"},
						{"х", "h"},
						{"ц", "c"},
						{"ч", "č"},
						{"ш", "š"},
						{"щ", "ŝ"},
						{"ъ", "ʺ"},
						{"ы", "y"},
						{"ь", "ʹ"},
						{"э", "è"},
						{"ю", "û"},
						{"я", "â"},
						{"і", "ì"},
						{"ѳ", "f̀"},
						{"ѣ", "ě"},
						{"ѵ", "ỳ"},
						{"ѕ", "ẑ"},
						{"ѫ", "ǎ"}
					};

				#endregion
			}

			/// <summary>
			/// Performs GOST 7.79-2000(A) romanization on Russian text.
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
