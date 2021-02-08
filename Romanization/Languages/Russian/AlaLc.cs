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
		/// The ALA-LC (American Library Association and Library of Congress) Russian romanization system.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/ALA-LC_romanization_for_Russian'>https://en.wikipedia.org/wiki/ALA-LC_romanization_for_Russian</a>
		/// </summary>
		public sealed class AlaLc : IMultiCulturalRomanizationSystem
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
			public AlaLc()
			{
				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/ALA-LC_romanization_for_Russian

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
				RomanizationTable["ц"] = "t͡s";
				RomanizationTable["ч"] = "ch";
				RomanizationTable["ш"] = "sh";
				RomanizationTable["щ"] = "shch";
				RomanizationTable["ъ"] = "ʺ";
				RomanizationTable["ы"] = "y";
				RomanizationTable["ь"] = "ʹ";
				RomanizationTable["э"] = "ė";
				RomanizationTable["ю"] = "i͡u";
				RomanizationTable["я"] = "i͡a";

				// Letters eliminated in the orthographic reform of 1918
				RomanizationTable["і"] = "ī";
				RomanizationTable["ѣ"] = "i͡e";
				RomanizationTable["ѳ"] = "ḟ";
				RomanizationTable["ѵ"] = "ẏ";

				// Pre-18th century letters
				RomanizationTable["є"] = "ē";
				RomanizationTable["ѥ"] = "i͡e";
				RomanizationTable["ѕ"] = "ż";
				RomanizationTable["ꙋ"] = "ū";
				RomanizationTable["ѿ"] = "ō͡t";
				RomanizationTable["ѡ"] = "ō";
				RomanizationTable["ѧ"] = "ę";
				RomanizationTable["ѯ"] = "k͡s";
				RomanizationTable["ѱ"] = "p͡s";
				RomanizationTable["ѫ"] = "ǫ";
				RomanizationTable["ѩ"] = "i͡ę";
				RomanizationTable["ѭ"] = "i͡ǫ";

				#endregion
			}

			/// <summary>
			/// Performs ALA-LC Russian romanization on the given text.<br />
			/// Supports providing a specific <paramref name="nativeCulture"/> to process with, as long as the country
			/// code is <c>ru</c>.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <param name="nativeCulture">The culture to use.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			/// <exception cref="IrrelevantCultureException"><paramref name="nativeCulture"/> is irrelevant to the
			/// language/region.</exception>
			[Pure]
			public string Process(string text, CultureInfo nativeCulture)
			{
				if (nativeCulture.TwoLetterISOLanguageName.ToLowerInvariant() != "ru")
					throw new IrrelevantCultureException(nativeCulture.DisplayName, nameof(nativeCulture));
				return Utilities.RunWithCulture(nativeCulture,
					() => text.LanguageWidePreparation().ReplaceFromChartWithSameCase(RomanizationTable));
			}

			/// <summary>
			/// Performs ALA-LC Russian romanization on the given text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			[Pure]
			public string Process(string text)
				=> Process(text, DefaultCulture);
		}
	}
}
