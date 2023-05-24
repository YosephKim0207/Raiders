using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x0200011D RID: 285
[InspectorDropdownName("BulletShotgunMan/RedBasicAttack1")]
public class BulletShotgunManRedBasicAttack1 : Script
{
	// Token: 0x0600043C RID: 1084 RVA: 0x00014894 File Offset: 0x00012A94
	protected override IEnumerator Top()
	{
		for (int i = -2; i <= 2; i++)
		{
			base.Fire(new Direction((float)(i * 6), DirectionType.Aim, -1f), new Speed(5f, SpeedType.Absolute), null);
		}
		return null;
	}
}
