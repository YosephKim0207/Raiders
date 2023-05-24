using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020002B7 RID: 695
[InspectorDropdownName("Minibosses/PhantomAgunim/Arc1")]
public class PhantomAgunimArc1 : Script
{
	// Token: 0x06000AA9 RID: 2729 RVA: 0x00033410 File Offset: 0x00031610
	protected override IEnumerator Top()
	{
		bool facingRight = BraveMathCollege.AbsAngleBetween(base.BulletBank.aiAnimator.FacingDirection, 0f) < 90f;
		float startAngle = ((!facingRight) ? (-110f) : (-70f));
		float deltaAngle = ((!facingRight) ? (-180f) : 180f) / 19f;
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

	// Token: 0x04000B1E RID: 2846
	private const float NumBullets = 19f;

	// Token: 0x04000B1F RID: 2847
	private const int ArcTime = 15;

	// Token: 0x04000B20 RID: 2848
	private const float EllipseA = 2.25f;

	// Token: 0x04000B21 RID: 2849
	private const float EllipseB = 1.5f;
}
