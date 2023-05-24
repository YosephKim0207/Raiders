using System;
using System.Collections.Generic;

// Token: 0x020014C7 RID: 5319
[Serializable]
public class SynergyEntry
{
	// Token: 0x060078E2 RID: 30946 RVA: 0x003057B0 File Offset: 0x003039B0
	public bool SynergyIsActive(PlayerController p, PlayerController p2)
	{
		return this.gunIDs.Count <= 0 || this.ActiveWhenGunUnequipped || (p && p.CurrentGun && this.gunIDs.Contains(p.CurrentGun.PickupObjectId)) || (p2 && p2.CurrentGun && this.gunIDs.Contains(p2.CurrentGun.PickupObjectId)) || (p && p.CurrentSecondaryGun && this.gunIDs.Contains(p.CurrentSecondaryGun.PickupObjectId)) || (p2 && p2.CurrentSecondaryGun && this.gunIDs.Contains(p.CurrentSecondaryGun.PickupObjectId));
	}

	// Token: 0x060078E3 RID: 30947 RVA: 0x003058BC File Offset: 0x00303ABC
	public bool SynergyIsAvailable(PlayerController p, PlayerController p2)
	{
		if (this.ActivationStatus == SynergyEntry.SynergyActivation.INACTIVE)
		{
			return false;
		}
		if (this.ActivationStatus == SynergyEntry.SynergyActivation.DEMO)
		{
			return false;
		}
		bool flag = true;
		bool flag2 = true;
		if (this.gunIDs.Count > 0)
		{
			if (this.GunsOR)
			{
				flag = false;
			}
			for (int i = 0; i < this.gunIDs.Count; i++)
			{
				bool flag3 = false;
				if (p && p.inventory != null && p.inventory.AllGuns != null)
				{
					for (int j = 0; j < p.inventory.AllGuns.Count; j++)
					{
						if (p.inventory.AllGuns[j].PickupObjectId == this.gunIDs[i])
						{
							flag3 = true;
							break;
						}
					}
				}
				if (p2 && p2.inventory != null && p2.inventory.AllGuns != null)
				{
					for (int k = 0; k < p2.inventory.AllGuns.Count; k++)
					{
						if (p2.inventory.AllGuns[k].PickupObjectId == this.gunIDs[i])
						{
							flag3 = true;
							break;
						}
					}
				}
				if (flag3 && this.GunsOR)
				{
					flag = true;
					break;
				}
				if (!flag3 && !this.GunsOR)
				{
					flag = false;
					break;
				}
			}
		}
		if (!flag)
		{
			return false;
		}
		if (this.itemIDs.Count > 0)
		{
			if (this.ItemsOR)
			{
				flag2 = false;
			}
			int num = 0;
			for (int l = 0; l < this.itemIDs.Count; l++)
			{
				bool flag4 = false;
				if (p)
				{
					for (int m = 0; m < p.activeItems.Count; m++)
					{
						if (p.activeItems[m].PickupObjectId == this.itemIDs[l])
						{
							flag4 = true;
							break;
						}
					}
					for (int n = 0; n < p.passiveItems.Count; n++)
					{
						if (p.passiveItems[n].PickupObjectId == this.itemIDs[l])
						{
							flag4 = true;
							break;
						}
					}
					if (this.itemIDs[l] == GlobalItemIds.Map && p.EverHadMap)
					{
						break;
					}
				}
				if (p2)
				{
					for (int num2 = 0; num2 < p2.activeItems.Count; num2++)
					{
						if (p2.activeItems[num2].PickupObjectId == this.itemIDs[l])
						{
							flag4 = true;
							break;
						}
					}
					for (int num3 = 0; num3 < p2.passiveItems.Count; num3++)
					{
						if (p2.passiveItems[num3].PickupObjectId == this.itemIDs[l])
						{
							flag4 = true;
							break;
						}
					}
					if (this.itemIDs[l] == GlobalItemIds.Map && p2.EverHadMap)
					{
						break;
					}
				}
				if (flag4 && this.ItemsOR)
				{
					num++;
				}
				if (!flag4 && !this.ItemsOR)
				{
					flag2 = false;
					break;
				}
			}
			if (this.ItemsOR && num > this.ExtraItemsOrForBrents)
			{
				flag2 = true;
			}
		}
		return flag && flag2;
	}

	// Token: 0x04007B27 RID: 31527
	public string NameKey;

	// Token: 0x04007B28 RID: 31528
	public SynergyEntry.SynergyActivation ActivationStatus;

	// Token: 0x04007B29 RID: 31529
	[PickupIdentifier]
	public List<int> gunIDs;

	// Token: 0x04007B2A RID: 31530
	[PickupIdentifier]
	public List<int> itemIDs;

	// Token: 0x04007B2B RID: 31531
	public bool GunsOR;

	// Token: 0x04007B2C RID: 31532
	public bool ItemsOR;

	// Token: 0x04007B2D RID: 31533
	public bool ActiveWhenGunUnequipped;

	// Token: 0x04007B2E RID: 31534
	public bool SuppressVFX;

	// Token: 0x04007B2F RID: 31535
	public int ExtraItemsOrForBrents;

	// Token: 0x04007B30 RID: 31536
	public List<StatModifier> statModifiers;

	// Token: 0x04007B31 RID: 31537
	[LongNumericEnum]
	public List<CustomSynergyType> bonusSynergies;

	// Token: 0x020014C8 RID: 5320
	public enum SynergyActivation
	{
		// Token: 0x04007B33 RID: 31539
		ACTIVE,
		// Token: 0x04007B34 RID: 31540
		DEMO,
		// Token: 0x04007B35 RID: 31541
		INACTIVE,
		// Token: 0x04007B36 RID: 31542
		ACTIVE_UNBOOSTED
	}
}
