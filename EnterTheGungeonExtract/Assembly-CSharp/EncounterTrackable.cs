using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020014F2 RID: 5362
public class EncounterTrackable : MonoBehaviour
{
	// Token: 0x170011FF RID: 4607
	// (get) Token: 0x06007A17 RID: 31255 RVA: 0x0030F768 File Offset: 0x0030D968
	// (set) Token: 0x06007A18 RID: 31256 RVA: 0x0030F770 File Offset: 0x0030D970
	public static bool SuppressNextNotification { get; set; }

	// Token: 0x06007A19 RID: 31257 RVA: 0x0030F778 File Offset: 0x0030D978
	private void GetProxy()
	{
		if (!string.IsNullOrEmpty(this.ProxyEncounterGuid))
		{
			this.m_proxyEncounterTrackable = EncounterDatabase.GetEntry(this.ProxyEncounterGuid);
		}
		this.m_hasCheckedForProxy = true;
	}

	// Token: 0x17001200 RID: 4608
	// (get) Token: 0x06007A1A RID: 31258 RVA: 0x0030F7A4 File Offset: 0x0030D9A4
	// (set) Token: 0x06007A1B RID: 31259 RVA: 0x0030F7D0 File Offset: 0x0030D9D0
	public string EncounterGuid
	{
		get
		{
			if (!this.m_hasCheckedForProxy)
			{
				this.GetProxy();
			}
			if (this.m_proxyEncounterTrackable != null)
			{
				return this.ProxyEncounterGuid;
			}
			return this.m_encounterGuid;
		}
		set
		{
			if (!this.m_hasCheckedForProxy)
			{
				this.GetProxy();
			}
			if (this.m_proxyEncounterTrackable != null)
			{
				throw new Exception("Trying to change an EncounterTrackable via a proxy!");
			}
			this.m_encounterGuid = value;
		}
	}

	// Token: 0x17001201 RID: 4609
	// (get) Token: 0x06007A1C RID: 31260 RVA: 0x0030F800 File Offset: 0x0030DA00
	// (set) Token: 0x06007A1D RID: 31261 RVA: 0x0030F808 File Offset: 0x0030DA08
	public string TrueEncounterGuid
	{
		get
		{
			return this.m_encounterGuid;
		}
		set
		{
			this.m_encounterGuid = value;
		}
	}

	// Token: 0x17001202 RID: 4610
	// (get) Token: 0x06007A1E RID: 31262 RVA: 0x0030F814 File Offset: 0x0030DA14
	// (set) Token: 0x06007A1F RID: 31263 RVA: 0x0030F844 File Offset: 0x0030DA44
	public bool DoNotificationOnEncounter
	{
		get
		{
			if (!this.m_hasCheckedForProxy)
			{
				this.GetProxy();
			}
			if (this.m_proxyEncounterTrackable != null)
			{
				return this.m_proxyEncounterTrackable.doNotificationOnEncounter;
			}
			return this.m_doNotificationOnEncounter;
		}
		set
		{
			if (!this.m_hasCheckedForProxy)
			{
				this.GetProxy();
			}
			if (this.m_proxyEncounterTrackable != null)
			{
				throw new Exception("Trying to change an EncounterTrackable via a proxy!");
			}
			this.m_doNotificationOnEncounter = value;
		}
	}

	// Token: 0x17001203 RID: 4611
	// (get) Token: 0x06007A20 RID: 31264 RVA: 0x0030F874 File Offset: 0x0030DA74
	// (set) Token: 0x06007A21 RID: 31265 RVA: 0x0030F8A4 File Offset: 0x0030DAA4
	public JournalEntry journalData
	{
		get
		{
			if (!this.m_hasCheckedForProxy)
			{
				this.GetProxy();
			}
			if (this.m_proxyEncounterTrackable != null)
			{
				return this.m_proxyEncounterTrackable.journalData;
			}
			return this.m_journalData;
		}
		set
		{
			if (!this.m_hasCheckedForProxy)
			{
				this.GetProxy();
			}
			if (this.m_proxyEncounterTrackable != null)
			{
				throw new Exception("Trying to change an EncounterTrackable via a proxy!");
			}
			this.m_journalData = value;
		}
	}

