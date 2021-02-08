using System;

namespace Romanization.Internal
{
	public class CaptureCountMismatchException : ArgumentException
	{
		internal CaptureCountMismatchException() { }
		internal CaptureCountMismatchException(string message) : base(message) { }
		internal CaptureCountMismatchException(string message, Exception inner) : base(message, inner) { }
		internal CaptureCountMismatchException(string message, string paramName) : base(message, paramName) { }
		internal CaptureCountMismatchException(string message, string paramName, Exception inner) : base(message, paramName, inner) { }
	}
}
