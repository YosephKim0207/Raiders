using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000098 RID: 152
[InspectorDropdownName("Bosses/BossFinalRobot/M16")]
public class BossFinalRobotM16 : Script
{
	// Token: 0x06000257 RID: 599 RVA: 0x0000BBE4 File Offset: 0x00009DE4
	protected override IEnumerator Top()
	{
		yield return base.Wait(5);
		float deltaAngle = 15.652174f;
		float deltaT = 2.347826f;
		float t = 0f;
		int i = 0;
		while ((float)i < 23f)
		{
			float angle = -90f - (float)i * deltaAngle;
			for (t += deltaT; t > 1f; t -= 1f)
			{
				yield return base.Wait(1);
			}
			Vector2 offset = BraveMathCollege.GetEllipsePoint(Vector2.zero, 2.92f, 2.03f, angle);
			for (int j = -2; j <= 2; j++)
			{
				base.Fire(new Offset(offset, 0f, string.Empty, DirectionType.Absolute), new Direction(angle + (float)j * 6f, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new DelayedBullet((j != -2) ? "default" : "default_vfx", (j + 2) * 10));
			}
			i++;
		}
		yield break;
	}

	// Token: 0x04000280 RID: 640
	private const float NumBullets = 23f;

	// Token: 0x04000281 RID: 641
	private const int ArcTime = 54;

	// Token: 0x04000282 RID: 642
	private const float ShotVariance = 6f;

	// Token: 0x04000283 RID: 643
	private const float EllipseA = 2.92f;

	// Token: 0x04000284 RID: 644
	private const float EllipseB = 2.03f;
}
