using System;
using System.Diagnostics.Contracts;

namespace Romanization
{
	/// <summary>
	/// Not actual romanization. This is a system exclusively for parsing other numeral systems
	/// (Greek numerals, for instance).
	/// </summary>
	public interface INumeralParsingSystem
	{
		/// <summary>
		/// The system-specific function that determines the numeric value of a numeral using the system.
		/// </summary>
		/// <param name="text">The numeral text to parse.</param>
		/// <returns>A numeric value representing the value of <paramref name="text"/>, with a unit if one
		/// could be found.</returns>
		[Pure]
		public NumeralValue Process(string text);

		/// <summary>
		/// Processes the given text, parsing any numerals with <see cref="Process"/> then replacing them with
		/// the result of <paramref name="numeralProcessor"/> run on their values.
		/// </summary>
		/// <param name="text">The text to process.</param>
		/// <param name="numeralProcessor">The function that converts parsed numeric value into a string.</param>
		/// <returns>A new copy of <paramref name="text"/> with all numerals found within replaced.</returns>
		[Pure]
		public string ProcessNumeralsInText(string text, Func<NumeralValue, string> numeralProcessor);
	}

	/// <summary>
	/// Not actual romanization. This is a system exclusively for parsing other numeral systems
	/// (Greek numerals, for instance).
	/// </summary>
	public interface INumeralParsingSystem<TLanguageUnits> : INumeralParsingSystem
		where TLanguageUnits : struct
	{
		/// <summary>
		/// The system-specific function that determines the numeric value of a numeral using the system.
		/// </summary>
		/// <param name="text">The numeral text to parse.</param>
		/// <returns>A numeric value representing the value of <paramref name="text"/>, with a unit if one
		/// could be found.</returns>
		[Pure]
		public new NumeralValue<TLanguageUnits> Process(string text);

		/// <summary>
		/// Processes the given text, parsing any numerals with <see cref="Process"/> then replacing them with
		/// the result of <paramref name="numeralProcessor"/> run on their values.
		/// </summary>
		/// <param name="text">The text to process.</param>
		/// <param name="numeralProcessor">The function that converts parsed numeric value (potentially with units) into
		/// a string.</param>
		/// <returns>A new copy of <paramref name="text"/> with all numerals found within replaced.</returns>
		[Pure]
		public string ProcessNumeralsInText(string text, Func<NumeralValue<TLanguageUnits>, string> numeralProcessor);
	}
}
