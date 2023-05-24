using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200139C RID: 5020
public class BankBagItem : PassiveItem, IPaydayItem
{
	// Token: 0x060071B7 RID: 29111 RVA: 0x002D30C0 File Offset: 0x002D12C0
	public void StoreData(string id1, string id2, string id3)
	{
		this.ID01 = id1;
		this.ID02 = id2;
		this.ID03 = id3;
		this.HasSetOrder = true;
	}

	// Token: 0x060071B8 RID: 29112 RVA: 0x002D30E0 File Offset: 0x002D12E0
	public bool HasCachedData()
	{
		return this.HasSetOrder;
	}

	// Token: 0x060071B9 RID: 29113 RVA: 0x002D30E8 File Offset: 0x002D12E8
	public string GetID(int placement)
	{
		if (placement == 0)
		{
			return this.ID01;
		}
		if (placement == 1)
		{
			return this.ID02;
		}
		return this.ID03;
	}

	// Token: 0x060071BA RID: 29114 RVA: 0x002D310C File Offset: 0x002D130C
	public override void MidGameSerialize(List<object> data)
	{
		base.MidGameSerialize(data);
		data.Add(this.HasSetOrder);
		data.Add(this.ID01);
		data.Add(this.ID02);
		data.Add(this.ID03);
	}

	// Token: 0x060071BB RID: 29115 RVA: 0x002D314C File Offset: 0x002D134C
	public override void MidGameDeserialize(List<object> data)
	{
		base.MidGameDeserialize(data);
		if (data.Count == 4)
		{
			this.HasSetOrder = (bool)data[0];
			this.ID01 = (string)data[1];
			this.ID02 = (string)data[2];
			this.ID03 = (string)data[3];
		}
	}

	// Token: 0x060071BC RID: 29116 RVA: 0x002D31B4 File Offset: 0x002D13B4
	public void Awake()
	{
		BankBagItem.cachedCoinLifespan = this.CoinLifespan;
	}

