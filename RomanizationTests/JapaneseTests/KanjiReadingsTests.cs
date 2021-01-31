using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.JapaneseTests
{
	[TestClass]
	public class KanjiReadingsTests
	{
		private readonly Japanese.KanjiReadings _system = new Japanese.KanjiReadings();

		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                                                       _system.Process(""));
			Assert.AreEqual("oshieruyomu",                                            _system.Process("訓読"));
			Assert.AreEqual("arawarerukawaru karakataru shikirinihikiiru kotobanori", _system.Process("現代 漢語 頻率 詞典"));

			Assert.AreEqual("tsukueuchi",     _system.Process("案内"));
			Assert.AreEqual("muragaruuma",    _system.Process("群馬"));
			Assert.AreEqual("fudayasashii",   _system.Process("簡易"));
			Assert.AreEqual("makotomochiiru", _system.Process("信用"));
			Assert.AreEqual("musubuhatasu",   _system.Process("結果"));
			Assert.AreEqual("kiruwarifu",     _system.Process("切符"));
			Assert.AreEqual("majirushirusu",  _system.Process("雑誌"));
			Assert.AreEqual("hitotsuo",       _system.Process("一緒"));
			Assert.AreEqual("surucha",        _system.Process("抹茶"));
			Assert.AreEqual("mitsu",          _system.Process("三"));
		}

		[TestMethod]
		public void ProcessWithKanaTest()
		{
			Assert.AreEqual("",              _system.Process(""));
			Assert.AreEqual("oshieruyomumi", _system.ProcessWithKana("訓読み"));
			Assert.AreEqual("mitsutsu",      _system.ProcessWithKana("三つ")); // An example of why the simple Process() function is not to be relied on for accurate parsing of written systems with multiple readings per character
		}

		[TestMethod]
		public void ProcessWithReadingsTest()
		{
			Assert.AreEqual("", _system.ProcessWithReadings("").ToString());
			Assert.AreEqual("[oshieru oshie yomu kun kin][yomu yomi toku tou doku]", _system.ProcessWithReadings("訓読").ToString());
			Assert.AreEqual("[arawareru arawasu utsutsu gen ken][kawaru yo shiro dai tai] [kara kan][kataru kotoba tsugeru go gyo] [shikirini hin bin][hikiiru oomune wariai ritsu sotsu] [kotoba shi ji][nori tsukasadoru sakan ten]", _system.ProcessWithReadings("現代 漢語 頻率 詞典").ToString());
		}
	}
}