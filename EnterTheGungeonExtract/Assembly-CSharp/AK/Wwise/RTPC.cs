using System;
using UnityEngine;

namespace AK.Wwise
{
	// Token: 0x020018D6 RID: 6358
	[Serializable]
	public class RTPC : BaseType
	{
		// Token: 0x06009CE8 RID: 40168 RVA: 0x003EDAB4 File Offset: 0x003EBCB4
		public void SetValue(GameObject gameObject, float value)
		{
			if (this.IsValid())
			{
				AKRESULT akresult = AkSoundEngine.SetRTPCValue(base.GetID(), value, gameObject);
				base.Verify(akresult);
			}
		}

		// Token: 0x06009CE9 RID: 40169 RVA: 0x003EDAE4 File Offset: 0x003EBCE4
		public void SetGlobalValue(float value)
		{
			if (this.IsValid())
			{
				AKRESULT akresult = AkSoundEngine.SetRTPCValue(base.GetID(), value);
				base.Verify(akresult);
			}
		}
	}
}
