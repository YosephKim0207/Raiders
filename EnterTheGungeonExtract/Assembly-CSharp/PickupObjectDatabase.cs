using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001504 RID: 5380
public class PickupObjectDatabase : ObjectDatabase<PickupObject>
{
	// Token: 0x17001212 RID: 4626
	// (get) Token: 0x06007ABD RID: 31421 RVA: 0x00313568 File Offset: 0x00311768
	public static PickupObjectDatabase Instance
	{
		get
		{
			if (PickupObjectDatabase.m_instance == null)
			{
				PickupObjectDatabase.m_instance = BraveResources.Load<PickupObjectDatabase>("PickupObjectDatabase", ".asset");
			}
			return PickupObjectDatabase.m_instance;
		}
	}

	// Token: 0x17001213 RID: 4627
	// (get) Token: 0x06007ABE RID: 31422 RVA: 0x00313594 File Offset: 0x00311794
	public static bool HasInstance
	{
		get
		{
			return PickupObjectDatabase.m_instance != null;
		}
	}

	// Token: 0x06007ABF RID: 31423 RVA: 0x003135A4 File Offset: 0x003117A4
	public static void Unload()
	{
		PickupObjectDatabase.m_instance = null;
		Resources.UnloadAsset(PickupObjectDatabase.m_instance);
	}

	// Token: 0x06007AC0 RID: 31424 RVA: 0x003135B8 File Offset: 0x003117B8
	public static int GetId(PickupObject obj)
	{
		return PickupObjectDatabase.Instance.InternalGetId(obj);
	}

	// Token: 0x06007AC1 RID: 31425 RVA: 0x003135C8 File Offset: 0x003117C8
	public static PickupObject GetById(int id)
	{
		return PickupObjectDatabase.Instance.InternalGetById(id);
	}

	// Token: 0x06007AC2 RID: 31426 RVA: 0x003135D8 File Offset: 0x003117D8
	public static PickupObject GetByName(string name)
	{
		return PickupObjectDatabase.Instance.InternalGetByName(name);
	}

