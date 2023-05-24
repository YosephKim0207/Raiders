using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x02000289 RID: 649
public class MetBurst1 : Script
{
	// Token: 0x060009ED RID: 2541 RVA: 0x0003025C File Offset: 0x0002E45C
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 3; i++)
		{
			if (base.BulletBank && base.BulletBank.behaviorSpeculator && base.BulletBank.behaviorSpeculator.IsStunned)
			{
				yield break;
			}
			base.BulletBank.aiAnimator.PlayUntilFinished("fire", true, null, -1f, false);
			yield return base.Wait(18);
			base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null);
			for (int j = 0; j < 3; j++)
			{
				base.Fire(new Direction(UnityEngine.Random.Range(-50f, 50f), DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null);
			}
			yield return base.Wait(24);
		}
		yield break;
	}

	// Token: 0x04000A42 RID: 2626
	private const int NumBullets = 3;
}
