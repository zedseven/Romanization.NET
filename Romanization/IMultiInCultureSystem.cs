using System.Diagnostics.Contracts;
using System.Globalization;

namespace Romanization
{
	/// <summary>
	/// A system used to romanize a language where there are multiple <see cref="CultureInfo"/> options to use for the
	/// native culture.
	/// </summary>
	public interface IMultiInCultureSystem : IRomanizationSystem
	{
		/// <summary>
		/// The default culture this system will go with for romanization if <see cref="IRomanizationSystem.Process"/> is used.
		/// </summary>
		public CultureInfo DefaultCulture { get; }

		/// <summary>
		/// The system-specific function that romanizes text according to the system's rules, using the provided
		/// <paramref name="nativeCulture"/> for processing of native text, and <see cref="CultureInfo.CurrentCulture"/>
		/// for processing of romanized text.<br />
		/// Note that use of non-relevant cultures WILL lead to unexpected behaviour. If you don't know which culture
		/// to use, use the standard <see cref="IRomanizationSystem.Process"/> instead.
		/// </summary>
		/// <param name="text">The text to romanize.</param>
		/// <param name="nativeCulture">The culture to romanize from.</param>
		/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
		[Pure]
		public string Process(string text, CultureInfo nativeCulture);
	}
}
