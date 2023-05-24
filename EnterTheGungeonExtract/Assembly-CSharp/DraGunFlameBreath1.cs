using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000162 RID: 354
[InspectorDropdownName("Bosses/DraGun/FlameBreath1")]
public class DraGunFlameBreath1 : Script
{
	// Token: 0x06000551 RID: 1361 RVA: 0x00019B54 File Offset: 0x00017D54
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 80; i++)
		{
			base.Fire(new Direction(UnityEngine.Random.Range(-20f, 20f), DirectionType.Aim, -1f), new Speed(14f, SpeedType.Absolute), new Bullet("Breath", false, false, false));
			yield return base.Wait(3);
		}
		yield break;
	}

	// Token: 0x04000532 RID: 1330
	private const int NumBullets = 80;
}
