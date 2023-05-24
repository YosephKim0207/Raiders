using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000117 RID: 279
[InspectorDropdownName("BulletShotgunMan/VeteranDeathBurst1")]
public class BulletShotgunManVeteranDeathBurst1 : Script
{
	// Token: 0x06000427 RID: 1063 RVA: 0x00014170 File Offset: 0x00012370
	protected override IEnumerator Top()
	{
		int num = 5;
		for (int i = 0; i < num; i++)
		{
			base.Fire(new Direction(base.SubdivideCircle(0f, num, i, 1f, false), DirectionType.Absolute, -1f), new Speed(6.5f, SpeedType.Absolute), new Bullet("flashybullet", false, false, false));
		}
		num = 5;
		for (int j = 0; j < num; j++)
		{
			base.Fire(new Direction(base.SubdivideCircle(0f, num, j, 1f, true), DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("flashybullet", false, false, false));
		}
		num = 3;
		for (int k = 0; k < num; k++)
		{
			base.Fire(new Direction(base.RandomAngle(), DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("flashybullet", false, false, false));
		}
		return null;
	}
}
