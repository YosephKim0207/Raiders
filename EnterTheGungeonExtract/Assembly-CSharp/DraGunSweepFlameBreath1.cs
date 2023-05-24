using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200018F RID: 399
[InspectorDropdownName("Bosses/DraGun/SweepFlameBreath1")]
public class DraGunSweepFlameBreath1 : Script
{
	// Token: 0x060005F9 RID: 1529 RVA: 0x0001D2E4 File Offset: 0x0001B4E4
	protected override IEnumerator Top()
	{
		for (;;)
		{
			base.Fire(new Direction(UnityEngine.Random.Range(-45f, 45f), DirectionType.Relative, -1f), new Speed(14f, SpeedType.Absolute), new Bullet("Sweep", false, false, false));
			yield return base.Wait(1);
		}
		yield break;
	}
}
