using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000E43 RID: 3651
public class Chest : DungeonPlaceableBehaviour, IPlaceConfigurable, IPlayerInteractable
{
	// Token: 0x17000AEF RID: 2799
	// (get) Token: 0x06004D7C RID: 19836 RVA: 0x001A9150 File Offset: 0x001A7350
	// (set) Token: 0x06004D7D RID: 19837 RVA: 0x001A9158 File Offset: 0x001A7358
	public Chest.GeneralChestType ChestType
	{
		get
		{
			return this.m_chestType;
		}
		set
		{
			if (this.m_chestType == Chest.GeneralChestType.WEAPON)
			{
				StaticReferenceManager.WeaponChestsSpawnedOnFloor--;
			}
			else if (this.m_chestType == Chest.GeneralChestType.ITEM)
			{
				StaticReferenceManager.ItemChestsSpawnedOnFloor--;
			}
			this.m_chestType = value;
			if (this.m_chestType == Chest.GeneralChestType.WEAPON)
			{
				StaticReferenceManager.WeaponChestsSpawnedOnFloor++;
			}
			else if (this.m_chestType == Chest.GeneralChestType.ITEM)
			{
				StaticReferenceManager.ItemChestsSpawnedOnFloor++;
			}
		}
	}

	// Token: 0x17000AF0 RID: 2800
	// (get) Token: 0x06004D7E RID: 19838 RVA: 0x001A91D8 File Offset: 0x001A73D8
	public bool TemporarilyUnopenable
	{
		get
		{
			return this.m_temporarilyUnopenable;
		}
	}

	// Token: 0x17000AF1 RID: 2801
	// (get) Token: 0x06004D7F RID: 19839 RVA: 0x001A91E0 File Offset: 0x001A73E0
	public bool IsTruthChest
	{
		get
		{
			return base.name.Contains("TruthChest");
		}
	}

	// Token: 0x17000AF2 RID: 2802
	// (get) Token: 0x06004D80 RID: 19840 RVA: 0x001A91F4 File Offset: 0x001A73F4
	public bool IsMimic
	{
		get
		{
			return this.m_isMimic;
		}
	}

	// Token: 0x06004D81 RID: 19841 RVA: 0x001A91FC File Offset: 0x001A73FC
	public static Chest Spawn(Chest chestPrefab, IntVector2 basePosition)
	{
		RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(basePosition);
		return Chest.Spawn(chestPrefab, basePosition, roomFromPosition, false);
	}

	// Token: 0x06004D82 RID: 19842 RVA: 0x001A9224 File Offset: 0x001A7424
	public static Chest Spawn(Chest chestPrefab, IntVector2 basePosition, RoomHandler room, bool ForceNoMimic = false)
	{
		return Chest.Spawn(chestPrefab, basePosition.ToVector3(), room, ForceNoMimic);
	}

	// Token: 0x06004D83 RID: 19843 RVA: 0x001A9238 File Offset: 0x001A7438
	public static Chest Spawn(Chest chestPrefab, Vector3 basePosition, RoomHandler room, bool ForceNoMimic = false)
	{
		if (chestPrefab == null)
		{
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(chestPrefab.gameObject, basePosition, Quaternion.identity);
		Chest component = gameObject.GetComponent<Chest>();
		if (ForceNoMimic)
		{
			component.MimicGuid = null;
		}
		component.m_room = room;
		component.HandleSpawnBehavior();
		return component;
	}

	// Token: 0x06004D84 RID: 19844 RVA: 0x001A9288 File Offset: 0x001A7488
	public static void ToggleCoopChests(bool coopDead)
	{
		Chest.m_IsCoopMode = coopDead;
		for (int i = 0; i < StaticReferenceManager.AllChests.Count; i++)
		{
			if (coopDead)
			{
				StaticReferenceManager.AllChests[i].BecomeCoopChest();
			}
			else
			{
				StaticReferenceManager.AllChests[i].UnbecomeCoopChest();
			}
		}
	}

	// Token: 0x06004D85 RID: 19845 RVA: 0x001A92E4 File Offset: 0x001A74E4
	public void RegisterChestOnMinimap(RoomHandler r)
	{
		this.m_registeredIconRoom = r;
		GameObject gameObject = this.MinimapIconPrefab ?? (BraveResources.Load("Global Prefabs/Minimap_Treasure_Icon", ".prefab") as GameObject);
		this.minimapIconInstance = Minimap.Instance.RegisterRoomIcon(r, gameObject, false);
	}

	// Token: 0x06004D86 RID: 19846 RVA: 0x001A9330 File Offset: 0x001A7530
	public void DeregisterChestOnMinimap()
	{
		if (this.m_registeredIconRoom != null)
		{
			Minimap.Instance.DeregisterRoomIcon(this.m_registeredIconRoom, this.minimapIconInstance);
		}
	}

	// Token: 0x17000AF3 RID: 2803
	// (get) Token: 0x06004D87 RID: 19847 RVA: 0x001A9354 File Offset: 0x001A7554
	private Color BaseOutlineColor
	{
		get
		{
			if (this.m_isMimic && !Dungeon.IsGenerating)
			{
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					if (GameManager.Instance.AllPlayers[i] && GameManager.Instance.AllPlayers[i].CanDetectHiddenEnemies)
					{
						return Color.red;
					}
				}
			}
			return Color.black;
		}
	}

