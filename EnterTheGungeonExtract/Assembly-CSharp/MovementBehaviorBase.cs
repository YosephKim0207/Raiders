using System;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x02000DFB RID: 3579
public abstract class MovementBehaviorBase : BehaviorBase
{
	// Token: 0x17000AA2 RID: 2722
	// (get) Token: 0x06004BC7 RID: 19399 RVA: 0x0019D834 File Offset: 0x0019BA34
	public virtual float DesiredCombatDistance
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x06004BC8 RID: 19400 RVA: 0x0019D83C File Offset: 0x0019BA3C
	public override void Start()
	{
		base.Start();
		this.m_behaviorSpeculator = this.m_aiActor.behaviorSpeculator;
	}

	// Token: 0x06004BC9 RID: 19401 RVA: 0x0019D858 File Offset: 0x0019BA58
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_fleeRepathTimer, false);
		this.UpdateFearVFX();
	}

	// Token: 0x06004BCA RID: 19402 RVA: 0x0019D874 File Offset: 0x0019BA74
	public override BehaviorResult Update()
	{
		base.Update();
		if (this.ShouldFleePlayer())
		{
			if (!this.m_isFleeing && this.m_behaviorSpeculator.IsInterruptable)
			{
				this.m_behaviorSpeculator.Interrupt();
			}
			this.m_isFleeing = true;
			this.UpdateFearVFX();
			FleePlayerData fleeData = this.m_behaviorSpeculator.FleePlayerData;
			if (this.m_fleeRepathTimer <= 0f)
			{
				Vector2 pointOfFear = fleeData.Player.CenterPosition;
				CellValidator cellValidator = (IntVector2 p) => Vector2.Distance(p.ToCenterVector2(), pointOfFear) > fleeData.StopDistance;
				CellTypes cellTypes = this.m_aiActor.PathableTiles;
				if (this.m_aiActor.DistanceToTarget < fleeData.DeathDistance)
				{
					cellTypes |= CellTypes.PIT;
				}
				IntVector2? nearestAvailableCell = this.m_aiActor.ParentRoom.GetNearestAvailableCell(this.m_aiActor.specRigidbody.UnitCenter, new IntVector2?(this.m_aiActor.Clearance), new CellTypes?(cellTypes), false, cellValidator);
				if (nearestAvailableCell != null)
				{
					AIActor aiActor = this.m_aiActor;
					Vector2 vector = nearestAvailableCell.Value.ToCenterVector2();
					CellTypes? cellTypes2 = new CellTypes?(cellTypes);
					aiActor.PathfindToPosition(vector, null, true, null, null, cellTypes2, false);
					this.m_fleeRepathTimer = MovementBehaviorBase.FleePathInterval;
				}
				else
				{
					this.m_aiActor.ClearPath();
				}
			}
			return BehaviorResult.SkipRemainingClassBehaviors;
		}
		this.m_isFleeing = false;
		this.UpdateFearVFX();
		return base.Update();
	}

	// Token: 0x06004BCB RID: 19403 RVA: 0x0019D9F8 File Offset: 0x0019BBF8
	public override void OnActorPreDeath()
	{
		base.OnActorPreDeath();
		this.m_isFleeing = false;
		if (this.m_extantFearVFX != null)
		{
			SpawnManager.Despawn(this.m_extantFearVFX.gameObject);
			this.m_extantFearVFX = null;
		}
	}

	// Token: 0x17000AA3 RID: 2723
	// (get) Token: 0x06004BCC RID: 19404 RVA: 0x0019DA30 File Offset: 0x0019BC30
	public virtual bool AllowFearRunState
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004BCD RID: 19405 RVA: 0x0019DA34 File Offset: 0x0019BC34
	private bool ShouldFleePlayer()
	{
		if (this.m_behaviorSpeculator == null)
		{
			return false;
		}
		FleePlayerData fleePlayerData = this.m_behaviorSpeculator.FleePlayerData;
		if (fleePlayerData == null || !fleePlayerData.Player)
		{
			return false;
		}
		float num = Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, fleePlayerData.Player.CenterPosition);
		return (this.m_isFleeing && num < fleePlayerData.StopDistance) || num < fleePlayerData.StartDistance;
	}

	// Token: 0x06004BCE RID: 19406 RVA: 0x0019DABC File Offset: 0x0019BCBC
	protected virtual void UpdateFearVFX()
	{
		if (!this.m_isFleeing && this.m_extantFearVFX != null)
		{
			if (this.m_extantFearVFX.IsPlaying("fear_face_vfx"))
			{
				this.m_extantFearVFX.Play("fear_face_vfx_out");
			}
			else if (!this.m_extantFearVFX.Playing)
			{
				SpawnManager.Despawn(this.m_extantFearVFX.gameObject);
				this.m_extantFearVFX = null;
			}
		}
		else if (this.m_isFleeing && this.m_extantFearVFX == null)
		{
			this.m_extantFearVFX = this.m_aiActor.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Fear") as GameObject, (this.m_aiActor.sprite.WorldTopCenter - this.m_aiActor.CenterPosition).WithX(0f), true, true, false).GetComponent<tk2dSpriteAnimator>();
		}
		else if (this.m_isFleeing && this.m_extantFearVFX != null)
		{
			if (!this.m_extantFearVFX.IsPlaying("fear_face_vfx"))
			{
				this.m_extantFearVFX.Play("fear_face_vfx");
			}
			this.m_extantFearVFX.transform.position = this.m_aiActor.sprite.WorldTopCenter.ToVector3ZUp(this.m_extantFearVFX.transform.position.z);
		}
	}

	// Token: 0x040041AC RID: 16812
	private static float FleePathInterval = 0.25f;

	// Token: 0x040041AD RID: 16813
	private BehaviorSpeculator m_behaviorSpeculator;

	// Token: 0x040041AE RID: 16814
	private tk2dSpriteAnimator m_extantFearVFX;

	// Token: 0x040041AF RID: 16815
	private float m_fleeRepathTimer;

	// Token: 0x040041B0 RID: 16816
	private bool m_isFleeing;
}
