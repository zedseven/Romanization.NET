using System;
using System.Collections.Generic;
using System.Linq;

namespace Romanization
{
	/// <summary>
	/// A character with all possible readings (pronunciations).
	/// </summary>
	/// <typeparam name="TType">The reading type enum to use, which contains all supported readings for a given language or system.<br />For example, <see cref="Japanese.KanjiReadings.ReadingTypes"/>.</typeparam>
	public class ReadingCharacter<TType>
		where TType : Enum
	{
		/// <summary>
		/// The actual character value.<br />Note that this is not necessarily one char in length - some Hànzì characters go outside the Basic Multilingual Plane (BMP), and as such take up 32 bits (two 16-bit chars).
		/// </summary>
		public readonly string Character;
		/// <summary>
		/// The collection of known readings for the character, in order as specified in the function used to generate this object.
		/// </summary>
		public readonly Reading<TType>[] Readings;

		internal ReadingCharacter(string character, IEnumerable<Reading<TType>> readings)
		{
			Character = character;
			Readings = readings.ToArray();
		}

		/// <summary>
		/// Returns a string that represents the current object.<br />
		/// The format is: <c>'&lt;char&gt;' [&lt;readings&gt;]</c>
		/// </summary>
		/// <returns>A string with the character and all known readings.</returns>
		public override string ToString()
			=> $"'{Character}' {FlattenReadings()}";

		/// <summary>
		/// Returns a string starting and ending with square brackets, containing all readings in the order they appear in <see cref="Readings"/>.<br />
		/// If the character has no known readings, the character itself is returned instead.<br />
		/// Example: <c>[shuài lǜ lüe l̈ù]</c><br />
		/// Note this does not output the source of each reading.
		/// </summary>
		/// <returns>A string representation of all readings of the character, or the character itself if there are none.</returns>
		public string FlattenReadings()
		{
			string[] readings = Readings.Select(r => r.Value).Distinct().ToArray();
			if (readings.Length > 1)
				return $"[{string.Join(" ", readings)}]";
			return readings.Length == 1 ? readings[0] : Character;
		}
	}
}
