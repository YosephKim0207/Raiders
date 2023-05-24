using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200143A RID: 5178
public class MimicRingItem : PassiveItem
{
	// Token: 0x0600758A RID: 30090 RVA: 0x002ECFB0 File Offset: 0x002EB1B0
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
		for (int i = 0; i < StaticReferenceManager.AllChests.Count; i++)
		{
			if (StaticReferenceManager.AllChests[i] && !StaticReferenceManager.AllChests[i].IsOpen)
			{
				StaticReferenceManager.AllChests[i].MaybeBecomeMimic();
			}
		}
		base.Pickup(player);
	}

	// Token: 0x0600758B RID: 30091 RVA: 0x002ED0AC File Offset: 0x002EB2AC
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
		debrisObject.GetComponent<MimicRingItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x0600758C RID: 30092 RVA: 0x002ED150 File Offset: 0x002EB350
	protected override void OnDestroy()
	{
		BraveTime.ClearMultiplier(base.gameObject);
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
}
