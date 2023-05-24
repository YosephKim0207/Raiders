using System;

// Token: 0x02000D75 RID: 3445
public interface IAttackBehaviorGroup
{
	// Token: 0x17000A7B RID: 2683
	// (get) Token: 0x060048C8 RID: 18632
	int Count { get; }

	// Token: 0x060048C9 RID: 18633
	AttackBehaviorBase GetAttackBehavior(int index);
}
