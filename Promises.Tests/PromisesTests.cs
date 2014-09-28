using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Infusion.Api.Promises;
using Xunit;

namespace Promises.Tests {
	public class ObjectPromiseTests {
		[Fact]
		public void CanResolve() {
			Deferred dfd = new Deferred();
			var promise = dfd.Promise;
			Assert.Equal<DeferredState>(DeferredState.Pending, promise.State);
			dfd.Resolve();
			Assert.Equal<DeferredState>(DeferredState.Resolved, promise.State);
		}

		[Fact]
		public void CanResolveInAThread() {
			Deferred dfd = new Deferred();
			var promise = dfd.Promise;
			Assert.Equal<DeferredState>(DeferredState.Pending, promise.State);

			ThreadPool.QueueUserWorkItem(d => {
				Thread.Sleep(10);
				(d as Deferred).Resolve();
			}, dfd);

			promise.Wait();
			Assert.Equal<DeferredState>(DeferredState.Resolved, promise.State);
		}

		[Fact]
		public void CanResolveWithValue() {
			Deferred dfd = new Deferred();
			var promise = dfd.Promise;
			Assert.Equal<DeferredState>(DeferredState.Pending, promise.State);
			dfd.Resolve(true);
			Assert.Equal<DeferredState>(DeferredState.Resolved, promise.State);
			promise.Then(o => { Assert.Equal(true, (bool)o); });
		}

		[Fact]
		public void CanReject() {
			Deferred dfd = new Deferred();
			var promise = dfd.Promise;
			Assert.Equal<DeferredState>(DeferredState.Pending, promise.State);
			dfd.Reject();
			Assert.Equal<DeferredState>(DeferredState.Rejected, promise.State);
		}

		[Fact]
		public void CanRejectWithValue() {
			Deferred dfd = new Deferred();
			var promise = dfd.Promise;
			Assert.Equal<DeferredState>(DeferredState.Pending, promise.State);
			dfd.Reject(false);
			Assert.Equal<DeferredState>(DeferredState.Rejected, promise.State);
			promise.Then(null, o => { Assert.Equal(false, (bool)o); });
		}

		[Fact]
		public void CanRejectInAThread() {
			Deferred dfd = new Deferred();
			var promise = dfd.Promise;
			Assert.Equal<DeferredState>(DeferredState.Pending, promise.State);

			ThreadPool.QueueUserWorkItem(d => {
				Thread.Sleep(10);
				(d as Deferred).Reject();
			}, dfd);

			promise.Wait();
			Assert.Equal<DeferredState>(DeferredState.Rejected, promise.State);
		}
	}
}
