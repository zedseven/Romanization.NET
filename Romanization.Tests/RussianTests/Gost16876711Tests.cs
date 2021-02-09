using Xunit;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	/// <summary>
	/// For testing the Russian GOST 16876-71(1) romanization system, <see cref="Russian.Gost16876711"/>.
	/// </summary>
	public class Gost16876711Tests : TestClass
	{
		private readonly Russian.Gost16876711 _system = new();

		/// <summary>
		/// Aims to test basic processing.
		/// </summary>
		[Fact]
		public void ProcessTest()
		{
			Assert.Equal("",                 _system.Process(""));
			Assert.Equal("Ėlektrogorsk",     _system.Process("Электрогорск"));
			Assert.Equal("Radioėlektronika", _system.Process("Радиоэлектроника"));
			Assert.Equal("Cimlânsk",         _system.Process("Цимлянск"));
			Assert.Equal("Severobajkalʹsk",  _system.Process("Северобайкальск"));
			Assert.Equal("Joškar-Ola",       _system.Process("Йошкар-Ола"));
			Assert.Equal("Rossiâ",           _system.Process("Россия"));
			Assert.Equal("Ygyatta",          _system.Process("Ыгыатта"));
			Assert.Equal("Kuyrkʺâvr",        _system.Process("Куыркъявр"));
			Assert.Equal("Ulan-Udė",         _system.Process("Улан-Удэ"));
			Assert.Equal("Tyaja",            _system.Process("Тыайа"));
			Assert.Equal("Čapaevsk",         _system.Process("Чапаевск"));
			Assert.Equal("Mejerovka",        _system.Process("Мейеровка"));
			Assert.Equal("Barnaul",          _system.Process("Барнаул"));
			Assert.Equal("Âkutsk",           _system.Process("Якутск"));
			Assert.Equal("Yttyk-Këlʹ",       _system.Process("Ыттык-Кёль"));
			Assert.Equal("Ufa",              _system.Process("Уфа"));
			Assert.Equal("radostʹ",          _system.Process("ра́дость"));
			Assert.Equal("radostʹ cvetok",   _system.Process("радость цветок"));
		}
	}
}
