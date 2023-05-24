using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Brave;
using HutongGames.PlayMaker.Actions;
using Pathfinding;
using tk2dRuntime.TileMap;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dungeonator
{
	// Token: 0x02000EBE RID: 3774
	public class Dungeon : MonoBehaviour
	{
		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x06004FB7 RID: 20407 RVA: 0x001BB05C File Offset: 0x001B925C
		// (remove) Token: 0x06004FB8 RID: 20408 RVA: 0x001BB094 File Offset: 0x001B9294
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<Dungeon, PlayerController> OnDungeonGenerationComplete;

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06004FB9 RID: 20409 RVA: 0x001BB0CC File Offset: 0x001B92CC
		public tk2dTileMap MainTilemap
		{
			get
			{
				return this.m_tilemap;
			}
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x06004FBA RID: 20410 RVA: 0x001BB0D4 File Offset: 0x001B92D4
		public int Width
		{
			get
			{
				return this.data.Width;
			}
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06004FBB RID: 20411 RVA: 0x001BB0E4 File Offset: 0x001B92E4
		public int Height
		{
			get
			{
				return this.data.Height;
			}
		}

		// Token: 0x06004FBC RID: 20412 RVA: 0x001BB0F4 File Offset: 0x001B92F4
		public int GetDungeonSeed()
		{
			if (!BraveRandom.IsInitialized())
			{
				BraveRandom.InitializeRandom();
			}
			int num = GameManager.Instance.CurrentRunSeed;
			if (num == 0)
			{
				num = this.DungeonSeed;
			}
			if (num == 0)
			{
				num = BraveRandom.GenerationRandomRange(1, 1000000000);
			}
			else
			{
				GameManager.Instance.InitializeForRunWithSeed(num);
			}
			return num;
		}

		// Token: 0x06004FBD RID: 20413 RVA: 0x001BB14C File Offset: 0x001B934C
		public DungeonWingDefinition SelectWingDefinition(bool criticalPath)
		{
			List<DungeonWingDefinition> list = new List<DungeonWingDefinition>();
			float num = 0f;
			for (int i = 0; i < this.dungeonWingDefinitions.Length; i++)
			{
				if ((this.dungeonWingDefinitions[i].canBeCriticalPath && criticalPath) || (this.dungeonWingDefinitions[i].canBeNoncriticalPath && !criticalPath))
				{
					list.Add(this.dungeonWingDefinitions[i]);
					num += this.dungeonWingDefinitions[i].weight;
				}
			}
			float num2 = num * BraveRandom.GenerationRandomValue();
			float num3 = 0f;
			for (int j = 0; j < list.Count; j++)
			{
				num3 += list[j].weight;
				if (num3 >= num2)
				{
					return list[j];
				}
			}
			return list[0];
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06004FBE RID: 20414 RVA: 0x001BB220 File Offset: 0x001B9420
		public float TargetAmbientIntensity
		{
			get
			{
				if (GameManager.Instance.IsFoyer)
				{
					return 1f;
				}
				return (GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW) ? 1f : 1.15f;
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x06004FBF RID: 20415 RVA: 0x001BB270 File Offset: 0x001B9470
		// (set) Token: 0x06004FC0 RID: 20416 RVA: 0x001BB278 File Offset: 0x001B9478
		public float ExplosionBulletDeletionMultiplier { get; set; }

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x06004FC1 RID: 20417 RVA: 0x001BB284 File Offset: 0x001B9484
		// (set) Token: 0x06004FC2 RID: 20418 RVA: 0x001BB28C File Offset: 0x001B948C
		public bool IsExplosionBulletDeletionRecovering { get; set; }

		// Token: 0x06004FC3 RID: 20419 RVA: 0x001BB298 File Offset: 0x001B9498
		private IEnumerator Start()
		{
			AkSoundEngine.PostEvent("Play_AMB_sewer_loop_01", base.gameObject);
			bool flag = !GameStatsManager.Instance.IsInSession;
			if (this.PrefabsToAutoSpawn.Length > 0)
			{
				for (int i = 0; i < this.PrefabsToAutoSpawn.Length; i++)
				{
					UnityEngine.Object.Instantiate<GameObject>(this.PrefabsToAutoSpawn[i]);
				}
			}
			if (flag)
			{
				IEnumerator enumerator = this.Regenerate(false);
				while (enumerator.MoveNext())
				{
				}
			}
			else
			{
				base.StartCoroutine(this.Regenerate(false));
			}
			RenderSettings.ambientLight = ((GameManager.Options.LightingQuality != GameOptions.GenericHighMedLowOption.LOW) ? this.decoSettings.ambientLightColor : this.decoSettings.lowQualityAmbientLightColor);
			RenderSettings.ambientIntensity = this.TargetAmbientIntensity;
			if (this.decoSettings.UsesAlienFXFloorColor)
			{
				PlatformInterface.SetAlienFXAmbientColor(this.decoSettings.AlienFXFloorColor);
			}
			else
			{
				PlatformInterface.SetAlienFXAmbientColor(RenderSettings.ambientLight);
			}
			yield break;
		}

		// Token: 0x06004FC4 RID: 20420 RVA: 0x001BB2B4 File Offset: 0x001B94B4
		public void RegenerationCleanup()
		{
			GameObject gameObject = GameObject.Find("A*");
			if (gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(gameObject);
			}
			GameObject gameObject2 = GameObject.Find("_Lights");
			if (gameObject2 != null)
			{
				UnityEngine.Object.DestroyImmediate(gameObject2);
			}
			GameObject gameObject3 = GameObject.Find("_Rooms");
			if (gameObject3 != null)
			{
				UnityEngine.Object.DestroyImmediate(gameObject3);
			}
			GameObject gameObject4 = GameObject.Find("_Doors");
			if (gameObject4 != null)
			{
				UnityEngine.Object.DestroyImmediate(gameObject4);
			}
			GameObject gameObject5 = GameObject.Find("_SpawnManager");
			if (gameObject5 != null)
			{
				UnityEngine.Object.DestroyImmediate(gameObject5);
			}
		}

		// Token: 0x06004FC5 RID: 20421 RVA: 0x001BB358 File Offset: 0x001B9558
		private void StartupFoyerChecks()
		{
			UnityEngine.Debug.LogError("Doing startup foyer checks!");
			if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHOP_HAS_MET_BEETLE) && !GameStatsManager.Instance.GetFlag(GungeonFlags.SHOP_BEETLE_ACTIVE) && GameStatsManager.Instance.AccumulatedBeetleMerchantChance > 0f)
			{
				float num = GameStatsManager.Instance.AccumulatedBeetleMerchantChance + GameStatsManager.Instance.AccumulatedUsedBeetleMerchantChance;
				if (UnityEngine.Random.value < num)
				{
					GameStatsManager.Instance.AccumulatedBeetleMerchantChance = 0f;
					GameStatsManager.Instance.AccumulatedUsedBeetleMerchantChance = 0f;
					GameStatsManager.Instance.SetFlag(GungeonFlags.SHOP_BEETLE_ACTIVE, true);
				}
				else
				{
					GameStatsManager.Instance.AccumulatedUsedBeetleMerchantChance += GameStatsManager.Instance.AccumulatedBeetleMerchantChance;
					GameStatsManager.Instance.AccumulatedBeetleMerchantChance = 0f;
				}
			}
		}

		// Token: 0x06004FC6 RID: 20422 RVA: 0x001BB42C File Offset: 0x001B962C
		public IEnumerator Regenerate(bool cleanup)
		{
			if (cleanup)
			{
				this.RegenerationCleanup();
			}
			if (this.LevelOverrideType == GameManager.LevelOverrideState.FOYER)
			{
				this.StartupFoyerChecks();
			}
			this.FrameDungeonGenerationFinished = -1;
			Dungeon.IsGenerating = true;
			float elapsedTimeAtStartup = Time.realtimeSinceStartup;
			int frameAtStartup = Time.frameCount;
			GameManager.Instance.InTutorial = this.SetTutorialFlag;
			MidGameSaveData midgameSave = null;
			if (Dungeon.ShouldAttemptToLoadFromMidgameSave && !GameManager.VerifyAndLoadMidgameSave(out midgameSave, true))
			{
				midgameSave = null;
			}
			Dungeon.ShouldAttemptToLoadFromMidgameSave = false;
			if (midgameSave != null)
			{
				List<string> list = new List<string>(Brave.PlayerPrefs.GetStringArray("recent_mgs", ';'));
				list.Insert(0, midgameSave.midGameSaveGuid);
				while (list.Count > 5)
				{
					list.RemoveAt(list.Count - 1);
				}
				Brave.PlayerPrefs.SetStringArray("recent_mgs", list.ToArray(), ';');
			}
			this.GeneratePlayerIfNecessary(midgameSave);
			int dSeed = this.GetDungeonSeed();
			UnityEngine.Random.InitState(dSeed);
			BraveRandom.InitializeWithSeed(dSeed);
			if (!MetaInjectionData.BlueprintGenerated && GameManager.Instance.GlobalInjectionData != null)
			{
				GameManager.Instance.GlobalInjectionData.PreprocessRun(false);
			}
			if (this.debugSettings.RAPID_DEBUG_DUNGEON_ITERATION)
			{
				for (int i = 0; i < this.debugSettings.RAPID_DEBUG_DUNGEON_COUNT; i++)
				{
					float realtimeSinceStartup = Time.realtimeSinceStartup;
					int num = i + 1;
					this.DungeonSeed = num;
					new LoopDungeonGenerator(this, this.GetDungeonSeed())
					{
						RAPID_DEBUG_ITERATION_MODE = true,
						RAPID_DEBUG_ITERATION_INDEX = i
					}.GenerateDungeonLayout();
					float num2 = Time.realtimeSinceStartup - realtimeSinceStartup;
					UnityEngine.Debug.Log("seed #" + num.ToString() + " took " + num2.ToString());
				}
				yield break;
			}
			DungeonData d = null;
			if (GameManager.Instance.PregeneratedDungeonData == null)
			{
				LoopDungeonGenerator loopDungeonGenerator = new LoopDungeonGenerator(this, this.GetDungeonSeed());
				d = loopDungeonGenerator.GenerateDungeonLayout();
			}
			else
			{
				d = GameManager.Instance.PregeneratedDungeonData;
				GameManager.Instance.PregeneratedDungeonData = null;
			}
			this.data = d;
			BraveUtility.Log("Layout phase complete.", Color.green, BraveUtility.LogVerbosity.IMPORTANT);
			if (this.assembler == null)
			{
				this.assembler = new TK2DDungeonAssembler();
			}
			this.assembler.Initialize(this.tileIndices);
			if (this.m_tilemap == null)
			{
				GameObject gameObject = GameObject.Find("TileMap");
				this.m_tilemap = gameObject.GetComponent<tk2dTileMap>();
			}
			else
			{
				this.assembler.ClearData(this.m_tilemap);
			}
			BraveUtility.Log("Dungeon Data Phase 1 Complete @ " + (Time.realtimeSinceStartup - elapsedTimeAtStartup), Color.green, BraveUtility.LogVerbosity.IMPORTANT);
			DebugTime.Log(elapsedTimeAtStartup, frameAtStartup, "Dungeon Data Phase 1 Complete", new object[0]);
			IEnumerator ApplicationTracker = d.Apply(this.tileIndices, this.decoSettings, this.m_tilemap);
			while (ApplicationTracker.MoveNext())
			{
				yield return null;
			}
			d.CheckIntegrity();
			if (this.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON && GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_BULLET_COMPLETE))
			{
				this.PlaceRatGrate();
			}
			if ((this.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.GUNGEON || this.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON || this.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CATACOMBGEON) && GameStatsManager.Instance.AnyPastBeaten() && !GameStatsManager.Instance.GetFlag(GungeonFlags.FLAG_EEVEE_UNLOCKED) && (!GameManager.Instance.PrimaryPlayer || !GameManager.Instance.PrimaryPlayer.IsTemporaryEeveeForUnlock) && UnityEngine.Random.value < 0.2f)
			{
				this.PlaceParadoxPortal();
			}
			this.PlaceWallMimics(null);
			TK2DDungeonAssembler.RuntimeResizeTileMap(this.m_tilemap, d.Width, d.Height, this.m_tilemap.partitionSizeX, this.m_tilemap.partitionSizeY);
			IEnumerator AssemblyTracker = this.assembler.ConstructTK2DDungeon(this, this.m_tilemap);
			while (AssemblyTracker.MoveNext())
			{
				yield return null;
			}
			d.PostProcessFeatures();
			TelevisionQuestController.RemoveMaintenanceRoomBackpack();
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.NONE)
			{
				TelevisionQuestController.HandlePuzzleSetup();
			}
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.NONE || GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
			{
				RobotArmQuestController.HandlePuzzleSetup();
			}
			BraveUtility.Log("Dungeon Data Phase 2 Complete @ " + (Time.realtimeSinceStartup - elapsedTimeAtStartup), Color.green, BraveUtility.LogVerbosity.IMPORTANT);
			DebugTime.Log(elapsedTimeAtStartup, frameAtStartup, "Dungeon Data Phase 2 Complete", new object[0]);
			if (Minimap.Instance != null)
			{
				Minimap.Instance.InitializeMinimap(d);
			}
			RoomHandler startRoom = d.Entrance;
			this.PlacePlayerInRoom(this.m_tilemap, startRoom);
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
			{
				if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.NUMBER_DEATHS) == 0f)
				{
					GameUIRoot.Instance.levelNameUI.ShowLevelName(this);
				}
			}
			else
			{
				GameUIRoot.Instance.levelNameUI.ShowLevelName(this);
			}
			Pathfinder.Instance.Initialize(d);
			ShadowSystem.ForceAllLightsUpdate();
			GameManager.Instance.ClearGenerativeDungeonData();
			this.data.PostGenerationCleanup();
			if (midgameSave != null)
			{
				midgameSave.LoadPreGenDataFromMidGameSave();
			}
			this.FloorReached();
			IEnumerator PostGenerationTracker = this.PostDungeonGenerationCleanup();
			while (PostGenerationTracker.MoveNext())
			{
				yield return null;
			}
			if (midgameSave != null)
			{
				if (midgameSave.StaticShopData != null)
				{
					BaseShopController.LoadFromMidGameSave(midgameSave.StaticShopData);
				}
				base.StartCoroutine(this.FrameDelayedMidgameInitialization(midgameSave));
			}
			this.FrameDungeonGenerationFinished = Time.frameCount;
			Dungeon.IsGenerating = false;
			BossManager.PriorFloorSelectedBossRoom = null;
			if (this.OnDungeonGenerationComplete != null)
			{
				this.OnDungeonGenerationComplete(this, GameManager.Instance.PrimaryPlayer);
			}
			this.AssignCurrencyDrops();
			if (GameStatsManager.Instance.IsRainbowRun)
			{
				List<TeleporterController> componentsAbsoluteInRoom = this.data.Entrance.GetComponentsAbsoluteInRoom<TeleporterController>();
				Vector3? vector = null;
				if (componentsAbsoluteInRoom != null && componentsAbsoluteInRoom.Count > 0)
				{
					vector = new Vector3?(componentsAbsoluteInRoom[0].transform.position + new Vector3(0.5f, 2f, 0f));
				}
				if (vector == null)
				{
					bool flag = false;
					IntVector2 centeredVisibleClearSpot = this.data.Entrance.GetCenteredVisibleClearSpot(4, 4, out flag, true);
					if (flag)
					{
						vector = new Vector3?((centeredVisibleClearSpot + IntVector2.One).ToVector2().ToVector3ZisY(0f));
					}
				}
				if (vector != null)
				{
					Chest chest = Chest.Spawn(GameManager.Instance.RewardManager.A_Chest, vector.Value, this.data.Entrance, true);
					chest.IsRainbowChest = true;
					chest.BecomeRainbowChest();
				}
			}
			for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[j];
				if (playerController)
				{
					playerController.specRigidbody.PhysicsRegistration = SpeculativeRigidbody.RegistrationState.Unknown;
					PhysicsEngine.Instance.Register(playerController.specRigidbody);
				}
			}
			yield break;
		}

		// Token: 0x06004FC7 RID: 20423 RVA: 0x001BB450 File Offset: 0x001B9650
		private IEnumerator FrameDelayedMidgameInitialization(MidGameSaveData midgameSave)
		{
			yield return null;
			if (midgameSave != null)
			{
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					midgameSave.LoadDataFromMidGameSave(GameManager.Instance.PrimaryPlayer, GameManager.Instance.SecondaryPlayer);
				}
				else
				{
					midgameSave.LoadDataFromMidGameSave(GameManager.Instance.PrimaryPlayer, null);
				}
			}
			yield break;
		}

		// Token: 0x06004FC8 RID: 20424 RVA: 0x001BB46C File Offset: 0x001B966C
		public void AssignCurrencyDrops()
		{
			FloorRewardData currentRewardData = GameManager.Instance.RewardManager.CurrentRewardData;
			float randomByNormalDistribution = BraveMathCollege.GetRandomByNormalDistribution(currentRewardData.AverageCurrencyDropsThisFloor, currentRewardData.CurrencyDropsStandardDeviation);
			float num = Mathf.Max(20f, currentRewardData.MinimumCurrencyDropsThisFloor);
			int num2 = Mathf.CeilToInt(Mathf.Clamp(randomByNormalDistribution, num, 250f));
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.NONE && GameManager.Instance.PrimaryPlayer && PrepareTakeSherpaPickup.IsOnSherpaMoneyStep && GameManager.Instance.PrimaryPlayer.carriedConsumables.KeyBullets > 1)
			{
				num2 = Mathf.CeilToInt(Mathf.Max((float)num2, currentRewardData.AverageCurrencyDropsThisFloor + currentRewardData.CurrencyDropsStandardDeviation));
			}
			float bossGoldCoinChance = GameManager.Instance.RewardManager.BossGoldCoinChance;
			float powerfulGoldCoinChance = GameManager.Instance.RewardManager.PowerfulGoldCoinChance;
			float normalGoldCoinChance = GameManager.Instance.RewardManager.NormalGoldCoinChance;
			List<AIActor> list = new List<AIActor>();
			List<AIActor> list2 = new List<AIActor>();
			List<AIActor> list3 = new List<AIActor>();
			for (int i = 0; i < StaticReferenceManager.AllEnemies.Count; i++)
			{
				AIActor aiactor = StaticReferenceManager.AllEnemies[i];
				if (aiactor)
				{
					if (aiactor.CanDropCurrency)
					{
						if (aiactor.healthHaver && aiactor.healthHaver.IsBoss)
						{
							list.Add(aiactor);
						}
						else if (aiactor.IsSignatureEnemy)
						{
							list2.Add(aiactor);
						}
						else
						{
							list3.Add(aiactor);
						}
					}
				}
			}
			int num3 = list3.Count + list2.Count;
			for (int j = 0; j < list3.Count; j++)
			{
				if (!(list3[j].EnemyGuid == GlobalEnemyGuids.GripMaster))
				{
					if (!list3[j].IsMimicEnemy)
					{
						this.RegisterGeneratedEnemyData(list3[j].EnemyGuid, num3, false);
					}
				}
			}
			for (int k = 0; k < list2.Count; k++)
			{
				if (!(list2[k].EnemyGuid == GlobalEnemyGuids.GripMaster))
				{
					if (!list2[k].IsMimicEnemy)
					{
						this.RegisterGeneratedEnemyData(list2[k].EnemyGuid, num3, true);
					}
				}
			}
			int num4 = ((list.Count <= 0) ? 0 : BraveRandom.GenerationRandomRange(5, num2 / 4));
			int num5 = list2.Count * 10;
			int num6 = ((list2.Count <= 0) ? 0 : Mathf.Min(num5, Mathf.FloorToInt((float)(num2 - num4) * 0.5f)));
			int num7 = num2 - (num4 + num6);
			int num8 = Mathf.CeilToInt((float)num4 / (float)list.Count);
			int num9 = Mathf.CeilToInt((float)num6 / (float)list2.Count);
			for (int l = 0; l < list.Count; l++)
			{
				list[l].AssignedCurrencyToDrop = Mathf.Min(num8, num4);
				num4 -= num8;
				if (BraveRandom.GenerationRandomValue() < bossGoldCoinChance)
				{
					list[l].AssignedCurrencyToDrop += 50;
				}
			}
			for (int m = 0; m < list2.Count; m++)
			{
				list2[m].AssignedCurrencyToDrop = Mathf.Min(num6, num9);
				num6 -= num9;
				if (BraveRandom.GenerationRandomValue() < powerfulGoldCoinChance)
				{
					list2[m].AssignedCurrencyToDrop += 50;
				}
			}
			while (num7 > 0 && list3.Count > 0)
			{
				list3[BraveRandom.GenerationRandomRange(0, list3.Count)].AssignedCurrencyToDrop++;
				num7--;
			}
			for (int n = 0; n < list3.Count; n++)
			{
				if (BraveRandom.GenerationRandomValue() < normalGoldCoinChance)
				{
					list3[n].AssignedCurrencyToDrop += 50;
				}
			}
		}

		// Token: 0x06004FC9 RID: 20425 RVA: 0x001BB8B0 File Offset: 0x001B9AB0
		private IEnumerator ForceRegenerateDelayed()
		{
			yield return new WaitForSeconds(5f);
			GameManager.Instance.LoadCustomLevel("tt_castle");
			yield break;
		}

		// Token: 0x06004FCA RID: 20426 RVA: 0x001BB8C4 File Offset: 0x001B9AC4
		private IEnumerator PostDungeonGenerationCleanup()
		{
			if ((Application.platform != RuntimePlatform.XboxOne && Application.platform != RuntimePlatform.MetroPlayerX64 && Application.platform != RuntimePlatform.MetroPlayerX86) || this.LevelOverrideType != GameManager.LevelOverrideState.FOYER)
			{
				for (int j = 0; j < this.PatternSettings.flows.Count; j++)
				{
					Resources.UnloadAsset(this.PatternSettings.flows[j]);
				}
				this.PatternSettings = null;
			}
			for (int i = 0; i < this.data.rooms.Count; i++)
			{
				this.data.rooms[i].PostGenerationCleanup();
				if (i % 5 == 0)
				{
					yield return null;
				}
			}
			HUDGC.SkipNextGC = true;
			Resources.UnloadUnusedAssets();
			BraveMemory.DoCollect();
			yield break;
		}

		// Token: 0x06004FCB RID: 20427 RVA: 0x001BB8E0 File Offset: 0x001B9AE0
		public tk2dTileMap DestroyWallAtPosition(int ix, int iy, bool deferRebuild = true)
		{
			if (this.data.cellData[ix][iy] == null)
			{
				return null;
			}
			if (this.data.cellData[ix][iy].type != CellType.WALL)
			{
				return null;
			}
			if (!this.data.cellData[ix][iy].breakable)
			{
				return null;
			}
			this.data.cellData[ix][iy].type = CellType.FLOOR;
			if (this.data.isSingleCellWall(ix, iy + 1))
			{
				this.data[ix, iy + 1].type = CellType.FLOOR;
			}
			if (this.data.isSingleCellWall(ix, iy - 1))
			{
				this.data[ix, iy - 1].type = CellType.FLOOR;
			}
			RoomHandler parentRoom = this.data.cellData[ix][iy].parentRoom;
			tk2dTileMap tk2dTileMap = ((parentRoom == null || !(parentRoom.OverrideTilemap != null)) ? this.m_tilemap : parentRoom.OverrideTilemap);
			for (int i = -1; i < 2; i++)
			{
				for (int j = -2; j < 4; j++)
				{
					CellData cellData = this.data.cellData[ix + i][iy + j];
					if (cellData != null)
					{
						cellData.hasBeenGenerated = false;
						if (cellData.parentRoom != null)
						{
							List<GameObject> list = new List<GameObject>();
							for (int k = 0; k < cellData.parentRoom.hierarchyParent.childCount; k++)
							{
								Transform child = cellData.parentRoom.hierarchyParent.GetChild(k);
								if (child.name.StartsWith("Chunk_"))
								{
									list.Add(child.gameObject);
								}
							}
							for (int l = list.Count - 1; l >= 0; l--)
							{
								UnityEngine.Object.Destroy(list[l]);
							}
						}
						this.assembler.ClearTileIndicesForCell(this, tk2dTileMap, cellData.position.x, cellData.position.y);
						this.assembler.BuildTileIndicesForCell(this, tk2dTileMap, cellData.position.x, cellData.position.y);
						cellData.HasCachedPhysicsTile = false;
						cellData.CachedPhysicsTile = null;
					}
				}
			}
			if (!deferRebuild)
			{
				this.RebuildTilemap(tk2dTileMap);
			}
			return tk2dTileMap;
		}

		// Token: 0x06004FCC RID: 20428 RVA: 0x001BBB34 File Offset: 0x001B9D34
		public tk2dTileMap ConstructWallAtPosition(int ix, int iy, bool deferRebuild = true)
		{
			if (this.data.cellData[ix][iy].type == CellType.WALL)
			{
				return null;
			}
			this.data.cellData[ix][iy].type = CellType.WALL;
			RoomHandler parentRoom = this.data.cellData[ix][iy].parentRoom;
			tk2dTileMap tk2dTileMap = ((parentRoom == null || !(parentRoom.OverrideTilemap != null)) ? this.m_tilemap : parentRoom.OverrideTilemap);
			for (int i = -1; i < 2; i++)
			{
				for (int j = -2; j < 4; j++)
				{
					CellData cellData = this.data.cellData[ix + i][iy + j];
					if (cellData != null)
					{
						cellData.hasBeenGenerated = false;
						if (cellData.parentRoom != null)
						{
							List<GameObject> list = new List<GameObject>();
							for (int k = 0; k < cellData.parentRoom.hierarchyParent.childCount; k++)
							{
								Transform child = cellData.parentRoom.hierarchyParent.GetChild(k);
								if (child.name.StartsWith("Chunk_"))
								{
									list.Add(child.gameObject);
								}
							}
							for (int l = list.Count - 1; l >= 0; l--)
							{
								UnityEngine.Object.Destroy(list[l]);
							}
						}
						this.assembler.ClearTileIndicesForCell(this, tk2dTileMap, cellData.position.x, cellData.position.y);
						this.assembler.BuildTileIndicesForCell(this, tk2dTileMap, cellData.position.x, cellData.position.y);
						cellData.HasCachedPhysicsTile = false;
						cellData.CachedPhysicsTile = null;
					}
				}
			}
			if (!deferRebuild)
			{
				this.RebuildTilemap(tk2dTileMap);
			}
			return tk2dTileMap;
		}

		// Token: 0x06004FCD RID: 20429 RVA: 0x001BBD04 File Offset: 0x001B9F04
		public void RebuildTilemap(tk2dTileMap targetTilemap)
		{
			RenderMeshBuilder.CurrentCellXOffset = Mathf.RoundToInt(targetTilemap.renderData.transform.position.x);
			RenderMeshBuilder.CurrentCellYOffset = Mathf.RoundToInt(targetTilemap.renderData.transform.position.y);
			targetTilemap.Build();
			targetTilemap.renderData.transform.position = new Vector3((float)RenderMeshBuilder.CurrentCellXOffset, (float)RenderMeshBuilder.CurrentCellYOffset, (float)RenderMeshBuilder.CurrentCellYOffset);
			RenderMeshBuilder.CurrentCellXOffset = 0;
			RenderMeshBuilder.CurrentCellYOffset = 0;
		}

		// Token: 0x06004FCE RID: 20430 RVA: 0x001BBD90 File Offset: 0x001B9F90
		public void InformRoomCleared(bool rewardDropped, bool rewardIsChest)
		{
			if (rewardDropped)
			{
				if (rewardIsChest)
				{
					GameManager.Instance.PrimaryPlayer.AdditionalChestSpawnChance = 0f;
					BraveUtility.Log("Spawning a chest: flooring chest spawn chance.", Color.yellow, BraveUtility.LogVerbosity.IMPORTANT);
				}
				else
				{
					GameManager.Instance.PrimaryPlayer.AdditionalChestSpawnChance = 0f;
					BraveUtility.Log("Spawning a single item: flooring chest spawn chance.", Color.yellow, BraveUtility.LogVerbosity.IMPORTANT);
				}
			}
			else
			{
				float num = GameManager.Instance.RewardManager.CurrentRewardData.ChestSystem_Increment;
				if (PassiveItem.IsFlagSetForCharacter(GameManager.Instance.PrimaryPlayer, typeof(AmazingChestAheadItem)))
				{
					num *= AmazingChestAheadItem.ChestIncrementMultiplier;
					int num2 = 0;
					if (PlayerController.AnyoneHasActiveBonusSynergy(CustomSynergyType.DOUBLE_CHEST_FRIENDS, out num2))
					{
						num *= 1.25f;
					}
				}
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					num *= GameManager.Instance.RewardManager.CoopPickupIncrementModifier;
				}
				else
				{
					num *= GameManager.Instance.RewardManager.SinglePlayerPickupIncrementModifier;
				}
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"LootSystem::",
					GameManager.Instance.PrimaryPlayer.AdditionalChestSpawnChance,
					" + ",
					num
				}));
				GameManager.Instance.PrimaryPlayer.AdditionalChestSpawnChance += num;
			}
		}

		// Token: 0x06004FCF RID: 20431 RVA: 0x001BBEE4 File Offset: 0x001BA0E4
		public void FloorReached()
		{
			string text = this.DungeonFloorName.Replace("#", string.Empty);
			GameManager.Instance.platformInterface.SetPresence(text);
			if (this.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.RATGEON)
			{
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_REACHED_RATGEON, 1f);
			}
			if (GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.NONE)
			{
				return;
			}
			GlobalDungeonData.ValidTilesets tilesetId = this.tileIndices.tilesetId;
			switch (tilesetId)
			{
			case GlobalDungeonData.ValidTilesets.GUNGEON:
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_REACHED_GUNGEON, 1f);
				break;
			case GlobalDungeonData.ValidTilesets.CASTLEGEON:
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.NUMBER_ATTEMPTS, 1f);
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.RUNS_PLAYED_POST_FTA, 1f);
				break;
			default:
				if (tilesetId != GlobalDungeonData.ValidTilesets.MINEGEON)
				{
					if (tilesetId != GlobalDungeonData.ValidTilesets.CATACOMBGEON)
					{
						if (tilesetId != GlobalDungeonData.ValidTilesets.FORGEGEON)
						{
							if (tilesetId != GlobalDungeonData.ValidTilesets.HELLGEON)
							{
								if (tilesetId != GlobalDungeonData.ValidTilesets.SPACEGEON)
								{
									if (tilesetId != GlobalDungeonData.ValidTilesets.PHOBOSGEON)
									{
										if (tilesetId != GlobalDungeonData.ValidTilesets.WESTGEON)
										{
											if (tilesetId != GlobalDungeonData.ValidTilesets.OFFICEGEON)
											{
												if (tilesetId != GlobalDungeonData.ValidTilesets.BELLYGEON)
												{
													if (tilesetId != GlobalDungeonData.ValidTilesets.JUNGLEGEON)
													{
														if (tilesetId == GlobalDungeonData.ValidTilesets.RATGEON)
														{
															GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_REACHED_RATGEON, 1f);
														}
													}
													else
													{
														GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_REACHED_JUNGLE, 1f);
													}
												}
												else
												{
													GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_REACHED_BELLY, 1f);
												}
											}
											else
											{
												GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_REACHED_NAKATOMI, 1f);
											}
										}
										else
										{
											GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_REACHED_WEST, 1f);
										}
									}
									else
									{
										GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_REACHED_PHOBOS, 1f);
									}
								}
								else
								{
									GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_REACHED_FUTURE, 1f);
								}
							}
							else
							{
								GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_REACHED_BULLET_HELL, 1f);
							}
						}
						else
						{
							GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_REACHED_FORGE, 1f);
						}
					}
					else
					{
						GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_REACHED_CATACOMBS, 1f);
					}
				}
				else
				{
					GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_REACHED_MINES, 1f);
				}
				break;
			case GlobalDungeonData.ValidTilesets.SEWERGEON:
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_REACHED_SEWERS, 1f);
				break;
			case GlobalDungeonData.ValidTilesets.CATHEDRALGEON:
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_REACHED_CATHEDRAL, 1f);
				break;
			}
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.NONE)
			{
				if (this.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON)
				{
					GameStatsManager.Instance.isChump = true;
				}
				else
				{
					GameStatsManager.Instance.isChump = false;
				}
			}
			if (!GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_UNLOCK_COMPANION_SHRINE) && GameStatsManager.Instance.GetNumberOfCompanionsUnlocked() >= 5)
			{
				GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_UNLOCK_COMPANION_SHRINE, true);
			}
			if (ChallengeManager.CHALLENGE_MODE_ACTIVE && this.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON)
			{
				GameStatsManager.Instance.SetFlag(GungeonFlags.DAISUKE_CHALLENGE_HALFITEM_UNLOCK, true);
			}
			if (!GameStatsManager.Instance.GetFlag(GungeonFlags.SECRET_BULLETMAN_SEEN_05))
			{
				float num = 0.04f;
				GlobalDungeonData.ValidTilesets tilesetId2 = this.tileIndices.tilesetId;
				if (tilesetId2 != GlobalDungeonData.ValidTilesets.GUNGEON && tilesetId2 != GlobalDungeonData.ValidTilesets.CATHEDRALGEON)
				{
					if (tilesetId2 != GlobalDungeonData.ValidTilesets.MINEGEON)
					{
						if (tilesetId2 != GlobalDungeonData.ValidTilesets.CATACOMBGEON)
						{
							if (tilesetId2 != GlobalDungeonData.ValidTilesets.FORGEGEON)
							{
								if (tilesetId2 == GlobalDungeonData.ValidTilesets.HELLGEON)
								{
									num = 0.25f;
								}
							}
							else
							{
								num = 0.2f;
							}
						}
						else
						{
							num = 0.16f;
						}
					}
					else
					{
						num = 0.12f;
					}
				}
				else
				{
					num = 0.08f;
				}
				if (GameStatsManager.Instance.AnyPastBeaten() && UnityEngine.Random.value < num)
				{
					List<int> list = Enumerable.Range(0, this.data.rooms.Count).ToList<int>();
					list = list.Shuffle<int>();
					for (int i = 0; i < list.Count; i++)
					{
						if (this.data.rooms[list[i]].area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.NORMAL && this.data.rooms[list[i]].EverHadEnemies && this.data.rooms[list[i]].AddMysteriousBulletManToRoom())
						{
							break;
						}
					}
				}
			}
			int numKeybulletMenForFloor = MetaInjectionData.GetNumKeybulletMenForFloor(this.tileIndices.tilesetId);
			if (numKeybulletMenForFloor > 0)
			{
				List<RoomHandler> list2 = new List<RoomHandler>();
				for (int j = 0; j < numKeybulletMenForFloor; j++)
				{
					List<int> list3 = Enumerable.Range(0, this.data.rooms.Count).ToList<int>();
					list3 = list3.Shuffle<int>();
					bool flag = false;
					for (int k = 0; k < 2; k++)
					{
						for (int l = 0; l < list3.Count; l++)
						{
							RoomHandler roomHandler = this.data.rooms[list3[l]];
							if (!list2.Contains(roomHandler) || UnityEngine.Random.value <= 0.1f)
							{
								if ((k == 1 || roomHandler.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.HUB) && roomHandler.EverHadEnemies && roomHandler.area.PrototypeRoomCategory != PrototypeDungeonRoom.RoomCategory.BOSS)
								{
									flag = true;
									roomHandler.AddSpecificEnemyToRoomProcedurally(GameManager.Instance.RewardManager.KeybulletsChances.EnemyGuid, false, null);
									list2.Add(roomHandler);
									break;
								}
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
			}
			int numChanceBulletMenForFloor = MetaInjectionData.GetNumChanceBulletMenForFloor(this.tileIndices.tilesetId);
			if (numChanceBulletMenForFloor > 0)
			{
				List<RoomHandler> list4 = new List<RoomHandler>();
				for (int m = 0; m < numChanceBulletMenForFloor; m++)
				{
					List<int> list5 = Enumerable.Range(0, this.data.rooms.Count).ToList<int>();
					list5 = list5.Shuffle<int>();
					bool flag2 = false;
					for (int n = 0; n < 2; n++)
					{
						for (int num2 = 0; num2 < list5.Count; num2++)
						{
							RoomHandler roomHandler2 = this.data.rooms[list5[num2]];
							if (!list4.Contains(roomHandler2) || UnityEngine.Random.value <= 0.1f)
							{
								if ((n == 1 || roomHandler2.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.HUB) && roomHandler2.EverHadEnemies && roomHandler2.area.PrototypeRoomCategory != PrototypeDungeonRoom.RoomCategory.BOSS)
								{
									flag2 = true;
									roomHandler2.AddSpecificEnemyToRoomProcedurally(GameManager.Instance.RewardManager.ChanceBulletChances.EnemyGuid, false, null);
									list4.Add(roomHandler2);
									break;
								}
							}
						}
						if (flag2)
						{
							break;
						}
					}
				}
			}
			if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIME_PLAYED) > 18000f && UnityEngine.Random.value < GameManager.Instance.RewardManager.FacelessChancePerFloor)
			{
				List<int> list6 = Enumerable.Range(0, this.data.rooms.Count).ToList<int>();
				list6 = list6.Shuffle<int>();
				for (int num3 = 0; num3 < list6.Count; num3++)
				{
					RoomHandler roomHandler3 = this.data.rooms[list6[num3]];
					if (roomHandler3.EverHadEnemies && (roomHandler3.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.HUB || roomHandler3.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) > 6))
					{
						AIActor toughestEnemy = roomHandler3.GetToughestEnemy();
						if (toughestEnemy)
						{
							UnityEngine.Object.Destroy(toughestEnemy.gameObject);
						}
						roomHandler3.AddSpecificEnemyToRoomProcedurally(GameManager.Instance.RewardManager.FacelessCultistGuid, false, null);
						break;
					}
				}
			}
			this.HandleAGDInjection();
			for (int num4 = 0; num4 < GameManager.Instance.AllPlayers.Length; num4++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[num4];
				if (playerController)
				{
					playerController.HasReceivedNewGunThisFloor = false;
					playerController.HasTakenDamageThisFloor = false;
				}
			}
		}

		// Token: 0x06004FD0 RID: 20432 RVA: 0x001BC738 File Offset: 0x001BA938
		private void HandleAGDInjection()
		{
			List<AIActor> list = new List<AIActor>();
			RunData runData = GameManager.Instance.RunData;
			if (runData == null)
			{
				runData = new RunData();
			}
			if (runData.AgdInjectionRunCounts == null || runData.AgdInjectionRunCounts.Length != GameManager.Instance.EnemyReplacementTiers.Count)
			{
				runData.AgdInjectionRunCounts = new int[GameManager.Instance.EnemyReplacementTiers.Count];
			}
			int[] agdInjectionRunCounts = runData.AgdInjectionRunCounts;
			List<RoomHandler> list2 = new List<RoomHandler>();
			if (this.data != null && this.data.rooms != null)
			{
				list2.AddRange(this.data.rooms);
			}
			for (int i = 0; i < GameManager.Instance.EnemyReplacementTiers.Count; i++)
			{
				AGDEnemyReplacementTier agdenemyReplacementTier = GameManager.Instance.EnemyReplacementTiers[i];
				int num = 0;
				if (agdenemyReplacementTier != null)
				{
					if (this.tileIndices == null || (this.tileIndices.tilesetId & agdenemyReplacementTier.TargetTileset) == this.tileIndices.tilesetId)
					{
						if (!agdenemyReplacementTier.ExcludeForPrereqs())
						{
							if (agdenemyReplacementTier.MaxPerRun <= 0 || agdInjectionRunCounts[i] < agdenemyReplacementTier.MaxPerRun)
							{
								BraveUtility.RandomizeList<RoomHandler>(list2, 0, -1);
								foreach (RoomHandler roomHandler in list2)
								{
									if (roomHandler.EverHadEnemies)
									{
										if (roomHandler.IsStandardRoom)
										{
											if (!agdenemyReplacementTier.ExcludeRoomForColumns(this.data, roomHandler))
											{
												if (!agdenemyReplacementTier.ExcludeRoom(roomHandler))
												{
													roomHandler.GetActiveEnemies(RoomHandler.ActiveEnemyType.All, ref list);
													if (!agdenemyReplacementTier.ExcludeRoomForEnemies(roomHandler, list))
													{
														for (int j = 0; j < list.Count; j++)
														{
															AIActor aiactor = list[j];
															if (aiactor)
															{
																if (aiactor.AdditionalSimpleItemDrops == null || aiactor.AdditionalSimpleItemDrops.Count <= 0)
																{
																	if (!aiactor.healthHaver || !aiactor.healthHaver.IsBoss)
																	{
																		if (((agdenemyReplacementTier.TargetAllSignatureEnemies && aiactor.IsSignatureEnemy) || (agdenemyReplacementTier.TargetAllNonSignatureEnemies && !aiactor.IsSignatureEnemy) || (agdenemyReplacementTier.TargetGuids != null && agdenemyReplacementTier.TargetGuids.Contains(aiactor.EnemyGuid))) && UnityEngine.Random.value < agdenemyReplacementTier.ChanceToReplace)
																		{
																			Vector2? vector = null;
																			if (agdenemyReplacementTier.RemoveAllOtherEnemies)
																			{
																				vector = new Vector2?(roomHandler.area.Center);
																				for (int k = list.Count - 1; k >= 0; k--)
																				{
																					AIActor aiactor2 = list[j];
																					if (aiactor2)
																					{
																						roomHandler.DeregisterEnemy(aiactor2, true);
																						UnityEngine.Object.Destroy(aiactor2.gameObject);
																					}
																				}
																			}
																			else
																			{
																				if (aiactor.specRigidbody)
																				{
																					aiactor.specRigidbody.Initialize();
																					vector = new Vector2?(aiactor.specRigidbody.UnitBottomLeft);
																				}
																				roomHandler.DeregisterEnemy(aiactor, true);
																				UnityEngine.Object.Destroy(aiactor.gameObject);
																			}
																			RoomHandler roomHandler2 = roomHandler;
																			string text = BraveUtility.RandomElement<string>(agdenemyReplacementTier.ReplacementGuids);
																			Vector2? vector2 = vector;
																			roomHandler2.AddSpecificEnemyToRoomProcedurally(text, false, vector2);
																			num++;
																			agdInjectionRunCounts[i]++;
																			if ((agdenemyReplacementTier.MaxPerFloor > 0 && num >= agdenemyReplacementTier.MaxPerFloor) || (agdenemyReplacementTier.MaxPerRun > 0 && agdInjectionRunCounts[i] >= agdenemyReplacementTier.MaxPerRun) || agdenemyReplacementTier.RemoveAllOtherEnemies)
																			{
																				break;
																			}
																		}
																	}
																}
															}
														}
														if ((agdenemyReplacementTier.MaxPerFloor > 0 && num >= agdenemyReplacementTier.MaxPerFloor) || (agdenemyReplacementTier.MaxPerRun > 0 && agdInjectionRunCounts[i] >= agdenemyReplacementTier.MaxPerRun))
														{
															break;
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			this.CullEnemiesForNewPlayers();
		}

		// Token: 0x06004FD1 RID: 20433 RVA: 0x001BCBB4 File Offset: 0x001BADB4
		private void CullEnemiesForNewPlayers()
		{
			List<AIActor> list = new List<AIActor>();
			float newPlayerEnemyCullFactor = GameStatsManager.Instance.NewPlayerEnemyCullFactor;
			if (newPlayerEnemyCullFactor > 0f)
			{
				foreach (RoomHandler roomHandler in this.data.rooms)
				{
					if (roomHandler.EverHadEnemies)
					{
						if (roomHandler.IsStandardRoom)
						{
							if (!roomHandler.IsGunslingKingChallengeRoom)
							{
								if (roomHandler.area.runtimePrototypeData != null && roomHandler.area.runtimePrototypeData.roomEvents != null)
								{
									bool flag = false;
									for (int i = 0; i < roomHandler.area.runtimePrototypeData.roomEvents.Count; i++)
									{
										if (roomHandler.area.runtimePrototypeData.roomEvents[i].action == RoomEventTriggerAction.BECOME_TERRIFYING_AND_DARK)
										{
											flag = true;
										}
									}
									if (flag)
									{
										continue;
									}
								}
								roomHandler.GetActiveEnemies(RoomHandler.ActiveEnemyType.All, ref list);
								for (int j = 0; j < list.Count; j++)
								{
									AIActor aiactor = list[j];
									if (aiactor)
									{
										if (aiactor.AdditionalSimpleItemDrops == null || aiactor.AdditionalSimpleItemDrops.Count <= 0)
										{
											if (aiactor.IsNormalEnemy && !aiactor.IsHarmlessEnemy && aiactor.IsWorthShootingAt && (!aiactor.healthHaver || !aiactor.healthHaver.IsBoss) && UnityEngine.Random.value < newPlayerEnemyCullFactor)
											{
												UnityEngine.Object.Destroy(aiactor.gameObject);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004FD2 RID: 20434 RVA: 0x001BCDA8 File Offset: 0x001BAFA8
		private void PlaceFloorObjectInternal(DungeonPlaceableBehaviour prefabPlaceable, IntVector2 dimensions, Vector2 offset)
		{
			List<IntVector2> list = new List<IntVector2>();
			for (int i = 0; i < this.data.rooms.Count; i++)
			{
				RoomHandler roomHandler = this.data.rooms[i];
				if (!roomHandler.area.IsProceduralRoom && roomHandler.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.NORMAL && !roomHandler.OptionalDoorTopDecorable && !roomHandler.area.prototypeRoom.UseCustomMusic)
				{
					for (int j = roomHandler.area.basePosition.x; j < roomHandler.area.basePosition.x + roomHandler.area.dimensions.x; j++)
					{
						for (int k = roomHandler.area.basePosition.y; k < roomHandler.area.basePosition.y + roomHandler.area.dimensions.y; k++)
						{
							if (this.ClearForFloorObject(dimensions.x, dimensions.y, j, k))
							{
								list.Add(new IntVector2(j, k));
							}
						}
					}
				}
			}
			if (list.Count > 0)
			{
				IntVector2 intVector = list[BraveRandom.GenerationRandomRange(0, list.Count)];
				RoomHandler absoluteRoom = intVector.ToVector2().GetAbsoluteRoom();
				GameObject gameObject = prefabPlaceable.InstantiateObject(absoluteRoom, intVector - absoluteRoom.area.basePosition, false);
				gameObject.transform.position = gameObject.transform.position + offset.ToVector3ZUp(0f);
				IPlayerInteractable[] interfacesInChildren = gameObject.GetInterfacesInChildren<IPlayerInteractable>();
				foreach (IPlayerInteractable playerInteractable in interfacesInChildren)
				{
					absoluteRoom.RegisterInteractable(playerInteractable);
				}
				for (int m = 0; m < dimensions.x; m++)
				{
					for (int n = 0; n < dimensions.y; n++)
					{
						IntVector2 intVector2 = intVector + new IntVector2(m, n);
						if (this.data.CheckInBoundsAndValid(intVector2))
						{
							this.data[intVector2].cellVisualData.floorTileOverridden = true;
						}
					}
				}
			}
		}

		// Token: 0x06004FD3 RID: 20435 RVA: 0x001BD004 File Offset: 0x001BB204
		private void PlaceParadoxPortal()
		{
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.NONE)
			{
				DungeonPlaceableBehaviour component = ((GameObject)BraveResources.Load("Global Prefabs/VFX_ParadoxPortal", ".prefab")).GetComponent<DungeonPlaceableBehaviour>();
				this.PlaceFloorObjectInternal(component, new IntVector2(4, 4), new Vector2(2f, 2f));
			}
		}

		// Token: 0x06004FD4 RID: 20436 RVA: 0x001BD058 File Offset: 0x001BB258
		private void PlaceRatGrate()
		{
			DungeonPlaceableBehaviour component = this.RatTrapdoor.GetComponent<DungeonPlaceableBehaviour>();
			this.PlaceFloorObjectInternal(component, new IntVector2(4, 4), Vector2.zero);
		}

		// Token: 0x06004FD5 RID: 20437 RVA: 0x001BD084 File Offset: 0x001BB284
		private bool ClearForFloorObject(int dmx, int dmy, int bpx, int bpy)
		{
			int num = -1;
			for (int i = 0; i < dmx; i++)
			{
				for (int j = 0; j < dmy; j++)
				{
					IntVector2 intVector = new IntVector2(bpx + i, bpy + j);
					if (!this.data.CheckInBoundsAndValid(intVector))
					{
						return false;
					}
					CellData cellData = this.data[intVector];
					if (num == -1)
					{
						num = cellData.cellVisualData.roomVisualTypeIndex;
						if (num != 0 && num != 1)
						{
							return false;
						}
					}
					if (cellData.parentRoom == null || cellData.parentRoom.IsMaintenanceRoom() || cellData.type != CellType.FLOOR || cellData.isOccupied || !cellData.IsPassable || cellData.containsTrap || cellData.IsTrapZone)
					{
						return false;
					}
					if (cellData.cellVisualData.roomVisualTypeIndex != num || cellData.HasPitNeighbor(this.data) || cellData.PreventRewardSpawn || cellData.cellVisualData.isPattern || cellData.cellVisualData.IsPhantomCarpet)
					{
						return false;
					}
					if (cellData.cellVisualData.floorType == CellVisualData.CellFloorType.Water || cellData.cellVisualData.floorType == CellVisualData.CellFloorType.Carpet || cellData.cellVisualData.floorTileOverridden)
					{
						return false;
					}
					if (cellData.doesDamage || cellData.cellVisualData.preventFloorStamping || cellData.cellVisualData.hasStampedPath || cellData.forceDisallowGoop)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06004FD6 RID: 20438 RVA: 0x001BD230 File Offset: 0x001BB430
		public void PlaceWallMimics(RoomHandler debugRoom = null)
		{
			if (GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.NONE && GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.RESOURCEFUL_RAT)
			{
				return;
			}
			int numWallMimicsForFloor = MetaInjectionData.GetNumWallMimicsForFloor(this.tileIndices.tilesetId);
			if (numWallMimicsForFloor <= 0)
			{
				return;
			}
			List<int> list = Enumerable.Range(0, this.data.rooms.Count).ToList<int>();
			list = list.Shuffle<int>();
			if (debugRoom != null)
			{
				list = new List<int>(new int[] { this.data.rooms.IndexOf(debugRoom) });
			}
			List<Tuple<IntVector2, DungeonData.Direction>> list2 = new List<Tuple<IntVector2, DungeonData.Direction>>();
			int num = 0;
			List<AIActor> list3 = new List<AIActor>();
			int num2 = 0;
			while (num2 < list.Count && num < numWallMimicsForFloor)
			{
				RoomHandler roomHandler = this.data.rooms[list[num2]];
				if (!roomHandler.IsShop && !roomHandler.IsMaintenanceRoom())
				{
					if (!roomHandler.area.IsProceduralRoom || roomHandler.area.proceduralCells == null)
					{
						if (roomHandler.area.PrototypeRoomCategory != PrototypeDungeonRoom.RoomCategory.BOSS || (PlayerStats.GetTotalCurse() >= 5 && !BraveUtility.RandomBool()))
						{
							if (!roomHandler.GetRoomName().StartsWith("DraGunRoom"))
							{
								if (roomHandler.connectedRooms != null)
								{
									for (int i = 0; i < roomHandler.connectedRooms.Count; i++)
									{
										if (roomHandler.connectedRooms[i] == null || roomHandler.connectedRooms[i].area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS)
										{
										}
									}
								}
								if (debugRoom == null)
								{
									bool flag = false;
									roomHandler.GetActiveEnemies(RoomHandler.ActiveEnemyType.All, ref list3);
									for (int j = 0; j < list3.Count; j++)
									{
										AIActor aiactor = list3[j];
										if (aiactor && aiactor.EnemyGuid == GameManager.Instance.RewardManager.WallMimicChances.EnemyGuid)
										{
											flag = true;
											break;
										}
									}
									if (flag)
									{
										goto IL_858;
									}
								}
								list2.Clear();
								for (int k = -1; k <= roomHandler.area.dimensions.x; k++)
								{
									for (int l = -1; l <= roomHandler.area.dimensions.y; l++)
									{
										int num3 = roomHandler.area.basePosition.x + k;
										int num4 = roomHandler.area.basePosition.y + l;
										if (this.data.isWall(num3, num4))
										{
											int m = 0;
											if (this.data.isWall(num3 - 1, num4) && this.data.isWall(num3 - 1, num4 - 1) && this.data.isWall(num3 - 1, num4 - 2) && this.data.isWall(num3, num4) && this.data.isWall(num3, num4 - 1) && this.data.isWall(num3, num4 - 2) && this.data.isPlainEmptyCell(num3, num4 + 1) && this.data.isWall(num3 + 1, num4) && this.data.isWall(num3 + 1, num4 - 1) && this.data.isWall(num3 + 1, num4 - 2) && this.data.isPlainEmptyCell(num3 + 1, num4 + 1) && this.data.isWall(num3 + 2, num4) && this.data.isWall(num3 + 2, num4 - 1) && this.data.isWall(num3 + 2, num4 - 2))
											{
												list2.Add(Tuple.Create<IntVector2, DungeonData.Direction>(new IntVector2(num3, num4), DungeonData.Direction.NORTH));
												m++;
											}
											else if (this.data.isWall(num3 - 1, num4) && this.data.isWall(num3 - 1, num4 + 1) && this.data.isWall(num3 - 1, num4 + 2) && this.data.isWall(num3, num4) && this.data.isWall(num3, num4 + 1) && this.data.isWall(num3, num4 + 2) && this.data.isPlainEmptyCell(num3, num4 - 1) && this.data.isWall(num3 + 1, num4) && this.data.isWall(num3 + 1, num4 + 1) && this.data.isWall(num3 + 1, num4 + 2) && this.data.isPlainEmptyCell(num3 + 1, num4 - 1) && this.data.isWall(num3 + 2, num4) && this.data.isWall(num3 + 2, num4 + 1) && this.data.isWall(num3 + 2, num4 + 2))
											{
												list2.Add(Tuple.Create<IntVector2, DungeonData.Direction>(new IntVector2(num3, num4), DungeonData.Direction.SOUTH));
												m++;
											}
											else if (this.data.isWall(num3, num4 + 2) && this.data.isWall(num3, num4 + 1) && this.data.isWall(num3, num4 - 1) && this.data.isWall(num3, num4 - 2) && this.data.isWall(num3 - 1, num4) && this.data.isPlainEmptyCell(num3 + 1, num4) && this.data.isPlainEmptyCell(num3 + 1, num4 - 1))
											{
												list2.Add(Tuple.Create<IntVector2, DungeonData.Direction>(new IntVector2(num3, num4), DungeonData.Direction.EAST));
												m++;
											}
											else if (this.data.isWall(num3, num4 + 2) && this.data.isWall(num3, num4 + 1) && this.data.isWall(num3, num4 - 1) && this.data.isWall(num3, num4 - 2) && this.data.isWall(num3 + 1, num4) && this.data.isPlainEmptyCell(num3 - 1, num4) && this.data.isPlainEmptyCell(num3 - 1, num4 - 1))
											{
												list2.Add(Tuple.Create<IntVector2, DungeonData.Direction>(new IntVector2(num3 - 1, num4), DungeonData.Direction.WEST));
												m++;
											}
											if (m > 0)
											{
												bool flag2 = true;
												int num5 = -5;
												while (num5 <= 5 && flag2)
												{
													int num6 = -5;
													while (num6 <= 5 && flag2)
													{
														int num7 = num3 + num5;
														int num8 = num4 + num6;
														if (this.data.CheckInBoundsAndValid(num7, num8))
														{
															CellData cellData = this.data[num7, num8];
															if (cellData != null)
															{
																if (cellData.type == CellType.PIT || cellData.diagonalWallType != DiagonalWallType.NONE)
																{
																	flag2 = false;
																}
															}
														}
														num6++;
													}
													num5++;
												}
												if (!flag2)
												{
													while (m > 0)
													{
														list2.RemoveAt(list2.Count - 1);
														m--;
													}
												}
											}
										}
									}
								}
								if (debugRoom == null)
								{
									if (list2.Count > 0)
									{
										Tuple<IntVector2, DungeonData.Direction> tuple = BraveUtility.RandomElement<Tuple<IntVector2, DungeonData.Direction>>(list2);
										IntVector2 first = tuple.First;
										DungeonData.Direction second = tuple.Second;
										if (second != DungeonData.Direction.WEST)
										{
											roomHandler.RuntimeStampCellComplex(first.x, first.y, CellType.FLOOR, DiagonalWallType.NONE);
										}
										if (second != DungeonData.Direction.EAST)
										{
											roomHandler.RuntimeStampCellComplex(first.x + 1, first.y, CellType.FLOOR, DiagonalWallType.NONE);
										}
										AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(GameManager.Instance.RewardManager.WallMimicChances.EnemyGuid);
										AIActor.Spawn(orLoadByGuid, first, roomHandler, true, AIActor.AwakenAnimationType.Default, true);
										num++;
									}
								}
							}
						}
					}
				}
				IL_858:
				num2++;
			}
			if (num > 0)
			{
				PhysicsEngine.Instance.ClearAllCachedTiles();
			}
		}

		// Token: 0x06004FD7 RID: 20439 RVA: 0x001BDAC0 File Offset: 0x001BBCC0
		public void FloorCleared()
		{
			if (GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.NONE)
			{
				return;
			}
			GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
			switch (tilesetId)
			{
			case GlobalDungeonData.ValidTilesets.GUNGEON:
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_CLEARED_GUNGEON, 1f);
				break;
			case GlobalDungeonData.ValidTilesets.CASTLEGEON:
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_CLEARED_CASTLE, 1f);
				break;
			default:
				if (tilesetId != GlobalDungeonData.ValidTilesets.MINEGEON)
				{
					if (tilesetId != GlobalDungeonData.ValidTilesets.CATACOMBGEON)
					{
						if (tilesetId != GlobalDungeonData.ValidTilesets.FORGEGEON)
						{
							if (tilesetId != GlobalDungeonData.ValidTilesets.HELLGEON)
							{
								if (tilesetId != GlobalDungeonData.ValidTilesets.SPACEGEON)
								{
									if (tilesetId != GlobalDungeonData.ValidTilesets.PHOBOSGEON)
									{
										if (tilesetId != GlobalDungeonData.ValidTilesets.WESTGEON)
										{
											if (tilesetId != GlobalDungeonData.ValidTilesets.OFFICEGEON)
											{
												if (tilesetId != GlobalDungeonData.ValidTilesets.BELLYGEON)
												{
													if (tilesetId == GlobalDungeonData.ValidTilesets.JUNGLEGEON)
													{
														GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_CLEARED_JUNGLE, 1f);
													}
												}
												else
												{
													GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_CLEARED_BELLY, 1f);
												}
											}
											else
											{
												GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_CLEARED_NAKATOMI, 1f);
											}
										}
										else
										{
											GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_CLEARED_WEST, 1f);
										}
									}
									else
									{
										GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_CLEARED_PHOBOS, 1f);
									}
								}
								else
								{
									GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_CLEARED_FUTURE, 1f);
								}
							}
							else
							{
								GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_CLEARED_BULLET_HELL, 1f);
							}
						}
						else
						{
							GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_CLEARED_FORGE, 1f);
						}
					}
					else
					{
						GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_CLEARED_CATACOMBS, 1f);
					}
				}
				else
				{
					GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_CLEARED_MINES, 1f);
				}
				break;
			case GlobalDungeonData.ValidTilesets.SEWERGEON:
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_CLEARED_SEWERS, 1f);
				break;
			case GlobalDungeonData.ValidTilesets.CATHEDRALGEON:
				GameStatsManager.Instance.RegisterStatChange(TrackedStats.TIMES_CLEARED_CATHEDRAL, 1f);
				break;
			}
			if (ChallengeManager.CHALLENGE_MODE_ACTIVE && this.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
			{
				GameStatsManager.Instance.SetFlag(GungeonFlags.DAISUKE_CHALLENGE_COMPLETE, true);
				GameStatsManager.Instance.SetFlag(GungeonFlags.DAISUKE_CHALLENGE_ITEM_UNLOCK, true);
				if (ChallengeManager.Instance.ChallengeMode == ChallengeModeType.ChallengeMegaMode)
				{
					GameStatsManager.Instance.SetFlag(GungeonFlags.DAISUKE_MEGA_CHALLENGE_COMPLETE, true);
				}
			}
			if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHOP_HAS_MET_BEETLE))
			{
				GlobalDungeonData.ValidTilesets tilesetId2 = GameManager.Instance.Dungeon.tileIndices.tilesetId;
				if (tilesetId2 != GlobalDungeonData.ValidTilesets.GUNGEON)
				{
					if (tilesetId2 != GlobalDungeonData.ValidTilesets.CASTLEGEON)
					{
						if (tilesetId2 != GlobalDungeonData.ValidTilesets.MINEGEON)
						{
							if (tilesetId2 != GlobalDungeonData.ValidTilesets.CATACOMBGEON)
							{
								if (tilesetId2 == GlobalDungeonData.ValidTilesets.FORGEGEON)
								{
									GameStatsManager.Instance.AccumulatedBeetleMerchantChance = 1f;
								}
							}
							else
							{
								GameStatsManager.Instance.AccumulatedBeetleMerchantChance = 0.8f;
							}
						}
						else
						{
							GameStatsManager.Instance.AccumulatedBeetleMerchantChance = 0.6f;
						}
					}
					else
					{
						GameStatsManager.Instance.AccumulatedBeetleMerchantChance = 0.2f;
					}
				}
				else
				{
					GameStatsManager.Instance.AccumulatedBeetleMerchantChance = 0.4f;
				}
			}
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
			{
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					PlayerController playerController = GameManager.Instance.AllPlayers[i];
					if (playerController)
					{
						if (!playerController.HasFiredNonStartingGun)
						{
							GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_STARTING_GUN, true);
						}
						if (playerController.CharacterUsesRandomGuns)
						{
							GameManager.Instance.platformInterface.AchievementUnlock(Achievement.COMPLETE_GAME_WITH_ENCHANTED_GUN, 0);
						}
						if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
						{
							GameManager.Instance.platformInterface.AchievementUnlock(Achievement.COMPLETE_GAME_WITH_CHALLENGE_MODE, 0);
						}
					}
				}
			}
		}

		// Token: 0x06004FD8 RID: 20440 RVA: 0x001BDE70 File Offset: 0x001BC070
		private void GeneratePlayerIfNecessary(MidGameSaveData midgameSave)
		{
			bool flag = false;
			bool flag2 = false;
			if (this.ForceRegenerationOfCharacters)
			{
				if (GameManager.Instance.PrimaryPlayer)
				{
					flag = GameManager.Instance.PrimaryPlayer.IsUsingAlternateCostume;
				}
				if (GameManager.Instance.PrimaryPlayer)
				{
					flag2 = GameManager.Instance.PrimaryPlayer.IsTemporaryEeveeForUnlock;
				}
				GameManager.Instance.ClearPlayers();
			}
			PlayerController playerController = GameManager.Instance.PrimaryPlayer;
			if (!playerController || this.ForceRegenerationOfCharacters)
			{
				if (midgameSave != null)
				{
					GameManager.PlayerPrefabForNewGame = midgameSave.GetPlayerOnePrefab();
				}
				if (GameManager.PlayerPrefabForNewGame == null)
				{
					if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
					{
						return;
					}
					BraveUtility.Log("Dungeon generation complete with no Player! Creating placeholder...", Color.yellow, BraveUtility.LogVerbosity.IMPORTANT);
					GameObject gameObject = this.defaultPlayerPrefab;
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, Vector3.zero, Quaternion.identity);
					gameObject2.SetActive(true);
					playerController = gameObject2.GetComponent<PlayerController>();
					if (playerController is PlayerSpaceshipController)
					{
						playerController.IsUsingAlternateCostume = flag;
						playerController.SetTemporaryEeveeSafeNoShader(flag2);
					}
				}
				else
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(GameManager.PlayerPrefabForNewGame, Vector3.zero, Quaternion.identity);
					GameManager.PlayerPrefabForNewGame = null;
					gameObject3.SetActive(true);
					playerController = gameObject3.GetComponent<PlayerController>();
				}
				if (GameManager.ForceQuickRestartAlternateCostumeP1)
				{
					playerController.SwapToAlternateCostume(null);
					GameManager.ForceQuickRestartAlternateCostumeP1 = false;
				}
				if (GameManager.ForceQuickRestartAlternateGunP1)
				{
					playerController.UsingAlternateStartingGuns = true;
					GameManager.ForceQuickRestartAlternateGunP1 = false;
				}
				GameManager.Instance.RefreshAllPlayers();
				if (this.StripPlayerOnArrival)
				{
					playerController.startingGunIds = new List<int>();
					playerController.startingAlternateGunIds = new List<int>();
					playerController.startingActiveItemIds.Clear();
					playerController.startingPassiveItemIds.Clear();
				}
			}
			else if (this.StripPlayerOnArrival)
			{
				playerController.inventory.DestroyAllGuns();
				playerController.RemoveAllActiveItems();
				playerController.RemoveAllPassiveItems();
			}
			playerController.PlayerIDX = 0;
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				if (GameManager.Instance.AllPlayers.Length < 2 || this.ForceRegenerationOfCharacters)
				{
					GameObject gameObject4 = ((!(GameManager.CoopPlayerPrefabForNewGame == null)) ? GameManager.CoopPlayerPrefabForNewGame : (ResourceCache.Acquire("PlayerCoopCultist") as GameObject));
					if (this.ForceRegenerationOfCharacters)
					{
						gameObject4 = ResourceCache.Acquire("PlayerCoopCultist") as GameObject;
					}
					if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST && GameManager.Instance.PrimaryPlayer.characterIdentity == PlayableCharacters.Pilot && !GameManager.IsCoopPast)
					{
						gameObject4 = BraveResources.Load("PlayerCoopShip", ".prefab") as GameObject;
					}
					GameObject gameObject5 = UnityEngine.Object.Instantiate<GameObject>(gameObject4, Vector3.zero, Quaternion.identity);
					GameManager.CoopPlayerPrefabForNewGame = null;
					gameObject5.SetActive(true);
					PlayerController component = gameObject5.GetComponent<PlayerController>();
					component.ActorName = "Player ID 1";
					component.PlayerIDX = 1;
					if (GameManager.ForceQuickRestartAlternateCostumeP2)
					{
						component.SwapToAlternateCostume(null);
						GameManager.ForceQuickRestartAlternateCostumeP2 = false;
					}
					if (GameManager.ForceQuickRestartAlternateGunP2)
					{
						component.UsingAlternateStartingGuns = true;
						GameManager.ForceQuickRestartAlternateGunP2 = false;
					}
					if (this.StripPlayerOnArrival)
					{
						component.startingGunIds = new List<int>();
						component.startingAlternateGunIds = new List<int>();
						component.startingActiveItemIds.Clear();
						component.startingPassiveItemIds.Clear();
					}
					GameManager.Instance.RefreshAllPlayers();
				}
				else if (this.StripPlayerOnArrival)
				{
					GameManager.Instance.SecondaryPlayer.inventory.DestroyAllGuns();
					GameManager.Instance.SecondaryPlayer.RemoveAllActiveItems();
					GameManager.Instance.SecondaryPlayer.RemoveAllPassiveItems();
				}
			}
			if (GameManager.Instance.InTutorial || (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST && GameManager.Instance.PrimaryPlayer.characterIdentity == PlayableCharacters.Convict))
			{
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					if (GameManager.Instance.AllPlayers[i].healthHaver.IsAlive)
					{
						GameManager.Instance.AllPlayers[i].sprite.gameObject.SetLayerRecursively(LayerMask.NameToLayer("ShadowCaster"));
					}
				}
			}
		}

		// Token: 0x06004FD9 RID: 20441 RVA: 0x001BE298 File Offset: 0x001BC498
		public void DarkSoulsReset(PlayerController targetPlayer, bool dropItems = true, int cursedHealthMaximum = -1)
		{
			base.StartCoroutine(this.HandleDarkSoulsReset_CR(targetPlayer, dropItems, cursedHealthMaximum));
		}

		// Token: 0x06004FDA RID: 20442 RVA: 0x001BE2AC File Offset: 0x001BC4AC
		private IEnumerator HandleDarkSoulsReset_CR(PlayerController p, bool dropItems, int cursedHealthMaximum)
		{
			GameManager.Instance.PauseRaw(true);
			float elapsed = 0f;
			float transitionTime = 0.5f;
			Pixelator.Instance.FadeToBlack(transitionTime, false, 0f);
			while (elapsed < transitionTime)
			{
				elapsed += GameManager.INVARIANT_DELTA_TIME;
				yield return null;
			}
			if (dropItems)
			{
				p.DropPileOfSouls();
				p.HandleDarkSoulsHollowTransition(true);
			}
			RoomHandler targetRoom = this.data.Entrance;
			if (ExtraLifeItem.LastActivatedBonfire != null)
			{
				targetRoom = ExtraLifeItem.LastActivatedBonfire.transform.position.GetAbsoluteRoom();
			}
			IntVector2 availableCell = targetRoom.GetCenteredVisibleClearSpot(3, 3);
			Vector3 playerPosition = new Vector3((float)availableCell.x + 0.5f, (float)availableCell.y + 0.5f, -0.1f);
			p.transform.position = playerPosition;
			p.Reinitialize();
			p.ForceChangeRoom(targetRoom);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				GameManager.Instance.GetOtherPlayer(p).ReuniteWithOtherPlayer(p, false);
				GameManager.Instance.GetOtherPlayer(p).ForceChangeRoom(targetRoom);
			}
			if (cursedHealthMaximum > 0)
			{
				p.healthHaver.CursedMaximum = (float)cursedHealthMaximum;
				p.IsDarkSoulsHollow = true;
			}
			if (p.characterIdentity == PlayableCharacters.Robot)
			{
				p.healthHaver.Armor = 2f;
			}
			for (int i = 0; i < this.data.rooms.Count; i++)
			{
				this.data.rooms[i].ResetPredefinedRoomLikeDarkSouls();
			}
			GameUIRoot.Instance.bossController.DisableBossHealth();
			GameUIRoot.Instance.bossController2.DisableBossHealth();
			GameUIRoot.Instance.bossControllerSide.DisableBossHealth();
			GameManager.Instance.MainCameraController.ForceToPlayerPosition(p);
			GameManager.Instance.ForceUnpause();
			GameManager.Instance.PreventPausing = false;
			p.CurrentInputState = PlayerInputState.AllInput;
			p.DoInitialFallSpawn(0f);
			Pixelator.Instance.FadeToBlack(transitionTime, true, 0f);
			yield break;
		}

		// Token: 0x06004FDB RID: 20443 RVA: 0x001BE2DC File Offset: 0x001BC4DC
		private void PlacePlayerInRoom(tk2dTileMap map, RoomHandler startRoom)
		{
			PlayerController[] allPlayers = GameManager.Instance.AllPlayers;
			if (allPlayers.Length == 0)
			{
				return;
			}
			int num = ((allPlayers.Length >= 2) ? allPlayers.Length : 1);
			for (int i = 0; i < num; i++)
			{
				PlayerController playerController = ((allPlayers.Length >= 2) ? allPlayers[i] : GameManager.Instance.PrimaryPlayer);
				EntranceController entranceController = UnityEngine.Object.FindObjectOfType<EntranceController>();
				ElevatorArrivalController elevatorArrivalController = UnityEngine.Object.FindObjectOfType<ElevatorArrivalController>();
				Vector2 vector = Vector2.zero;
				float num2 = 0.25f;
				if (GameManager.IsReturningToFoyerWithPlayer)
				{
					vector = GameObject.Find("ReturnToFoyerPoint").transform.position.XY();
					vector += Vector2.right * (float)i;
					playerController.transform.position = vector.ToVector3ZUp(-0.1f);
					playerController.Reinitialize();
				}
				else
				{
					if (elevatorArrivalController != null)
					{
						vector = elevatorArrivalController.spawnTransform.position.XY();
						num2 = 1f;
						elevatorArrivalController.DoArrival(playerController, num2);
						num2 += 0.4f;
					}
					else
					{
						if (entranceController != null)
						{
							vector = entranceController.spawnTransform.position.XY();
							vector += Vector2.right * (float)i;
							playerController.transform.position = new Vector3(map.transform.position.x + vector.x - 0.5f, map.transform.position.y + vector.y, -0.1f);
							playerController.Reinitialize();
							num2 += 0.4f;
							playerController.DoSpinfallSpawn(num2);
							goto IL_356;
						}
						if (i == 1 && GameObject.Find("SecondaryPlayerSpawnPoint") != null)
						{
							vector = GameObject.Find("SecondaryPlayerSpawnPoint").transform.position.XY();
							vector += Vector2.right * (float)i;
							playerController.transform.position = vector.ToVector3ZUp(-0.1f);
							playerController.Reinitialize();
							goto IL_356;
						}
						if (GameObject.Find("PlayerSpawnPoint") != null)
						{
							vector = GameObject.Find("PlayerSpawnPoint").transform.position.XY();
							vector += Vector2.right * (float)i;
							playerController.transform.position = vector.ToVector3ZUp(-0.1f);
							playerController.Reinitialize();
							goto IL_356;
						}
						vector = startRoom.GetCenterCell().ToVector2();
						if (this.data[vector.ToIntVector2(VectorConversions.Round)].type == CellType.WALL || this.data[vector.ToIntVector2(VectorConversions.Round)].type == CellType.PIT)
						{
							vector = startRoom.Epicenter.ToVector2();
						}
						vector += Vector2.right * (float)i;
					}
					Vector3 vector2 = new Vector3(map.transform.position.x + vector.x + 0.5f, map.transform.position.y + vector.y + 0.5f, -0.1f);
					playerController.transform.position = vector2;
					playerController.Reinitialize();
					playerController.DoInitialFallSpawn(num2);
				}
				IL_356:;
			}
			GameManager.IsReturningToFoyerWithPlayer = false;
			GameManager.Instance.MainCameraController.ForceToPlayerPosition(GameManager.Instance.PrimaryPlayer);
		}

		// Token: 0x06004FDC RID: 20444 RVA: 0x001BE66C File Offset: 0x001BC86C
		public bool CellExists(IntVector2 pos)
		{
			return pos.x >= 0 && pos.x < this.Width && pos.y >= 0 && pos.y < this.Height;
		}

		// Token: 0x06004FDD RID: 20445 RVA: 0x001BE6AC File Offset: 0x001BC8AC
		public bool CellExists(int x, int y)
		{
			return x >= 0 && x < this.Width && y >= 0 && y < this.Height;
		}

		// Token: 0x06004FDE RID: 20446 RVA: 0x001BE6D4 File Offset: 0x001BC8D4
		public bool CellExists(Vector2 pos)
		{
			int num = (int)pos.x;
			int num2 = (int)pos.y;
			return num >= 0 && num < this.Width && num2 >= 0 && num2 < this.Height;
		}

		// Token: 0x06004FDF RID: 20447 RVA: 0x001BE71C File Offset: 0x001BC91C
		public bool CellIsNearPit(Vector3 position)
		{
			IntVector2 intVector = position.IntXY(VectorConversions.Floor);
			CellData cellData = this.data[intVector];
			return cellData != null && (cellData.type == CellType.PIT || cellData.HasPitNeighbor(this.data));
		}

		// Token: 0x06004FE0 RID: 20448 RVA: 0x001BE764 File Offset: 0x001BC964
		public bool CellIsPit(Vector3 position)
		{
			IntVector2 intVector = position.IntXY(VectorConversions.Floor);
			CellData cellData = this.data[intVector];
			return cellData.type == CellType.PIT;
		}

		// Token: 0x06004FE1 RID: 20449 RVA: 0x001BE790 File Offset: 0x001BC990
		public bool CellSupportsFalling(Vector3 position)
		{
			IntVector2 intVector = position.IntXY(VectorConversions.Floor);
			if (!this.data.CheckInBounds(intVector))
			{
				return false;
			}
			CellData cellData = this.data[intVector];
			return cellData != null && cellData.type == CellType.PIT && !cellData.fallingPrevented;
		}

		// Token: 0x06004FE2 RID: 20450 RVA: 0x001BE7E8 File Offset: 0x001BC9E8
		public List<SpeculativeRigidbody> GetPlatformsAt(Vector3 position)
		{
			IntVector2 intVector = position.IntXY(VectorConversions.Floor);
			CellData cellData = this.data[intVector];
			return cellData.platforms;
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x001BE810 File Offset: 0x001BCA10
		public bool IsPixelOnPlatform(Vector3 position, out SpeculativeRigidbody platform)
		{
			return this.IsPixelOnPlatform(PhysicsEngine.UnitToPixel(position.XY()), out platform);
		}

		// Token: 0x06004FE4 RID: 20452 RVA: 0x001BE824 File Offset: 0x001BCA24
		public bool IsPixelOnPlatform(IntVector2 pixel, out SpeculativeRigidbody platform)
		{
			platform = null;
			IntVector2 intVector = PhysicsEngine.PixelToUnitMidpoint(pixel).ToIntVector2(VectorConversions.Floor);
			CellData cellData = this.data[intVector];
			if (cellData.platforms != null)
			{
				for (int i = 0; i < cellData.platforms.Count; i++)
				{
					if (cellData.platforms[i].PrimaryPixelCollider.ContainsPixel(pixel))
					{
						platform = cellData.platforms[i];
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004FE5 RID: 20453 RVA: 0x001BE8A4 File Offset: 0x001BCAA4
		public bool PositionInCustomPitSRB(Vector3 position)
		{
			if (DebrisObject.SRB_Pits != null && DebrisObject.SRB_Pits.Count > 0)
			{
				for (int i = 0; i < DebrisObject.SRB_Pits.Count; i++)
				{
					if (DebrisObject.SRB_Pits[i].ContainsPoint(position.XY(), 2147483647, true))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004FE6 RID: 20454 RVA: 0x001BE90C File Offset: 0x001BCB0C
		public bool ShouldReallyFall(Vector3 position)
		{
			bool flag = !this.CellSupportsFalling(position);
			if (this.PositionInCustomPitSRB(position))
			{
				flag = false;
			}
			SpeculativeRigidbody speculativeRigidbody;
			return !flag && !this.IsPixelOnPlatform(position, out speculativeRigidbody);
		}

		// Token: 0x06004FE7 RID: 20455 RVA: 0x001BE948 File Offset: 0x001BCB48
		public void DoSplashDustupAtPosition(Vector2 bottomCenter)
		{
			DustUpVFX dustUpVFX = this.dungeonDustups;
			Color clear = Color.clear;
			GameObject gameObject = SpawnManager.SpawnVFX(dustUpVFX.waterDustup, bottomCenter, Quaternion.identity);
			if (gameObject)
			{
				Renderer component = gameObject.GetComponent<Renderer>();
				if (component)
				{
					gameObject.GetComponent<tk2dBaseSprite>().OverrideMaterialMode = tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_SIMPLE;
					component.material.SetColor("_OverrideColor", clear);
				}
			}
			if (dustUpVFX.additionalWaterDustup != null)
			{
				SpawnManager.SpawnVFX(dustUpVFX.additionalWaterDustup, bottomCenter, Quaternion.identity, true);
			}
		}

		// Token: 0x06004FE8 RID: 20456 RVA: 0x001BE9DC File Offset: 0x001BCBDC
		public IntVector2 RandomCellInRandomRoom()
		{
			RoomHandler roomHandler = this.data.rooms[UnityEngine.Random.Range(0, this.data.rooms.Count)];
			return roomHandler.GetRandomAvailableCellDumb();
		}

		// Token: 0x06004FE9 RID: 20457 RVA: 0x001BEA18 File Offset: 0x001BCC18
		public RoomHandler GetRoomFromPosition(IntVector2 pos)
		{
			return this.data.GetAbsoluteRoomFromPosition(pos);
		}

		// Token: 0x06004FEA RID: 20458 RVA: 0x001BEA28 File Offset: 0x001BCC28
		public CellVisualData.CellFloorType GetFloorTypeFromPosition(Vector2 pos)
		{
			for (int i = 0; i < StaticReferenceManager.AllGoops.Count; i++)
			{
				if (StaticReferenceManager.AllGoops[i].IsPositionInGoop(pos))
				{
					return (!StaticReferenceManager.AllGoops[i].IsPositionFrozen(pos)) ? CellVisualData.CellFloorType.Water : CellVisualData.CellFloorType.Ice;
				}
			}
			return this.data.GetFloorTypeFromPosition(pos.ToIntVector2(VectorConversions.Floor));
		}

		// Token: 0x06004FEB RID: 20459 RVA: 0x001BEA98 File Offset: 0x001BCC98
		public IntVector2 RandomCellInArea(CellArea ca)
		{
			int num = UnityEngine.Random.Range(0, ca.dimensions.x);
			int num2 = UnityEngine.Random.Range(0, ca.dimensions.y);
			return new IntVector2(ca.basePosition.x + num, ca.basePosition.y + num2);
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x06004FEC RID: 20460 RVA: 0x001BEAE8 File Offset: 0x001BCCE8
		public bool AllRoomsVisited
		{
			get
			{
				return this.m_allRoomsVisited;
			}
		}

		// Token: 0x06004FED RID: 20461 RVA: 0x001BEAF0 File Offset: 0x001BCCF0
		public void NotifyAllRoomsVisited()
		{
			if (this.m_allRoomsVisited)
			{
				return;
			}
			this.m_allRoomsVisited = true;
			if (this.OnAllRoomsVisited != null)
			{
				this.OnAllRoomsVisited();
			}
		}

		// Token: 0x06004FEE RID: 20462 RVA: 0x001BEB1C File Offset: 0x001BCD1C
		public TertiaryBossRewardSet GetTertiaryRewardSet()
		{
			List<TertiaryBossRewardSet> list;
			if (this.UsesOverrideTertiaryBossSets && this.OverrideTertiaryRewardSets.Count > 0)
			{
				list = this.OverrideTertiaryRewardSets;
			}
			else
			{
				list = GameManager.Instance.RewardManager.CurrentRewardData.TertiaryBossRewardSets;
			}
			float num = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				num += list[i].weight;
			}
			float num2 = UnityEngine.Random.value * num;
			float num3 = 0f;
			for (int j = 0; j < list.Count; j++)
			{
				num3 += list[j].weight;
				if (num3 >= num2)
				{
					return list[j];
				}
			}
			return list[list.Count - 1];
		}

		// Token: 0x06004FEF RID: 20463 RVA: 0x001BEBF0 File Offset: 0x001BCDF0
		private void Update()
		{
			if (!this.m_ambientVFXProcessingActive)
			{
				base.StartCoroutine(this.HandleAmbientPitVFX());
				if (this.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON)
				{
					base.StartCoroutine(this.HandleAmbientChannelVFX());
				}
			}
			if (!this.m_musicIsPlaying && Time.timeScale > 0f)
			{
				if (Foyer.DoMainMenu && SceneManager.GetSceneByName("tt_foyer").isLoaded)
				{
					this.m_musicIsPlaying = true;
				}
				else if (GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.FOYER)
				{
					this.m_musicIsPlaying = true;
					GameManager.Instance.DungeonMusicController.ResetForNewFloor(this);
				}
			}
			if (this.ExplosionBulletDeletionMultiplier < 1f)
			{
				if (this.ExplosionBulletDeletionMultiplier <= 0f)
				{
					this.IsExplosionBulletDeletionRecovering = true;
				}
				this.ExplosionBulletDeletionMultiplier = Mathf.Clamp01(this.ExplosionBulletDeletionMultiplier + BraveTime.DeltaTime / 3f);
			}
			else
			{
				this.IsExplosionBulletDeletionRecovering = false;
			}
		}

		// Token: 0x06004FF0 RID: 20464 RVA: 0x001BECF4 File Offset: 0x001BCEF4
		public float GetNewPlayerSpeedMultiplier()
		{
			if (GameManager.Instance.Dungeon && GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.CASTLEGEON)
			{
				return 1f;
			}
			if (this.m_newPlayerMultiplier > 0f)
			{
				return this.m_newPlayerMultiplier;
			}
			float playerStatValue = GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.NUMBER_ATTEMPTS);
			float num = Mathf.Clamp01(0.2f - 0.025f * (playerStatValue - 1f));
			this.m_newPlayerMultiplier = 1f - num;
			return this.m_newPlayerMultiplier;
		}

		// Token: 0x06004FF1 RID: 20465 RVA: 0x001BED84 File Offset: 0x001BCF84
		public RoomHandler RuntimeDuplicateChunk(IntVector2 basePosition, IntVector2 dimensions, int tilemapExpansion, RoomHandler sourceRoom = null, bool ignoreOtherRoomCells = false)
		{
			int num = tilemapExpansion + 3;
			IntVector2 intVector = new IntVector2(this.data.Width + num, num);
			int num2 = this.data.Width + num * 2 + dimensions.x;
			int num3 = Mathf.Max(this.data.Height, dimensions.y + num * 2);
			CellData[][] array = BraveUtility.MultidimensionalArrayResize<CellData>(this.data.cellData, this.data.Width, this.data.Height, num2, num3);
			CellArea cellArea = new CellArea(intVector, dimensions, 0);
			cellArea.IsProceduralRoom = true;
			this.data.cellData = array;
			this.data.ClearCachedCellData();
			RoomHandler roomHandler = new RoomHandler(cellArea);
			GameObject gameObject = GameObject.Find("_Rooms");
			Transform transform = new GameObject("Room_ChunkDuplicate").transform;
			transform.parent = gameObject.transform;
			roomHandler.hierarchyParent = transform;
			for (int i = -num; i < dimensions.x + num; i++)
			{
				for (int j = -num; j < dimensions.y + num; j++)
				{
					IntVector2 intVector2 = basePosition + new IntVector2(i, j);
					IntVector2 intVector3 = new IntVector2(i, j) + intVector;
					CellData cellData = ((!this.data.CheckInBoundsAndValid(intVector2)) ? null : this.data[intVector2]);
					CellData cellData2 = new CellData(intVector3, CellType.WALL);
					if (cellData != null && sourceRoom != null && cellData.nearestRoom != sourceRoom)
					{
						cellData2.cellVisualData.roomVisualTypeIndex = sourceRoom.RoomVisualSubtype;
						cellData = null;
					}
					if (cellData != null && cellData.isExitCell && ignoreOtherRoomCells)
					{
						cellData2.cellVisualData.roomVisualTypeIndex = sourceRoom.RoomVisualSubtype;
						cellData = null;
					}
					cellData2.positionInTilemap = cellData2.positionInTilemap - intVector + new IntVector2(tilemapExpansion, tilemapExpansion);
					cellData2.parentArea = cellArea;
					cellData2.parentRoom = roomHandler;
					cellData2.nearestRoom = roomHandler;
					cellData2.occlusionData.overrideOcclusion = true;
					array[intVector3.x][intVector3.y] = cellData2;
					BraveUtility.DrawDebugSquare(intVector3.ToVector2(), Color.yellow, 1000f);
					CellType cellType = ((cellData == null) ? CellType.WALL : cellData.type);
					roomHandler.RuntimeStampCellComplex(intVector3.x, intVector3.y, cellType, DiagonalWallType.NONE);
					if (cellData != null)
					{
						cellData2.distanceFromNearestRoom = cellData.distanceFromNearestRoom;
						cellData2.cellVisualData.CopyFrom(cellData.cellVisualData);
						if (cellData.cellVisualData.containsLight)
						{
							this.data.ReplicateLighting(cellData, cellData2);
						}
					}
				}
			}
			this.data.rooms.Add(roomHandler);
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("RuntimeTileMap", ".prefab"));
			tk2dTileMap component = gameObject2.GetComponent<tk2dTileMap>();
			component.Editor__SpriteCollection = this.tileIndices.dungeonCollection;
			TK2DDungeonAssembler.RuntimeResizeTileMap(component, dimensions.x + tilemapExpansion * 2, dimensions.y + tilemapExpansion * 2, this.m_tilemap.partitionSizeX, dimensions.y + tilemapExpansion * 2);
			for (int k = -tilemapExpansion; k < dimensions.x + tilemapExpansion; k++)
			{
				for (int l = -tilemapExpansion; l < dimensions.y + tilemapExpansion; l++)
				{
					IntVector2 intVector4 = basePosition + new IntVector2(k, l);
					IntVector2 intVector5 = new IntVector2(k, l) + intVector;
					bool flag = false;
					CellData cellData3 = ((!this.data.CheckInBoundsAndValid(intVector4)) ? null : this.data[intVector4]);
					if (ignoreOtherRoomCells && cellData3 != null)
					{
						bool flag2 = cellData3.isExitCell;
						if (!flag2 && sourceRoom != null && cellData3.parentRoom != sourceRoom)
						{
							flag2 = true;
						}
						if (!flag2 && cellData3.IsAnyFaceWall() && this.data.CheckInBoundsAndValid(cellData3.position + new IntVector2(0, -2)) && this.data[cellData3.position + new IntVector2(0, -2)].isExitCell)
						{
							flag2 = true;
						}
						if (!flag2 && cellData3.type == CellType.WALL && this.data.CheckInBoundsAndValid(cellData3.position + new IntVector2(0, -3)) && this.data[cellData3.position + new IntVector2(0, -3)].isExitCell)
						{
							flag2 = true;
						}
						if (!flag2 && cellData3.type == CellType.FLOOR && this.data.CheckInBoundsAndValid(cellData3.position + new IntVector2(0, -1)) && (this.data[cellData3.position + new IntVector2(0, -1)].isExitCell || this.data[cellData3.position + new IntVector2(0, -1)].GetExitNeighbor() != null))
						{
							flag2 = true;
						}
						if (!flag2 && (cellData3.IsAnyFaceWall() || cellData3.type == CellType.WALL) && cellData3.GetExitNeighbor() != null)
						{
							flag2 = true;
						}
						if (flag2)
						{
							BraveUtility.DrawDebugSquare(intVector5.ToVector2() + new Vector2(0.3f, 0.3f), intVector5.ToVector2() + new Vector2(0.7f, 0.7f), Color.cyan, 1000f);
							this.assembler.BuildTileIndicesForCell(this, component, intVector.x + k, intVector.y + l);
							flag = true;
						}
					}
					if (!flag)
					{
						if (intVector4.x >= 0 && intVector4.y >= 0)
						{
							for (int m = 0; m < component.Layers.Length; m++)
							{
								int tile = this.MainTilemap.Layers[m].GetTile(intVector4.x, intVector4.y);
								component.Layers[m].SetTile(k + tilemapExpansion, l + tilemapExpansion, tile);
							}
						}
					}
				}
			}
			RenderMeshBuilder.CurrentCellXOffset = intVector.x - tilemapExpansion;
			RenderMeshBuilder.CurrentCellYOffset = intVector.y - tilemapExpansion;
			component.Build();
			RenderMeshBuilder.CurrentCellXOffset = 0;
			RenderMeshBuilder.CurrentCellYOffset = 0;
			component.renderData.transform.position = new Vector3((float)(intVector.x - tilemapExpansion), (float)(intVector.y - tilemapExpansion), (float)(intVector.y - tilemapExpansion));
			roomHandler.OverrideTilemap = component;
			roomHandler.PostGenerationCleanup();
			DeadlyDeadlyGoopManager.ReinitializeData();
			return roomHandler;
		}

		// Token: 0x06004FF2 RID: 20466 RVA: 0x001BF474 File Offset: 0x001BD674
		private void ConnectClusteredRuntimeRooms(RoomHandler first, RoomHandler second, PrototypeDungeonRoom firstPrototype, PrototypeDungeonRoom secondPrototype, int firstRoomExitIndex, int secondRoomExitIndex)
		{
			first.area.instanceUsedExits.Add(firstPrototype.exitData.exits[firstRoomExitIndex]);
			RuntimeRoomExitData runtimeRoomExitData = new RuntimeRoomExitData(firstPrototype.exitData.exits[firstRoomExitIndex]);
			first.area.exitToLocalDataMap.Add(firstPrototype.exitData.exits[firstRoomExitIndex], runtimeRoomExitData);
			second.area.instanceUsedExits.Add(secondPrototype.exitData.exits[secondRoomExitIndex]);
			RuntimeRoomExitData runtimeRoomExitData2 = new RuntimeRoomExitData(secondPrototype.exitData.exits[secondRoomExitIndex]);
			second.area.exitToLocalDataMap.Add(secondPrototype.exitData.exits[secondRoomExitIndex], runtimeRoomExitData2);
			first.connectedRooms.Add(second);
			first.connectedRoomsByExit.Add(firstPrototype.exitData.exits[firstRoomExitIndex], second);
			first.childRooms.Add(second);
			second.connectedRooms.Add(first);
			second.connectedRoomsByExit.Add(secondPrototype.exitData.exits[secondRoomExitIndex], first);
			second.parentRoom = first;
			runtimeRoomExitData.linkedExit = runtimeRoomExitData2;
			runtimeRoomExitData2.linkedExit = runtimeRoomExitData;
			runtimeRoomExitData.additionalExitLength = 3;
			runtimeRoomExitData2.additionalExitLength = 3;
		}

		// Token: 0x06004FF3 RID: 20467 RVA: 0x001BF5C4 File Offset: 0x001BD7C4
		public List<RoomHandler> AddRuntimeRoomCluster(List<PrototypeDungeonRoom> prototypes, List<IntVector2> basePositions, Action<RoomHandler> postProcessCellData = null, DungeonData.LightGenerationStyle lightStyle = DungeonData.LightGenerationStyle.FORCE_COLOR)
		{
			if (prototypes.Count != basePositions.Count)
			{
				UnityEngine.Debug.LogError("Attempting to add a malformed room cluster at runtime!");
				return null;
			}
			List<RoomHandler> list = new List<RoomHandler>();
			int num = 6;
			int num2 = 3;
			IntVector2 intVector = new IntVector2(int.MaxValue, int.MaxValue);
			IntVector2 intVector2 = new IntVector2(int.MinValue, int.MinValue);
			for (int i = 0; i < prototypes.Count; i++)
			{
				intVector = IntVector2.Min(intVector, basePositions[i]);
				intVector2 = IntVector2.Max(intVector2, basePositions[i] + new IntVector2(prototypes[i].Width, prototypes[i].Height));
			}
			IntVector2 intVector3 = intVector2 - intVector;
			IntVector2 intVector4 = IntVector2.Min(IntVector2.Zero, -1 * intVector);
			intVector3 += intVector4;
			IntVector2 intVector5 = new IntVector2(this.data.Width + num, num);
			int num3 = this.data.Width + num * 2 + intVector3.x;
			int num4 = Mathf.Max(this.data.Height, intVector3.y + num * 2);
			CellData[][] array = BraveUtility.MultidimensionalArrayResize<CellData>(this.data.cellData, this.data.Width, this.data.Height, num3, num4);
			this.data.cellData = array;
			this.data.ClearCachedCellData();
			for (int j = 0; j < prototypes.Count; j++)
			{
				IntVector2 intVector6 = new IntVector2(prototypes[j].Width, prototypes[j].Height);
				IntVector2 intVector7 = basePositions[j] + intVector4;
				IntVector2 intVector8 = intVector5 + intVector7;
				CellArea cellArea = new CellArea(intVector8, intVector6, 0);
				cellArea.prototypeRoom = prototypes[j];
				RoomHandler roomHandler = new RoomHandler(cellArea);
				for (int k = -num; k < intVector6.x + num; k++)
				{
					for (int l = -num; l < intVector6.y + num; l++)
					{
						IntVector2 intVector9 = new IntVector2(k, l) + intVector8;
						if ((k >= 0 && l >= 0 && k < intVector6.x && l < intVector6.y) || array[intVector9.x][intVector9.y] == null)
						{
							CellData cellData = new CellData(intVector9, CellType.WALL);
							cellData.positionInTilemap = cellData.positionInTilemap - intVector5 + new IntVector2(num2, num2);
							cellData.parentArea = cellArea;
							cellData.parentRoom = roomHandler;
							cellData.nearestRoom = roomHandler;
							cellData.distanceFromNearestRoom = 0f;
							array[intVector9.x][intVector9.y] = cellData;
						}
					}
				}
				this.data.rooms.Add(roomHandler);
				list.Add(roomHandler);
			}
			for (int m = 1; m < list.Count; m++)
			{
				this.ConnectClusteredRuntimeRooms(list[m - 1], list[m], prototypes[m - 1], prototypes[m], (m != 1) ? 1 : 0, 0);
			}
			for (int n = 0; n < list.Count; n++)
			{
				RoomHandler roomHandler2 = list[n];
				roomHandler2.WriteRoomData(this.data);
				GameManager.Instance.Dungeon.data.GenerateLightsForRoom(GameManager.Instance.Dungeon.decoSettings, roomHandler2, GameObject.Find("_Lights").transform, lightStyle);
				if (postProcessCellData != null)
				{
					postProcessCellData(roomHandler2);
				}
				if (roomHandler2.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET)
				{
					roomHandler2.BuildSecretRoomCover();
				}
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("RuntimeTileMap", ".prefab"));
			tk2dTileMap component = gameObject.GetComponent<tk2dTileMap>();
			string text = Guid.NewGuid().ToString();
			gameObject.name = "RuntimeTilemap_" + text;
			component.renderData.name = "RuntimeTilemap_" + text + " Render Data";
			component.Editor__SpriteCollection = this.tileIndices.dungeonCollection;
			TK2DDungeonAssembler.RuntimeResizeTileMap(component, intVector3.x + num2 * 2, intVector3.y + num2 * 2, this.m_tilemap.partitionSizeX, this.m_tilemap.partitionSizeY);
			for (int num5 = 0; num5 < prototypes.Count; num5++)
			{
				IntVector2 intVector10 = new IntVector2(prototypes[num5].Width, prototypes[num5].Height);
				IntVector2 intVector11 = basePositions[num5] + intVector4;
				IntVector2 intVector12 = intVector5 + intVector11;
				for (int num6 = -num2; num6 < intVector10.x + num2; num6++)
				{
					for (int num7 = -num2; num7 < intVector10.y + num2 + 2; num7++)
					{
						this.assembler.BuildTileIndicesForCell(this, component, intVector12.x + num6, intVector12.y + num7);
					}
				}
			}
			RenderMeshBuilder.CurrentCellXOffset = intVector5.x - num2;
			RenderMeshBuilder.CurrentCellYOffset = intVector5.y - num2;
			component.ForceBuild();
			RenderMeshBuilder.CurrentCellXOffset = 0;
			RenderMeshBuilder.CurrentCellYOffset = 0;
			component.renderData.transform.position = new Vector3((float)(intVector5.x - num2), (float)(intVector5.y - num2), (float)(intVector5.y - num2));
			for (int num8 = 0; num8 < list.Count; num8++)
			{
				list[num8].OverrideTilemap = component;
				for (int num9 = 0; num9 < list[num8].area.dimensions.x; num9++)
				{
					for (int num10 = 0; num10 < list[num8].area.dimensions.y + 2; num10++)
					{
						IntVector2 intVector13 = list[num8].area.basePosition + new IntVector2(num9, num10);
						if (this.data.CheckInBoundsAndValid(intVector13))
						{
							CellData cellData2 = this.data[intVector13];
							TK2DInteriorDecorator.PlaceLightDecorationForCell(this, component, cellData2, intVector13);
						}
					}
				}
				Pathfinder.Instance.InitializeRegion(this.data, list[num8].area.basePosition + new IntVector2(-3, -3), list[num8].area.dimensions + new IntVector2(3, 3));
				list[num8].PostGenerationCleanup();
			}
			DeadlyDeadlyGoopManager.ReinitializeData();
			return list;
		}

		// Token: 0x06004FF4 RID: 20468 RVA: 0x001BFCA8 File Offset: 0x001BDEA8
		public RoomHandler AddRuntimeRoom(PrototypeDungeonRoom prototype, Action<RoomHandler> postProcessCellData = null, DungeonData.LightGenerationStyle lightStyle = DungeonData.LightGenerationStyle.FORCE_COLOR)
		{
			int num = 6;
			int num2 = 3;
			IntVector2 intVector = new IntVector2(prototype.Width, prototype.Height);
			IntVector2 intVector2 = new IntVector2(this.data.Width + num, num);
			int num3 = this.data.Width + num * 2 + intVector.x;
			int num4 = Mathf.Max(this.data.Height, intVector.y + num * 2);
			CellData[][] array = BraveUtility.MultidimensionalArrayResize<CellData>(this.data.cellData, this.data.Width, this.data.Height, num3, num4);
			CellArea cellArea = new CellArea(intVector2, intVector, 0);
			cellArea.prototypeRoom = prototype;
			this.data.cellData = array;
			this.data.ClearCachedCellData();
			RoomHandler roomHandler = new RoomHandler(cellArea);
			for (int i = -num; i < intVector.x + num; i++)
			{
				for (int j = -num; j < intVector.y + num; j++)
				{
					IntVector2 intVector3 = new IntVector2(i, j) + intVector2;
					CellData cellData = new CellData(intVector3, CellType.WALL);
					cellData.positionInTilemap = cellData.positionInTilemap - intVector2 + new IntVector2(num2, num2);
					cellData.parentArea = cellArea;
					cellData.parentRoom = roomHandler;
					cellData.nearestRoom = roomHandler;
					cellData.distanceFromNearestRoom = 0f;
					array[intVector3.x][intVector3.y] = cellData;
				}
			}
			roomHandler.WriteRoomData(this.data);
			for (int k = -num; k < intVector.x + num; k++)
			{
				for (int l = -num; l < intVector.y + num; l++)
				{
					IntVector2 intVector4 = new IntVector2(k, l) + intVector2;
					array[intVector4.x][intVector4.y].breakable = true;
				}
			}
			this.data.rooms.Add(roomHandler);
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("RuntimeTileMap", ".prefab"));
			tk2dTileMap component = gameObject.GetComponent<tk2dTileMap>();
			component.Editor__SpriteCollection = this.tileIndices.dungeonCollection;
			GameManager.Instance.Dungeon.data.GenerateLightsForRoom(GameManager.Instance.Dungeon.decoSettings, roomHandler, GameObject.Find("_Lights").transform, lightStyle);
			if (postProcessCellData != null)
			{
				postProcessCellData(roomHandler);
			}
			TK2DDungeonAssembler.RuntimeResizeTileMap(component, intVector.x + num2 * 2, intVector.y + num2 * 2, this.m_tilemap.partitionSizeX, this.m_tilemap.partitionSizeY);
			for (int m = -num2; m < intVector.x + num2; m++)
			{
				for (int n = -num2; n < intVector.y + num2; n++)
				{
					this.assembler.BuildTileIndicesForCell(this, component, intVector2.x + m, intVector2.y + n);
				}
			}
			RenderMeshBuilder.CurrentCellXOffset = intVector2.x - num2;
			RenderMeshBuilder.CurrentCellYOffset = intVector2.y - num2;
			component.Build();
			RenderMeshBuilder.CurrentCellXOffset = 0;
			RenderMeshBuilder.CurrentCellYOffset = 0;
			component.renderData.transform.position = new Vector3((float)(intVector2.x - num2), (float)(intVector2.y - num2), (float)(intVector2.y - num2));
			roomHandler.OverrideTilemap = component;
			Pathfinder.Instance.InitializeRegion(this.data, roomHandler.area.basePosition + new IntVector2(-3, -3), roomHandler.area.dimensions + new IntVector2(3, 3));
			roomHandler.PostGenerationCleanup();
			DeadlyDeadlyGoopManager.ReinitializeData();
			return roomHandler;
		}

		// Token: 0x06004FF5 RID: 20469 RVA: 0x001C0078 File Offset: 0x001BE278
		public RoomHandler AddRuntimeRoom(IntVector2 dimensions, GameObject roomPrefab)
		{
			IntVector2 intVector = new IntVector2(this.data.Width + 10, 10);
			int num = this.data.Width + 10 + dimensions.x;
			int num2 = Mathf.Max(this.data.Height, dimensions.y + 10);
			CellData[][] array = BraveUtility.MultidimensionalArrayResize<CellData>(this.data.cellData, this.data.Width, this.data.Height, num, num2);
			CellArea cellArea = new CellArea(intVector, dimensions, 0);
			cellArea.IsProceduralRoom = true;
			this.data.cellData = array;
			this.data.ClearCachedCellData();
			RoomHandler roomHandler = new RoomHandler(cellArea);
			for (int i = 0; i < dimensions.x; i++)
			{
				for (int j = 0; j < dimensions.y; j++)
				{
					IntVector2 intVector2 = new IntVector2(i, j) + intVector;
					CellData cellData = new CellData(intVector2, CellType.FLOOR);
					cellData.parentArea = cellArea;
					cellData.parentRoom = roomHandler;
					cellData.nearestRoom = roomHandler;
					array[intVector2.x][intVector2.y] = cellData;
					roomHandler.RuntimeStampCellComplex(intVector2.x, intVector2.y, CellType.FLOOR, DiagonalWallType.NONE);
				}
			}
			this.data.rooms.Add(roomHandler);
			UnityEngine.Object.Instantiate<GameObject>(roomPrefab, new Vector3((float)intVector.x, (float)intVector.y, 0f), Quaternion.identity);
			DeadlyDeadlyGoopManager.ReinitializeData();
			return roomHandler;
		}

		// Token: 0x06004FF6 RID: 20470 RVA: 0x001C0200 File Offset: 0x001BE400
		public GeneratedEnemyData GetWeightedProceduralEnemy()
		{
			float num = 0f;
			float value = UnityEngine.Random.value;
			for (int i = 0; i < this.m_generatedEnemyData.Count; i++)
			{
				num += this.m_generatedEnemyData[i].percentOfEnemies;
				if (num > value)
				{
					return this.m_generatedEnemyData[i];
				}
			}
			return this.m_generatedEnemyData[this.m_generatedEnemyData.Count - 1];
		}

		// Token: 0x06004FF7 RID: 20471 RVA: 0x001C0278 File Offset: 0x001BE478
		protected void RegisterGeneratedEnemyData(string id, int totalEnemyCount, bool isSignature)
		{
			int num = -1;
			for (int i = 0; i < this.m_generatedEnemyData.Count; i++)
			{
				if (this.m_generatedEnemyData[i].enemyGuid == id)
				{
					num = i;
					break;
				}
			}
			if (num < 0)
			{
				GeneratedEnemyData generatedEnemyData = new GeneratedEnemyData(id, 1f / (float)totalEnemyCount, isSignature);
				this.m_generatedEnemyData.Add(generatedEnemyData);
			}
			else
			{
				GeneratedEnemyData generatedEnemyData2 = this.m_generatedEnemyData[num];
				generatedEnemyData2.percentOfEnemies += 1f / (float)totalEnemyCount;
				this.m_generatedEnemyData[num] = generatedEnemyData2;
			}
		}

		// Token: 0x06004FF8 RID: 20472 RVA: 0x001C0324 File Offset: 0x001BE524
		public void SpawnCurseReaper()
		{
			if (!GameManager.HasInstance || !GameManager.Instance.BestActivePlayer || GameManager.Instance.BestActivePlayer.CurrentRoom == null)
			{
				return;
			}
			this.CurseReaperActive = true;
			GameObject superReaper = PrefabDatabase.Instance.SuperReaper;
			Vector2 vector = GameManager.Instance.BestActivePlayer.CurrentRoom.GetRandomVisibleClearSpot(2, 2).ToVector2();
			SpeculativeRigidbody component = superReaper.GetComponent<SpeculativeRigidbody>();
			if (component)
			{
				PixelCollider primaryPixelCollider = component.PrimaryPixelCollider;
				Vector2 vector2 = PhysicsEngine.PixelToUnit(new IntVector2(primaryPixelCollider.ManualOffsetX, primaryPixelCollider.ManualOffsetY));
				Vector2 vector3 = PhysicsEngine.PixelToUnit(new IntVector2(primaryPixelCollider.ManualWidth, primaryPixelCollider.ManualHeight));
				Vector2 vector4 = new Vector2((float)Mathf.CeilToInt(vector3.x), (float)Mathf.CeilToInt(vector3.y));
				Vector2 vector5 = new Vector2((vector4.x - vector3.x) / 2f, 0f).Quantize(0.0625f);
				vector -= vector2 - vector5;
			}
			UnityEngine.Object.Instantiate<GameObject>(superReaper, vector.ToVector3ZUp(0f), Quaternion.identity);
		}

		// Token: 0x06004FF9 RID: 20473 RVA: 0x001C0458 File Offset: 0x001BE658
		private IEnumerator HandleAmbientChannelVFX()
		{
			this.m_ambientVFXProcessingActive = true;
			while (this.m_ambientVFXProcessingActive)
			{
				if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.LOW || GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.VERY_LOW)
				{
					yield return null;
				}
				else
				{
					if (!GameManager.Instance.IsLoadingLevel)
					{
						CameraController mainCameraController = GameManager.Instance.MainCameraController;
						if (!mainCameraController)
						{
							continue;
						}
						IntVector2 intVector = mainCameraController.MinVisiblePoint.ToIntVector2(VectorConversions.Floor);
						IntVector2 intVector2 = mainCameraController.MaxVisiblePoint.ToIntVector2(VectorConversions.Ceil);
						for (int i = intVector.x; i <= intVector2.x; i++)
						{
							for (int j = intVector.y; j <= intVector2.y; j++)
							{
								IntVector2 intVector3 = new IntVector2(i, j);
								if (this.data != null && this.data.CheckInBounds(intVector3, 3))
								{
									CellData cellData = this.data[intVector3];
									if (cellData != null && (cellData.cellVisualData.IsChannel || cellData.doesDamage) && !cellData.cellVisualData.precludeAllTileDrawing && this.IsCentralChannel(cellData))
									{
										DungeonMaterial dungeonMaterial = this.roomMaterialDefinitions[cellData.cellVisualData.roomVisualTypeIndex];
										RoomHandler parentRoom = cellData.parentRoom;
										if (!(dungeonMaterial == null))
										{
											if (dungeonMaterial.UseChannelAmbientVFX && dungeonMaterial.AmbientChannelVFX != null)
											{
												if (cellData.cellVisualData.PitVFXCooldown > 0f)
												{
													CellData cellData2 = cellData;
													cellData2.cellVisualData.PitVFXCooldown = cellData2.cellVisualData.PitVFXCooldown - BraveTime.DeltaTime;
												}
												else
												{
													float num = 0.5f;
													if (UnityEngine.Random.value < num)
													{
														GameObject gameObject = dungeonMaterial.AmbientChannelVFX[UnityEngine.Random.Range(0, dungeonMaterial.AmbientChannelVFX.Count)];
														Vector3 position = gameObject.transform.position;
														SpawnManager.SpawnVFX(gameObject, cellData.position.ToVector2().ToVector3ZisY(0f) + position + new Vector3(UnityEngine.Random.Range(0.25f, 0.75f), UnityEngine.Random.Range(0.25f, 0.75f), 2f), Quaternion.identity);
													}
													cellData.cellVisualData.PitVFXCooldown = UnityEngine.Random.Range(dungeonMaterial.ChannelVFXMinCooldown, dungeonMaterial.ChannelVFXMaxCooldown);
												}
											}
										}
									}
								}
							}
						}
					}
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x06004FFA RID: 20474 RVA: 0x001C0474 File Offset: 0x001BE674
		private bool IsCentralChannel(CellData cell)
		{
			IntVector2 position = cell.position;
			for (int i = 0; i < IntVector2.Cardinals.Length; i++)
			{
				IntVector2 intVector = position + IntVector2.Cardinals[i];
				if (!this.data[intVector].cellVisualData.IsChannel && !this.data[intVector].doesDamage)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004FFB RID: 20475 RVA: 0x001C04EC File Offset: 0x001BE6EC
		private IEnumerator HandleAmbientPitVFX()
		{
			this.m_ambientVFXProcessingActive = true;
			while (this.m_ambientVFXProcessingActive)
			{
				if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.LOW || GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.VERY_LOW)
				{
					yield return null;
				}
				else
				{
					if (!GameManager.Instance.IsLoadingLevel)
					{
						CameraController mainCameraController = GameManager.Instance.MainCameraController;
						if (!mainCameraController)
						{
							continue;
						}
						IntVector2 intVector = mainCameraController.MinVisiblePoint.ToIntVector2(VectorConversions.Floor);
						IntVector2 intVector2 = mainCameraController.MaxVisiblePoint.ToIntVector2(VectorConversions.Ceil);
						for (int i = intVector.x; i <= intVector2.x; i++)
						{
							for (int j = intVector.y; j <= intVector2.y; j++)
							{
								IntVector2 intVector3 = new IntVector2(i, j);
								if (this.data != null && this.data.CheckInBounds(intVector3, 3))
								{
									CellData cellData = this.data[intVector3];
									CellData cellData2 = this.data[intVector3 + IntVector2.Up];
									CellData cellData3 = this.data[intVector3 + IntVector2.Down];
									bool flag = true;
									if (this.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON)
									{
										CellData cellData4 = this.data[intVector3 + IntVector2.Up * 2];
										flag = cellData4 != null && cellData4.type == CellType.PIT;
									}
									if (cellData != null && flag && cellData.type == CellType.PIT && !cellData.fallingPrevented && !cellData.cellVisualData.precludeAllTileDrawing && cellData2 != null && cellData2.type == CellType.PIT && cellData3 != null)
									{
										DungeonMaterial dungeonMaterial = this.roomMaterialDefinitions[cellData.cellVisualData.roomVisualTypeIndex];
										RoomHandler parentRoom = cellData.parentRoom;
										if (!(dungeonMaterial == null))
										{
											if (!cellData.cellVisualData.HasTriggeredPitVFX)
											{
												cellData.cellVisualData.HasTriggeredPitVFX = true;
												cellData.cellVisualData.PitVFXCooldown = UnityEngine.Random.Range(1f, dungeonMaterial.PitVFXMaxCooldown / 2f);
												cellData.cellVisualData.PitParticleCooldown = UnityEngine.Random.Range(0f, 1f);
											}
											if (this.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON || (this.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON && dungeonMaterial.usesFacewallGrids))
											{
												CellData cellData5 = cellData;
												cellData5.cellVisualData.PitParticleCooldown = cellData5.cellVisualData.PitParticleCooldown - BraveTime.DeltaTime;
												if (cellData.cellVisualData.PitParticleCooldown <= 0f)
												{
													Vector3 vector = BraveUtility.RandomVector2(cellData.position.ToVector2(), cellData.position.ToVector2() + Vector2.one).ToVector3ZisY(0f);
													cellData.cellVisualData.PitParticleCooldown = UnityEngine.Random.Range(0.35f, 0.95f);
													if (this.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON && dungeonMaterial.usesFacewallGrids)
													{
														GlobalSparksDoer.DoSingleParticle(vector, Vector3.zero, null, new float?(0.375f), null, GlobalSparksDoer.SparksType.EMBERS_SWIRLING);
													}
													else if (this.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON && parentRoom != null && parentRoom.area.PrototypeRoomCategory != PrototypeDungeonRoom.RoomCategory.BOSS)
													{
														GlobalSparksDoer.DoSingleParticle(vector, Vector3.up, null, null, null, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
													}
												}
											}
											if (dungeonMaterial.UsePitAmbientVFX && dungeonMaterial.AmbientPitVFX != null && cellData3.type == CellType.PIT)
											{
												if (cellData.cellVisualData.PitVFXCooldown > 0f)
												{
													CellData cellData6 = cellData;
													cellData6.cellVisualData.PitVFXCooldown = cellData6.cellVisualData.PitVFXCooldown - BraveTime.DeltaTime;
												}
												else
												{
													float num = 1f;
													if (UnityEngine.Random.value < dungeonMaterial.ChanceToSpawnPitVFXOnCooldown * num)
													{
														GameObject gameObject = dungeonMaterial.AmbientPitVFX[UnityEngine.Random.Range(0, dungeonMaterial.AmbientPitVFX.Count)];
														Vector3 position = gameObject.transform.position;
														SpawnManager.SpawnVFX(gameObject, cellData.position.ToVector2().ToVector3ZisY(0f) + position + new Vector3(UnityEngine.Random.Range(0.25f, 0.75f), UnityEngine.Random.Range(0.25f, 0.75f), 2f), Quaternion.identity);
													}
													cellData.cellVisualData.PitVFXCooldown = UnityEngine.Random.Range(dungeonMaterial.PitVFXMinCooldown, dungeonMaterial.PitVFXMaxCooldown);
												}
											}
										}
									}
								}
							}
						}
					}
					yield return null;
				}
			}
			this.m_ambientVFXProcessingActive = false;
			yield break;
		}

		// Token: 0x040047DC RID: 18396
		public static bool IsGenerating;

		// Token: 0x040047DD RID: 18397
		public ContentSource contentSource;

		// Token: 0x040047DE RID: 18398
		public int DungeonSeed;

		// Token: 0x040047DF RID: 18399
		public string DungeonShortName = string.Empty;

		// Token: 0x040047E0 RID: 18400
		public string DungeonFloorName = "Gungeon";

		// Token: 0x040047E1 RID: 18401
		public string DungeonFloorLevelTextOverride = "no override";

		// Token: 0x040047E2 RID: 18402
		public GameManager.LevelOverrideState LevelOverrideType;

		// Token: 0x040047E3 RID: 18403
		public DebugDungeonSettings debugSettings;

		// Token: 0x040047E4 RID: 18404
		public SemioticDungeonGenSettings PatternSettings;

		// Token: 0x040047E5 RID: 18405
		public bool ForceRegenerationOfCharacters;

		// Token: 0x040047E6 RID: 18406
		public bool ActuallyGenerateTilemap = true;

		// Token: 0x040047E7 RID: 18407
		public TilemapDecoSettings decoSettings;

		// Token: 0x040047E8 RID: 18408
		public TileIndices tileIndices;

		// Token: 0x040047E9 RID: 18409
		[SerializeField]
		public DungeonMaterial[] roomMaterialDefinitions;

		// Token: 0x040047EA RID: 18410
		[SerializeField]
		public DungeonWingDefinition[] dungeonWingDefinitions;

		// Token: 0x040047EB RID: 18411
		[SerializeField]
		public List<TileIndexGrid> pathGridDefinitions;

		// Token: 0x040047EC RID: 18412
		public DustUpVFX dungeonDustups;

		// Token: 0x040047ED RID: 18413
		public DamageTypeEffectMatrix damageTypeEffectMatrix;

		// Token: 0x040047EE RID: 18414
		public DungeonTileStampData stampData;

		// Token: 0x040047EF RID: 18415
		[Header("Procedural Room Population")]
		public bool UsesCustomFloorIdea;

		// Token: 0x040047F0 RID: 18416
		public RobotDaveIdea FloorIdea;

		// Token: 0x040047F1 RID: 18417
		[Header("Doors")]
		public bool PlaceDoors;

		// Token: 0x040047F2 RID: 18418
		public DungeonPlaceable doorObjects;

		// Token: 0x040047F3 RID: 18419
		public DungeonPlaceable lockedDoorObjects;

		// Token: 0x040047F4 RID: 18420
		public DungeonPlaceable oneWayDoorObjects;

		// Token: 0x040047F5 RID: 18421
		public GameObject oneWayDoorPressurePlate;

		// Token: 0x040047F6 RID: 18422
		public DungeonPlaceable phantomBlockerDoorObjects;

		// Token: 0x040047F7 RID: 18423
		public DungeonPlaceable alternateDoorObjectsNakatomi;

		// Token: 0x040047F8 RID: 18424
		public bool UsesWallWarpWingDoors;

		// Token: 0x040047F9 RID: 18425
		public GameObject WarpWingDoorPrefab;

		// Token: 0x040047FA RID: 18426
		public GenericLootTable baseChestContents;

		// Token: 0x040047FB RID: 18427
		[Header("Secret Rooms")]
		public List<GameObject> SecretRoomSimpleTriggersFacewall;

		// Token: 0x040047FC RID: 18428
		public List<GameObject> SecretRoomSimpleTriggersSidewall;

		// Token: 0x040047FD RID: 18429
		public List<ComplexSecretRoomTrigger> SecretRoomComplexTriggers;

		// Token: 0x040047FE RID: 18430
		public GameObject SecretRoomDoorSparkVFX;

		// Token: 0x040047FF RID: 18431
		public GameObject SecretRoomHorizontalPoofVFX;

		// Token: 0x04004800 RID: 18432
		public GameObject SecretRoomVerticalPoofVFX;

		// Token: 0x04004801 RID: 18433
		public GameObject RatTrapdoor;

		// Token: 0x04004802 RID: 18434
		[EnemyIdentifier]
		public string NormalRatGUID;

		// Token: 0x04004803 RID: 18435
		public SharedDungeonSettings sharedSettingsPrefab;

		// Token: 0x04004804 RID: 18436
		[PickupIdentifier]
		public int BossMasteryTokenItemId = -1;

		// Token: 0x04004805 RID: 18437
		public bool UsesOverrideTertiaryBossSets;

		// Token: 0x04004806 RID: 18438
		public List<TertiaryBossRewardSet> OverrideTertiaryRewardSets;

		// Token: 0x04004807 RID: 18439
		public DungeonData data;

		// Token: 0x04004808 RID: 18440
		public GameObject defaultPlayerPrefab;

		// Token: 0x0400480A RID: 18442
		public Action OnAnyRoomVisited;

		// Token: 0x0400480B RID: 18443
		public Action OnAllRoomsVisited;

		// Token: 0x0400480C RID: 18444
		private TK2DDungeonAssembler assembler;

		// Token: 0x0400480D RID: 18445
		private tk2dTileMap m_tilemap;

		// Token: 0x0400480E RID: 18446
		[Header("Special Scene Options")]
		public bool StripPlayerOnArrival;

		// Token: 0x0400480F RID: 18447
		public bool SuppressEmergencyCrates;

		// Token: 0x04004810 RID: 18448
		public bool SetTutorialFlag;

		// Token: 0x04004811 RID: 18449
		[NonSerialized]
		public bool PreventPlayerLightInDarkTerrifyingRooms;

		// Token: 0x04004812 RID: 18450
		public bool PlayerIsLight;

		// Token: 0x04004813 RID: 18451
		public Color PlayerLightColor = Color.white;

		// Token: 0x04004814 RID: 18452
		public float PlayerLightIntensity = 3f;

		// Token: 0x04004815 RID: 18453
		public float PlayerLightRadius = 5f;

		// Token: 0x04004816 RID: 18454
		public GameObject[] PrefabsToAutoSpawn;

		// Token: 0x04004817 RID: 18455
		[NonSerialized]
		public int FrameDungeonGenerationFinished = -1;

		// Token: 0x04004818 RID: 18456
		[NonSerialized]
		public bool IsGlitchDungeon;

		// Token: 0x04004819 RID: 18457
		[NonSerialized]
		public bool OverrideAmbientLight;

		// Token: 0x0400481A RID: 18458
		[NonSerialized]
		public Color OverrideAmbientColor;

		// Token: 0x0400481B RID: 18459
		public const int TOP_WALL_ENEMY_BULLET_BLOCKER_PIXEL_HEIGHT = 14;

		// Token: 0x0400481C RID: 18460
		public const int TOP_WALL_ENEMY_BLOCKER_PIXEL_HEIGHT = 12;

		// Token: 0x0400481D RID: 18461
		public const int TOP_WALL_PLAYER_BLOCKER_PIXEL_HEIGHT = 8;

		// Token: 0x0400481E RID: 18462
		[NonSerialized]
		public bool HasGivenMasteryToken;

		// Token: 0x0400481F RID: 18463
		[NonSerialized]
		public bool HasGivenBossrushGun;

		// Token: 0x04004820 RID: 18464
		[NonSerialized]
		public bool IsEndTimes;

		// Token: 0x04004821 RID: 18465
		[NonSerialized]
		public bool CurseReaperActive;

		// Token: 0x04004822 RID: 18466
		[NonSerialized]
		public float GeneratedMagnificence;

		// Token: 0x04004825 RID: 18469
		public static bool ShouldAttemptToLoadFromMidgameSave;

		// Token: 0x04004826 RID: 18470
		private bool m_allRoomsVisited;

		// Token: 0x04004827 RID: 18471
		private bool m_musicIsPlaying;

		// Token: 0x04004828 RID: 18472
		public string musicEventName;

		// Token: 0x04004829 RID: 18473
		private float m_newPlayerMultiplier = -1f;

		// Token: 0x0400482A RID: 18474
		protected List<GeneratedEnemyData> m_generatedEnemyData = new List<GeneratedEnemyData>();

		// Token: 0x0400482B RID: 18475
		private bool m_ambientVFXProcessingActive;
	}
}
