using Microsoft.VisualStudio.TestTools.UnitTesting;
using Romanization;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace RomanizationTests.JapaneseTests
{
	/// <summary>
	/// For testing the Japanese Modified Hepburn romanization system, <see cref="Japanese.ModifiedHepburn"/>.
	/// </summary>
	[TestClass]
	public class ModifiedHepburnTests
	{
		private readonly Japanese.ModifiedHepburn _system = new Japanese.ModifiedHepburn();

		/// <summary>
		/// Aims to test Katakana long vowel (chōonpu, ー) conversion into macron versions of respective vowels.
		/// </summary>
		[TestMethod]
		public void KatakanaLongVowelTest()
		{
			Assert.AreEqual("nintendō",             _system.Process("ニンテンドー"));
			Assert.AreEqual("burauzā",              _system.Process("ブラウザー"));
			Assert.AreEqual("nintendō DSi burauzā", _system.Process("ニンテンドーDSiブラウザー"));
		}

		/// <summary>
		/// Aims to test Hiragana syllabic n (ん) conversion into <c>n</c>, or <c>n'</c> if before a vowel or y.
		/// </summary>
		[TestMethod]
		public void HiraganaSyllabicNTest()
		{
			Assert.AreEqual("annai",    _system.Process("あんない"));
			Assert.AreEqual("gunma",    _system.Process("ぐんま"));
			Assert.AreEqual("kan'i",    _system.Process("かんい"));
			Assert.AreEqual("shin'you", _system.Process("しんよう"));
		}

		/// <summary>
		/// Aims to test Hiragana long consonant (sokuon, っ) conversion into doubled-up Rômaji consonants.
		/// </summary>
		[TestMethod]
		public void HiraganaLongConsonantTest()
		{
			Assert.AreEqual("kekka",   _system.Process("けっか"));
			Assert.AreEqual("sassato", _system.Process("さっさと"));
			Assert.AreEqual("zutto",   _system.Process("ずっと"));
			Assert.AreEqual("kippu",   _system.Process("きっぷ"));
			Assert.AreEqual("zasshi",  _system.Process("ざっし"));
			Assert.AreEqual("issho",   _system.Process("いっしょ"));
			Assert.AreEqual("kotchi",  _system.Process("こっち"));
			Assert.AreEqual("matcha",  _system.Process("まっちゃ"));
			Assert.AreEqual("mittsu",  _system.Process("みっつ"));
		}
	}
}
