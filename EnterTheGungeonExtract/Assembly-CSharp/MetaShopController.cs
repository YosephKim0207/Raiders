using System;
using System.Collections.Generic;
using Dungeonator;
using HutongGames.PlayMaker;
using UnityEngine;

// Token: 0x02001552 RID: 5458
public class MetaShopController : ShopController, IPlaceConfigurable
{
	// Token: 0x06007CEF RID: 31983 RVA: 0x00325FE8 File Offset: 0x003241E8
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x06007CF0 RID: 31984 RVA: 0x00325FF0 File Offset: 0x003241F0
	public override void OnRoomEnter(PlayerController p)
	{
		if (this.firstTime)
		{
			this.firstTime = false;
			this.OnInitialRoomEnter();
		}
		else
		{
			this.OnSequentialRoomEnter();
		}
	}

	// Token: 0x06007CF1 RID: 31985 RVA: 0x00326018 File Offset: 0x00324218
	public override void OnRoomExit()
	{
	}

	// Token: 0x06007CF2 RID: 31986 RVA: 0x0032601C File Offset: 0x0032421C
	protected override void OnInitialRoomEnter()
	{
		float num = UnityEngine.Random.value;
		if (!GameStatsManager.Instance.GetFlag(GungeonFlags.META_SHOP_RECEIVED_ROBOT_ARM_REWARD) && GameStatsManager.Instance.CurrentRobotArmFloor == 0)
		{
			num = this.ChanceToBeAsleep - 0.01f;
		}
		if (!GameStatsManager.Instance.GetFlag(GungeonFlags.META_SHOP_EVER_TALKED))
		{
			num = this.ChanceToBeAsleep - 0.01f;
		}
		string text = ((!GameStatsManager.Instance.GetFlag(GungeonFlags.META_SHOP_RECEIVED_ROBOT_ARM_REWARD)) ? "idle" : "idle_hand");
		if (num < this.ChanceToBeAsleep)
		{
			if (num < this.ChanceToBeAsleep / 2f)
			{
				this.Witchkeeper.transform.position = this.WitchChairPoint.position;
				this.Witchkeeper.specRigidbody.Reinitialize();
				this.Witchkeeper.SendPlaymakerEvent("SetChairMode");
				text += "_mask";
			}
			else
			{
				this.Witchkeeper.transform.position = this.WitchSleepPoint.position;
				this.Witchkeeper.specRigidbody.Reinitialize();
				this.Witchkeeper.SendPlaymakerEvent("SetShopMode");
			}
		}
		else
		{
			this.Witchkeeper.transform.position = this.WitchStandPoint.position;
			this.Witchkeeper.specRigidbody.Reinitialize();
			this.Witchkeeper.SendPlaymakerEvent("SetStandMode");
		}
		FsmString fsmString = this.speakerAnimator.playmakerFsm.FsmVariables.GetFsmString("idleAnim");
		fsmString.Value = text;
		this.speakerAnimator.Play(fsmString.Value);
	}

	// Token: 0x06007CF3 RID: 31987 RVA: 0x003261BC File Offset: 0x003243BC
	protected override void OnSequentialRoomEnter()
	{
	}

