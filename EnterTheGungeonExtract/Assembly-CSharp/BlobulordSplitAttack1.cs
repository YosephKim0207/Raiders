using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x02000046 RID: 70
[InspectorDropdownName("Bosses/Blobulord/SplitAttack1")]
public class BlobulordSplitAttack1 : Script
{
	// Token: 0x06000108 RID: 264 RVA: 0x0000601C File Offset: 0x0000421C
	protected override IEnumerator Top()
	{
		for (int j = 0; j < 32; j++)
		{
			float num = (float)j * 11.25f;
			base.Fire(new Offset(1f, 0f, num, string.Empty, DirectionType.Absolute), new Direction(num, DirectionType.Absolute, -1f), new Speed(UnityEngine.Random.Range(4f, 11f), SpeedType.Absolute), new BlobulordSplitAttack1.BlobulonBullet(base.Position, 0, false));
		}
		for (int i = 0; i < 10; i++)
		{
			float angle = base.RandomAngle();
			base.Fire(new Offset(UnityEngine.Random.Range(0f, 1.5f), 0f, angle, string.Empty, DirectionType.Absolute), new Direction(angle, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new BlobulordSplitAttack1.BlobulonBullet(base.Position, i * 30, true));
			yield return base.Wait(30);
		}
		yield break;
	}

	// Token: 0x0400010B RID: 267
	private const int NumBullets = 32;

	// Token: 0x0400010C RID: 268
	private const int TotalTime = 352;

	// Token: 0x0400010D RID: 269
	private const float BulletSpeed = 10f;

	// Token: 0x02000047 RID: 71
	public class BlobulonBullet : Bullet
	{
		// Token: 0x06000109 RID: 265 RVA: 0x00006038 File Offset: 0x00004238
		public BlobulonBullet(Vector2 spawnPoint, int spawnDelay = 0, bool doSpawn = false)
			: base(null, false, false, false)
		{
			this.BankName = BraveUtility.RandomElement<string>(BlobulordSplitAttack1.BlobulonBullet.Projectiles);
			this.m_spawnPoint = spawnPoint;
			this.m_spawnDelay = spawnDelay;
			this.m_doSpawn = doSpawn;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000606C File Offset: 0x0000426C
		protected override IEnumerator Top()
		{
			tk2dSpriteAnimator spriteAnimator = this.Projectile.spriteAnimator;
			if (this.m_doSpawn)
			{
				this.Projectile.specRigidbody.CollideWithOthers = false;
				tk2dSpriteAnimationClip spawnClip = spriteAnimator.GetClipByName(this.BankName + "_projectile_spawn");
				spriteAnimator.Play(spawnClip);
				while (spriteAnimator.IsPlaying(spawnClip))
				{
					yield return base.Wait(1);
				}
				this.Projectile.specRigidbody.CollideWithOthers = true;
			}
			else
			{
				spriteAnimator.Play();
			}
			int timeRemaining = 352 - this.m_spawnDelay;
			SpeculativeRigidbody specRigidbody = this.Projectile.specRigidbody;
			specRigidbody.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Combine(specRigidbody.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.OnTileCollision));
			this.Projectile.BulletScriptSettings.surviveTileCollisions = true;
			base.ChangeSpeed(new Speed(10f, SpeedType.Absolute), 30);
			while (timeRemaining > 100)
			{
				if (timeRemaining == 145)
				{
					base.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 45);
				}
				timeRemaining--;
				yield return base.Wait(1);
			}
			if (Vector2.Distance(this.m_spawnPoint, base.Position) > 20f)
			{
				this.Speed = 0f;
				spriteAnimator.Play(this.BankName + "_projectile_impact");
				tk2dSpriteAnimator tk2dSpriteAnimator = spriteAnimator;
				tk2dSpriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(tk2dSpriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnAnimationCompleted));
				yield break;
			}
			Vector2 startVelocity = this.Velocity;
			base.ManualControl = true;
			while (timeRemaining > 50)
			{
				Vector2 goalVelocity = (this.m_spawnPoint - base.Position) / ((float)timeRemaining / 60f);
				this.Velocity = Vector2.Lerp(startVelocity, goalVelocity, (float)(100 - timeRemaining) / 50f);
				base.Position += this.Velocity / 60f;
				timeRemaining--;
				yield return base.Wait(1);
			}
			SpeculativeRigidbody specRigidbody2 = this.Projectile.specRigidbody;
			specRigidbody2.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Remove(specRigidbody2.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.OnTileCollision));
			Vector2 goal = this.m_spawnPoint + (base.Position - this.m_spawnPoint).normalized * UnityEngine.Random.Range(0.5f, 2f);
			this.Direction = (goal - base.Position).ToAngle();
			this.Speed = (goal - base.Position).magnitude / 50f * 60f;
			base.ManualControl = false;
			yield return base.Wait(50);
			this.Speed = 0f;
			spriteAnimator.Play(this.BankName + "_projectile_impact");
			tk2dSpriteAnimator tk2dSpriteAnimator2 = spriteAnimator;
			tk2dSpriteAnimator2.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(tk2dSpriteAnimator2.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.OnAnimationCompleted));
			yield break;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006088 File Offset: 0x00004288
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

		// Token: 0x0600010C RID: 268 RVA: 0x00006108 File Offset: 0x00004308
		private void OnAnimationCompleted(tk2dSpriteAnimator tk2DSpriteAnimator, tk2dSpriteAnimationClip tk2DSpriteAnimationClip)
		{
			base.Vanish(true);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00006114 File Offset: 0x00004314
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

		// Token: 0x0400010E RID: 270
		private static string[] Projectiles = new string[] { "blobulon", "blobulon", "blobuloid" };

		// Token: 0x0400010F RID: 271
		private Vector2 m_spawnPoint;

		// Token: 0x04000110 RID: 272
		private int m_spawnDelay;

		// Token: 0x04000111 RID: 273
		private bool m_doSpawn;
	}
}
