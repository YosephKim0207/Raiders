using System;
using FullInspector.Modules.SerializableDelegates;

namespace FullInspector
{
	// Token: 0x02000628 RID: 1576
	public class SerializedAction<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9> : BaseSerializedAction
	{
		// Token: 0x0600248B RID: 9355 RVA: 0x0009E7B8 File Offset: 0x0009C9B8
		public void Invoke(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6, TParam7 param7, TParam8 param8, TParam9 param9)
		{
			base.DoInvoke(new object[] { param1, param2, param3, param4, param5, param6, param7, param8, param9 });
		}
	}
}
