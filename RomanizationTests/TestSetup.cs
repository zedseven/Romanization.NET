using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

namespace RomanizationTests
{
    [TestClass]
    public class TestSetup
    {
        [AssemblyInitialize]
        public static void TestInit(TestContext context)
        {
            Console.WriteLine("Test initialization starting...");
            CultureInfo.CurrentCulture   = CultureInfo.GetCultureInfo("en-CA");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-CA");
            Console.WriteLine("Test initialization done.");
        }
    }
}
