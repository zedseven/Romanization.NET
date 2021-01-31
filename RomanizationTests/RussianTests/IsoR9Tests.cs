using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	[TestClass]
	public class IsoR9Tests
	{
		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                 Russian.IsoR9.Value.Process(""));
			Assert.AreEqual("Ėlektrogorsk",     Russian.IsoR9.Value.Process("Электрогорск"));
			Assert.AreEqual("Radioėlektronika", Russian.IsoR9.Value.Process("Радиоэлектроника"));
			Assert.AreEqual("Cimljansk",        Russian.IsoR9.Value.Process("Цимлянск"));
			Assert.AreEqual("Severobajkalʹsk",  Russian.IsoR9.Value.Process("Северобайкальск"));
			Assert.AreEqual("Joškar-Ola",       Russian.IsoR9.Value.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossija",          Russian.IsoR9.Value.Process("Россия"));
			Assert.AreEqual("Ygyatta",          Russian.IsoR9.Value.Process("Ыгыатта"));
			Assert.AreEqual("Kuyrkʺjavr",       Russian.IsoR9.Value.Process("Куыркъявр"));
			Assert.AreEqual("Ulan-Udė",         Russian.IsoR9.Value.Process("Улан-Удэ"));
			Assert.AreEqual("Tyaja",            Russian.IsoR9.Value.Process("Тыайа"));
			Assert.AreEqual("Čapaevsk",         Russian.IsoR9.Value.Process("Чапаевск"));
			Assert.AreEqual("Mejerovka",        Russian.IsoR9.Value.Process("Мейеровка"));
			Assert.AreEqual("Barnaul",          Russian.IsoR9.Value.Process("Барнаул"));
			Assert.AreEqual("Jakutsk",          Russian.IsoR9.Value.Process("Якутск"));
			Assert.AreEqual("Yttyk-Këlʹ",       Russian.IsoR9.Value.Process("Ыттык-Кёль"));
			Assert.AreEqual("Ufa",              Russian.IsoR9.Value.Process("Уфа"));
			Assert.AreEqual("rádostʹ",          Russian.IsoR9.Value.Process("ра́дость"));
			Assert.AreEqual("radostʹ cvetok",   Russian.IsoR9.Value.Process("радость цветок"));
		}
	}
}
