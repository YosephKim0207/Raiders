using System;
using UnityEngine;

namespace AK.Wwise
{
	// Token: 0x020018D9 RID: 6361
	[Serializable]
	public class Switch : BaseGroupType
	{
		// Token: 0x06009CEF RID: 40175 RVA: 0x003EDB88 File Offset: 0x003EBD88
		public void SetValue(GameObject gameObject)
		{
			if (this.IsValid())
			{
				AKRESULT akresult = AkSoundEngine.SetSwitch(base.GetGroupID(), base.GetID(), gameObject);
				base.Verify(akresult);
			}
		}
	}
}
