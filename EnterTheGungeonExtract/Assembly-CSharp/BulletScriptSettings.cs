using System;

// Token: 0x02001611 RID: 5649
[Serializable]
public class BulletScriptSettings
{
	// Token: 0x060083B6 RID: 33718 RVA: 0x0035F9A0 File Offset: 0x0035DBA0
	public BulletScriptSettings()
	{
	}

	// Token: 0x060083B7 RID: 33719 RVA: 0x0035F9A8 File Offset: 0x0035DBA8
	public BulletScriptSettings(BulletScriptSettings other)
	{
		this.SetAll(other);
	}

	// Token: 0x060083B8 RID: 33720 RVA: 0x0035F9B8 File Offset: 0x0035DBB8
	public void SetAll(BulletScriptSettings other)
	{
		this.overrideMotion = other.overrideMotion;
		this.preventPooling = other.preventPooling;
		this.surviveRigidbodyCollisions = other.surviveRigidbodyCollisions;
		this.surviveTileCollisions = other.surviveTileCollisions;
	}

	// Token: 0x04008707 RID: 34567
	public bool overrideMotion;

	// Token: 0x04008708 RID: 34568
	public bool preventPooling;

	// Token: 0x04008709 RID: 34569
	public bool surviveRigidbodyCollisions;

	// Token: 0x0400870A RID: 34570
	public bool surviveTileCollisions;
}
