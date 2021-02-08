using System;
using System.Globalization;

namespace Romanization
{
	/// <summary>
	/// Thrown when the culture passed to <see cref="IMultiInCultureSystem.Process(string, CultureInfo)"/> is deemed irrelevant to the language.
	/// </summary>
	public class IrrelevantCultureException : ArgumentException
	{
		internal IrrelevantCultureException() { }
		internal IrrelevantCultureException(string message) : base(message) { }
		internal IrrelevantCultureException(string message, Exception inner) : base(message, inner) { }
		internal IrrelevantCultureException(string message, string paramName) : base(message, paramName) { }
		internal IrrelevantCultureException(string message, string paramName, Exception inner) : base(message, paramName, inner) { }
	}
}
