using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.LanguageAgnosticTests
{
	[TestClass]
	public class LanguageAgnosticTests
	{
		[TestInitialize]
		public void Initialize()
		{
			// Force the initialization of the Romanizers before testing begins
			_ = Japanese.ModifiedHepburn.Value;
			_ = Korean.RevisedRomanization.Value;
		}

		[TestMethod]
		public void LanguageBoundaryTest()
		{
			Assert.AreEqual("nintendō DSi burauzā",        Japanese.ModifiedHepburn.Value.Process("ニンテンドーDSiブラウザー"));
			Assert.AreEqual("G.G shirīzu dorifutosākitto", Japanese.ModifiedHepburn.Value.Process("G.Gシリーズ ドリフトサーキット"));

			Assert.AreEqual("G.G sirijeu DRIFT CIRCUIT",                     Korean.RevisedRomanization.Value.Process("G.G 시리즈 DRIFT CIRCUIT"));
			Assert.AreEqual("sutjarang nolja! maejikseukweeowa imijigyesan", Korean.RevisedRomanization.Value.Process("숫자랑 놀자! 매직스퀘어와 이미지계산"));
			Assert.AreEqual("rieolsakeo 2010",                               Korean.RevisedRomanization.Value.Process("리얼사커 2010"));
		}
	}
}