using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000168 RID: 360
[InspectorDropdownName("Bosses/DraGun/GlockDirected1")]
public class DraGunGlockDirected1 : Script
{
	// Token: 0x1700013C RID: 316
	// (get) Token: 0x06000569 RID: 1385 RVA: 0x0001A040 File Offset: 0x00018240
	protected virtual string BulletName
	{
		get
		{
			return "glock";
		}
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x0600056A RID: 1386 RVA: 0x0001A048 File Offset: 0x00018248
	protected virtual bool IsHard
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x0001A04C File Offset: 0x0001824C
	protected override IEnumerator Top()
	{
		float num = BraveMathCollege.ClampAngle180(this.Direction);
		if (num > -91f && num < -89f)
		{
			int num2 = 8;
			float num3 = -170f;
			for (int i = 0; i < num2; i++)
			{
				base.Fire(new Direction(base.SubdivideArc(num3, 160f, num2, i, false), DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new Bullet(this.BulletName, false, false, false));
			}
			if (this.IsHard)
			{
				for (int j = 0; j < num2 - 1; j++)
				{
					base.Fire(new Direction(base.SubdivideArc(num3, 160f, num2, j, true), DirectionType.Absolute, -1f), new Speed(1f, SpeedType.Absolute), new SpeedChangingBullet(this.BulletName, 9f, 60, -1, false));
				}
			}
			float aimDirection = base.AimDirection;
			if (BraveMathCollege.AbsAngleBetween(aimDirection, -90f) <= 90f)
			{
				base.Fire(new Direction(base.AimDirection, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new Bullet(this.BulletName, false, false, false));
			}
		}
		else
		{
			int num4 = 12;
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
		}
		return null;
	}
}
