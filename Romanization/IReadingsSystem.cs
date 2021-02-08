using System;
using System.Diagnostics.Contracts;

namespace Romanization
{
	/// <summary>
	/// A system used to romanize a language with multiple readings (pronunciations) per character.
	/// </summary>
	/// <typeparam name="TType">The types of reading the system uses.</typeparam>
	public interface IReadingsSystem<TType> : IRomanizationSystem
		where TType : Enum
	{
		/// <summary>
		/// The system-specific function that romanizes text in a language with multiple readings (pronunciations) per character.
		/// </summary>
		/// <param name="text">The text to romanize.</param>
		/// <returns>A <see cref="ReadingsString{ReadingTypes}"/> with all readings for each character in <paramref name="text"/>.</returns>
		[Pure]
		public ReadingsString<TType> ProcessWithReadings(string text);
	}
}
