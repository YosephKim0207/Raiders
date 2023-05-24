using System;

// Token: 0x02000E8D RID: 3725
[Serializable]
public class LightStampData : ObjectStampData
{
	// Token: 0x04004626 RID: 17958
	public bool CanBeTopWallLight = true;

	// Token: 0x04004627 RID: 17959
	public bool CanBeCenterLight = true;

	// Token: 0x04004628 RID: 17960
	public int FallbackIndex;
}
