using Microsoft.VisualStudio.TestTools.UnitTesting;
using Romanization;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace RomanizationTests.RussianTests
{
	/// <summary>
	/// For testing the Russian road sign romanization system, <see cref="Russian.RoadSigns"/>.
	/// </summary>
	[TestClass]
	public class RoadSignsTests
	{
		private readonly Russian.RoadSigns _system = new Russian.RoadSigns();

		/// <summary>
		/// Aims to test basic processing.
		/// </summary>
		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                 _system.Process(""));
			Assert.AreEqual("Elektrogorsk",     _system.Process("Электрогорск"));
			Assert.AreEqual("Radioelektronika", _system.Process("Радиоэлектроника"));
			Assert.AreEqual("Tsimlyansk",       _system.Process("Цимлянск"));
			Assert.AreEqual("Severobaykalʹsk",  _system.Process("Северобайкальск"));
			Assert.AreEqual("Yoshkar-Ola",      _system.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossiya",          _system.Process("Россия"));
			Assert.AreEqual("Ygyatta",          _system.Process("Ыгыатта"));
			Assert.AreEqual("Kuyrkʹyavr",       _system.Process("Куыркъявр"));
			Assert.AreEqual("Ulan-Ude",         _system.Process("Улан-Удэ"));
			Assert.AreEqual("Tyaya",            _system.Process("Тыайа"));
			Assert.AreEqual("Chapayevsk",       _system.Process("Чапаевск"));
			Assert.AreEqual("Meyerovka",        _system.Process("Мейеровка"));
			Assert.AreEqual("Barnaul",          _system.Process("Барнаул"));
			Assert.AreEqual("Yakutsk",          _system.Process("Якутск"));
			Assert.AreEqual("Yttyk-Kyelʹ",      _system.Process("Ыттык-Кёль"));
			Assert.AreEqual("Ufa",              _system.Process("Уфа"));
			Assert.AreEqual("radostʹ",          _system.Process("ра́дость"));
			Assert.AreEqual("radostʹ tsvetok",  _system.Process("радость цветок"));
		}
	}
}
