using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000226 RID: 550
[InspectorDropdownName("Bosses/Lich/QuickDrawBurstShot1")]
public class LichQuickDrawBurstShot1 : Script
{
	// Token: 0x06000845 RID: 2117 RVA: 0x000281FC File Offset: 0x000263FC
	protected override IEnumerator Top()
	{
		float aimDirection = base.GetAimDirection((float)UnityEngine.Random.Range(0, 3), 12f);
		for (int i = -2; i <= 2; i++)
		{
			base.Fire(new Direction(aimDirection + (float)(i * 10), DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("quickHoming", false, false, false));
		}
		return null;
	}
}
