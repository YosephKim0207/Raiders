using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x02000197 RID: 407
public class FleshCubeImpact1 : Script
{
	// Token: 0x06000619 RID: 1561 RVA: 0x0001D800 File Offset: 0x0001BA00
	protected override IEnumerator Top()
	{
		this.FireLine(0f);
		this.FireLine(90f);
		this.FireLine(180f);
		this.FireLine(270f);
		return null;
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x0001D830 File Offset: 0x0001BA30
	private void FireLine(float startingAngle)
	{
		float num = 9f;
		for (int i = 0; i < 11; i++)
		{
			float num2 = Mathf.Atan((-45f + (float)i * num) / 45f) * 57.29578f;
			float num3 = Mathf.Cos(num2 * 0.017453292f);
			float num4 = (((double)Mathf.Abs(num3) >= 0.0001) ? (1f / num3) : 1f);
			base.Fire(new Direction(num2 + startingAngle, DirectionType.Absolute, -1f), new Speed(num4 * 9f, SpeedType.Absolute), null);
		}
	}

	// Token: 0x040005F0 RID: 1520
	private const int NumBullets = 11;
}
