using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200016D RID: 365
[InspectorDropdownName("Bosses/DraGun/GlockDirected2")]
public class DraGunGlockDirected2 : Script
{
	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06000577 RID: 1399 RVA: 0x0001A2DC File Offset: 0x000184DC
	protected virtual string BulletName
	{
		get
		{
			return "glock";
		}
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06000578 RID: 1400 RVA: 0x0001A2E4 File Offset: 0x000184E4
	protected virtual bool IsHard
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x0001A2E8 File Offset: 0x000184E8
	protected override IEnumerator Top()
	{
		float num = BraveMathCollege.ClampAngle180(this.Direction);
		bool flag = num > -91f && num < -89f;
		float aimDirection = base.AimDirection;
		int num4;
		if (flag || BraveMathCollege.AbsAngleBetween(aimDirection, -90f) < 45f)
		{
			float num2 = (aimDirection + -90f) / 2f;
			float num3 = 45f;
			num4 = 13;
			for (int i = 0; i < num4; i++)
			{
				base.Fire(new Direction(base.SubdivideArc(num2 - num3, num3 * 2f, num4, i, false), DirectionType.Absolute, -1f), new Speed((float)UnityEngine.Random.Range(6, 11), SpeedType.Absolute), new SpeedChangingBullet(this.BulletName, 11f, 60, -1, false));
			}
			base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new SpeedChangingBullet(this.BulletName, 11f, 60, -1, false));
			if (this.IsHard)
			{
				for (int j = 0; j < num4; j++)
				{
					base.Fire(new Direction(base.SubdivideArc(num2 - num3, num3 * 2f, num4, j, false), DirectionType.Absolute, -1f), new Speed(1f, SpeedType.Absolute), new SpeedChangingBullet(this.BulletName, (float)UnityEngine.Random.Range(9, 12), 60, -1, false));
				}
				base.Fire(new Direction(aimDirection, DirectionType.Absolute, -1f), new Speed(1f, SpeedType.Absolute), new SpeedChangingBullet(this.BulletName, 11f, 60, -1, false));
			}
			yield break;
		}
		num4 = 12;
		float num5 = base.RandomAngle();
		for (int k = 0; k < num4; k++)
		{
			base.Fire(new Direction(base.SubdivideCircle(num5, num4, k, 1f, false), DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new Bullet(this.BulletName + "_spin", false, false, false));
		}
		if (this.IsHard)
		{
			for (int l = 0; l < num4; l++)
			{
				base.Fire(new Direction(base.SubdivideCircle(num5, num4, l, 1f, true), DirectionType.Absolute, -1f), new Speed(1f, SpeedType.Absolute), new SpeedChangingBullet(this.BulletName + "_spin", 9f, 60, -1, false));
			}
		}
		base.Fire(new Direction(base.AimDirection, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new Bullet(this.BulletName, false, false, false));
		yield break;
	}
}
