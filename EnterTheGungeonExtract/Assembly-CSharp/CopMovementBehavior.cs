using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02000DE8 RID: 3560
public class CopMovementBehavior : MovementBehaviorBase
{
	// Token: 0x06004B64 RID: 19300 RVA: 0x00198EE0 File Offset: 0x001970E0
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_repathTimer, false);
	}

	// Token: 0x06004B65 RID: 19301 RVA: 0x00198EF8 File Offset: 0x001970F8
	private void CatchUpMovementModifier(ref Vector2 voluntaryVel, ref Vector2 involuntaryVel)
	{
		this.m_catchUpTime += this.m_aiActor.LocalDeltaTime;
		voluntaryVel = voluntaryVel.normalized * Mathf.Lerp(this.CatchUpSpeed, this.CatchUpMaxSpeed, this.m_catchUpTime / this.CatchUpAccelTime);
	}

	// Token: 0x06004B66 RID: 19302 RVA: 0x00198F4C File Offset: 0x0019714C
	public override BehaviorResult Update()
	{
		this.m_aiActor.DustUpInterval = Mathf.Lerp(0.5f, 0.125f, this.m_aiActor.specRigidbody.Velocity.magnitude / this.CatchUpSpeed);
		PlayerController primaryPlayer = GameManager.Instance.PrimaryPlayer;
		if (!primaryPlayer || primaryPlayer.CurrentRoom == null)
		{
			this.m_aiActor.ClearPath();
			return BehaviorResult.Continue;
		}
		if (!primaryPlayer.IsStealthed && primaryPlayer.CurrentRoom.IsSealed && this.m_aiActor.transform.position.GetAbsoluteRoom() == primaryPlayer.CurrentRoom && this.DisableInCombat)
		{
			IntVector2 intVector = this.m_aiActor.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor);
			if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector) && !GameManager.Instance.Dungeon.data[intVector].isExitCell)
			{
				if (this.m_isCatchingUp)
				{
					this.m_isCatchingUp = false;
					if (!string.IsNullOrEmpty(this.CatchUpOutAnimation))
					{
						this.m_aiAnimator.PlayUntilFinished(this.CatchUpOutAnimation, false, null, -1f, false);
					}
					this.m_aiActor.MovementModifiers -= this.CatchUpMovementModifier;
				}
				return BehaviorResult.Continue;
			}
		}
		float num = Vector2.Distance(primaryPlayer.CenterPosition, this.m_aiActor.CenterPosition);
		if (num <= this.IdealRadius)
		{
			this.m_aiActor.ClearPath();
			if (this.m_isCatchingUp)
			{
				this.m_isCatchingUp = false;
				if (!string.IsNullOrEmpty(this.CatchUpOutAnimation))
				{
					this.m_aiAnimator.PlayUntilFinished(this.CatchUpOutAnimation, false, null, -1f, false);
				}
				this.m_aiActor.MovementModifiers -= this.CatchUpMovementModifier;
			}
			if (!this.m_hasIdled && !this.m_aiAnimator.IsPlaying(this.CatchUpOutAnimation) && this.IdleAnimations.Length > 0)
			{
				this.m_hasIdled = true;
				this.m_aiAnimator.PlayUntilCancelled(this.IdleAnimations[UnityEngine.Random.Range(0, this.IdleAnimations.Length)], false, null, -1f, false);
			}
			return BehaviorResult.SkipRemainingClassBehaviors;
		}
		if (num > 30f)
		{
			this.m_sequentialPathFails = 0;
			this.m_aiActor.CompanionWarp(this.m_aiActor.CompanionOwner.CenterPosition);
		}
		else
		{
			this.m_hasIdled = false;
			if (!this.m_isCatchingUp && num > this.CatchUpRadius)
			{
				this.m_isCatchingUp = true;
				this.m_catchUpTime = 0f;
				if (!string.IsNullOrEmpty(this.CatchUpAnimation))
				{
					this.m_aiAnimator.PlayUntilFinished(this.CatchUpAnimation, false, null, -1f, false);
				}
				else
				{
					this.m_aiAnimator.EndAnimation();
				}
				this.m_aiActor.MovementModifiers += this.CatchUpMovementModifier;
			}
			else if (!this.m_isCatchingUp && num < this.CatchUpRadius)
			{
				this.m_aiAnimator.EndAnimation();
			}
			if (this.m_repathTimer <= 0f && primaryPlayer && primaryPlayer.specRigidbody && !primaryPlayer.IsInMinecart)
			{
				this.m_repathTimer = this.PathInterval;
				this.m_aiActor.PathfindToPosition(primaryPlayer.specRigidbody.UnitCenter, null, true, null, null, null, false);
				if (this.m_aiActor.Path != null && this.m_aiActor.Path.InaccurateLength > 50f)
				{
					this.m_aiActor.ClearPath();
					this.m_sequentialPathFails = 0;
					this.m_aiActor.CompanionWarp(this.m_aiActor.CompanionOwner.CenterPosition);
				}
				else if (this.m_aiActor.Path != null && !this.m_aiActor.Path.WillReachFinalGoal)
				{
					this.m_sequentialPathFails++;
					IntVector2 intVector2 = this.m_aiActor.CompanionOwner.CenterPosition.ToIntVector2(VectorConversions.Floor);
					CellData cellData = GameManager.Instance.Dungeon.data[intVector2];
					if (this.m_sequentialPathFails > 3 && cellData != null && cellData.IsPassable)
					{
						this.m_sequentialPathFails = 0;
						this.m_aiActor.CompanionWarp(this.m_aiActor.CompanionOwner.CenterPosition);
					}
				}
				else
				{
					this.m_sequentialPathFails = 0;
				}
			}
		}
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x040040EF RID: 16623
	public float PathInterval = 0.25f;

	// Token: 0x040040F0 RID: 16624
	public bool DisableInCombat = true;

	// Token: 0x040040F1 RID: 16625
	public float IdealRadius = 3f;

	// Token: 0x040040F2 RID: 16626
	public float CatchUpRadius = 7f;

	// Token: 0x040040F3 RID: 16627
	public float CatchUpAccelTime = 5f;

	// Token: 0x040040F4 RID: 16628
	public float CatchUpSpeed = 7f;

	// Token: 0x040040F5 RID: 16629
	public float CatchUpMaxSpeed = 10f;

	// Token: 0x040040F6 RID: 16630
	public string CatchUpAnimation;

	// Token: 0x040040F7 RID: 16631
	public string CatchUpOutAnimation;

	// Token: 0x040040F8 RID: 16632
	public string[] IdleAnimations;

	// Token: 0x040040F9 RID: 16633
	private bool m_hasIdled;

	// Token: 0x040040FA RID: 16634
	private bool m_isCatchingUp;

	// Token: 0x040040FB RID: 16635
	private float m_catchUpTime;

	// Token: 0x040040FC RID: 16636
	private int m_sequentialPathFails;

	// Token: 0x040040FD RID: 16637
	private float m_repathTimer;
}
