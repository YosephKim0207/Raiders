using System;
using System.Collections;
using FullInspector;

// Token: 0x02000320 RID: 800
[InspectorDropdownName("MimicWall/IntroSlam1")]
public class WallMimicIntroSlam1 : WallMimicSlam1
{
	// Token: 0x06000C5D RID: 3165 RVA: 0x0003B994 File Offset: 0x00039B94
	protected override IEnumerator Top()
	{
		float facingDirection = base.BulletBank.aiAnimator.CurrentArtAngle;
		base.FireLine(facingDirection - 90f, 5f, 45f, -15f, false);
		base.FireLine(facingDirection, 11f, -45f, 45f, false);
		base.FireLine(facingDirection + 90f, 5f, -45f, 15f, false);
		yield return base.Wait(10);
		base.FireLine(facingDirection - 90f, 4f, 45f, -15f, true);
		base.FireLine(facingDirection, 10f, -45f, 45f, true);
		base.FireLine(facingDirection + 90f, 4f, -45f, 15f, true);
		yield break;
	}
}
