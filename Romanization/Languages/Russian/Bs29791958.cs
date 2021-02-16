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
		/// The British Standard 2979:1958 system of romanization for Russian.<br />
		/// It is the main system of Oxford University Press, and was used by the British Library up until 1975. ALA-LC
		/// is now used instead.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Romanization_of_Russian#British_Standard'>https://en.wikipedia.org/wiki/Romanization_of_Russian#British_Standard</a>
		/// </summary>
		public sealed class Bs29791958 : IMultiInCultureSystem
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

			private readonly CharSub HardSignSub = new("[Ъъ]\\b", "");

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.
			/// </summary>
			public Bs29791958() : this(CultureInfo.GetCultureInfo(DefaultNativeCulture)) {}

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.<br />
			/// Supports providing a specific <paramref name="nativeCulture"/> to process with, as long as the country
			/// code is <c>ru</c>.
			/// </summary>
			/// <param name="nativeCulture">The culture to romanize from.</param>
			/// <exception cref="IrrelevantCultureException"><paramref name="nativeCulture"/> is irrelevant to the
			/// language/region.</exception>
			public Bs29791958(CultureInfo nativeCulture)
			{
				if (nativeCulture.TwoLetterISOLanguageName.ToLowerInvariant() != "ru")
					throw new IrrelevantCultureException(nativeCulture.DisplayName, nameof(nativeCulture));

				NativeCulture = nativeCulture;

				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/BGN/PCGN_romanization_of_Russian

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
						{"ц",   "ts"},
						{"ч",   "ch"},
						{"ш",   "sh"},
						{"щ", "shch"},
						{"ъ",    "ʺ"},
						{"ы",    "ȳ"},
						{"ь",    "ʹ"},
						{"э",    "é"},
						{"ю",   "yu"},
						{"я",   "ya"},

						// Letters eliminated in the orthographic reform of 1918
						{"і", "ī"},
						{"ѳ", "ḟ"},
						{"ѣ", "ê"},
						{"ѵ", "y̆"}
					};

				// Digraphs specific to this system
				DigraphTable = new ReplacementChart(StringComparer.Create(NativeCulture, CompareOptions.IgnoreCase))
				{
					{"тс", "t-s"}
				};

				#endregion
			}

			/// <summary>
			/// Performs romanization according to the British Standard 2979:1958 on the given text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			[Pure]
			public string Process(string text)
				=> CulturalOperations.RunWithCulture(NativeCulture, () => text.LanguageWidePreparation()
					.ReplaceFromChartCaseAware(DigraphTable)
					.ReplaceMany(HardSignSub)
					.ReplaceFromChartCaseAware(RomanizationTable));
		}
	}
}
