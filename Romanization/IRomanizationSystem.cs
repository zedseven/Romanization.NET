using System.Diagnostics.Contracts;

namespace Romanization
{
	/// <summary>
	/// A system used to romanize a language.
	/// </summary>
	public interface IRomanizationSystem
	{
		/// <summary>
		/// The system-specific function that romanizes text according to the system's rules.
		/// </summary>
		/// <param name="text">The text to romanize.</param>
		/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
		[Pure]
		public string Process(string text);
	}
}