	// Token: 0x17001204 RID: 4612
	// (get) Token: 0x06007A22 RID: 31266 RVA: 0x0030F8D4 File Offset: 0x0030DAD4
	// (set) Token: 0x06007A23 RID: 31267 RVA: 0x0030F904 File Offset: 0x0030DB04
	public bool IgnoreDifferentiator
	{
		get
		{
			if (!this.m_hasCheckedForProxy)
			{
				this.GetProxy();
			}
			if (this.m_proxyEncounterTrackable != null)
			{
				return this.m_proxyEncounterTrackable.IgnoreDifferentiator;
			}
			return this.m_ignoreDifferentiator;
		}
		set
		{
			if (!this.m_hasCheckedForProxy)
			{
				this.GetProxy();
			}
			if (this.m_proxyEncounterTrackable != null)
			{
				throw new Exception("Trying to change an EncounterTrackable via a proxy!");
			}
			this.m_ignoreDifferentiator = value;
		}
	}

	// Token: 0x17001205 RID: 4613
	// (get) Token: 0x06007A24 RID: 31268 RVA: 0x0030F934 File Offset: 0x0030DB34
	// (set) Token: 0x06007A25 RID: 31269 RVA: 0x0030F964 File Offset: 0x0030DB64
	public DungeonPrerequisite[] prerequisites
	{
		get
		{
			if (!this.m_hasCheckedForProxy)
			{
				this.GetProxy();
			}
			if (this.m_proxyEncounterTrackable != null)
			{
				return this.m_proxyEncounterTrackable.prerequisites;
			}
			return this.m_prerequisites;
		}
		set
		{
			if (!this.m_hasCheckedForProxy)
			{
				this.GetProxy();
			}
			if (this.m_proxyEncounterTrackable != null)
			{
				throw new Exception("Trying to change an EncounterTrackable via a proxy!");
			}
			this.m_prerequisites = value;
		}
	}

	// Token: 0x17001206 RID: 4614
	// (get) Token: 0x06007A26 RID: 31270 RVA: 0x0030F994 File Offset: 0x0030DB94
	// (set) Token: 0x06007A27 RID: 31271 RVA: 0x0030F9C4 File Offset: 0x0030DBC4
	public bool UsesPurpleNotifications
	{
		get
		{
			if (!this.m_hasCheckedForProxy)
			{
				this.GetProxy();
			}
			if (this.m_proxyEncounterTrackable != null)
			{
				return this.m_proxyEncounterTrackable.usesPurpleNotifications;
			}
			return this.m_usesPurpleNotifications;
		}
		set
		{
			if (!this.m_hasCheckedForProxy)
			{
				this.GetProxy();
			}
			if (this.m_proxyEncounterTrackable != null)
			{
				throw new Exception("Trying to change an EncounterTrackable via a proxy!");
			}
			this.m_usesPurpleNotifications = value;
		}
	}

	// Token: 0x06007A28 RID: 31272 RVA: 0x0030F9F4 File Offset: 0x0030DBF4
	public void Awake()
	{
		this.m_pickup = base.GetComponent<PickupObject>();
		this.m_hasCheckedForPickup = true;
	}

	// Token: 0x06007A29 RID: 31273 RVA: 0x0030FA0C File Offset: 0x0030DC0C
	public bool PrerequisitesMet()
	{
		if (this.m_prerequisites == null || this.m_prerequisites.Length == 0)
		{
			return true;
		}
		if (GameStatsManager.Instance.IsForceUnlocked(this.EncounterGuid))
		{
			return true;
		}
		for (int i = 0; i < this.m_prerequisites.Length; i++)
		{
			if (!this.m_prerequisites[i].CheckConditionsFulfilled())
			{
				return false;
			}
		}
		if (!this.m_hasCheckedForPickup)
		{
			this.m_pickup = base.GetComponent<PickupObject>();
			this.m_hasCheckedForPickup = true;
		}
		return !this.m_pickup || this.m_pickup.quality != PickupObject.ItemQuality.EXCLUDED;
	}

