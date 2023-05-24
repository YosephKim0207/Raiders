using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000088 RID: 136
[InspectorDropdownName("Bosses/BossFinalGuide/Spray1")]
public class BossFinalGuideSpray1 : Script
{
	// Token: 0x06000216 RID: 534 RVA: 0x0000AA1C File Offset: 0x00008C1C
	protected override IEnumerator Top()
	{
		int numBullets = Mathf.RoundToInt(14.727273f);
		for (int i = 0; i < numBullets; i++)
		{
			float t = (float)i / (float)numBullets;
			float tInFullPass = t * 3f % 2f;
			base.Fire(new Offset("left gun"), new Direction(base.GetAimDirection("left gun") - 45f + Mathf.PingPong(tInFullPass * 90f, 90f), DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), null);
			yield return base.Wait(4);
			base.Fire(new Offset("right gun"), new Direction(base.GetAimDirection("right gun") + 45f - Mathf.PingPong(tInFullPass * 90f, 90f), DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), null);
			yield return base.Wait(4);
		}
		yield break;
	}

	// Token: 0x0400022B RID: 555
	private const float SprayAngle = 90f;

	// Token: 0x0400022C RID: 556
	private const float SpraySpeed = 110f;

	// Token: 0x0400022D RID: 557
	private const int SprayIterations = 3;
}
