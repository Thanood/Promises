using System;
using System.Collections.Generic;
using System.Text;

namespace Promises {
	public class Callback<TResult> {
		Action<TResult> method;
		CallbackState state;
		internal Callback(Action<TResult> method, CallbackState state) {
			this.method = method;
			this.state = state;
		}

		internal void Call(TResult result) {
			method(result);
		}

		internal CallbackState State { get { return state; } }
	}
}
