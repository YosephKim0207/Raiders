using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200133A RID: 4922
[Serializable]
public class AdvancedSynergyEntry
{
	// Token: 0x06006F9C RID: 28572 RVA: 0x002C39DC File Offset: 0x002C1BDC
	public bool SynergyIsActive(PlayerController p, PlayerController p2)
	{
		bool flag = this.MandatoryGunIDs.Count > 0 || (this.RequiresAtLeastOneGunAndOneItem && this.OptionalGunIDs.Count > 0);
		return !flag || this.ActiveWhenGunUnequipped || (p && p.CurrentGun && (this.MandatoryGunIDs.Contains(p.CurrentGun.PickupObjectId) || this.OptionalGunIDs.Contains(p.CurrentGun.PickupObjectId))) || (p2 && p2.CurrentGun && (this.MandatoryGunIDs.Contains(p2.CurrentGun.PickupObjectId) || this.OptionalGunIDs.Contains(p2.CurrentGun.PickupObjectId))) || (p && p.CurrentSecondaryGun && (this.MandatoryGunIDs.Contains(p.CurrentSecondaryGun.PickupObjectId) || this.OptionalGunIDs.Contains(p.CurrentSecondaryGun.PickupObjectId))) || (p2 && p2.CurrentSecondaryGun && (this.MandatoryGunIDs.Contains(p2.CurrentSecondaryGun.PickupObjectId) || this.OptionalGunIDs.Contains(p2.CurrentSecondaryGun.PickupObjectId)));
	}

	// Token: 0x06006F9D RID: 28573 RVA: 0x002C3B7C File Offset: 0x002C1D7C
	public bool ContainsPickup(int id)
	{
		return this.MandatoryGunIDs.Contains(id) || this.MandatoryItemIDs.Contains(id) || this.OptionalGunIDs.Contains(id) || this.OptionalItemIDs.Contains(id);
	}

