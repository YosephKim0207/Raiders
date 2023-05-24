using System;
using UnityEngine;

// Token: 0x02000FE1 RID: 4065
[RequireComponent(typeof(GenericIntroDoer))]
public class BossFinalRobotIntroDoer : SpecificIntroDoer
{
	// Token: 0x060058A7 RID: 22695 RVA: 0x0021E268 File Offset: 0x0021C468
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060058A8 RID: 22696 RVA: 0x0021E270 File Offset: 0x0021C470
	public override void EndIntro()
	{
		base.aiAnimator.StopVfx("torch_intro");
	}
}
