using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020000D0 RID: 208
[InspectorDropdownName("Bosses/BulletBros/JumpBurst1")]
public class BulletBrosJumpBurst1 : Script
{
	// Token: 0x0600032A RID: 810 RVA: 0x000104A4 File Offset: 0x0000E6A4
	protected override IEnumerator Top()
	{
		float num = base.RandomAngle();
		float num2 = 30f;
		for (int i = 0; i < 12; i++)
		{
			base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("jump", true, false, false));
		}
		return null;
	}

	// Token: 0x0400034D RID: 845
	private const int NumBullets = 12;
}
