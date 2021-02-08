using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Romanization.Internal;

// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

namespace RomanizationTests.InternalTests
{
	/// <summary>
	/// For testing the <see cref="NumeralRenderer.ToRomanNumerals(decimal)"/> function.
	/// </summary>
	[TestClass]
	public class ToRomanNumeralsTests
	{
		/// <summary>
		/// Aims to test simple numbers where only additive notation is required.
		/// </summary>
		[TestMethod]
		public void AdditiveTest()
		{
			Assert.AreEqual("I",            1.ToRomanNumerals());
			Assert.AreEqual("VIII",         8.ToRomanNumerals());
			Assert.AreEqual("CCLXVII",    267.ToRomanNumerals());
			Assert.AreEqual("DCCLXXVII",  777.ToRomanNumerals());
			Assert.AreEqual("MLXVI",     1066.ToRomanNumerals());
			Assert.AreEqual("MMXXI",     2021.ToRomanNumerals());
		}

		/// <summary>
		/// Aims to test numerals requiring the use of subtractive notation for concise display.
		/// </summary>
		[TestMethod]
		public void SubtractiveTest()
		{
			Assert.AreEqual("LXIX",        69.ToRomanNumerals());
			Assert.AreEqual("MIX",       1009.ToRomanNumerals());
			Assert.AreEqual("MCMXVIII",  1918.ToRomanNumerals());
			Assert.AreEqual("MCMXXVI",   1926.ToRomanNumerals());
			Assert.AreEqual("MCMLIV",    1954.ToRomanNumerals());
			Assert.AreEqual("MMCDXXI",   2421.ToRomanNumerals());
			Assert.AreEqual("MMMCMXCIX", 3999.ToRomanNumerals());
		}

		/// <summary>
		/// Aims to test fractional numbers where the value should be exactly displayable with Roman values.
		/// </summary>
		[TestMethod]
		public void FractionalExactTest()
		{
			Assert.AreEqual("IS",                                          ((decimal) 1.5).ToRomanNumerals());
			Assert.AreEqual("I∴",                                         ((decimal) 1.25).ToRomanNumerals());
			Assert.AreEqual("Є·",                                          ((decimal) 1/8).ToRomanNumerals()); // Tests dot order
			Assert.AreEqual("XIIS⁙", (12 + (decimal) 1/2 + (decimal) 1/3 + (decimal) 1/12).ToRomanNumerals());
			Assert.AreEqual("MMDCXCIXSƧ·",        (2699 + (decimal) 7/12 + (decimal) 1/72).ToRomanNumerals());
			Assert.AreEqual("CCLXVIIS",                                  ((decimal) 267.5).ToRomanNumerals());
		}

		/// <summary>
		/// Aims to test fractional numbers where the value isn't exactly displayable with Roman values.
		/// </summary>
		[TestMethod]
		public void FractionalApproximationTest()
		{
			Assert.AreEqual("IƧ℈»»»»", ((decimal) 1.02).ToRomanNumerals());
		}

		/// <summary>
		/// Aims to test zero values.
		/// </summary>
		[TestMethod]
		public void ZeroTest()
		{
			Assert.AreEqual("N", ((decimal) 0).ToRomanNumerals());
			Assert.AreEqual("N",             0.ToRomanNumerals());
		}

		/// <summary>
		/// Aims to test contract enforcement.
		/// </summary>
		[TestMethod]
		public void ContractTest()
		{
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => (-5).ToRomanNumerals());
		}
	}
}
