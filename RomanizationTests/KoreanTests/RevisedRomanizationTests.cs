using Microsoft.VisualStudio.TestTools.UnitTesting;
using Romanization;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace RomanizationTests.KoreanTests
{
	[TestClass]
	public class RevisedRomanizationTests
	{
		private readonly Korean.RevisedRomanization _system = new Korean.RevisedRomanization();

		[TestMethod]
		public void BasicWordTest()
		{
			Assert.AreEqual("han-geul", _system.Process("한글"));
			Assert.AreEqual("joko",     _system.Process("좋고"));
			Assert.AreEqual("nota",     _system.Process("놓다"));
			Assert.AreEqual("japyeo",   _system.Process("잡혀"));
			Assert.AreEqual("nachi",    _system.Process("낳지"));
			Assert.AreEqual("",         _system.Process(""));
		}

		[TestMethod]
		public void GivenNameTest()
		{
			Assert.AreEqual("jeong seokmin",  _system.Process("정 석민", true));
			Assert.AreEqual("jeong seongmin", _system.Process("정 석민", false));
			Assert.AreEqual("choe bitna",     _system.Process("최 빛나", true));
			Assert.AreEqual("choe binna",     _system.Process("최 빛나", false));
		}

		[TestMethod]
		public void NounAspirationTest()
		{
			Assert.AreEqual("mukho",        _system.Process("묵호", false, true));
			Assert.AreEqual("muko",         _system.Process("묵호", false, false));
			Assert.AreEqual("jiphyeonjeon", _system.Process("집현전", false, true));
			Assert.AreEqual("jipyeonjeon",  _system.Process("집현전", false, false));
		}

		[TestMethod]
		public void SyllableHyphenationTest()
		{
			Assert.AreEqual("jeong seok-min",  _system.Process("정 석민", true, false, true));
			Assert.AreEqual("jeong seong-min", _system.Process("정 석민", false, false, true));
			Assert.AreEqual("jiph-yeon-jeon",  _system.Process("집현전", false, true, true));
			Assert.AreEqual("sirijeu",         _system.Process("시리즈", false, false, false));
			Assert.AreEqual("si-ri-jeu",       _system.Process("시리즈", false, false, true));
		}
	}
}