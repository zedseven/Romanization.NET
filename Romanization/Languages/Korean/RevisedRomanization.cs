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
					BaseString = baseString;
					HyphenIndex = hyphenIndex;
					InsertHyphen = hyphenIndex <= -1 && !baseString.AspiratedString.Contains('-');
				}

				public static implicit operator HyphenString(string s)
					=> new HyphenString(s);

				public static implicit operator HyphenString(AspirationString s)
					=> new HyphenString(s);

				public static implicit operator HyphenString(ValueTuple<AspirationString, int> s)
					=> new HyphenString(s.Item1, s.Item2);

				public static implicit operator HyphenString(Tuple<AspirationString, int> s)
					=> new HyphenString(s.Item1, s.Item2);

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
			private readonly Dictionary<char, string> HangeulVowelRomanizations = new Dictionary<char, string>();
			private readonly Dictionary<char, string> HangeulConsonantInitialRomanizations = new Dictionary<char, string>();
			private readonly Dictionary<char, string> HangeulConsonantFinalRomanizations = new Dictionary<char, string>();
			private readonly Dictionary<(char, char), HyphenString> HangeulConsonantCombinationRomanizations = new Dictionary<(char, char), HyphenString>();

			/// <summary>
			/// Instantiates a copy of the system to process romanizations.
			/// </summary>
			public RevisedRomanization()
			{
				#region Romanization Chart

				// Sourced from https://en.wikipedia.org/wiki/Revised_Romanization_of_Korean#Transcription_rules

				// Vowels
				HangeulVowelRomanizations['ㅏ'] = "a";
				HangeulVowelRomanizations['ㅐ'] = "ae";
				HangeulVowelRomanizations['ㅑ'] = "ya";
				HangeulVowelRomanizations['ㅒ'] = "yae";
				HangeulVowelRomanizations['ㅓ'] = "eo";
				HangeulVowelRomanizations['ㅔ'] = "e";
				HangeulVowelRomanizations['ㅕ'] = "yeo";
				HangeulVowelRomanizations['ㅖ'] = "ye";
				HangeulVowelRomanizations['ㅗ'] = "o";
				HangeulVowelRomanizations['ㅘ'] = "wa";
				HangeulVowelRomanizations['ㅙ'] = "wae";
				HangeulVowelRomanizations['ㅚ'] = "oe";
				HangeulVowelRomanizations['ㅛ'] = "yo";
				HangeulVowelRomanizations['ㅜ'] = "u";
				HangeulVowelRomanizations['ㅝ'] = "wo";
				HangeulVowelRomanizations['ㅞ'] = "we";
				HangeulVowelRomanizations['ㅟ'] = "wi";
				HangeulVowelRomanizations['ㅠ'] = "yu";
				HangeulVowelRomanizations['ㅡ'] = "eu";
				HangeulVowelRomanizations['ㅢ'] = "ui";
				HangeulVowelRomanizations['ㅣ'] = "i";

				// Consonants in initial positions
				HangeulConsonantInitialRomanizations['ㄱ'] = "g";
				HangeulConsonantInitialRomanizations['ㄲ'] = "kk";
				HangeulConsonantInitialRomanizations['ㄴ'] = "n";
				HangeulConsonantInitialRomanizations['ㄷ'] = "d";
				HangeulConsonantInitialRomanizations['ㄸ'] = "tt";
				HangeulConsonantInitialRomanizations['ㄹ'] = "r";
				HangeulConsonantInitialRomanizations['ㅁ'] = "m";
				HangeulConsonantInitialRomanizations['ㅂ'] = "b";
				HangeulConsonantInitialRomanizations['ㅃ'] = "pp";
				HangeulConsonantInitialRomanizations['ㅅ'] = "s";
				HangeulConsonantInitialRomanizations['ㅆ'] = "ss";
				HangeulConsonantInitialRomanizations['ㅇ'] = "";
				HangeulConsonantInitialRomanizations['ㅈ'] = "j";
				HangeulConsonantInitialRomanizations['ㅉ'] = "jj";
				HangeulConsonantInitialRomanizations['ㅊ'] = "ch";
				HangeulConsonantInitialRomanizations['ㅋ'] = "k";
				HangeulConsonantInitialRomanizations['ㅌ'] = "t";
				HangeulConsonantInitialRomanizations['ㅍ'] = "p";
				HangeulConsonantInitialRomanizations['ㅎ'] = "h";

				// Consonants in final positions
				HangeulConsonantFinalRomanizations['ㄱ'] = "k";
				HangeulConsonantFinalRomanizations['ㄲ'] = "k";
				HangeulConsonantFinalRomanizations['ㄴ'] = "n";
				HangeulConsonantFinalRomanizations['ㄷ'] = "t";
				HangeulConsonantFinalRomanizations['ㄹ'] = "l";
				HangeulConsonantFinalRomanizations['ㅁ'] = "m";
				HangeulConsonantFinalRomanizations['ㅂ'] = "p";
				HangeulConsonantFinalRomanizations['ㅅ'] = "t";
				HangeulConsonantFinalRomanizations['ㅆ'] = "t";
				HangeulConsonantFinalRomanizations['ㅇ'] = "ng";
				HangeulConsonantFinalRomanizations['ㅈ'] = "t";
				HangeulConsonantFinalRomanizations['ㅊ'] = "t";
				HangeulConsonantFinalRomanizations['ㅋ'] = "k";
				HangeulConsonantFinalRomanizations['ㅌ'] = "t";
				HangeulConsonantFinalRomanizations['ㅍ'] = "p";
				HangeulConsonantFinalRomanizations['ㅎ'] = "t";

				// Special cases of combinations of an ending from one block and the beginning of a new one
				HangeulConsonantCombinationRomanizations[('ㄱ', 'ㅇ')] = "g";
				HangeulConsonantCombinationRomanizations[('ㄱ', 'ㄴ')] = ("ngn", 2);
				HangeulConsonantCombinationRomanizations[('ㄱ', 'ㄹ')] = ("ngn", 2);
				HangeulConsonantCombinationRomanizations[('ㄱ', 'ㅁ')] = ("ngm", 2);
				HangeulConsonantCombinationRomanizations[('ㄱ', 'ㄱ')] = "k-k";
				HangeulConsonantCombinationRomanizations[('ㄱ', 'ㅎ')] = (("kh", "k"), -1); // kh,k
				HangeulConsonantCombinationRomanizations[('ㄴ', 'ㄱ')] = "n-g";
				HangeulConsonantCombinationRomanizations[('ㄴ', 'ㄹ')] = ("ll", 1); // ll,nn
				HangeulConsonantCombinationRomanizations[('ㄷ', 'ㅇ')] = "d"; // d,j
				HangeulConsonantCombinationRomanizations[('ㄷ', 'ㄴ')] = ("nn", 1);
				HangeulConsonantCombinationRomanizations[('ㄷ', 'ㄹ')] = ("nn", 1);
				HangeulConsonantCombinationRomanizations[('ㄷ', 'ㅁ')] = ("nm", 1);
				HangeulConsonantCombinationRomanizations[('ㄷ', 'ㅌ')] = "t-t";
				HangeulConsonantCombinationRomanizations[('ㄷ', 'ㅎ')] = (("th", "t"), -1); // th,t,ch
				HangeulConsonantCombinationRomanizations[('ㄹ', 'ㅇ')] = "r";
				HangeulConsonantCombinationRomanizations[('ㄹ', 'ㄴ')] = ("ll", 1); // ll,nn
				HangeulConsonantCombinationRomanizations[('ㄹ', 'ㄹ')] = ("ll", 1);
				HangeulConsonantCombinationRomanizations[('ㅁ', 'ㄹ')] = ("mn", 1);
				HangeulConsonantCombinationRomanizations[('ㅂ', 'ㅇ')] = "b";
				HangeulConsonantCombinationRomanizations[('ㅂ', 'ㄴ')] = ("mn", 1);
				HangeulConsonantCombinationRomanizations[('ㅂ', 'ㄹ')] = ("mn", 1);
				HangeulConsonantCombinationRomanizations[('ㅂ', 'ㅁ')] = ("mm", 1);
				HangeulConsonantCombinationRomanizations[('ㅂ', 'ㅍ')] = "p-p";
				HangeulConsonantCombinationRomanizations[('ㅂ', 'ㅎ')] = (("ph", "p"), -1); // ph,p
				HangeulConsonantCombinationRomanizations[('ㅅ', 'ㅇ')] = "s";
				HangeulConsonantCombinationRomanizations[('ㅅ', 'ㄴ')] = ("nn", 1);
				HangeulConsonantCombinationRomanizations[('ㅅ', 'ㄹ')] = ("nn", 1);
				HangeulConsonantCombinationRomanizations[('ㅅ', 'ㅁ')] = ("nm", 1);
				HangeulConsonantCombinationRomanizations[('ㅅ', 'ㅌ')] = "t-t";
				HangeulConsonantCombinationRomanizations[('ㅇ', 'ㅇ')] = "ng-";
				HangeulConsonantCombinationRomanizations[('ㅇ', 'ㄹ')] = ("ngn", 2);
				HangeulConsonantCombinationRomanizations[('ㅈ', 'ㅇ')] = "j";
				HangeulConsonantCombinationRomanizations[('ㅈ', 'ㄴ')] = ("nn", 1);
				HangeulConsonantCombinationRomanizations[('ㅈ', 'ㄹ')] = ("nn", 1);
				HangeulConsonantCombinationRomanizations[('ㅈ', 'ㅁ')] = ("nm", 1);
				HangeulConsonantCombinationRomanizations[('ㅈ', 'ㅌ')] = "t-t";
				HangeulConsonantCombinationRomanizations[('ㅈ', 'ㅎ')] = (("th", "t"), -1); // th,t,ch
				HangeulConsonantCombinationRomanizations[('ㅊ', 'ㅇ')] = "ch";
				HangeulConsonantCombinationRomanizations[('ㅊ', 'ㄴ')] = ("nn", 1);
				HangeulConsonantCombinationRomanizations[('ㅊ', 'ㄹ')] = ("nn", 1);
				HangeulConsonantCombinationRomanizations[('ㅊ', 'ㅁ')] = ("nm", 1);
				HangeulConsonantCombinationRomanizations[('ㅊ', 'ㅌ')] = "t-t";
				HangeulConsonantCombinationRomanizations[('ㅊ', 'ㅎ')] = (("th", "t"), -1); // th,t,ch
				HangeulConsonantCombinationRomanizations[('ㅌ', 'ㄴ')] = ("nn", 1);
				HangeulConsonantCombinationRomanizations[('ㅌ', 'ㄹ')] = ("nn", 1);
				HangeulConsonantCombinationRomanizations[('ㅌ', 'ㅁ')] = ("nm", 1);
				HangeulConsonantCombinationRomanizations[('ㅌ', 'ㅌ')] = "t-t";
				HangeulConsonantCombinationRomanizations[('ㅌ', 'ㅎ')] = (("th", "t"), -1); // th,t,ch
				HangeulConsonantCombinationRomanizations[('ㅎ', 'ㅇ')] = "h";
				HangeulConsonantCombinationRomanizations[('ㅎ', 'ㄱ')] = "k";
				HangeulConsonantCombinationRomanizations[('ㅎ', 'ㄷ')] = "t";
				HangeulConsonantCombinationRomanizations[('ㅎ', 'ㄴ')] = ("nn", 1);
				HangeulConsonantCombinationRomanizations[('ㅎ', 'ㄹ')] = ("nn", 1);
				HangeulConsonantCombinationRomanizations[('ㅎ', 'ㅁ')] = ("nm", 1);
				HangeulConsonantCombinationRomanizations[('ㅎ', 'ㅂ')] = "p";
				HangeulConsonantCombinationRomanizations[('ㅎ', 'ㅅ')] = ("hs", 1);
				HangeulConsonantCombinationRomanizations[('ㅎ', 'ㅈ')] = "ch";
				HangeulConsonantCombinationRomanizations[('ㅎ', 'ㅎ')] = "t";

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
