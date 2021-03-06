﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Promises;

namespace ConsoleTester {
	class Program {
		static void Main(string[] args) {
			var promise = new Deferred<string, string>(MyMethod, "my promise").Promise().Done(SuccessCallback).Fail(FailCallback).Always(AlwaysCallback);
			var promise2 = new Deferred<string, string>(MyMethod2, "my promise").Promise().Done(SuccessCallback).Always(AlwaysCallback).Fail(FailCallback);

			Console.WriteLine("waiting a second ...");
			Thread.Sleep(1000);
			promise2.Always((result) => { Console.WriteLine("deferred always"); });
			Console.ReadLine();
		}

		private static void MyMethod2(Deferred<string, string> deferred, string obj) {
			Console.WriteLine(obj);
			Thread.Sleep(500);
			Console.WriteLine("thread awake");
			deferred.Resolve(obj);
		}

		private static void MyMethod(Deferred<string, string> deferred, string obj) {
			Console.WriteLine(obj);
			throw new Exception("real test exception");
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
