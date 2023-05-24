using System;
using FullInspector.Modules.SerializableDelegates;

namespace FullInspector
{
	// Token: 0x0200062C RID: 1580
	public class SerializedFunc<TParam1, TParam2, TResult> : BaseSerializedFunc
	{
		// Token: 0x06002492 RID: 9362 RVA: 0x0009E878 File Offset: 0x0009CA78
		public TResult Invoke(TParam1 param1, TParam2 param2)
		{
			return (TResult)((object)base.DoInvoke(new object[] { param1, param2 }));
		}
	}
}
