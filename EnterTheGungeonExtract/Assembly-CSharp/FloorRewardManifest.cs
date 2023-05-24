using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001492 RID: 5266
public class FloorRewardManifest
{
	// Token: 0x060077D0 RID: 30672 RVA: 0x002FD5F0 File Offset: 0x002FB7F0
	public void Initialize(RewardManager manager)
	{
		for (int i = 0; i < 5; i++)
		{
			GameObject gameObject = manager.GetRewardObjectForBossSeeded(null, false);
			if (gameObject)
			{
				PickupObject component = gameObject.GetComponent<PickupObject>();
				this.PregeneratedBossRewards.Add(component);
			}
			gameObject = manager.GetRewardObjectForBossSeeded(null, true);
			if (gameObject)
			{
				PickupObject component2 = gameObject.GetComponent<PickupObject>();
				this.PregeneratedBossRewardsGunsOnly.Add(component2);
			}
		}
	}

	// Token: 0x060077D1 RID: 30673 RVA: 0x002FD660 File Offset: 0x002FB860
	public void Reinitialize(RewardManager manager)
	{
		this.PregeneratedChestContents.Clear();
		this.OtherRegisteredRewards.Clear();
	}

	// Token: 0x060077D2 RID: 30674 RVA: 0x002FD678 File Offset: 0x002FB878
	public bool CheckManifestDifferentiator(PickupObject testItem)
	{
		if (this.PregeneratedBossRewards.Count > 0 && testItem.PickupObjectId == this.PregeneratedBossRewards[0].PickupObjectId)
		{
			return true;
		}
		if (this.PregeneratedBossRewardsGunsOnly.Count > 0 && testItem.PickupObjectId == this.PregeneratedBossRewardsGunsOnly[0].PickupObjectId)
		{
			return true;
		}
		foreach (KeyValuePair<Chest, List<PickupObject>> keyValuePair in this.PregeneratedChestContents)
		{
			for (int i = 0; i < keyValuePair.Value.Count; i++)
			{
				if (keyValuePair.Value[i].PickupObjectId == testItem.PickupObjectId)
				{
					return true;
				}
			}
		}
		for (int j = 0; j < this.OtherRegisteredRewards.Count; j++)
		{
			if (this.OtherRegisteredRewards[j].PickupObjectId == testItem.PickupObjectId)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060077D3 RID: 30675 RVA: 0x002FD7B0 File Offset: 0x002FB9B0
	public PickupObject GetNextBossReward(bool forceGun)
	{
		if (forceGun)
		{
			this.m_bossGunIndex++;
			return this.PregeneratedBossRewardsGunsOnly[this.m_bossGunIndex - 1];
		}
		this.m_bossIndex++;
		return this.PregeneratedBossRewards[this.m_bossIndex - 1];
	}

	// Token: 0x060077D4 RID: 30676 RVA: 0x002FD808 File Offset: 0x002FBA08
	public void RegisterContents(Chest source, List<PickupObject> contents)
	{
		this.PregeneratedChestContents.Add(source, contents);
	}

	// Token: 0x040079FC RID: 31228
	public Dictionary<Chest, List<PickupObject>> PregeneratedChestContents = new Dictionary<Chest, List<PickupObject>>();

	// Token: 0x040079FD RID: 31229
	public List<PickupObject> PregeneratedBossRewards = new List<PickupObject>();

	// Token: 0x040079FE RID: 31230
	private int m_bossIndex;

	// Token: 0x040079FF RID: 31231
	public List<PickupObject> PregeneratedBossRewardsGunsOnly = new List<PickupObject>();

	// Token: 0x04007A00 RID: 31232
	private int m_bossGunIndex;

	// Token: 0x04007A01 RID: 31233
	public List<PickupObject> OtherRegisteredRewards = new List<PickupObject>();
}
