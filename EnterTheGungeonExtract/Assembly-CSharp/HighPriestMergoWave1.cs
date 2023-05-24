using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001E8 RID: 488
[InspectorDropdownName("Bosses/HighPriest/MergoWave1")]
public class HighPriestMergoWave1 : Script
{
	// Token: 0x0600074C RID: 1868 RVA: 0x00023434 File Offset: 0x00021634
	protected override IEnumerator Top()
	{
		float startAngle = -60f;
		float deltaAngle = 8.571428f;
		AIAnimator aiAnimator = base.BulletBank.aiAnimator;
		string text = "mergo";
		Vector2? vector = new Vector2?(base.Position);
		aiAnimator.PlayVfx(text, null, null, vector);
		yield return base.Wait(60);
		for (int i = 0; i < 15; i++)
		{
			base.Fire(new Direction(startAngle + (float)i * deltaAngle, DirectionType.Aim, -1f), new Speed(8f, SpeedType.Absolute), new Bullet("mergoWave", false, false, false));
		}
		yield break;
	}

	// Token: 0x04000725 RID: 1829
	private const int NumBullets = 15;

	// Token: 0x04000726 RID: 1830
	private const float Angle = 120f;
}
