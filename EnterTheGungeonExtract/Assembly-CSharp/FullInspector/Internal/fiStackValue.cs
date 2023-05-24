using System;
using System.Collections.Generic;

namespace FullInspector.Internal
{
	// Token: 0x02000582 RID: 1410
	public class fiStackValue<T>
	{
		// Token: 0x0600215F RID: 8543 RVA: 0x000932BC File Offset: 0x000914BC
		public void Push(T value)
		{
			this._stack.Push(value);
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x000932CC File Offset: 0x000914CC
		public T Pop()
		{
			if (this._stack.Count > 0)
			{
				return this._stack.Pop();
			}
			return default(T);
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06002161 RID: 8545 RVA: 0x00093300 File Offset: 0x00091500
		// (set) Token: 0x06002162 RID: 8546 RVA: 0x00093310 File Offset: 0x00091510
		public T Value
		{
			get
			{
				return this._stack.Peek();
			}
			set
			{
				this.Pop();
				this.Push(value);
			}
		}

		// Token: 0x0400181A RID: 6170
		private readonly Stack<T> _stack = new Stack<T>();
	}
}
