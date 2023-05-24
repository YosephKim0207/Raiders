using System;

// Token: 0x02000BD8 RID: 3032
[Serializable]
public class AttachPointData
{
	// Token: 0x06004016 RID: 16406 RVA: 0x001457F8 File Offset: 0x001439F8
	public AttachPointData(tk2dSpriteDefinition.AttachPoint[] bcs)
	{
		this.attachPoints = bcs;
	}

	// Token: 0x04003310 RID: 13072
	public tk2dSpriteDefinition.AttachPoint[] attachPoints;
}
