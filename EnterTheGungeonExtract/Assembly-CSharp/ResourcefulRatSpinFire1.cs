using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020002D5 RID: 725
[InspectorDropdownName("Bosses/ResourcefulRat/SpinFire1")]
public class ResourcefulRatSpinFire1 : Script
{
	// Token: 0x06000B2D RID: 2861 RVA: 0x000363F0 File Offset: 0x000345F0
	protected override IEnumerator Top()
	{
		yield return base.Wait(5);
		float deltaAngle = 15.652174f;
		float deltaT = 3.0434783f;
		float t = 0f;
		int i = 0;
		while ((float)i < 23f)
		{
			float angle = -90f - (float)i * deltaAngle;
			for (t += deltaT; t > 1f; t -= 1f)
			{
				yield return base.Wait(1);
			}
			Vector2 offset = BraveMathCollege.GetEllipsePoint(Vector2.zero, 1.39f, 0.92f, angle);
			base.Fire(new Offset(offset, 0f, string.Empty, DirectionType.Absolute), new Direction(angle - 6f, DirectionType.Absolute, -1f), new Speed(16f, SpeedType.Absolute), new Bullet("cheese", true, false, false));
			base.Fire(new Offset(offset, 0f, string.Empty, DirectionType.Absolute), new Direction(angle, DirectionType.Absolute, -1f), new Speed(16f, SpeedType.Absolute), new Bullet("cheese", true, false, false));
			base.Fire(new Offset(offset, 0f, string.Empty, DirectionType.Absolute), new Direction(angle + 6f, DirectionType.Absolute, -1f), new Speed(16f, SpeedType.Absolute), new Bullet("cheese", true, false, false));
			base.Fire(new Offset(offset, 0f, string.Empty, DirectionType.Absolute), new Direction(angle - 6f, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new SpeedChangingBullet("cheese", 16f, 50, -1, true));
			base.Fire(new Offset(offset, 0f, string.Empty, DirectionType.Absolute), new Direction(angle, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new SpeedChangingBullet("cheese", 16f, 50, -1, true));
			base.Fire(new Offset(offset, 0f, string.Empty, DirectionType.Absolute), new Direction(angle + 6f, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new SpeedChangingBullet("cheese", 16f, 50, -1, true));
			i++;
		}
		yield break;
	}

	// Token: 0x04000BD6 RID: 3030
	private const float NumBullets = 23f;

	// Token: 0x04000BD7 RID: 3031
	private const int ArcTime = 70;

	// Token: 0x04000BD8 RID: 3032
	private const float SpreadAngle = 6f;

	// Token: 0x04000BD9 RID: 3033
	private const float BulletSpeed = 16f;

	// Token: 0x04000BDA RID: 3034
	private const float EllipseA = 1.39f;

	// Token: 0x04000BDB RID: 3035
	private const float EllipseB = 0.92f;
}
