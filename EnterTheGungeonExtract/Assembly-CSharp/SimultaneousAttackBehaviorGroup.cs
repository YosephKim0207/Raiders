using System;
using System.Collections.Generic;
using FullInspector;
using UnityEngine;

// Token: 0x02000D51 RID: 3409
[InspectorDropdownName(".Groups/SimultaneousAttackBehaviorGroup")]
public class SimultaneousAttackBehaviorGroup : AttackBehaviorBase, IAttackBehaviorGroup
{
	// Token: 0x06004804 RID: 18436 RVA: 0x0017C220 File Offset: 0x0017A420
	public override void Start()
	{
		base.Start();
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			this.AttackBehaviors[i].Start();
		}
		this.m_finished = new bool[this.AttackBehaviors.Count];
	}

	// Token: 0x06004805 RID: 18437 RVA: 0x0017C278 File Offset: 0x0017A478
	public override void Upkeep()
	{
		base.Upkeep();
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			this.AttackBehaviors[i].Upkeep();
		}
	}

	// Token: 0x06004806 RID: 18438 RVA: 0x0017C2B8 File Offset: 0x0017A4B8
	public override bool OverrideOtherBehaviors()
	{
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			if (this.AttackBehaviors[i].OverrideOtherBehaviors())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004807 RID: 18439 RVA: 0x0017C2FC File Offset: 0x0017A4FC
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
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			BehaviorResult behaviorResult2 = this.AttackBehaviors[i].Update();
			if ((i > 0) & (behaviorResult2 != behaviorResult))
			{
				Debug.LogError("Mismatching result returned from a SimultaneousAttackBehaviorGroup: this is not supported!");
			}
			behaviorResult = behaviorResult2;
			this.m_finished[i] = false;
		}
		return behaviorResult;
	}

	// Token: 0x06004808 RID: 18440 RVA: 0x0017C37C File Offset: 0x0017A57C
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		bool flag = false;
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			if (!this.m_finished[i])
			{
				if (this.AttackBehaviors[i].ContinuousUpdate() == ContinuousBehaviorResult.Continue)
				{
					flag = true;
				}
				else
				{
					this.m_finished[i] = true;
					this.AttackBehaviors[i].EndContinuousUpdate();
				}
			}
		}
		return (!flag) ? ContinuousBehaviorResult.Finished : ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004809 RID: 18441 RVA: 0x0017C404 File Offset: 0x0017A604
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			if (!this.m_finished[i])
			{
				this.AttackBehaviors[i].EndContinuousUpdate();
			}
		}
	}

	// Token: 0x0600480A RID: 18442 RVA: 0x0017C454 File Offset: 0x0017A654
	public override void Destroy()
	{
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			this.AttackBehaviors[i].Destroy();
		}
		base.Destroy();
	}

	// Token: 0x0600480B RID: 18443 RVA: 0x0017C494 File Offset: 0x0017A694
	public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
	{
		base.Init(gameObject, aiActor, aiShooter);
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			this.AttackBehaviors[i].Init(gameObject, aiActor, aiShooter);
		}
	}

	// Token: 0x0600480C RID: 18444 RVA: 0x0017C4DC File Offset: 0x0017A6DC
	public override void SetDeltaTime(float deltaTime)
	{
		base.SetDeltaTime(deltaTime);
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			this.AttackBehaviors[i].SetDeltaTime(deltaTime);
		}
	}

	// Token: 0x0600480D RID: 18445 RVA: 0x0017C520 File Offset: 0x0017A720
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

	// Token: 0x0600480E RID: 18446 RVA: 0x0017C564 File Offset: 0x0017A764
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

	// Token: 0x0600480F RID: 18447 RVA: 0x0017C5C0 File Offset: 0x0017A7C0
	public override float GetMaxRange()
	{
		float num = float.MinValue;
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			num = Mathf.Max(num, this.AttackBehaviors[i].GetMaxRange());
		}
		return num;
	}

	// Token: 0x06004810 RID: 18448 RVA: 0x0017C608 File Offset: 0x0017A808
	public override bool UpdateEveryFrame()
	{
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			if (this.AttackBehaviors[i].UpdateEveryFrame())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004811 RID: 18449 RVA: 0x0017C64C File Offset: 0x0017A84C
	public override bool IsOverridable()
	{
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			if (!this.AttackBehaviors[i].IsOverridable())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004812 RID: 18450 RVA: 0x0017C690 File Offset: 0x0017A890
	public override void OnActorPreDeath()
	{
		base.OnActorPreDeath();
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			this.AttackBehaviors[i].OnActorPreDeath();
		}
	}

	// Token: 0x17000A6C RID: 2668
	// (get) Token: 0x06004813 RID: 18451 RVA: 0x0017C6D0 File Offset: 0x0017A8D0
	public int Count
	{
		get
		{
			return this.AttackBehaviors.Count;
		}
	}

	// Token: 0x06004814 RID: 18452 RVA: 0x0017C6E0 File Offset: 0x0017A8E0
	public AttackBehaviorBase GetAttackBehavior(int index)
	{
		return this.AttackBehaviors[index];
	}

	// Token: 0x04003B80 RID: 15232
	[InspectorCollectionRotorzFlags(HideRemoveButtons = true)]
	public List<AttackBehaviorBase> AttackBehaviors;

	// Token: 0x04003B81 RID: 15233
	private bool[] m_finished;
}
