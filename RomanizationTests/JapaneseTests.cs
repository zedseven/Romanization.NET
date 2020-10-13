using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests
{
	[TestClass]
	public class JapaneseTests
	{
		[TestInitialize]
		public void Initialize()
		{
			// Force the initialization of the Romanizer before testing begins
			_ = Japanese.Romanizer.Value;
		}

		[TestMethod]
		public void HepburnTestLongVowel()
		{
			Assert.AreEqual("nintendō", Japanese.Romanizer.Value.ModifiedHepburn("ニンテンドー"));
			Assert.AreEqual("burauzā", Japanese.Romanizer.Value.ModifiedHepburn("ブラウザー"));
			Assert.AreEqual("nintendō DSi burauzā", Japanese.Romanizer.Value.ModifiedHepburn("ニンテンドーDSiブラウザー"));
		}

		[TestMethod]
		public void HepburnTestLanguageBoundary()
		{
			Assert.AreEqual("nintendō DSi burauzā", Japanese.Romanizer.Value.ModifiedHepburn("ニンテンドーDSiブラウザー"));
			Assert.AreEqual("G.G shirīzu dorifutosākitto", Japanese.Romanizer.Value.ModifiedHepburn("G.Gシリーズ ドリフトサーキット"));
		}

		[TestMethod]
		public void HepburnTestSyllabicN()
		{
			Assert.AreEqual("annai", Japanese.Romanizer.Value.ModifiedHepburn("あんない"));
			Assert.AreEqual("gunma", Japanese.Romanizer.Value.ModifiedHepburn("ぐんま"));
			Assert.AreEqual("kan'i", Japanese.Romanizer.Value.ModifiedHepburn("かんい"));
			Assert.AreEqual("shin'you", Japanese.Romanizer.Value.ModifiedHepburn("しんよう"));
		}

		[TestMethod]
		public void HepburnTestLongConsonant()
		{
			Assert.AreEqual("kekka", Japanese.Romanizer.Value.ModifiedHepburn("けっか"));
			Assert.AreEqual("sassato", Japanese.Romanizer.Value.ModifiedHepburn("さっさと"));
			Assert.AreEqual("zutto", Japanese.Romanizer.Value.ModifiedHepburn("ずっと"));
			Assert.AreEqual("kippu", Japanese.Romanizer.Value.ModifiedHepburn("きっぷ"));
			Assert.AreEqual("zasshi", Japanese.Romanizer.Value.ModifiedHepburn("ざっし"));
			Assert.AreEqual("issho", Japanese.Romanizer.Value.ModifiedHepburn("いっしょ"));
			Assert.AreEqual("kotchi", Japanese.Romanizer.Value.ModifiedHepburn("こっち"));
			Assert.AreEqual("matcha", Japanese.Romanizer.Value.ModifiedHepburn("まっちゃ"));
			Assert.AreEqual("mittsu", Japanese.Romanizer.Value.ModifiedHepburn("みっつ"));
		}
	}
}