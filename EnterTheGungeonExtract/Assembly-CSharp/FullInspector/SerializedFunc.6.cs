using System;
using FullInspector.Modules.SerializableDelegates;

namespace FullInspector
{
	// Token: 0x0200062F RID: 1583
	public class SerializedFunc<TParam1, TParam2, TParam3, TParam4, TParam5, TResult> : BaseSerializedFunc
	{
		// Token: 0x06002498 RID: 9368 RVA: 0x0009E920 File Offset: 0x0009CB20
		public TResult Invoke(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
		{
			return (TResult)((object)base.DoInvoke(new object[] { param1, param2, param3, param4, param5 }));
		}
	}
}
