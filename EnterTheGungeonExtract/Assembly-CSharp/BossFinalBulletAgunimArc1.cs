using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200005E RID: 94
[InspectorDropdownName("Bosses/BossFinalBullet/AgunimArc1")]
public class BossFinalBulletAgunimArc1 : Script
{
	// Token: 0x0600016D RID: 365 RVA: 0x00007A0C File Offset: 0x00005C0C
	protected override IEnumerator Top()
	{
		bool facingRight = BraveMathCollege.AbsAngleBetween(base.BulletBank.aiAnimator.FacingDirection, 0f) < 90f;
		float startAngle = ((!facingRight) ? (-170f) : (-10f));
		float deltaAngle = ((!facingRight) ? 160f : (-160f)) / 19f;
		float deltaT = 0.7894737f;
		float t = 0f;
		int i = 0;
		while ((float)i < 19f)
		{
			float angle = startAngle + (float)i * deltaAngle;
			for (t += deltaT; t > 1f; t -= 1f)
			{
				yield return base.Wait(1);
			}
			Vector2 offset = BraveMathCollege.GetEllipsePoint(Vector2.zero, 2.25f, 1.5f, angle);
			for (int j = 0; j < 3; j++)
			{
				base.Fire(new Offset(offset, 0f, string.Empty, DirectionType.Absolute), new Direction(angle + UnityEngine.Random.Range(-25f, 25f), DirectionType.Absolute, -1f), new Speed(UnityEngine.Random.Range(10f, 14f) - (float)j, SpeedType.Absolute), new DelayedBullet("default", j * 9));
			}
			i++;
		}
		yield break;
	}

	// Token: 0x0400017A RID: 378
	private const float NumBullets = 19f;

	// Token: 0x0400017B RID: 379
	private const int ArcTime = 15;

	// Token: 0x0400017C RID: 380
	private const float EllipseA = 2.25f;

	// Token: 0x0400017D RID: 381
	private const float EllipseB = 1.5f;
}
