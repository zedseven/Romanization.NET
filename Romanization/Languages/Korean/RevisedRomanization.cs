using Romanization.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming

namespace Romanization
{
	public static partial class Korean
	{
		/// <summary>
		/// The Revised Romanization of Korean system.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Revised_Romanization_of_Korean'>https://en.wikipedia.org/wiki/Revised_Romanization_of_Korean</a>
		/// </summary>
		public sealed class RevisedRomanization : IRomanizationSystem
		{
			/// <inheritdoc />
			public SystemType Type => SystemType.PhonemicTranscription;

			/// <summary>
			/// Whether to insert a hyphen ('-') between syllables in non-required
			/// spots. This can help to distinguish between ambiguous words: <c>가을 -> ga-eul</c> (fall; autumn) vs.
			/// <c>개울</c> -> gae-ul (stream).
			/// </summary>
			public readonly bool HyphenateSyllables;

			private readonly struct HyphenString
			{
				public readonly AspirationString BaseString;
				public readonly int HyphenIndex;
				private readonly bool InsertHyphen;

				public HyphenString(AspirationString baseString, int hyphenIndex = -1)
				{
					BaseString   = baseString;
					HyphenIndex  = hyphenIndex;
					InsertHyphen = hyphenIndex <= -1 && !baseString.AspiratedString.Contains('-');
				}

				public static implicit operator HyphenString(string s)
					=> new(s);

				public static implicit operator HyphenString(AspirationString s)
					=> new(s);

				public static implicit operator HyphenString(ValueTuple<AspirationString, int> s)
					=> new(s.Item1, s.Item2);

				public static implicit operator HyphenString(Tuple<AspirationString, int> s)
					=> new(s.Item1, s.Item2);

				public string ToString(bool aspirated)
					=> aspirated
						? HyphenIndex > -1
							? $"{BaseString.AspiratedString.Substring(0, HyphenIndex)}-{BaseString.AspiratedString.Substring(HyphenIndex)}"
							: $"{BaseString.AspiratedString}{(InsertHyphen ? "-" : "")}"
						: HyphenIndex > -1
							? $"{BaseString.NonAspiratedString.Substring(0, HyphenIndex)}-{BaseString.NonAspiratedString.Substring(HyphenIndex)}"
							: $"{BaseString.NonAspiratedString}{(InsertHyphen ? "-" : "")}";

				public override string ToString()
					=> ToString(true);
			}

