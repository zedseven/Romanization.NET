using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.JapaneseTests
{
	[TestClass]
	public class KanjiReadingsSystemTests
	{
		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                                                       Japanese.KanjiReadings.Value.Process(""));
			Assert.AreEqual("oshieruyomu",                                            Japanese.KanjiReadings.Value.Process("訓読"));
			Assert.AreEqual("arawarerukawaru karakataru shikirinihikiiru kotobanori", Japanese.KanjiReadings.Value.Process("現代 漢語 頻率 詞典"));

			Assert.AreEqual("tsukueuchi",     Japanese.KanjiReadings.Value.Process("案内"));
			Assert.AreEqual("muragaruuma",    Japanese.KanjiReadings.Value.Process("群馬"));
			Assert.AreEqual("fudayasashii",   Japanese.KanjiReadings.Value.Process("簡易"));
			Assert.AreEqual("makotomochiiru", Japanese.KanjiReadings.Value.Process("信用"));
			Assert.AreEqual("musubuhatasu",   Japanese.KanjiReadings.Value.Process("結果"));
			Assert.AreEqual("kiruwarifu",     Japanese.KanjiReadings.Value.Process("切符"));
			Assert.AreEqual("majirushirusu",  Japanese.KanjiReadings.Value.Process("雑誌"));
			Assert.AreEqual("hitotsuo",       Japanese.KanjiReadings.Value.Process("一緒"));
			Assert.AreEqual("surucha",        Japanese.KanjiReadings.Value.Process("抹茶"));
			Assert.AreEqual("mitsu",          Japanese.KanjiReadings.Value.Process("三"));
		}

		[TestMethod]
		public void ProcessWithKanaTest()
		{
			Assert.AreEqual("",              Japanese.KanjiReadings.Value.Process(""));
			Assert.AreEqual("oshieruyomumi", Japanese.KanjiReadings.Value.ProcessWithKana("訓読み"));
			Assert.AreEqual("mitsutsu",      Japanese.KanjiReadings.Value.ProcessWithKana("三つ")); // An example of why the simple Process() function is not to be relied on for accurate parsing of written systems with multiple readings per character
		}

		[TestMethod]
		public void ProcessWithReadingsTest()
		{
			Assert.AreEqual("", Japanese.KanjiReadings.Value.ProcessWithReadings("").ToString());
			Assert.AreEqual("[oshieru oshie yomu kun kin][yomu yomi toku tou doku]", Japanese.KanjiReadings.Value.ProcessWithReadings("訓読").ToString());
			Assert.AreEqual("[arawareru arawasu utsutsu gen ken][kawaru yo shiro dai tai] [kara kan][kataru kotoba tsugeru go gyo] [shikirini hin bin][hikiiru oomune wariai ritsu sotsu] [kotoba shi ji][nori tsukasadoru sakan ten]", Japanese.KanjiReadings.Value.ProcessWithReadings("現代 漢語 頻率 詞典").ToString());
		}
	}
}