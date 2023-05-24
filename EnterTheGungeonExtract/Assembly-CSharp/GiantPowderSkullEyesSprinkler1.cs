using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020001B0 RID: 432
[InspectorDropdownName("Bosses/GiantPowderSkull/EyesSprinkler1")]
public class GiantPowderSkullEyesSprinkler1 : Script
{
	// Token: 0x0600066E RID: 1646 RVA: 0x0001ED3C File Offset: 0x0001CF3C
	protected override IEnumerator Top()
	{
		float startAngle = base.RandomAngle();
		float startAngle2 = base.RandomAngle();
		for (int i = 0; i < 75; i++)
		{
			base.Fire(new Offset("left eye"), new Direction(startAngle + (float)i * 12f, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("default_novfx", false, false, false));
			yield return base.Wait(2);
			base.Fire(new Offset("right eye"), new Direction(startAngle2 + (float)i * 16f, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("default_novfx", false, false, false));
			yield return base.Wait(2);
		}
		yield break;
	}

	// Token: 0x04000640 RID: 1600
	private const int NumBullets = 75;

	// Token: 0x04000641 RID: 1601
	private const float DeltaAngle1 = 12f;

	// Token: 0x04000642 RID: 1602
	private const float DeltaAngle2 = 16f;
}
