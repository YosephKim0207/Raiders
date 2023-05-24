using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001DD RID: 477
[InspectorDropdownName("Bosses/HighPriest/CrossRandom1")]
public class HighPriestCrossRandom1 : Script
{
	// Token: 0x06000722 RID: 1826 RVA: 0x00022B7C File Offset: 0x00020D7C
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 120; i++)
		{
			base.Fire(new Offset("left hand"), new Direction(-65f - UnityEngine.Random.value * 230f, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("cross", false, false, false));
			yield return base.Wait(1);
			base.Fire(new Offset("right hand"), new Direction(-115f + UnityEngine.Random.value * 230f, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("cross", false, false, false));
			yield return base.Wait(1);
		}
		yield break;
	}

	// Token: 0x04000706 RID: 1798
	private const int NumBullets = 120;
}
