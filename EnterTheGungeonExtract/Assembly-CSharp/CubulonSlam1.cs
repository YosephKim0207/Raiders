using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class CubulonSlam1 : Script
{
	// Token: 0x060004CE RID: 1230 RVA: 0x000174CC File Offset: 0x000156CC
	protected override IEnumerator Top()
	{
		this.FireLine(45f);
		this.FireLine(135f);
		this.FireLine(225f);
		this.FireLine(315f);
		return null;
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x000174FC File Offset: 0x000156FC
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

	// Token: 0x040004A3 RID: 1187
	private const int NumBullets = 11;
}
