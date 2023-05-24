using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000054 RID: 84
[InspectorDropdownName("Bosses/BossDoorMimic/Flames1")]
public class BossDoorMimicFlames1 : Script
{
	// Token: 0x06000145 RID: 325 RVA: 0x00006F40 File Offset: 0x00005140
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 70; i++)
		{
			base.Fire(new Offset((i % 2 != 0) ? "right eye" : "left eye"), new Direction(UnityEngine.Random.Range(-30f, 30f), DirectionType.Aim, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("flame", false, false, false));
			yield return base.Wait(5);
		}
		yield break;
	}

	// Token: 0x04000143 RID: 323
	private const int NumBullets = 70;
}
