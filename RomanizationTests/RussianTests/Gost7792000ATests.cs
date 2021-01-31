using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	[TestClass]
	public class Gost7792000ATests
	{
		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                 Russian.Gost7792000A.Value.Process(""));
			Assert.AreEqual("Èlektrogorsk",     Russian.Gost7792000A.Value.Process("Электрогорск"));
			Assert.AreEqual("Radioèlektronika", Russian.Gost7792000A.Value.Process("Радиоэлектроника"));
			Assert.AreEqual("Cimlânsk",         Russian.Gost7792000A.Value.Process("Цимлянск"));
			Assert.AreEqual("Severobajkalʹsk",  Russian.Gost7792000A.Value.Process("Северобайкальск"));
			Assert.AreEqual("Joškar-Ola",       Russian.Gost7792000A.Value.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossiâ",           Russian.Gost7792000A.Value.Process("Россия"));
			Assert.AreEqual("Ygyatta",          Russian.Gost7792000A.Value.Process("Ыгыатта"));
			Assert.AreEqual("Kuyrkʺâvr",        Russian.Gost7792000A.Value.Process("Куыркъявр"));
			Assert.AreEqual("Ulan-Udè",         Russian.Gost7792000A.Value.Process("Улан-Удэ"));
			Assert.AreEqual("Tyaja",            Russian.Gost7792000A.Value.Process("Тыайа"));
			Assert.AreEqual("Čapaevsk",         Russian.Gost7792000A.Value.Process("Чапаевск"));
			Assert.AreEqual("Mejerovka",        Russian.Gost7792000A.Value.Process("Мейеровка"));
			Assert.AreEqual("Barnaul",          Russian.Gost7792000A.Value.Process("Барнаул"));
			Assert.AreEqual("Âkutsk",           Russian.Gost7792000A.Value.Process("Якутск"));
			Assert.AreEqual("Yttyk-Këlʹ",       Russian.Gost7792000A.Value.Process("Ыттык-Кёль"));
			Assert.AreEqual("Ufa",              Russian.Gost7792000A.Value.Process("Уфа"));
			Assert.AreEqual("rádostʹ",          Russian.Gost7792000A.Value.Process("ра́дость"));
			Assert.AreEqual("radostʹ cvetok",   Russian.Gost7792000A.Value.Process("радость цветок"));
		}
	}
}
