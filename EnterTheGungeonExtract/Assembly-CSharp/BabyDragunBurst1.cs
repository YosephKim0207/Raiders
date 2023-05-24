using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x02000018 RID: 24
public class BabyDragunBurst1 : Script
{
	// Token: 0x0600005C RID: 92 RVA: 0x00003514 File Offset: 0x00001714
	protected override IEnumerator Top()
	{
		yield return base.Wait(15);
		float startDirection = base.AimDirection - 15f;
		for (int i = 0; i < 7; i++)
		{
			base.Fire(new Direction(base.SubdivideArc(startDirection, 30f, 7, i, false), DirectionType.Absolute, -1f), new Speed(UnityEngine.Random.Range(10f, 12f), SpeedType.Absolute), null);
		}
		yield break;
	}

	// Token: 0x0400005E RID: 94
	private const int NumBullets = 7;

	// Token: 0x0400005F RID: 95
	private const float HalfArc = 15f;
}
