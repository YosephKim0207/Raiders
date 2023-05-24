using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001AA RID: 426
[InspectorDropdownName("Bosses/GatlingGull/FanSpray1")]
public class GatlingGullFanSpray1 : Script
{
	// Token: 0x06000656 RID: 1622 RVA: 0x0001E820 File Offset: 0x0001CA20
	protected override IEnumerator Top()
	{
		float angle = base.AimDirection - 45f;
		float totalDuration = 2.4f;
		int numBullets = Mathf.RoundToInt(totalDuration * 10f);
		for (int i = 0; i < numBullets; i++)
		{
			float t = (float)i / (float)numBullets;
			float tInFullPass = t * 4f % 2f;
			float currentAngle = angle + Mathf.PingPong(tInFullPass * 90f, 90f);
			base.Fire(new Direction(currentAngle, DirectionType.Absolute, -1f), new Speed((float)((i != 12) ? 12 : 30), SpeedType.Absolute), null);
			yield return base.Wait(6);
		}
		yield break;
	}

	// Token: 0x04000622 RID: 1570
	private const float SprayAngle = 90f;

	// Token: 0x04000623 RID: 1571
	private const float SpraySpeed = 150f;

	// Token: 0x04000624 RID: 1572
	private const int SprayIterations = 4;
}
