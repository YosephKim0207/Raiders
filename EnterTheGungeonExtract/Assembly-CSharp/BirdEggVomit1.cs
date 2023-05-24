using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200002A RID: 42
[InspectorDropdownName("Bird/EggVomit1")]
public class BirdEggVomit1 : Script
{
	// Token: 0x0600009C RID: 156 RVA: 0x000044C8 File Offset: 0x000026C8
	protected override IEnumerator Top()
	{
		float num = BraveMathCollege.ClampAngle360(this.Direction);
		float num2 = (float)((num <= 90f || num > 180f) ? 20 : 160);
		base.Fire(new Direction(num2, DirectionType.Absolute, -1f), new Speed(2f, SpeedType.Absolute), new BirdEggVomit1.EggBullet());
		return null;
	}

	// Token: 0x0400009D RID: 157
	private const int NumBullets = 36;

	// Token: 0x0200002B RID: 43
	public class EggBullet : Bullet
	{
		// Token: 0x0600009D RID: 157 RVA: 0x00004528 File Offset: 0x00002728
		public EggBullet()
			: base("egg", false, false, false)
		{
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004538 File Offset: 0x00002738
		protected override IEnumerator Top()
		{
			this.Projectile.sprite.SetSprite("egg_projectile_001");
			float startRotation = (float)((this.Direction <= 90f || this.Direction >= 270f) ? 135 : (-135));
			for (int i = 0; i < 45; i++)
			{
				Vector2 velocity = BraveMathCollege.DegreesToVector(this.Direction, this.Speed);
				velocity += new Vector2(0f, -7f) / 60f;
				this.Direction = velocity.ToAngle();
				this.Speed = velocity.magnitude;
				this.Projectile.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(startRotation, 0f, (float)i / 45f));
				yield return base.Wait(1);
			}
			this.Projectile.transform.rotation = Quaternion.identity;
			this.Speed = 0f;
			this.Projectile.spriteAnimator.Play();
			int animTime = Mathf.RoundToInt(this.Projectile.spriteAnimator.DefaultClip.BaseClipLength * 60f);
			yield return base.Wait(animTime / 2);
			if (!this.spawnedBursts)
			{
				this.SpawnBursts();
			}
			yield return base.Wait(animTime / 2);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004554 File Offset: 0x00002754
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (!this.spawnedBursts && !preventSpawningProjectiles)
			{
				this.SpawnBursts();
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004570 File Offset: 0x00002770
		private void SpawnBursts()
		{
			float num = base.RandomAngle();
			float num2 = 10f;
			for (int i = 0; i < 36; i++)
			{
				base.Fire(new Direction(num + (float)i * num2, DirectionType.Absolute, -1f), new Speed(9f, SpeedType.Absolute), new BirdEggVomit1.AcceleratingBullet());
			}
			num += num2 / 2f;
			for (int j = 0; j < 36; j++)
			{
				base.Fire(new Direction(num + (float)j * num2, DirectionType.Absolute, -1f), new Speed(5f, SpeedType.Absolute), new BirdEggVomit1.AcceleratingBullet());
			}
			num += num2 / 2f;
			for (int k = 0; k < 36; k++)
			{
				base.Fire(new Direction(num + (float)k * num2, DirectionType.Absolute, -1f), new Speed(1f, SpeedType.Absolute), new BirdEggVomit1.AcceleratingBullet());
			}
			this.spawnedBursts = true;
		}

		// Token: 0x0400009E RID: 158
		private bool spawnedBursts;
	}

	// Token: 0x0200002D RID: 45
	public class AcceleratingBullet : Bullet
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x00004924 File Offset: 0x00002B24
		public AcceleratingBullet()
			: base(null, false, false, false)
		{
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004930 File Offset: 0x00002B30
		protected override IEnumerator Top()
		{
			base.ChangeSpeed(new Speed(9f, SpeedType.Absolute), 180);
			return null;
		}
	}
}
