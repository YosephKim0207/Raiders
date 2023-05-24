using System;
using System.Collections;
using Brave.BulletScript;

// Token: 0x020001BA RID: 442
public class GripMasterRing1 : Script
{
	// Token: 0x06000693 RID: 1683 RVA: 0x0001FB60 File Offset: 0x0001DD60
	protected override IEnumerator Top()
	{
		float aimDirection = base.AimDirection;
		int num = 16;
		for (int i = 0; i < num; i++)
		{
			base.Fire(new Direction(base.SubdivideCircle(aimDirection, num, i, 1f, false), DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), null);
		}
		float num2 = 135f;
		float num3 = aimDirection - num2 / 2f;
		num = 7;
		for (int j = 0; j < num - 1; j++)
		{
			base.Fire(new Direction(base.SubdivideArc(num3, num2, num, j, true), DirectionType.Absolute, -1f), new Speed(17f, SpeedType.Absolute), new SpeedChangingBullet(9f, 30, -1));
		}
		for (int k = 0; k < num - 1; k++)
		{
			base.Fire(new Direction(base.SubdivideArc(num3, num2, num, k, true), DirectionType.Absolute, -1f), new Speed(1f, SpeedType.Absolute), new SpeedChangingBullet(9f, 30, -1));
		}
		return null;
	}
}
