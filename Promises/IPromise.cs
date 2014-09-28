using System;
using System.Collections.Generic;
using System.Text;

namespace Infusion.Api.Promises {
	public interface IPromise {
		DeferredState State { get; }
		Promise Then(Action<object> resolveCallback, Action<object> rejectCallback);
		Promise Then(Action<object> resolveCallback);
		void Wait();
	}
}
