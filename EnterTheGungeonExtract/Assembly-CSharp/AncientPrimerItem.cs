using System;

// Token: 0x02001345 RID: 4933
public class AncientPrimerItem : PassiveItem
{
	// Token: 0x06006FE1 RID: 28641 RVA: 0x002C5C30 File Offset: 0x002C3E30
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
	}

	// Token: 0x06006FE2 RID: 28642 RVA: 0x002C5C48 File Offset: 0x002C3E48
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<AncientPrimerItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x06006FE3 RID: 28643 RVA: 0x002C5C6C File Offset: 0x002C3E6C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
