using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020000D1 RID: 209
[InspectorDropdownName("Bosses/BulletBros/JumpBurst2")]
public class BulletBrosJumpBurst2 : Script
{
	// Token: 0x0600032C RID: 812 RVA: 0x0001050C File Offset: 0x0000E70C
	protected override IEnumerator Top()
	{
		float num = base.RandomAngle();
		for (int i = 0; i < 18; i++)
		{
			base.Fire(new Direction(base.SubdivideCircle(num, 18, i, 1f, false), DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("jump", true, false, false));
		}
		num += 10f;
		for (int j = 0; j < 9; j++)
		{
			base.Fire(new Direction(base.SubdivideCircle(num, 9, j, 1f, false), DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new SpeedChangingBullet("jump", 9f, 75, -1, true));
		}
		return null;
	}

	// Token: 0x0400034E RID: 846
	private const int NumFastBullets = 18;

	// Token: 0x0400034F RID: 847
	private const int NumSlowBullets = 9;
}
