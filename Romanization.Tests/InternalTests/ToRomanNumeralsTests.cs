using System;
using Romanization.Internal;
using Xunit;

// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.InternalTests
{
	/// <summary>
	/// For testing the <see cref="NumeralRenderer.ToRomanNumerals(decimal)"/> function.
	/// </summary>
	public class ToRomanNumeralsTests : TestClass
	{
		/// <summary>
		/// Aims to test simple numbers where only additive notation is required.
		/// </summary>
		[Fact]
		public void AdditiveTest()
		{
			Assert.Equal("I",            1.ToRomanNumerals());
			Assert.Equal("VIII",         8.ToRomanNumerals());
			Assert.Equal("CCLXVII",    267.ToRomanNumerals());
			Assert.Equal("DCCLXXVII",  777.ToRomanNumerals());
			Assert.Equal("MLXVI",     1066.ToRomanNumerals());
			Assert.Equal("MMXXI",     2021.ToRomanNumerals());
		}

		/// <summary>
		/// Aims to test numerals requiring the use of subtractive notation for concise display.
		/// </summary>
		[Fact]
		public void SubtractiveTest()
		{
			Assert.Equal("LXIX",        69.ToRomanNumerals());
			Assert.Equal("MIX",       1009.ToRomanNumerals());
			Assert.Equal("MCMXVIII",  1918.ToRomanNumerals());
			Assert.Equal("MCMXXVI",   1926.ToRomanNumerals());
			Assert.Equal("MCMLIV",    1954.ToRomanNumerals());
			Assert.Equal("MMCDXXI",   2421.ToRomanNumerals());
			Assert.Equal("MMMCMXCIX", 3999.ToRomanNumerals());
		}

		/// <summary>
		/// Aims to test fractional numbers where the value should be exactly displayable with Roman values.
		/// </summary>
		[Fact]
		public void FractionalExactTest()
		{
			Assert.Equal("IS",                                          ((decimal) 1.5).ToRomanNumerals());
			Assert.Equal("I∴",                                         ((decimal) 1.25).ToRomanNumerals());
			Assert.Equal("Є·",                                          ((decimal) 1/8).ToRomanNumerals()); // Tests dot order
			Assert.Equal("XIIS⁙", (12 + (decimal) 1/2 + (decimal) 1/3 + (decimal) 1/12).ToRomanNumerals());
			Assert.Equal("MMDCXCIXSƧ·",        (2699 + (decimal) 7/12 + (decimal) 1/72).ToRomanNumerals());
			Assert.Equal("CCLXVIIS",                                  ((decimal) 267.5).ToRomanNumerals());
		}

		/// <summary>
		/// Aims to test fractional numbers where the value isn't exactly displayable with Roman values.
		/// </summary>
		[Fact]
		public void FractionalApproximationTest()
		{
			Assert.Equal("IƧ℈»»»»", ((decimal) 1.02).ToRomanNumerals());
		}

		/// <summary>
		/// Aims to test zero values.
		/// </summary>
		[Fact]
		public void ZeroTest()
		{
			Assert.Equal("N", ((decimal) 0).ToRomanNumerals());
			Assert.Equal("N",             0.ToRomanNumerals());
		}

		/// <summary>
		/// Aims to test contract enforcement.
		/// </summary>
		[Fact]
		public void ContractTest()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => (-5).ToRomanNumerals());
		}
	}
}
