using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200008A RID: 138
[InspectorDropdownName("Bosses/BossFinalGuide/Spray2")]
public class BossFinalGuideSpray2 : Script
{
	// Token: 0x0600021E RID: 542 RVA: 0x0000AC1C File Offset: 0x00008E1C
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 40; i++)
		{
			base.Fire(new Offset("left gun"), new Direction(base.GetAimDirection("left gun") + (UnityEngine.Random.value - 0.5f) * 110f, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), null);
			yield return base.Wait(6);
			base.Fire(new Offset("right gun"), new Direction(base.GetAimDirection("right gun") + (UnityEngine.Random.value - 0.5f) * 110f, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), null);
			yield return base.Wait(6);
		}
		yield break;
	}

	// Token: 0x04000236 RID: 566
	private const int NumBullets = 40;

	// Token: 0x04000237 RID: 567
	private const float SprayAngle = 110f;
}
