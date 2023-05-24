using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02000DF3 RID: 3571
public class SimpleTurtleMillAboutBehavior : MovementBehaviorBase
{
	// Token: 0x06004BA3 RID: 19363 RVA: 0x0019BCC0 File Offset: 0x00199EC0
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x06004BA4 RID: 19364 RVA: 0x0019BCC8 File Offset: 0x00199EC8
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_repathTimer, false);
		base.DecrementTimer(ref this.m_newPositionTimer, false);
	}

	// Token: 0x06004BA5 RID: 19365 RVA: 0x0019BCEC File Offset: 0x00199EEC
	private Vector2 GetNewTargetPosition(PlayerController owner)
	{
		Vector2? vector = null;
		int num = 30;
		while (vector == null && num > 0)
		{
			num--;
			Vector2 vector2 = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(0.5f, 1f);
			vector = new Vector2?(owner.specRigidbody.HitboxPixelCollider.UnitCenter + vector2 * this.MillRadius);
			vector = ((vector == null) ? null : new Vector2?(vector.GetValueOrDefault() + owner.specRigidbody.Velocity.normalized * UnityEngine.Random.Range(0f, this.MillRadius * 1.5f)));
			CellData cell = vector.Value.GetCell();
			if (cell == null || cell.type != CellType.FLOOR || !cell.IsPassable)
			{
				vector = null;
			}
		}
		if (vector == null)
		{
			return owner.specRigidbody.HitboxPixelCollider.UnitBottomCenter;
		}
		return vector.Value;
	}

	// Token: 0x06004BA6 RID: 19366 RVA: 0x0019BE1C File Offset: 0x0019A01C
	public override BehaviorResult Update()
	{
		if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel)
		{
			return BehaviorResult.SkipAllRemainingBehaviors;
		}
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			this.m_aiActor.ClearPath();
			return BehaviorResult.SkipAllRemainingBehaviors;
		}
		PlayerController playerController = GameManager.Instance.PrimaryPlayer;
		if (this.m_aiActor && this.m_aiActor.CompanionOwner)
		{
			playerController = this.m_aiActor.CompanionOwner;
		}
		this.m_aiActor.MovementSpeed = this.m_aiActor.BaseMovementSpeed;
		if (!playerController || !playerController.IsInCombat)
		{
			return BehaviorResult.Continue;
		}
		float num = Vector2.Distance(playerController.CenterPosition, this.m_currentTargetPosition);
		float num2 = Vector2.Distance(this.m_aiActor.CenterPosition, this.m_currentTargetPosition);
		if (this.m_newPositionTimer <= 0f || num > this.MillRadius * 1.75f || num2 <= 0.25f)
		{
			this.m_aiActor.ClearPath();
			this.m_currentTargetPosition = this.GetNewTargetPosition(playerController);
			this.m_newPositionTimer = this.TargetInterval;
		}
		else if (num2 > 30f)
		{
			this.m_aiActor.CompanionWarp(this.m_aiActor.CompanionOwner.CenterPosition);
		}
		this.m_aiActor.MovementSpeed = Mathf.Lerp(this.m_aiActor.BaseMovementSpeed, this.m_aiActor.BaseMovementSpeed * 2f, Mathf.Clamp01(num2 / 30f));
		if (this.m_repathTimer <= 0f && !playerController.IsOverPitAtAll && !playerController.IsInMinecart)
		{
			this.m_repathTimer = this.PathInterval;
			this.m_aiActor.FallingProhibited = false;
			this.m_aiActor.PathfindToPosition(this.m_currentTargetPosition, null, true, null, null, null, false);
			if (this.m_aiActor.Path != null && this.m_aiActor.Path.InaccurateLength > 50f)
			{
				this.m_aiActor.ClearPath();
				this.m_aiActor.CompanionWarp(this.m_aiActor.CompanionOwner.CenterPosition);
			}
			else if (this.m_aiActor.Path != null && !this.m_aiActor.Path.WillReachFinalGoal)
			{
				this.m_aiActor.CompanionWarp(this.m_aiActor.CompanionOwner.CenterPosition);
			}
		}
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x0400415E RID: 16734
	public float PathInterval = 0.25f;

	// Token: 0x0400415F RID: 16735
	public float TargetInterval = 3f;

	// Token: 0x04004160 RID: 16736
	public float MillRadius = 5f;

	// Token: 0x04004161 RID: 16737
	private Vector2 m_currentTargetPosition;

	// Token: 0x04004162 RID: 16738
	private float m_repathTimer;

	// Token: 0x04004163 RID: 16739
	private float m_newPositionTimer;
}
