using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Promises {
	public class Promise<T> : IPromise<T> {
		readonly IList<Action<T>> onResolve;
		readonly IList<Action<Exception>> onReject;
		readonly object stateLock;
		T value = default(T);
		Exception exception = null;

		internal Promise() {
			onResolve = new List<Action<T>>();
			onReject = new List<Action<Exception>>();
			stateLock = new object();
			State = DeferredState.Pending;
		}

		internal void Resolve(T value) {
			if (State == DeferredState.Pending) {
				lock (stateLock) {
					if (State == DeferredState.Pending) {
						this.value = value;
						State = DeferredState.Resolved;
						foreach (var action in onResolve) {
							action(this.value);
						}
					}
				}
			}
		}

		internal void Reject(Exception exception) {
			if (State == DeferredState.Pending) {
				lock (stateLock) {
					if (State == DeferredState.Pending) {
						this.exception = exception;
						this.value = default(T);
						State = DeferredState.Rejected;
						foreach (var action in onReject) {
							action(this.exception);
						}
					}
				}
			}
		}

		public Promise<T> Then(Action<T> resolveCallback, Action<Exception> rejectCallback) {
			lock (stateLock) {
				if (State == DeferredState.Pending) {
					onResolve.Add(resolveCallback);
					onReject.Add(rejectCallback);
				} else {
					if (State == DeferredState.Resolved) {
						resolveCallback(this.value);
					} else {
						rejectCallback(this.exception);
					}
				}
				return this;
			}
		}
		public Promise<T> Then(Action<T> resolveCallback) {
			lock (stateLock) {
				if (State == DeferredState.Pending) {
					onResolve.Add(resolveCallback);
				} else {
					if (State == DeferredState.Resolved) {
						resolveCallback(this.value);
					} else {
						throw exception;
					}
				}
				return this;
			}
		}
		public void Wait(int timeout = 0) {
			var start = DateTime.Now;
			while (State == DeferredState.Pending) {
				Thread.Sleep(0);
				if (timeout > 0) {
					var ms = (DateTime.Now - start).TotalMilliseconds;
					if (ms > timeout) {
						Reject(new TimeoutException(timeout));
					}
				}
			}
			return;
		}
		public DeferredState State { get; private set; }
	}
}
