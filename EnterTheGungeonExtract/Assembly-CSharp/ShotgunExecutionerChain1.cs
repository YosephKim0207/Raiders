using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x020002F7 RID: 759
public class ShotgunExecutionerChain1 : Script
{
	// Token: 0x06000BC1 RID: 3009 RVA: 0x000397DC File Offset: 0x000379DC
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		ShotgunExecutionerChain1.HandBullet handBullet = null;
		for (int i = 0; i < 3; i++)
		{
			handBullet = this.FireVolley(i, (float)(20 + i * 5));
			if (i < 2)
			{
				yield return base.Wait(30);
			}
		}
		while (!handBullet.IsEnded && !handBullet.HasStopped)
		{
			yield return base.Wait(1);
		}
		yield return base.Wait(120);
		yield break;
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x000397F8 File Offset: 0x000379F8
	private ShotgunExecutionerChain1.HandBullet FireVolley(int volleyIndex, float speed)
	{
		ShotgunExecutionerChain1.HandBullet handBullet = new ShotgunExecutionerChain1.HandBullet(this);
		base.Fire(new Direction(base.AimDirection, DirectionType.Absolute, -1f), new Speed(speed, SpeedType.Absolute), handBullet);
		for (int i = 0; i < 20; i++)
		{
			base.Fire(new Direction(base.AimDirection, DirectionType.Absolute, -1f), new ShotgunExecutionerChain1.ArmBullet(this, handBullet, i));
		}
		return handBullet;
	}

	// Token: 0x04000C92 RID: 3218
	private const int NumArmBullets = 20;

	// Token: 0x04000C93 RID: 3219
	private const int NumVolley = 3;

	// Token: 0x04000C94 RID: 3220
	private const int FramesBetweenVolleys = 30;

	// Token: 0x020002F8 RID: 760
	private class ArmBullet : Bullet
	{
		// Token: 0x06000BC3 RID: 3011 RVA: 0x00039860 File Offset: 0x00037A60
		public ArmBullet(ShotgunExecutionerChain1 parentScript, ShotgunExecutionerChain1.HandBullet handBullet, int index)
			: base("chain", false, false, false)
		{
			this.m_parentScript = parentScript;
			this.m_handBullet = handBullet;
			this.m_index = index;
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x00039888 File Offset: 0x00037A88
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			while (!this.m_parentScript.IsEnded && !this.m_handBullet.IsEnded && !this.m_handBullet.HasStopped && base.BulletBank)
			{
				base.Position = Vector2.Lerp(this.m_parentScript.Position, this.m_handBullet.Position, (float)this.m_index / 20f);
				yield return base.Wait(1);
			}
			if (this.m_parentScript.IsEnded)
			{
				base.Vanish(false);
				yield break;
			}
			int delay = 20 - this.m_index - 5;
			if (delay > 0)
			{
				yield return base.Wait(delay);
			}
			float currentOffset = 0f;
			Vector2 truePosition = base.Position;
			int halfWiggleTime = 10;
			for (int i = 0; i < 30; i++)
			{
				if (i == 0 && delay < 0)
				{
					i = -delay;
				}
				float magnitude = 0.4f;
				magnitude = Mathf.Min(magnitude, Mathf.Lerp(0.2f, 0.4f, (float)this.m_index / 8f));
				magnitude = Mathf.Min(magnitude, Mathf.Lerp(0.2f, 0.4f, (float)(20 - this.m_index - 1) / 3f));
				magnitude = Mathf.Lerp(magnitude, 0f, (float)i / (float)halfWiggleTime - 2f);
				currentOffset = Mathf.SmoothStep(-magnitude, magnitude, Mathf.PingPong(0.5f + (float)i / (float)halfWiggleTime, 1f));
				base.Position = truePosition + BraveMathCollege.DegreesToVector(this.Direction - 90f, currentOffset);
				yield return base.Wait(1);
			}
			yield return base.Wait(this.m_index + 1 + 60);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000C95 RID: 3221
		public const int BulletDelay = 60;

		// Token: 0x04000C96 RID: 3222
		private const float WiggleMagnitude = 0.4f;

		// Token: 0x04000C97 RID: 3223
		public const int WiggleTime = 30;

		// Token: 0x04000C98 RID: 3224
		private const int NumBulletsToPreShake = 5;

		// Token: 0x04000C99 RID: 3225
		private ShotgunExecutionerChain1 m_parentScript;

		// Token: 0x04000C9A RID: 3226
		private ShotgunExecutionerChain1 shotgunExecutionerChain1;

		// Token: 0x04000C9B RID: 3227
		private ShotgunExecutionerChain1.HandBullet m_handBullet;

		// Token: 0x04000C9C RID: 3228
		private int m_index;
	}

	// Token: 0x020002FA RID: 762
	private class HandBullet : Bullet
	{
		// Token: 0x06000BCB RID: 3019 RVA: 0x00039C4C File Offset: 0x00037E4C
		public HandBullet(ShotgunExecutionerChain1 parentScript)
			: base("chain", false, false, false)
		{
			this.m_parentScript = parentScript;
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000BCC RID: 3020 RVA: 0x00039C64 File Offset: 0x00037E64
		// (set) Token: 0x06000BCD RID: 3021 RVA: 0x00039C6C File Offset: 0x00037E6C
		public bool HasStopped { get; set; }

		// Token: 0x06000BCE RID: 3022 RVA: 0x00039C78 File Offset: 0x00037E78
		protected override IEnumerator Top()
		{
			this.Projectile.BulletScriptSettings.surviveRigidbodyCollisions = true;
			this.Projectile.BulletScriptSettings.surviveTileCollisions = true;
			SpeculativeRigidbody specRigidbody = this.Projectile.specRigidbody;
			specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
			while (!this.m_parentScript.IsEnded && !this.HasStopped)
			{
				yield return base.Wait(1);
			}
			if (this.m_parentScript.IsEnded)
			{
				base.Vanish(false);
				yield break;
			}
			yield return base.Wait(111);
			base.Vanish(false);
			yield break;
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00039C94 File Offset: 0x00037E94
		private void OnCollision(CollisionData collision)
		{
			bool flag = collision.collisionType == CollisionData.CollisionType.TileMap;
			SpeculativeRigidbody otherRigidbody = collision.OtherRigidbody;
			if (otherRigidbody)
			{
				flag = otherRigidbody.majorBreakable || otherRigidbody.PreventPiercing || (!otherRigidbody.gameActor && !otherRigidbody.minorBreakable);
			}
			if (flag)
			{
				base.Position = collision.MyRigidbody.UnitCenter + PhysicsEngine.PixelToUnit(collision.NewPixelsToMove);
				this.Speed = 0f;
				this.HasStopped = true;
				PhysicsEngine.PostSliceVelocity = new Vector2?(new Vector2(0f, 0f));
				SpeculativeRigidbody specRigidbody = this.Projectile.specRigidbody;
				specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Remove(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
			}
			else
			{
				PhysicsEngine.PostSliceVelocity = new Vector2?(collision.MyRigidbody.Velocity);
			}
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00039DA0 File Offset: 0x00037FA0
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (this.Projectile)
			{
				SpeculativeRigidbody specRigidbody = this.Projectile.specRigidbody;
				specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Remove(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
			}
			this.HasStopped = true;
		}

		// Token: 0x04000CA8 RID: 3240
		private ShotgunExecutionerChain1 m_parentScript;
	}
}
