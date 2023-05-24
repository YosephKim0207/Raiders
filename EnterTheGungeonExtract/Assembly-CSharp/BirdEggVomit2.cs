using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200002E RID: 46
[InspectorDropdownName("Bird/EggVomit2")]
public class BirdEggVomit2 : Script
{
	// Token: 0x060000AA RID: 170 RVA: 0x00004954 File Offset: 0x00002B54
	protected override IEnumerator Top()
	{
		float num = BraveMathCollege.ClampAngle360(this.Direction);
		float num2 = (float)((num <= 90f || num > 180f) ? 20 : 160);
		base.Fire(new Direction(num2, DirectionType.Absolute, -1f), new Speed(2f, SpeedType.Absolute), new BirdEggVomit2.EggBullet());
		return null;
	}

	// Token: 0x040000A7 RID: 167
	private const int ClusterBullets = 0;

	// Token: 0x040000A8 RID: 168
	private const float ClusterRotation = 150f;

	// Token: 0x040000A9 RID: 169
	private const float ClusterRadius = 0.5f;

	// Token: 0x040000AA RID: 170
	private const float ClusterRadiusSpeed = 2f;

	// Token: 0x040000AB RID: 171
	private const int InnerBullets = 12;

	// Token: 0x040000AC RID: 172
	private const int InnerBounceTime = 30;

	// Token: 0x0200002F RID: 47
	public class EggBullet : Bullet
	{
		// Token: 0x060000AB RID: 171 RVA: 0x000049B4 File Offset: 0x00002BB4
		public EggBullet()
			: base("egg", false, false, false)
		{
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000049C4 File Offset: 0x00002BC4
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

		// Token: 0x060000AD RID: 173 RVA: 0x000049E0 File Offset: 0x00002BE0
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (!this.spawnedBursts && !preventSpawningProjectiles)
			{
				this.SpawnBursts();
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000049FC File Offset: 0x00002BFC
		private void SpawnBursts()
		{
			float positiveInfinity = float.PositiveInfinity;
			for (int i = 0; i < 0; i++)
			{
				base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new BirdEggVomit2.ClusterBullet((float)i * positiveInfinity));
			}
			for (int j = 0; j < 12; j++)
			{
				base.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(9f, SpeedType.Absolute), new BirdEggVomit2.InnerBullet());
			}
			this.spawnedBursts = true;
		}

		// Token: 0x040000AD RID: 173
		private bool spawnedBursts;
	}

	// Token: 0x02000031 RID: 49
	public class ClusterBullet : Bullet
	{
		// Token: 0x060000B5 RID: 181 RVA: 0x00004D58 File Offset: 0x00002F58
		public ClusterBullet(float clusterAngle)
			: base(null, false, false, false)
		{
			this.clusterAngle = clusterAngle;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004D6C File Offset: 0x00002F6C
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			Vector2 centerPosition = base.Position;
			float radius = 0.5f;
			for (int i = 0; i < 180; i++)
			{
				base.UpdateVelocity();
				centerPosition += this.Velocity / 60f;
				radius += 0.033333335f;
				this.clusterAngle += 2.5f;
				base.Position = centerPosition + BraveMathCollege.DegreesToVector(this.clusterAngle, radius);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x040000B6 RID: 182
		private float clusterAngle;
	}

	// Token: 0x02000033 RID: 51
	public class InnerBullet : Bullet
	{
		// Token: 0x060000BD RID: 189 RVA: 0x00004F00 File Offset: 0x00003100
		public InnerBullet()
			: base(null, false, false, false)
		{
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00004F0C File Offset: 0x0000310C
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			Vector2 centerPosition = base.Position;
			float radius = 0.5f;
			int bounceDelay = UnityEngine.Random.Range(0, 30);
			Vector2 startOffset = BraveMathCollege.DegreesToVector(base.RandomAngle(), UnityEngine.Random.Range(0f, radius));
			float goalAngle = base.RandomAngle();
			float goalRadiusPercent = UnityEngine.Random.value;
			for (int i = 0; i < 180; i++)
			{
				base.UpdateVelocity();
				centerPosition += this.Velocity / 60f;
				radius += 0.033333335f;
				Vector2 goalOffset = BraveMathCollege.DegreesToVector(goalAngle, goalRadiusPercent * radius);
				if (bounceDelay == 0)
				{
					startOffset = goalOffset;
					goalAngle = base.RandomAngle();
					goalRadiusPercent = UnityEngine.Random.value;
					goalOffset = BraveMathCollege.DegreesToVector(goalAngle, goalRadiusPercent * radius);
					bounceDelay = 30;
					if (radius > 1f)
					{
						bounceDelay = Mathf.RoundToInt(radius * (float)bounceDelay);
					}
				}
				else
				{
					bounceDelay--;
				}
				base.Position = centerPosition + Vector2.Lerp(startOffset, goalOffset, 1f - (float)bounceDelay / 30f);
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}
	}
}
