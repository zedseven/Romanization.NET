using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	[TestClass]
	public class ScholarlyTests
	{
		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                 Russian.Scholarly.Value.Process(""));
			Assert.AreEqual("Èlektrogorsk",     Russian.Scholarly.Value.Process("Электрогорск"));
			Assert.AreEqual("Radioèlektronika", Russian.Scholarly.Value.Process("Радиоэлектроника"));
			Assert.AreEqual("Cimljansk",        Russian.Scholarly.Value.Process("Цимлянск"));
			Assert.AreEqual("Severobajkalʹsk",  Russian.Scholarly.Value.Process("Северобайкальск"));
			Assert.AreEqual("Joškar-Ola",       Russian.Scholarly.Value.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossija",          Russian.Scholarly.Value.Process("Россия"));
			Assert.AreEqual("Ygyatta",          Russian.Scholarly.Value.Process("Ыгыатта"));
			Assert.AreEqual("Kuyrkʺjavr",       Russian.Scholarly.Value.Process("Куыркъявр"));
			Assert.AreEqual("Ulan-Udè",         Russian.Scholarly.Value.Process("Улан-Удэ"));
			Assert.AreEqual("Tyaja",            Russian.Scholarly.Value.Process("Тыайа"));
			Assert.AreEqual("Čapaevsk",         Russian.Scholarly.Value.Process("Чапаевск"));
			Assert.AreEqual("Mejerovka",        Russian.Scholarly.Value.Process("Мейеровка"));
			Assert.AreEqual("Barnaul",          Russian.Scholarly.Value.Process("Барнаул"));
			Assert.AreEqual("Jakutsk",          Russian.Scholarly.Value.Process("Якутск"));
			Assert.AreEqual("Yttyk-Këlʹ",       Russian.Scholarly.Value.Process("Ыттык-Кёль"));
			Assert.AreEqual("Ufa",              Russian.Scholarly.Value.Process("Уфа"));
			Assert.AreEqual("rádostʹ",          Russian.Scholarly.Value.Process("ра́дость"));
			Assert.AreEqual("radostʹ cvetok",   Russian.Scholarly.Value.Process("радость цветок"));
		}
	}
}
