using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x0200028B RID: 651
public class MimicBlackMiniguns1 : Script
{
	// Token: 0x060009F5 RID: 2549 RVA: 0x00030448 File Offset: 0x0002E648
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 10; i++)
		{
			this.FireBurst((i % 2 != 0) ? "right gun" : "left gun");
			if (i % 3 == 2)
			{
				yield return base.Wait(6);
				this.QuadShot(base.AimDirection + UnityEngine.Random.Range(-60f, 60f), (!BraveUtility.RandomBool()) ? "right gun" : "left gun", UnityEngine.Random.Range(9f, 11f));
				yield return base.Wait(6);
			}
			yield return base.Wait(12);
		}
		yield break;
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x00030464 File Offset: 0x0002E664
	private void FireBurst(string transform)
	{
		float num = base.RandomAngle();
		float num2 = 22.5f;
		for (int i = 0; i < 16; i++)
		{
			base.Fire(new Offset(transform), new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), null);
		}
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x000304BC File Offset: 0x0002E6BC
	private void QuadShot(float direction, string transform, float speed)
	{
		for (int i = 0; i < 4; i++)
		{
			base.Fire(new Offset(transform), new Direction(direction, DirectionType.Absolute, -1f), new Speed(speed - (float)i * 1.5f, SpeedType.Absolute), new SpeedChangingBullet("bigBullet", speed, 120, -1, false));
		}
	}

	// Token: 0x04000A48 RID: 2632
	private const int NumBursts = 10;

	// Token: 0x04000A49 RID: 2633
	private const int NumBulletsInBurst = 16;
}
