using System;
using System.Collections.Generic;
using System.Text;

namespace Promises {
	public interface IPromise<T> {
		DeferredState State { get; }
		Promise<T> Then(Action<T> resolveCallback, Action<Exception> rejectCallback);
		Promise<T> Then(Action<T> resolveCallback);
		void Wait(int timeout = 0);
	}
}
