using Xunit;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.JapaneseTests
{
	/// <summary>
	/// For testing the Japanese Modified Hepburn romanization system, <see cref="Japanese.ModifiedHepburn"/>.
	/// </summary>
	public class ModifiedHepburnTests : TestClass
	{
		private readonly Japanese.ModifiedHepburn _system = new();

		/// <summary>
		/// Aims to test Katakana long vowel (chōonpu, ー) conversion into macron versions of respective vowels.
		/// </summary>
		[Fact]
		public void KatakanaLongVowelTest()
		{
			Assert.Equal("nintendō",             _system.Process("ニンテンドー"));
			Assert.Equal("burauzā",              _system.Process("ブラウザー"));
			Assert.Equal("nintendō DSi burauzā", _system.Process("ニンテンドーDSiブラウザー"));
		}

		/// <summary>
		/// Aims to test Hiragana syllabic n (ん) conversion into <c>n</c>, or <c>n'</c> if before a vowel or y.
		/// </summary>
		[Fact]
		public void HiraganaSyllabicNTest()
		{
			Assert.Equal("annai",    _system.Process("あんない"));
			Assert.Equal("gunma",    _system.Process("ぐんま"));
			Assert.Equal("kan'i",    _system.Process("かんい"));
			Assert.Equal("shin'you", _system.Process("しんよう"));
		}

		/// <summary>
		/// Aims to test Hiragana long consonant (sokuon, っ) conversion into doubled-up Rômaji consonants.
		/// </summary>
		[Fact]
		public void HiraganaLongConsonantTest()
		{
			Assert.Equal("kekka",   _system.Process("けっか"));
			Assert.Equal("sassato", _system.Process("さっさと"));
			Assert.Equal("zutto",   _system.Process("ずっと"));
			Assert.Equal("kippu",   _system.Process("きっぷ"));
			Assert.Equal("zasshi",  _system.Process("ざっし"));
			Assert.Equal("issho",   _system.Process("いっしょ"));
			Assert.Equal("kotchi",  _system.Process("こっち"));
			Assert.Equal("matcha",  _system.Process("まっちゃ"));
			Assert.Equal("mittsu",  _system.Process("みっつ"));
		}
	}
}
