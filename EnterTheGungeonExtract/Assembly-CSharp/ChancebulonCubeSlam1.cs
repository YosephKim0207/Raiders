using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000137 RID: 311
[InspectorDropdownName("Chancebulon/CubeSlam1")]
public class ChancebulonCubeSlam1 : Script
{
	// Token: 0x06000498 RID: 1176 RVA: 0x000161DC File Offset: 0x000143DC
	protected override IEnumerator Top()
	{
		this.FireLine(45f);
		this.FireLine(135f);
		this.FireLine(225f);
		this.FireLine(315f);
		yield return base.Wait(190);
		yield break;
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x000161F8 File Offset: 0x000143F8
	private void FireLine(float startingAngle)
	{
		float num = 9f;
		for (int i = 0; i < 11; i++)
		{
			float num2 = Mathf.Atan((-45f + (float)i * num) / 45f) * 57.29578f;
			float num3 = Mathf.Cos(num2 * 0.017453292f);
			float num4 = (((double)Mathf.Abs(num3) >= 0.0001) ? (1f / num3) : 1f);
			base.Fire(new Direction(num2 + startingAngle, DirectionType.Absolute, -1f), new Speed(num4 * 9f, SpeedType.Absolute), new ChancebulonCubeSlam1.ReversingBullet());
		}
	}

	// Token: 0x0400047B RID: 1147
	private const int NumBullets = 11;

	// Token: 0x02000138 RID: 312
	public class ReversingBullet : Bullet
	{
		// Token: 0x0600049A RID: 1178 RVA: 0x00016298 File Offset: 0x00014498
		public ReversingBullet()
			: base("reversible", false, false, false)
		{
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x000162A8 File Offset: 0x000144A8
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

		// Token: 0x0600049C RID: 1180 RVA: 0x000162C4 File Offset: 0x000144C4
		private void OnPreDeath(Vector2 deathDir)
		{
			base.Vanish(false);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x000162D0 File Offset: 0x000144D0
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (base.BulletBank && base.BulletBank.healthHaver)
			{
				base.BulletBank.healthHaver.OnPreDeath -= this.OnPreDeath;
			}
		}
	}
}
