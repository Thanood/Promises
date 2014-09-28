using System;
using System.Collections.Generic;
using System.Text;

namespace System.Threading.Tasks {
#if NET20
	public class Task {
		Action action;
		Thread thread;
		
		public Task(Action action) {
			this.action = action;
			thread = new Thread(() => { this.action(); });
		}
		
		public void Start() {
			if (thread.ThreadState == ThreadState.Unstarted) {
				thread.Start();
			}
		}
	}

	public class Task<TResult> {
		Func<TResult> action;
		Thread thread;

		public Task(Func<TResult> action) {
			this.action = action;
			thread = new Thread(() => { this.action(); });
		}
		public void Start() {
			if (thread.ThreadState == ThreadState.Unstarted) {
				thread.Start();
			}
		}
	}
#endif
}
