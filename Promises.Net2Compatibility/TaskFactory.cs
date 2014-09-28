using System;
using System.Collections.Generic;
using System.Text;

namespace System.Threading.Tasks {
#if NET20
	public class TaskFactory {
		Task StartNew(Action action) {
			var task = new Task(action);
			task.Start();
			return task;
		}
	}
#endif
}
