using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x0200012E RID: 302
[InspectorDropdownName("Chancebulon/BlobProjectileAttack1")]
public class ChancebulonBlobProjectileAttack1 : Script
{
	// Token: 0x06000478 RID: 1144 RVA: 0x000155B8 File Offset: 0x000137B8
	protected override IEnumerator Top()
	{
		ChancebulonBlobProjectileAttack1.BlobType blobType = (ChancebulonBlobProjectileAttack1.BlobType)UnityEngine.Random.Range(0, 3);
		for (int i = 0; i < 5; i++)
		{
			float num = base.RandomAngle();
			base.Fire(new Offset(1f, 0f, num, string.Empty, DirectionType.Absolute), new Direction(num, DirectionType.Absolute, -1f), new Speed(UnityEngine.Random.Range(4f, 11f), SpeedType.Absolute), new ChancebulonBlobProjectileAttack1.BlobulonBullet(blobType));
		}
		return null;
	}

	// Token: 0x04000456 RID: 1110
	private const int NumBullets = 5;

	// Token: 0x04000457 RID: 1111
	private const int TotalTime = 200;

	// Token: 0x04000458 RID: 1112
	public const float BulletSpeed = 10f;

	// Token: 0x0200012F RID: 303
	public enum BlobType
	{
		// Token: 0x0400045A RID: 1114
		Normal,
		// Token: 0x0400045B RID: 1115
		Poison,
		// Token: 0x0400045C RID: 1116
		Lead
	}

	// Token: 0x02000130 RID: 304
	public class BlobulonBullet : Bullet
	{
		// Token: 0x06000479 RID: 1145 RVA: 0x0001562C File Offset: 0x0001382C
		public BlobulonBullet(ChancebulonBlobProjectileAttack1.BlobType blobType)
			: base(null, false, false, false)
		{
			this.BankName = BraveUtility.RandomElement<string>(ChancebulonBlobProjectileAttack1.BlobulonBullet.Projectiles);
			this.m_blobType = blobType;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00015650 File Offset: 0x00013850
		protected override IEnumerator Top()
		{
			tk2dSpriteAnimator spriteAnimator = this.Projectile.spriteAnimator;
			spriteAnimator.Play();
			int timeRemaining = 200;
			SpeculativeRigidbody specRigidbody = this.Projectile.specRigidbody;
			specRigidbody.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Combine(specRigidbody.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.OnTileCollision));
			this.Projectile.BulletScriptSettings.surviveTileCollisions = true;
			base.ChangeSpeed(new Speed(10f, SpeedType.Absolute), 30);
			if (this.m_blobType == ChancebulonBlobProjectileAttack1.BlobType.Poison)
			{
				this.Projectile.GetComponents<GoopDoer>()[1].enabled = true;
			}
			else if (this.m_blobType == ChancebulonBlobProjectileAttack1.BlobType.Lead)
			{
				this.Projectile.GetComponents<GoopDoer>()[0].enabled = true;
			}
			while (timeRemaining > 0)
			{
				if (timeRemaining == 45)
				{
					base.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 45);
				}
				timeRemaining--;
				yield return base.Wait(1);
			}
			SpeculativeRigidbody specRigidbody2 = this.Projectile.specRigidbody;
			specRigidbody2.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Remove(specRigidbody2.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.OnTileCollision));
			this.Speed = 0f;
			spriteAnimator.Play(this.BankName + "_projectile_impact");
			tk2dSpriteAnimator tk2dSpriteAnimator = spriteAnimator;
			tk2dSpriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(tk2dSpriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnAnimationCompleted));
			yield break;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0001566C File Offset: 0x0001386C
		private void OnTileCollision(CollisionData tilecollision)
		{
			float num = (-tilecollision.MyRigidbody.Velocity).ToAngle();
			float num2 = tilecollision.Normal.ToAngle();
			float num3 = BraveMathCollege.ClampAngle360(num + 2f * (num2 - num));
			num3 += UnityEngine.Random.Range(-30f, 30f);
			this.Direction = num3;
			this.Velocity = BraveMathCollege.DegreesToVector(this.Direction, this.Speed);
			PhysicsEngine.PostSliceVelocity = new Vector2?(this.Velocity);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x000156EC File Offset: 0x000138EC
		private void OnAnimationCompleted(tk2dSpriteAnimator tk2DSpriteAnimator, tk2dSpriteAnimationClip tk2DSpriteAnimationClip)
		{
			base.Vanish(true);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x000156F8 File Offset: 0x000138F8
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (this.Projectile)
			{
				SpeculativeRigidbody specRigidbody = this.Projectile.specRigidbody;
				specRigidbody.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Remove(specRigidbody.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.OnTileCollision));
				tk2dSpriteAnimator spriteAnimator = this.Projectile.spriteAnimator;
				spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(spriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnAnimationCompleted));
			}
		}

		// Token: 0x0400045D RID: 1117
		private static string[] Projectiles = new string[] { "blobulon", "blobulon", "blobuloid" };

		// Token: 0x0400045E RID: 1118
		private ChancebulonBlobProjectileAttack1.BlobType m_blobType;
	}
}
