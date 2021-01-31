using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	[TestClass]
	public class BgnPcgnTests
	{
		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                 Russian.BgnPcgn.Value.Process(""));
			Assert.AreEqual("Elektrogorsk",     Russian.BgnPcgn.Value.Process("Электрогорск"));
			Assert.AreEqual("Radioelektronika", Russian.BgnPcgn.Value.Process("Радиоэлектроника"));
			Assert.AreEqual("Tsimlyansk",       Russian.BgnPcgn.Value.Process("Цимлянск"));
			Assert.AreEqual("Severobaykalʹsk",  Russian.BgnPcgn.Value.Process("Северобайкальск"));
			Assert.AreEqual("Yoshkar-Ola",      Russian.BgnPcgn.Value.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossiya",          Russian.BgnPcgn.Value.Process("Россия"));
			Assert.AreEqual("Ygy·atta",         Russian.BgnPcgn.Value.Process("Ыгыатта"));
			Assert.AreEqual("Ku·yrkʺyavr",      Russian.BgnPcgn.Value.Process("Куыркъявр"));
			Assert.AreEqual("Ulan-Ud·e",        Russian.BgnPcgn.Value.Process("Улан-Удэ"));
			Assert.AreEqual("Ty·ay·a",          Russian.BgnPcgn.Value.Process("Тыайа"));
			Assert.AreEqual("Chapayevsk",       Russian.BgnPcgn.Value.Process("Чапаевск"));
			Assert.AreEqual("Meyyerovka",       Russian.BgnPcgn.Value.Process("Мейеровка"));
			Assert.AreEqual("Barnaul",          Russian.BgnPcgn.Value.Process("Барнаул"));
			Assert.AreEqual("Yakut·sk",         Russian.BgnPcgn.Value.Process("Якутск"));
			Assert.AreEqual("Yttyk-Këlʹ",       Russian.BgnPcgn.Value.Process("Ыттык-Кёль"));
			Assert.AreEqual("Ufa",              Russian.BgnPcgn.Value.Process("Уфа"));
			Assert.AreEqual("rádostʹ",          Russian.BgnPcgn.Value.Process("ра́дость"));
			Assert.AreEqual("radostʹ tsvetok",  Russian.BgnPcgn.Value.Process("радость цветок"));
		}
	}
}
