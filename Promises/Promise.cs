using System;
using System.Collections.Generic;
using System.Text;

namespace Promises {
	public enum PromiseState {
		Pending,
		Resolved,
		Rejected
	}

	internal enum CallbackState {
		Always,
		Progress,
		Resolved,
		Rejected
	}

	public class Promise<TResult, TInput> {
		Deferred<TResult, TInput> deferred;
		Queue<Callback<TResult>> callbacks;
		Queue<Callback<Exception>> exceptions;

		internal Promise(Deferred<TResult, TInput> deferred) {
			this.deferred = deferred;
			callbacks = new Queue<Callback<TResult>>();
			exceptions = new Queue<Callback<Exception>>();
		}

		public Promise<TResult, TInput> Done(Action<TResult> callback) {
			if (deferred.State == PromiseState.Pending) {
				var cb = new Callback<TResult>(callback, CallbackState.Resolved);
				callbacks.Enqueue(cb);
			} else {
				if (deferred.State == PromiseState.Resolved) {
					callback(deferred.Result);
				}
			}
			return this;
		}

		public Promise<TResult, TInput> Fail(Action<Exception> callback) {
			if (deferred.State == PromiseState.Pending) {
				var cb = new Callback<Exception>(callback, CallbackState.Rejected);
				exceptions.Enqueue(cb);
			} else {
				if (deferred.State == PromiseState.Rejected) {
					callback(deferred.Exception);
				}
			}
			return this;
		}

		public Promise<TResult, TInput> Always(Action<TResult> callback) {
			if (deferred.State == PromiseState.Pending) {
				var cb = new Callback<TResult>(callback, CallbackState.Always);
				callbacks.Enqueue(cb);
			} else {
				callback(deferred.Result);
			}
			return this;
		}

		internal void DequeueCallbacks(PromiseState promiseState) {
			while (callbacks.Count > 0) {
				var callback = callbacks.Dequeue();
				if (callback.State == CallbackState.Always) {
					callback.Call(deferred.Result);
				} else if (promiseState == PromiseState.Resolved && callback.State == CallbackState.Resolved) {
					callback.Call(deferred.Result);
				} //else if (promiseState == PromiseState.Rejected && callback.State == CallbackState.Rejected) {
				//	callback.Call(deferred.Result); // TODO: return error
				//}
			}
			while (exceptions.Count > 0) {
				var callback = exceptions.Dequeue();
				if (callback.State == CallbackState.Always) {
					callback.Call(deferred.Exception);
				} else if (promiseState == PromiseState.Rejected && callback.State == CallbackState.Rejected) {
					callback.Call(deferred.Exception);
				}
			}
		}
	}
}
