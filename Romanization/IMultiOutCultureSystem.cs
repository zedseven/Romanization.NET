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
		/// The culture to romanize to.
		/// </summary>
		public CultureInfo RomanizedCulture { get; }
	}
}
