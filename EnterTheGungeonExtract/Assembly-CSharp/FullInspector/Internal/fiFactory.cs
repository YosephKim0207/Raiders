using System;
using System.Collections.Generic;

namespace FullInspector.Internal
{
	// Token: 0x02000552 RID: 1362
	public class fiFactory<T> where T : new()
	{
		// Token: 0x0600205B RID: 8283 RVA: 0x0008FB38 File Offset: 0x0008DD38
		public fiFactory(Action<T> reset)
		{
			this._reset = reset;
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x0008FB54 File Offset: 0x0008DD54
		public T GetInstance()
		{
			if (this._reusable.Count == 0)
			{
				return new T();
			}
			return this._reusable.Pop();
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x0008FB78 File Offset: 0x0008DD78
		public void ReuseInstance(T instance)
		{
			this._reset(instance);
			this._reusable.Push(instance);
		}

		// Token: 0x04001799 RID: 6041
		private Stack<T> _reusable = new Stack<T>();

		// Token: 0x0400179A RID: 6042
		private Action<T> _reset;
	}
}
