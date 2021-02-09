using Xunit;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.KoreanTests
{
	/// <summary>
	/// For testing the Korean Revised Romanization system, <see cref="Korean.RevisedRomanization"/>.
	/// </summary>
	public class RevisedRomanizationTests : TestClass
	{
		private readonly Korean.RevisedRomanization _system = new();

		/// <summary>
		/// Aims to test basic words.
		/// </summary>
		[Fact]
		public void BasicWordTest()
		{
			Assert.Equal("han-geul", _system.Process("한글"));
			Assert.Equal("joko",     _system.Process("좋고"));
			Assert.Equal("nota",     _system.Process("놓다"));
			Assert.Equal("japyeo",   _system.Process("잡혀"));
			Assert.Equal("nachi",    _system.Process("낳지"));
			Assert.Equal("",         _system.Process(""));
		}

		/// <summary>
		/// Aims to test the difference and behaviour of the given name processing. (given names are often romanized
		/// without consideration for special Jamo combinations)
		/// </summary>
		[Fact]
		public void GivenNameTest()
		{
			Assert.Equal("jeong seokmin",  _system.Process("정 석민", true));
			Assert.Equal("jeong seongmin", _system.Process("정 석민", false));
			Assert.Equal("choe bitna",     _system.Process("최 빛나", true));
			Assert.Equal("choe binna",     _system.Process("최 빛나", false));
		}

		/// <summary>
		/// Aims to test whether noun aspiration is handled properly.
		/// </summary>
		[Fact]
		public void NounAspirationTest()
		{
			Assert.Equal("mukho",        _system.Process("묵호", false, true));
			Assert.Equal("muko",         _system.Process("묵호", false, false));
			Assert.Equal("jiphyeonjeon", _system.Process("집현전", false, true));
			Assert.Equal("jipyeonjeon",  _system.Process("집현전", false, false));
		}

		/// <summary>
		/// Aims to test whether or not syllables are hyphenated properly.
		/// </summary>
		[Fact]
		public void SyllableHyphenationTest()
		{
			Assert.Equal("jeong seok-min",  _system.Process("정 석민", true, false, true));
			Assert.Equal("jeong seong-min", _system.Process("정 석민", false, false, true));
			Assert.Equal("jiph-yeon-jeon",  _system.Process("집현전", false, true, true));
			Assert.Equal("sirijeu",         _system.Process("시리즈", false, false, false));
			Assert.Equal("si-ri-jeu",       _system.Process("시리즈", false, false, true));
		}
	}
}
