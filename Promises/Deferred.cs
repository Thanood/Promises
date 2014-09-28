using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Infusion.Api.Promises {
	public enum DeferredState {
		Pending,
		Resolved,
		Rejected
	}

	public class Deferred : IDeferred {
		public Deferred() {
			Promise = new Promise();
		}
		public void Resolve() {
			Resolve(null);
		}
		public void Resolve(object value) {
			(Promise as Promise).Finish(DeferredState.Resolved, value);
		}
		public void Reject() {
			Reject(null);
		}
		public void Reject(object value) {
			(Promise as Promise).Finish(DeferredState.Rejected, value);
		}
		public IPromise Promise { get; private set; }
	}
}
