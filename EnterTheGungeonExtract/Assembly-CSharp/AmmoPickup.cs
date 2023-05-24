using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001343 RID: 4931
public class AmmoPickup : PickupObject, IPlayerInteractable
{
	// Token: 0x170010FC RID: 4348
	// (get) Token: 0x06006FD4 RID: 28628 RVA: 0x002C5498 File Offset: 0x002C3698
	public bool pickedUp
	{
		get
		{
			return this.m_pickedUp;
		}
	}

	// Token: 0x06006FD5 RID: 28629 RVA: 0x002C54A0 File Offset: 0x002C36A0
	private void Start()
	{
		if (this.minimapIcon != null && !this.m_pickedUp)
		{
			this.m_minimapIconRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
			this.m_instanceMinimapIcon = Minimap.Instance.RegisterRoomIcon(this.m_minimapIconRoom, this.minimapIcon, false);
		}
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		if (this.AppliesCustomAmmunition)
		{
			base.sprite.usesOverrideMaterial = true;
			base.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/RainbowChestShader");
		}
	}

	// Token: 0x06006FD6 RID: 28630 RVA: 0x002C5564 File Offset: 0x002C3764
	private void Update()
	{
		if (!this.m_pickedUp && !this.m_isBeingEyedByRat && base.ShouldBeTakenByRat(base.sprite.WorldCenter))
		{
			GameManager.Instance.Dungeon.StartCoroutine(base.HandleRatTheft());
		}
	}

	// Token: 0x06006FD7 RID: 28631 RVA: 0x002C55B4 File Offset: 0x002C37B4
	private void GetRidOfMinimapIcon()
	{
		if (this.m_instanceMinimapIcon != null)
		{
			Minimap.Instance.DeregisterRoomIcon(this.m_minimapIconRoom, this.m_instanceMinimapIcon);
			this.m_instanceMinimapIcon = null;
		}
	}

