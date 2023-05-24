using System;
using FullInspector.Modules.SerializableDelegates;

namespace FullInspector
{
	// Token: 0x02000625 RID: 1573
	public class SerializedAction<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> : BaseSerializedAction
	{
		// Token: 0x06002485 RID: 9349 RVA: 0x0009E684 File Offset: 0x0009C884
		public void Invoke(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, TParam6 param6)
		{
			base.DoInvoke(new object[] { param1, param2, param3, param4, param5, param6 });
		}
	}
}
