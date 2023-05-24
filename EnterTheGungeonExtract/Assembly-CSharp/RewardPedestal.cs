using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02001698 RID: 5784
public class RewardPedestal : DungeonPlaceableBehaviour, IPlayerInteractable
{
	// Token: 0x17001439 RID: 5177
	// (get) Token: 0x060086CE RID: 34510 RVA: 0x0037E058 File Offset: 0x0037C258
	// (set) Token: 0x060086CF RID: 34511 RVA: 0x0037E060 File Offset: 0x0037C260
	public bool OffsetTertiarySet { get; set; }

	// Token: 0x1700143A RID: 5178
	// (get) Token: 0x060086D0 RID: 34512 RVA: 0x0037E06C File Offset: 0x0037C26C
	public bool IsMimic
	{
		get
		{
			return this.m_isMimic;
		}
	}

	// Token: 0x060086D1 RID: 34513 RVA: 0x0037E074 File Offset: 0x0037C274
	private void Awake()
	{
		if (base.majorBreakable)
		{
			base.majorBreakable.TemporarilyInvulnerable = true;
		}
	}

	// Token: 0x060086D2 RID: 34514 RVA: 0x0037E094 File Offset: 0x0037C294
	private void Start()
	{
		if (this.UsesSpecificItem)
		{
			this.m_room = base.GetAbsoluteParentRoom();
			this.HandleSpawnBehavior();
		}
	}

	// Token: 0x060086D3 RID: 34515 RVA: 0x0037E0B4 File Offset: 0x0037C2B4
	private void OnEnable()
	{
		if (this.m_isMimic && !this.m_isMimicBreathing)
		{
			base.StartCoroutine(this.MimicIdleAnimCR());
		}
	}

	// Token: 0x060086D4 RID: 34516 RVA: 0x0037E0DC File Offset: 0x0037C2DC
	public static RewardPedestal Spawn(RewardPedestal pedestalPrefab, IntVector2 basePosition)
	{
		RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(basePosition);
		return RewardPedestal.Spawn(pedestalPrefab, basePosition, roomFromPosition);
	}

