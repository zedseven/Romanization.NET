using Microsoft.VisualStudio.TestTools.UnitTesting;
using Romanization;

// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

namespace RomanizationTests.GreekTests.AncientTests
{
	/// <summary>
	/// For testing the <see cref="Greek.Ancient.GreekNumerals"/> system.
	/// </summary>
	[TestClass]
	public class GreekNumeralsTests
	{
		private readonly Greek.Ancient.GreekNumerals _system = new Greek.Ancient.GreekNumerals();

		/// <summary>
		/// Aims to test whole numbers in the antiquated format.
		/// </summary>
		[TestMethod]
		public void WholeNumbersAncientTest()
		{
			Assert.AreEqual(new NumeralValue(666), _system.Process("χ̅ξ̅ϛ̅"));
		}

		/// <summary>
		/// Aims to test fractional numbers in the antiquated format.
		/// </summary>
		[TestMethod]
		public void FractionsAncientTest()
		{
			Assert.AreEqual(new NumeralValue(103 + (decimal) 1/3),   _system.Process("ρ̅γ̅ γʹ"));
			Assert.AreEqual(new NumeralValue(43 + (decimal) 5/6),    _system.Process("μ̅γ̅∠ʹγʹ"));
			Assert.AreEqual(new NumeralValue(9996 + (decimal) 5/12), _system.Process("͵̅θ̅ϡ̅ϟ̅ϛ̅δʹϛʹ"));
		}

		/// <summary>
		/// Aims to test whole numbers in the modern format.
		/// </summary>
		[TestMethod]
		public void WholeNumbersModernTest()
		{
			Assert.AreEqual(new NumeralValue(4),  _system.Process("δ’"));
			Assert.AreEqual(new NumeralValue(12), _system.Process("ιβʹ"));
		}

		/// <summary>
		/// Aims to test fractional numbers in the modern format.
		/// </summary>
		[TestMethod]
		public void FractionsModernTest()
		{
			Assert.AreEqual(new NumeralValue(12 + (decimal) 1/2 + (decimal) 1/3 + (decimal) 1/12),
				_system.Process("ι̅β̅∠ʹγʹιβʹ"));
		}
	}
}
