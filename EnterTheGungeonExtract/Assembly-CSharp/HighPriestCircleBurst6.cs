using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020001DC RID: 476
[InspectorDropdownName("Bosses/HighPriest/CircleBurst6")]
public class HighPriestCircleBurst6 : Script
{
	// Token: 0x06000720 RID: 1824 RVA: 0x00022B18 File Offset: 0x00020D18
	protected override IEnumerator Top()
	{
		float num = base.RandomAngle();
		float num2 = 60f;
		for (int i = 0; i < 6; i++)
		{
			base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new Bullet("homingPop", false, false, false));
		}
		return null;
	}

	// Token: 0x04000705 RID: 1797
	private const int NumBullets = 6;
}
