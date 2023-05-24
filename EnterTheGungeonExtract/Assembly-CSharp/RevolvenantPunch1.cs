using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x020002E6 RID: 742
public class RevolvenantPunch1 : Script
{
	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x06000B7F RID: 2943 RVA: 0x00038500 File Offset: 0x00036700
	// (set) Token: 0x06000B80 RID: 2944 RVA: 0x00038508 File Offset: 0x00036708
	public bool Spew { get; set; }

	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x06000B81 RID: 2945 RVA: 0x00038514 File Offset: 0x00036714
	// (set) Token: 0x06000B82 RID: 2946 RVA: 0x0003851C File Offset: 0x0003671C
	public Vector2 ArmPosition { get; set; }

	// Token: 0x06000B83 RID: 2947 RVA: 0x00038528 File Offset: 0x00036728
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		string transform = ((BraveMathCollege.AbsAngleBetween(base.BulletBank.aiAnimator.FacingDirection, 0f) <= 90f) ? "left arm" : "right arm");
		float direction = base.GetAimDirection(transform);
		base.BulletBank.aiAnimator.FacingDirection = direction;
		this.ArmPosition = base.BulletBank.TransformOffset(base.Position, transform);
		RevolvenantPunch1.HandBullet handBullet = new RevolvenantPunch1.HandBullet(this);
		base.Fire(Offset.OverridePosition(this.ArmPosition), new Direction(direction, DirectionType.Absolute, -1f), new Speed(40f, SpeedType.Absolute), handBullet);
		for (int i = 0; i < 20; i++)
		{
			base.Fire(Offset.OverridePosition(this.ArmPosition), new Direction(direction, DirectionType.Absolute, -1f), new RevolvenantPunch1.ArmBullet(this, handBullet, i));
		}
		while (!handBullet.IsEnded && !handBullet.HasStopped)
		{
			yield return base.Wait(1);
		}
		this.Spew = true;
		yield return base.Wait(240);
		yield break;
	}

	// Token: 0x04000C54 RID: 3156
	private const int NumArmBullets = 20;

	// Token: 0x020002E7 RID: 743
	private class ArmBullet : Bullet
	{
		// Token: 0x06000B84 RID: 2948 RVA: 0x00038544 File Offset: 0x00036744
		public ArmBullet(RevolvenantPunch1 parentScript, RevolvenantPunch1.HandBullet handBullet, int index)
			: base(null, false, false, false)
		{
			this.m_parentScript = parentScript;
			this.m_handBullet = handBullet;
			this.m_index = index;
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x00038568 File Offset: 0x00036768
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			while (!this.m_parentScript.IsEnded && !this.m_parentScript.Spew && base.BulletBank)
			{
				base.Position = Vector2.Lerp(this.m_parentScript.ArmPosition, this.m_handBullet.Position, (float)this.m_index / 20f);
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
			yield return base.Wait(this.m_index + 1);
			yield return base.Wait(UnityEngine.Random.Range(0, 60));
			for (int j = 0; j < 3; j++)
			{
				if (this.m_parentScript.IsEnded || !this.m_parentScript.BulletBank || this.m_parentScript.BulletBank.healthHaver.IsDead || this.m_parentScript.BulletBank.aiActor.IsFrozen)
				{
					base.ManualControl = false;
					this.Direction += BraveUtility.RandomSign() * UnityEngine.Random.Range(60f, 120f);
					this.Speed = 8f;
					yield break;
				}
				base.Fire(new Direction(this.Direction + BraveUtility.RandomSign() * UnityEngine.Random.Range(60f, 120f), DirectionType.Absolute, -1f), new Speed(12f, SpeedType.Absolute), null);
				yield return base.Wait(60);
			}
			base.ManualControl = false;
			this.Direction = base.RandomAngle();
			this.Speed = 9f;
			yield break;
		}

		// Token: 0x04000C57 RID: 3159
		public const int NumBullets = 3;

		// Token: 0x04000C58 RID: 3160
		public const int BulletDelay = 60;

		// Token: 0x04000C59 RID: 3161
		private const float WiggleMagnitude = 0.4f;

		// Token: 0x04000C5A RID: 3162
		private const int WiggleTime = 30;

		// Token: 0x04000C5B RID: 3163
		private const int NumBulletsToPreShake = 5;

		// Token: 0x04000C5C RID: 3164
		private RevolvenantPunch1 m_parentScript;

		// Token: 0x04000C5D RID: 3165
		private RevolvenantPunch1 revolvenantPunch1;

		// Token: 0x04000C5E RID: 3166
		private RevolvenantPunch1.HandBullet m_handBullet;

		// Token: 0x04000C5F RID: 3167
		private int m_index;
	}

	// Token: 0x020002E9 RID: 745
	private class HandBullet : Bullet
	{
		// Token: 0x06000B8C RID: 2956 RVA: 0x00038AC4 File Offset: 0x00036CC4
		public HandBullet(RevolvenantPunch1 parentScript)
			: base("hand", false, false, false)
		{
			this.m_parentScript = parentScript;
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000B8D RID: 2957 RVA: 0x00038ADC File Offset: 0x00036CDC
		// (set) Token: 0x06000B8E RID: 2958 RVA: 0x00038AE4 File Offset: 0x00036CE4
		public bool HasStopped { get; set; }

		// Token: 0x06000B8F RID: 2959 RVA: 0x00038AF0 File Offset: 0x00036CF0
		protected override IEnumerator Top()
		{
			this.Projectile.BulletScriptSettings.surviveRigidbodyCollisions = true;
			this.Projectile.BulletScriptSettings.surviveTileCollisions = true;
			SpeculativeRigidbody specRigidbody = this.Projectile.specRigidbody;
			specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
			while (!this.m_parentScript.IsEnded && this.m_parentScript.BulletBank && !this.m_parentScript.BulletBank.healthHaver.IsDead && !this.m_parentScript.BulletBank.aiActor.IsFrozen)
			{
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
			yield break;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00038B0C File Offset: 0x00036D0C
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

		// Token: 0x06000B91 RID: 2961 RVA: 0x00038C18 File Offset: 0x00036E18
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (this.Projectile)
			{
				SpeculativeRigidbody specRigidbody = this.Projectile.specRigidbody;
				specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Remove(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
			}
			this.HasStopped = true;
		}

		// Token: 0x04000C6C RID: 3180
		private RevolvenantPunch1 m_parentScript;
	}
}
