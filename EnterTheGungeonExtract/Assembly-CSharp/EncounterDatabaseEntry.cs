using System;
using FullInspector;
using UnityEngine;

// Token: 0x020014F1 RID: 5361
[Serializable]
public class EncounterDatabaseEntry : AssetBundleDatabaseEntry
{
	// Token: 0x06007A0A RID: 31242 RVA: 0x0030F2BC File Offset: 0x0030D4BC
	public EncounterDatabaseEntry()
	{
	}

	// Token: 0x06007A0B RID: 31243 RVA: 0x0030F2D4 File Offset: 0x0030D4D4
	public EncounterDatabaseEntry(EncounterTrackable encounterTrackable)
	{
		this.myGuid = encounterTrackable.EncounterGuid;
		this.SetAll(encounterTrackable);
	}

	// Token: 0x170011FD RID: 4605
	// (get) Token: 0x06007A0C RID: 31244 RVA: 0x0030F300 File Offset: 0x0030D500
	// (set) Token: 0x06007A0D RID: 31245 RVA: 0x0030F308 File Offset: 0x0030D508
	[InspectorDisabled]
	public bool ForceEncounterState { get; set; }

	// Token: 0x170011FE RID: 4606
	// (get) Token: 0x06007A0E RID: 31246 RVA: 0x0030F314 File Offset: 0x0030D514
	public override AssetBundle assetBundle
	{
		get
		{
			return EncounterDatabase.AssetBundle;
		}
	}

