using System;

// Token: 0x02001346 RID: 4934
public class ArcaneGunpowderItem : PlayerItem
{
	// Token: 0x06006FE5 RID: 28645 RVA: 0x002C5C7C File Offset: 0x002C3E7C
	public override bool CanBeUsed(PlayerController user)
	{
		return false;
	}

	// Token: 0x06006FE6 RID: 28646 RVA: 0x002C5C80 File Offset: 0x002C3E80
	protected override void DoEffect(PlayerController user)
	{
	}

	// Token: 0x06006FE7 RID: 28647 RVA: 0x002C5C84 File Offset: 0x002C3E84
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
