using System;

// Token: 0x02000BB1 RID: 2993
[Serializable]
public class tk2dSpriteAnimationFrame
{
	// Token: 0x06003F5E RID: 16222 RVA: 0x00141498 File Offset: 0x0013F698
	public void CopyFrom(tk2dSpriteAnimationFrame source)
	{
		this.CopyFrom(source, true);
	}

	// Token: 0x06003F5F RID: 16223 RVA: 0x001414A4 File Offset: 0x0013F6A4
	public void CopyTriggerFrom(tk2dSpriteAnimationFrame source)
	{
		this.triggerEvent = source.triggerEvent;
		this.eventInfo = source.eventInfo;
		this.eventInt = source.eventInt;
		this.eventFloat = source.eventFloat;
		this.eventAudio = source.eventAudio;
		this.eventVfx = source.eventVfx;
		this.eventStopVfx = source.eventStopVfx;
		this.eventOutline = source.eventOutline;
		this.forceMaterialUpdate = source.forceMaterialUpdate;
		this.finishedSpawning = source.finishedSpawning;
		this.eventLerpEmissive = source.eventLerpEmissive;
		this.eventLerpEmissivePower = source.eventLerpEmissivePower;
		this.eventLerpEmissiveTime = source.eventLerpEmissiveTime;
	}

	// Token: 0x06003F60 RID: 16224 RVA: 0x00141550 File Offset: 0x0013F750
	public void ClearTrigger()
	{
		this.triggerEvent = false;
		this.eventInt = 0;
		this.eventFloat = 0f;
		this.eventInfo = string.Empty;
		this.eventAudio = string.Empty;
		this.eventVfx = string.Empty;
		this.eventStopVfx = string.Empty;
		this.eventOutline = tk2dSpriteAnimationFrame.OutlineModifier.Unspecified;
		this.forceMaterialUpdate = false;
		this.finishedSpawning = false;
		this.eventLerpEmissive = false;
		this.eventLerpEmissivePower = 30f;
		this.eventLerpEmissiveTime = 0.5f;
	}

	// Token: 0x06003F61 RID: 16225 RVA: 0x001415D4 File Offset: 0x0013F7D4
	public void CopyFrom(tk2dSpriteAnimationFrame source, bool full)
	{
		this.spriteCollection = source.spriteCollection;
		this.spriteId = source.spriteId;
		this.invulnerableFrame = source.invulnerableFrame;
		this.groundedFrame = source.groundedFrame;
		this.requiresOffscreenUpdate = source.requiresOffscreenUpdate;
		if (full)
		{
			this.CopyTriggerFrom(source);
		}
	}

	// Token: 0x04003197 RID: 12695
	public tk2dSpriteCollectionData spriteCollection;

	// Token: 0x04003198 RID: 12696
	public int spriteId;

	// Token: 0x04003199 RID: 12697
	public bool invulnerableFrame;

	// Token: 0x0400319A RID: 12698
	public bool groundedFrame = true;

	// Token: 0x0400319B RID: 12699
	public bool requiresOffscreenUpdate;

	// Token: 0x0400319C RID: 12700
	public string eventAudio = string.Empty;

	// Token: 0x0400319D RID: 12701
	public string eventVfx = string.Empty;

	// Token: 0x0400319E RID: 12702
	public string eventStopVfx = string.Empty;

	// Token: 0x0400319F RID: 12703
	public bool eventLerpEmissive;

	// Token: 0x040031A0 RID: 12704
	public float eventLerpEmissiveTime = 0.5f;

	// Token: 0x040031A1 RID: 12705
	public float eventLerpEmissivePower = 30f;

	// Token: 0x040031A2 RID: 12706
	public bool forceMaterialUpdate;

	// Token: 0x040031A3 RID: 12707
	public bool finishedSpawning;

	// Token: 0x040031A4 RID: 12708
	public bool triggerEvent;

	// Token: 0x040031A5 RID: 12709
	public string eventInfo = string.Empty;

	// Token: 0x040031A6 RID: 12710
	public int eventInt;

	// Token: 0x040031A7 RID: 12711
	public float eventFloat;

	// Token: 0x040031A8 RID: 12712
	public tk2dSpriteAnimationFrame.OutlineModifier eventOutline;

	// Token: 0x02000BB2 RID: 2994
	public enum OutlineModifier
	{
		// Token: 0x040031AA RID: 12714
		Unspecified,
		// Token: 0x040031AB RID: 12715
		TurnOn = 10,
		// Token: 0x040031AC RID: 12716
		TurnOff = 20
	}
}
