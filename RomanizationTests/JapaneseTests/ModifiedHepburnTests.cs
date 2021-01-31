using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.JapaneseTests
{
	[TestClass]
	public class ModifiedHepburnTests
	{
		private readonly Japanese.ModifiedHepburn _system = new Japanese.ModifiedHepburn();

		[TestMethod]
		public void LongVowelTest()
		{
			Assert.AreEqual("nintendō",             _system.Process("ニンテンドー"));
			Assert.AreEqual("burauzā",              _system.Process("ブラウザー"));
			Assert.AreEqual("nintendō DSi burauzā", _system.Process("ニンテンドーDSiブラウザー"));
		}

		[TestMethod]
		public void SyllabicNTest()
		{
			Assert.AreEqual("annai",                _system.Process("あんない"));
			Assert.AreEqual("gunma",                _system.Process("ぐんま"));
			Assert.AreEqual("kan'i",                _system.Process("かんい"));
			Assert.AreEqual("shin'you",             _system.Process("しんよう"));
		}

		[TestMethod]
		public void LongConsonantTest()
		{
			Assert.AreEqual("kekka",                _system.Process("けっか"));
			Assert.AreEqual("sassato",              _system.Process("さっさと"));
			Assert.AreEqual("zutto",                _system.Process("ずっと"));
			Assert.AreEqual("kippu",                _system.Process("きっぷ"));
			Assert.AreEqual("zasshi",               _system.Process("ざっし"));
			Assert.AreEqual("issho",                _system.Process("いっしょ"));
			Assert.AreEqual("kotchi",               _system.Process("こっち"));
			Assert.AreEqual("matcha",               _system.Process("まっちゃ"));
			Assert.AreEqual("mittsu",               _system.Process("みっつ"));
		}
	}
}