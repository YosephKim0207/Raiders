using System;
using FullInspector.Modules.SerializableDelegates;

namespace FullInspector
{
	// Token: 0x02000624 RID: 1572
	public class SerializedAction<TParam1, TParam2, TParam3, TParam4, TParam5> : BaseSerializedAction
	{
		// Token: 0x06002483 RID: 9347 RVA: 0x0009E63C File Offset: 0x0009C83C
		public void Invoke(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5)
		{
			base.DoInvoke(new object[] { param1, param2, param3, param4, param5 });
		}
	}
}
