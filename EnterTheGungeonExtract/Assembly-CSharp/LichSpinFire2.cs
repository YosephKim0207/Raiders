using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x0200022C RID: 556
[InspectorDropdownName("Bosses/Lich/SpinFire2")]
public class LichSpinFire2 : Script
{
	// Token: 0x06000859 RID: 2137 RVA: 0x000285AC File Offset: 0x000267AC
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 6; i++)
		{
			for (int k = 0; k < 48; k++)
			{
				float num = (float)k * 7.5f;
				base.Fire(new Offset(1f, 0f, num, string.Empty, DirectionType.Absolute), new Direction(num, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new Bullet("twirl", false, false, false));
			}
			yield return base.Wait(6);
			for (int l = 0; l < 48; l++)
			{
				float num2 = ((float)l + 0.5f) * 7.5f;
				base.Fire(new Offset(1f, 0f, num2, string.Empty, DirectionType.Absolute), new Direction(num2, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new Bullet("twirl", false, false, false));
			}
			for (int j = 0; j < 20; j++)
			{
				float direction = base.RandomAngle();
				base.Fire(new Offset(1f, 0f, direction, string.Empty, DirectionType.Absolute), new Direction(direction, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new Bullet("twirl", false, false, false));
				yield return base.Wait(3);
			}
		}
		yield break;
	}

	// Token: 0x04000857 RID: 2135
	private const int NumWaves = 6;

	// Token: 0x04000858 RID: 2136
	private const int NumBulletsPerWave = 48;
}
