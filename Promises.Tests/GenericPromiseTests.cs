using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Promises;
using Xunit;

namespace Promises.Tests {
	public class GenericPromiseTests {
		[Fact]
		public void CanResolve() {
			Deferred<string> dfd = new Deferred<string>();
			var promise = dfd.Promise;
			Assert.Equal<DeferredState>(DeferredState.Pending, promise.State);
			dfd.Resolve();
			Assert.Equal<DeferredState>(DeferredState.Resolved, promise.State);
		}

		[Fact]
		public void CanResolveInAThread() {
			Deferred<string> dfd = new Deferred<string>();
			var promise = dfd.Promise;
			Assert.Equal<DeferredState>(DeferredState.Pending, promise.State);

			ThreadPool.QueueUserWorkItem(d => {
				Thread.Sleep(10);
				(d as Deferred<string>).Resolve();
			}, dfd);

			promise.Wait();
			Assert.Equal<DeferredState>(DeferredState.Resolved, promise.State);
		}

		[Fact]
		public void CanResolveWithValue() {
			Deferred<string> dfd = new Deferred<string>();
			var promise = dfd.Promise;
			Assert.Equal<DeferredState>(DeferredState.Pending, promise.State);
			dfd.Resolve("value");
			Assert.Equal<DeferredState>(DeferredState.Resolved, promise.State);
			promise.Then(value => { Assert.Equal(value, "value"); });
		}

		[Fact]
		public void CanReject() {
			Deferred<string> dfd = new Deferred<string>();
			var promise = dfd.Promise;
			Assert.Equal<DeferredState>(DeferredState.Pending, promise.State);
			dfd.Reject();
			Assert.Equal<DeferredState>(DeferredState.Rejected, promise.State);
		}

		[Fact]
		public void CanRejectWithValue() {
			Deferred<string> dfd = new Deferred<string>();
			var promise = dfd.Promise;
			Assert.Equal<DeferredState>(DeferredState.Pending, promise.State);
			dfd.Reject(new Exception("test exception"));
			Assert.Equal<DeferredState>(DeferredState.Rejected, promise.State);
			promise.Then(null, ex => { Assert.Equal("test exception", ex.Message); });
		}

		[Fact]
		public void CanRejectInAThread() {
			Deferred<string> dfd = new Deferred<string>();
			var promise = dfd.Promise;
			Assert.Equal<DeferredState>(DeferredState.Pending, promise.State);

			ThreadPool.QueueUserWorkItem(d => {
				Thread.Sleep(10);
				(d as Deferred<string>).Reject();
			}, dfd);

			promise.Wait();
			Assert.Equal<DeferredState>(DeferredState.Rejected, promise.State);
		}
	}
}
