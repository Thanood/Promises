using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Promises;

namespace ConsoleTester {
	class Program {
		static void Main(string[] args) {
			var promise = new Deferred<string, string>(MyMethod, "my promise").Promise().Done(SuccessCallback).Fail(FailCallback).Always(AlwaysCallback);
			var promise2 = new Deferred<string, string>(MyMethod2, "my promise").Promise().Done(SuccessCallback).Always(AlwaysCallback).Fail(FailCallback);
			Console.ReadLine();
		}

		private static void MyMethod2(Deferred<string, string> deferred, string obj) {
			Console.WriteLine(obj);
			deferred.Resolve(obj);
		}

		private static void MyMethod(Deferred<string, string> deferred, string obj) {
			Console.WriteLine(obj);
			deferred.Reject(new Exception("test exception"));
		}

		private static void SuccessCallback(string result) {
			Console.WriteLine("success: {0}", result);
		}

		private static void FailCallback(Exception result) {
			Console.WriteLine("fail: {0}", result);
		}

		public static void AlwaysCallback(string result) {
			Console.WriteLine("always: {0}", result);
		}
	}
}
