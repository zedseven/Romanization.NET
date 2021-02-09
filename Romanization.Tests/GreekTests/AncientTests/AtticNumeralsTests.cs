using Xunit;

// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.GreekTests.AncientTests
{
	/// <summary>
	/// For testing the <see cref="Greek.Ancient.AtticNumerals"/> system.
	/// </summary>
	public class AtticNumeralsTests : TestClass
	{
		private readonly Greek.Ancient.AtticNumerals _system = new();

		/// <summary>
		/// Aims to test the unitless values.
		/// </summary>
		[Fact]
		public void UnitlessTest()
		{
			Assert.Equal(MakeVal(    6), _system.Process("ΠΙ"));
			Assert.Equal(MakeVal(    6), _system.Process("ΙΠ"));
			Assert.Equal(MakeVal(   49), _system.Process("ΔΔΔΔΠΙΙΙΙ"));
			Assert.Equal(MakeVal(  239), _system.Process("ΗΗΔΔΔΠΙΙΙΙ"));
			Assert.Equal(MakeVal( 1982), _system.Process("Χ𐅅ΗΗΗΗ𐅄ΔΔΔΙΙ"));
			Assert.Equal(MakeVal( 2001), _system.Process("ΧΧΙ"));
			Assert.Equal(MakeVal(62708), _system.Process("𐅇ΜΧΧ𐅅ΗΗΠΙΙΙ"));
		}

		/// <summary>
		/// Aims to test numerals with the unit encoded in one of the numeric values. (ie. 𐅈 is 5 talents)
		/// </summary>
		[Fact]
		public void EncodedUnitsTest()
		{
			Assert.Equal(MakeVal(1000, Greek.Ancient.Units.Staters), _system.Process("𐅔"));
			Assert.Equal(MakeVal(  60, Greek.Ancient.Units.Talents), _system.Process("𐅊Δ"));
			Assert.Equal(MakeVal( 121, Greek.Ancient.Units.Mnas),    _system.Process("Η𐅗𐅗Ι"));

			// Checking that 6 Obols = 1 Drachma
			Assert.Equal(
				_system.Process("𐅂"),   // 1 Drachma
				_system.Process("𐅿𐅽")); // 4 + 2 Obols
		}

		/// <summary>
		/// Aims to test numerals with the unit symbol attached separately similar to how a dollar sign ($) is
		/// prepended to a monetary amount today. (ie. 𐅺 is the dedicated talent symbol, conveying that the value
		/// is in talents)
		/// </summary>
		[Fact]
		public void SeparateUnitsTest()
		{
			Assert.Equal(MakeVal( 239, Greek.Ancient.Units.Talents), _system.Process("ΗΗΔΔΔΠΙΙΙΙ𐅺"));
			Assert.Equal(MakeVal(1000, Greek.Ancient.Units.Staters), _system.Process("𐅔"));
			Assert.Equal(MakeVal(   0, Greek.Ancient.Units.Years),   _system.Process("𐆌"));
			Assert.Equal(MakeVal(   0, Greek.Ancient.Units.Years),   _system.Process("𐆊𐆌"));
			Assert.Equal(MakeVal( 121, Greek.Ancient.Units.Mnas),    _system.Process("Η𐅗𐅗Ι"));
		}

		private static NumeralValue<Greek.Ancient.Units> MakeVal(decimal value,
			Greek.Ancient.Units? unit = null)
			=> new(value, unit);
	}
}
