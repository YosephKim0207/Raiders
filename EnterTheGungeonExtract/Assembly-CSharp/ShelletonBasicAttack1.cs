using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x020002EC RID: 748
public class ShelletonBasicAttack1 : Script
{
	// Token: 0x06000B9F RID: 2975 RVA: 0x00039028 File Offset: 0x00037228
	protected override IEnumerator Top()
	{
		for (int i = 0; i < 21; i++)
		{
			float num = Mathf.Lerp(-80f, 80f, (float)i / 20f);
			base.Fire(new Direction(num, DirectionType.Aim, -1f), new Speed((float)((i % 2 != 0) ? 10 : 4), SpeedType.Absolute), new SpeedChangingBullet(10f, 60, 180));
		}
		for (int j = 0; j < 2; j++)
		{
			int num2 = UnityEngine.Random.Range(0, 21);
			float num3 = Mathf.Lerp(-80f, 80f, (float)num2 / 20f);
			base.Fire(new Direction(num3, DirectionType.Aim, -1f), new Speed((float)((num2 % 2 != 1) ? 10 : 4), SpeedType.Absolute), new SpeedChangingBullet(10f, 60, 180));
		}
		return null;
	}

	// Token: 0x04000C78 RID: 3192
	private const int NumBullets = 21;

	// Token: 0x04000C79 RID: 3193
	private const int NumPlugs = 2;
}
