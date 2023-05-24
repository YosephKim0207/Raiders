using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000164 RID: 356
[InspectorDropdownName("Bosses/DraGun/FlameBreath2")]
public class DraGunFlameBreath2 : Script
{
	// Token: 0x06000559 RID: 1369 RVA: 0x00019C70 File Offset: 0x00017E70
	protected override IEnumerator Top()
	{
		DraGunFlameBreath2.StopYHeight = base.BulletBank.aiActor.ParentRoom.area.UnitBottomLeft.y + 21f;
		int pocketResetTimer = 0;
		float pocketAngle = 0f;
		float pocketSign = BraveUtility.RandomSign();
		for (int i = 0; i < 120; i++)
		{
			if (i % 40 == 27)
			{
				for (int j = 0; j < 12; j++)
				{
					base.Fire(new Direction(base.SubdivideArc(-30f, 60f, 12, j, false), DirectionType.Aim, -1f), new Speed(14f, SpeedType.Absolute), new DraGunFlameBreath2.FlameBullet());
				}
			}
			float direction = UnityEngine.Random.Range(-30f, 30f);
			if (pocketResetTimer == 0)
			{
				pocketAngle = pocketSign * UnityEngine.Random.Range(0f, 15f);
				pocketSign *= -1f;
				pocketResetTimer = 30;
			}
			pocketResetTimer--;
			if (direction >= pocketAngle - 5f && direction <= pocketAngle)
			{
				direction -= 5f;
			}
			else if (direction <= pocketAngle + 5f && direction >= pocketAngle)
			{
				direction += 5f;
			}
			base.Fire(new Direction(direction, DirectionType.Aim, -1f), new Speed(14f, SpeedType.Absolute), new DraGunFlameBreath2.FlameBullet());
			yield return base.Wait(2);
		}
		yield break;
	}

	// Token: 0x04000538 RID: 1336
	private const int NumBullets = 120;

	// Token: 0x04000539 RID: 1337
	private const int NumWaveBullets = 12;

	// Token: 0x0400053A RID: 1338
	private const float Spread = 30f;

	// Token: 0x0400053B RID: 1339
	private const int PocketResetTime = 30;

	// Token: 0x0400053C RID: 1340
	private const float PocketWidth = 5f;

	// Token: 0x0400053D RID: 1341
	protected static float StopYHeight;

	// Token: 0x02000165 RID: 357
	public class FlameBullet : Bullet
	{
		// Token: 0x0600055A RID: 1370 RVA: 0x00019C8C File Offset: 0x00017E8C
		public FlameBullet()
			: base("Breath", false, false, false)
		{
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00019C9C File Offset: 0x00017E9C
		protected override IEnumerator Top()
		{
			while (base.Position.y > DraGunFlameBreath2.StopYHeight)
			{
				yield return base.Wait(1);
			}
			base.ChangeSpeed(new Speed(0.33f, SpeedType.Absolute), 12);
			yield return base.Wait(60);
			base.Vanish(false);
			yield break;
		}
	}
}
