using System;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Romanization
{
	/// <summary>
	/// The type of a system - this is an important consideration depending on the purpose of romanizing the text.<br />
	/// For more information, visit:
	/// <a href='https://en.wikipedia.org/wiki/Romanization#Methods'>https://en.wikipedia.org/wiki/Romanization#Methods</a>
	/// </summary>
	public enum SystemType
	{
		/// <summary>
		/// A transliteration system, which is primarily concerned with maintaining an unambiguous map between
		/// the source language and the target script so it can be reconstructed from the romanized text later
		/// if need be.<br />
		/// This does not mean they aren't viable as romanization methods, but for ease of pronunciation a
		/// transcription system is the better choice.<br />
		/// Some languages only have transliteration systems.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Transliteration'>https://en.wikipedia.org/wiki/Transliteration</a>
		/// </summary>
		Transliteration,
		/// <summary>
		/// A phonemic transcription system, which intends to make the source text pronounceable to a reader of
		/// the target script.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Phonemic_orthography'>https://en.wikipedia.org/wiki/Phonemic_orthography</a>
		/// </summary>
		PhonemicTranscription,
		/// <summary>
		/// A phonetic transcription system, which attempts to record the sounds of the language, not necessarily
		/// making it easy to pronounce for a reader.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Phonetic_transcription'>https://en.wikipedia.org/wiki/Phonetic_transcription</a>
		/// </summary>
		PhoneticTranscription
	}

	/// <summary>
	/// The type of output numeral parsed numbers should be put into.<br />
	/// For instance, Greek numerals are traditionally romanized as Roman numerals except for when in
	/// official/government documents.
	/// </summary>
	public enum OutputNumeralType
	{
		/// <summary>
		/// The numeral system of much of the world.<br />
		/// Example: <c>267.5</c>
		/// </summary>
		Arabic,
		/// <summary>
		/// The numeral system of Rome, still often used today for names etc.<br />
		/// Example: <c>CCLXVIIS</c>
		/// </summary>
		Roman
	}

	/// <summary>
	/// A system used to romanize a language.
	/// </summary>
	public interface IRomanizationSystem
	{
		/// <summary>
		/// The type of the system - this is an important consideration depending on the purpose of romanizing the text.<br />
		/// For more information, visit:
		/// <a href='https://en.wikipedia.org/wiki/Romanization#Methods'>https://en.wikipedia.org/wiki/Romanization#Methods</a>
		/// </summary>
		public SystemType Type { get; }

		/// <summary>
		/// The system-specific function that romanizes text according to the system's rules.
		/// </summary>
		/// <param name="text">The text to romanize.</param>
		/// <returns>A romanized version of the text, leaving unrecognized characters untouched.</returns>
		[Pure]
		public string Process(string text);
	}

	/// <summary>
	/// A system used to romanize a language where there are multiple <see cref="CultureInfo"/> options to use for the
	/// native culture.
	/// </summary>
	public interface IMultiCulturalRomanizationSystem : IRomanizationSystem
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

	/// <summary>
	/// An extended version of <see cref="IMultiCulturalRomanizationSystem"/> that supports providing a culture to
	/// romanize to, as well as from. the reason this is separate from <see cref="IMultiCulturalRomanizationSystem"/>
	/// is because many systems don't have to do anything culture-specific when romanizing to a culture, but some do.
	/// </summary>
	public interface IExtendedMultiCulturalRomanizationSystem : IMultiCulturalRomanizationSystem
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

	/// <summary>
	/// Thrown when the culture passed to <see cref="IMultiCulturalRomanizationSystem.Process(string, CultureInfo)"/> is deemed irrelevant to the language.
	/// </summary>
	public class IrrelevantCultureException : ArgumentException
	{
		internal IrrelevantCultureException() { }
		internal IrrelevantCultureException(string message) : base(message) { }
		internal IrrelevantCultureException(string message, Exception inner) : base(message, inner) { }
		internal IrrelevantCultureException(string message, string paramName) : base(message, paramName) { }
		internal IrrelevantCultureException(string message, string paramName, Exception inner) : base(message, paramName, inner) { }
	}

	/// <summary>
	/// A system used to romanize a language with multiple readings (pronunciations) per character.
	/// </summary>
	/// <typeparam name="TType">The types of reading the system uses.</typeparam>
	public interface IReadingsRomanizationSystem<TType> : IRomanizationSystem
		where TType : Enum
	{
		/// <summary>
		/// The system-specific function that romanizes text in a language with multiple readings (pronunciations) per character.
		/// </summary>
		/// <param name="text">The text to romanize.</param>
		/// <returns>A <see cref="LanguageAgnostic.ReadingsString{ReadingTypes}"/> with all readings for each character in <paramref name="text"/>.</returns>
		[Pure]
		public LanguageAgnostic.ReadingsString<TType> ProcessWithReadings(string text);
	}

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
    		public NumeralValue<TLanguageUnits> Process(string text);

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

    	/// <summary>
	/// A numeral value with an associated unit if there is one.<br />
	/// Some numeral systems have special characters that indicate what the number is for, which is what the
	/// <see cref="Unit"/> field is for.
	/// </summary>
	public readonly struct NumeralValue<TLanguageUnits> : IEquatable<NumeralValue<TLanguageUnits>>, IComparable<NumeralValue<TLanguageUnits>>
		where TLanguageUnits : struct
	{
		/// <summary>
		/// The numeric value.
		/// </summary>
		public readonly decimal Value;
		/// <summary>
		/// The unit of the value, if known. Some numeral systems have symbols built in that convey the unit
		/// the number is of.
		/// </summary>
		public readonly TLanguageUnits? Unit;

		public NumeralValue(decimal value, TLanguageUnits? unit = null)
		{
			Value = value;
			Unit  = unit;
		}

		public bool Equals(NumeralValue<TLanguageUnits> other)
			=> Value == other.Value && Nullable.Equals(Unit, other.Unit);

		public override bool Equals(object obj)
			=> obj is NumeralValue<TLanguageUnits> other && Equals(other);

		public override int GetHashCode()
			=> HashCode.Combine(Value, Unit);

		public static bool operator ==(NumeralValue<TLanguageUnits> left, NumeralValue<TLanguageUnits> right)
			=> left.Equals(right);

		public static bool operator !=(NumeralValue<TLanguageUnits> left, NumeralValue<TLanguageUnits> right)
			=> !left.Equals(right);

		public int CompareTo(NumeralValue<TLanguageUnits> other)
			=> Value.CompareTo(other.Value);

		public override string ToString()
			=> Unit.HasValue ? $"{Value} {Unit}" : Value.ToString(CultureInfo.InvariantCulture);

		internal NumeralValue ToUnitlessNumeralValue()
			=> new NumeralValue(Value);
	}

	/// <summary>
	/// A numeral value with an associated unit if there is one.<br />
	/// Some numeral systems have special characters that indicate what the number is for, which is what the
	/// <see cref="Unit"/> field is for.
	/// </summary>
	public readonly struct NumeralValue : IEquatable<NumeralValue>, IComparable<NumeralValue>
	{
		/// <summary>
		/// The numeric value.
		/// </summary>
		public readonly decimal Value;

		public NumeralValue(decimal value)
			=> Value = value;

		public bool Equals(NumeralValue other)
			=> Value == other.Value;

		public override bool Equals(object obj)
			=> obj is NumeralValue other && Equals(other);

		public override int GetHashCode()
			=> Value.GetHashCode();

		public static bool operator ==(NumeralValue left, NumeralValue right)
			=> left.Equals(right);

		public static bool operator !=(NumeralValue left, NumeralValue right)
			=> !left.Equals(right);

		public int CompareTo(NumeralValue other)
			=> Value.CompareTo(other.Value);

		public override string ToString()
			=> Value.ToString(CultureInfo.InvariantCulture);
	}
}
