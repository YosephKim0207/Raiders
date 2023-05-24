using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001557 RID: 5463
public class ShopItemController : BraveBehaviour, IPlayerInteractable
{
	// Token: 0x17001272 RID: 4722
	// (get) Token: 0x06007D29 RID: 32041 RVA: 0x0032824C File Offset: 0x0032644C
	// (set) Token: 0x06007D2A RID: 32042 RVA: 0x00328254 File Offset: 0x00326454
	public bool Locked { get; set; }

	// Token: 0x17001273 RID: 4723
	// (get) Token: 0x06007D2B RID: 32043 RVA: 0x00328260 File Offset: 0x00326460
	public int ModifiedPrice
	{
		get
		{
			if (this.m_baseParentShop && this.m_baseParentShop.baseShopType == BaseShopController.AdditionalShopType.RESRAT_SHORTCUT)
			{
				return 0;
			}
			if (this.IsResourcefulRatKey)
			{
				int num = Mathf.RoundToInt(GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.AMOUNT_PAID_FOR_RAT_KEY));
				int num2 = 1000 - num;
				if (num2 <= 0)
				{
					return this.CurrentPrice;
				}
				return num2;
			}
			else
			{
				if (this.CurrencyType == ShopItemController.ShopCurrencyType.META_CURRENCY)
				{
					return this.CurrentPrice;
				}
				if (this.CurrencyType == ShopItemController.ShopCurrencyType.KEYS)
				{
					return this.CurrentPrice;
				}
				if (this.OverridePrice != null)
				{
					return this.OverridePrice.Value;
				}
				if (this.PrecludeAllDiscounts)
				{
					return this.CurrentPrice;
				}
				float num3 = GameManager.Instance.PrimaryPlayer.stats.GetStatValue(PlayerStats.StatType.GlobalPriceMultiplier);
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.SecondaryPlayer)
				{
					num3 *= GameManager.Instance.SecondaryPlayer.stats.GetStatValue(PlayerStats.StatType.GlobalPriceMultiplier);
				}
				GameLevelDefinition lastLoadedLevelDefinition = GameManager.Instance.GetLastLoadedLevelDefinition();
				float num4 = ((lastLoadedLevelDefinition == null) ? 1f : lastLoadedLevelDefinition.priceMultiplier);
				float num5 = 1f;
				if (this.m_baseParentShop != null && this.m_baseParentShop.ShopCostModifier != 1f)
				{
					num5 *= this.m_baseParentShop.ShopCostModifier;
				}
				if (this.m_baseParentShop.GetAbsoluteParentRoom().area.PrototypeRoomName.Contains("Black Market"))
				{
					num5 *= 0.5f;
				}
				return Mathf.RoundToInt((float)this.CurrentPrice * num3 * num4 * num5);
			}
		}
	}

	// Token: 0x17001274 RID: 4724
	// (get) Token: 0x06007D2C RID: 32044 RVA: 0x00328410 File Offset: 0x00326610
	public bool Acquired
	{
		get
		{
			return this.pickedUp;
		}
	}

	// Token: 0x06007D2D RID: 32045 RVA: 0x00328418 File Offset: 0x00326618
	public void Initialize(PickupObject i, BaseShopController parent)
	{
		this.m_baseParentShop = parent;
		this.InitializeInternal(i);
		if (parent.baseShopType != BaseShopController.AdditionalShopType.NONE)
		{
			base.sprite.depthUsesTrimmedBounds = true;
			base.sprite.HeightOffGround = -1.25f;
			base.sprite.UpdateZDepth();
		}
	}

	// Token: 0x06007D2E RID: 32046 RVA: 0x00328468 File Offset: 0x00326668
	public void Initialize(PickupObject i, ShopController parent)
	{
		this.m_parentShop = parent;
		this.InitializeInternal(i);
	}

	// Token: 0x06007D2F RID: 32047 RVA: 0x00328478 File Offset: 0x00326678
	private void InitializeInternal(PickupObject i)
	{
		this.item = i;
		if (i is SpecialKeyItem && (i as SpecialKeyItem).keyType == SpecialKeyItem.SpecialKeyType.RESOURCEFUL_RAT_LAIR)
		{
			this.IsResourcefulRatKey = true;
		}
		if (this.item && this.item.encounterTrackable)
		{
			GameStatsManager.Instance.SingleIncrementDifferentiator(this.item.encounterTrackable);
		}
		this.CurrentPrice = this.item.PurchasePrice;
		if (this.m_baseParentShop != null && this.m_baseParentShop.baseShopType == BaseShopController.AdditionalShopType.KEY)
		{
			this.CurrentPrice = 1;
			if (this.item.quality == PickupObject.ItemQuality.A)
			{
				this.CurrentPrice = 2;
			}
			if (this.item.quality == PickupObject.ItemQuality.S)
			{
				this.CurrentPrice = 3;
			}
		}
		if (this.m_baseParentShop != null && this.m_baseParentShop.baseShopType == BaseShopController.AdditionalShopType.NONE && (this.item is BankMaskItem || this.item is BankBagItem || this.item is PaydayDrillItem))
		{
			EncounterTrackable encounterTrackable = this.item.encounterTrackable;
			if (encounterTrackable && !encounterTrackable.PrerequisitesMet())
			{
				if (this.item is BankMaskItem)
				{
					this.SetsFlagOnSteal = true;
					this.FlagToSetOnSteal = GungeonFlags.ITEMSPECIFIC_STOLE_BANKMASK;
				}
				else if (this.item is BankBagItem)
				{
					this.SetsFlagOnSteal = true;
					this.FlagToSetOnSteal = GungeonFlags.ITEMSPECIFIC_STOLE_BANKBAG;
				}
				else if (this.item is PaydayDrillItem)
				{
					this.SetsFlagOnSteal = true;
					this.FlagToSetOnSteal = GungeonFlags.ITEMSPECIFIC_STOLE_DRILL;
				}
				this.OverridePrice = new int?(9999);
			}
		}
		base.gameObject.AddComponent<tk2dSprite>();
		tk2dSprite tk2dSprite = i.GetComponent<tk2dSprite>();
		if (tk2dSprite == null)
		{
			tk2dSprite = i.GetComponentInChildren<tk2dSprite>();
		}
		base.sprite.SetSprite(tk2dSprite.Collection, tk2dSprite.spriteId);
		base.sprite.IsPerpendicular = true;
		if (this.UseOmnidirectionalItemFacing)
		{
			base.sprite.IsPerpendicular = false;
		}
		base.sprite.HeightOffGround = 1f;
		if (this.m_parentShop != null)
		{
			if (this.m_parentShop is MetaShopController)
			{
				this.UseOmnidirectionalItemFacing = true;
				base.sprite.IsPerpendicular = false;
			}
			base.sprite.HeightOffGround += this.m_parentShop.ItemHeightOffGroundModifier;
		}
		else if (this.m_baseParentShop.baseShopType == BaseShopController.AdditionalShopType.BLACKSMITH)
		{
			this.UseOmnidirectionalItemFacing = true;
		}
		else if (this.m_baseParentShop.baseShopType == BaseShopController.AdditionalShopType.TRUCK || this.m_baseParentShop.baseShopType == BaseShopController.AdditionalShopType.GOOP || this.m_baseParentShop.baseShopType == BaseShopController.AdditionalShopType.CURSE || this.m_baseParentShop.baseShopType == BaseShopController.AdditionalShopType.BLANK || this.m_baseParentShop.baseShopType == BaseShopController.AdditionalShopType.KEY || this.m_baseParentShop.baseShopType == BaseShopController.AdditionalShopType.RESRAT_SHORTCUT)
		{
			this.UseOmnidirectionalItemFacing = true;
		}
		base.sprite.PlaceAtPositionByAnchor(base.transform.parent.position, tk2dBaseSprite.Anchor.MiddleCenter);
		base.sprite.transform.position = base.sprite.transform.position.Quantize(0.0625f);
		DepthLookupManager.ProcessRenderer(base.sprite.renderer);
		tk2dSprite componentInParent = base.transform.parent.gameObject.GetComponentInParent<tk2dSprite>();
		if (componentInParent != null)
		{
			componentInParent.AttachRenderer(base.sprite);
		}
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0.05f, SpriteOutlineManager.OutlineType.NORMAL);
		GameObject gameObject = null;
		if (this.m_parentShop != null && this.m_parentShop.shopItemShadowPrefab != null)
		{
			gameObject = this.m_parentShop.shopItemShadowPrefab;
		}
		if (this.m_baseParentShop != null && this.m_baseParentShop.shopItemShadowPrefab != null)
		{
			gameObject = this.m_baseParentShop.shopItemShadowPrefab;
		}
		if (gameObject != null)
		{
			if (!this.m_shadowObject)
			{
				this.m_shadowObject = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			}
			tk2dBaseSprite component = this.m_shadowObject.GetComponent<tk2dBaseSprite>();
			component.PlaceAtPositionByAnchor(base.sprite.WorldBottomCenter, tk2dBaseSprite.Anchor.MiddleCenter);
			component.transform.position = component.transform.position.Quantize(0.0625f);
			base.sprite.AttachRenderer(component);
			component.transform.parent = base.sprite.transform;
			component.HeightOffGround = -0.5f;
			if (this.m_parentShop is MetaShopController)
			{
				component.HeightOffGround = -0.0625f;
			}
		}
		base.sprite.UpdateZDepth();
		SpeculativeRigidbody orAddComponent = base.gameObject.GetOrAddComponent<SpeculativeRigidbody>();
		orAddComponent.PixelColliders = new List<PixelCollider>();
		PixelCollider pixelCollider = new PixelCollider
		{
			ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Circle,
			CollisionLayer = CollisionLayer.HighObstacle,
			ManualDiameter = 14
		};
		Vector2 vector = base.sprite.WorldCenter - base.transform.position.XY();
		pixelCollider.ManualOffsetX = PhysicsEngine.UnitToPixel(vector.x) - 7;
		pixelCollider.ManualOffsetY = PhysicsEngine.UnitToPixel(vector.y) - 7;
		orAddComponent.PixelColliders.Add(pixelCollider);
		orAddComponent.Initialize();
		orAddComponent.OnPreRigidbodyCollision = null;
		SpeculativeRigidbody speculativeRigidbody = orAddComponent;
		speculativeRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(speculativeRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.ItemOnPreRigidbodyCollision));
		base.RegenerateCache();
		if (!GameManager.Instance.IsFoyer && this.item is Gun && GameManager.Instance.PrimaryPlayer.CharacterUsesRandomGuns)
		{
			this.ForceOutOfStock();
		}
	}

	// Token: 0x06007D30 RID: 32048 RVA: 0x00328A6C File Offset: 0x00326C6C
	private void ItemOnPreRigidbodyCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (!otherRigidbody || otherRigidbody.PrimaryPixelCollider == null || otherRigidbody.PrimaryPixelCollider.CollisionLayer != CollisionLayer.Projectile)
		{
			PhysicsEngine.SkipCollision = true;
		}
	}

	// Token: 0x06007D31 RID: 32049 RVA: 0x00328A9C File Offset: 0x00326C9C
	private void Update()
	{
		if (this.m_baseParentShop && this.m_baseParentShop.baseShopType == BaseShopController.AdditionalShopType.CURSE && !this.pickedUp && base.sprite)
		{
			PickupObject.HandlePickupCurseParticles(base.sprite, 1f);
		}
	}

	// Token: 0x06007D32 RID: 32050 RVA: 0x00328AF8 File Offset: 0x00326CF8
	protected override void OnDestroy()
	{
		if (this.m_parentShop != null && this.m_parentShop is MetaShopController)
		{
			MetaShopController metaShopController = this.m_parentShop as MetaShopController;
			if (metaShopController.Hologramophone && this.item is ItemBlueprintItem)
			{
				metaShopController.Hologramophone.HideSprite(base.gameObject, false);
			}
		}
		base.OnDestroy();
	}

	// Token: 0x06007D33 RID: 32051 RVA: 0x00328B6C File Offset: 0x00326D6C
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
		Vector3 vector = new Vector3(base.sprite.GetBounds().max.x + 0.1875f, base.sprite.GetBounds().min.y, 0f);
		EncounterTrackable component = this.item.GetComponent<EncounterTrackable>();
		string text = ((!(component != null)) ? this.item.DisplayName : component.journalData.GetPrimaryDisplayName(false));
		string text2 = this.ModifiedPrice.ToString();
		if (this.m_baseParentShop != null)
		{
			if (this.m_baseParentShop.baseShopType == BaseShopController.AdditionalShopType.FOYER_META)
			{
				text2 += "[sprite \"hbux_text_icon\"]";
			}
			else if (this.m_baseParentShop.baseShopType == BaseShopController.AdditionalShopType.CURSE)
			{
				text2 += "[sprite \"ui_coin\"]?";
			}
			else if (this.m_baseParentShop.baseShopType == BaseShopController.AdditionalShopType.RESRAT_SHORTCUT)
			{
				text2 = "0[sprite \"ui_coin\"]?";
			}
			else if (this.m_baseParentShop.baseShopType == BaseShopController.AdditionalShopType.KEY)
			{
				text2 += "[sprite \"ui_key\"]";
			}
			else
			{
				text2 += "[sprite \"ui_coin\"]";
			}
		}
		if (this.m_parentShop != null && this.m_parentShop is MetaShopController)
		{
			text2 += "[sprite \"hbux_text_icon\"]";
			MetaShopController metaShopController = this.m_parentShop as MetaShopController;
			if (metaShopController.Hologramophone && this.item is ItemBlueprintItem)
			{
				ItemBlueprintItem itemBlueprintItem = this.item as ItemBlueprintItem;
				tk2dSpriteCollectionData encounterIconCollection = AmmonomiconController.ForceInstance.EncounterIconCollection;
				metaShopController.Hologramophone.ChangeToSprite(base.gameObject, encounterIconCollection, encounterIconCollection.GetSpriteIdByName(itemBlueprintItem.HologramIconSpriteName));
			}
		}
		string text3;
		if ((this.m_baseParentShop && this.m_baseParentShop.IsCapableOfBeingStolenFrom) || interactor.IsCapableOfStealing)
		{
			text3 = string.Format("[color red]{0}: {1} {2}[/color]", text, text2, StringTableManager.GetString("#STEAL"));
		}
		else
		{
			text3 = string.Format("{0}: {1}", text, text2);
		}
		if (this.IsResourcefulRatKey)
		{
			int num = Mathf.RoundToInt(GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.AMOUNT_PAID_FOR_RAT_KEY));
			if (num < 1000)
			{
				int num2 = Mathf.Min(interactor.carriedConsumables.Currency, 1000 - num);
				if (num2 > 0)
				{
					text3 = text3 + "[color green] (-" + num2.ToString() + ")[/color]";
				}
			}
		}
		GameObject gameObject = GameUIRoot.Instance.RegisterDefaultLabel(base.transform, vector, text3);
		dfLabel componentInChildren = gameObject.GetComponentInChildren<dfLabel>();
		componentInChildren.ColorizeSymbols = false;
		componentInChildren.ProcessMarkup = true;
	}

	// Token: 0x06007D34 RID: 32052 RVA: 0x00328E70 File Offset: 0x00327070
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0.05f, SpriteOutlineManager.OutlineType.NORMAL);
		GameUIRoot.Instance.DeregisterDefaultLabel(base.transform);
		if (this.m_parentShop != null && this.m_parentShop is MetaShopController)
		{
			MetaShopController metaShopController = this.m_parentShop as MetaShopController;
			if (metaShopController.Hologramophone && this.item is ItemBlueprintItem)
			{
				metaShopController.Hologramophone.HideSprite(base.gameObject, false);
			}
		}
	}

	// Token: 0x06007D35 RID: 32053 RVA: 0x00328F20 File Offset: 0x00327120
	public float GetDistanceToPoint(Vector2 point)
	{
		if (!this)
		{
			return 1000f;
		}
		if (this.Locked)
		{
			return 1000f;
		}
		if (this.UseOmnidirectionalItemFacing)
		{
			Bounds bounds = base.sprite.GetBounds();
			return BraveMathCollege.DistToRectangle(point, bounds.min + base.transform.position, bounds.size);
		}
		if (this.itemFacing == DungeonData.Direction.EAST)
		{
			Bounds bounds2 = base.sprite.GetBounds();
			bounds2.SetMinMax(bounds2.min + base.transform.position, bounds2.max + base.transform.position);
			Vector2 vector = bounds2.center.XY();
			float num = vector.x - point.x;
			float num2 = Mathf.Abs(point.y - vector.y);
			if (num > 0f)
			{
				return 1000f;
			}
			if (num < -this.THRESHOLD_CUTOFF_PRIMARY)
			{
				return 1000f;
			}
			if (num2 > this.THRESHOLD_CUTOFF_SECONDARY)
			{
				return 1000f;
			}
			return num2;
		}
		else if (this.itemFacing == DungeonData.Direction.NORTH)
		{
			Bounds bounds3 = base.sprite.GetBounds();
			bounds3.SetMinMax(bounds3.min + base.transform.position, bounds3.max + base.transform.position);
			Vector2 vector2 = bounds3.center.XY();
			float num3 = Mathf.Abs(point.x - vector2.x);
			float num4 = vector2.y - point.y;
			if (num4 > bounds3.extents.y)
			{
				return 1000f;
			}
			if (num4 < -this.THRESHOLD_CUTOFF_PRIMARY)
			{
				return 1000f;
			}
			if (num3 > this.THRESHOLD_CUTOFF_SECONDARY)
			{
				return 1000f;
			}
			return num3;
		}
		else if (this.itemFacing == DungeonData.Direction.WEST)
		{
			Bounds bounds4 = base.sprite.GetBounds();
			bounds4.SetMinMax(bounds4.min + base.transform.position, bounds4.max + base.transform.position);
			Vector2 vector3 = bounds4.center.XY();
			float num5 = vector3.x - point.x;
			float num6 = Mathf.Abs(point.y - vector3.y);
			if (num5 < 0f)
			{
				return 1000f;
			}
			if (num5 > this.THRESHOLD_CUTOFF_PRIMARY)
			{
				return 1000f;
			}
			if (num6 > this.THRESHOLD_CUTOFF_SECONDARY)
			{
				return 1000f;
			}
			return num6;
		}
		else
		{
			Bounds bounds5 = base.sprite.GetBounds();
			bounds5.SetMinMax(bounds5.min + base.transform.position, bounds5.max + base.transform.position);
			Vector2 vector4 = bounds5.center.XY();
			float num7 = Mathf.Abs(point.x - vector4.x);
			float num8 = vector4.y - point.y;
			if (num8 < bounds5.extents.y)
			{
				return 1000f;
			}
			if (num8 > this.THRESHOLD_CUTOFF_PRIMARY)
			{
				return 1000f;
			}
			if (num7 > this.THRESHOLD_CUTOFF_SECONDARY)
			{
				return 1000f;
			}
			return num7;
		}
	}

	// Token: 0x06007D36 RID: 32054 RVA: 0x00329294 File Offset: 0x00327494
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06007D37 RID: 32055 RVA: 0x0032929C File Offset: 0x0032749C
	private bool ShouldSteal(PlayerController player)
	{
		return GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.FOYER && (this.m_baseParentShop.IsCapableOfBeingStolenFrom || player.IsCapableOfStealing);
	}

	// Token: 0x06007D38 RID: 32056 RVA: 0x003292CC File Offset: 0x003274CC
	public void Interact(PlayerController player)
	{
		if (this.item && this.item is HealthPickup)
		{
			if ((this.item as HealthPickup).healAmount > 0f && (this.item as HealthPickup).armorAmount <= 0 && player.healthHaver.GetCurrentHealthPercentage() >= 1f)
			{
				return;
			}
		}
		else if (this.item && this.item is AmmoPickup && (player.CurrentGun == null || player.CurrentGun.ammo == player.CurrentGun.AdjustedMaxAmmo || !player.CurrentGun.CanGainAmmo || player.CurrentGun.InfiniteAmmo))
		{
			GameUIRoot.Instance.InformNeedsReload(player, new Vector3(player.specRigidbody.UnitCenter.x - player.transform.position.x, 1.25f, 0f), 1f, "#RELOAD_FULL");
			return;
		}
		this.LastInteractingPlayer = player;
		if (this.CurrencyType == ShopItemController.ShopCurrencyType.COINS || this.CurrencyType == ShopItemController.ShopCurrencyType.BLANKS || this.CurrencyType == ShopItemController.ShopCurrencyType.KEYS)
		{
			bool flag = false;
			bool flag2 = true;
			if (this.ShouldSteal(player))
			{
				flag = this.m_baseParentShop.AttemptToSteal();
				flag2 = false;
				if (!flag)
				{
					player.DidUnstealthyAction();
					this.m_baseParentShop.NotifyStealFailed();
					return;
				}
			}
			if (flag2)
			{
				bool flag3 = false;
				if (this.CurrencyType == ShopItemController.ShopCurrencyType.COINS || this.CurrencyType == ShopItemController.ShopCurrencyType.BLANKS)
				{
					flag3 = player.carriedConsumables.Currency >= this.ModifiedPrice;
				}
				else if (this.CurrencyType == ShopItemController.ShopCurrencyType.KEYS)
				{
					flag3 = player.carriedConsumables.KeyBullets >= this.ModifiedPrice;
				}
				if (this.IsResourcefulRatKey)
				{
					if (!flag3)
					{
						int num = Mathf.RoundToInt(GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.AMOUNT_PAID_FOR_RAT_KEY));
						if (num >= 1000)
						{
							AkSoundEngine.PostEvent("Play_OBJ_purchase_unable_01", base.gameObject);
							if (this.m_parentShop != null)
							{
								this.m_parentShop.NotifyFailedPurchase(this);
							}
							if (this.m_baseParentShop != null)
							{
								this.m_baseParentShop.NotifyFailedPurchase(this);
							}
							return;
						}
						if (player.carriedConsumables.Currency > 0)
						{
							GameStatsManager.Instance.RegisterStatChange(TrackedStats.AMOUNT_PAID_FOR_RAT_KEY, (float)player.carriedConsumables.Currency);
							player.carriedConsumables.Currency = 0;
							this.OnExitRange(player);
							this.OnEnteredRange(player);
						}
						else
						{
							AkSoundEngine.PostEvent("Play_OBJ_purchase_unable_01", base.gameObject);
							if (this.m_parentShop != null)
							{
								this.m_parentShop.NotifyFailedPurchase(this);
							}
							if (this.m_baseParentShop != null)
							{
								this.m_baseParentShop.NotifyFailedPurchase(this);
							}
						}
						return;
					}
					else
					{
						player.carriedConsumables.Currency -= this.ModifiedPrice;
						GameStatsManager.Instance.RegisterStatChange(TrackedStats.AMOUNT_PAID_FOR_RAT_KEY, (float)this.ModifiedPrice);
						flag2 = false;
					}
				}
				else if (!flag3)
				{
					AkSoundEngine.PostEvent("Play_OBJ_purchase_unable_01", base.gameObject);
					if (this.m_parentShop != null)
					{
						this.m_parentShop.NotifyFailedPurchase(this);
					}
					if (this.m_baseParentShop != null)
					{
						this.m_baseParentShop.NotifyFailedPurchase(this);
					}
					return;
				}
			}
			if (!this.pickedUp)
			{
				this.pickedUp = !this.item.PersistsOnPurchase;
				LootEngine.GivePrefabToPlayer(this.item.gameObject, player);
				if (flag2)
				{
					if (this.CurrencyType == ShopItemController.ShopCurrencyType.COINS || this.CurrencyType == ShopItemController.ShopCurrencyType.BLANKS)
					{
						player.carriedConsumables.Currency -= this.ModifiedPrice;
					}
					else if (this.CurrencyType == ShopItemController.ShopCurrencyType.KEYS)
					{
						player.carriedConsumables.KeyBullets -= this.ModifiedPrice;
					}
				}
				if (this.m_parentShop != null)
				{
					this.m_parentShop.PurchaseItem(this, !flag, true);
				}
				if (this.m_baseParentShop != null)
				{
					this.m_baseParentShop.PurchaseItem(this, !flag, true);
				}
				if (flag)
				{
					StatModifier statModifier = new StatModifier();
					statModifier.statToBoost = PlayerStats.StatType.Curse;
					statModifier.amount = 1f;
					statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
					player.ownerlessStatModifiers.Add(statModifier);
					player.stats.RecalculateStats(player, false, false);
					player.HandleItemStolen(this);
					this.m_baseParentShop.NotifyStealSucceeded();
					player.IsThief = true;
					GameStatsManager.Instance.RegisterStatChange(TrackedStats.MERCHANT_ITEMS_STOLEN, 1f);
					if (this.SetsFlagOnSteal)
					{
						GameStatsManager.Instance.SetFlag(this.FlagToSetOnSteal, true);
					}
				}
				else
				{
					if (this.CurrencyType == ShopItemController.ShopCurrencyType.BLANKS)
					{
						player.Blanks++;
					}
					player.HandleItemPurchased(this);
				}
				if (!this.item.PersistsOnPurchase)
				{
					GameUIRoot.Instance.DeregisterDefaultLabel(base.transform);
				}
				AkSoundEngine.PostEvent("Play_OBJ_item_purchase_01", base.gameObject);
			}
		}
		else if (this.CurrencyType == ShopItemController.ShopCurrencyType.META_CURRENCY)
		{
			int num2 = Mathf.RoundToInt(GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.META_CURRENCY));
			if (num2 < this.ModifiedPrice)
			{
				AkSoundEngine.PostEvent("Play_OBJ_purchase_unable_01", base.gameObject);
				if (this.m_parentShop != null)
				{
					this.m_parentShop.NotifyFailedPurchase(this);
				}
				if (this.m_baseParentShop != null)
				{
					this.m_baseParentShop.NotifyFailedPurchase(this);
				}
				return;
			}
			if (!this.pickedUp)
			{
				this.pickedUp = !this.item.PersistsOnPurchase;
				GameStatsManager.Instance.ClearStatValueGlobal(TrackedStats.META_CURRENCY);
				GameStatsManager.Instance.SetStat(TrackedStats.META_CURRENCY, (float)(num2 - this.ModifiedPrice));
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.META_CURRENCY_SPENT_AT_META_SHOP, (float)this.ModifiedPrice);
				LootEngine.GivePrefabToPlayer(this.item.gameObject, player);
				if (this.m_parentShop != null)
				{
					this.m_parentShop.PurchaseItem(this, true, true);
				}
				if (this.m_baseParentShop != null)
				{
					this.m_baseParentShop.PurchaseItem(this, true, true);
				}
				player.HandleItemPurchased(this);
				if (!this.item.PersistsOnPurchase)
				{
					GameUIRoot.Instance.DeregisterDefaultLabel(base.transform);
				}
				AkSoundEngine.PostEvent("Play_OBJ_item_purchase_01", base.gameObject);
			}
		}
	}

	// Token: 0x06007D39 RID: 32057 RVA: 0x00329960 File Offset: 0x00327B60
	public void ForceSteal(PlayerController player)
	{
		this.pickedUp = true;
		LootEngine.GivePrefabToPlayer(this.item.gameObject, player);
		if (this.m_parentShop != null)
		{
			this.m_parentShop.PurchaseItem(this, false, false);
		}
		if (this.m_baseParentShop != null)
		{
			this.m_baseParentShop.PurchaseItem(this, false, false);
		}
		StatModifier statModifier = new StatModifier();
		statModifier.statToBoost = PlayerStats.StatType.Curse;
		statModifier.amount = 1f;
		statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
		player.ownerlessStatModifiers.Add(statModifier);
		player.stats.RecalculateStats(player, false, false);
		player.HandleItemStolen(this);
		this.m_baseParentShop.NotifyStealSucceeded();
		player.IsThief = true;
		GameStatsManager.Instance.RegisterStatChange(TrackedStats.MERCHANT_ITEMS_STOLEN, 1f);
		if (!this.m_baseParentShop.AttemptToSteal())
		{
			player.DidUnstealthyAction();
			this.m_baseParentShop.NotifyStealFailed();
		}
		GameUIRoot.Instance.DeregisterDefaultLabel(base.transform);
		AkSoundEngine.PostEvent("Play_OBJ_item_purchase_01", base.gameObject);
	}

	// Token: 0x06007D3A RID: 32058 RVA: 0x00329A6C File Offset: 0x00327C6C
	public void ForceOutOfStock()
	{
		this.pickedUp = true;
		if (this.m_parentShop != null)
		{
			this.m_parentShop.PurchaseItem(this, false, true);
		}
		if (this.m_baseParentShop != null)
		{
			this.m_baseParentShop.PurchaseItem(this, false, true);
		}
		GameUIRoot.Instance.DeregisterDefaultLabel(base.transform);
	}

	// Token: 0x06007D3B RID: 32059 RVA: 0x00329AD0 File Offset: 0x00327CD0
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x04008032 RID: 32818
	public PickupObject item;

	// Token: 0x04008033 RID: 32819
	public bool UseOmnidirectionalItemFacing;

	// Token: 0x04008034 RID: 32820
	public DungeonData.Direction itemFacing = DungeonData.Direction.SOUTH;

	// Token: 0x04008035 RID: 32821
	[NonSerialized]
	public PlayerController LastInteractingPlayer;

	// Token: 0x04008036 RID: 32822
	public ShopItemController.ShopCurrencyType CurrencyType;

	// Token: 0x04008038 RID: 32824
	public bool PrecludeAllDiscounts;

	// Token: 0x04008039 RID: 32825
	public int CurrentPrice = -1;

	// Token: 0x0400803A RID: 32826
	[NonSerialized]
	public int? OverridePrice;

	// Token: 0x0400803B RID: 32827
	[NonSerialized]
	public bool SetsFlagOnSteal;

	// Token: 0x0400803C RID: 32828
	[NonSerialized]
	public GungeonFlags FlagToSetOnSteal;

	// Token: 0x0400803D RID: 32829
	[NonSerialized]
	public bool IsResourcefulRatKey;

	// Token: 0x0400803E RID: 32830
	private bool pickedUp;

	// Token: 0x0400803F RID: 32831
	private ShopController m_parentShop;

	// Token: 0x04008040 RID: 32832
	private BaseShopController m_baseParentShop;

	// Token: 0x04008041 RID: 32833
	private float THRESHOLD_CUTOFF_PRIMARY = 3f;

	// Token: 0x04008042 RID: 32834
	private float THRESHOLD_CUTOFF_SECONDARY = 2f;

	// Token: 0x04008043 RID: 32835
	[NonSerialized]
	private GameObject m_shadowObject;

	// Token: 0x02001558 RID: 5464
	public enum ShopCurrencyType
	{
		// Token: 0x04008045 RID: 32837
		COINS,
		// Token: 0x04008046 RID: 32838
		META_CURRENCY,
		// Token: 0x04008047 RID: 32839
		KEYS,
		// Token: 0x04008048 RID: 32840
		BLANKS
	}
}
