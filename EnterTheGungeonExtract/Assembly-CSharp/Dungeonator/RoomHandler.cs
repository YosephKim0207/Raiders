using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Pathfinding;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000F16 RID: 3862
	public class RoomHandler
	{
		// Token: 0x0600525D RID: 21085 RVA: 0x001D9464 File Offset: 0x001D7664
		public RoomHandler(CellArea a)
		{
			this.area = a;
			if (GameManager.Instance.BestGenerationDungeonPrefab != null)
			{
				this.RoomVisualSubtype = GameManager.Instance.BestGenerationDungeonPrefab.decoSettings.standardRoomVisualSubtypes.SelectByWeight();
			}
			else
			{
				this.RoomVisualSubtype = GameManager.Instance.Dungeon.decoSettings.standardRoomVisualSubtypes.SelectByWeight();
			}
			if (this.area.prototypeRoom == null)
			{
				this.RoomVisualSubtype = 0;
			}
			if (a.prototypeRoom != null && a.prototypeRoom.overrideRoomVisualType >= 0)
			{
				this.RoomVisualSubtype = a.prototypeRoom.overrideRoomVisualType;
			}
			if (a.prototypeRoom != null)
			{
				Dungeon dungeon = ((!(GameManager.Instance.BestGenerationDungeonPrefab != null)) ? GameManager.Instance.Dungeon : GameManager.Instance.BestGenerationDungeonPrefab);
				DungeonMaterial dungeonMaterial = dungeon.roomMaterialDefinitions[this.m_roomVisualType];
				bool flag = a.prototypeRoom.ContainsPit();
				bool flag2 = false;
				for (int i = 0; i < dungeon.decoSettings.standardRoomVisualSubtypes.elements.Length; i++)
				{
					WeightedInt weightedInt = dungeon.decoSettings.standardRoomVisualSubtypes.elements[i];
					if (weightedInt.weight > 0f)
					{
						if (dungeon.roomMaterialDefinitions[weightedInt.value].supportsPits)
						{
							flag2 = true;
							break;
						}
					}
				}
				if (flag2)
				{
					while (flag && !dungeonMaterial.supportsPits)
					{
						this.RoomVisualSubtype = dungeon.decoSettings.standardRoomVisualSubtypes.SelectByWeight();
						dungeonMaterial = dungeon.roomMaterialDefinitions[this.m_roomVisualType];
					}
				}
				this.PrecludeTilemapDrawing = a.prototypeRoom.precludeAllTilemapDrawing;
				this.DrawPrecludedCeilingTiles = a.prototypeRoom.drawPrecludedCeilingTiles;
			}
			if (GameManager.Instance.BestGenerationDungeonPrefab != null)
			{
				if (this.m_roomVisualType < 0 || this.m_roomVisualType >= GameManager.Instance.BestGenerationDungeonPrefab.roomMaterialDefinitions.Length)
				{
					this.m_roomVisualType = 0;
				}
			}
			else if (this.m_roomVisualType < 0 || this.m_roomVisualType >= GameManager.Instance.Dungeon.roomMaterialDefinitions.Length)
			{
				this.m_roomVisualType = 0;
			}
			this.roomType = RoomCreationStrategy.RoomType.PREDEFINED_ROOM;
			this.opulence = Opulence.FINE;
			this.childRooms = new List<RoomHandler>();
			this.connectedDoors = new List<DungeonDoorController>();
			this.standaloneBlockers = new List<DungeonDoorSubsidiaryBlocker>();
			this.connectedRooms = new List<RoomHandler>();
			this.connectedRoomsByExit = new Dictionary<PrototypeRoomExit, RoomHandler>();
			this.interactableObjects = new List<IPlayerInteractable>();
			this.autoAimTargets = new List<IAutoAimTarget>();
			this.OnEnemiesCleared = (Action)Delegate.Combine(this.OnEnemiesCleared, new Action(this.NotifyPlayerRoomCleared));
			this.OnEnemiesCleared = (Action)Delegate.Combine(this.OnEnemiesCleared, new Action(this.HandleRoomClearReward));
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x0600525E RID: 21086 RVA: 0x001D97C0 File Offset: 0x001D79C0
		// (set) Token: 0x0600525F RID: 21087 RVA: 0x001D97DC File Offset: 0x001D79DC
		public RoomHandler.VisibilityStatus visibility
		{
			get
			{
				if (this.OverrideVisibility != RoomHandler.VisibilityStatus.INVALID)
				{
					return this.OverrideVisibility;
				}
				return this.m_visibility;
			}
			set
			{
				this.m_visibility = value;
				if (this.m_visibility == RoomHandler.VisibilityStatus.OBSCURED || this.m_visibility == RoomHandler.VisibilityStatus.REOBSCURED)
				{
					this.hasEverBeenVisited = false;
				}
				else if (this.m_visibility == RoomHandler.VisibilityStatus.VISITED)
				{
					this.hasEverBeenVisited = true;
				}
			}
		}

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x06005260 RID: 21088 RVA: 0x001D981C File Offset: 0x001D7A1C
		public bool TeleportersActive
		{
			get
			{
				return this.IsVisible || this.forceTeleportersActive;
			}
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06005261 RID: 21089 RVA: 0x001D9834 File Offset: 0x001D7A34
		public bool IsVisible
		{
			get
			{
				return this.visibility != RoomHandler.VisibilityStatus.OBSCURED && this.visibility != RoomHandler.VisibilityStatus.REOBSCURED;
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06005262 RID: 21090 RVA: 0x001D9850 File Offset: 0x001D7A50
		// (set) Token: 0x06005263 RID: 21091 RVA: 0x001D9858 File Offset: 0x001D7A58
		public bool IsShop { get; set; }

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x06005264 RID: 21092 RVA: 0x001D9864 File Offset: 0x001D7A64
		public bool IsWildWestEntrance
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06005265 RID: 21093 RVA: 0x001D9868 File Offset: 0x001D7A68
		// (set) Token: 0x06005266 RID: 21094 RVA: 0x001D9880 File Offset: 0x001D7A80
		public bool RevealedOnMap
		{
			get
			{
				return this.visibility != RoomHandler.VisibilityStatus.OBSCURED || this.m_forceRevealedOnMap;
			}
			set
			{
				if (!this.m_forceRevealedOnMap && this.OnRevealedOnMap != null)
				{
					this.OnRevealedOnMap();
				}
				this.m_forceRevealedOnMap = value;
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x06005267 RID: 21095 RVA: 0x001D98AC File Offset: 0x001D7AAC
		// (set) Token: 0x06005268 RID: 21096 RVA: 0x001D98B4 File Offset: 0x001D7AB4
		public int RoomVisualSubtype
		{
			get
			{
				return this.m_roomVisualType;
			}
			set
			{
				this.m_roomVisualType = value;
			}
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x06005269 RID: 21097 RVA: 0x001D98C0 File Offset: 0x001D7AC0
		public DungeonMaterial RoomMaterial
		{
			get
			{
				return GameManager.Instance.Dungeon.roomMaterialDefinitions[this.RoomVisualSubtype];
			}
		}

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x0600526A RID: 21098 RVA: 0x001D98D8 File Offset: 0x001D7AD8
		public List<IntVector2> Cells
		{
			get
			{
				return this.roomCells;
			}
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x0600526B RID: 21099 RVA: 0x001D98E0 File Offset: 0x001D7AE0
		public List<IntVector2> CellsWithoutExits
		{
			get
			{
				return this.roomCellsWithoutExits;
			}
		}

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x0600526C RID: 21100 RVA: 0x001D98E8 File Offset: 0x001D7AE8
		public HashSet<IntVector2> RawCells
		{
			get
			{
				return this.rawRoomCells;
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x0600526D RID: 21101 RVA: 0x001D98F0 File Offset: 0x001D7AF0
		public List<IntVector2> FeatureCells
		{
			get
			{
				return this.featureCells;
			}
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x0600526E RID: 21102 RVA: 0x001D98F8 File Offset: 0x001D7AF8
		public bool IsSealed
		{
			get
			{
				return this.m_isSealed;
			}
		}

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x0600526F RID: 21103 RVA: 0x001D9900 File Offset: 0x001D7B00
		// (set) Token: 0x06005270 RID: 21104 RVA: 0x001D9908 File Offset: 0x001D7B08
		public IntVector2? OverrideBossPedestalLocation { get; set; }

		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x06005271 RID: 21105 RVA: 0x001D9914 File Offset: 0x001D7B14
		// (remove) Token: 0x06005272 RID: 21106 RVA: 0x001D994C File Offset: 0x001D7B4C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event RoomHandler.OnEnteredEventHandler Entered;

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x06005273 RID: 21107 RVA: 0x001D9984 File Offset: 0x001D7B84
		// (set) Token: 0x06005274 RID: 21108 RVA: 0x001D998C File Offset: 0x001D7B8C
		public bool IsGunslingKingChallengeRoom { get; set; }

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x06005275 RID: 21109 RVA: 0x001D9998 File Offset: 0x001D7B98
		// (set) Token: 0x06005276 RID: 21110 RVA: 0x001D99A0 File Offset: 0x001D7BA0
		public bool IsWinchesterArcadeRoom { get; set; }

		// Token: 0x06005277 RID: 21111 RVA: 0x001D99AC File Offset: 0x001D7BAC
		protected virtual void OnEntered(PlayerController p)
		{
			this.SetRoomActive(true);
			if (this.OverrideTilemap != null && PhysicsEngine.Instance.TileMap != this.OverrideTilemap)
			{
				PhysicsEngine.Instance.ClearAllCachedTiles();
				PhysicsEngine.Instance.TileMap = this.OverrideTilemap;
			}
			else if (this.OverrideTilemap == null && PhysicsEngine.Instance.TileMap != GameManager.Instance.Dungeon.MainTilemap)
			{
				PhysicsEngine.Instance.ClearAllCachedTiles();
				PhysicsEngine.Instance.TileMap = GameManager.Instance.Dungeon.MainTilemap;
			}
			if (GameManager.Instance.CurrentLevelOverrideState != GameManager.LevelOverrideState.TUTORIAL)
			{
				GameManager.Instance.Dungeon.StartCoroutine(this.DeferredMarkVisibleRoomsActive(p));
			}
			if (!this.area.IsProceduralRoom && !this.m_hasBeenEncountered)
			{
				this.m_hasBeenEncountered = true;
				GameStatsManager.Instance.HandleEncounteredRoom(this.area.runtimePrototypeData);
			}
			if (!this.m_currentlyVisible)
			{
				this.OnBecameVisible(p);
			}
			if (GameManager.Instance.NumberOfLivingPlayers == 1 && !p.IsGhost)
			{
				Minimap.Instance.RevealMinimapRoom(this, false, true, true);
			}
			else if (p.IsPrimaryPlayer)
			{
				Minimap.Instance.RevealMinimapRoom(this, false, true, true);
			}
			if (this.m_secretRoomCoverObject != null)
			{
				this.m_secretRoomCoverObject.SetActive(false);
			}
			this.ProcessRoomEvents(RoomEventTriggerCondition.ON_ENTER);
			List<AIActor> list = this.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear);
			int num = 0;
			if (list != null)
			{
				if (list.Exists((AIActor a) => !a.healthHaver.IsDead))
				{
					num += list.Count;
					this.ProcessRoomEvents(RoomEventTriggerCondition.ON_ENTER_WITH_ENEMIES);
					if (this.remainingReinforcementLayers != null)
					{
						for (int i = 0; i < this.remainingReinforcementLayers.Count; i++)
						{
							num += this.remainingReinforcementLayers[i].placedObjects.Count;
							if (this.remainingReinforcementLayers[i].reinforcementTriggerCondition == RoomEventTriggerCondition.TIMER)
							{
								GameManager.Instance.StartCoroutine(this.HandleTimedReinforcementLayer(this.remainingReinforcementLayers[i]));
							}
						}
					}
				}
			}
			if (this.Entered != null)
			{
				this.Entered(p);
			}
			bool flag = true;
			for (int j = 0; j < GameManager.Instance.Dungeon.data.rooms.Count; j++)
			{
				if (GameManager.Instance.Dungeon.data.rooms[j].visibility == RoomHandler.VisibilityStatus.OBSCURED)
				{
					flag = false;
					break;
				}
			}
			if (GameManager.Instance.Dungeon.OnAnyRoomVisited != null)
			{
				GameManager.Instance.Dungeon.OnAnyRoomVisited();
			}
			if (flag)
			{
				GameManager.Instance.Dungeon.NotifyAllRoomsVisited();
			}
		}

		// Token: 0x06005278 RID: 21112 RVA: 0x001D9CAC File Offset: 0x001D7EAC
		public IEnumerator DeferredMarkVisibleRoomsActive(PlayerController p)
		{
			bool shouldActiveAllRoomsForEntranceDelayPeriod = GameManager.Instance.Dungeon.data.Entrance == this;
			if (!GameManager.Instance.IsFoyer && GameManager.Instance.Dungeon.FrameDungeonGenerationFinished > 0 && Time.frameCount - GameManager.Instance.Dungeon.FrameDungeonGenerationFinished > 100)
			{
				shouldActiveAllRoomsForEntranceDelayPeriod = false;
			}
			if (shouldActiveAllRoomsForEntranceDelayPeriod)
			{
				for (int j = 0; j < this.connectedRooms.Count; j++)
				{
					if (this.connectedRooms[j].visibility != RoomHandler.VisibilityStatus.OBSCURED)
					{
						shouldActiveAllRoomsForEntranceDelayPeriod = false;
					}
				}
			}
			this.m_allRoomsActive = shouldActiveAllRoomsForEntranceDelayPeriod;
			int changedCounter = 0;
			for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
			{
				bool changedRoom = false;
				RoomHandler room = GameManager.Instance.Dungeon.data.rooms[i];
				RoomHandler otherCurrentRoom = null;
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
				{
					PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(p);
					if (otherPlayer && otherPlayer.CurrentRoom != this)
					{
						otherCurrentRoom = otherPlayer.CurrentRoom;
					}
				}
				if (room != this)
				{
					Rect rect = room.cameraBoundingRect;
					rect.xMin -= 1f;
					rect.xMax += 1f;
					rect.yMin -= 1f;
					rect.yMax += 3f;
					if (shouldActiveAllRoomsForEntranceDelayPeriod)
					{
						changedRoom = room.SetRoomActive(true);
					}
					else if (GameManager.Instance.Dungeon.data.Entrance == this && room.IsMaintenanceRoom())
					{
						changedRoom = room.SetRoomActive(true);
					}
					else if (this.connectedRooms.Contains(room) || BraveMathCollege.DistBetweenRectangles(room.cameraBoundingRect.min, room.cameraBoundingRect.size, this.cameraBoundingRect.min, this.cameraBoundingRect.size) <= GameManager.Instance.MainCameraController.Camera.orthographicSize * GameManager.Instance.MainCameraController.Camera.aspect)
					{
						if (this.connectedRooms.Contains(room))
						{
							changedRoom = room.SetRoomActive(true);
						}
						else
						{
							changedRoom = room.SetRoomActive(true && (room.visibility == RoomHandler.VisibilityStatus.VISITED || room.visibility == RoomHandler.VisibilityStatus.CURRENT));
						}
					}
					else if (otherCurrentRoom != null && BraveMathCollege.DistBetweenRectangles(room.cameraBoundingRect.min, room.cameraBoundingRect.size, otherCurrentRoom.cameraBoundingRect.min, otherCurrentRoom.cameraBoundingRect.size) <= GameManager.Instance.MainCameraController.Camera.orthographicSize * GameManager.Instance.MainCameraController.Camera.aspect)
					{
						if (otherCurrentRoom.connectedRooms.Contains(room))
						{
							changedRoom = room.SetRoomActive(true);
						}
						else
						{
							changedRoom = room.SetRoomActive(true && (room.visibility == RoomHandler.VisibilityStatus.VISITED || room.visibility == RoomHandler.VisibilityStatus.CURRENT));
						}
					}
					else
					{
						changedRoom = room.SetRoomActive(false);
					}
				}
				if (changedRoom)
				{
					changedCounter++;
					if (changedCounter >= 3)
					{
						changedCounter = 0;
						yield return null;
					}
				}
			}
			yield break;
		}

		// Token: 0x06005279 RID: 21113 RVA: 0x001D9CD0 File Offset: 0x001D7ED0
		public bool SetRoomActive(bool active)
		{
			if (this.ForcedActiveState != null && this.ForcedActiveState.Value != active)
			{
				return false;
			}
			if (this.m_roomMotionHandler != null && this.m_roomMotionHandler.gameObject.activeSelf != active)
			{
				this.m_roomMotionHandler.gameObject.SetActive(active);
				return true;
			}
			return false;
		}

		// Token: 0x0600527A RID: 21114 RVA: 0x001D9D3C File Offset: 0x001D7F3C
		private IEnumerator HandleTimedReinforcementLayer(PrototypeRoomObjectLayer layer)
		{
			this.numberOfTimedWavesOnDeck++;
			yield return null;
			float elapsed = 0f;
			while (elapsed < layer.delayTime && GameManager.Instance.IsAnyPlayerInRoom(this) && this.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
			{
				elapsed += BraveTime.DeltaTime;
				yield return null;
			}
			this.numberOfTimedWavesOnDeck--;
			if (GameManager.Instance.IsAnyPlayerInRoom(this) && this.remainingReinforcementLayers != null && this.remainingReinforcementLayers.Count > 0)
			{
				this.TriggerReinforcementLayer(this.remainingReinforcementLayers.IndexOf(layer), true, false, -1, -1, false);
			}
			yield break;
		}

		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x0600527B RID: 21115 RVA: 0x001D9D60 File Offset: 0x001D7F60
		// (remove) Token: 0x0600527C RID: 21116 RVA: 0x001D9D98 File Offset: 0x001D7F98
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event RoomHandler.OnExitedEventHandler Exited;

		// Token: 0x0600527D RID: 21117 RVA: 0x001D9DD0 File Offset: 0x001D7FD0
		protected virtual void OnExited(PlayerController p)
		{
			if (this.m_currentlyVisible)
			{
				this.OnBecameInvisible(p);
			}
			if (this.ExtantEmergencyCrate != null)
			{
				EmergencyCrateController component = this.ExtantEmergencyCrate.GetComponent<EmergencyCrateController>();
				if (component)
				{
					component.ClearLandingTarget();
				}
				UnityEngine.Object.Destroy(this.ExtantEmergencyCrate);
				this.ExtantEmergencyCrate = null;
			}
			this.ProcessRoomEvents(RoomEventTriggerCondition.ON_EXIT);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				if (!GameManager.Instance.GetOtherPlayer(p) || GameManager.Instance.GetOtherPlayer(p).CurrentRoom != this)
				{
					Minimap.Instance.DeflagPreviousRoom(this);
				}
			}
			else
			{
				Minimap.Instance.DeflagPreviousRoom(this);
			}
			if (this.Exited != null)
			{
				this.Exited();
			}
		}

		// Token: 0x0600527E RID: 21118 RVA: 0x001D9EA4 File Offset: 0x001D80A4
		public bool RoomFallValidForMaintenance()
		{
			GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
			return GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.NONE && this == GameManager.Instance.Dungeon.data.Entrance && (tilesetId == GlobalDungeonData.ValidTilesets.GUNGEON || tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON || tilesetId == GlobalDungeonData.ValidTilesets.CATACOMBGEON || tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON);
		}

		// Token: 0x0600527F RID: 21119 RVA: 0x001D9F14 File Offset: 0x001D8114
		public void BecomeTerrifyingDarkRoom(float duration = 1f, float goalIntensity = 0.1f, float lightIntensity = 1f, string wwiseEvent = "Play_ENM_darken_world_01")
		{
			if (this.IsDarkAndTerrifying)
			{
				return;
			}
			if (this.area.PrototypeRoomCategory != PrototypeDungeonRoom.RoomCategory.BOSS && this.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) == 0)
			{
				return;
			}
			if (this.OnChangedTerrifyingDarkState != null)
			{
				this.OnChangedTerrifyingDarkState(true);
			}
			GameManager.Instance.StartCoroutine(this.HandleBecomeTerrifyingDarkRoom(duration, goalIntensity, lightIntensity, false));
			AkSoundEngine.PostEvent(wwiseEvent, GameManager.Instance.PrimaryPlayer.gameObject);
		}

		// Token: 0x06005280 RID: 21120 RVA: 0x001D9F90 File Offset: 0x001D8190
		public void EndTerrifyingDarkRoom(float duration = 1f, float goalIntensity = 0.1f, float lightIntensity = 1f, string wwiseEvent = "Play_ENM_lighten_world_01")
		{
			if (!this.IsDarkAndTerrifying)
			{
				return;
			}
			if (this.OnChangedTerrifyingDarkState != null)
			{
				this.OnChangedTerrifyingDarkState(false);
			}
			GameManager.Instance.StartCoroutine(this.HandleBecomeTerrifyingDarkRoom(duration, goalIntensity, lightIntensity, true));
			AkSoundEngine.PostEvent(wwiseEvent, GameManager.Instance.PrimaryPlayer.gameObject);
		}

		// Token: 0x06005281 RID: 21121 RVA: 0x001D9FEC File Offset: 0x001D81EC
		private IEnumerator HandleBecomeTerrifyingDarkRoom(float duration, float goalIntensity, float lightIntensity = 1f, bool reverse = false)
		{
			float elapsed = 0f;
			this.IsDarkAndTerrifying = !reverse;
			while (elapsed < duration || duration == 0f)
			{
				elapsed += GameManager.INVARIANT_DELTA_TIME;
				float t = ((duration != 0f) ? Mathf.Clamp01(elapsed / duration) : 1f);
				if (reverse)
				{
					t = 1f - t;
				}
				float num = ((GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW) ? 1f : 1.25f);
				RenderSettings.ambientIntensity = num;
				float targetAmbient = num;
				RenderSettings.ambientIntensity = Mathf.Lerp(targetAmbient, goalIntensity, t);
				if (!GameManager.Instance.Dungeon.PreventPlayerLightInDarkTerrifyingRooms)
				{
					GameManager.Instance.Dungeon.PlayerIsLight = true;
					GameManager.Instance.Dungeon.PlayerLightColor = Color.white;
					GameManager.Instance.Dungeon.PlayerLightIntensity = Mathf.Lerp(0f, lightIntensity * 4.25f, t);
					GameManager.Instance.Dungeon.PlayerLightRadius = Mathf.Lerp(0f, lightIntensity * 7.25f, t);
				}
				Pixelator.Instance.pointLightMultiplier = Mathf.Lerp(1f, 0f, t);
				if (duration == 0f)
				{
					break;
				}
				yield return null;
			}
			if (!GameManager.Instance.Dungeon.PreventPlayerLightInDarkTerrifyingRooms && reverse)
			{
				GameManager.Instance.Dungeon.PlayerIsLight = false;
			}
			yield break;
		}

		// Token: 0x06005282 RID: 21122 RVA: 0x001DA024 File Offset: 0x001D8224
		public bool IsMaintenanceRoom()
		{
			if (this.m_cachedIsMaintenance == null)
			{
				this.m_cachedIsMaintenance = new bool?(!string.IsNullOrEmpty(this.GetRoomName()) && this.GetRoomName().Contains("Maintenance"));
			}
			return this.m_cachedIsMaintenance.Value;
		}

		// Token: 0x06005283 RID: 21123 RVA: 0x001DA07C File Offset: 0x001D827C
		public string GetRoomName()
		{
			return this.area.PrototypeRoomName;
		}

		// Token: 0x06005284 RID: 21124 RVA: 0x001DA08C File Offset: 0x001D828C
		public void PlayerEnter(PlayerController playerEntering)
		{
			if (Pathfinder.HasInstance)
			{
				Pathfinder.Instance.TryRecalculateRoomClearances(this);
			}
			this.OnEntered(playerEntering);
			GameManager.Instance.DungeonMusicController.NotifyEnteredNewRoom(this);
			Pixelator.Instance.ProcessRoomAdditionalExits(playerEntering.transform.position.IntXY(VectorConversions.Floor), this, false);
		}

		// Token: 0x06005285 RID: 21125 RVA: 0x001DA0E4 File Offset: 0x001D82E4
		public void PlayerInCell(PlayerController p, IntVector2 playerCellPosition, Vector2 relevantCornerOfPlayer)
		{
			if (this.m_roomMotionHandler != null && !this.m_roomMotionHandler.gameObject.activeSelf)
			{
				this.m_roomMotionHandler.gameObject.SetActive(true);
				GameManager.Instance.Dungeon.StartCoroutine(this.DeferredMarkVisibleRoomsActive(p));
			}
			if (GameManager.Instance.Dungeon.data.Entrance == this && this.m_allRoomsActive && GameManager.Instance.Dungeon.FrameDungeonGenerationFinished > 0 && Time.frameCount - GameManager.Instance.Dungeon.FrameDungeonGenerationFinished > 100)
			{
				GameManager.Instance.Dungeon.StartCoroutine(this.DeferredMarkVisibleRoomsActive(p));
			}
			CellData cellData = GameManager.Instance.Dungeon.data[playerCellPosition];
			if (cellData != null && !cellData.isExitCell && cellData.parentRoom != null && !cellData.parentRoom.RevealedOnMap)
			{
				Minimap.Instance.RevealMinimapRoom(cellData.parentRoom, true, true, true);
			}
			if (this.ForcePitfallForFliers && p && !p.IsFalling && p.IsFlying && cellData != null && cellData.type == CellType.PIT && !cellData.fallingPrevented)
			{
				Rect rect = default(Rect);
				rect.min = PhysicsEngine.PixelToUnitMidpoint(p.specRigidbody.PrimaryPixelCollider.LowerLeft);
				rect.max = PhysicsEngine.PixelToUnitMidpoint(p.specRigidbody.PrimaryPixelCollider.UpperRight);
				Dungeon dungeon = GameManager.Instance.Dungeon;
				bool flag = dungeon.ShouldReallyFall(rect.min);
				bool flag2 = dungeon.ShouldReallyFall(new Vector3(rect.xMax, rect.yMin));
				bool flag3 = dungeon.ShouldReallyFall(new Vector3(rect.xMin, rect.yMax));
				bool flag4 = dungeon.ShouldReallyFall(rect.max);
				bool flag5 = dungeon.ShouldReallyFall(rect.center);
				if (flag && flag2 && flag5 && flag3 && flag4)
				{
					p.ForceFall();
				}
			}
			if (cellData.doesDamage && (cellData.damageDefinition.damageToPlayersPerTick > 0f || cellData.damageDefinition.isPoison) && p.IsGrounded && p.CurrentFloorDamageCooldown <= 0f && p.healthHaver.IsVulnerable)
			{
				bool flag6 = true;
				int tile = GameManager.Instance.Dungeon.MainTilemap.Layers[GlobalDungeonData.floorLayerIndex].GetTile(playerCellPosition.x, playerCellPosition.y);
				if (tile >= 0 && tile < GameManager.Instance.Dungeon.tileIndices.dungeonCollection.spriteDefinitions.Length)
				{
					tk2dSpriteDefinition tk2dSpriteDefinition = GameManager.Instance.Dungeon.tileIndices.dungeonCollection.spriteDefinitions[tile];
					BagelCollider[] array = ((tk2dSpriteDefinition == null) ? null : GameManager.Instance.Dungeon.tileIndices.dungeonCollection.GetBagelColliders(tile));
					if (array != null && array.Length > 0)
					{
						flag6 = false;
						BagelCollider bagelCollider = array[0];
						IntVector2 intVector = ((p.specRigidbody.PrimaryPixelCollider.UnitCenter - playerCellPosition.ToVector2()) * 16f).ToIntVector2(VectorConversions.Floor);
						if (intVector.x >= 0 && intVector.y >= 0 && intVector.x < 16 && intVector.y < 16 && bagelCollider[intVector.x, bagelCollider.height - intVector.y - 1])
						{
							flag6 = true;
						}
					}
				}
				if (flag6)
				{
					if (cellData.damageDefinition.isPoison || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON)
					{
						p.IncreasePoison(BraveTime.DeltaTime / cellData.damageDefinition.tickFrequency);
						if (p.CurrentPoisonMeterValue >= 1f)
						{
							p.healthHaver.ApplyDamage(cellData.damageDefinition.damageToPlayersPerTick, Vector2.zero, StringTableManager.GetEnemiesString("#THEFLOOR", -1), cellData.damageDefinition.damageTypes, DamageCategory.Environment, false, null, false);
							p.CurrentPoisonMeterValue -= 1f;
						}
					}
					else if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
					{
						p.IsOnFire = true;
					}
					else
					{
						p.healthHaver.ApplyDamage(cellData.damageDefinition.damageToPlayersPerTick, Vector2.zero, StringTableManager.GetEnemiesString("#THEFLOOR", -1), cellData.damageDefinition.damageTypes, DamageCategory.Environment, false, null, false);
						p.CurrentFloorDamageCooldown = cellData.damageDefinition.tickFrequency;
					}
				}
			}
			if (this.eventTriggerAreas != null)
			{
				for (int i = 0; i < this.eventTriggerAreas.Count; i++)
				{
					RoomEventTriggerArea roomEventTriggerArea = this.eventTriggerAreas[i];
					if (roomEventTriggerArea.triggerCells.Contains(playerCellPosition))
					{
						roomEventTriggerArea.Trigger(i);
					}
				}
			}
			if (this.activeEnemies != null)
			{
				for (int j = 0; j < this.activeEnemies.Count; j++)
				{
					if (!this.activeEnemies[j])
					{
						this.activeEnemies.RemoveAt(j);
						j--;
					}
				}
			}
			if (!this.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear) && this.numberOfTimedWavesOnDeck <= 0 && (this.area.IsProceduralRoom || this.area.runtimePrototypeData.DoesUnsealOnClear()))
			{
				this.UnsealRoom();
			}
		}

		// Token: 0x06005286 RID: 21126 RVA: 0x001DA6D4 File Offset: 0x001D88D4
		public void PlayerExit(PlayerController playerExiting)
		{
			this.OnExited(playerExiting);
		}

		// Token: 0x06005287 RID: 21127 RVA: 0x001DA6E0 File Offset: 0x001D88E0
		public bool ContainsPosition(IntVector2 pos)
		{
			return this.rawRoomCells.Contains(pos);
		}

		// Token: 0x06005288 RID: 21128 RVA: 0x001DA6F0 File Offset: 0x001D88F0
		public bool ContainsCell(CellData cell)
		{
			return this.roomCells.Contains(cell.position);
		}

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x06005289 RID: 21129 RVA: 0x001DA704 File Offset: 0x001D8904
		public bool IsStartOfWarpWing
		{
			get
			{
				if (this.area.instanceUsedExits.Count == 0 && !this.area.IsProceduralRoom)
				{
					return true;
				}
				for (int i = 0; i < this.area.instanceUsedExits.Count; i++)
				{
					if (this.area.exitToLocalDataMap.ContainsKey(this.area.instanceUsedExits[i]) && this.area.exitToLocalDataMap[this.area.instanceUsedExits[i]].isWarpWingStart)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x0600528A RID: 21130 RVA: 0x001DA7B0 File Offset: 0x001D89B0
		public bool IsStandardRoom
		{
			get
			{
				return this.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.NORMAL || this.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.CONNECTOR || this.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.HUB;
			}
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x0600528B RID: 21131 RVA: 0x001DA7E8 File Offset: 0x001D89E8
		public bool IsSecretRoom
		{
			get
			{
				return !(this.secretRoomManager == null) && !this.secretRoomManager.IsOpen;
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x0600528C RID: 21132 RVA: 0x001DA810 File Offset: 0x001D8A10
		public bool WasEverSecretRoom
		{
			get
			{
				return !(this.secretRoomManager == null);
			}
		}

		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x0600528D RID: 21133 RVA: 0x001DA828 File Offset: 0x001D8A28
		// (remove) Token: 0x0600528E RID: 21134 RVA: 0x001DA860 File Offset: 0x001D8A60
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event RoomHandler.OnBecameVisibleEventHandler BecameVisible;

		// Token: 0x0600528F RID: 21135 RVA: 0x001DA898 File Offset: 0x001D8A98
		public virtual void OnBecameVisible(PlayerController p)
		{
			if (this.m_currentlyVisible)
			{
				return;
			}
			this.m_currentlyVisible = true;
			this.visibility = RoomHandler.VisibilityStatus.CURRENT;
			float num = this.UpdateOcclusionData(p, 1f, true);
			if (this.BecameVisible != null)
			{
				this.BecameVisible(num);
			}
		}

		// Token: 0x140000A7 RID: 167
		// (add) Token: 0x06005290 RID: 21136 RVA: 0x001DA8E4 File Offset: 0x001D8AE4
		// (remove) Token: 0x06005291 RID: 21137 RVA: 0x001DA91C File Offset: 0x001D8B1C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event RoomHandler.OnBecameInvisibleEventHandler BecameInvisible;

		// Token: 0x06005292 RID: 21138 RVA: 0x001DA954 File Offset: 0x001D8B54
		public virtual void OnBecameInvisible(PlayerController p)
		{
			if (!this.m_currentlyVisible)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				if (!GameManager.Instance.AllPlayers[i].healthHaver.IsDead)
				{
					if (GameManager.Instance.AllPlayers[i].CurrentRoom == this)
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				this.m_currentlyVisible = false;
				this.visibility = RoomHandler.VisibilityStatus.VISITED;
				this.UpdateOcclusionData(0.3f, p.transform.position.IntXY(VectorConversions.Floor), true);
				if (this.BecameInvisible != null)
				{
					this.BecameInvisible();
				}
			}
		}

		// Token: 0x06005293 RID: 21139 RVA: 0x001DAA10 File Offset: 0x001D8C10
		public bool WillSealOnEntry()
		{
			List<AIActor> list = this.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear);
			bool flag;
			if (list != null)
			{
				flag = list.Exists((AIActor a) => !a.healthHaver.IsDead);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (this.area.IsProceduralRoom)
			{
				return flag2;
			}
			if (this.area.runtimePrototypeData.roomEvents != null && this.area.runtimePrototypeData.roomEvents.Count > 0)
			{
				for (int i = 0; i < this.area.runtimePrototypeData.roomEvents.Count; i++)
				{
					RoomEventDefinition roomEventDefinition = this.area.runtimePrototypeData.roomEvents[i];
					if (roomEventDefinition.condition == RoomEventTriggerCondition.ON_ENTER && roomEventDefinition.action == RoomEventTriggerAction.SEAL_ROOM)
					{
						return true;
					}
					if (flag2 && roomEventDefinition.condition == RoomEventTriggerCondition.ON_ENTER_WITH_ENEMIES && roomEventDefinition.action == RoomEventTriggerAction.SEAL_ROOM)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x001DAB14 File Offset: 0x001D8D14
		protected virtual void ProcessRoomEvents(RoomEventTriggerCondition eventCondition)
		{
			if (!this.area.IsProceduralRoom && this.area.runtimePrototypeData.roomEvents != null && this.area.runtimePrototypeData.roomEvents.Count > 0)
			{
				for (int i = 0; i < this.area.runtimePrototypeData.roomEvents.Count; i++)
				{
					RoomEventDefinition roomEventDefinition = this.area.runtimePrototypeData.roomEvents[i];
					if (roomEventDefinition.condition == eventCondition)
					{
						this.HandleRoomAction(roomEventDefinition.action);
					}
				}
			}
			else if (this.area.IsProceduralRoom)
			{
				if (eventCondition == RoomEventTriggerCondition.ON_ENTER_WITH_ENEMIES)
				{
					this.HandleRoomAction(RoomEventTriggerAction.SEAL_ROOM);
				}
				else if (eventCondition == RoomEventTriggerCondition.ON_ENEMIES_CLEARED)
				{
					this.HandleRoomAction(RoomEventTriggerAction.UNSEAL_ROOM);
				}
			}
		}

		// Token: 0x06005295 RID: 21141 RVA: 0x001DABEC File Offset: 0x001D8DEC
		public virtual void HandleRoomAction(RoomEventTriggerAction action)
		{
			switch (action)
			{
			case RoomEventTriggerAction.SEAL_ROOM:
				this.SealRoom();
				break;
			case RoomEventTriggerAction.UNSEAL_ROOM:
				this.UnsealRoom();
				break;
			case RoomEventTriggerAction.BECOME_TERRIFYING_AND_DARK:
				this.BecomeTerrifyingDarkRoom(1f, 0.1f, 1f, "Play_ENM_darken_world_01");
				break;
			case RoomEventTriggerAction.END_TERRIFYING_AND_DARK:
				this.EndTerrifyingDarkRoom(1f, 0.1f, 1f, "Play_ENM_lighten_world_01");
				break;
			default:
				BraveUtility.Log("RoomHandler received event that is triggering undefined RoomEventTriggerAction.", Color.red, BraveUtility.LogVerbosity.IMPORTANT);
				break;
			}
		}

		// Token: 0x06005296 RID: 21142 RVA: 0x001DAC80 File Offset: 0x001D8E80
		protected void PreLoadReinforcements()
		{
			if (this.area.runtimePrototypeData == null || this.area.runtimePrototypeData.additionalObjectLayers == null || this.area.runtimePrototypeData.additionalObjectLayers.Count == 0)
			{
				return;
			}
			if (this.preloadedReinforcementLayerData == null)
			{
				this.preloadedReinforcementLayerData = new Dictionary<PrototypeRoomObjectLayer, Dictionary<PrototypePlacedObjectData, GameObject>>();
			}
			int i = 0;
			int num = 0;
			while (i < this.area.runtimePrototypeData.additionalObjectLayers.Count)
			{
				PrototypeRoomObjectLayer prototypeRoomObjectLayer = this.area.runtimePrototypeData.additionalObjectLayers[i];
				if (prototypeRoomObjectLayer.layerIsReinforcementLayer)
				{
					List<Vector2> list;
					if (prototypeRoomObjectLayer.shuffle)
					{
						list = new List<Vector2>(prototypeRoomObjectLayer.placedObjectBasePositions);
						for (int j = list.Count - 1; j > 0; j--)
						{
							int num2 = UnityEngine.Random.Range(0, j + 1);
							if (j != num2)
							{
								Vector2 vector = list[j];
								list[j] = list[num2];
								list[num2] = vector;
							}
						}
					}
					else
					{
						list = prototypeRoomObjectLayer.placedObjectBasePositions;
					}
					for (int k = 0; k < prototypeRoomObjectLayer.placedObjects.Count; k++)
					{
						if (this.remainingReinforcementLayers == null || !this.remainingReinforcementLayers.Contains(prototypeRoomObjectLayer))
						{
							break;
						}
						GameObject gameObject = this.PreloadReinforcementObject(prototypeRoomObjectLayer.placedObjects[k], list[k].ToIntVector2(VectorConversions.Round), prototypeRoomObjectLayer.suppressPlayerChecks);
						if (gameObject != null)
						{
							num++;
						}
						if (!this.preloadedReinforcementLayerData.ContainsKey(prototypeRoomObjectLayer))
						{
							this.preloadedReinforcementLayerData.Add(prototypeRoomObjectLayer, new Dictionary<PrototypePlacedObjectData, GameObject>());
						}
						this.preloadedReinforcementLayerData[prototypeRoomObjectLayer].Add(prototypeRoomObjectLayer.placedObjects[k], gameObject);
					}
				}
				i++;
			}
		}

		// Token: 0x06005297 RID: 21143 RVA: 0x001DAE68 File Offset: 0x001D9068
		protected IEnumerator HandleReinforcementPreloading()
		{
			while (Time.timeSinceLevelLoad < 1f)
			{
				yield return null;
			}
			if (this.area.runtimePrototypeData == null || this.area.runtimePrototypeData.additionalObjectLayers == null || this.area.runtimePrototypeData.additionalObjectLayers.Count == 0)
			{
				yield break;
			}
			if (this.preloadedReinforcementLayerData == null)
			{
				this.preloadedReinforcementLayerData = new Dictionary<PrototypeRoomObjectLayer, Dictionary<PrototypePlacedObjectData, GameObject>>();
			}
			int targetLayerIndex = 0;
			int roomIndex = Mathf.Max(0, GameManager.Instance.Dungeon.data.rooms.IndexOf(this));
			int totalRooms = GameManager.Instance.Dungeon.data.rooms.Count;
			int nonNullInstantiations = 0;
			while (targetLayerIndex < this.area.runtimePrototypeData.additionalObjectLayers.Count)
			{
				PrototypeRoomObjectLayer currentLayer = this.area.runtimePrototypeData.additionalObjectLayers[targetLayerIndex];
				if (currentLayer.layerIsReinforcementLayer)
				{
					List<Vector2> modifiedPlacedObjectPositions = null;
					if (currentLayer.shuffle)
					{
						modifiedPlacedObjectPositions = new List<Vector2>(currentLayer.placedObjectBasePositions);
						for (int i = modifiedPlacedObjectPositions.Count - 1; i > 0; i--)
						{
							int num = UnityEngine.Random.Range(0, i + 1);
							if (i != num)
							{
								Vector2 vector = modifiedPlacedObjectPositions[i];
								modifiedPlacedObjectPositions[i] = modifiedPlacedObjectPositions[num];
								modifiedPlacedObjectPositions[num] = vector;
							}
						}
					}
					else
					{
						modifiedPlacedObjectPositions = currentLayer.placedObjectBasePositions;
					}
					for (int objectIndex = 0; objectIndex < currentLayer.placedObjects.Count; objectIndex++)
					{
						while (Time.frameCount % totalRooms != roomIndex)
						{
							yield return null;
						}
						if (this.remainingReinforcementLayers == null || !this.remainingReinforcementLayers.Contains(currentLayer))
						{
							break;
						}
						GameObject preloadedObject = this.PreloadReinforcementObject(currentLayer.placedObjects[objectIndex], modifiedPlacedObjectPositions[objectIndex].ToIntVector2(VectorConversions.Round), currentLayer.suppressPlayerChecks);
						if (preloadedObject != null)
						{
							nonNullInstantiations++;
						}
						if (!this.preloadedReinforcementLayerData.ContainsKey(currentLayer))
						{
							this.preloadedReinforcementLayerData.Add(currentLayer, new Dictionary<PrototypePlacedObjectData, GameObject>());
						}
						this.preloadedReinforcementLayerData[currentLayer].Add(currentLayer.placedObjects[objectIndex], preloadedObject);
						yield return null;
					}
				}
				targetLayerIndex++;
			}
			yield break;
		}

		// Token: 0x06005298 RID: 21144 RVA: 0x001DAE84 File Offset: 0x001D9084
		public int GetEnemiesInReinforcementLayer(int index)
		{
			if (this.remainingReinforcementLayers == null)
			{
				return 0;
			}
			if (index >= this.remainingReinforcementLayers.Count)
			{
				return 0;
			}
			return this.remainingReinforcementLayers[index].placedObjects.Count;
		}

		// Token: 0x06005299 RID: 21145 RVA: 0x001DAEBC File Offset: 0x001D90BC
		public bool TriggerReinforcementLayer(int index, bool removeLayer = true, bool disableDrops = false, int specifyObjectIndex = -1, int specifyObjectCount = -1, bool instant = false)
		{
			if (this.remainingReinforcementLayers == null || index < 0 || index >= this.remainingReinforcementLayers.Count)
			{
				return false;
			}
			PrototypeRoomObjectLayer prototypeRoomObjectLayer = this.remainingReinforcementLayers[index];
			if (removeLayer)
			{
				this.remainingReinforcementLayers.RemoveAt(index);
			}
			float activeDifficultyModifier = this.m_activeDifficultyModifier;
			this.m_activeDifficultyModifier = 1f;
			int num = ((this.activeEnemies != null) ? this.activeEnemies.Count : 0);
			List<GameObject> list = this.PlaceObjectsFromLayer(prototypeRoomObjectLayer.placedObjects, prototypeRoomObjectLayer, prototypeRoomObjectLayer.placedObjectBasePositions, null, !instant, prototypeRoomObjectLayer.shuffle, prototypeRoomObjectLayer.randomize, prototypeRoomObjectLayer.suppressPlayerChecks, disableDrops, specifyObjectIndex, specifyObjectCount);
			bool flag = this.activeEnemies.Count > num;
			if (activeDifficultyModifier != 1f)
			{
				this.MakeRoomMoreDifficult(activeDifficultyModifier, list);
			}
			if (GameManager.Instance.DungeonMusicController.CurrentState != DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_A && GameManager.Instance.DungeonMusicController.CurrentState != DungeonFloorMusicController.DungeonMusicState.ACTIVE_SIDE_B && !GameManager.Instance.DungeonMusicController.MusicOverridden)
			{
				GameManager.Instance.DungeonMusicController.NotifyRoomSuddenlyHasEnemies(this);
			}
			this.ResetEnemyHPPercentage();
			return flag;
		}

		// Token: 0x0600529A RID: 21146 RVA: 0x001DAFE8 File Offset: 0x001D91E8
		public void TriggerNextReinforcementLayer()
		{
			if (this.remainingReinforcementLayers != null && this.remainingReinforcementLayers.Count > 0)
			{
				this.TriggerReinforcementLayer(0, true, false, -1, -1, false);
			}
		}

		// Token: 0x0600529B RID: 21147 RVA: 0x001DB014 File Offset: 0x001D9214
		public void ClearReinforcementLayers()
		{
			this.remainingReinforcementLayers = null;
		}

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x0600529C RID: 21148 RVA: 0x001DB020 File Offset: 0x001D9220
		public List<SpeculativeRigidbody> RoomMovingPlatforms
		{
			get
			{
				return this.m_roomMovingPlatforms;
			}
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x0600529D RID: 21149 RVA: 0x001DB028 File Offset: 0x001D9228
		public List<DeadlyDeadlyGoopManager> RoomGoops
		{
			get
			{
				return this.m_goops;
			}
		}

		// Token: 0x0600529E RID: 21150 RVA: 0x001DB030 File Offset: 0x001D9230
		public void RegisterGoopManagerInRoom(DeadlyDeadlyGoopManager manager)
		{
			if (this.m_goops == null)
			{
				this.m_goops = new List<DeadlyDeadlyGoopManager>();
			}
			if (!this.m_goops.Contains(manager))
			{
				this.m_goops.Add(manager);
			}
		}

		// Token: 0x0600529F RID: 21151 RVA: 0x001DB068 File Offset: 0x001D9268
		public RoomHandler GetRandomDownstreamRoom()
		{
			List<RoomHandler> list = new List<RoomHandler>();
			for (int i = 0; i < this.connectedRooms.Count; i++)
			{
				if (this.connectedRooms[i].distanceFromEntrance > this.distanceFromEntrance)
				{
					list.Add(this.connectedRooms[i]);
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list[UnityEngine.Random.Range(0, list.Count)];
		}

		// Token: 0x060052A0 RID: 21152 RVA: 0x001DB0E4 File Offset: 0x001D92E4
		public HashSet<IntVector2> GetCellsAndAllConnectedExitsSlow(bool includeSecret = false)
		{
			HashSet<IntVector2> hashSet = new HashSet<IntVector2>(this.RawCells);
			List<IntVector2> list = new List<IntVector2>();
			if (this.area != null && this.area.instanceUsedExits != null)
			{
				for (int i = 0; i < this.area.instanceUsedExits.Count; i++)
				{
					RuntimeRoomExitData runtimeRoomExitData;
					RuntimeExitDefinition runtimeExitDefinition;
					if (this.area.exitToLocalDataMap.TryGetValue(this.area.instanceUsedExits[i], out runtimeRoomExitData) && this.exitDefinitionsByExit.TryGetValue(runtimeRoomExitData, out runtimeExitDefinition))
					{
						if (runtimeExitDefinition != null && ((!runtimeExitDefinition.downstreamRoom.IsSecretRoom && !runtimeExitDefinition.upstreamRoom.IsSecretRoom) || includeSecret))
						{
							HashSet<IntVector2> cellsForRoom = runtimeExitDefinition.GetCellsForRoom(this);
							HashSet<IntVector2> cellsForOtherRoom = runtimeExitDefinition.GetCellsForOtherRoom(this);
							foreach (IntVector2 intVector in cellsForRoom)
							{
								hashSet.Add(intVector);
							}
							foreach (IntVector2 intVector2 in cellsForOtherRoom)
							{
								hashSet.Add(intVector2);
							}
						}
					}
				}
			}
			DungeonData data = GameManager.Instance.BestGenerationDungeonPrefab.data;
			foreach (IntVector2 intVector3 in hashSet)
			{
				if (data[intVector3] != null && data[intVector3].isWallMimicHideout)
				{
					list.Add(intVector3);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				hashSet.Remove(list[j]);
			}
			return hashSet;
		}

		// Token: 0x060052A1 RID: 21153 RVA: 0x001DB304 File Offset: 0x001D9504
		private List<Tuple<IntVector2, float>> GetGoodSpotsInternal(int dx, int dy, bool restrictive = false)
		{
			List<Tuple<IntVector2, float>> list = new List<Tuple<IntVector2, float>>();
			for (int i = 0; i < this.CellsWithoutExits.Count; i++)
			{
				bool flag = true;
				CellData cellData = GameManager.Instance.Dungeon.data[this.CellsWithoutExits[i]];
				float num = 0f;
				for (int j = 0; j < dx; j++)
				{
					for (int k = 0; k < dy; k++)
					{
						CellData cellData2 = GameManager.Instance.Dungeon.data[cellData.position + new IntVector2(j, k)];
						if (cellData2.IsTopWall())
						{
							num -= 5f;
						}
						if (cellData2.HasWallNeighbor(true, true))
						{
							num -= 2f;
						}
						else
						{
							num += 2f;
						}
						if (GameManager.Instance.Dungeon.data[cellData.position + new IntVector2(j, k - 1)].type == CellType.PIT)
						{
							num -= 50f;
						}
						if (cellData2.type != CellType.FLOOR || cellData2.isOccupied)
						{
							flag = false;
							break;
						}
						if (restrictive && (cellData2.doesDamage || cellData2.cellVisualData.IsPhantomCarpet || cellData2.containsTrap))
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
				}
				int num2 = Math.Abs(this.area.basePosition.x + this.area.dimensions.x - (cellData.position.x + dx / 2) - (cellData.position.x + dx / 2 - this.area.basePosition.x));
				int num3 = Math.Abs(this.area.basePosition.y + this.area.dimensions.y - (cellData.position.y + dy / 2) - (cellData.position.y + dy / 2 - this.area.basePosition.y));
				if (num2 <= 3 && num3 <= 3)
				{
					num += 10f;
				}
				else if (num2 <= 5 && num3 <= 5)
				{
					num += 5f;
				}
				if (flag)
				{
					float num4 = 1f + num;
					Tuple<IntVector2, float> tuple = new Tuple<IntVector2, float>(cellData.position, num4);
					list.Add(tuple);
				}
			}
			return list;
		}

		// Token: 0x060052A2 RID: 21154 RVA: 0x001DB5A4 File Offset: 0x001D97A4
		public IntVector2 GetRandomVisibleClearSpot(int dx, int dy)
		{
			List<Tuple<IntVector2, float>> goodSpotsInternal = this.GetGoodSpotsInternal(dx, dy, false);
			if (goodSpotsInternal.Count == 0)
			{
				return IntVector2.Zero;
			}
			return goodSpotsInternal[UnityEngine.Random.Range(0, goodSpotsInternal.Count)].First;
		}

		// Token: 0x060052A3 RID: 21155 RVA: 0x001DB5E4 File Offset: 0x001D97E4
		public IntVector2 GetCenteredVisibleClearSpot(int dx, int dy, out bool success, bool restrictive = false)
		{
			List<Tuple<IntVector2, float>> goodSpotsInternal = this.GetGoodSpotsInternal(dx, dy, restrictive);
			float num = float.MinValue;
			IntVector2 intVector = this.Epicenter;
			success = false;
			for (int i = 0; i < goodSpotsInternal.Count; i++)
			{
				if (goodSpotsInternal[i].Second > num)
				{
					intVector = goodSpotsInternal[i].First;
					num = goodSpotsInternal[i].Second;
					success = true;
				}
			}
			return intVector;
		}

		// Token: 0x060052A4 RID: 21156 RVA: 0x001DB654 File Offset: 0x001D9854
		public IntVector2 GetCenteredVisibleClearSpot(int dx, int dy)
		{
			bool flag = false;
			return this.GetCenteredVisibleClearSpot(dx, dy, out flag, false);
		}

		// Token: 0x060052A5 RID: 21157 RVA: 0x001DB670 File Offset: 0x001D9870
		protected virtual void HandleBossClearReward()
		{
			if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.SHORTCUT)
			{
				GameStatsManager.Instance.CurrentResRatShopSeed = UnityEngine.Random.Range(1, 1000000);
			}
			GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
			if (!this.PlayerHasTakenDamageInThisRoom && GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.NONE)
			{
				if (tilesetId != GlobalDungeonData.ValidTilesets.GUNGEON)
				{
					if (tilesetId != GlobalDungeonData.ValidTilesets.CASTLEGEON)
					{
						if (tilesetId != GlobalDungeonData.ValidTilesets.MINEGEON)
						{
							if (tilesetId != GlobalDungeonData.ValidTilesets.CATACOMBGEON)
							{
								if (tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
								{
									GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_FORGE, true);
								}
							}
							else
							{
								GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_HOLLOW, true);
							}
						}
						else
						{
							GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_MINES, true);
						}
					}
					else
					{
						GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_CASTLE, true);
					}
				}
				else
				{
					GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_GUNGEON, true);
				}
			}
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST)
			{
				return;
			}
			if (tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON)
			{
				return;
			}
			if (tilesetId == GlobalDungeonData.ValidTilesets.RATGEON)
			{
				return;
			}
			for (int i = 0; i < this.connectedRooms.Count; i++)
			{
				if (this.connectedRooms[i].area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.EXIT)
				{
					this.connectedRooms[i].OnBecameVisible(GameManager.Instance.BestActivePlayer);
				}
			}
			IntVector2 intVector = IntVector2.Zero;
			if (this.OverrideBossPedestalLocation != null)
			{
				intVector = this.OverrideBossPedestalLocation.Value;
			}
			else if (!this.area.IsProceduralRoom && this.area.runtimePrototypeData.rewardChestSpawnPosition != IntVector2.NegOne)
			{
				intVector = this.area.basePosition + this.area.runtimePrototypeData.rewardChestSpawnPosition;
			}
			else
			{
				UnityEngine.Debug.LogWarning("BOSS REWARD PEDESTALS SHOULD REALLY HAVE FIXED LOCATIONS! The spawn code was written by dave, so no guarantees...");
				intVector = this.GetCenteredVisibleClearSpot(2, 2);
			}
			GameObject gameObject = GameManager.Instance.Dungeon.sharedSettingsPrefab.ChestsForBosses.SelectByWeight();
			Chest chest = gameObject.GetComponent<Chest>();
			bool isRainbowRun = GameStatsManager.Instance.IsRainbowRun;
			if (isRainbowRun)
			{
				chest = null;
			}
			if (chest != null)
			{
				Chest chest2 = Chest.Spawn(chest, intVector, this, false);
				chest2.RegisterChestOnMinimap(this);
			}
			else
			{
				DungeonData data = GameManager.Instance.Dungeon.data;
				RewardPedestal component = gameObject.GetComponent<RewardPedestal>();
				if (component)
				{
					bool flag = tilesetId != GlobalDungeonData.ValidTilesets.FORGEGEON;
					bool flag2 = !this.PlayerHasTakenDamageInThisRoom && GameManager.Instance.Dungeon.BossMasteryTokenItemId >= 0 && !GameManager.Instance.Dungeon.HasGivenMasteryToken;
					if (flag && flag2)
					{
						intVector += IntVector2.Left;
					}
					if (flag)
					{
						RewardPedestal rewardPedestal = RewardPedestal.Spawn(component, intVector, this);
						rewardPedestal.IsBossRewardPedestal = true;
						rewardPedestal.lootTable.lootTable = this.OverrideBossRewardTable;
						rewardPedestal.RegisterChestOnMinimap(this);
						data[intVector].isOccupied = true;
						data[intVector + IntVector2.Right].isOccupied = true;
						data[intVector + IntVector2.Up].isOccupied = true;
						data[intVector + IntVector2.One].isOccupied = true;
						if (flag2)
						{
							rewardPedestal.OffsetTertiarySet = true;
						}
						if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.NumberOfLivingPlayers == 1)
						{
							rewardPedestal.ReturnCoopPlayerOnLand = true;
						}
						if (this.area.PrototypeRoomName == "DoubleBeholsterRoom01")
						{
							for (int j = 0; j < 8; j++)
							{
								IntVector2 centeredVisibleClearSpot = this.GetCenteredVisibleClearSpot(2, 2);
								RewardPedestal rewardPedestal2 = RewardPedestal.Spawn(component, centeredVisibleClearSpot, this);
								rewardPedestal2.IsBossRewardPedestal = true;
								rewardPedestal2.lootTable.lootTable = this.OverrideBossRewardTable;
								data[centeredVisibleClearSpot].isOccupied = true;
								data[centeredVisibleClearSpot + IntVector2.Right].isOccupied = true;
								data[centeredVisibleClearSpot + IntVector2.Up].isOccupied = true;
								data[centeredVisibleClearSpot + IntVector2.One].isOccupied = true;
							}
						}
					}
					else if (tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.NumberOfLivingPlayers == 1)
					{
						PlayerController playerController = ((!GameManager.Instance.PrimaryPlayer.healthHaver.IsDead) ? GameManager.Instance.SecondaryPlayer : GameManager.Instance.PrimaryPlayer);
						playerController.specRigidbody.enabled = true;
						playerController.gameObject.SetActive(true);
						playerController.ResurrectFromBossKill();
					}
					if (flag2)
					{
						GameStatsManager.Instance.RegisterStatChange(TrackedStats.MASTERY_TOKENS_RECEIVED, 1f);
						GameManager.Instance.PrimaryPlayer.MasteryTokensCollectedThisRun++;
						if (flag)
						{
							intVector += new IntVector2(2, 0);
						}
						RewardPedestal rewardPedestal3 = RewardPedestal.Spawn(component, intVector, this);
						data[intVector].isOccupied = true;
						data[intVector + IntVector2.Right].isOccupied = true;
						data[intVector + IntVector2.Up].isOccupied = true;
						data[intVector + IntVector2.One].isOccupied = true;
						GameManager.Instance.Dungeon.HasGivenMasteryToken = true;
						rewardPedestal3.SpawnsTertiarySet = false;
						rewardPedestal3.contents = PickupObjectDatabase.GetById(GameManager.Instance.Dungeon.BossMasteryTokenItemId);
						rewardPedestal3.MimicGuid = null;
					}
				}
				if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CATHEDRALGEON && GameManager.Options.CurrentGameLootProfile == GameOptions.GameLootProfile.CURRENT)
				{
					IntVector2? randomAvailableCell = this.GetRandomAvailableCell(new IntVector2?(IntVector2.One * 4), new CellTypes?(CellTypes.FLOOR), false, null);
					IntVector2? intVector2 = ((randomAvailableCell == null) ? null : new IntVector2?(randomAvailableCell.GetValueOrDefault() + IntVector2.One));
					if (intVector2 != null)
					{
						Chest chest3 = Chest.Spawn(GameManager.Instance.RewardManager.Synergy_Chest, intVector2.Value);
						if (chest3)
						{
							chest3.RegisterChestOnMinimap(this);
						}
					}
				}
			}
		}

		// Token: 0x060052A6 RID: 21158 RVA: 0x001DBCEC File Offset: 0x001D9EEC
		public Chest SpawnRoomRewardChest(WeightedGameObjectCollection chestCollection, IntVector2 pos)
		{
			Chest chest;
			if (chestCollection != null)
			{
				GameObject gameObject = chestCollection.SelectByWeight();
				chest = Chest.Spawn(gameObject.GetComponent<Chest>(), pos, this, false);
			}
			else
			{
				chest = GameManager.Instance.RewardManager.SpawnRoomClearChestAt(pos);
			}
			if (chest != null)
			{
				chest.RegisterChestOnMinimap(this);
			}
			return chest;
		}

		// Token: 0x060052A7 RID: 21159 RVA: 0x001DBD44 File Offset: 0x001D9F44
		public IntVector2 GetBestRewardLocation(IntVector2 rewardSize, RoomHandler.RewardLocationStyle locationStyle = RoomHandler.RewardLocationStyle.CameraCenter, bool giveChestBuffer = true)
		{
			Vector2 vector;
			if (locationStyle == RoomHandler.RewardLocationStyle.CameraCenter && !GameManager.Instance.InTutorial)
			{
				vector = BraveUtility.ScreenCenterWorldPoint();
			}
			else if (locationStyle == RoomHandler.RewardLocationStyle.CameraCenter && !GameManager.Instance.InTutorial)
			{
				if (!GameManager.Instance.PrimaryPlayer.healthHaver.IsDead)
				{
					vector = GameManager.Instance.PrimaryPlayer.CenterPosition;
				}
				else if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && !GameManager.Instance.SecondaryPlayer.healthHaver.IsDead)
				{
					vector = GameManager.Instance.SecondaryPlayer.CenterPosition;
				}
				else
				{
					vector = BraveUtility.ScreenCenterWorldPoint();
				}
			}
			else if (locationStyle == RoomHandler.RewardLocationStyle.PlayerCenter)
			{
				vector = ((!GameManager.Instance.PrimaryPlayer.healthHaver.IsDead || GameManager.Instance.CurrentGameType != GameManager.GameType.COOP_2_PLAYER) ? GameManager.Instance.PrimaryPlayer.CenterPosition : GameManager.Instance.SecondaryPlayer.CenterPosition);
			}
			else if (!this.area.IsProceduralRoom && this.area.runtimePrototypeData != null && this.area.runtimePrototypeData.rewardChestSpawnPosition != IntVector2.NegOne)
			{
				vector = (this.area.basePosition + this.area.runtimePrototypeData.rewardChestSpawnPosition).ToVector2() + rewardSize.ToVector2() / 2f;
			}
			else if (!this.area.IsProceduralRoom && this.area.prototypeRoom != null && this.area.prototypeRoom.rewardChestSpawnPosition != IntVector2.NegOne)
			{
				vector = (this.area.basePosition + this.area.prototypeRoom.rewardChestSpawnPosition).ToVector2() + rewardSize.ToVector2() / 2f;
			}
			else
			{
				vector = this.area.basePosition.ToVector2() + this.area.dimensions.ToVector2() / 2f;
			}
			return this.GetBestRewardLocation(rewardSize, vector, giveChestBuffer);
		}

		// Token: 0x060052A8 RID: 21160 RVA: 0x001DBFA8 File Offset: 0x001DA1A8
		public IntVector2 GetBestRewardLocation(IntVector2 rewardSize, Vector2 idealPoint, bool giveChestBuffer = true)
		{
			IntVector2[] playerPos = new IntVector2[GameManager.Instance.AllPlayers.Length];
			IntVector2[] playerDim = new IntVector2[playerPos.Length];
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				PixelCollider hitboxPixelCollider = GameManager.Instance.AllPlayers[i].specRigidbody.HitboxPixelCollider;
				playerPos[i] = hitboxPixelCollider.UnitBottomLeft.ToIntVector2(VectorConversions.Floor);
				playerDim[i] = hitboxPixelCollider.UnitTopRight.ToIntVector2(VectorConversions.Floor) - playerPos[i] + IntVector2.One;
			}
			IntVector2 modifiedRewardSize = ((!giveChestBuffer) ? rewardSize : (rewardSize + new IntVector2(2, 2)));
			CellValidator cellValidator = delegate(IntVector2 c)
			{
				IntVector2 intVector3 = ((!giveChestBuffer) ? c : (c + new IntVector2(1, 2)));
				for (int j = 0; j < playerPos.Length; j++)
				{
					if (IntVector2.AABBOverlap(playerPos[j], playerDim[j], intVector3, rewardSize))
					{
						return false;
					}
				}
				for (int k = 0; k < modifiedRewardSize.x; k++)
				{
					if (GameManager.Instance.Dungeon.data.isTopWall(c.x + k, c.y))
					{
						return false;
					}
					for (int l = 0; l < modifiedRewardSize.y; l++)
					{
						if (!GameManager.Instance.Dungeon.data.CheckInBounds(c.x + k, c.y + l))
						{
							return false;
						}
						if (GameManager.Instance.Dungeon.data.isWall(c.x + k, c.y + l))
						{
							return false;
						}
						CellData cellData = GameManager.Instance.Dungeon.data.cellData[c.x + k][c.y + l];
						if (cellData.containsTrap || cellData.PreventRewardSpawn)
						{
							return false;
						}
					}
				}
				return true;
			};
			IntVector2? intVector = this.GetNearestAvailableCell(idealPoint, new IntVector2?(modifiedRewardSize), new CellTypes?(CellTypes.FLOOR), false, cellValidator);
			if (intVector != null)
			{
				if (giveChestBuffer)
				{
					intVector = new IntVector2?(intVector.Value + new IntVector2(1, 2));
				}
				return intVector.Value;
			}
			IntVector2 intVector2 = IntVector2.Zero;
			if (!this.area.IsProceduralRoom && this.area.runtimePrototypeData != null && this.area.runtimePrototypeData.rewardChestSpawnPosition != IntVector2.NegOne)
			{
				intVector2 = this.area.basePosition + this.area.runtimePrototypeData.rewardChestSpawnPosition;
			}
			else if (!this.area.IsProceduralRoom && this.area.prototypeRoom != null && this.area.prototypeRoom.rewardChestSpawnPosition != IntVector2.NegOne)
			{
				intVector2 = this.area.basePosition + this.area.prototypeRoom.rewardChestSpawnPosition;
			}
			else
			{
				intVector2 = this.GetCenteredVisibleClearSpot(3, 2);
			}
			return intVector2;
		}

		// Token: 0x060052A9 RID: 21161 RVA: 0x001DC1F8 File Offset: 0x001DA3F8
		public virtual void HandleRoomClearReward()
		{
			if (GameManager.Instance.IsFoyer || GameManager.Instance.InTutorial)
			{
				return;
			}
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST)
			{
				return;
			}
			if (this.m_hasGivenReward)
			{
				return;
			}
			if (this.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.REWARD)
			{
				return;
			}
			this.m_hasGivenReward = true;
			if (this.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS && this.area.PrototypeRoomBossSubcategory == PrototypeDungeonRoom.RoomBossSubCategory.FLOOR_BOSS)
			{
				this.HandleBossClearReward();
				return;
			}
			if (this.PreventStandardRoomReward)
			{
				return;
			}
			FloorRewardData currentRewardData = GameManager.Instance.RewardManager.CurrentRewardData;
			LootEngine.AmmoDropType ammoDropType = LootEngine.AmmoDropType.DEFAULT_AMMO;
			bool flag = LootEngine.DoAmmoClipCheck(currentRewardData, out ammoDropType);
			string text = ((ammoDropType != LootEngine.AmmoDropType.SPREAD_AMMO) ? "Ammo_Pickup" : "Ammo_Pickup_Spread");
			float value = UnityEngine.Random.value;
			float num = currentRewardData.ChestSystem_ChestChanceLowerBound;
			float num2 = GameManager.Instance.PrimaryPlayer.stats.GetStatValue(PlayerStats.StatType.Coolness) / 100f;
			float num3 = -(GameManager.Instance.PrimaryPlayer.stats.GetStatValue(PlayerStats.StatType.Curse) / 100f);
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				num2 += GameManager.Instance.SecondaryPlayer.stats.GetStatValue(PlayerStats.StatType.Coolness) / 100f;
				num3 -= GameManager.Instance.SecondaryPlayer.stats.GetStatValue(PlayerStats.StatType.Curse) / 100f;
			}
			if (PassiveItem.IsFlagSetAtAll(typeof(ChamberOfEvilItem)))
			{
				num3 *= -2f;
			}
			num = Mathf.Clamp(num + GameManager.Instance.PrimaryPlayer.AdditionalChestSpawnChance, currentRewardData.ChestSystem_ChestChanceLowerBound, currentRewardData.ChestSystem_ChestChanceUpperBound) + num2 + num3;
			bool flag2 = currentRewardData.SingleItemRewardTable != null;
			bool flag3 = false;
			float num4 = 0.1f;
			if (!RoomHandler.HasGivenRoomChestRewardThisRun && MetaInjectionData.ForceEarlyChest)
			{
				flag3 = true;
			}
			if (flag3)
			{
				if (!RoomHandler.HasGivenRoomChestRewardThisRun && (GameManager.Instance.CurrentFloor == 1 || GameManager.Instance.CurrentFloor == -1))
				{
					flag2 = false;
					num += num4;
					if (GameManager.Instance.PrimaryPlayer && GameManager.Instance.PrimaryPlayer.NumRoomsCleared > 4)
					{
						num = 1f;
					}
				}
				if (!RoomHandler.HasGivenRoomChestRewardThisRun && this.distanceFromEntrance < RoomHandler.NumberOfRoomsToPreventChestSpawning)
				{
					GameManager.Instance.Dungeon.InformRoomCleared(false, false);
					return;
				}
			}
			BraveUtility.Log("Current chest spawn chance: " + num, Color.yellow, BraveUtility.LogVerbosity.IMPORTANT);
			if (value > num)
			{
				if (flag)
				{
					IntVector2 intVector = this.GetBestRewardLocation(new IntVector2(1, 1), RoomHandler.RewardLocationStyle.CameraCenter, true);
					LootEngine.SpawnItem((GameObject)BraveResources.Load(text, ".prefab"), intVector.ToVector3(), Vector2.up, 1f, true, true, false);
				}
				GameManager.Instance.Dungeon.InformRoomCleared(false, false);
				return;
			}
			if (flag2)
			{
				float num5 = currentRewardData.PercentOfRoomClearRewardsThatAreChests;
				if (PassiveItem.IsFlagSetAtAll(typeof(AmazingChestAheadItem)))
				{
					num5 *= 2f;
					num5 = Mathf.Max(0.5f, num5);
				}
				flag2 = UnityEngine.Random.value > num5;
			}
			if (flag2)
			{
				float num6 = ((GameManager.Instance.CurrentGameType != GameManager.GameType.COOP_2_PLAYER) ? GameManager.Instance.RewardManager.SinglePlayerPickupIncrementModifier : GameManager.Instance.RewardManager.CoopPickupIncrementModifier);
				GameObject gameObject;
				if (UnityEngine.Random.value < 1f / num6)
				{
					gameObject = currentRewardData.SingleItemRewardTable.SelectByWeight(false);
				}
				else
				{
					gameObject = ((UnityEngine.Random.value >= 0.9f) ? GameManager.Instance.RewardManager.FullHeartPrefab.gameObject : GameManager.Instance.RewardManager.HalfHeartPrefab.gameObject);
				}
				UnityEngine.Debug.Log(gameObject.name + "SPAWNED");
				DebrisObject debrisObject = LootEngine.SpawnItem(gameObject, this.GetBestRewardLocation(new IntVector2(1, 1), RoomHandler.RewardLocationStyle.CameraCenter, true).ToVector3() + new Vector3(0.25f, 0f, 0f), Vector2.up, 1f, true, true, false);
				Exploder.DoRadialPush(debrisObject.sprite.WorldCenter.ToVector3ZUp(debrisObject.sprite.WorldCenter.y), 8f, 3f);
				AkSoundEngine.PostEvent("Play_OBJ_item_spawn_01", debrisObject.gameObject);
				GameManager.Instance.Dungeon.InformRoomCleared(true, false);
			}
			else
			{
				IntVector2 intVector = this.GetBestRewardLocation(new IntVector2(2, 1), RoomHandler.RewardLocationStyle.CameraCenter, true);
				bool isRainbowRun = GameStatsManager.Instance.IsRainbowRun;
				if (isRainbowRun)
				{
					LootEngine.SpawnBowlerNote(GameManager.Instance.RewardManager.BowlerNoteChest, intVector.ToCenterVector2(), this, true);
					RoomHandler.HasGivenRoomChestRewardThisRun = true;
				}
				else
				{
					Chest chest = this.SpawnRoomRewardChest(null, intVector);
					if (chest)
					{
						RoomHandler.HasGivenRoomChestRewardThisRun = true;
					}
				}
				GameManager.Instance.Dungeon.InformRoomCleared(true, true);
			}
			if (flag)
			{
				IntVector2 intVector = this.GetBestRewardLocation(new IntVector2(1, 1), RoomHandler.RewardLocationStyle.CameraCenter, true);
				LootEngine.DelayedSpawnItem(1f, (GameObject)BraveResources.Load(text, ".prefab"), intVector.ToVector3() + new Vector3(0.25f, 0f, 0f), Vector2.up, 1f, true, true, false);
			}
		}

		// Token: 0x060052AA RID: 21162 RVA: 0x001DC764 File Offset: 0x001DA964
		protected virtual void NotifyPlayerRoomCleared()
		{
			if (GameManager.Instance == null || GameManager.Instance.PrimaryPlayer == null)
			{
				return;
			}
			GameManager.Instance.PrimaryPlayer.OnRoomCleared();
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.SecondaryPlayer)
			{
				GameManager.Instance.SecondaryPlayer.OnRoomCleared();
			}
		}

		// Token: 0x060052AB RID: 21163 RVA: 0x001DC7DC File Offset: 0x001DA9DC
		public void AssignRoomVisualType(int type, bool respectPrototypeRooms = false)
		{
			if (respectPrototypeRooms && this.area != null && this.area.prototypeRoom != null && this.area.prototypeRoom.overrideRoomVisualType > -1 && !this.area.prototypeRoom.overrideRoomVisualTypeForSecretRooms)
			{
				return;
			}
			this.RoomVisualSubtype = type;
		}

		// Token: 0x060052AC RID: 21164 RVA: 0x001DC844 File Offset: 0x001DAA44
		public void CalculateOpulence()
		{
			if (this.area.prototypeRoom != null && (this.area.prototypeRoom.category == PrototypeDungeonRoom.RoomCategory.BOSS || this.area.prototypeRoom.category == PrototypeDungeonRoom.RoomCategory.REWARD || this.area.prototypeRoom.category == PrototypeDungeonRoom.RoomCategory.SPECIAL))
			{
				this.opulence++;
			}
			if (this.distanceFromEntrance > 12)
			{
				this.opulence++;
			}
		}

		// Token: 0x060052AD RID: 21165 RVA: 0x001DC8D4 File Offset: 0x001DAAD4
		public RoomEventTriggerArea GetEventTriggerAreaFromObject(IEventTriggerable triggerable)
		{
			for (int i = 0; i < this.eventTriggerAreas.Count; i++)
			{
				RoomEventTriggerArea roomEventTriggerArea = this.eventTriggerAreas[i];
				if (roomEventTriggerArea.events.Contains(triggerable))
				{
					return roomEventTriggerArea;
				}
			}
			return null;
		}

		// Token: 0x060052AE RID: 21166 RVA: 0x001DC920 File Offset: 0x001DAB20
		public void RegisterConnectedRoom(RoomHandler other, RuntimeRoomExitData usedExit)
		{
			this.area.instanceUsedExits.Add(usedExit.referencedExit);
			this.area.exitToLocalDataMap.Add(usedExit.referencedExit, usedExit);
			this.connectedRooms.Add(other);
			this.connectedRoomsByExit.Add(usedExit.referencedExit, other);
		}

		// Token: 0x060052AF RID: 21167 RVA: 0x001DC978 File Offset: 0x001DAB78
		public void DeregisterConnectedRoom(RoomHandler other, RuntimeRoomExitData usedExit)
		{
			this.area.instanceUsedExits.Remove(usedExit.referencedExit);
			this.area.exitToLocalDataMap.Remove(usedExit.referencedExit);
			this.connectedRooms.Remove(other);
			this.connectedRoomsByExit.Remove(usedExit.referencedExit);
		}

		// Token: 0x060052B0 RID: 21168 RVA: 0x001DC9D4 File Offset: 0x001DABD4
		public DungeonData.Direction GetDirectionToConnectedRoom(RoomHandler other)
		{
			if (this.area.IsProceduralRoom)
			{
				PrototypeRoomExit exitConnectedToRoom = other.GetExitConnectedToRoom(this);
				return (exitConnectedToRoom.exitDirection + 4) % (DungeonData.Direction)8;
			}
			PrototypeRoomExit exitConnectedToRoom2 = this.GetExitConnectedToRoom(other);
			return exitConnectedToRoom2.exitDirection;
		}

		// Token: 0x060052B1 RID: 21169 RVA: 0x001DCA14 File Offset: 0x001DAC14
		public void TransferInteractableOwnershipToDungeon(IPlayerInteractable ixable)
		{
			this.DeregisterInteractable(ixable);
			RoomHandler.unassignedInteractableObjects.Remove(ixable);
			RoomHandler.unassignedInteractableObjects.Add(ixable);
		}

		// Token: 0x060052B2 RID: 21170 RVA: 0x001DCA34 File Offset: 0x001DAC34
		public void RegisterInteractable(IPlayerInteractable ixable)
		{
			if (!this.interactableObjects.Contains(ixable))
			{
				this.interactableObjects.Add(ixable);
			}
		}

		// Token: 0x060052B3 RID: 21171 RVA: 0x001DCA54 File Offset: 0x001DAC54
		public bool IsRegistered(IPlayerInteractable ixable)
		{
			return this.interactableObjects.Contains(ixable);
		}

		// Token: 0x060052B4 RID: 21172 RVA: 0x001DCA64 File Offset: 0x001DAC64
		public void DeregisterInteractable(IPlayerInteractable ixable)
		{
			if (this.interactableObjects.Contains(ixable))
			{
				this.interactableObjects.Remove(ixable);
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					GameManager.Instance.AllPlayers[i].RemoveBrokenInteractable(ixable);
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Deregistering an unregistered interactable. Talk to Brent.");
			}
		}

		// Token: 0x060052B5 RID: 21173 RVA: 0x001DCAD0 File Offset: 0x001DACD0
		public void RegisterAutoAimTarget(IAutoAimTarget target)
		{
			if (!this.autoAimTargets.Contains(target))
			{
				this.autoAimTargets.Add(target);
			}
		}

		// Token: 0x060052B6 RID: 21174 RVA: 0x001DCAF0 File Offset: 0x001DACF0
		public List<IAutoAimTarget> GetAutoAimTargets()
		{
			return this.autoAimTargets;
		}

		// Token: 0x060052B7 RID: 21175 RVA: 0x001DCAF8 File Offset: 0x001DACF8
		public void DeregisterAutoAimTarget(IAutoAimTarget target)
		{
			if (this.autoAimTargets.Contains(target))
			{
				this.autoAimTargets.Remove(target);
			}
		}

		// Token: 0x060052B8 RID: 21176 RVA: 0x001DCB18 File Offset: 0x001DAD18
		public List<T> GetComponentsInRoom<T>() where T : Behaviour
		{
			T[] array = UnityEngine.Object.FindObjectsOfType<T>();
			List<T> list = new List<T>();
			for (int i = 0; i < array.Length; i++)
			{
				if (GameManager.Instance.Dungeon.GetRoomFromPosition(array[i].transform.position.IntXY(VectorConversions.Floor)) == this)
				{
					list.Add(array[i]);
				}
			}
			return list;
		}

		// Token: 0x060052B9 RID: 21177 RVA: 0x001DCB88 File Offset: 0x001DAD88
		public List<T> GetComponentsAbsoluteInRoom<T>() where T : Behaviour
		{
			T[] array = UnityEngine.Object.FindObjectsOfType<T>();
			List<T> list = new List<T>();
			for (int i = 0; i < array.Length; i++)
			{
				if (GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(array[i].transform.position.IntXY(VectorConversions.Floor)) == this)
				{
					list.Add(array[i]);
				}
			}
			return list;
		}

		// Token: 0x060052BA RID: 21178 RVA: 0x001DCBFC File Offset: 0x001DADFC
		public void MakeRoomMoreDifficult(float difficultyMultiplier, List<GameObject> sourceObjects = null)
		{
			if (this.activeEnemies == null || this.activeEnemies.Count == 0)
			{
				return;
			}
			this.m_activeDifficultyModifier *= difficultyMultiplier;
			if (difficultyMultiplier > 1f)
			{
				List<AIActor> list;
				if (sourceObjects != null)
				{
					list = new List<AIActor>();
					for (int i = 0; i < sourceObjects.Count; i++)
					{
						AIActor component = sourceObjects[i].GetComponent<AIActor>();
						if (component)
						{
							list.Add(component);
						}
					}
				}
				else
				{
					list = new List<AIActor>(this.activeEnemies);
				}
				list = list.Shuffle<AIActor>();
				int num = Mathf.FloorToInt((float)list.Count * difficultyMultiplier);
				num -= list.Count;
				for (int j = 0; j < num; j++)
				{
					AIActor enemyToDuplicate = list[j % list.Count];
					IntVector2? targetCenter = null;
					if (enemyToDuplicate.TargetRigidbody)
					{
						targetCenter = new IntVector2?(enemyToDuplicate.TargetRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor));
					}
					CellValidator cellValidator = delegate(IntVector2 c)
					{
						for (int k = 0; k < enemyToDuplicate.Clearance.x; k++)
						{
							for (int l = 0; l < enemyToDuplicate.Clearance.y; l++)
							{
								if (GameManager.Instance.Dungeon.data.isTopWall(c.x + k, c.y + l))
								{
									return false;
								}
								if (targetCenter != null && IntVector2.DistanceSquared(targetCenter.Value, c.x + k, c.y + l) < 16f)
								{
									return false;
								}
							}
						}
						return true;
					};
					IntVector2? randomAvailableCell = this.GetRandomAvailableCell(new IntVector2?(enemyToDuplicate.Clearance), new CellTypes?(enemyToDuplicate.PathableTiles), false, cellValidator);
					if (randomAvailableCell != null)
					{
						AIActor aiactor = AIActor.Spawn(EnemyDatabase.GetOrLoadByGuid(enemyToDuplicate.EnemyGuid), randomAvailableCell.Value, this, true, AIActor.AwakenAnimationType.Default, false);
						if (GameManager.Instance.BestActivePlayer.CurrentRoom == this)
						{
							aiactor.HandleReinforcementFallIntoRoom(0f);
						}
					}
				}
			}
		}

		// Token: 0x060052BB RID: 21179 RVA: 0x001DCDC0 File Offset: 0x001DAFC0
		public virtual void WriteRoomData(DungeonData data)
		{
			if (this.area.prototypeRoom != null)
			{
				this.MakePredefinedRoom();
				this.StampAdditionalAppearanceData();
			}
			else if (this.area.proceduralCells != null)
			{
				this.MakeCustomProceduralRoom();
			}
			else
			{
				BraveUtility.Log("STAMPING RECTANGULAR ROOM", Color.red, BraveUtility.LogVerbosity.IMPORTANT);
				RobotDaveIdea robotDaveIdea = ((!GameManager.Instance.Dungeon.UsesCustomFloorIdea) ? GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultProceduralIdea : GameManager.Instance.Dungeon.FloorIdea);
				PrototypeDungeonRoom prototypeDungeonRoom = RobotDave.RuntimeProcessIdea(robotDaveIdea, this.area.dimensions);
				this.area.prototypeRoom = prototypeDungeonRoom;
				this.MakePredefinedRoom();
				this.area.prototypeRoom = null;
				this.area.IsProceduralRoom = true;
			}
			this.DefineRoomBorderCells();
			this.cameraBoundingPolygon = new RoomHandlerBoundingPolygon(this.GetPolygonDecomposition(), GameManager.Instance.MainCameraController.controllerCamera.VisibleBorder);
			this.cameraBoundingRect = this.GetBoundingRect();
			if (this.area.prototypeRoom != null && this.area.prototypeRoom.associatedMinimapIcon != null)
			{
				Minimap.Instance.RegisterRoomIcon(this, this.area.prototypeRoom.associatedMinimapIcon, false);
			}
		}

		// Token: 0x060052BC RID: 21180 RVA: 0x001DCF20 File Offset: 0x001DB120
		private void PreprocessVisualData()
		{
			if (this.area.prototypeRoom == null && this.area.proceduralCells != null)
			{
				return;
			}
			DungeonMaterial dungeonMaterial = GameManager.Instance.Dungeon.roomMaterialDefinitions[this.RoomVisualSubtype];
			if (dungeonMaterial.usesInternalMaterialTransitions)
			{
				int num = UnityEngine.Random.Range(0, 5);
				for (int i = 0; i < num; i++)
				{
					int num2 = this.area.basePosition.x + UnityEngine.Random.Range(0, this.area.dimensions.x - 3);
					int num3 = this.area.basePosition.y + UnityEngine.Random.Range(0, this.area.dimensions.y - 3);
					int num4 = UnityEngine.Random.Range(3, this.area.dimensions.x - (num2 - this.area.basePosition.x));
					int num5 = UnityEngine.Random.Range(3, this.area.dimensions.y - (num3 - this.area.basePosition.y));
					for (int j = num2; j < num2 + num4; j++)
					{
						for (int k = num3; k < num3 + num5; k++)
						{
							CellData cellData = GameManager.Instance.Dungeon.data[j, k];
							if (cellData.type != CellType.WALL && !cellData.IsTopWall())
							{
								cellData.cellVisualData.roomVisualTypeIndex = dungeonMaterial.internalMaterialTransitions[0].materialTransition;
							}
						}
					}
				}
			}
		}

		// Token: 0x060052BD RID: 21181 RVA: 0x001DD0C4 File Offset: 0x001DB2C4
		public void PostGenerationCleanup()
		{
			if (!this.area.IsProceduralRoom)
			{
				if (this.area.prototypeRoom != null)
				{
					this.area.runtimePrototypeData = new RuntimePrototypeRoomData(this.area.prototypeRoom);
					if (!this.area.runtimePrototypeData.usesCustomAmbient)
					{
						this.area.runtimePrototypeData.usesCustomAmbient = true;
						this.area.runtimePrototypeData.usesDifferentCustomAmbientLowQuality = true;
						this.area.runtimePrototypeData.customAmbient = Color.Lerp(GameManager.Instance.Dungeon.decoSettings.ambientLightColor, GameManager.Instance.Dungeon.decoSettings.ambientLightColorTwo, UnityEngine.Random.value);
						this.area.runtimePrototypeData.customAmbientLowQuality = Color.Lerp(GameManager.Instance.Dungeon.decoSettings.lowQualityAmbientLightColor, GameManager.Instance.Dungeon.decoSettings.lowQualityAmbientLightColorTwo, UnityEngine.Random.value);
					}
					this.PreLoadReinforcements();
					this.area.prototypeRoom = null;
				}
				else if (this.area.runtimePrototypeData == null)
				{
					this.area.IsProceduralRoom = true;
				}
			}
		}

		// Token: 0x060052BE RID: 21182 RVA: 0x001DD200 File Offset: 0x001DB400
		private void DefineRoomBorderCells()
		{
			HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
			DungeonData data = GameManager.Instance.Dungeon.data;
			IntVector2[] cardinalsAndOrdinals = IntVector2.CardinalsAndOrdinals;
			foreach (IntVector2 intVector in this.roomCellsWithoutExits)
			{
				CellData cellData = data[intVector];
				cellData.nearestRoom = this;
				cellData.distanceFromNearestRoom = 0f;
				data[cellData.position + IntVector2.Up].nearestRoom = this;
				data[cellData.position + IntVector2.Up].distanceFromNearestRoom = 0f;
				data[cellData.position + IntVector2.Up * 2].nearestRoom = this;
				data[cellData.position + IntVector2.Up * 2].distanceFromNearestRoom = 0f;
				for (int i = 0; i < cardinalsAndOrdinals.Length; i++)
				{
					if (data.CheckInBounds(cellData.position + cardinalsAndOrdinals[i]))
					{
						CellData cellData2 = data[cellData.position + cardinalsAndOrdinals[i]];
						if (cellData2 != null)
						{
							if (i == 0 || i == 1 || i == 7)
							{
								cellData2 = data[cellData2.position + IntVector2.Up * 2];
							}
							if (cellData2.type == CellType.WALL || cellData2.isExitCell)
							{
								cellData2.distanceFromNearestRoom = 1f;
								cellData2.nearestRoom = this;
								hashSet.Add(cellData2.position);
							}
						}
					}
				}
			}
			this.DefineEpicenter(hashSet);
		}

		// Token: 0x060052BF RID: 21183 RVA: 0x001DD40C File Offset: 0x001DB60C
		private void DebugDrawCross(Vector3 centerPoint, Color crosscolor)
		{
			UnityEngine.Debug.DrawLine(centerPoint + new Vector3(-0.5f, 0f, 0f), centerPoint + new Vector3(0.5f, 0f, 0f), crosscolor, 1000f);
			UnityEngine.Debug.DrawLine(centerPoint + new Vector3(0f, -0.5f, 0f), centerPoint + new Vector3(0f, 0.5f, 0f), crosscolor, 1000f);
		}

		// Token: 0x060052C0 RID: 21184 RVA: 0x001DD498 File Offset: 0x001DB698
		private float UpdateOcclusionData(PlayerController p, float visibility, bool useFloodFill = true)
		{
			IntVector2 intVector = ((!(p != null)) ? GameManager.Instance.MainCameraController.Camera.transform.position.IntXY(VectorConversions.Floor) : p.transform.position.IntXY(VectorConversions.Floor));
			return Pixelator.Instance.ProcessOcclusionChange(intVector, visibility, this, useFloodFill);
		}

		// Token: 0x060052C1 RID: 21185 RVA: 0x001DD4F8 File Offset: 0x001DB6F8
		private float UpdateOcclusionData(float visibility, IntVector2 startPosition, bool useFloodFill = true)
		{
			return Pixelator.Instance.ProcessOcclusionChange(startPosition, visibility, this, useFloodFill);
		}

		// Token: 0x060052C2 RID: 21186 RVA: 0x001DD508 File Offset: 0x001DB708
		public AIActor GetToughestEnemy()
		{
			AIActor aiactor = null;
			float num = 0f;
			if (this.activeEnemies != null)
			{
				for (int i = 0; i < this.activeEnemies.Count; i++)
				{
					if (this.activeEnemies[i] && this.activeEnemies[i].IsNormalEnemy && this.activeEnemies[i].healthHaver && !this.activeEnemies[i].healthHaver.IsBoss)
					{
						float num2 = this.activeEnemies[i].healthHaver.GetMaxHealth() + (float)((!this.activeEnemies[i].IsSignatureEnemy) ? 0 : 1000);
						if (num2 > num)
						{
							aiactor = this.activeEnemies[i];
							num = num2;
						}
					}
				}
			}
			return aiactor;
		}

		// Token: 0x060052C3 RID: 21187 RVA: 0x001DD5F8 File Offset: 0x001DB7F8
		public bool AddMysteriousBulletManToRoom()
		{
			if (GameStatsManager.Instance.AnyPastBeaten())
			{
				DungeonPlaceable dungeonPlaceable = BraveResources.Load("MysteriousBullet", ".asset") as DungeonPlaceable;
				if (dungeonPlaceable == null)
				{
					return false;
				}
				CellValidator cellValidator = (IntVector2 a) => !GameManager.Instance.Dungeon.data[a].IsTopWall();
				IntVector2? randomAvailableCell = this.GetRandomAvailableCell(new IntVector2?(new IntVector2(2, 2)), new CellTypes?(CellTypes.FLOOR), false, cellValidator);
				if (randomAvailableCell != null)
				{
					dungeonPlaceable.InstantiateObject(this, randomAvailableCell.Value - this.area.basePosition, false, false);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060052C4 RID: 21188 RVA: 0x001DD6A0 File Offset: 0x001DB8A0
		public void AddSpecificEnemyToRoomProcedurally(string enemyGuid, bool reinforcementSpawn = false, Vector2? goalPosition = null)
		{
			AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(enemyGuid);
			IntVector2 clearance = orLoadByGuid.specRigidbody.UnitDimensions.ToIntVector2(VectorConversions.Ceil);
			CellValidator cellValidator = delegate(IntVector2 c)
			{
				for (int i = 0; i < clearance.x; i++)
				{
					int num = c.x + i;
					for (int j = 0; j < clearance.y; j++)
					{
						int num2 = c.y + j;
						if (GameManager.Instance.Dungeon.data.isTopWall(num, num2))
						{
							return false;
						}
					}
				}
				return true;
			};
			IntVector2? intVector;
			if (goalPosition != null)
			{
				intVector = this.GetNearestAvailableCell(goalPosition.Value, new IntVector2?(clearance), new CellTypes?(CellTypes.FLOOR), false, cellValidator);
			}
			else
			{
				intVector = this.GetRandomAvailableCell(new IntVector2?(clearance), new CellTypes?(CellTypes.FLOOR), false, cellValidator);
			}
			if (intVector != null)
			{
				AIActor aiactor = AIActor.Spawn(orLoadByGuid, intVector.Value, this, true, AIActor.AwakenAnimationType.Spawn, false);
				if (aiactor && reinforcementSpawn)
				{
					if (aiactor.specRigidbody)
					{
						aiactor.specRigidbody.CollideWithOthers = false;
					}
					aiactor.HandleReinforcementFallIntoRoom(0f);
				}
			}
			else
			{
				UnityEngine.Debug.LogError("failed placement");
			}
		}

		// Token: 0x060052C5 RID: 21189 RVA: 0x001DD794 File Offset: 0x001DB994
		private void MakeCustomProceduralRoom()
		{
			for (int i = 0; i < this.area.proceduralCells.Count; i++)
			{
				IntVector2 intVector = this.area.basePosition + this.area.proceduralCells[i];
				bool flag = this.StampCellComplex(intVector.x, intVector.y, CellType.FLOOR, DiagonalWallType.NONE, false);
			}
			if (GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON)
			{
				this.AssignRoomVisualType(3, false);
				DungeonData data = GameManager.Instance.BestGenerationDungeonPrefab.data;
				for (int j = 0; j < this.area.proceduralCells.Count; j++)
				{
					IntVector2 intVector2 = this.area.basePosition + this.area.proceduralCells[j];
					this.HandleStampedCellVisualData(intVector2.x, intVector2.y, null);
				}
			}
		}

		// Token: 0x060052C6 RID: 21190 RVA: 0x001DD890 File Offset: 0x001DBA90
		private GameObject PreloadReinforcementObject(PrototypePlacedObjectData objectData, IntVector2 pos, bool suppressPlayerChecks = false)
		{
			if (objectData.spawnChance < 1f && UnityEngine.Random.value > objectData.spawnChance)
			{
				return null;
			}
			if (objectData.instancePrerequisites != null && objectData.instancePrerequisites.Length > 0)
			{
				bool flag = true;
				for (int i = 0; i < objectData.instancePrerequisites.Length; i++)
				{
					if (!objectData.instancePrerequisites[i].CheckConditionsFulfilled())
					{
						flag = false;
					}
				}
				if (!flag)
				{
					return null;
				}
			}
			GameObject gameObject = null;
			if (objectData.placeableContents != null)
			{
				gameObject = objectData.placeableContents.InstantiateObject(this, pos, false, true);
			}
			if (objectData.nonenemyBehaviour != null)
			{
				gameObject = objectData.nonenemyBehaviour.InstantiateObject(this, pos, true);
				gameObject.GetComponent<DungeonPlaceableBehaviour>().PlacedPosition = pos + this.area.basePosition;
			}
			if (!string.IsNullOrEmpty(objectData.enemyBehaviourGuid))
			{
				AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(objectData.enemyBehaviourGuid);
				gameObject = orLoadByGuid.InstantiateObject(this, pos, true);
			}
			if (gameObject != null)
			{
				AIActor component = gameObject.GetComponent<AIActor>();
				if (component)
				{
					component.IsInReinforcementLayer = true;
					if (suppressPlayerChecks)
					{
						component.HasDonePlayerEnterCheck = true;
					}
					if (component.healthHaver && component.healthHaver.IsBoss)
					{
						component.HasDonePlayerEnterCheck = true;
					}
					component.PlacedPosition = pos + this.area.basePosition;
				}
				this.HandleFields(objectData, gameObject);
				gameObject.transform.parent = this.hierarchyParent;
				gameObject.SetActive(false);
			}
			return gameObject;
		}

		// Token: 0x060052C7 RID: 21191 RVA: 0x001DDA34 File Offset: 0x001DBC34
		public void HandleFields(PrototypePlacedObjectData objectData, GameObject instantiatedObject)
		{
			if (objectData.nonenemyBehaviour != null || !string.IsNullOrEmpty(objectData.enemyBehaviourGuid))
			{
				object[] components = instantiatedObject.GetComponents<IHasDwarfConfigurables>();
				bool flag = false;
				for (int i = 0; i < components.Length; i++)
				{
					if (!flag)
					{
						object obj = components[i];
						Type type = obj.GetType();
						for (int j = 0; j < objectData.fieldData.Count; j++)
						{
							FieldInfo field = type.GetField(objectData.fieldData[j].fieldName);
							if (field != null)
							{
								flag = true;
								if (objectData.fieldData[j].fieldType == PrototypePlacedObjectFieldData.FieldType.FLOAT)
								{
									if (field.FieldType == typeof(int))
									{
										float floatValue = objectData.fieldData[j].floatValue;
										int num = (int)floatValue;
										field.SetValue(obj, num);
									}
									else
									{
										field.SetValue(obj, objectData.fieldData[j].floatValue);
									}
								}
								else
								{
									field.SetValue(obj, objectData.fieldData[j].boolValue);
								}
							}
						}
						if (obj is ConveyorBelt)
						{
							(obj as ConveyorBelt).PostFieldConfiguration(this);
						}
					}
				}
			}
		}

		// Token: 0x060052C8 RID: 21192 RVA: 0x001DDB90 File Offset: 0x001DBD90
		private void ForceConfigure(GameObject instantiated)
		{
			Component[] componentsInChildren = instantiated.GetComponentsInChildren(typeof(IPlaceConfigurable));
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				IPlaceConfigurable placeConfigurable = componentsInChildren[i] as IPlaceConfigurable;
				if (placeConfigurable != null)
				{
					placeConfigurable.ConfigureOnPlacement(this);
				}
			}
		}

		// Token: 0x060052C9 RID: 21193 RVA: 0x001DDBD8 File Offset: 0x001DBDD8
		private List<GameObject> PlaceObjectsFromLayer(List<PrototypePlacedObjectData> placedObjectList, PrototypeRoomObjectLayer sourceLayer, List<Vector2> placedObjectPositions, Dictionary<int, RoomEventTriggerArea> eventTriggerMap, bool spawnPoofs = false, bool shuffleSpawns = false, int randomizeSpawns = 0, bool suppressPlayerChecks = false, bool disableDrops = false, int specifyObjectIndex = -1, int specifyObjectCount = -1)
		{
			DungeonData data = GameManager.Instance.Dungeon.data;
			List<Vector2> list;
			if (shuffleSpawns)
			{
				list = new List<Vector2>(placedObjectPositions);
				for (int i = list.Count - 1; i > 0; i--)
				{
					int num = UnityEngine.Random.Range(0, i + 1);
					if (i != num)
					{
						Vector2 vector = list[i];
						list[i] = list[num];
						list[num] = vector;
					}
				}
			}
			else
			{
				list = placedObjectPositions;
			}
			List<GameObject> list2 = new List<GameObject>();
			Dictionary<PrototypePlacedObjectData, GameObject> dictionary = null;
			if (sourceLayer != null && this.preloadedReinforcementLayerData != null && this.preloadedReinforcementLayerData.ContainsKey(sourceLayer))
			{
				dictionary = this.preloadedReinforcementLayerData[sourceLayer];
			}
			int num2 = 0;
			for (int j = 0; j < placedObjectList.Count; j++)
			{
				if (specifyObjectIndex < 0 || j >= specifyObjectIndex)
				{
					if (specifyObjectCount >= 0)
					{
						if (num2 >= specifyObjectCount)
						{
							break;
						}
						num2++;
					}
					PrototypePlacedObjectData prototypePlacedObjectData = placedObjectList[j];
					GameObject gameObject = null;
					if (dictionary != null && dictionary.ContainsKey(prototypePlacedObjectData))
					{
						gameObject = dictionary[prototypePlacedObjectData];
						if (gameObject == null)
						{
							goto IL_960;
						}
					}
					if (gameObject == null)
					{
						if (prototypePlacedObjectData.spawnChance < 1f && UnityEngine.Random.value > prototypePlacedObjectData.spawnChance)
						{
							goto IL_960;
						}
						if (prototypePlacedObjectData.instancePrerequisites != null && prototypePlacedObjectData.instancePrerequisites.Length > 0)
						{
							bool flag = true;
							for (int k = 0; k < prototypePlacedObjectData.instancePrerequisites.Length; k++)
							{
								if (!prototypePlacedObjectData.instancePrerequisites[k].CheckConditionsFulfilled())
								{
									flag = false;
								}
							}
							if (!flag)
							{
								goto IL_960;
							}
						}
					}
					GameObject gameObject2 = null;
					IntVector2 instantiatedDimensions = IntVector2.Zero;
					if (j >= list.Count)
					{
						UnityEngine.Debug.LogError("i > modifiedPlacedObjectPositions.Count, this is very bad!");
					}
					IntVector2 intVector = list[j].ToIntVector2(VectorConversions.Round);
					bool flag2 = true;
					if (gameObject != null)
					{
						AIActor component = gameObject.GetComponent<AIActor>();
						if (component)
						{
							intVector = component.PlacedPosition - this.area.basePosition;
						}
						else
						{
							intVector = gameObject.transform.position.IntXY(VectorConversions.Round) - this.area.basePosition;
						}
						gameObject2 = gameObject;
						gameObject2.SetActive(true);
						if (prototypePlacedObjectData.placeableContents != null)
						{
							DungeonPlaceable placeableContents = prototypePlacedObjectData.placeableContents;
							instantiatedDimensions = new IntVector2(placeableContents.width, placeableContents.height);
							flag2 = placeableContents.isPassable;
						}
						if (prototypePlacedObjectData.nonenemyBehaviour != null)
						{
							DungeonPlaceableBehaviour nonenemyBehaviour = prototypePlacedObjectData.nonenemyBehaviour;
							instantiatedDimensions = new IntVector2(nonenemyBehaviour.placeableWidth, nonenemyBehaviour.placeableHeight);
							flag2 = nonenemyBehaviour.isPassable;
						}
						if (!string.IsNullOrEmpty(prototypePlacedObjectData.enemyBehaviourGuid))
						{
							DungeonPlaceableBehaviour orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(prototypePlacedObjectData.enemyBehaviourGuid);
							instantiatedDimensions = new IntVector2(orLoadByGuid.placeableWidth, orLoadByGuid.placeableHeight);
							flag2 = orLoadByGuid.isPassable;
						}
						this.ForceConfigure(gameObject2);
					}
					else
					{
						if (prototypePlacedObjectData.placeableContents != null)
						{
							DungeonPlaceable placeableContents2 = prototypePlacedObjectData.placeableContents;
							instantiatedDimensions = new IntVector2(placeableContents2.width, placeableContents2.height);
							flag2 = placeableContents2.isPassable;
							gameObject2 = prototypePlacedObjectData.placeableContents.InstantiateObject(this, intVector, false, false);
						}
						if (prototypePlacedObjectData.nonenemyBehaviour != null)
						{
							DungeonPlaceableBehaviour nonenemyBehaviour2 = prototypePlacedObjectData.nonenemyBehaviour;
							instantiatedDimensions = new IntVector2(nonenemyBehaviour2.placeableWidth, nonenemyBehaviour2.placeableHeight);
							flag2 = nonenemyBehaviour2.isPassable;
							gameObject2 = prototypePlacedObjectData.nonenemyBehaviour.InstantiateObject(this, intVector, false);
							gameObject2.GetComponent<DungeonPlaceableBehaviour>().PlacedPosition = intVector + this.area.basePosition;
						}
						if (!string.IsNullOrEmpty(prototypePlacedObjectData.enemyBehaviourGuid))
						{
							AIActor orLoadByGuid2 = EnemyDatabase.GetOrLoadByGuid(prototypePlacedObjectData.enemyBehaviourGuid);
							if (orLoadByGuid2 == null)
							{
								UnityEngine.Debug.LogError(prototypePlacedObjectData.enemyBehaviourGuid + "|" + this.area.prototypeRoom.name);
							}
							instantiatedDimensions = new IntVector2(orLoadByGuid2.placeableWidth, orLoadByGuid2.placeableHeight);
							flag2 = orLoadByGuid2.isPassable;
							gameObject2 = orLoadByGuid2.InstantiateObject(this, intVector, false);
						}
					}
					if (gameObject2 != null)
					{
						AIActor component2 = gameObject2.GetComponent<AIActor>();
						if (component2)
						{
							if (suppressPlayerChecks)
							{
								component2.HasDonePlayerEnterCheck = true;
							}
							if (component2.healthHaver && component2.healthHaver.IsBoss)
							{
								component2.HasDonePlayerEnterCheck = true;
							}
							component2.PlacedPosition = intVector + this.area.basePosition;
							if (component2.specRigidbody)
							{
								component2.specRigidbody.Initialize();
								instantiatedDimensions = component2.Clearance;
							}
						}
						list2.Add(gameObject2);
						AIActor component3 = gameObject2.GetComponent<AIActor>();
						if (disableDrops && component3)
						{
							component3.CanDropCurrency = false;
							component3.CanDropItems = false;
						}
						if (randomizeSpawns > 0)
						{
							float sqrMinDist = 8f;
							Vector2 playerPosition = GameManager.Instance.PrimaryPlayer.specRigidbody.UnitCenter;
							IntVector2 truePlaceablePosition = intVector + this.area.basePosition;
							CellValidator cellValidator = delegate(IntVector2 c)
							{
								if (GameManager.Instance.Dungeon.data[c + IntVector2.Down] != null && GameManager.Instance.Dungeon.data[c + IntVector2.Down].isExitCell)
								{
									return false;
								}
								if (c.x < truePlaceablePosition.x - randomizeSpawns || c.x > truePlaceablePosition.x + randomizeSpawns || c.y < truePlaceablePosition.y - randomizeSpawns || c.y > truePlaceablePosition.y + randomizeSpawns)
								{
									return false;
								}
								if ((playerPosition - Pathfinder.GetClearanceOffset(c, instantiatedDimensions)).sqrMagnitude <= sqrMinDist)
								{
									return false;
								}
								for (int num6 = 0; num6 < instantiatedDimensions.x; num6++)
								{
									for (int num7 = 0; num7 < instantiatedDimensions.y; num7++)
									{
										if (!GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(new IntVector2(c.x + num6, c.y + num7)))
										{
											return false;
										}
										if (GameManager.Instance.Dungeon.data.isTopWall(c.x + num6, c.y + num7))
										{
											return false;
										}
										if (!GameManager.Instance.Dungeon.data[c.x + num6, c.y + num7].isGridConnected)
										{
											return false;
										}
									}
								}
								return true;
							};
							CellTypes cellTypes = CellTypes.FLOOR;
							if (component3)
							{
								cellTypes = component3.PathableTiles;
							}
							IntVector2? randomAvailableCell = this.GetRandomAvailableCell(new IntVector2?(instantiatedDimensions), new CellTypes?(cellTypes), false, cellValidator);
							if (randomAvailableCell != null)
							{
								gameObject2.transform.position += (randomAvailableCell.Value - truePlaceablePosition).ToVector3();
								if (gameObject != null)
								{
									SpeculativeRigidbody component4 = gameObject2.GetComponent<SpeculativeRigidbody>();
									if (component4)
									{
										component4.Reinitialize();
									}
								}
							}
						}
						if (spawnPoofs)
						{
							AIActor component5 = gameObject2.GetComponent<AIActor>();
							if (component5 != null)
							{
								float num3 = 0f;
								if (sourceLayer != null && specifyObjectIndex == -1 && specifyObjectIndex == -1)
								{
									num3 = 0.25f * (float)j;
								}
								component5.HandleReinforcementFallIntoRoom(num3);
							}
						}
						if (prototypePlacedObjectData.xMPxOffset != 0 || prototypePlacedObjectData.yMPxOffset != 0)
						{
							Vector2 vector2 = new Vector2((float)prototypePlacedObjectData.xMPxOffset * 0.0625f, (float)prototypePlacedObjectData.yMPxOffset * 0.0625f);
							gameObject2.transform.position = gameObject2.transform.position + vector2.ToVector3ZUp(0f);
							SpeculativeRigidbody componentInChildren = gameObject2.GetComponentInChildren<SpeculativeRigidbody>();
							if (componentInChildren)
							{
								componentInChildren.Reinitialize();
							}
						}
						for (int l = 0; l < instantiatedDimensions.x; l++)
						{
							for (int m = 0; m < instantiatedDimensions.y; m++)
							{
								IntVector2 intVector2 = new IntVector2(this.area.basePosition.x + intVector.x + l, this.area.basePosition.y + intVector.y + m);
								if (data.CheckInBoundsAndValid(intVector2))
								{
									CellData cellData = data.cellData[intVector2.x][intVector2.y];
									cellData.isOccupied = !flag2;
								}
							}
						}
						IPlayerInteractable[] interfacesInChildren = gameObject2.GetInterfacesInChildren<IPlayerInteractable>();
						for (int n = 0; n < interfacesInChildren.Length; n++)
						{
							this.interactableObjects.Add(interfacesInChildren[n]);
						}
						SurfaceDecorator component6 = gameObject2.GetComponent<SurfaceDecorator>();
						if (component6 != null)
						{
							component6.Decorate(this);
						}
						if (gameObject == null)
						{
							this.HandleFields(prototypePlacedObjectData, gameObject2);
							gameObject2.transform.parent = this.hierarchyParent;
						}
						if (prototypePlacedObjectData.linkedTriggerAreaIDs != null && prototypePlacedObjectData.linkedTriggerAreaIDs.Count > 0)
						{
							for (int num4 = 0; num4 < prototypePlacedObjectData.linkedTriggerAreaIDs.Count; num4++)
							{
								int num5 = prototypePlacedObjectData.linkedTriggerAreaIDs[num4];
								if (eventTriggerMap != null && eventTriggerMap.ContainsKey(num5))
								{
									eventTriggerMap[num5].AddGameObject(gameObject2);
								}
							}
						}
						if (prototypePlacedObjectData.assignedPathIDx != -1)
						{
							PathMover component7 = gameObject2.GetComponent<PathMover>();
							if (component7 != null && this.area.prototypeRoom.paths.Count > prototypePlacedObjectData.assignedPathIDx && prototypePlacedObjectData.assignedPathIDx >= 0)
							{
								component7.Path = this.area.prototypeRoom.paths[prototypePlacedObjectData.assignedPathIDx];
								component7.PathStartNode = prototypePlacedObjectData.assignedPathStartNode;
								component7.RoomHandler = this;
							}
						}
					}
				}
				IL_960:;
			}
			if (sourceLayer != null && this.preloadedReinforcementLayerData != null && this.preloadedReinforcementLayerData.ContainsKey(sourceLayer))
			{
				this.preloadedReinforcementLayerData.Remove(sourceLayer);
			}
			return list2;
		}

		// Token: 0x060052CA RID: 21194 RVA: 0x001DE58C File Offset: 0x001DC78C
		public void AddDarkSoulsRoomResetDependency(RoomHandler room)
		{
			if (this.DarkSoulsRoomResetDependencies == null)
			{
				this.DarkSoulsRoomResetDependencies = new List<RoomHandler>();
			}
			if (!this.DarkSoulsRoomResetDependencies.Contains(room))
			{
				this.DarkSoulsRoomResetDependencies.Add(room);
			}
		}

		// Token: 0x060052CB RID: 21195 RVA: 0x001DE5C4 File Offset: 0x001DC7C4
		public bool CanBeEscaped()
		{
			return true;
		}

		// Token: 0x060052CC RID: 21196 RVA: 0x001DE5C8 File Offset: 0x001DC7C8
		public void ResetPredefinedRoomLikeDarkSouls()
		{
			if (GameManager.Instance.PrimaryPlayer.CurrentRoom == this || this.visibility == RoomHandler.VisibilityStatus.OBSCURED || (this.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS && this.m_hasGivenReward))
			{
				if (this.DarkSoulsRoomResetDependencies != null)
				{
					for (int i = 0; i < this.DarkSoulsRoomResetDependencies.Count; i++)
					{
						this.DarkSoulsRoomResetDependencies[i].m_hasGivenReward = false;
						this.DarkSoulsRoomResetDependencies[i].ResetPredefinedRoomLikeDarkSouls();
					}
				}
				return;
			}
			if (this.OnDarkSoulsReset != null)
			{
				this.OnDarkSoulsReset();
			}
			if (this.activeEnemies != null)
			{
				for (int j = this.activeEnemies.Count - 1; j >= 0; j--)
				{
					AIActor aiactor = this.activeEnemies[j];
					if (aiactor)
					{
						if (aiactor.behaviorSpeculator)
						{
							aiactor.behaviorSpeculator.InterruptAndDisable();
						}
						if (aiactor.healthHaver.IsBoss && aiactor.healthHaver.IsAlive)
						{
							aiactor.healthHaver.EndBossState(false);
						}
						UnityEngine.Object.Destroy(aiactor.gameObject);
					}
				}
				this.activeEnemies.Clear();
			}
			if (GameManager.Instance.InTutorial)
			{
				List<TalkDoerLite> componentsInRoom = this.GetComponentsInRoom<TalkDoerLite>();
				for (int k = 0; k < componentsInRoom.Count; k++)
				{
					this.DeregisterInteractable(componentsInRoom[k]);
					IEventTriggerable @interface = componentsInRoom[k].gameObject.GetInterface<IEventTriggerable>();
					for (int l = 0; l < this.eventTriggerAreas.Count; l++)
					{
						this.eventTriggerAreas[l].events.Remove(@interface);
					}
					UnityEngine.Object.Destroy(componentsInRoom[k].gameObject);
				}
				this.npcSealState = RoomHandler.NPCSealState.SealNone;
			}
			else
			{
				List<TalkDoerLite> componentsInRoom2 = this.GetComponentsInRoom<TalkDoerLite>();
				for (int m = 0; m < componentsInRoom2.Count; m++)
				{
					componentsInRoom2[m].SendPlaymakerEvent("resetRoomLikeDarkSouls");
				}
			}
			if (this.bossTriggerZones != null)
			{
				for (int n = 0; n < this.bossTriggerZones.Count; n++)
				{
					this.bossTriggerZones[n].HasTriggered = false;
				}
			}
			if (this.remainingReinforcementLayers != null)
			{
				this.remainingReinforcementLayers.Clear();
			}
			this.UnsealRoom();
			this.visibility = RoomHandler.VisibilityStatus.REOBSCURED;
			for (int num = 0; num < this.connectedDoors.Count; num++)
			{
				this.connectedDoors[num].Close();
			}
			this.PreventStandardRoomReward = true;
			if (this.area.IsProceduralRoom)
			{
				if (this.area.proceduralCells == null)
				{
				}
			}
			else
			{
				for (int num2 = -1; num2 < this.area.runtimePrototypeData.additionalObjectLayers.Count; num2++)
				{
					if (num2 != -1 && this.area.runtimePrototypeData.additionalObjectLayers[num2].layerIsReinforcementLayer)
					{
						PrototypeRoomObjectLayer prototypeRoomObjectLayer = this.area.runtimePrototypeData.additionalObjectLayers[num2];
						if (prototypeRoomObjectLayer.numberTimesEncounteredRequired > 0)
						{
							if (this.area.prototypeRoom != null)
							{
								if (GameStatsManager.Instance.QueryRoomEncountered(this.area.prototypeRoom.GUID) < prototypeRoomObjectLayer.numberTimesEncounteredRequired)
								{
									goto IL_7EC;
								}
							}
							else if (this.area.runtimePrototypeData != null && GameStatsManager.Instance.QueryRoomEncountered(this.area.runtimePrototypeData.GUID) < prototypeRoomObjectLayer.numberTimesEncounteredRequired)
							{
								goto IL_7EC;
							}
						}
						if (prototypeRoomObjectLayer.probability >= 1f || UnityEngine.Random.value <= prototypeRoomObjectLayer.probability)
						{
							if (this.remainingReinforcementLayers == null)
							{
								this.remainingReinforcementLayers = new List<PrototypeRoomObjectLayer>();
							}
							if (this.area.runtimePrototypeData.additionalObjectLayers[num2].placedObjects.Count > 0)
							{
								this.remainingReinforcementLayers.Add(this.area.runtimePrototypeData.additionalObjectLayers[num2]);
							}
						}
					}
					else
					{
						List<PrototypePlacedObjectData> list = ((num2 != -1) ? this.area.runtimePrototypeData.additionalObjectLayers[num2].placedObjects : this.area.runtimePrototypeData.placedObjects);
						List<Vector2> list2 = ((num2 != -1) ? this.area.runtimePrototypeData.additionalObjectLayers[num2].placedObjectBasePositions : this.area.runtimePrototypeData.placedObjectPositions);
						for (int num3 = 0; num3 < list.Count; num3++)
						{
							PrototypePlacedObjectData prototypePlacedObjectData = list[num3];
							if (prototypePlacedObjectData.spawnChance >= 1f || UnityEngine.Random.value <= prototypePlacedObjectData.spawnChance)
							{
								GameObject gameObject = null;
								IntVector2 intVector = list2[num3].ToIntVector2(VectorConversions.Round);
								if (prototypePlacedObjectData.placeableContents != null)
								{
									DungeonPlaceable placeableContents = prototypePlacedObjectData.placeableContents;
									gameObject = placeableContents.InstantiateObject(this, intVector, true, false);
								}
								if (prototypePlacedObjectData.nonenemyBehaviour != null)
								{
									DungeonPlaceableBehaviour nonenemyBehaviour = prototypePlacedObjectData.nonenemyBehaviour;
									if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.TUTORIAL && nonenemyBehaviour.GetComponent<TalkDoerLite>() != null)
									{
										gameObject = nonenemyBehaviour.InstantiateObject(this, intVector, false);
									}
									else
									{
										gameObject = nonenemyBehaviour.InstantiateObjectOnlyActors(this, intVector, false);
									}
								}
								if (!string.IsNullOrEmpty(prototypePlacedObjectData.enemyBehaviourGuid))
								{
									DungeonPlaceableBehaviour orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(prototypePlacedObjectData.enemyBehaviourGuid);
									gameObject = orLoadByGuid.InstantiateObjectOnlyActors(this, intVector, false);
								}
								if (gameObject != null)
								{
									AIActor component = gameObject.GetComponent<AIActor>();
									if (component)
									{
										if (component.healthHaver && component.healthHaver.IsBoss)
										{
											component.HasDonePlayerEnterCheck = true;
										}
										if (component.EnemyGuid == GlobalEnemyGuids.GripMaster)
										{
											UnityEngine.Object.Destroy(component.gameObject);
											goto IL_7D8;
										}
									}
									if (prototypePlacedObjectData.xMPxOffset != 0 || prototypePlacedObjectData.yMPxOffset != 0)
									{
										Vector2 vector = new Vector2((float)prototypePlacedObjectData.xMPxOffset * 0.0625f, (float)prototypePlacedObjectData.yMPxOffset * 0.0625f);
										gameObject.transform.position = gameObject.transform.position + vector.ToVector3ZUp(0f);
									}
									IPlayerInteractable[] interfacesInChildren = gameObject.GetInterfacesInChildren<IPlayerInteractable>();
									for (int num4 = 0; num4 < interfacesInChildren.Length; num4++)
									{
										this.interactableObjects.Add(interfacesInChildren[num4]);
									}
									this.HandleFields(prototypePlacedObjectData, gameObject);
									gameObject.transform.parent = this.hierarchyParent;
								}
								if (prototypePlacedObjectData.linkedTriggerAreaIDs != null && prototypePlacedObjectData.linkedTriggerAreaIDs.Count > 0 && gameObject != null)
								{
									for (int num5 = 0; num5 < prototypePlacedObjectData.linkedTriggerAreaIDs.Count; num5++)
									{
										int num6 = prototypePlacedObjectData.linkedTriggerAreaIDs[num5];
										if (this.eventTriggerMap != null && this.eventTriggerMap.ContainsKey(num6))
										{
											this.eventTriggerMap[num6].AddGameObject(gameObject);
										}
									}
								}
								if (prototypePlacedObjectData.assignedPathIDx != -1 && gameObject)
								{
									PathMover component2 = gameObject.GetComponent<PathMover>();
									if (component2 != null)
									{
										component2.Path = this.area.runtimePrototypeData.paths[prototypePlacedObjectData.assignedPathIDx];
										component2.PathStartNode = prototypePlacedObjectData.assignedPathStartNode;
										component2.RoomHandler = this;
									}
								}
							}
							IL_7D8:;
						}
					}
					IL_7EC:;
				}
			}
			Pixelator.Instance.ProcessOcclusionChange(IntVector2.Zero, 0f, this, false);
			if (this.DarkSoulsRoomResetDependencies != null)
			{
				for (int num7 = 0; num7 < this.DarkSoulsRoomResetDependencies.Count; num7++)
				{
					this.DarkSoulsRoomResetDependencies[num7].m_hasGivenReward = false;
					this.DarkSoulsRoomResetDependencies[num7].ResetPredefinedRoomLikeDarkSouls();
				}
			}
		}

		// Token: 0x060052CD RID: 21197 RVA: 0x001DEE4C File Offset: 0x001DD04C
		private void HandleCellDungeonMaterialOverride(int ix, int iy, int overrideMaterialIndex)
		{
			DungeonData data = GameManager.Instance.Dungeon.data;
			int num = 0;
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON)
			{
				num = 1;
			}
			for (int i = -1 * num; i < num + 1; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					CellData cellData = data.cellData[ix + i][iy + j];
					if (cellData == null || ((i != 0 || j != 0) && cellData.type != CellType.WALL))
					{
						break;
					}
					cellData.cellVisualData.roomVisualTypeIndex = overrideMaterialIndex;
				}
			}
		}

		// Token: 0x060052CE RID: 21198 RVA: 0x001DEEF4 File Offset: 0x001DD0F4
		private IntVector2 GetFirstCellOfSpecificQuality(int xStart, int yStart, int xDim, int yDim, Func<CellData, bool> validator)
		{
			for (int i = yStart; i < yStart + yDim; i++)
			{
				for (int j = xStart; j < xStart + xDim; j++)
				{
					if (validator(GameManager.Instance.Dungeon.data[j, i]))
					{
						return new IntVector2(j, i);
					}
				}
			}
			return IntVector2.NegOne;
		}

		// Token: 0x060052CF RID: 21199 RVA: 0x001DEF5C File Offset: 0x001DD15C
		public void EnsureUpstreamLocksUnlocked()
		{
			if (this.IsOnCriticalPath)
			{
				return;
			}
			for (int i = 0; i < this.connectedRooms.Count; i++)
			{
				if (this.connectedRooms[i].distanceFromEntrance < this.distanceFromEntrance)
				{
					RuntimeExitDefinition exitDefinitionForConnectedRoom = this.GetExitDefinitionForConnectedRoom(this.connectedRooms[i]);
					if (exitDefinitionForConnectedRoom != null && exitDefinitionForConnectedRoom.linkedDoor != null && exitDefinitionForConnectedRoom.linkedDoor.isLocked)
					{
						exitDefinitionForConnectedRoom.linkedDoor.Unlock();
					}
					this.connectedRooms[i].EnsureUpstreamLocksUnlocked();
				}
			}
		}

		// Token: 0x060052D0 RID: 21200 RVA: 0x001DF004 File Offset: 0x001DD204
		private void HandleProceduralLocking()
		{
			if (!this.IsOnCriticalPath && this.ShouldAttemptProceduralLock)
			{
				float value = UnityEngine.Random.value;
				if (value < this.AttemptProceduralLockChance)
				{
					if (this.ProceduralLockingType == RoomHandler.ProceduralLockType.BASE_SHOP)
					{
						BaseShopController.HasLockedShopProcedurally = true;
					}
					for (int i = 0; i < this.connectedDoors.Count; i++)
					{
						RoomHandler roomHandler = ((this.connectedDoors[i].upstreamRoom != this) ? this.connectedDoors[i].upstreamRoom : this.connectedDoors[i].downstreamRoom);
						if (roomHandler != null && roomHandler.distanceFromEntrance < this.distanceFromEntrance)
						{
							this.connectedDoors[i].isLocked = true;
							this.connectedDoors[i].ForceBecomeLockedDoor();
						}
					}
				}
			}
		}

		// Token: 0x060052D1 RID: 21201 RVA: 0x001DF0E0 File Offset: 0x001DD2E0
		public void PostProcessFeatures()
		{
			if (this.area.prototypeRoom != null && this.area.prototypeRoom.rectangularFeatures != null)
			{
				for (int i = 0; i < this.area.prototypeRoom.rectangularFeatures.Count; i++)
				{
					PrototypeRectangularFeature prototypeRectangularFeature = this.area.prototypeRoom.rectangularFeatures[i];
					GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
					if (tilesetId == GlobalDungeonData.ValidTilesets.SEWERGEON)
					{
						this.PostProcessSewersFeature(prototypeRectangularFeature);
					}
				}
			}
		}

		// Token: 0x060052D2 RID: 21202 RVA: 0x001DF188 File Offset: 0x001DD388
		public void ProcessFeatures()
		{
			this.HandleProceduralLocking();
			if (this.area.prototypeRoom != null && this.area.prototypeRoom.rectangularFeatures != null)
			{
				for (int i = 0; i < this.area.prototypeRoom.rectangularFeatures.Count; i++)
				{
					PrototypeRectangularFeature prototypeRectangularFeature = this.area.prototypeRoom.rectangularFeatures[i];
					GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
					if (tilesetId != GlobalDungeonData.ValidTilesets.WESTGEON)
					{
						if (tilesetId == GlobalDungeonData.ValidTilesets.SEWERGEON)
						{
							this.ProcessSewersFeature(prototypeRectangularFeature);
						}
					}
					else
					{
						this.ProcessWestgeonFeature(prototypeRectangularFeature);
					}
				}
			}
			if (this.area.IsProceduralRoom)
			{
				for (int j = -1; j < this.area.dimensions.x + 2; j++)
				{
					for (int k = -1; k < this.area.dimensions.y + 2; k++)
					{
						IntVector2 intVector = this.area.basePosition + new IntVector2(j, k);
						CellData cellData = GameManager.Instance.Dungeon.data[intVector];
						if (cellData != null && cellData.isExitCell)
						{
							for (int l = 0; l < 4; l++)
							{
								IntVector2 intVector2 = IntVector2.Cardinals[l];
								CellData cellData2 = GameManager.Instance.Dungeon.data[intVector + intVector2];
								while (cellData2 != null && !cellData2.isExitCell && cellData2.parentRoom == this && cellData2.type != CellType.WALL)
								{
									cellData2.type = CellType.FLOOR;
									cellData2 = GameManager.Instance.Dungeon.data[cellData2.position + intVector2];
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060052D3 RID: 21203 RVA: 0x001DF394 File Offset: 0x001DD594
		private void PostProcessSewersFeature(PrototypeRectangularFeature feature)
		{
			for (int i = feature.basePosition.x; i < feature.basePosition.x + feature.dimensions.x; i++)
			{
				for (int j = feature.basePosition.y; j < feature.basePosition.y + feature.dimensions.y; j++)
				{
					IntVector2 intVector = this.area.basePosition + new IntVector2(i, j);
					CellData cellData = GameManager.Instance.Dungeon.data[intVector];
					cellData.type = CellType.FLOOR;
				}
			}
		}

		// Token: 0x060052D4 RID: 21204 RVA: 0x001DF440 File Offset: 0x001DD640
		private void ProcessSewersFeature(PrototypeRectangularFeature feature)
		{
			for (int i = feature.basePosition.x; i < feature.basePosition.x + feature.dimensions.x; i++)
			{
				for (int j = feature.basePosition.y; j < feature.basePosition.y + feature.dimensions.y; j++)
				{
					IntVector2 intVector = this.area.basePosition + new IntVector2(i, j);
					CellData cellData = GameManager.Instance.Dungeon.data[intVector];
					cellData.fallingPrevented = true;
					int num = 91;
					if (this.RoomMaterial.bridgeGrid != null)
					{
						bool[] array = new bool[8];
						for (int k = 0; k < array.Length; k++)
						{
							IntVector2 intVector2FromDirection = DungeonData.GetIntVector2FromDirection((DungeonData.Direction)k);
							IntVector2 intVector2 = intVector + intVector2FromDirection - this.area.basePosition;
							if (intVector2.x >= feature.basePosition.x && intVector2.x < feature.basePosition.x + feature.dimensions.x && intVector2.y >= feature.basePosition.y && intVector2.y < feature.basePosition.y + feature.dimensions.y)
							{
								array[k] = true;
							}
						}
						num = this.RoomMaterial.bridgeGrid.GetIndexGivenEightSides(array);
					}
					cellData.cellVisualData.UsesCustomIndexOverride01 = true;
					cellData.cellVisualData.CustomIndexOverride01 = num;
					cellData.cellVisualData.CustomIndexOverride01Layer = GlobalDungeonData.patternLayerIndex;
				}
			}
		}

		// Token: 0x060052D5 RID: 21205 RVA: 0x001DF608 File Offset: 0x001DD808
		private void ProcessWestgeonFeature(PrototypeRectangularFeature feature)
		{
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.WESTGEON)
			{
				int num = UnityEngine.Random.Range(1, 3);
				DungeonMaterial dungeonMaterial = GameManager.Instance.Dungeon.roomMaterialDefinitions[num];
				for (int i = feature.basePosition.x; i < feature.basePosition.x + feature.dimensions.x; i++)
				{
					for (int j = feature.basePosition.y; j < feature.basePosition.y + feature.dimensions.y; j++)
					{
						IntVector2 intVector = this.area.basePosition + new IntVector2(i, j);
						if (GameManager.Instance.Dungeon.data[intVector].nearestRoom == this)
						{
							GameManager.Instance.Dungeon.data[intVector].cellVisualData.IsFeatureCell = true;
							this.featureCells.Add(intVector);
							GameManager.Instance.Dungeon.data[intVector].cellVisualData.roomVisualTypeIndex = num;
						}
					}
				}
				IntVector2 intVector2 = this.GetFirstCellOfSpecificQuality(this.area.basePosition.x + feature.basePosition.x, this.area.basePosition.y + feature.basePosition.y, feature.dimensions.x, feature.dimensions.y, (CellData a) => a.IsUpperFacewall());
				if (intVector2 != IntVector2.NegOne)
				{
					int num2 = 0;
					IntVector2 intVector3 = intVector2;
					while (GameManager.Instance.Dungeon.data[intVector3].IsUpperFacewall() && intVector3.x < this.area.basePosition.x + feature.basePosition.x + feature.dimensions.x)
					{
						num2++;
						intVector3 += IntVector2.Right;
					}
					if (num2 > 3)
					{
						int num3 = UnityEngine.Random.Range(0, num2 - 3);
						num2 -= num3;
						intVector2 = intVector2.WithX(intVector2.x + UnityEngine.Random.Range(0, num3));
						for (int k = intVector2.x; k < intVector2.x + num2; k++)
						{
							for (int l = intVector2.y + 1; l <= intVector2.y + 2; l++)
							{
								GameManager.Instance.Dungeon.data[k, l].cellVisualData.UsesCustomIndexOverride01 = true;
								GameManager.Instance.Dungeon.data[k, l].cellVisualData.CustomIndexOverride01Layer = GlobalDungeonData.aboveBorderLayerIndex;
								GameManager.Instance.Dungeon.data[k, l].cellVisualData.CustomIndexOverride01 = dungeonMaterial.facadeTopGrid.GetIndexGivenSides(l == intVector2.y + 2, l == intVector2.y + 2 && k == intVector2.x + num2 - 1, k == intVector2.x + num2 - 1, l == intVector2.y + 1 && k == intVector2.x + num2 - 1, l == intVector2.y + 1, l == intVector2.y + 1 && k == intVector2.x, k == intVector2.x, l == intVector2.y + 2 && k == intVector2.x);
							}
						}
					}
				}
			}
		}

		// Token: 0x060052D6 RID: 21206 RVA: 0x001DF9EC File Offset: 0x001DDBEC
		private void StampAdditionalAppearanceData()
		{
			float num = UnityEngine.Random.Range(0f, 0.05f);
			float num2 = UnityEngine.Random.Range(0f, 0.05f);
			for (int i = this.area.basePosition.x; i < this.area.basePosition.x + this.area.dimensions.x; i++)
			{
				for (int j = this.area.basePosition.y; j < this.area.basePosition.y + this.area.dimensions.y; j++)
				{
					int num3 = i - this.area.basePosition.x;
					int num4 = j - this.area.basePosition.y;
					PrototypeDungeonRoomCellData prototypeDungeonRoomCellData = this.area.prototypeRoom.ForceGetCellDataAtPoint(num3, num4);
					if (!prototypeDungeonRoomCellData.doesDamage)
					{
						DungeonMaterial dungeonMaterial = GameManager.Instance.Dungeon.roomMaterialDefinitions[this.RoomVisualSubtype];
						if (prototypeDungeonRoomCellData.appearance.overrideDungeonMaterialIndex != -1)
						{
							this.HandleCellDungeonMaterialOverride(i, j, prototypeDungeonRoomCellData.appearance.overrideDungeonMaterialIndex);
						}
						else if (dungeonMaterial.usesInternalMaterialTransitions && dungeonMaterial.usesProceduralMaterialTransitions && Mathf.PerlinNoise(num + (float)num3 / 10f, num2 + (float)num4 / 10f) > dungeonMaterial.internalMaterialTransitions[0].proceduralThreshold)
						{
							this.HandleCellDungeonMaterialOverride(i, j, dungeonMaterial.internalMaterialTransitions[0].materialTransition);
						}
					}
				}
			}
		}

		// Token: 0x060052D7 RID: 21207 RVA: 0x001DFB8C File Offset: 0x001DDD8C
		private bool NonenemyPlaceableBehaviorIsEnemylike(DungeonPlaceableBehaviour dpb)
		{
			return dpb is ForgeHammerController;
		}

		// Token: 0x060052D8 RID: 21208 RVA: 0x001DFB98 File Offset: 0x001DDD98
		private void CleanupPrototypeRoomLayers()
		{
			this.area.prototypeRoom.runtimeAdditionalObjectLayers = new List<PrototypeRoomObjectLayer>();
			for (int i = 0; i < this.area.prototypeRoom.additionalObjectLayers.Count; i++)
			{
				PrototypeRoomObjectLayer prototypeRoomObjectLayer = this.area.prototypeRoom.additionalObjectLayers[i];
				if (!prototypeRoomObjectLayer.layerIsReinforcementLayer)
				{
					this.area.prototypeRoom.runtimeAdditionalObjectLayers.Add(prototypeRoomObjectLayer);
				}
				else
				{
					Action<PrototypeRoomObjectLayer, PrototypeRoomObjectLayer> action = delegate(PrototypeRoomObjectLayer source, PrototypeRoomObjectLayer target)
					{
						target.shuffle = source.shuffle;
						target.randomize = source.randomize;
						target.suppressPlayerChecks = source.suppressPlayerChecks;
						target.delayTime = source.delayTime;
						target.reinforcementTriggerCondition = source.reinforcementTriggerCondition;
						target.probability = source.probability;
						target.numberTimesEncounteredRequired = source.numberTimesEncounteredRequired;
					};
					bool flag = false;
					bool flag2 = false;
					PrototypeRoomObjectLayer prototypeRoomObjectLayer2 = null;
					PrototypeRoomObjectLayer prototypeRoomObjectLayer3 = null;
					for (int j = 0; j < prototypeRoomObjectLayer.placedObjects.Count; j++)
					{
						if (prototypeRoomObjectLayer.placedObjects[j].placeableContents != null)
						{
							if (prototypeRoomObjectLayer.placedObjects[j].placeableContents.ContainsEnemy || prototypeRoomObjectLayer.placedObjects[j].placeableContents.ContainsEnemylikeObjectForReinforcement)
							{
								flag = true;
								if (prototypeRoomObjectLayer2 == null)
								{
									prototypeRoomObjectLayer2 = new PrototypeRoomObjectLayer();
								}
								prototypeRoomObjectLayer2.placedObjects.Add(prototypeRoomObjectLayer.placedObjects[j]);
								prototypeRoomObjectLayer2.placedObjectBasePositions.Add(prototypeRoomObjectLayer.placedObjectBasePositions[j]);
							}
							else
							{
								flag2 = true;
								if (prototypeRoomObjectLayer3 == null)
								{
									prototypeRoomObjectLayer3 = new PrototypeRoomObjectLayer();
								}
								prototypeRoomObjectLayer3.placedObjects.Add(prototypeRoomObjectLayer.placedObjects[j]);
								prototypeRoomObjectLayer3.placedObjectBasePositions.Add(prototypeRoomObjectLayer.placedObjectBasePositions[j]);
							}
						}
						else if (prototypeRoomObjectLayer.placedObjects[j].nonenemyBehaviour != null)
						{
							if (this.NonenemyPlaceableBehaviorIsEnemylike(prototypeRoomObjectLayer.placedObjects[j].nonenemyBehaviour))
							{
								flag = true;
								if (prototypeRoomObjectLayer2 == null)
								{
									prototypeRoomObjectLayer2 = new PrototypeRoomObjectLayer();
								}
								prototypeRoomObjectLayer2.placedObjects.Add(prototypeRoomObjectLayer.placedObjects[j]);
								prototypeRoomObjectLayer2.placedObjectBasePositions.Add(prototypeRoomObjectLayer.placedObjectBasePositions[j]);
							}
							else
							{
								flag2 = true;
								if (prototypeRoomObjectLayer3 == null)
								{
									prototypeRoomObjectLayer3 = new PrototypeRoomObjectLayer();
								}
								prototypeRoomObjectLayer3.placedObjects.Add(prototypeRoomObjectLayer.placedObjects[j]);
								prototypeRoomObjectLayer3.placedObjectBasePositions.Add(prototypeRoomObjectLayer.placedObjectBasePositions[j]);
							}
						}
						else if (!string.IsNullOrEmpty(prototypeRoomObjectLayer.placedObjects[j].enemyBehaviourGuid))
						{
							flag = true;
							if (prototypeRoomObjectLayer2 == null)
							{
								prototypeRoomObjectLayer2 = new PrototypeRoomObjectLayer();
							}
							prototypeRoomObjectLayer2.placedObjects.Add(prototypeRoomObjectLayer.placedObjects[j]);
							prototypeRoomObjectLayer2.placedObjectBasePositions.Add(prototypeRoomObjectLayer.placedObjectBasePositions[j]);
						}
					}
					if (flag && flag2)
					{
						action(prototypeRoomObjectLayer, prototypeRoomObjectLayer2);
						action(prototypeRoomObjectLayer, prototypeRoomObjectLayer3);
						prototypeRoomObjectLayer2.layerIsReinforcementLayer = prototypeRoomObjectLayer.layerIsReinforcementLayer;
						prototypeRoomObjectLayer3.layerIsReinforcementLayer = false;
						this.area.prototypeRoom.runtimeAdditionalObjectLayers.Add(prototypeRoomObjectLayer2);
						this.area.prototypeRoom.runtimeAdditionalObjectLayers.Add(prototypeRoomObjectLayer3);
					}
					else if (flag2)
					{
						action(prototypeRoomObjectLayer, prototypeRoomObjectLayer3);
						prototypeRoomObjectLayer3.layerIsReinforcementLayer = false;
						this.area.prototypeRoom.runtimeAdditionalObjectLayers.Add(prototypeRoomObjectLayer3);
					}
					else
					{
						this.area.prototypeRoom.runtimeAdditionalObjectLayers.Add(prototypeRoomObjectLayer);
					}
				}
			}
		}

		// Token: 0x060052D9 RID: 21209 RVA: 0x001DFF24 File Offset: 0x001DE124
		public void RegisterExternalReinforcementLayer(PrototypeDungeonRoom source, int layerIndex)
		{
			if (this.remainingReinforcementLayers == null)
			{
				this.remainingReinforcementLayers = new List<PrototypeRoomObjectLayer>();
			}
			if (source.runtimeAdditionalObjectLayers[layerIndex].placedObjects.Count > 0)
			{
				this.remainingReinforcementLayers.Add(this.area.prototypeRoom.runtimeAdditionalObjectLayers[layerIndex]);
			}
		}

		// Token: 0x060052DA RID: 21210 RVA: 0x001DFF84 File Offset: 0x001DE184
		private void MakePredefinedRoom()
		{
			this.CleanupPrototypeRoomLayers();
			DungeonData data = GameManager.Instance.Dungeon.data;
			GameObject gameObject = GameObject.Find("_Rooms");
			if (gameObject == null)
			{
				gameObject = new GameObject("_Rooms");
			}
			Transform transform = new GameObject("Room_" + this.area.prototypeRoom.name).transform;
			transform.parent = gameObject.transform;
			this.m_roomMotionHandler = transform.gameObject.AddComponent<RoomMotionHandler>();
			this.m_roomMotionHandler.Initialize(this);
			this.hierarchyParent = transform;
			List<IntVector2> list = new List<IntVector2>();
			if (this.area.prototypeRoom.ContainsEnemies)
			{
				this.EverHadEnemies = true;
			}
			for (int i = this.area.basePosition.x; i < this.area.basePosition.x + this.area.dimensions.x; i++)
			{
				for (int j = this.area.basePosition.y; j < this.area.basePosition.y + this.area.dimensions.y; j++)
				{
					int num = i - this.area.basePosition.x;
					int num2 = j - this.area.basePosition.y;
					PrototypeDungeonRoomCellData prototypeDungeonRoomCellData = this.area.prototypeRoom.ForceGetCellDataAtPoint(num, num2);
					if (this.area.prototypeRoom.HasNonWallNeighborWithDiagonals(num, num2) || prototypeDungeonRoomCellData.breakable)
					{
						bool flag = true;
						if (prototypeDungeonRoomCellData.conditionalOnParentExit && !this.area.instanceUsedExits.Contains(this.area.prototypeRoom.exitData.exits[prototypeDungeonRoomCellData.parentExitIndex]))
						{
							if (prototypeDungeonRoomCellData.conditionalCellIsPit)
							{
								flag = this.StampCellComplex(i, j, CellType.PIT, DiagonalWallType.NONE, false);
								if (flag)
								{
									this.HandleStampedCellVisualData(i, j, prototypeDungeonRoomCellData);
								}
							}
						}
						else
						{
							if (prototypeDungeonRoomCellData.state != CellType.WALL)
							{
								flag = this.StampCellComplex(i, j, prototypeDungeonRoomCellData.state, prototypeDungeonRoomCellData.diagonalWallType, false);
								if (flag)
								{
									this.HandleStampedCellVisualData(i, j, prototypeDungeonRoomCellData);
								}
							}
							else if (prototypeDungeonRoomCellData.state == CellType.WALL)
							{
								flag = this.StampCellComplex(i, j, prototypeDungeonRoomCellData.state, prototypeDungeonRoomCellData.diagonalWallType, prototypeDungeonRoomCellData.breakable);
							}
							if (flag)
							{
								CellData cellData = data.cellData[i][j];
								if (prototypeDungeonRoomCellData != null)
								{
									cellData.cellVisualData.IsPhantomCarpet = prototypeDungeonRoomCellData.appearance.IsPhantomCarpet;
									cellData.forceDisallowGoop = prototypeDungeonRoomCellData.appearance.ForceDisallowGoop;
									if (prototypeDungeonRoomCellData.appearance.OverrideFloorType != CellVisualData.CellFloorType.Ice || this.RoomMaterial.supportsIceSquares)
									{
										if (prototypeDungeonRoomCellData.appearance.OverrideFloorType != CellVisualData.CellFloorType.Stone)
										{
											cellData.cellVisualData.floorType = prototypeDungeonRoomCellData.appearance.OverrideFloorType;
											if (cellData.cellVisualData.floorType == CellVisualData.CellFloorType.Water)
											{
												cellData.cellVisualData.absorbsDebris = true;
											}
										}
									}
								}
								List<int> overridesForTilemap = prototypeDungeonRoomCellData.appearance.GetOverridesForTilemap(this.area.prototypeRoom, GameManager.Instance.Dungeon.tileIndices.tilesetId);
								if (overridesForTilemap != null && overridesForTilemap.Count != 0)
								{
									int num3 = Mathf.FloorToInt(cellData.UniqueHash * (float)overridesForTilemap.Count);
									if (num3 == overridesForTilemap.Count)
									{
										num3--;
									}
									cellData.cellVisualData.inheritedOverrideIndex = overridesForTilemap[num3];
									cellData.cellVisualData.floorTileOverridden = true;
								}
								if (prototypeDungeonRoomCellData.doesDamage && GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.JUNGLEGEON)
								{
									list.Add(cellData.position);
								}
								else if (prototypeDungeonRoomCellData.doesDamage && GameManager.Instance.Dungeon.roomMaterialDefinitions[this.m_roomVisualType].supportsLavaOrLavalikeSquares)
								{
									cellData.doesDamage = true;
									cellData.damageDefinition = prototypeDungeonRoomCellData.damageDefinition;
									cellData.cellVisualData.floorType = CellVisualData.CellFloorType.Water;
								}
								if (prototypeDungeonRoomCellData.ForceTileNonDecorated)
								{
									cellData.cellVisualData.containsObjectSpaceStamp = true;
									cellData.cellVisualData.containsWallSpaceStamp = true;
									data.cellData[i][j + 1].cellVisualData.containsObjectSpaceStamp = true;
									data.cellData[i][j + 1].cellVisualData.containsWallSpaceStamp = true;
									data.cellData[i][j + 2].cellVisualData.containsObjectSpaceStamp = true;
									data.cellData[i][j + 2].cellVisualData.containsWallSpaceStamp = true;
								}
							}
						}
					}
				}
			}
			for (int k = 0; k < this.area.prototypeRoom.paths.Count; k++)
			{
				this.area.prototypeRoom.paths[k].StampPathToTilemap(this);
			}
			this.eventTriggerAreas = new List<RoomEventTriggerArea>();
			this.eventTriggerMap = new Dictionary<int, RoomEventTriggerArea>();
			for (int l = 0; l < this.area.prototypeRoom.eventTriggerAreas.Count; l++)
			{
				PrototypeEventTriggerArea prototypeEventTriggerArea = this.area.prototypeRoom.eventTriggerAreas[l];
				RoomEventTriggerArea roomEventTriggerArea = new RoomEventTriggerArea(prototypeEventTriggerArea, this.area.basePosition);
				this.eventTriggerAreas.Add(roomEventTriggerArea);
				this.eventTriggerMap.Add(l, roomEventTriggerArea);
			}
			for (int m = -1; m < this.area.prototypeRoom.runtimeAdditionalObjectLayers.Count; m++)
			{
				if (m != -1 && this.area.prototypeRoom.runtimeAdditionalObjectLayers[m].layerIsReinforcementLayer)
				{
					PrototypeRoomObjectLayer prototypeRoomObjectLayer = this.area.prototypeRoom.runtimeAdditionalObjectLayers[m];
					if (prototypeRoomObjectLayer.numberTimesEncounteredRequired <= 0 || GameStatsManager.Instance.QueryRoomEncountered(this.area.prototypeRoom.GUID) >= prototypeRoomObjectLayer.numberTimesEncounteredRequired)
					{
						if (prototypeRoomObjectLayer.probability >= 1f || UnityEngine.Random.value <= prototypeRoomObjectLayer.probability)
						{
							if (this.remainingReinforcementLayers == null)
							{
								this.remainingReinforcementLayers = new List<PrototypeRoomObjectLayer>();
							}
							if (this.area.prototypeRoom.runtimeAdditionalObjectLayers[m].placedObjects.Count > 0)
							{
								this.remainingReinforcementLayers.Add(this.area.prototypeRoom.runtimeAdditionalObjectLayers[m]);
							}
						}
					}
				}
				else
				{
					List<PrototypePlacedObjectData> list2 = ((m != -1) ? this.area.prototypeRoom.runtimeAdditionalObjectLayers[m].placedObjects : this.area.prototypeRoom.placedObjects);
					List<Vector2> list3 = ((m != -1) ? this.area.prototypeRoom.runtimeAdditionalObjectLayers[m].placedObjectBasePositions : this.area.prototypeRoom.placedObjectPositions);
					if (m != -1)
					{
						PrototypeRoomObjectLayer prototypeRoomObjectLayer2 = this.area.prototypeRoom.runtimeAdditionalObjectLayers[m];
						if (prototypeRoomObjectLayer2.numberTimesEncounteredRequired > 0 && GameStatsManager.Instance.QueryRoomEncountered(this.area.prototypeRoom.GUID) < prototypeRoomObjectLayer2.numberTimesEncounteredRequired)
						{
							goto IL_7D2;
						}
						if (prototypeRoomObjectLayer2.probability < 1f && UnityEngine.Random.value > prototypeRoomObjectLayer2.probability)
						{
							goto IL_7D2;
						}
					}
					this.PlaceObjectsFromLayer(list2, null, list3, this.eventTriggerMap, false, false, 0, false, false, -1, -1);
				}
				IL_7D2:;
			}
			GameObject gameObject2 = GameObject.Find("_Doors");
			if (gameObject2 == null)
			{
				gameObject2 = new GameObject("_Doors");
			}
			for (int n = 0; n < this.area.instanceUsedExits.Count; n++)
			{
				PrototypeRoomExit prototypeRoomExit = this.area.instanceUsedExits[n];
				RuntimeRoomExitData runtimeRoomExitData = this.area.exitToLocalDataMap[prototypeRoomExit];
				bool flag2 = false;
				RoomHandler roomHandler = null;
				if (this.connectedRoomsByExit[prototypeRoomExit].area.prototypeRoom != null)
				{
					flag2 = this.connectedRoomsByExit[prototypeRoomExit].area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET;
				}
				roomHandler = this.connectedRoomsByExit[prototypeRoomExit];
				RuntimeExitDefinition runtimeExitDefinition = null;
				if (this.exitDefinitionsByExit != null && this.exitDefinitionsByExit.ContainsKey(runtimeRoomExitData))
				{
					runtimeExitDefinition = this.exitDefinitionsByExit[runtimeRoomExitData];
					foreach (IntVector2 intVector in runtimeExitDefinition.GetCellsForRoom(this))
					{
						this.StampCellAsExit(intVector.x, intVector.y, prototypeRoomExit.exitDirection, roomHandler, flag2);
					}
					runtimeExitDefinition.StampCellVisualTypes(data);
					runtimeExitDefinition.ProcessExitDecorables();
				}
				else
				{
					runtimeExitDefinition = new RuntimeExitDefinition(runtimeRoomExitData, runtimeRoomExitData.linkedExit, this, roomHandler);
					if (this.exitDefinitionsByExit == null)
					{
						this.exitDefinitionsByExit = new Dictionary<RuntimeRoomExitData, RuntimeExitDefinition>();
					}
					if (roomHandler.exitDefinitionsByExit == null)
					{
						roomHandler.exitDefinitionsByExit = new Dictionary<RuntimeRoomExitData, RuntimeExitDefinition>();
					}
					this.exitDefinitionsByExit.Add(runtimeRoomExitData, runtimeExitDefinition);
					if (runtimeRoomExitData.linkedExit != null)
					{
						roomHandler.exitDefinitionsByExit.Add(runtimeRoomExitData.linkedExit, runtimeExitDefinition);
					}
					foreach (IntVector2 intVector2 in runtimeExitDefinition.GetCellsForRoom(this))
					{
						this.StampCellAsExit(intVector2.x, intVector2.y, prototypeRoomExit.exitDirection, roomHandler, flag2);
					}
					if (runtimeRoomExitData.linkedExit == null)
					{
						foreach (IntVector2 intVector3 in runtimeExitDefinition.GetCellsForRoom(roomHandler))
						{
							roomHandler.StampCellAsExit(intVector3.x, intVector3.y, prototypeRoomExit.exitDirection, this, false);
							data[intVector3].parentRoom = roomHandler;
							data[intVector3].occlusionData.sharedRoomAndExitCell = true;
						}
						runtimeExitDefinition.StampCellVisualTypes(data);
					}
					if (runtimeExitDefinition.IntermediaryCells != null)
					{
						foreach (IntVector2 intVector4 in runtimeExitDefinition.IntermediaryCells)
						{
							if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector4))
							{
								if (!this.Cells.Contains(intVector4))
								{
									this.Cells.Add(intVector4);
								}
								GameManager.Instance.Dungeon.data[intVector4].parentRoom = null;
								GameManager.Instance.Dungeon.data[intVector4].isDoorFrameCell = true;
							}
						}
					}
					runtimeExitDefinition.GenerateDoorsForExit(data, gameObject2.transform);
				}
			}
			if (list.Count > 0 && GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.JUNGLEGEON)
			{
				GameObject gameObject3 = new GameObject("grass patch");
				TallGrassPatch tallGrassPatch = gameObject3.AddComponent<TallGrassPatch>();
				tallGrassPatch.cells = list;
				tallGrassPatch.BuildPatch();
			}
		}

		// Token: 0x060052DB RID: 21211 RVA: 0x001E0B94 File Offset: 0x001DED94
		public void AddProceduralTeleporterToRoom()
		{
			if (Minimap.Instance.HasTeleporterIcon(this))
			{
				return;
			}
			GameObject gameObject = ResourceCache.Acquire("Global Prefabs/Teleporter_Gungeon_01") as GameObject;
			DungeonData dungeonData = GameManager.Instance.Dungeon.data;
			bool isStrict = true;
			Func<CellData, bool> func = (CellData a) => a != null && !a.isOccupied && !a.doesDamage && !a.containsTrap && !a.IsTrapZone && !a.cellVisualData.hasStampedPath && (!isStrict || !a.HasPitNeighbor(dungeonData)) && a.type == CellType.FLOOR;
			this.ProcessTeleporterTiles(func);
			Func<CellData, bool> func2 = (CellData a) => a == null || !a.cachedCanContainTeleporter || a.parentRoom != this;
			Tuple<IntVector2, IntVector2> tuple = Carpetron.RawMaxSubmatrix(dungeonData.cellData, this.area.basePosition, this.area.dimensions, func2);
			if (tuple.Second.x < 3 || tuple.Second.y < 3)
			{
				isStrict = false;
				this.ProcessTeleporterTiles(func);
				tuple = Carpetron.RawMaxSubmatrix(dungeonData.cellData, this.area.basePosition, this.area.dimensions, func2);
			}
			BraveUtility.DrawDebugSquare(tuple.First.ToVector2(), tuple.Second.ToVector2(), Color.red, 1000f);
			if (tuple.Second.x >= 3 && tuple.Second.y >= 3)
			{
				IntVector2 intVector = tuple.First;
				IntVector2 intVector2 = tuple.Second - tuple.First;
				int num = ((intVector2.x % 2 != 1 && intVector2.x != 4) ? (-1) : 0);
				int num2 = ((intVector2.y % 2 != 1 && intVector2.y != 4) ? (-1) : 0);
				while (intVector2.x > 3)
				{
					intVector.x++;
					intVector2.x -= 2;
				}
				while (intVector2.y > 3)
				{
					intVector.y++;
					intVector2.y -= 2;
				}
				intVector += new IntVector2(num, num2);
				GameObject gameObject2 = DungeonPlaceableUtility.InstantiateDungeonPlaceable(gameObject, this, intVector, false, AIActor.AwakenAnimationType.Default, false);
				TeleporterController component = gameObject2.GetComponent<TeleporterController>();
				this.RegisterInteractable(component);
			}
		}

		// Token: 0x060052DC RID: 21212 RVA: 0x001E0DD4 File Offset: 0x001DEFD4
		private void ProcessTeleporterTiles(Func<CellData, bool> canContainTeleporter)
		{
			IntVector2 basePosition = this.area.basePosition;
			IntVector2 intVector = this.area.basePosition + this.area.dimensions - IntVector2.One;
			DungeonData data = GameManager.Instance.Dungeon.data;
			for (int i = basePosition.x; i <= intVector.x; i++)
			{
				for (int j = basePosition.y; j <= intVector.y; j++)
				{
					if (data[i, j] != null)
					{
						data[i, j].cachedCanContainTeleporter = false;
					}
				}
			}
			for (int k = basePosition.x; k <= intVector.x; k++)
			{
				for (int l = basePosition.y; l <= intVector.y; l++)
				{
					bool flag = true;
					int num = 0;
					while (num < 4 && flag)
					{
						int num2 = 0;
						while (num2 < 4 && flag)
						{
							if (!data.CheckInBounds(k + num, l + num2) || !canContainTeleporter(data[k + num, l + num2]))
							{
								flag = false;
								break;
							}
							num2++;
						}
						num++;
					}
					if (flag)
					{
						int num3 = 0;
						while (num3 < 4 && flag)
						{
							int num4 = 0;
							while (num4 < 4 && flag)
							{
								data[k + num3, l + num4].cachedCanContainTeleporter = true;
								num4++;
							}
							num3++;
						}
					}
				}
			}
		}

		// Token: 0x060052DD RID: 21213 RVA: 0x001E0F98 File Offset: 0x001DF198
		protected IntVector2 GetDoorPositionForExit(RuntimeRoomExitData exit)
		{
			IntVector2 intVector = exit.ExitOrigin - IntVector2.One;
			IntVector2 intVector2 = intVector + this.area.basePosition;
			if (exit.jointedExit)
			{
				if (exit.TotalExitLength > exit.linkedExit.TotalExitLength)
				{
					intVector = exit.HalfExitAttachPoint - IntVector2.One;
					intVector2 = intVector + this.area.basePosition;
				}
				else
				{
					intVector = exit.linkedExit.HalfExitAttachPoint - IntVector2.One;
					intVector2 = intVector + this.connectedRoomsByExit[exit.referencedExit].area.basePosition;
				}
			}
			return intVector2;
		}

		// Token: 0x060052DE RID: 21214 RVA: 0x001E104C File Offset: 0x001DF24C
		protected void AttachDoorControllerToAllConnectedExitCells(DungeonDoorController controller, IntVector2 exitCellPosition)
		{
			Queue<CellData> queue = new Queue<CellData>();
			queue.Enqueue(GameManager.Instance.Dungeon.data[exitCellPosition]);
			while (queue.Count > 0)
			{
				CellData cellData = queue.Dequeue();
				cellData.exitDoor = controller;
				List<CellData> cellNeighbors = GameManager.Instance.Dungeon.data.GetCellNeighbors(cellData, false);
				for (int i = 0; i < cellNeighbors.Count; i++)
				{
					CellData cellData2 = cellNeighbors[i];
					if (!(cellData2.exitDoor == controller) && cellData2.isExitCell)
					{
						queue.Enqueue(cellData2);
					}
				}
			}
		}

		// Token: 0x060052DF RID: 21215 RVA: 0x001E10FC File Offset: 0x001DF2FC
		public bool UnsealConditionsMet()
		{
			if (!this.area.IsProceduralRoom && this.area.runtimePrototypeData.roomEvents != null && this.area.runtimePrototypeData.roomEvents.Count > 0)
			{
				bool flag = true;
				for (int i = 0; i < this.area.runtimePrototypeData.roomEvents.Count; i++)
				{
					RoomEventDefinition roomEventDefinition = this.area.runtimePrototypeData.roomEvents[i];
					if (roomEventDefinition.action == RoomEventTriggerAction.UNSEAL_ROOM && roomEventDefinition.condition == RoomEventTriggerCondition.ON_ENEMIES_CLEARED && this.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
					{
						flag = false;
					}
				}
				return flag;
			}
			return true;
		}

		// Token: 0x060052E0 RID: 21216 RVA: 0x001E11B4 File Offset: 0x001DF3B4
		public bool CanTeleportFromRoom()
		{
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.TUTORIAL)
			{
				if (this.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
				{
					return false;
				}
			}
			else
			{
				for (int i = 0; i < this.connectedDoors.Count; i++)
				{
					if (this.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
					{
						return false;
					}
					if (this.connectedDoors[i].IsSealed && this.connectedDoors[i].Mode != DungeonDoorController.DungeonDoorMode.FINAL_BOSS_DOOR && this.connectedDoors[i].Mode != DungeonDoorController.DungeonDoorMode.BOSS_DOOR_ONLY_UNSEALS && this.connectedDoors[i].Mode != DungeonDoorController.DungeonDoorMode.ONE_WAY_DOOR_ONLY_UNSEALS)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060052E1 RID: 21217 RVA: 0x001E126C File Offset: 0x001DF46C
		public bool CanTeleportToRoom()
		{
			if (!this.TeleportersActive)
			{
				return false;
			}
			for (int i = 0; i < this.connectedDoors.Count; i++)
			{
				if (this.connectedDoors[i].IsSealed && this.connectedDoors[i].Mode != DungeonDoorController.DungeonDoorMode.FINAL_BOSS_DOOR && this.connectedDoors[i].Mode != DungeonDoorController.DungeonDoorMode.BOSS_DOOR_ONLY_UNSEALS && this.connectedDoors[i].Mode != DungeonDoorController.DungeonDoorMode.ONE_WAY_DOOR_ONLY_UNSEALS)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060052E2 RID: 21218 RVA: 0x001E1300 File Offset: 0x001DF500
		public void SealRoom()
		{
			if (this.m_isSealed)
			{
				return;
			}
			this.m_isSealed = true;
			for (int i = 0; i < this.connectedDoors.Count; i++)
			{
				if (!this.connectedDoors[i].OneWayDoor && this.npcSealState == RoomHandler.NPCSealState.SealNext)
				{
					RoomHandler roomHandler = ((this.connectedDoors[i].upstreamRoom != this) ? this.connectedDoors[i].upstreamRoom : this.connectedDoors[i].downstreamRoom);
					if (roomHandler.distanceFromEntrance >= this.distanceFromEntrance)
					{
						this.connectedDoors[i].DoSeal(this);
					}
				}
				else
				{
					this.connectedDoors[i].DoSeal(this);
				}
			}
			for (int j = 0; j < this.standaloneBlockers.Count; j++)
			{
				if (this.standaloneBlockers[j])
				{
					this.standaloneBlockers[j].Seal();
				}
			}
			for (int k = 0; k < this.connectedRooms.Count; k++)
			{
				if (this.connectedRooms[k].secretRoomManager != null)
				{
					this.connectedRooms[k].secretRoomManager.DoSeal();
				}
			}
			if (GameManager.Instance.AllPlayers.Length > 1)
			{
				PlayerController playerController = null;
				for (int l = 0; l < GameManager.Instance.AllPlayers.Length; l++)
				{
					if (GameManager.Instance.AllPlayers[l].CurrentRoom == this)
					{
						playerController = GameManager.Instance.AllPlayers[l];
						break;
					}
				}
				for (int m = 0; m < GameManager.Instance.AllPlayers.Length; m++)
				{
					if (GameManager.Instance.AllPlayers[m] == playerController)
					{
					}
				}
			}
			if (this.OnSealChanged != null)
			{
				this.OnSealChanged(true);
			}
		}

		// Token: 0x060052E3 RID: 21219 RVA: 0x001E1520 File Offset: 0x001DF720
		public void UnsealRoom()
		{
			if (this.npcSealState == RoomHandler.NPCSealState.SealAll)
			{
				return;
			}
			if (this.npcSealState == RoomHandler.NPCSealState.SealNone && !this.m_isSealed)
			{
				return;
			}
			this.m_isSealed = false;
			for (int i = 0; i < this.connectedDoors.Count; i++)
			{
				if (this.connectedDoors[i].IsSealed || (this.connectedDoors[i].subsidiaryBlocker != null && this.connectedDoors[i].subsidiaryBlocker.isSealed) || (this.connectedDoors[i].subsidiaryDoor != null && this.connectedDoors[i].subsidiaryDoor.IsSealed))
				{
					if (!this.connectedDoors[i].OneWayDoor)
					{
						if (this.npcSealState == RoomHandler.NPCSealState.SealNone)
						{
							this.connectedDoors[i].DoUnseal(this);
						}
						else if (this.npcSealState == RoomHandler.NPCSealState.SealPrior)
						{
							RoomHandler roomHandler = ((this.connectedDoors[i].upstreamRoom != this) ? this.connectedDoors[i].upstreamRoom : this.connectedDoors[i].downstreamRoom);
							if (roomHandler.distanceFromEntrance >= this.distanceFromEntrance)
							{
								this.connectedDoors[i].DoUnseal(this);
							}
						}
						else if (this.npcSealState == RoomHandler.NPCSealState.SealNext)
						{
							RoomHandler roomHandler2 = ((this.connectedDoors[i].upstreamRoom != this) ? this.connectedDoors[i].upstreamRoom : this.connectedDoors[i].downstreamRoom);
							if (roomHandler2.distanceFromEntrance < this.distanceFromEntrance)
							{
								this.connectedDoors[i].DoUnseal(this);
							}
						}
					}
					else
					{
						if (this.connectedDoors[i].subsidiaryDoor != null)
						{
							this.connectedDoors[i].subsidiaryDoor.DoUnseal(this);
						}
						if (this.connectedDoors[i].subsidiaryBlocker != null)
						{
							this.connectedDoors[i].subsidiaryBlocker.Unseal();
						}
					}
				}
			}
			for (int j = 0; j < this.standaloneBlockers.Count; j++)
			{
				if (this.standaloneBlockers[j])
				{
					this.standaloneBlockers[j].Unseal();
				}
			}
			for (int k = 0; k < this.connectedRooms.Count; k++)
			{
				if (this.connectedRooms[k].secretRoomManager != null)
				{
					this.connectedRooms[k].secretRoomManager.DoUnseal();
				}
			}
			if (this.OnSealChanged != null)
			{
				this.OnSealChanged(false);
			}
		}

		// Token: 0x060052E4 RID: 21220 RVA: 0x001E182C File Offset: 0x001DFA2C
		public IPlayerInteractable GetNearestInteractable(Vector2 position, float maxDistance, PlayerController player)
		{
			IPlayerInteractable playerInteractable = null;
			float num = float.MaxValue;
			bool flag = GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.GetOtherPlayer(player).IsTalking;
			for (int i = 0; i < this.interactableObjects.Count; i++)
			{
				IPlayerInteractable playerInteractable2 = this.interactableObjects[i];
				if (playerInteractable2 as MonoBehaviour)
				{
					if (!flag || (!(playerInteractable2 is TalkDoer) && !(playerInteractable2 is TalkDoerLite)))
					{
						if (!player.IsPrimaryPlayer && playerInteractable2 is TalkDoerLite)
						{
							TalkDoerLite talkDoerLite = playerInteractable2 as TalkDoerLite;
							if (talkDoerLite.PreventCoopInteraction)
							{
								goto IL_E4;
							}
						}
						float distanceToPoint = playerInteractable2.GetDistanceToPoint(position);
						float num2 = playerInteractable2.GetOverrideMaxDistance();
						if (num2 <= 0f)
						{
							num2 = maxDistance;
						}
						if (distanceToPoint < num2 && distanceToPoint < num)
						{
							playerInteractable = playerInteractable2;
							num = distanceToPoint;
						}
					}
				}
				IL_E4:;
			}
			if (RoomHandler.unassignedInteractableObjects != null)
			{
				for (int j = 0; j < RoomHandler.unassignedInteractableObjects.Count; j++)
				{
					IPlayerInteractable playerInteractable3 = RoomHandler.unassignedInteractableObjects[j];
					if (playerInteractable3 as MonoBehaviour)
					{
						if (!flag || (!(playerInteractable3 is TalkDoer) && !(playerInteractable3 is TalkDoerLite)))
						{
							float distanceToPoint2 = playerInteractable3.GetDistanceToPoint(position);
							float num3 = playerInteractable3.GetOverrideMaxDistance();
							if (num3 <= 0f)
							{
								num3 = maxDistance;
							}
							if (distanceToPoint2 < num3 && distanceToPoint2 < num)
							{
								playerInteractable = playerInteractable3;
								num = distanceToPoint2;
							}
						}
					}
				}
			}
			return playerInteractable;
		}

		// Token: 0x060052E5 RID: 21221 RVA: 0x001E19DC File Offset: 0x001DFBDC
		public ReadOnlyCollection<IPlayerInteractable> GetRoomInteractables()
		{
			return this.interactableObjects.AsReadOnly();
		}

		// Token: 0x060052E6 RID: 21222 RVA: 0x001E19EC File Offset: 0x001DFBEC
		public List<IPlayerInteractable> GetNearbyInteractables(Vector2 position, float maxDistance)
		{
			List<IPlayerInteractable> list = new List<IPlayerInteractable>();
			for (int i = 0; i < this.interactableObjects.Count; i++)
			{
				IPlayerInteractable playerInteractable = this.interactableObjects[i];
				if (playerInteractable.GetDistanceToPoint(position) < maxDistance)
				{
					list.Add(playerInteractable);
				}
			}
			return list;
		}

		// Token: 0x060052E7 RID: 21223 RVA: 0x001E1A40 File Offset: 0x001DFC40
		public void RegisterEnemy(AIActor enemy)
		{
			this.EverHadEnemies = true;
			if (this.activeEnemies == null)
			{
				this.activeEnemies = new List<AIActor>();
			}
			if (this.activeEnemies.Contains(enemy))
			{
				BraveUtility.Log("Registering an enemy to a RoomHandler twice!", Color.red, BraveUtility.LogVerbosity.IMPORTANT);
				return;
			}
			this.activeEnemies.Add(enemy);
			this.m_totalSpawnedEnemyHP += enemy.healthHaver.GetMaxHealth();
			this.m_lastTotalSpawnedEnemyHP += enemy.healthHaver.GetMaxHealth();
			this.RegisterAutoAimTarget(enemy);
			if (this.OnEnemyRegistered != null)
			{
				this.OnEnemyRegistered(enemy);
			}
		}

		// Token: 0x060052E8 RID: 21224 RVA: 0x001E1AE8 File Offset: 0x001DFCE8
		public AIActor GetRandomActiveEnemy(bool allowHarmless = true)
		{
			if (this.activeEnemies == null || this.activeEnemies.Count <= 0)
			{
				return null;
			}
			if (allowHarmless)
			{
				return this.activeEnemies[UnityEngine.Random.Range(0, this.activeEnemies.Count)];
			}
			RoomHandler.s_tempActiveEnemies.Clear();
			for (int i = 0; i < this.activeEnemies.Count; i++)
			{
				if (this.activeEnemies[i] && !this.activeEnemies[i].IsHarmlessEnemy)
				{
					RoomHandler.s_tempActiveEnemies.Add(this.activeEnemies[i]);
				}
			}
			if (RoomHandler.s_tempActiveEnemies.Count == 0)
			{
				return null;
			}
			AIActor aiactor = RoomHandler.s_tempActiveEnemies[UnityEngine.Random.Range(0, RoomHandler.s_tempActiveEnemies.Count)];
			RoomHandler.s_tempActiveEnemies.Clear();
			return aiactor;
		}

		// Token: 0x060052E9 RID: 21225 RVA: 0x001E1BD8 File Offset: 0x001DFDD8
		public List<AIActor> GetActiveEnemies(RoomHandler.ActiveEnemyType type)
		{
			if (type != RoomHandler.ActiveEnemyType.RoomClear)
			{
				return this.activeEnemies;
			}
			if (this.activeEnemies == null)
			{
				return null;
			}
			return new List<AIActor>(this.activeEnemies.Where((AIActor a) => !a.IgnoreForRoomClear));
		}

		// Token: 0x060052EA RID: 21226 RVA: 0x001E1C30 File Offset: 0x001DFE30
		public void GetActiveEnemies(RoomHandler.ActiveEnemyType type, ref List<AIActor> outList)
		{
			outList.Clear();
			if (this.activeEnemies == null)
			{
				return;
			}
			if (type == RoomHandler.ActiveEnemyType.RoomClear)
			{
				for (int i = 0; i < this.activeEnemies.Count; i++)
				{
					if (!this.activeEnemies[i].IgnoreForRoomClear)
					{
						outList.Add(this.activeEnemies[i]);
					}
				}
			}
			else
			{
				outList.AddRange(this.activeEnemies);
			}
		}

		// Token: 0x060052EB RID: 21227 RVA: 0x001E1CB0 File Offset: 0x001DFEB0
		public int GetActiveEnemiesCount(RoomHandler.ActiveEnemyType type)
		{
			if (this.activeEnemies == null)
			{
				return 0;
			}
			if (type == RoomHandler.ActiveEnemyType.RoomClear)
			{
				return this.activeEnemies.Count((AIActor a) => !a.IgnoreForRoomClear);
			}
			return this.activeEnemies.Count;
		}

		// Token: 0x060052EC RID: 21228 RVA: 0x001E1D08 File Offset: 0x001DFF08
		public AIActor GetNearestEnemy(Vector2 position, out float nearestDistance, bool includeBosses = true, bool excludeDying = false)
		{
			AIActor aiactor = null;
			nearestDistance = float.MaxValue;
			if (this.activeEnemies == null)
			{
				return null;
			}
			for (int i = 0; i < this.activeEnemies.Count; i++)
			{
				if (includeBosses || !this.activeEnemies[i].healthHaver.IsBoss)
				{
					if (!excludeDying || !this.activeEnemies[i].healthHaver.IsDead)
					{
						float num = Vector2.Distance(position, this.activeEnemies[i].CenterPosition);
						if (num < nearestDistance)
						{
							nearestDistance = num;
							aiactor = this.activeEnemies[i];
						}
					}
				}
			}
			return aiactor;
		}

		// Token: 0x060052ED RID: 21229 RVA: 0x001E1DC8 File Offset: 0x001DFFC8
		public AIActor GetNearestEnemyInDirection(Vector2 position, Vector2 direction, float angleTolerance, out float nearestDistance, bool includeBosses = true)
		{
			AIActor aiactor = null;
			nearestDistance = float.MaxValue;
			float num = direction.ToAngle();
			if (this.activeEnemies == null)
			{
				return null;
			}
			for (int i = 0; i < this.activeEnemies.Count; i++)
			{
				if (includeBosses || !this.activeEnemies[i].healthHaver.IsBoss)
				{
					Vector2 vector = this.activeEnemies[i].CenterPosition - position;
					float num2 = vector.ToAngle();
					float num3 = Mathf.Abs(Mathf.DeltaAngle(num, num2));
					if (num3 < angleTolerance)
					{
						float magnitude = vector.magnitude;
						if (magnitude < nearestDistance)
						{
							nearestDistance = magnitude;
							aiactor = this.activeEnemies[i];
						}
					}
				}
			}
			return aiactor;
		}

		// Token: 0x060052EE RID: 21230 RVA: 0x001E1E94 File Offset: 0x001E0094
		public AIActor GetNearestEnemyInDirection(Vector2 position, Vector2 direction, float angleTolerance, out float nearestDistance, bool includeBosses, AIActor excludeActor)
		{
			AIActor aiactor = null;
			nearestDistance = float.MaxValue;
			float num = direction.ToAngle();
			if (this.activeEnemies == null)
			{
				return null;
			}
			for (int i = 0; i < this.activeEnemies.Count; i++)
			{
				if (includeBosses || !this.activeEnemies[i].healthHaver.IsBoss)
				{
					if (!(this.activeEnemies[i] == excludeActor))
					{
						Vector2 vector = this.activeEnemies[i].CenterPosition - position;
						float num2 = vector.ToAngle();
						float num3 = Mathf.Abs(Mathf.DeltaAngle(num, num2));
						if (num3 < angleTolerance)
						{
							float magnitude = vector.magnitude;
							if (magnitude < nearestDistance)
							{
								nearestDistance = magnitude;
								aiactor = this.activeEnemies[i];
							}
						}
					}
				}
			}
			return aiactor;
		}

		// Token: 0x060052EF RID: 21231 RVA: 0x001E1F7C File Offset: 0x001E017C
		public void ApplyActionToNearbyEnemies(Vector2 position, float radius, Action<AIActor, float> lambda)
		{
			float num = radius * radius;
			if (this.activeEnemies != null)
			{
				for (int i = 0; i < this.activeEnemies.Count; i++)
				{
					if (this.activeEnemies[i])
					{
						bool flag = radius < 0f;
						Vector2 vector = this.activeEnemies[i].CenterPosition - position;
						if (!flag)
						{
							flag = vector.sqrMagnitude < num;
						}
						if (flag)
						{
							lambda(this.activeEnemies[i], vector.magnitude);
						}
					}
				}
			}
		}

		// Token: 0x060052F0 RID: 21232 RVA: 0x001E202C File Offset: 0x001E022C
		public bool HasOtherBoss(AIActor boss)
		{
			if (this.activeEnemies == null)
			{
				return false;
			}
			for (int i = 0; i < this.activeEnemies.Count; i++)
			{
				if (!(this.activeEnemies[i] == boss))
				{
					if (this.activeEnemies[i].healthHaver.IsBoss)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060052F1 RID: 21233 RVA: 0x001E209C File Offset: 0x001E029C
		public bool HasActiveEnemies(RoomHandler.ActiveEnemyType type)
		{
			if (this.activeEnemies == null)
			{
				return false;
			}
			if (type == RoomHandler.ActiveEnemyType.RoomClear)
			{
				for (int i = 0; i < this.activeEnemies.Count; i++)
				{
					if (!this.activeEnemies[i].IgnoreForRoomClear)
					{
						return true;
					}
				}
				return false;
			}
			return this.activeEnemies.Count > 0;
		}

		// Token: 0x060052F2 RID: 21234 RVA: 0x001E2104 File Offset: 0x001E0304
		public bool TriggerReinforcementLayersOnEvent(RoomEventTriggerCondition condition, bool instant = false)
		{
			bool flag = false;
			if (this.remainingReinforcementLayers == null)
			{
				return false;
			}
			for (int i = 0; i < this.remainingReinforcementLayers.Count; i++)
			{
				if (this.remainingReinforcementLayers[i].reinforcementTriggerCondition == condition)
				{
					int num = i;
					flag = this.TriggerReinforcementLayer(num, true, false, -1, -1, instant);
					break;
				}
			}
			return flag;
		}

		// Token: 0x060052F3 RID: 21235 RVA: 0x001E216C File Offset: 0x001E036C
		public void ResetEnemyHPPercentage()
		{
			this.m_totalSpawnedEnemyHP = 0f;
			this.m_lastTotalSpawnedEnemyHP = 0f;
			for (int i = 0; i < this.activeEnemies.Count; i++)
			{
				if (!this.activeEnemies[i].IgnoreForRoomClear)
				{
					this.m_totalSpawnedEnemyHP += this.activeEnemies[i].healthHaver.GetCurrentHealth();
				}
			}
			this.m_lastTotalSpawnedEnemyHP = this.m_totalSpawnedEnemyHP;
		}

		// Token: 0x060052F4 RID: 21236 RVA: 0x001E21F0 File Offset: 0x001E03F0
		private void CheckEnemyHPPercentageEvents()
		{
			float num = 0f;
			for (int i = 0; i < this.activeEnemies.Count; i++)
			{
				if (!this.activeEnemies[i].IgnoreForRoomClear)
				{
					num += this.activeEnemies[i].healthHaver.GetCurrentHealth();
				}
			}
			float num2 = this.m_lastTotalSpawnedEnemyHP / this.m_totalSpawnedEnemyHP;
			float num3 = num / this.m_totalSpawnedEnemyHP;
			if (num2 > 0.75f && num3 <= 0.75f)
			{
				this.ProcessRoomEvents(RoomEventTriggerCondition.ON_ONE_QUARTER_ENEMY_HP_DEPLETED);
				this.TriggerReinforcementLayersOnEvent(RoomEventTriggerCondition.ON_ONE_QUARTER_ENEMY_HP_DEPLETED, false);
			}
			if (num2 > 0.5f && num3 <= 0.5f)
			{
				this.ProcessRoomEvents(RoomEventTriggerCondition.ON_HALF_ENEMY_HP_DEPLETED);
				this.TriggerReinforcementLayersOnEvent(RoomEventTriggerCondition.ON_HALF_ENEMY_HP_DEPLETED, false);
			}
			if (num2 > 0.25f && num3 <= 0.25f)
			{
				this.ProcessRoomEvents(RoomEventTriggerCondition.ON_THREE_QUARTERS_ENEMY_HP_DEPLETED);
				this.TriggerReinforcementLayersOnEvent(RoomEventTriggerCondition.ON_THREE_QUARTERS_ENEMY_HP_DEPLETED, false);
			}
			if (num2 > 0.1f && num3 <= 0.1f)
			{
				this.ProcessRoomEvents(RoomEventTriggerCondition.ON_NINETY_PERCENT_ENEMY_HP_DEPLETED);
				this.TriggerReinforcementLayersOnEvent(RoomEventTriggerCondition.ON_NINETY_PERCENT_ENEMY_HP_DEPLETED, false);
			}
			this.m_lastTotalSpawnedEnemyHP = num;
		}

		// Token: 0x060052F5 RID: 21237 RVA: 0x001E230C File Offset: 0x001E050C
		public void DeregisterEnemy(AIActor enemy, bool suppressClearChecks = false)
		{
			if (this.activeEnemies == null || !this.activeEnemies.Contains(enemy))
			{
				this.DeregisterAutoAimTarget(enemy);
				return;
			}
			this.activeEnemies.Remove(enemy);
			if (!enemy.IgnoreForRoomClear && !suppressClearChecks)
			{
				this.CheckEnemyHPPercentageEvents();
				if (!this.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
				{
					bool flag = false;
					if (this.remainingReinforcementLayers != null && this.remainingReinforcementLayers.Count > 0)
					{
						flag = this.TriggerReinforcementLayersOnEvent(RoomEventTriggerCondition.ON_ENEMIES_CLEARED, false);
					}
					flag = flag || this.numberOfTimedWavesOnDeck > 0 || this.SpawnSequentialReinforcementWaves();
					if (this.PreEnemiesCleared != null)
					{
						bool flag2 = this.PreEnemiesCleared();
						flag = flag || flag2;
					}
					if (!flag || !this.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
					{
						this.ProcessRoomEvents(RoomEventTriggerCondition.ON_ENEMIES_CLEARED);
						GameManager.Instance.DungeonMusicController.NotifyCurrentRoomEnemiesCleared();
						this.OnEnemiesCleared();
						GameManager.BroadcastRoomTalkDoerFsmEvent("roomCleared");
					}
				}
			}
			this.DeregisterAutoAimTarget(enemy);
		}

		// Token: 0x060052F6 RID: 21238 RVA: 0x001E2420 File Offset: 0x001E0620
		private bool SpawnSequentialReinforcementWaves()
		{
			int num = -1;
			if (this.remainingReinforcementLayers == null)
			{
				return false;
			}
			for (int i = 0; i < this.remainingReinforcementLayers.Count; i++)
			{
				if (this.remainingReinforcementLayers[i].reinforcementTriggerCondition == RoomEventTriggerCondition.SHRINE_WAVE_A)
				{
					return false;
				}
			}
			for (int j = 0; j < this.remainingReinforcementLayers.Count; j++)
			{
				if (this.remainingReinforcementLayers[j].reinforcementTriggerCondition == RoomEventTriggerCondition.SEQUENTIAL_WAVE_TRIGGER)
				{
					num = j;
					break;
				}
			}
			return num >= 0 && this.TriggerReinforcementLayer(num, true, false, -1, -1, false);
		}

		// Token: 0x060052F7 RID: 21239 RVA: 0x001E24C4 File Offset: 0x001E06C4
		public void BuildSecretRoomCover()
		{
			this.m_secretRoomCoverObject = SecretRoomBuilder.BuildRoomCover(this, GameManager.Instance.Dungeon.data.tilemap, GameManager.Instance.Dungeon.data);
		}

		// Token: 0x060052F8 RID: 21240 RVA: 0x001E24F8 File Offset: 0x001E06F8
		private void StampCell(CellData cell, bool isSecretConnection = false)
		{
			cell.type = CellType.FLOOR;
			cell.parentArea = this.area;
			cell.parentRoom = this;
			if (this.area.prototypeRoom != null && (this.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET || isSecretConnection))
			{
				cell.isSecretRoomCell = true;
			}
			this.roomCells.Add(cell.position);
			this.roomCellsWithoutExits.Add(cell.position);
			this.rawRoomCells.Add(cell.position);
		}

		// Token: 0x060052F9 RID: 21241 RVA: 0x001E2588 File Offset: 0x001E0788
		private void StampCell(int ix, int iy, bool isSecretConnection = false)
		{
			if (ix >= GameManager.Instance.Dungeon.data.Width || iy >= GameManager.Instance.Dungeon.data.Height)
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"Attempting to stamp ",
					ix,
					",",
					iy,
					" in cellData of lengths ",
					GameManager.Instance.Dungeon.data.Width,
					",",
					GameManager.Instance.Dungeon.data.Height
				}));
			}
			this.StampCell(GameManager.Instance.Dungeon.data.cellData[ix][iy], isSecretConnection);
		}

		// Token: 0x060052FA RID: 21242 RVA: 0x001E2664 File Offset: 0x001E0864
		private void StampCellAsExit(int ix, int iy, DungeonData.Direction exitDirection, RoomHandler connectedRoom, bool isSecretConnection = false)
		{
			DungeonData data = GameManager.Instance.Dungeon.data;
			if (ix < 0 || ix >= data.Width || iy < 0 || iy >= data.Height)
			{
				UnityEngine.Debug.LogWarningFormat("Invalid StampCellAsExit({0}, {1}, {2}, {3}, {4}) call!", new object[]
				{
					ix,
					iy,
					exitDirection,
					(connectedRoom != null) ? connectedRoom.ToString() : "null",
					isSecretConnection
				});
				return;
			}
			CellData cellData = data.cellData[ix][iy];
			this.StampCell(ix, iy, isSecretConnection);
			this.roomCellsWithoutExits.Remove(cellData.position);
			cellData.cellVisualData.roomVisualTypeIndex = this.m_roomVisualType;
			if (exitDirection == DungeonData.Direction.NORTH || exitDirection == DungeonData.Direction.SOUTH)
			{
				IntVector2 intVector = new IntVector2(ix - 1, iy + 2);
				if (data.CheckInBoundsAndValid(intVector) && data[intVector].type == CellType.WALL)
				{
					data[intVector].cellVisualData.roomVisualTypeIndex = this.m_roomVisualType;
				}
				IntVector2 intVector2 = new IntVector2(ix + 1, iy + 2);
				if (data.CheckInBoundsAndValid(intVector2) && data[intVector2].type == CellType.WALL)
				{
					data[intVector2].cellVisualData.roomVisualTypeIndex = this.m_roomVisualType;
				}
			}
			else
			{
				for (int i = -1; i < 4; i++)
				{
					IntVector2 intVector3 = new IntVector2(ix, iy + i);
					if (data.CheckInBoundsAndValid(intVector3) && data[intVector3].type == CellType.WALL)
					{
						data[intVector3].cellVisualData.roomVisualTypeIndex = this.m_roomVisualType;
					}
					if (this.area.PrototypeLostWoodsRoom && GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.RATGEON && data.CheckInBoundsAndValid(intVector3))
					{
						CellData cellData2 = data[intVector3];
						if (data.isAnyFaceWall(intVector3.x, intVector3.y))
						{
							TilesetIndexMetadata.TilesetFlagType tilesetFlagType = TilesetIndexMetadata.TilesetFlagType.FACEWALL_UPPER;
							if (data.isFaceWallLower(intVector3.x, intVector3.y))
							{
								tilesetFlagType = TilesetIndexMetadata.TilesetFlagType.FACEWALL_LOWER;
							}
							int indexFromTupleArray = SecretRoomUtility.GetIndexFromTupleArray(cellData2, SecretRoomUtility.metadataLookupTableRef[tilesetFlagType], cellData2.cellVisualData.roomVisualTypeIndex, 0f);
							cellData2.cellVisualData.faceWallOverrideIndex = indexFromTupleArray;
						}
					}
				}
			}
			cellData.isExitCell = true;
			cellData.exitDirection = exitDirection;
			cellData.connectedRoom2 = connectedRoom;
			cellData.connectedRoom1 = this;
		}

		// Token: 0x060052FB RID: 21243 RVA: 0x001E28F0 File Offset: 0x001E0AF0
		public void UpdateCellVisualData(int ix, int iy)
		{
			DungeonData data = GameManager.Instance.Dungeon.data;
			for (int i = -1; i < 2; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					if (data.CheckInBoundsAndValid(new IntVector2(ix + i, iy + j)))
					{
						data.cellData[ix + i][iy + j].cellVisualData.roomVisualTypeIndex = this.m_roomVisualType;
					}
				}
			}
		}

		// Token: 0x060052FC RID: 21244 RVA: 0x001E296C File Offset: 0x001E0B6C
		private void HandleStampedCellVisualData(int ix, int iy, PrototypeDungeonRoomCellData sourceCell)
		{
			DungeonData data = GameManager.Instance.Dungeon.data;
			for (int i = -1; i < 2; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					if (data.CheckInBounds(ix + i, iy + j))
					{
						data.cellData[ix + i][iy + j].cellVisualData.roomVisualTypeIndex = this.m_roomVisualType;
					}
				}
			}
		}

		// Token: 0x060052FD RID: 21245 RVA: 0x001E29E4 File Offset: 0x001E0BE4
		public void RuntimeStampCellComplex(int ix, int iy, CellType type, DiagonalWallType diagonalWallType)
		{
			this.StampCellComplex(ix, iy, type, diagonalWallType, false);
		}

		// Token: 0x060052FE RID: 21246 RVA: 0x001E29F4 File Offset: 0x001E0BF4
		private bool StampCellComplex(int ix, int iy, CellType type, DiagonalWallType diagonalWallType, bool breakable = false)
		{
			DungeonData data = GameManager.Instance.Dungeon.data;
			if (!data.CheckInBounds(new IntVector2(ix, iy)))
			{
				return false;
			}
			CellData cellData = data.cellData[ix][iy];
			if (type == CellType.WALL && cellData.type != CellType.WALL)
			{
				UnityEngine.Debug.LogError(string.Concat(new object[] { "Attempted to stamp intersecting rooms at: ", ix, ",", iy, ". This is a CATEGORY FOUR problem. Talk to Brent." }));
				return false;
			}
			cellData.type = type;
			if (!GameManager.Instance.Dungeon.roomMaterialDefinitions[this.RoomVisualSubtype].supportsDiagonalWalls)
			{
				diagonalWallType = DiagonalWallType.NONE;
			}
			if (cellData.diagonalWallType == DiagonalWallType.NONE || diagonalWallType != DiagonalWallType.NONE)
			{
				cellData.diagonalWallType = diagonalWallType;
			}
			if (cellData.diagonalWallType != DiagonalWallType.NONE && cellData.diagonalWallType == diagonalWallType)
			{
				data.cellData[ix][iy + 1].diagonalWallType = diagonalWallType;
				data.cellData[ix][iy + 2].diagonalWallType = diagonalWallType;
			}
			cellData.breakable = breakable;
			if (!GlobalDungeonData.GUNGEON_EXPERIMENTAL && cellData.breakable)
			{
				cellData.breakable = false;
				cellData.type = CellType.FLOOR;
			}
			if (this.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET)
			{
				cellData.isSecretRoomCell = true;
			}
			cellData.parentArea = this.area;
			cellData.parentRoom = this;
			if (data.CheckInBoundsAndValid(ix, iy + 1) && data.cellData[ix][iy + 1].type == CellType.WALL)
			{
				data.cellData[ix][iy + 1].parentRoom = this;
			}
			if (data.CheckInBoundsAndValid(ix, iy + 2) && data.cellData[ix][iy + 2].type == CellType.WALL)
			{
				data.cellData[ix][iy + 2].parentRoom = this;
			}
			if (data.CheckInBoundsAndValid(ix, iy + 3) && data.cellData[ix][iy + 3].type == CellType.WALL)
			{
				data.cellData[ix][iy + 3].parentRoom = this;
			}
			if (type == CellType.PIT)
			{
				cellData.cellVisualData.containsObjectSpaceStamp = true;
			}
			if (type == CellType.FLOOR || type == CellType.PIT || (type == CellType.WALL && cellData.breakable))
			{
				this.rawRoomCells.Add(cellData.position);
				this.roomCells.Add(cellData.position);
				this.roomCellsWithoutExits.Add(cellData.position);
			}
			if (this.area.prototypeRoom != null && this.area.prototypeRoom.usesProceduralDecoration)
			{
				if (!this.area.prototypeRoom.allowWallDecoration)
				{
					cellData.cellVisualData.containsWallSpaceStamp = true;
				}
				if (!this.area.prototypeRoom.allowFloorDecoration)
				{
					cellData.cellVisualData.containsObjectSpaceStamp = true;
				}
			}
			return true;
		}

		// Token: 0x060052FF RID: 21247 RVA: 0x001E2CD4 File Offset: 0x001E0ED4
		private bool PointInsidePolygon(List<Vector2> points, Vector2 position)
		{
			int num = points.Count - 1;
			bool flag = false;
			for (int i = 0; i < points.Count; i++)
			{
				if (((points[i].y < position.y && points[num].y >= position.y) || (points[num].y < position.y && points[i].y >= position.y)) && (points[i].x <= position.x || points[num].x <= position.x))
				{
					flag ^= points[i].x + (position.y - points[i].y) / (points[num].y - points[i].y) * (points[num].x - points[i].x) < position.x;
				}
				num = i;
			}
			return flag;
		}

		// Token: 0x06005300 RID: 21248 RVA: 0x001E2E28 File Offset: 0x001E1028
		public IntVector2 GetCellAdjacentToExit(RuntimeExitDefinition exitDef)
		{
			IntVector2 intVector = IntVector2.Zero;
			for (int i = 0; i < this.CellsWithoutExits.Count; i++)
			{
				CellData cellData = GameManager.Instance.Dungeon.data[this.CellsWithoutExits[i]];
				CellData exitNeighbor = cellData.GetExitNeighbor();
				if (exitNeighbor != null && (exitNeighbor.position - cellData.position).sqrMagnitude == 1 && exitDef.ContainsPosition(exitNeighbor.position))
				{
					intVector = cellData.position;
					break;
				}
			}
			return intVector;
		}

		// Token: 0x06005301 RID: 21249 RVA: 0x001E2EC4 File Offset: 0x001E10C4
		public RuntimeExitDefinition GetExitDefinitionForConnectedRoom(RoomHandler otherRoom)
		{
			if (!this.area.IsProceduralRoom)
			{
				return this.exitDefinitionsByExit[this.area.exitToLocalDataMap[this.GetExitConnectedToRoom(otherRoom)]];
			}
			return otherRoom.exitDefinitionsByExit[otherRoom.area.exitToLocalDataMap[otherRoom.GetExitConnectedToRoom(this)]];
		}

		// Token: 0x06005302 RID: 21250 RVA: 0x001E2F28 File Offset: 0x001E1128
		public PrototypeRoomExit GetExitConnectedToRoom(RoomHandler otherRoom)
		{
			for (int i = 0; i < this.area.instanceUsedExits.Count; i++)
			{
				RoomHandler roomHandler = this.connectedRoomsByExit[this.area.instanceUsedExits[i]];
				if (roomHandler == otherRoom)
				{
					return this.area.instanceUsedExits[i];
				}
			}
			return null;
		}

		// Token: 0x06005303 RID: 21251 RVA: 0x001E2F90 File Offset: 0x001E1190
		public CellData GetNearestFloorFacewall(IntVector2 startPosition)
		{
			CellData cellData = null;
			float num = float.MaxValue;
			for (int i = 0; i < this.CellsWithoutExits.Count; i++)
			{
				CellData cellData2 = GameManager.Instance.Dungeon.data[this.CellsWithoutExits[i]];
				if (GameManager.Instance.Dungeon.data[cellData2.position + IntVector2.Up].IsAnyFaceWall() && cellData2.type == CellType.FLOOR)
				{
					float num2 = Vector2.Distance(cellData2.position.ToCenterVector2(), startPosition.ToCenterVector2());
					if (num2 < num)
					{
						num = num2;
						cellData = cellData2;
					}
				}
			}
			return cellData;
		}

		// Token: 0x06005304 RID: 21252 RVA: 0x001E3044 File Offset: 0x001E1244
		public CellData GetNearestFaceOrSidewall(IntVector2 startPosition)
		{
			CellData cellData = null;
			float num = float.MaxValue;
			for (int i = 0; i < this.CellsWithoutExits.Count; i++)
			{
				CellData cellData2 = GameManager.Instance.Dungeon.data[this.CellsWithoutExits[i]];
				if (GameManager.Instance.Dungeon.data[cellData2.position + IntVector2.Up].IsAnyFaceWall() || cellData2.IsSideWallAdjacent())
				{
					float num2 = Vector2.Distance(cellData2.position.ToCenterVector2(), startPosition.ToCenterVector2());
					if (num2 < num)
					{
						num = num2;
						cellData = cellData2;
					}
				}
			}
			return cellData;
		}

		// Token: 0x06005305 RID: 21253 RVA: 0x001E30F8 File Offset: 0x001E12F8
		private List<Vector2> GetPolygonDecomposition()
		{
			List<Vector2> list = new List<Vector2>();
			if (!this.area.IsProceduralRoom)
			{
				Rect boundingRect = this.GetBoundingRect();
				list.Add(boundingRect.min);
				list.Add(new Vector2(boundingRect.xMin, boundingRect.yMax));
				list.Add(boundingRect.max);
				list.Add(new Vector2(boundingRect.xMax, boundingRect.yMin));
			}
			else
			{
				Rect boundingRect2 = this.GetBoundingRect();
				list.Add(boundingRect2.min);
				list.Add(new Vector2(boundingRect2.xMin, boundingRect2.yMax));
				list.Add(boundingRect2.max);
				list.Add(new Vector2(boundingRect2.xMax, boundingRect2.yMin));
			}
			return list;
		}

		// Token: 0x06005306 RID: 21254 RVA: 0x001E31C8 File Offset: 0x001E13C8
		private Rect GetBoundingRect()
		{
			return new Rect((float)this.area.basePosition.x, (float)this.area.basePosition.y, (float)this.area.dimensions.x, (float)this.area.dimensions.y);
		}

		// Token: 0x06005307 RID: 21255 RVA: 0x001E3220 File Offset: 0x001E1420
		public CellData GetNearestCellToPosition(Vector2 position)
		{
			CellData cellData = null;
			float num = float.MaxValue;
			for (int i = 0; i < this.roomCells.Count; i++)
			{
				float num2 = Vector2.Distance(position, this.roomCells[i].ToVector2());
				if (num2 < num)
				{
					cellData = GameManager.Instance.Dungeon.data[this.roomCells[i]];
					num = num2;
				}
			}
			return cellData;
		}

		// Token: 0x06005308 RID: 21256 RVA: 0x001E3298 File Offset: 0x001E1498
		public IntVector2 GetRandomAvailableCellDumb()
		{
			for (int i = 0; i < 1000; i++)
			{
				int num = UnityEngine.Random.Range(this.area.basePosition.x, this.area.basePosition.x + this.area.dimensions.x);
				int num2 = UnityEngine.Random.Range(this.area.basePosition.y, this.area.basePosition.y + this.area.dimensions.y);
				IntVector2 intVector = new IntVector2(num, num2);
				if (this.CheckCellArea(intVector, IntVector2.One))
				{
					return intVector;
				}
			}
			UnityEngine.Debug.LogError("No available cells. Error.");
			return IntVector2.Zero;
		}

		// Token: 0x06005309 RID: 21257 RVA: 0x001E3354 File Offset: 0x001E1554
		public IntVector2? GetOffscreenCell(IntVector2? footprint = null, CellTypes? passableCellTypes = null, bool canPassOccupied = false, Vector2? idealPosition = null)
		{
			if (footprint == null)
			{
				footprint = new IntVector2?(IntVector2.One);
			}
			if (passableCellTypes == null)
			{
				passableCellTypes = new CellTypes?((CellTypes)2147483647);
			}
			Dungeon dungeon = GameManager.Instance.Dungeon;
			List<IntVector2> list = new List<IntVector2>();
			for (int i = this.area.basePosition.x; i < this.area.basePosition.x + this.area.dimensions.x; i++)
			{
				for (int j = this.area.basePosition.y; j < this.area.basePosition.y + this.area.dimensions.y; j++)
				{
					IntVector2 intVector = new IntVector2(i, j);
					if (dungeon.data.CheckInBoundsAndValid(intVector) && !GameManager.Instance.MainCameraController.PointIsVisible(intVector.ToCenterVector2()) && Pathfinder.Instance.IsPassable(intVector, footprint, passableCellTypes, canPassOccupied, null))
					{
						list.Add(intVector);
					}
				}
			}
			if (idealPosition != null)
			{
				if (list.Count > 0)
				{
					list.Sort((IntVector2 a, IntVector2 b) => Mathf.Abs((float)a.y - idealPosition.Value.y).CompareTo(Mathf.Abs((float)b.y - idealPosition.Value.y)));
					return new IntVector2?(list[0]);
				}
			}
			else if (list.Count > 0)
			{
				IntVector2 intVector2 = list[UnityEngine.Random.Range(0, list.Count)];
				return new IntVector2?(intVector2);
			}
			return null;
		}

		// Token: 0x0600530A RID: 21258 RVA: 0x001E3504 File Offset: 0x001E1704
		public IntVector2? GetRandomAvailableCell(IntVector2? footprint = null, CellTypes? passableCellTypes = null, bool canPassOccupied = false, CellValidator cellValidator = null)
		{
			if (footprint == null)
			{
				footprint = new IntVector2?(IntVector2.One);
			}
			if (passableCellTypes == null)
			{
				passableCellTypes = new CellTypes?((CellTypes)2147483647);
			}
			DungeonData data = GameManager.Instance.Dungeon.data;
			List<IntVector2> list = new List<IntVector2>();
			for (int i = this.area.basePosition.x; i < this.area.basePosition.x + this.area.dimensions.x; i++)
			{
				for (int j = this.area.basePosition.y; j < this.area.basePosition.y + this.area.dimensions.y; j++)
				{
					CellData cellData = data[i, j];
					if (cellData != null && cellData.parentRoom == this && !cellData.isExitCell)
					{
						if (canPassOccupied || !cellData.containsTrap)
						{
							IntVector2 intVector = new IntVector2(i, j);
							if (Pathfinder.Instance.IsPassable(intVector, footprint, passableCellTypes, canPassOccupied, cellValidator))
							{
								list.Add(intVector);
							}
						}
					}
				}
			}
			if (list.Count > 0)
			{
				IntVector2 intVector2 = list[UnityEngine.Random.Range(0, list.Count)];
				return new IntVector2?(intVector2);
			}
			return null;
		}

		// Token: 0x0600530B RID: 21259 RVA: 0x001E3680 File Offset: 0x001E1880
		public IntVector2? GetNearestAvailableCell(Vector2 nearbyPoint, IntVector2? footprint = null, CellTypes? passableCellTypes = null, bool canPassOccupied = false, CellValidator cellValidator = null)
		{
			if (footprint == null)
			{
				footprint = new IntVector2?(IntVector2.One);
			}
			if (passableCellTypes == null)
			{
				passableCellTypes = new CellTypes?((CellTypes)2147483647);
			}
			DungeonData data = GameManager.Instance.Dungeon.data;
			Vector2 vector = footprint.Value.ToVector2() / 2f;
			float num = float.MaxValue;
			IntVector2? intVector = null;
			for (int i = this.area.basePosition.x; i < this.area.basePosition.x + this.area.dimensions.x; i++)
			{
				for (int j = this.area.basePosition.y; j < this.area.basePosition.y + this.area.dimensions.y; j++)
				{
					CellData cellData = data[i, j];
					if (cellData != null && cellData.parentRoom == this && !cellData.isExitCell)
					{
						IntVector2 intVector2 = new IntVector2(i, j);
						if (Pathfinder.Instance.IsPassable(intVector2, footprint, passableCellTypes, canPassOccupied, cellValidator))
						{
							Vector2 vector2 = intVector2.ToVector2() + vector;
							float sqrMagnitude = (nearbyPoint - vector2).sqrMagnitude;
							if (sqrMagnitude < num)
							{
								num = sqrMagnitude;
								intVector = new IntVector2?(intVector2);
							}
						}
					}
				}
			}
			return intVector;
		}

		// Token: 0x0600530C RID: 21260 RVA: 0x001E3814 File Offset: 0x001E1A14
		public IntVector2? GetRandomWeightedAvailableCell(IntVector2? footprint = null, CellTypes? passableCellTypes = null, bool canPassOccupied = false, CellValidator cellValidator = null, Func<IntVector2, float> cellWeightFinder = null, float percentageBounds = 1f)
		{
			if (footprint == null)
			{
				footprint = new IntVector2?(IntVector2.One);
			}
			if (passableCellTypes == null)
			{
				passableCellTypes = new CellTypes?((CellTypes)2147483647);
			}
			DungeonData data = GameManager.Instance.Dungeon.data;
			List<Tuple<IntVector2, float>> list = new List<Tuple<IntVector2, float>>();
			for (int i = this.area.basePosition.x; i < this.area.basePosition.x + this.area.dimensions.x; i++)
			{
				for (int j = this.area.basePosition.y; j < this.area.basePosition.y + this.area.dimensions.y; j++)
				{
					CellData cellData = data[i, j];
					if (cellData != null && cellData.parentRoom == this && !cellData.isExitCell)
					{
						IntVector2 intVector = new IntVector2(i, j);
						if (Pathfinder.Instance.IsPassable(intVector, footprint, passableCellTypes, canPassOccupied, cellValidator))
						{
							list.Add(Tuple.Create<IntVector2, float>(intVector, cellWeightFinder(intVector)));
						}
					}
				}
			}
			list.Sort(new RoomHandler.TupleComparer());
			if (list.Count > 0)
			{
				return new IntVector2?(list[UnityEngine.Random.Range(0, Mathf.RoundToInt((float)list.Count * percentageBounds))].First);
			}
			return null;
		}

		// Token: 0x0600530D RID: 21261 RVA: 0x001E399C File Offset: 0x001E1B9C
		public IntVector2 GetCenterCell()
		{
			return new IntVector2(this.area.basePosition.x + Mathf.FloorToInt((float)(this.area.dimensions.x / 2)), this.area.basePosition.y + Mathf.FloorToInt((float)(this.area.dimensions.y / 2)));
		}

		// Token: 0x0600530E RID: 21262 RVA: 0x001E3A00 File Offset: 0x001E1C00
		public void DefineEpicenter(HashSet<IntVector2> startingBorder)
		{
			DungeonData data = GameManager.Instance.Dungeon.data;
			bool flag = true;
			HashSet<IntVector2> hashSet = startingBorder;
			HashSet<IntVector2> hashSet2 = new HashSet<IntVector2>();
			HashSet<IntVector2> hashSet3 = new HashSet<IntVector2>();
			while (flag)
			{
				flag = false;
				IntVector2[] cardinals = IntVector2.Cardinals;
				foreach (IntVector2 intVector in hashSet)
				{
					if (!data[intVector].isExitCell)
					{
						hashSet3.Add(intVector);
						for (int i = 0; i < cardinals.Length; i++)
						{
							IntVector2 intVector2 = intVector + cardinals[i];
							if (!hashSet3.Contains(intVector2))
							{
								if (!hashSet2.Contains(intVector2))
								{
									if (data.CheckInBoundsAndValid(intVector2))
									{
										CellData cellData = data[intVector2];
										if (cellData != null)
										{
											if (!cellData.isExitCell)
											{
												if (cellData.type != CellType.WALL && cellData.parentRoom == this)
												{
													hashSet2.Add(intVector2);
													this.Epicenter = intVector2;
													flag = true;
												}
											}
										}
									}
								}
							}
						}
					}
				}
				hashSet = hashSet2;
				hashSet2 = new HashSet<IntVector2>();
			}
		}

		// Token: 0x0600530F RID: 21263 RVA: 0x001E3B6C File Offset: 0x001E1D6C
		private List<IntVector2> CollectRandomValidCells(IntVector2 objDimensions, int offset)
		{
			List<IntVector2> list = new List<IntVector2>();
			for (int i = this.area.basePosition.x + offset; i < this.area.basePosition.x + this.area.dimensions.x - offset - (objDimensions.x - 1); i++)
			{
				for (int j = this.area.basePosition.y + offset; j < this.area.basePosition.y + this.area.dimensions.y - offset - (objDimensions.y - 1); j++)
				{
					IntVector2 intVector = new IntVector2(i, j);
					if (this.CheckCellArea(intVector, objDimensions))
					{
						list.Add(intVector);
					}
				}
			}
			return list;
		}

		// Token: 0x06005310 RID: 21264 RVA: 0x001E3C3C File Offset: 0x001E1E3C
		public List<TK2DInteriorDecorator.WallExpanse> GatherExpanses(DungeonData.Direction dir, bool breakAfterFirst = true, bool debugMe = false, bool disallowPits = false)
		{
			DungeonData data = GameManager.Instance.Dungeon.data;
			List<TK2DInteriorDecorator.WallExpanse> list = new List<TK2DInteriorDecorator.WallExpanse>(12);
			bool flag = false;
			TK2DInteriorDecorator.WallExpanse wallExpanse = default(TK2DInteriorDecorator.WallExpanse);
			IntVector2 intVector2FromDirection = DungeonData.GetIntVector2FromDirection(dir);
			IntVector2 intVector = -1 * intVector2FromDirection;
			int num = ((intVector.x != 0) ? ((intVector.x >= 0) ? (-1) : this.area.dimensions.x) : (-1));
			int num2 = ((intVector.y != 0) ? ((intVector.y >= 0) ? (-1) : this.area.dimensions.y) : (-1));
			int num3 = ((intVector.x != 0) ? ((intVector.x >= 0) ? 1 : (-1)) : 1);
			int num4 = ((intVector.y != 0) ? ((intVector.y >= 0) ? 1 : (-1)) : 1);
			bool flag2 = intVector.x != 0;
			IntVector2 intVector2 = ((!flag2) ? IntVector2.Right : IntVector2.Up);
			if (flag2)
			{
				int num5 = num2;
				while (num5 <= this.area.dimensions.y && num5 >= -1)
				{
					bool flag3 = false;
					int num6 = num;
					while (num6 <= this.area.dimensions.x && num6 >= -1)
					{
						IntVector2 intVector3 = this.area.basePosition + new IntVector2(num6, num5);
						CellData cellData = data[intVector3];
						bool flag4 = cellData == null || cellData.type == CellType.WALL;
						CellData cellData2 = data[intVector3 + intVector2FromDirection];
						bool flag5 = cellData2 == null || ((cellData2.type == CellType.WALL || data.isAnyFaceWall(cellData2.position.x, cellData2.position.y)) && !cellData2.isExitCell);
						if (flag5 && cellData2 != null && cellData2.diagonalWallType != DiagonalWallType.NONE)
						{
							flag5 = false;
						}
						if (!flag4 && (!disallowPits || cellData.type != CellType.PIT) && cellData.parentRoom == this && flag5)
						{
							flag3 = true;
							if (cellData.isExitCell)
							{
								if (flag)
								{
									list.Add(wallExpanse);
								}
								flag = false;
								break;
							}
							if (!flag)
							{
								flag = true;
								wallExpanse = new TK2DInteriorDecorator.WallExpanse(new IntVector2(num6, num5), 1);
							}
							else if (flag2 && wallExpanse.basePosition.x == num6)
							{
								wallExpanse.width++;
							}
							else if (!flag2 && wallExpanse.basePosition.y == num5)
							{
								wallExpanse.width++;
							}
							else
							{
								list.Add(wallExpanse);
								wallExpanse = new TK2DInteriorDecorator.WallExpanse(new IntVector2(num6, num5), 1);
							}
							if (breakAfterFirst)
							{
								break;
							}
						}
						num6 += num3;
					}
					if (!flag3)
					{
						if (flag)
						{
							list.Add(wallExpanse);
						}
						flag = false;
					}
					num5 += num4;
				}
			}
			else
			{
				int num7 = num;
				while (num7 <= this.area.dimensions.x && num7 >= -1)
				{
					bool flag6 = false;
					int num8 = num2;
					while (num8 <= this.area.dimensions.y && num8 >= -1)
					{
						IntVector2 intVector4 = this.area.basePosition + new IntVector2(num7, num8);
						CellData cellData3 = data[intVector4];
						bool flag7 = cellData3 == null || cellData3.type == CellType.WALL;
						CellData cellData4 = data[intVector4 + intVector2FromDirection];
						bool flag8 = cellData4 == null || ((cellData4.type == CellType.WALL || data.isAnyFaceWall(cellData4.position.x, cellData4.position.y)) && !cellData4.isExitCell);
						if (flag8 && cellData4 != null && cellData4.diagonalWallType != DiagonalWallType.NONE)
						{
							flag8 = false;
						}
						if (!flag7 && cellData3.parentRoom == this && flag8)
						{
							flag6 = true;
							if (cellData3.isExitCell)
							{
								if (flag)
								{
									list.Add(wallExpanse);
								}
								flag = false;
								break;
							}
							if (!flag)
							{
								flag = true;
								wallExpanse = new TK2DInteriorDecorator.WallExpanse(new IntVector2(num7, num8), 1);
							}
							else if (flag2 && wallExpanse.basePosition.x == num7)
							{
								wallExpanse.width++;
							}
							else if (!flag2 && wallExpanse.basePosition.y == num8)
							{
								wallExpanse.width++;
							}
							else
							{
								list.Add(wallExpanse);
								wallExpanse = new TK2DInteriorDecorator.WallExpanse(new IntVector2(num7, num8), 1);
							}
							if (breakAfterFirst)
							{
								break;
							}
						}
						num8 += num4;
					}
					if (!flag6)
					{
						if (flag)
						{
							list.Add(wallExpanse);
						}
						flag = false;
					}
					num7 += num3;
				}
			}
			if (flag && !list.Contains(wallExpanse))
			{
				list.Add(wallExpanse);
			}
			if (debugMe)
			{
				foreach (TK2DInteriorDecorator.WallExpanse wallExpanse2 in list)
				{
					for (int i = 0; i < wallExpanse2.width; i++)
					{
						BraveUtility.DrawDebugSquare(this.area.basePosition + wallExpanse2.basePosition + intVector2 * i, Color.yellow);
					}
				}
			}
			return list;
		}

		// Token: 0x06005311 RID: 21265 RVA: 0x001E423C File Offset: 0x001E243C
		public List<IntVector2> GatherPitLighting(TilemapDecoSettings decoSettings, List<IntVector2> existingLights)
		{
			float num = (float)(decoSettings.lightOverlapRadius - 2);
			List<IntVector2> list = new List<IntVector2>();
			for (int i = 0; i < this.Cells.Count; i++)
			{
				IntVector2 intVector = this.Cells[i];
				bool flag = true;
				for (int j = 0; j < list.Count; j++)
				{
					if ((float)IntVector2.ManhattanDistance(intVector, list[j] + this.area.basePosition) <= num)
					{
						flag = false;
					}
				}
				for (int k = 0; k < existingLights.Count; k++)
				{
					if ((float)IntVector2.ManhattanDistance(intVector, existingLights[k] + this.area.basePosition) <= num)
					{
						flag = false;
					}
				}
				if (flag)
				{
					if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector))
					{
						CellData cellData = GameManager.Instance.Dungeon.data[intVector];
						if (cellData.type == CellType.PIT && !cellData.SurroundedByPits(true))
						{
							list.Add(intVector - this.area.basePosition);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06005312 RID: 21266 RVA: 0x001E437C File Offset: 0x001E257C
		private List<IntVector2> GatherLightPositionForDirection(TilemapDecoSettings decoSettings, DungeonData.Direction dir)
		{
			float num = (float)decoSettings.lightOverlapRadius;
			List<IntVector2> list = new List<IntVector2>();
			IntVector2 intVector = ((dir != DungeonData.Direction.NORTH && dir != DungeonData.Direction.SOUTH) ? IntVector2.Up : IntVector2.Right);
			List<TK2DInteriorDecorator.WallExpanse> list2 = this.GatherExpanses(dir, true, false, false);
			for (int i = 0; i < list2.Count; i++)
			{
				TK2DInteriorDecorator.WallExpanse wallExpanse = list2[i];
				if (wallExpanse.width >= decoSettings.minLightExpanseWidth)
				{
					if ((float)wallExpanse.width < num * 2f)
					{
						IntVector2 intVector2 = wallExpanse.basePosition + intVector * Mathf.FloorToInt((float)wallExpanse.width / 2f);
						list.Add(intVector2);
					}
					else
					{
						int num2 = Mathf.FloorToInt((float)wallExpanse.width / num);
						int num3 = Mathf.FloorToInt(((float)wallExpanse.width - (float)(num2 - 1) * num) / 2f);
						for (int j = 0; j < num2; j++)
						{
							int num4 = num3 + Mathf.FloorToInt(num) * j;
							IntVector2 intVector3 = wallExpanse.basePosition + intVector * num4;
							list.Add(intVector3);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06005313 RID: 21267 RVA: 0x001E44BC File Offset: 0x001E26BC
		public List<IntVector2> GatherManualLightPositions()
		{
			List<IntVector2> list = new List<IntVector2>();
			for (int i = this.area.basePosition.x; i < this.area.basePosition.x + this.area.dimensions.x; i++)
			{
				for (int j = this.area.basePosition.y; j < this.area.basePosition.y + this.area.dimensions.y; j++)
				{
					int num = i - this.area.basePosition.x;
					int num2 = j - this.area.basePosition.y;
					PrototypeDungeonRoomCellData prototypeDungeonRoomCellData = this.area.prototypeRoom.ForceGetCellDataAtPoint(num, num2);
					if (prototypeDungeonRoomCellData.containsManuallyPlacedLight)
					{
						list.Add(new IntVector2(i, j) - this.area.basePosition);
					}
				}
			}
			return list;
		}

		// Token: 0x06005314 RID: 21268 RVA: 0x001E45B8 File Offset: 0x001E27B8
		public List<IntVector2> GatherOptimalLightPositions(TilemapDecoSettings decoSettings)
		{
			List<IntVector2> list = this.GatherLightPositionForDirection(decoSettings, DungeonData.Direction.NORTH);
			list.AddRange(this.GatherLightPositionForDirection(decoSettings, DungeonData.Direction.EAST));
			list.AddRange(this.GatherLightPositionForDirection(decoSettings, DungeonData.Direction.SOUTH));
			list.AddRange(this.GatherLightPositionForDirection(decoSettings, DungeonData.Direction.WEST));
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (i != j)
					{
						float num = Vector2.Distance(list[i].ToVector2(), list[j].ToVector2());
						if (num < decoSettings.nearestAllowedLight)
						{
							if (list[i].y < list[j].y)
							{
								list.RemoveAt(i);
								i--;
								break;
							}
							list.RemoveAt(j);
							j--;
							if (i > j)
							{
								i--;
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06005315 RID: 21269 RVA: 0x001E46B8 File Offset: 0x001E28B8
		private bool CheckCellArea(IntVector2 basePosition, IntVector2 objDimensions)
		{
			DungeonData data = GameManager.Instance.Dungeon.data;
			bool flag = true;
			for (int i = basePosition.x; i < basePosition.x + objDimensions.x; i++)
			{
				for (int j = basePosition.y; j < basePosition.y + objDimensions.y; j++)
				{
					CellData cellData = data.cellData[i][j];
					if (!cellData.IsPassable)
					{
						return false;
					}
				}
			}
			return flag;
		}

		// Token: 0x06005316 RID: 21270 RVA: 0x001E4744 File Offset: 0x001E2944
		private bool CellWithinRadius(Vector2 center, float radius, IntVector2 cubePos)
		{
			Vector2 vector = new Vector2((float)cubePos.x + 0.5f, (float)cubePos.y + 0.5f);
			float num = Vector2.Distance(center, vector);
			return radius >= num;
		}

		// Token: 0x04004AFC RID: 19196
		public static bool DrawRandomCellLines = false;

		// Token: 0x04004AFD RID: 19197
		public int distanceFromEntrance;

		// Token: 0x04004AFE RID: 19198
		public bool IsLoopMember;

		// Token: 0x04004AFF RID: 19199
		public bool LoopIsUnidirectional;

		// Token: 0x04004B00 RID: 19200
		public Guid LoopGuid;

		// Token: 0x04004B01 RID: 19201
		public CellArea area;

		// Token: 0x04004B02 RID: 19202
		public Rect cameraBoundingRect;

		// Token: 0x04004B03 RID: 19203
		public RoomHandlerBoundingPolygon cameraBoundingPolygon;

		// Token: 0x04004B04 RID: 19204
		public RoomHandler parentRoom;

		// Token: 0x04004B05 RID: 19205
		public List<RoomHandler> childRooms;

		// Token: 0x04004B06 RID: 19206
		public FlowActionLine flowActionLine;

		// Token: 0x04004B07 RID: 19207
		public List<DungeonDoorController> connectedDoors;

		// Token: 0x04004B08 RID: 19208
		public List<DungeonDoorSubsidiaryBlocker> standaloneBlockers;

		// Token: 0x04004B09 RID: 19209
		public List<BossTriggerZone> bossTriggerZones;

		// Token: 0x04004B0A RID: 19210
		public List<RoomHandler> connectedRooms;

		// Token: 0x04004B0B RID: 19211
		public Dictionary<PrototypeRoomExit, RoomHandler> connectedRoomsByExit;

		// Token: 0x04004B0C RID: 19212
		public Dictionary<RuntimeRoomExitData, RuntimeExitDefinition> exitDefinitionsByExit;

		// Token: 0x04004B0D RID: 19213
		public RoomHandler injectionTarget;

		// Token: 0x04004B0E RID: 19214
		public List<RoomHandler> injectionFrameData;

		// Token: 0x04004B0F RID: 19215
		public Opulence opulence;

		// Token: 0x04004B10 RID: 19216
		public GameObject OptionalDoorTopDecorable;

		// Token: 0x04004B11 RID: 19217
		public RoomCreationStrategy.RoomType roomType;

		// Token: 0x04004B12 RID: 19218
		public bool CanReceiveCaps;

		// Token: 0x04004B13 RID: 19219
		[NonSerialized]
		public RoomHandler.ProceduralLockType ProceduralLockingType;

		// Token: 0x04004B14 RID: 19220
		[NonSerialized]
		public bool ShouldAttemptProceduralLock;

		// Token: 0x04004B15 RID: 19221
		[NonSerialized]
		public float AttemptProceduralLockChance;

		// Token: 0x04004B16 RID: 19222
		public bool IsOnCriticalPath;

		// Token: 0x04004B17 RID: 19223
		public int DungeonWingID = -1;

		// Token: 0x04004B18 RID: 19224
		public bool PrecludeTilemapDrawing;

		// Token: 0x04004B19 RID: 19225
		public bool DrawPrecludedCeilingTiles;

		// Token: 0x04004B1A RID: 19226
		[NonSerialized]
		public bool PlayerHasTakenDamageInThisRoom;

		// Token: 0x04004B1B RID: 19227
		[NonSerialized]
		public bool ForcePreventChannels;

		// Token: 0x04004B1C RID: 19228
		[NonSerialized]
		public tk2dTileMap OverrideTilemap;

		// Token: 0x04004B1D RID: 19229
		[NonSerialized]
		public bool PreventMinimapUpdates;

		// Token: 0x04004B1E RID: 19230
		public RoomHandler.VisibilityStatus OverrideVisibility = RoomHandler.VisibilityStatus.INVALID;

		// Token: 0x04004B1F RID: 19231
		public bool PreventRevealEver;

		// Token: 0x04004B20 RID: 19232
		private RoomHandler.VisibilityStatus m_visibility;

		// Token: 0x04004B21 RID: 19233
		public bool forceTeleportersActive;

		// Token: 0x04004B22 RID: 19234
		public bool hasEverBeenVisited;

		// Token: 0x04004B24 RID: 19236
		public bool CompletelyPreventLeaving;

		// Token: 0x04004B25 RID: 19237
		public Action OnRevealedOnMap;

		// Token: 0x04004B26 RID: 19238
		private bool m_forceRevealedOnMap;

		// Token: 0x04004B27 RID: 19239
		public Transform hierarchyParent;

		// Token: 0x04004B28 RID: 19240
		public IntVector2 Epicenter;

		// Token: 0x04004B29 RID: 19241
		public GameObject ExtantEmergencyCrate;

		// Token: 0x04004B2A RID: 19242
		public bool PreventStandardRoomReward;

		// Token: 0x04004B2B RID: 19243
		public static bool HasGivenRoomChestRewardThisRun = false;

		// Token: 0x04004B2C RID: 19244
		public static int NumberOfRoomsToPreventChestSpawning = 0;

		// Token: 0x04004B2D RID: 19245
		private int m_roomVisualType;

		// Token: 0x04004B2E RID: 19246
		private RoomMotionHandler m_roomMotionHandler;

		// Token: 0x04004B2F RID: 19247
		public Dictionary<IntVector2, float> OcclusionPerimeterCellMap;

		// Token: 0x04004B30 RID: 19248
		public SecretRoomManager secretRoomManager;

		// Token: 0x04004B31 RID: 19249
		private HashSet<IntVector2> rawRoomCells = new HashSet<IntVector2>();

		// Token: 0x04004B32 RID: 19250
		private List<IntVector2> roomCells = new List<IntVector2>();

		// Token: 0x04004B33 RID: 19251
		private List<IntVector2> roomCellsWithoutExits = new List<IntVector2>();

		// Token: 0x04004B34 RID: 19252
		private List<IntVector2> featureCells = new List<IntVector2>();

		// Token: 0x04004B35 RID: 19253
		private List<RoomEventTriggerArea> eventTriggerAreas;

		// Token: 0x04004B36 RID: 19254
		private Dictionary<int, RoomEventTriggerArea> eventTriggerMap;

		// Token: 0x04004B37 RID: 19255
		private float m_totalSpawnedEnemyHP;

		// Token: 0x04004B38 RID: 19256
		private float m_lastTotalSpawnedEnemyHP;

		// Token: 0x04004B39 RID: 19257
		private float m_activeDifficultyModifier = 1f;

		// Token: 0x04004B3A RID: 19258
		private List<AIActor> activeEnemies;

		// Token: 0x04004B3B RID: 19259
		private List<IPlayerInteractable> interactableObjects;

		// Token: 0x04004B3C RID: 19260
		private List<IAutoAimTarget> autoAimTargets;

		// Token: 0x04004B3D RID: 19261
		private List<PrototypeRoomObjectLayer> remainingReinforcementLayers;

		// Token: 0x04004B3E RID: 19262
		private Dictionary<PrototypeRoomObjectLayer, Dictionary<PrototypePlacedObjectData, GameObject>> preloadedReinforcementLayerData;

		// Token: 0x04004B3F RID: 19263
		public static List<IPlayerInteractable> unassignedInteractableObjects = new List<IPlayerInteractable>();

		// Token: 0x04004B40 RID: 19264
		public RoomHandler.NPCSealState npcSealState;

		// Token: 0x04004B41 RID: 19265
		private bool m_isSealed;

		// Token: 0x04004B42 RID: 19266
		private bool m_currentlyVisible;

		// Token: 0x04004B43 RID: 19267
		private bool m_hasGivenReward;

		// Token: 0x04004B44 RID: 19268
		private GameObject m_secretRoomCoverObject;

		// Token: 0x04004B46 RID: 19270
		[NonSerialized]
		public RoomHandler TargetPitfallRoom;

		// Token: 0x04004B47 RID: 19271
		[NonSerialized]
		public bool ForcePitfallForFliers;

		// Token: 0x04004B48 RID: 19272
		public Action OnTargetPitfallRoom;

		// Token: 0x04004B49 RID: 19273
		public Action<PlayerController> OnPlayerReturnedFromPit;

		// Token: 0x04004B4A RID: 19274
		public Action OnDarkSoulsReset;

		// Token: 0x04004B4C RID: 19276
		[NonSerialized]
		private bool m_hasBeenEncountered;

		// Token: 0x04004B4F RID: 19279
		public RoomHandler.CustomRoomState AdditionalRoomState;

		// Token: 0x04004B50 RID: 19280
		private bool m_allRoomsActive;

		// Token: 0x04004B51 RID: 19281
		[NonSerialized]
		public bool? ForcedActiveState;

		// Token: 0x04004B52 RID: 19282
		private int numberOfTimedWavesOnDeck;

		// Token: 0x04004B54 RID: 19284
		public Action<bool> OnChangedTerrifyingDarkState;

		// Token: 0x04004B55 RID: 19285
		public bool IsDarkAndTerrifying;

		// Token: 0x04004B56 RID: 19286
		private bool? m_cachedIsMaintenance;

		// Token: 0x04004B59 RID: 19289
		public Action OnEnemiesCleared;

		// Token: 0x04004B5A RID: 19290
		public Func<bool> PreEnemiesCleared;

		// Token: 0x04004B5B RID: 19291
		private List<SpeculativeRigidbody> m_roomMovingPlatforms = new List<SpeculativeRigidbody>();

		// Token: 0x04004B5C RID: 19292
		private List<DeadlyDeadlyGoopManager> m_goops;

		// Token: 0x04004B5D RID: 19293
		[NonSerialized]
		public GenericLootTable OverrideBossRewardTable;

		// Token: 0x04004B5E RID: 19294
		public bool EverHadEnemies;

		// Token: 0x04004B5F RID: 19295
		public List<RoomHandler> DarkSoulsRoomResetDependencies;

		// Token: 0x04004B60 RID: 19296
		public Action<bool> OnSealChanged;

		// Token: 0x04004B61 RID: 19297
		public Action<AIActor> OnEnemyRegistered;

		// Token: 0x04004B62 RID: 19298
		private static List<AIActor> s_tempActiveEnemies = new List<AIActor>();

		// Token: 0x02000F17 RID: 3863
		public enum ProceduralLockType
		{
			// Token: 0x04004B6B RID: 19307
			NONE,
			// Token: 0x04004B6C RID: 19308
			BASE_SHOP
		}

		// Token: 0x02000F18 RID: 3864
		public enum VisibilityStatus
		{
			// Token: 0x04004B6E RID: 19310
			OBSCURED,
			// Token: 0x04004B6F RID: 19311
			VISITED,
			// Token: 0x04004B70 RID: 19312
			CURRENT,
			// Token: 0x04004B71 RID: 19313
			REOBSCURED,
			// Token: 0x04004B72 RID: 19314
			INVALID = 99
		}

		// Token: 0x02000F19 RID: 3865
		public enum NPCSealState
		{
			// Token: 0x04004B74 RID: 19316
			SealNone,
			// Token: 0x04004B75 RID: 19317
			SealAll,
			// Token: 0x04004B76 RID: 19318
			SealPrior,
			// Token: 0x04004B77 RID: 19319
			SealNext
		}

		// Token: 0x02000F1A RID: 3866
		// (Invoke) Token: 0x06005320 RID: 21280
		public delegate void OnEnteredEventHandler(PlayerController p);

		// Token: 0x02000F1B RID: 3867
		public enum CustomRoomState
		{
			// Token: 0x04004B79 RID: 19321
			NONE,
			// Token: 0x04004B7A RID: 19322
			LICH_PHASE_THREE = 100
		}

		// Token: 0x02000F1C RID: 3868
		// (Invoke) Token: 0x06005324 RID: 21284
		public delegate void OnExitedEventHandler();

		// Token: 0x02000F1D RID: 3869
		// (Invoke) Token: 0x06005328 RID: 21288
		public delegate void OnBecameVisibleEventHandler(float delay);

		// Token: 0x02000F1E RID: 3870
		// (Invoke) Token: 0x0600532C RID: 21292
		public delegate void OnBecameInvisibleEventHandler();

		// Token: 0x02000F1F RID: 3871
		public enum RewardLocationStyle
		{
			// Token: 0x04004B7C RID: 19324
			CameraCenter,
			// Token: 0x04004B7D RID: 19325
			PlayerCenter,
			// Token: 0x04004B7E RID: 19326
			Original
		}

		// Token: 0x02000F20 RID: 3872
		public enum ActiveEnemyType
		{
			// Token: 0x04004B80 RID: 19328
			All,
			// Token: 0x04004B81 RID: 19329
			RoomClear
		}

		// Token: 0x02000F21 RID: 3873
		private class TupleComparer : IComparer<Tuple<IntVector2, float>>
		{
			// Token: 0x06005330 RID: 21296 RVA: 0x001E487C File Offset: 0x001E2A7C
			public int Compare(Tuple<IntVector2, float> a, Tuple<IntVector2, float> b)
			{
				if (a.Second < b.Second)
				{
					return -1;
				}
				if (b.Second > a.Second)
				{
					return 1;
				}
				return 0;
			}
		}
	}
}
