using Xunit;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.JapaneseTests
{
	/// <summary>
	/// For testing the Japanese Kanji readings system, <see cref="Japanese.KanjiReadings"/>.
	/// </summary>
	public class KanjiReadingsTests : TestClass
	{
		private readonly Japanese.KanjiReadings _system = new();

		/// <summary>
		/// Aims to test quick readings where *a* reading of each character is used. (the first in the Unihan database
		/// for the character)
		/// </summary>
		[Fact]
		public void ProcessTest()
		{
			Assert.Equal("",                                                       _system.Process(""));
			Assert.Equal("oshieruyomu",                                            _system.Process("訓読"));
			Assert.Equal("arawarerukawaru karakataru shikirinihikiiru kotobanori", _system.Process("現代 漢語 頻率 詞典"));

			Assert.Equal("tsukueuchi",     _system.Process("案内"));
			Assert.Equal("muragaruuma",    _system.Process("群馬"));
			Assert.Equal("fudayasashii",   _system.Process("簡易"));
			Assert.Equal("makotomochiiru", _system.Process("信用"));
			Assert.Equal("musubuhatasu",   _system.Process("結果"));
			Assert.Equal("kiruwarifu",     _system.Process("切符"));
			Assert.Equal("majirushirusu",  _system.Process("雑誌"));
			Assert.Equal("hitotsuo",       _system.Process("一緒"));
			Assert.Equal("surucha",        _system.Process("抹茶"));
			Assert.Equal("mitsu",          _system.Process("三"));
		}

		/// <summary>
		/// Aims to test the processing of the combination of Kana and Kanji together.
		/// </summary>
		[Fact]
		public void ProcessWithKanaTest()
		{
			Assert.Equal("",              _system.Process(""));
			Assert.Equal("oshieruyomumi", _system.ProcessWithKana("訓読み"));
			// An example of why the simple Process() function is not to be relied on for accurate parsing of written systems with multiple readings per character
			Assert.Equal("mitsutsu",      _system.ProcessWithKana("三つ"));
		}

		/// <summary>
		/// Aims to test processing with all readings for every character.<br />
		/// This test does not demonstrate realistic use of this system.
		/// </summary>
		[Fact]
		public void ProcessWithReadingsTest()
		{
			Assert.Equal("", _system.ProcessWithReadings("").ToString());
			Assert.Equal("[oshieru oshie yomu kun kin][yomu yomi toku tou doku]",
				_system.ProcessWithReadings("訓読").ToString());
			Assert.Equal("[arawareru arawasu utsutsu gen ken][kawaru yo shiro dai tai] [kara kan]" +
							"[kataru kotoba tsugeru go gyo] [shikirini hin bin][hikiiru oomune wariai ritsu sotsu] " +
							"[kotoba shi ji][nori tsukasadoru sakan ten]",
				_system.ProcessWithReadings("現代 漢語 頻率 詞典").ToString());
		}
	}
}
