using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	[TestClass]
	public class Icao9303Tests
	{
		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                 Russian.Icao9303.Value.Process(""));
			Assert.AreEqual("Elektrogorsk",     Russian.Icao9303.Value.Process("Электрогорск"));
			Assert.AreEqual("Radioelektronika", Russian.Icao9303.Value.Process("Радиоэлектроника"));
			Assert.AreEqual("Tsimliansk",       Russian.Icao9303.Value.Process("Цимлянск"));
			Assert.AreEqual("Severobaikalsk",   Russian.Icao9303.Value.Process("Северобайкальск"));
			Assert.AreEqual("Ioshkar-Ola",      Russian.Icao9303.Value.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossiia",          Russian.Icao9303.Value.Process("Россия"));
			Assert.AreEqual("Ygyatta",          Russian.Icao9303.Value.Process("Ыгыатта"));
			Assert.AreEqual("Kuyrkieiavr",      Russian.Icao9303.Value.Process("Куыркъявр"));
			Assert.AreEqual("Ulan-Ude",         Russian.Icao9303.Value.Process("Улан-Удэ"));
			Assert.AreEqual("Tyaia",            Russian.Icao9303.Value.Process("Тыайа"));
			Assert.AreEqual("Chapaevsk",        Russian.Icao9303.Value.Process("Чапаевск"));
			Assert.AreEqual("Meierovka",        Russian.Icao9303.Value.Process("Мейеровка"));
			Assert.AreEqual("Barnaul",          Russian.Icao9303.Value.Process("Барнаул"));
			Assert.AreEqual("Iakutsk",          Russian.Icao9303.Value.Process("Якутск"));
			Assert.AreEqual("Yttyk-Kel",        Russian.Icao9303.Value.Process("Ыттык-Кёль"));
			Assert.AreEqual("Ufa",              Russian.Icao9303.Value.Process("Уфа"));
			Assert.AreEqual("rádost",           Russian.Icao9303.Value.Process("ра́дость"));
			Assert.AreEqual("radost tsvetok",   Russian.Icao9303.Value.Process("радость цветок"));
		}
	}
}
