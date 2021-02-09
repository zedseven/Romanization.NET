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
		/// The British Standard 2979:1958 system of romanization for Russian.<br />
		/// It is the main system of Oxford University Press, and was used by the British Library up until 1975. ALA-LC is now used instead.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Romanization_of_Russian#British_Standard'>https://en.wikipedia.org/wiki/Romanization_of_Russian#British_Standard</a>
		/// </summary>
		public sealed class Bs29791958 : IMultiInCultureSystem
		{
			/// <inheritdoc />
			public SystemType Type => SystemType.Transliteration;

			/// <inheritdoc />
			public CultureInfo DefaultCulture => CultureInfo.GetCultureInfo("ru-RU");

			// System-Specific Constants
			private readonly Dictionary<string, string> RomanizationTable = new();
			private readonly Dictionary<string, string> DigraphTable = new();

			private readonly CharSub HardSignSub = new("[Ъъ]\\b", "");

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.
			/// </summary>
			public Bs29791958()
			{
				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/BGN/PCGN_romanization_of_Russian

				// Main characters (2021)
				RomanizationTable["а"] = "a";
				RomanizationTable["б"] = "b";
				RomanizationTable["в"] = "v";
				RomanizationTable["г"] = "g";
				RomanizationTable["д"] = "d";
				RomanizationTable["е"] = "e";
				RomanizationTable["ё"] = "ë";
				RomanizationTable["ж"] = "zh";
				RomanizationTable["з"] = "z";
				RomanizationTable["и"] = "i";
				RomanizationTable["й"] = "ĭ";
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
				RomanizationTable["ъ"] = "ʺ";
				RomanizationTable["ы"] = "ȳ";
				RomanizationTable["ь"] = "ʹ";
				RomanizationTable["э"] = "é";
				RomanizationTable["ю"] = "yu";
				RomanizationTable["я"] = "ya";

				// Letters eliminated in the orthographic reform of 1918
				RomanizationTable["і"] = "ī";
				RomanizationTable["ѳ"] = "ḟ";
				RomanizationTable["ѣ"] = "ê";
				RomanizationTable["ѵ"] = "y̆";

				// Digraphs specific to this system
				DigraphTable["тс"] = "t-s";

				#endregion
			}

			/// <summary>
			/// Performs romanization according to the British Standard 2979:1958 on the given text.<br />
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
					.ReplaceFromChartWithSameCase(DigraphTable)
					.ReplaceMany(HardSignSub)
					.ReplaceFromChartWithSameCase(RomanizationTable));
			}

			/// <summary>
			/// Performs romanization according to the British Standard 2979:1958 on the given text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			[Pure]
			public string Process(string text)
				=> Process(text, DefaultCulture);
		}
	}
}
