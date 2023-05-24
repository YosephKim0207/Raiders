using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

namespace tk2dRuntime.TileMap
{
	// Token: 0x02000BF1 RID: 3057
	[Serializable]
	public class SpriteChunk
	{
		// Token: 0x060040FC RID: 16636 RVA: 0x0014ED20 File Offset: 0x0014CF20
		public SpriteChunk(int sX, int sY, int eX, int eY)
		{
			this.startX = sX;
			this.startY = sY;
			this.endX = eX;
			this.endY = eY;
			this.spriteIds = new int[0];
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x0014ED5C File Offset: 0x0014CF5C
		public static void ClearPerLevelData()
		{
			SpriteChunk.s_roomChunks = null;
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x060040FE RID: 16638 RVA: 0x0014ED64 File Offset: 0x0014CF64
		public int Width
		{
			get
			{
				return this.endX - this.startX;
			}
		}

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x060040FF RID: 16639 RVA: 0x0014ED74 File Offset: 0x0014CF74
		public int Height
		{
			get
			{
				return this.endY - this.startY;
			}
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06004100 RID: 16640 RVA: 0x0014ED84 File Offset: 0x0014CF84
		// (set) Token: 0x06004101 RID: 16641 RVA: 0x0014ED8C File Offset: 0x0014CF8C
		public bool Dirty
		{
			get
			{
				return this.dirty;
			}
			set
			{
				this.dirty = value;
			}
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06004102 RID: 16642 RVA: 0x0014ED98 File Offset: 0x0014CF98
		public bool IsEmpty
		{
			get
			{
				return this.spriteIds.Length == 0;
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06004103 RID: 16643 RVA: 0x0014EDA8 File Offset: 0x0014CFA8
		public bool IrrelevantToGameplay
		{
			get
			{
				float num = float.MaxValue;
				for (int i = this.startX; i < this.endX; i++)
				{
					for (int j = this.startY; j < this.endY; j++)
					{
						IntVector2 intVector = new IntVector2(i + RenderMeshBuilder.CurrentCellXOffset, j + RenderMeshBuilder.CurrentCellYOffset);
						if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector))
						{
							num = Mathf.Min(num, GameManager.Instance.Dungeon.data[intVector].distanceFromNearestRoom);
						}
					}
				}
				return num > 15f;
			}
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x06004104 RID: 16644 RVA: 0x0014EE54 File Offset: 0x0014D054
		public bool HasGameData
		{
			get
			{
				return this.gameObject != null || this.mesh != null || this.meshCollider != null || this.colliderMesh != null || this.edgeColliders.Count > 0;
			}
		}

		// Token: 0x06004105 RID: 16645 RVA: 0x0014EEB8 File Offset: 0x0014D0B8
		public void DestroyGameData(tk2dTileMap tileMap)
		{
			if (this.mesh != null)
			{
				tileMap.DestroyMesh(this.mesh);
			}
			if (this.gameObject != null)
			{
				tk2dUtil.DestroyImmediate(this.gameObject);
			}
			this.gameObject = null;
			this.mesh = null;
			this.DestroyColliderData(tileMap);
		}

		// Token: 0x06004106 RID: 16646 RVA: 0x0014EF14 File Offset: 0x0014D114
		public void DestroyColliderData(tk2dTileMap tileMap)
		{
			if (this.colliderMesh != null)
			{
				tileMap.DestroyMesh(this.colliderMesh);
			}
			if (this.meshCollider != null && this.meshCollider.sharedMesh != null && this.meshCollider.sharedMesh != this.colliderMesh)
			{
				tileMap.DestroyMesh(this.meshCollider.sharedMesh);
			}
			if (this.meshCollider != null)
			{
				tk2dUtil.DestroyImmediate(this.meshCollider);
			}
			this.meshCollider = null;
			this.colliderMesh = null;
			if (this.edgeColliders.Count > 0)
			{
				for (int i = 0; i < this.edgeColliders.Count; i++)
				{
					tk2dUtil.DestroyImmediate(this.edgeColliders[i]);
				}
				this.edgeColliders.Clear();
			}
		}

		// Token: 0x040033BD RID: 13245
		public static Dictionary<LayerInfo, List<SpriteChunk>> s_roomChunks;

		// Token: 0x040033BE RID: 13246
		private bool dirty;

		// Token: 0x040033BF RID: 13247
		public int startX;

		// Token: 0x040033C0 RID: 13248
		public int startY;

		// Token: 0x040033C1 RID: 13249
		public int endX;

		// Token: 0x040033C2 RID: 13250
		public int endY;

		// Token: 0x040033C3 RID: 13251
		public RoomHandler roomReference;

		// Token: 0x040033C4 RID: 13252
		public int[] spriteIds;

		// Token: 0x040033C5 RID: 13253
		public bool[] chunkPreprocessFlags;

		// Token: 0x040033C6 RID: 13254
		public GameObject gameObject;

		// Token: 0x040033C7 RID: 13255
		public Mesh mesh;

		// Token: 0x040033C8 RID: 13256
		public MeshCollider meshCollider;

		// Token: 0x040033C9 RID: 13257
		public Mesh colliderMesh;

		// Token: 0x040033CA RID: 13258
		public List<EdgeCollider2D> edgeColliders = new List<EdgeCollider2D>();
	}
}
