using Xunit;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	/// <summary>
	/// For testing the Russian BGN/PCGN romanization system, <see cref="Russian.BgnPcgn"/>.
	/// </summary>
	public class BgnPcgnTests : TestClass
	{
		private readonly Russian.BgnPcgn _system = new();

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
			Assert.Equal("Ygy·atta",         _system.Process("Ыгыатта"));
			Assert.Equal("Ku·yrkʺyavr",      _system.Process("Куыркъявр"));
			Assert.Equal("Ulan-Ud·e",        _system.Process("Улан-Удэ"));
			Assert.Equal("Ty·ay·a",          _system.Process("Тыайа"));
			Assert.Equal("Chapayevsk",       _system.Process("Чапаевск"));
			Assert.Equal("Meyyerovka",       _system.Process("Мейеровка"));
			Assert.Equal("Barnaul",          _system.Process("Барнаул"));
			Assert.Equal("Yakut·sk",         _system.Process("Якутск"));
			Assert.Equal("Yttyk-Këlʹ",       _system.Process("Ыттык-Кёль"));
			Assert.Equal("Ufa",              _system.Process("Уфа"));
			Assert.Equal("radostʹ",          _system.Process("ра́дость"));
			Assert.Equal("radostʹ tsvetok",  _system.Process("радость цветок"));
		}
	}
}
