using System;
using System.Globalization;

namespace Romanization.Internal
{
	internal static class CulturalOperations
	{
		/// <summary>
		/// Runs <paramref name="func"/> with a specified <paramref name="culture"/>, then reverts the thread culture
		/// back to what it was before.
		/// </summary>
		/// <param name="culture">The culture to run with.</param>
		/// <param name="func">The function to run. Note that this takes a culture as an argument: this is the culture
		/// the thread was using before.</param>
		/// <typeparam name="TRes">The type of the result of running <paramref name="func"/>.</typeparam>
		/// <returns>The result of <paramref name="func"/>.</returns>
		public static TRes RunWithCulture<TRes>(CultureInfo culture, Func<CultureInfo, TRes> func)
		{
			CultureInfo previousCulture = CultureInfo.CurrentCulture;
			if (Equals(previousCulture, culture))
				return func(previousCulture);

			TRes res;
			try
			{
				CultureInfo.CurrentCulture = culture;
				res = func(previousCulture);
			}
			finally
			{
				CultureInfo.CurrentCulture = previousCulture;
			}
			return res;
		}

		/// <summary>
		/// Runs <paramref name="func"/>, then reverts the thread culture back to what it was before.
		/// </summary>
		/// <param name="culture">The culture to run with.</param>
		/// <param name="func">The function to run.</param>
		/// <typeparam name="TRes">The type of the result of running <paramref name="func"/>.</typeparam>
		/// <returns>The result of <paramref name="func"/>.</returns>
		public static TRes RunWithCulture<TRes>(CultureInfo culture, Func<TRes> func)
		{
			CultureInfo previousCulture = CultureInfo.CurrentCulture;
			if (Equals(previousCulture, culture))
				return func();

			TRes res;
			try
			{
				CultureInfo.CurrentCulture = culture;
				res = func();
			}
			finally
			{
				CultureInfo.CurrentCulture = previousCulture;
			}
			return res;
		}

		/// <summary>
		/// Runs <paramref name="action"/>, then reverts the thread culture back to what it was before.
		/// </summary>
		/// <param name="culture">The culture to run with.</param>
		/// <param name="action">The action to run.</param>
		public static void RunWithCulture(CultureInfo culture, Action<CultureInfo> action)
		{
			CultureInfo previousCulture = CultureInfo.CurrentCulture;
			if (Equals(previousCulture, culture))
			{
				action(previousCulture);
				return;
			}

			try
			{
				CultureInfo.CurrentCulture = culture;
				action(previousCulture);
			}
			finally
			{
				CultureInfo.CurrentCulture = previousCulture;
			}
		}

		/// <summary>
		/// Runs <paramref name="action"/>, then reverts the thread culture back to what it was before.
		/// </summary>
		/// <param name="culture">The culture to run with.</param>
		/// <param name="action">The action to run.</param>
		public static void RunWithCulture(CultureInfo culture, Action action)
		{
			CultureInfo previousCulture = CultureInfo.CurrentCulture;
			if (Equals(previousCulture, culture))
			{
				action();
				return;
			}

			try
			{
				CultureInfo.CurrentCulture = culture;
				action();
			}
			finally
			{
				CultureInfo.CurrentCulture = previousCulture;
			}
		}
	}
}
