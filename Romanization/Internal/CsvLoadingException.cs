using System;

namespace Romanization.Internal
{
	/// <summary>
	/// Represents an error that occurred during loading of a library CSV file.
	/// </summary>
	internal class CsvLoadingException : Exception
	{
		internal CsvLoadingException() {}
		internal CsvLoadingException(string message) : base(message) {}
		internal CsvLoadingException(string message, Exception inner) : base(message, inner) {}
	}
}
