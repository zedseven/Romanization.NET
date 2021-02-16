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
		/// The International Scholarly System of romanization for Russian.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Scientific_transliteration_of_Cyrillic'>https://en.wikipedia.org/wiki/Scientific_transliteration_of_Cyrillic</a>
		/// </summary>
		public sealed class Scholarly : IMultiInCultureSystem
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
			public Scholarly() : this(CultureInfo.GetCultureInfo(DefaultNativeCulture)) {}

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.<br />
			/// Supports providing a specific <paramref name="nativeCulture"/> to process with, as long as the country
			/// code is <c>ru</c>.
			/// </summary>
			/// <param name="nativeCulture">The culture to romanize from.</param>
			/// <exception cref="IrrelevantCultureException"><paramref name="nativeCulture"/> is irrelevant to the
			/// language/region.</exception>
			public Scholarly(CultureInfo nativeCulture)
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
						{"а",  "a"},
						{"б",  "b"},
						{"в",  "v"},
						{"г",  "g"},
						{"д",  "d"},
						{"е",  "e"},
						{"ё",  "ë"},
						{"ж",  "ž"},
						{"з",  "z"},
						{"и",  "i"},
						{"й",  "j"},
						{"к",  "k"},
						{"л",  "l"},
						{"м",  "m"},
						{"н",  "n"},
						{"о",  "o"},
						{"п",  "p"},
						{"р",  "r"},
						{"с",  "s"},
						{"т",  "t"},
						{"у",  "u"},
						{"ф",  "f"},
						{"х",  "x"},
						{"ц",  "c"},
						{"ч",  "č"},
						{"ш",  "š"},
						{"щ", "šč"},
						{"ъ",  "ʺ"},
						{"ы",  "y"},
						{"ь",  "ʹ"},
						{"э",  "è"},
						{"ю", "ju"},
						{"я", "ja"},

						// Letters eliminated in the orthographic reform of 1918
						{"і", "i"},
						{"ѳ", "f"},
						{"ѣ", "ě"},
						{"ѵ", "i"},

						// Pre-18th century letters
						{"є",  "e"},
						{"ѥ", "je"},
						{"ѕ", "dz"},
						{"ꙋ",  "u"},
						{"ѡ",  "ô"},
						{"ѿ", "ôt"},
						{"ѫ",  "ǫ"},
						{"ѧ",  "ę"},
						{"ѭ", "jǫ"},
						{"ѩ", "ję"},
						{"ѯ", "ks"},
						{"ѱ", "ps"}
					};

				#endregion
			}

			/// <summary>
			/// Performs romanization according to the International Scholarly System on the given text.
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
