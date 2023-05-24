using System;

// Token: 0x0200108F RID: 4239
public class DynamiteGuyController : BraveBehaviour
{
	// Token: 0x06005D48 RID: 23880 RVA: 0x0023C478 File Offset: 0x0023A678
	public void Update()
	{
		if (base.aiActor.HasBeenAwoken && !base.aiAnimator.IsPlaying("spawn"))
		{
			this.SparksDoer.enabled = true;
			base.enabled = false;
		}
	}

	// Token: 0x06005D49 RID: 23881 RVA: 0x0023C4B4 File Offset: 0x0023A6B4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400572A RID: 22314
	public SimpleSparksDoer SparksDoer;
}