	// Token: 0x060086D5 RID: 34517 RVA: 0x0037E104 File Offset: 0x0037C304
	public static RewardPedestal Spawn(RewardPedestal pedestalPrefab, IntVector2 basePosition, RoomHandler room)
	{
		if (pedestalPrefab == null)
		{
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(pedestalPrefab.gameObject, basePosition.ToVector3() + new Vector3(0.1875f, 0f, 0f), Quaternion.identity);
		RewardPedestal component = gameObject.GetComponent<RewardPedestal>();
		component.m_room = room;
		component.HandleSpawnBehavior();
		return component;
	}

	// Token: 0x060086D6 RID: 34518 RVA: 0x0037E168 File Offset: 0x0037C368
	public void RegisterChestOnMinimap(RoomHandler r)
	{
		if (GameStatsManager.HasInstance && GameStatsManager.Instance.IsRainbowRun)
		{
			return;
		}
		this.m_registeredIconRoom = r;
		GameObject gameObject = BraveResources.Load("Global Prefabs/Minimap_Treasure_Icon", ".prefab") as GameObject;
		this.minimapIconInstance = Minimap.Instance.RegisterRoomIcon(r, gameObject, false);
	}

	// Token: 0x060086D7 RID: 34519 RVA: 0x0037E1C0 File Offset: 0x0037C3C0
	public void GiveCoopPartnerBack()
	{
		PlayerController playerController = ((!GameManager.Instance.PrimaryPlayer.healthHaver.IsDead) ? GameManager.Instance.SecondaryPlayer : GameManager.Instance.PrimaryPlayer);
		playerController.specRigidbody.enabled = true;
		playerController.gameObject.SetActive(true);
		playerController.ResurrectFromBossKill();
	}

	// Token: 0x060086D8 RID: 34520 RVA: 0x0037E220 File Offset: 0x0037C420
	private void HandleSpawnBehavior()
	{
		GameManager.Instance.Dungeon.StartCoroutine(this.SpawnBehavior_CR());
	}

	// Token: 0x060086D9 RID: 34521 RVA: 0x0037E238 File Offset: 0x0037C438
	private IEnumerator SpawnBehavior_CR()
	{
		if (this.VFX_PreSpawn != null)
		{
			base.renderer.enabled = false;
			this.VFX_PreSpawn.SetActive(true);
			yield return new WaitForSeconds(0.1f);
			base.renderer.enabled = true;
		}
		if (!string.IsNullOrEmpty(this.spawnAnimName))
		{
			tk2dSpriteAnimationClip clip = base.spriteAnimator.GetClipByName(this.spawnAnimName);
			if (clip != null)
			{
				base.specRigidbody.enabled = false;
				float clipTime = (float)clip.frames.Length / clip.fps;
				base.spriteAnimator.Play(clip);
				base.sprite.UpdateZDepth();
				float elapsed = 0f;
				bool groundHitTriggered = false;
				while (elapsed < clipTime)
				{
					elapsed += BraveTime.DeltaTime;
					if (elapsed >= this.groundHitDelay && !groundHitTriggered)
					{
						Exploder.DoRadialPush(base.sprite.WorldCenter.ToVector3ZUp(base.sprite.WorldCenter.y), 22f, 5f);
						groundHitTriggered = true;
						this.VFX_GroundHit.SetActive(true);
					}
					yield return null;
				}
			}
		}
		base.sprite.UpdateZDepth();
		this.m_room.RegisterInteractable(this);
		base.specRigidbody.enabled = true;
		List<CollisionData> overlappingRigidbodies = new List<CollisionData>();
		PhysicsEngine.Instance.OverlapCast(base.specRigidbody, overlappingRigidbodies, false, true, null, null, false, null, null, new SpeculativeRigidbody[0]);
		for (int i = 0; i < overlappingRigidbodies.Count; i++)
		{
			SpeculativeRigidbody otherRigidbody = overlappingRigidbodies[i].OtherRigidbody;
			if (otherRigidbody)
			{
				if (otherRigidbody.minorBreakable)
				{
					otherRigidbody.minorBreakable.Break(otherRigidbody.UnitCenter - base.specRigidbody.UnitCenter);
				}
				if (otherRigidbody.majorBreakable)
				{
					otherRigidbody.majorBreakable.Break(otherRigidbody.UnitCenter - base.specRigidbody.UnitCenter);
				}
				MeduziDeathController component = otherRigidbody.GetComponent<MeduziDeathController>();
				if (component)
				{
					component.Shatter();
				}
			}
		}
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
		if (this.ReturnCoopPlayerOnLand)
		{
			this.GiveCoopPartnerBack();
		}
		if (this.SpawnsTertiarySet)
		{
			TertiaryBossRewardSet tertiaryRewardSet = GameManager.Instance.Dungeon.GetTertiaryRewardSet();
			for (int j = 0; j < tertiaryRewardSet.dropIds.Count; j++)
			{
				Vector2 vector = base.sprite.WorldCenter + new Vector2(-3f, 0f);
				if (j == 1)
				{
					vector = base.sprite.WorldCenter + new Vector2(2f, 0f);
				}
				if (j == 2)
				{
					vector = base.sprite.WorldCenter + new Vector2(0f, -3f);
				}
				if (this.OffsetTertiarySet)
				{
					vector += Vector2.right;
				}
				if (tertiaryRewardSet.dropIds.Count > 3)
				{
					vector = base.sprite.WorldCenter + (Quaternion.Euler(0f, 0f, 360f / (float)tertiaryRewardSet.dropIds.Count * (float)j) * new Vector2(3f, 0f)).XY();
				}
				if (GameManager.Instance.Dungeon.IsGlitchDungeon)
				{
					IntVector2? randomAvailableCell = base.transform.position.GetAbsoluteRoom().GetRandomAvailableCell(new IntVector2?(new IntVector2(2, 2)), new CellTypes?(CellTypes.FLOOR), false, null);
					if (randomAvailableCell != null)
					{
						vector = randomAvailableCell.Value.ToCenterVector2();
					}
				}
				PickupObject byId = PickupObjectDatabase.GetById(tertiaryRewardSet.dropIds[j]);
				LootEngine.SpawnItem(byId.gameObject, vector.ToVector3ZUp(0f), Vector2.zero, 0f, true, false, false);
			}
		}
		this.DetermineContents(GameManager.Instance.PrimaryPlayer);
		this.MaybeBecomeMimic();
		if (base.majorBreakable)
		{
			base.majorBreakable.TemporarilyInvulnerable = false;
			MajorBreakable majorBreakable = base.majorBreakable;
			majorBreakable.OnDamaged = (Action<float>)Delegate.Combine(majorBreakable.OnDamaged, new Action<float>(this.OnDamaged));
		}
		yield break;
	}

	// Token: 0x060086DA RID: 34522 RVA: 0x0037E254 File Offset: 0x0037C454
	protected void DetermineContents(PlayerController player)
	{
		if (this.contents == null)
		{
			if (this.IsBossRewardPedestal)
			{
				if (GameStatsManager.Instance.IsRainbowRun)
				{
					LootEngine.SpawnBowlerNote(GameManager.Instance.RewardManager.BowlerNoteBoss, base.sprite.WorldCenter + new Vector2(-0.5f, -3f), base.GetAbsoluteParentRoom(), false);
					return;
				}
				if (this.lootTable.lootTable != null)
				{
					this.contents = this.lootTable.lootTable.SelectByWeightWithoutDuplicatesFullPrereqs(null, true, false).GetComponent<PickupObject>();
				}
				else
				{
					if (GameManager.Instance.IsSeeded)
					{
						FloorRewardManifest seededManifestForCurrentFloor = GameManager.Instance.RewardManager.GetSeededManifestForCurrentFloor();
						if (seededManifestForCurrentFloor != null)
						{
							this.contents = seededManifestForCurrentFloor.GetNextBossReward(GameManager.Instance.RewardManager.IsBossRewardForcedGun());
						}
					}
					if (this.contents == null)
					{
						this.contents = GameManager.Instance.RewardManager.GetRewardObjectBossStyle(player).GetComponent<PickupObject>();
					}
				}
			}
			else if (this.UsesSpecificItem)
			{
				this.contents = PickupObjectDatabase.GetById(this.SpecificItemId);
			}
			else if (this.lootTable.lootTable == null)
			{
				this.contents = GameManager.Instance.Dungeon.baseChestContents.SelectByWeight(false).GetComponent<PickupObject>();
			}
			else if (this.lootTable != null)
			{
				this.contents = this.lootTable.GetSingleItemForPlayer(player, 0);
				if (this.contents == null)
				{
				}
			}
		}
		if (this.m_itemDisplaySprite == null)
		{
			this.m_itemDisplaySprite = tk2dSprite.AddComponent(new GameObject("Display Sprite")
			{
				transform = 
				{
					parent = this.spawnTransform
				}
			}, this.contents.sprite.Collection, this.contents.sprite.spriteId);
			SpriteOutlineManager.AddOutlineToSprite(this.m_itemDisplaySprite, Color.black, 0.1f, 0.05f, SpriteOutlineManager.OutlineType.NORMAL);
			base.sprite.AttachRenderer(this.m_itemDisplaySprite);
			this.m_itemDisplaySprite.HeightOffGround = 0.25f;
			this.m_itemDisplaySprite.depthUsesTrimmedBounds = true;
			this.m_itemDisplaySprite.PlaceAtPositionByAnchor(this.spawnTransform.position, tk2dBaseSprite.Anchor.LowerCenter);
			this.m_itemDisplaySprite.transform.position = this.m_itemDisplaySprite.transform.position.Quantize(0.0625f);
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
			tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
			component.PlaceAtPositionByAnchor(this.m_itemDisplaySprite.WorldCenter.ToVector3ZUp(0f), tk2dBaseSprite.Anchor.MiddleCenter);
			component.HeightOffGround = 5f;
			component.UpdateZDepth();
			base.sprite.UpdateZDepth();
		}
	}

	// Token: 0x060086DB RID: 34523 RVA: 0x0037E534 File Offset: 0x0037C734
	private void DoPickup(PlayerController player)
	{
		if (this.pickedUp)
		{
			return;
		}
		this.pickedUp = true;
		if (this.IsMimic && this.contents != null)
		{
			this.DoMimicTransformation(this.contents);
			return;
		}
		if (this.contents != null)
		{
			LootEngine.GivePrefabToPlayer(this.contents.gameObject, player);
			if (this.contents is Gun)
			{
				AkSoundEngine.PostEvent("Play_OBJ_weapon_pickup_01", base.gameObject);
			}
			else
			{
				AkSoundEngine.PostEvent("Play_OBJ_item_pickup_01", base.gameObject);
			}
			GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Item_Pickup");
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			tk2dSprite component = gameObject2.GetComponent<tk2dSprite>();
			component.PlaceAtPositionByAnchor(this.m_itemDisplaySprite.WorldCenter, tk2dBaseSprite.Anchor.MiddleCenter);
			component.HeightOffGround = 6f;
			component.UpdateZDepth();
			UnityEngine.Object.Destroy(this.m_itemDisplaySprite);
		}
		if (this.m_registeredIconRoom != null)
		{
			Minimap.Instance.DeregisterRoomIcon(this.m_registeredIconRoom, this.minimapIconInstance);
		}
	}

	// Token: 0x060086DC RID: 34524 RVA: 0x0037E648 File Offset: 0x0037C848
	private void DoMimicTransformation(PickupObject overrideDeathRewards)
	{
		if (this.m_itemDisplaySprite)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
			tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
			component.PlaceAtPositionByAnchor(this.m_itemDisplaySprite.WorldCenter.ToVector3ZUp(0f), tk2dBaseSprite.Anchor.MiddleCenter);
			component.HeightOffGround = 5f;
			component.UpdateZDepth();
		}
		base.sprite.UpdateZDepth();
		IntVector2 intVector = base.specRigidbody.UnitBottomLeft.ToIntVector2(VectorConversions.Floor);
		IntVector2 intVector2 = base.specRigidbody.UnitTopRight.ToIntVector2(VectorConversions.Floor);
		for (int i = intVector.x; i <= intVector2.x; i++)
		{
			for (int j = intVector.y; j <= intVector2.y; j++)
			{
				GameManager.Instance.Dungeon.data[new IntVector2(i, j)].isOccupied = false;
			}
		}
		if (!this.pickedUp)
		{
			this.pickedUp = true;
			this.m_room.DeregisterInteractable(this);
		}
		if (this.m_registeredIconRoom != null)
		{
			Minimap.Instance.DeregisterRoomIcon(this.m_registeredIconRoom, this.minimapIconInstance);
		}
		AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(this.MimicGuid);
		AIActor aiactor = AIActor.Spawn(orLoadByGuid, base.transform.position.XY().ToIntVector2(VectorConversions.Floor), base.GetAbsoluteParentRoom(), false, AIActor.AwakenAnimationType.Default, true);
		if (overrideDeathRewards != null)
		{
			aiactor.AdditionalSafeItemDrops.Add(overrideDeathRewards);
		}
		PickupObject.ItemQuality itemQuality = ((!BraveUtility.RandomBool()) ? PickupObject.ItemQuality.C : PickupObject.ItemQuality.D);
		GenericLootTable genericLootTable = ((!BraveUtility.RandomBool()) ? GameManager.Instance.RewardManager.GunsLootTable : GameManager.Instance.RewardManager.ItemsLootTable);
		PickupObject itemOfTypeAndQuality = LootEngine.GetItemOfTypeAndQuality<PickupObject>(itemQuality, genericLootTable, false);
		if (itemOfTypeAndQuality)
		{
			aiactor.AdditionalSafeItemDrops.Add(itemOfTypeAndQuality);
		}
		aiactor.specRigidbody.Initialize();
		Vector2 unitBottomLeft = aiactor.specRigidbody.UnitBottomLeft;
		Vector2 unitBottomLeft2 = base.specRigidbody.UnitBottomLeft;
		aiactor.transform.position -= unitBottomLeft - unitBottomLeft2;
		aiactor.transform.position += PhysicsEngine.PixelToUnit(this.mimicOffset);
		aiactor.specRigidbody.Reinitialize();
		aiactor.HasDonePlayerEnterCheck = true;
		GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_HAS_BEEN_PEDESTAL_MIMICKED, true);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060086DD RID: 34525 RVA: 0x0037E8DC File Offset: 0x0037CADC
	public void MaybeBecomeMimic()
	{
		this.m_isMimic = false;
		bool flag = false;
		if (!string.IsNullOrEmpty(this.MimicGuid))
		{
			flag |= GameManager.Instance.Dungeon.sharedSettingsPrefab.RandomShouldBecomePedestalMimic(this.overrideMimicChance);
			GameLevelDefinition lastLoadedLevelDefinition = GameManager.Instance.GetLastLoadedLevelDefinition();
			flag |= lastLoadedLevelDefinition != null && lastLoadedLevelDefinition.lastSelectedFlowEntry != null && lastLoadedLevelDefinition.lastSelectedFlowEntry.levelMode == FlowLevelEntryMode.ALL_MIMICS;
			if (this.m_room != null)
			{
				string roomName = this.m_room.GetRoomName();
				if (roomName.StartsWith("DemonWallRoom"))
				{
					flag = false;
				}
				if (roomName.StartsWith("DoubleBeholsterRoom"))
				{
					flag = BraveUtility.RandomBool();
				}
			}
			if (flag)
			{
				if (PassiveItem.IsFlagSetAtAll(typeof(MimicRingItem)))
				{
					return;
				}
				this.m_isMimic = true;
				if (base.gameObject.activeInHierarchy)
				{
					base.StartCoroutine(this.MimicIdleAnimCR());
				}
			}
		}
	}

