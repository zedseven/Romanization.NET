using System;
using System.Globalization;

namespace Romanization
{
	/// <summary>
	/// A numeral value with no associated unit.
	/// </summary>
	public readonly struct NumeralValue : IEquatable<NumeralValue>, IComparable<NumeralValue>
	{
		/// <summary>
		/// The numeric value.
		/// </summary>
		public readonly decimal Value;

		/// <summary>
		/// Constructs a new <see cref="NumeralValue{TLanguageUnits}"/> with a <paramref name="value"/>.
		/// </summary>
		/// <param name="value">The value.</param>
		public NumeralValue(decimal value)
			=> Value = value;

		/// <inheritdoc />
		public bool Equals(NumeralValue other)
			=> Value == other.Value;

		/// <inheritdoc />
		public override bool Equals(object? obj)
			=> obj is NumeralValue other && Equals(other);

		/// <inheritdoc />
		public override int GetHashCode()
			=> Value.GetHashCode();

		/// <summary>
		/// Checks for equality between two <see cref="NumeralValue"/> instances.
		/// </summary>
		/// <param name="left">The left side of the operator.</param>
		/// <param name="right">The right side of the operator.</param>
		/// <returns>Whether <paramref name="left"/> and <paramref name="right"/> are value-equal.</returns>
		public static bool operator ==(NumeralValue left, NumeralValue right)
			=> left.Equals(right);

		/// <summary>
		/// Checks for inequality between two <see cref="NumeralValue"/> instances.
		/// </summary>
		/// <param name="left">The left side of the operator.</param>
		/// <param name="right">The right side of the operator.</param>
		/// <returns>Whether <paramref name="left"/> and <paramref name="right"/> are not value-equal.</returns>
		public static bool operator !=(NumeralValue left, NumeralValue right)
			=> !left.Equals(right);

		/// <inheritdoc />
		public int CompareTo(NumeralValue other)
			=> Value.CompareTo(other.Value);

		/// <inheritdoc />
		public override string ToString()
			=> Value.ToString(CultureInfo.InvariantCulture);
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

		/// <summary>
		/// Constructs a new <see cref="NumeralValue{TLanguageUnits}"/> with a <paramref name="value"/> and <paramref name="unit"/>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="unit">The unit the value is in, if any.</param>
		public NumeralValue(decimal value, TLanguageUnits? unit = null)
		{
			Value = value;
			Unit  = unit;
		}

		/// <inheritdoc />
		public bool Equals(NumeralValue<TLanguageUnits> other)
			=> Value == other.Value && Nullable.Equals(Unit, other.Unit);

		/// <inheritdoc />
		public override bool Equals(object? obj)
			=> obj is NumeralValue<TLanguageUnits> other && Equals(other);

		/// <inheritdoc />
		public override int GetHashCode()
			=> HashCode.Combine(Value, Unit);

		/// <summary>
		/// Checks for equality between two <see cref="NumeralValue{TLanguageUnits}"/> instances.
		/// </summary>
		/// <param name="left">The left side of the operator.</param>
		/// <param name="right">The right side of the operator.</param>
		/// <returns>Whether <paramref name="left"/> and <paramref name="right"/> are value-equal.</returns>
		public static bool operator ==(NumeralValue<TLanguageUnits> left, NumeralValue<TLanguageUnits> right)
			=> left.Equals(right);

		/// <summary>
		/// Checks for inequality between two <see cref="NumeralValue{TLanguageUnits}"/> instances.
		/// </summary>
		/// <param name="left">The left side of the operator.</param>
		/// <param name="right">The right side of the operator.</param>
		/// <returns>Whether <paramref name="left"/> and <paramref name="right"/> are not value-equal.</returns>
		public static bool operator !=(NumeralValue<TLanguageUnits> left, NumeralValue<TLanguageUnits> right)
			=> !left.Equals(right);

		/// <inheritdoc />
		public int CompareTo(NumeralValue<TLanguageUnits> other)
			=> Value.CompareTo(other.Value);

		/// <inheritdoc />
		public override string ToString()
			=> Unit.HasValue ? $"{Value} {Unit}" : Value.ToString(CultureInfo.InvariantCulture);

		/// <summary>
		/// Converts this <see cref="NumeralValue{TLanguageUnits}"/> instance into a new <see cref="NumeralValue"/>,
		/// without a unit.
		/// </summary>
		/// <returns>A new <see cref="NumeralValue"/>, without support for a unit.</returns>
		internal NumeralValue ToUnitlessNumeralValue()
			=> new NumeralValue(Value);
	}
}
