using System;
using FullInspector.Modules.SerializableDelegates;

namespace FullInspector
{
	// Token: 0x0200062B RID: 1579
	public class SerializedFunc<TParam1, TResult> : BaseSerializedFunc
	{
		// Token: 0x06002490 RID: 9360 RVA: 0x0009E854 File Offset: 0x0009CA54
		public TResult Invoke(TParam1 param1)
		{
			return (TResult)((object)base.DoInvoke(new object[] { param1 }));
		}
	}
}