	// Token: 0x06007A0F RID: 31247 RVA: 0x0030F31C File Offset: 0x0030D51C
	public bool PrerequisitesMet()
	{
		if (this.prerequisites == null || this.prerequisites.Length == 0)
		{
			return true;
		}
		if (GameStatsManager.Instance.IsForceUnlocked(this.myGuid))
		{
			return true;
		}
		for (int i = 0; i < this.prerequisites.Length; i++)
		{
			if (!this.prerequisites[i].CheckConditionsFulfilled())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06007A10 RID: 31248 RVA: 0x0030F388 File Offset: 0x0030D588
	public string GetSecondTapeDescriptor()
	{
		string text = string.Empty;
		if (this.shootStyleInt >= 0)
		{
			text = this.GetShootStyleString((ProjectileModule.ShootStyle)this.shootStyleInt);
		}
		else if (this.isPlayerItem)
		{
			text = StringTableManager.GetItemsString("#ITEMSTYLE_ACTIVE", -1);
		}
		else if (this.isPassiveItem)
		{
			text = StringTableManager.GetItemsString("#ITEMSTYLE_PASSIVE", -1);
		}
		return text;
	}

	// Token: 0x06007A11 RID: 31249 RVA: 0x0030F3F0 File Offset: 0x0030D5F0
	public string GetModifiedLongDescription()
	{
		return this.journalData.GetAmmonomiconFullEntry(this.isInfiniteAmmoGun, this.doesntDamageSecretWalls);
	}

	// Token: 0x06007A12 RID: 31250 RVA: 0x0030F418 File Offset: 0x0030D618
	private string GetShootStyleString(ProjectileModule.ShootStyle shootStyle)
	{
		switch (shootStyle)
		{
		case ProjectileModule.ShootStyle.SemiAutomatic:
			return StringTableManager.GetItemsString("#SHOOTSTYLE_SEMIAUTOMATIC", -1);
		case ProjectileModule.ShootStyle.Automatic:
			return StringTableManager.GetItemsString("#SHOOTSTYLE_AUTOMATIC", -1);
		case ProjectileModule.ShootStyle.Beam:
			return StringTableManager.GetItemsString("#SHOOTSTYLE_BEAM", -1);
		case ProjectileModule.ShootStyle.Charged:
			return StringTableManager.GetItemsString("#SHOOTSTYLE_CHARGE", -1);
		case ProjectileModule.ShootStyle.Burst:
			return StringTableManager.GetItemsString("#SHOOTSTYLE_BURST", -1);
		default:
			return string.Empty;
		}
	}

	// Token: 0x06007A13 RID: 31251 RVA: 0x0030F488 File Offset: 0x0030D688
	public void SetAll(EncounterTrackable encounterTrackable)
	{
		this.ProxyEncounterGuid = encounterTrackable.ProxyEncounterGuid;
		this.doNotificationOnEncounter = encounterTrackable.DoNotificationOnEncounter;
		this.IgnoreDifferentiator = encounterTrackable.IgnoreDifferentiator;
		this.prerequisites = encounterTrackable.prerequisites;
		this.journalData = encounterTrackable.journalData.Clone();
		this.usesPurpleNotifications = encounterTrackable.UsesPurpleNotifications;
		PickupObject component = encounterTrackable.GetComponent<PickupObject>();
		this.pickupObjectId = ((!component) ? (-1) : component.PickupObjectId);
		Gun gun = component as Gun;
		this.shootStyleInt = (int)((!gun) ? ((ProjectileModule.ShootStyle)(-1)) : gun.DefaultModule.shootStyle);
		this.isPlayerItem = encounterTrackable.GetComponent<PlayerItem>();
		this.isPassiveItem = encounterTrackable.GetComponent<PassiveItem>();
		this.isInfiniteAmmoGun = gun && gun.InfiniteAmmo;
		this.doesntDamageSecretWalls = gun && gun.InfiniteAmmo;
	}

	// Token: 0x06007A14 RID: 31252 RVA: 0x0030F58C File Offset: 0x0030D78C
	public bool Equals(EncounterTrackable other)
	{
		if (other == null)
		{
			return false;
		}
		if (this.ProxyEncounterGuid != other.ProxyEncounterGuid || this.doNotificationOnEncounter != other.DoNotificationOnEncounter || this.IgnoreDifferentiator != other.IgnoreDifferentiator || !EncounterDatabaseEntry.PrereqArraysEqual(this.prerequisites, other.prerequisites) || !this.journalData.Equals(other.journalData) || this.usesPurpleNotifications != other.UsesPurpleNotifications)
		{
			return false;
		}
		PickupObject component = other.GetComponent<PickupObject>();
		int num = ((!component) ? (-1) : component.PickupObjectId);
		if (this.pickupObjectId != num)
		{
			return false;
		}
		Gun gun = component as Gun;
		int num2 = (int)((!gun) ? ((ProjectileModule.ShootStyle)(-1)) : gun.DefaultModule.shootStyle);
		return this.shootStyleInt == num2 && this.isPlayerItem == other.GetComponent<PlayerItem>() && this.isPassiveItem == other.GetComponent<PassiveItem>() && this.isInfiniteAmmoGun == (gun && gun.InfiniteAmmo) && this.doesntDamageSecretWalls == (gun && gun.InfiniteAmmo);
	}

	// Token: 0x06007A15 RID: 31253 RVA: 0x0030F6F8 File Offset: 0x0030D8F8
	private static bool PrereqArraysEqual(DungeonPrerequisite[] a, DungeonPrerequisite[] b)
	{
		if (a == null && b != null)
		{
			return false;
		}
		if (b == null && a != null)
		{
			return false;
		}
		if (a.Length != b.Length)
		{
			return false;
		}
		for (int i = 0; i < a.Length; i++)
		{
			if (!a[i].Equals(b[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04007C81 RID: 31873
	[InspectorDisabled]
	public string ProxyEncounterGuid;

	// Token: 0x04007C82 RID: 31874
	[InspectorDisabled]
	public bool doNotificationOnEncounter;

	// Token: 0x04007C83 RID: 31875
	[InspectorDisabled]
	public bool IgnoreDifferentiator;

	// Token: 0x04007C84 RID: 31876
	[InspectorDisabled]
	public DungeonPrerequisite[] prerequisites;

	// Token: 0x04007C85 RID: 31877
	[InspectorDisabled]
	public JournalEntry journalData;

	// Token: 0x04007C86 RID: 31878
	[InspectorDisabled]
	public bool usesPurpleNotifications;

	// Token: 0x04007C87 RID: 31879
	[InspectorDisabled]
	public int pickupObjectId = -1;

	// Token: 0x04007C88 RID: 31880
	[InspectorDisabled]
	public int shootStyleInt = -1;

	// Token: 0x04007C89 RID: 31881
	[InspectorDisabled]
	public bool isPlayerItem;

	// Token: 0x04007C8A RID: 31882
	[InspectorDisabled]
	public bool isPassiveItem;

	// Token: 0x04007C8B RID: 31883
	[InspectorDisabled]
	public bool isInfiniteAmmoGun;

	// Token: 0x04007C8C RID: 31884
	[InspectorDisabled]
	public bool doesntDamageSecretWalls;
}