	// Token: 0x06007AC3 RID: 31427 RVA: 0x003135E8 File Offset: 0x003117E8
	public static Gun GetRandomGun()
	{
		List<Gun> list = new List<Gun>();
		for (int i = 0; i < PickupObjectDatabase.Instance.Objects.Count; i++)
		{
			if (PickupObjectDatabase.Instance.Objects[i] != null && PickupObjectDatabase.Instance.Objects[i] is Gun)
			{
				if (PickupObjectDatabase.Instance.Objects[i].quality != PickupObject.ItemQuality.EXCLUDED && PickupObjectDatabase.Instance.Objects[i].quality != PickupObject.ItemQuality.SPECIAL)
				{
					if (PickupObjectDatabase.Instance.Objects[i].contentSource != ContentSource.EXCLUDED)
					{
						if (!(PickupObjectDatabase.Instance.Objects[i] is ContentTeaserGun))
						{
							EncounterTrackable component = PickupObjectDatabase.Instance.Objects[i].GetComponent<EncounterTrackable>();
							if (component && component.PrerequisitesMet())
							{
								list.Add(PickupObjectDatabase.Instance.Objects[i] as Gun);
							}
						}
					}
				}
			}
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x06007AC4 RID: 31428 RVA: 0x0031372C File Offset: 0x0031192C
	public static Gun GetRandomStartingGun(System.Random usedRandom)
	{
		List<Gun> list = new List<Gun>();
		for (int i = 0; i < PickupObjectDatabase.Instance.Objects.Count; i++)
		{
			if (PickupObjectDatabase.Instance.Objects[i] != null && PickupObjectDatabase.Instance.Objects[i] is Gun)
			{
				if (PickupObjectDatabase.Instance.Objects[i].quality != PickupObject.ItemQuality.EXCLUDED)
				{
					if (!(PickupObjectDatabase.Instance.Objects[i] is ContentTeaserGun))
					{
						if ((PickupObjectDatabase.Instance.Objects[i] as Gun).StarterGunForAchievement)
						{
							if ((PickupObjectDatabase.Instance.Objects[i] as Gun).InfiniteAmmo)
							{
								EncounterTrackable component = PickupObjectDatabase.Instance.Objects[i].GetComponent<EncounterTrackable>();
								if (component && component.PrerequisitesMet())
								{
									list.Add(PickupObjectDatabase.Instance.Objects[i] as Gun);
								}
							}
						}
					}
				}
			}
		}
		return list[usedRandom.Next(list.Count)];
	}

	// Token: 0x06007AC5 RID: 31429 RVA: 0x00313878 File Offset: 0x00311A78
	public static Gun GetRandomGunOfQualities(System.Random usedRandom, List<int> excludedIDs, params PickupObject.ItemQuality[] qualities)
	{
		List<Gun> list = new List<Gun>();
		for (int i = 0; i < PickupObjectDatabase.Instance.Objects.Count; i++)
		{
			if (PickupObjectDatabase.Instance.Objects[i] != null && PickupObjectDatabase.Instance.Objects[i] is Gun)
			{
				if (PickupObjectDatabase.Instance.Objects[i].quality != PickupObject.ItemQuality.EXCLUDED && PickupObjectDatabase.Instance.Objects[i].quality != PickupObject.ItemQuality.SPECIAL)
				{
					if (!(PickupObjectDatabase.Instance.Objects[i] is ContentTeaserGun))
					{
						if (Array.IndexOf<PickupObject.ItemQuality>(qualities, PickupObjectDatabase.Instance.Objects[i].quality) != -1)
						{
							if (!excludedIDs.Contains(PickupObjectDatabase.Instance.Objects[i].PickupObjectId))
							{
								if (PickupObjectDatabase.Instance.Objects[i].PickupObjectId != GlobalItemIds.UnfinishedGun || !GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_AMMONOMICON_COMPLETE))
								{
									EncounterTrackable component = PickupObjectDatabase.Instance.Objects[i].GetComponent<EncounterTrackable>();
									if (component && component.PrerequisitesMet())
									{
										list.Add(PickupObjectDatabase.Instance.Objects[i] as Gun);
									}
								}
							}
						}
					}
				}
			}
		}
		int num = usedRandom.Next(list.Count);
		if (num < 0 || num >= list.Count)
		{
			return null;
		}
		return list[num];
	}

	// Token: 0x06007AC6 RID: 31430 RVA: 0x00313A30 File Offset: 0x00311C30
	public static PassiveItem GetRandomPassiveOfQualities(System.Random usedRandom, List<int> excludedIDs, params PickupObject.ItemQuality[] qualities)
	{
		List<PassiveItem> list = new List<PassiveItem>();
		for (int i = 0; i < PickupObjectDatabase.Instance.Objects.Count; i++)
		{
			if (PickupObjectDatabase.Instance.Objects[i] != null && PickupObjectDatabase.Instance.Objects[i] is PassiveItem)
			{
				if (PickupObjectDatabase.Instance.Objects[i].quality != PickupObject.ItemQuality.EXCLUDED && PickupObjectDatabase.Instance.Objects[i].quality != PickupObject.ItemQuality.SPECIAL)
				{
					if (!(PickupObjectDatabase.Instance.Objects[i] is ContentTeaserItem))
					{
						if (Array.IndexOf<PickupObject.ItemQuality>(qualities, PickupObjectDatabase.Instance.Objects[i].quality) != -1)
						{
							if (!excludedIDs.Contains(PickupObjectDatabase.Instance.Objects[i].PickupObjectId))
							{
								EncounterTrackable component = PickupObjectDatabase.Instance.Objects[i].GetComponent<EncounterTrackable>();
								if (component && component.PrerequisitesMet())
								{
									list.Add(PickupObjectDatabase.Instance.Objects[i] as PassiveItem);
								}
							}
						}
					}
				}
			}
		}
		int num = usedRandom.Next(list.Count);
		if (num < 0 || num >= list.Count)
		{
			return null;
		}
		return list[num];
	}

	// Token: 0x06007AC7 RID: 31431 RVA: 0x00313BB0 File Offset: 0x00311DB0
	public static PickupObject GetByEncounterName(string name)
	{
		for (int i = 0; i < PickupObjectDatabase.Instance.Objects.Count; i++)
		{
			PickupObject pickupObject = PickupObjectDatabase.Instance.Objects[i];
			if (pickupObject)
			{
				EncounterTrackable encounterTrackable = pickupObject.encounterTrackable;
				if (encounterTrackable)
				{
					string primaryDisplayName = encounterTrackable.journalData.GetPrimaryDisplayName(false);
					if (primaryDisplayName.Equals(name, StringComparison.OrdinalIgnoreCase))
					{
						return pickupObject;
					}
				}
			}
		}
		return null;
	}

	// Token: 0x04007D4F RID: 32079
	private static PickupObjectDatabase m_instance;
}
