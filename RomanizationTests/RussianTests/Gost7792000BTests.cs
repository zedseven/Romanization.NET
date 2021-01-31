using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	[TestClass]
	public class Gost7792000BTests
	{
		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                  Russian.Gost7792000B.Value.Process(""));
			Assert.AreEqual("E`lektrogorsk",     Russian.Gost7792000B.Value.Process("Электрогорск"));
			Assert.AreEqual("Radioe`lektronika", Russian.Gost7792000B.Value.Process("Радиоэлектроника"));
			Assert.AreEqual("Cimlyansk",         Russian.Gost7792000B.Value.Process("Цимлянск"));
			Assert.AreEqual("Severobajkalʹsk",   Russian.Gost7792000B.Value.Process("Северобайкальск"));
			Assert.AreEqual("Joshkar-Ola",       Russian.Gost7792000B.Value.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossiya",           Russian.Gost7792000B.Value.Process("Россия"));
			Assert.AreEqual("Ygyatta",           Russian.Gost7792000B.Value.Process("Ыгыатта"));
			Assert.AreEqual("Kuyrkʺyavr",        Russian.Gost7792000B.Value.Process("Куыркъявр"));
			Assert.AreEqual("Ulan-Ude`",         Russian.Gost7792000B.Value.Process("Улан-Удэ"));
			Assert.AreEqual("Tyaja",             Russian.Gost7792000B.Value.Process("Тыайа"));
			Assert.AreEqual("Chapaevsk",         Russian.Gost7792000B.Value.Process("Чапаевск"));
			Assert.AreEqual("Mejerovka",         Russian.Gost7792000B.Value.Process("Мейеровка"));
			Assert.AreEqual("Barnaul",           Russian.Gost7792000B.Value.Process("Барнаул"));
			Assert.AreEqual("Yakutsk",           Russian.Gost7792000B.Value.Process("Якутск"));
			Assert.AreEqual("Yttyk-Kyolʹ",       Russian.Gost7792000B.Value.Process("Ыттык-Кёль"));
			Assert.AreEqual("Ufa",               Russian.Gost7792000B.Value.Process("Уфа"));
			Assert.AreEqual("rádostʹ",           Russian.Gost7792000B.Value.Process("ра́дость"));
			Assert.AreEqual("radostʹ czvetok",   Russian.Gost7792000B.Value.Process("радость цветок"));
		}
	}
}
