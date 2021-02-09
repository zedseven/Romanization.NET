using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;

namespace Romanization.Internal
{
	internal static class NumeralRenderer
	{
		/// <summary>
		/// Converts an integer into Roman numerals.<br />
		/// <paramref name="num"/> must be non-negative, and only numbers up to <c>3999</c> are expected - beyond that
		/// there is no standard, and roman numerals shouldn't be necessary.
		/// </summary>
		/// <param name="num">The number to convert.</param>
		/// <returns>The value of <paramref name="num"/>, encoded in Roman numerals.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="num"/> is negative.</exception>
		[Pure]
		public static string ToRomanNumerals(this decimal num)
		{
			if (num < 0)
				throw new ArgumentOutOfRangeException(nameof(num), num, "Must be a non-negative value");

			// Zero doesn't have an actual value in the system, but N was commonly used in it's place and this is better
			// than returning an empty string
			if (num == 0)
				return "N";

			// Value maps
			const decimal closenessMargin = (decimal) 1/20736; // 1 order of magnitude (12^4) smaller than the smallest value
			Dictionary<int, char> valueRepresentations = new()
			{
				{ 1000, 'M' },
				{  500, 'D' },
				{  100, 'C' },
				{   50, 'L' },
				{   10, 'X' },
				{    5, 'V' },
				{    1, 'I' }
			};
			Dictionary<int, int> subtractiveSteps = new()
			{
				{ 1000, 100 },
				{  500, 100 },
				{  100,  10 },
				{   50,  10 },
				{   10,   1 },
				{    5,   1 }
			};
			Dictionary<decimal, char> fractionRepresentations = new()
			{
				{ (decimal)   6/12, 'S' },
				{ (decimal)   5/12, '⁙' },
				{ (decimal)   4/12, '∷' },
				{ (decimal)   3/12, '∴' },
				{ (decimal)   2/12, ':' },
				{ (decimal)   1/12, '·' },
				{ (decimal)   1/24, 'Є' },
				{ (decimal)   1/48, 'Ɔ' },
				{ (decimal)   1/72, 'Ƨ' },
				{ (decimal)  1/288, '℈' },
				{ (decimal) 1/1728, '»' }
			};
			const string dots = "·:∴∷⁙";

			StringBuilder result = new(5);
			int numToAdd;

			// Whole numbers
			foreach ((int value, char representation) in valueRepresentations)
			{
				if ((numToAdd = (int) (num / value)) >= 1)
				{
					num -= numToAdd * value;
					for (int i = 0; i < numToAdd; i++)
						result.Append(representation);
				}

				// Check if the remaining value is close to another instance of the current numeral since
				// it can be written more concisely with subtractive notation (ie. IV instead of IIII)
				if (!subtractiveSteps.TryGetValue(value, out int step) || (int) ((float) (num + step) / value) < 1)
					continue;

				num -= value - step;
				result.Append(valueRepresentations[step]);
				result.Append(representation);
			}

			// Fractions, to the nearest value the Roman system supports
			if (num == 0)
				return result.ToString();
			foreach ((decimal value, char representation) in fractionRepresentations)
			{
				if ((numToAdd = (int) ((num + closenessMargin) / value)) < 1)
					continue;
				num -= numToAdd * value;
				for (int i = 0; i < numToAdd; i++)
					result.Append(representation);
			}
			// Place dots after symbols, as that looks cleaner and is closer to Є·
			int length = result.Length;
			for (int i = 0; i < length; i++)
			{
				if (!dots.Contains(result[i]))
					continue;
				result.Append(result[i]);
				result.Remove(i, 1);
				i--;
				length--;
			}

			return result.ToString();
		}

		public static string ToRomanNumerals(this int num)
			=> ((decimal) num).ToRomanNumerals();

		/// <summary>
		/// Converts an integer into Arabic numerals for display.
		/// </summary>
		/// <param name="num">The number to convert.</param>
		/// <param name="culture">The culture to use for display.</param>
		/// <param name="decimalPlaces">The number of decimal places to round to.</param>
		/// <returns>The value of <paramref name="num"/>, encoded in Arabic numerals.</returns>
		public static string ToArabicNumerals(this decimal num, CultureInfo culture, int decimalPlaces = 2)
		{
			num = decimal.Round(num, decimalPlaces, MidpointRounding.AwayFromZero);
			return num.ToString(culture);
		}
	}
}
