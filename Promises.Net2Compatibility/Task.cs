using System;
using System.Collections.Generic;
using System.Text;

namespace System.Threading.Tasks {
	public class Task {
		public delegate void TaskActionDelegate();
		TaskActionDelegate action;
		
		public Task(TaskActionDelegate action) {
			this.action = action;
		}
		
		public void Start() {
			ThreadPool.QueueUserWorkItem((o) => { action(); });
		}
	}
}
