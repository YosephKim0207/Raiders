using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x0200021E RID: 542
public class LeadMaidenSustain1 : Script
{
	// Token: 0x06000825 RID: 2085 RVA: 0x00027904 File Offset: 0x00025B04
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		float startDirection = base.RandomAngle();
		float delta = 30f;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 12; j++)
			{
				base.Fire(new Direction(startDirection + (float)j * delta, DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new LeadMaidenSustain1.SpikeBullet(90 + (3 - i) * 30));
			}
			yield return base.Wait(30);
			startDirection += 10f;
		}
		yield return base.Wait(90);
		yield break;
	}

	// Token: 0x0400082A RID: 2090
	private const int NumWaves = 3;

	// Token: 0x0400082B RID: 2091
	private const int NumBullets = 12;

	// Token: 0x0200021F RID: 543
	public class SpikeBullet : Bullet
	{
		// Token: 0x06000826 RID: 2086 RVA: 0x00027920 File Offset: 0x00025B20
		public SpikeBullet(int fireTick)
			: base(null, false, false, false)
		{
			this.m_fireTick = fireTick;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00027934 File Offset: 0x00025B34
		protected override IEnumerator Top()
		{
			this.Projectile.BulletScriptSettings.surviveTileCollisions = true;
			SpeculativeRigidbody specRigidbody = this.Projectile.specRigidbody;
			specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
			while (this.Speed > 0f)
			{
				yield return base.Wait(1);
			}
			float turnSpeed = BraveMathCollege.AbsAngleBetween(this.m_hitNormal, this.Direction) / 30f;
			for (int i = 0; i < 30; i++)
			{
				this.Direction = Mathf.MoveTowardsAngle(this.Direction, this.m_hitNormal, turnSpeed);
				yield return base.Wait(1);
			}
			while (base.Tick < this.m_fireTick)
			{
				yield return base.Wait(1);
			}
			base.Position = this.Projectile.transform.position;
			this.Projectile.spriteAnimator.Play();
			float startDirection = this.Direction;
			for (int j = 0; j < 30; j++)
			{
				this.Direction = Mathf.LerpAngle(startDirection, base.AimDirection, (float)j / 29f);
				yield return base.Wait(1);
			}
			this.Projectile.spriteAnimator.StopAndResetFrameToDefault();
			Vector2 target = this.BulletManager.PlayerPosition() + UnityEngine.Random.insideUnitCircle * 3f;
			this.Direction = (target - base.Position).ToAngle();
			this.Projectile.BulletScriptSettings.surviveTileCollisions = false;
			this.Speed = UnityEngine.Random.Range(6f, 9f);
			yield return base.Wait(180);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00027950 File Offset: 0x00025B50
		private void OnCollision(CollisionData tileCollision)
		{
			this.Speed = 0f;
			this.m_hitNormal = tileCollision.Normal.ToAngle();
			PhysicsEngine.PostSliceVelocity = new Vector2?(default(Vector2));
			SpeculativeRigidbody specRigidbody = this.Projectile.specRigidbody;
			specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Remove(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
			if (tileCollision.OtherRigidbody)
			{
				base.Vanish(false);
			}
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x000279D0 File Offset: 0x00025BD0
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (this.Projectile)
			{
				SpeculativeRigidbody specRigidbody = this.Projectile.specRigidbody;
				specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Remove(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
			}
		}

		// Token: 0x0400082C RID: 2092
		private int m_fireTick;

		// Token: 0x0400082D RID: 2093
		private float m_hitNormal;
	}
}
