using System;
using System.Collections.Generic;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x02000BF9 RID: 3065
	public static class ColliderBuilder2D
	{
		// Token: 0x06004143 RID: 16707 RVA: 0x00150A1C File Offset: 0x0014EC1C
		public static void Build(tk2dTileMap tileMap, bool forceBuild)
		{
			bool flag = !forceBuild;
			int num = tileMap.Layers.Length;
			for (int i = 0; i < num; i++)
			{
				Layer layer = tileMap.Layers[i];
				if (!layer.IsEmpty && tileMap.data.Layers[i].generateCollider)
				{
					for (int j = 0; j < layer.numRows; j++)
					{
						int num2 = j * layer.divY;
						for (int k = 0; k < layer.numColumns; k++)
						{
							int num3 = k * layer.divX;
							SpriteChunk chunk = layer.GetChunk(k, j);
							if (!flag || chunk.Dirty)
							{
								if (!chunk.IsEmpty)
								{
									ColliderBuilder2D.BuildForChunk(tileMap, chunk, num3, num2);
									PhysicsMaterial2D physicsMaterial2D = tileMap.data.Layers[i].physicsMaterial2D;
									foreach (EdgeCollider2D edgeCollider2D in chunk.edgeColliders)
									{
										if (edgeCollider2D != null)
										{
											edgeCollider2D.sharedMaterial = physicsMaterial2D;
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004144 RID: 16708 RVA: 0x00150B7C File Offset: 0x0014ED7C
		public static void BuildForChunk(tk2dTileMap tileMap, SpriteChunk chunk, int baseX, int baseY)
		{
			Vector2[] array = new Vector2[0];
			int[] array2 = new int[0];
			List<Vector2[]> list = new List<Vector2[]>();
			ColliderBuilder2D.BuildLocalMeshForChunk(tileMap, chunk, baseX, baseY, ref array, ref array2);
			if (array2.Length > 4)
			{
				array = ColliderBuilder2D.WeldVertices(array, ref array2);
				array2 = ColliderBuilder2D.RemoveDuplicateEdges(array2);
			}
			list = ColliderBuilder2D.MergeEdges(array, array2);
			if (chunk.meshCollider != null)
			{
				tk2dUtil.DestroyImmediate(chunk.meshCollider);
				chunk.meshCollider = null;
			}
			if (list.Count == 0)
			{
				for (int i = 0; i < chunk.edgeColliders.Count; i++)
				{
					if (chunk.edgeColliders[i] != null)
					{
						tk2dUtil.DestroyImmediate(chunk.edgeColliders[i]);
					}
				}
				chunk.edgeColliders.Clear();
			}
			else
			{
				int count = list.Count;
				for (int j = count; j < chunk.edgeColliders.Count; j++)
				{
					if (chunk.edgeColliders[j] != null)
					{
						tk2dUtil.DestroyImmediate(chunk.edgeColliders[j]);
					}
				}
				int num = chunk.edgeColliders.Count - count;
				if (num > 0)
				{
					chunk.edgeColliders.RemoveRange(chunk.edgeColliders.Count - num, num);
				}
				for (int k = 0; k < chunk.edgeColliders.Count; k++)
				{
					if (chunk.edgeColliders[k] == null)
					{
						chunk.edgeColliders[k] = tk2dUtil.AddComponent<EdgeCollider2D>(chunk.gameObject);
					}
				}
				while (chunk.edgeColliders.Count < count)
				{
					chunk.edgeColliders.Add(tk2dUtil.AddComponent<EdgeCollider2D>(chunk.gameObject));
				}
				for (int l = 0; l < count; l++)
				{
					chunk.edgeColliders[l].points = list[l];
				}
			}
		}

		// Token: 0x06004145 RID: 16709 RVA: 0x00150D84 File Offset: 0x0014EF84
		private static void BuildLocalMeshForChunk(tk2dTileMap tileMap, SpriteChunk chunk, int baseX, int baseY, ref Vector2[] vertices, ref int[] indices)
		{
			List<Vector2> list = new List<Vector2>();
			List<int> list2 = new List<int>();
			Vector2[] array = new Vector2[4];
			int[] array2 = new int[] { 0, 1, 1, 2, 2, 3, 3, 0 };
			int[] array3 = new int[] { 0, 3, 3, 2, 2, 1, 1, 0 };
			int num = tileMap.SpriteCollectionInst.spriteDefinitions.Length;
			Vector2 vector = new Vector3(tileMap.data.tileSize.x, tileMap.data.tileSize.y);
			GameObject[] tilePrefabs = tileMap.data.tilePrefabs;
			float num2 = 0f;
			float num3 = 0f;
			tileMap.data.GetTileOffset(out num2, out num3);
			int[] spriteIds = chunk.spriteIds;
			for (int i = 0; i < tileMap.partitionSizeY; i++)
			{
				float num4 = (float)((baseY + i) & 1) * num2;
				for (int j = 0; j < tileMap.partitionSizeX; j++)
				{
					int num5 = spriteIds[i * tileMap.partitionSizeX + j];
					int tileFromRawTile = BuilderUtil.GetTileFromRawTile(num5);
					Vector2 vector2 = new Vector2(vector.x * ((float)j + num4), vector.y * (float)i);
					if (tileFromRawTile >= 0 && tileFromRawTile < num)
					{
						if (!tilePrefabs[tileFromRawTile])
						{
							bool flag = BuilderUtil.IsRawTileFlagSet(num5, tk2dTileFlags.FlipX);
							bool flag2 = BuilderUtil.IsRawTileFlagSet(num5, tk2dTileFlags.FlipY);
							bool flag3 = BuilderUtil.IsRawTileFlagSet(num5, tk2dTileFlags.Rot90);
							bool flag4 = false;
							if (flag)
							{
								flag4 = !flag4;
							}
							if (flag2)
							{
								flag4 = !flag4;
							}
							tk2dSpriteDefinition tk2dSpriteDefinition = tileMap.SpriteCollectionInst.spriteDefinitions[tileFromRawTile];
							int count = list.Count;
							if (tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Box)
							{
								Vector3 vector3 = tk2dSpriteDefinition.colliderVertices[0];
								Vector3 vector4 = tk2dSpriteDefinition.colliderVertices[1];
								Vector3 vector5 = vector3 - vector4;
								Vector3 vector6 = vector3 + vector4;
								array[0] = new Vector2(vector5.x, vector5.y);
								array[1] = new Vector2(vector6.x, vector5.y);
								array[2] = new Vector2(vector6.x, vector6.y);
								array[3] = new Vector2(vector5.x, vector6.y);
								for (int k = 0; k < 4; k++)
								{
									list.Add(BuilderUtil.ApplySpriteVertexTileFlags(tileMap, tk2dSpriteDefinition, array[k], flag, flag2, flag3) + vector2);
								}
								int[] array4 = ((!flag4) ? array2 : array3);
								for (int l = 0; l < 8; l++)
								{
									list2.Add(count + array4[l]);
								}
							}
							else if (tk2dSpriteDefinition.colliderType == tk2dSpriteDefinition.ColliderType.Mesh)
							{
							}
						}
					}
				}
			}
			vertices = list.ToArray();
			indices = list2.ToArray();
		}

		// Token: 0x06004146 RID: 16710 RVA: 0x0015109C File Offset: 0x0014F29C
		private static int CompareWeldVertices(Vector2 a, Vector2 b)
		{
			float num = 0.01f;
			float num2 = a.x - b.x;
			if (Mathf.Abs(num2) > num)
			{
				return (int)Mathf.Sign(num2);
			}
			float num3 = a.y - b.y;
			if (Mathf.Abs(num3) > num)
			{
				return (int)Mathf.Sign(num3);
			}
			return 0;
		}

		// Token: 0x06004147 RID: 16711 RVA: 0x001510F8 File Offset: 0x0014F2F8
		private static Vector2[] WeldVertices(Vector2[] vertices, ref int[] indices)
		{
			int[] array = new int[vertices.Length];
			for (int i = 0; i < vertices.Length; i++)
			{
				array[i] = i;
			}
			Array.Sort<int>(array, (int a, int b) => ColliderBuilder2D.CompareWeldVertices(vertices[a], vertices[b]));
			List<Vector2> list = new List<Vector2>();
			int[] array2 = new int[vertices.Length];
			Vector2 vector = vertices[array[0]];
			list.Add(vector);
			array2[array[0]] = list.Count - 1;
			for (int j = 1; j < array.Length; j++)
			{
				Vector2 vector2 = vertices[array[j]];
				if (ColliderBuilder2D.CompareWeldVertices(vector2, vector) != 0)
				{
					vector = vector2;
					list.Add(vector);
					array2[array[j]] = list.Count - 1;
				}
				array2[array[j]] = list.Count - 1;
			}
			for (int k = 0; k < indices.Length; k++)
			{
				indices[k] = array2[indices[k]];
			}
			return list.ToArray();
		}

		// Token: 0x06004148 RID: 16712 RVA: 0x00151224 File Offset: 0x0014F424
		private static int CompareDuplicateFaces(int[] indices, int face0index, int face1index)
		{
			for (int i = 0; i < 2; i++)
			{
				int num = indices[face0index + i] - indices[face1index + i];
				if (num != 0)
				{
					return num;
				}
			}
			return 0;
		}

		// Token: 0x06004149 RID: 16713 RVA: 0x00151258 File Offset: 0x0014F458
		private static int[] RemoveDuplicateEdges(int[] indices)
		{
			int[] sortedFaceIndices = new int[indices.Length];
			for (int i = 0; i < indices.Length; i += 2)
			{
				if (indices[i] > indices[i + 1])
				{
					sortedFaceIndices[i] = indices[i + 1];
					sortedFaceIndices[i + 1] = indices[i];
				}
				else
				{
					sortedFaceIndices[i] = indices[i];
					sortedFaceIndices[i + 1] = indices[i + 1];
				}
			}
			int[] array = new int[indices.Length / 2];
			for (int j = 0; j < indices.Length; j += 2)
			{
				array[j / 2] = j;
			}
			Array.Sort<int>(array, (int a, int b) => ColliderBuilder2D.CompareDuplicateFaces(sortedFaceIndices, a, b));
			List<int> list = new List<int>();
			for (int k = 0; k < array.Length; k++)
			{
				if (k != array.Length - 1 && ColliderBuilder2D.CompareDuplicateFaces(sortedFaceIndices, array[k], array[k + 1]) == 0)
				{
					k++;
				}
				else
				{
					for (int l = 0; l < 2; l++)
					{
						list.Add(indices[array[k] + l]);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600414A RID: 16714 RVA: 0x00151388 File Offset: 0x0014F588
		private static List<Vector2[]> MergeEdges(Vector2[] verts, int[] indices)
		{
			List<Vector2[]> list = new List<Vector2[]>();
			List<Vector2> list2 = new List<Vector2>();
			List<int> list3 = new List<int>();
			Vector2 vector = Vector2.zero;
			Vector2 vector2 = Vector2.zero;
			bool[] array = new bool[indices.Length / 2];
			bool flag = true;
			while (flag)
			{
				flag = false;
				for (int i = 0; i < array.Length; i++)
				{
					if (!array[i])
					{
						array[i] = true;
						int num = indices[i * 2];
						int num2 = indices[i * 2 + 1];
						vector = (verts[num2] - verts[num]).normalized;
						list3.Add(num);
						list3.Add(num2);
						for (int j = i + 1; j < array.Length; j++)
						{
							if (!array[j])
							{
								int num3 = indices[j * 2];
								if (num3 == num2)
								{
									int num4 = indices[j * 2 + 1];
									vector2 = (verts[num4] - verts[num3]).normalized;
									if (Vector2.Dot(vector2, vector) > 0.999f)
									{
										list3.RemoveAt(list3.Count - 1);
									}
									list3.Add(num4);
									array[j] = true;
									vector = vector2;
									j = i;
									num2 = num4;
								}
							}
						}
						flag = true;
						break;
					}
				}
				if (flag)
				{
					list2.Clear();
					list2.Capacity = Mathf.Max(list2.Capacity, list3.Count);
					for (int k = 0; k < list3.Count; k++)
					{
						list2.Add(verts[list3[k]]);
					}
					list.Add(list2.ToArray());
					list3.Clear();
				}
			}
			return list;
		}
	}
}
