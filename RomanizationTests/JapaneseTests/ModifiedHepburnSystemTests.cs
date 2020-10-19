﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.JapaneseTests
{
	[TestClass]
	public class ModifiedHepburnSystemTests
	{
		[TestMethod]
		public void LongVowelTest()
		{
			Assert.AreEqual("nintendō",             Japanese.ModifiedHepburn.Value.Process("ニンテンドー"));
			Assert.AreEqual("burauzā",              Japanese.ModifiedHepburn.Value.Process("ブラウザー"));
			Assert.AreEqual("nintendō DSi burauzā", Japanese.ModifiedHepburn.Value.Process("ニンテンドーDSiブラウザー"));
		}

		[TestMethod]
		public void SyllabicNTest()
		{
			Assert.AreEqual("annai",                Japanese.ModifiedHepburn.Value.Process("あんない"));
			Assert.AreEqual("gunma",                Japanese.ModifiedHepburn.Value.Process("ぐんま"));
			Assert.AreEqual("kan'i",                Japanese.ModifiedHepburn.Value.Process("かんい"));
			Assert.AreEqual("shin'you",             Japanese.ModifiedHepburn.Value.Process("しんよう"));
		}

		[TestMethod]
		public void LongConsonantTest()
		{
			Assert.AreEqual("kekka",                Japanese.ModifiedHepburn.Value.Process("けっか"));
			Assert.AreEqual("sassato",              Japanese.ModifiedHepburn.Value.Process("さっさと"));
			Assert.AreEqual("zutto",                Japanese.ModifiedHepburn.Value.Process("ずっと"));
			Assert.AreEqual("kippu",                Japanese.ModifiedHepburn.Value.Process("きっぷ"));
			Assert.AreEqual("zasshi",               Japanese.ModifiedHepburn.Value.Process("ざっし"));
			Assert.AreEqual("issho",                Japanese.ModifiedHepburn.Value.Process("いっしょ"));
			Assert.AreEqual("kotchi",               Japanese.ModifiedHepburn.Value.Process("こっち"));
			Assert.AreEqual("matcha",               Japanese.ModifiedHepburn.Value.Process("まっちゃ"));
			Assert.AreEqual("mittsu",               Japanese.ModifiedHepburn.Value.Process("みっつ"));
		}
	}
}