using System;
using FullInspector.Modules.SerializableDelegates;

namespace FullInspector
{
	// Token: 0x02000630 RID: 1584
	public class SerializedFunc<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult> : BaseSerializedFunc
	{
		// Token: 0x0600249A RID: 9370 RVA: 0x0009E978 File Offset: 0x0009CB78
		public TResult Invoke(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
		{
			return (TResult)((object)base.DoInvoke(new object[] { param1, param2, param3, param4, param5, param6 }));
		}
	}
}
