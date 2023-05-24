using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020002C5 RID: 709
[InspectorDropdownName("R2G2/Burst1")]
public class R2G2Burst1 : Script
{
	// Token: 0x06000AE3 RID: 2787 RVA: 0x000342CC File Offset: 0x000324CC
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 6; i++)
		{
			base.Fire(new Direction((float)UnityEngine.Random.Range(-10, 10), DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null);
			yield return 10;
		}
		yield break;
	}
}