	// Token: 0x06004D88 RID: 19848 RVA: 0x001A93CC File Offset: 0x001A75CC
	private void Awake()
	{
		if (this.IsTruthChest)
		{
			this.PreventFuse = true;
		}
		StaticReferenceManager.AllChests.Add(this);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, this.BaseOutlineColor, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		tk2dSpriteAnimator spriteAnimator = base.spriteAnimator;
		spriteAnimator.AnimationEventTriggered = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>)Delegate.Combine(spriteAnimator.AnimationEventTriggered, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.HandleCoopChestAnimationEvent));
		MajorBreakable majorBreakable = base.majorBreakable;
		majorBreakable.OnDamaged = (Action<float>)Delegate.Combine(majorBreakable.OnDamaged, new Action<float>(this.OnDamaged));
		if (base.majorBreakable.DamageReduction > 1000f)
		{
			base.majorBreakable.ReportZeroDamage = true;
		}
		base.majorBreakable.InvulnerableToEnemyBullets = true;
		this.m_runtimeRandom = new System.Random();
	}

	// Token: 0x06004D89 RID: 19849 RVA: 0x001A9498 File Offset: 0x001A7698
	private void OnEnable()
	{
		if (this.m_isMimic && !this.m_isMimicBreathing)
		{
			base.StartCoroutine(this.MimicIdleAnimCR());
		}
	}

	// Token: 0x06004D8A RID: 19850 RVA: 0x001A94C0 File Offset: 0x001A76C0
	private void HandleCoopChestAnimationEvent(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameNo)
	{
		tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNo);
		if (frame.eventInfo == "coopchestvfx")
		{
			UnityEngine.Object.Instantiate(BraveResources.Load("Global VFX/VFX_ChestKnock_001", ".prefab"), base.sprite.WorldCenter + new Vector2(0f, 0.3125f), Quaternion.identity);
		}
	}

	// Token: 0x06004D8B RID: 19851 RVA: 0x001A9528 File Offset: 0x001A7728
	protected void HandleSpawnBehavior()
	{
		base.StartCoroutine(this.SpawnBehavior_CR());
	}

	// Token: 0x06004D8C RID: 19852 RVA: 0x001A9538 File Offset: 0x001A7738
	private IEnumerator SpawnBehavior_CR()
	{
		if (base.majorBreakable)
		{
			base.majorBreakable.TemporarilyInvulnerable = true;
		}
		this.Initialize();
		this.m_cachedSpriteForCoop = base.sprite.spriteId;
		this.m_temporarilyUnopenable = true;
		if (Chest.m_IsCoopMode)
		{
			this.BecomeCoopChest();
		}
		if (this.VFX_PreSpawn != null)
		{
			base.renderer.enabled = false;
			if (this.LockAnimator != null)
			{
				this.LockAnimator.GetComponent<Renderer>().enabled = false;
			}
			this.VFX_PreSpawn.SetActive(true);
			yield return new WaitForSeconds(0.1f);
			base.renderer.enabled = true;
			if (this.LockAnimator != null && this.IsLocked)
			{
				this.LockAnimator.GetComponent<Renderer>().enabled = true;
			}
		}
		string targetSpawnAnimName = (string.IsNullOrEmpty(this.overrideSpawnAnimName) ? this.spawnAnimName : this.overrideSpawnAnimName);
		if (!string.IsNullOrEmpty(targetSpawnAnimName))
		{
			tk2dSpriteAnimationClip clip = base.spriteAnimator.GetClipByName(targetSpawnAnimName);
			if (clip != null)
			{
				this.m_temporarilyUnopenable = true;
				base.specRigidbody.enabled = false;
				float clipTime = (float)clip.frames.Length / clip.fps;
				base.spriteAnimator.Play(targetSpawnAnimName);
				base.sprite.UpdateZDepth();
				float elapsed = 0f;
				bool groundHitTriggered = false;
				while (elapsed < clipTime)
				{
					elapsed += BraveTime.DeltaTime;
					if (elapsed >= this.groundHitDelay && !groundHitTriggered)
					{
						groundHitTriggered = true;
						Exploder.DoRadialPush(base.sprite.WorldCenter.ToVector3ZUp(base.sprite.WorldCenter.y), 22f, 5f);
						this.VFX_GroundHit.SetActive(true);
						base.specRigidbody.enabled = true;
						List<CollisionData> list = new List<CollisionData>();
						PhysicsEngine.Instance.OverlapCast(base.specRigidbody, list, false, true, null, null, false, null, null, new SpeculativeRigidbody[0]);
						for (int i = 0; i < list.Count; i++)
						{
							CollisionData collisionData = list[i];
							if (collisionData.OtherRigidbody && collisionData.OtherRigidbody.minorBreakable)
							{
								collisionData.OtherRigidbody.minorBreakable.Break(collisionData.OtherRigidbody.UnitCenter - base.specRigidbody.UnitCenter);
							}
						}
					}
					yield return null;
				}
			}
		}
		base.sprite.UpdateZDepth();
		this.m_room.RegisterInteractable(this);
		PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(base.specRigidbody, null, false);
		this.m_temporarilyUnopenable = false;
		if (base.majorBreakable)
		{
			base.majorBreakable.TemporarilyInvulnerable = false;
		}
		if (Chest.m_IsCoopMode)
		{
			if (this.LockAnimator != null)
			{
				this.LockAnimator.renderer.enabled = false;
			}
			base.spriteAnimator.Play("coop_chest_knock");
		}
		this.PossiblyCreateBowler(true);
		yield break;
	}

	// Token: 0x06004D8D RID: 19853 RVA: 0x001A9554 File Offset: 0x001A7754
	private void PossiblyCreateBowler(bool mightBeActive)
	{
		if (this.m_hasCheckedBowler)
		{
			return;
		}
		this.m_hasCheckedBowler = true;
		if (!this.IsRainbowChest && GameStatsManager.HasInstance && GameStatsManager.Instance.IsRainbowRun)
		{
			bool flag = true;
			for (int i = 0; i < StaticReferenceManager.AllChests.Count; i++)
			{
				if (StaticReferenceManager.AllChests[i] && !StaticReferenceManager.AllChests[i].IsRainbowChest && StaticReferenceManager.AllChests[i].GetAbsoluteParentRoom() == base.GetAbsoluteParentRoom() && StaticReferenceManager.AllChests[i] != this)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				bool flag2 = this.breakAnimName.Contains("redgold") || this.breakAnimName.Contains("black");
				this.m_bowlerInstance = (GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/VFX_BowlerSit"));
				this.m_bowlerInstance.transform.parent = base.transform;
				tk2dBaseSprite component = this.m_bowlerInstance.GetComponent<tk2dBaseSprite>();
				if (component)
				{
					SpriteOutlineManager.AddOutlineToSprite(component, Color.black, 0.05f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
				}
				if (UnityEngine.Random.value < 0.01f)
				{
					this.m_bowlerInstance.GetComponent<tk2dSpriteAnimator>().Play("salute_right");
					if (flag2)
					{
						this.m_bowlerInstance.transform.localPosition = new Vector3(0f, 0.625f);
						this.m_bowlerInstance.GetComponent<tk2dBaseSprite>().HeightOffGround = 0.5f;
					}
					else
					{
						this.m_bowlerInstance.transform.localPosition = new Vector3(-0.4375f, 0.125f);
						this.m_bowlerInstance.GetComponent<tk2dBaseSprite>().HeightOffGround = -0.75f;
					}
					this.m_bowlerFireStatus = Tribool.Ready;
				}
				else
				{
					this.m_bowlerInstance.GetComponent<tk2dSpriteAnimator>().Play("sit_right");
					if (flag2)
					{
						this.m_bowlerInstance.transform.localPosition = new Vector3(0f, 0.625f);
						this.m_bowlerInstance.GetComponent<tk2dBaseSprite>().HeightOffGround = 0.5f;
					}
					else
					{
						this.m_bowlerInstance.transform.localPosition = new Vector3(-0.25f, -0.3125f);
						this.m_bowlerInstance.GetComponent<tk2dBaseSprite>().HeightOffGround = -1.5f;
					}
					this.m_bowlerFireStatus = Tribool.Unready;
				}
				if (mightBeActive)
				{
					LootEngine.DoDefaultPurplePoof(this.m_bowlerInstance.GetComponent<tk2dBaseSprite>().WorldCenter, false);
				}
			}
		}
	}

	// Token: 0x06004D8E RID: 19854 RVA: 0x001A97FC File Offset: 0x001A79FC
	public void BecomeRainbowChest()
	{
		this.IsRainbowChest = true;
		this.lootTable.S_Chance = 0.2f;
		this.lootTable.A_Chance = 0.7f;
		this.lootTable.B_Chance = 0.4f;
		this.lootTable.C_Chance = 0f;
		this.lootTable.D_Chance = 0f;
		this.lootTable.Common_Chance = 0f;
		this.lootTable.canDropMultipleItems = true;
		this.lootTable.multipleItemDropChances = new WeightedIntCollection();
		this.lootTable.multipleItemDropChances.elements = new WeightedInt[1];
		this.lootTable.overrideItemLootTables = new List<GenericLootTable>();
		this.lootTable.lootTable = GameManager.Instance.RewardManager.GunsLootTable;
		this.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
		this.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
		this.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
		this.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
		this.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
		this.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
		this.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
		this.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
		if (GameStatsManager.Instance.IsRainbowRun)
		{
			this.lootTable.C_Chance = 0.2f;
			this.lootTable.D_Chance = 0.2f;
			this.lootTable.overrideItemQualities = new List<PickupObject.ItemQuality>();
			float value = UnityEngine.Random.value;
			if (value < 0.5f)
			{
				this.lootTable.overrideItemQualities.Add(PickupObject.ItemQuality.S);
				this.lootTable.overrideItemQualities.Add(PickupObject.ItemQuality.A);
			}
			else
			{
				this.lootTable.overrideItemQualities.Add(PickupObject.ItemQuality.A);
				this.lootTable.overrideItemQualities.Add(PickupObject.ItemQuality.S);
			}
		}
		WeightedInt weightedInt = new WeightedInt();
		weightedInt.value = 8;
		weightedInt.weight = 1f;
		weightedInt.additionalPrerequisites = new DungeonPrerequisite[0];
		this.lootTable.multipleItemDropChances.elements[0] = weightedInt;
		this.lootTable.onlyOneGunCanDrop = false;
		if (this.ChestIdentifier == Chest.SpecialChestIdentifier.SECRET_RAINBOW)
		{
			this.spawnAnimName = "wood_chest_appear";
			this.openAnimName = "wood_chest_open";
			this.breakAnimName = "wood_chest_break";
		}
		else
		{
			this.spawnAnimName = "redgold_chest_appear";
			this.openAnimName = "redgold_chest_open";
			this.breakAnimName = "redgold_chest_break";
			base.majorBreakable.spriteNameToUseAtZeroHP = "chest_redgold_break_001";
		}
		base.sprite.usesOverrideMaterial = true;
		tk2dSpriteAnimationClip clipByName = base.spriteAnimator.GetClipByName(this.spawnAnimName);
		base.sprite.SetSprite(clipByName.frames[clipByName.frames.Length - 1].spriteId);
		if (this.ChestIdentifier != Chest.SpecialChestIdentifier.SECRET_RAINBOW)
		{
			if (this.LockAnimator)
			{
				UnityEngine.Object.Destroy(this.LockAnimator.gameObject);
				this.LockAnimator = null;
			}
			this.IsLocked = false;
			base.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/RainbowChestShader");
		}
	}

	// Token: 0x06004D8F RID: 19855 RVA: 0x001A9B9C File Offset: 0x001A7D9C
	public void RevealSecretRainbow()
	{
		if (this.m_secretRainbowRevealed)
		{
			return;
		}
		this.m_secretRainbowRevealed = true;
		base.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/RainbowChestShader");
		base.sprite.renderer.material.SetFloat("_HueTestValue", -3.5f);
		if (this.LockAnimator)
		{
			UnityEngine.Object.Destroy(this.LockAnimator.gameObject);
			this.LockAnimator = null;
		}
		this.IsLocked = false;
	}

	// Token: 0x06004D90 RID: 19856 RVA: 0x001A9C28 File Offset: 0x001A7E28
	protected void Initialize()
	{
		if (Chest.m_IsCoopMode && GameManager.Instance.CurrentGameType != GameManager.GameType.COOP_2_PLAYER)
		{
			Chest.m_IsCoopMode = false;
		}
		base.specRigidbody.Initialize();
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreRigidbodyCollision));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnRigidbodyCollision = (SpeculativeRigidbody.OnRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody2.OnRigidbodyCollision, new SpeculativeRigidbody.OnRigidbodyCollisionDelegate(this.OnRigidbodyCollision));
		SpeculativeRigidbody specRigidbody3 = base.specRigidbody;
		specRigidbody3.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody3.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.OnTriggerCollision));
		base.specRigidbody.PreventPiercing = true;
		MajorBreakable component = base.GetComponent<MajorBreakable>();
		if (component != null)
		{
			MajorBreakable majorBreakable = component;
			majorBreakable.OnBreak = (Action)Delegate.Combine(majorBreakable.OnBreak, new Action(this.OnBroken));
		}
		IntVector2 intVector = base.specRigidbody.UnitBottomLeft.ToIntVector2(VectorConversions.Floor);
		IntVector2 intVector2 = base.specRigidbody.UnitTopRight.ToIntVector2(VectorConversions.Floor);
		for (int i = intVector.x; i <= intVector2.x; i++)
		{
			for (int j = intVector.y; j <= intVector2.y; j++)
			{
				GameManager.Instance.Dungeon.data[new IntVector2(i, j)].isOccupied = true;
			}
		}
		bool flag = UnityEngine.Random.value < 0.000333f;
		if (this.ChestIdentifier == Chest.SpecialChestIdentifier.RAT || (this.lootTable != null && this.lootTable.CompletesSynergy))
		{
			flag = false;
		}
		else if (!flag && this.spawnAnimName.StartsWith("wood_") && GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.WOODEN_CHESTS_BROKEN) >= 5f && UnityEngine.Random.value < 0.000333f)
		{
			flag = true;
			this.ChestIdentifier = Chest.SpecialChestIdentifier.SECRET_RAINBOW;
		}
		if (this.IsMirrorChest)
		{
			base.sprite.renderer.enabled = false;
			if (this.LockAnimator)
			{
				this.LockAnimator.Sprite.renderer.enabled = false;
			}
			if (this.ShadowSprite)
			{
				this.ShadowSprite.renderer.enabled = false;
			}
			base.specRigidbody.enabled = false;
		}
		else if (this.IsRainbowChest || flag)
		{
			this.BecomeRainbowChest();
		}
		else if (this.ForceGlitchChest || ((GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.GUNGEON || GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON || GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON) && GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.NUMBER_ATTEMPTS) > 10f && GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_BEHOLSTER) && !GameManager.Instance.InTutorial && UnityEngine.Random.value < 0.001f))
		{
			this.BecomeGlitchChest();
		}
		else if (!Chest.m_IsCoopMode)
		{
			this.MaybeBecomeMimic();
		}
	}

	// Token: 0x06004D91 RID: 19857 RVA: 0x001A9F74 File Offset: 0x001A8174
	private void Update()
	{
		if (this.m_isMimic && !this.m_temporarilyUnopenable && base.sprite)
		{
			Color baseOutlineColor = this.BaseOutlineColor;
			if (baseOutlineColor != this.m_cachedOutlineColor)
			{
				this.m_cachedOutlineColor = baseOutlineColor;
				SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
				SpriteOutlineManager.AddOutlineToSprite(base.sprite, baseOutlineColor, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			}
		}
		if (this.m_bowlerInstance)
		{
			if (this.m_bowlerFireStatus == Tribool.Ready)
			{
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					float num = Vector2.Distance(GameManager.Instance.AllPlayers[i].CenterPosition, this.m_bowlerInstance.transform.position);
					if (num < 5f)
					{
						this.m_bowlerFireStatus = Tribool.Complete;
						AkSoundEngine.PostEvent("Play_obj_bowler_ignite_01", base.gameObject);
						AkSoundEngine.PostEvent("Play_obj_bowler_burn_01", base.gameObject);
					}
				}
			}
			else if (this.m_bowlerFireStatus == Tribool.Complete)
			{
				this.m_bowlerFireTimer += BraveTime.DeltaTime * 15f;
				if (this.m_bowlerFireTimer > 1f)
				{
					GlobalSparksDoer.DoRadialParticleBurst(Mathf.FloorToInt(this.m_bowlerFireTimer), this.m_bowlerInstance.GetComponent<tk2dBaseSprite>().WorldBottomLeft + new Vector2(0.125f, 0.1875f), this.m_bowlerInstance.GetComponent<tk2dBaseSprite>().WorldTopRight - new Vector2(0.125f, 0.125f), 0f, 0f, 0f, null, null, null, GlobalSparksDoer.SparksType.STRAIGHT_UP_FIRE);
					if (base.sprite)
					{
						GlobalSparksDoer.DoRadialParticleBurst(Mathf.FloorToInt(this.m_bowlerFireTimer * 3f), base.sprite.WorldBottomLeft + new Vector2(0.125f, 0.1875f), base.sprite.WorldTopRight - new Vector2(0.125f, 0.125f), 0f, 0f, 0f, null, null, null, GlobalSparksDoer.SparksType.STRAIGHT_UP_FIRE);
					}
					this.m_bowlerFireTimer %= 1f;
				}
			}
		}
	}

	// Token: 0x06004D92 RID: 19858 RVA: 0x001AA20C File Offset: 0x001A840C
	protected void TriggerCountdownTimer()
	{
		if (!this)
		{
			return;
		}
		GameObject gameObject = (GameObject)BraveResources.Load("Chest_Fuse", ".prefab");
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, base.transform.position + new Vector3(-1.75f, -1.5f, 0f), Quaternion.identity);
		this.extantFuse = gameObject2.GetComponent<ChestFuseController>();
		base.StartCoroutine(this.HandleExplosionCountdown(this.extantFuse));
	}

	// Token: 0x06004D93 RID: 19859 RVA: 0x001AA28C File Offset: 0x001A848C
	public void ForceKillFuse()
	{
		if (this.extantFuse)
		{
			AkSoundEngine.PostEvent("stop_obj_fuse_loop_01", base.gameObject);
			this.extantFuse = null;
		}
	}

	// Token: 0x06004D94 RID: 19860 RVA: 0x001AA2B8 File Offset: 0x001A84B8
	private IEnumerator HandleExplosionCountdown(ChestFuseController fuse)
	{
		float elapsed = 0f;
		float timer = 10f;
		while (elapsed < timer)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / timer;
			if (this.IsBroken || this.IsOpen || this.PreventFuse)
			{
				yield break;
			}
			if (!this.extantFuse)
			{
				yield break;
			}
			Vector2? sparkPos = fuse.SetFuseCompletion(t);
			if (sparkPos != null)
			{
				IntVector2 intVector = (sparkPos.Value / DeadlyDeadlyGoopManager.GOOP_GRID_SIZE).ToIntVector2(VectorConversions.Floor);
				if (DeadlyDeadlyGoopManager.allGoopPositionMap.ContainsKey(intVector))
				{
					DeadlyDeadlyGoopManager deadlyDeadlyGoopManager = DeadlyDeadlyGoopManager.allGoopPositionMap[intVector];
					GoopDefinition goopDefinition = deadlyDeadlyGoopManager.goopDefinition;
					if (!goopDefinition.CanBeIgnited)
					{
						AkSoundEngine.PostEvent("stop_obj_fuse_loop_01", base.gameObject);
						yield break;
					}
					deadlyDeadlyGoopManager.IgniteGoopCircle(sparkPos.Value, 0.5f);
				}
			}
			yield return null;
		}
		if (!this.IsOpen && !this.IsBroken)
		{
			this.m_isMimic = false;
			this.ExplodeInSadness();
		}
		yield break;
	}

	// Token: 0x06004D95 RID: 19861 RVA: 0x001AA2DC File Offset: 0x001A84DC
	private void ExplodeInSadness()
	{
		MajorBreakable component = base.GetComponent<MajorBreakable>();
		this.GetRidOfBowler();
		if (component != null)
		{
			MajorBreakable majorBreakable = component;
			majorBreakable.OnBreak = (Action)Delegate.Remove(majorBreakable.OnBreak, new Action(this.OnBroken));
		}
		base.spriteAnimator.Play(string.IsNullOrEmpty(this.overrideBreakAnimName) ? this.breakAnimName : this.overrideBreakAnimName);
		base.specRigidbody.enabled = false;
		this.IsBroken = true;
		if (this.LockAnimator != null && this.LockAnimator)
		{
			UnityEngine.Object.Destroy(this.LockAnimator.gameObject);
		}
		Transform transform = base.transform.Find("Shadow");
		if (transform != null)
		{
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		this.pickedUp = true;
		this.HandleGeneratedMagnificence();
		if (this.m_registeredIconRoom != null)
		{
			Minimap.Instance.DeregisterRoomIcon(this.m_registeredIconRoom, this.minimapIconInstance);
		}
		this.m_room.DeregisterInteractable(this);
		Exploder.DoDefaultExplosion(base.sprite.WorldCenter, Vector2.zero, null, false, CoreDamageTypes.None, false);
	}

	// Token: 0x06004D96 RID: 19862 RVA: 0x001AA414 File Offset: 0x001A8614
	private void OnBroken()
	{
		this.GetRidOfBowler();
		if (this.ChestIdentifier == Chest.SpecialChestIdentifier.SECRET_RAINBOW)
		{
			this.RevealSecretRainbow();
		}
		if (this.ChestIdentifier == Chest.SpecialChestIdentifier.SECRET_RAINBOW || this.IsRainbowChest || this.breakAnimName.Contains("redgold") || this.breakAnimName.Contains("black"))
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_GOLD_JUNK, true);
		}
		base.spriteAnimator.Play(string.IsNullOrEmpty(this.overrideBreakAnimName) ? this.breakAnimName : this.overrideBreakAnimName);
		base.specRigidbody.enabled = false;
		this.IsBroken = true;
		IntVector2 intVector = base.specRigidbody.UnitBottomLeft.ToIntVector2(VectorConversions.Floor);
		IntVector2 intVector2 = base.specRigidbody.UnitTopRight.ToIntVector2(VectorConversions.Floor);
		for (int i = intVector.x; i <= intVector2.x; i++)
		{
			for (int j = intVector.y; j <= intVector2.y; j++)
			{
				GameManager.Instance.Dungeon.data[new IntVector2(i, j)].isOccupied = false;
			}
		}
		if (this.LockAnimator != null && this.LockAnimator)
		{
			UnityEngine.Object.Destroy(this.LockAnimator.gameObject);
		}
		Transform transform = base.transform.Find("Shadow");
		if (transform != null)
		{
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		if (!this.pickedUp)
		{
			this.pickedUp = true;
			this.HandleGeneratedMagnificence();
			this.m_room.DeregisterInteractable(this);
			if (this.m_registeredIconRoom != null)
			{
				Minimap.Instance.DeregisterRoomIcon(this.m_registeredIconRoom, this.minimapIconInstance);
			}
			if (this.spawnAnimName.StartsWith("wood_"))
			{
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.WOODEN_CHESTS_BROKEN, 1f);
			}
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.NumberOfLivingPlayers == 1)
			{
				base.StartCoroutine(this.GiveCoopPartnerBack(false));
			}
			else
			{
				bool flag = PassiveItem.IsFlagSetAtAll(typeof(ChestBrokenImprovementItem));
				bool flag2 = GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_GOLD_JUNK);
				float num = GameManager.Instance.RewardManager.ChestDowngradeChance;
				float num2 = GameManager.Instance.RewardManager.ChestHalfHeartChance;
				float num3 = GameManager.Instance.RewardManager.ChestExplosionChance;
				float num4 = GameManager.Instance.RewardManager.ChestJunkChance;
				float num5 = ((!flag2) ? 0f : 0.005f);
				bool flag3 = GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_SER_JUNKAN_UNLOCKED);
				float num6 = ((!flag3 || Chest.HasDroppedSerJunkanThisSession) ? 0f : GameManager.Instance.RewardManager.ChestJunkanUnlockedChance);
				if (GameManager.Instance.PrimaryPlayer && GameManager.Instance.PrimaryPlayer.carriedConsumables.KeyBullets > 0)
				{
					num4 *= GameManager.Instance.RewardManager.HasKeyJunkMultiplier;
				}
				if (SackKnightController.HasJunkan())
				{
					num4 *= GameManager.Instance.RewardManager.HasJunkanJunkMultiplier;
					num5 *= 3f;
				}
				if (this.IsTruthChest)
				{
					num = 0f;
					num2 = 0f;
					num3 = 0f;
					num4 = 1f;
				}
				num4 -= num5;
				float num7 = num5 + num + num2 + num3 + num4 + num6;
				float num8 = UnityEngine.Random.value * num7;
				if (flag2 && num8 < num5)
				{
					this.contents = new List<PickupObject>();
					int goldJunk = GlobalItemIds.GoldJunk;
					this.contents.Add(PickupObjectDatabase.GetById(goldJunk));
					this.m_forceDropOkayForRainbowRun = true;
					base.StartCoroutine(this.PresentItem());
				}
				else if (num8 < num || flag)
				{
					int num9 = -4;
					bool flag4 = false;
					if (flag)
					{
						float value = UnityEngine.Random.value;
						if (value < ChestBrokenImprovementItem.PickupQualChance)
						{
							flag4 = true;
							this.contents = new List<PickupObject>();
							PickupObject pickupObject = null;
							while (pickupObject == null)
							{
								GameObject gameObject = GameManager.Instance.RewardManager.CurrentRewardData.SingleItemRewardTable.SelectByWeight(false);
								if (gameObject)
								{
									pickupObject = gameObject.GetComponent<PickupObject>();
								}
							}
							this.contents.Add(pickupObject);
							base.StartCoroutine(this.PresentItem());
						}
						else if (value < ChestBrokenImprovementItem.PickupQualChance + ChestBrokenImprovementItem.MinusOneQualChance)
						{
							num9 = -1;
						}
						else if (value < ChestBrokenImprovementItem.PickupQualChance + ChestBrokenImprovementItem.EqualQualChance + ChestBrokenImprovementItem.MinusOneQualChance)
						{
							num9 = 0;
						}
						else
						{
							num9 = 1;
						}
					}
					if (!flag4)
					{
						this.DetermineContents(GameManager.Instance.PrimaryPlayer, num9);
						base.StartCoroutine(this.PresentItem());
					}
				}
				else if (num8 < num + num2)
				{
					this.contents = new List<PickupObject>();
					this.contents.Add(GameManager.Instance.RewardManager.HalfHeartPrefab);
					this.m_forceDropOkayForRainbowRun = true;
					base.StartCoroutine(this.PresentItem());
				}
				else if (num8 < num + num2 + num4)
				{
					bool flag5 = false;
					if (!Chest.HasDroppedSerJunkanThisSession && !flag3 && UnityEngine.Random.value < 0.2f)
					{
						Chest.HasDroppedSerJunkanThisSession = true;
						flag5 = true;
					}
					this.contents = new List<PickupObject>();
					int num10 = ((this.overrideJunkId < 0) ? GlobalItemIds.Junk : this.overrideJunkId);
					if (flag5)
					{
						num10 = GlobalItemIds.SackKnightBoon;
					}
					this.contents.Add(PickupObjectDatabase.GetById(num10));
					this.m_forceDropOkayForRainbowRun = true;
					base.StartCoroutine(this.PresentItem());
				}
				else if (num8 < num + num2 + num4 + num6)
				{
					Chest.HasDroppedSerJunkanThisSession = true;
					this.contents = new List<PickupObject>();
					this.contents.Add(PickupObjectDatabase.GetById(GlobalItemIds.SackKnightBoon));
					base.StartCoroutine(this.PresentItem());
				}
				else
				{
					Exploder.DoDefaultExplosion(base.sprite.WorldCenter, Vector2.zero, null, false, CoreDamageTypes.None, false);
				}
			}
			for (int k = 0; k < GameManager.Instance.AllPlayers.Length; k++)
			{
				if (GameManager.Instance.AllPlayers[k].OnChestBroken != null)
				{
					GameManager.Instance.AllPlayers[k].OnChestBroken(GameManager.Instance.AllPlayers[k], this);
				}
			}
		}
	}

	// Token: 0x06004D97 RID: 19863 RVA: 0x001AAABC File Offset: 0x001A8CBC
	private IEnumerator GiveCoopPartnerBack(bool doDelay = true)
	{
		if (doDelay)
		{
			yield return new WaitForSeconds(0.7f);
		}
		AkSoundEngine.PostEvent("play_obj_chest_open_01", base.gameObject);
		AkSoundEngine.PostEvent("stop_obj_fuse_loop_01", base.gameObject);
		PlayerController deadPlayer = ((!GameManager.Instance.PrimaryPlayer.healthHaver.IsDead) ? GameManager.Instance.SecondaryPlayer : GameManager.Instance.PrimaryPlayer);
		deadPlayer.specRigidbody.enabled = true;
		deadPlayer.gameObject.SetActive(true);
		deadPlayer.sprite.renderer.enabled = true;
		deadPlayer.ResurrectFromChest(base.sprite.WorldBottomCenter);
		yield break;
	}

	// Token: 0x06004D98 RID: 19864 RVA: 0x001AAAE0 File Offset: 0x001A8CE0
	private void OnPreRigidbodyCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (!myPixelCollider.IsTrigger && otherRigidbody.GetComponent<KeyBullet>() != null)
		{
			PhysicsEngine.SkipCollision = true;
		}
	}

	// Token: 0x06004D99 RID: 19865 RVA: 0x001AAB04 File Offset: 0x001A8D04
	private void OnTriggerCollision(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (this.m_isKeyOpening)
		{
			return;
		}
		KeyBullet component = specRigidbody.GetComponent<KeyBullet>();
		if (component != null)
		{
			this.HandleKeyEncounter(component);
		}
	}

	// Token: 0x06004D9A RID: 19866 RVA: 0x001AAB38 File Offset: 0x001A8D38
	public void HandleKeyEncounter(KeyBullet key)
	{
		if (this.IsSealed)
		{
			return;
		}
		if (this.IsLocked)
		{
			this.m_isKeyOpening = true;
			Projectile component = key.GetComponent<Projectile>();
			component.specRigidbody.Velocity = Vector2.zero;
			GameObject overrideMidairDeathVFX = component.hitEffects.overrideMidairDeathVFX;
			PlayerController playerController = component.Owner as PlayerController;
			UnityEngine.Object.Destroy(component.specRigidbody);
			UnityEngine.Object.Destroy(component);
			base.StartCoroutine(this.HandleKeyEncounter_CR(key, overrideMidairDeathVFX, playerController));
		}
	}

	// Token: 0x06004D9B RID: 19867 RVA: 0x001AABB4 File Offset: 0x001A8DB4
	private IEnumerator HandleKeyEncounter_CR(KeyBullet key, GameObject vfxPrefab, PlayerController optionalPlayer)
	{
		tk2dBaseSprite keySprite = key.GetComponentInChildren<tk2dBaseSprite>();
		Vector2 keyPositionOffset = keySprite.GetRelativePositionFromAnchor(tk2dBaseSprite.Anchor.MiddleCenter);
		Vector2 lockCenter = ((!(this.LockAnimator != null)) ? base.sprite.WorldCenter : this.LockAnimator.Sprite.WorldCenter);
		Vector2 lockToKey = (key.transform.position + key.transform.rotation * keyPositionOffset).XY() - lockCenter;
		float distFromLock = lockToKey.magnitude;
		float degreeDiff = BraveMathCollege.ClampAngle180(lockToKey.ToAngle() + 90f);
		while (Mathf.Abs(degreeDiff) > 0f)
		{
			degreeDiff = Mathf.MoveTowards(degreeDiff, 0f, 600f * BraveTime.DeltaTime);
			key.transform.position = (lockCenter + new Vector2(0f, -distFromLock).Rotate(degreeDiff)).ToVector3ZUp(key.transform.position.z) + key.transform.rotation * keyPositionOffset;
			key.transform.rotation = Quaternion.Euler(0f, 0f, degreeDiff + 90f);
			keySprite.UpdateZDepth();
			yield return null;
		}
		while (distFromLock > 1f)
		{
			distFromLock = Mathf.MoveTowards(distFromLock, 1f, BraveTime.DeltaTime * 14f);
			key.transform.position = (lockCenter + new Vector2(0f, -distFromLock)).ToVector3ZUp(key.transform.position.z) + key.transform.rotation * keyPositionOffset;
			yield return null;
		}
		GameObject vfxInstance = UnityEngine.Object.Instantiate<GameObject>(vfxPrefab);
		vfxInstance.GetComponent<tk2dBaseSprite>().PlaceAtPositionByAnchor(keySprite.WorldCenter.ToVector3ZUp(0f), tk2dBaseSprite.Anchor.MiddleCenter);
		UnityEngine.Object.Destroy(key.gameObject);
		if (this.IsLocked)
		{
			this.IsLocked = false;
			if (!this.pickedUp)
			{
				if (this.LockAnimator != null)
				{
					this.LockAnimator.PlayAndDestroyObject(this.LockOpenAnim, null);
				}
				this.Open(optionalPlayer ?? GameManager.Instance.PrimaryPlayer);
			}
		}
		yield break;
	}

	// Token: 0x06004D9C RID: 19868 RVA: 0x001AABE4 File Offset: 0x001A8DE4
	public void ForceUnlock()
	{
		if (this.IsLocked)
		{
			this.IsLocked = false;
			if (this.LockAnimator != null)
			{
				this.LockAnimator.PlayAndDestroyObject(this.LockOpenAnim, null);
			}
		}
	}

	// Token: 0x06004D9D RID: 19869 RVA: 0x001AAC1C File Offset: 0x001A8E1C
	private void OnRigidbodyCollision(CollisionData rigidbodyCollision)
	{
		if (this.ChestIdentifier == Chest.SpecialChestIdentifier.SECRET_RAINBOW && rigidbodyCollision.OtherRigidbody && rigidbodyCollision.OtherRigidbody.projectile && BraveUtility.EnumFlagsContains((uint)rigidbodyCollision.OtherRigidbody.projectile.damageTypes, 32U) > 0)
		{
			this.RevealSecretRainbow();
		}
	}

	// Token: 0x06004D9E RID: 19870 RVA: 0x001AAC80 File Offset: 0x001A8E80
	public void ForceOpen(PlayerController player)
	{
		this.Open(player);
	}

	// Token: 0x06004D9F RID: 19871 RVA: 0x001AAC8C File Offset: 0x001A8E8C
	protected void HandleGeneratedMagnificence()
	{
		if (this.GeneratedMagnificence > 0f)
		{
			GameManager.Instance.Dungeon.GeneratedMagnificence -= this.GeneratedMagnificence;
			this.GeneratedMagnificence = 0f;
		}
	}

	// Token: 0x06004DA0 RID: 19872 RVA: 0x001AACC8 File Offset: 0x001A8EC8
	private void GetRidOfBowler()
	{
		if (this.m_bowlerInstance)
		{
			LootEngine.DoDefaultPurplePoof(this.m_bowlerInstance.GetComponent<tk2dBaseSprite>().WorldCenter, false);
			UnityEngine.Object.Destroy(this.m_bowlerInstance);
			this.m_bowlerInstance = null;
			AkSoundEngine.PostEvent("Stop_SND_OBJ", base.gameObject);
		}
	}

	// Token: 0x06004DA1 RID: 19873 RVA: 0x001AAD20 File Offset: 0x001A8F20
	protected void Open(PlayerController player)
	{
		if (player != null)
		{
			this.GetRidOfBowler();
			if (GameManager.Instance.InTutorial || this.AlwaysBroadcastsOpenEvent)
			{
				GameManager.BroadcastRoomTalkDoerFsmEvent("playerOpenedChest");
			}
			if (this.m_registeredIconRoom != null)
			{
				Minimap.Instance.DeregisterRoomIcon(this.m_registeredIconRoom, this.minimapIconInstance);
			}
			if (this.m_isGlitchChest)
			{
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(player);
					if (otherPlayer && otherPlayer.IsGhost)
					{
						base.StartCoroutine(this.GiveCoopPartnerBack(false));
					}
				}
				GameManager.Instance.InjectedFlowPath = "Core Game Flows/Secret_DoubleBeholster_Flow";
				Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
				GameManager.Instance.DelayedLoadNextLevel(0.5f);
				return;
			}
			if (this.m_isMimic && !Chest.m_IsCoopMode)
			{
				this.DetermineContents(player, 0);
				this.DoMimicTransformation(this.contents);
				return;
			}
			if (this.ChestIdentifier == Chest.SpecialChestIdentifier.SECRET_RAINBOW)
			{
				this.RevealSecretRainbow();
			}
			this.pickedUp = true;
			this.IsOpen = true;
			this.HandleGeneratedMagnificence();
			this.m_room.DeregisterInteractable(this);
			MajorBreakable component = base.GetComponent<MajorBreakable>();
			if (component != null)
			{
				component.usesTemporaryZeroHitPointsState = false;
			}
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.NumberOfLivingPlayers == 1 && this.ChestIdentifier == Chest.SpecialChestIdentifier.NORMAL)
			{
				base.spriteAnimator.Play((!string.IsNullOrEmpty(this.overrideOpenAnimName)) ? this.overrideOpenAnimName : this.openAnimName);
				this.m_isMimic = false;
				base.StartCoroutine(this.GiveCoopPartnerBack(true));
			}
			else if (this.lootTable.CompletesSynergy)
			{
				base.StartCoroutine(this.HandleSynergyGambleChest(player));
			}
			else
			{
				this.DetermineContents(player, 0);
				if (base.name.Contains("Chest_Red") && this.contents != null && this.contents.Count == 1 && this.contents[0] && this.contents[0].ItemSpansBaseQualityTiers)
				{
					this.contents.Add(PickupObjectDatabase.GetById(GlobalItemIds.Key));
				}
				base.spriteAnimator.Play((!string.IsNullOrEmpty(this.overrideOpenAnimName)) ? this.overrideOpenAnimName : this.openAnimName);
				AkSoundEngine.PostEvent("play_obj_chest_open_01", base.gameObject);
				AkSoundEngine.PostEvent("stop_obj_fuse_loop_01", base.gameObject);
				if (!this.m_isMimic)
				{
					if (this.SubAnimators != null && this.SubAnimators.Length > 0)
					{
						for (int i = 0; i < this.SubAnimators.Length; i++)
						{
							this.SubAnimators[i].Play();
						}
					}
					player.TriggerItemAcquisition();
					base.StartCoroutine(this.PresentItem());
				}
			}
		}
	}

	// Token: 0x06004DA2 RID: 19874 RVA: 0x001AB034 File Offset: 0x001A9234
	private IEnumerator HandleSynergyGambleChest(PlayerController player)
	{
		base.majorBreakable.TemporarilyInvulnerable = true;
		this.DetermineContents(player, 0);
		base.spriteAnimator.Play(this.openAnimName);
		AkSoundEngine.PostEvent("stop_obj_fuse_loop_01", base.gameObject);
		if (this.SubAnimators.Length > 0 && this.SubAnimators[0])
		{
			this.SubAnimators[0].gameObject.SetActive(true);
			this.SubAnimators[0].Play("synergy_chest_open_gamble_vfx");
		}
		while (base.spriteAnimator.IsPlaying(this.openAnimName))
		{
			yield return null;
		}
		bool succeeded = false;
		for (int i = 0; i < this.contents.Count; i++)
		{
			PickupObject pickupObject = this.contents[i];
			bool flag = false;
			if (RewardManager.AnyPlayerHasItemInSynergyContainingOtherItem(pickupObject, ref flag))
			{
				succeeded = true;
				break;
			}
		}
		if (succeeded)
		{
			base.spriteAnimator.Play("synergy_chest_open_synergy");
			if (this.SubAnimators.Length > 0 && this.SubAnimators[0])
			{
				this.SubAnimators[0].PlayAndDisableObject("synergy_chest_open_synergy_vfx", null);
			}
			yield return new WaitForSeconds(0.725f);
		}
		else
		{
			base.spriteAnimator.Play("synergy_chest_open_fail");
			if (this.SubAnimators.Length > 0 && this.SubAnimators[0])
			{
				this.SubAnimators[0].PlayAndDisableObject("synergy_chest_open_fail_vfx", null);
			}
			yield return new WaitForSeconds(0.44f);
		}
		if (!this.m_isMimic)
		{
			player.TriggerItemAcquisition();
			base.StartCoroutine(this.PresentItem());
		}
		base.majorBreakable.TemporarilyInvulnerable = false;
		yield break;
	}

	// Token: 0x06004DA3 RID: 19875 RVA: 0x001AB058 File Offset: 0x001A9258
	protected bool HandleQuestContentsModification()
	{
		if (GameManager.Instance.InTutorial)
		{
			return false;
		}
		if (this.IsRainbowChest)
		{
			return false;
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_01) && GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_02) && GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_03) && GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_04) && GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_05) && GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_06))
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.RESOURCEFUL_RAT_COMPLETE, true);
		}
		if (!GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_COMPLETE) && !Chest.HasDroppedResourcefulRatNoteThisSession)
		{
			float num = 0.15f;
			if (GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_01))
			{
				num = 0.33f;
			}
			if (GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_LICH))
			{
				num = 10f;
			}
			float playerStatValue = GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.NUMBER_ATTEMPTS);
			if (GameManager.Instance.Dungeon && GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON)
			{
				num = 0f;
			}
			if (playerStatValue < 10f)
			{
				num = 0f;
			}
			else
			{
				num = Mathf.Lerp(0f, num, Mathf.Clamp01(playerStatValue / 20f));
			}
			if (UnityEngine.Random.value < num)
			{
				bool flag = !GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_01) && GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_CLEARED_GUNGEON) > 0f;
				bool flag2 = !GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_02) && GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_01) && GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.ITEMS_TAKEN_BY_RAT) > 0f;
				bool flag3 = !GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_03) && GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_02) && GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_REACHED_SEWERS) > 0f;
				bool flag4 = !GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_04) && GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_03) && GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.MASTERY_TOKENS_RECEIVED) > 0f;
				bool flag5 = !GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_05) && GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_04) && GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_MINI_FUSELIER);
				bool flag6 = this.m_isMimic && !GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_06) && GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_05) && GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_DRAGUN);
				if (flag)
				{
					this.contents.Clear();
					this.contents.Add(PickupObjectDatabase.GetById(GlobalItemIds.RatNote01));
					Chest.HasDroppedResourcefulRatNoteThisSession = true;
					return true;
				}
				if (flag2)
				{
					this.contents.Clear();
					this.contents.Add(PickupObjectDatabase.GetById(GlobalItemIds.RatNote02));
					Chest.HasDroppedResourcefulRatNoteThisSession = true;
					return true;
				}
				if (flag3)
				{
					this.contents.Clear();
					this.contents.Add(PickupObjectDatabase.GetById(GlobalItemIds.RatNote03));
					Chest.HasDroppedResourcefulRatNoteThisSession = true;
					return true;
				}
				if (flag4)
				{
					this.contents.Clear();
					this.contents.Add(PickupObjectDatabase.GetById(GlobalItemIds.RatNote04));
					Chest.HasDroppedResourcefulRatNoteThisSession = true;
					return true;
				}
				if (flag5)
				{
					this.contents.Clear();
					this.contents.Add(PickupObjectDatabase.GetById(GlobalItemIds.RatNote05));
					Chest.HasDroppedResourcefulRatNoteThisSession = true;
					return true;
				}
				if (flag6)
				{
					this.contents.Clear();
					this.contents.Add(PickupObjectDatabase.GetById(GlobalItemIds.RatNote06));
					Chest.HasDroppedResourcefulRatNoteThisSession = true;
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06004DA4 RID: 19876 RVA: 0x001AB464 File Offset: 0x001A9664
	public void GenerationDetermineContents(FloorRewardManifest manifest, System.Random safeRandom)
	{
		List<PickupObject> list = this.GenerateContents(null, 0, safeRandom);
		manifest.RegisterContents(this, list);
	}

	// Token: 0x06004DA5 RID: 19877 RVA: 0x001AB484 File Offset: 0x001A9684
	protected List<PickupObject> GenerateContents(PlayerController player, int tierShift, System.Random safeRandom = null)
	{
		List<PickupObject> list = new List<PickupObject>();
		if (this.lootTable.lootTable == null)
		{
			list.Add(GameManager.Instance.Dungeon.baseChestContents.SelectByWeight(false).GetComponent<PickupObject>());
		}
		else if (this.lootTable != null)
		{
			if (tierShift <= -1)
			{
				if (this.breakertronLootTable.lootTable != null)
				{
					list = this.breakertronLootTable.GetItemsForPlayer(player, -1, null, safeRandom);
				}
				else
				{
					list = this.lootTable.GetItemsForPlayer(player, tierShift, null, safeRandom);
				}
			}
			else
			{
				list = this.lootTable.GetItemsForPlayer(player, tierShift, null, safeRandom);
				if (this.lootTable.CompletesSynergy)
				{
					float num = Mathf.Clamp01(0.6f - 0.1f * (float)this.lootTable.LastGenerationNumSynergiesCalculated);
					num = Mathf.Clamp(num, 0.2f, 1f);
					if (num > 0f)
					{
						float num2 = ((safeRandom == null) ? UnityEngine.Random.value : ((float)safeRandom.NextDouble()));
						if (num2 < num)
						{
							this.lootTable.CompletesSynergy = false;
							list = this.lootTable.GetItemsForPlayer(player, tierShift, null, safeRandom);
							this.lootTable.CompletesSynergy = true;
						}
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06004DA6 RID: 19878 RVA: 0x001AB5C8 File Offset: 0x001A97C8
	public List<PickupObject> PredictContents(PlayerController player)
	{
		this.DetermineContents(player, 0);
		return this.contents;
	}

	// Token: 0x06004DA7 RID: 19879 RVA: 0x001AB5D8 File Offset: 0x001A97D8
	protected void DetermineContents(PlayerController player, int tierShift = 0)
	{
		if (this.contents == null)
		{
			this.contents = new List<PickupObject>();
			if (this.forceContentIds.Count > 0)
			{
				for (int i = 0; i < this.forceContentIds.Count; i++)
				{
					this.contents.Add(PickupObjectDatabase.GetById(this.forceContentIds[i]));
				}
			}
		}
		bool flag = this.HandleQuestContentsModification();
		if (this.contents.Count == 0 && !flag)
		{
			FloorRewardManifest seededManifestForCurrentFloor = GameManager.Instance.RewardManager.GetSeededManifestForCurrentFloor();
			if (seededManifestForCurrentFloor != null && seededManifestForCurrentFloor.PregeneratedChestContents.ContainsKey(this))
			{
				this.contents = seededManifestForCurrentFloor.PregeneratedChestContents[this];
			}
			else
			{
				this.contents = this.GenerateContents(player, tierShift, this.m_runtimeRandom);
			}
			if (this.contents.Count == 0)
			{
				Debug.LogError("Emergency Mimic swap... what's going to happen to the loot now?");
				this.m_isMimic = true;
				this.DoMimicTransformation(null);
			}
		}
	}

	// Token: 0x06004DA8 RID: 19880 RVA: 0x001AB6DC File Offset: 0x001A98DC
	private void DoMimicTransformation(List<PickupObject> overrideDeathRewards)
	{
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
			aiactor.AdditionalSafeItemDrops.AddRange(overrideDeathRewards);
		}
		aiactor.specRigidbody.Initialize();
		Vector2 unitBottomLeft = aiactor.specRigidbody.UnitBottomLeft;
		Vector2 unitBottomLeft2 = base.specRigidbody.UnitBottomLeft;
		aiactor.transform.position -= unitBottomLeft - unitBottomLeft2;
		aiactor.transform.position += PhysicsEngine.PixelToUnit(this.mimicOffset);
		aiactor.specRigidbody.Reinitialize();
		aiactor.HasDonePlayerEnterCheck = true;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06004DA9 RID: 19881 RVA: 0x001AB884 File Offset: 0x001A9A84
	protected void SpewContentsOntoGround(List<Transform> spawnTransforms)
	{
		List<DebrisObject> list = new List<DebrisObject>();
		bool isRainbowRun = GameStatsManager.Instance.IsRainbowRun;
		if (isRainbowRun && !this.IsRainbowChest && !this.m_forceDropOkayForRainbowRun)
		{
			Vector2 vector;
			if (this.spawnTransform != null)
			{
				vector = this.spawnTransform.position;
			}
			else
			{
				Bounds bounds = base.sprite.GetBounds();
				vector = base.transform.position + bounds.extents;
			}
			LootEngine.SpawnBowlerNote(GameManager.Instance.RewardManager.BowlerNoteChest, vector + new Vector2(-0.5f, -2.25f), this.m_room, true);
		}
		else
		{
			for (int i = 0; i < this.contents.Count; i++)
			{
				List<DebrisObject> list2 = LootEngine.SpewLoot(new List<GameObject> { this.contents[i].gameObject }, spawnTransforms[i].position);
				list.AddRange(list2);
				for (int j = 0; j < list2.Count; j++)
				{
					if (list2[j])
					{
						list2[j].PreventFallingInPits = true;
					}
					if (!(list2[j].GetComponent<Gun>() != null))
					{
						if (!(list2[j].GetComponent<CurrencyPickup>() != null))
						{
							if (list2[j].specRigidbody != null)
							{
								list2[j].specRigidbody.CollideWithOthers = false;
								DebrisObject debrisObject = list2[j];
								debrisObject.OnTouchedGround = (Action<DebrisObject>)Delegate.Combine(debrisObject.OnTouchedGround, new Action<DebrisObject>(this.BecomeViableItem));
							}
						}
					}
				}
			}
		}
		if (this.IsRainbowChest && isRainbowRun && base.transform.position.GetAbsoluteRoom() == GameManager.Instance.Dungeon.data.Entrance)
		{
			GameManager.Instance.Dungeon.StartCoroutine(this.HandleRainbowRunLootProcessing(list));
		}
	}

	// Token: 0x06004DAA RID: 19882 RVA: 0x001ABAC4 File Offset: 0x001A9CC4
	private IEnumerator HandleRainbowRunLootProcessing(List<DebrisObject> items)
	{
		if (base.majorBreakable)
		{
			base.majorBreakable.Break(Vector2.zero);
		}
		int i;
		for (;;)
		{
			for (i = 0; i < items.Count; i++)
			{
				if (!items[i])
				{
					goto Block_2;
				}
			}
			yield return null;
		}
		Block_2:
		for (int j = 0; j < items.Count; j++)
		{
			if (i != j)
			{
				LootEngine.DoDefaultItemPoof(items[j].transform.position, false, true);
				UnityEngine.Object.Destroy(items[j].gameObject);
			}
		}
		if (this)
		{
			LootEngine.SpawnBowlerNote(GameManager.Instance.RewardManager.BowlerNotePostRainbow, base.transform.position.XY() + new Vector2(1f, 1.5f), base.transform.position.GetAbsoluteRoom(), true);
		}
		yield break;
		yield break;
	}

	// Token: 0x06004DAB RID: 19883 RVA: 0x001ABAE8 File Offset: 0x001A9CE8
	protected void BecomeViableItem(DebrisObject debris)
	{
		debris.OnTouchedGround = (Action<DebrisObject>)Delegate.Remove(debris.OnTouchedGround, new Action<DebrisObject>(this.BecomeViableItem));
		debris.OnGrounded = (Action<DebrisObject>)Delegate.Remove(debris.OnGrounded, new Action<DebrisObject>(this.BecomeViableItem));
		debris.specRigidbody.CollideWithOthers = true;
		Vector2 vector = Vector2.zero;
		if (this.spawnTransform != null)
		{
			vector = debris.sprite.WorldCenter - this.spawnTransform.position.XY();
		}
		else
		{
			vector = debris.sprite.WorldCenter - base.sprite.WorldCenter;
		}
		debris.ClearVelocity();
		debris.ApplyVelocity(vector.normalized * 2f);
	}

	// Token: 0x06004DAC RID: 19884 RVA: 0x001ABBBC File Offset: 0x001A9DBC
	private bool CheckPresentedItemTheoreticalPosition(Vector3 targetPosition, Vector3 objectOffset)
	{
		Vector3 vector = targetPosition - new Vector3(objectOffset.x * 2f, 0f, 0f);
		Vector3 vector2 = targetPosition - new Vector3(0f, objectOffset.y * 2f, 0f);
		Vector3 vector3 = targetPosition - new Vector3(objectOffset.x * 2f, objectOffset.y * 2f, 0f);
		return !this.CheckCellValidForItemSpawn(targetPosition) || !this.CheckCellValidForItemSpawn(vector) || !this.CheckCellValidForItemSpawn(vector2) || !this.CheckCellValidForItemSpawn(vector3);
	}

	// Token: 0x06004DAD RID: 19885 RVA: 0x001ABC70 File Offset: 0x001A9E70
	private bool CheckCellValidForItemSpawn(Vector3 pos)
	{
		IntVector2 intVector = pos.IntXY(VectorConversions.Floor);
		Dungeon dungeon = GameManager.Instance.Dungeon;
		return dungeon.data.CheckInBoundsAndValid(intVector) && !dungeon.CellIsPit(pos) && !dungeon.data.isTopWall(intVector.x, intVector.y) && (!dungeon.data.isWall(intVector.x, intVector.y) || dungeon.data.isFaceWallLower(intVector.x, intVector.y));
	}

	// Token: 0x06004DAE RID: 19886 RVA: 0x001ABD0C File Offset: 0x001A9F0C
	private IEnumerator PresentItem()
	{
		bool shouldActuallyPresent = !GameStatsManager.Instance.IsRainbowRun || this.IsRainbowChest || this.m_forceDropOkayForRainbowRun;
		List<Transform> vfxTransforms = new List<Transform>();
		List<Vector3> vfxObjectOffsets = new List<Vector3>();
		Vector3 attachPoint = Vector3.zero;
		if (shouldActuallyPresent)
		{
			if (this.spawnTransform != null)
			{
				attachPoint = this.spawnTransform.position;
			}
			else
			{
				Bounds bounds = base.sprite.GetBounds();
				attachPoint = base.transform.position + bounds.extents;
			}
			for (int i = 0; i < this.contents.Count; i++)
			{
				PickupObject pickupObject = this.contents[i];
				tk2dSprite tk2dSprite = pickupObject.GetComponent<tk2dSprite>();
				if (tk2dSprite == null)
				{
					tk2dSprite = pickupObject.GetComponentInChildren<tk2dSprite>();
				}
				GameObject gameObject = new GameObject("VFX_Chest_Item");
				Transform transform = gameObject.transform;
				Vector3 vector = Vector3.zero;
				if (tk2dSprite != null)
				{
					tk2dSprite tk2dSprite2 = tk2dSprite.AddComponent(gameObject, tk2dSprite.Collection, tk2dSprite.spriteId);
					tk2dSprite2.HeightOffGround = 2f;
					NotePassiveItem component = tk2dSprite.GetComponent<NotePassiveItem>();
					if (component != null && component.ResourcefulRatNoteIdentifier >= 0)
					{
						tk2dSprite2.SetSprite(component.GetAppropriateSpriteName(false));
					}
					SpriteOutlineManager.AddOutlineToSprite(tk2dSprite2, Color.white, 0.5f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
					vector = -BraveUtility.QuantizeVector(gameObject.GetComponent<tk2dSprite>().GetBounds().extents);
					tk2dSprite2.UpdateZDepth();
				}
				transform.position = attachPoint + vector;
				vfxTransforms.Add(transform);
				vfxObjectOffsets.Add(vector);
			}
			float displayTime = 1f;
			float elapsed = 0f;
			while (elapsed < displayTime)
			{
				elapsed += BraveTime.DeltaTime * 1.5f;
				float t = Mathf.Clamp01(elapsed / displayTime);
				float curveValue = this.spawnCurve.Evaluate(t);
				float modT = Mathf.SmoothStep(0f, 1f, t);
				if (vfxTransforms.Count <= 4)
				{
					for (int j = 0; j < vfxTransforms.Count; j++)
					{
						float num = ((vfxTransforms.Count != 1) ? (-1f + 2f / (float)(vfxTransforms.Count - 1) * (float)j) : 0f);
						num = num * ((float)vfxTransforms.Count / 2f) * 1f;
						Vector3 vector2 = attachPoint + vfxObjectOffsets[j] + new Vector3(Mathf.Lerp(0f, num, modT), curveValue, -2.5f);
						if (this.CheckPresentedItemTheoreticalPosition(vector2, vfxObjectOffsets[j]))
						{
							vector2 = vfxTransforms[j].position;
						}
						vfxTransforms[j].position = vector2;
					}
				}
				else
				{
					for (int k = 0; k < vfxTransforms.Count; k++)
					{
						float num2 = 360f / (float)vfxTransforms.Count;
						Vector3 vector3 = Quaternion.Euler(0f, 0f, num2 * (float)k) * Vector3.right;
						float num3 = 3f;
						Vector2 vector4 = vector3.XY().normalized * num3;
						Vector3 vector5 = attachPoint + vfxObjectOffsets[k] + new Vector3(0f, curveValue, -2.5f) + Vector2.Lerp(Vector2.zero, vector4, modT).ToVector3ZUp(0f);
						if (this.CheckPresentedItemTheoreticalPosition(vector5, vfxObjectOffsets[k]))
						{
							vector5 = vfxTransforms[k].position;
						}
						vfxTransforms[k].position = vector5;
					}
				}
				yield return null;
			}
		}
		this.SpewContentsOntoGround(vfxTransforms);
		yield return null;
		for (int l = 0; l < vfxTransforms.Count; l++)
		{
			UnityEngine.Object.Destroy(vfxTransforms[l].gameObject);
		}
		yield break;
	}

	// Token: 0x06004DAF RID: 19887 RVA: 0x001ABD28 File Offset: 0x001A9F28
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		if (this.IsLocked && interactor.carriedConsumables.KeyBullets <= 0 && !interactor.carriedConsumables.InfiniteKeys)
		{
			return;
		}
		if (this.IsSealed)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x06004DB0 RID: 19888 RVA: 0x001ABDAC File Offset: 0x001A9FAC
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, this.BaseOutlineColor, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x06004DB1 RID: 19889 RVA: 0x001ABDF8 File Offset: 0x001A9FF8
	public float GetDistanceToPoint(Vector2 point)
	{
		if (this.IsMirrorChest)
		{
			return 1000f;
		}
		Bounds bounds = base.sprite.GetBounds();
		bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
	}

	// Token: 0x06004DB2 RID: 19890 RVA: 0x001ABEE8 File Offset: 0x001AA0E8
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06004DB3 RID: 19891 RVA: 0x001ABEF0 File Offset: 0x001AA0F0
	public void BreakLock()
	{
		this.IsSealed = true;
		this.IsLocked = false;
		this.IsLockBroken = true;
		if (!this.pickedUp && this.LockAnimator != null)
		{
			AkSoundEngine.PostEvent("Play_OBJ_lock_pick_01", base.gameObject);
			this.LockAnimator.Play(this.LockBreakAnim);
		}
	}

	// Token: 0x06004DB4 RID: 19892 RVA: 0x001ABF50 File Offset: 0x001AA150
	private void Unlock()
	{
		this.IsLocked = false;
		if (!this.pickedUp && this.LockAnimator != null)
		{
			this.LockAnimator.PlayAndDestroyObject(this.LockOpenAnim, null);
		}
	}

	// Token: 0x06004DB5 RID: 19893 RVA: 0x001ABF88 File Offset: 0x001AA188
	public void Interact(PlayerController player)
	{
		if (this.IsSealed || this.IsLockBroken)
		{
			return;
		}
		if (!this.IsLocked)
		{
			if (!this.pickedUp)
			{
				this.Open(player);
			}
			return;
		}
		if (this.ChestIdentifier == Chest.SpecialChestIdentifier.RAT)
		{
			if (player.carriedConsumables.ResourcefulRatKeys > 0)
			{
				player.carriedConsumables.ResourcefulRatKeys--;
				this.Unlock();
				if (!this.pickedUp)
				{
					if (this.forceContentIds != null && this.forceContentIds.Count == 1)
					{
						for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
						{
							PlayerController playerController = GameManager.Instance.AllPlayers[i];
							if (playerController && playerController.HasPickupID(this.forceContentIds[0]))
							{
								this.forceContentIds.Clear();
								if (UnityEngine.Random.value < 0.5f)
								{
									this.ChestType = Chest.GeneralChestType.WEAPON;
									this.lootTable.lootTable = GameManager.Instance.RewardManager.GunsLootTable;
								}
								else
								{
									this.ChestType = Chest.GeneralChestType.ITEM;
									this.lootTable.lootTable = GameManager.Instance.RewardManager.ItemsLootTable;
								}
							}
						}
					}
					this.Open(player);
				}
			}
			return;
		}
		if (this.LockAnimator == null || !this.LockAnimator.renderer.enabled)
		{
			this.Unlock();
			if (!this.pickedUp)
			{
				this.Open(player);
			}
			return;
		}
		if (player.carriedConsumables.KeyBullets <= 0 && !player.carriedConsumables.InfiniteKeys)
		{
			if (this.LockAnimator != null)
			{
				this.LockAnimator.Play(this.LockNoKeyAnim);
			}
			return;
		}
		if (!player.carriedConsumables.InfiniteKeys)
		{
			player.carriedConsumables.KeyBullets--;
		}
		GameStatsManager.Instance.RegisterStatChange(TrackedStats.CHESTS_UNLOCKED_WITH_KEY_BULLETS, 1f);
		this.Unlock();
		if (!this.pickedUp)
		{
			this.Open(player);
		}
	}

	// Token: 0x06004DB6 RID: 19894 RVA: 0x001AC1AC File Offset: 0x001AA3AC
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x06004DB7 RID: 19895 RVA: 0x001AC1B8 File Offset: 0x001AA3B8
	public void BecomeGlitchChest()
	{
		AkSoundEngine.PostEvent("Play_OBJ_chestglitch_loop_01", base.gameObject);
		base.sprite.usesOverrideMaterial = true;
		Material material = base.sprite.renderer.material;
		material.shader = ShaderCache.Acquire("Brave/Internal/Glitch");
		material.SetFloat("_GlitchInterval", 0.1f);
		material.SetFloat("_DispProbability", 0.4f);
		material.SetFloat("_DispIntensity", 0.01f);
		material.SetFloat("_ColorProbability", 0.4f);
		material.SetFloat("_ColorIntensity", 0.04f);
		this.m_isGlitchChest = true;
	}

	// Token: 0x06004DB8 RID: 19896 RVA: 0x001AC25C File Offset: 0x001AA45C
	private void BecomeCoopChest()
	{
		if (this.ChestIdentifier == Chest.SpecialChestIdentifier.RAT)
		{
			return;
		}
		GameManager.Instance.Dungeon.StartCoroutine(this.HandleCoopChestTransform(false));
	}

	// Token: 0x17000AF4 RID: 2804
	// (get) Token: 0x06004DB9 RID: 19897 RVA: 0x001AC284 File Offset: 0x001AA484
	public bool IsGlitched
	{
		get
		{
			return this.m_isGlitchChest;
		}
	}

	// Token: 0x06004DBA RID: 19898 RVA: 0x001AC28C File Offset: 0x001AA48C
	private IEnumerator HandleCoopChestTransform(bool unbecome = false)
	{
		if (this.IsOpen || this.IsBroken)
		{
			yield break;
		}
		if (this.ChestIdentifier == Chest.SpecialChestIdentifier.RAT)
		{
			yield break;
		}
		while (base.spriteAnimator.IsPlaying(base.spriteAnimator.CurrentClip) && !base.spriteAnimator.IsPlaying("coop_chest_knock"))
		{
			yield return null;
		}
		if (unbecome)
		{
			this.overrideSpawnAnimName = string.Empty;
			this.overrideOpenAnimName = string.Empty;
			this.overrideBreakAnimName = string.Empty;
			if (base.majorBreakable)
			{
				base.majorBreakable.overrideSpriteNameToUseAtZeroHP = string.Empty;
			}
			base.spriteAnimator.Stop();
			base.spriteAnimator.ForceClearCurrentClip();
			this.IsLocked = this.m_cachedLockedState;
			if (this.LockAnimator != null && this.IsLocked)
			{
				this.LockAnimator.renderer.enabled = true;
			}
			if (this.ShadowSprite && this.m_cachedShadowSpriteID > -1)
			{
				this.ShadowSprite.SetSprite(this.m_cachedShadowSpriteID);
			}
			if (!string.IsNullOrEmpty(this.spawnAnimName))
			{
				tk2dSpriteAnimationClip clipByName = base.spriteAnimator.GetClipByName(this.spawnAnimName);
				base.sprite.SetSprite(clipByName.frames[clipByName.frames.Length - 1].spriteId);
			}
			else
			{
				base.sprite.SetSprite(this.m_cachedSpriteForCoop);
			}
			if (this.m_cachedCoopManualOffset != IntVector2.Zero)
			{
				IntVector2 intVector = new IntVector2(25, 14);
				PixelCollider primaryPixelCollider = base.specRigidbody.PrimaryPixelCollider;
				if (primaryPixelCollider.ManualOffsetX != this.m_cachedCoopManualOffset.x || primaryPixelCollider.ManualOffsetY != this.m_cachedCoopManualOffset.y || primaryPixelCollider.ManualWidth != this.m_cachedCoopManualDimensions.x || primaryPixelCollider.ManualHeight != this.m_cachedCoopManualDimensions.y)
				{
					primaryPixelCollider.ManualOffsetX = this.m_cachedCoopManualOffset.x;
					primaryPixelCollider.ManualOffsetY = this.m_cachedCoopManualOffset.y;
					primaryPixelCollider.ManualWidth = this.m_cachedCoopManualDimensions.x;
					primaryPixelCollider.ManualHeight = this.m_cachedCoopManualDimensions.y;
					float num = (float)(this.m_cachedCoopManualDimensions.x - intVector.x) / 2f;
					base.transform.position += new Vector3((float)(Mathf.RoundToInt(num) * -1) * 0.0625f, 0f, 0f);
					base.specRigidbody.Reinitialize();
				}
				this.m_cachedCoopManualOffset = IntVector2.Zero;
				this.m_cachedCoopManualDimensions = IntVector2.Zero;
			}
		}
		else
		{
			this.overrideSpawnAnimName = "coop_chest_appear";
			this.overrideOpenAnimName = "coop_chest_open";
			this.overrideBreakAnimName = "coop_chest_break";
			this.m_cachedLockedState = this.IsLocked;
			if (this.ShadowSprite)
			{
				this.m_cachedShadowSpriteID = this.ShadowSprite.spriteId;
				this.ShadowSprite.SetSprite("low_chest_shadow_001");
			}
			this.IsLocked = false;
			if (this.LockAnimator != null)
			{
				this.LockAnimator.renderer.enabled = false;
			}
			if (base.majorBreakable)
			{
				base.majorBreakable.overrideSpriteNameToUseAtZeroHP = "coop_chest_break001";
			}
			if (this.m_cachedSpriteForCoop == -1)
			{
				this.m_cachedSpriteForCoop = base.sprite.spriteId;
			}
			if (!this.m_temporarilyUnopenable)
			{
				base.spriteAnimator.Play("coop_chest_knock");
			}
			base.sprite.SetSprite("coop_chest_idle_001");
			IntVector2 intVector2 = new IntVector2(3, 0);
			IntVector2 intVector3 = new IntVector2(25, 14);
			PixelCollider primaryPixelCollider2 = base.specRigidbody.PrimaryPixelCollider;
			if (primaryPixelCollider2.ManualOffsetX != intVector2.x || primaryPixelCollider2.ManualOffsetY != intVector2.y || primaryPixelCollider2.ManualWidth != intVector3.x || primaryPixelCollider2.ManualHeight != intVector3.y)
			{
				this.m_cachedCoopManualOffset = new IntVector2(primaryPixelCollider2.ManualOffsetX, primaryPixelCollider2.ManualOffsetY);
				this.m_cachedCoopManualDimensions = new IntVector2(primaryPixelCollider2.ManualWidth, primaryPixelCollider2.ManualHeight);
				primaryPixelCollider2.ManualOffsetX = intVector2.x;
				primaryPixelCollider2.ManualOffsetY = intVector2.y;
				primaryPixelCollider2.ManualWidth = intVector3.x;
				primaryPixelCollider2.ManualHeight = intVector3.y;
				float num2 = (float)(this.m_cachedCoopManualDimensions.x - primaryPixelCollider2.ManualWidth) / 2f;
				base.transform.position += new Vector3((float)Mathf.RoundToInt(num2) * 0.0625f, 0f, 0f);
				base.specRigidbody.Reinitialize();
			}
		}
		yield break;
	}

	// Token: 0x06004DBB RID: 19899 RVA: 0x001AC2B0 File Offset: 0x001AA4B0
	private void UnbecomeCoopChest()
	{
		GameManager.Instance.Dungeon.StartCoroutine(this.HandleCoopChestTransform(true));
	}

	// Token: 0x06004DBC RID: 19900 RVA: 0x001AC2CC File Offset: 0x001AA4CC
	public void MaybeBecomeMimic()
	{
		if (this.IsTruthChest)
		{
			return;
		}
		if (this.IsRainbowChest)
		{
			return;
		}
		if (this.lootTable.CompletesSynergy)
		{
			return;
		}
		this.m_isMimic = false;
		bool flag = false;
		if (!GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_COMPLETE) && !Chest.HasDroppedResourcefulRatNoteThisSession && !Chest.DoneResourcefulRatMimicThisSession)
		{
			bool flag2 = !GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_06) && GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_05) && GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_LICH);
			if (flag2 && GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.CASTLEGEON)
			{
				Chest.DoneResourcefulRatMimicThisSession = true;
				flag = true;
			}
		}
		if (!string.IsNullOrEmpty(this.MimicGuid))
		{
			flag |= GameManager.Instance.Dungeon.sharedSettingsPrefab.RandomShouldBecomeMimic(this.overrideMimicChance);
			GameLevelDefinition lastLoadedLevelDefinition = GameManager.Instance.GetLastLoadedLevelDefinition();
			flag |= lastLoadedLevelDefinition != null && lastLoadedLevelDefinition.lastSelectedFlowEntry != null && lastLoadedLevelDefinition.lastSelectedFlowEntry.levelMode == FlowLevelEntryMode.ALL_MIMICS;
			if (PassiveItem.IsFlagSetAtAll(typeof(MimicToothNecklaceItem)) && this.ChestIdentifier == Chest.SpecialChestIdentifier.RAT)
			{
				flag = false;
			}
			if (flag)
			{
				if (PassiveItem.IsFlagSetAtAll(typeof(MimicToothNecklaceItem)))
				{
					this.Unlock();
				}
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

	// Token: 0x06004DBD RID: 19901 RVA: 0x001AC470 File Offset: 0x001AA670
	private IEnumerator MimicIdleAnimCR()
	{
		this.m_isMimicBreathing = true;
		while (this.m_isMimic)
		{
			yield return new WaitForSeconds(this.preMimicIdleAnimDelay);
			if (!this.m_isMimic)
			{
				yield break;
			}
			while (Chest.m_IsCoopMode)
			{
				yield return null;
			}
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

	// Token: 0x06004DBE RID: 19902 RVA: 0x001AC48C File Offset: 0x001AA68C
	private void OnDamaged(float damage)
	{
		if (this.m_isMimic && !Chest.m_IsCoopMode && !this.IsMirrorChest)
		{
			GameManager.Instance.platformInterface.AchievementUnlock(Achievement.PREFIRE_ON_MIMIC, 0);
			this.DetermineContents(GameManager.Instance.PrimaryPlayer, 0);
			this.DoMimicTransformation(this.contents);
		}
	}

	// Token: 0x06004DBF RID: 19903 RVA: 0x001AC4E8 File Offset: 0x001AA6E8
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.m_room = room;
		if (!this.m_configured)
		{
			this.m_room.Entered += this.RoomEntered;
		}
		this.Initialize();
		if (!this.m_configured)
		{
			this.RegisterChestOnMinimap(room);
		}
		this.m_configured = true;
		this.PossiblyCreateBowler(false);
	}

	// Token: 0x06004DC0 RID: 19904 RVA: 0x001AC544 File Offset: 0x001AA744
	private void RoomEntered(PlayerController enterer)
	{
		if (Chest.m_IsCoopMode && GameManager.HasInstance && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.PrimaryPlayer && GameManager.Instance.PrimaryPlayer.healthHaver.IsAlive && GameManager.Instance.SecondaryPlayer && GameManager.Instance.SecondaryPlayer.healthHaver.IsAlive)
		{
			this.UnbecomeCoopChest();
		}
		if (!Chest.m_IsCoopMode && !this.IsOpen && !this.IsBroken && !this.m_hasBeenCheckedForFuses && !this.PreventFuse && this.ChestIdentifier != Chest.SpecialChestIdentifier.RAT)
		{
			this.m_hasBeenCheckedForFuses = true;
			float num = 0.02f;
			num += (float)PlayerStats.GetTotalCurse() * 0.05f;
			num += (float)PlayerStats.GetTotalCoolness() * -0.025f;
			num = Mathf.Max(0.01f, Mathf.Clamp01(num));
			if (this.lootTable != null && this.lootTable.CompletesSynergy)
			{
				num = 1f;
			}
			if (UnityEngine.Random.value < num)
			{
				this.TriggerCountdownTimer();
				AkSoundEngine.PostEvent("Play_OBJ_fuse_loop_01", base.gameObject);
			}
		}
	}

	// Token: 0x06004DC1 RID: 19905 RVA: 0x001AC698 File Offset: 0x001AA898
	protected override void OnDestroy()
	{
		MajorBreakable majorBreakable = base.majorBreakable;
		majorBreakable.OnDamaged = (Action<float>)Delegate.Remove(majorBreakable.OnDamaged, new Action<float>(this.OnDamaged));
		StaticReferenceManager.AllChests.Remove(this);
		base.OnDestroy();
		AkSoundEngine.PostEvent("Stop_SND_OBJ", base.gameObject);
		AkSoundEngine.PostEvent("stop_obj_fuse_loop_01", base.gameObject);
	}

	// Token: 0x040043B5 RID: 17333
	public static bool HasDroppedResourcefulRatNoteThisSession;

	// Token: 0x040043B6 RID: 17334
	public static bool DoneResourcefulRatMimicThisSession;

	// Token: 0x040043B7 RID: 17335
	public static bool HasDroppedSerJunkanThisSession;

	// Token: 0x040043B8 RID: 17336
	protected const float MULTI_ITEM_SPREAD_FACTOR = 1f;

	// Token: 0x040043B9 RID: 17337
	protected const float ITEM_PRESENTATION_SPEED = 1.5f;

	// Token: 0x040043BA RID: 17338
	public Chest.SpecialChestIdentifier ChestIdentifier;

	// Token: 0x040043BB RID: 17339
	private Chest.GeneralChestType m_chestType;

	// Token: 0x040043BC RID: 17340
	[NonSerialized]
	public List<PickupObject> contents;

	// Token: 0x040043BD RID: 17341
	[PickupIdentifier]
	public List<int> forceContentIds;

	// Token: 0x040043BE RID: 17342
	public LootData lootTable;

	// Token: 0x040043BF RID: 17343
	public float breakertronNothingChance = 0.1f;

	// Token: 0x040043C0 RID: 17344
	public LootData breakertronLootTable;

	// Token: 0x040043C1 RID: 17345
	public bool prefersDungeonProcContents;

	// Token: 0x040043C2 RID: 17346
	public tk2dSprite ShadowSprite;

	// Token: 0x040043C3 RID: 17347
	public bool pickedUp;

	// Token: 0x040043C4 RID: 17348
	[CheckAnimation(null)]
	public string spawnAnimName;

	// Token: 0x040043C5 RID: 17349
	[CheckAnimation(null)]
	public string openAnimName;

	// Token: 0x040043C6 RID: 17350
	[CheckAnimation(null)]
	public string breakAnimName;

	// Token: 0x040043C7 RID: 17351
	[NonSerialized]
	private string overrideSpawnAnimName;

	// Token: 0x040043C8 RID: 17352
	[NonSerialized]
	private string overrideOpenAnimName;

	// Token: 0x040043C9 RID: 17353
	[NonSerialized]
	private string overrideBreakAnimName;

	// Token: 0x040043CA RID: 17354
	[PickupIdentifier]
	public int overrideJunkId = -1;

	// Token: 0x040043CB RID: 17355
	public GameObject VFX_PreSpawn;

	// Token: 0x040043CC RID: 17356
	public GameObject VFX_GroundHit;

	// Token: 0x040043CD RID: 17357
	public float groundHitDelay = 0.73f;

	// Token: 0x040043CE RID: 17358
	public Transform spawnTransform;

	// Token: 0x040043CF RID: 17359
	public AnimationCurve spawnCurve;

	// Token: 0x040043D0 RID: 17360
	public tk2dSpriteAnimator LockAnimator;

	// Token: 0x040043D1 RID: 17361
	public string LockOpenAnim = "lock_open";

	// Token: 0x040043D2 RID: 17362
	public string LockBreakAnim = "lock_break";

	// Token: 0x040043D3 RID: 17363
	public string LockNoKeyAnim = "lock_nokey";

	// Token: 0x040043D4 RID: 17364
	public tk2dSpriteAnimator[] SubAnimators;

	// Token: 0x040043D5 RID: 17365
	[NonSerialized]
	private GameObject minimapIconInstance;

	// Token: 0x040043D6 RID: 17366
	[NonSerialized]
	public bool IsLockBroken;

	// Token: 0x040043D7 RID: 17367
	public bool IsLocked;

	// Token: 0x040043D8 RID: 17368
	public bool IsSealed;

	// Token: 0x040043D9 RID: 17369
	public bool IsOpen;

	// Token: 0x040043DA RID: 17370
	public bool IsBroken;

	// Token: 0x040043DB RID: 17371
	public bool AlwaysBroadcastsOpenEvent;

	// Token: 0x040043DC RID: 17372
	[NonSerialized]
	public float GeneratedMagnificence;

	// Token: 0x040043DD RID: 17373
	protected bool m_temporarilyUnopenable;

	// Token: 0x040043DE RID: 17374
	public bool IsRainbowChest;

	// Token: 0x040043DF RID: 17375
	public bool IsMirrorChest;

	// Token: 0x040043E0 RID: 17376
	[NonSerialized]
	public bool ForceGlitchChest;

	// Token: 0x040043E1 RID: 17377
	protected bool m_isKeyOpening;

	// Token: 0x040043E2 RID: 17378
	private RoomHandler m_room;

	// Token: 0x040043E3 RID: 17379
	private RoomHandler m_registeredIconRoom;

	// Token: 0x040043E4 RID: 17380
	private bool m_isMimic;

	// Token: 0x040043E5 RID: 17381
	private bool m_isMimicBreathing;

	// Token: 0x040043E6 RID: 17382
	private System.Random m_runtimeRandom;

	// Token: 0x040043E7 RID: 17383
	[EnemyIdentifier]
	[Header("Mimic")]
	public string MimicGuid;

	// Token: 0x040043E8 RID: 17384
	public IntVector2 mimicOffset;

	// Token: 0x040043E9 RID: 17385
	[CheckAnimation(null)]
	public string preMimicIdleAnim;

	// Token: 0x040043EA RID: 17386
	public float preMimicIdleAnimDelay = 3f;

	// Token: 0x040043EB RID: 17387
	public float overrideMimicChance = -1f;

	// Token: 0x040043EC RID: 17388
	private static bool m_IsCoopMode;

	// Token: 0x040043ED RID: 17389
	public GameObject MinimapIconPrefab;

	// Token: 0x040043EE RID: 17390
	private const float SPAWN_PUSH_RADIUS = 5f;

	// Token: 0x040043EF RID: 17391
	private const float SPAWN_PUSH_FORCE = 22f;

	// Token: 0x040043F0 RID: 17392
	private bool m_hasCheckedBowler;

	// Token: 0x040043F1 RID: 17393
	private GameObject m_bowlerInstance;

	// Token: 0x040043F2 RID: 17394
	private Tribool m_bowlerFireStatus;

	// Token: 0x040043F3 RID: 17395
	private bool m_secretRainbowRevealed;

	// Token: 0x040043F4 RID: 17396
	private float m_bowlerFireTimer;

	// Token: 0x040043F5 RID: 17397
	private Color m_cachedOutlineColor;

	// Token: 0x040043F6 RID: 17398
	[NonSerialized]
	private ChestFuseController extantFuse;

	// Token: 0x040043F7 RID: 17399
	private const float RESOURCEFULRAT_CHEST_NOTE_CHANCE = 10.025f;

	// Token: 0x040043F8 RID: 17400
	private bool m_forceDropOkayForRainbowRun;

	// Token: 0x040043F9 RID: 17401
	private bool m_isGlitchChest;

	// Token: 0x040043FA RID: 17402
	private bool m_cachedLockedState;

	// Token: 0x040043FB RID: 17403
	private int m_cachedShadowSpriteID = -1;

	// Token: 0x040043FC RID: 17404
	[NonSerialized]
	private int m_cachedSpriteForCoop = -1;

	// Token: 0x040043FD RID: 17405
	private IntVector2 m_cachedCoopManualOffset;

	// Token: 0x040043FE RID: 17406
	private IntVector2 m_cachedCoopManualDimensions;

	// Token: 0x040043FF RID: 17407
	private bool m_configured;

	// Token: 0x04004400 RID: 17408
	private bool m_hasBeenCheckedForFuses;

	// Token: 0x04004401 RID: 17409
	[NonSerialized]
	public bool PreventFuse;

	// Token: 0x02000E44 RID: 3652
	public enum GeneralChestType
	{
		// Token: 0x04004403 RID: 17411
		UNSPECIFIED,
		// Token: 0x04004404 RID: 17412
		WEAPON,
		// Token: 0x04004405 RID: 17413
		ITEM
	}

	// Token: 0x02000E45 RID: 3653
	public enum SpecialChestIdentifier
	{
		// Token: 0x04004407 RID: 17415
		NORMAL,
		// Token: 0x04004408 RID: 17416
		RAT,
		// Token: 0x04004409 RID: 17417
		SECRET_RAINBOW
	}
}
