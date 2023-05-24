using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000094 RID: 148
[InspectorDropdownName("Bosses/BossFinalMarine/SpinFire1")]
public class BossFinalMarineSpinFire1 : Script
{
	// Token: 0x06000247 RID: 583 RVA: 0x0000B760 File Offset: 0x00009960
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 25; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				float num = (float)j * 60f + 37f * (float)i;
				base.Fire(new Direction(num, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), null);
			}
			yield return base.Wait(8);
		}
		for (int k = 0; k < 64; k++)
		{
			base.Fire(new Direction((float)k * 360f / 64f, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), null);
		}
		yield break;
	}

	// Token: 0x04000269 RID: 617
	private const int NumWaves = 25;

	// Token: 0x0400026A RID: 618
	private const int NumBulletsPerWave = 6;

	// Token: 0x0400026B RID: 619
	private const float AngleDeltaEachWave = 37f;

	// Token: 0x0400026C RID: 620
	private const int NumBulletsFinalWave = 64;
}
