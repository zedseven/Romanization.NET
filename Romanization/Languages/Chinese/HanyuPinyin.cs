using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming

namespace Romanization
{
	public static partial class Chinese
	{
		/// <summary>
		/// The Hànyǔ Pīnyīn Chinese romanization system.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Hanyu_Pinyin'>https://en.wikipedia.org/wiki/Hanyu_Pinyin</a>
		/// </summary>
		public static readonly Lazy<HanyuPinyinSystem> HanyuPinyin = new Lazy<HanyuPinyinSystem>(() => new HanyuPinyinSystem());

		/// <summary>
		/// The Hànyǔ Pīnyīn Chinese romanization system.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Hanyu_Pinyin'>https://en.wikipedia.org/wiki/Hanyu_Pinyin</a>
		/// </summary>
		public sealed class HanyuPinyinSystem : IReadingsRomanizationSystem<HanyuPinyinSystem.ReadingTypes>
		{
			/// <summary>
			/// The supported reading types for Hànyǔ Pīnyīn.
			/// </summary>
			[Flags]
			public enum ReadingTypes
			{
				/// <summary>
				/// Standard Hànyǔ Pīnyīn.
				/// </summary>
				HanyuPinyin = 1,
				/// <summary>
				/// Hànyǔ Pīnyīn as it appeared in Xiàndài Hànyǔ Pínlǜ Cídiǎn.
				/// </summary>
				HanyuPinlu = 1 << 1,
				/// <summary>
				/// Hànyǔ Pīnyīn as it appeared in Xiàndài Hànyǔ Cídiǎn.
				/// </summary>
				XHC = 1 << 2
			}

			private const string HanyuPinyinFileName = "HanziHanyuPinyin.csv";
			private const string HanyuPinluFileName = "HanziHanyuPinlu.csv";
			private const string XhcFileName = "HanziXHC.csv";

			private static readonly Dictionary<string, string[]> HanyuPinyinReadings = new Dictionary<string, string[]>();
			private static readonly Dictionary<string, string[]> HanyuPinluReadings = new Dictionary<string, string[]>();
			private static readonly Dictionary<string, string[]> XhcReadings = new Dictionary<string, string[]>();

			internal HanyuPinyinSystem()
			{
				Utilities.LoadCharacterMap(HanyuPinyinFileName, HanyuPinyinReadings, k => k, v => v.Split(' '));
				Utilities.LoadCharacterMap(HanyuPinluFileName, HanyuPinluReadings, k => k, v => v.Split(' '));
				Utilities.LoadCharacterMap(XhcFileName, XhcReadings, k => k, v => v.Split(' '));
			}

			/// <summary>
			/// Performs Hànyǔ Pīnyīn romanization on the given text.<br />
			/// Uses the first (oft-most-common) reading of the character - standard Hànyǔ Pīnyīn first if available, then Hànyǔ Pīnyīn as it appeared in Xiàndài Hànyǔ Pínlǜ Cídiǎn, then as it appeared in Xiàndài Hànyǔ Cídiǎn.<br />
			/// If more readings are required, use <see cref="ProcessWithReadings(string, ReadingTypes)"/> instead.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <param name="readingsToUse">The reading types to use.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched. Note that all romanized text will be lowercase.</returns>
			[Pure]
			public string Process(string text, ReadingTypes readingsToUse)
				=> string.Join("", ProcessWithReadings(text, readingsToUse).Characters
					.Select(c => c.Readings.Length > 0 ? c.Readings[0].Value : c.Character));

			/// <summary>
			/// Performs Hànyǔ Pīnyīn romanization on the given text.<br />
			/// Uses the first (oft-most-common) reading of the character - standard Hànyǔ Pīnyīn first if available, then Hànyǔ Pīnyīn as it appeared in Xiàndài Hànyǔ Pínlǜ Cídiǎn, then as it appeared in Xiàndài Hànyǔ Cídiǎn.<br />
			/// If more readings are required, use <see cref="ProcessWithReadings(string)"/> instead.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched. Note that all romanized text will be lowercase.</returns>
			[Pure]
			public string Process(string text)
				=> string.Join("", ProcessWithReadings(text).Characters
					.Select(c => c.Readings.Length > 0 ? c.Readings[0].Value : c.Character));

			/// <summary>
			/// Performs Hànyǔ Pīnyīn romanization on the given text.<br />
			/// Returns a collection of all the characters in <paramref name="text"/>, but with all readings (pronunciations) of each.<br />
			/// Returns the following readings for characters if in <paramref name="readingsToUse"/> and they exist: standard Hànyǔ Pīnyīn, Hànyǔ Pīnyīn as it appeared in Xiàndài Hànyǔ Pínlǜ Cídiǎn, and Hànyǔ Pīnyīn as it appeared in Xiàndài Hànyǔ Cídiǎn.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <param name="readingsToUse">The reading types to use.</param>
			/// <returns>A <see cref="LanguageAgnostic.ReadingsString{ReadingTypes}"/> with all readings for each character in <paramref name="text"/>.</returns>
			[Pure]
			public LanguageAgnostic.ReadingsString<ReadingTypes> ProcessWithReadings(string text, ReadingTypes readingsToUse)
				=> new LanguageAgnostic.ReadingsString<ReadingTypes>(text.SplitIntoSurrogatePairs()
					.Select(c =>
					{
						List<LanguageAgnostic.Reading<ReadingTypes>> readings = new List<LanguageAgnostic.Reading<ReadingTypes>>(text.Length);

						if (readingsToUse.HasFlag(ReadingTypes.HanyuPinyin) && HanyuPinyinReadings.TryGetValue(c, out string[] rawHanyuPinyinReadings))
							readings.AddRange(rawHanyuPinyinReadings.Select(r => new LanguageAgnostic.Reading<ReadingTypes>(ReadingTypes.HanyuPinyin, r)));
						if (readingsToUse.HasFlag(ReadingTypes.HanyuPinlu) && HanyuPinluReadings.TryGetValue(c, out string[] rawHanyuPinluReadings))
							readings.AddRange(rawHanyuPinluReadings.Select(r => new LanguageAgnostic.Reading<ReadingTypes>(ReadingTypes.HanyuPinlu, r)));
						if (readingsToUse.HasFlag(ReadingTypes.XHC) && XhcReadings.TryGetValue(c, out string[] rawXhcReadings))
							readings.AddRange(rawXhcReadings.Select(r => new LanguageAgnostic.Reading<ReadingTypes>(ReadingTypes.XHC, r)));

						return new LanguageAgnostic.ReadingCharacter<ReadingTypes>(c, readings);
					})
					.ToArray());

			/// <summary>
			/// Performs Hànyǔ Pīnyīn romanization on the given text.<br />
			/// Returns a collection of all the characters in <paramref name="text"/>, but with all readings (pronunciations) of each.<br />
			/// Returns the following readings for characters if they exist: standard Hànyǔ Pīnyīn, Hànyǔ Pīnyīn as it appeared in Xiàndài Hànyǔ Pínlǜ Cídiǎn, and Hànyǔ Pīnyīn as it appeared in Xiàndài Hànyǔ Cídiǎn.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A <see cref="LanguageAgnostic.ReadingsString{ReadingTypes}"/> with all readings for each character in <paramref name="text"/>.</returns>
			[Pure]
			public LanguageAgnostic.ReadingsString<ReadingTypes> ProcessWithReadings(string text)
				=> ProcessWithReadings(text, ReadingTypes.HanyuPinyin | ReadingTypes.HanyuPinlu | ReadingTypes.XHC);
		}
	}
}