			// System-Specific Constants
			private readonly Dictionary<char, string> HangeulVowelRomanizations                              = new();
			private readonly Dictionary<char, string> HangeulConsonantInitialRomanizations                   = new();
			private readonly Dictionary<char, string> HangeulConsonantFinalRomanizations                     = new();
			private readonly Dictionary<(char, char), HyphenString> HangeulConsonantCombinationRomanizations = new();

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.
			/// </summary>
			public RevisedRomanization()
			{
				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/Revised_Romanization_of_Korean#Transcription_rules

				// Vowels
				HangeulVowelRomanizations.Add('ㅏ',   "a");
				HangeulVowelRomanizations.Add('ㅐ',  "ae");
				HangeulVowelRomanizations.Add('ㅑ',  "ya");
				HangeulVowelRomanizations.Add('ㅒ', "yae");
				HangeulVowelRomanizations.Add('ㅓ',  "eo");
				HangeulVowelRomanizations.Add('ㅔ',   "e");
				HangeulVowelRomanizations.Add('ㅕ', "yeo");
				HangeulVowelRomanizations.Add('ㅖ',  "ye");
				HangeulVowelRomanizations.Add('ㅗ',   "o");
				HangeulVowelRomanizations.Add('ㅘ',  "wa");
				HangeulVowelRomanizations.Add('ㅙ', "wae");
				HangeulVowelRomanizations.Add('ㅚ',  "oe");
				HangeulVowelRomanizations.Add('ㅛ',  "yo");
				HangeulVowelRomanizations.Add('ㅜ',   "u");
				HangeulVowelRomanizations.Add('ㅝ',  "wo");
				HangeulVowelRomanizations.Add('ㅞ',  "we");
				HangeulVowelRomanizations.Add('ㅟ',  "wi");
				HangeulVowelRomanizations.Add('ㅠ',  "yu");
				HangeulVowelRomanizations.Add('ㅡ',  "eu");
				HangeulVowelRomanizations.Add('ㅢ',  "ui");
				HangeulVowelRomanizations.Add('ㅣ',   "i");

				// Consonants in initial positions
				HangeulConsonantInitialRomanizations.Add('ㄱ',  "g");
				HangeulConsonantInitialRomanizations.Add('ㄲ', "kk");
				HangeulConsonantInitialRomanizations.Add('ㄴ',  "n");
				HangeulConsonantInitialRomanizations.Add('ㄷ',  "d");
				HangeulConsonantInitialRomanizations.Add('ㄸ', "tt");
				HangeulConsonantInitialRomanizations.Add('ㄹ',  "r");
				HangeulConsonantInitialRomanizations.Add('ㅁ',  "m");
				HangeulConsonantInitialRomanizations.Add('ㅂ',  "b");
				HangeulConsonantInitialRomanizations.Add('ㅃ', "pp");
				HangeulConsonantInitialRomanizations.Add('ㅅ',  "s");
				HangeulConsonantInitialRomanizations.Add('ㅆ', "ss");
				HangeulConsonantInitialRomanizations.Add('ㅇ',   "");
				HangeulConsonantInitialRomanizations.Add('ㅈ',  "j");
				HangeulConsonantInitialRomanizations.Add('ㅉ', "jj");
				HangeulConsonantInitialRomanizations.Add('ㅊ', "ch");
				HangeulConsonantInitialRomanizations.Add('ㅋ',  "k");
				HangeulConsonantInitialRomanizations.Add('ㅌ',  "t");
				HangeulConsonantInitialRomanizations.Add('ㅍ',  "p");
				HangeulConsonantInitialRomanizations.Add('ㅎ',  "h");

				// Consonants in final positions
				HangeulConsonantFinalRomanizations.Add('ㄱ',  "k");
				HangeulConsonantFinalRomanizations.Add('ㄲ',  "k");
				HangeulConsonantFinalRomanizations.Add('ㄴ',  "n");
				HangeulConsonantFinalRomanizations.Add('ㄷ',  "t");
				HangeulConsonantFinalRomanizations.Add('ㄹ',  "l");
				HangeulConsonantFinalRomanizations.Add('ㅁ',  "m");
				HangeulConsonantFinalRomanizations.Add('ㅂ',  "p");
				HangeulConsonantFinalRomanizations.Add('ㅅ',  "t");
				HangeulConsonantFinalRomanizations.Add('ㅆ',  "t");
				HangeulConsonantFinalRomanizations.Add('ㅇ', "ng");
				HangeulConsonantFinalRomanizations.Add('ㅈ',  "t");
				HangeulConsonantFinalRomanizations.Add('ㅊ',  "t");
				HangeulConsonantFinalRomanizations.Add('ㅋ',  "k");
				HangeulConsonantFinalRomanizations.Add('ㅌ',  "t");
				HangeulConsonantFinalRomanizations.Add('ㅍ',  "p");
				HangeulConsonantFinalRomanizations.Add('ㅎ',  "t");

				// Special cases of combinations of an ending from one block and the beginning of a new one
				HangeulConsonantCombinationRomanizations.Add(('ㄱ', 'ㅇ'), "g");
				HangeulConsonantCombinationRomanizations.Add(('ㄱ', 'ㄴ'), ("ngn", 2));
				HangeulConsonantCombinationRomanizations.Add(('ㄱ', 'ㄹ'), ("ngn", 2));
				HangeulConsonantCombinationRomanizations.Add(('ㄱ', 'ㅁ'), ("ngm", 2));
				HangeulConsonantCombinationRomanizations.Add(('ㄱ', 'ㄱ'), "k-k");
				HangeulConsonantCombinationRomanizations.Add(('ㄱ', 'ㅎ'), (("kh", "k"), -1)); // kh,k
				HangeulConsonantCombinationRomanizations.Add(('ㄴ', 'ㄱ'), "n-g");
				HangeulConsonantCombinationRomanizations.Add(('ㄴ', 'ㄹ'), ("ll", 1)); // ll,nn
				HangeulConsonantCombinationRomanizations.Add(('ㄷ', 'ㅇ'), "d"); // d,j
				HangeulConsonantCombinationRomanizations.Add(('ㄷ', 'ㄴ'), ("nn", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㄷ', 'ㄹ'), ("nn", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㄷ', 'ㅁ'), ("nm", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㄷ', 'ㅌ'), "t-t");
				HangeulConsonantCombinationRomanizations.Add(('ㄷ', 'ㅎ'), (("th", "t"), -1)); // th,t,ch
				HangeulConsonantCombinationRomanizations.Add(('ㄹ', 'ㅇ'), "r");
				HangeulConsonantCombinationRomanizations.Add(('ㄹ', 'ㄴ'), ("ll", 1)); // ll,nn
				HangeulConsonantCombinationRomanizations.Add(('ㄹ', 'ㄹ'), ("ll", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅁ', 'ㄹ'), ("mn", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅂ', 'ㅇ'), "b");
				HangeulConsonantCombinationRomanizations.Add(('ㅂ', 'ㄴ'), ("mn", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅂ', 'ㄹ'), ("mn", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅂ', 'ㅁ'), ("mm", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅂ', 'ㅍ'), "p-p");
				HangeulConsonantCombinationRomanizations.Add(('ㅂ', 'ㅎ'), (("ph", "p"), -1)); // ph,p
				HangeulConsonantCombinationRomanizations.Add(('ㅅ', 'ㅇ'), "s");
				HangeulConsonantCombinationRomanizations.Add(('ㅅ', 'ㄴ'), ("nn", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅅ', 'ㄹ'), ("nn", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅅ', 'ㅁ'), ("nm", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅅ', 'ㅌ'), "t-t");
				HangeulConsonantCombinationRomanizations.Add(('ㅇ', 'ㅇ'), "ng-");
				HangeulConsonantCombinationRomanizations.Add(('ㅇ', 'ㄹ'), ("ngn", 2));
				HangeulConsonantCombinationRomanizations.Add(('ㅈ', 'ㅇ'), "j");
				HangeulConsonantCombinationRomanizations.Add(('ㅈ', 'ㄴ'), ("nn", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅈ', 'ㄹ'), ("nn", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅈ', 'ㅁ'), ("nm", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅈ', 'ㅌ'), "t-t");
				HangeulConsonantCombinationRomanizations.Add(('ㅈ', 'ㅎ'), (("th", "t"), -1)); // th,t,ch
				HangeulConsonantCombinationRomanizations.Add(('ㅊ', 'ㅇ'), "ch");
				HangeulConsonantCombinationRomanizations.Add(('ㅊ', 'ㄴ'), ("nn", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅊ', 'ㄹ'), ("nn", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅊ', 'ㅁ'), ("nm", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅊ', 'ㅌ'), "t-t");
				HangeulConsonantCombinationRomanizations.Add(('ㅊ', 'ㅎ'), (("th", "t"), -1)); // th,t,ch
				HangeulConsonantCombinationRomanizations.Add(('ㅌ', 'ㄴ'), ("nn", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅌ', 'ㄹ'), ("nn", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅌ', 'ㅁ'), ("nm", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅌ', 'ㅌ'), "t-t");
				HangeulConsonantCombinationRomanizations.Add(('ㅌ', 'ㅎ'), (("th", "t"), -1)); // th,t,ch
				HangeulConsonantCombinationRomanizations.Add(('ㅎ', 'ㅇ'), "h");
				HangeulConsonantCombinationRomanizations.Add(('ㅎ', 'ㄱ'), "k");
				HangeulConsonantCombinationRomanizations.Add(('ㅎ', 'ㄷ'), "t");
				HangeulConsonantCombinationRomanizations.Add(('ㅎ', 'ㄴ'), ("nn", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅎ', 'ㄹ'), ("nn", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅎ', 'ㅁ'), ("nm", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅎ', 'ㅂ'), "p");
				HangeulConsonantCombinationRomanizations.Add(('ㅎ', 'ㅅ'), ("hs", 1));
				HangeulConsonantCombinationRomanizations.Add(('ㅎ', 'ㅈ'), "ch");
				HangeulConsonantCombinationRomanizations.Add(('ㅎ', 'ㅎ'), "t");

				#endregion
			}

			/// <summary>
			/// Performs romanization on the given text, according to the Revised Romanization of Korean system.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <param name="givenName">Whether or not the text to romanize is a given name, since Korean names are
			/// often romanized without consideration for special Jamo combinations.</param>
			/// <param name="noun">Whether or not the text to romanize is a noun, since there is a distinction between
			/// whether or not aspiration is reflected based on nouns.</param>
			/// <param name="hyphenateSyllables">Whether to insert a hyphen ('-') between syllables in non-required
			/// spots. This can help to distinguish between ambiguous words: <c>가을 -> ga-eul</c> (fall; autumn) vs.
			/// <c>개울</c> -> gae-ul (stream).</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched. Note that all
			/// romanized text will be lowercase.</returns>
			[Pure]
			public string Process(string text, bool givenName, bool noun = false, bool hyphenateSyllables = false)
			{
				text = text
					// Replace common alternate characters
					.ReplaceCommonAlternates()
					// Insert spaces at boundaries between Latin characters and Korean ones
					.SeparateLanguageBoundaries();

				// Decompose all syllable blocks in text into their component Jamo
				List<PlacementChar> jamoList = text.SelectMany(c =>
					{
						SyllableBlock? b = DecomposeSyllableBlock(c);
						return b?.FlattenToArray() ?? new[] {(PlacementChar) c};
					})
					.ToList();

				// Use the component Jamo to build the romanization
				StringBuilder romanizedText = new();
				for (int i = 0; i < jamoList.Count; i++)
				{
					bool lastChar = i >= jamoList.Count - 1;
					switch (jamoList[i].Placement)
					{
						case PlacementChar.Placements.NotApplicable:
							romanizedText.Append(jamoList[i]);
							continue;
						case PlacementChar.Placements.Initial:
							romanizedText.Append(HangeulConsonantInitialRomanizations[jamoList[i]]);
							continue;
						case PlacementChar.Placements.Medial:
							romanizedText.Append(HangeulVowelRomanizations[jamoList[i]]);

							// Two-jamo syllable hyphenation
							if (hyphenateSyllables && !lastChar &&
							    jamoList[i + 1].Placement == PlacementChar.Placements.Initial)
								romanizedText.Append('-');

							continue;
						case PlacementChar.Placements.Final:
							if (!givenName && !lastChar)
							{
								(char, char) key = (jamoList[i], jamoList[i + 1]);
								if (HangeulConsonantCombinationRomanizations.TryGetValue(key, out HyphenString specialCaseRomanization))
								{
									// TODO: This may be backwards - (!noun may need to be inverted) - this is because documentation for this is heavily unclear on whether aspiration should be reflected in nouns
									// More info: "... However, aspirated sounds are *not* reflected in case of nouns
									// where ㅎ follows ㄱ, ㄷ, and ㅂ: 묵호 → Mukho, 집현전 → Jiphyeonjeon." (emphasis mine)
									// The text says aspiration should not be reflected in such nouns, yet both examples
									// it gives are nouns that reflect aspiration.
									// Furthermore, the previous examples all exclude aspiration and whether or not the
									// words are nouns is unclear - this leads me to believe the text has it backwards.
									// As someone with a very rudimentary understanding of Korean, I can't determine one
									// way or the other for certain, so for now this is how it will stay.
									if (!noun && jamoList[i + 1] == 'ㅎ' &&
										(jamoList[i] == 'ㄱ' || jamoList[i] == 'ㄷ' || jamoList[i] == 'ㅂ'))
										romanizedText.Append(hyphenateSyllables
											? specialCaseRomanization.ToString(false)
											: specialCaseRomanization.BaseString.NonAspiratedString);
									else
										romanizedText.Append(hyphenateSyllables
											? specialCaseRomanization.ToString(true)
											: specialCaseRomanization.BaseString.AspiratedString);
									i++;
									continue;
								}
							}

							romanizedText.Append(HangeulConsonantFinalRomanizations[jamoList[i]]);

							// Three-Jamo syllable hyphenation
							if (hyphenateSyllables && !lastChar && jamoList[i + 1].Placement == PlacementChar.Placements.Initial)
								romanizedText.Append('-');

							continue;
					}
				}

				return romanizedText.ToString();
			}

			/// <summary>
			/// Performs romanization on the given text, according to the Revised Romanization of Korean system.
			/// </summary>
			/// <param name="text">The text to romanize.</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched. Note that all
			/// romanized text will be lowercase.</returns>
			[Pure]
			public string Process(string text)
				=> Process(text, false, false, false);
		}
	}
}
