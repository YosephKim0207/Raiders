using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x020002DD RID: 733
public class RevolvenantGrabby1 : Script
{
	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x06000B55 RID: 2901 RVA: 0x00036FB8 File Offset: 0x000351B8
	// (set) Token: 0x06000B56 RID: 2902 RVA: 0x00036FC0 File Offset: 0x000351C0
	public bool Aborting { get; set; }

	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x06000B57 RID: 2903 RVA: 0x00036FCC File Offset: 0x000351CC
	// (set) Token: 0x06000B58 RID: 2904 RVA: 0x00036FD4 File Offset: 0x000351D4
	public bool NearDone { get; set; }

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x06000B59 RID: 2905 RVA: 0x00036FE0 File Offset: 0x000351E0
	// (set) Token: 0x06000B5A RID: 2906 RVA: 0x00036FE8 File Offset: 0x000351E8
	public bool DoShrink { get; set; }

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x06000B5B RID: 2907 RVA: 0x00036FF4 File Offset: 0x000351F4
	// (set) Token: 0x06000B5C RID: 2908 RVA: 0x00036FFC File Offset: 0x000351FC
	public Vector2 PlayerPos { get; set; }

	// Token: 0x06000B5D RID: 2909 RVA: 0x00037008 File Offset: 0x00035208
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		RevolvenantGrabby1.ArmBullet lastLeftBullet = null;
		RevolvenantGrabby1.ArmBullet lastRightBullet = null;
		this.PlayerPos = this.BulletManager.PlayerPosition();
		for (int i = 0; i < 8; i++)
		{
			RevolvenantGrabby1.ArmBullet leftBullet = new RevolvenantGrabby1.ArmBullet(this, "left arm", i, -90f);
			leftBullet.BulletManager = this.BulletManager;
			base.Fire(Offset.OverridePosition(leftBullet.GetArmPosition(3f)), leftBullet);
			RevolvenantGrabby1.ArmBullet rightBullet = new RevolvenantGrabby1.ArmBullet(this, "right arm", i, 90f);
			rightBullet.BulletManager = this.BulletManager;
			base.Fire(Offset.OverridePosition(rightBullet.GetArmPosition(3f)), rightBullet);
			if (i == 0)
			{
				this.firstLeftBullet = leftBullet;
				this.firstRightBullet = rightBullet;
				this.leftHandBullet = new RevolvenantGrabby1.HandBullet(this, 120);
				base.Fire(Offset.OverridePosition(leftBullet.Position), this.leftHandBullet);
				this.rightHandBullet = new RevolvenantGrabby1.HandBullet(this, 240);
				base.Fire(Offset.OverridePosition(leftBullet.Position), this.rightHandBullet);
			}
			else
			{
				this.leftHandBullet.Position = leftBullet.Position;
				this.rightHandBullet.Position = rightBullet.Position;
				if (i == 7)
				{
					lastLeftBullet = leftBullet;
					lastRightBullet = rightBullet;
				}
			}
			for (int j = 0; j < 3; j++)
			{
				this.PlayerPos = this.BulletManager.PlayerPosition();
				yield return base.Wait(1);
			}
		}
		Vector2 pos = lastLeftBullet.GetArmPosition(3f);
		float startAngle = (pos - this.PlayerPos).ToAngle();
		for (int k = 0; k < 6; k++)
		{
			if (this.ShouldAbort(false))
			{
				yield break;
			}
			pos = lastLeftBullet.GetArmPosition(3f);
			float angle = (pos - this.PlayerPos).ToAngle();
			base.Fire(Offset.OverridePosition(pos), new RevolvenantGrabby1.CircleBullet(this, angle, startAngle));
			if (k == 0)
			{
				this.leftHandBullet.Position = pos;
				this.leftHandBullet.Angle = angle;
			}
			pos = lastRightBullet.GetArmPosition(3f);
			angle = (pos - this.PlayerPos).ToAngle();
			base.Fire(Offset.OverridePosition(pos), new RevolvenantGrabby1.CircleBullet(this, angle, BraveMathCollege.ClampAngle360(startAngle + 180f)));
			if (k == 0)
			{
				this.rightHandBullet.Position = pos;
				this.rightHandBullet.Angle = angle;
			}
			if (k == 5)
			{
				lastLeftBullet.Vanish(true);
				lastRightBullet.Vanish(true);
			}
			for (int l = 0; l < 15; l++)
			{
				this.PlayerPos = this.BulletManager.PlayerPosition();
				yield return base.Wait(1);
			}
		}
		int waitTime = 270;
		for (int m = 0; m < waitTime; m++)
		{
			if (this.ShouldAbort(true))
			{
				yield break;
			}
			this.PlayerPos = this.BulletManager.PlayerPosition();
			yield return base.Wait(1);
		}
		this.leftHandBullet.Vanish(false);
		this.rightHandBullet.Vanish(false);
		this.PlayerPos = this.BulletManager.PlayerPosition();
		for (int n = 60; n > 0; n--)
		{
			if (this.ShouldAbort(false))
			{
				yield break;
			}
			Vector2 newPlayerPos = this.BulletManager.PlayerPosition();
			this.PlayerPos = Vector2.MoveTowards(this.PlayerPos, newPlayerPos, 0.11666667f * ((float)n / 60f));
			yield return base.Wait(1);
		}
		this.DoShrink = true;
		base.BulletBank.aiAnimator.LockFacingDirection = true;
		waitTime = 122;
		for (int i2 = 0; i2 < waitTime; i2++)
		{
			if (this.ShouldAbort(false))
			{
				base.BulletBank.aiAnimator.LockFacingDirection = false;
				yield break;
			}
			yield return base.Wait(1);
		}
		base.BulletBank.aiAnimator.LockFacingDirection = false;
		yield break;
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x00037024 File Offset: 0x00035224
	private bool ShouldAbort(bool checkHands = true)
	{
		this.Aborting = this.firstLeftBullet.Destroyed || this.firstRightBullet.Destroyed;
		if (checkHands)
		{
			this.Aborting |= this.leftHandBullet.Destroyed && this.rightHandBullet.Destroyed;
		}
		return this.Aborting;
	}

