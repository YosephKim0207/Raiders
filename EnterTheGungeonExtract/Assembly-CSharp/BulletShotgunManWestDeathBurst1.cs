using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200011C RID: 284
[InspectorDropdownName("BulletShotgunMan/WestDeathBurst1")]
public class BulletShotgunManWestDeathBurst1 : Script
{
	// Token: 0x0600043A RID: 1082 RVA: 0x0001472C File Offset: 0x0001292C
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
		base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("bigBullet", false, false, false));
		base.Fire(new Direction(BraveUtility.RandomSign() * UnityEngine.Random.Range(20f, 40f), DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("bigBullet", false, false, false));
		return null;
	}
}
