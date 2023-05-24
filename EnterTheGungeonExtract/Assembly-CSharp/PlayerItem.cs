using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020015FA RID: 5626
public class PlayerItem : PickupObject, IPlayerInteractable
{
	// Token: 0x17001377 RID: 4983
	// (get) Token: 0x06008285 RID: 33413 RVA: 0x00355780 File Offset: 0x00353980
	// (set) Token: 0x06008286 RID: 33414 RVA: 0x00355788 File Offset: 0x00353988
	public bool IsCurrentlyActive
	{
		get
		{
			return this.m_isCurrentlyActive;
		}
		protected set
		{
			if (value != this.m_isCurrentlyActive)
			{
				this.m_isCurrentlyActive = value;
				if (this.OnActivationStatusChanged != null)
				{
					this.OnActivationStatusChanged(this);
				}
			}
		}
	}

	// Token: 0x17001378 RID: 4984
	// (get) Token: 0x06008287 RID: 33415 RVA: 0x003557B4 File Offset: 0x003539B4
	public bool PickedUp
	{
		get
		{
			return this.m_pickedUp;
		}
	}

	// Token: 0x17001379 RID: 4985
	// (get) Token: 0x06008288 RID: 33416 RVA: 0x003557BC File Offset: 0x003539BC
	// (set) Token: 0x06008289 RID: 33417 RVA: 0x003557C4 File Offset: 0x003539C4
	public int CurrentRoomCooldown
	{
		get
		{
			return this.remainingRoomCooldown;
		}
		set
		{
			this.remainingRoomCooldown = value;
		}
	}

	// Token: 0x1700137A RID: 4986
	// (get) Token: 0x0600828A RID: 33418 RVA: 0x003557D0 File Offset: 0x003539D0
	// (set) Token: 0x0600828B RID: 33419 RVA: 0x003557D8 File Offset: 0x003539D8
	public float CurrentTimeCooldown
	{
		get
		{
			return this.remainingTimeCooldown;
		}
		set
		{
			this.remainingTimeCooldown = value;
		}
	}

	// Token: 0x1700137B RID: 4987
	// (get) Token: 0x0600828C RID: 33420 RVA: 0x003557E4 File Offset: 0x003539E4
	// (set) Token: 0x0600828D RID: 33421 RVA: 0x003557EC File Offset: 0x003539EC
	public float CurrentDamageCooldown
	{
		get
		{
			return this.remainingDamageCooldown;
		}
		set
		{
			this.remainingDamageCooldown = value;
		}
	}

	// Token: 0x1700137C RID: 4988
	// (get) Token: 0x0600828E RID: 33422 RVA: 0x003557F8 File Offset: 0x003539F8
	public bool IsActive
	{
		get
		{
			return this.IsCurrentlyActive;
		}
	}

	// Token: 0x1700137D RID: 4989
	// (get) Token: 0x0600828F RID: 33423 RVA: 0x00355800 File Offset: 0x00353A00
	public bool IsOnCooldown
	{
		get
		{
			return this.remainingRoomCooldown > 0 || this.remainingTimeCooldown > 0f || this.remainingDamageCooldown > 0f;
		}
	}

	// Token: 0x1700137E RID: 4990
	// (get) Token: 0x06008290 RID: 33424 RVA: 0x00355830 File Offset: 0x00353A30
	public float ActivePercentage
	{
		get
		{
			return Mathf.Clamp01(this.m_activeElapsed / this.m_activeDuration);
		}
	}

	// Token: 0x1700137F RID: 4991
	// (get) Token: 0x06008291 RID: 33425 RVA: 0x00355844 File Offset: 0x00353A44
	public float CooldownPercentage
	{
		get
		{
			if (this.IsCurrentlyActive)
			{
				return this.ActivePercentage;
			}
			if (!this.IsOnCooldown)
			{
				return 0f;
			}
			if (this.remainingRoomCooldown > 0)
			{
				return (float)this.remainingRoomCooldown / (float)this.roomCooldown;
			}
			if (this.remainingDamageCooldown > 0f)
			{
				return this.remainingDamageCooldown / this.damageCooldown;
			}
			if (this.remainingTimeCooldown > 0f)
			{
				return this.remainingTimeCooldown / this.timeCooldown;
			}
			return 0f;
		}
	}