	// Token: 0x06006FD8 RID: 28632 RVA: 0x002C55E4 File Offset: 0x002C37E4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Minimap.HasInstance)
		{
			this.GetRidOfMinimapIcon();
		}
	}

	// Token: 0x06006FD9 RID: 28633 RVA: 0x002C55FC File Offset: 0x002C37FC
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		player.ResetTarnisherClipCapacity();
		if (!(player.CurrentGun == null) && player.CurrentGun.ammo != player.CurrentGun.AdjustedMaxAmmo && player.CurrentGun.CanGainAmmo)
		{
			switch (this.mode)
			{
			case AmmoPickup.AmmoPickupMode.ONE_CLIP:
				player.CurrentGun.GainAmmo(player.CurrentGun.ClipCapacity);
				break;
			case AmmoPickup.AmmoPickupMode.FULL_AMMO:
				if (player.CurrentGun.AdjustedMaxAmmo > 0)
				{
					player.CurrentGun.GainAmmo(player.CurrentGun.AdjustedMaxAmmo);
					player.CurrentGun.ForceImmediateReload(false);
					string @string = StringTableManager.GetString("#AMMO_SINGLE_GUN_REFILLED_HEADER");
					string text = player.CurrentGun.GetComponent<EncounterTrackable>().journalData.GetPrimaryDisplayName(false) + " " + StringTableManager.GetString("#AMMO_SINGLE_GUN_REFILLED_BODY");
					tk2dBaseSprite sprite = player.CurrentGun.GetSprite();
					if (!GameUIRoot.Instance.BossHealthBarVisible)
					{
						GameUIRoot.Instance.notificationController.DoCustomNotification(@string, text, sprite.Collection, sprite.spriteId, UINotificationController.NotificationColor.SILVER, false, false);
					}
				}
				break;
			case AmmoPickup.AmmoPickupMode.SPREAD_AMMO:
			{
				player.CurrentGun.GainAmmo(Mathf.CeilToInt((float)player.CurrentGun.AdjustedMaxAmmo * this.SpreadAmmoCurrentGunPercent));
				for (int i = 0; i < player.inventory.AllGuns.Count; i++)
				{
					if (player.inventory.AllGuns[i] && player.CurrentGun != player.inventory.AllGuns[i])
					{
						player.inventory.AllGuns[i].GainAmmo(Mathf.FloorToInt((float)player.inventory.AllGuns[i].AdjustedMaxAmmo * this.SpreadAmmoOtherGunsPercent));
					}
				}
				player.CurrentGun.ForceImmediateReload(false);
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(player);
					if (!otherPlayer.IsGhost)
					{
						for (int j = 0; j < otherPlayer.inventory.AllGuns.Count; j++)
						{
							if (otherPlayer.inventory.AllGuns[j])
							{
								otherPlayer.inventory.AllGuns[j].GainAmmo(Mathf.FloorToInt((float)otherPlayer.inventory.AllGuns[j].AdjustedMaxAmmo * this.SpreadAmmoOtherGunsPercent));
							}
						}
						otherPlayer.CurrentGun.ForceImmediateReload(false);
					}
				}
				string string2 = StringTableManager.GetString("#AMMO_SINGLE_GUN_REFILLED_HEADER");
				string string3 = StringTableManager.GetString("#AMMO_SPREAD_REFILLED_BODY");
				tk2dBaseSprite sprite2 = base.sprite;
				if (!GameUIRoot.Instance.BossHealthBarVisible)
				{
					GameUIRoot.Instance.notificationController.DoCustomNotification(string2, string3, sprite2.Collection, sprite2.spriteId, UINotificationController.NotificationColor.SILVER, false, false);
				}
				break;
			}
			}
			this.m_pickedUp = true;
			this.m_isBeingEyedByRat = false;
			this.GetRidOfMinimapIcon();
			if (this.pickupVFX != null)
			{
				player.PlayEffectOnActor(this.pickupVFX, Vector3.zero, true, false, false);
			}
			UnityEngine.Object.Destroy(base.gameObject);
			AkSoundEngine.PostEvent("Play_OBJ_ammo_pickup_01", base.gameObject);
		}
	}

	// Token: 0x06006FDA RID: 28634 RVA: 0x002C5974 File Offset: 0x002C3B74
	public float GetDistanceToPoint(Vector2 point)
	{
		if (!base.sprite)
		{
			return 1000f;
		}
		Bounds bounds = base.sprite.GetBounds();
		bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2)) / 1.5f;
	}

	// Token: 0x06006FDB RID: 28635 RVA: 0x002C5A70 File Offset: 0x002C3C70
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06006FDC RID: 28636 RVA: 0x002C5A78 File Offset: 0x002C3C78
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		if (!interactor.CurrentRoom.IsRegistered(this) && !RoomHandler.unassignedInteractableObjects.Contains(this))
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x06006FDD RID: 28637 RVA: 0x002C5AE8 File Offset: 0x002C3CE8
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x06006FDE RID: 28638 RVA: 0x002C5B28 File Offset: 0x002C3D28
	public void Interact(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		if (interactor.CurrentGun == null || interactor.CurrentGun.ammo == interactor.CurrentGun.AdjustedMaxAmmo || interactor.CurrentGun.InfiniteAmmo || interactor.CurrentGun.RequiresFundsToShoot)
		{
			if (interactor.CurrentGun != null)
			{
				GameUIRoot.Instance.InformNeedsReload(interactor, new Vector3(interactor.specRigidbody.UnitCenter.x - interactor.transform.position.x, 1.25f, 0f), 1f, "#RELOAD_FULL");
			}
			return;
		}
		if (RoomHandler.unassignedInteractableObjects.Contains(this))
		{
			RoomHandler.unassignedInteractableObjects.Remove(this);
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		this.Pickup(interactor);
	}

	// Token: 0x06006FDF RID: 28639 RVA: 0x002C5C1C File Offset: 0x002C3E1C
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x04006F2E RID: 28462
	public AmmoPickup.AmmoPickupMode mode = AmmoPickup.AmmoPickupMode.FULL_AMMO;

	// Token: 0x04006F2F RID: 28463
	public GameObject pickupVFX;

	// Token: 0x04006F30 RID: 28464
	public GameObject minimapIcon;

	// Token: 0x04006F31 RID: 28465
	public float SpreadAmmoCurrentGunPercent = 0.5f;

	// Token: 0x04006F32 RID: 28466
	public float SpreadAmmoOtherGunsPercent = 0.2f;

	// Token: 0x04006F33 RID: 28467
	[Header("Custom Ammo")]
	public bool AppliesCustomAmmunition;

	// Token: 0x04006F34 RID: 28468
	[ShowInInspectorIf("AppliesCustomAmmunition", false)]
	public float CustomAmmunitionDamageModifier = 1f;

	// Token: 0x04006F35 RID: 28469
	[ShowInInspectorIf("AppliesCustomAmmunition", false)]
	public float CustomAmmunitionSpeedModifier = 1f;

	// Token: 0x04006F36 RID: 28470
	[ShowInInspectorIf("AppliesCustomAmmunition", false)]
	public float CustomAmmunitionRangeModifier = 1f;

	// Token: 0x04006F37 RID: 28471
	private bool m_pickedUp;

	// Token: 0x04006F38 RID: 28472
	private RoomHandler m_minimapIconRoom;

	// Token: 0x04006F39 RID: 28473
	private GameObject m_instanceMinimapIcon;

	// Token: 0x02001344 RID: 4932
	public enum AmmoPickupMode
	{
		// Token: 0x04006F3B RID: 28475
		ONE_CLIP,
		// Token: 0x04006F3C RID: 28476
		FULL_AMMO,
		// Token: 0x04006F3D RID: 28477
		SPREAD_AMMO
	}
}
