using System;
using System.Collections;
using System.Collections.Generic;
using SimplexNoise;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000ECC RID: 3788
	public class DungeonData
	{
		// Token: 0x06005037 RID: 20535 RVA: 0x001C2360 File Offset: 0x001C0560
		public DungeonData(CellData[][] data)
		{
			this.cellData = data;
		}

		// Token: 0x06005038 RID: 20536 RVA: 0x001C2380 File Offset: 0x001C0580
		public void ClearCachedCellData()
		{
			this.m_width = -1;
			this.m_height = -1;
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06005039 RID: 20537 RVA: 0x001C2390 File Offset: 0x001C0590
		public int Width
		{
			get
			{
				if (this.m_width == -1)
				{
					this.m_width = this.cellData.Length;
				}
				return this.m_width;
			}
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x0600503A RID: 20538 RVA: 0x001C23B4 File Offset: 0x001C05B4
		public int Height
		{
			get
			{
				if (this.m_height == -1)
				{
					this.m_height = this.cellData[0].Length;
				}
				return this.m_height;
			}
		}

		// Token: 0x17000B61 RID: 2913
		public CellData this[IntVector2 key]
		{
			get
			{
				return this.cellData[key.x][key.y];
			}
			set
			{
				this.cellData[key.x][key.y] = value;
			}
		}

		// Token: 0x17000B62 RID: 2914
		public CellData this[int x, int y]
		{
			get
			{
				return this.cellData[x][y];
			}
			set
			{
				this.cellData[x][y] = value;
			}
		}

		// Token: 0x0600503F RID: 20543 RVA: 0x001C2428 File Offset: 0x001C0628
		public static DungeonData.Direction InvertDirection(DungeonData.Direction inDir)
		{
			switch (inDir)
			{
			case DungeonData.Direction.NORTH:
				return DungeonData.Direction.SOUTH;
			case DungeonData.Direction.NORTHEAST:
				return DungeonData.Direction.SOUTHWEST;
			case DungeonData.Direction.EAST:
				return DungeonData.Direction.WEST;
			case DungeonData.Direction.SOUTHEAST:
				return DungeonData.Direction.NORTHWEST;
			case DungeonData.Direction.SOUTH:
				return DungeonData.Direction.NORTH;
			case DungeonData.Direction.SOUTHWEST:
				return DungeonData.Direction.NORTHEAST;
			case DungeonData.Direction.WEST:
				return DungeonData.Direction.EAST;
			case DungeonData.Direction.NORTHWEST:
				return DungeonData.Direction.SOUTHEAST;
			default:
				return inDir;
			}
		}

		// Token: 0x06005040 RID: 20544 RVA: 0x001C2468 File Offset: 0x001C0668
		public static DungeonData.Direction GetRandomCardinalDirection()
		{
			float value = UnityEngine.Random.value;
			if (value < 0.25f)
			{
				return DungeonData.Direction.NORTH;
			}
			if (value < 0.5f)
			{
				return DungeonData.Direction.EAST;
			}
			if (value < 0.75f)
			{
				return DungeonData.Direction.SOUTH;
			}
			return DungeonData.Direction.WEST;
		}

		// Token: 0x06005041 RID: 20545 RVA: 0x001C24A4 File Offset: 0x001C06A4
		public static DungeonData.Direction GetCardinalFromVector2(Vector2 vec)
		{
			return DungeonData.GetDirectionFromVector2(BraveUtility.GetMajorAxis(vec));
		}

		// Token: 0x06005042 RID: 20546 RVA: 0x001C24B4 File Offset: 0x001C06B4
		public static DungeonData.Direction GetDirectionFromInts(int x, int y)
		{
			if (x == 0)
			{
				if (y > 0)
				{
					return DungeonData.Direction.NORTH;
				}
				if (y < 0)
				{
					return DungeonData.Direction.SOUTH;
				}
				return (DungeonData.Direction)(-1);
			}
			else if (x < 0)
			{
				if (y > 0)
				{
					return DungeonData.Direction.NORTHWEST;
				}
				if (y < 0)
				{
					return DungeonData.Direction.SOUTHWEST;
				}
				return DungeonData.Direction.WEST;
			}
			else
			{
				if (y > 0)
				{
					return DungeonData.Direction.NORTHEAST;
				}
				if (y < 0)
				{
					return DungeonData.Direction.SOUTHEAST;
				}
				return DungeonData.Direction.EAST;
			}
		}

		// Token: 0x06005043 RID: 20547 RVA: 0x001C250C File Offset: 0x001C070C
		public static DungeonData.Direction GetDirectionFromIntVector2(IntVector2 vec)
		{
			return DungeonData.GetDirectionFromInts(vec.x, vec.y);
		}

		// Token: 0x06005044 RID: 20548 RVA: 0x001C2524 File Offset: 0x001C0724
		public static DungeonData.Direction GetDirectionFromVector2(Vector2 vec)
		{
			if (vec.x == 0f)
			{
				if (vec.y > 0f)
				{
					return DungeonData.Direction.NORTH;
				}
				if (vec.y < 0f)
				{
					return DungeonData.Direction.SOUTH;
				}
				return (DungeonData.Direction)(-1);
			}
			else if (vec.x < 0f)
			{
				if (vec.y > 0f)
				{
					return DungeonData.Direction.NORTHWEST;
				}
				if (vec.y < 0f)
				{
					return DungeonData.Direction.SOUTHWEST;
				}
				return DungeonData.Direction.WEST;
			}
			else
			{
				if (vec.y > 0f)
				{
					return DungeonData.Direction.NORTHEAST;
				}
				if (vec.y < 0f)
				{
					return DungeonData.Direction.SOUTHEAST;
				}
				return DungeonData.Direction.EAST;
			}
		}

		// Token: 0x06005045 RID: 20549 RVA: 0x001C25CC File Offset: 0x001C07CC
		public static IntVector2 GetIntVector2FromDirection(DungeonData.Direction dir)
		{
			if (dir == DungeonData.Direction.NORTH)
			{
				return IntVector2.Up;
			}
			if (dir == DungeonData.Direction.NORTHEAST)
			{
				return IntVector2.Up + IntVector2.Right;
			}
			if (dir == DungeonData.Direction.EAST)
			{
				return IntVector2.Right;
			}
			if (dir == DungeonData.Direction.SOUTHEAST)
			{
				return IntVector2.Right + IntVector2.Down;
			}
			if (dir == DungeonData.Direction.SOUTH)
			{
				return IntVector2.Down;
			}
			if (dir == DungeonData.Direction.SOUTHWEST)
			{
				return IntVector2.Down + IntVector2.Left;
			}
			if (dir == DungeonData.Direction.WEST)
			{
				return IntVector2.Left;
			}
			if (dir == DungeonData.Direction.NORTHWEST)
			{
				return IntVector2.Left + IntVector2.Up;
			}
			return IntVector2.Zero;
		}

		// Token: 0x06005046 RID: 20550 RVA: 0x001C2670 File Offset: 0x001C0870
		public static DungeonData.Direction GetInverseDirection(DungeonData.Direction dir)
		{
			if (dir == DungeonData.Direction.NORTH)
			{
				return DungeonData.Direction.SOUTH;
			}
			if (dir == DungeonData.Direction.NORTHEAST)
			{
				return DungeonData.Direction.SOUTHWEST;
			}
			if (dir == DungeonData.Direction.EAST)
			{
				return DungeonData.Direction.WEST;
			}
			if (dir == DungeonData.Direction.SOUTHEAST)
			{
				return DungeonData.Direction.NORTHWEST;
			}
			if (dir == DungeonData.Direction.SOUTH)
			{
				return DungeonData.Direction.NORTH;
			}
			if (dir == DungeonData.Direction.SOUTHWEST)
			{
				return DungeonData.Direction.NORTHEAST;
			}
			if (dir == DungeonData.Direction.WEST)
			{
				return DungeonData.Direction.EAST;
			}
			if (dir == DungeonData.Direction.NORTHWEST)
			{
				return DungeonData.Direction.SOUTHEAST;
			}
			return DungeonData.Direction.SOUTH;
		}

		// Token: 0x06005047 RID: 20551 RVA: 0x001C26C8 File Offset: 0x001C08C8
		public static float GetAngleFromDirection(DungeonData.Direction dir)
		{
			if (dir == DungeonData.Direction.NORTH)
			{
				return 90f;
			}
			if (dir == DungeonData.Direction.NORTHEAST)
			{
				return 45f;
			}
			if (dir == DungeonData.Direction.EAST)
			{
				return 0f;
			}
			if (dir == DungeonData.Direction.SOUTHEAST)
			{
				return 315f;
			}
			if (dir == DungeonData.Direction.SOUTH)
			{
				return 270f;
			}
			if (dir == DungeonData.Direction.SOUTHWEST)
			{
				return 225f;
			}
			if (dir == DungeonData.Direction.WEST)
			{
				return 180f;
			}
			if (dir == DungeonData.Direction.NORTHWEST)
			{
				return 135f;
			}
			return 0f;
		}

		// Token: 0x06005048 RID: 20552 RVA: 0x001C2744 File Offset: 0x001C0944
		public void InitializeCoreData(List<RoomHandler> r)
		{
			this.rooms = r;
		}

		// Token: 0x06005049 RID: 20553 RVA: 0x001C2750 File Offset: 0x001C0950
		private void PreprocessDungeonWings()
		{
			if (this.Exit == null || this.Exit.area.PrototypeRoomCategory != PrototypeDungeonRoom.RoomCategory.EXIT)
			{
				return;
			}
			List<RoomHandler> list = SenseOfDirectionItem.FindPathBetweenNodes(this.Entrance, this.Exit, this.rooms);
			if (list == null)
			{
				return;
			}
			DungeonWingDefinition dungeonWingDefinition = null;
			if (GameManager.Instance.Dungeon.dungeonWingDefinitions.Length > 0)
			{
				dungeonWingDefinition = GameManager.Instance.Dungeon.SelectWingDefinition(true);
			}
			foreach (RoomHandler roomHandler in list)
			{
				roomHandler.IsOnCriticalPath = true;
				if (dungeonWingDefinition != null)
				{
					roomHandler.AssignRoomVisualType(dungeonWingDefinition.includedMaterialIndices.SelectByWeight(), true);
				}
			}
			int num = 0;
			if (dungeonWingDefinition != null)
			{
				dungeonWingDefinition = GameManager.Instance.Dungeon.SelectWingDefinition(false);
			}
			foreach (RoomHandler roomHandler2 in list)
			{
				foreach (RoomHandler roomHandler3 in roomHandler2.connectedRooms)
				{
					if (!roomHandler3.IsOnCriticalPath)
					{
						if (roomHandler3.DungeonWingID == -1)
						{
							roomHandler3.DungeonWingID = num;
							if (dungeonWingDefinition != null)
							{
								roomHandler3.AssignRoomVisualType(dungeonWingDefinition.includedMaterialIndices.SelectByWeight(), true);
							}
							Queue<RoomHandler> queue = new Queue<RoomHandler>();
							queue.Enqueue(roomHandler3);
							while (queue.Count > 0)
							{
								RoomHandler roomHandler4 = queue.Dequeue();
								foreach (RoomHandler roomHandler5 in roomHandler4.connectedRooms)
								{
									if (!roomHandler5.IsOnCriticalPath)
									{
										if (roomHandler5.DungeonWingID == -1)
										{
											roomHandler5.DungeonWingID = num;
											if (dungeonWingDefinition != null)
											{
												roomHandler5.AssignRoomVisualType(dungeonWingDefinition.includedMaterialIndices.SelectByWeight(), true);
											}
											queue.Enqueue(roomHandler5);
										}
									}
								}
							}
							num++;
							if (dungeonWingDefinition != null)
							{
								dungeonWingDefinition = GameManager.Instance.Dungeon.SelectWingDefinition(false);
							}
						}
					}
				}
			}
			foreach (RoomHandler roomHandler6 in this.rooms)
			{
				if (roomHandler6.IsOnCriticalPath)
				{
					BraveUtility.DrawDebugSquare(roomHandler6.area.basePosition.ToVector2(), roomHandler6.area.basePosition.ToVector2() + roomHandler6.area.dimensions.ToVector2(), Color.cyan, 1000f);
				}
				else
				{
					Color color = new Color(1f - (float)roomHandler6.DungeonWingID / 7f, 1f - (float)roomHandler6.DungeonWingID / 7f, (float)roomHandler6.DungeonWingID / 7f);
					BraveUtility.DrawDebugSquare(roomHandler6.area.basePosition.ToVector2(), roomHandler6.area.basePosition.ToVector2() + roomHandler6.area.dimensions.ToVector2(), color, 1000f);
				}
			}
		}

		// Token: 0x0600504A RID: 20554 RVA: 0x001C2B4C File Offset: 0x001C0D4C
		public IEnumerator Apply(TileIndices indices, TilemapDecoSettings decoSettings, tk2dTileMap tilemapRef)
		{
			this.tilemap = tilemapRef;
			this.PreprocessDungeonWings();
			foreach (RoomHandler r in this.rooms)
			{
				r.WriteRoomData(this);
				yield return null;
			}
			this.HandleTrapAreas();
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.NONE)
			{
				this.AddProceduralTeleporters();
			}
			this.ComputeRoomDistanceData();
			this.FloodFillDungeonExterior();
			this.FloodFillDungeonInterior();
			yield return null;
			foreach (RoomHandler roomHandler in this.rooms)
			{
				roomHandler.ProcessFeatures();
				if (indices.tilesetId == GlobalDungeonData.ValidTilesets.WESTGEON)
				{
					for (int i = 0; i < roomHandler.connectedRooms.Count; i++)
					{
						roomHandler.GetExitDefinitionForConnectedRoom(roomHandler.connectedRooms[i]).ProcessWestgeonData();
					}
				}
			}
			this.CalculatePerRoomOcclusionData();
			if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.SHORTCUT && GameManager.Instance.IsLoadingFirstShortcutFloor)
			{
				RoomHandler entrance = this.Entrance;
				DungeonPlaceable dungeonPlaceable = BraveResources.Load("Global Prefabs/Merchant_Rat_Placeable", ".asset") as DungeonPlaceable;
				GameObject gameObject = dungeonPlaceable.InstantiateObject(entrance, new IntVector2(1, 3), false, false);
				IPlayerInteractable[] interfacesInChildren = gameObject.GetInterfacesInChildren<IPlayerInteractable>();
				for (int j = 0; j < interfacesInChildren.Length; j++)
				{
					entrance.RegisterInteractable(interfacesInChildren[j]);
				}
			}
			GameManager.Instance.IsLoadingFirstShortcutFloor = false;
			if (GameManager.Instance.Dungeon.decoSettings.generateLights)
			{
				IEnumerator LightTracker = this.GenerateLights(decoSettings);
				while (LightTracker.MoveNext())
				{
					yield return null;
				}
			}
			this.GenerateInterestingVisuals(decoSettings);
			SecretRoomUtility.ClearPerLevelData();
			if (!GameManager.Instance.Dungeon.debugSettings.DISABLE_SECRET_ROOM_COVERS)
			{
				foreach (RoomHandler roomHandler2 in this.rooms)
				{
					if (roomHandler2.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET)
					{
						roomHandler2.BuildSecretRoomCover();
					}
				}
			}
			yield break;
		}

		// Token: 0x0600504B RID: 20555 RVA: 0x001C2B7C File Offset: 0x001C0D7C
		public void PostProcessFeatures()
		{
			foreach (RoomHandler roomHandler in this.rooms)
			{
				roomHandler.PostProcessFeatures();
			}
			this.HandleFloorSpecificCustomization();
		}

		// Token: 0x0600504C RID: 20556 RVA: 0x001C2BE0 File Offset: 0x001C0DE0
		private void HandleFloorSpecificCustomization()
		{
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON)
			{
				FireplaceController fireplace = UnityEngine.Object.FindObjectOfType<FireplaceController>();
				if (!fireplace)
				{
					return;
				}
				RoomHandler fireplaceRoom = fireplace.transform.position.GetAbsoluteRoom();
				List<MinorBreakable> targetBarrels = new List<MinorBreakable>();
				List<MinorBreakable> allMinorBreakables = StaticReferenceManager.AllMinorBreakables;
				int numToReplace = 2;
				Func<MinorBreakable, bool> func = delegate(MinorBreakable testBarrel)
				{
					int num = -1;
					if (!GameStatsManager.Instance.GetFlag(GungeonFlags.FLAG_ROLLED_BARREL_INTO_FIREPLACE) && testBarrel.transform.position.GetAbsoluteRoom() == fireplaceRoom)
					{
						return false;
					}
					bool flag = testBarrel.CastleReplacedWithWaterDrum;
					if (!flag)
					{
						return false;
					}
					IntVector2 intVector2 = testBarrel.transform.position.IntXY(VectorConversions.Floor);
					if (!GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector2) || GameManager.Instance.Dungeon.data[intVector2].HasWallNeighbor(true, true))
					{
						return false;
					}
					for (int l = 0; l < targetBarrels.Count; l++)
					{
						if (targetBarrels[l].transform.position.GetAbsoluteRoom() == testBarrel.transform.position.GetAbsoluteRoom())
						{
							flag = false;
						}
						else if (targetBarrels.Count >= numToReplace)
						{
							if (Vector2.Distance(fireplace.transform.position, targetBarrels[l].transform.position) > Vector2.Distance(fireplace.transform.position, testBarrel.transform.position))
							{
								num = l;
							}
						}
					}
					if (flag && targetBarrels.Count < numToReplace)
					{
						targetBarrels.Add(testBarrel);
						return true;
					}
					if (flag && num != -1)
					{
						targetBarrels[num] = testBarrel;
						return true;
					}
					return false;
				};
				for (int i = 0; i < allMinorBreakables.Count; i++)
				{
					func(allMinorBreakables[i]);
				}
				DungeonPlaceable dungeonPlaceable = BraveResources.Load("Drum_Water_Castle", ".asset") as DungeonPlaceable;
				for (int j = 0; j < targetBarrels.Count; j++)
				{
					Vector3 vector = targetBarrels[j].transform.position + new Vector3(0.75f, 0f, 0f);
					RoomHandler absoluteRoom = targetBarrels[j].transform.position.GetAbsoluteRoom();
					IntVector2 intVector = vector.IntXY(VectorConversions.Floor) - absoluteRoom.area.basePosition;
					GameObject gameObject = dungeonPlaceable.InstantiateObject(absoluteRoom, intVector, false, false);
					gameObject.transform.position = gameObject.transform.position;
					KickableObject componentInChildren = gameObject.GetComponentInChildren<KickableObject>();
					if (componentInChildren)
					{
						componentInChildren.specRigidbody.Reinitialize();
						componentInChildren.rollSpeed = 3f;
						componentInChildren.AllowTopWallTraversal = true;
						absoluteRoom.RegisterInteractable(componentInChildren);
					}
					KickableObject component = targetBarrels[j].GetComponent<KickableObject>();
					if (component)
					{
						component.ForceDeregister();
					}
					UnityEngine.Object.Destroy(targetBarrels[j].gameObject);
				}
				if (targetBarrels.Count < numToReplace)
				{
					for (int k = 0; k < this.rooms.Count; k++)
					{
						if (this.rooms[k].IsShop)
						{
							IntVector2 bestRewardLocation = this.rooms[k].GetBestRewardLocation(IntVector2.One * 2, RoomHandler.RewardLocationStyle.Original, false);
							GameObject gameObject2 = dungeonPlaceable.InstantiateObject(this.rooms[k], bestRewardLocation - this.rooms[k].area.basePosition, false, false);
							KickableObject componentInChildren2 = gameObject2.GetComponentInChildren<KickableObject>();
							if (componentInChildren2)
							{
								componentInChildren2.rollSpeed = 3f;
								componentInChildren2.AllowTopWallTraversal = true;
								this.rooms[k].RegisterInteractable(componentInChildren2);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600504D RID: 20557 RVA: 0x001C2EB4 File Offset: 0x001C10B4
		private void HandleTrapAreas()
		{
			foreach (PathingTrapController pathingTrapController in UnityEngine.Object.FindObjectsOfType<PathingTrapController>())
			{
				if (!pathingTrapController.specRigidbody)
				{
					return;
				}
				pathingTrapController.specRigidbody.Initialize();
				RoomHandler absoluteRoomFromPosition = this.GetAbsoluteRoomFromPosition(pathingTrapController.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Round));
				PathMover component = pathingTrapController.GetComponent<PathMover>();
				Vector2 unitDimensions = pathingTrapController.specRigidbody.UnitDimensions;
				ResizableCollider component2 = pathingTrapController.GetComponent<ResizableCollider>();
				if (component2)
				{
					if (component2.IsHorizontal)
					{
						unitDimensions.x = component2.NumTiles;
					}
					else
					{
						unitDimensions.y = component2.NumTiles;
					}
				}
				Vector2 vector = Vector2Extensions.max;
				Vector2 vector2 = Vector2Extensions.min;
				for (int j = 0; j < component.Path.nodes.Count; j++)
				{
					Vector2 vector3 = absoluteRoomFromPosition.area.basePosition.ToVector2() + component.Path.nodes[j].RoomPosition;
					vector = Vector2.Min(vector, vector3);
					vector2 = Vector2.Max(vector2, vector3 + unitDimensions);
				}
				IntVector2 intVector = vector.ToIntVector2(VectorConversions.Floor);
				IntVector2 intVector2 = vector2.ToIntVector2(VectorConversions.Floor);
				for (int k = intVector.x; k <= intVector2.x; k++)
				{
					for (int l = intVector.y; l <= intVector2.y; l++)
					{
						this[k, l].IsTrapZone = true;
					}
				}
			}
			foreach (ProjectileTrapController projectileTrapController in UnityEngine.Object.FindObjectsOfType<ProjectileTrapController>())
			{
				IntVector2 intVector3 = projectileTrapController.shootPoint.position.IntXY(VectorConversions.Floor);
				IntVector2 intVector2FromDirection = DungeonData.GetIntVector2FromDirection(projectileTrapController.shootDirection);
				if (!(intVector2FromDirection == IntVector2.Zero))
				{
					IntVector2 intVector4 = intVector3;
					for (;;)
					{
						if (!this.CheckInBoundsAndValid(intVector4) || this.isWall(intVector4.x, intVector4.y))
						{
							if (!(intVector3 == intVector4))
							{
								break;
							}
						}
						else
						{
							this[intVector4].IsTrapZone = true;
						}
						intVector4 += intVector2FromDirection;
					}
				}
			}
		}

		// Token: 0x0600504E RID: 20558 RVA: 0x001C311C File Offset: 0x001C131C
		private void AddProceduralTeleporters()
		{
			List<List<RoomHandler>> list = new List<List<RoomHandler>>();
			List<RoomHandler> roomsContainingTeleporters = new List<RoomHandler>();
			Func<RoomHandler, bool> func = delegate(RoomHandler r)
			{
				if (r.area.IsProceduralRoom)
				{
					return false;
				}
				if (!r.EverHadEnemies)
				{
					return false;
				}
				if (r.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.REWARD || r.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET)
				{
					return false;
				}
				if (r.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS || r.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.EXIT)
				{
					return false;
				}
				if (r.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.NORMAL && r.area.PrototypeRoomNormalSubcategory == PrototypeDungeonRoom.RoomNormalSubCategory.TRAP)
				{
					return false;
				}
				for (int n = 0; n < r.connectedRooms.Count; n++)
				{
					if (roomsContainingTeleporters.Contains(r.connectedRooms[n]))
					{
						return false;
					}
				}
				return true;
			};
			for (int i = 0; i < this.rooms.Count; i++)
			{
				if (Minimap.Instance.HasTeleporterIcon(this.rooms[i]))
				{
					roomsContainingTeleporters.Add(this.rooms[i]);
				}
				if (!roomsContainingTeleporters.Contains(this.rooms[i]) && this.rooms[i].connectedRooms.Count >= 4)
				{
					this.rooms[i].AddProceduralTeleporterToRoom();
					roomsContainingTeleporters.Add(this.rooms[i]);
				}
				if (this.rooms[i].IsLoopMember)
				{
					List<RoomHandler> list2 = null;
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j][0].LoopGuid.Equals(this.rooms[i].LoopGuid))
						{
							list2 = list[j];
							break;
						}
					}
					if (list2 != null)
					{
						list2.Add(this.rooms[i]);
					}
					else
					{
						list.Add(new List<RoomHandler> { this.rooms[i] });
					}
				}
				else if (!roomsContainingTeleporters.Contains(this.rooms[i]) && this.rooms[i].connectedRooms.Count == 1)
				{
					if (func(this.rooms[i]))
					{
						this.rooms[i].AddProceduralTeleporterToRoom();
						roomsContainingTeleporters.Add(this.rooms[i]);
					}
					else if (func(this.rooms[i].connectedRooms[0]))
					{
						this.rooms[i].connectedRooms[0].AddProceduralTeleporterToRoom();
						roomsContainingTeleporters.Add(this.rooms[i].connectedRooms[0]);
					}
				}
			}
			Func<RoomHandler, int> func2 = delegate(RoomHandler r)
			{
				int num4 = int.MaxValue;
				for (int n = 0; n < roomsContainingTeleporters.Count; n++)
				{
					int num5 = IntVector2.ManhattanDistance(roomsContainingTeleporters[n].Epicenter, r.Epicenter);
					if (num5 < num4)
					{
						num4 = num5;
					}
				}
				return num4;
			};
			for (int k = 0; k < list.Count; k++)
			{
				List<RoomHandler> list3 = list[k];
				int num = Mathf.Max(1, Mathf.RoundToInt((float)list3.Count / 4f));
				for (int l = 0; l < num; l++)
				{
					RoomHandler roomHandler = null;
					int num2 = int.MinValue;
					for (int m = 0; m < list3.Count; m++)
					{
						if (func(list3[m]))
						{
							int num3 = func2(list3[m]);
							if (list3[m].connectedRooms.Count > 2)
							{
								num3 += 10;
							}
							if (num3 > num2)
							{
								roomHandler = list3[m];
								num2 = num3;
							}
						}
					}
					if (roomHandler != null)
					{
						roomHandler.AddProceduralTeleporterToRoom();
						if (!roomsContainingTeleporters.Contains(roomHandler))
						{
							roomsContainingTeleporters.Add(roomHandler);
						}
					}
				}
			}
		}

		// Token: 0x0600504F RID: 20559 RVA: 0x001C34AC File Offset: 0x001C16AC
		public void PostGenerationCleanup()
		{
			for (int i = 0; i < this.Width; i++)
			{
				for (int j = 0; j < this.Height; j++)
				{
					bool flag = true;
					if (this.cellData[i][j] != null && this.cellData[i][j].cellVisualData.IsFeatureCell)
					{
						flag = false;
					}
					if (flag)
					{
						for (int k = -3; k <= 3; k++)
						{
							for (int l = -3; l <= 3; l++)
							{
								if (this.CheckInBounds(i + k, j + l) && this.cellData[i + k][j + l] != null && this.cellData[i + k][j + l].type != CellType.WALL)
								{
									flag = false;
								}
								if (!flag)
								{
									break;
								}
							}
							if (!flag)
							{
								break;
							}
						}
					}
					if (flag)
					{
						this.cellData[i][j] = null;
					}
					else if (this.cellData[i][j].type != CellType.WALL)
					{
						bool isNextToWall = this.cellData[i][j].isNextToWall;
					}
				}
			}
		}

		// Token: 0x06005050 RID: 20560 RVA: 0x001C35D8 File Offset: 0x001C17D8
		public RoomHandler GetAbsoluteRoomFromPosition(IntVector2 pos)
		{
			CellData cellData = ((!this.CheckInBounds(pos)) ? null : this[pos]);
			if (cellData == null)
			{
				float num = float.MaxValue;
				RoomHandler roomHandler = null;
				for (int i = 0; i < this.rooms.Count; i++)
				{
					float num2 = BraveMathCollege.DistToRectangle(pos.ToCenterVector2(), this.rooms[i].area.basePosition.ToVector2(), this.rooms[i].area.dimensions.ToVector2());
					if (num2 < num)
					{
						num = num2;
						roomHandler = this.rooms[i];
					}
				}
				return roomHandler;
			}
			if (cellData.parentRoom == null)
			{
				return cellData.nearestRoom;
			}
			return cellData.parentRoom;
		}

		// Token: 0x06005051 RID: 20561 RVA: 0x001C36A0 File Offset: 0x001C18A0
		public RoomHandler GetRoomFromPosition(IntVector2 pos)
		{
			CellData cellData = this[pos];
			return cellData.parentRoom;
		}

		// Token: 0x06005052 RID: 20562 RVA: 0x001C36BC File Offset: 0x001C18BC
		public CellVisualData.CellFloorType GetFloorTypeFromPosition(IntVector2 pos)
		{
			if (!this.CheckInBoundsAndValid(pos))
			{
				return CellVisualData.CellFloorType.Stone;
			}
			return this[pos].cellVisualData.floorType;
		}

		// Token: 0x06005053 RID: 20563 RVA: 0x001C36E0 File Offset: 0x001C18E0
		public CellType GetCellTypeSafe(IntVector2 pos)
		{
			if (!this.CheckInBounds(pos))
			{
				return CellType.WALL;
			}
			CellData cellData = this[pos];
			if (cellData == null)
			{
				return CellType.WALL;
			}
			return cellData.type;
		}

		// Token: 0x06005054 RID: 20564 RVA: 0x001C3714 File Offset: 0x001C1914
		public CellType GetCellTypeSafe(int x, int y)
		{
			if (!this.CheckInBounds(x, y))
			{
				return CellType.WALL;
			}
			CellData cellData = this[x, y];
			if (cellData == null)
			{
				return CellType.WALL;
			}
			return cellData.type;
		}

		// Token: 0x06005055 RID: 20565 RVA: 0x001C3748 File Offset: 0x001C1948
		public CellData GetCellSafe(IntVector2 pos)
		{
			return (!this.CheckInBounds(pos)) ? null : this[pos];
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x001C3764 File Offset: 0x001C1964
		public CellData GetCellSafe(int x, int y)
		{
			return (!this.CheckInBounds(x, y)) ? null : this[x, y];
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x001C3784 File Offset: 0x001C1984
		private static bool CheckCellNeedsAdditionalLight(List<IntVector2> positions, RoomHandler room, CellData currentCell)
		{
			int num = ((!room.area.IsProceduralRoom) ? 10 : 20);
			if (currentCell.isExitCell)
			{
				return false;
			}
			if (currentCell.type == CellType.WALL)
			{
				return false;
			}
			bool flag = true;
			for (int i = 0; i < positions.Count; i++)
			{
				int num2 = IntVector2.ManhattanDistance(positions[i] + room.area.basePosition, currentCell.position);
				if (num2 <= num)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				positions.Add(currentCell.position - room.area.basePosition);
			}
			return flag;
		}

		// Token: 0x06005058 RID: 20568 RVA: 0x001C3834 File Offset: 0x001C1A34
		private void PostprocessLightPositions(List<IntVector2> positions, RoomHandler room)
		{
			DungeonData.CheckCellNeedsAdditionalLight(positions, room, this[room.GetCenterCell()]);
			for (int i = 0; i < room.Cells.Count; i++)
			{
				CellData cellData = this[room.Cells[i]];
				DungeonData.CheckCellNeedsAdditionalLight(positions, room, cellData);
			}
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x001C3890 File Offset: 0x001C1A90
		public void ReplicateLighting(CellData sourceCell, CellData targetCell)
		{
			Vector3 vector = sourceCell.cellVisualData.lightObject.transform.position - sourceCell.position.ToVector2().ToVector3ZisY(0f) + targetCell.position.ToVector2().ToVector3ZisY(0f);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(sourceCell.cellVisualData.lightObject, vector, Quaternion.identity);
			gameObject.transform.parent = sourceCell.cellVisualData.lightObject.transform.parent;
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
			{
				this[targetCell.position + IntVector2.Down].cellVisualData.containsObjectSpaceStamp = true;
			}
			targetCell.cellVisualData.containsLight = true;
			targetCell.cellVisualData.lightObject = gameObject;
			targetCell.cellVisualData.facewallLightStampData = sourceCell.cellVisualData.facewallLightStampData;
			targetCell.cellVisualData.sidewallLightStampData = sourceCell.cellVisualData.sidewallLightStampData;
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x001C39A0 File Offset: 0x001C1BA0
		public void GenerateLightsForRoom(TilemapDecoSettings decoSettings, RoomHandler rh, Transform lightParent, DungeonData.LightGenerationStyle style = DungeonData.LightGenerationStyle.STANDARD)
		{
			if (!GameManager.Instance.Dungeon.roomMaterialDefinitions[rh.RoomVisualSubtype].useLighting)
			{
				return;
			}
			bool flag = decoSettings.lightCookies.Length > 0;
			List<Tuple<IntVector2, float>> list = new List<Tuple<IntVector2, float>>();
			bool flag2 = false;
			List<IntVector2> list2;
			int num;
			if (rh.area != null && !rh.area.IsProceduralRoom && !rh.area.prototypeRoom.usesProceduralLighting)
			{
				list2 = rh.GatherManualLightPositions();
				num = list2.Count;
			}
			else
			{
				flag2 = true;
				list2 = rh.GatherOptimalLightPositions(decoSettings);
				num = list2.Count;
				if (rh.area != null && rh.area.prototypeRoom != null)
				{
					this.PostprocessLightPositions(list2, rh);
				}
			}
			if (rh.area.prototypeRoom != null)
			{
				for (int i = 0; i < rh.area.instanceUsedExits.Count; i++)
				{
					RuntimeRoomExitData runtimeRoomExitData = rh.area.exitToLocalDataMap[rh.area.instanceUsedExits[i]];
					RuntimeExitDefinition runtimeExitDefinition = rh.exitDefinitionsByExit[runtimeRoomExitData];
					if (runtimeRoomExitData.TotalExitLength > 4 && !runtimeExitDefinition.containsLight)
					{
						IntVector2 intVector = ((!runtimeRoomExitData.jointedExit) ? runtimeExitDefinition.GetLinearMidpoint(rh) : (runtimeRoomExitData.ExitOrigin - IntVector2.One));
						list.Add(new Tuple<IntVector2, float>(intVector, 0.5f));
						runtimeExitDefinition.containsLight = true;
					}
				}
			}
			GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
			float lightCullingPercentage = decoSettings.lightCullingPercentage;
			if (flag2 && lightCullingPercentage > 0f)
			{
				int num2 = Mathf.FloorToInt((float)list2.Count * lightCullingPercentage);
				int num3 = Mathf.FloorToInt((float)list.Count * lightCullingPercentage);
				if (num2 == 0 && num3 == 0 && list2.Count + list.Count > 4)
				{
					num2 = 1;
				}
				while (num2 > 0 && list2.Count > 0)
				{
					list2.RemoveAt(UnityEngine.Random.Range(0, list2.Count));
					num2--;
				}
				while (num3 > 0 && list.Count > 0)
				{
					list.RemoveAt(UnityEngine.Random.Range(0, list.Count));
					num3--;
				}
			}
			int count = list2.Count;
			if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.NONE && (tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON || tilesetId == GlobalDungeonData.ValidTilesets.GUNGEON || tilesetId == GlobalDungeonData.ValidTilesets.CATACOMBGEON) && (flag2 || rh.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.NORMAL || rh.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.HUB || rh.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.CONNECTOR))
			{
				list2.AddRange(rh.GatherPitLighting(decoSettings, list2));
			}
			for (int j = 0; j < list2.Count + list.Count; j++)
			{
				IntVector2 intVector2 = IntVector2.NegOne;
				float num4 = 1f;
				bool flag3 = false;
				if (j < list2.Count && j >= count)
				{
					flag3 = true;
					num4 = 0.6f;
				}
				if (j < list2.Count)
				{
					intVector2 = rh.area.basePosition + list2[j];
				}
				else
				{
					intVector2 = rh.area.basePosition + list[j - list2.Count].First;
					num4 = list[j - list2.Count].Second;
				}
				bool flag4 = false;
				if (flag && flag2 && intVector2 == rh.GetCenterCell())
				{
					flag4 = true;
				}
				IntVector2 intVector3 = intVector2 + IntVector2.Up;
				bool flag5 = j >= num;
				bool flag6 = false;
				Vector3 vector = Vector3.zero;
				if (this[intVector2 + IntVector2.Up].type == CellType.WALL)
				{
					this[intVector3].cellVisualData.lightDirection = DungeonData.Direction.NORTH;
					vector = Vector3.down;
				}
				else if (this[intVector2 + IntVector2.Right].type == CellType.WALL)
				{
					this[intVector3].cellVisualData.lightDirection = DungeonData.Direction.EAST;
				}
				else if (this[intVector2 + IntVector2.Left].type == CellType.WALL)
				{
					this[intVector3].cellVisualData.lightDirection = DungeonData.Direction.WEST;
				}
				else if (this[intVector2 + IntVector2.Down].type == CellType.WALL)
				{
					flag6 = true;
					this[intVector3].cellVisualData.lightDirection = DungeonData.Direction.SOUTH;
				}
				else
				{
					this[intVector3].cellVisualData.lightDirection = (DungeonData.Direction)(-1);
				}
				int num5 = rh.RoomVisualSubtype;
				float num6 = 0f;
				if (rh.area.prototypeRoom != null)
				{
					PrototypeDungeonRoomCellData prototypeDungeonRoomCellData = ((j >= list2.Count) ? rh.area.prototypeRoom.ForceGetCellDataAtPoint(list[j - list2.Count].First.x, list[j - list2.Count].First.y) : rh.area.prototypeRoom.ForceGetCellDataAtPoint(list2[j].x, list2[j].y));
					if (prototypeDungeonRoomCellData != null && prototypeDungeonRoomCellData.containsManuallyPlacedLight)
					{
						num5 = prototypeDungeonRoomCellData.lightStampIndex;
						num6 = (float)prototypeDungeonRoomCellData.lightPixelsOffsetY / 16f;
					}
				}
				if (num5 < 0 || num5 >= GameManager.Instance.Dungeon.roomMaterialDefinitions.Length)
				{
					num5 = 0;
				}
				DungeonMaterial dungeonMaterial = GameManager.Instance.Dungeon.roomMaterialDefinitions[num5];
				int num7 = -1;
				GameObject gameObject;
				if (style == DungeonData.LightGenerationStyle.FORCE_COLOR || style == DungeonData.LightGenerationStyle.RAT_HALLWAY)
				{
					num7 = 0;
					gameObject = dungeonMaterial.lightPrefabs.elements[0].gameObject;
				}
				else
				{
					gameObject = dungeonMaterial.lightPrefabs.SelectByWeight(out num7, false);
				}
				if ((!dungeonMaterial.facewallLightStamps[num7].CanBeTopWallLight && flag6) || (!dungeonMaterial.facewallLightStamps[num7].CanBeCenterLight && flag5))
				{
					if (num7 >= dungeonMaterial.facewallLightStamps.Count)
					{
						num7 = 0;
					}
					num7 = dungeonMaterial.facewallLightStamps[num7].FallbackIndex;
					gameObject = dungeonMaterial.lightPrefabs.elements[num7].gameObject;
				}
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, intVector3.ToVector3(0f), Quaternion.identity);
				gameObject2.transform.parent = lightParent;
				gameObject2.transform.position = intVector3.ToCenterVector3((float)intVector3.y + decoSettings.lightHeight) + new Vector3(0f, num6, 0f) + vector;
				ShadowSystem componentInChildren = gameObject2.GetComponentInChildren<ShadowSystem>();
				Light componentInChildren2 = gameObject2.GetComponentInChildren<Light>();
				if (componentInChildren2 != null)
				{
					componentInChildren2.intensity *= num4;
				}
				if (style == DungeonData.LightGenerationStyle.FORCE_COLOR || style == DungeonData.LightGenerationStyle.RAT_HALLWAY)
				{
					SceneLightManager component = gameObject2.GetComponent<SceneLightManager>();
					if (component)
					{
						Color[] array = new Color[] { component.validColors[0] };
						component.validColors = array;
					}
				}
				if (flag3 && componentInChildren != null)
				{
					if (componentInChildren2)
					{
						componentInChildren2.range += (float)((GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.CATACOMBGEON) ? 3 : 5);
					}
					componentInChildren.ignoreCustomFloorLight = true;
				}
				if (flag4 && flag && componentInChildren != null)
				{
					componentInChildren.uLightCookie = decoSettings.GetRandomLightCookie();
					componentInChildren.uLightCookieAngle = UnityEngine.Random.Range(0f, 6.28f);
					componentInChildren2.intensity *= 1.5f;
				}
				if (this[intVector3].cellVisualData.lightDirection == DungeonData.Direction.NORTH)
				{
					bool flag7 = true;
					for (int k = -2; k < 3; k++)
					{
						if (this[intVector3 + IntVector2.Right * k].type == CellType.FLOOR)
						{
							flag7 = false;
							break;
						}
					}
					if (flag7 && componentInChildren)
					{
						GameObject gameObject3 = (GameObject)BraveResources.Load("Global VFX/Wall_Light_Cookie", ".prefab");
						GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(gameObject3);
						Transform transform = gameObject4.transform;
						transform.parent = gameObject2.transform;
						transform.localPosition = Vector3.zero;
						componentInChildren.PersonalCookies.Add(gameObject4.GetComponent<Renderer>());
					}
				}
				CellData cellData = this[intVector3 + new IntVector2(0, Mathf.RoundToInt(num6))];
				if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
				{
					this[cellData.position + IntVector2.Down].cellVisualData.containsObjectSpaceStamp = true;
				}
				BraveUtility.DrawDebugSquare(cellData.position.ToVector2(), Color.magenta, 1000f);
				cellData.cellVisualData.containsLight = true;
				cellData.cellVisualData.lightObject = gameObject2;
				LightStampData lightStampData = dungeonMaterial.facewallLightStamps[num7];
				LightStampData lightStampData2 = dungeonMaterial.sidewallLightStamps[num7];
				cellData.cellVisualData.facewallLightStampData = lightStampData;
				cellData.cellVisualData.sidewallLightStampData = lightStampData2;
			}
		}

		// Token: 0x0600505B RID: 20571 RVA: 0x001C4374 File Offset: 0x001C2574
		private IEnumerator GenerateLights(TilemapDecoSettings decoSettings)
		{
			Transform lightParent = new GameObject("_Lights").transform;
			bool lightCookiesAvailable = decoSettings.lightCookies.Length > 0;
			int rhIterator = 0;
			foreach (RoomHandler rh in this.rooms)
			{
				rhIterator++;
				this.GenerateLightsForRoom(decoSettings, rh, lightParent, DungeonData.LightGenerationStyle.STANDARD);
				if (rhIterator % 5 == 0)
				{
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x001C4398 File Offset: 0x001C2598
		private void GenerateInterestingVisuals(TilemapDecoSettings dungeonDecoSettings)
		{
			List<IntVector2> patchPoints = new List<IntVector2>();
			for (int i = 0; i < this.Width; i++)
			{
				for (int j = 0; j < this.Height; j++)
				{
					if (UnityEngine.Random.value < dungeonDecoSettings.decoPatchFrequency)
					{
						patchPoints.Add(new IntVector2(i, j));
					}
				}
			}
			Func<IntVector2, int> func = delegate(IntVector2 a)
			{
				int num22 = int.MaxValue;
				for (int num23 = 0; num23 < patchPoints.Count; num23++)
				{
					int num24 = IntVector2.ManhattanDistance(patchPoints[num23], a);
					num22 = Mathf.Min(num22, num24);
				}
				return num22;
			};
			for (int k = 0; k < this.Width; k++)
			{
				for (int l = 0; l < this.Height; l++)
				{
					CellData cellData = this.cellData[k][l];
					if (cellData != null)
					{
						if (cellData.type != CellType.WALL || cellData.IsLowerFaceWall())
						{
							if (!cellData.doesDamage)
							{
								TilemapDecoSettings.DecoStyle decoStyle = dungeonDecoSettings.decalLayerStyle;
								int num = dungeonDecoSettings.decalSize;
								int num2 = dungeonDecoSettings.decalSpacing;
								TilemapDecoSettings.DecoStyle decoStyle2 = dungeonDecoSettings.patternLayerStyle;
								int num3 = dungeonDecoSettings.patternSize;
								int num4 = dungeonDecoSettings.patternSpacing;
								bool flag = false;
								bool flag2 = false;
								if (cellData.cellVisualData.roomVisualTypeIndex >= 0 && cellData.cellVisualData.roomVisualTypeIndex < GameManager.Instance.Dungeon.roomMaterialDefinitions.Length)
								{
									DungeonMaterial dungeonMaterial = GameManager.Instance.Dungeon.roomMaterialDefinitions[cellData.cellVisualData.roomVisualTypeIndex];
									if (dungeonMaterial.usesDecalLayer)
									{
										flag = true;
										decoStyle = dungeonMaterial.decalLayerStyle;
										num = dungeonMaterial.decalSize;
										num2 = dungeonMaterial.decalSpacing;
									}
									if (dungeonMaterial.usesPatternLayer)
									{
										flag2 = true;
										decoStyle2 = dungeonMaterial.patternLayerStyle;
										num3 = dungeonMaterial.patternSize;
										num4 = dungeonMaterial.patternSpacing;
									}
								}
								float num5 = -0.35f + (float)num / 10f;
								float num6 = -0.35f + (float)num3 / 10f;
								if (flag)
								{
									switch (decoStyle)
									{
									case TilemapDecoSettings.DecoStyle.GROW_FROM_WALLS:
										if (cellData.HasMossyNeighbor(this) && UnityEngine.Random.value > (float)(10 - num) / 10f)
										{
											cellData.cellVisualData.isDecal = true;
										}
										if (cellData.IsLowerFaceWall())
										{
											cellData.cellVisualData.isDecal = true;
										}
										break;
									case TilemapDecoSettings.DecoStyle.PERLIN_NOISE:
									{
										float num7 = Noise.Generate((float)cellData.position.x / (4f + (float)num2), (float)cellData.position.y / (4f + (float)num2));
										if (num7 < num5)
										{
											cellData.cellVisualData.isDecal = true;
										}
										break;
									}
									case TilemapDecoSettings.DecoStyle.HORIZONTAL_STRIPES:
									{
										int num8 = cellData.position.y % (num + num2);
										if (num8 < num)
										{
											cellData.cellVisualData.isDecal = true;
										}
										break;
									}
									case TilemapDecoSettings.DecoStyle.VERTICAL_STRIPES:
									{
										int num9 = cellData.position.x % (num + num2);
										if (num9 < num)
										{
											cellData.cellVisualData.isDecal = true;
										}
										break;
									}
									case TilemapDecoSettings.DecoStyle.AROUND_LIGHTS:
										if (cellData.cellVisualData.distanceToNearestLight <= num)
										{
											float num10 = (float)cellData.cellVisualData.distanceToNearestLight / ((float)num * 1f);
											if (UnityEngine.Random.value > num10)
											{
												cellData.cellVisualData.isDecal = true;
											}
										}
										break;
									case TilemapDecoSettings.DecoStyle.WATER_CHANNELS:
										break;
									case TilemapDecoSettings.DecoStyle.PATCHES:
									{
										int num11 = func(cellData.position);
										if (num11 < num || (num11 == num && (double)UnityEngine.Random.value > 0.5))
										{
											cellData.cellVisualData.isDecal = true;
										}
										break;
									}
									default:
										if (decoStyle == TilemapDecoSettings.DecoStyle.NONE)
										{
											cellData.cellVisualData.isDecal = false;
										}
										break;
									}
								}
								if (flag2)
								{
									switch (decoStyle2)
									{
									case TilemapDecoSettings.DecoStyle.GROW_FROM_WALLS:
										if (cellData.HasPatternNeighbor(this) && UnityEngine.Random.value > (float)(10 - num3) / 10f)
										{
											cellData.cellVisualData.isPattern = true;
										}
										if (cellData.IsLowerFaceWall())
										{
											cellData.cellVisualData.isPattern = true;
										}
										break;
									case TilemapDecoSettings.DecoStyle.PERLIN_NOISE:
									{
										float num12 = Noise.Generate((float)cellData.position.x / (4f + (float)num4), (float)cellData.position.y / (4f + (float)num4));
										if (num12 < num6)
										{
											cellData.cellVisualData.isPattern = true;
										}
										break;
									}
									case TilemapDecoSettings.DecoStyle.HORIZONTAL_STRIPES:
									{
										int num13 = cellData.position.y % (num3 + num4);
										if (num13 < num3)
										{
											cellData.cellVisualData.isPattern = true;
										}
										break;
									}
									case TilemapDecoSettings.DecoStyle.VERTICAL_STRIPES:
									{
										int num14 = cellData.position.x % (num3 + num4);
										if (num14 < num3)
										{
											cellData.cellVisualData.isPattern = true;
										}
										break;
									}
									case TilemapDecoSettings.DecoStyle.AROUND_LIGHTS:
										if (cellData.cellVisualData.distanceToNearestLight <= num3)
										{
											cellData.cellVisualData.isPattern = true;
										}
										break;
									case TilemapDecoSettings.DecoStyle.WATER_CHANNELS:
										break;
									case TilemapDecoSettings.DecoStyle.PATCHES:
									{
										int num15 = func(cellData.position);
										if (num15 < num3 || (num15 == num3 && (double)UnityEngine.Random.value > 0.5))
										{
											cellData.cellVisualData.isPattern = true;
										}
										break;
									}
									default:
										if (decoStyle2 == TilemapDecoSettings.DecoStyle.NONE)
										{
											cellData.cellVisualData.isPattern = false;
										}
										break;
									}
								}
							}
						}
					}
				}
			}
			if (dungeonDecoSettings.patternExpansion > 0)
			{
				for (int m = 0; m < dungeonDecoSettings.patternExpansion; m++)
				{
					HashSet<CellData> hashSet = new HashSet<CellData>();
					for (int n = 0; n < this.Width; n++)
					{
						for (int num16 = 0; num16 < this.Height; num16++)
						{
							CellData cellData2 = this.cellData[n][num16];
							if (cellData2 != null)
							{
								if (!cellData2.cellVisualData.isPattern && cellData2.HasPatternNeighbor(this) && !cellData2.doesDamage)
								{
									hashSet.Add(cellData2);
								}
							}
						}
					}
					foreach (CellData cellData3 in hashSet)
					{
						cellData3.cellVisualData.isPattern = true;
					}
				}
			}
			if (dungeonDecoSettings.decalExpansion > 0)
			{
				for (int num17 = 0; num17 < dungeonDecoSettings.decalExpansion; num17++)
				{
					HashSet<CellData> hashSet2 = new HashSet<CellData>();
					for (int num18 = 0; num18 < this.Width; num18++)
					{
						for (int num19 = 0; num19 < this.Height; num19++)
						{
							CellData cellData4 = this.cellData[num18][num19];
							if (cellData4 != null)
							{
								if (!cellData4.cellVisualData.isDecal && cellData4.HasMossyNeighbor(this) && !cellData4.doesDamage)
								{
									hashSet2.Add(cellData4);
								}
							}
						}
					}
					foreach (CellData cellData5 in hashSet2)
					{
						cellData5.cellVisualData.isDecal = true;
					}
				}
			}
			if (dungeonDecoSettings.debug_view)
			{
				for (int num20 = 0; num20 < this.Width; num20++)
				{
					for (int num21 = 0; num21 < this.Height; num21++)
					{
						CellData cellData6 = this.cellData[num20][num21];
						if (cellData6 != null)
						{
							if (cellData6.cellVisualData.isDecal && cellData6.cellVisualData.isPattern)
							{
								this.DebugDrawCross(cellData6.position.ToCenterVector3(-10f), Color.grey);
							}
							else if (cellData6.cellVisualData.isDecal)
							{
								this.DebugDrawCross(cellData6.position.ToCenterVector3(-10f), Color.green);
							}
							else if (cellData6.cellVisualData.isPattern)
							{
								this.DebugDrawCross(cellData6.position.ToCenterVector3(-10f), Color.red);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x001C4C28 File Offset: 0x001C2E28
		private void DoRoomDistanceDebug()
		{
			for (int i = 0; i < this.Width; i++)
			{
				for (int j = 0; j < this.Height; j++)
				{
					CellData cellData = this.cellData[i][j];
					Vector3 vector = cellData.position.ToCenterVector3(-10f);
					if (cellData.distanceFromNearestRoom <= (float)Pixelator.Instance.perimeterTileWidth)
					{
						Color color = new Color(cellData.distanceFromNearestRoom / 7f, cellData.distanceFromNearestRoom / 7f, cellData.distanceFromNearestRoom / 7f);
						this.DebugDrawCross(vector, color);
					}
				}
			}
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x001C4CD0 File Offset: 0x001C2ED0
		private void DebugDrawCross(Vector3 centerPoint, Color crosscolor)
		{
			Debug.DrawLine(centerPoint + new Vector3(-0.5f, 0f, 0f), centerPoint + new Vector3(0.5f, 0f, 0f), crosscolor, 1000f);
			Debug.DrawLine(centerPoint + new Vector3(0f, -0.5f, 0f), centerPoint + new Vector3(0f, 0.5f, 0f), crosscolor, 1000f);
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x001C4D5C File Offset: 0x001C2F5C
		private void FloodFillDungeonInterior()
		{
			Stack<CellData> stack = new Stack<CellData>();
			for (int i = 0; i < this.rooms.Count; i++)
			{
				if (this.rooms[i] == this.Entrance || this.rooms[i].IsStartOfWarpWing)
				{
					stack.Push(this[this.rooms[i].GetRandomAvailableCellDumb()]);
				}
			}
			while (stack.Count > 0)
			{
				CellData cellData = stack.Pop();
				if (cellData.type != CellType.WALL)
				{
					List<CellData> cellNeighbors = this.GetCellNeighbors(cellData, false);
					cellData.isGridConnected = true;
					for (int j = 0; j < cellNeighbors.Count; j++)
					{
						if (cellNeighbors[j] != null && cellNeighbors[j].type != CellType.WALL && !cellNeighbors[j].isGridConnected)
						{
							stack.Push(cellNeighbors[j]);
						}
					}
				}
			}
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x001C4E6C File Offset: 0x001C306C
		private void FloodFillDungeonExterior()
		{
			Stack<IntVector2> stack = new Stack<IntVector2>();
			HashSet<IntVector2> hashSet = new HashSet<IntVector2>();
			stack.Push(IntVector2.Zero);
			while (stack.Count > 0)
			{
				IntVector2 intVector = stack.Pop();
				hashSet.Add(intVector);
				CellData cellData = this[intVector];
				if (cellData == null || (cellData.type == CellType.WALL && !cellData.breakable) || cellData.isExitCell)
				{
					if (cellData != null)
					{
						cellData.isRoomInternal = false;
					}
					for (int i = 0; i < IntVector2.Cardinals.Length; i++)
					{
						IntVector2 intVector2 = intVector + IntVector2.Cardinals[i];
						if (!hashSet.Contains(intVector2) && intVector2.x >= 0 && intVector2.y >= 0 && intVector2.x < this.Width && intVector2.y < this.Height)
						{
							stack.Push(intVector2);
						}
					}
				}
			}
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x001C4F78 File Offset: 0x001C3178
		private void ComputeRoomDistanceData()
		{
			Queue<CellData> queue = new Queue<CellData>();
			HashSet<CellData> hashSet = new HashSet<CellData>();
			for (int i = 0; i < this.cellData.Length; i++)
			{
				for (int j = 0; j < this.cellData[i].Length; j++)
				{
					CellData cellData = this.cellData[i][j];
					if (cellData != null)
					{
						if (cellData.distanceFromNearestRoom == 1f)
						{
							queue.Enqueue(cellData);
							hashSet.Add(cellData);
						}
					}
				}
			}
			while (queue.Count > 0)
			{
				CellData cellData2 = queue.Dequeue();
				hashSet.Remove(cellData2);
				List<CellData> cellNeighbors = this.GetCellNeighbors(cellData2, true);
				for (int k = 0; k < cellNeighbors.Count; k++)
				{
					CellData cellData3 = cellNeighbors[k];
					if (cellData3 != null)
					{
						float num = ((k % 2 != 1) ? (cellData2.distanceFromNearestRoom + 1f) : (cellData2.distanceFromNearestRoom + 1.414f));
						if (cellData3.distanceFromNearestRoom > num)
						{
							cellData3.distanceFromNearestRoom = num;
							cellData3.nearestRoom = cellData2.nearestRoom;
							if (!hashSet.Contains(cellData3))
							{
								queue.Enqueue(cellData3);
								hashSet.Add(cellData3);
							}
						}
					}
				}
			}
		}

		// Token: 0x06005062 RID: 20578 RVA: 0x001C50D0 File Offset: 0x001C32D0
		private void CalculatePerRoomOcclusionData()
		{
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x001C50D4 File Offset: 0x001C32D4
		private HashSet<CellData> ComputeRoomDistanceHorizon(HashSet<CellData> horizon, float dist)
		{
			HashSet<CellData> hashSet = new HashSet<CellData>();
			foreach (CellData cellData in horizon)
			{
				List<CellData> cellNeighbors = this.GetCellNeighbors(cellData, true);
				for (int i = 0; i < cellNeighbors.Count; i++)
				{
					CellData cellData2 = cellNeighbors[i];
					if (cellData2 != null && !hashSet.Contains(cellData2))
					{
						float num = ((i % 2 != 1) ? (dist + 1f) : (dist + 1.414f));
						if (cellData2.distanceFromNearestRoom > num)
						{
							cellData2.distanceFromNearestRoom = num;
							cellData2.nearestRoom = cellData.nearestRoom;
							hashSet.Add(cellData2);
						}
					}
				}
			}
			return hashSet;
		}

		// Token: 0x06005064 RID: 20580 RVA: 0x001C51C4 File Offset: 0x001C33C4
		public void CheckIntegrity()
		{
			for (int i = 0; i < this.Width; i++)
			{
				for (int j = 0; j < this.Height; j++)
				{
					if (j > 1 && j < this.Height - 1 && this.cellData[i][j + 1] != null && this.isSingleCellWall(i, j))
					{
						this.cellData[i][j + 1].type = CellType.WALL;
						RoomHandler parentRoom = this.cellData[i][j].parentRoom;
						if (parentRoom != null)
						{
							IntVector2 intVector = new IntVector2(i, j + 1);
							if (parentRoom.RawCells.Remove(intVector))
							{
								parentRoom.Cells.Remove(intVector);
								parentRoom.CellsWithoutExits.Remove(intVector);
							}
						}
					}
					if (this.cellData[i][j] != null)
					{
						if (this.cellData[i][j].type == CellType.FLOOR)
						{
							bool flag = false;
							foreach (CellData cellData in this.GetCellNeighbors(this.cellData[i][j], false))
							{
								if (cellData != null)
								{
									if (cellData.type == CellType.FLOOR)
									{
										flag = true;
									}
								}
							}
							if (!flag)
							{
								this.cellData[i][j].type = CellType.WALL;
							}
						}
					}
				}
			}
			this.ExciseElbows();
		}

		// Token: 0x06005065 RID: 20581 RVA: 0x001C5350 File Offset: 0x001C3550
		protected void ExciseElbows()
		{
			bool flag = true;
			List<CellData> list = new List<CellData>();
			int num = 0;
			while (flag && num < 1000)
			{
				num++;
				list.Clear();
				flag = false;
				for (int i = 0; i < this.Width; i++)
				{
					for (int j = 0; j < this.Height; j++)
					{
						if (this.cellData[i][j] != null)
						{
							if (this.cellData[i][j].isExitCell || (this.cellData[i][j].parentRoom != null && this.cellData[i][j].parentRoom.area.IsProceduralRoom))
							{
								if (this.cellData[i][j].isExitCell && !GameManager.Instance.Dungeon.UsesWallWarpWingDoors)
								{
									bool flag2 = false;
									RoomHandler absoluteRoomFromPosition = this.GetAbsoluteRoomFromPosition(new IntVector2(i, j));
									foreach (RoomHandler roomHandler in absoluteRoomFromPosition.connectedRooms)
									{
										RuntimeExitDefinition exitDefinitionForConnectedRoom = absoluteRoomFromPosition.GetExitDefinitionForConnectedRoom(roomHandler);
										if (((exitDefinitionForConnectedRoom.upstreamExit != null && exitDefinitionForConnectedRoom.upstreamExit.isWarpWingStart) || (exitDefinitionForConnectedRoom.downstreamExit != null && exitDefinitionForConnectedRoom.downstreamExit.isWarpWingStart)) && exitDefinitionForConnectedRoom.ContainsPosition(new IntVector2(i, j)))
										{
											flag2 = true;
											break;
										}
									}
									if (flag2)
									{
										goto IL_221;
									}
								}
								int num2 = 0;
								int num3 = 0;
								for (int k = 0; k < 4; k++)
								{
									CellData cellData = this[new IntVector2(i, j) + IntVector2.Cardinals[k]];
									if (cellData.type != CellType.WALL)
									{
										num2++;
										CellData cellData2 = this[new IntVector2(i, j) + 2 * IntVector2.Cardinals[k]];
										if (cellData2.type != CellType.WALL)
										{
											num3++;
										}
									}
								}
								if (num2 == 2 && num3 != 2)
								{
									list.Add(this.cellData[i][j]);
								}
							}
						}
						IL_221:;
					}
				}
				if (list.Count > 0)
				{
					flag = true;
				}
				foreach (CellData cellData3 in list)
				{
					BraveUtility.DrawDebugSquare(cellData3.position.ToVector2(), Color.yellow, 1000f);
					cellData3.type = CellType.WALL;
					for (int l = 0; l < this.rooms.Count; l++)
					{
						RoomHandler roomHandler2 = this.rooms[l];
						roomHandler2.RawCells.Remove(cellData3.position);
						roomHandler2.Cells.Remove(cellData3.position);
						roomHandler2.CellsWithoutExits.Remove(cellData3.position);
						if (roomHandler2.area.instanceUsedExits != null)
						{
							for (int m = 0; m < roomHandler2.area.instanceUsedExits.Count; m++)
							{
								if (roomHandler2.area.exitToLocalDataMap.ContainsKey(roomHandler2.area.instanceUsedExits[m]))
								{
									RuntimeExitDefinition runtimeExitDefinition = roomHandler2.exitDefinitionsByExit[roomHandler2.area.exitToLocalDataMap[roomHandler2.area.instanceUsedExits[m]]];
									runtimeExitDefinition.RemovePosition(cellData3.position);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06005066 RID: 20582 RVA: 0x001C5758 File Offset: 0x001C3958
		public int GetRoomVisualTypeAtPosition(Vector2 position)
		{
			return this.GetRoomVisualTypeAtPosition(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x001C5778 File Offset: 0x001C3978
		public int GetRoomVisualTypeAtPosition(IntVector2 position)
		{
			return this[position].cellVisualData.roomVisualTypeIndex;
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x001C578C File Offset: 0x001C398C
		public int GetRoomVisualTypeAtPosition(int x, int y)
		{
			if (x < 0 || x >= this.Width || y < 0 || y >= this.Height || this.cellData[x][y] == null)
			{
				return 0;
			}
			return this.cellData[x][y].cellVisualData.roomVisualTypeIndex;
		}

		// Token: 0x06005069 RID: 20585 RVA: 0x001C57E4 File Offset: 0x001C39E4
		public void FakeRegisterDoorFeet(IntVector2 position, bool northSouth)
		{
			if (northSouth)
			{
				this[position].cellVisualData.doorFeetOverrideMode = 1;
				this[position + IntVector2.Right].cellVisualData.doorFeetOverrideMode = 1;
			}
			else
			{
				this[position].cellVisualData.doorFeetOverrideMode = 2;
				this[position + IntVector2.Up].cellVisualData.doorFeetOverrideMode = 2;
			}
		}

		// Token: 0x0600506A RID: 20586 RVA: 0x001C5858 File Offset: 0x001C3A58
		public void RegisterDoor(IntVector2 position, DungeonDoorController door, IntVector2 subsidiaryDoorPosition)
		{
			if (this.doors == null)
			{
				this.doors = new Dictionary<IntVector2, DungeonDoorController>(new IntVector2EqualityComparer());
			}
			if (this.doors.ContainsKey(position))
			{
				return;
			}
			this.doors.Add(position, door);
			this[position].isDoorFrameCell = true;
			if (door.northSouth)
			{
				this.doors.Add(position + IntVector2.Right, door);
				this[position + IntVector2.Right].isDoorFrameCell = true;
				this[position + IntVector2.Up].isDoorFrameCell = true;
				this[position + IntVector2.UpRight].isDoorFrameCell = true;
				this[position].isExitNonOccluder = true;
				this[position + IntVector2.Right].isExitNonOccluder = true;
			}
			else
			{
				this.doors.Add(position + IntVector2.Up, door);
				this[position + IntVector2.Up].isDoorFrameCell = true;
				for (int i = -2; i < 3; i++)
				{
					this[position + new IntVector2(i, 1)].isExitNonOccluder = true;
					if (Math.Abs(i) < 2)
					{
						this[position + IntVector2.Right * i].isExitNonOccluder = true;
						this[position + new IntVector2(i, 2)].isExitNonOccluder = true;
					}
				}
			}
			if (door.subsidiaryDoor != null)
			{
				this.doors.Add(subsidiaryDoorPosition, door.subsidiaryDoor);
				this[subsidiaryDoorPosition].isDoorFrameCell = true;
				if (door.subsidiaryDoor.northSouth)
				{
					this.doors.Add(subsidiaryDoorPosition + IntVector2.Right, door.subsidiaryDoor);
					this[subsidiaryDoorPosition + IntVector2.Right].isDoorFrameCell = true;
					this[subsidiaryDoorPosition + IntVector2.Up].isDoorFrameCell = true;
					this[subsidiaryDoorPosition + IntVector2.UpRight].isDoorFrameCell = true;
				}
				else
				{
					this.doors.Add(subsidiaryDoorPosition + IntVector2.Up, door.subsidiaryDoor);
					this[subsidiaryDoorPosition + IntVector2.Up].isDoorFrameCell = true;
				}
			}
		}

		// Token: 0x0600506B RID: 20587 RVA: 0x001C5AB4 File Offset: 0x001C3CB4
		public DungeonDoorController GetDoorAtPosition(IntVector2 position)
		{
			if (this.doors == null)
			{
				return null;
			}
			if (!this.doors.ContainsKey(position))
			{
				return null;
			}
			return this.doors[position];
		}

		// Token: 0x0600506C RID: 20588 RVA: 0x001C5AE4 File Offset: 0x001C3CE4
		public bool HasDoorAtPosition(IntVector2 position)
		{
			return this.doors != null && this.doors.ContainsKey(position);
		}

		// Token: 0x0600506D RID: 20589 RVA: 0x001C5B00 File Offset: 0x001C3D00
		public void DestroyDoorAtPosition(IntVector2 position)
		{
			DungeonDoorController dungeonDoorController = this.doors[position];
			this.doors.Remove(position);
			if (dungeonDoorController.northSouth)
			{
				this.doors.Remove(position + IntVector2.Right);
			}
			else
			{
				this.doors.Remove(position + IntVector2.Up);
			}
			UnityEngine.Object.Destroy(dungeonDoorController.gameObject);
		}

		// Token: 0x0600506E RID: 20590 RVA: 0x001C5B70 File Offset: 0x001C3D70
		public List<CellData> GetCellNeighbors(CellData d, bool getDiagonals = false)
		{
			if (getDiagonals)
			{
			}
			DungeonData.s_neighborsList.Clear();
			int num = ((!getDiagonals) ? 2 : 1);
			for (int i = 0; i < 8; i += num)
			{
				DungeonData.s_neighborsList.Add(this.GetCellInDirection(d, (DungeonData.Direction)i));
			}
			return DungeonData.s_neighborsList;
		}

		// Token: 0x0600506F RID: 20591 RVA: 0x001C5BCC File Offset: 0x001C3DCC
		public bool CheckLineForCellType(IntVector2 p1, IntVector2 p2, CellType t)
		{
			List<CellData> linearCells = this.GetLinearCells(p1, p2);
			for (int i = 0; i < linearCells.Count; i++)
			{
				if (linearCells[i].type == t)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005070 RID: 20592 RVA: 0x001C5C10 File Offset: 0x001C3E10
		public List<CellData> GetLinearCells(IntVector2 p1, IntVector2 p2)
		{
			HashSet<CellData> hashSet = new HashSet<CellData>();
			int num = p1.y;
			int num2 = p1.x;
			int num3 = p2.x - p1.x;
			int num4 = p2.y - p1.y;
			hashSet.Add(this.cellData[num2][num]);
			int num5;
			if (num4 < 0)
			{
				num5 = -1;
				num4 = -num4;
			}
			else
			{
				num5 = 1;
			}
			int num6;
			if (num3 < 0)
			{
				num6 = -1;
				num3 = -num3;
			}
			else
			{
				num6 = 1;
			}
			int num7 = 2 * num4;
			int num8 = 2 * num3;
			if (num8 >= num7)
			{
				int num9 = num3;
				int num10 = num3;
				for (int i = 0; i < num3; i++)
				{
					num2 += num6;
					num9 += num7;
					if (num9 > num8)
					{
						num += num5;
						num9 -= num8;
						if (num9 + num10 < num8)
						{
							hashSet.Add(this.cellData[num2][num - num5]);
						}
						else if (num9 + num10 > num8)
						{
							hashSet.Add(this.cellData[num2 - num6][num]);
						}
						else
						{
							hashSet.Add(this.cellData[num2][num - num5]);
							hashSet.Add(this.cellData[num2 - num6][num]);
						}
					}
					hashSet.Add(this.cellData[num2][num]);
					num10 = num9;
				}
			}
			else
			{
				int num9 = num4;
				int num10 = num9;
				for (int i = 0; i < num4; i++)
				{
					num += num5;
					num9 += num8;
					if (num9 > num7)
					{
						num2 += num6;
						num9 -= num7;
						if (num9 + num10 < num7)
						{
							hashSet.Add(this.cellData[num2 - num6][num]);
						}
						else if (num9 + num10 > num7)
						{
							hashSet.Add(this.cellData[num2][num - num5]);
						}
						else
						{
							hashSet.Add(this.cellData[num2 - num6][num]);
							hashSet.Add(this.cellData[num2][num - num5]);
						}
					}
					hashSet.Add(this.cellData[num2][num]);
					num10 = num9;
				}
			}
			return new List<CellData>(hashSet);
		}

		// Token: 0x06005071 RID: 20593 RVA: 0x001C5E58 File Offset: 0x001C4058
		public CellData GetCellInDirection(CellData d, DungeonData.Direction dir)
		{
			IntVector2 intVector = d.position;
			switch (dir)
			{
			case DungeonData.Direction.NORTH:
				intVector += IntVector2.Up;
				break;
			case DungeonData.Direction.NORTHEAST:
				intVector += IntVector2.Up + IntVector2.Right;
				break;
			case DungeonData.Direction.EAST:
				intVector += IntVector2.Right;
				break;
			case DungeonData.Direction.SOUTHEAST:
				intVector += IntVector2.Right + IntVector2.Down;
				break;
			case DungeonData.Direction.SOUTH:
				intVector += IntVector2.Down;
				break;
			case DungeonData.Direction.SOUTHWEST:
				intVector += IntVector2.Down + IntVector2.Left;
				break;
			case DungeonData.Direction.WEST:
				intVector += IntVector2.Left;
				break;
			case DungeonData.Direction.NORTHWEST:
				intVector += IntVector2.Left + IntVector2.Up;
				break;
			default:
				Debug.LogError("Switching on invalid direction in GetCellInDirection: " + dir.ToString());
				break;
			}
			return (!this.CheckInBounds(intVector)) ? null : this.cellData[intVector.x][intVector.y];
		}

		// Token: 0x06005072 RID: 20594 RVA: 0x001C5F94 File Offset: 0x001C4194
		public bool CheckInBounds(int x, int y)
		{
			return x >= 0 && x < this.Width && y >= 0 && y < this.Height;
		}

		// Token: 0x06005073 RID: 20595 RVA: 0x001C5FC0 File Offset: 0x001C41C0
		public bool CheckInBounds(IntVector2 vec)
		{
			return vec.x >= 0 && vec.x < this.Width && vec.y >= 0 && vec.y < this.Height;
		}

		// Token: 0x06005074 RID: 20596 RVA: 0x001C6010 File Offset: 0x001C4210
		public bool CheckInBoundsAndValid(int x, int y)
		{
			return x >= 0 && x < this.Width && y >= 0 && y < this.Height && this[x, y] != null;
		}

		// Token: 0x06005075 RID: 20597 RVA: 0x001C6048 File Offset: 0x001C4248
		public bool CheckInBoundsAndValid(IntVector2 vec)
		{
			return vec.x >= 0 && vec.x < this.Width && vec.y >= 0 && vec.y < this.Height && this[vec] != null;
		}

		// Token: 0x06005076 RID: 20598 RVA: 0x001C60A4 File Offset: 0x001C42A4
		public bool CheckInBounds(IntVector2 vec, int distanceThresh)
		{
			return vec.x >= distanceThresh && vec.x < this.Width - distanceThresh && vec.y >= distanceThresh && vec.y < this.Height - distanceThresh;
		}

		// Token: 0x06005077 RID: 20599 RVA: 0x001C60F8 File Offset: 0x001C42F8
		public void DistributeComplexSecretPuzzleItems(List<PickupObject> requiredObjects, RoomHandler puzzleRoom, bool preferSignatureEnemies = false, float preferBossesChance = 0f)
		{
			int i = 0;
			bool flag = UnityEngine.Random.value < preferBossesChance;
			List<AIActor> list = new List<AIActor>();
			List<AIActor> list2 = new List<AIActor>();
			List<AIActor> list3 = new List<AIActor>();
			for (int j = 0; j < StaticReferenceManager.AllEnemies.Count; j++)
			{
				AIActor aiactor = StaticReferenceManager.AllEnemies[j];
				if (aiactor.IsNormalEnemy && !aiactor.IsHarmlessEnemy)
				{
					if (!aiactor.IsInReinforcementLayer)
					{
						if (aiactor.healthHaver)
						{
							if (puzzleRoom == null || aiactor.ParentRoom != puzzleRoom)
							{
								if (aiactor.healthHaver.IsBoss)
								{
									list3.Add(aiactor);
								}
								else
								{
									list.Add(StaticReferenceManager.AllEnemies[j]);
									if (StaticReferenceManager.AllEnemies[j].IsSignatureEnemy && preferSignatureEnemies)
									{
										list2.Add(StaticReferenceManager.AllEnemies[j]);
									}
								}
							}
						}
					}
				}
			}
			AIActor aiactor2 = null;
			while (i < requiredObjects.Count)
			{
				if (flag && list3.Count > 0)
				{
					aiactor2 = list3[UnityEngine.Random.Range(0, list3.Count)];
				}
				else if (list2.Count > 0)
				{
					aiactor2 = list2[UnityEngine.Random.Range(0, list2.Count)];
				}
				else if (list.Count > 0)
				{
					aiactor2 = list[UnityEngine.Random.Range(0, list.Count)];
				}
				if (!(aiactor2 != null))
				{
					Debug.LogError("Failed to attach an item to any enemy!");
					break;
				}
				aiactor2.AdditionalSimpleItemDrops.Add(requiredObjects[i]);
				if (requiredObjects[i] is RobotArmBalloonsItem)
				{
					RobotArmBalloonsItem robotArmBalloonsItem = requiredObjects[i] as RobotArmBalloonsItem;
					robotArmBalloonsItem.AttachBalloonToGameActor(aiactor2);
				}
				list3.Remove(aiactor2);
				list2.Remove(aiactor2);
				list.Remove(aiactor2);
				i++;
			}
		}

		// Token: 0x06005078 RID: 20600 RVA: 0x001C6310 File Offset: 0x001C4510
		public void SolidifyLavaInRadius(Vector2 position, float radius)
		{
			int num = Mathf.CeilToInt(radius);
			IntVector2 intVector = position.ToIntVector2(VectorConversions.Floor) + new IntVector2(-num, -num);
			IntVector2 intVector2 = position.ToIntVector2(VectorConversions.Ceil) + new IntVector2(num, num);
			for (int i = intVector.x; i <= intVector2.x; i++)
			{
				for (int j = intVector.y; j <= intVector2.y; j++)
				{
					Vector2 vector = new Vector2((float)i + 0.5f, (float)j + 0.5f);
					float num2 = Vector2.Distance(position, vector);
					if (num2 <= radius)
					{
						this.SolidifyLavaInCell(new IntVector2(i, j));
					}
				}
			}
		}

		// Token: 0x06005079 RID: 20601 RVA: 0x001C63C8 File Offset: 0x001C45C8
		private void InitializeSizzleSystem()
		{
			GameObject gameObject = GameObject.Find("Gungeon_Sizzle_Main");
			if (gameObject == null)
			{
				gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Particles/Gungeon_Sizzle_Main", ".prefab"), Vector3.zero, Quaternion.identity);
				gameObject.name = "Gungeon_Sizzle_Main";
			}
			this.m_sizzleSystem = gameObject.GetComponent<ParticleSystem>();
		}

		// Token: 0x0600507A RID: 20602 RVA: 0x001C6428 File Offset: 0x001C4628
		private void SpawnWorldParticle(ParticleSystem system, Vector3 worldPos)
		{
			system.Emit(new ParticleSystem.EmitParams
			{
				position = worldPos,
				velocity = Vector3.zero,
				startSize = system.startSize,
				startLifetime = system.startLifetime,
				startColor = system.startColor,
				randomSeed = (uint)(UnityEngine.Random.value * 4.2949673E+09f)
			}, 1);
		}

		// Token: 0x0600507B RID: 20603 RVA: 0x001C6498 File Offset: 0x001C4698
		public void SolidifyLavaInCell(IntVector2 cellPosition)
		{
			if (!this.CheckInBounds(cellPosition))
			{
				return;
			}
			CellData cellData = this[cellPosition];
			if (cellData != null && cellData.doesDamage)
			{
				cellData.doesDamage = false;
				if (this.m_sizzleSystem == null)
				{
					this.InitializeSizzleSystem();
				}
				this.SpawnWorldParticle(this.m_sizzleSystem, cellPosition.ToCenterVector3((float)cellPosition.y) + UnityEngine.Random.insideUnitCircle.ToVector3ZUp(0f) / 3f);
				if (UnityEngine.Random.value < 0.5f)
				{
					this.SpawnWorldParticle(this.m_sizzleSystem, cellPosition.ToCenterVector3((float)cellPosition.y) + UnityEngine.Random.insideUnitCircle.ToVector3ZUp(0f) / 3f);
				}
			}
		}

		// Token: 0x0600507C RID: 20604 RVA: 0x001C656C File Offset: 0x001C476C
		public void TriggerFloorAnimationsInCell(IntVector2 cellPosition)
		{
			if (!this.CheckInBounds(cellPosition))
			{
				return;
			}
			CellData cellData = this[cellPosition];
			if (cellData != null && cellData.type == CellType.FLOOR && TK2DTilemapChunkAnimator.PositionToAnimatorMap.ContainsKey(cellPosition))
			{
				for (int i = 0; i < TK2DTilemapChunkAnimator.PositionToAnimatorMap[cellPosition].Count; i++)
				{
					TK2DTilemapChunkAnimator.PositionToAnimatorMap[cellPosition][i].TriggerAnimationSequence();
				}
			}
		}

		// Token: 0x0600507D RID: 20605 RVA: 0x001C65E8 File Offset: 0x001C47E8
		public void UntriggerFloorAnimationsInCell(IntVector2 cellPosition)
		{
			if (!this.CheckInBounds(cellPosition))
			{
				return;
			}
			CellData cellData = this[cellPosition];
			if (cellData != null && cellData.type == CellType.FLOOR && TK2DTilemapChunkAnimator.PositionToAnimatorMap.ContainsKey(cellPosition))
			{
				for (int i = 0; i < TK2DTilemapChunkAnimator.PositionToAnimatorMap[cellPosition].Count; i++)
				{
					TK2DTilemapChunkAnimator.PositionToAnimatorMap[cellPosition][i].UntriggerAnimationSequence();
				}
			}
		}

		// Token: 0x0600507E RID: 20606 RVA: 0x001C6664 File Offset: 0x001C4864
		public void GenerateLocksAndKeys(RoomHandler start, RoomHandler exit)
		{
			ReducedDungeonGraph reducedDungeonGraph = new ReducedDungeonGraph();
			this.PlaceSingleLockAndKey(reducedDungeonGraph);
		}

		// Token: 0x0600507F RID: 20607 RVA: 0x001C6680 File Offset: 0x001C4880
		public void PlaceSingleLockAndKey(ReducedDungeonGraph graph)
		{
		}

		// Token: 0x06005080 RID: 20608 RVA: 0x001C6684 File Offset: 0x001C4884
		public void GetObjectOcclusionAndObstruction(Vector2 sourcePoint, Vector2 listenerPoint, out float occlusion, out float obstruction)
		{
			occlusion = 0f;
			obstruction = 0f;
			IntVector2 intVector = sourcePoint.ToIntVector2(VectorConversions.Floor);
			IntVector2 intVector2 = listenerPoint.ToIntVector2(VectorConversions.Floor);
			if (!this.CheckInBounds(intVector) || !this.CheckInBounds(intVector2))
			{
				return;
			}
			RoomHandler absoluteRoomFromPosition = this.GetAbsoluteRoomFromPosition(intVector);
			RoomHandler absoluteRoomFromPosition2 = this.GetAbsoluteRoomFromPosition(intVector2);
			if (absoluteRoomFromPosition != null && absoluteRoomFromPosition2 != null && absoluteRoomFromPosition != absoluteRoomFromPosition2)
			{
				occlusion = 0.5f;
				obstruction = 0.5f;
			}
		}

		// Token: 0x06005081 RID: 20609 RVA: 0x001C66FC File Offset: 0x001C48FC
		public bool isDeadEnd(int x, int y)
		{
			int num = 0;
			if (this.CheckInBounds(x, y - 1) && this.cellData[x][y - 1].type == CellType.FLOOR)
			{
				num++;
			}
			if (this.CheckInBounds(x + 1, y) && this.cellData[x + 1][y].type == CellType.FLOOR)
			{
				num++;
			}
			if (this.CheckInBounds(x, y + 1) && this.cellData[x][y + 1].type == CellType.FLOOR)
			{
				num++;
			}
			if (this.CheckInBounds(x - 1, y) && this.cellData[x - 1][y].type == CellType.FLOOR)
			{
				num++;
			}
			return num <= 1;
		}

		// Token: 0x06005082 RID: 20610 RVA: 0x001C67C0 File Offset: 0x001C49C0
		public bool isSingleCellWall(int x, int y)
		{
			return this.CheckInBounds(x, y) && this.CheckInBounds(x, y - 1) && this.CheckInBounds(x, y + 1) && this.cellData[x][y] != null && this.cellData[x][y - 1] != null && this.cellData[x][y + 1] != null && (this.cellData[x][y].type == CellType.WALL && this.cellData[x][y - 1].type != CellType.WALL && this.cellData[x][y + 1].type != CellType.WALL);
		}

		// Token: 0x06005083 RID: 20611 RVA: 0x001C6874 File Offset: 0x001C4A74
		public bool isTopDiagonalWall(int x, int y)
		{
			return this.isFaceWallHigher(x, y - 1) && (this.cellData[x][y].diagonalWallType == DiagonalWallType.NORTHEAST || this.cellData[x][y].diagonalWallType == DiagonalWallType.NORTHWEST);
		}

		// Token: 0x06005084 RID: 20612 RVA: 0x001C68B4 File Offset: 0x001C4AB4
		public bool isAnyFaceWall(int x, int y)
		{
			return this.isFaceWallLower(x, y) || this.isFaceWallHigher(x, y);
		}

		// Token: 0x06005085 RID: 20613 RVA: 0x001C68D4 File Offset: 0x001C4AD4
		public bool isFaceWallLower(int x, int y)
		{
			return this.CheckInBoundsAndValid(x, y) && this.CheckInBoundsAndValid(x, y - 1) && (this.cellData[x][y].type == CellType.WALL && this.cellData[x][y - 1].type != CellType.WALL);
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x001C6930 File Offset: 0x001C4B30
		public bool isFaceWallHigher(int x, int y)
		{
			return this.CheckInBoundsAndValid(x, y) && this.CheckInBoundsAndValid(x, y - 1) && this.CheckInBoundsAndValid(x, y - 2) && (this.cellData[x][y].type == CellType.WALL && this.cellData[x][y - 1].type == CellType.WALL && this.cellData[x][y - 2].type != CellType.WALL);
		}

		// Token: 0x06005087 RID: 20615 RVA: 0x001C69B0 File Offset: 0x001C4BB0
		public bool isPlainEmptyCell(int x, int y)
		{
			if (!this.CheckInBounds(x, y))
			{
				return false;
			}
			CellData cellData = this.cellData[x][y];
			return cellData != null && !cellData.isExitCell && !this.isTopWall(x, y) && (!cellData.isOccupied && cellData.IsPassable && !cellData.containsTrap && !cellData.IsTrapZone && !cellData.PreventRewardSpawn && !cellData.doesDamage) && (!cellData.cellVisualData.hasStampedPath & !cellData.forceDisallowGoop);
		}

		// Token: 0x06005088 RID: 20616 RVA: 0x001C6A58 File Offset: 0x001C4C58
		public bool isWallUp(int x, int y)
		{
			return this.CheckInBoundsAndValid(x, y + 1) && this.cellData[x][y + 1].type == CellType.WALL;
		}

		// Token: 0x06005089 RID: 20617 RVA: 0x001C6A88 File Offset: 0x001C4C88
		public bool isWall(int x, int y)
		{
			return this.CheckInBounds(x, y) && (this.cellData[x][y] == null || this.cellData[x][y].type == CellType.WALL);
		}

		// Token: 0x0600508A RID: 20618 RVA: 0x001C6AC0 File Offset: 0x001C4CC0
		public bool isWall(IntVector2 pos)
		{
			return this.CheckInBounds(pos) && (this.cellData[pos.x][pos.y] == null || this.cellData[pos.x][pos.y].type == CellType.WALL);
		}

		// Token: 0x0600508B RID: 20619 RVA: 0x001C6B18 File Offset: 0x001C4D18
		public bool isWallRight(int x, int y)
		{
			return this.CheckInBoundsAndValid(x + 1, y) && this.cellData[x + 1][y].type == CellType.WALL;
		}

		// Token: 0x0600508C RID: 20620 RVA: 0x001C6B48 File Offset: 0x001C4D48
		public bool isWallLeft(int x, int y)
		{
			return this.CheckInBoundsAndValid(x - 1, y) && this.cellData[x - 1][y].type == CellType.WALL;
		}

		// Token: 0x0600508D RID: 20621 RVA: 0x001C6B78 File Offset: 0x001C4D78
		public bool isWallUpRight(int x, int y)
		{
			return this.CheckInBoundsAndValid(x + 1, y + 1) && this.cellData[x + 1][y + 1].type == CellType.WALL;
		}

		// Token: 0x0600508E RID: 20622 RVA: 0x001C6BAC File Offset: 0x001C4DAC
		public bool isWallUpLeft(int x, int y)
		{
			return this.CheckInBoundsAndValid(x - 1, y + 1) && this.cellData[x - 1][y + 1].type == CellType.WALL;
		}

		// Token: 0x0600508F RID: 20623 RVA: 0x001C6BE0 File Offset: 0x001C4DE0
		public bool isWallDownRight(int x, int y)
		{
			return this.CheckInBoundsAndValid(x + 1, y - 1) && this.cellData[x + 1][y - 1].type == CellType.WALL;
		}

		// Token: 0x06005090 RID: 20624 RVA: 0x001C6C14 File Offset: 0x001C4E14
		public bool isWallDownLeft(int x, int y)
		{
			return this.CheckInBoundsAndValid(x - 1, y - 1) && this.cellData[x - 1][y - 1].type == CellType.WALL;
		}

		// Token: 0x06005091 RID: 20625 RVA: 0x001C6C48 File Offset: 0x001C4E48
		public bool isFaceWallRight(int x, int y)
		{
			return this.isAnyFaceWall(x + 1, y);
		}

		// Token: 0x06005092 RID: 20626 RVA: 0x001C6C54 File Offset: 0x001C4E54
		public bool isFaceWallLeft(int x, int y)
		{
			return this.isAnyFaceWall(x - 1, y);
		}

		// Token: 0x06005093 RID: 20627 RVA: 0x001C6C60 File Offset: 0x001C4E60
		public bool isShadowFloor(int x, int y)
		{
			return this.CheckInBoundsAndValid(x, y) && this.CheckInBoundsAndValid(x, y + 1) && (this.cellData[x][y].type == CellType.FLOOR && this.cellData[x][y + 1].type == CellType.WALL);
		}

		// Token: 0x06005094 RID: 20628 RVA: 0x001C6CBC File Offset: 0x001C4EBC
		public bool isTopWall(int x, int y)
		{
			return this.CheckInBoundsAndValid(x, y) && this.CheckInBoundsAndValid(x, y - 1) && (this.cellData[x][y].type != CellType.WALL && this.cellData[x][y - 1].type == CellType.WALL);
		}

		// Token: 0x06005095 RID: 20629 RVA: 0x001C6D18 File Offset: 0x001C4F18
		public bool isLeftTopWall(int x, int y)
		{
			return this.CheckInBoundsAndValid(x - 1, y) && this.CheckInBoundsAndValid(x - 1, y - 1) && (this.cellData[x - 1][y].type != CellType.WALL && this.cellData[x - 1][y - 1].type == CellType.WALL);
		}

		// Token: 0x06005096 RID: 20630 RVA: 0x001C6D7C File Offset: 0x001C4F7C
		public bool isRightTopWall(int x, int y)
		{
			return this.CheckInBoundsAndValid(x + 1, y) && this.CheckInBoundsAndValid(x + 1, y - 1) && (this.cellData[x + 1][y].type != CellType.WALL && this.cellData[x + 1][y - 1].type == CellType.WALL);
		}

		// Token: 0x06005097 RID: 20631 RVA: 0x001C6DE0 File Offset: 0x001C4FE0
		public bool hasTopWall(int x, int y)
		{
			return this.CheckInBoundsAndValid(x, y) && this.CheckInBoundsAndValid(x, y + 1) && (this.cellData[x][y].type == CellType.WALL && this.cellData[x][y + 1].type != CellType.WALL);
		}

		// Token: 0x06005098 RID: 20632 RVA: 0x001C6E3C File Offset: 0x001C503C
		public bool leftHasTopWall(int x, int y)
		{
			return this.CheckInBoundsAndValid(x - 1, y) && this.CheckInBoundsAndValid(x - 1, y + 1) && (this.cellData[x - 1][y].type == CellType.WALL && this.cellData[x - 1][y + 1].type != CellType.WALL);
		}

		// Token: 0x06005099 RID: 20633 RVA: 0x001C6EA0 File Offset: 0x001C50A0
		public bool rightHasTopWall(int x, int y)
		{
			return this.CheckInBoundsAndValid(x + 1, y) && this.CheckInBoundsAndValid(x + 1, y + 1) && (this.cellData[x + 1][y].type == CellType.WALL && this.cellData[x + 1][y + 1].type != CellType.WALL);
		}

		// Token: 0x0600509A RID: 20634 RVA: 0x001C6F04 File Offset: 0x001C5104
		public bool isPit(int x, int y)
		{
			if (!this.CheckInBoundsAndValid(x, y))
			{
				return false;
			}
			CellData cellData = this.cellData[x][y];
			return cellData != null && cellData.type == CellType.PIT;
		}

		// Token: 0x0600509B RID: 20635 RVA: 0x001C6F40 File Offset: 0x001C5140
		public bool isLeftSideWall(int x, int y)
		{
			return this.isWall(x, y) && !this.isWall(x + 1, y);
		}

		// Token: 0x0600509C RID: 20636 RVA: 0x001C6F60 File Offset: 0x001C5160
		public bool isRightSideWall(int x, int y)
		{
			return this.isWall(x, y) && !this.isWall(x - 1, y);
		}

		// Token: 0x04004870 RID: 18544
		public CellData[][] cellData;

		// Token: 0x04004871 RID: 18545
		private int m_width = -1;

		// Token: 0x04004872 RID: 18546
		private int m_height = -1;

		// Token: 0x04004873 RID: 18547
		public List<RoomHandler> rooms;

		// Token: 0x04004874 RID: 18548
		public Dictionary<IntVector2, DungeonDoorController> doors;

		// Token: 0x04004875 RID: 18549
		public RoomHandler Entrance;

		// Token: 0x04004876 RID: 18550
		public RoomHandler Exit;

		// Token: 0x04004877 RID: 18551
		public tk2dTileMap tilemap;

		// Token: 0x04004878 RID: 18552
		private static List<CellData> s_neighborsList = new List<CellData>(8);

		// Token: 0x04004879 RID: 18553
		private ParticleSystem m_sizzleSystem;

		// Token: 0x02000ECD RID: 3789
		public enum Direction
		{
			// Token: 0x0400487B RID: 18555
			NORTH,
			// Token: 0x0400487C RID: 18556
			NORTHEAST,
			// Token: 0x0400487D RID: 18557
			EAST,
			// Token: 0x0400487E RID: 18558
			SOUTHEAST,
			// Token: 0x0400487F RID: 18559
			SOUTH,
			// Token: 0x04004880 RID: 18560
			SOUTHWEST,
			// Token: 0x04004881 RID: 18561
			WEST,
			// Token: 0x04004882 RID: 18562
			NORTHWEST
		}

		// Token: 0x02000ECE RID: 3790
		public enum LightGenerationStyle
		{
			// Token: 0x04004884 RID: 18564
			STANDARD,
			// Token: 0x04004885 RID: 18565
			FORCE_COLOR,
			// Token: 0x04004886 RID: 18566
			RAT_HALLWAY
		}
	}
}
