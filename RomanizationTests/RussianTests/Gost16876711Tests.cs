using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	[TestClass]
	public class Gost16876711Tests
	{
		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                 Russian.Gost16876711.Value.Process(""));
			Assert.AreEqual("Ėlektrogorsk",     Russian.Gost16876711.Value.Process("Электрогорск"));
			Assert.AreEqual("Radioėlektronika", Russian.Gost16876711.Value.Process("Радиоэлектроника"));
			Assert.AreEqual("Cimlânsk",         Russian.Gost16876711.Value.Process("Цимлянск"));
			Assert.AreEqual("Severobajkalʹsk",  Russian.Gost16876711.Value.Process("Северобайкальск"));
			Assert.AreEqual("Joškar-Ola",       Russian.Gost16876711.Value.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossiâ",           Russian.Gost16876711.Value.Process("Россия"));
			Assert.AreEqual("Ygyatta",          Russian.Gost16876711.Value.Process("Ыгыатта"));
			Assert.AreEqual("Kuyrkʺâvr",        Russian.Gost16876711.Value.Process("Куыркъявр"));
			Assert.AreEqual("Ulan-Udė",         Russian.Gost16876711.Value.Process("Улан-Удэ"));
			Assert.AreEqual("Tyaja",            Russian.Gost16876711.Value.Process("Тыайа"));
			Assert.AreEqual("Čapaevsk",         Russian.Gost16876711.Value.Process("Чапаевск"));
			Assert.AreEqual("Mejerovka",        Russian.Gost16876711.Value.Process("Мейеровка"));
			Assert.AreEqual("Barnaul",          Russian.Gost16876711.Value.Process("Барнаул"));
			Assert.AreEqual("Âkutsk",           Russian.Gost16876711.Value.Process("Якутск"));
			Assert.AreEqual("Yttyk-Këlʹ",       Russian.Gost16876711.Value.Process("Ыттык-Кёль"));
			Assert.AreEqual("Ufa",              Russian.Gost16876711.Value.Process("Уфа"));
			Assert.AreEqual("rádostʹ",          Russian.Gost16876711.Value.Process("ра́дость"));
			Assert.AreEqual("radostʹ cvetok",   Russian.Gost16876711.Value.Process("радость цветок"));
		}
	}
}
