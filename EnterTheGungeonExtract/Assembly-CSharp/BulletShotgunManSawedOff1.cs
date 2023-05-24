using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200011F RID: 287
[InspectorDropdownName("BulletShotgunMan/SawedOff1")]
public class BulletShotgunManSawedOff1 : Script
{
	// Token: 0x06000440 RID: 1088 RVA: 0x0001492C File Offset: 0x00012B2C
	protected override IEnumerator Top()
	{
		float aimDirection = base.GetAimDirection(1f, 9f);
		for (int i = -2; i <= 2; i++)
		{
			base.Fire(new Direction(aimDirection + (float)(i * 6), DirectionType.Absolute, -1f), new Speed(10f - (float)Mathf.Abs(i) * 0.5f, SpeedType.Absolute), null);
		}
		return null;
	}
}
