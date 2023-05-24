using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000179 RID: 377
[InspectorDropdownName("Bosses/DraGun/KnifeSpray1")]
public class DraGunFlameKnifeSpray1 : Script
{
	// Token: 0x060005A8 RID: 1448 RVA: 0x0001B1E0 File Offset: 0x000193E0
	protected override IEnumerator Top()
	{
		float deltaAngle = 7f;
		float sign = BraveUtility.RandomSign();
		for (int i = 0; i < 12; i++)
		{
			base.Fire(new Direction((-45f + deltaAngle * (float)i + (float)UnityEngine.Random.Range(-5, 5)) * sign, DirectionType.Relative, -1f), new Speed(12f, SpeedType.Absolute), null);
			yield return base.Wait(2);
		}
		yield break;
	}

	// Token: 0x04000573 RID: 1395
	private const int NumBullets = 12;
}
