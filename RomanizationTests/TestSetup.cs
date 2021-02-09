using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

namespace RomanizationTests
{
	/// <summary>
	/// The setup class, which sets the culture before tests begin so that test results are uniform.
	/// </summary>
	[TestClass]
	public class TestSetup
	{
		/// <summary>
		/// The initialization method.
		/// </summary>
		/// <param name="context">Unused.</param>
		[AssemblyInitialize]
		public static void TestInit(TestContext context)
		{
			Console.WriteLine("Test initialization starting...");
			CultureInfo.DefaultThreadCurrentCulture   = CultureInfo.GetCultureInfo("en-CA");
			CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-CA");
			CultureInfo.CurrentCulture                = CultureInfo.GetCultureInfo("en-CA");
			CultureInfo.CurrentUICulture              = CultureInfo.GetCultureInfo("en-CA");
			Console.WriteLine("Test initialization done.");
		}
	}
}
