using System;

// Token: 0x02000BD7 RID: 3031
[Serializable]
public class BagelColliderData
{
	// Token: 0x06004015 RID: 16405 RVA: 0x001457E8 File Offset: 0x001439E8
	public BagelColliderData(BagelCollider[] bcs)
	{
		this.bagelColliders = bcs;
	}

	// Token: 0x0400330F RID: 13071
	public BagelCollider[] bagelColliders;
}
