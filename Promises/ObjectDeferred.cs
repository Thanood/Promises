using System;
using System.Collections.Generic;
using System.Text;

namespace Promises {
	public class ObjectDeferred : Deferred<object> {
		public ObjectDeferred() : base() { }
		public ObjectDeferred(Func<object> action) : this() {
			try {
				object result = action();
				Resolve(result);
			} catch (Exception e) {
				Reject(e);
			}
		}
	}
}
