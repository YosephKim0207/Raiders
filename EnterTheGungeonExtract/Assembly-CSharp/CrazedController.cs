using System;
using System.Collections.Generic;
using Dungeonator;

// Token: 0x02001085 RID: 4229
public class CrazedController : BraveBehaviour
{
	// Token: 0x06005D17 RID: 23831 RVA: 0x0023A8FC File Offset: 0x00238AFC
	public void Update()
	{
		if (!base.aiActor || !base.aiActor.enabled || !base.healthHaver || base.healthHaver.IsDead)
		{
			return;
		}
		if (this.m_state == CrazedController.State.Idle)
		{
			if (this.TriggerWhenLastEnemy && base.behaviorSpeculator.enabled)
			{
				base.aiActor.ParentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear, ref CrazedController.s_activeEnemies);
				bool flag = false;
				for (int i = 0; i < CrazedController.s_activeEnemies.Count; i++)
				{
					if (CrazedController.s_activeEnemies[i].EnemyGuid != base.aiActor.EnemyGuid)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.GoCrazed();
				}
			}
		}
		else if (this.m_state == CrazedController.State.Transforming)
		{
			base.behaviorSpeculator.GlobalCooldown = 1f;
			if (!base.behaviorSpeculator.enabled)
			{
				this.m_state = CrazedController.State.Idle;
				return;
			}
			if (!base.aiAnimator.IsPlaying(this.TellAnimation))
			{
				this.DoCrazedBehavior();
			}
		}
		else if (this.m_state == CrazedController.State.Crazed)
		{
		}
	}

	// Token: 0x06005D18 RID: 23832 RVA: 0x0023AA44 File Offset: 0x00238C44
	public void GoCrazed()
	{
		base.aiActor.ClearPath();
		base.behaviorSpeculator.GlobalCooldown = 1f;
		if (this.DisableHitAnims)
		{
			base.aiActor.aiAnimator.HitAnimation.Type = DirectionalAnimation.DirectionType.None;
		}
		if (!string.IsNullOrEmpty(this.TellAnimation))
		{
			if (!this.SpecifyTellDuration)
			{
				base.aiAnimator.PlayUntilFinished(this.TellAnimation, true, null, -1f, false);
			}
			else
			{
				base.aiAnimator.PlayForDurationOrUntilFinished(this.TellAnimation, this.TellDuration, true, null, -1f, false);
			}
			this.m_state = CrazedController.State.Transforming;
		}
		else
		{
			this.DoCrazedBehavior();
		}
	}

	// Token: 0x06005D19 RID: 23833 RVA: 0x0023AAFC File Offset: 0x00238CFC
	private void DoCrazedBehavior()
	{
		base.behaviorSpeculator.GlobalCooldown = 0f;
		if (this.DoCharge)
		{
			base.aiAnimator.SetBaseAnim(this.CrazedAnimaton, false);
			base.behaviorSpeculator.MovementBehaviors.Clear();
			base.behaviorSpeculator.AttackBehaviors.Clear();
			SeekTargetBehavior seekTargetBehavior = new SeekTargetBehavior
			{
				StopWhenInRange = false,
				CustomRange = -1f,
				LineOfSight = false,
				ReturnToSpawn = false,
				SpawnTetherDistance = 0f,
				PathInterval = 0.25f
			};
			base.behaviorSpeculator.MovementBehaviors.Add(seekTargetBehavior);
			base.behaviorSpeculator.RefreshBehaviors();
			base.behaviorSpeculator.enabled = true;
			if (this.CrazedRunSpeed > 0f)
			{
				base.aiActor.MovementSpeed = this.CrazedRunSpeed;
			}
			base.aiActor.CollisionDamage = 0.5f;
		}
		if (this.EnableBehavior)
		{
			for (int i = 0; i < base.behaviorSpeculator.AttackBehaviors.Count; i++)
			{
				if (base.behaviorSpeculator.AttackBehaviors[i] is AttackBehaviorGroup)
				{
					this.ProcessAttackGroup(base.behaviorSpeculator.AttackBehaviors[i] as AttackBehaviorGroup);
				}
			}
			base.behaviorSpeculator.enabled = true;
		}
		this.m_state = CrazedController.State.Crazed;
	}

	// Token: 0x06005D1A RID: 23834 RVA: 0x0023AC64 File Offset: 0x00238E64
	private void ProcessAttackGroup(AttackBehaviorGroup attackGroup)
	{
		for (int i = 0; i < attackGroup.AttackBehaviors.Count; i++)
		{
			AttackBehaviorGroup.AttackGroupItem attackGroupItem = attackGroup.AttackBehaviors[i];
			if (attackGroupItem.NickName == this.BehaviorName)
			{
				attackGroupItem.Probability = 1f;
			}
			if (attackGroupItem.Behavior is AttackBehaviorGroup)
			{
				this.ProcessAttackGroup(attackGroupItem.Behavior as AttackBehaviorGroup);
			}
		}
	}

	// Token: 0x040056E0 RID: 22240
	[CheckDirectionalAnimation(null)]
	public string TellAnimation;

	// Token: 0x040056E1 RID: 22241
	public bool SpecifyTellDuration = true;

	// Token: 0x040056E2 RID: 22242
	[ShowInInspectorIf("SpecifyTellDuration", true)]
	public float TellDuration;

	// Token: 0x040056E3 RID: 22243
	public bool DoCharge = true;

	// Token: 0x040056E4 RID: 22244
	[ShowInInspectorIf("DoCharge", true)]
	public string CrazedAnimaton;

	// Token: 0x040056E5 RID: 22245
	[ShowInInspectorIf("DoCharge", true)]
	public float CrazedRunSpeed = -1f;

	// Token: 0x040056E6 RID: 22246
	public bool EnableBehavior;

	// Token: 0x040056E7 RID: 22247
	[ShowInInspectorIf("EnableBehavior", true)]
	public string BehaviorName;

	// Token: 0x040056E8 RID: 22248
	public bool TriggerWhenLastEnemy = true;

	// Token: 0x040056E9 RID: 22249
	public bool DisableHitAnims;

	// Token: 0x040056EA RID: 22250
	private CrazedController.State m_state;

	// Token: 0x040056EB RID: 22251
	private static List<AIActor> s_activeEnemies = new List<AIActor>();

	// Token: 0x02001086 RID: 4230
	private enum State
	{
		// Token: 0x040056ED RID: 22253
		Idle,
		// Token: 0x040056EE RID: 22254
		Transforming,
		// Token: 0x040056EF RID: 22255
		Crazed
	}
}
