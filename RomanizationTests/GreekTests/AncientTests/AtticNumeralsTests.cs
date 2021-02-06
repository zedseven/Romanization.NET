using Microsoft.VisualStudio.TestTools.UnitTesting;
using Romanization;

// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

namespace RomanizationTests.GreekTests.AncientTests
{
	/// <summary>
	/// For testing the <see cref="Greek.Ancient.AtticNumerals"/> system.
	/// </summary>
	[TestClass]
	public class AtticNumeralsTests
	{
		private readonly Greek.Ancient.AtticNumerals _system = new Greek.Ancient.AtticNumerals();

		/// <summary>
		/// Aims to test the unitless values.
		/// </summary>
		[TestMethod]
		public void UnitlessTest()
		{
			Assert.AreEqual(MakeVal(    6), _system.Process("ΠΙ"));
			Assert.AreEqual(MakeVal(    6), _system.Process("ΙΠ"));
			Assert.AreEqual(MakeVal(   49), _system.Process("ΔΔΔΔΠΙΙΙΙ"));
			Assert.AreEqual(MakeVal(  239), _system.Process("ΗΗΔΔΔΠΙΙΙΙ"));
			Assert.AreEqual(MakeVal( 1982), _system.Process("Χ𐅅ΗΗΗΗ𐅄ΔΔΔΙΙ"));
			Assert.AreEqual(MakeVal( 2001), _system.Process("ΧΧΙ"));
			Assert.AreEqual(MakeVal(62708), _system.Process("𐅇ΜΧΧ𐅅ΗΗΠΙΙΙ"));
		}

		/// <summary>
		/// Aims to test numerals with the unit encoded in one of the numeric values. (ie. 𐅈 is 5 talents)
		/// </summary>
		[TestMethod]
		public void EncodedUnitsTest()
		{
			Assert.AreEqual(MakeVal(1000, Greek.Ancient.Units.Staters), _system.Process("𐅔"));
			Assert.AreEqual(MakeVal(  60, Greek.Ancient.Units.Talents), _system.Process("𐅊Δ"));
			Assert.AreEqual(MakeVal( 121, Greek.Ancient.Units.Mnas),    _system.Process("Η𐅗𐅗Ι"));
			
			// Checking that 6 Obols = 1 Drachma
			Assert.AreEqual(
				_system.Process("𐅂"),  // 1 Drachma
				_system.Process("𐅿𐅽")); // 4 + 2 Obols
		}

		/// <summary>
		/// Aims to test numerals with the unit symbol attached separately similar to how a dollar sign ($) is
		/// prepended to a monetary amount today. (ie. 𐅺 is the dedicated talent symbol, conveying that the value
		/// is in talents)
		/// </summary>
		[TestMethod]
		public void SeparateUnitsTest()
		{
			Assert.AreEqual(MakeVal( 239, Greek.Ancient.Units.Talents), _system.Process("ΗΗΔΔΔΠΙΙΙΙ𐅺"));
			Assert.AreEqual(MakeVal(1000, Greek.Ancient.Units.Staters), _system.Process("𐅔"));
			Assert.AreEqual(MakeVal(   0, Greek.Ancient.Units.Years),   _system.Process("𐆌"));
			Assert.AreEqual(MakeVal(   0, Greek.Ancient.Units.Years),   _system.Process("𐆊𐆌"));
			Assert.AreEqual(MakeVal( 121, Greek.Ancient.Units.Mnas),    _system.Process("Η𐅗𐅗Ι"));
		}

		private NumeralValue<Greek.Ancient.Units> MakeVal(decimal value,
			Greek.Ancient.Units? unit = null)
			=> new NumeralValue<Greek.Ancient.Units>(value, unit);
	}
}
