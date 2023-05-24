using System;
using System.Collections.Generic;
using FullInspector;
using UnityEngine;

// Token: 0x02000D3E RID: 3390
[InspectorDropdownName(".Groups/SequentialAttackBehaviorGroup")]
public class SequentialAttackBehaviorGroup : AttackBehaviorBase
{
	// Token: 0x17000A62 RID: 2658
	// (get) Token: 0x06004792 RID: 18322 RVA: 0x00178AD8 File Offset: 0x00176CD8
	private AttackBehaviorBase currentBehavior
	{
		get
		{
			return this.AttackBehaviors[this.m_currentIndex];
		}
	}

	// Token: 0x06004793 RID: 18323 RVA: 0x00178AEC File Offset: 0x00176CEC
	public override void Start()
	{
		base.Start();
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			this.AttackBehaviors[i].Start();
		}
	}

	// Token: 0x06004794 RID: 18324 RVA: 0x00178B2C File Offset: 0x00176D2C
	public override void Upkeep()
	{
		base.Upkeep();
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			this.AttackBehaviors[i].Upkeep();
		}
	}

	// Token: 0x06004795 RID: 18325 RVA: 0x00178B6C File Offset: 0x00176D6C
	public override bool OverrideOtherBehaviors()
	{
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			if (!this.AttackBehaviors[i].OverrideOtherBehaviors())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004796 RID: 18326 RVA: 0x00178BB0 File Offset: 0x00176DB0
	public override BehaviorResult Update()
	{
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (!this.IsReady())
		{
			return BehaviorResult.Continue;
		}
		this.m_currentIndex = 0;
		this.m_state = SequentialAttackBehaviorGroup.State.Update;
		bool flag = this.StepBehaviors();
		if (flag)
		{
			return (!this.RunInClass) ? BehaviorResult.RunContinuous : BehaviorResult.RunContinuousInClass;
		}
		return BehaviorResult.SkipAllRemainingBehaviors;
	}

	// Token: 0x06004797 RID: 18327 RVA: 0x00178C08 File Offset: 0x00176E08
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		bool flag = this.StepBehaviors();
		return (!flag) ? ContinuousBehaviorResult.Finished : ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004798 RID: 18328 RVA: 0x00178C30 File Offset: 0x00176E30
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.m_currentIndex < this.AttackBehaviors.Count)
		{
			this.AttackBehaviors[this.m_currentIndex].EndContinuousUpdate();
		}
		this.m_currentIndex = -1;
	}

	// Token: 0x06004799 RID: 18329 RVA: 0x00178C6C File Offset: 0x00176E6C
	public override void Destroy()
	{
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			this.AttackBehaviors[i].Destroy();
		}
		base.Destroy();
	}

	// Token: 0x0600479A RID: 18330 RVA: 0x00178CAC File Offset: 0x00176EAC
	private bool StepBehaviors()
	{
		if (this.m_state == SequentialAttackBehaviorGroup.State.Cooldown)
		{
			this.m_overrideCooldownTimer += this.m_deltaTime;
			if (this.m_currentIndex == this.AttackBehaviors.Count - 1)
			{
				return false;
			}
			bool flag = false;
			if (this.OverrideCooldowns != null && this.OverrideCooldowns.Count > 0)
			{
				flag = this.m_overrideCooldownTimer >= this.OverrideCooldowns[this.m_currentIndex];
			}
			else if (this.currentBehavior.IsReady())
			{
				flag = true;
			}
			if (flag)
			{
				this.m_currentIndex++;
				this.m_state = SequentialAttackBehaviorGroup.State.Update;
				return this.StepBehaviors();
			}
			return true;
		}
		else if (this.m_state == SequentialAttackBehaviorGroup.State.Update)
		{
			BehaviorResult behaviorResult = this.currentBehavior.Update();
			if (behaviorResult == BehaviorResult.Continue || behaviorResult == BehaviorResult.SkipRemainingClassBehaviors || behaviorResult == BehaviorResult.SkipAllRemainingBehaviors)
			{
				this.m_state = SequentialAttackBehaviorGroup.State.Cooldown;
				this.m_overrideCooldownTimer = 0f;
				return this.StepBehaviors();
			}
			if (behaviorResult == BehaviorResult.RunContinuous || behaviorResult == BehaviorResult.RunContinuousInClass)
			{
				this.m_state = SequentialAttackBehaviorGroup.State.ContinuousUpdate;
				return true;
			}
			Debug.LogError("Unrecognized BehaviorResult " + behaviorResult);
			return false;
		}
		else
		{
			if (this.m_state != SequentialAttackBehaviorGroup.State.ContinuousUpdate)
			{
				Debug.LogError("Unrecognized State " + this.m_state);
				return false;
			}
			ContinuousBehaviorResult continuousBehaviorResult = this.currentBehavior.ContinuousUpdate();
			if (continuousBehaviorResult == ContinuousBehaviorResult.Finished)
			{
				this.currentBehavior.EndContinuousUpdate();
				this.m_state = SequentialAttackBehaviorGroup.State.Cooldown;
				this.m_overrideCooldownTimer = 0f;
				return this.StepBehaviors();
			}
			if (continuousBehaviorResult == ContinuousBehaviorResult.Continue)
			{
				return true;
			}
			Debug.LogError("Unrecognized BehaviorResult " + continuousBehaviorResult);
			return false;
		}
	}

	// Token: 0x0600479B RID: 18331 RVA: 0x00178E58 File Offset: 0x00177058
	public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
	{
		base.Init(gameObject, aiActor, aiShooter);
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			this.AttackBehaviors[i].Init(gameObject, aiActor, aiShooter);
		}
	}

	// Token: 0x0600479C RID: 18332 RVA: 0x00178EA0 File Offset: 0x001770A0
	public override void SetDeltaTime(float deltaTime)
	{
		base.SetDeltaTime(deltaTime);
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			this.AttackBehaviors[i].SetDeltaTime(deltaTime);
		}
	}

	// Token: 0x0600479D RID: 18333 RVA: 0x00178EE4 File Offset: 0x001770E4
	public override bool IsReady()
	{
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			if (!this.AttackBehaviors[i].IsReady())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600479E RID: 18334 RVA: 0x00178F28 File Offset: 0x00177128
	public override float GetMinReadyRange()
	{
		if (!this.IsReady())
		{
			return -1f;
		}
		float num = float.MaxValue;
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			num = Mathf.Min(num, this.AttackBehaviors[i].GetMinReadyRange());
		}
		return num;
	}

	// Token: 0x0600479F RID: 18335 RVA: 0x00178F84 File Offset: 0x00177184
	public override float GetMaxRange()
	{
		float num = float.MinValue;
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			num = Mathf.Max(num, this.AttackBehaviors[i].GetMaxRange());
		}
		return num;
	}

	// Token: 0x060047A0 RID: 18336 RVA: 0x00178FCC File Offset: 0x001771CC
	public override bool UpdateEveryFrame()
	{
		return this.m_currentIndex >= 0 && this.currentBehavior.UpdateEveryFrame();
	}

	// Token: 0x060047A1 RID: 18337 RVA: 0x00178FE8 File Offset: 0x001771E8
	public override bool IsOverridable()
	{
		return this.m_currentIndex < 0 || this.currentBehavior.IsOverridable();
	}

	// Token: 0x060047A2 RID: 18338 RVA: 0x00179004 File Offset: 0x00177204
	public override void OnActorPreDeath()
	{
		base.OnActorPreDeath();
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			this.AttackBehaviors[i].OnActorPreDeath();
		}
	}

	// Token: 0x17000A63 RID: 2659
	// (get) Token: 0x060047A3 RID: 18339 RVA: 0x00179044 File Offset: 0x00177244
	public int Count
	{
		get
		{
			return this.AttackBehaviors.Count;
		}
	}

	// Token: 0x060047A4 RID: 18340 RVA: 0x00179054 File Offset: 0x00177254
	public AttackBehaviorBase GetAttackBehavior(int index)
	{
		return this.AttackBehaviors[index];
	}

	// Token: 0x04003AC4 RID: 15044
	public bool RunInClass;

	// Token: 0x04003AC5 RID: 15045
	[InspectorCollectionRotorzFlags(HideRemoveButtons = true)]
	public List<AttackBehaviorBase> AttackBehaviors;

	// Token: 0x04003AC6 RID: 15046
	public List<float> OverrideCooldowns;

	// Token: 0x04003AC7 RID: 15047
	private int m_currentIndex = -1;

	// Token: 0x04003AC8 RID: 15048
	private float m_overrideCooldownTimer;

	// Token: 0x04003AC9 RID: 15049
	private SequentialAttackBehaviorGroup.State m_state;

	// Token: 0x02000D3F RID: 3391
	private enum State
	{
		// Token: 0x04003ACB RID: 15051
		Idle,
		// Token: 0x04003ACC RID: 15052
		Update,
		// Token: 0x04003ACD RID: 15053
		ContinuousUpdate,
		// Token: 0x04003ACE RID: 15054
		Cooldown
	}
}
