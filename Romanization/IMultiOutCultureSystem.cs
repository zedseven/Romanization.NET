using System.Diagnostics.Contracts;
using System.Globalization;

namespace Romanization
{
	/// <summary>
	/// A system used to romanize a language where there are culture-specific ways to output the romanized text.<br />
	/// For example, numeral-parsing systems that can output numbers in Arabic format pay attention to comma/period use
	/// in the culture they're romanizing for. (North America uses a period for the decimal place, whereas Europe uses
	/// a comma)
	/// </summary>
	public interface IMultiOutCultureSystem : IRomanizationSystem
	{
		/// <summary>
		/// The system-specific function that romanizes text according to the system's rules, using the culture for the
		/// language, and <paramref name="romanizedCulture"/> for processing of romanized text.
		/// </summary>
		/// <param name="text">The text to romanize.</param>
		/// <param name="romanizedCulture">The culture to romanize to.</param>
		/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
		[Pure]
		public string ProcessToCulture(string text, CultureInfo romanizedCulture);
	}
}
