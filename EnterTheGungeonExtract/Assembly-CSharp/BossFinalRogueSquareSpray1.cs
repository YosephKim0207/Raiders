using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020000A9 RID: 169
[InspectorDropdownName("Bosses/BossFinalRogue/SquareSpray1")]
public class BossFinalRogueSquareSpray1 : Script
{
	// Token: 0x06000295 RID: 661 RVA: 0x0000D7F0 File Offset: 0x0000B9F0
	protected override IEnumerator Top()
	{
		float angle = -162.5f;
		float totalDuration = 4.8333335f;
		int numBullets = Mathf.RoundToInt(totalDuration * 10f);
		for (int i = 0; i < numBullets; i++)
		{
			float t = (float)i / (float)numBullets;
			float tInFullPass = t * 4f % 2f;
			float currentAngle = angle + Mathf.PingPong(tInFullPass * 145f, 145f);
			base.Fire(new Direction(currentAngle, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("square", false, false, false));
			yield return base.Wait(7);
		}
		yield break;
	}

	// Token: 0x040002D6 RID: 726
	private const float SprayAngle = 145f;

	// Token: 0x040002D7 RID: 727
	private const float SpraySpeed = 120f;

	// Token: 0x040002D8 RID: 728
	private const int SprayIterations = 4;
}
