using System;
using System.Collections.Generic;
using System.Text;

namespace Infusion.Api.Promises {
	public interface IDeferred {
		void Resolve();
		void Resolve(object value);
		void Reject();
		void Reject(object value);
		IPromise Promise { get; }
	}
}
