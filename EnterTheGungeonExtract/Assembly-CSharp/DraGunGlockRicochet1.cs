using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x02000173 RID: 371
[InspectorDropdownName("Bosses/DraGun/GlockRicochet1")]
public class DraGunGlockRicochet1 : Script
{
	// Token: 0x0600058B RID: 1419 RVA: 0x0001A6A8 File Offset: 0x000188A8
	protected override IEnumerator Top()
	{
		float num = BraveMathCollege.ClampAngle180(this.Direction);
		if (num > -91f && num < -89f)
		{
			int num2 = 8;
			float num3 = -170f;
			float num4 = 160f / (float)(num2 - 1);
			for (int i = 0; i < num2; i++)
			{
				base.Fire(new Direction(num3 + (float)i * num4, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("ricochet", false, false, false));
			}
			float aimDirection = base.AimDirection;
			if (BraveMathCollege.AbsAngleBetween(aimDirection, -90f) <= 90f)
			{
				base.Fire(new Direction(base.AimDirection, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("ricochet", false, false, false));
			}
		}
		else
		{
			int num5 = 8;
			float num6 = -45f;
			float num7 = 90f / (float)(num5 - 1);
			for (int j = 0; j < num5; j++)
			{
				base.Fire(new Direction(num6 + (float)j * num7, DirectionType.Relative, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("ricochet", false, false, false));
			}
		}
		return null;
	}
}
