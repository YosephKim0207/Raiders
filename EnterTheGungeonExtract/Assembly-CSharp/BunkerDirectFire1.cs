using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000128 RID: 296
[InspectorDropdownName("Bosses/Bunker/DirectFire1")]
public class BunkerDirectFire1 : Script
{
	// Token: 0x06000460 RID: 1120 RVA: 0x00014F5C File Offset: 0x0001315C
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 15; i++)
		{
			base.Fire(new Offset(0f, -3.25f + (float)i * 0.43f, 0f, string.Empty, DirectionType.Relative), new Direction(0f, DirectionType.Aim, -1f), new Speed(12f, SpeedType.Absolute), new Bullet("default3", false, false, false));
			yield return base.Wait(8);
		}
		yield break;
	}
}