	// Token: 0x04000C06 RID: 3078
	private const int NumArmBullets = 8;

	// Token: 0x04000C07 RID: 3079
	private const int ArmSpawnDelay = 3;

	// Token: 0x04000C08 RID: 3080
	private const int ArmDestroyTime = 4;

	// Token: 0x04000C09 RID: 3081
	private const int NumCircleBullets = 6;

	// Token: 0x04000C0A RID: 3082
	private const float CircleRadius = 3f;

	// Token: 0x04000C0B RID: 3083
	private const float CircleSpeed = 120f;

	// Token: 0x04000C0C RID: 3084
	private const int InitialHandAttackDelay = 30;

	// Token: 0x04000C0D RID: 3085
	private const int HandAttackTime = 120;

	// Token: 0x04000C0E RID: 3086
	private const int NumHandAttacks = 2;

	// Token: 0x04000C13 RID: 3091
	private RevolvenantGrabby1.ArmBullet firstLeftBullet;

	// Token: 0x04000C14 RID: 3092
	private RevolvenantGrabby1.ArmBullet firstRightBullet;

	// Token: 0x04000C15 RID: 3093
	private RevolvenantGrabby1.HandBullet leftHandBullet;

	// Token: 0x04000C16 RID: 3094
	private RevolvenantGrabby1.HandBullet rightHandBullet;

