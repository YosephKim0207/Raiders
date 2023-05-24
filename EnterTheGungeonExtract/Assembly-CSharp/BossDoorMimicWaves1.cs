using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x0200005C RID: 92
[InspectorDropdownName("Bosses/BossDoorMimic/Waves1")]
public class BossDoorMimicWaves1 : Script
{
	// Token: 0x06000165 RID: 357 RVA: 0x00007880 File Offset: 0x00005A80
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 7; i++)
		{
			bool offset = false;
			int numBullets = 5;
			if (i % 2 == 1)
			{
				offset = true;
				numBullets--;
			}
			float startDirection = base.AimDirection - 30f;
			for (int j = 0; j < numBullets; j++)
			{
				base.Fire(new Direction(base.SubdivideArc(startDirection, 60f, 5, j, offset), DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("teeth_wave", false, false, false));
			}
			yield return base.Wait(15);
		}
		yield break;
	}

	// Token: 0x0400016F RID: 367
	private const int NumWaves = 7;

	// Token: 0x04000170 RID: 368
	private const int NumBulletsPerWave = 5;

	// Token: 0x04000171 RID: 369
	private const float Arc = 60f;
}
