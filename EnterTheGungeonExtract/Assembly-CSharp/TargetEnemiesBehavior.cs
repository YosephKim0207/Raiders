using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000E0C RID: 3596
public class TargetEnemiesBehavior : TargetBehaviorBase
{
	// Token: 0x06004C1A RID: 19482 RVA: 0x0019F23C File Offset: 0x0019D43C
	public override void Start()
	{
	}

	// Token: 0x06004C1B RID: 19483 RVA: 0x0019F240 File Offset: 0x0019D440
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_losTimer, false);
	}

	// Token: 0x06004C1C RID: 19484 RVA: 0x0019F258 File Offset: 0x0019D458
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
		if (this.m_aiActor.PlayerTarget)
		{
			if (this.m_aiActor.PlayerTarget.IsFalling)
			{
				this.m_aiActor.PlayerTarget = null;
				this.m_aiActor.ClearPath();
				return BehaviorResult.SkipRemainingClassBehaviors;
			}
			if (this.m_aiActor.PlayerTarget.healthHaver && this.m_aiActor.PlayerTarget.healthHaver.IsDead)
			{
				this.m_aiActor.PlayerTarget = null;
				this.m_aiActor.ClearPath();
				return BehaviorResult.SkipRemainingClassBehaviors;
			}
		}
		else
		{
			this.m_aiActor.PlayerTarget = null;
		}
		if (!this.ObjectPermanence)
		{
			this.m_aiActor.PlayerTarget = null;
		}
		if (this.m_aiActor.PlayerTarget != null)
		{
			return BehaviorResult.Continue;
		}
		if (!this.m_aiActor.CanTargetEnemies)
		{
			return BehaviorResult.Continue;
		}
		List<AIActor> activeEnemies = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(this.m_aiActor.GridPosition).GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		if (activeEnemies != null && activeEnemies.Count > 0)
		{
			AIActor aiactor = null;
			float num = float.MaxValue;
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				AIActor aiactor2 = activeEnemies[i];
				if (!(aiactor2 == this.m_aiActor))
				{
					float num2 = Vector2.Distance(this.m_aiActor.CenterPosition, aiactor2.CenterPosition);
					if (num2 < num)
					{
						if (this.LineOfSight)
						{
							int standardPlayerVisibilityMask = CollisionMask.StandardPlayerVisibilityMask;
							RaycastResult raycastResult;
							if (!PhysicsEngine.Instance.Raycast(this.m_aiActor.CenterPosition, aiactor2.CenterPosition - this.m_aiActor.CenterPosition, num2, out raycastResult, true, true, standardPlayerVisibilityMask, null, false, null, this.m_aiActor.specRigidbody))
							{
								RaycastResult.Pool.Free(ref raycastResult);
								goto IL_258;
							}
							if (raycastResult.SpeculativeRigidbody == null || raycastResult.SpeculativeRigidbody.GetComponent<PlayerController>() == null)
							{
								RaycastResult.Pool.Free(ref raycastResult);
								goto IL_258;
							}
							RaycastResult.Pool.Free(ref raycastResult);
						}
						aiactor = aiactor2;
						num = num2;
					}
				}
				IL_258:;
			}
			this.m_aiActor.PlayerTarget = aiactor;
		}
		if (this.m_aiShooter != null && this.m_aiActor.PlayerTarget != null)
		{
			this.m_aiShooter.AimAtPoint(this.m_aiActor.PlayerTarget.CenterPosition);
		}
		if (!this.m_aiActor.HasBeenEngaged)
		{
			this.m_aiActor.HasBeenEngaged = true;
			return BehaviorResult.SkipAllRemainingBehaviors;
		}
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x040041F0 RID: 16880
	public bool LineOfSight = true;

	// Token: 0x040041F1 RID: 16881
	public bool ObjectPermanence = true;

	// Token: 0x040041F2 RID: 16882
	public float SearchInterval = 0.25f;

	// Token: 0x040041F3 RID: 16883
	private float m_losTimer;
}
