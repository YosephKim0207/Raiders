using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001342 RID: 4930
public class AmazingChestAheadItem : PassiveItem
{
	// Token: 0x06006FCF RID: 28623 RVA: 0x002C521C File Offset: 0x002C341C
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (!PassiveItem.ActiveFlagItems.ContainsKey(player))
		{
			PassiveItem.ActiveFlagItems.Add(player, new Dictionary<Type, int>());
		}
		if (!PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player].Add(base.GetType(), 1);
		}
		else
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = PassiveItem.ActiveFlagItems[player][base.GetType()] + 1;
		}
		base.Pickup(player);
	}

	// Token: 0x06006FD0 RID: 28624 RVA: 0x002C52C4 File Offset: 0x002C34C4
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		if (PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[player][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[player][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[player].Remove(base.GetType());
			}
		}
		debrisObject.GetComponent<AmazingChestAheadItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x06006FD1 RID: 28625 RVA: 0x002C5368 File Offset: 0x002C3568
	protected override void OnDestroy()
	{
		if (this.m_pickedUp && PassiveItem.ActiveFlagItems.ContainsKey(this.m_owner) && PassiveItem.ActiveFlagItems[this.m_owner].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[this.m_owner].Remove(base.GetType());
			}
		}
		base.OnDestroy();
	}

	// Token: 0x04006F2D RID: 28461
	public static float ChestIncrementMultiplier = 1.5f;
}
