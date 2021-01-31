using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	[TestClass]
	public class Bs29791958Tests
	{
		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                 Russian.Bs29791958.Value.Process(""));
			Assert.AreEqual("Élektrogorsk",     Russian.Bs29791958.Value.Process("Электрогорск"));
			Assert.AreEqual("Radioélektronika", Russian.Bs29791958.Value.Process("Радиоэлектроника"));
			Assert.AreEqual("Tsimlyansk",       Russian.Bs29791958.Value.Process("Цимлянск"));
			Assert.AreEqual("Severobaĭkalʹsk",  Russian.Bs29791958.Value.Process("Северобайкальск"));
			Assert.AreEqual("Ĭoshkar-Ola",      Russian.Bs29791958.Value.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossiya",          Russian.Bs29791958.Value.Process("Россия"));
			Assert.AreEqual("Ȳgȳatta",          Russian.Bs29791958.Value.Process("Ыгыатта"));
			Assert.AreEqual("Kuȳrkʺyavr",       Russian.Bs29791958.Value.Process("Куыркъявр"));
			Assert.AreEqual("Ulan-Udé",         Russian.Bs29791958.Value.Process("Улан-Удэ"));
			Assert.AreEqual("Tȳaĭa",            Russian.Bs29791958.Value.Process("Тыайа"));
			Assert.AreEqual("Chapaevsk",        Russian.Bs29791958.Value.Process("Чапаевск"));
			Assert.AreEqual("Meĭerovka",        Russian.Bs29791958.Value.Process("Мейеровка"));
			Assert.AreEqual("Barnaul",          Russian.Bs29791958.Value.Process("Барнаул"));
			Assert.AreEqual("Yakut-sk",         Russian.Bs29791958.Value.Process("Якутск"));
			Assert.AreEqual("Ȳttȳk-Këlʹ",       Russian.Bs29791958.Value.Process("Ыттык-Кёль"));
			Assert.AreEqual("Ufa",              Russian.Bs29791958.Value.Process("Уфа"));
			Assert.AreEqual("rádostʹ",          Russian.Bs29791958.Value.Process("ра́дость"));
			Assert.AreEqual("radostʹ tsvetok",  Russian.Bs29791958.Value.Process("радость цветок"));
		}
	}
}
