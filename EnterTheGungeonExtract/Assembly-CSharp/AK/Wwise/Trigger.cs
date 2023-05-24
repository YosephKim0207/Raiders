using System;
using UnityEngine;

namespace AK.Wwise
{
	// Token: 0x020018D7 RID: 6359
	[Serializable]
	public class Trigger : BaseType
	{
		// Token: 0x06009CEB RID: 40171 RVA: 0x003EDB18 File Offset: 0x003EBD18
		public void Post(GameObject gameObject)
		{
			if (this.IsValid())
			{
				AKRESULT akresult = AkSoundEngine.PostTrigger(base.GetID(), gameObject);
				base.Verify(akresult);
			}
		}
	}
}