	// Token: 0x06008292 RID: 33426 RVA: 0x003558D4 File Offset: 0x00353AD4
	protected virtual void Start()
	{
		this.m_baseSpriteID = base.sprite.spriteId;
		this.m_cachedNumberOfUses = this.numberOfUses;
		if (!this.m_pickedUp)
		{
			base.renderer.enabled = true;
			if (!(this is SilencerItem))
			{
				SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			}
			this.RegisterMinimapIcon();
		}
	}

	// Token: 0x06008293 RID: 33427 RVA: 0x00355944 File Offset: 0x00353B44
	public virtual void Update()
	{
		if (this.m_pickedUp)
		{
			if (this.LastOwner == null)
			{
				this.LastOwner = base.GetComponentInParent<PlayerController>();
			}
			if (this.remainingTimeCooldown > 0f && (PlayerItem.AllowDamageCooldownOnActive || !this.IsCurrentlyActive))
			{
				this.remainingTimeCooldown = Mathf.Max(0f, this.remainingTimeCooldown - BraveTime.DeltaTime);
			}
			if (this.IsCurrentlyActive)
			{
				this.m_activeElapsed += BraveTime.DeltaTime * this.m_adjustedTimeScale;
				if (!string.IsNullOrEmpty(this.OnActivatedSprite))
				{
					base.sprite.SetSprite(this.OnActivatedSprite);
				}
			}
		}
		else
		{
			base.HandlePickupCurseParticles();
			if (!this.m_isBeingEyedByRat && Time.frameCount % 47 == 0 && base.ShouldBeTakenByRat(base.sprite.WorldCenter))
			{
				GameManager.Instance.Dungeon.StartCoroutine(base.HandleRatTheft());
			}
		}
	}

