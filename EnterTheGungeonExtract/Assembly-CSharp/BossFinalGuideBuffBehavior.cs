using System;
using FullInspector;

// Token: 0x02000D8B RID: 3467
[InspectorDropdownName("Bosses/BossFinalGuide/BuffBehavior")]
public class BossFinalGuideBuffBehavior : BuffEnemiesBehavior
{
	// Token: 0x0600496B RID: 18795 RVA: 0x0018826C File Offset: 0x0018646C
	protected override void BuffEnemy(AIActor enemy)
	{
		if (this.m_behavior == null)
		{
			this.m_behavior = this.FindBehavior(enemy);
		}
		if (this.m_behavior != null)
		{
			this.m_cachedProb = this.m_behavior.Probability;
			this.m_cachedCooldown = (this.m_behavior.Behavior as BasicAttackBehavior).Cooldown;
			this.m_behavior.Probability = this.behaviorProb;
			(this.m_behavior.Behavior as BasicAttackBehavior).Cooldown = this.behaviorCooldown;
		}
		base.BuffEnemy(enemy);
	}

	// Token: 0x0600496C RID: 18796 RVA: 0x001882FC File Offset: 0x001864FC
	protected override void UnbuffEnemy(AIActor enemy)
	{
		if (this.m_behavior != null)
		{
			this.m_behavior.Probability = this.m_cachedProb;
			(this.m_behavior.Behavior as BasicAttackBehavior).Cooldown = this.m_cachedCooldown;
		}
		base.UnbuffEnemy(enemy);
	}

	// Token: 0x0600496D RID: 18797 RVA: 0x0018833C File Offset: 0x0018653C
	private AttackBehaviorGroup.AttackGroupItem FindBehavior(AIActor enemy)
	{
		AttackBehaviorGroup attackBehaviorGroup = enemy.behaviorSpeculator.AttackBehaviors.Find((AttackBehaviorBase b) => b is AttackBehaviorGroup) as AttackBehaviorGroup;
		if (attackBehaviorGroup == null)
		{
			return null;
		}
		return attackBehaviorGroup.AttackBehaviors.Find((AttackBehaviorGroup.AttackGroupItem i) => i.NickName.Equals(this.behaviorName, StringComparison.OrdinalIgnoreCase));
	}

	// Token: 0x04003DBC RID: 15804
	public string behaviorName;

	// Token: 0x04003DBD RID: 15805
	public float behaviorProb;

	// Token: 0x04003DBE RID: 15806
	public float behaviorCooldown;

	// Token: 0x04003DBF RID: 15807
	private AttackBehaviorGroup.AttackGroupItem m_behavior;

	// Token: 0x04003DC0 RID: 15808
	private float m_cachedProb;

	// Token: 0x04003DC1 RID: 15809
	private float m_cachedCooldown;
}
