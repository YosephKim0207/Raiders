using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000294 RID: 660
[InspectorDropdownName("Bosses/MineFlayer/BellBursts1")]
public class MineFlayerBurst1 : Script
{
	// Token: 0x06000A1B RID: 2587 RVA: 0x00030CF0 File Offset: 0x0002EEF0
	protected override IEnumerator Top()
	{
		float num = base.RandomAngle();
		float num2 = 15.652174f;
		float num3 = UnityEngine.Random.Range(-3f, 3f);
		for (int i = 0; i < 23; i++)
		{
			base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(6f + num3, SpeedType.Absolute), new SpeedChangingBullet(16f + num3, 60, -1));
		}
		return null;
	}

	// Token: 0x04000A63 RID: 2659
	private const int NumBullets = 23;
}
