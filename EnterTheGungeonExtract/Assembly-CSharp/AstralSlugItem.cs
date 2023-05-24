using System;

// Token: 0x02001349 RID: 4937
public class AstralSlugItem : PassiveItem
{
	// Token: 0x06006FF5 RID: 28661 RVA: 0x002C5F9C File Offset: 0x002C419C
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
	}

	// Token: 0x06006FF6 RID: 28662 RVA: 0x002C5FB4 File Offset: 0x002C41B4
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<AstralSlugItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x06006FF7 RID: 28663 RVA: 0x002C5FD8 File Offset: 0x002C41D8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
