using System;
using Dungeonator;
using UnityEngine;

// Token: 0x0200132B RID: 4907
public class HealthPickup : PickupObject, IPlayerInteractable
{
	// Token: 0x06006F31 RID: 28465 RVA: 0x002C17F8 File Offset: 0x002BF9F8
	private void Awake()
	{
		if (Dungeon.IsGenerating)
		{
			this.m_placedInWorld = true;
		}
	}

	// Token: 0x06006F32 RID: 28466 RVA: 0x002C180C File Offset: 0x002BFA0C
	public void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.TriggerWasEntered));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody2.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnTrigger));
		if (this.minimapIcon != null && !this.m_pickedUp)
		{
			this.m_minimapIconRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
			this.m_instanceMinimapIcon = Minimap.Instance.RegisterRoomIcon(this.m_minimapIconRoom, this.minimapIcon, false);
		}
	}

	// Token: 0x06006F33 RID: 28467 RVA: 0x002C18CC File Offset: 0x002BFACC
	private void TriggerWasEntered(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody selfRigidbody, CollisionData collisionData)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (otherRigidbody.GetComponent<PlayerController>() != null)
		{
			this.PrePickupLogic(otherRigidbody, selfRigidbody);
		}
		else if (otherRigidbody.GetComponent<PickupObject>() != null && base.debris)
		{
			base.debris.ApplyVelocity((selfRigidbody.UnitCenter - otherRigidbody.UnitCenter).normalized);
			selfRigidbody.RegisterGhostCollisionException(otherRigidbody);
		}
	}

	// Token: 0x06006F34 RID: 28468 RVA: 0x002C1950 File Offset: 0x002BFB50
	private void GetRidOfMinimapIcon()
	{
		if (this.m_instanceMinimapIcon != null)
		{
			Minimap.Instance.DeregisterRoomIcon(this.m_minimapIconRoom, this.m_instanceMinimapIcon);
			this.m_instanceMinimapIcon = null;
		}
	}

	// Token: 0x06006F35 RID: 28469 RVA: 0x002C1980 File Offset: 0x002BFB80
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (GameUIRoot.HasInstance)
		{
			this.ToggleLabel(false);
		}
		if (Minimap.HasInstance)
		{
			this.GetRidOfMinimapIcon();
		}
	}

	// Token: 0x06006F36 RID: 28470 RVA: 0x002C19AC File Offset: 0x002BFBAC
	public void OnTrigger(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody selfRigidbody, CollisionData collisionData)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (otherRigidbody.GetComponent<PlayerController>() != null)
		{
			this.PrePickupLogic(otherRigidbody, selfRigidbody);
		}
	}

	// Token: 0x06006F37 RID: 28471 RVA: 0x002C19D4 File Offset: 0x002BFBD4
	private void PrePickupLogic(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody selfRigidbody)
	{
		PlayerController component = otherRigidbody.GetComponent<PlayerController>();
		if (component.IsGhost)
		{
			return;
		}
		HealthHaver healthHaver = otherRigidbody.healthHaver;
		if (component.HealthAndArmorSwapped)
		{
			if (healthHaver.GetCurrentHealth() == healthHaver.GetMaxHealth() && this.armorAmount > 0)
			{
				if (base.debris)
				{
					base.debris.ApplyVelocity(otherRigidbody.Velocity / 4f);
					selfRigidbody.RegisterTemporaryCollisionException(otherRigidbody, 0.25f, null);
				}
				return;
			}
			this.Pickup(component);
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else if (healthHaver.GetCurrentHealth() == healthHaver.GetMaxHealth() && this.armorAmount == 0)
		{
			if (!component.HasActiveBonusSynergy(CustomSynergyType.COIN_KING_OF_HEARTS, false))
			{
				if (base.debris)
				{
					base.debris.ApplyVelocity(otherRigidbody.Velocity / 4f);
					selfRigidbody.RegisterTemporaryCollisionException(otherRigidbody, 0.25f, null);
				}
				return;
			}
			this.m_pickedUp = true;
			AkSoundEngine.PostEvent("Play_OBJ_coin_medium_01", base.gameObject);
			int num = ((this.healAmount >= 1f) ? UnityEngine.Random.Range(5, 12) : UnityEngine.Random.Range(3, 7));
			LootEngine.SpawnCurrency((!base.sprite) ? component.CenterPosition : base.sprite.WorldCenter, num, false);
			this.GetRidOfMinimapIcon();
			this.ToggleLabel(false);
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			this.Pickup(healthHaver.GetComponent<PlayerController>());
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06006F38 RID: 28472 RVA: 0x002C1B8C File Offset: 0x002BFD8C
	public virtual void Update()
	{
		if (this.armorAmount > 0 && this.healAmount <= 0f)
		{
			if (!this.m_pickedUp)
			{
				if (!this.m_isBeingEyedByRat && Time.frameCount % 47 == 0 && !this.m_placedInWorld && base.ShouldBeTakenByRat(base.sprite.WorldCenter))
				{
					GameManager.Instance.Dungeon.StartCoroutine(base.HandleRatTheft());
				}
			}
		}
	}

	// Token: 0x06006F39 RID: 28473 RVA: 0x002C1C14 File Offset: 0x002BFE14
	public override void Pickup(PlayerController player)
	{
		if (player.IsGhost)
		{
			return;
		}
		base.HandleEncounterable(player);
		this.GetRidOfMinimapIcon();
		this.ToggleLabel(false);
		this.m_pickedUp = true;
		AkSoundEngine.PostEvent("Play_OBJ_heart_heal_01", base.gameObject);
		if (this.armorAmount > 0 && this.healAmount > 0f)
		{
			bool flag = player.healthHaver.GetCurrentHealth() != player.healthHaver.GetMaxHealth();
			if (player.HealthAndArmorSwapped)
			{
				player.healthHaver.Armor += (float)Mathf.CeilToInt(this.healAmount);
				player.healthHaver.ApplyHealing((float)this.armorAmount);
			}
			else
			{
				player.healthHaver.ApplyHealing(this.healAmount);
				player.healthHaver.Armor += (float)this.armorAmount;
			}
			if (flag && this.healVFX != null)
			{
				player.PlayEffectOnActor(this.healVFX, Vector3.zero, true, false, false);
			}
			else if (this.armorVFX != null)
			{
				player.PlayEffectOnActor(this.armorVFX, Vector3.zero, true, false, false);
			}
			else if (this.healVFX != null)
			{
				player.PlayEffectOnActor(this.healVFX, Vector3.zero, true, false, false);
			}
		}
		else if (this.armorAmount > 0)
		{
			if (this.armorVFX != null)
			{
				player.PlayEffectOnActor(this.armorVFX, Vector3.zero, true, false, false);
			}
			if (player.HealthAndArmorSwapped)
			{
				player.healthHaver.ApplyHealing((float)this.armorAmount);
			}
			else
			{
				player.healthHaver.Armor += (float)this.armorAmount;
			}
		}
		else
		{
			if (this.healVFX != null)
			{
				player.PlayEffectOnActor(this.healVFX, Vector3.zero, true, false, false);
			}
			if (player.HealthAndArmorSwapped)
			{
				player.healthHaver.Armor += (float)Mathf.CeilToInt(this.healAmount);
			}
			else
			{
				player.healthHaver.ApplyHealing(this.healAmount);
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06006F3A RID: 28474 RVA: 0x002C1E64 File Offset: 0x002C0064
	public float GetDistanceToPoint(Vector2 point)
	{
		if (this.IsBeingSold || this.m_pickedUp)
		{
			return 1000f;
		}
		if (!base.sprite)
		{
			return 1000f;
		}
		if (this.armorAmount > 0)
		{
			return 1000f;
		}
		Bounds bounds = base.sprite.GetBounds();
		bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
	}

	// Token: 0x06006F3B RID: 28475 RVA: 0x002C1F88 File Offset: 0x002C0188
	public void ToggleLabel(bool enabledValue)
	{
		if (enabledValue)
		{
			GameObject gameObject = GameUIRoot.Instance.RegisterDefaultLabel(base.transform, new Vector3(1f, 0.1875f, 0f), StringTableManager.GetString("#SAVE_FOR_LATER"));
			dfLabel componentInChildren = gameObject.GetComponentInChildren<dfLabel>();
			componentInChildren.ColorizeSymbols = false;
			componentInChildren.ProcessMarkup = true;
		}
		else if (!GameManager.Instance.IsLoadingLevel && GameUIRoot.Instance)
		{
			GameUIRoot.Instance.DeregisterDefaultLabel(base.transform);
		}
	}

	// Token: 0x06006F3C RID: 28476 RVA: 0x002C2014 File Offset: 0x002C0214
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this || this.m_pickedUp)
		{
			return;
		}
		if (this.armorAmount > 0)
		{
			return;
		}
		if (!HeartDispenser.DispenserOnFloor)
		{
			return;
		}
		if (!RoomHandler.unassignedInteractableObjects.Contains(this))
		{
			return;
		}
		if (interactor.healthHaver.GetCurrentHealthPercentage() >= 1f)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			base.sprite.UpdateZDepth();
			this.ToggleLabel(true);
		}
	}

	// Token: 0x06006F3D RID: 28477 RVA: 0x002C20B0 File Offset: 0x002C02B0
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		if (this.armorAmount > 0)
		{
			return;
		}
		if (!HeartDispenser.DispenserOnFloor)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		if (this.m_pickedUp)
		{
			return;
		}
		base.sprite.UpdateZDepth();
		this.ToggleLabel(false);
	}

	// Token: 0x06006F3E RID: 28478 RVA: 0x002C210C File Offset: 0x002C030C
	public void Interact(PlayerController interactor)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (!HeartDispenser.DispenserOnFloor)
		{
			return;
		}
		if (this.armorAmount > 0)
		{
			return;
		}
		if (interactor.healthHaver.GetCurrentHealthPercentage() >= 1f)
		{
			this.ToggleLabel(false);
			base.spriteAnimator.PlayAndDestroyObject((this.healAmount <= 0.5f) ? "heart_small_teleport" : "heart_big_teleport", null);
			if (this.healAmount > 0.5f)
			{
				HeartDispenser.CurrentHalfHeartsStored += 2;
			}
			else
			{
				HeartDispenser.CurrentHalfHeartsStored++;
			}
			this.m_pickedUp = true;
		}
	}

	// Token: 0x06006F3F RID: 28479 RVA: 0x002C21B8 File Offset: 0x002C03B8
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x06006F40 RID: 28480 RVA: 0x002C21C4 File Offset: 0x002C03C4
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06006F41 RID: 28481 RVA: 0x002C21CC File Offset: 0x002C03CC
	private RoomHandler FindShop()
	{
		RoomHandler roomHandler = null;
		for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
		{
			RoomHandler roomHandler2 = GameManager.Instance.Dungeon.data.rooms[i];
			if (roomHandler2.IsShop)
			{
				BaseShopController[] componentsInChildren = roomHandler2.hierarchyParent.GetComponentsInChildren<BaseShopController>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					if (componentsInChildren[j] && componentsInChildren[j].baseShopType == BaseShopController.AdditionalShopType.NONE)
					{
						roomHandler = roomHandler2;
						break;
					}
				}
				if (roomHandler != null)
				{
					break;
				}
			}
		}
		return roomHandler;
	}

	// Token: 0x04006E95 RID: 28309
	public string pickupName;

	// Token: 0x04006E96 RID: 28310
	public float healAmount = 1f;

	// Token: 0x04006E97 RID: 28311
	public int armorAmount;

	// Token: 0x04006E98 RID: 28312
	public GameObject healVFX;

	// Token: 0x04006E99 RID: 28313
	public GameObject armorVFX;

	// Token: 0x04006E9A RID: 28314
	public GameObject minimapIcon;

	// Token: 0x04006E9B RID: 28315
	private bool m_pickedUp;

	// Token: 0x04006E9C RID: 28316
	private RoomHandler m_minimapIconRoom;

	// Token: 0x04006E9D RID: 28317
	private GameObject m_instanceMinimapIcon;

	// Token: 0x04006E9E RID: 28318
	private bool m_placedInWorld;
}
