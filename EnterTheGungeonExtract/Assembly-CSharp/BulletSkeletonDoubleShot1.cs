using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000120 RID: 288
[InspectorDropdownName("BulletSkeleton/DoubleShot1")]
public class BulletSkeletonDoubleShot1 : Script
{
	// Token: 0x170000FB RID: 251
	// (get) Token: 0x06000442 RID: 1090 RVA: 0x00014998 File Offset: 0x00012B98
	protected virtual bool IsHard
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x0001499C File Offset: 0x00012B9C
	protected override IEnumerator Top()
	{
		int numBullets = 4;
		float sign = BraveUtility.RandomSign();
		bool skipInFirstWave = true;
		bool skipInSecondWave = true;
		if (this.IsHard)
		{
			if (BraveUtility.RandomBool())
			{
				skipInFirstWave = false;
			}
			else
			{
				skipInSecondWave = false;
			}
		}
		int skip = UnityEngine.Random.Range(0, numBullets - 1);
		for (int i = 0; i < numBullets - 1; i++)
		{
			if (i != skip || !skipInFirstWave)
			{
				base.Fire(new Direction(base.SubdivideArc(-sign * 25f, sign * 50f, numBullets, i, true), DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null);
			}
			yield return base.Wait(3);
		}
		yield return base.Wait(10);
		skip = UnityEngine.Random.Range(0, numBullets);
		for (int j = 0; j < numBullets; j++)
		{
			if (j != skip || !skipInSecondWave)
			{
				base.Fire(new Direction(base.SubdivideArc(sign * 25f, -sign * 50f, numBullets, j, false), DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null);
			}
			yield return base.Wait(3);
		}
		yield break;
	}
}
