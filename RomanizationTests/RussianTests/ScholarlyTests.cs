using Microsoft.VisualStudio.TestTools.UnitTesting;
using Romanization;

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace RomanizationTests.RussianTests
{
	/// <summary>
	/// For testing the Russian scientific/scholarly romanization system, <see cref="Russian.Scholarly"/>.
	/// </summary>
	[TestClass]
	public class ScholarlyTests
	{
		private readonly Russian.Scholarly _system = new Russian.Scholarly();

		/// <summary>
		/// Aims to test basic processing.
		/// </summary>
		[TestMethod]
		public void ProcessTest()
		{
			Assert.AreEqual("",                 _system.Process(""));
			Assert.AreEqual("Èlektrogorsk",     _system.Process("Электрогорск"));
			Assert.AreEqual("Radioèlektronika", _system.Process("Радиоэлектроника"));
			Assert.AreEqual("Cimljansk",        _system.Process("Цимлянск"));
			Assert.AreEqual("Severobajkalʹsk",  _system.Process("Северобайкальск"));
			Assert.AreEqual("Joškar-Ola",       _system.Process("Йошкар-Ола"));
			Assert.AreEqual("Rossija",          _system.Process("Россия"));
			Assert.AreEqual("Ygyatta",          _system.Process("Ыгыатта"));
			Assert.AreEqual("Kuyrkʺjavr",       _system.Process("Куыркъявр"));
			Assert.AreEqual("Ulan-Udè",         _system.Process("Улан-Удэ"));
			Assert.AreEqual("Tyaja",            _system.Process("Тыайа"));
			Assert.AreEqual("Čapaevsk",         _system.Process("Чапаевск"));
			Assert.AreEqual("Mejerovka",        _system.Process("Мейеровка"));
			Assert.AreEqual("Barnaul",          _system.Process("Барнаул"));
			Assert.AreEqual("Jakutsk",          _system.Process("Якутск"));
			Assert.AreEqual("Yttyk-Këlʹ",       _system.Process("Ыттык-Кёль"));
			Assert.AreEqual("Ufa",              _system.Process("Уфа"));
			Assert.AreEqual("radostʹ",          _system.Process("ра́дость"));
			Assert.AreEqual("radostʹ cvetok",   _system.Process("радость цветок"));
		}
	}
}
