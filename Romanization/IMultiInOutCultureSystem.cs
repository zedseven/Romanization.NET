using System.Diagnostics.Contracts;
using System.Globalization;

namespace Romanization
{
	/// <summary>
	/// An extended version of <see cref="IMultiInCultureSystem"/> that supports providing a culture to
	/// romanize to, as well as from. the reason this is separate from <see cref="IMultiInCultureSystem"/>
	/// is because many systems don't have to do anything culture-specific when romanizing to a culture, but some do.
	/// </summary>
	public interface IMultiInOutCultureSystem
		: IMultiInCultureSystem, IMultiOutCultureSystem
	{
		/// <summary>
		/// The system-specific function that romanizes text according to the system's rules, using the provided
		/// <paramref name="nativeCulture"/> for processing of native text, and <paramref name="romanizedCulture"/> for
		/// processing of romanized text.<br />
		/// Note that use of non-relevant cultures WILL lead to unexpected behaviour. If you don't know which cultures to use, use the standard
		/// <see cref="IRomanizationSystem.Process"/> instead.
		/// </summary>
		/// <param name="text">The text to romanize.</param>
		/// <param name="nativeCulture">The culture to romanize from.</param>
		/// <param name="romanizedCulture">The culture to romanize to.</param>
		/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
		[Pure]
		public string Process(string text, CultureInfo nativeCulture, CultureInfo romanizedCulture);
	}
}
