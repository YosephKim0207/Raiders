using System;
using FullInspector.Modules.SerializableDelegates;

namespace FullInspector
{
	// Token: 0x02000621 RID: 1569
	public class SerializedAction<TParam1, TParam2> : BaseSerializedAction
	{
		// Token: 0x0600247D RID: 9341 RVA: 0x0009E5A0 File Offset: 0x0009C7A0
		public void Invoke(TParam1 param1, TParam2 param2)
		{
			base.DoInvoke(new object[] { param1, param2 });
		}
	}
}