	// Token: 0x060071BD RID: 29117 RVA: 0x002D31C4 File Offset: 0x002D13C4
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		if (!PassiveItem.ActiveFlagItems.ContainsKey(player))
		{
			PassiveItem.ActiveFlagItems.Add(player, new Dictionary<Type, int>());
		}
		player.OnReceivedDamage += this.HandlePlayerDamaged;
		this.instanceAttachment = player.RegisterAttachedObject(this.AttachmentObject, "center", 0f);
		this.instanceAttachment.transform.parent = player.sprite.transform;
		this.instanceAttachmentSprite = this.instanceAttachment.GetComponentInChildren<tk2dSprite>();
		if (!PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player].Add(base.GetType(), 1);
		}
		else
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = PassiveItem.ActiveFlagItems[player][base.GetType()] + 1;
		}
	}

	// Token: 0x060071BE RID: 29118 RVA: 0x002D32C4 File Offset: 0x002D14C4
	private void HandlePlayerDamaged(PlayerController p)
	{
		if (p.carriedConsumables.Currency > 0)
		{
			int num = UnityEngine.Random.Range(Mathf.FloorToInt((float)p.carriedConsumables.Currency * this.MinPercentToDrop), Mathf.CeilToInt((float)p.carriedConsumables.Currency * this.MaxPercentToDrop) + 1);
			if (this.MaxCoinsToDrop > 0)
			{
				num = Mathf.Clamp(num, 0, this.MaxCoinsToDrop);
			}
			num = Mathf.Min(num, p.carriedConsumables.Currency);
			if (this.DropVFX)
			{
				p.PlayEffectOnActor(this.DropVFX, Vector3.zero, false, true, false);
			}
			AkSoundEngine.PostEvent("Play_OBJ_coin_spill_01", base.gameObject);
			p.carriedConsumables.Currency = p.carriedConsumables.Currency - num;
			LootEngine.SpawnCurrencyManual(p.CenterPosition, num);
		}
	}

	// Token: 0x060071BF RID: 29119 RVA: 0x002D33A0 File Offset: 0x002D15A0
	private void LateUpdate()
	{
		if (this.instanceAttachment && this.m_pickedUp && this.m_owner)
		{
			this.instanceAttachment.transform.position = this.m_owner.sprite.WorldCenter + new Vector2(0f, -0.125f);
			this.instanceAttachmentSprite.FlipX = this.m_owner.sprite.FlipX;
			this.instanceAttachmentSprite.transform.localPosition = new Vector3((!this.instanceAttachmentSprite.FlipX) ? (-0.5f) : 0.5f, -0.5f, 0f);
			this.instanceAttachmentSprite.renderer.enabled = this.m_owner.IsVisible && this.m_owner.sprite.renderer.enabled && !this.m_owner.IsFalling && !this.m_owner.IsDodgeRolling;
			this.instanceAttachmentSprite.UpdateZDepth();
		}
	}

	// Token: 0x060071C0 RID: 29120 RVA: 0x002D34D0 File Offset: 0x002D16D0
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		if (player)
		{
			player.OnReceivedDamage -= this.HandlePlayerDamaged;
			player.DeregisterAttachedObject(this.instanceAttachment, true);
			this.instanceAttachment = null;
			this.instanceAttachmentSprite = null;
		}
		if (PassiveItem.ActiveFlagItems.ContainsKey(player) && PassiveItem.ActiveFlagItems[player].ContainsKey(base.GetType()))
		{
			PassiveItem.ActiveFlagItems[player][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[player][base.GetType()] - 1);
			if (PassiveItem.ActiveFlagItems[player][base.GetType()] == 0)
			{
				PassiveItem.ActiveFlagItems[player].Remove(base.GetType());
			}
		}
		debrisObject.GetComponent<BankBagItem>().m_pickedUpThisRun = true;
		return debrisObject;
	}

	// Token: 0x060071C1 RID: 29121 RVA: 0x002D35BC File Offset: 0x002D17BC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_pickedUp && this.m_owner)
		{
			this.m_owner.OnReceivedDamage -= this.HandlePlayerDamaged;
			this.m_owner.DeregisterAttachedObject(this.instanceAttachment, true);
			this.instanceAttachment = null;
			this.instanceAttachmentSprite = null;
			if (PassiveItem.ActiveFlagItems[this.m_owner].ContainsKey(base.GetType()))
			{
				PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] = Mathf.Max(0, PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] - 1);
				if (PassiveItem.ActiveFlagItems[this.m_owner][base.GetType()] == 0)
				{
					PassiveItem.ActiveFlagItems[this.m_owner].Remove(base.GetType());
				}
			}
		}
	}

	// Token: 0x0400732D RID: 29485
	public static float cachedCoinLifespan = 6f;

	// Token: 0x0400732E RID: 29486
	public float CoinLifespan = 6f;

	// Token: 0x0400732F RID: 29487
	public float MinPercentToDrop = 0.5f;

	// Token: 0x04007330 RID: 29488
	public float MaxPercentToDrop = 1f;

	// Token: 0x04007331 RID: 29489
	public int MaxCoinsToDrop = -1;

	// Token: 0x04007332 RID: 29490
	public GameObject DropVFX;

	// Token: 0x04007333 RID: 29491
	public GameObject AttachmentObject;

	// Token: 0x04007334 RID: 29492
	private GameObject instanceAttachment;

	// Token: 0x04007335 RID: 29493
	private tk2dSprite instanceAttachmentSprite;

	// Token: 0x04007336 RID: 29494
	[NonSerialized]
	public bool HasSetOrder;

	// Token: 0x04007337 RID: 29495
	[NonSerialized]
	public string ID01;

	// Token: 0x04007338 RID: 29496
	[NonSerialized]
	public string ID02;

	// Token: 0x04007339 RID: 29497
	[NonSerialized]
	public string ID03;
}
