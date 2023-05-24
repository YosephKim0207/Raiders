using System;
using FullInspector.Modules.SerializableDelegates;

namespace FullInspector
{
	// Token: 0x02000632 RID: 1586
	public class SerializedFunc<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TResult> : BaseSerializedFunc
	{
		// Token: 0x0600249E RID: 9374 RVA: 0x0009EA44 File Offset: 0x0009CC44
		public TResult Invoke(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8)
		{
			return (TResult)((object)base.DoInvoke(new object[] { param1, param2, param3, param4, param5, param6, param7, param8 }));
		}
	}
}
