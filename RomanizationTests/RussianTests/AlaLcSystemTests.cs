using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.RussianTests
{
	[TestClass]
	public class AlaLcSystemTests
	{
		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                 Russian.AlaLc.Value.Process(""));
			Assert.AreEqual("Ėlektrogorsk",     Russian.AlaLc.Value.Process("Электрогорск"));
			Assert.AreEqual("Radioėlektronika", Russian.AlaLc.Value.Process("Радиоэлектроника"));
			Assert.AreEqual("T͡Simli͡ansk",       Russian.AlaLc.Value.Process("Цимлянск"));
			Assert.AreEqual("Severobaĭkalʹsk",  Russian.AlaLc.Value.Process("Северобайкальск"));
			Assert.AreEqual("Ĭoshkar-Ola",      Russian.AlaLc.Value.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossii͡a",          Russian.AlaLc.Value.Process("Россия"));
		}
	}
}