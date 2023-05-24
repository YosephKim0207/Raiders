using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000246 RID: 582
[InspectorDropdownName("Bosses/Meduzi/SpinFire1")]
public class MeduziSpinFire1 : Script
{
	// Token: 0x060008CD RID: 2253 RVA: 0x0002B4F0 File Offset: 0x000296F0
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 29; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				float num = (float)j * 60f + -37f * (float)i;
				base.Fire(new Offset(1.66f, 0f, num, string.Empty, DirectionType.Absolute), new Direction(num, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("bigBullet", false, false, false));
			}
			yield return base.Wait(6);
		}
		yield break;
	}

	// Token: 0x040008F0 RID: 2288
	private const int NumWaves = 29;

	// Token: 0x040008F1 RID: 2289
	private const int NumBulletsPerWave = 6;

	// Token: 0x040008F2 RID: 2290
	private const float AngleDeltaEachWave = -37f;
}
