using System;
using System.Collections.Generic;

// Token: 0x0200138D RID: 5005
public class CoopPassiveItem : PassiveItem
{
	// Token: 0x06007171 RID: 29041 RVA: 0x002D12CC File Offset: 0x002CF4CC
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
	}

	// Token: 0x06007172 RID: 29042 RVA: 0x002D12E4 File Offset: 0x002CF4E4
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<CoopPassiveItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x06007173 RID: 29043 RVA: 0x002D1308 File Offset: 0x002CF508
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007134 RID: 28980
	public List<StatModifier> modifiers;
}
