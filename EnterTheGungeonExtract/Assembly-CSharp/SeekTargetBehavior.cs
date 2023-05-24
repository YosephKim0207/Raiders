using System;
using FullInspector;
using Pathfinding;
using UnityEngine;

// Token: 0x02000DF1 RID: 3569
public class SeekTargetBehavior : RangedMovementBehavior
{
	// Token: 0x06004B9D RID: 19357 RVA: 0x0019BA10 File Offset: 0x00199C10
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_repathTimer, false);
	}

	// Token: 0x06004B9E RID: 19358 RVA: 0x0019BA28 File Offset: 0x00199C28
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		SpeculativeRigidbody targetRigidbody = this.m_aiActor.TargetRigidbody;
		if (!base.InRange() || !targetRigidbody)
		{
			if (this.m_state == SeekTargetBehavior.State.PathingToTarget)
			{
				this.m_aiActor.ClearPath();
				this.m_state = SeekTargetBehavior.State.Idle;
			}
			else if (this.m_state == SeekTargetBehavior.State.Idle)
			{
				if (this.ReturnToSpawn && this.m_aiActor.GridPosition != this.m_aiActor.SpawnGridPosition && this.m_aiActor.PathComplete)
				{
					this.m_aiActor.PathfindToPosition(this.m_aiActor.SpawnPosition, null, true, null, null, null, false);
					this.m_state = SeekTargetBehavior.State.ReturningToSpawn;
				}
			}
			else if (this.m_state == SeekTargetBehavior.State.ReturningToSpawn && this.m_aiActor.PathComplete)
			{
				this.m_state = SeekTargetBehavior.State.Idle;
			}
			return BehaviorResult.Continue;
		}
		bool flag = this.m_aiActor.HasLineOfSightToTarget;
		float desiredCombatDistance = this.m_aiActor.DesiredCombatDistance;
		this.m_state = SeekTargetBehavior.State.PathingToTarget;
		if (this.m_aiActor.TargetRigidbody && this.m_aiActor.TargetRigidbody.aiActor && !this.m_aiActor.TargetRigidbody.CollideWithOthers)
		{
			flag = true;
		}
		if (this.ExternalCooldownSource)
		{
			this.m_aiActor.ClearPath();
			return BehaviorResult.Continue;
		}
		if (this.StopWhenInRange && this.m_aiActor.DistanceToTarget <= desiredCombatDistance && (!this.LineOfSight || flag))
		{
			this.m_aiActor.ClearPath();
			return BehaviorResult.Continue;
		}
		if (this.m_repathTimer <= 0f)
		{
			CellValidator cellValidator = null;
			if (this.SpawnTetherDistance > 0f)
			{
				cellValidator = (IntVector2 p) => Vector2.Distance(p.ToCenterVector2(), this.m_aiActor.SpawnPosition) < this.SpawnTetherDistance;
			}
			Vector2 unitCenter = targetRigidbody.UnitCenter;
			AIActor aiActor = this.m_aiActor;
			Vector2 vector = unitCenter;
			CellValidator cellValidator2 = cellValidator;
			aiActor.PathfindToPosition(vector, null, true, cellValidator2, null, null, false);
			this.m_repathTimer = this.PathInterval;
		}
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x17000A9D RID: 2717
	// (get) Token: 0x06004B9F RID: 19359 RVA: 0x0019BC60 File Offset: 0x00199E60
	public override float DesiredCombatDistance
	{
		get
		{
			return this.CustomRange;
		}
	}

	// Token: 0x17000A9E RID: 2718
	// (get) Token: 0x06004BA0 RID: 19360 RVA: 0x0019BC68 File Offset: 0x00199E68
	public override bool AllowFearRunState
	{
		get
		{
			return true;
		}
	}

	// Token: 0x04004151 RID: 16721
	public bool StopWhenInRange = true;

	// Token: 0x04004152 RID: 16722
	public float CustomRange = -1f;

	// Token: 0x04004153 RID: 16723
	[InspectorShowIf("StopWhenInRange")]
	public bool LineOfSight = true;

	// Token: 0x04004154 RID: 16724
	public bool ReturnToSpawn = true;

	// Token: 0x04004155 RID: 16725
	public float SpawnTetherDistance;

	// Token: 0x04004156 RID: 16726
	public float PathInterval = 0.25f;

	// Token: 0x04004157 RID: 16727
	[NonSerialized]
	public bool ExternalCooldownSource;

	// Token: 0x04004158 RID: 16728
	private float m_repathTimer;

	// Token: 0x04004159 RID: 16729
	private SeekTargetBehavior.State m_state;

	// Token: 0x02000DF2 RID: 3570
	private enum State
	{
		// Token: 0x0400415B RID: 16731
		Idle,
		// Token: 0x0400415C RID: 16732
		PathingToTarget,
		// Token: 0x0400415D RID: 16733
		ReturningToSpawn
	}
}
