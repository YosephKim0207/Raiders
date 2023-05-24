using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001708 RID: 5896
public class PaydaySynergyProcessor : MonoBehaviour
{
	// Token: 0x06008915 RID: 35093 RVA: 0x0038DC58 File Offset: 0x0038BE58
	public IEnumerator Start()
	{
		while (Dungeon.IsGenerating)
		{
			yield return null;
		}
		if (base.transform.parent && base.transform.parent.GetComponent<PlayerController>())
		{
			this.Initialize(base.transform.parent.GetComponent<PlayerController>());
		}
		yield break;
	}

	// Token: 0x06008916 RID: 35094 RVA: 0x0038DC74 File Offset: 0x0038BE74
	private List<IPaydayItem> GetExtantPaydayItems()
	{
		List<IPaydayItem> list = new List<IPaydayItem>();
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			if (playerController && !playerController.IsGhost)
			{
				for (int j = 0; j < playerController.activeItems.Count; j++)
				{
					if (playerController.activeItems[j] is IPaydayItem)
					{
						list.Add(playerController.activeItems[j] as IPaydayItem);
					}
				}
				for (int k = 0; k < playerController.passiveItems.Count; k++)
				{
					if (playerController.passiveItems[k] is IPaydayItem)
					{
						list.Add(playerController.passiveItems[k] as IPaydayItem);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06008917 RID: 35095 RVA: 0x0038DD60 File Offset: 0x0038BF60
	public void Initialize(PlayerController ownerPlayer)
	{
		if (ownerPlayer == null)
		{
			return;
		}
		this.m_player = ownerPlayer;
		CompanionSynergyProcessor[] components = base.GetComponents<CompanionSynergyProcessor>();
		List<string> list = new List<string>();
		for (int i = 0; i < components.Length; i++)
		{
			list.Add(components[i].CompanionGuid);
			components[i].ManuallyAssignedPlayer = this.m_player;
		}
		List<IPaydayItem> extantPaydayItems = this.GetExtantPaydayItems();
		bool flag = false;
		IPaydayItem paydayItem = null;
		for (int j = 0; j < extantPaydayItems.Count; j++)
		{
			if (extantPaydayItems[j].HasCachedData())
			{
				flag = true;
				paydayItem = extantPaydayItems[j];
				break;
			}
		}
		if (flag)
		{
			list.Clear();
			list.Add(paydayItem.GetID(0));
			list.Add(paydayItem.GetID(1));
			list.Add(paydayItem.GetID(2));
			for (int k = 0; k < components.Length; k++)
			{
				components[k].CompanionGuid = list[k];
			}
		}
		else
		{
			list = list.Shuffle<string>();
			for (int l = 0; l < components.Length; l++)
			{
				components[l].CompanionGuid = list[l];
			}
			for (int m = 0; m < components.Length; m++)
			{
				for (int n = 0; n < extantPaydayItems.Count; n++)
				{
					extantPaydayItems[n].StoreData(components[0].CompanionGuid, components[1].CompanionGuid, components[2].CompanionGuid);
				}
			}
		}
	}

	// Token: 0x06008918 RID: 35096 RVA: 0x0038DEF8 File Offset: 0x0038C0F8
	public void Update()
	{
		int num = 0;
		bool flag = false;
		if (!this.m_player)
		{
			this.Initialize(base.transform.parent.GetComponent<PlayerController>());
		}
		for (int i = 0; i < this.m_player.passiveItems.Count; i++)
		{
			if (this.m_player.passiveItems[i] is BankMaskItem)
			{
				flag = true;
			}
			if (this.m_player.passiveItems[i].PickupObjectId == this.ItemID01)
			{
				num++;
			}
			if (this.m_player.passiveItems[i].PickupObjectId == this.ItemID02)
			{
				num++;
			}
			if (this.m_player.passiveItems[i].PickupObjectId == this.ItemID03)
			{
				num++;
			}
		}
		for (int j = 0; j < this.m_player.activeItems.Count; j++)
		{
			if (this.m_player.activeItems[j].PickupObjectId == this.ItemID01)
			{
				num++;
			}
			if (this.m_player.activeItems[j].PickupObjectId == this.ItemID02)
			{
				num++;
			}
			if (this.m_player.activeItems[j].PickupObjectId == this.ItemID03)
			{
				num++;
			}
		}
		if (!flag)
		{
			num = 0;
		}
		this.m_player.CustomEventSynergies.Remove(CustomSynergyType.PAYDAY_ONEITEM);
		this.m_player.CustomEventSynergies.Remove(CustomSynergyType.PAYDAY_TWOITEM);
		this.m_player.CustomEventSynergies.Remove(CustomSynergyType.PAYDAY_THREEITEM);
		if (num > 0)
		{
			if (num == 1)
			{
				this.m_player.CustomEventSynergies.Add(CustomSynergyType.PAYDAY_ONEITEM);
			}
			else if (num == 2)
			{
				this.m_player.CustomEventSynergies.Add(CustomSynergyType.PAYDAY_ONEITEM);
				this.m_player.CustomEventSynergies.Add(CustomSynergyType.PAYDAY_TWOITEM);
			}
			else
			{
				this.m_player.CustomEventSynergies.Add(CustomSynergyType.PAYDAY_ONEITEM);
				this.m_player.CustomEventSynergies.Add(CustomSynergyType.PAYDAY_TWOITEM);
				this.m_player.CustomEventSynergies.Add(CustomSynergyType.PAYDAY_THREEITEM);
			}
		}
	}

	// Token: 0x04008EED RID: 36589
	[PickupIdentifier]
	public int ItemID01;

	// Token: 0x04008EEE RID: 36590
	[PickupIdentifier]
	public int ItemID02;

	// Token: 0x04008EEF RID: 36591
	[PickupIdentifier]
	public int ItemID03;

	// Token: 0x04008EF0 RID: 36592
	private PlayerController m_player;
}
