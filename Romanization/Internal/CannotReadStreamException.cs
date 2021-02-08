using System;

namespace Romanization.Internal
{
	/// <summary>
	/// Represents the error when the stream provided to <see cref="CsvLoader.LoadCsvIntoDictionary{TKey,TVal}"/> cannot be read.
	/// </summary>
	internal class CannotReadStreamException : ArgumentException
	{
		internal CannotReadStreamException() {}
		internal CannotReadStreamException(string message) : base(message) {}
		internal CannotReadStreamException(string message, Exception inner) : base(message, inner) {}
		internal CannotReadStreamException(string message, string paramName) : base(message, paramName) { }
		internal CannotReadStreamException(string message, string paramName, Exception inner) : base(message, paramName, inner) { }
	}
}
