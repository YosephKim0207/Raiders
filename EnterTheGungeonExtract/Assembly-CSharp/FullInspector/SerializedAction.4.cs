using System;
using FullInspector.Modules.SerializableDelegates;

namespace FullInspector
{
	// Token: 0x02000622 RID: 1570
	public class SerializedAction<TParam1, TParam2, TParam3> : BaseSerializedAction
	{
		// Token: 0x0600247F RID: 9343 RVA: 0x0009E5CC File Offset: 0x0009C7CC
		public void Invoke(TParam1 param1, TParam2 param2, TParam3 param3)
		{
			base.DoInvoke(new object[] { param1, param2, param3 });
		}
	}
}
