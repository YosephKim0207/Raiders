using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020000E5 RID: 229
[InspectorDropdownName("Bosses/BulletKing/CrazySpin1")]
public class BulletKingCrazySpin1 : Script
{
	// Token: 0x170000CD RID: 205
	// (get) Token: 0x06000378 RID: 888 RVA: 0x000115B8 File Offset: 0x0000F7B8
	protected bool IsHard
	{
		get
		{
			return this is BulletKingCrazySpinHard1;
		}
	}

	// Token: 0x06000379 RID: 889 RVA: 0x000115C4 File Offset: 0x0000F7C4
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 29; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				float num = (float)j * 60f + 37f * (float)i;
				base.Fire(new Offset(1.66f, 0f, num, string.Empty, DirectionType.Absolute), new Direction(num, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new Bullet("default_novfx", false, false, false));
			}
			yield return base.Wait(6);
		}
		for (int k = 0; k < 64; k++)
		{
			base.Fire(new Direction((float)k * 360f / 64f, DirectionType.Absolute, -1f), new Speed((float)((!this.IsHard) ? 8 : 13), SpeedType.Absolute), new Bullet("default_novfx", false, false, false));
		}
		yield break;
	}

	// Token: 0x04000386 RID: 902
	private const int NumWaves = 29;

	// Token: 0x04000387 RID: 903
	private const int NumBulletsPerWave = 6;

	// Token: 0x04000388 RID: 904
	private const float AngleDeltaEachWave = 37f;

	// Token: 0x04000389 RID: 905
	private const int NumBulletsFinalWave = 64;
}
