using Xunit;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	/// <summary>
	/// For testing the Russian British Standard 2979:1958 romanization system, <see cref="Russian.Bs29791958"/>.
	/// </summary>
	public class Bs29791958Tests : TestClass
	{
		private readonly Russian.Bs29791958 _system = new();

		/// <summary>
		/// Aims to test basic processing.
		/// </summary>
		[Fact]
		public void ProcessTest()
		{
			Assert.Equal("",                 _system.Process(""));
			Assert.Equal("Élektrogorsk",     _system.Process("Электрогорск"));
			Assert.Equal("Radioélektronika", _system.Process("Радиоэлектроника"));
			Assert.Equal("Tsimlyansk",       _system.Process("Цимлянск"));
			Assert.Equal("Severobaĭkalʹsk",  _system.Process("Северобайкальск"));
			Assert.Equal("Ĭoshkar-Ola",      _system.Process("Йошкар-Ола"));
			Assert.Equal("Rossiya",          _system.Process("Россия"));
			Assert.Equal("Ȳgȳatta",          _system.Process("Ыгыатта"));
			Assert.Equal("Kuȳrkʺyavr",       _system.Process("Куыркъявр"));
			Assert.Equal("Ulan-Udé",         _system.Process("Улан-Удэ"));
			Assert.Equal("Tȳaĭa",            _system.Process("Тыайа"));
			Assert.Equal("Chapaevsk",        _system.Process("Чапаевск"));
			Assert.Equal("Meĭerovka",        _system.Process("Мейеровка"));
			Assert.Equal("Barnaul",          _system.Process("Барнаул"));
			Assert.Equal("Yakut-sk",         _system.Process("Якутск"));
			Assert.Equal("Ȳttȳk-Këlʹ",       _system.Process("Ыттык-Кёль"));
			Assert.Equal("Ufa",              _system.Process("Уфа"));
			Assert.Equal("radostʹ",          _system.Process("ра́дость"));
			Assert.Equal("radostʹ tsvetok",  _system.Process("радость цветок"));
		}
	}
}
