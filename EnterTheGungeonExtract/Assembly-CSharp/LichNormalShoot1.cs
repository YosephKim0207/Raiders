using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000224 RID: 548
[InspectorDropdownName("Bosses/Lich/NormalShoot1")]
public class LichNormalShoot1 : Script
{
	// Token: 0x0600083D RID: 2109 RVA: 0x0002805C File Offset: 0x0002625C
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				base.Fire(new Direction(UnityEngine.Random.Range(-15f, 15f), DirectionType.Aim, 15f), new Speed(12f, SpeedType.Absolute), new Bullet("default", false, false, false));
				yield return base.Wait(8);
			}
			float dirToTarget = BraveMathCollege.ClampAngle360(base.AimDirection);
			if (dirToTarget > 10f && dirToTarget < 170f)
			{
				yield break;
			}
			yield return 20;
		}
		yield break;
	}
}