	// Token: 0x06007CF4 RID: 31988 RVA: 0x003261C0 File Offset: 0x003243C0
	protected override void GunsmithTalk(string message)
	{
		TextBoxManager.ShowTextBox(this.speechPoint.position, this.speechPoint, 5f, message, "shopkeep", false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
		this.speakerAnimator.aiAnimator.PlayForDuration(this.defaultTalkAction, 2.5f, false, null, -1f, false);
	}

	// Token: 0x06007CF5 RID: 31989 RVA: 0x00326218 File Offset: 0x00324418
	public override void OnBetrayalWarning()
	{
	}

	// Token: 0x06007CF6 RID: 31990 RVA: 0x0032621C File Offset: 0x0032441C
	public override void PullGun()
	{
	}

	// Token: 0x06007CF7 RID: 31991 RVA: 0x00326220 File Offset: 0x00324420
	public override void NotifyFailedPurchase(ShopItemController itemController)
	{
		if (UnityEngine.Random.value < 0.75f)
		{
			this.speakerAnimator.SendPlaymakerEvent("playerHasNoMoney");
		}
		else
		{
			this.Witchkeeper.SendPlaymakerEvent("playerHasNoMoney");
		}
	}

	// Token: 0x06007CF8 RID: 31992 RVA: 0x00326258 File Offset: 0x00324458
	public override void ReturnToIdle(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
	{
		animator.Play(this.defaultIdleAction);
		animator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(animator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.ReturnToIdle));
	}

	// Token: 0x06007CF9 RID: 31993 RVA: 0x0032628C File Offset: 0x0032448C
	public override void PurchaseItem(ShopItemController item, bool actualPurchase = true, bool allowSign = true)
	{
		if (actualPurchase && !GameManager.Instance.IsSelectingCharacter)
		{
			if (UnityEngine.Random.value < 0.75f)
			{
				this.speakerAnimator.SendPlaymakerEvent("playerPaid");
			}
			else
			{
				this.Witchkeeper.SendPlaymakerEvent("playerPaid");
			}
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.MERCHANT_PURCHASES_META, 1f);
			GameStatsManager.Instance.RegisterStatChange(TrackedStats.MONEY_SPENT_AT_CURSE_SHOP, (float)item.ModifiedPrice);
		}
		if (!item.item.PersistsOnPurchase)
		{
			this.m_room.DeregisterInteractable(item);
			if (allowSign)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Global Prefabs/Sign_SoldOut", ".prefab"));
				gameObject.GetComponent<tk2dBaseSprite>().PlaceAtPositionByAnchor(item.sprite.WorldCenter, tk2dBaseSprite.Anchor.MiddleCenter);
				gameObject.transform.position = gameObject.transform.position.Quantize(0.0625f);
				GameObject gameObject2 = null;
				if (this.shopItemShadowPrefab != null)
				{
					gameObject2 = this.shopItemShadowPrefab;
				}
				tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
				if (gameObject2 != null)
				{
					tk2dBaseSprite component2 = UnityEngine.Object.Instantiate<GameObject>(gameObject2).GetComponent<tk2dBaseSprite>();
					component.AttachRenderer(component2);
					component2.transform.parent = component.transform;
					component2.transform.localPosition = new Vector3(0f, 0.0625f, 0f);
					component2.HeightOffGround = -0.0625f;
				}
				gameObject.GetComponent<tk2dBaseSprite>().UpdateZDepth();
			}
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}

	// Token: 0x06007CFA RID: 31994 RVA: 0x00326410 File Offset: 0x00324610
	public override void ConfigureOnPlacement(RoomHandler room)
	{
		base.ConfigureOnPlacement(room);
	}

	// Token: 0x06007CFB RID: 31995 RVA: 0x0032641C File Offset: 0x0032461C
	protected MetaShopTier GetCurrentTier()
	{
		for (int i = 0; i < this.metaShopTiers.Count; i++)
		{
			if (!GameStatsManager.Instance.GetFlag(this.GetFlagFromTargetItem(this.metaShopTiers[i].itemId1)) || !GameStatsManager.Instance.GetFlag(this.GetFlagFromTargetItem(this.metaShopTiers[i].itemId2)) || !GameStatsManager.Instance.GetFlag(this.GetFlagFromTargetItem(this.metaShopTiers[i].itemId3)))
			{
				return this.metaShopTiers[i];
			}
		}
		return this.metaShopTiers[this.metaShopTiers.Count - 1];
	}

	// Token: 0x06007CFC RID: 31996 RVA: 0x003264DC File Offset: 0x003246DC
	protected MetaShopTier GetProximateTier()
	{
		for (int i = 0; i < this.metaShopTiers.Count - 1; i++)
		{
			if (!GameStatsManager.Instance.GetFlag(this.GetFlagFromTargetItem(this.metaShopTiers[i].itemId1)) || !GameStatsManager.Instance.GetFlag(this.GetFlagFromTargetItem(this.metaShopTiers[i].itemId2)) || !GameStatsManager.Instance.GetFlag(this.GetFlagFromTargetItem(this.metaShopTiers[i].itemId3)))
			{
				return this.metaShopTiers[i + 1];
			}
		}
		return null;
	}

	// Token: 0x06007CFD RID: 31997 RVA: 0x0032658C File Offset: 0x0032478C
	protected GungeonFlags GetFlagFromTargetItem(int shopItemId)
	{
		GungeonFlags gungeonFlags = GungeonFlags.NONE;
		PickupObject byId = PickupObjectDatabase.GetById(shopItemId);
		for (int i = 0; i < byId.encounterTrackable.prerequisites.Length; i++)
		{
			if (byId.encounterTrackable.prerequisites[i].prerequisiteType == DungeonPrerequisite.PrerequisiteType.FLAG)
			{
				gungeonFlags = byId.encounterTrackable.prerequisites[i].saveFlagToCheck;
			}
		}
		return gungeonFlags;
	}

