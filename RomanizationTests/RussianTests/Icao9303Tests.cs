using Microsoft.VisualStudio.TestTools.UnitTesting;
using Romanization;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace RomanizationTests.RussianTests
{
	[TestClass]
	public class Icao9303Tests
	{
		private readonly Russian.Icao9303 _system = new Russian.Icao9303();

		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                 _system.Process(""));
			Assert.AreEqual("Elektrogorsk",     _system.Process("Электрогорск"));
			Assert.AreEqual("Radioelektronika", _system.Process("Радиоэлектроника"));
			Assert.AreEqual("Tsimliansk",       _system.Process("Цимлянск"));
			Assert.AreEqual("Severobaikalsk",   _system.Process("Северобайкальск"));
			Assert.AreEqual("Ioshkar-Ola",      _system.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossiia",          _system.Process("Россия"));
			Assert.AreEqual("Ygyatta",          _system.Process("Ыгыатта"));
			Assert.AreEqual("Kuyrkieiavr",      _system.Process("Куыркъявр"));
			Assert.AreEqual("Ulan-Ude",         _system.Process("Улан-Удэ"));
			Assert.AreEqual("Tyaia",            _system.Process("Тыайа"));
			Assert.AreEqual("Chapaevsk",        _system.Process("Чапаевск"));
			Assert.AreEqual("Meierovka",        _system.Process("Мейеровка"));
			Assert.AreEqual("Barnaul",          _system.Process("Барнаул"));
			Assert.AreEqual("Iakutsk",          _system.Process("Якутск"));
			Assert.AreEqual("Yttyk-Kel",        _system.Process("Ыттык-Кёль"));
			Assert.AreEqual("Ufa",              _system.Process("Уфа"));
			Assert.AreEqual("radost",           _system.Process("ра́дость"));
			Assert.AreEqual("radost tsvetok",   _system.Process("радость цветок"));
		}
	}
}
