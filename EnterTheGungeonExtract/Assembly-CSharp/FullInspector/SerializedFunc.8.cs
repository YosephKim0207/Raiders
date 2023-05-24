using System;
using FullInspector.Modules.SerializableDelegates;

namespace FullInspector
{
	// Token: 0x02000631 RID: 1585
	public class SerializedFunc<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult> : BaseSerializedFunc
	{
		// Token: 0x0600249C RID: 9372 RVA: 0x0009E9D8 File Offset: 0x0009CBD8
		public TResult Invoke(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7)
		{
			return (TResult)((object)base.DoInvoke(new object[] { param1, param2, param3, param4, param5, param6, param7 }));
		}
	}
}
