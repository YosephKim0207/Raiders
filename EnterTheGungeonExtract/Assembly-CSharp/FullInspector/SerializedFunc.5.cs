using System;
using FullInspector.Modules.SerializableDelegates;

namespace FullInspector
{
	// Token: 0x0200062E RID: 1582
	public class SerializedFunc<TParam1, TParam2, TParam3, TParam4, TResult> : BaseSerializedFunc
	{
		// Token: 0x06002496 RID: 9366 RVA: 0x0009E8E0 File Offset: 0x0009CAE0
		public TResult Invoke(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
		{
			return (TResult)((object)base.DoInvoke(new object[] { param1, param2, param3, param4 }));
		}
	}
}
