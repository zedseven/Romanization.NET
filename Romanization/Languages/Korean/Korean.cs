using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming

namespace Romanization
{
	public static partial class Korean
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

			if (syllableBlockIndex.TryGetValue(block, out SyllableBlock cachedBlock))
				return cachedBlock;

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
	}
}
