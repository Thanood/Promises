using System;
using System.Collections.Generic;
using System.Text;

namespace Promises {
	public class TimeoutException : Exception {
		public TimeoutException(int timeout) : this(timeout, string.Empty) { }
		public TimeoutException(int timeout, string message) : this(timeout, message, null) { }

		public TimeoutException(int timeout, string message, Exception innerException) : base(message, innerException) {
				Timeout = timeout;
		}

		public int Timeout { get; private set; }
	}
}
