using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020000D2 RID: 210
[InspectorDropdownName("Bosses/BulletBros/SweepAttack1")]
public class BulletBrosSweepAttack1 : Script
{
	// Token: 0x0600032E RID: 814 RVA: 0x000105D0 File Offset: 0x0000E7D0
	protected override IEnumerator Top()
	{
		float sign = 1f;
		if (this.BulletManager.PlayerVelocity() != Vector2.zero)
		{
			float num = base.AimDirection + 90f;
			float num2 = this.BulletManager.PlayerVelocity().ToAngle();
			if (BraveMathCollege.AbsAngleBetween(num, num2) > 90f)
			{
				sign = -1f;
			}
		}
		for (int i = 0; i < 15; i++)
		{
			base.Fire(new Direction(base.SubdivideArc(-sign * 60f / 2f, sign * 60f, 15, i, false), DirectionType.Aim, -1f), new Speed(10f, SpeedType.Absolute), null);
			yield return base.Wait(6);
		}
		yield break;
	}

	// Token: 0x04000350 RID: 848
	private const int NumBullets = 15;

	// Token: 0x04000351 RID: 849
	private const float ArcDegrees = 60f;
}
