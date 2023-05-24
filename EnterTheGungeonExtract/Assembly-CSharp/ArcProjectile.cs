using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02001619 RID: 5657
public class ArcProjectile : Projectile
{
	// Token: 0x140000CF RID: 207
	// (add) Token: 0x060083D3 RID: 33747 RVA: 0x003602EC File Offset: 0x0035E4EC
	// (remove) Token: 0x060083D4 RID: 33748 RVA: 0x00360324 File Offset: 0x0035E524
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnGrounded;

	// Token: 0x060083D5 RID: 33749 RVA: 0x0036035C File Offset: 0x0035E55C
	public override void Start()
	{
		base.Start();
		this.m_currentHeight = this.startingHeight;
		this.m_current3DVelocity = (this.m_currentDirection * this.m_currentSpeed).ToVector3ZUp(this.startingZSpeed);
		if (this.LandingTargetSprite && !this.m_landingTarget)
		{
			float timeInFlight = this.GetTimeInFlight();
			this.m_targetLandPosition = base.transform.position.XY() + this.m_currentDirection * this.m_currentSpeed * timeInFlight + new Vector2(0f, -this.m_currentHeight);
			this.m_landingTarget = SpawnManager.SpawnVFX(this.LandingTargetSprite, this.m_targetLandPosition, Quaternion.identity).GetComponent<tk2dBaseSprite>();
			this.m_landingTarget.UpdateZDepth();
			tk2dSpriteAnimator componentInChildren = this.m_landingTarget.GetComponentInChildren<tk2dSpriteAnimator>();
			componentInChildren.Play(componentInChildren.DefaultClip, 0f, (float)componentInChildren.DefaultClip.frames.Length / timeInFlight, false);
		}
	}

	// Token: 0x060083D6 RID: 33750 RVA: 0x0036046C File Offset: 0x0035E66C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060083D7 RID: 33751 RVA: 0x00360474 File Offset: 0x0035E674
	protected override void Move()
	{
		if (this.angularVelocity != 0f)
		{
			base.transform.RotateAround(base.sprite.WorldCenter, Vector3.forward, this.angularVelocity * base.LocalDeltaTime);
		}
		this.m_current3DVelocity.x = this.m_currentDirection.x;
		this.m_current3DVelocity.y = this.m_currentDirection.y;
		this.m_current3DVelocity.z = this.m_current3DVelocity.z + base.LocalDeltaTime * this.gravity;
		float num = this.m_currentHeight + this.m_current3DVelocity.z * base.LocalDeltaTime;
		if (num < 0f)
		{
			if (this.OnGrounded != null)
			{
				this.OnGrounded();
			}
			if (!string.IsNullOrEmpty(this.groundAudioEvent))
			{
				AkSoundEngine.PostEvent(this.groundAudioEvent, base.gameObject);
			}
			if (this.destroyOnGroundContact)
			{
				base.DieInAir(false, true, true, false);
			}
			else
			{
				this.m_current3DVelocity.z = -this.m_current3DVelocity.z;
				num = this.m_currentHeight + this.m_current3DVelocity.z * base.LocalDeltaTime;
			}
		}
		this.m_currentHeight = num;
		this.m_currentDirection = this.m_current3DVelocity.XY();
		Vector2 vector = this.m_current3DVelocity.XY().normalized * this.m_currentSpeed;
		base.specRigidbody.Velocity = new Vector2(vector.x, vector.y + this.m_current3DVelocity.z);
		base.LastVelocity = this.m_current3DVelocity.XY();
		this.UpdateTargetPosition(false);
	}

	// Token: 0x060083D8 RID: 33752 RVA: 0x0036062C File Offset: 0x0035E82C
	public float GetTimeInFlight()
	{
		float num = -this.startingHeight;
		float num2 = this.startingZSpeed;
		float num3 = this.gravity;
		float num4 = (Mathf.Sqrt(2f * num3 * num + num2 * num2) + num2) / -num3;
		if (num4 < 0f)
		{
			num4 = (Mathf.Sqrt(2f * num3 * num + num2 * num2) - num2) / num3;
		}
		return num4;
	}

