using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x020001D6 RID: 470
public class HellFaceFire1 : Script
{
	// Token: 0x06000708 RID: 1800 RVA: 0x00022448 File Offset: 0x00020648
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 150; i++)
		{
			if (i % 14 == 0)
			{
				base.Fire(new Offset("third eye"), new Direction(base.GetAimDirection("third eye"), DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), null);
			}
			if (i % 4 == 0)
			{
				base.Fire(new Offset(UnityEngine.Random.Range(-0.75f, 0.75f), UnityEngine.Random.Range(-0.25f, 0.25f), 0f, "mouth", DirectionType.Absolute), new Direction(UnityEngine.Random.Range(-160f, -20f), DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), null);
			}
			if (i % 7 == 0)
			{
				base.Fire(new Offset(UnityEngine.Random.insideUnitCircle * 0.4f, 0f, "left eye", DirectionType.Absolute), new Direction(UnityEngine.Random.Range(90f, 240f), DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), null);
			}
			if (i % 7 == 0)
			{
				base.Fire(new Offset(UnityEngine.Random.insideUnitCircle * 0.4f, 0f, "right eye", DirectionType.Absolute), new Direction(UnityEngine.Random.Range(-60f, 90f), DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), null);
			}
			yield return base.Wait(1);
		}
		yield break;
	}

	// Token: 0x040006F2 RID: 1778
	public const int NumEyeBullets = 8;
}
