using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	[TestClass]
	public class Gost16876712Tests
	{
		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                  Russian.Gost16876712.Value.Process(""));
			Assert.AreEqual("Ehlektrogorsk",     Russian.Gost16876712.Value.Process("Электрогорск"));
			Assert.AreEqual("Radioehlektronika", Russian.Gost16876712.Value.Process("Радиоэлектроника"));
			Assert.AreEqual("Cimljansk",         Russian.Gost16876712.Value.Process("Цимлянск"));
			Assert.AreEqual("Severobajkalʹsk",   Russian.Gost16876712.Value.Process("Северобайкальск"));
			Assert.AreEqual("Joshkar-Ola",       Russian.Gost16876712.Value.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossija",           Russian.Gost16876712.Value.Process("Россия"));
			Assert.AreEqual("Ygyatta",           Russian.Gost16876712.Value.Process("Ыгыатта"));
			Assert.AreEqual("Kuyrkʺjavr",        Russian.Gost16876712.Value.Process("Куыркъявр"));
			Assert.AreEqual("Ulan-Udeh",         Russian.Gost16876712.Value.Process("Улан-Удэ"));
			Assert.AreEqual("Tyaja",             Russian.Gost16876712.Value.Process("Тыайа"));
			Assert.AreEqual("Chapaevsk",         Russian.Gost16876712.Value.Process("Чапаевск"));
			Assert.AreEqual("Mejerovka",         Russian.Gost16876712.Value.Process("Мейеровка"));
			Assert.AreEqual("Barnaul",           Russian.Gost16876712.Value.Process("Барнаул"));
			Assert.AreEqual("Jakutsk",           Russian.Gost16876712.Value.Process("Якутск"));
			Assert.AreEqual("Yttyk-Kjolʹ",       Russian.Gost16876712.Value.Process("Ыттык-Кёль"));
			Assert.AreEqual("Ufa",               Russian.Gost16876712.Value.Process("Уфа"));
			Assert.AreEqual("rádostʹ",           Russian.Gost16876712.Value.Process("ра́дость"));
			Assert.AreEqual("radostʹ cvetok",    Russian.Gost16876712.Value.Process("радость цветок"));
		}
	}
}
