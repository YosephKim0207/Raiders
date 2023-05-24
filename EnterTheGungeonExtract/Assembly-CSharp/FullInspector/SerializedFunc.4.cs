using System;
using FullInspector.Modules.SerializableDelegates;

namespace FullInspector
{
	// Token: 0x0200062D RID: 1581
	public class SerializedFunc<TParam1, TParam2, TParam3, TResult> : BaseSerializedFunc
	{
		// Token: 0x06002494 RID: 9364 RVA: 0x0009E8A8 File Offset: 0x0009CAA8
		public TResult Invoke(TParam1 param1, TParam2 param2, TParam3 param3)
		{
			return (TResult)((object)base.DoInvoke(new object[] { param1, param2, param3 }));
		}
	}
}
