using Romanization.LanguageAgnostic;
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
		/// The ISO Recommendation No. 9 (ISO/R 9:1968) system of romanization, specialized for Russian.<br />
		/// This transliteration table is designed to cover Bulgarian, Russian, Belarusian, Ukrainian, Serbo-Croatian and Macedonian in general, with regional specializations for certain languages.<br />
		/// This is largely superceded by ISO 9 (GOST 7.79-2000(A)).<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/ISO_9#ISO/R_9'>https://en.wikipedia.org/wiki/ISO_9#ISO/R_9</a>
		/// </summary>
		public sealed class IsoR9 : IMultiCulturalRomanizationSystem
		{
			/// <inheritdoc />
			public SystemType Type => SystemType.Transliteration;

			/// <inheritdoc />
			public CultureInfo DefaultCulture => CultureInfo.GetCultureInfo("ru-RU");

			// System-Specific Constants
			private readonly Dictionary<string, string> RomanizationTable = new Dictionary<string, string>();

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.
			/// </summary>
			public IsoR9()
			{
				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/ISO_9#ISO/R_9

				RomanizationTable["а"] = "a";
				RomanizationTable["б"] = "b";
				RomanizationTable["в"] = "v";
				RomanizationTable["г"] = "g";
				RomanizationTable["д"] = "d";
				RomanizationTable["ѓ"] = "ǵ";
				RomanizationTable["ђ"] = "đ";
				RomanizationTable["е"] = "e";
				RomanizationTable["ё"] = "ë";
				RomanizationTable["є"] = "je";
				RomanizationTable["ж"] = "ž";
				RomanizationTable["з"] = "z";
				RomanizationTable["ѕ"] = "dz";
				RomanizationTable["и"] = "i";
				RomanizationTable["і"] = "i";
				RomanizationTable["ї"] = "ï";
				RomanizationTable["й"] = "j";
				RomanizationTable["к"] = "k";
				RomanizationTable["л"] = "l";
				RomanizationTable["љ"] = "lj";
				RomanizationTable["м"] = "m";
				RomanizationTable["н"] = "n";
				RomanizationTable["њ"] = "nj";
				RomanizationTable["о"] = "o";
				RomanizationTable["п"] = "p";
				RomanizationTable["р"] = "r";
				RomanizationTable["с"] = "s";
				RomanizationTable["т"] = "t";
				RomanizationTable["ќ"] = "ḱ";
				RomanizationTable["ћ"] = "ć";
				RomanizationTable["у"] = "u";
				RomanizationTable["ў"] = "ŭ";
				RomanizationTable["ф"] = "f";
				RomanizationTable["х"] = "h"; // RU specialization
				RomanizationTable["ц"] = "c";
				RomanizationTable["ч"] = "č";
				RomanizationTable["џ"] = "dž";
				RomanizationTable["ш"] = "š";
				RomanizationTable["щ"] = "šč";
				RomanizationTable["ъ"] = "ʺ";
				RomanizationTable["ы"] = "y";
				RomanizationTable["ь"] = "ʹ";
				RomanizationTable["ѣ"] = "ě";
				RomanizationTable["э"] = "ė";
				RomanizationTable["ю"] = "ju";
				RomanizationTable["я"] = "ja";
				RomanizationTable["’"] = "ʺ";
				RomanizationTable["ѫ"] = "ʺ̣";
				RomanizationTable["ѳ"] = "ḟ";
				RomanizationTable["ѵ"] = "ẏ";

				#endregion
			}

			/// <summary>
			/// Performs romanization according to ICAO Doc 9303 on the given text.<br />
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
				return Utilities.RunWithCulture(nativeCulture, () => text.LanguageWidePreparation().ReplaceFromChartWithSameCase(RomanizationTable));
			}

			/// <summary>
			/// Performs romanization according to ISO/R 9:1968 on the given text, with regional specializations applied for Russian.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			[Pure]
			public string Process(string text)
				=> Process(text, DefaultCulture);
		}
	}
}
