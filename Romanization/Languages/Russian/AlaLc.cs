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
		/// The ALA-LC (American Library Association and Library of Congress) Russian romanization system.<br />
		/// For more information, visit:<br />
		/// <a href='https://en.wikipedia.org/wiki/ALA-LC_romanization_for_Russian'>https://en.wikipedia.org/wiki/ALA-LC_romanization_for_Russian</a><br />
		/// and<br />
		/// <a href='https://www.loc.gov/catdir/cpso/romanization/russian.pdf'>https://www.loc.gov/catdir/cpso/romanization/russian.pdf</a>
		/// </summary>
		public sealed class AlaLc : IMultiInCultureSystem
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
			public AlaLc() : this(CultureInfo.GetCultureInfo(DefaultNativeCulture)) {}

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.<br />
			/// Supports providing a specific <paramref name="nativeCulture"/> to process with, as long as the country
			/// code is <c>ru</c>.
			/// </summary>
			/// <param name="nativeCulture">The culture to romanize from.</param>
			/// <exception cref="IrrelevantCultureException"><paramref name="nativeCulture"/> is irrelevant to the
			/// language/region.</exception>
			public AlaLc(CultureInfo nativeCulture)
			{
				if (nativeCulture.TwoLetterISOLanguageName.ToLowerInvariant() != "ru")
					throw new IrrelevantCultureException(nativeCulture.DisplayName, nameof(nativeCulture));

				NativeCulture = nativeCulture;

				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/ALA-LC_romanization_for_Russian

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
						{"ё",    "ë"},
						{"ж",   "zh"},
						{"з",    "z"},
						{"и",    "i"},
						{"й",    "ĭ"},
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
						{"ц",   "t͡s"},
						{"ч",   "ch"},
						{"ш",   "sh"},
						{"щ", "shch"},
						{"ъ",    "ʺ"},
						{"ы",    "y"},
						{"ь",    "ʹ"},
						{"э",    "ė"},
						{"ю",   "i͡u"},
						{"я",   "i͡a"},

						// Letters eliminated in the orthographic reform of 1918
						{"і",  "ī"},
						{"ѣ", "i͡e"},
						{"ѳ",  "ḟ"},
						{"ѵ",  "ẏ"},

						// Pre-18th century letters
						{"є",  "ē"},
						{"ѥ", "i͡e"},
						{"ѕ",  "ż"},
						{"ꙋ",  "ū"},
						{"ѿ", "ō͡t"},
						{"ѡ",  "ō"},
						{"ѧ",  "ę"},
						{"ѯ", "k͡s"},
						{"ѱ", "p͡s"},
						{"ѫ",  "ǫ"},
						{"ѩ", "i͡ę"},
						{"ѭ", "i͡ǫ"}
					};

				#endregion
			}

			/// <summary>
			/// Performs ALA-LC Russian romanization on the given text.
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
