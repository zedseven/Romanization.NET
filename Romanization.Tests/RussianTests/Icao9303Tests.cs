using Xunit;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	/// <summary>
	/// For testing the Russian ICAO Doc 9303 romanization system, <see cref="Russian.Icao9303"/>.
	/// </summary>
	public class Icao9303Tests : TestClass
	{
		private readonly Russian.Icao9303 _system = new();

		/// <summary>
		/// Aims to test basic processing.
		/// </summary>
		[Fact]
		public void ProcessTest()
		{
			Assert.Equal("",                 _system.Process(""));
			Assert.Equal("Elektrogorsk",     _system.Process("Электрогорск"));
			Assert.Equal("Radioelektronika", _system.Process("Радиоэлектроника"));
			Assert.Equal("Tsimliansk",       _system.Process("Цимлянск"));
			Assert.Equal("Severobaikalsk",   _system.Process("Северобайкальск"));
			Assert.Equal("Ioshkar-Ola",      _system.Process("Йошкар-Ола"));
			Assert.Equal("Rossiia",          _system.Process("Россия"));
			Assert.Equal("Ygyatta",          _system.Process("Ыгыатта"));
			Assert.Equal("Kuyrkieiavr",      _system.Process("Куыркъявр"));
			Assert.Equal("Ulan-Ude",         _system.Process("Улан-Удэ"));
			Assert.Equal("Tyaia",            _system.Process("Тыайа"));
			Assert.Equal("Chapaevsk",        _system.Process("Чапаевск"));
			Assert.Equal("Meierovka",        _system.Process("Мейеровка"));
			Assert.Equal("Barnaul",          _system.Process("Барнаул"));
			Assert.Equal("Iakutsk",          _system.Process("Якутск"));
			Assert.Equal("Yttyk-Kel",        _system.Process("Ыттык-Кёль"));
			Assert.Equal("Ufa",              _system.Process("Уфа"));
			Assert.Equal("radost",           _system.Process("ра́дость"));
			Assert.Equal("radost tsvetok",   _system.Process("радость цветок"));
		}
	}
}
