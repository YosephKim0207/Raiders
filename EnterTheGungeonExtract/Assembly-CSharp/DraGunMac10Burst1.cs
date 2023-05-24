using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200017B RID: 379
[InspectorDropdownName("Bosses/DraGun/Mac10Burst1")]
public class DraGunMac10Burst1 : Script
{
	// Token: 0x060005B0 RID: 1456 RVA: 0x0001B31C File Offset: 0x0001951C
	protected override IEnumerator Top()
	{
		for (;;)
		{
			base.Fire(new Direction(UnityEngine.Random.Range(-45f, 45f), DirectionType.Relative, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("UziBurst", false, false, false));
			yield return base.Wait(UnityEngine.Random.Range(2, 4));
		}
		yield break;
	}
}
