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
		/// The culture to romanize from.
		/// </summary>
		public CultureInfo NativeCulture { get; }
	}
}
