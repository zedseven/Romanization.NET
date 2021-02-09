using Xunit;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	/// <summary>
	/// For testing the Russian road sign romanization system, <see cref="Russian.RoadSigns"/>.
	/// </summary>
	public class RoadSignsTests : TestClass
	{
		private readonly Russian.RoadSigns _system = new();

		/// <summary>
		/// Aims to test basic processing.
		/// </summary>
		[Fact]
		public void ProcessTest()
		{
			Assert.Equal("",                 _system.Process(""));
			Assert.Equal("Elektrogorsk",     _system.Process("Электрогорск"));
			Assert.Equal("Radioelektronika", _system.Process("Радиоэлектроника"));
			Assert.Equal("Tsimlyansk",       _system.Process("Цимлянск"));
			Assert.Equal("Severobaykalʹsk",  _system.Process("Северобайкальск"));
			Assert.Equal("Yoshkar-Ola",      _system.Process("Йошкар-Ола"));
			Assert.Equal("Rossiya",          _system.Process("Россия"));
			Assert.Equal("Ygyatta",          _system.Process("Ыгыатта"));
			Assert.Equal("Kuyrkʹyavr",       _system.Process("Куыркъявр"));
			Assert.Equal("Ulan-Ude",         _system.Process("Улан-Удэ"));
			Assert.Equal("Tyaya",            _system.Process("Тыайа"));
			Assert.Equal("Chapayevsk",       _system.Process("Чапаевск"));
			Assert.Equal("Meyerovka",        _system.Process("Мейеровка"));
			Assert.Equal("Barnaul",          _system.Process("Барнаул"));
			Assert.Equal("Yakutsk",          _system.Process("Якутск"));
			Assert.Equal("Yttyk-Kyelʹ",      _system.Process("Ыттык-Кёль"));
			Assert.Equal("Ufa",              _system.Process("Уфа"));
			Assert.Equal("radostʹ",          _system.Process("ра́дость"));
			Assert.Equal("radostʹ tsvetok",  _system.Process("радость цветок"));
		}
	}
}
