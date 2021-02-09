using Xunit;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	/// <summary>
	/// For testing the Russian ALA-LC romanization system, <see cref="Russian.AlaLc"/>.
	/// </summary>
	public class AlaLcTests : TestClass
	{
		private readonly Russian.AlaLc _system = new();

		/// <summary>
		/// Aims to test basic processing.
		/// </summary>
		[Fact]
		public void ProcessTest()
		{
			Assert.Equal("",                 _system.Process(""));
			Assert.Equal("Ėlektrogorsk",     _system.Process("Электрогорск"));
			Assert.Equal("Radioėlektronika", _system.Process("Радиоэлектроника"));
			Assert.Equal("T͡simli͡ansk",       _system.Process("Цимлянск"));
			Assert.Equal("Severobaĭkalʹsk",  _system.Process("Северобайкальск"));
			Assert.Equal("Ĭoshkar-Ola",      _system.Process("Йошкар-Ола"));
			Assert.Equal("Rossii͡a",          _system.Process("Россия"));
			Assert.Equal("Ygyatta",          _system.Process("Ыгыатта"));
			Assert.Equal("Kuyrkʺi͡avr",       _system.Process("Куыркъявр"));
			Assert.Equal("Ulan-Udė",         _system.Process("Улан-Удэ"));
			Assert.Equal("Tyaĭa",            _system.Process("Тыайа"));
			Assert.Equal("Chapaevsk",        _system.Process("Чапаевск"));
			Assert.Equal("Meĭerovka",        _system.Process("Мейеровка"));
			Assert.Equal("Barnaul",          _system.Process("Барнаул"));
			Assert.Equal("I͡akutsk",          _system.Process("Якутск"));
			Assert.Equal("Yttyk-Këlʹ",       _system.Process("Ыттык-Кёль"));
			Assert.Equal("Ufa",              _system.Process("Уфа"));
			Assert.Equal("radostʹ",          _system.Process("ра́дость"));
			Assert.Equal("radostʹ t͡svetok",  _system.Process("радость цветок"));
		}
	}
}
