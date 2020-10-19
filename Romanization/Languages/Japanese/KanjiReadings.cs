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
	public static partial class Japanese
	{
		public static readonly Lazy<KanjiReadingsSystem> KanjiReadings = new Lazy<KanjiReadingsSystem>(() => new KanjiReadingsSystem());

		/// <summary>
		/// A system for romanizing Kanji characters.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Kanji'>https://en.wikipedia.org/wiki/Kanji</a>
		/// </summary>
		public sealed class KanjiReadingsSystem : IReadingsRomanizationSystem<KanjiReadingsSystem.ReadingTypes>
		{
			/// <summary>
			/// The supported reading types for Kanji.
			/// </summary>
			[Flags]
			public enum ReadingTypes
			{
				/// <summary>
				/// Kun'yomi, the Japanese-native reading. Often referred to as just Kun.
				/// </summary>
				Kunyomi = 1,
				/// <summary>
				/// On'yomi, the Sino-Japanese reading. Often referred to as just On.
				/// </summary>
				Onyomi = 1 << 1
			}

			private const string KanjiKunFileName = "KanjiKun.csv";
			private const string KanjiOnFileName = "KanjiOn.csv";

			private static readonly Dictionary<string, string[]> KanjiKunReadings = new Dictionary<string, string[]>();
			private static readonly Dictionary<string, string[]> KanjiOnReadings = new Dictionary<string, string[]>();

			internal KanjiReadingsSystem()
			{
				Utilities.LoadCharacterMap(KanjiKunFileName, KanjiKunReadings, k => k, v => v.Split(' '));
				Utilities.LoadCharacterMap(KanjiOnFileName, KanjiOnReadings, k => k, v => v.Split(' '));
			}

			/// <summary>
			/// Performs romanization of all Kanji in the given text.<br />
			/// Uses the first reading of the character - Kun'yomi first, if requested and available, then On'yomi if requested and available.<br />
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
			/// Performs romanization of all Kanji in the given text.<br />
			/// Uses the first reading of the character - Kun'yomi first, if available, then On'yomi.<br />
			/// If more readings are required, use <see cref="ProcessWithReadings(string)"/> instead.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched. Note that all romanized text will be lowercase.</returns>
			[Pure]
			public string Process(string text)
				=> string.Join("", ProcessWithReadings(text).Characters
					.Select(c => c.Readings.Length > 0 ? c.Readings[0].Value : c.Character));

			/// <summary>
			/// Performs romanization of all Kanji in the given text, after using <paramref name="system"/> to handle the kana.<br />
			/// <paramref name="system"/> defaults to <see cref="ModifiedHepburnSystem"/> if left as null.<br />
			/// See the documentation for <see cref="Process(string)"/> and the chosen system for more implementation details.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <param name="system">The system to romanize the kana in <paramref name="text"/> before the Kanji are touched.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched. Note that all romanized text will be lowercase.</returns>
			[Pure]
			public string ProcessWithKana(string text, IRomanizationSystem system = null)
			{
				system ??= ModifiedHepburn.Value;
				return Process(system.Process(text));
			}

			/// <summary>
			/// Performs romanization of all Kanji in the given text.<br />
			/// Returns a collection of all the characters in <paramref name="text"/>, but with all readings (pronunciations) of each.<br />
			/// Can return the following readings for characters if in <paramref name="readingsToUse"/> and they exist: Kun'yomi and On'yomi.
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

						if (readingsToUse.HasFlag(ReadingTypes.Kunyomi) && KanjiKunReadings.TryGetValue(c, out string[] rawKanjiKunReadings))
							readings.AddRange(rawKanjiKunReadings.Select(r => new LanguageAgnostic.Reading<ReadingTypes>(ReadingTypes.Kunyomi, r)));
						if (readingsToUse.HasFlag(ReadingTypes.Onyomi) && KanjiOnReadings.TryGetValue(c, out string[] rawKanjiOnReadings))
							readings.AddRange(rawKanjiOnReadings.Select(r => new LanguageAgnostic.Reading<ReadingTypes>(ReadingTypes.Onyomi, r)));

						return new LanguageAgnostic.ReadingCharacter<ReadingTypes>(c, readings);
					})
					.ToArray());

			/// <summary>
			/// Performs romanization of all Kanji in the given text.<br />
			/// Returns a collection of all the characters in <paramref name="text"/>, but with all readings (pronunciations) of each.<br />
			/// Returns the following readings for characters if they exist: Kun'yomi and On'yomi.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A <see cref="LanguageAgnostic.ReadingsString{ReadingTypes}"/> with all readings for each character in <paramref name="text"/>.</returns>
			[Pure]
			public LanguageAgnostic.ReadingsString<ReadingTypes> ProcessWithReadings(string text)
				=> ProcessWithReadings(text, ReadingTypes.Kunyomi | ReadingTypes.Onyomi);
		}
	}
}
