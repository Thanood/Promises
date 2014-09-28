using System;
using System.Collections.Generic;
using System.Text;

namespace Promises {
	public interface IDeferred<T> {
		void Resolve();
		void Resolve(T value);
		void Reject();
		void Reject(Exception exception);
		IPromise<T> Promise { get; }
	}
}
