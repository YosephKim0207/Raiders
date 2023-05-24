using System;
using UnityEngine;

namespace AK.Wwise
{
	// Token: 0x020018D2 RID: 6354
	[Serializable]
	public class BaseType
	{
		// Token: 0x06009CD4 RID: 40148 RVA: 0x003ED8D8 File Offset: 0x003EBAD8
		protected uint GetID()
		{
			return (uint)this.ID;
		}

		// Token: 0x06009CD5 RID: 40149 RVA: 0x003ED8E0 File Offset: 0x003EBAE0
		public virtual bool IsValid()
		{
			return (long)this.ID != 0L;
		}

		// Token: 0x06009CD6 RID: 40150 RVA: 0x003ED8F0 File Offset: 0x003EBAF0
		public bool Validate()
		{
			if (this.IsValid())
			{
				return true;
			}
			Debug.LogWarning("Wwise ID has not been resolved. Consider picking a new " + base.GetType().Name + ".");
			return false;
		}

		// Token: 0x06009CD7 RID: 40151 RVA: 0x003ED920 File Offset: 0x003EBB20
		protected void Verify(AKRESULT result)
		{
		}

		// Token: 0x04009E7F RID: 40575
		public int ID;
	}
}
