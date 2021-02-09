using Xunit;

// ReSharper disable StringLiteralTypo

namespace Romanization.Tests.GreekTests.AncientTests
{
	/// <summary>
	/// For testing the <see cref="Greek.Ancient.GreekNumerals"/> system.
	/// </summary>
	public class GreekNumeralsTests : TestClass
	{
		private readonly Greek.Ancient.GreekNumerals _system = new();

		/// <summary>
		/// Aims to test whole numbers in the antiquated format.
		/// </summary>
		[Fact]
		public void WholeNumbersAncientTest()
		{
			Assert.Equal(new NumeralValue(666), _system.Process("χ̅ξ̅ϛ̅"));
		}

		/// <summary>
		/// Aims to test fractional numbers in the antiquated format.
		/// </summary>
		[Fact]
		public void FractionsAncientTest()
		{
			Assert.Equal(new NumeralValue(103 + (decimal) 1/3),   _system.Process("ρ̅γ̅ γʹ"));
			Assert.Equal(new NumeralValue(43 + (decimal) 5/6),    _system.Process("μ̅γ̅∠ʹγʹ"));
			Assert.Equal(new NumeralValue(9996 + (decimal) 5/12), _system.Process("͵̅θ̅ϡ̅ϟ̅ϛ̅δʹϛʹ"));
		}

		/// <summary>
		/// Aims to test whole numbers in the modern format.
		/// </summary>
		[Fact]
		public void WholeNumbersModernTest()
		{
			Assert.Equal(new NumeralValue(4),  _system.Process("δ’"));
			Assert.Equal(new NumeralValue(12), _system.Process("ιβʹ"));
		}

		/// <summary>
		/// Aims to test fractional numbers in the modern format.
		/// </summary>
		[Fact]
		public void FractionsModernTest()
		{
			Assert.Equal(new NumeralValue(12 + (decimal) 1/2 + (decimal) 1/3 + (decimal) 1/12),
				_system.Process("ι̅β̅∠ʹγʹιβʹ"));
		}
	}
}