	// Token: 0x020002DE RID: 734
	private class ArmBullet : Bullet
	{
		// Token: 0x06000B5F RID: 2911 RVA: 0x0003708C File Offset: 0x0003528C
		public ArmBullet(RevolvenantGrabby1 parentScript, string armTransform, int i, float offsetAngle)
			: base(null, false, false, false)
		{
			this.m_parentScript = parentScript;
			this.m_armTransform = armTransform;
			this.m_index = i;
			this.m_offsetAngle = offsetAngle;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x000370B8 File Offset: 0x000352B8
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			float t = (float)this.m_index / 7f;
			this.Projectile.sprite.HeightOffGround = (1f - t) * 10f;
			this.Projectile.sprite.UpdateZDepth();
			this.Projectile.specRigidbody.CollideWithTileMap = false;
			while (!this.m_parentScript.DoShrink)
			{
				if (!base.BulletBank || this.m_parentScript.Aborting || this.m_parentScript.Destroyed)
				{
					base.Vanish(false);
					yield break;
				}
				t = (float)this.m_index / 7f;
				Vector2 desiredPosition = this.GetArmPosition(3f);
				base.Position = Vector2.MoveTowards(base.Position, desiredPosition, (2f + t * 12f) / 60f);
				yield return base.Wait(1);
			}
			for (int i = 0; i < 90; i++)
			{
				if (!base.BulletBank || this.m_parentScript.Aborting)
				{
					base.Vanish(false);
					yield break;
				}
				float radius = Mathf.Lerp(3f, 0f, (float)i / 90f);
				t = (float)this.m_index / 7f;
				Vector2 desiredPosition2 = this.GetArmPosition(radius);
				base.Position = Vector2.MoveTowards(base.Position, desiredPosition2, (2f + t * 12f) / 60f);
				yield return base.Wait(1);
			}
			int destroyOrder = 8 - this.m_index - 1;
			yield return base.Wait(destroyOrder * 4);
			base.Vanish(this.m_index < 4);
			yield break;
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x000370D4 File Offset: 0x000352D4
		public Vector2 GetArmPosition(float circleRadius = 3f)
		{
			Vector2 vector = this.BulletManager.TransformOffset(this.m_parentScript.Position, this.m_armTransform);
			Vector2 vector2 = this.m_parentScript.PlayerPos;
			vector2 += (vector2 - this.m_parentScript.Position).Rotate(this.m_offsetAngle).normalized * circleRadius;
			float num = (float)this.m_index / 7f;
			Vector2 vector3 = (vector2 - vector).Rotate(this.m_offsetAngle).normalized;
			vector3 *= Mathf.Sin(num * 3.1415927f) * 0.5f * Mathf.PingPong((float)(base.Tick + this.m_index * 3) / 75f, 1f);
			if (this.Projectile)
			{
				float num2 = BraveMathCollege.ClampAngle360((vector2 - vector).ToAngle());
				if ((this.m_offsetAngle < 0f && num2 > 90f && num2 < 210f) || (this.m_offsetAngle > 0f && num2 < 90f && num2 > -30f))
				{
					this.Projectile.sprite.HeightOffGround = 0f;
				}
				else
				{
					this.Projectile.sprite.HeightOffGround = (1f - num) * 10f;
				}
			}
			return Vector2.Lerp(vector, vector2, num) + vector3;
		}

		// Token: 0x04000C17 RID: 3095
		private RevolvenantGrabby1 m_parentScript;

		// Token: 0x04000C18 RID: 3096
		private string m_armTransform;

		// Token: 0x04000C19 RID: 3097
		private int m_index;

		// Token: 0x04000C1A RID: 3098
		private float m_offsetAngle;
	}

	// Token: 0x020002E0 RID: 736
	private class CircleBullet : Bullet
	{
		// Token: 0x06000B68 RID: 2920 RVA: 0x000375B8 File Offset: 0x000357B8
		public CircleBullet(RevolvenantGrabby1 parentScript, float angle, float desiredAngle)
			: base(null, false, false, false)
		{
			this.m_parentScript = parentScript;
			this.m_angle = angle;
			this.m_desiredAngle = desiredAngle;
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x000375DC File Offset: 0x000357DC
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			this.Projectile.specRigidbody.CollideWithTileMap = false;
			this.Projectile.BulletScriptSettings.surviveRigidbodyCollisions = true;
			while (!this.m_parentScript.DoShrink)
			{
				if (!base.BulletBank || this.m_parentScript.Aborting || this.m_parentScript.Destroyed)
				{
					base.Vanish(false);
					yield break;
				}
				this.Projectile.ResetDistance();
				this.Projectile.sprite.HeightOffGround = 4f;
				this.m_angle += 2f;
				this.m_desiredAngle += 2f;
				this.m_angle = Mathf.MoveTowardsAngle(this.m_angle, this.m_desiredAngle, 1f);
				base.Position = this.m_parentScript.PlayerPos + BraveMathCollege.DegreesToVector(this.m_angle, 3f);
				yield return base.Wait(1);
			}
			Vector2 origin = this.m_parentScript.PlayerPos;
			for (int i = 0; i < 90; i++)
			{
				if (this.m_parentScript.Aborting)
				{
					base.Vanish(false);
					yield break;
				}
				float radius = Mathf.Lerp(3f, 0f, (float)i / 90f);
				base.Position = origin + BraveMathCollege.DegreesToVector(this.m_angle, radius);
				yield return base.Wait(1);
			}
			base.Vanish(true);
			yield break;
		}

