using System;
using System.Collections.Generic;
using System.Text;

namespace Promises {
	public class Deferred<TResult, TInput> {
		public delegate void Action<CInput>(Deferred<TResult, CInput> deferred, CInput obj);

		Action<TInput> method;
		IAsyncResult asyncResult;
		PromiseState state;
		Promise<TResult, TInput> promise;
		TResult result;
		Exception exception;

		public Deferred(Action<TInput> method, TInput obj) {
			this.method = method;
			state = PromiseState.Pending;
			promise = new Promise<TResult, TInput>(this);
			asyncResult = method.BeginInvoke(this, obj, MasterCallback, null);
		}

		public void Resolve(TResult result) {
			this.result = result;
			state = PromiseState.Resolved;
			promise.DequeueCallbacks(PromiseState.Resolved);
		}

		public void Reject(Exception error) {
			state = PromiseState.Rejected;
			exception = error;
			promise.DequeueCallbacks(PromiseState.Rejected);
		}

		public void Progress() {
			
		}

		public Promise<TResult, TInput> Promise() {
			return promise;
		}

		public PromiseState State { get { return state; } }

		internal TResult Result { get { return result; } }
		internal Exception Exception { get { return exception; } }

		void MasterCallback(IAsyncResult ar) {
			try {
				if (state == PromiseState.Pending) {
					method.EndInvoke(ar);
				}
			} catch (Exception error) {
				Reject(error);
			}
		}
	}
}
