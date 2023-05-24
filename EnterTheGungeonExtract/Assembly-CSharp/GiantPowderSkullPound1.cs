using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001B4 RID: 436
[InspectorDropdownName("Bosses/GiantPowderSkull/Pound1")]
public class GiantPowderSkullPound1 : Script
{
	// Token: 0x0600067E RID: 1662 RVA: 0x0001F25C File Offset: 0x0001D45C
	protected override IEnumerator Top()
	{
		int num = BraveUtility.SequentialRandomRange(0, 4, GiantPowderSkullPound1.s_lastPatternNum, null, true);
		GiantPowderSkullPound1.s_lastPatternNum = num;
		switch (num)
		{
		case 0:
		{
			float num2 = base.AimDirection - 48f;
			for (int i = 0; i < 9; i++)
			{
				float num3 = num2 + (float)i * 12f;
				base.Fire(new Offset(new Vector2(1f, 0f), num3, string.Empty, DirectionType.Absolute), new Direction(num3, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new Bullet("default_ground", false, false, false));
			}
			break;
		}
		case 1:
		{
			float num2 = base.AimDirection - 48f + UnityEngine.Random.Range(-35f, 35f);
			for (int j = 0; j < 9; j++)
			{
				float num4 = num2 + (float)j * 12f;
				base.Fire(new Offset(new Vector2(1f, 0f), num4, string.Empty, DirectionType.Absolute), new Direction(num4, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new Bullet("default_ground", false, false, false));
			}
			break;
		}
		case 2:
		{
			float num2 = base.RandomAngle();
			for (int k = 0; k < 12; k++)
			{
				float num5 = num2 + (float)k * 30f;
				base.Fire(new Offset(new Vector2(1f, 0f), num5, string.Empty, DirectionType.Absolute), new Direction(num5, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new Bullet("default_ground", false, false, false));
			}
			break;
		}
		case 3:
		{
			float num2 = base.RandomAngle();
			for (int l = 0; l < 36; l++)
			{
				float num6 = num2 + (float)l * 10f;
				base.Fire(new Offset(new Vector2(1f, 0f), num6, string.Empty, DirectionType.Absolute), new Direction(num6, DirectionType.Absolute, -1f), new Speed(8f, SpeedType.Absolute), new Bullet("default_ground", false, false, false));
			}
			break;
		}
		}
		return null;
	}

	// Token: 0x04000657 RID: 1623
	private const float WaveSeparation = 12f;

	// Token: 0x04000658 RID: 1624
	private const float OffsetDist = 1f;

	// Token: 0x04000659 RID: 1625
	private static int s_lastPatternNum;
}
