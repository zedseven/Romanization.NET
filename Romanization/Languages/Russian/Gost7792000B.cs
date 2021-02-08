using System;
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
		/// The GOST 7.79-2000(B) romanization system of Russian.<br />
		/// This is System B of the GOST 7.79-2000 system with 1 Cyrillic to potentially many Latin chars, without diacritics.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/GOST_7.79-2000'>https://en.wikipedia.org/wiki/GOST_7.79-2000</a>
		/// </summary>
		public sealed class Gost7792000B : IMultiInCultureSystem
		{
			/// <inheritdoc />
			public SystemType Type => SystemType.Transliteration;

			/// <inheritdoc />
			public CultureInfo DefaultCulture => CultureInfo.GetCultureInfo("ru-RU");

			// System-Specific Constants
			private readonly Dictionary<string, string> RomanizationTable = new Dictionary<string, string>();
			private readonly Dictionary<string, string> CasedTable = new Dictionary<string, string>();

			private readonly CharSubCased TseVowelsSub = new CharSubCased(
				"Ц([eijy])", "ц([eijy])",
				"C${1}", "c${1}");

			// The reason this is done as opposed to having the consonant value in the chart is because the vowel exception is based on latin vowels
			private readonly CharSubCased TseConsonantsSub = new CharSubCased(
				"Ц([abcdfghklmnopqrstuvwxz])", "ц([abcdfghklmnopqrstuvwxz])",
				"Cz${1}", "cz${1}");

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.
			/// </summary>
			public Gost7792000B()
			{
				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/Romanization_of_Russian and https://en.wikipedia.org/wiki/GOST_7.79-2000

				RomanizationTable["а"] = "a";
				RomanizationTable["б"] = "b";
				RomanizationTable["в"] = "v";
				RomanizationTable["г"] = "g";
				RomanizationTable["д"] = "d";
				RomanizationTable["е"] = "e";
				RomanizationTable["ё"] = "yo";
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
				RomanizationTable["х"] = "x";
				//RomanizationTable["ц"] = "cz";
				RomanizationTable["ч"] = "ch";
				RomanizationTable["ш"] = "sh";
				RomanizationTable["щ"] = "shh";
				RomanizationTable["ъ"] = "ʺ";
				RomanizationTable["ы"] = "y";
				RomanizationTable["э"] = "e`";
				RomanizationTable["ю"] = "yu";
				RomanizationTable["я"] = "ya";
				RomanizationTable["ѣ"] = "ye";
				RomanizationTable["і"] = "i";
				RomanizationTable["ѳ"] = "fh";
				RomanizationTable["ѵ"] = "yh";
				RomanizationTable["ѕ"] = "js";

				CasedTable["Ь"] = "";
				CasedTable["ь"] = "ʹ";

				#endregion
			}

			/// <summary>
			/// Performs GOST 7.79-2000(B) romanization on Russian text.<br />
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
				return CulturalOperations.RunWithCulture(nativeCulture, () => text.LanguageWidePreparation()
					// Do romanization replacements
					.ReplaceFromChartWithSameCase(RomanizationTable)
					// Do cased romanization replacements
					.ReplaceFromChartWithSameCase(CasedTable, StringComparison.CurrentCulture)
					// Render tse (Ц/ц) as "c" if in front of e, i, j, or y, and as "cz" otherwise
					.ReplaceMany(TseVowelsSub, TseConsonantsSub));
			}

			/// <summary>
			/// Performs GOST 7.79-2000(B) romanization on Russian text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			[Pure]
			public string Process(string text)
				=> Process(text, DefaultCulture);
		}
	}
}
