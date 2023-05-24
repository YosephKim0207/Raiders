using System;

namespace FullInspector
{
	// Token: 0x02000634 RID: 1588
	public class SharedInstance<TInstance, TSerializer> : BaseScriptableObject<TSerializer> where TSerializer : BaseSerializer
	{
		// Token: 0x04001930 RID: 6448
		public TInstance Instance;
	}
}
