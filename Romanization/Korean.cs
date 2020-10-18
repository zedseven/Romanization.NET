using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Text;

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming

namespace Romanization
{
	public static class Korean
	{
		// Unicode Constants (sourced from https://en.wikipedia.org/wiki/Korean_language_and_computers#Hangul_in_Unicode)
		private const int HangeulUnicodeBaseOffset = 44032;
		private const int HangeulUnicodeMedialSpace = 28;
		private const int HangeulUnicodeInitialSpace = 588; // 21 * 28

		private class SyllableBlock
		{
			public readonly char Character;
			public readonly PlacementChar Initial;
			public readonly PlacementChar Medial;
			public readonly PlacementChar? Final;

			public SyllableBlock(char character, char initial, char medial, char? final)
			{
				Character = character;
				Initial   = new PlacementChar(initial, PlacementChar.Placements.Initial);
				Medial    = new PlacementChar(medial,  PlacementChar.Placements.Medial);
				Final     = final.HasValue ? new PlacementChar(final.Value, PlacementChar.Placements.Final) : (PlacementChar?) null;
			}

			[Pure]
			public string Flatten()
				=> $"{Initial}{Medial}{Final}";

			[Pure]
			public PlacementChar[] FlattenToArray()
				=> Final.HasValue ? new [] { Initial, Medial, Final.Value } : new[] { Initial, Medial };
		}

		private readonly struct PlacementChar : IEquatable<PlacementChar>
		{
			public enum Placements
			{
				NotApplicable,
				Initial,
				Medial, // Technically irrelevant and useless, but I'm keeping it anyways for posterity
				Final
			}

			public readonly char BaseChar;
			public readonly Placements Placement;

			public PlacementChar(char baseChar, Placements placement)
			{
				BaseChar = baseChar;
				Placement = placement;
			}

			public static implicit operator char(PlacementChar c)
				=> c.BaseChar;

			public static explicit operator PlacementChar(char c)
				=> new PlacementChar(c, Placements.NotApplicable);

			public bool Equals(PlacementChar other)
				=> BaseChar == other.BaseChar && Placement == other.Placement;

			public override bool Equals(object obj)
				=> obj is PlacementChar other && Equals(other);

			public override int GetHashCode()
				=> HashCode.Combine(BaseChar, (int)Placement);

			public static bool operator ==(PlacementChar left, PlacementChar right)
				=> left.Equals(right);

			public static bool operator !=(PlacementChar left, PlacementChar right)
				=> !left.Equals(right);

			public override string ToString()
				=> Placement != Placements.NotApplicable ? $"'{BaseChar}' ({Placement})" : BaseChar.ToString();

			public string ToString(IFormatProvider provider)
				=> Placement != Placements.NotApplicable ? $"'{BaseChar.ToString(provider)}' ({Placement})" : BaseChar.ToString(provider);
		}

		private readonly struct AspirationString
		{
			public readonly string AspiratedString;
			public readonly string NonAspiratedString;

			public AspirationString(string aspirated, string nonAspirated)
			{
				AspiratedString = aspirated;
				NonAspiratedString = nonAspirated;
			}

			public AspirationString(string both)
			{
				AspiratedString = both;
				NonAspiratedString = both;
			}

			public static implicit operator AspirationString(string s)
				=> new AspirationString(s);

			public static implicit operator AspirationString(ValueTuple<string, string> s)
				=> new AspirationString(s.Item1, s.Item2);

			public static implicit operator AspirationString(Tuple<string, string> s)
				=> new AspirationString(s.Item1, s.Item2);
		}

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

		// System Singletons
		public static readonly Lazy<RevisedRomanizationSystem> RevisedRomanization = new Lazy<RevisedRomanizationSystem>(() => new RevisedRomanizationSystem());

		private static readonly char[]  HangeulUnicodeJamoInitialMap =
		{
			'ㄱ',
			'ㄲ',
			'ㄴ',
			'ㄷ',
			'ㄸ',
			'ㄹ',
			'ㅁ',
			'ㅂ',
			'ㅃ',
			'ㅅ',
			'ㅆ',
			'ㅇ',
			'ㅈ',
			'ㅉ',
			'ㅊ',
			'ㅋ',
			'ㅌ',
			'ㅍ',
			'ㅎ'
		};
		private static readonly char[]  HangeulUnicodeJamoMedialMap =
		{
			'ㅏ',
			'ㅐ',
			'ㅑ',
			'ㅒ',
			'ㅓ',
			'ㅔ',
			'ㅕ',
			'ㅖ',
			'ㅗ',
			'ㅘ',
			'ㅙ',
			'ㅚ',
			'ㅛ',
			'ㅜ',
			'ㅝ',
			'ㅞ',
			'ㅟ',
			'ㅠ',
			'ㅡ',
			'ㅢ',
			'ㅣ'
		};
		private static readonly char?[] HangeulUnicodeJamoFinalMap =
		{
			null,
			'ㄱ',
			'ㄲ',
			'ㄳ',
			'ㄴ',
			'ㄵ',
			'ㄶ',
			'ㄷ',
			'ㄹ',
			'ㄺ',
			'ㄻ',
			'ㄼ',
			'ㄽ',
			'ㄾ',
			'ㄿ',
			'ㅀ',
			'ㅁ',
			'ㅂ',
			'ㅄ',
			'ㅅ',
			'ㅆ',
			'ㅇ',
			'ㅈ',
			'ㅊ',
			'ㅋ',
			'ㅌ',
			'ㅍ',
			'ㅎ'
		};

		private static readonly Dictionary<char, SyllableBlock> syllableBlockIndex = new Dictionary<char, SyllableBlock>();

