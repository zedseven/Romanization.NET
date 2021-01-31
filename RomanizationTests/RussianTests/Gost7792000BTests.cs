using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	[TestClass]
	public class Gost7792000BTests
	{
		private readonly Russian.Gost7792000B _system = new Russian.Gost7792000B();

		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                  _system.Process(""));
			Assert.AreEqual("E`lektrogorsk",     _system.Process("Электрогорск"));
			Assert.AreEqual("Radioe`lektronika", _system.Process("Радиоэлектроника"));
			Assert.AreEqual("Cimlyansk",         _system.Process("Цимлянск"));
			Assert.AreEqual("Severobajkalʹsk",   _system.Process("Северобайкальск"));
			Assert.AreEqual("Joshkar-Ola",       _system.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossiya",           _system.Process("Россия"));
			Assert.AreEqual("Ygyatta",           _system.Process("Ыгыатта"));
			Assert.AreEqual("Kuyrkʺyavr",        _system.Process("Куыркъявр"));
			Assert.AreEqual("Ulan-Ude`",         _system.Process("Улан-Удэ"));
			Assert.AreEqual("Tyaja",             _system.Process("Тыайа"));
			Assert.AreEqual("Chapaevsk",         _system.Process("Чапаевск"));
			Assert.AreEqual("Mejerovka",         _system.Process("Мейеровка"));
			Assert.AreEqual("Barnaul",           _system.Process("Барнаул"));
			Assert.AreEqual("Yakutsk",           _system.Process("Якутск"));
			Assert.AreEqual("Yttyk-Kyolʹ",       _system.Process("Ыттык-Кёль"));
			Assert.AreEqual("Ufa",               _system.Process("Уфа"));
			Assert.AreEqual("rádostʹ",           _system.Process("ра́дость"));
			Assert.AreEqual("radostʹ czvetok",   _system.Process("радость цветок"));
		}
	}
}
