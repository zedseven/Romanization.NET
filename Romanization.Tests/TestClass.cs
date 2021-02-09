using System.Globalization;

namespace Romanization.Tests
{
	public abstract class TestClass
	{
		/// <summary>
		/// Performs test initialization so cultures can be normalized across test platforms.
		/// </summary>
		protected TestClass()
		{
			CultureInfo culture = CultureInfo.GetCultureInfo("en-CA");

			CultureInfo.DefaultThreadCurrentCulture   = culture;
			CultureInfo.DefaultThreadCurrentUICulture = culture;
			CultureInfo.CurrentCulture                = culture;
			CultureInfo.CurrentUICulture              = culture;
		}
	}
}