		// Helper Functions
		[Pure]
		private static bool IsKorean(char character)
			=> character >= 0xAC00 && character <= 0xD7A3;

		/// <summary>
		/// Decomposes a Korean syllable block into it's component jamo, in order.<br />
		/// ie. '한' -> 'ㅎ', 'ㅏ', 'ㄴ'<br />
		/// Sourced from a function derived from:
		/// <a href='https://en.wikipedia.org/wiki/Korean_language_and_computers#Hangul_in_Unicode'>https://en.wikipedia.org/wiki/Korean_language_and_computers#Hangul_in_Unicode</a>
		/// </summary>
		/// <param name="block">The syllable block / Unicode character ('한').</param>
		/// <returns>The syllable block decomposed into it's component jamo ('ㅎ', 'ㅏ', 'ㄴ'), or null if the input block is invalid.</returns>
		private static SyllableBlock DecomposeSyllableBlock(char block)
		{
			if (!IsKorean(block))
				return null;

			if (syllableBlockIndex.ContainsKey(block))
				return syllableBlockIndex[block];

			int codePoint = block - HangeulUnicodeBaseOffset;
			int finalIndex = codePoint % HangeulUnicodeMedialSpace;
			int medialIndex = (codePoint - finalIndex) % HangeulUnicodeInitialSpace / HangeulUnicodeMedialSpace;
			int initialIndex = (codePoint - finalIndex - medialIndex * HangeulUnicodeMedialSpace) / HangeulUnicodeInitialSpace;
			
			SyllableBlock newBlock = new SyllableBlock(block,
				HangeulUnicodeJamoInitialMap[initialIndex],
				HangeulUnicodeJamoMedialMap[medialIndex],
				HangeulUnicodeJamoFinalMap[finalIndex]);
			syllableBlockIndex[block] = newBlock;

			return newBlock;
		}

		/// <summary>
		/// The Revised Romanization of Korean system.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Revised_Romanization_of_Korean'>https://en.wikipedia.org/wiki/Revised_Romanization_of_Korean</a>
		/// </summary>
		public sealed class RevisedRomanizationSystem : IRomanizationSystem
		{
			// System-Specific Constants
			private static readonly Dictionary<char, string> HangeulVowelRomanizations = new Dictionary<char, string>();
			private static readonly Dictionary<char, string> HangeulConsonantInitialRomanizations = new Dictionary<char, string>();
			private static readonly Dictionary<char, string> HangeulConsonantFinalRomanizations = new Dictionary<char, string>();
			private static readonly Dictionary<(char, char), HyphenString> HangeulConsonantCombinationRomanizations = new Dictionary<(char, char), HyphenString>();

			internal RevisedRomanizationSystem()
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
			/// <param name="givenName">Whether or not the text to romanize is a given name, since Korean names are often romanized without consideration for special jamo combinations.</param>
			/// <param name="noun">Whether or not the text to romanize is a noun, since there is a distinction between whether or not aspiration is reflected based on nouns.</param>
			/// <param name="hyphenateSyllables">Whether to insert a hyphen ('-') between syllables in non-required spots. This can help to distinguish between ambiguous words: 가을 -> ga-eul (fall; autumn) vs. 개울 -> gae-ul (stream).</param>
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched. Note that all romanized text will be lowercase.</returns>
			[Pure]
			public string Process(string text, bool givenName, bool noun = false, bool hyphenateSyllables = false)
			{
				// Replace common alternate characters
				text = LanguageAgnostic.ReplaceCommonAlternates(text);

				// Insert spaces at boundaries between Latin characters and Korean ones
				text = LanguageAgnostic.SeparateLanguageBoundaries(text);

				// Decompose all syllable blocks in text into their component jamo
				List<PlacementChar> jamoList = text.SelectMany(c =>
					{
						SyllableBlock b = DecomposeSyllableBlock(c);
						return b != null ? b.FlattenToArray() : new[] { (PlacementChar) c };
					})
					.ToList();

				// Use the component jamo to build the romanization
				StringBuilder romanizedText = new StringBuilder();
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
							if (hyphenateSyllables && !lastChar && jamoList[i + 1].Placement == PlacementChar.Placements.Initial)
								romanizedText.Append('-');

							continue;
						case PlacementChar.Placements.Final:
							if (!givenName && !lastChar)
							{
								(char, char) key = (jamoList[i], jamoList[i + 1]);
								if (HangeulConsonantCombinationRomanizations.ContainsKey(key))
								{
									HyphenString specialCaseRomanization = HangeulConsonantCombinationRomanizations[key];
									// TODO: This may be backwards - (!noun may need to be inverted) - this is because documentation for this is heavily unclear on whether aspiration should be reflected in nouns
									// More info: "... However, aspirated sounds are *not* reflected in case of nouns where ㅎ follows ㄱ, ㄷ, and ㅂ: 묵호 → Mukho, 집현전 → Jiphyeonjeon." (emphasis mine)
									// The text says aspiration should not be reflected in such nouns, yet both examples it gives are nouns that reflect aspiration.
									// Furthermore, the previous examples all exclude aspiration and whether or not the words are nouns is unclear - this leads me to believe the text has it backwards.
									// As someone with a very rudimentary understanding of Korean, I can't determine one way or the other for certain, so for now this is how it will stay.
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

							// Three-jamo syllable hyphenation
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
			/// <returns>A romanized version of the text, leaving unrecognized characters untouched. Note that all romanized text will be lowercase.</returns>
			[Pure]
			public string Process(string text)
				=> Process(text, false, false, false);
		}
	}
}
