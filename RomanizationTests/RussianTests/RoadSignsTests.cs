using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	[TestClass]
	public class RoadSignsTests
	{
		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                 Russian.RoadSigns.Value.Process(""));
			Assert.AreEqual("Elektrogorsk",     Russian.RoadSigns.Value.Process("Электрогорск"));
			Assert.AreEqual("Radioelektronika", Russian.RoadSigns.Value.Process("Радиоэлектроника"));
			Assert.AreEqual("Tsimlyansk",       Russian.RoadSigns.Value.Process("Цимлянск"));
			Assert.AreEqual("Severobaykalʹsk",  Russian.RoadSigns.Value.Process("Северобайкальск"));
			Assert.AreEqual("Yoshkar-Ola",      Russian.RoadSigns.Value.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossiya",          Russian.RoadSigns.Value.Process("Россия"));
			Assert.AreEqual("Ygyatta",          Russian.RoadSigns.Value.Process("Ыгыатта"));
			Assert.AreEqual("Kuyrkʹyavr",       Russian.RoadSigns.Value.Process("Куыркъявр"));
			Assert.AreEqual("Ulan-Ude",         Russian.RoadSigns.Value.Process("Улан-Удэ"));
			Assert.AreEqual("Tyaya",            Russian.RoadSigns.Value.Process("Тыайа"));
			Assert.AreEqual("Chapayevsk",       Russian.RoadSigns.Value.Process("Чапаевск"));
			Assert.AreEqual("Meyerovka",        Russian.RoadSigns.Value.Process("Мейеровка"));
			Assert.AreEqual("Barnaul",          Russian.RoadSigns.Value.Process("Барнаул"));
			Assert.AreEqual("Yakutsk",          Russian.RoadSigns.Value.Process("Якутск"));
			Assert.AreEqual("Yttyk-Kyelʹ",      Russian.RoadSigns.Value.Process("Ыттык-Кёль"));
			Assert.AreEqual("Ufa",              Russian.RoadSigns.Value.Process("Уфа"));
			Assert.AreEqual("rádostʹ",          Russian.RoadSigns.Value.Process("ра́дость"));
			Assert.AreEqual("radostʹ tsvetok",  Russian.RoadSigns.Value.Process("радость цветок"));
		}
	}
}
