using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200026B RID: 619
[InspectorDropdownName("Bosses/MetalGearRat/JumpPound1")]
public class MetalGearRatJumpPound1 : Script
{
	// Token: 0x06000960 RID: 2400 RVA: 0x0002D4C8 File Offset: 0x0002B6C8
	protected override IEnumerator Top()
	{
		float deltaAngle = 8.372093f;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				for (int k = 0; k < 43; k++)
				{
					float num = -90f - ((float)k + (float)j * 0.5f) * deltaAngle;
					Vector2 ellipsePointSmooth = BraveMathCollege.GetEllipsePointSmooth(Vector2.zero, 6f, 2f, num);
					base.Fire(new Offset(ellipsePointSmooth, 0f, string.Empty, DirectionType.Absolute), new Direction(num, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new DelayedBullet("default_noramp", j * 4));
				}
			}
			yield return base.Wait(40);
		}
		yield break;
	}

	// Token: 0x0400098C RID: 2444
	private const int NumWaves = 3;

	// Token: 0x0400098D RID: 2445
	private const int NumBullets = 43;

	// Token: 0x0400098E RID: 2446
	private const float EllipseA = 6f;

	// Token: 0x0400098F RID: 2447
	private const float EllipseB = 2f;
}
