using System;
using FullInspector.Modules.SerializableDelegates;

namespace FullInspector
{
	// Token: 0x02000620 RID: 1568
	public class SerializedAction<TParam1> : BaseSerializedAction
	{
		// Token: 0x0600247B RID: 9339 RVA: 0x0009E580 File Offset: 0x0009C780
		public void Invoke(TParam1 param1)
		{
			base.DoInvoke(new object[] { param1 });
		}
	}
}
