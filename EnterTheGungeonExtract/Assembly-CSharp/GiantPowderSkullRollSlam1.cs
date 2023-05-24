using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020001B5 RID: 437
[InspectorDropdownName("Bosses/GiantPowderSkull/RollSlam1")]
public class GiantPowderSkullRollSlam1 : Script
{
	// Token: 0x06000681 RID: 1665 RVA: 0x0001F4A4 File Offset: 0x0001D6A4
	protected override IEnumerator Top()
	{
		AkSoundEngine.PostEvent("Play_BOSS_doormimic_blast_01", base.BulletBank.gameObject);
		float startDirection = 0f;
		for (int i = 0; i < 30; i++)
		{
			float num = startDirection + (float)(i * 12);
			base.Fire(new Offset(new Vector2(1.5f, 0f), num, string.Empty, DirectionType.Absolute), new Direction(num, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new SpeedChangingBullet("slam", 12f, 180, -1, false));
		}
		yield return base.Wait(5);
		startDirection = 3f;
		for (int j = 0; j < 60; j++)
		{
			float num2 = startDirection + (float)(j * 6);
			base.Fire(new Offset(new Vector2(1.5f, 0f), num2, string.Empty, DirectionType.Absolute), new Direction(num2, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new SpeedChangingBullet("slam", 12f, 180, -1, false));
		}
		yield return base.Wait(5);
		startDirection = 6f;
		for (int k = 0; k < 30; k++)
		{
			float num3 = startDirection + (float)(k * 12);
			base.Fire(new Offset(new Vector2(1.5f, 0f), num3, string.Empty, DirectionType.Absolute), new Direction(num3, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new SpeedChangingBullet("slam", 12f, 180, -1, false));
		}
		yield break;
	}

	// Token: 0x0400065A RID: 1626
	private const float OffsetDist = 1.5f;
}
