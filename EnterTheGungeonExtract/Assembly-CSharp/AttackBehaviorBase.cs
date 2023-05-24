using System;

// Token: 0x02000D74 RID: 3444
public abstract class AttackBehaviorBase : BehaviorBase
{
	// Token: 0x060048C5 RID: 18629
	public abstract bool IsReady();

	// Token: 0x060048C6 RID: 18630
	public abstract float GetMinReadyRange();

	// Token: 0x060048C7 RID: 18631
	public abstract float GetMaxRange();
}
