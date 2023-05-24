using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200007D RID: 125
[InspectorDropdownName("Bosses/BossFinalConvict/WalkAndShoot1")]
public class BossFinalConvictWalkAndShoot1 : Script
{
	// Token: 0x060001E3 RID: 483 RVA: 0x000095D8 File Offset: 0x000077D8
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 100; i++)
		{
			base.Fire(new Direction(UnityEngine.Random.Range(-15f, 15f), DirectionType.Relative, -1f), new Speed(9f, SpeedType.Absolute), null);
			yield return base.Wait(3);
		}
		yield break;
	}

	// Token: 0x040001F7 RID: 503
	private const int NumBullets = 100;
}
