using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001494 RID: 5268
public class RingOfResourcefulRatItem : PassiveItem, ILevelLoadedListener
{
	// Token: 0x060077DB RID: 30683 RVA: 0x002FD930 File Offset: 0x002FBB30
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
		if (!this.m_initializedEver)
		{
			if (!PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
			{
				PassiveItem.ActiveFlagItems[player].Add(base.GetType(), 1);
			}
			else
			{
				PassiveItem.ActiveFlagItems[player][base.GetType()] = PassiveItem.ActiveFlagItems[player][base.GetType()] + 1;
			}
			this.m_initializedEver = true;
		}
		base.Pickup(player);
	}

	// Token: 0x060077DC RID: 30684 RVA: 0x002FD9E8 File Offset: 0x002FBBE8
	protected override void Update()
	{
		base.Update();
	}

	// Token: 0x060077DD RID: 30685 RVA: 0x002FD9F0 File Offset: 0x002FBBF0
	public void BraveOnLevelWasLoaded()
	{
		if (this.m_owner)
		{
			if (!PassiveItem.ActiveFlagItems[this.m_owner].ContainsKey(base.GetType()))
			{
				PassiveItem.ActiveFlagItems[this.m_owner].Add(base.GetType(), 1);
			}
			else
			{
				PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] = PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] + 1;
			}
		}
	}

	// Token: 0x060077DE RID: 30686 RVA: 0x002FDA88 File Offset: 0x002FBC88
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
		debrisObject.GetComponent<RingOfResourcefulRatItem>().m_pickedUpThisRun = true;
		debrisObject.GetComponent<RingOfResourcefulRatItem>().m_initializedEver = true;
		return debrisObject;
	}

	// Token: 0x060077DF RID: 30687 RVA: 0x002FDB48 File Offset: 0x002FBD48
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

	// Token: 0x04007A04 RID: 31236
	private bool m_initializedEver;
}
