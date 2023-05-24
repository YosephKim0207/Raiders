using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;

// Token: 0x020000A1 RID: 161
[InspectorDropdownName("Bosses/BossFinalRogue/BigGun1")]
public class BossFinalRogueBigGun1 : Script
{
	// Token: 0x0600027B RID: 635 RVA: 0x0000CC90 File Offset: 0x0000AE90
	protected override IEnumerator Top()
	{
		float delta = 13.846154f;
		for (int i = 0; i < 4; i++)
		{
			int num = 0;
			while ((float)num < 26f)
			{
				base.Fire(new Offset("big gun left cannon"), new Direction(-90f + delta * (float)num + (float)(i * 2), DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("bigGunSlow", false, false, false));
				num++;
			}
			yield return base.Wait(22);
			int num2 = 0;
			while ((float)num2 < 26f)
			{
				base.Fire(new Offset("big gun right cannon"), new Direction(-90f + delta * (float)num2 + (float)(i * 2), DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("bigGunSlow", false, false, false));
				num2++;
			}
			yield return base.Wait(23);
		}
		yield return base.Wait(56);
		int num3 = 0;
		while ((float)num3 < 44f)
		{
			base.Fire(new Offset("big gun left cannon"), new Direction(-90f + delta * (float)num3, DirectionType.Absolute, -1f), new Speed(18f, SpeedType.Absolute), new Bullet("bigGunFast", false, false, false));
			base.Fire(new Offset("big gun right cannon"), new Direction(-90f + delta * (float)num3, DirectionType.Absolute, -1f), new Speed(18f, SpeedType.Absolute), new Bullet("bigGunFast", false, false, false));
			num3++;
		}
		yield break;
	}

	// Token: 0x040002AE RID: 686
	private const float NumBullets = 26f;

	// Token: 0x040002AF RID: 687
	private const float NumFastBullets = 44f;
}
