using Xunit;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	/// <summary>
	/// For testing the Russian GOST 7.79-2000(B) romanization system, <see cref="Russian.Gost7792000B"/>.
	/// </summary>
	public class Gost7792000BTests : TestClass
	{
		private readonly Russian.Gost7792000B _system = new();

		/// <summary>
		/// Aims to test basic processing.
		/// </summary>
		[Fact]
		public void ProcessTest()
		{
			Assert.Equal("",                  _system.Process(""));
			Assert.Equal("E`lektrogorsk",     _system.Process("Электрогорск"));
			Assert.Equal("Radioe`lektronika", _system.Process("Радиоэлектроника"));
			Assert.Equal("Cimlyansk",         _system.Process("Цимлянск"));
			Assert.Equal("Severobajkalʹsk",   _system.Process("Северобайкальск"));
			Assert.Equal("Joshkar-Ola",       _system.Process("Йошкар-Ола"));
			Assert.Equal("Rossiya",           _system.Process("Россия"));
			Assert.Equal("Ygyatta",           _system.Process("Ыгыатта"));
			Assert.Equal("Kuyrkʺyavr",        _system.Process("Куыркъявр"));
			Assert.Equal("Ulan-Ude`",         _system.Process("Улан-Удэ"));
			Assert.Equal("Tyaja",             _system.Process("Тыайа"));
			Assert.Equal("Chapaevsk",         _system.Process("Чапаевск"));
			Assert.Equal("Mejerovka",         _system.Process("Мейеровка"));
			Assert.Equal("Barnaul",           _system.Process("Барнаул"));
			Assert.Equal("Yakutsk",           _system.Process("Якутск"));
			Assert.Equal("Yttyk-Kyolʹ",       _system.Process("Ыттык-Кёль"));
			Assert.Equal("Ufa",               _system.Process("Уфа"));
			Assert.Equal("radostʹ",           _system.Process("ра́дость"));
			Assert.Equal("radostʹ czvetok",   _system.Process("радость цветок"));
			Assert.Equal("radost czvetok",    _system.Process("ра́достЬ цветок"));
		}
	}
}
