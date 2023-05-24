using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200145F RID: 5215
public class PileOfDarkSoulsPickup : PickupObject, IPlayerInteractable
{
	// Token: 0x06007675 RID: 30325 RVA: 0x002F28C8 File Offset: 0x002F0AC8
	private void Start()
	{
		if (this.minimapIcon != null && !this.m_pickedUp)
		{
			this.m_minimapIconRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
			this.m_instanceMinimapIcon = Minimap.Instance.RegisterRoomIcon(this.m_minimapIconRoom, this.minimapIcon, false);
		}
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
	}

	// Token: 0x06007676 RID: 30326 RVA: 0x002F2954 File Offset: 0x002F0B54
	public void ToggleItems(bool val)
	{
		for (int i = 0; i < this.guns.Count; i++)
		{
			this.guns[i].gameObject.SetActive(val);
			this.guns[i].GetRidOfMinimapIcon();
		}
		for (int j = 0; j < this.activeItems.Count; j++)
		{
			this.activeItems[j].gameObject.SetActive(val);
			this.activeItems[j].GetRidOfMinimapIcon();
		}
		for (int k = 0; k < this.passiveItems.Count; k++)
		{
			this.passiveItems[k].gameObject.SetActive(val);
			this.passiveItems[k].GetRidOfMinimapIcon();
		}
		for (int l = 0; l < this.additionalItems.Count; l++)
		{
			this.additionalItems[l].gameObject.SetActive(val);
		}
	}

	// Token: 0x06007677 RID: 30327 RVA: 0x002F2A60 File Offset: 0x002F0C60
	private void GetRidOfMinimapIcon()
	{
		if (this.m_instanceMinimapIcon != null)
		{
			Minimap.Instance.DeregisterRoomIcon(this.m_minimapIconRoom, this.m_instanceMinimapIcon);
			this.m_instanceMinimapIcon = null;
		}
	}

	// Token: 0x06007678 RID: 30328 RVA: 0x002F2A90 File Offset: 0x002F0C90
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Minimap.HasInstance)
		{
			this.GetRidOfMinimapIcon();
		}
	}

	// Token: 0x06007679 RID: 30329 RVA: 0x002F2AA8 File Offset: 0x002F0CA8
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		this.ToggleItems(true);
		player.HandleDarkSoulsHollowTransition(false);
		this.m_pickedUp = true;
		player.healthHaver.CursedMaximum = float.MaxValue;
		float currentHealth = player.healthHaver.GetCurrentHealth();
		player.carriedConsumables.Currency += this.containedCurrency;
		EncounterTrackable.SuppressNextNotification = true;
		PileOfDarkSoulsPickup.IsPileOfDarkSoulsPickup = true;
		bool flag = false;
		for (int i = 0; i < this.passiveItems.Count; i++)
		{
			EncounterTrackable.SuppressNextNotification = true;
			this.passiveItems[i].Pickup(player);
			if (this.passiveItems[i] is ExtraLifeItem && !flag)
			{
				ExtraLifeItem extraLifeItem = this.passiveItems[i] as ExtraLifeItem;
				if (extraLifeItem.extraLifeMode == ExtraLifeItem.ExtraLifeMode.DARK_SOULS && extraLifeItem.encounterTrackable)
				{
					flag = true;
					EncounterTrackable.SuppressNextNotification = false;
					GameUIRoot.Instance.notificationController.DoNotification(extraLifeItem.encounterTrackable, false);
					EncounterTrackable.SuppressNextNotification = true;
				}
			}
		}
		for (int j = 0; j < this.activeItems.Count; j++)
		{
			EncounterTrackable.SuppressNextNotification = true;
			this.activeItems[j].Pickup(player);
		}
		for (int k = 0; k < this.guns.Count; k++)
		{
			EncounterTrackable.SuppressNextNotification = true;
			this.guns[k].Pickup(player);
		}
		for (int l = 0; l < this.additionalItems.Count; l++)
		{
			EncounterTrackable.SuppressNextNotification = true;
			this.additionalItems[l].Pickup(player);
		}
		player.ChangeGun(1, false, false);
		EncounterTrackable.SuppressNextNotification = false;
		PileOfDarkSoulsPickup.IsPileOfDarkSoulsPickup = false;
		player.healthHaver.ForceSetCurrentHealth(currentHealth);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600767A RID: 30330 RVA: 0x002F2C94 File Offset: 0x002F0E94
	public float GetDistanceToPoint(Vector2 point)
	{
		if (!base.sprite)
		{
			return 1000f;
		}
		return Vector2.Distance(point, base.sprite.WorldCenter) / 2f;
	}

	// Token: 0x0600767B RID: 30331 RVA: 0x002F2CC4 File Offset: 0x002F0EC4
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x0600767C RID: 30332 RVA: 0x002F2CCC File Offset: 0x002F0ECC
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		if (interactor.PlayerIDX != this.TargetPlayerID)
		{
			return;
		}
		if (!RoomHandler.unassignedInteractableObjects.Contains(this))
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x0600767D RID: 30333 RVA: 0x002F2D3C File Offset: 0x002F0F3C
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		if (interactor.PlayerIDX != this.TargetPlayerID)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x0600767E RID: 30334 RVA: 0x002F2D9C File Offset: 0x002F0F9C
	public void Interact(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		if (interactor.PlayerIDX != this.TargetPlayerID)
		{
			return;
		}
		if (RoomHandler.unassignedInteractableObjects.Contains(this))
		{
			RoomHandler.unassignedInteractableObjects.Remove(this);
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		this.Pickup(interactor);
	}

	// Token: 0x0600767F RID: 30335 RVA: 0x002F2DF8 File Offset: 0x002F0FF8
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x0400784F RID: 30799
	[NonSerialized]
	public List<PlayerItem> activeItems = new List<PlayerItem>();

	// Token: 0x04007850 RID: 30800
	[NonSerialized]
	public List<PassiveItem> passiveItems = new List<PassiveItem>();

	// Token: 0x04007851 RID: 30801
	[NonSerialized]
	public List<Gun> guns = new List<Gun>();

	// Token: 0x04007852 RID: 30802
	[NonSerialized]
	public List<PickupObject> additionalItems = new List<PickupObject>();

	// Token: 0x04007853 RID: 30803
	[NonSerialized]
	public int TargetPlayerID = -1;

	// Token: 0x04007854 RID: 30804
	public int containedCurrency;

	// Token: 0x04007855 RID: 30805
	public GameObject pickupVFX;

	// Token: 0x04007856 RID: 30806
	public GameObject minimapIcon;

	// Token: 0x04007857 RID: 30807
	private bool m_pickedUp;

	// Token: 0x04007858 RID: 30808
	private RoomHandler m_minimapIconRoom;

	// Token: 0x04007859 RID: 30809
	private GameObject m_instanceMinimapIcon;

	// Token: 0x0400785A RID: 30810
	public static bool IsPileOfDarkSoulsPickup;
}
