using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000114 RID: 276
[InspectorDropdownName("BulletShotgunMan/BlueBasicAttack1")]
public class BulletShotgunManBlueBasicAttack1 : Script
{
	// Token: 0x0600041D RID: 1053 RVA: 0x00013F58 File Offset: 0x00012158
	protected override IEnumerator Top()
	{
		float aimDirection = base.AimDirection;
		for (int i = -2; i <= 2; i++)
		{
			base.Fire(new Direction((float)(i * 20) + aimDirection, DirectionType.Absolute, -1f), new Speed(5f, SpeedType.Absolute), null);
		}
		yield return base.Wait(40);
		if (base.BulletBank && base.BulletBank.behaviorSpeculator.IsStunned)
		{
			yield break;
		}
		if (BraveMathCollege.AbsAngleBetween(base.AimDirection, aimDirection) > 30f)
		{
			aimDirection = base.AimDirection;
		}
		for (float num = -1.5f; num <= 1.5f; num += 1f)
		{
			base.Fire(new Direction(num * 20f + aimDirection, DirectionType.Absolute, -1f), new Speed(5f, SpeedType.Absolute), null);
		}
		yield break;
	}
}
