using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x0200011E RID: 286
[InspectorDropdownName("BulletShotgunMan/MutantBasicAttack1")]
public class BulletShotgunManMutantBasicAttack1 : Script
{
	// Token: 0x0600043E RID: 1086 RVA: 0x000148E0 File Offset: 0x00012AE0
	protected override IEnumerator Top()
	{
		for (int i = -2; i <= 2; i++)
		{
			base.Fire(new Direction((float)(i * 6), DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null);
		}
		return null;
	}
}
