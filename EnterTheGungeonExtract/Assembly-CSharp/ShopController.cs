using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001556 RID: 5462
public class ShopController : DungeonPlaceableBehaviour, IPlaceConfigurable
{
	// Token: 0x17001271 RID: 4721
	// (get) Token: 0x06007D18 RID: 32024 RVA: 0x00327890 File Offset: 0x00325A90
	public RoomHandler Room
	{
		get
		{
			return this.m_room;
		}
	}

	// Token: 0x06007D19 RID: 32025 RVA: 0x00327898 File Offset: 0x00325A98
	protected virtual void Start()
	{
		this.DoSetup();
	}

	// Token: 0x06007D1A RID: 32026 RVA: 0x003278A0 File Offset: 0x00325AA0
	public virtual void OnRoomEnter(PlayerController p)
	{
		if (p.IsStealthed)
		{
			return;
		}
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

	// Token: 0x06007D1B RID: 32027 RVA: 0x003278D4 File Offset: 0x00325AD4
	public virtual void OnRoomExit()
	{
		if (GameManager.Instance.PrimaryPlayer && GameManager.Instance.PrimaryPlayer.IsStealthed)
		{
			return;
		}
		this.GunsmithTalk(StringTableManager.GetString("#SHOP_EXIT"));
	}

	// Token: 0x06007D1C RID: 32028 RVA: 0x00327910 File Offset: 0x00325B10
	protected virtual void OnInitialRoomEnter()
	{
		this.GunsmithTalk(StringTableManager.GetString("#SHOP_ENTER"));
	}

	// Token: 0x06007D1D RID: 32029 RVA: 0x00327924 File Offset: 0x00325B24
	protected virtual void OnSequentialRoomEnter()
	{
		this.GunsmithTalk(StringTableManager.GetString("#SHOP_REENTER"));
	}

	// Token: 0x06007D1E RID: 32030 RVA: 0x00327938 File Offset: 0x00325B38
	protected virtual void GunsmithTalk(string message)
	{
		TextBoxManager.ShowTextBox(this.speechPoint.position, this.speechPoint, 5f, message, "shopkeep", false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
		this.speakerAnimator.PlayForDuration(this.defaultTalkAction, 2.5f, this.defaultIdleAction, false);
	}

	// Token: 0x06007D1F RID: 32031 RVA: 0x00327988 File Offset: 0x00325B88
	public virtual void OnBetrayalWarning()
	{
		this.speakerAnimator.PlayForDuration("scold", 1f, this.defaultIdleAction, false);
		TextBoxManager.ShowTextBox(this.speechPoint.position, this.speechPoint, 5f, StringTableManager.GetString("#SHOPKEEP_BETRAYAL_WARNING"), "shopkeep", false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
	}

	// Token: 0x06007D20 RID: 32032 RVA: 0x003279E0 File Offset: 0x00325BE0
	public virtual void PullGun()
	{
		this.speakerAnimator.Play("gun");
		this.defaultIdleAction = "gun_idle";
		this.defaultTalkAction = "gun_talk";
		TextBoxManager.ShowTextBox(this.speechPoint.position, this.speechPoint, 5f, StringTableManager.GetString("#SHOPKEEP_ANGRYTOWN"), "shopkeep", false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
		for (int i = 0; i < this.m_itemControllers.Count; i++)
		{
			this.m_itemControllers[i].CurrentPrice *= 2;
		}
		TalkDoer component = this.speakerAnimator.GetComponent<TalkDoer>();
		component.modules[0].stringKeys[0] = "#SHOPKEEP_ANGRY_CHAT";
		component.modules[0].usesAnimation = true;
		component.modules[0].animationDuration = 2.5f;
		component.modules[0].animationName = "gun_talk";
		component.defaultSpeechAnimName = "gun_talk";
		component.fallbackAnimName = "gun_idle";
	}

	// Token: 0x06007D21 RID: 32033 RVA: 0x00327AF0 File Offset: 0x00325CF0
	public virtual void NotifyFailedPurchase(ShopItemController itemController)
	{
		TextBoxManager.ShowTextBox(this.speechPoint.position, this.speechPoint, 5f, StringTableManager.GetString("#SHOP_NOMONEY"), "shopkeep", false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
		if (this.defaultIdleAction == "idle")
		{
			this.speakerAnimator.PlayForDuration("shake", 2.5f, this.defaultIdleAction, false);
		}
		else
		{
			this.speakerAnimator.PlayForDuration("scold", 1f, this.defaultIdleAction, false);
		}
	}

	// Token: 0x06007D22 RID: 32034 RVA: 0x00327B80 File Offset: 0x00325D80
	public virtual void ReturnToIdle(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip)
	{
		animator.Play(this.defaultIdleAction);
		animator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(animator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.ReturnToIdle));
	}

	// Token: 0x06007D23 RID: 32035 RVA: 0x00327BB4 File Offset: 0x00325DB4
	public virtual void PurchaseItem(ShopItemController item, bool actualPurchase = true, bool allowSign = true)
	{
		if (actualPurchase)
		{
			if (this.defaultIdleAction == "gun_idle")
			{
				this.GunsmithTalk(StringTableManager.GetString("#SHOPKEEP_PURCHASE_ANGRY"));
			}
			else
			{
				this.GunsmithTalk(StringTableManager.GetString("#SHOP_PURCHASE"));
				this.speakerAnimator.PlayForDuration("nod", 1.5f, this.defaultIdleAction, false);
			}
		}
		if (!item.item.PersistsOnPurchase)
		{
			this.m_room.DeregisterInteractable(item);
			if (allowSign)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Global Prefabs/Sign_SoldOut", ".prefab"));
				gameObject.GetComponent<tk2dBaseSprite>().PlaceAtPositionByAnchor(item.sprite.WorldCenter, tk2dBaseSprite.Anchor.MiddleCenter);
				gameObject.transform.position = gameObject.transform.position.Quantize(0.0625f);
			}
		}
		UnityEngine.Object.Destroy(item.gameObject);
	}

	// Token: 0x06007D24 RID: 32036 RVA: 0x00327CA0 File Offset: 0x00325EA0
	public virtual void ConfigureOnPlacement(RoomHandler room)
	{
		room.IsShop = true;
		this.m_room = room;
	}

	// Token: 0x06007D25 RID: 32037 RVA: 0x00327CB0 File Offset: 0x00325EB0
	protected virtual void DoSetup()
	{
		this.m_shopItems = new List<GameObject>();
		this.m_room.Entered += this.OnRoomEnter;
		this.m_room.Exited += this.OnRoomExit;
		Func<GameObject, float, float> func = null;
		if (SecretHandshakeItem.NumActive > 0)
		{
			func = delegate(GameObject prefabObject, float sourceWeight)
			{
				PickupObject component6 = prefabObject.GetComponent<PickupObject>();
				float num2 = sourceWeight;
				if (component6 != null)
				{
					int quality = (int)component6.quality;
					num2 *= 1f + (float)quality / 10f;
				}
				return num2;
			};
		}
		for (int i = 0; i < this.spawnPositions.Length; i++)
		{
			GameObject gameObject = this.shopItems.SelectByWeightWithoutDuplicatesFullPrereqs(this.m_shopItems, func, false);
			this.m_shopItems.Add(gameObject);
		}
		this.m_itemControllers = new List<ShopItemController>();
		for (int j = 0; j < this.spawnPositions.Length; j++)
		{
			Transform transform = this.spawnPositions[j];
			if (!(this.m_shopItems[j] == null))
			{
				PickupObject component = this.m_shopItems[j].GetComponent<PickupObject>();
				if (!(component == null))
				{
					GameObject gameObject2 = new GameObject("Shop item " + j.ToString());
					Transform transform2 = gameObject2.transform;
					transform2.parent = transform;
					transform2.localPosition = Vector3.zero;
					EncounterTrackable component2 = component.GetComponent<EncounterTrackable>();
					if (component2 != null)
					{
						GameManager.Instance.ExtantShopTrackableGuids.Add(component2.EncounterGuid);
					}
					ShopItemController shopItemController = gameObject2.AddComponent<ShopItemController>();
					if (transform.name.Contains("SIDE") || transform.name.Contains("EAST"))
					{
						shopItemController.itemFacing = DungeonData.Direction.EAST;
					}
					else if (transform.name.Contains("WEST"))
					{
						shopItemController.itemFacing = DungeonData.Direction.WEST;
					}
					else if (transform.name.Contains("NORTH"))
					{
						shopItemController.itemFacing = DungeonData.Direction.NORTH;
					}
					if (!this.m_room.IsRegistered(shopItemController))
					{
						this.m_room.RegisterInteractable(shopItemController);
					}
					shopItemController.Initialize(component, this);
					this.m_itemControllers.Add(shopItemController);
				}
			}
		}
		if (this.shopItemsGroup2 != null && this.spawnPositionsGroup2.Length > 0)
		{
			int count = this.m_shopItems.Count;
			for (int k = 0; k < this.spawnPositionsGroup2.Length; k++)
			{
				float num = 1f - (float)k * 0.25f;
				if (UnityEngine.Random.value < num)
				{
					GameObject rewardObjectShopStyle = GameManager.Instance.RewardManager.GetRewardObjectShopStyle(GameManager.Instance.PrimaryPlayer, false, false, this.m_shopItems);
					this.m_shopItems.Add(rewardObjectShopStyle);
				}
				else
				{
					this.m_shopItems.Add(null);
				}
			}
			for (int l = 0; l < this.spawnPositionsGroup2.Length; l++)
			{
				Transform transform3 = this.spawnPositionsGroup2[l];
				if (!(this.m_shopItems[count + l] == null))
				{
					PickupObject component3 = this.m_shopItems[count + l].GetComponent<PickupObject>();
					if (!(component3 == null))
					{
						GameObject gameObject3 = new GameObject("Shop 2 item " + l.ToString());
						Transform transform4 = gameObject3.transform;
						transform4.parent = transform3;
						transform4.localPosition = Vector3.zero;
						EncounterTrackable component4 = component3.GetComponent<EncounterTrackable>();
						if (component4 != null)
						{
							GameManager.Instance.ExtantShopTrackableGuids.Add(component4.EncounterGuid);
						}
						ShopItemController shopItemController2 = gameObject3.AddComponent<ShopItemController>();
						if (transform3.name.Contains("SIDE") || transform3.name.Contains("EAST"))
						{
							shopItemController2.itemFacing = DungeonData.Direction.EAST;
						}
						else if (transform3.name.Contains("WEST"))
						{
							shopItemController2.itemFacing = DungeonData.Direction.WEST;
						}
						else if (transform3.name.Contains("NORTH"))
						{
							shopItemController2.itemFacing = DungeonData.Direction.NORTH;
						}
						if (!this.m_room.IsRegistered(shopItemController2))
						{
							this.m_room.RegisterInteractable(shopItemController2);
						}
						shopItemController2.Initialize(component3, this);
						this.m_itemControllers.Add(shopItemController2);
					}
				}
			}
		}
		List<ShopSubsidiaryZone> componentsInRoom = this.m_room.GetComponentsInRoom<ShopSubsidiaryZone>();
		for (int m = 0; m < componentsInRoom.Count; m++)
		{
			componentsInRoom[m].HandleSetup(this, this.m_room, this.m_shopItems, this.m_itemControllers);
		}
		TalkDoer component5 = this.speakerAnimator.GetComponent<TalkDoer>();
		if (component5 != null)
		{
			component5.usesCustomBetrayalLogic = true;
			TalkDoer talkDoer = component5;
			talkDoer.OnBetrayalWarning = (Action)Delegate.Combine(talkDoer.OnBetrayalWarning, new Action(this.OnBetrayalWarning));
			TalkDoer talkDoer2 = component5;
			talkDoer2.OnBetrayal = (Action)Delegate.Combine(talkDoer2.OnBetrayal, new Action(this.PullGun));
		}
	}

	// Token: 0x06007D26 RID: 32038 RVA: 0x003281DC File Offset: 0x003263DC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04008022 RID: 32802
	[Header("Spawn Group 1")]
	public GenericLootTable shopItems;

	// Token: 0x04008023 RID: 32803
	public Transform[] spawnPositions;

	// Token: 0x04008024 RID: 32804
	[Header("Spawn Group 2")]
	public GenericLootTable shopItemsGroup2;

	// Token: 0x04008025 RID: 32805
	public Transform[] spawnPositionsGroup2;

	// Token: 0x04008026 RID: 32806
	[Header("Other Settings")]
	public tk2dSpriteAnimator speakerAnimator;

	// Token: 0x04008027 RID: 32807
	public Transform speechPoint;

	// Token: 0x04008028 RID: 32808
	public float ItemHeightOffGroundModifier;

	// Token: 0x04008029 RID: 32809
	public GameObject shopItemShadowPrefab;

	// Token: 0x0400802A RID: 32810
	protected List<GameObject> m_shopItems;

	// Token: 0x0400802B RID: 32811
	protected List<ShopItemController> m_itemControllers;

	// Token: 0x0400802C RID: 32812
	protected bool firstTime = true;

	// Token: 0x0400802D RID: 32813
	[NonSerialized]
	public int StolenCount;

	// Token: 0x0400802E RID: 32814
	protected string defaultTalkAction = "talk";

	// Token: 0x0400802F RID: 32815
	protected string defaultIdleAction = "idle";

	// Token: 0x04008030 RID: 32816
	protected RoomHandler m_room;
}
