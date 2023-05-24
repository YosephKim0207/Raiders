using System;
using System.Collections;
using Brave.BulletScript;
using FullInspector;
using UnityEngine;

// Token: 0x020002A3 RID: 675
[InspectorDropdownName("Bosses/MineFlayer/SoundWaves1")]
public class MineFlayerSoundWaves1 : Script
{
	// Token: 0x06000A57 RID: 2647 RVA: 0x000320C8 File Offset: 0x000302C8
	protected override IEnumerator Top()
	{
		float delta = 20f;
		for (int i = 0; i < 5; i++)
		{
			yield return base.Wait(33);
			int numBullets = 18;
			float startDirection = base.RandomAngle();
			if (i == 4)
			{
				numBullets /= 2;
				delta *= 2f;
			}
			for (int j = 0; j < numBullets; j++)
			{
				base.Fire(new Direction(startDirection + (float)j * delta, DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), new MineFlayerSoundWaves1.ReflectBullet());
			}
			yield return base.Wait(12);
		}
		yield break;
	}

	// Token: 0x04000AC7 RID: 2759
	private const int NumWaves = 5;

	// Token: 0x04000AC8 RID: 2760
	private const int NumBullets = 18;

	// Token: 0x020002A4 RID: 676
	private class ReflectBullet : Bullet
	{
		// Token: 0x06000A58 RID: 2648 RVA: 0x000320E4 File Offset: 0x000302E4
		public ReflectBullet()
			: base("bounce", false, false, false)
		{
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x000320FC File Offset: 0x000302FC
		protected override IEnumerator Top()
		{
			SpeculativeRigidbody specRigidbody = this.Projectile.specRigidbody;
			specRigidbody.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Combine(specRigidbody.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.OnTileCollision));
			this.Projectile.BulletScriptSettings.surviveTileCollisions = true;
			while (this.m_ticksLeft < 0)
			{
				if (base.ManualControl)
				{
					this.Reflect();
					base.ManualControl = false;
				}
				yield return base.Wait(1);
			}
			yield return base.Wait(this.m_ticksLeft);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00032118 File Offset: 0x00030318
		private void OnTileCollision(CollisionData tilecollision)
		{
			this.Reflect();
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00032120 File Offset: 0x00030320
		private void Reflect()
		{
			this.Speed = 8f;
			this.Direction += 180f + UnityEngine.Random.Range(-10f, 10f);
			this.Velocity = BraveMathCollege.DegreesToVector(this.Direction, this.Speed);
			PhysicsEngine.PostSliceVelocity = new Vector2?(this.Velocity);
			this.m_ticksLeft = (int)((float)base.Tick * 1.5f);
			if (this.Projectile.TrailRendererController)
			{
				this.Projectile.TrailRendererController.Stop();
			}
			this.Projectile.BulletScriptSettings.surviveTileCollisions = false;
			SpeculativeRigidbody specRigidbody = this.Projectile.specRigidbody;
			specRigidbody.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Remove(specRigidbody.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.OnTileCollision));
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x000321F8 File Offset: 0x000303F8
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (this.Projectile)
			{
				SpeculativeRigidbody specRigidbody = this.Projectile.specRigidbody;
				specRigidbody.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Remove(specRigidbody.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.OnTileCollision));
			}
		}

		// Token: 0x04000AC9 RID: 2761
		private int m_ticksLeft = -1;
	}
}
