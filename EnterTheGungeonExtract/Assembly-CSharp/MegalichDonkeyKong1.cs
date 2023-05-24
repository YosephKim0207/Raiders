using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200024F RID: 591
[InspectorDropdownName("Bosses/Megalich/DonkeyKong1")]
public class MegalichDonkeyKong1 : Script
{
	// Token: 0x060008EC RID: 2284 RVA: 0x0002B9D4 File Offset: 0x00029BD4
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 3; i++)
		{
			yield return base.Wait(6);
			this.ShootSmallBullets(1f, false);
			yield return base.Wait(36);
			this.ShootSmallBullets(-1f, true);
			yield return base.Wait(30);
		}
		yield break;
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x0002B9F0 File Offset: 0x00029BF0
	private void ShootSmallBullets(float dir, bool isOffset)
	{
		for (int i = 0; i < 5; i++)
		{
			int num = 5;
			float num2 = 0f;
			if (isOffset)
			{
				num--;
				num2 += 0.5f;
			}
			for (int j = 0; j < num; j++)
			{
				base.Fire(new Offset(dir * -19.5f, -0.25f + Mathf.Lerp(0f, -10f, ((float)j + num2) / 4f), 0f, string.Empty, DirectionType.Absolute), new Direction((float)((dir <= 0f) ? 180 : 0), DirectionType.Absolute, -1f), new Speed(14f, SpeedType.Absolute), new DelayedBullet("frogger", 7 * i));
			}
		}
	}

	// Token: 0x04000909 RID: 2313
	private const int NumWaves = 3;

	// Token: 0x0400090A RID: 2314
	private const int NumLargeWaves = 5;

	// Token: 0x0400090B RID: 2315
	private const int NumLargeBulletsPerWave = 5;
}
