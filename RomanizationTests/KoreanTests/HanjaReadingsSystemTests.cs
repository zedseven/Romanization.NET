using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.KoreanTests
{
	[TestClass]
	public class HanjaReadingsSystemTests
	{
		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("gotjeongsa",               Korean.HanjaReadings.Value.Process("串井事"));
			Assert.AreEqual("ryoageukyang",             Korean.HanjaReadings.Value.Process("了亞亟享"));
			Assert.AreEqual("hyu",                      Korean.HanjaReadings.Value.Process("休"));
			Assert.AreEqual("ryeryunhyeongjiseolcheom", Korean.HanjaReadings.Value.Process("例侖詗誌說諂"));
			Assert.AreEqual("geunjijihyeonin",          Korean.HanjaReadings.Value.Process("謹識識見璘"));
		}

		[TestMethod]
		public void ProcessToHangeulTest()
		{
			Assert.AreEqual("곶정사",       Korean.HanjaReadings.Value.ProcessToHangeul("串井事"));
			Assert.AreEqual("료아극향",     Korean.HanjaReadings.Value.ProcessToHangeul("了亞亟享"));
			Assert.AreEqual("휴",          Korean.HanjaReadings.Value.ProcessToHangeul("休"));
			Assert.AreEqual("례륜형지설첨", Korean.HanjaReadings.Value.ProcessToHangeul("例侖詗誌說諂"));
			Assert.AreEqual("근지지현인",   Korean.HanjaReadings.Value.ProcessToHangeul("謹識識見璘"));
		}

		[TestMethod]
		public void ProcessWithReadingsTest()
		{
			Assert.AreEqual("[곶 관]정사",              Korean.HanjaReadings.Value.ProcessWithReadings("串井事").ToString());
			Assert.AreEqual("[료 요]아극향",            Korean.HanjaReadings.Value.ProcessWithReadings("了亞亟享").ToString());
			Assert.AreEqual("휴",                       Korean.HanjaReadings.Value.ProcessWithReadings("休").ToString());
			Assert.AreEqual("[례 예]륜형지[설 세 열]첨", Korean.HanjaReadings.Value.ProcessWithReadings("例侖詗誌說諂").ToString());
			Assert.AreEqual("근지지현인",               Korean.HanjaReadings.Value.ProcessWithReadings("謹識識見璘").ToString());
		}
	}
}