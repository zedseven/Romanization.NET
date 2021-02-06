using Microsoft.VisualStudio.TestTools.UnitTesting;
using Romanization;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace RomanizationTests.KoreanTests
{
	[TestClass]
	public class HanjaReadingsTests
	{
		private readonly Korean.HanjaReadings _system = new Korean.HanjaReadings();

		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("gotjeongsa",               _system.Process("串井事"));
			Assert.AreEqual("ryoageukyang",             _system.Process("了亞亟享"));
			Assert.AreEqual("hyu",                      _system.Process("休"));
			Assert.AreEqual("ryeryunhyeongjiseolcheom", _system.Process("例侖詗誌說諂"));
			Assert.AreEqual("geunjijihyeonin",          _system.Process("謹識識見璘"));
		}

		[TestMethod]
		public void ProcessToHangeulTest()
		{
			Assert.AreEqual("곶정사",       _system.ProcessToHangeul("串井事"));
			Assert.AreEqual("료아극향",     _system.ProcessToHangeul("了亞亟享"));
			Assert.AreEqual("휴",          _system.ProcessToHangeul("休"));
			Assert.AreEqual("례륜형지설첨", _system.ProcessToHangeul("例侖詗誌說諂"));
			Assert.AreEqual("근지지현인",   _system.ProcessToHangeul("謹識識見璘"));
		}

		[TestMethod]
		public void ProcessWithReadingsTest()
		{
			Assert.AreEqual("[곶 관]정사",              _system.ProcessWithReadings("串井事").ToString());
			Assert.AreEqual("[료 요]아극향",            _system.ProcessWithReadings("了亞亟享").ToString());
			Assert.AreEqual("휴",                       _system.ProcessWithReadings("休").ToString());
			Assert.AreEqual("[례 예]륜형지[설 세 열]첨", _system.ProcessWithReadings("例侖詗誌說諂").ToString());
			Assert.AreEqual("근지지현인",               _system.ProcessWithReadings("謹識識見璘").ToString());
		}
	}
}