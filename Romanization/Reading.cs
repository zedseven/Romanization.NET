using System;

namespace Romanization
{
	/// <summary>
	/// A reading (pronunciation) of a character.
	/// </summary>
	/// <typeparam name="TType">The reading type enum to use, which contains all supported readings for a given language or system.<br />For example, <see cref="Japanese.KanjiReadings.ReadingTypes"/>.</typeparam>
	public class Reading<TType>
		where TType : Enum
	{
		/// <summary>
		/// The type of reading it is. For example, it could be <see cref="Japanese.KanjiReadings.ReadingTypes.Kunyomi"/>.
		/// </summary>
		public readonly TType Type;
		/// <summary>
		/// The reading itself - a romanized string representing how a character should be pronounced.
		/// </summary>
		public readonly string Value;

		internal Reading(TType type, string value)
		{
			Type = type;
			Value = value;
		}
	}
}
