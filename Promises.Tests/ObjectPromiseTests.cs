using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Promises;
using Xunit;

namespace Promises.Tests {
	public class ObjectPromiseTests {
		[Fact]
		public void CanResolve() {
			ObjectDeferred dfd = new ObjectDeferred();
			var promise = dfd.Promise;
			Assert.Equal<DeferredState>(DeferredState.Pending, promise.State);
			dfd.Resolve();
			Assert.Equal<DeferredState>(DeferredState.Resolved, promise.State);
		}

		[Fact]
		public void CanResolveInAThread() {
			ObjectDeferred dfd = new ObjectDeferred();
			var promise = dfd.Promise;
			Assert.Equal<DeferredState>(DeferredState.Pending, promise.State);

			ThreadPool.QueueUserWorkItem(d => {
				Thread.Sleep(10);
				(d as ObjectDeferred).Resolve();
			}, dfd);

			promise.Wait();
			Assert.Equal<DeferredState>(DeferredState.Resolved, promise.State);
		}

		[Fact]
		public void CanResolveWithValue() {
			ObjectDeferred dfd = new ObjectDeferred();
			var promise = dfd.Promise;
			Assert.Equal<DeferredState>(DeferredState.Pending, promise.State);
			dfd.Resolve(true);
			Assert.Equal<DeferredState>(DeferredState.Resolved, promise.State);
			promise.Then(o => { Assert.Equal(true, (bool)o); });
		}

		[Fact]
		public void CanReject() {
			ObjectDeferred dfd = new ObjectDeferred();
			var promise = dfd.Promise;
			Assert.Equal<DeferredState>(DeferredState.Pending, promise.State);
			dfd.Reject();
			Assert.Equal<DeferredState>(DeferredState.Rejected, promise.State);
		}

		[Fact]
		public void CanRejectWithValue() {
			ObjectDeferred dfd = new ObjectDeferred();
			var promise = dfd.Promise;
			Assert.Equal<DeferredState>(DeferredState.Pending, promise.State);
			dfd.Reject(new Exception("test exception"));
			Assert.Equal<DeferredState>(DeferredState.Rejected, promise.State);
			promise.Then(null, ex => { Assert.Equal("test exception", ex.Message); });
		}

		[Fact]
		public void CanRejectInAThread() {
			ObjectDeferred dfd = new ObjectDeferred();
			var promise = dfd.Promise;
			Assert.Equal<DeferredState>(DeferredState.Pending, promise.State);

			ThreadPool.QueueUserWorkItem(d => {
				Thread.Sleep(10);
				(d as ObjectDeferred).Reject();
			}, dfd);

			promise.Wait();
			Assert.Equal<DeferredState>(DeferredState.Rejected, promise.State);
		}
	}
}
