using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x020002EE RID: 750
public class ShopkeepBlast1 : Script
{
	// Token: 0x06000BA3 RID: 2979 RVA: 0x00039170 File Offset: 0x00037370
	protected override IEnumerator Top()
	{
		this.FireBurst("left barrel");
		this.FireBurst("right barrel");
		this.QuadShot(base.AimDirection + UnityEngine.Random.Range(-60f, 60f), (!BraveUtility.RandomBool()) ? "right barrel" : "left barrel", UnityEngine.Random.Range(9f, 11f));
		return null;
	}

	// Token: 0x06000BA4 RID: 2980 RVA: 0x000391D8 File Offset: 0x000373D8
	private void FireBurst(string transform)
	{
		float num = 22.5f;
		float num2 = UnityEngine.Random.Range(-num / 2f, num / 2f);
		for (int i = 0; i < 16; i++)
		{
			Offset offset = new Offset(transform);
			Direction direction = new Direction(num2 + (float)i * num, DirectionType.Relative, -1f);
			Speed speed = new Speed(9f, SpeedType.Absolute);
			bool flag = i > 0;
			base.Fire(offset, direction, speed, new Bullet(null, flag, false, false));
		}
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x0003924C File Offset: 0x0003744C
	private void QuadShot(float direction, string transform, float speed)
	{
		for (int i = 0; i < 4; i++)
		{
			Bullet bullet;
			if (i == 0)
			{
				bullet = new ShopkeepBlast1.BurstBullet("burstBullet", speed, 120, true);
			}
			else
			{
				bullet = new SpeedChangingBullet("bigBullet", speed, 120, -1, true);
			}
			base.Fire(new Offset(transform), new Direction(direction, DirectionType.Absolute, -1f), new Speed(speed - (float)i * 1.5f, SpeedType.Absolute), bullet);
		}
	}

	// Token: 0x04000C7B RID: 3195
	private const int NumBulletsInBurst = 16;

	// Token: 0x020002EF RID: 751
	private class BurstBullet : Bullet
	{
		// Token: 0x06000BA6 RID: 2982 RVA: 0x000392C0 File Offset: 0x000374C0
		public BurstBullet(string name, float newSpeed, int term, bool suppressVfx)
			: base(name, suppressVfx, false, false)
		{
			this.m_newSpeed = newSpeed;
			this.m_term = term;
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x000392DC File Offset: 0x000374DC
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(this.m_newSpeed, SpeedType.Absolute), this.m_term);
			return null;
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x000392F8 File Offset: 0x000374F8
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (preventSpawningProjectiles)
			{
				return;
			}
			float num = 22.5f;
			float num2 = UnityEngine.Random.Range(-num / 2f, num / 2f);
			for (int i = 0; i < 16; i++)
			{
				base.Fire(new Direction(num2 + (float)i * num, DirectionType.Relative, -1f), new Speed(9f, SpeedType.Absolute), new Bullet(null, true, false, false));
			}
		}

		// Token: 0x04000C7C RID: 3196
		private float m_newSpeed;

		// Token: 0x04000C7D RID: 3197
		private int m_term;
	}
}
