using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200030B RID: 779
[InspectorDropdownName("Bosses/TankTreader/GuySpray1")]
public class TankTreaderGuySpray1 : Script
{
	// Token: 0x06000C11 RID: 3089 RVA: 0x0003AA64 File Offset: 0x00038C64
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 42; i++)
		{
			float t = Mathf.PingPong((float)i / 6f, 1f);
			float dir = Mathf.Lerp(-30f, 30f, t) + (float)UnityEngine.Random.Range(-5, 5);
			base.Fire(new Direction(dir, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), new Bullet("guyBullet", false, false, false));
			yield return base.Wait(6);
		}
		yield break;
	}

	// Token: 0x04000CD5 RID: 3285
	private const int NumBullets = 42;
}
