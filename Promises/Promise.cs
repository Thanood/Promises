using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Infusion.Api.Promises {
	public class Promise : IPromise {
		readonly IList<Action<object>> onResolve;
		readonly IList<Action<object>> onReject;
		readonly object stateLock;
		object value = null;

		internal Promise() {
			onResolve = new List<Action<object>>();
			onReject = new List<Action<object>>();
			stateLock = new object();
			State = DeferredState.Pending;
		}
		internal void Finish(DeferredState state, object value) {
			lock (stateLock) {
				this.value = value;
				if (state == DeferredState.Resolved) {
					foreach (var action in onResolve) {
						action(this.value);
					}
				} else {
					foreach (var action in onReject) {
						action(this.value);
					}
				}
				State = state;
			}
		}

		public Promise Then(Action<object> resolveCallback, Action<object> rejectCallback) {
			lock (stateLock) {
				if (State == DeferredState.Pending) {
					onResolve.Add(resolveCallback);
					onReject.Add(rejectCallback);
				} else {
					if (State == DeferredState.Resolved) {
						resolveCallback(this.value);
					} else {
						rejectCallback(this.value);
					}
				}
				return this;
			}
		}
		public Promise Then(Action<object> resolveCallback) {
			lock (stateLock) {
				if (State == DeferredState.Pending) {
					onResolve.Add(resolveCallback);
				} else {
					if (State == DeferredState.Resolved) {
						resolveCallback(this.value);
					}
				}
				return this;
			}
		}
		public void Wait() {
			while (State == DeferredState.Pending) {
				Thread.Sleep(0);
			}
			return;
		}
		public DeferredState State { get; private set; }
	}
}
