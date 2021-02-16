using System;

namespace Romanization.Internal
{
	internal static class QualityOfLife
	{
		/// <summary>
		/// Exists to allow for small extra steps in a chained execution without breaking the format.
		/// </summary>
		/// <param name="value">The input value to pass to <paramref name="func"/>.</param>
		/// <param name="condition">The condition to determine whether or not to execute
		/// <paramref name="func"/>.</param>
		/// <param name="func">The function that should be executed only if <paramref name="condition"/> is
		/// <c>true</c>.</param>
		/// <typeparam name="T">The type of value in the chain.</typeparam>
		/// <returns>The result of <paramref name="func"/> if <paramref name="condition"/> is <c>true</c>, or
		/// <paramref name="value"/> otherwise.</returns>
		public static T ExecuteIf<T>(this T value, bool condition, Func<T, T> func)
			=> condition ? func(value) : value;

		/// <summary>
		/// Exists to allow for small extra steps in a chained execution without breaking the format.
		/// </summary>
		/// <param name="value">The input value to pass to <paramref name="trueFunc"/> and
		/// <paramref name="falseFunc"/>.</param>
		/// <param name="condition">The condition to determine whether to execute <paramref name="trueFunc"/> or
		/// <paramref name="falseFunc"/>.</param>
		/// <param name="trueFunc">The function that should be executed only if <paramref name="condition"/> is
		/// <c>true</c>.</param>
		/// <param name="falseFunc">The function that should be executed only if <paramref name="condition"/> is
		/// <c>false</c>.</param>
		/// <typeparam name="T">The type of value in the chain.</typeparam>
		/// <returns>The result of <paramref name="trueFunc"/> if <paramref name="condition"/> is <c>true</c>, or
		/// <paramref name="falseFunc"/> otherwise.</returns>
		public static T ExecuteIfElse<T>(this T value, bool condition, Func<T, T> trueFunc, Func<T, T> falseFunc)
			=> condition ? trueFunc(value) : falseFunc(value);

		/// <summary>
		/// Exists to allow for function calls in a chained execution without breaking the format.
		/// </summary>
		/// <param name="value">The input value to pass to <paramref name="func"/>.</param>
		/// <param name="func">The function to execute.</param>
		/// <typeparam name="T">The type of value in the chain.</typeparam>
		/// <returns>The result of <paramref name="func"/>.</returns>
		public static T Execute<T>(this T value, Func<T, T> func)
			=> func(value);
	}
}
