using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200022A RID: 554
[InspectorDropdownName("Bosses/Lich/SpinFire1")]
public class LichSpinFire1 : Script
{
	// Token: 0x06000851 RID: 2129 RVA: 0x0002841C File Offset: 0x0002661C
	protected override IEnumerator Top()
	{
		float offset = 0f;
		for (int i = 0; i < 60; i++)
		{
			offset += Mathf.SmoothStep(-9f, 9f, Mathf.PingPong((float)i / 30f, 1f) * 2f - 0.5f);
			for (int j = 0; j < 6; j++)
			{
				float num = (float)j * 60f + offset;
				base.Fire(new Offset(1f, 0f, num, string.Empty, DirectionType.Absolute), new Direction(num, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new Bullet("twirl", false, false, false));
			}
			yield return base.Wait(6);
		}
		yield break;
	}

	// Token: 0x0400084E RID: 2126
	private const int NumWaves = 60;

	// Token: 0x0400084F RID: 2127
	private const int NumBulletsPerWave = 6;

	// Token: 0x04000850 RID: 2128
	private const float AngleDeltaEachWave = 9f;
}
