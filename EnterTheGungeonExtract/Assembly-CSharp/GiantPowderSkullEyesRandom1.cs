using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001AE RID: 430
[InspectorDropdownName("Bosses/GiantPowderSkull/EyesRandom1")]
public class GiantPowderSkullEyesRandom1 : Script
{
	// Token: 0x06000666 RID: 1638 RVA: 0x0001EBA0 File Offset: 0x0001CDA0
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 50; i++)
		{
			base.Fire(new Offset("left eye"), new Direction(UnityEngine.Random.Range(-75f, 75f), DirectionType.Aim, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("default_novfx", false, false, false));
			yield return base.Wait(3);
			base.Fire(new Offset("right eye"), new Direction(UnityEngine.Random.Range(-75f, 75f), DirectionType.Aim, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("default_novfx", false, false, false));
			yield return base.Wait(3);
		}
		yield break;
	}

	// Token: 0x04000639 RID: 1593
	private const int NumBullets = 50;

	// Token: 0x0400063A RID: 1594
	private const float BulletRange = 150f;
}
