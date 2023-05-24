using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x02000335 RID: 821
public class ZombulletBurst : Script
{
	// Token: 0x06000CBC RID: 3260 RVA: 0x0003D414 File Offset: 0x0003B614
	protected override IEnumerator Top()
	{
		float num = base.RandomAngle();
		float num2 = 20f;
		for (int i = 0; i < 18; i++)
		{
			base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Absolute), new ZombulletBurst.OscillatingBullet());
		}
		return null;
	}

	// Token: 0x04000D63 RID: 3427
	private const int NumBullets = 18;

	// Token: 0x02000336 RID: 822
	private class OscillatingBullet : Bullet
	{
		// Token: 0x06000CBD RID: 3261 RVA: 0x0003D46C File Offset: 0x0003B66C
		public OscillatingBullet()
			: base(null, false, false, false)
		{
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x0003D478 File Offset: 0x0003B678
		protected override IEnumerator Top()
		{
			float randomOffset = UnityEngine.Random.value;
			float startSpeed = this.Speed;
			for (int i = 0; i < 300; i++)
			{
				this.Speed = startSpeed + Mathf.SmoothStep(-2f, 2f, Mathf.PingPong((float)base.Tick / 60f + randomOffset, 1f));
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}
	}
}
