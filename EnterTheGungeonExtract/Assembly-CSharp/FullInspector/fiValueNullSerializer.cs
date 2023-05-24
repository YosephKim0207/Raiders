using System;
using FullInspector.Internal;

namespace FullInspector
{
	// Token: 0x02000606 RID: 1542
	public abstract class fiValueNullSerializer<T> : fiValueProxyEditor, fiIValueProxyAPI
	{
		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06002413 RID: 9235 RVA: 0x0009D4EC File Offset: 0x0009B6EC
		// (set) Token: 0x06002414 RID: 9236 RVA: 0x0009D4FC File Offset: 0x0009B6FC
		object fiIValueProxyAPI.Value
		{
			get
			{
				return this.Value;
			}
			set
			{
				this.Value = (T)((object)value);
			}
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x0009D50C File Offset: 0x0009B70C
		void fiIValueProxyAPI.SaveState()
		{
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x0009D510 File Offset: 0x0009B710
		void fiIValueProxyAPI.LoadState()
		{
		}

		// Token: 0x04001907 RID: 6407
		public T Value;
	}
}
