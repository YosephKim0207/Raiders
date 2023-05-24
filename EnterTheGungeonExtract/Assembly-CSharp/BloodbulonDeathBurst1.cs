using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class BloodbulonDeathBurst1 : Script
{
	// Token: 0x0600011C RID: 284 RVA: 0x0000688C File Offset: 0x00004A8C
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 20; i++)
		{
			this.QuadShot(base.RandomAngle(), UnityEngine.Random.Range(0f, 1.5f), UnityEngine.Random.Range(7f, 11f));
			yield return base.Wait(1);
		}
		yield break;
	}

	// Token: 0x0600011D RID: 285 RVA: 0x000068A8 File Offset: 0x00004AA8
	private void QuadShot(float direction, float offset, float speed)
	{
		for (int i = 0; i < 4; i++)
		{
			base.Fire(new Offset(offset, 0f, direction, string.Empty, DirectionType.Absolute), new Direction(direction, DirectionType.Absolute, -1f), new Speed(speed - (float)i * 1.5f, SpeedType.Absolute), new SpeedChangingBullet(speed, 120, -1));
		}
	}

	// Token: 0x04000122 RID: 290
	private const int NumBullets = 20;
}
