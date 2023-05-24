using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000191 RID: 401
[InspectorDropdownName("Bosses/DraGun/SweepFlameBreath2")]
public class DraGunSweepFlameBreath2 : Script
{
	// Token: 0x06000601 RID: 1537 RVA: 0x0001D3E0 File Offset: 0x0001B5E0
	protected override IEnumerator Top()
	{
		for (;;)
		{
			base.Fire(new Direction(UnityEngine.Random.Range(-45f, 45f), DirectionType.Relative, -1f), new Speed(14f, SpeedType.Absolute), new Bullet("Sweep", false, false, false));
			if (base.Tick % 2 == 1)
			{
				base.Fire(new Direction(UnityEngine.Random.Range(-15f, 15f), DirectionType.Relative, -1f), new Speed((float)UnityEngine.Random.Range(2, 8), SpeedType.Absolute), new SpeedChangingBullet("Sweep", 14f, 120, -1, false));
			}
			yield return base.Wait(1);
		}
		yield break;
	}
}
