using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200146A RID: 5226
public class PoweredByRevengeItem : PassiveItem
{
	// Token: 0x060076D5 RID: 30421 RVA: 0x002F5D80 File Offset: 0x002F3F80
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

	// Token: 0x060076D6 RID: 30422 RVA: 0x002F5E28 File Offset: 0x002F4028
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
		debrisObject.GetComponent<PoweredByRevengeItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x060076D7 RID: 30423 RVA: 0x002F5ECC File Offset: 0x002F40CC
	protected override void OnDestroy()
	{
		if (this.m_pickedUp && PassiveItem.ActiveFlagItems[this.m_owner].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[this.m_owner].Remove(base.GetType());
			}
		}
		base.OnDestroy();
	}

	// Token: 0x040078C9 RID: 30921
	public static float EXPLOSION_RADIUS_MULTIPLIER = 0.1f;
}
