using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	[TestClass]
	public class Bs29791958Tests
	{
		private readonly Russian.Bs29791958 _system = new Russian.Bs29791958();

		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                 _system.Process(""));
			Assert.AreEqual("Élektrogorsk",     _system.Process("Электрогорск"));
			Assert.AreEqual("Radioélektronika", _system.Process("Радиоэлектроника"));
			Assert.AreEqual("Tsimlyansk",       _system.Process("Цимлянск"));
			Assert.AreEqual("Severobaĭkalʹsk",  _system.Process("Северобайкальск"));
			Assert.AreEqual("Ĭoshkar-Ola",      _system.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossiya",          _system.Process("Россия"));
			Assert.AreEqual("Ȳgȳatta",          _system.Process("Ыгыатта"));
			Assert.AreEqual("Kuȳrkʺyavr",       _system.Process("Куыркъявр"));
			Assert.AreEqual("Ulan-Udé",         _system.Process("Улан-Удэ"));
			Assert.AreEqual("Tȳaĭa",            _system.Process("Тыайа"));
			Assert.AreEqual("Chapaevsk",        _system.Process("Чапаевск"));
			Assert.AreEqual("Meĭerovka",        _system.Process("Мейеровка"));
			Assert.AreEqual("Barnaul",          _system.Process("Барнаул"));
			Assert.AreEqual("Yakut-sk",         _system.Process("Якутск"));
			Assert.AreEqual("Ȳttȳk-Këlʹ",       _system.Process("Ыттык-Кёль"));
			Assert.AreEqual("Ufa",              _system.Process("Уфа"));
			Assert.AreEqual("rádostʹ",          _system.Process("ра́дость"));
			Assert.AreEqual("radostʹ tsvetok",  _system.Process("радость цветок"));
		}
	}
}
