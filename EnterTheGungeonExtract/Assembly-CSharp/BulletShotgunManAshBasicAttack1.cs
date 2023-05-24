using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000113 RID: 275
[InspectorDropdownName("BulletShotgunMan/AshBasicAttack1")]
public class BulletShotgunManAshBasicAttack1 : Script
{
	// Token: 0x0600041B RID: 1051 RVA: 0x00013F0C File Offset: 0x0001210C
	protected override IEnumerator Top()
	{
		for (int i = -2; i <= 2; i++)
		{
			base.Fire(new Direction((float)(i * 6), DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), null);
		}
		return null;
	}
}
