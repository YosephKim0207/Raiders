using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020001A8 RID: 424
[InspectorDropdownName("Bosses/GatlingGull/FanBursts1")]
public class GatlingGullFanBursts1 : Script
{
	// Token: 0x0600064E RID: 1614 RVA: 0x0001E630 File Offset: 0x0001C830
	protected override IEnumerator Top()
	{
		float startAngle = base.AimDirection - 65f;
		float deltaAngle = 6.8421054f;
		base.BulletBank.aiAnimator.LockFacingDirection = true;
		base.BulletBank.aiAnimator.FacingDirection = base.AimDirection;
		for (int i = 0; i < 2; i++)
		{
			float angle = startAngle;
			for (int j = 0; j < 20; j++)
			{
				base.Fire(new Direction(angle, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("defaultWithVfx", false, false, false));
				angle += deltaAngle;
			}
			if (i < 1)
			{
				yield return base.Wait(75);
			}
		}
		yield return base.Wait(20);
		base.BulletBank.aiAnimator.LockFacingDirection = false;
		yield break;
	}

	// Token: 0x04000617 RID: 1559
	private const int NumWaves = 2;

	// Token: 0x04000618 RID: 1560
	private const int NumBulletsPerWave = 20;

	// Token: 0x04000619 RID: 1561
	private const float WaveArcLength = 130f;
}
