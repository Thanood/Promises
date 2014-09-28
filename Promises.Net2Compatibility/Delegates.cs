using System;
using System.Collections.Generic;
using System.Text;

namespace System {
#if NET20
	public delegate void Action();
	public delegate TResult Func<out TResult>();
#endif
}
