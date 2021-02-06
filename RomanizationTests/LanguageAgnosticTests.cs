using Microsoft.VisualStudio.TestTools.UnitTesting;
using Romanization;

// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

namespace RomanizationTests.LanguageAgnosticTests
{
	/// <summary>
	/// For testing language agnostic functions.
	/// </summary>
	[TestClass]
	public class LanguageAgnosticTests
	{
		private readonly Japanese.ModifiedHepburn _modifiedHepburn = new Japanese.ModifiedHepburn();
		private readonly Korean.RevisedRomanization _revisedRomanization = new Korean.RevisedRomanization();

		/// <summary>
		/// Aims to test the separation of language boundaries.
		/// </summary>
		[TestMethod]
		public void LanguageBoundaryTest()
		{
			Assert.AreEqual("nintendō DSi burauzā",        _modifiedHepburn.Process("ニンテンドーDSiブラウザー"));
			Assert.AreEqual("G.G shirīzu dorifutosākitto", _modifiedHepburn.Process("G.Gシリーズ ドリフトサーキット"));

			Assert.AreEqual("G.G sirijeu DRIFT CIRCUIT",                     _revisedRomanization.Process("G.G 시리즈 DRIFT CIRCUIT"));
			Assert.AreEqual("sutjarang nolja! maejikseukweeowa imijigyesan", _revisedRomanization.Process("숫자랑 놀자! 매직스퀘어와 이미지계산"));
			Assert.AreEqual("rieolsakeo 2010",                               _revisedRomanization.Process("리얼사커 2010"));
		}
	}
}
