using System;
using FullInspector.Modules.SerializableDelegates;

namespace FullInspector
{
	// Token: 0x0200062A RID: 1578
	public class SerializedFunc<TResult> : BaseSerializedFunc
	{
		// Token: 0x0600248E RID: 9358 RVA: 0x0009E83C File Offset: 0x0009CA3C
		public TResult Invoke()
		{
			return (TResult)((object)base.DoInvoke(null));
		}
	}
}
