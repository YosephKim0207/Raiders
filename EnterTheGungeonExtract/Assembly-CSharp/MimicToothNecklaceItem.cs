using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200143B RID: 5179
public class MimicToothNecklaceItem : PassiveItem
{
	// Token: 0x0600758E RID: 30094 RVA: 0x002ED230 File Offset: 0x002EB430
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
			Chest chest = StaticReferenceManager.AllChests[i];
			if (chest && !chest.IsOpen && !chest.IsBroken)
			{
				chest.MaybeBecomeMimic();
			}
		}
		base.Pickup(player);
	}

	// Token: 0x0600758F RID: 30095 RVA: 0x002ED324 File Offset: 0x002EB524
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
		debrisObject.GetComponent<MimicToothNecklaceItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x06007590 RID: 30096 RVA: 0x002ED3D8 File Offset: 0x002EB5D8
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
