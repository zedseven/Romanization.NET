using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	[TestClass]
	public class AlaLcTests
	{
		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                 Russian.AlaLc.Value.Process(""));
			Assert.AreEqual("Ėlektrogorsk",     Russian.AlaLc.Value.Process("Электрогорск"));
			Assert.AreEqual("Radioėlektronika", Russian.AlaLc.Value.Process("Радиоэлектроника"));
			Assert.AreEqual("T͡simli͡ansk",       Russian.AlaLc.Value.Process("Цимлянск"));
			Assert.AreEqual("Severobaĭkalʹsk",  Russian.AlaLc.Value.Process("Северобайкальск"));
			Assert.AreEqual("Ĭoshkar-Ola",      Russian.AlaLc.Value.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossii͡a",          Russian.AlaLc.Value.Process("Россия"));
			Assert.AreEqual("Ygyatta",          Russian.AlaLc.Value.Process("Ыгыатта"));
			Assert.AreEqual("Kuyrkʺi͡avr",       Russian.AlaLc.Value.Process("Куыркъявр"));
			Assert.AreEqual("Ulan-Udė",         Russian.AlaLc.Value.Process("Улан-Удэ"));
			Assert.AreEqual("Tyaĭa",            Russian.AlaLc.Value.Process("Тыайа"));
			Assert.AreEqual("Chapaevsk",        Russian.AlaLc.Value.Process("Чапаевск"));
			Assert.AreEqual("Meĭerovka",        Russian.AlaLc.Value.Process("Мейеровка"));
			Assert.AreEqual("Barnaul",          Russian.AlaLc.Value.Process("Барнаул"));
			Assert.AreEqual("I͡akutsk",          Russian.AlaLc.Value.Process("Якутск"));
			Assert.AreEqual("Yttyk-Këlʹ",       Russian.AlaLc.Value.Process("Ыттык-Кёль"));
			Assert.AreEqual("Ufa",              Russian.AlaLc.Value.Process("Уфа"));
			Assert.AreEqual("rádostʹ",          Russian.AlaLc.Value.Process("ра́дость"));
			Assert.AreEqual("radostʹ t͡svetok",  Russian.AlaLc.Value.Process("радость цветок"));
		}
	}
}
