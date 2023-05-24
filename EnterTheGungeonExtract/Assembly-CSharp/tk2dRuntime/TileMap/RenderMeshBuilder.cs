using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x02000C01 RID: 3073
	public static class RenderMeshBuilder
	{
		// Token: 0x06004158 RID: 16728 RVA: 0x001517E4 File Offset: 0x0014F9E4
		public static void BuildForChunk(tk2dTileMap tileMap, SpriteChunk chunk, bool useColor, bool skipPrefabs, int baseX, int baseY, LayerInfo layerData)
		{
			GameManager instance = GameManager.Instance;
			Dungeon dungeon = instance.Dungeon;
			List<Vector3> list = new List<Vector3>();
			List<Color> list2 = new List<Color>();
			List<Vector2> list3 = new List<Vector2>();
			if (layerData.preprocessedFlags == null || layerData.preprocessedFlags.Length == 0)
			{
				layerData.preprocessedFlags = new bool[tileMap.width * tileMap.height];
			}
			int[] spriteIds = chunk.spriteIds;
			Vector3 tileSize = tileMap.data.tileSize;
			int num = tileMap.SpriteCollectionInst.spriteDefinitions.Length;
			UnityEngine.Object[] tilePrefabs = tileMap.data.tilePrefabs;
			tk2dSpriteDefinition firstValidDefinition = tileMap.SpriteCollectionInst.FirstValidDefinition;
			bool flag = firstValidDefinition != null && firstValidDefinition.normals != null && firstValidDefinition.normals.Length > 0;
			Color32 color = ((!useColor || tileMap.ColorChannel == null) ? Color.white : tileMap.ColorChannel.clearColor);
			int num2;
			int num3;
			int num4;
			int num5;
			int num6;
			int num7;
			BuilderUtil.GetLoopOrder(tileMap.data.sortMethod, chunk.Width, chunk.Height, out num2, out num3, out num4, out num5, out num6, out num7);
			float num8 = 0f;
			float num9 = 0f;
			tileMap.data.GetTileOffset(out num8, out num9);
			List<int>[] array = new List<int>[tileMap.SpriteCollectionInst.materials.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new List<int>();
			}
			IntVector2 intVector = new IntVector2(layerData.overrideChunkXOffset, layerData.overrideChunkYOffset);
			int num10 = tileMap.partitionSizeX + 1;
			for (int num11 = num5; num11 != num6; num11 += num7)
			{
				float num12 = (float)((baseY + num11) & 1) * num8;
				int num13 = num2;
				while (num13 != num3)
				{
					Vector3 vector = new Vector3(tileSize.x * ((float)num13 + num12), tileSize.y * (float)num11, 0f);
					IntVector2 intVector2 = IntVector2.Zero;
					if (!tileMap.isGungeonTilemap)
					{
						goto IL_284;
					}
					intVector2 = vector.IntXY(VectorConversions.Round) + new IntVector2(baseX, baseY);
					if (chunk.roomReference == null || chunk.roomReference.ContainsPosition(intVector2 - intVector))
					{
						if (intVector2.y * tileMap.width + intVector2.x < layerData.preprocessedFlags.Length)
						{
							if (!layerData.preprocessedFlags[intVector2.y * tileMap.width + intVector2.x])
							{
								layerData.preprocessedFlags[intVector2.y * tileMap.width + intVector2.x] = true;
								goto IL_284;
							}
						}
					}
					IL_BCD:
					num13 += num4;
					continue;
					IL_284:
					int num14 = spriteIds[num11 * chunk.Width + num13];
					int tileFromRawTile = BuilderUtil.GetTileFromRawTile(num14);
					bool flag2 = BuilderUtil.IsRawTileFlagSet(num14, tk2dTileFlags.FlipX);
					bool flag3 = BuilderUtil.IsRawTileFlagSet(num14, tk2dTileFlags.FlipY);
					bool flag4 = BuilderUtil.IsRawTileFlagSet(num14, tk2dTileFlags.Rot90);
					ColorChunk colorChunk;
					if (tileMap.isGungeonTilemap)
					{
						colorChunk = tileMap.ColorChannel.GetChunk(Mathf.FloorToInt((float)intVector2.x / (float)tileMap.partitionSizeX), Mathf.FloorToInt((float)intVector2.y / (float)tileMap.partitionSizeY));
					}
					else
					{
						colorChunk = tileMap.ColorChannel.GetChunk(Mathf.FloorToInt((float)baseX / (float)tileMap.partitionSizeX), Mathf.FloorToInt((float)baseY / (float)tileMap.partitionSizeY));
					}
					bool flag5 = useColor;
					if (colorChunk == null || (colorChunk.colors.Length == 0 && colorChunk.colorOverrides.GetLength(0) == 0))
					{
						flag5 = false;
					}
					if (tileFromRawTile < 0 || tileFromRawTile >= num)
					{
						goto IL_BCD;
					}
					if (skipPrefabs && tilePrefabs[tileFromRawTile])
					{
						goto IL_BCD;
					}
					tk2dSpriteDefinition tk2dSpriteDefinition = tileMap.SpriteCollectionInst.spriteDefinitions[tileFromRawTile];
					if (!layerData.ForceNonAnimating && tk2dSpriteDefinition.metadata.usesAnimSequence)
					{
						goto IL_BCD;
					}
					int count = list.Count;
					Vector3[] array2 = tk2dSpriteDefinition.ConstructExpensivePositions();
					for (int j = 0; j < array2.Length; j++)
					{
						Vector3 vector2 = BuilderUtil.ApplySpriteVertexTileFlags(tileMap, tk2dSpriteDefinition, array2[j], flag2, flag3, flag4);
						if (flag5)
						{
							IntVector2 intVector3 = new IntVector2(num13, num11);
							if (tileMap.isGungeonTilemap)
							{
								intVector3 = new IntVector2(intVector2.x % tileMap.partitionSizeX, intVector2.y % tileMap.partitionSizeY);
							}
							int num15 = j % 4;
							Color32 color2 = colorChunk.colorOverrides[intVector3.y * num10 + intVector3.x, num15];
							if (tileMap.isGungeonTilemap && (color2.r != color.r || color2.g != color.g || color2.b != color.b || color2.a != color.a))
							{
								Color color3 = color2;
								list2.Add(color3);
							}
							else
							{
								Color color4 = colorChunk.colors[intVector3.y * num10 + intVector3.x];
								Color color5 = colorChunk.colors[intVector3.y * num10 + intVector3.x + 1];
								Color color6 = colorChunk.colors[(intVector3.y + 1) * num10 + intVector3.x];
								Color color7 = colorChunk.colors[(intVector3.y + 1) * num10 + (intVector3.x + 1)];
								Vector3 vector3 = vector2 - tk2dSpriteDefinition.untrimmedBoundsDataCenter;
								Vector3 vector4 = vector3 + tileMap.data.tileSize * 0.5f;
								float num16 = Mathf.Clamp01(vector4.x / tileMap.data.tileSize.x);
								float num17 = Mathf.Clamp01(vector4.y / tileMap.data.tileSize.y);
								Color color8 = Color.Lerp(Color.Lerp(color4, color5, num16), Color.Lerp(color6, color7, num16), num17);
								list2.Add(color8);
							}
						}
						else
						{
							list2.Add(Color.black);
						}
						Vector3 vector5 = vector;
						if (tileMap.isGungeonTilemap)
						{
							IntVector2 intVector4 = vector.IntXY(VectorConversions.Round) + new IntVector2(baseX + RenderMeshBuilder.CurrentCellXOffset, baseY + RenderMeshBuilder.CurrentCellYOffset);
							if (dungeon.data.CheckInBounds(intVector4, 1) && dungeon.data.isAnyFaceWall(intVector4.x, intVector4.y))
							{
								Vector3 vector6 = ((!dungeon.data.isFaceWallHigher(intVector4.x, intVector4.y)) ? new Vector3(0f, 0f, 1f) : new Vector3(0f, 0f, -1f));
								CellData cellData = dungeon.data[intVector4];
								if (cellData.diagonalWallType == DiagonalWallType.NORTHEAST)
								{
									vector6.z += (1f - vector2.x) * 2f;
								}
								else if (cellData.diagonalWallType == DiagonalWallType.NORTHWEST)
								{
									vector6.z += vector2.x * 2f;
								}
								vector5 += new Vector3(0f, 0f, vector.y - vector2.y) + vector2 + vector6;
							}
							else if (dungeon.data.CheckInBounds(intVector4, 1) && dungeon.data.isTopDiagonalWall(intVector4.x, intVector4.y) && layerData.name == "Collision Layer")
							{
								Vector3 vector7 = new Vector3(0f, 0f, -3f);
								vector5 += new Vector3(0f, 0f, vector.y + vector2.y) + vector2 + vector7;
							}
							else if (layerData.name == "AOandShadows")
							{
								if (dungeon.data.CheckInBounds(intVector4, 1) && dungeon.data[intVector4] != null && dungeon.data[intVector4].type == CellType.PIT)
								{
									Vector3 vector8 = new Vector3(0f, 0f, 2.5f);
									vector5 += new Vector3(0f, 0f, vector.y + vector2.y) + vector2 + vector8;
								}
								else
								{
									Vector3 vector9 = new Vector3(0f, 0f, 1f);
									vector5 += new Vector3(0f, 0f, vector.y + vector2.y) + vector2 + vector9;
								}
							}
							else if (layerData.name == "Pit Layer")
							{
								Vector3 vector10 = new Vector3(0f, 0f, 2f);
								if (dungeon.data.CheckInBounds(intVector4.x, intVector4.y + 2))
								{
									bool flag6 = dungeon.data.cellData[intVector4.x][intVector4.y + 1].type != CellType.PIT || dungeon.data.cellData[intVector4.x][intVector4.y + 2].type != CellType.PIT;
									if (flag6)
									{
										bool flag7 = dungeon.data.cellData[intVector4.x][intVector4.y + 1].type != CellType.PIT;
										if (dungeon.debugSettings.WALLS_ARE_PITS && dungeon.data.cellData[intVector4.x][intVector4.y + 1].isExitCell)
										{
											flag7 = false;
										}
										if (flag7)
										{
											vector10 = new Vector3(0f, 0f, 0f);
										}
										vector5 += new Vector3(0f, 0f, vector.y - vector2.y) + vector2 + vector10;
									}
									else
									{
										vector5 += new Vector3(0f, 0f, vector.y + vector2.y + 1f) + vector2;
									}
								}
								else
								{
									vector5 += new Vector3(0f, 0f, vector.y + vector2.y + 1f) + vector2;
								}
							}
							else
							{
								vector5 += new Vector3(0f, 0f, vector.y + vector2.y) + vector2;
							}
						}
						else
						{
							vector5 += vector2;
						}
						list.Add(vector5);
						list3.Add(tk2dSpriteDefinition.uvs[j]);
					}
					bool flag8 = false;
					if (flag2)
					{
						flag8 = !flag8;
					}
					if (flag3)
					{
						flag8 = !flag8;
					}
					List<int> list4 = array[tk2dSpriteDefinition.materialId];
					for (int k = 0; k < tk2dSpriteDefinition.indices.Length; k++)
					{
						int num18 = ((!flag8) ? k : (tk2dSpriteDefinition.indices.Length - 1 - k));
						list4.Add(count + tk2dSpriteDefinition.indices[num18]);
					}
					goto IL_BCD;
				}
			}
			if (chunk.mesh == null)
			{
				chunk.mesh = tk2dUtil.CreateMesh();
			}
			chunk.mesh.vertices = list.ToArray();
			chunk.mesh.uv = list3.ToArray();
			chunk.mesh.colors = list2.ToArray();
			List<Material> list5 = new List<Material>();
			int num19 = 0;
			int num20 = 0;
			foreach (List<int> list6 in array)
			{
				if (list6.Count > 0)
				{
					list5.Add(tileMap.SpriteCollectionInst.materialInsts[num19]);
					num20++;
				}
				num19++;
			}
			if (num20 > 0)
			{
				chunk.mesh.subMeshCount = num20;
				chunk.gameObject.GetComponent<Renderer>().materials = list5.ToArray();
				int num21 = 0;
				foreach (List<int> list7 in array)
				{
					if (list7.Count > 0)
					{
						chunk.mesh.SetTriangles(list7.ToArray(), num21);
						num21++;
					}
				}
			}
			chunk.mesh.RecalculateBounds();
			if (flag)
			{
				chunk.mesh.RecalculateNormals();
			}
			if (tileMap.isGungeonTilemap)
			{
				chunk.gameObject.transform.position = chunk.gameObject.transform.position.WithZ((float)baseY + chunk.gameObject.transform.position.z);
			}
			MeshFilter component = chunk.gameObject.GetComponent<MeshFilter>();
			component.sharedMesh = chunk.mesh;
		}

		// Token: 0x06004159 RID: 16729 RVA: 0x0015258C File Offset: 0x0015078C
		public static IEnumerator Build(tk2dTileMap tileMap, bool editMode, bool forceBuild)
		{
			bool skipPrefabs = !editMode;
			bool incremental = !forceBuild;
			int numLayers = tileMap.data.NumLayers;
			for (int layerId = 0; layerId < numLayers; layerId++)
			{
				Layer layer = tileMap.Layers[layerId];
				if (!layer.IsEmpty)
				{
					LayerInfo layerData = tileMap.data.Layers[layerId];
					bool useColor = !tileMap.ColorChannel.IsEmpty && tileMap.data.Layers[layerId].useColor;
					bool useSortingLayer = tileMap.data.useSortingLayers;
					if (tileMap.isGungeonTilemap && SpriteChunk.s_roomChunks != null && SpriteChunk.s_roomChunks.ContainsKey(tileMap.data.Layers[layerId]))
					{
						for (int i = 0; i < SpriteChunk.s_roomChunks[tileMap.data.Layers[layerId]].Count; i++)
						{
							SpriteChunk spriteChunk = SpriteChunk.s_roomChunks[tileMap.data.Layers[layerId]][i];
							if (!spriteChunk.IsEmpty)
							{
								RenderMeshBuilder.BuildForChunk(tileMap, spriteChunk, useColor, skipPrefabs, spriteChunk.startX, spriteChunk.startY, tileMap.data.Layers[layerId]);
							}
						}
					}
					for (int j = 0; j < layer.numRows; j++)
					{
						int num = j * layer.divY;
						for (int k = 0; k < layer.numColumns; k++)
						{
							int num2 = k * layer.divX;
							SpriteChunk chunk = layer.GetChunk(k, j);
							ColorChunk chunk2 = tileMap.ColorChannel.GetChunk(k, j);
							bool flag = chunk2 != null && chunk2.Dirty;
							if (!incremental || flag || chunk.Dirty)
							{
								if (chunk.mesh != null)
								{
									chunk.mesh.Clear();
								}
								if (!chunk.IsEmpty)
								{
									if (!chunk.IrrelevantToGameplay)
									{
										if (editMode || (!editMode && !layerData.skipMeshGeneration))
										{
											RenderMeshBuilder.BuildForChunk(tileMap, chunk, useColor, skipPrefabs, num2, num, tileMap.data.Layers[layerId]);
											if (chunk.gameObject != null && useSortingLayer)
											{
												Renderer component = chunk.gameObject.GetComponent<Renderer>();
												if (component != null)
												{
													component.sortingLayerName = layerData.sortingLayerName;
													component.sortingOrder = layerData.sortingOrder;
												}
											}
										}
										if (chunk.mesh != null)
										{
											tileMap.TouchMesh(chunk.mesh);
										}
									}
								}
							}
						}
					}
					yield return null;
				}
			}
			for (int l = 0; l < numLayers; l++)
			{
				Layer layer2 = tileMap.Layers[l];
				if (!layer2.IsEmpty)
				{
					LayerInfo layerInfo = tileMap.data.Layers[l];
					layerInfo.preprocessedFlags = null;
				}
			}
			yield break;
		}

		// Token: 0x0400340A RID: 13322
		public static int CurrentCellXOffset;

		// Token: 0x0400340B RID: 13323
		public static int CurrentCellYOffset;
	}
}
