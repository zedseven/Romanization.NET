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
		/// The general road sign romanization system of Russian.<br />
		/// This consists of Russian GOST R 52290-2004 (tables Г.4, Г.5) as well as GOST 10807-78 (tables 17, 18), historically.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Romanization_of_Russian#Street_and_road_signs'>https://en.wikipedia.org/wiki/Romanization_of_Russian#Street_and_road_signs</a>
		/// </summary>
		public sealed class RoadSigns : IMultiCulturalRomanizationSystem
		{
			/// <inheritdoc />
			public SystemType Type => SystemType.Transliteration;

			/// <inheritdoc />
			public CultureInfo DefaultCulture => CultureInfo.GetCultureInfo("ru-RU");

			// System-Specific Constants
			private readonly Dictionary<string, string> RomanizationTable = new Dictionary<string, string>();

			private readonly CharSubCased YeVowelsSub = new CharSubCased(
				$"(^|\\b|[{RussianVowels}ЪъЬь])Е", $"(^|\\b|[{RussianVowels}ЪъЬь])е",
				"${1}Ye", "${1}ye");

			private readonly CharSubCased YoVowelsSub = new CharSubCased(
				$"(^|\\b|[{RussianVowels}ЪъЬь])Ё", $"(^|\\b|[{RussianVowels}ЪъЬь])ё",
				"${1}Yo", "${1}yo");

			private readonly CharSubCased YoExceptionsSub = new CharSubCased(
				"(^|\\b|[ЧчШшЩщЖж])Ё", "(^|\\b|[ЧчШшЩщЖж])ё",
				"${1}E", "${1}e");

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.
			/// </summary>
			public RoadSigns()
			{
				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/Romanization_of_Russian

				RomanizationTable["а"] = "a";
				RomanizationTable["б"] = "b";
				RomanizationTable["в"] = "v";
				RomanizationTable["г"] = "g";
				RomanizationTable["д"] = "d";
				RomanizationTable["е"] = "e";
				RomanizationTable["ё"] = "ye";
				RomanizationTable["ж"] = "zh";
				RomanizationTable["з"] = "z";
				RomanizationTable["и"] = "i";
				RomanizationTable["й"] = "y";
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
				RomanizationTable["ъ"] = "ʹ";
				RomanizationTable["ы"] = "y";
				RomanizationTable["ь"] = "ʹ";
				RomanizationTable["э"] = "e";
				RomanizationTable["ю"] = "yu";
				RomanizationTable["я"] = "ya";

				#endregion
			}

			/// <summary>
			/// Performs general road sign romanization on Russian text.<br />
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
				return Utilities.RunWithCulture(nativeCulture, () => text.LanguageWidePreparation()
					// Render ye (Е) and yo (Ё) in different forms depending on what preceeds them
					.ReplaceMany(YeVowelsSub, YoVowelsSub, YoExceptionsSub)
					// Do remaining romanization replacements
					.ReplaceFromChart(RomanizationTable));
			}

			/// <summary>
			/// Performs general road sign romanization on Russian text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			[Pure]
			public string Process(string text)
				=> Process(text, DefaultCulture);
		}
	}
}
