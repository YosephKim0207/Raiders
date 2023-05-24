using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200141B RID: 5147
public class HelmetItem : BasicStatPickup
{
	// Token: 0x060074D2 RID: 29906 RVA: 0x002E8574 File Offset: 0x002E6774
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

	// Token: 0x060074D3 RID: 29907 RVA: 0x002E861C File Offset: 0x002E681C
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		if (PassiveItem.ActiveFlagItems.ContainsKey(player) && PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[player][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[player][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[player].Remove(base.GetType());
			}
		}
		debrisObject.GetComponent<HelmetItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x060074D4 RID: 29908 RVA: 0x002E86D0 File Offset: 0x002E68D0
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

	// Token: 0x040076A5 RID: 30373
	public static float EXPLOSION_RADIUS_MULTIPLIER = 0.1f;
}
