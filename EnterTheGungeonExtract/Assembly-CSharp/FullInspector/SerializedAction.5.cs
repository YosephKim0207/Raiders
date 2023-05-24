using System;
using FullInspector.Modules.SerializableDelegates;

namespace FullInspector
{
	// Token: 0x02000623 RID: 1571
	public class SerializedAction<TParam1, TParam2, TParam3, TParam4> : BaseSerializedAction
	{
		// Token: 0x06002481 RID: 9345 RVA: 0x0009E600 File Offset: 0x0009C800
		public void Invoke(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
		{
			base.DoInvoke(new object[] { param1, param2, param3, param4 });
		}
	}
}
