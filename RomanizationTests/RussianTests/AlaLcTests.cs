using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	[TestClass]
	public class AlaLcTests
	{
		private readonly Russian.AlaLc _system = new Russian.AlaLc();

		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                 _system.Process(""));
			Assert.AreEqual("Ėlektrogorsk",     _system.Process("Электрогорск"));
			Assert.AreEqual("Radioėlektronika", _system.Process("Радиоэлектроника"));
			Assert.AreEqual("T͡simli͡ansk",       _system.Process("Цимлянск"));
			Assert.AreEqual("Severobaĭkalʹsk",  _system.Process("Северобайкальск"));
			Assert.AreEqual("Ĭoshkar-Ola",      _system.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossii͡a",          _system.Process("Россия"));
			Assert.AreEqual("Ygyatta",          _system.Process("Ыгыатта"));
			Assert.AreEqual("Kuyrkʺi͡avr",       _system.Process("Куыркъявр"));
			Assert.AreEqual("Ulan-Udė",         _system.Process("Улан-Удэ"));
			Assert.AreEqual("Tyaĭa",            _system.Process("Тыайа"));
			Assert.AreEqual("Chapaevsk",        _system.Process("Чапаевск"));
			Assert.AreEqual("Meĭerovka",        _system.Process("Мейеровка"));
			Assert.AreEqual("Barnaul",          _system.Process("Барнаул"));
			Assert.AreEqual("I͡akutsk",          _system.Process("Якутск"));
			Assert.AreEqual("Yttyk-Këlʹ",       _system.Process("Ыттык-Кёль"));
			Assert.AreEqual("Ufa",              _system.Process("Уфа"));
			Assert.AreEqual("rádostʹ",          _system.Process("ра́дость"));
			Assert.AreEqual("radostʹ t͡svetok",  _system.Process("радость цветок"));
		}
	}
}