	// Token: 0x06008294 RID: 33428 RVA: 0x00355A50 File Offset: 0x00353C50
	public void RegisterMinimapIcon()
	{
		if (base.transform.position.y < -300f)
		{
			return;
		}
		if (this.minimapIcon == null)
		{
			if (PlayerItem.m_defaultIcon == null)
			{
				PlayerItem.m_defaultIcon = (GameObject)BraveResources.Load("Global Prefabs/Minimap_Item_Icon", ".prefab");
			}
			this.minimapIcon = PlayerItem.m_defaultIcon;
		}
		if (this.minimapIcon != null && !this.m_pickedUp)
		{
			this.m_minimapIconRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Floor));
			this.m_instanceMinimapIcon = Minimap.Instance.RegisterRoomIcon(this.m_minimapIconRoom, this.minimapIcon, false);
		}
	}

	// Token: 0x06008295 RID: 33429 RVA: 0x00355B24 File Offset: 0x00353D24
	public void GetRidOfMinimapIcon()
	{
		if (this.m_instanceMinimapIcon != null)
		{
			Minimap.Instance.DeregisterRoomIcon(this.m_minimapIconRoom, this.m_instanceMinimapIcon);
			this.m_instanceMinimapIcon = null;
		}
	}

	// Token: 0x06008296 RID: 33430 RVA: 0x00355B54 File Offset: 0x00353D54
	protected bool UseConsumableStack()
	{
		this.numberOfUses--;
		if (this.numberOfUses <= 0)
		{
			this.m_isDestroyed = true;
			return true;
		}
		return false;
	}

	// Token: 0x06008297 RID: 33431 RVA: 0x00355B7C File Offset: 0x00353D7C
	public virtual bool CanBeUsed(PlayerController user)
	{
		return true;
	}

	// Token: 0x06008298 RID: 33432 RVA: 0x00355B80 File Offset: 0x00353D80
	public void ResetSprite()
	{
		if ((!string.IsNullOrEmpty(this.OnCooldownSprite) || !string.IsNullOrEmpty(this.OnActivatedSprite)) && base.sprite.spriteId != this.m_baseSpriteID)
		{
			base.sprite.SetSprite(this.m_baseSpriteID);
		}
	}

	// Token: 0x06008299 RID: 33433 RVA: 0x00355BD4 File Offset: 0x00353DD4
	public bool Use(PlayerController user, out float destroyTime)
	{
		destroyTime = -1f;
		if (this.m_isDestroyed)
		{
			return false;
		}
		if (!this.CanBeUsed(user))
		{
			return false;
		}
		if (this.IsCurrentlyActive)
		{
			this.DoActiveEffect(user);
			if (this.consumable && this.consumableOnActiveUse)
			{
				bool flag = this.UseConsumableStack();
				if (flag)
				{
					return true;
				}
			}
			if (!string.IsNullOrEmpty(this.OnActivatedSprite) && base.sprite.spriteId != this.m_baseSpriteID)
			{
				base.sprite.SetSprite(this.m_baseSpriteID);
			}
			return false;
		}
		if (this.IsOnCooldown)
		{
			this.DoOnCooldownEffect(user);
			if (this.consumable && this.consumableOnCooldownUse)
			{
				bool flag2 = this.UseConsumableStack();
				if (flag2)
				{
					return true;
				}
			}
			if (!string.IsNullOrEmpty(this.OnCooldownSprite) && base.sprite.spriteId != this.m_baseSpriteID)
			{
				base.sprite.SetSprite(this.m_baseSpriteID);
			}
			return false;
		}
		this.DoEffect(user);
		if (!string.IsNullOrEmpty(this.useAnimation))
		{
			tk2dSpriteAnimationClip clipByName = base.spriteAnimator.GetClipByName(this.useAnimation);
			base.spriteAnimator.Play(clipByName);
			destroyTime = (float)clipByName.frames.Length / clipByName.fps;
		}
		if (this.consumable && !this.consumableOnCooldownUse && !this.consumableOnActiveUse)
		{
			bool flag3 = this.UseConsumableStack();
			if (this.consumableHandlesOwnDuration)
			{
				destroyTime = this.customDestroyTime;
			}
			if (flag3)
			{
				return true;
			}
		}
		else if (this.UsesNumberOfUsesBeforeCooldown)
		{
			this.numberOfUses--;
		}
		if (destroyTime >= 0f)
		{
			base.StartCoroutine(this.HandleAnimationReset(destroyTime));
		}
		if (!this.UsesNumberOfUsesBeforeCooldown || this.numberOfUses <= 0)
		{
			if (this.UsesNumberOfUsesBeforeCooldown)
			{
				this.numberOfUses = this.m_cachedNumberOfUses;
			}
			this.ApplyCooldown(user);
			this.AfterCooldownApplied(user);
		}
		return false;
	}

	// Token: 0x0600829A RID: 33434 RVA: 0x00355DE4 File Offset: 0x00353FE4
	public void ForceApplyCooldown(PlayerController user)
	{
		this.ApplyCooldown(user);
		this.AfterCooldownApplied(user);
	}

	// Token: 0x0600829B RID: 33435 RVA: 0x00355DF4 File Offset: 0x00353FF4
	protected void ApplyCooldown(PlayerController user)
	{
		float num = 1f;
		if (user != null)
		{
			float num2 = user.stats.GetStatValue(PlayerStats.StatType.Coolness) * 0.05f;
			if (PassiveItem.IsFlagSetForCharacter(user, typeof(ChamberOfEvilItem)))
			{
				float num3 = user.stats.GetStatValue(PlayerStats.StatType.Curse) * 0.05f;
				num2 += num3;
			}
			num2 = Mathf.Clamp(num2, 0f, 0.5f);
			num = Mathf.Max(0f, num - num2);
		}
		this.remainingRoomCooldown += this.roomCooldown;
		this.remainingTimeCooldown += this.timeCooldown * num;
		this.remainingDamageCooldown += this.damageCooldown * num;
		if (!string.IsNullOrEmpty(this.OnCooldownSprite))
		{
			base.sprite.SetSprite(this.OnCooldownSprite);
		}
	}

	// Token: 0x0600829C RID: 33436 RVA: 0x00355ED4 File Offset: 0x003540D4
	protected void ApplyAdditionalTimeCooldown(float addTimeCooldown)
	{
		this.remainingTimeCooldown += addTimeCooldown;
	}

	// Token: 0x0600829D RID: 33437 RVA: 0x00355EE4 File Offset: 0x003540E4
	protected void ApplyAdditionalDamageCooldown(float addDamageCooldown)
	{
		this.remainingDamageCooldown += addDamageCooldown;
	}

	// Token: 0x0600829E RID: 33438 RVA: 0x00355EF4 File Offset: 0x003540F4
	private IEnumerator HandleAnimationReset(float delay)
	{
		yield return new WaitForSeconds(delay);
		base.spriteAnimator.StopAndResetFrame();
		yield break;
	}

	// Token: 0x0600829F RID: 33439 RVA: 0x00355F18 File Offset: 0x00354118
	public void ClearCooldowns()
	{
		this.remainingRoomCooldown = 0;
		this.remainingDamageCooldown = 0f;
		this.remainingTimeCooldown = 0f;
	}

	// Token: 0x060082A0 RID: 33440 RVA: 0x00355F38 File Offset: 0x00354138
	public void DidDamage(PlayerController Owner, float damageDone)
	{
		if (this.IsActive && !PlayerItem.AllowDamageCooldownOnActive)
		{
			return;
		}
		float num = 1f;
		GameLevelDefinition lastLoadedLevelDefinition = GameManager.Instance.GetLastLoadedLevelDefinition();
		if (lastLoadedLevelDefinition != null)
		{
			num /= lastLoadedLevelDefinition.enemyHealthMultiplier;
		}
		damageDone *= num;
		this.remainingDamageCooldown = Mathf.Max(0f, this.remainingDamageCooldown - damageDone);
	}

	// Token: 0x060082A1 RID: 33441 RVA: 0x00355F98 File Offset: 0x00354198
	public void ClearedRoom()
	{
		if (this.remainingRoomCooldown > 0)
		{
			this.remainingRoomCooldown--;
		}
	}

	// Token: 0x060082A2 RID: 33442 RVA: 0x00355FB4 File Offset: 0x003541B4
	public virtual void OnItemSwitched(PlayerController user)
	{
	}

	// Token: 0x060082A3 RID: 33443 RVA: 0x00355FB8 File Offset: 0x003541B8
	protected virtual void DoEffect(PlayerController user)
	{
	}

	// Token: 0x060082A4 RID: 33444 RVA: 0x00355FBC File Offset: 0x003541BC
	protected virtual void AfterCooldownApplied(PlayerController user)
	{
	}

	// Token: 0x060082A5 RID: 33445 RVA: 0x00355FC0 File Offset: 0x003541C0
	protected virtual void DoActiveEffect(PlayerController user)
	{
	}

	// Token: 0x060082A6 RID: 33446 RVA: 0x00355FC4 File Offset: 0x003541C4
	protected virtual void DoOnCooldownEffect(PlayerController user)
	{
	}

	// Token: 0x060082A7 RID: 33447 RVA: 0x00355FC8 File Offset: 0x003541C8
	protected virtual void OnPreDrop(PlayerController user)
	{
	}

	// Token: 0x060082A8 RID: 33448 RVA: 0x00355FCC File Offset: 0x003541CC
	public DebrisObject Drop(PlayerController player, float overrideForce = 4f)
	{
		this.OnPreDrop(player);
		if (this.OnPreDropEvent != null)
		{
			this.OnPreDropEvent();
		}
		Vector2 vector = player.unadjustedAimPoint - player.sprite.WorldCenter.ToVector3ZUp(0f);
		if (player.CurrentRoom != null && player.CurrentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SPECIAL)
		{
			overrideForce = 2f;
			vector = Vector2.down;
		}
		Vector3 vector2 = player.sprite.WorldCenter.ToVector3ZUp(0f);
		if (this is RobotUnlockTelevisionItem)
		{
			vector2 += new Vector3(0f, -0.875f, 0f);
		}
		DebrisObject debrisObject = LootEngine.SpawnItem(base.gameObject, vector2, vector, overrideForce, true, false, false);
		PlayerItem component = debrisObject.GetComponent<PlayerItem>();
		component.m_baseSpriteID = this.m_baseSpriteID;
		component.m_pickedUp = false;
		component.m_pickedUpThisRun = true;
		component.HasBeenStatProcessed = true;
		component.HasProcessedStatMods = this.HasProcessedStatMods;
		component.remainingDamageCooldown = this.remainingDamageCooldown;
		component.remainingRoomCooldown = this.remainingRoomCooldown;
		component.remainingTimeCooldown = this.remainingTimeCooldown;
		component.ResetSprite();
		component.CopyStateFrom(this);
		player.stats.RecalculateStats(player, false, false);
		return debrisObject;
	}

	// Token: 0x060082A9 RID: 33449 RVA: 0x0035610C File Offset: 0x0035430C
	protected virtual void CopyStateFrom(PlayerItem other)
	{
	}

	// Token: 0x060082AA RID: 33450 RVA: 0x00356110 File Offset: 0x00354310
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		if (GameManager.Instance.InTutorial)
		{
			GameManager.BroadcastRoomTalkDoerFsmEvent("playerAcquiredPlayerItem");
		}
		if (RoomHandler.unassignedInteractableObjects.Contains(this))
		{
			RoomHandler.unassignedInteractableObjects.Remove(this);
		}
		base.OnSharedPickup();
		this.GetRidOfMinimapIcon();
		if (this.ShouldBeDestroyedOnExistence(false))
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (!PileOfDarkSoulsPickup.IsPileOfDarkSoulsPickup)
		{
			AkSoundEngine.PostEvent("Play_OBJ_item_pickup_01", base.gameObject);
			GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Item_Pickup");
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			tk2dSprite component = gameObject2.GetComponent<tk2dSprite>();
			component.PlaceAtPositionByAnchor(base.sprite.WorldCenter, tk2dBaseSprite.Anchor.MiddleCenter);
			component.UpdateZDepth();
		}
		if (!this.m_pickedUpThisRun)
		{
			base.HandleLootMods(player);
			base.HandleEncounterable(player);
		}
		else if (base.encounterTrackable && base.encounterTrackable.m_doNotificationOnEncounter && !EncounterTrackable.SuppressNextNotification && !GameUIRoot.Instance.BossHealthBarVisible)
		{
			GameUIRoot.Instance.notificationController.DoNotification(base.encounterTrackable, true);
		}
		this.LastOwner = player;
		this.m_isBeingEyedByRat = false;
		DebrisObject component2 = base.GetComponent<DebrisObject>();
		if (component2 != null || this.ForceAsExtant)
		{
			if (component2)
			{
				UnityEngine.Object.Destroy(component2);
			}
			this.m_pickedUp = true;
			this.m_pickedUpThisRun = true;
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
			base.renderer.enabled = false;
			SquishyBounceWiggler component3 = base.GetComponent<SquishyBounceWiggler>();
			if (component3 != null)
			{
				UnityEngine.Object.Destroy(component3);
				base.sprite.ForceBuild();
			}
			player.GetEquippedWith(this, false);
		}
		else
		{
			GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
			PlayerItem component4 = gameObject3.GetComponent<PlayerItem>();
			gameObject3.GetComponent<Renderer>().enabled = false;
			gameObject3.transform.position = player.transform.position;
			component4.m_pickedUp = true;
			component4.m_pickedUpThisRun = true;
			player.GetEquippedWith(component4, false);
		}
		if (this.OnPickedUp != null)
		{
			this.OnPickedUp(player);
		}
		PlatformInterface.SetAlienFXColor(this.m_alienPickupColor, 1f);
		player.DoVibration(Vibration.Time.Quick, Vibration.Strength.Medium);
	}

	// Token: 0x060082AB RID: 33451 RVA: 0x00356368 File Offset: 0x00354568
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

	// Token: 0x060082AC RID: 33452 RVA: 0x00356474 File Offset: 0x00354674
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x060082AD RID: 33453 RVA: 0x0035647C File Offset: 0x0035467C
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
		if (!(this is SilencerItem))
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		}
		base.sprite.UpdateZDepth();
		SquishyBounceWiggler component = base.GetComponent<SquishyBounceWiggler>();
		if (component != null)
		{
			component.WiggleHold = true;
		}
	}

	// Token: 0x060082AE RID: 33454 RVA: 0x00356500 File Offset: 0x00354700
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
		if (!(this is SilencerItem))
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		}
		base.sprite.UpdateZDepth();
		SquishyBounceWiggler component = base.GetComponent<SquishyBounceWiggler>();
		if (component != null)
		{
			component.WiggleHold = false;
		}
	}

	// Token: 0x060082AF RID: 33455 RVA: 0x0035657C File Offset: 0x0035477C
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
		if (GameManager.Instance.InTutorial)
		{
			EncounterTrackable.SuppressNextNotification = true;
		}
		this.Pickup(interactor);
	}

	// Token: 0x060082B0 RID: 33456 RVA: 0x0035661C File Offset: 0x0035481C
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x060082B1 RID: 33457 RVA: 0x00356628 File Offset: 0x00354828
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Minimap.HasInstance)
		{
			this.GetRidOfMinimapIcon();
		}
	}

	// Token: 0x04008596 RID: 34198
	public static bool AllowDamageCooldownOnActive;

	// Token: 0x04008597 RID: 34199
	private static GameObject m_defaultIcon;

	// Token: 0x04008598 RID: 34200
	public bool consumable = true;

	// Token: 0x04008599 RID: 34201
	[ShowInInspectorIf("consumable", false)]
	public bool consumableOnCooldownUse;

	// Token: 0x0400859A RID: 34202
	[ShowInInspectorIf("consumable", false)]
	public bool consumableOnActiveUse;

	// Token: 0x0400859B RID: 34203
	[ShowInInspectorIf("consumable", false)]
	public bool consumableHandlesOwnDuration;

	// Token: 0x0400859C RID: 34204
	[ShowInInspectorIf("consumableHandlesOwnDuration", false)]
	public float customDestroyTime = -1f;

	// Token: 0x0400859D RID: 34205
	public int numberOfUses = 1;

	// Token: 0x0400859E RID: 34206
	public bool UsesNumberOfUsesBeforeCooldown;

	// Token: 0x0400859F RID: 34207
	public bool canStack = true;

	// Token: 0x040085A0 RID: 34208
	public int roomCooldown = 1;

	// Token: 0x040085A1 RID: 34209
	public float timeCooldown;

	// Token: 0x040085A2 RID: 34210
	public float damageCooldown;

	// Token: 0x040085A3 RID: 34211
	public bool usableDuringDodgeRoll;

	// Token: 0x040085A4 RID: 34212
	public string useAnimation;

	// Token: 0x040085A5 RID: 34213
	[NonSerialized]
	public bool ForceAsExtant;

	// Token: 0x040085A6 RID: 34214
	[NonSerialized]
	public bool PreventCooldownBar;

	// Token: 0x040085A7 RID: 34215
	public GameObject minimapIcon;

	// Token: 0x040085A8 RID: 34216
	private RoomHandler m_minimapIconRoom;

	// Token: 0x040085A9 RID: 34217
	private GameObject m_instanceMinimapIcon;

	// Token: 0x040085AA RID: 34218
	public Action<PlayerItem> OnActivationStatusChanged;

	// Token: 0x040085AB RID: 34219
	[NonSerialized]
	protected bool m_isCurrentlyActive;

	// Token: 0x040085AC RID: 34220
	private bool m_isDestroyed;

	// Token: 0x040085AD RID: 34221
	[NonSerialized]
	protected float m_activeElapsed;

	// Token: 0x040085AE RID: 34222
	[NonSerialized]
	protected float m_activeDuration;

	// Token: 0x040085AF RID: 34223
	[NonSerialized]
	protected int m_cachedNumberOfUses;

	// Token: 0x040085B0 RID: 34224
	[NonSerialized]
	protected bool m_pickedUp;

	// Token: 0x040085B1 RID: 34225
	[NonSerialized]
	protected bool m_pickedUpThisRun;

	// Token: 0x040085B2 RID: 34226
	private int remainingRoomCooldown;

	// Token: 0x040085B3 RID: 34227
	private float remainingTimeCooldown;

	// Token: 0x040085B4 RID: 34228
	private float remainingDamageCooldown;

	// Token: 0x040085B5 RID: 34229
	public string OnActivatedSprite = string.Empty;

	// Token: 0x040085B6 RID: 34230
	public string OnCooldownSprite = string.Empty;

	// Token: 0x040085B7 RID: 34231
	[SerializeField]
	public StatModifier[] passiveStatModifiers;

	// Token: 0x040085B8 RID: 34232
	private int m_baseSpriteID = -1;

	// Token: 0x040085B9 RID: 34233
	[NonSerialized]
	public PlayerController LastOwner;

	// Token: 0x040085BA RID: 34234
	[NonSerialized]
	protected float m_adjustedTimeScale = 1f;

	// Token: 0x040085BB RID: 34235
	public Action<PlayerController> OnPickedUp;

	// Token: 0x040085BC RID: 34236
	public Action OnPreDropEvent;
}