	// Token: 0x06007CFE RID: 31998 RVA: 0x003265EC File Offset: 0x003247EC
	protected override void DoSetup()
	{
		this.m_shopItems = new List<GameObject>();
		this.m_room.Entered += this.OnRoomEnter;
		this.m_room.Exited += this.OnRoomExit;
		MetaShopTier currentTier = this.GetCurrentTier();
		MetaShopTier proximateTier = this.GetProximateTier();
		this.m_itemControllers = new List<ShopItemController>();
		int num = 0;
		if (currentTier != null)
		{
			if (currentTier.itemId1 >= 0)
			{
				this.m_shopItems.Add(PickupObjectDatabase.GetById(currentTier.itemId1).gameObject);
			}
			if (currentTier.itemId2 >= 0)
			{
				this.m_shopItems.Add(PickupObjectDatabase.GetById(currentTier.itemId2).gameObject);
			}
			if (currentTier.itemId3 >= 0)
			{
				this.m_shopItems.Add(PickupObjectDatabase.GetById(currentTier.itemId3).gameObject);
			}
			for (int i = 0; i < this.spawnPositions.Length; i++)
			{
				Transform transform = this.spawnPositions[i];
				if (i < this.m_shopItems.Count && !(this.m_shopItems[i] == null))
				{
					PickupObject component = this.m_shopItems[i].GetComponent<PickupObject>();
					GameObject gameObject = this.ExampleBlueprintPrefab;
					if (!(component is Gun))
					{
						gameObject = this.ExampleBlueprintPrefabItem;
					}
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, new Vector3(150f, -50f, -100f), Quaternion.identity);
					ItemBlueprintItem component2 = gameObject2.GetComponent<ItemBlueprintItem>();
					EncounterTrackable component3 = gameObject2.GetComponent<EncounterTrackable>();
					component3.journalData.PrimaryDisplayName = component.encounterTrackable.journalData.PrimaryDisplayName;
					component3.journalData.NotificationPanelDescription = component.encounterTrackable.journalData.NotificationPanelDescription;
					component3.journalData.AmmonomiconFullEntry = component.encounterTrackable.journalData.AmmonomiconFullEntry;
					component3.journalData.AmmonomiconSprite = component.encounterTrackable.journalData.AmmonomiconSprite;
					component3.DoNotificationOnEncounter = false;
					component2.UsesCustomCost = true;
					int num2 = this.metaShopTiers.IndexOf(currentTier) + 1;
					if (currentTier.overrideTierCost > 0)
					{
						num2 = currentTier.overrideTierCost;
					}
					if (i == 0 && currentTier.overrideItem1Cost > 0)
					{
						num2 = currentTier.overrideItem1Cost;
					}
					if (i == 1 && currentTier.overrideItem2Cost > 0)
					{
						num2 = currentTier.overrideItem2Cost;
					}
					if (i == 2 && currentTier.overrideItem3Cost > 0)
					{
						num2 = currentTier.overrideItem3Cost;
					}
					component2.CustomCost = num2;
					GungeonFlags flagFromTargetItem = this.GetFlagFromTargetItem(component.PickupObjectId);
					component2.SaveFlagToSetOnAcquisition = flagFromTargetItem;
					component2.HologramIconSpriteName = component3.journalData.AmmonomiconSprite;
					this.HandleItemPlacement(transform, component2);
					gameObject2.SetActive(false);
					if (GameStatsManager.Instance.GetFlag(component2.SaveFlagToSetOnAcquisition))
					{
						this.m_itemControllers[i].ForceOutOfStock();
					}
					else
					{
						num++;
					}
				}
			}
		}
		if (proximateTier != null)
		{
			this.m_shopItems.Clear();
			if (proximateTier.itemId1 >= 0)
			{
				this.m_shopItems.Add(PickupObjectDatabase.GetById(proximateTier.itemId1).gameObject);
			}
			if (proximateTier.itemId2 >= 0)
			{
				this.m_shopItems.Add(PickupObjectDatabase.GetById(proximateTier.itemId2).gameObject);
			}
			if (proximateTier.itemId3 >= 0)
			{
				this.m_shopItems.Add(PickupObjectDatabase.GetById(proximateTier.itemId3).gameObject);
			}
			for (int j = 0; j < this.spawnPositionsGroup2.Length; j++)
			{
				Transform transform2 = this.spawnPositionsGroup2[j];
				if (j < this.m_shopItems.Count && !(this.m_shopItems[j] == null))
				{
					PickupObject component4 = this.m_shopItems[j].GetComponent<PickupObject>();
					GameObject gameObject3 = this.ExampleBlueprintPrefab;
					if (!(component4 is Gun))
					{
						gameObject3 = this.ExampleBlueprintPrefabItem;
					}
					GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(gameObject3, new Vector3(150f, -50f, -100f), Quaternion.identity);
					ItemBlueprintItem component5 = gameObject4.GetComponent<ItemBlueprintItem>();
					EncounterTrackable component6 = gameObject4.GetComponent<EncounterTrackable>();
					component6.journalData.PrimaryDisplayName = component4.encounterTrackable.journalData.PrimaryDisplayName;
					component6.journalData.NotificationPanelDescription = component4.encounterTrackable.journalData.NotificationPanelDescription;
					component6.journalData.AmmonomiconFullEntry = component4.encounterTrackable.journalData.AmmonomiconFullEntry;
					component6.journalData.AmmonomiconSprite = component4.encounterTrackable.journalData.AmmonomiconSprite;
					component6.DoNotificationOnEncounter = false;
					component5.UsesCustomCost = true;
					int num3 = this.metaShopTiers.IndexOf(proximateTier) + 1;
					if (proximateTier.overrideTierCost > 0)
					{
						num3 = proximateTier.overrideTierCost;
					}
					if (j == 0 && proximateTier.overrideItem1Cost > 0)
					{
						num3 = proximateTier.overrideItem1Cost;
					}
					if (j == 1 && proximateTier.overrideItem2Cost > 0)
					{
						num3 = proximateTier.overrideItem2Cost;
					}
					if (j == 2 && proximateTier.overrideItem3Cost > 0)
					{
						num3 = proximateTier.overrideItem3Cost;
					}
					component5.CustomCost = num3;
					GungeonFlags flagFromTargetItem2 = this.GetFlagFromTargetItem(component4.PickupObjectId);
					component5.SaveFlagToSetOnAcquisition = flagFromTargetItem2;
					component5.HologramIconSpriteName = component6.journalData.AmmonomiconSprite;
					this.HandleItemPlacement(transform2, component5);
					gameObject4.SetActive(false);
					if (GameStatsManager.Instance.GetFlag(component5.SaveFlagToSetOnAcquisition))
					{
						this.m_itemControllers[this.m_itemControllers.Count - 1].ForceOutOfStock();
					}
					else
					{
						num++;
					}
				}
			}
		}
		if (GameManager.Instance.platformInterface != null && num == 0 && proximateTier == null)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.SPEND_META_CURRENCY, 0);
		}
		for (int k = 0; k < this.m_itemControllers.Count; k++)
		{
			this.m_itemControllers[k].sprite.IsPerpendicular = true;
			this.m_itemControllers[k].sprite.UpdateZDepth();
			this.m_itemControllers[k].CurrencyType = ShopItemController.ShopCurrencyType.META_CURRENCY;
		}
	}

	// Token: 0x06007CFF RID: 31999 RVA: 0x00326C38 File Offset: 0x00324E38
	private void HandleItemPlacement(Transform spawnTransform, PickupObject shopItem)
	{
		GameObject gameObject = new GameObject("Shop item " + Array.IndexOf<Transform>(this.spawnPositions, spawnTransform).ToString());
		Transform transform = gameObject.transform;
		transform.parent = spawnTransform;
		transform.localPosition = Vector3.zero;
		EncounterTrackable component = shopItem.GetComponent<EncounterTrackable>();
		if (component != null)
		{
			GameManager.Instance.ExtantShopTrackableGuids.Add(component.EncounterGuid);
		}
		ShopItemController shopItemController = gameObject.AddComponent<ShopItemController>();
		if (spawnTransform.name.Contains("SIDE") || spawnTransform.name.Contains("EAST"))
		{
			shopItemController.itemFacing = DungeonData.Direction.EAST;
		}
		else if (spawnTransform.name.Contains("WEST"))
		{
			shopItemController.itemFacing = DungeonData.Direction.WEST;
		}
		else if (spawnTransform.name.Contains("NORTH"))
		{
			shopItemController.itemFacing = DungeonData.Direction.NORTH;
		}
		if (!this.m_room.IsRegistered(shopItemController))
		{
			this.m_room.RegisterInteractable(shopItemController);
		}
		shopItemController.Initialize(shopItem, this);
		transform.localPosition += new Vector3(0.0625f, 0f, 0f);
		this.m_itemControllers.Add(shopItemController);
	}

	// Token: 0x06007D00 RID: 32000 RVA: 0x00326D88 File Offset: 0x00324F88
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007FFE RID: 32766
	public TalkDoerLite Witchkeeper;

	// Token: 0x04007FFF RID: 32767
	public Transform WitchStandPoint;

	// Token: 0x04008000 RID: 32768
	public Transform WitchSleepPoint;

	// Token: 0x04008001 RID: 32769
	public Transform WitchChairPoint;

	// Token: 0x04008002 RID: 32770
	public float ChanceToBeAsleep = 0.5f;

	// Token: 0x04008003 RID: 32771
	public HologramDoer Hologramophone;

	// Token: 0x04008004 RID: 32772
	public GameObject ExampleBlueprintPrefab;

	// Token: 0x04008005 RID: 32773
	public GameObject ExampleBlueprintPrefabItem;

	// Token: 0x04008006 RID: 32774
	[SerializeField]
	public List<MetaShopTier> metaShopTiers;
}
