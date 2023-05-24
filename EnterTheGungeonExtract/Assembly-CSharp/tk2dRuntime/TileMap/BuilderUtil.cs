using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x02000BEE RID: 3054
	public static class BuilderUtil
	{
		// Token: 0x060040E8 RID: 16616 RVA: 0x0014CF24 File Offset: 0x0014B124
		public static bool InitDataStore(tk2dTileMap tileMap)
		{
			bool flag = false;
			int numLayers = tileMap.data.NumLayers;
			if (tileMap.Layers == null)
			{
				tileMap.Layers = new Layer[numLayers];
				for (int i = 0; i < numLayers; i++)
				{
					tileMap.Layers[i] = new Layer(tileMap.data.Layers[i].hash, tileMap.width, tileMap.height, tileMap.partitionSizeX, tileMap.partitionSizeY);
				}
				flag = true;
			}
			else
			{
				Layer[] array = new Layer[numLayers];
				for (int j = 0; j < numLayers; j++)
				{
					LayerInfo layerInfo = tileMap.data.Layers[j];
					bool flag2 = false;
					for (int k = 0; k < tileMap.Layers.Length; k++)
					{
						if (tileMap.Layers[k].hash == layerInfo.hash)
						{
							array[j] = tileMap.Layers[k];
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						array[j] = new Layer(layerInfo.hash, tileMap.width, tileMap.height, tileMap.partitionSizeX, tileMap.partitionSizeY);
					}
				}
				int num = 0;
				foreach (Layer layer in array)
				{
					if (!layer.IsEmpty)
					{
						num++;
					}
				}
				int num2 = 0;
				foreach (Layer layer2 in tileMap.Layers)
				{
					if (!layer2.IsEmpty)
					{
						num2++;
					}
				}
				if (num != num2)
				{
					flag = true;
				}
				tileMap.Layers = array;
			}
			if (tileMap.ColorChannel == null)
			{
				tileMap.ColorChannel = new ColorChannel(tileMap.width, tileMap.height, tileMap.partitionSizeX, tileMap.partitionSizeY);
			}
			return flag;
		}

		// Token: 0x060040E9 RID: 16617 RVA: 0x0014D108 File Offset: 0x0014B308
		private static GameObject GetExistingTilePrefabInstance(tk2dTileMap tileMap, int tileX, int tileY, int tileLayer)
		{
			int tilePrefabsListCount = tileMap.GetTilePrefabsListCount();
			for (int i = 0; i < tilePrefabsListCount; i++)
			{
				int num;
				int num2;
				int num3;
				GameObject gameObject;
				tileMap.GetTilePrefabsListItem(i, out num, out num2, out num3, out gameObject);
				if (num == tileX && num2 == tileY && num3 == tileLayer)
				{
					return gameObject;
				}
			}
			return null;
		}

		// Token: 0x060040EA RID: 16618 RVA: 0x0014D158 File Offset: 0x0014B358
		public static void SpawnAnimatedTiles(tk2dTileMap tileMap, bool forceBuild)
		{
			int num = tileMap.Layers.Length;
			for (int i = 0; i < num; i++)
			{
				Layer layer = tileMap.Layers[i];
				LayerInfo layerInfo = tileMap.data.Layers[i];
				if (!layer.IsEmpty && !layerInfo.skipMeshGeneration)
				{
					for (int j = 0; j < layer.numRows; j++)
					{
						int num2 = j * layer.divY;
						for (int k = 0; k < layer.numColumns; k++)
						{
							int num3 = k * layer.divX;
							SpriteChunk chunk = layer.GetChunk(k, j);
							if (!chunk.IsEmpty)
							{
								if (forceBuild || chunk.Dirty)
								{
									BuilderUtil.SpawnAnimatedTilesForChunk(tileMap, chunk, num3, num2, i);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x0014D240 File Offset: 0x0014B440
		public static void SpawnAnimatedTilesForChunk(tk2dTileMap tileMap, SpriteChunk chunk, int baseX, int baseY, int layer)
		{
			LayerInfo layerInfo = tileMap.data.Layers[layer];
			if (layerInfo.ForceNonAnimating)
			{
				return;
			}
			int[] spriteIds = chunk.spriteIds;
			float num = 0f;
			float num2 = 0f;
			tileMap.data.GetTileOffset(out num, out num2);
			List<Material> list = new List<Material>();
			for (int i = 0; i < tileMap.partitionSizeY; i++)
			{
				for (int j = 0; j < tileMap.partitionSizeX; j++)
				{
					int tileFromRawTile = BuilderUtil.GetTileFromRawTile(spriteIds[i * tileMap.partitionSizeX + j]);
					if (tileFromRawTile >= 0)
					{
						if (tileFromRawTile >= tileMap.SpriteCollectionInst.spriteDefinitions.Length)
						{
							Debug.Log(tileFromRawTile.ToString() + " tile is oob");
						}
						else
						{
							tk2dSpriteDefinition tk2dSpriteDefinition = tileMap.SpriteCollectionInst.spriteDefinitions[tileFromRawTile];
							if (tk2dSpriteDefinition.metadata.usesAnimSequence && !list.Contains(tk2dSpriteDefinition.materialInst))
							{
								list.Add(tk2dSpriteDefinition.materialInst);
							}
						}
					}
				}
			}
			while (list.Count > 0)
			{
				BuilderUtil.ProcGenMeshData procGenMeshData = null;
				Material material = list[0];
				list.RemoveAt(0);
				List<TilemapAnimatorTileManager> list2 = new List<TilemapAnimatorTileManager>();
				bool flag = false;
				int unityLayer = layerInfo.unityLayer;
				for (int k = 0; k < tileMap.partitionSizeY; k++)
				{
					for (int l = 0; l < tileMap.partitionSizeX; l++)
					{
						IntVector2 intVector = new IntVector2(baseX + l, baseY + k);
						int tileFromRawTile2 = BuilderUtil.GetTileFromRawTile(spriteIds[k * tileMap.partitionSizeX + l]);
						if (tileFromRawTile2 >= 0)
						{
							if (tileFromRawTile2 >= tileMap.SpriteCollectionInst.spriteDefinitions.Length)
							{
								Debug.Log(tileFromRawTile2.ToString() + " tile is oob");
							}
							else
							{
								tk2dSpriteDefinition tk2dSpriteDefinition2 = tileMap.SpriteCollectionInst.spriteDefinitions[tileFromRawTile2];
								if (!(tk2dSpriteDefinition2.materialInst != material))
								{
									if (tk2dSpriteDefinition2.metadata.usesAnimSequence)
									{
										if (procGenMeshData == null)
										{
											procGenMeshData = new BuilderUtil.ProcGenMeshData();
										}
										TilemapAnimatorTileManager tilemapAnimatorTileManager = new TilemapAnimatorTileManager(tileMap.SpriteCollectionInst, tileFromRawTile2, tk2dSpriteDefinition2.metadata, procGenMeshData.vertices.Count, tk2dSpriteDefinition2.uvs.Length, tileMap);
										tilemapAnimatorTileManager.worldPosition = intVector;
										if (TK2DTilemapChunkAnimator.PositionToAnimatorMap.ContainsKey(intVector))
										{
											TK2DTilemapChunkAnimator.PositionToAnimatorMap[intVector].Add(tilemapAnimatorTileManager);
										}
										else
										{
											List<TilemapAnimatorTileManager> list3 = new List<TilemapAnimatorTileManager>();
											list3.Add(tilemapAnimatorTileManager);
											TK2DTilemapChunkAnimator.PositionToAnimatorMap.Add(intVector, list3);
										}
										bool flag2 = false;
										for (int m = 0; m < list2.Count; m++)
										{
											TilemapAnimatorTileManager tilemapAnimatorTileManager2 = list2[m];
											List<IndexNeighborDependency> dependencies = tilemapAnimatorTileManager2.associatedCollection.GetDependencies(tilemapAnimatorTileManager2.associatedSpriteId);
											if (dependencies != null && dependencies.Count > 0)
											{
												for (int n = 0; n < dependencies.Count; n++)
												{
													if (tilemapAnimatorTileManager2.worldPosition + DungeonData.GetIntVector2FromDirection(dependencies[n].neighborDirection) == intVector)
													{
														flag2 = true;
														tilemapAnimatorTileManager2.linkedManagers.Add(tilemapAnimatorTileManager);
														goto IL_328;
													}
												}
											}
										}
										IL_328:
										if (!flag2)
										{
											list2.Add(tilemapAnimatorTileManager);
										}
										if (BuilderUtil.AddSquareToAnimChunk(tileMap, chunk, tk2dSpriteDefinition2, baseX, baseY, num, l, k, layer, procGenMeshData))
										{
											flag = true;
										}
									}
									if (tk2dSpriteDefinition2.metadata.usesPerTileVFX && UnityEngine.Random.value < tk2dSpriteDefinition2.metadata.tileVFXChance)
									{
										TileVFXManager.Instance.RegisterCellVFX(intVector, tk2dSpriteDefinition2.metadata);
									}
								}
							}
						}
					}
				}
				if (layerInfo.unityLayer == 19 || layerInfo.unityLayer == 20)
				{
					flag = false;
				}
				if (procGenMeshData != null)
				{
					GameObject gameObject = new GameObject("anim chunk data");
					gameObject.transform.parent = tileMap.Layers[layer].gameObject.transform;
					if (flag)
					{
						gameObject.layer = LayerMask.NameToLayer("ShadowCaster");
					}
					else
					{
						gameObject.layer = unityLayer;
					}
					gameObject.transform.localPosition = new Vector3((float)baseX, (float)baseY, (float)baseY);
					MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
					MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
					Mesh mesh = new Mesh();
					mesh.vertices = procGenMeshData.vertices.ToArray();
					mesh.triangles = procGenMeshData.triangles.ToArray();
					mesh.uv = procGenMeshData.uvs.ToArray();
					mesh.colors = procGenMeshData.colors.ToArray();
					mesh.RecalculateBounds();
					mesh.RecalculateNormals();
					meshFilter.mesh = mesh;
					meshRenderer.material = material;
					for (int num3 = 0; num3 < meshRenderer.materials.Length; num3++)
					{
						meshRenderer.materials[num3].renderQueue += layerInfo.renderQueueOffset;
					}
					TK2DTilemapChunkAnimator tk2DTilemapChunkAnimator = gameObject.AddComponent<TK2DTilemapChunkAnimator>();
					tk2DTilemapChunkAnimator.Initialize(list2, mesh, tileMap);
				}
			}
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x0014D770 File Offset: 0x0014B970
		private static bool AddSquareToAnimChunk(tk2dTileMap tileMap, SpriteChunk chunk, tk2dSpriteDefinition sprite, int baseX, int baseY, float xOffsetMult, int x, int y, int layer, BuilderUtil.ProcGenMeshData genMeshData)
		{
			bool flag = false;
			LayerInfo layerInfo = tileMap.data.Layers[layer];
			int count = genMeshData.vertices.Count;
			float num = (float)((baseY + y) & 1) * xOffsetMult;
			Vector3[] array = sprite.ConstructExpensivePositions();
			for (int i = 0; i < array.Length; i++)
			{
				Vector3 vector = new Vector3(tileMap.data.tileSize.x * ((float)x + num), tileMap.data.tileSize.y * (float)y, 0f);
				Vector3 vector2 = BuilderUtil.ApplySpriteVertexTileFlags(tileMap, sprite, array[i], false, false, false);
				Vector3 vector3 = vector;
				IntVector2 intVector = vector.IntXY(VectorConversions.Round) + new IntVector2(baseX, baseY);
				CellData cellData = ((!GameManager.Instance.Dungeon.data.CheckInBounds(intVector, 1)) ? null : GameManager.Instance.Dungeon.data[intVector]);
				if (cellData != null && cellData.IsAnyFaceWall())
				{
					if (GameManager.Instance.Dungeon.data.isFaceWallHigher(cellData.position.x, cellData.position.y))
					{
						if (i > 1)
						{
							genMeshData.colors.Add(new Color(0f, 1f, 1f));
						}
						else
						{
							genMeshData.colors.Add(new Color(0f, 0.5f, 1f));
						}
					}
					else if (i > 1)
					{
						genMeshData.colors.Add(new Color(0f, 0.5f, 1f));
					}
					else
					{
						genMeshData.colors.Add(new Color(0f, 0f, 1f));
					}
					flag = true;
					BraveUtility.DrawDebugSquare(intVector.ToVector2(), Color.blue, 1000f);
				}
				else
				{
					genMeshData.colors.Add(Color.black);
				}
				if (tileMap.isGungeonTilemap)
				{
					if (cellData != null && GameManager.Instance.Dungeon.data.isAnyFaceWall(intVector.x, intVector.y))
					{
						Vector3 vector4 = ((!GameManager.Instance.Dungeon.data.isFaceWallHigher(intVector.x, intVector.y)) ? new Vector3(0f, 0f, 1f) : new Vector3(0f, 0f, -1f));
						if (cellData.diagonalWallType == DiagonalWallType.NORTHEAST)
						{
							vector4.z += (1f - vector2.x) * 2f;
						}
						else if (cellData.diagonalWallType == DiagonalWallType.NORTHWEST)
						{
							vector4.z += vector2.x * 2f;
						}
						vector3 += new Vector3(0f, 0f, vector.y - vector2.y) + vector2 + vector4;
					}
					else if (cellData != null && GameManager.Instance.Dungeon.data.isTopDiagonalWall(intVector.x, intVector.y) && layerInfo.name == "Collision Layer")
					{
						Vector3 vector5 = new Vector3(0f, 0f, -3f);
						vector3 += new Vector3(0f, 0f, vector.y + vector2.y) + vector2 + vector5;
					}
					else if (layerInfo.name == "Pit Layer")
					{
						Vector3 vector6 = new Vector3(0f, 0f, 2f);
						if (GameManager.Instance.Dungeon.data.CheckInBounds(intVector.x, intVector.y + 2))
						{
							bool flag2 = GameManager.Instance.Dungeon.data.cellData[intVector.x][intVector.y + 1].type != CellType.PIT || GameManager.Instance.Dungeon.data.cellData[intVector.x][intVector.y + 2].type != CellType.PIT;
							if (flag2)
							{
								bool flag3 = GameManager.Instance.Dungeon.data.cellData[intVector.x][intVector.y + 1].type != CellType.PIT;
								if (GameManager.Instance.Dungeon.debugSettings.WALLS_ARE_PITS && GameManager.Instance.Dungeon.data.cellData[intVector.x][intVector.y + 1].isExitCell)
								{
									flag3 = false;
								}
								if (flag3)
								{
									vector6 = new Vector3(0f, 0f, 0f);
								}
								vector3 += new Vector3(0f, 0f, vector.y - vector2.y) + vector2 + vector6;
							}
							else
							{
								vector3 += new Vector3(0f, 0f, vector.y + vector2.y + 1f) + vector2;
							}
						}
						else
						{
							vector3 += new Vector3(0f, 0f, vector.y + vector2.y + 1f) + vector2;
						}
					}
					else
					{
						vector3 += new Vector3(0f, 0f, vector.y + vector2.y) + vector2;
					}
				}
				else
				{
					vector3 += vector2;
				}
				genMeshData.vertices.Add(vector3);
				genMeshData.uvs.Add(sprite.uvs[i]);
			}
			bool flag4 = false;
			for (int j = 0; j < sprite.indices.Length; j++)
			{
				int num2 = ((!flag4) ? j : (sprite.indices.Length - 1 - j));
				genMeshData.triangles.Add(count + sprite.indices[num2]);
			}
			return flag;
		}

		// Token: 0x060040ED RID: 16621 RVA: 0x0014DE00 File Offset: 0x0014C000
		public static void SpawnPrefabsForChunk(tk2dTileMap tileMap, SpriteChunk chunk, int baseX, int baseY, int layer, int[] prefabCounts)
		{
			int[] spriteIds = chunk.spriteIds;
			GameObject[] tilePrefabs = tileMap.data.tilePrefabs;
			Vector3 tileSize = tileMap.data.tileSize;
			Transform transform = chunk.gameObject.transform;
			float num = 0f;
			float num2 = 0f;
			tileMap.data.GetTileOffset(out num, out num2);
			for (int i = 0; i < tileMap.partitionSizeY; i++)
			{
				float num3 = (float)((baseY + i) & 1) * num;
				for (int j = 0; j < tileMap.partitionSizeX; j++)
				{
					int tileFromRawTile = BuilderUtil.GetTileFromRawTile(spriteIds[i * tileMap.partitionSizeX + j]);
					if (tileFromRawTile >= 0 && tileFromRawTile < tilePrefabs.Length)
					{
						UnityEngine.Object @object = tilePrefabs[tileFromRawTile];
						if (@object != null)
						{
							prefabCounts[tileFromRawTile]++;
							GameObject gameObject = BuilderUtil.GetExistingTilePrefabInstance(tileMap, baseX + j, baseY + i, layer);
							bool flag = gameObject != null;
							if (gameObject == null)
							{
								gameObject = UnityEngine.Object.Instantiate(@object, Vector3.zero, Quaternion.identity) as GameObject;
							}
							if (gameObject != null)
							{
								GameObject gameObject2 = @object as GameObject;
								Vector3 vector = new Vector3(tileSize.x * ((float)j + num3), tileSize.y * (float)i, 0f);
								bool flag2 = false;
								TileInfo tileInfoForSprite = tileMap.data.GetTileInfoForSprite(tileFromRawTile);
								if (tileInfoForSprite != null)
								{
									flag2 = tileInfoForSprite.enablePrefabOffset;
								}
								if (flag2 && gameObject2 != null)
								{
									vector += gameObject2.transform.position;
								}
								if (!flag)
								{
									gameObject.name = @object.name + " " + prefabCounts[tileFromRawTile].ToString();
								}
								tk2dUtil.SetTransformParent(gameObject.transform, transform);
								gameObject.transform.localPosition = vector;
								BuilderUtil.TilePrefabsX.Add(baseX + j);
								BuilderUtil.TilePrefabsY.Add(baseY + i);
								BuilderUtil.TilePrefabsLayer.Add(layer);
								BuilderUtil.TilePrefabsInstance.Add(gameObject);
							}
						}
					}
				}
			}
		}

		// Token: 0x060040EE RID: 16622 RVA: 0x0014E030 File Offset: 0x0014C230
		public static void SpawnPrefabs(tk2dTileMap tileMap, bool forceBuild)
		{
			BuilderUtil.TilePrefabsX = new List<int>();
			BuilderUtil.TilePrefabsY = new List<int>();
			BuilderUtil.TilePrefabsLayer = new List<int>();
			BuilderUtil.TilePrefabsInstance = new List<GameObject>();
			int[] array = new int[tileMap.data.tilePrefabs.Length];
			int num = tileMap.Layers.Length;
			for (int i = 0; i < num; i++)
			{
				Layer layer = tileMap.Layers[i];
				LayerInfo layerInfo = tileMap.data.Layers[i];
				if (!layer.IsEmpty && !layerInfo.skipMeshGeneration)
				{
					for (int j = 0; j < layer.numRows; j++)
					{
						int num2 = j * layer.divY;
						for (int k = 0; k < layer.numColumns; k++)
						{
							int num3 = k * layer.divX;
							SpriteChunk chunk = layer.GetChunk(k, j);
							if (!chunk.IsEmpty)
							{
								if (forceBuild || chunk.Dirty)
								{
									BuilderUtil.SpawnPrefabsForChunk(tileMap, chunk, num3, num2, i, array);
								}
							}
						}
					}
				}
			}
			tileMap.SetTilePrefabsList(BuilderUtil.TilePrefabsX, BuilderUtil.TilePrefabsY, BuilderUtil.TilePrefabsLayer, BuilderUtil.TilePrefabsInstance);
		}

		// Token: 0x060040EF RID: 16623 RVA: 0x0014E170 File Offset: 0x0014C370
		public static void HideTileMapPrefabs(tk2dTileMap tileMap)
		{
			if (tileMap.renderData == null || tileMap.Layers == null)
			{
				return;
			}
			if (tileMap.PrefabsRoot == null)
			{
				GameObject gameObject = tk2dUtil.CreateGameObject("Prefabs");
				tileMap.PrefabsRoot = gameObject;
				GameObject gameObject2 = gameObject;
				gameObject2.transform.parent = tileMap.renderData.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.identity;
				gameObject2.transform.localScale = Vector3.one;
			}
			int tilePrefabsListCount = tileMap.GetTilePrefabsListCount();
			bool[] array = new bool[tilePrefabsListCount];
			for (int i = 0; i < tileMap.Layers.Length; i++)
			{
				Layer layer = tileMap.Layers[i];
				for (int j = 0; j < layer.spriteChannel.chunks.Length; j++)
				{
					SpriteChunk spriteChunk = layer.spriteChannel.chunks[j];
					if (!(spriteChunk.gameObject == null))
					{
						Transform transform = spriteChunk.gameObject.transform;
						int childCount = transform.childCount;
						for (int k = 0; k < childCount; k++)
						{
							GameObject gameObject3 = transform.GetChild(k).gameObject;
							for (int l = 0; l < tilePrefabsListCount; l++)
							{
								int num;
								int num2;
								int num3;
								GameObject gameObject4;
								tileMap.GetTilePrefabsListItem(l, out num, out num2, out num3, out gameObject4);
								if (gameObject4 == gameObject3)
								{
									array[l] = true;
									break;
								}
							}
						}
					}
				}
			}
			UnityEngine.Object[] tilePrefabs = tileMap.data.tilePrefabs;
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			List<int> list3 = new List<int>();
			List<GameObject> list4 = new List<GameObject>();
			for (int m = 0; m < tilePrefabsListCount; m++)
			{
				int num4;
				int num5;
				int num6;
				GameObject gameObject5;
				tileMap.GetTilePrefabsListItem(m, out num4, out num5, out num6, out gameObject5);
				if (!array[m])
				{
					int num7 = ((num4 < 0 || num4 >= tileMap.width || num5 < 0 || num5 >= tileMap.height) ? (-1) : tileMap.GetTile(num4, num5, num6));
					if (num7 >= 0 && num7 < tilePrefabs.Length && tilePrefabs[num7] != null)
					{
						array[m] = true;
					}
				}
				if (array[m])
				{
					list.Add(num4);
					list2.Add(num5);
					list3.Add(num6);
					list4.Add(gameObject5);
					tk2dUtil.SetTransformParent(gameObject5.transform, tileMap.PrefabsRoot.transform);
				}
			}
			tileMap.SetTilePrefabsList(list, list2, list3, list4);
		}

		// Token: 0x060040F0 RID: 16624 RVA: 0x0014E414 File Offset: 0x0014C614
		private static Vector3 GetTilePosition(tk2dTileMap tileMap, int x, int y)
		{
			return new Vector3(tileMap.data.tileSize.x * (float)x, tileMap.data.tileSize.y * (float)y, 0f);
		}

		// Token: 0x060040F1 RID: 16625 RVA: 0x0014E448 File Offset: 0x0014C648
		public static void CreateOverrideChunkData(SpriteChunk chunk, tk2dTileMap tileMap, int layerId, string overrideChunkName)
		{
			Layer layer = tileMap.Layers[layerId];
			bool flag = layer.IsEmpty || chunk.IsEmpty;
			if (flag && chunk.HasGameData)
			{
				chunk.DestroyGameData(tileMap);
			}
			else if (!flag && chunk.gameObject == null)
			{
				string text = "Chunk_" + overrideChunkName + "_" + tileMap.data.Layers[layerId].name;
				GameObject gameObject = (chunk.gameObject = tk2dUtil.CreateGameObject(text));
				gameObject.transform.parent = layer.gameObject.transform;
				MeshFilter meshFilter = tk2dUtil.AddComponent<MeshFilter>(gameObject);
				tk2dUtil.AddComponent<MeshRenderer>(gameObject);
				chunk.mesh = tk2dUtil.CreateMesh();
				meshFilter.mesh = chunk.mesh;
			}
			if (chunk.gameObject != null)
			{
				Vector3 vector = new Vector3((float)chunk.startX, (float)chunk.startY, 0f);
				chunk.gameObject.transform.localPosition = vector;
				chunk.gameObject.transform.localRotation = Quaternion.identity;
				chunk.gameObject.transform.localScale = Vector3.one;
				chunk.gameObject.layer = tileMap.data.Layers[layerId].unityLayer;
			}
			if (chunk.gameObject != null && chunk.roomReference != null)
			{
				chunk.gameObject.transform.parent = chunk.roomReference.hierarchyParent;
			}
		}

		// Token: 0x060040F2 RID: 16626 RVA: 0x0014E5D4 File Offset: 0x0014C7D4
		public static void CreateRenderData(tk2dTileMap tileMap, bool editMode, Dictionary<Layer, bool> layersActive)
		{
			if (tileMap.renderData == null)
			{
				GameObject gameObject = GameObject.Find(tileMap.name + " Render Data");
				if (gameObject != null)
				{
					tileMap.renderData = gameObject;
				}
				else
				{
					tileMap.renderData = tk2dUtil.CreateGameObject(tileMap.name + " Render Data");
				}
			}
			tileMap.renderData.transform.position = tileMap.transform.position;
			float num = 0f;
			int num2 = 0;
			foreach (Layer layer in tileMap.Layers)
			{
				float z = tileMap.data.Layers[num2].z;
				if (num2 != 0)
				{
					num -= z;
				}
				if (layer.IsEmpty && layer.gameObject != null)
				{
					tk2dUtil.DestroyImmediate(layer.gameObject);
					layer.gameObject = null;
				}
				else if (!layer.IsEmpty && layer.gameObject == null)
				{
					Transform transform = tileMap.renderData.transform.Find(tileMap.data.Layers[num2].name);
					if (transform != null)
					{
						layer.gameObject = transform.gameObject;
					}
					else
					{
						GameObject gameObject2 = (layer.gameObject = tk2dUtil.CreateGameObject(string.Empty));
						gameObject2.transform.parent = tileMap.renderData.transform;
					}
				}
				int unityLayer = tileMap.data.Layers[num2].unityLayer;
				if (layer.gameObject != null)
				{
					if (!editMode && layersActive.ContainsKey(layer) && layer.gameObject.activeSelf != layersActive[layer])
					{
						layer.gameObject.SetActive(layersActive[layer]);
					}
					layer.gameObject.name = tileMap.data.Layers[num2].name;
					layer.gameObject.transform.localPosition = new Vector3(0f, 0f, (!tileMap.data.layersFixedZ) ? num : (-z));
					layer.gameObject.transform.localRotation = Quaternion.identity;
					layer.gameObject.transform.localScale = Vector3.one;
					layer.gameObject.layer = unityLayer;
				}
				int num3;
				int num4;
				int num5;
				int num6;
				int num7;
				int num8;
				BuilderUtil.GetLoopOrder(tileMap.data.sortMethod, layer.numColumns, layer.numRows, out num3, out num4, out num5, out num6, out num7, out num8);
				float num9 = 0f;
				for (int num10 = num6; num10 != num7; num10 += num8)
				{
					for (int num11 = num3; num11 != num4; num11 += num5)
					{
						SpriteChunk chunk = layer.GetChunk(num11, num10);
						bool flag = layer.IsEmpty || chunk.IsEmpty;
						if (editMode)
						{
							flag = false;
						}
						if (flag && chunk.HasGameData)
						{
							chunk.DestroyGameData(tileMap);
						}
						else if (!flag && chunk.gameObject == null)
						{
							string text = "Chunk " + num10.ToString() + " " + num11.ToString();
							GameObject gameObject3 = (chunk.gameObject = tk2dUtil.CreateGameObject(text));
							gameObject3.transform.parent = layer.gameObject.transform;
							MeshFilter meshFilter = tk2dUtil.AddComponent<MeshFilter>(gameObject3);
							tk2dUtil.AddComponent<MeshRenderer>(gameObject3);
							chunk.mesh = tk2dUtil.CreateMesh();
							meshFilter.mesh = chunk.mesh;
						}
						if (chunk.gameObject != null)
						{
							Vector3 tilePosition = BuilderUtil.GetTilePosition(tileMap, num11 * tileMap.partitionSizeX, num10 * tileMap.partitionSizeY);
							tilePosition.z += num9;
							chunk.gameObject.transform.localPosition = tilePosition;
							chunk.gameObject.transform.localRotation = Quaternion.identity;
							chunk.gameObject.transform.localScale = Vector3.one;
							chunk.gameObject.layer = unityLayer;
							if (editMode)
							{
								chunk.DestroyColliderData(tileMap);
							}
						}
						num9 -= 1E-06f;
					}
				}
				num2++;
			}
		}

		// Token: 0x060040F3 RID: 16627 RVA: 0x0014EA40 File Offset: 0x0014CC40
		public static void GetLoopOrder(tk2dTileMapData.SortMethod sortMethod, int w, int h, out int x0, out int x1, out int dx, out int y0, out int y1, out int dy)
		{
			switch (sortMethod)
			{
			case tk2dTileMapData.SortMethod.BottomLeft:
				break;
			case tk2dTileMapData.SortMethod.TopLeft:
				x0 = 0;
				x1 = w;
				dx = 1;
				y0 = h - 1;
				y1 = -1;
				dy = -1;
				return;
			case tk2dTileMapData.SortMethod.BottomRight:
				x0 = w - 1;
				x1 = -1;
				dx = -1;
				y0 = 0;
				y1 = h;
				dy = 1;
				return;
			case tk2dTileMapData.SortMethod.TopRight:
				x0 = w - 1;
				x1 = -1;
				dx = -1;
				y0 = h - 1;
				y1 = -1;
				dy = -1;
				return;
			default:
				Debug.LogError("Unhandled sort method");
				break;
			}
			x0 = 0;
			x1 = w;
			dx = 1;
			y0 = 0;
			y1 = h;
			dy = 1;
		}

		// Token: 0x060040F4 RID: 16628 RVA: 0x0014EAF0 File Offset: 0x0014CCF0
		public static int GetTileFromRawTile(int rawTile)
		{
			if (rawTile == -1)
			{
				return -1;
			}
			return rawTile & 16777215;
		}

		// Token: 0x060040F5 RID: 16629 RVA: 0x0014EB04 File Offset: 0x0014CD04
		public static bool IsRawTileFlagSet(int rawTile, tk2dTileFlags flag)
		{
			return rawTile != -1 && (rawTile & (int)flag) != 0;
		}

		// Token: 0x060040F6 RID: 16630 RVA: 0x0014EB18 File Offset: 0x0014CD18
		public static void SetRawTileFlag(ref int rawTile, tk2dTileFlags flag, bool setValue)
		{
			if (rawTile == -1)
			{
				return;
			}
			rawTile = ((!setValue) ? (rawTile & (int)(~(int)flag)) : (rawTile | (int)flag));
		}

		// Token: 0x060040F7 RID: 16631 RVA: 0x0014EB3C File Offset: 0x0014CD3C
		public static void InvertRawTileFlag(ref int rawTile, tk2dTileFlags flag)
		{
			if (rawTile == -1)
			{
				return;
			}
			bool flag2 = (rawTile & (int)flag) == 0;
			rawTile = ((!flag2) ? (rawTile & (int)(~(int)flag)) : (rawTile | (int)flag));
		}

		// Token: 0x060040F8 RID: 16632 RVA: 0x0014EB70 File Offset: 0x0014CD70
		public static Vector3 ApplySpriteVertexTileFlags(tk2dTileMap tileMap, tk2dSpriteDefinition spriteDef, Vector3 pos, bool flipH, bool flipV, bool rot90)
		{
			float num = tileMap.data.tileOrigin.x + 0.5f * tileMap.data.tileSize.x;
			float num2 = tileMap.data.tileOrigin.y + 0.5f * tileMap.data.tileSize.y;
			float num3 = pos.x - num;
			float num4 = pos.y - num2;
			if (rot90)
			{
				float num5 = num3;
				num3 = num4;
				num4 = -num5;
			}
			if (flipH)
			{
				num3 *= -1f;
			}
			if (flipV)
			{
				num4 *= -1f;
			}
			pos.x = num + num3;
			pos.y = num2 + num4;
			return pos;
		}

		// Token: 0x060040F9 RID: 16633 RVA: 0x0014EC24 File Offset: 0x0014CE24
		public static Vector2 ApplySpriteVertexTileFlags(tk2dTileMap tileMap, tk2dSpriteDefinition spriteDef, Vector2 pos, bool flipH, bool flipV, bool rot90)
		{
			float num = tileMap.data.tileOrigin.x + 0.5f * tileMap.data.tileSize.x;
			float num2 = tileMap.data.tileOrigin.y + 0.5f * tileMap.data.tileSize.y;
			float num3 = pos.x - num;
			float num4 = pos.y - num2;
			if (rot90)
			{
				float num5 = num3;
				num3 = num4;
				num4 = -num5;
			}
			if (flipH)
			{
				num3 *= -1f;
			}
			if (flipV)
			{
				num4 *= -1f;
			}
			pos.x = num + num3;
			pos.y = num2 + num4;
			return pos;
		}

		// Token: 0x040033B3 RID: 13235
		private static List<int> TilePrefabsX;

		// Token: 0x040033B4 RID: 13236
		private static List<int> TilePrefabsY;

		// Token: 0x040033B5 RID: 13237
		private static List<int> TilePrefabsLayer;

		// Token: 0x040033B6 RID: 13238
		private static List<GameObject> TilePrefabsInstance;

		// Token: 0x040033B7 RID: 13239
		private const int tileMask = 16777215;

		// Token: 0x02000BEF RID: 3055
		internal class ProcGenMeshData
		{
			// Token: 0x040033B8 RID: 13240
			public List<Vector3> vertices = new List<Vector3>();

			// Token: 0x040033B9 RID: 13241
			public List<int> triangles = new List<int>();

			// Token: 0x040033BA RID: 13242
			public List<Vector2> uvs = new List<Vector2>();

			// Token: 0x040033BB RID: 13243
			public List<Color> colors = new List<Color>();
		}
	}
}