	// Token: 0x060083D9 RID: 33753 RVA: 0x0036068C File Offset: 0x0035E88C
	public float GetRemainingTimeInFlight()
	{
		float num = -this.m_currentHeight;
		float z = this.m_current3DVelocity.z;
		float num2 = this.gravity;
		float num3 = (Mathf.Sqrt(2f * num2 * num + z * z) + z) / -num2;
		if (num3 < 0f)
		{
			num3 = (Mathf.Sqrt(2f * num2 * num + z * z) - z) / num2;
		}
		return num3;
	}

	// Token: 0x060083DA RID: 33754 RVA: 0x003606F0 File Offset: 0x0035E8F0
	public override float EstimatedTimeToTarget(Vector2 targetPoint, Vector2? overridePos = null)
	{
		return this.GetTimeInFlight();
	}

	// Token: 0x060083DB RID: 33755 RVA: 0x003606F8 File Offset: 0x0035E8F8
	public override Vector2 GetPredictedTargetPosition(Vector2 targetCenter, Vector2 targetVelocity, Vector2? overridePos = null, float? overrideProjectileSpeed = null)
	{
		return BraveMathCollege.GetPredictedPosition(targetCenter, targetVelocity, this.EstimatedTimeToTarget(targetCenter, overridePos));
	}

	// Token: 0x060083DC RID: 33756 RVA: 0x0036070C File Offset: 0x0035E90C
	public void AdjustSpeedToHit(Vector2 target)
	{
		Vector2 vector = target - base.transform.position.XY();
		this.baseData.speed = vector.magnitude / this.GetTimeInFlight();
		base.UpdateSpeed();
		this.UpdateTargetPosition(true);
	}

	// Token: 0x060083DD RID: 33757 RVA: 0x00360758 File Offset: 0x0035E958
	protected override void HandleDestruction(CollisionData lcr, bool allowActorSpawns = true, bool allowProjectileSpawns = true)
	{
		if (this.m_landingTarget)
		{
			SpawnManager.Despawn(this.m_landingTarget.gameObject);
			this.m_landingTarget = null;
		}
		base.HandleDestruction(lcr, allowActorSpawns, allowProjectileSpawns);
	}

	// Token: 0x060083DE RID: 33758 RVA: 0x0036078C File Offset: 0x0035E98C
	public override void OnDespawned()
	{
		if (this.m_landingTarget)
		{
			SpawnManager.Despawn(this.m_landingTarget.gameObject);
			this.m_landingTarget = null;
		}
		base.OnDespawned();
	}

	// Token: 0x060083DF RID: 33759 RVA: 0x003607BC File Offset: 0x0035E9BC
	private void UpdateTargetPosition(bool useStartPosition)
	{
		if (this.LandingTargetSprite && this.m_landingTarget)
		{
			float num = ((!useStartPosition) ? this.GetRemainingTimeInFlight() : this.GetTimeInFlight());
			Vector2 vector = this.m_targetLandPosition;
			Vector2 vector2 = base.transform.position.XY() + this.m_currentDirection * this.m_currentSpeed * num + new Vector2(0f, -this.m_currentHeight);
			this.m_targetLandPosition = Vector2.Lerp(vector, vector2, (!useStartPosition) ? Mathf.Clamp01(BraveTime.DeltaTime * 4f) : 1f);
			this.m_landingTarget.transform.position = this.m_targetLandPosition;
			this.m_landingTarget.UpdateZDepth();
		}
	}

	// Token: 0x0400872B RID: 34603
	[Header("Arc Projectile")]
	public float startingHeight = 1f;

	// Token: 0x0400872C RID: 34604
	public float startingZSpeed;

	// Token: 0x0400872D RID: 34605
	public float gravity = -10f;

	// Token: 0x0400872E RID: 34606
	public bool destroyOnGroundContact = true;

	// Token: 0x0400872F RID: 34607
	public string groundAudioEvent = string.Empty;

	// Token: 0x04008730 RID: 34608
	public GameObject LandingTargetSprite;

	// Token: 0x04008732 RID: 34610
	private float m_currentHeight;

	// Token: 0x04008733 RID: 34611
	private Vector3 m_current3DVelocity;

	// Token: 0x04008734 RID: 34612
	private tk2dBaseSprite m_landingTarget;

	// Token: 0x04008735 RID: 34613
	private Vector3 m_targetLandPosition;
}
