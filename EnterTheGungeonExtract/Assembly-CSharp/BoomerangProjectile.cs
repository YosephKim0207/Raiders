using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001629 RID: 5673
public class BoomerangProjectile : Projectile
{
	// Token: 0x0600846F RID: 33903 RVA: 0x00368254 File Offset: 0x00366454
	public override void Start()
	{
		base.Start();
		if (this.PossibleSourceGun && this.PossibleSourceGun.OwnerHasSynergy(CustomSynergyType.CRAVE_THE_GLAIVE))
		{
			this.MaximumNumberOfTargets = -1;
			this.MaximumTraversalDistance = -1f;
		}
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.HandleRigidbodyCollision));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnTileCollision = (SpeculativeRigidbody.OnTileCollisionDelegate)Delegate.Combine(specRigidbody2.OnTileCollision, new SpeculativeRigidbody.OnTileCollisionDelegate(this.HandleTileCollision));
		this.RemainingEnemiesToHit = new Queue<AIActor>();
		this.GatherTargetEnemies();
		if (this.RemainingEnemiesToHit.Count > 0)
		{
			AIActor aiactor = this.RemainingEnemiesToHit.Peek();
			if (aiactor != null)
			{
				Vector2 vector = aiactor.CenterPosition - base.transform.position.XY();
				float num = BraveMathCollege.Atan2Degrees(base.specRigidbody.Velocity);
				float num2 = BraveMathCollege.Atan2Degrees(vector);
				float num3 = Mathf.DeltaAngle(num, num2);
				base.transform.Rotate(0f, 0f, num3);
			}
		}
		else if (base.Owner && base.Owner is PlayerController && base.Owner.CurrentGun)
		{
			float num4 = BraveMathCollege.Atan2Degrees(base.specRigidbody.Velocity);
			float currentAngle = base.Owner.CurrentGun.CurrentAngle;
			float num5 = Mathf.DeltaAngle(num4, currentAngle);
			base.transform.Rotate(0f, 0f, num5);
		}
	}

	// Token: 0x06008470 RID: 33904 RVA: 0x003683FC File Offset: 0x003665FC
	private void GatherTargetEnemies()
	{
		List<AIActor> activeEnemies = base.transform.position.GetAbsoluteRoom().GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		Vector2 vector = base.transform.position.XY();
		this.throwerPlayer = GameManager.Instance.GetActivePlayerClosestToPoint(vector, false);
		if (this.UsesMouseAimPoint)
		{
			vector = this.throwerPlayer.unadjustedAimPoint.XY();
		}
		if (activeEnemies != null && this.RemainingEnemiesToHit != null)
		{
			while (this.RemainingEnemiesToHit.Count < activeEnemies.Count)
			{
				if (this.MaximumNumberOfTargets > 0 && this.RemainingEnemiesToHit.Count >= this.MaximumNumberOfTargets)
				{
					break;
				}
				AIActor aiactor;
				if (this.MaximumTraversalDistance > 0f && this.RemainingEnemiesToHit.Count > 0)
				{
					aiactor = BraveUtility.GetClosestToPosition<AIActor>(activeEnemies, vector, null, this.MaximumTraversalDistance, this.RemainingEnemiesToHit.ToArray());
				}
				else
				{
					aiactor = BraveUtility.GetClosestToPosition<AIActor>(activeEnemies, vector, this.RemainingEnemiesToHit.ToArray());
				}
				if (aiactor == null)
				{
					break;
				}
				this.RemainingEnemiesToHit.Enqueue(aiactor);
				vector = aiactor.CenterPosition;
			}
		}
	}

	// Token: 0x06008471 RID: 33905 RVA: 0x0036852C File Offset: 0x0036672C
	private void HandleTileCollision(CollisionData tileCollision)
	{
		if (this.RemainingEnemiesToHit.Count == 0)
		{
			if (this.m_elapsedTargetlessTime >= 5f)
			{
				base.DieInAir(false, true, true, false);
			}
			else if (this.m_targetlessTime <= 0f)
			{
				this.m_targetlessTime = 0.2f;
			}
		}
		else if (this.m_elapsedTargetlessTime >= 1f)
		{
			this.m_elapsedTargetlessTime = 0f;
			this.m_targetlessTime = 0f;
			this.RemainingEnemiesToHit.Dequeue();
		}
		else if (this.m_targetlessTime <= 0f)
		{
			this.m_targetlessTime = 0.5f;
		}
	}

	// Token: 0x06008472 RID: 33906 RVA: 0x003685DC File Offset: 0x003667DC
	private void HandleRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		if (rigidbodyCollision.OtherRigidbody.aiActor)
		{
			if (rigidbodyCollision.OtherRigidbody.aiActor.behaviorSpeculator)
			{
				rigidbodyCollision.OtherRigidbody.aiActor.behaviorSpeculator.Stun(this.StunDuration, true);
			}
			if (this.RemainingEnemiesToHit.Count > 0 && this.RemainingEnemiesToHit.Peek() == rigidbodyCollision.OtherRigidbody.aiActor)
			{
				this.m_elapsedTargetlessTime = 0f;
				this.m_targetlessTime = 0f;
				this.RemainingEnemiesToHit.Dequeue();
			}
		}
	}

	// Token: 0x06008473 RID: 33907 RVA: 0x00368688 File Offset: 0x00366888
	protected override void Move()
	{
		GameActor gameActor;
		if (this.RemainingEnemiesToHit.Count > 0)
		{
			gameActor = this.RemainingEnemiesToHit.Peek();
		}
		else
		{
			gameActor = this.throwerPlayer;
		}
		if (this.m_targetlessTime <= 0f)
		{
			if (gameActor != null)
			{
				Vector2 vector = gameActor.CenterPosition - base.transform.position.XY();
				float num = BraveMathCollege.Atan2Degrees(base.specRigidbody.Velocity);
				float num2 = BraveMathCollege.Atan2Degrees(vector);
				float num3 = Mathf.DeltaAngle(num, num2);
				float num4 = Mathf.Min(Mathf.Abs(num3), this.trackingSpeed * BraveTime.DeltaTime) * Mathf.Sign(num3);
				base.transform.Rotate(0f, 0f, num4);
			}
		}
		else
		{
			this.m_targetlessTime -= BraveTime.DeltaTime;
			this.m_elapsedTargetlessTime += BraveTime.DeltaTime;
		}
		base.specRigidbody.Velocity = base.transform.right * this.baseData.speed;
		base.LastVelocity = base.specRigidbody.Velocity;
		if (gameActor == this.throwerPlayer && Vector2.Distance(gameActor.CenterPosition, base.transform.position) < 1f)
		{
			base.DieInAir(true, true, true, false);
		}
	}

	// Token: 0x06008474 RID: 33908 RVA: 0x003687F8 File Offset: 0x003669F8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04008801 RID: 34817
	public float StunDuration = 5f;

	// Token: 0x04008802 RID: 34818
	public float trackingSpeed = 5f;

	// Token: 0x04008803 RID: 34819
	public bool stopTrackingIfLeaveRadius;

	// Token: 0x04008804 RID: 34820
	public bool UsesMouseAimPoint;

	// Token: 0x04008805 RID: 34821
	public int MaximumNumberOfTargets = -1;

	// Token: 0x04008806 RID: 34822
	public float MaximumTraversalDistance = -1f;

	// Token: 0x04008807 RID: 34823
	private PlayerController throwerPlayer;

	// Token: 0x04008808 RID: 34824
	private Queue<AIActor> RemainingEnemiesToHit;

	// Token: 0x04008809 RID: 34825
	private float m_targetlessTime;

	// Token: 0x0400880A RID: 34826
	private float m_elapsedTargetlessTime;
}
