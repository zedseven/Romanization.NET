using Romanization.Internal;
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
		/// The GOST 16876-71(1) (UNGEGN) romanization system of Russian.<br />
		/// This system was recommended by the United Nations Group of Experts on Geographical Names.<br />
		/// This is Table 1 of the GOST 16876-71 system with 1 Cyrillic to 1 Latin char, with diacritics.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/GOST_16876-71'>https://en.wikipedia.org/wiki/GOST_16876-71</a>
		/// </summary>
		public sealed class Gost16876711 : IMultiInCultureSystem
		{
			/// <inheritdoc />
			public SystemType Type => SystemType.Transliteration;

			/// <inheritdoc />
			public CultureInfo DefaultCulture => CultureInfo.GetCultureInfo("ru-RU");

			// System-Specific Constants
			private readonly Dictionary<string, string> RomanizationTable = new();

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.
			/// </summary>
			public Gost16876711()
			{
				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/Romanization_of_Russian and https://en.wikipedia.org/wiki/GOST_16876-71

				// Main characters (2021)
				RomanizationTable["а"] = "a";
				RomanizationTable["б"] = "b";
				RomanizationTable["в"] = "v";
				RomanizationTable["г"] = "g";
				RomanizationTable["д"] = "d";
				RomanizationTable["е"] = "e";
				RomanizationTable["ё"] = "ë";
				RomanizationTable["ж"] = "ž";
				RomanizationTable["з"] = "z";
				RomanizationTable["и"] = "i";
				RomanizationTable["й"] = "j";
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
				RomanizationTable["х"] = "h";
				RomanizationTable["ц"] = "c";
				RomanizationTable["ч"] = "č";
				RomanizationTable["ш"] = "š";
				RomanizationTable["щ"] = "ŝ";
				RomanizationTable["ъ"] = "ʺ";
				RomanizationTable["ы"] = "y";
				RomanizationTable["ь"] = "ʹ";
				RomanizationTable["э"] = "ė";
				RomanizationTable["ю"] = "û";
				RomanizationTable["я"] = "â";

				// Letters eliminated in the orthographic reform of 1918
				RomanizationTable["і"] = "i";
				RomanizationTable["ѳ"] = "ḟ";
				RomanizationTable["ѣ"] = "ě";
				RomanizationTable["ѵ"] = "ẏ";

				#endregion
			}

			/// <summary>
			/// Performs GOST 16876-71(1) romanization on Russian text.<br />
			/// Supports providing a specific <paramref name="nativeCulture"/> to process with, as long as the country code is <c>ru</c>.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <param name="nativeCulture">The culture to use.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			/// <exception cref="IrrelevantCultureException"><paramref name="nativeCulture"/> is irrelevant to the language/region.</exception>
			[Pure]
			public string Process(string text, CultureInfo nativeCulture)
			{
				if (nativeCulture.TwoLetterISOLanguageName.ToLowerInvariant() != "ru")
					throw new IrrelevantCultureException(nativeCulture.DisplayName, nameof(nativeCulture));
				return CulturalOperations.RunWithCulture(nativeCulture, () => text.LanguageWidePreparation().ReplaceFromChartWithSameCase(RomanizationTable));
			}

			/// <summary>
			/// Performs GOST 16876-71(1) romanization on Russian text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			[Pure]
			public string Process(string text)
				=> Process(text, DefaultCulture);
		}
	}
}
