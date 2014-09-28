using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Promises {
	public enum DeferredState {
		Pending,
		Resolved,
		Rejected
	}

	public class Deferred<T> : IDeferred<T> {
		public Deferred() {
			Promise = new Promise<T>();
		}
		public void Resolve() {
			Resolve(default(T));
		}
		public void Resolve(T value) {
			(Promise as Promise<T>).Resolve(value);
		}
		public void Reject() {
			Reject(default(Exception));
		}
		public void Reject(Exception exception) {
			(Promise as Promise<T>).Reject(exception);
		}
		public IPromise<T> Promise { get; private set; }
	}
}