	// Token: 0x06007A2A RID: 31274 RVA: 0x0030FABC File Offset: 0x0030DCBC
	public void HandleEncounter()
	{
		GameStatsManager.Instance.HandleEncounteredObject(this);
		if (this.m_doNotificationOnEncounter && !EncounterTrackable.SuppressNextNotification && !GameUIRoot.Instance.BossHealthBarVisible)
		{
			GameUIRoot.Instance.DoNotification(this);
		}
		EncounterTrackable.SuppressNextNotification = false;
	}

	// Token: 0x06007A2B RID: 31275 RVA: 0x0030FB0C File Offset: 0x0030DD0C
	public void ForceDoNotification(tk2dBaseSprite overrideSprite = null)
	{
		tk2dBaseSprite tk2dBaseSprite = ((!(overrideSprite == null)) ? overrideSprite : base.GetComponent<tk2dBaseSprite>());
		GameUIRoot.Instance.notificationController.DoCustomNotification(this.m_journalData.GetPrimaryDisplayName(false), this.m_journalData.GetNotificationPanelDescription(), tk2dBaseSprite.Collection, tk2dBaseSprite.spriteId, UINotificationController.NotificationColor.SILVER, false, false);
	}

	// Token: 0x06007A2C RID: 31276 RVA: 0x0030FB68 File Offset: 0x0030DD68
	public void HandleEncounter_GeneratedObjects()
	{
		GameStatsManager.Instance.HandleEncounteredObject(this);
	}

	// Token: 0x06007A2D RID: 31277 RVA: 0x0030FB78 File Offset: 0x0030DD78
	public string GetModifiedDisplayName()
	{
		if (!this.m_hasCheckedForPickup)
		{
			this.m_pickup = base.GetComponent<PickupObject>();
			this.m_hasCheckedForPickup = true;
		}
		string text = this.m_journalData.GetPrimaryDisplayName(false);
		if (this.m_pickup != null)
		{
			if (this.m_pickup.GetComponent<CursedItemModifier>() != null)
			{
				text = StringTableManager.GetItemsString("#CURSED_NAMEMOD", -1) + " " + text;
			}
			if (this.m_pickup is Gun)
			{
				bool isMinusOneGun = (this.m_pickup as Gun).IsMinusOneGun;
				if (isMinusOneGun)
				{
					text += " -1";
				}
				GunderfuryController component = this.m_pickup.GetComponent<GunderfuryController>();
				if (component)
				{
					text = text + " Lv" + IntToStringSansGarbage.GetStringForInt(GunderfuryController.GetCurrentLevel());
				}
			}
		}
		return text;
	}

	// Token: 0x04007C8F RID: 31887
	[HideInInspector]
	public string ProxyEncounterGuid;

	// Token: 0x04007C90 RID: 31888
	[FormerlySerializedAs("EncounterGuid")]
	[Header("Local Settings")]
	[SerializeField]
	public string m_encounterGuid;

	// Token: 0x04007C91 RID: 31889
	[FormerlySerializedAs("DoNotificationOnEncounter")]
	[SerializeField]
	public bool m_doNotificationOnEncounter = true;

	// Token: 0x04007C92 RID: 31890
	public bool SuppressInInventory;

	// Token: 0x04007C93 RID: 31891
	[Header("Database Settings")]
	[SerializeField]
	[FormerlySerializedAs("journalData")]
	public JournalEntry m_journalData;

	// Token: 0x04007C94 RID: 31892
	[FormerlySerializedAs("IgnoreDifferentiator")]
	[SerializeField]
	public bool m_ignoreDifferentiator;

	// Token: 0x04007C95 RID: 31893
	[FormerlySerializedAs("prerequisites")]
	[SerializeField]
	public DungeonPrerequisite[] m_prerequisites;

	// Token: 0x04007C96 RID: 31894
	[SerializeField]
	[FormerlySerializedAs("UsesPurpleNotifications")]
	public bool m_usesPurpleNotifications;

	// Token: 0x04007C97 RID: 31895
	[NonSerialized]
	public bool m_hasCheckedForPickup;

	// Token: 0x04007C98 RID: 31896
	[NonSerialized]
	public bool m_hasCheckedForProxy;

	// Token: 0x04007C99 RID: 31897
	[NonSerialized]
	public EncounterDatabaseEntry m_proxyEncounterTrackable;

	// Token: 0x04007C9A RID: 31898
	private PickupObject m_pickup;
}
