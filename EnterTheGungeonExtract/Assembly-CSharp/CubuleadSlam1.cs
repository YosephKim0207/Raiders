using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x02000140 RID: 320
public class CubuleadSlam1 : Script
{
	// Token: 0x060004C1 RID: 1217 RVA: 0x00017178 File Offset: 0x00015378
	protected override IEnumerator Top()
	{
		this.FireLine(45f);
		this.FireLine(135f);
		this.FireLine(225f);
		this.FireLine(315f);
		return null;
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x000171A8 File Offset: 0x000153A8
	private void FireLine(float startingAngle)
	{
		float num = 9f;
		for (int i = 0; i < 11; i++)
		{
			float num2 = Mathf.Atan((-45f + (float)i * num) / 45f) * 57.29578f;
			float num3 = Mathf.Cos(num2 * 0.017453292f);
			float num4 = (((double)Mathf.Abs(num3) >= 0.0001) ? (1f / num3) : 1f);
			base.Fire(new Direction(num2 + startingAngle, DirectionType.Absolute, -1f), new Speed(num4 * 9f, SpeedType.Absolute), new CubuleadSlam1.ReversingBullet());
		}
	}

	// Token: 0x0400049D RID: 1181
	private const int NumBullets = 11;

	// Token: 0x02000141 RID: 321
	public class ReversingBullet : Bullet
	{
		// Token: 0x060004C3 RID: 1219 RVA: 0x00017248 File Offset: 0x00015448
		public ReversingBullet()
			: base("reversible", false, false, false)
		{
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00017258 File Offset: 0x00015458
		protected override IEnumerator Top()
		{
			if (base.BulletBank && base.BulletBank.healthHaver)
			{
				base.BulletBank.healthHaver.OnPreDeath += this.OnPreDeath;
			}
			float speed = this.Speed;
			yield return base.Wait(40);
			base.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 20);
			yield return base.Wait(20);
			this.Direction += 180f;
			this.Projectile.spriteAnimator.Play();
			yield return base.Wait(60);
			base.ChangeSpeed(new Speed(speed, SpeedType.Absolute), 40);
			yield return base.Wait(70);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00017274 File Offset: 0x00015474
		private void OnPreDeath(Vector2 deathDir)
		{
			base.Vanish(false);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00017280 File Offset: 0x00015480
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (base.BulletBank && base.BulletBank.healthHaver)
			{
				base.BulletBank.healthHaver.OnPreDeath -= this.OnPreDeath;
			}
		}
	}
}
