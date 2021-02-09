using Xunit;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.KoreanTests
{
	/// <summary>
	/// For testing the Korean Hanja readings system, <see cref="Korean.HanjaReadings"/>.
	/// </summary>
	public class HanjaReadingsTests : TestClass
	{
		private readonly Korean.HanjaReadings _system = new();

		/// <summary>
		/// Aims to test quick readings where *a* reading of each character is used. (the first in the Unihan database
		/// for the character)<br />
		/// The reading selected for the character, which is in Hangeul, is then further romanized.
		/// </summary>
		[Fact]
		public void ProcessTest()
		{
			Assert.Equal("gotjeongsa",               _system.Process("串井事"));
			Assert.Equal("ryoageukyang",             _system.Process("了亞亟享"));
			Assert.Equal("hyu",                      _system.Process("休"));
			Assert.Equal("ryeryunhyeongjiseolcheom", _system.Process("例侖詗誌說諂"));
			Assert.Equal("geunjijihyeonin",          _system.Process("謹識識見璘"));
		}

		/// <summary>
		/// Aims to test quick readings where *a* reading (in Hangeul) of each character is used. (the first in the
		/// Unihan database for the character)
		/// </summary>
		[Fact]
		public void ProcessToHangeulTest()
		{
			Assert.Equal("곶정사",       _system.ProcessToHangeul("串井事"));
			Assert.Equal("료아극향",     _system.ProcessToHangeul("了亞亟享"));
			Assert.Equal("휴",           _system.ProcessToHangeul("休"));
			Assert.Equal("례륜형지설첨", _system.ProcessToHangeul("例侖詗誌說諂"));
			Assert.Equal("근지지현인",   _system.ProcessToHangeul("謹識識見璘"));
		}

		/// <summary>
		/// Aims to test processing with all readings for every character (in Hangeul).<br />
		/// This test does not demonstrate realistic use of this system.
		/// </summary>
		[Fact]
		public void ProcessWithReadingsTest()
		{
			Assert.Equal("[곶 관]정사",               _system.ProcessWithReadings("串井事").ToString());
			Assert.Equal("[료 요]아극향",             _system.ProcessWithReadings("了亞亟享").ToString());
			Assert.Equal("휴",                        _system.ProcessWithReadings("休").ToString());
			Assert.Equal("[례 예]륜형지[설 세 열]첨", _system.ProcessWithReadings("例侖詗誌說諂").ToString());
			Assert.Equal("근지지현인",                _system.ProcessWithReadings("謹識識見璘").ToString());
		}
	}
}