		// Token: 0x04000C25 RID: 3109
		private RevolvenantGrabby1 m_parentScript;

		// Token: 0x04000C26 RID: 3110
		private float m_angle;

		// Token: 0x04000C27 RID: 3111
		private float m_desiredAngle;
	}

	// Token: 0x020002E2 RID: 738
	private class HandBullet : Bullet
	{
		// Token: 0x06000B70 RID: 2928 RVA: 0x000378D4 File Offset: 0x00035AD4
		public HandBullet(RevolvenantGrabby1 parentScript, int initialAttackDelay)
			: base("hand", false, false, false)
		{
			this.m_parentScript = parentScript;
			this.m_stateChangeTimer = initialAttackDelay;
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x000378F4 File Offset: 0x00035AF4
		protected override IEnumerator Top()
		{
			base.ManualControl = true;
			this.Projectile.specRigidbody.CollideWithTileMap = false;
			this.Projectile.BulletScriptSettings.surviveRigidbodyCollisions = true;
			yield return base.Wait(24);
			Vector2 returnStart = base.Position;
			Vector2 returnTarget = base.Position;
			while (!this.m_parentScript.Destroyed && !this.m_parentScript.DoShrink)
			{
				if (!base.BulletBank || this.m_parentScript.Aborting)
				{
					base.Vanish(false);
					yield break;
				}
				this.Projectile.ResetDistance();
				this.Projectile.sprite.HeightOffGround = 5f;
				this.m_stateChangeTimer--;
				if (this.m_state == RevolvenantGrabby1.HandBullet.State.Spin)
				{
					if (!this.m_hasDoneTell && this.m_stateChangeTimer < 30)
					{
						this.Projectile.spriteAnimator.Play();
						this.m_hasDoneTell = true;
					}
					if (this.m_stateChangeTimer <= 0)
					{
						this.m_state = RevolvenantGrabby1.HandBullet.State.Attack;
						this.m_stateChangeTimer = 75;
						this.Speed = 6f;
						this.Direction = (this.m_parentScript.PlayerPos - base.Position).ToAngle();
						base.ManualControl = false;
					}
					else
					{
						this.Angle += 2f;
						base.Position = this.m_parentScript.PlayerPos + BraveMathCollege.DegreesToVector(this.Angle, 3f);
					}
				}
				else if (this.m_state == RevolvenantGrabby1.HandBullet.State.Attack)
				{
					if (this.m_stateChangeTimer <= 0)
					{
						this.Projectile.spriteAnimator.StopAndResetFrameToDefault();
						returnStart = base.Position;
						returnTarget = (base.Position - this.m_parentScript.PlayerPos).normalized * 3f;
						this.m_state = RevolvenantGrabby1.HandBullet.State.Return;
						this.m_stateChangeTimer = 30;
						base.ManualControl = true;
					}
				}
				else if (this.m_state == RevolvenantGrabby1.HandBullet.State.Return)
				{
					if (this.m_stateChangeTimer <= 0)
					{
						base.Position = this.m_parentScript.PlayerPos + returnTarget;
						this.Angle = returnTarget.ToAngle();
						this.m_state = RevolvenantGrabby1.HandBullet.State.Spin;
						this.m_stateChangeTimer = 135;
					}
					else
					{
						base.Position = Vector2.Lerp(this.m_parentScript.PlayerPos + returnTarget, returnStart, (float)this.m_stateChangeTimer / 30f);
					}
				}
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000C2F RID: 3119
		private const int AttackTime = 75;

		// Token: 0x04000C30 RID: 3120
		private const int ResetTime = 30;

		// Token: 0x04000C31 RID: 3121
		public float Angle;

		// Token: 0x04000C32 RID: 3122
		private RevolvenantGrabby1.HandBullet.State m_state;

		// Token: 0x04000C33 RID: 3123
		private bool m_hasDoneTell;

		// Token: 0x04000C34 RID: 3124
		private RevolvenantGrabby1 m_parentScript;

		// Token: 0x04000C35 RID: 3125
		private int m_stateChangeTimer;

		// Token: 0x020002E3 RID: 739
		private enum State
		{
			// Token: 0x04000C37 RID: 3127
			Spin,
			// Token: 0x04000C38 RID: 3128
			Attack,
			// Token: 0x04000C39 RID: 3129
			Return
		}
	}
}
