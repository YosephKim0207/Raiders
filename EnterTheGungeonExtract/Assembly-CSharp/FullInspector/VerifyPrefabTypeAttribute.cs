using System;

namespace FullInspector
{
	// Token: 0x020005EB RID: 1515
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class VerifyPrefabTypeAttribute : Attribute
	{
		// Token: 0x060023C5 RID: 9157 RVA: 0x0009CA94 File Offset: 0x0009AC94
		public VerifyPrefabTypeAttribute(VerifyPrefabTypeFlags prefabType)
		{
			this.PrefabType = prefabType;
		}

		// Token: 0x040018D5 RID: 6357
		public VerifyPrefabTypeFlags PrefabType;
	}
}
