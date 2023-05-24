using System;
using System.Collections.Generic;
using FullInspector;
using UnityEngine;

// Token: 0x02000D0C RID: 3340
[InspectorDropdownName(".Groups/AttackBehaviorGroup")]
public class AttackBehaviorGroup : AttackBehaviorBase, IAttackBehaviorGroup
{
	// Token: 0x17000A55 RID: 2645
	// (get) Token: 0x0600466D RID: 18029 RVA: 0x0016D900 File Offset: 0x0016BB00
	public AttackBehaviorBase CurrentBehavior
	{
		get
		{
			return this.m_currentBehavior;
		}
	}

	// Token: 0x0600466E RID: 18030 RVA: 0x0016D908 File Offset: 0x0016BB08
	public override void Start()
	{
		base.Start();
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			if (this.AttackBehaviors[i].Behavior != null)
			{
				this.AttackBehaviors[i].Behavior.Start();
			}
		}
	}

	// Token: 0x0600466F RID: 18031 RVA: 0x0016D964 File Offset: 0x0016BB64
	public override void Upkeep()
	{
		base.Upkeep();
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			if (this.AttackBehaviors[i].Behavior != null)
			{
				this.AttackBehaviors[i].Behavior.Upkeep();
			}
		}
	}

	// Token: 0x06004670 RID: 18032 RVA: 0x0016D9C0 File Offset: 0x0016BBC0
	public override bool OverrideOtherBehaviors()
	{
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			if (this.AttackBehaviors[i].Behavior != null && this.AttackBehaviors[i].Behavior.OverrideOtherBehaviors())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004671 RID: 18033 RVA: 0x0016DA20 File Offset: 0x0016BC20
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
		float num = 0f;
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			if (this.AttackBehaviors[i].Probability > 0f && this.AttackBehaviors[i].Behavior.IsReady())
			{
				num += this.AttackBehaviors[i].Probability;
			}
		}
		if (num == 0f)
		{
			return BehaviorResult.Continue;
		}
		float num2 = UnityEngine.Random.Range(0f, num);
		for (int j = 0; j < this.AttackBehaviors.Count; j++)
		{
			if (this.AttackBehaviors[j].Probability > 0f && this.AttackBehaviors[j].Behavior.IsReady())
			{
				this.m_currentBehavior = this.AttackBehaviors[j].Behavior;
				if (num2 < this.AttackBehaviors[j].Probability)
				{
					break;
				}
				num2 -= this.AttackBehaviors[j].Probability;
			}
		}
		return this.m_currentBehavior.Update();
	}

	// Token: 0x06004672 RID: 18034 RVA: 0x0016DB7C File Offset: 0x0016BD7C
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		base.ContinuousUpdate();
		return this.m_currentBehavior.ContinuousUpdate();
	}

	// Token: 0x06004673 RID: 18035 RVA: 0x0016DB90 File Offset: 0x0016BD90
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		if (this.m_currentBehavior != null)
		{
			this.m_currentBehavior.EndContinuousUpdate();
			this.m_currentBehavior = null;
		}
	}

	// Token: 0x06004674 RID: 18036 RVA: 0x0016DBB8 File Offset: 0x0016BDB8
	public override void Destroy()
	{
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			if (this.AttackBehaviors[i].Behavior != null)
			{
				this.AttackBehaviors[i].Behavior.Destroy();
			}
		}
		base.Destroy();
	}

	// Token: 0x06004675 RID: 18037 RVA: 0x0016DC14 File Offset: 0x0016BE14
	public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
	{
		base.Init(gameObject, aiActor, aiShooter);
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			if (this.AttackBehaviors[i].Behavior != null)
			{
				this.AttackBehaviors[i].Behavior.Init(gameObject, aiActor, aiShooter);
			}
		}
	}

	// Token: 0x06004676 RID: 18038 RVA: 0x0016DC78 File Offset: 0x0016BE78
	public override void SetDeltaTime(float deltaTime)
	{
		base.SetDeltaTime(deltaTime);
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			if (this.AttackBehaviors[i].Behavior != null)
			{
				this.AttackBehaviors[i].Behavior.SetDeltaTime(deltaTime);
			}
		}
	}

	// Token: 0x06004677 RID: 18039 RVA: 0x0016DCD8 File Offset: 0x0016BED8
	public override bool IsReady()
	{
		if (this.ShareCooldowns)
		{
			for (int i = 0; i < this.AttackBehaviors.Count; i++)
			{
				if (this.AttackBehaviors[i].Behavior != null && !this.AttackBehaviors[i].Behavior.IsReady())
				{
					return false;
				}
			}
			return true;
		}
		for (int j = 0; j < this.AttackBehaviors.Count; j++)
		{
			if (this.AttackBehaviors[j].Behavior != null && this.AttackBehaviors[j].Behavior.IsReady())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004678 RID: 18040 RVA: 0x0016DD94 File Offset: 0x0016BF94
	public override float GetMinReadyRange()
	{
		if (!this.IsReady())
		{
			return -1f;
		}
		float num = float.MaxValue;
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			if (this.AttackBehaviors[i].Behavior != null)
			{
				num = Mathf.Min(num, this.AttackBehaviors[i].Behavior.GetMinReadyRange());
			}
		}
		return num;
	}

	// Token: 0x06004679 RID: 18041 RVA: 0x0016DE08 File Offset: 0x0016C008
	public override float GetMaxRange()
	{
		float num = float.MinValue;
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			if (this.AttackBehaviors[i].Behavior != null)
			{
				num = Mathf.Max(num, this.AttackBehaviors[i].Behavior.GetMaxRange());
			}
		}
		return num;
	}

	// Token: 0x0600467A RID: 18042 RVA: 0x0016DE6C File Offset: 0x0016C06C
	public override bool UpdateEveryFrame()
	{
		return this.m_currentBehavior != null && this.m_currentBehavior.UpdateEveryFrame();
	}

	// Token: 0x0600467B RID: 18043 RVA: 0x0016DE88 File Offset: 0x0016C088
	public override bool IsOverridable()
	{
		return (this.m_currentBehavior == null) ? base.IsOverridable() : this.m_currentBehavior.IsOverridable();
	}

	// Token: 0x0600467C RID: 18044 RVA: 0x0016DEAC File Offset: 0x0016C0AC
	public override void OnActorPreDeath()
	{
		base.OnActorPreDeath();
		for (int i = 0; i < this.AttackBehaviors.Count; i++)
		{
			if (this.AttackBehaviors[i].Behavior != null)
			{
				this.AttackBehaviors[i].Behavior.OnActorPreDeath();
			}
		}
	}

	// Token: 0x17000A56 RID: 2646
	// (get) Token: 0x0600467D RID: 18045 RVA: 0x0016DF08 File Offset: 0x0016C108
	public int Count
	{
		get
		{
			return this.AttackBehaviors.Count;
		}
	}

	// Token: 0x0600467E RID: 18046 RVA: 0x0016DF18 File Offset: 0x0016C118
	public AttackBehaviorBase GetAttackBehavior(int index)
	{
		return this.AttackBehaviors[index].Behavior;
	}

	// Token: 0x040038EC RID: 14572
	public bool ShareCooldowns;

	// Token: 0x040038ED RID: 14573
	[InspectorCollectionRotorzFlags(HideRemoveButtons = true)]
	public List<AttackBehaviorGroup.AttackGroupItem> AttackBehaviors;

	// Token: 0x040038EE RID: 14574
	private AttackBehaviorBase m_currentBehavior;

	// Token: 0x02000D0D RID: 3341
	public class AttackGroupItem
	{
		// Token: 0x040038EF RID: 14575
		[InspectorName("Nickname")]
		public string NickName;

		// Token: 0x040038F0 RID: 14576
		public float Probability = 1f;

		// Token: 0x040038F1 RID: 14577
		public AttackBehaviorBase Behavior;
	}
}
