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
		/// The International Scholarly System of romanization for Russian.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Scientific_transliteration_of_Cyrillic'>https://en.wikipedia.org/wiki/Scientific_transliteration_of_Cyrillic</a>
		/// </summary>
		public sealed class Scholarly : IMultiInCultureSystem
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
			public Scholarly()
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
				RomanizationTable["ё"] = "ë";
				RomanizationTable["ж"] = "ž";
				RomanizationTable["з"] = "z";
				RomanizationTable["и"] = "i";
				RomanizationTable["й"] = "j";
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
				RomanizationTable["х"] = "x";
				RomanizationTable["ц"] = "c";
				RomanizationTable["ч"] = "č";
				RomanizationTable["ш"] = "š";
				RomanizationTable["щ"] = "šč";
				RomanizationTable["ъ"] = "ʺ";
				RomanizationTable["ы"] = "y";
				RomanizationTable["ь"] = "ʹ";
				RomanizationTable["э"] = "è";
				RomanizationTable["ю"] = "ju";
				RomanizationTable["я"] = "ja";

				// Letters eliminated in the orthographic reform of 1918
				RomanizationTable["і"] = "i";
				RomanizationTable["ѳ"] = "f";
				RomanizationTable["ѣ"] = "ě";
				RomanizationTable["ѵ"] = "i";

				// Pre-18th century letters
				RomanizationTable["є"] = "e";
				RomanizationTable["ѥ"] = "je";
				RomanizationTable["ѕ"] = "dz";
				RomanizationTable["ꙋ"] = "u";
				RomanizationTable["ѡ"] = "ô";
				RomanizationTable["ѿ"] = "ôt";
				RomanizationTable["ѫ"] = "ǫ";
				RomanizationTable["ѧ"] = "ę";
				RomanizationTable["ѭ"] = "jǫ";
				RomanizationTable["ѩ"] = "ję";
				RomanizationTable["ѯ"] = "ks";
				RomanizationTable["ѱ"] = "ps";

				#endregion
			}

			/// <summary>
			/// Performs romanization according to the International Scholarly System on the given text.<br />
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
			/// Performs romanization according to the International Scholarly System on the given text.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
			[Pure]
			public string Process(string text)
				=> Process(text, DefaultCulture);
		}
	}
}