	// Token: 0x060086DE RID: 34526 RVA: 0x0037E9D0 File Offset: 0x0037CBD0
	private IEnumerator MimicIdleAnimCR()
	{
		this.m_isMimicBreathing = true;
		tk2dSpriteAnimationClip clip = base.spriteAnimator.GetClipByName(this.preMimicIdleAnim);
		tk2dSpriteAnimationFrame finalFrame = clip.GetFrame(clip.frames.Length - 1);
		base.spriteAnimator.sprite.SetSprite(finalFrame.spriteCollection, finalFrame.spriteId);
		while (this.m_isMimic)
		{
			yield return new WaitForSeconds(this.preMimicIdleAnimDelay);
			if (!this.m_isMimic)
			{
				yield break;
			}
			base.spriteAnimator.Play(this.preMimicIdleAnim);
			while (base.spriteAnimator.IsPlaying(this.preMimicIdleAnim))
			{
				if (!this.m_isMimic)
				{
					yield break;
				}
				yield return null;
			}
		}
		this.m_isMimicBreathing = false;
		yield break;
	}

	// Token: 0x060086DF RID: 34527 RVA: 0x0037E9EC File Offset: 0x0037CBEC
	private void OnDamaged(float damage)
	{
		if (this.m_isMimic)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.PREFIRE_ON_MIMIC, 0);
			this.DoMimicTransformation(this.contents);
		}
	}

	// Token: 0x060086E0 RID: 34528 RVA: 0x0037EA18 File Offset: 0x0037CC18
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		if (this.m_itemDisplaySprite != null)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(this.m_itemDisplaySprite, true);
			SpriteOutlineManager.AddOutlineToSprite(this.m_itemDisplaySprite, Color.white, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		}
		base.sprite.UpdateZDepth();
	}

	// Token: 0x060086E1 RID: 34529 RVA: 0x0037EA74 File Offset: 0x0037CC74
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		if (this.m_itemDisplaySprite != null)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(this.m_itemDisplaySprite, true);
			SpriteOutlineManager.AddOutlineToSprite(this.m_itemDisplaySprite, Color.black, 0.1f, 0.05f, SpriteOutlineManager.OutlineType.NORMAL);
		}
		base.sprite.UpdateZDepth();
	}

	// Token: 0x060086E2 RID: 34530 RVA: 0x0037EAD0 File Offset: 0x0037CCD0
	public float GetDistanceToPoint(Vector2 point)
	{
		Bounds bounds = base.sprite.GetBounds();
		bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
	}

	// Token: 0x060086E3 RID: 34531 RVA: 0x0037EBB0 File Offset: 0x0037CDB0
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x060086E4 RID: 34532 RVA: 0x0037EBB8 File Offset: 0x0037CDB8
	public void Interact(PlayerController player)
	{
		if (!this.pickedUp)
		{
			this.m_room.DeregisterInteractable(this);
			if (this.m_itemDisplaySprite != null)
			{
				SpriteOutlineManager.RemoveOutlineFromSprite(this.m_itemDisplaySprite, false);
			}
			this.DoPickup(player);
		}
	}

	// Token: 0x060086E5 RID: 34533 RVA: 0x0037EBF8 File Offset: 0x0037CDF8
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x060086E6 RID: 34534 RVA: 0x0037EC04 File Offset: 0x0037CE04
	public void ForceConfiguration()
	{
		this.DetermineContents(GameManager.Instance.PrimaryPlayer);
	}

	// Token: 0x060086E7 RID: 34535 RVA: 0x0037EC18 File Offset: 0x0037CE18
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.m_room = room;
		this.RegisterChestOnMinimap(room);
		if (!this.UsesDelayedConfiguration)
		{
			this.ForceConfiguration();
		}
	}

	// Token: 0x060086E8 RID: 34536 RVA: 0x0037EC3C File Offset: 0x0037CE3C
	protected override void OnDestroy()
	{
		if (base.majorBreakable)
		{
			MajorBreakable majorBreakable = base.majorBreakable;
			majorBreakable.OnDamaged = (Action<float>)Delegate.Remove(majorBreakable.OnDamaged, new Action<float>(this.OnDamaged));
		}
		base.OnDestroy();
	}

	// Token: 0x04008BFE RID: 35838
	[NonSerialized]
	public PickupObject contents;

	// Token: 0x04008BFF RID: 35839
	public bool UsesSpecificItem;

	// Token: 0x04008C00 RID: 35840
	[PickupIdentifier]
	[ShowInInspectorIf("UsesSpecificItem", true)]
	public int SpecificItemId = -1;

	// Token: 0x04008C01 RID: 35841
	[HideInInspectorIf("UsesSpecificItem", false)]
	public LootData lootTable;

	// Token: 0x04008C02 RID: 35842
	public bool UsesDelayedConfiguration;

	// Token: 0x04008C03 RID: 35843
	public Transform spawnTransform;

	// Token: 0x04008C04 RID: 35844
	[FormerlySerializedAs("spawnsHearts")]
	public bool SpawnsTertiarySet = true;

	// Token: 0x04008C06 RID: 35846
	[CheckAnimation(null)]
	public string spawnAnimName;

	// Token: 0x04008C07 RID: 35847
	public GameObject VFX_PreSpawn;

	// Token: 0x04008C08 RID: 35848
	public GameObject VFX_GroundHit;

	// Token: 0x04008C09 RID: 35849
	public float groundHitDelay = 0.73f;

	// Token: 0x04008C0A RID: 35850
	[NonSerialized]
	public bool pickedUp;

	// Token: 0x04008C0B RID: 35851
	[NonSerialized]
	public bool ReturnCoopPlayerOnLand;

	// Token: 0x04008C0C RID: 35852
	[NonSerialized]
	public bool IsBossRewardPedestal;

	// Token: 0x04008C0D RID: 35853
	private GameObject minimapIconInstance;

	// Token: 0x04008C0E RID: 35854
	private RoomHandler m_room;

	// Token: 0x04008C0F RID: 35855
	private RoomHandler m_registeredIconRoom;

	// Token: 0x04008C10 RID: 35856
	private tk2dBaseSprite m_itemDisplaySprite;

	// Token: 0x04008C11 RID: 35857
	private bool m_isMimic;

	// Token: 0x04008C12 RID: 35858
	private bool m_isMimicBreathing;

	// Token: 0x04008C13 RID: 35859
	[Header("Mimic")]
	[EnemyIdentifier]
	public string MimicGuid;

	// Token: 0x04008C14 RID: 35860
	public IntVector2 mimicOffset;

	// Token: 0x04008C15 RID: 35861
	[CheckAnimation(null)]
	public string preMimicIdleAnim;

	// Token: 0x04008C16 RID: 35862
	public float preMimicIdleAnimDelay = 3f;

	// Token: 0x04008C17 RID: 35863
	public float overrideMimicChance = -1f;

	// Token: 0x04008C18 RID: 35864
	private const float SPAWN_PUSH_RADIUS = 5f;

	// Token: 0x04008C19 RID: 35865
	private const float SPAWN_PUSH_FORCE = 22f;
}
