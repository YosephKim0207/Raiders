using System;
using System.Collections.Generic;
using Dungeonator;
using FullInspector;
using UnityEngine;

// Token: 0x02000E0D RID: 3597
public class TargetPlayerBehavior : TargetBehaviorBase
{
	// Token: 0x06004C1E RID: 19486 RVA: 0x0019F578 File Offset: 0x0019D778
	public override void Start()
	{
	}

	// Token: 0x06004C1F RID: 19487 RVA: 0x0019F57C File Offset: 0x0019D77C
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_losTimer, false);
		base.DecrementTimer(ref this.m_coopRefreshSearchTimer, false);
	}

	// Token: 0x06004C20 RID: 19488 RVA: 0x0019F5A0 File Offset: 0x0019D7A0
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (this.m_losTimer > 0f)
		{
			return BehaviorResult.Continue;
		}
		this.m_losTimer = this.SearchInterval;
		if (this.m_behaviorSpeculator.PlayerTarget)
		{
			if (this.m_behaviorSpeculator.PlayerTarget.IsFalling)
			{
				this.m_behaviorSpeculator.PlayerTarget = null;
				if (this.m_aiActor)
				{
					this.m_aiActor.ClearPath();
				}
				return BehaviorResult.SkipRemainingClassBehaviors;
			}
			if (this.m_behaviorSpeculator.PlayerTarget.healthHaver && (this.m_behaviorSpeculator.PlayerTarget.healthHaver.IsDead || this.m_behaviorSpeculator.PlayerTarget.healthHaver.PreventAllDamage))
			{
				this.m_behaviorSpeculator.PlayerTarget = null;
				if (this.m_aiActor)
				{
					this.m_aiActor.ClearPath();
				}
				return BehaviorResult.SkipRemainingClassBehaviors;
			}
		}
		else
		{
			this.m_behaviorSpeculator.PlayerTarget = null;
		}
		if (!this.ObjectPermanence)
		{
			this.m_behaviorSpeculator.PlayerTarget = null;
		}
		if (this.m_behaviorSpeculator.PlayerTarget != null && this.m_behaviorSpeculator.PlayerTarget.IsStealthed)
		{
			this.m_behaviorSpeculator.PlayerTarget = null;
		}
		if (GameManager.Instance.AllPlayers.Length > 1 && this.m_coopRefreshSearchTimer <= 0f)
		{
			this.m_behaviorSpeculator.PlayerTarget = null;
		}
		if (this.m_behaviorSpeculator.PlayerTarget is AIActor)
		{
			float num = Vector2.Distance(this.m_specRigidbody.UnitCenter, this.m_behaviorSpeculator.PlayerTarget.specRigidbody.UnitCenter);
			if (this.m_prevDistToTarget + 3f < num)
			{
				this.m_behaviorSpeculator.PlayerTarget = null;
			}
			this.m_prevDistToTarget = num;
			if (this.m_aiActor && !this.m_aiActor.IsNormalEnemy && this.m_aiActor.CompanionOwner && this.m_behaviorSpeculator.PlayerTarget is AIActor && this.m_behaviorSpeculator.PlayerTarget.GetAbsoluteParentRoom() != this.m_aiActor.CompanionOwner.CurrentRoom)
			{
				this.m_behaviorSpeculator.PlayerTarget = null;
			}
		}
		if (this.m_behaviorSpeculator.PlayerTarget != null)
		{
			return BehaviorResult.Continue;
		}
		PlayerController playerController = GameManager.Instance.GetActivePlayerClosestToPoint(this.m_specRigidbody.UnitCenter, false);
		if (this.m_aiActor && this.m_aiActor.SuppressTargetSwitch)
		{
			playerController = this.m_previousPlayer;
		}
		if (!this.m_aiActor || (this.m_aiActor.CanTargetPlayers && !this.m_aiActor.CanTargetEnemies))
		{
			if (playerController == null)
			{
				return BehaviorResult.Continue;
			}
			this.m_behaviorSpeculator.PlayerTarget = playerController;
			if (GameManager.Instance.AllPlayers.Length > 1)
			{
				this.m_coopRefreshSearchTimer = 1f;
			}
		}
		else if (this.m_aiActor.CanTargetEnemies && !this.m_aiActor.CanTargetPlayers)
		{
			List<AIActor> activeEnemies = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(this.m_aiActor.GridPosition).GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies != null && activeEnemies.Count > 0)
			{
				AIActor aiactor = null;
				float num2 = -1f;
				if (!this.m_aiActor || this.m_aiActor.IsNormalEnemy || !this.m_aiActor.CompanionOwner || !this.m_aiActor.CompanionOwner.IsStealthed)
				{
					for (int i = 0; i < activeEnemies.Count; i++)
					{
						AIActor aiactor2 = activeEnemies[i];
						if (aiactor2 && aiactor2.IsNormalEnemy && !aiactor2.IsGone && !aiactor2.IsHarmlessEnemy)
						{
							if (!(aiactor2 == this.m_aiActor))
							{
								if (!aiactor2.healthHaver || !aiactor2.healthHaver.PreventAllDamage)
								{
									float num3 = Vector2.Distance(this.m_specRigidbody.UnitCenter, aiactor2.specRigidbody.UnitCenter);
									if (aiactor == null || num3 < num2)
									{
										aiactor = aiactor2;
										num2 = num3;
									}
								}
							}
						}
					}
				}
				if (aiactor)
				{
					this.m_behaviorSpeculator.PlayerTarget = aiactor;
					this.m_prevDistToTarget = num2;
				}
			}
		}
		else if (!this.m_aiActor.CanTargetEnemies || this.m_aiActor.CanTargetPlayers)
		{
		}
		if (this.m_aiShooter != null && this.m_behaviorSpeculator.PlayerTarget != null)
		{
			this.m_aiShooter.AimAtPoint(this.m_behaviorSpeculator.PlayerTarget.CenterPosition);
		}
		if (this.m_aiActor && this.PauseOnTargetSwitch && this.m_aiActor.HasBeenEngaged && this.m_previousPlayer && playerController && this.m_previousPlayer != playerController)
		{
			this.m_aiActor.behaviorSpeculator.AttackCooldown = Mathf.Max(this.m_aiActor.behaviorSpeculator.AttackCooldown, this.PauseTime);
			return BehaviorResult.SkipAllRemainingBehaviors;
		}
		this.m_previousPlayer = playerController;
		if (this.m_aiActor && !this.m_aiActor.HasBeenEngaged)
		{
			this.m_aiActor.HasBeenEngaged = true;
			return BehaviorResult.SkipAllRemainingBehaviors;
		}
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x06004C21 RID: 19489 RVA: 0x0019FB98 File Offset: 0x0019DD98
	public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
	{
		base.Init(gameObject, aiActor, aiShooter);
		this.m_specRigidbody = gameObject.GetComponent<SpeculativeRigidbody>();
		this.m_behaviorSpeculator = gameObject.GetComponent<BehaviorSpeculator>();
	}

	// Token: 0x040041F4 RID: 16884
	public float Radius = 10f;

	// Token: 0x040041F5 RID: 16885
	public bool LineOfSight = true;

	// Token: 0x040041F6 RID: 16886
	public bool ObjectPermanence = true;

	// Token: 0x040041F7 RID: 16887
	public float SearchInterval = 0.25f;

	// Token: 0x040041F8 RID: 16888
	public bool PauseOnTargetSwitch;

	// Token: 0x040041F9 RID: 16889
	[InspectorShowIf("PauseOnTargetSwitch")]
	public float PauseTime = 0.25f;

	// Token: 0x040041FA RID: 16890
	private const float PLAYER_REFRESH_TIMER = 1f;

	// Token: 0x040041FB RID: 16891
	private float m_losTimer;

	// Token: 0x040041FC RID: 16892
	private float m_coopRefreshSearchTimer;

	// Token: 0x040041FD RID: 16893
	private float m_prevDistToTarget;

	// Token: 0x040041FE RID: 16894
	private PlayerController m_previousPlayer;

	// Token: 0x040041FF RID: 16895
	private SpeculativeRigidbody m_specRigidbody;

	// Token: 0x04004200 RID: 16896
	private BehaviorSpeculator m_behaviorSpeculator;
}
