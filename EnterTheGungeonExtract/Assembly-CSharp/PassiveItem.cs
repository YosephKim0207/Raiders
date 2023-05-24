using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200145B RID: 5211
public class PassiveItem : PickupObject, IPlayerInteractable
{
	// Token: 0x06007652 RID: 30290 RVA: 0x002F1640 File Offset: 0x002EF840
	public static void IncrementFlag(PlayerController player, Type flagType)
	{
		if (!PassiveItem.ActiveFlagItems.ContainsKey(player))
		{
			PassiveItem.ActiveFlagItems.Add(player, new Dictionary<Type, int>());
		}
		if (!PassiveItem.ActiveFlagItems[player].ContainsKey(flagType))
		{
			PassiveItem.ActiveFlagItems[player].Add(flagType, 1);
		}
		else
		{
			PassiveItem.ActiveFlagItems[player][flagType] = PassiveItem.ActiveFlagItems[player][flagType] + 1;
		}
	}

	// Token: 0x06007653 RID: 30291 RVA: 0x002F16C0 File Offset: 0x002EF8C0
	public static void DecrementFlag(PlayerController player, Type flagType)
	{
		if (PassiveItem.ActiveFlagItems.ContainsKey(player) && PassiveItem.ActiveFlagItems[player].ContainsKey(flagType))
		{
			PassiveItem.ActiveFlagItems[player][flagType] = PassiveItem.ActiveFlagItems[player][flagType] - 1;
			if (PassiveItem.ActiveFlagItems[player][flagType] <= 0)
			{
				PassiveItem.ActiveFlagItems[player].Remove(flagType);
			}
		}
	}

	// Token: 0x06007654 RID: 30292 RVA: 0x002F1740 File Offset: 0x002EF940
	public static bool IsFlagSetAtAll(Type flagType)
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			if (PassiveItem.IsFlagSetForCharacter(GameManager.Instance.AllPlayers[i], flagType))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06007655 RID: 30293 RVA: 0x002F1784 File Offset: 0x002EF984
	public static bool IsFlagSetForCharacter(PlayerController player, Type flagType)
	{
		return PassiveItem.ActiveFlagItems.ContainsKey(player) && PassiveItem.ActiveFlagItems[player].ContainsKey(flagType) && PassiveItem.ActiveFlagItems[player][flagType] > 0;
	}

	// Token: 0x170011A3 RID: 4515
	// (get) Token: 0x06007656 RID: 30294 RVA: 0x002F17C4 File Offset: 0x002EF9C4
	public bool PickedUp
	{
		get
		{
			return this.m_pickedUp;
		}
	}

	// Token: 0x170011A4 RID: 4516
	// (get) Token: 0x06007657 RID: 30295 RVA: 0x002F17CC File Offset: 0x002EF9CC
	public PlayerController Owner
	{
		get
		{
			return this.m_owner;
		}
	}

