using System;
using System.Linq;

namespace Romanization
{
	/// <summary>
	/// A string of characters with all possible readings (pronunciations) for each character.
	/// </summary>
	/// <typeparam name="TType">The reading type enum to use, which contains all supported readings for a given language or system.<br />For example, <see cref="Japanese.KanjiReadings.ReadingTypes"/>.</typeparam>
	public class ReadingsString<TType>
		where TType : Enum
	{
		/// <summary>
		/// The characters of the string.<br />
		/// Each one stores both the character itself (not necessarily equivalent to a char, as some Hànzì characters are double-wide), and all known readings (pronunciations).
		/// </summary>
		public readonly ReadingCharacter<TType>[] Characters;

		internal ReadingsString(ReadingCharacter<TType>[] characters)
		{
			Characters = characters;
		}

		// TODO: Add additional ToString() implementations that do display reading types.
		/// <summary>
		/// Returns a string that displays all readings of each character.<br />
		/// For characters with 0 readings, they are displayed simply as themselves.<br />
		/// For characters with 1 reading, they are displayed as their only reading.<br />
		/// For characters with more than 1 reading, they are displayed as a space-delimited list of all readings in order, within square brackets.<br />
		/// Example: <c>"xiàndài [hàn tān][yǔ yù] [pín bīn][shuài lǜ lüe l̈ù] cí[diǎn tiǎn]."</c><br />
		/// Note that this does not display the source of each reading.
		/// </summary>
		/// <returns>A string with all known readings of each character.</returns>
		public override string ToString()
			=> Characters.Aggregate("", (current, character) => current + character.FlattenReadings());
	}
}
