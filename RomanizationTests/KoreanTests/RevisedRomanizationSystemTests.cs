using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

namespace Romanization.KoreanTests
{
	[TestClass]
	public class RevisedRomanizationSystemTests
	{
		[TestInitialize]
		public void Initialize()
		{
			// Force the initialization of the Romanizer before testing begins
			_ = Korean.RevisedRomanization.Value;
		}

		[TestMethod]
		public void BasicWordTest()
		{
			Assert.AreEqual("han-geul", Korean.RevisedRomanization.Value.Process("한글"));
			Assert.AreEqual("joko",     Korean.RevisedRomanization.Value.Process("좋고"));
			Assert.AreEqual("nota",     Korean.RevisedRomanization.Value.Process("놓다"));
			Assert.AreEqual("japyeo",   Korean.RevisedRomanization.Value.Process("잡혀"));
			Assert.AreEqual("nachi",    Korean.RevisedRomanization.Value.Process("낳지"));
			Assert.AreEqual("",         Korean.RevisedRomanization.Value.Process(""));
		}

		[TestMethod]
		public void GivenNameTest()
		{
			Assert.AreEqual("jeong seokmin",  Korean.RevisedRomanization.Value.Process("정 석민", true));
			Assert.AreEqual("jeong seongmin", Korean.RevisedRomanization.Value.Process("정 석민", false));
			Assert.AreEqual("choe bitna",     Korean.RevisedRomanization.Value.Process("최 빛나", true));
			Assert.AreEqual("choe binna",     Korean.RevisedRomanization.Value.Process("최 빛나", false));
		}

		[TestMethod]
		public void NounAspirationTest()
		{
			Assert.AreEqual("mukho",        Korean.RevisedRomanization.Value.Process("묵호", false, true));
			Assert.AreEqual("muko",         Korean.RevisedRomanization.Value.Process("묵호", false, false));
			Assert.AreEqual("jiphyeonjeon", Korean.RevisedRomanization.Value.Process("집현전", false, true));
			Assert.AreEqual("jipyeonjeon",  Korean.RevisedRomanization.Value.Process("집현전", false, false));
		}

		[TestMethod]
		public void SyllableHyphenationTest()
		{
			Assert.AreEqual("jeong seok-min",  Korean.RevisedRomanization.Value.Process("정 석민", true, false, true));
			Assert.AreEqual("jeong seong-min", Korean.RevisedRomanization.Value.Process("정 석민", false, false, true));
			Assert.AreEqual("jiph-yeon-jeon",  Korean.RevisedRomanization.Value.Process("집현전", false, true, true));
			Assert.AreEqual("sirijeu",         Korean.RevisedRomanization.Value.Process("시리즈", false, false, false));
			Assert.AreEqual("si-ri-jeu",       Korean.RevisedRomanization.Value.Process("시리즈", false, false, true));
		}
	}
}