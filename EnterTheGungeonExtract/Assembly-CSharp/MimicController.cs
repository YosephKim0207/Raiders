using System;

// Token: 0x020010B3 RID: 4275
public class MimicController : BraveBehaviour
{
	// Token: 0x06005E3D RID: 24125 RVA: 0x00242508 File Offset: 0x00240708
	public void Awake()
	{
		base.aiActor.enabled = false;
		base.aiShooter.enabled = false;
		base.behaviorSpeculator.enabled = false;
	}

	// Token: 0x06005E3E RID: 24126 RVA: 0x00242530 File Offset: 0x00240730
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
