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
		/// The system from ICAO Doc 9303 "Machine Readable Travel Documents, Part 3".<br />
		/// This is the standard for modern Russian passports, in 2021.<br />
		/// For more information, visit:
		/// <a href='https://www.icao.int/publications/Documents/9303_p3_cons_en.pdf'>https://www.icao.int/publications/Documents/9303_p3_cons_en.pdf</a>
		/// </summary>
		public sealed class Icao9303 : IMultiInCultureSystem
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
			public Icao9303()
			{
				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/Scientific_transliteration_of_Cyrillic

				// Main characters (2021)
				RomanizationTable["а"] = "a";
				RomanizationTable["б"] = "b";
				RomanizationTable["в"] = "v";
				RomanizationTable["г"] = "g";
				RomanizationTable["д"] = "d";
				RomanizationTable["е"] = "e";
				RomanizationTable["ё"] = "e";
				RomanizationTable["ж"] = "zh";
				RomanizationTable["з"] = "z";
				RomanizationTable["и"] = "i";
				RomanizationTable["й"] = "i";
				RomanizationTable["і"] = "i";
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
				RomanizationTable["х"] = "kh";
				RomanizationTable["ц"] = "ts";
				RomanizationTable["ч"] = "ch";
				RomanizationTable["ш"] = "sh";
				RomanizationTable["щ"] = "shch";
				RomanizationTable["ъ"] = "ie";
				RomanizationTable["ы"] = "y";
				RomanizationTable["ь"] = "";
				RomanizationTable["э"] = "e";
				RomanizationTable["ю"] = "iu";
				RomanizationTable["я"] = "ia";

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
				return CulturalOperations.RunWithCulture(nativeCulture, () => text.LanguageWidePreparation().ReplaceFromChartWithSameCase(RomanizationTable));
			}

			/// <summary>
			/// Performs romanization according to ICAO Doc 9303 on the given text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			[Pure]
			public string Process(string text)
				=> Process(text, DefaultCulture);
		}
	}
}