	// Token: 0x06007658 RID: 30296 RVA: 0x002F17D4 File Offset: 0x002EF9D4
	private void Start()
	{
		if (!this.m_pickedUp)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		}
		if (!this.m_pickedUp)
		{
			this.RegisterMinimapIcon();
		}
	}

	// Token: 0x06007659 RID: 30297 RVA: 0x002F1810 File Offset: 0x002EFA10
	public void RegisterMinimapIcon()
	{
		if (base.transform.position.y < -300f)
		{
			return;
		}
		if (this.minimapIcon == null)
		{
			if (PassiveItem.m_defaultIcon == null)
			{
				PassiveItem.m_defaultIcon = (GameObject)BraveResources.Load("Global Prefabs/Minimap_Item_Icon", ".prefab");
			}
			this.minimapIcon = PassiveItem.m_defaultIcon;
		}
		if (this.minimapIcon != null && !this.m_pickedUp)
		{
			this.m_minimapIconRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
			this.m_instanceMinimapIcon = Minimap.Instance.RegisterRoomIcon(this.m_minimapIconRoom, this.minimapIcon, false);
		}
	}

	// Token: 0x0600765A RID: 30298 RVA: 0x002F18E4 File Offset: 0x002EFAE4
	public void GetRidOfMinimapIcon()
	{
		if (this.m_instanceMinimapIcon != null)
		{
			Minimap.Instance.DeregisterRoomIcon(this.m_minimapIconRoom, this.m_instanceMinimapIcon);
			this.m_instanceMinimapIcon = null;
		}
	}

	// Token: 0x0600765B RID: 30299 RVA: 0x002F1914 File Offset: 0x002EFB14
	public virtual DebrisObject Drop(PlayerController player)
	{
		this.m_pickedUp = false;
		this.m_pickedUpThisRun = true;
		this.HasBeenStatProcessed = true;
		this.DisableEffect(player);
		this.m_owner = null;
		DebrisObject debrisObject = LootEngine.DropItemWithoutInstantiating(base.gameObject, player.LockedApproximateSpriteCenter, player.unadjustedAimPoint - player.LockedApproximateSpriteCenter, 4f, true, false, false, false);
		SpriteOutlineManager.AddOutlineToSprite(debrisObject.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		this.RegisterMinimapIcon();
		return debrisObject;
	}

	// Token: 0x0600765C RID: 30300 RVA: 0x002F1998 File Offset: 0x002EFB98
	public float GetDistanceToPoint(Vector2 point)
	{
		if (this.IsBeingSold)
		{
			return 1000f;
		}
		if (!base.sprite)
		{
			return 1000f;
		}
		Bounds bounds = base.sprite.GetBounds();
		bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
	}

	// Token: 0x0600765D RID: 30301 RVA: 0x002F1AA0 File Offset: 0x002EFCA0
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x0600765E RID: 30302 RVA: 0x002F1AA8 File Offset: 0x002EFCA8
	protected virtual void Update()
	{
		if (!this.m_pickedUp && this.m_owner == null)
		{
			base.HandlePickupCurseParticles();
			if (!this.m_isBeingEyedByRat && Time.frameCount % 51 == 0 && base.ShouldBeTakenByRat(base.sprite.WorldCenter))
			{
				GameManager.Instance.Dungeon.StartCoroutine(base.HandleRatTheft());
			}
		}
	}

	// Token: 0x0600765F RID: 30303 RVA: 0x002F1B1C File Offset: 0x002EFD1C
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this)
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
		SquishyBounceWiggler component = base.GetComponent<SquishyBounceWiggler>();
		if (component != null)
		{
			component.WiggleHold = true;
		}
	}

	// Token: 0x06007660 RID: 30304 RVA: 0x002F1B94 File Offset: 0x002EFD94
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		if (this.m_pickedUp)
		{
			return;
		}
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		base.sprite.UpdateZDepth();
		SquishyBounceWiggler component = base.GetComponent<SquishyBounceWiggler>();
		if (component != null)
		{
			component.WiggleHold = false;
		}
	}

	// Token: 0x06007661 RID: 30305 RVA: 0x002F1C08 File Offset: 0x002EFE08
	public void Interact(PlayerController interactor)
	{
		if (GameStatsManager.HasInstance && GameStatsManager.Instance.IsRainbowRun)
		{
			if (interactor && interactor.CurrentRoom != null && interactor.CurrentRoom == GameManager.Instance.Dungeon.data.Entrance && Time.frameCount == PickupObject.s_lastRainbowPickupFrame)
			{
				return;
			}
			PickupObject.s_lastRainbowPickupFrame = Time.frameCount;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		this.Pickup(interactor);
	}

	// Token: 0x06007662 RID: 30306 RVA: 0x002F1C90 File Offset: 0x002EFE90
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x06007663 RID: 30307 RVA: 0x002F1C9C File Offset: 0x002EFE9C
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (RoomHandler.unassignedInteractableObjects.Contains(this))
		{
			RoomHandler.unassignedInteractableObjects.Remove(this);
		}
		if (!Dungeon.IsGenerating)
		{
			RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
			if (absoluteRoom.IsRegistered(this))
			{
				absoluteRoom.DeregisterInteractable(this);
			}
		}
		if (GameManager.Instance.InTutorial)
		{
			GameManager.BroadcastRoomTalkDoerFsmEvent("playerAcquiredPassiveItem");
		}
		base.OnSharedPickup();
		this.GetRidOfMinimapIcon();
		this.m_isBeingEyedByRat = false;
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		this.m_pickedUp = true;
		this.m_owner = player;
		if (this.OnPickedUp != null)
		{
			this.OnPickedUp(this.m_owner);
		}
		if (this.ShouldBeDestroyedOnExistence(false))
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (!this.m_pickedUpThisRun)
		{
			base.HandleLootMods(player);
			base.HandleEncounterable(player);
			if (this.ArmorToGainOnInitialPickup > 0)
			{
				player.healthHaver.Armor += (float)this.ArmorToGainOnInitialPickup;
			}
			if (this.ItemSpansBaseQualityTiers || this.ItemRespectsHeartMagnificence)
			{
				RewardManager.AdditionalHeartTierMagnificence += 1f;
			}
		}
		else if (base.encounterTrackable && base.encounterTrackable.m_doNotificationOnEncounter && !EncounterTrackable.SuppressNextNotification && !GameUIRoot.Instance.BossHealthBarVisible)
		{
			GameUIRoot.Instance.notificationController.DoNotification(base.encounterTrackable, true);
		}
		if (!this.m_pickedUpThisRun && player.characterIdentity == PlayableCharacters.Robot)
		{
			for (int i = 0; i < this.passiveStatModifiers.Length; i++)
			{
				if (this.passiveStatModifiers[i].statToBoost == PlayerStats.StatType.Health && this.passiveStatModifiers[i].amount > 0f)
				{
					int num = Mathf.FloorToInt(this.passiveStatModifiers[i].amount * (float)UnityEngine.Random.Range(GameManager.Instance.RewardManager.RobotMinCurrencyPerHealthItem, GameManager.Instance.RewardManager.RobotMaxCurrencyPerHealthItem + 1));
					LootEngine.SpawnCurrency(player.CenterPosition, num, false);
				}
			}
		}
		if (!this.suppressPickupVFX && !PileOfDarkSoulsPickup.IsPileOfDarkSoulsPickup)
		{
			GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Item_Pickup");
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			tk2dSprite component = gameObject2.GetComponent<tk2dSprite>();
			component.PlaceAtPositionByAnchor(base.sprite.WorldCenter, tk2dBaseSprite.Anchor.MiddleCenter);
			component.UpdateZDepth();
		}
		this.m_pickedUpThisRun = true;
		PlatformInterface.SetAlienFXColor(this.m_alienPickupColor, 1f);
		player.AcquirePassiveItem(this);
	}

	// Token: 0x06007664 RID: 30308 RVA: 0x002F1F4C File Offset: 0x002F014C
	protected virtual void DisableEffect(PlayerController disablingPlayer)
	{
		if (this.OnDisabled != null)
		{
			this.OnDisabled(disablingPlayer);
		}
	}

	// Token: 0x06007665 RID: 30309 RVA: 0x002F1F68 File Offset: 0x002F0168
	public override void MidGameDeserialize(List<object> data)
	{
		base.MidGameDeserialize(data);
		for (int i = 0; i < this.passiveStatModifiers.Length; i++)
		{
			if (this.m_owner && this.passiveStatModifiers[i].statToBoost == PlayerStats.StatType.AdditionalBlanksPerFloor)
			{
				this.m_owner.Blanks += Mathf.RoundToInt(this.passiveStatModifiers[i].amount);
			}
		}
	}

	// Token: 0x06007666 RID: 30310 RVA: 0x002F1FE0 File Offset: 0x002F01E0
	protected override void OnDestroy()
	{
		if (Minimap.HasInstance)
		{
			this.GetRidOfMinimapIcon();
		}
		this.DisableEffect(this.m_owner);
		this.m_owner = null;
		base.OnDestroy();
	}

	// Token: 0x04007837 RID: 30775
	public static Dictionary<PlayerController, Dictionary<Type, int>> ActiveFlagItems = new Dictionary<PlayerController, Dictionary<Type, int>>();

	// Token: 0x04007838 RID: 30776
	private static GameObject m_defaultIcon;

	// Token: 0x04007839 RID: 30777
	protected bool m_pickedUp;

	// Token: 0x0400783A RID: 30778
	protected bool m_pickedUpThisRun;

	// Token: 0x0400783B RID: 30779
	[NonSerialized]
	public bool suppressPickupVFX;

	// Token: 0x0400783C RID: 30780
	[SerializeField]
	public StatModifier[] passiveStatModifiers;

	// Token: 0x0400783D RID: 30781
	[SerializeField]
	public int ArmorToGainOnInitialPickup;

	// Token: 0x0400783E RID: 30782
	public GameObject minimapIcon;

	// Token: 0x0400783F RID: 30783
	private RoomHandler m_minimapIconRoom;

	// Token: 0x04007840 RID: 30784
	private GameObject m_instanceMinimapIcon;

	// Token: 0x04007841 RID: 30785
	protected PlayerController m_owner;

	// Token: 0x04007842 RID: 30786
	public Action<PlayerController> OnPickedUp;

	// Token: 0x04007843 RID: 30787
	public Action<PlayerController> OnDisabled;
}
