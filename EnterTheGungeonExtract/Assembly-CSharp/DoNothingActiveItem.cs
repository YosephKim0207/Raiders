using System;

// Token: 0x020013EB RID: 5099
public class DoNothingActiveItem : PlayerItem
{
	// Token: 0x060073AE RID: 29614 RVA: 0x002E0434 File Offset: 0x002DE634
	public override bool CanBeUsed(PlayerController user)
	{
		return base.CanBeUsed(user);
	}

	// Token: 0x060073AF RID: 29615 RVA: 0x002E0440 File Offset: 0x002DE640
	protected override void DoEffect(PlayerController user)
	{
	}

	// Token: 0x060073B0 RID: 29616 RVA: 0x002E0444 File Offset: 0x002DE644
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
