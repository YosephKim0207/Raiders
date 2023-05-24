using System;
using FullInspector.Modules.SerializableDelegates;

namespace FullInspector
{
	// Token: 0x02000626 RID: 1574
	public class SerializedAction<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> : BaseSerializedAction
	{
		// Token: 0x06002487 RID: 9351 RVA: 0x0009E6E0 File Offset: 0x0009C8E0
		public void Invoke(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7)
		{
			base.DoInvoke(new object[] { param1, param2, param3, param4, param5, param6, param7 });
		}
	}
}