	// Token: 0x06006F9E RID: 28574 RVA: 0x002C3BCC File Offset: 0x002C1DCC
	private bool PlayerHasSynergyCompletionItem(PlayerController p)
	{
		if (p)
		{
			for (int i = 0; i < p.passiveItems.Count; i++)
			{
				if (p.passiveItems[i] is SynergyCompletionItem)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06006F9F RID: 28575 RVA: 0x002C3C1C File Offset: 0x002C1E1C
	private bool PlayerHasPickup(PlayerController p, int pickupID)
	{
		if (p && p.inventory != null && p.inventory.AllGuns != null)
		{
			for (int i = 0; i < p.inventory.AllGuns.Count; i++)
			{
				if (p.inventory.AllGuns[i].PickupObjectId == pickupID)
				{
					return true;
				}
			}
		}
		if (p)
		{
			for (int j = 0; j < p.activeItems.Count; j++)
			{
				if (p.activeItems[j].PickupObjectId == pickupID)
				{
					return true;
				}
			}
			for (int k = 0; k < p.passiveItems.Count; k++)
			{
				if (p.passiveItems[k].PickupObjectId == pickupID)
				{
					return true;
				}
			}
			if (pickupID == GlobalItemIds.Map && p.EverHadMap)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006FA0 RID: 28576 RVA: 0x002C3D1C File Offset: 0x002C1F1C
	public bool SynergyIsAvailable(PlayerController p, PlayerController p2, int additionalID = -1)
	{
		if (this.ActivationStatus == SynergyEntry.SynergyActivation.INACTIVE)
		{
			return false;
		}
		if (this.ActivationStatus == SynergyEntry.SynergyActivation.DEMO)
		{
			return false;
		}
		bool flag = this.PlayerHasSynergyCompletionItem(p) || this.PlayerHasSynergyCompletionItem(p2);
		if (this.IgnoreLichEyeBullets)
		{
			flag = false;
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.MandatoryGunIDs.Count; i++)
		{
			if (this.PlayerHasPickup(p, this.MandatoryGunIDs[i]) || this.PlayerHasPickup(p2, this.MandatoryGunIDs[i]) || this.MandatoryGunIDs[i] == additionalID)
			{
				num++;
			}
		}
		for (int j = 0; j < this.MandatoryItemIDs.Count; j++)
		{
			if (this.PlayerHasPickup(p, this.MandatoryItemIDs[j]) || this.PlayerHasPickup(p2, this.MandatoryItemIDs[j]) || this.MandatoryItemIDs[j] == additionalID)
			{
				num2++;
			}
		}
		int num3 = 0;
		int num4 = 0;
		for (int k = 0; k < this.OptionalGunIDs.Count; k++)
		{
			if (this.PlayerHasPickup(p, this.OptionalGunIDs[k]) || this.PlayerHasPickup(p2, this.OptionalGunIDs[k]) || this.OptionalGunIDs[k] == additionalID)
			{
				num3++;
			}
		}
		for (int l = 0; l < this.OptionalItemIDs.Count; l++)
		{
			if (this.PlayerHasPickup(p, this.OptionalItemIDs[l]) || this.PlayerHasPickup(p2, this.OptionalItemIDs[l]) || this.OptionalItemIDs[l] == additionalID)
			{
				num4++;
			}
		}
		bool flag2 = this.MandatoryItemIDs.Count > 0 && this.MandatoryGunIDs.Count == 0 && this.OptionalGunIDs.Count > 0 && this.OptionalItemIDs.Count == 0;
		if (((this.MandatoryGunIDs.Count > 0 && num > 0) || (flag2 && num3 > 0)) && flag)
		{
			num++;
			num2++;
		}
		if (num < this.MandatoryGunIDs.Count || num2 < this.MandatoryItemIDs.Count)
		{
			return false;
		}
		int num5 = this.MandatoryItemIDs.Count + this.MandatoryGunIDs.Count + num3 + num4;
		int num6 = this.MandatoryGunIDs.Count + num3;
		int num7 = this.MandatoryItemIDs.Count + num4;
		if (num6 > 0 && (this.MandatoryGunIDs.Count > 0 || flag2 || (this.RequiresAtLeastOneGunAndOneItem && num6 > 0)) && flag)
		{
			num7++;
			num6++;
			num5 += 2;
		}
		if (this.RequiresAtLeastOneGunAndOneItem && this.OptionalGunIDs.Count + this.MandatoryGunIDs.Count > 0 && this.OptionalItemIDs.Count + this.MandatoryItemIDs.Count > 0 && (num6 < 1 || num7 < 1))
		{
			return false;
		}
		int num8 = Mathf.Max(2, this.NumberObjectsRequired);
		return num5 >= num8;
	}

	// Token: 0x04006EDC RID: 28380
	public string NameKey;

	// Token: 0x04006EDD RID: 28381
	public SynergyEntry.SynergyActivation ActivationStatus;

	// Token: 0x04006EDE RID: 28382
	[PickupIdentifier]
	public List<int> MandatoryGunIDs = new List<int>();

	// Token: 0x04006EDF RID: 28383
	[PickupIdentifier]
	public List<int> MandatoryItemIDs = new List<int>();

	// Token: 0x04006EE0 RID: 28384
	[PickupIdentifier]
	public List<int> OptionalGunIDs = new List<int>();

	// Token: 0x04006EE1 RID: 28385
	[PickupIdentifier]
	public List<int> OptionalItemIDs = new List<int>();

	// Token: 0x04006EE2 RID: 28386
	public int NumberObjectsRequired = 2;

	// Token: 0x04006EE3 RID: 28387
	public bool ActiveWhenGunUnequipped;

	// Token: 0x04006EE4 RID: 28388
	public bool SuppressVFX;

	// Token: 0x04006EE5 RID: 28389
	public bool RequiresAtLeastOneGunAndOneItem;

	// Token: 0x04006EE6 RID: 28390
	public bool IgnoreLichEyeBullets;

	// Token: 0x04006EE7 RID: 28391
	public List<StatModifier> statModifiers;

	// Token: 0x04006EE8 RID: 28392
	[LongNumericEnum]
	public List<CustomSynergyType> bonusSynergies;
}
