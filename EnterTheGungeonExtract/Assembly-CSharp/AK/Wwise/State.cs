using System;

namespace AK.Wwise
{
	// Token: 0x020018D8 RID: 6360
	[Serializable]
	public class State : BaseGroupType
	{
		// Token: 0x06009CED RID: 40173 RVA: 0x003EDB4C File Offset: 0x003EBD4C
		public void SetValue()
		{
			if (this.IsValid())
			{
				AKRESULT akresult = AkSoundEngine.SetState(base.GetGroupID(), base.GetID());
				base.Verify(akresult);
			}
		}
	}
}
