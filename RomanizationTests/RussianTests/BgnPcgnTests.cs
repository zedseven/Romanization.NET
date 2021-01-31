using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	[TestClass]
	public class BgnPcgnTests
	{
		private readonly Russian.BgnPcgn _system = new Russian.BgnPcgn();

		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                 _system.Process(""));
			Assert.AreEqual("Elektrogorsk",     _system.Process("Электрогорск"));
			Assert.AreEqual("Radioelektronika", _system.Process("Радиоэлектроника"));
			Assert.AreEqual("Tsimlyansk",       _system.Process("Цимлянск"));
			Assert.AreEqual("Severobaykalʹsk",  _system.Process("Северобайкальск"));
			Assert.AreEqual("Yoshkar-Ola",      _system.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossiya",          _system.Process("Россия"));
			Assert.AreEqual("Ygy·atta",         _system.Process("Ыгыатта"));
			Assert.AreEqual("Ku·yrkʺyavr",      _system.Process("Куыркъявр"));
			Assert.AreEqual("Ulan-Ud·e",        _system.Process("Улан-Удэ"));
			Assert.AreEqual("Ty·ay·a",          _system.Process("Тыайа"));
			Assert.AreEqual("Chapayevsk",       _system.Process("Чапаевск"));
			Assert.AreEqual("Meyyerovka",       _system.Process("Мейеровка"));
			Assert.AreEqual("Barnaul",          _system.Process("Барнаул"));
			Assert.AreEqual("Yakut·sk",         _system.Process("Якутск"));
			Assert.AreEqual("Yttyk-Këlʹ",       _system.Process("Ыттык-Кёль"));
			Assert.AreEqual("Ufa",              _system.Process("Уфа"));
			Assert.AreEqual("rádostʹ",          _system.Process("ра́дость"));
			Assert.AreEqual("radostʹ tsvetok",  _system.Process("радость цветок"));
		}
	}
}
