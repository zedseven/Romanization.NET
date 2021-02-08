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
		/// The GOST 16876-71(2) romanization system of Russian.<br />
		/// This is Table 2 of the GOST 16876-71 system with 1 Cyrillic to potentially many Latin chars, without diacritics.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/GOST_16876-71'>https://en.wikipedia.org/wiki/GOST_16876-71</a>
		/// </summary>
		public sealed class Gost16876712 : IMultiCulturalRomanizationSystem
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
			public Gost16876712()
			{
				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/Romanization_of_Russian and https://en.wikipedia.org/wiki/GOST_16876-71

				RomanizationTable["а"] = "a";
				RomanizationTable["б"] = "b";
				RomanizationTable["в"] = "v";
				RomanizationTable["г"] = "g";
				RomanizationTable["д"] = "d";
				RomanizationTable["е"] = "e";
				RomanizationTable["ё"] = "jo";
				RomanizationTable["ж"] = "zh";
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
				RomanizationTable["х"] = "kh";
				RomanizationTable["ц"] = "c";
				RomanizationTable["ч"] = "ch";
				RomanizationTable["ш"] = "sh";
				RomanizationTable["щ"] = "shh";
				RomanizationTable["ъ"] = "ʺ";
				RomanizationTable["ы"] = "y";
				RomanizationTable["ь"] = "ʹ";
				RomanizationTable["э"] = "eh";
				RomanizationTable["ю"] = "ju";
				RomanizationTable["я"] = "ja";

				#endregion
			}

			/// <summary>
			/// Performs GOST 16876-71(2) romanization on Russian text.<br />
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
			/// Performs GOST 16876-71(2) romanization on Russian text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			[Pure]
			public string Process(string text)
				=> Process(text, DefaultCulture);
		}
	}
}
