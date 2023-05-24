using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using tk2dRuntime;
using tk2dRuntime.TileMap;
using UnityEngine;

// Token: 0x02000BEA RID: 3050
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/TileMap/TileMap")]
public class tk2dTileMap : MonoBehaviour, ISpriteCollectionForceBuild
{
	// Token: 0x170009D1 RID: 2513
	// (get) Token: 0x060040B7 RID: 16567 RVA: 0x0014BB30 File Offset: 0x00149D30
	// (set) Token: 0x060040B8 RID: 16568 RVA: 0x0014BB38 File Offset: 0x00149D38
	public tk2dSpriteCollectionData Editor__SpriteCollection
	{
		get
		{
			return this.spriteCollection;
		}
		set
		{
			this.spriteCollection = value;
		}
	}

	// Token: 0x170009D2 RID: 2514
	// (get) Token: 0x060040B9 RID: 16569 RVA: 0x0014BB44 File Offset: 0x00149D44
	public tk2dSpriteCollectionData SpriteCollectionInst
	{
		get
		{
			if (this.spriteCollection != null)
			{
				return this.spriteCollection.inst;
			}
			return null;
		}
	}

	// Token: 0x170009D3 RID: 2515
	// (get) Token: 0x060040BA RID: 16570 RVA: 0x0014BB64 File Offset: 0x00149D64
	public bool AllowEdit
	{
		get
		{
			return this._inEditMode;
		}
	}

	// Token: 0x060040BB RID: 16571 RVA: 0x0014BB6C File Offset: 0x00149D6C
	private void Awake()
	{
		bool flag = true;
		if (this.SpriteCollectionInst && (this.SpriteCollectionInst.buildKey != this.spriteCollectionKey || this.SpriteCollectionInst.needMaterialInstance))
		{
			flag = false;
		}
		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
		{
			if ((Application.isPlaying && this._inEditMode) || !flag)
			{
				this.EndEditMode();
			}
			else if (this.spriteCollection != null && this.data != null && this.renderData == null)
			{
				this.Build(tk2dTileMap.BuildFlags.ForceBuild);
			}
		}
		else if (this._inEditMode)
		{
			Debug.LogError("Tilemap " + base.name + " is still in edit mode. Please fix.Building overhead will be significant.");
			this.EndEditMode();
		}
		else if (!flag)
		{
			this.Build(tk2dTileMap.BuildFlags.ForceBuild);
		}
		else if (this.spriteCollection != null && this.data != null && this.renderData == null)
		{
			this.Build(tk2dTileMap.BuildFlags.ForceBuild);
		}
	}

	// Token: 0x060040BC RID: 16572 RVA: 0x0014BCAC File Offset: 0x00149EAC
	private void OnDestroy()
	{
		if (this.layers != null)
		{
			foreach (Layer layer in this.layers)
			{
				layer.DestroyGameData(this);
			}
		}
		if (this.renderData != null)
		{
			tk2dUtil.DestroyImmediate(this.renderData);
		}
	}

	// Token: 0x060040BD RID: 16573 RVA: 0x0014BD08 File Offset: 0x00149F08
	public void Build()
	{
		this.Build(tk2dTileMap.BuildFlags.Default);
	}

	// Token: 0x060040BE RID: 16574 RVA: 0x0014BD14 File Offset: 0x00149F14
	public void ForceBuild()
	{
		this.Build(tk2dTileMap.BuildFlags.ForceBuild);
	}

	// Token: 0x060040BF RID: 16575 RVA: 0x0014BD20 File Offset: 0x00149F20
	private void ClearSpawnedInstances()
	{
		if (this.layers == null)
		{
			return;
		}
		BuilderUtil.HideTileMapPrefabs(this);
		for (int i = 0; i < this.layers.Length; i++)
		{
			Layer layer = this.layers[i];
			for (int j = 0; j < layer.spriteChannel.chunks.Length; j++)
			{
				SpriteChunk spriteChunk = layer.spriteChannel.chunks[j];
				if (!(spriteChunk.gameObject == null))
				{
					Transform transform = spriteChunk.gameObject.transform;
					List<Transform> list = new List<Transform>();
					for (int k = 0; k < transform.childCount; k++)
					{
						list.Add(transform.GetChild(k));
					}
					for (int l = 0; l < list.Count; l++)
					{
						tk2dUtil.DestroyImmediate(list[l].gameObject);
					}
				}
			}
		}
	}

	// Token: 0x060040C0 RID: 16576 RVA: 0x0014BE14 File Offset: 0x0014A014
	private void SetPrefabsRootActive(bool active)
	{
		if (this.prefabsRoot != null)
		{
			tk2dUtil.SetActive(this.prefabsRoot, active);
		}
	}

	// Token: 0x060040C1 RID: 16577 RVA: 0x0014BE34 File Offset: 0x0014A034
	public void Build(tk2dTileMap.BuildFlags buildFlags)
	{
		IEnumerator enumerator = this.DeferredBuild(buildFlags);
		while (enumerator.MoveNext())
		{
		}
	}

	// Token: 0x060040C2 RID: 16578 RVA: 0x0014BE5C File Offset: 0x0014A05C
	public IEnumerator DeferredBuild(tk2dTileMap.BuildFlags buildFlags)
	{
		if (this.data != null && this.spriteCollection != null)
		{
			if (this.data.tilePrefabs == null)
			{
				this.data.tilePrefabs = new GameObject[this.SpriteCollectionInst.Count];
			}
			else if (this.data.tilePrefabs.Length != this.SpriteCollectionInst.Count)
			{
				Array.Resize<GameObject>(ref this.data.tilePrefabs, this.SpriteCollectionInst.Count);
			}
			BuilderUtil.InitDataStore(this);
			if (this.SpriteCollectionInst)
			{
				this.SpriteCollectionInst.InitMaterialIds();
			}
			bool forceBuild = (buildFlags & tk2dTileMap.BuildFlags.ForceBuild) != tk2dTileMap.BuildFlags.Default;
			if (this.SpriteCollectionInst && this.SpriteCollectionInst.buildKey != this.spriteCollectionKey)
			{
				forceBuild = true;
			}
			Dictionary<Layer, bool> layersActive = new Dictionary<Layer, bool>();
			if (this.layers != null)
			{
				for (int i = 0; i < this.layers.Length; i++)
				{
					Layer layer = this.layers[i];
					if (layer != null && layer.gameObject != null)
					{
						layersActive[layer] = layer.gameObject.activeSelf;
					}
				}
			}
			if (forceBuild)
			{
				this.ClearSpawnedInstances();
			}
			BuilderUtil.CreateRenderData(this, this._inEditMode, layersActive);
			SpriteChunk.s_roomChunks = new Dictionary<LayerInfo, List<SpriteChunk>>();
			if (Application.isPlaying && GameManager.Instance.Dungeon != null && GameManager.Instance.Dungeon.data != null && GameManager.Instance.Dungeon.MainTilemap == this)
			{
				List<RoomHandler> rooms = GameManager.Instance.Dungeon.data.rooms;
				if (rooms != null && rooms.Count > 0)
				{
					for (int j = 0; j < this.data.Layers.Length; j++)
					{
						if (this.data.Layers[j].overrideChunkable)
						{
							for (int k = 0; k < rooms.Count; k++)
							{
								if (!SpriteChunk.s_roomChunks.ContainsKey(this.data.Layers[j]))
								{
									SpriteChunk.s_roomChunks.Add(this.data.Layers[j], new List<SpriteChunk>());
								}
								SpriteChunk spriteChunk = new SpriteChunk(rooms[k].area.basePosition.x + this.data.Layers[j].overrideChunkXOffset, rooms[k].area.basePosition.y + this.data.Layers[j].overrideChunkYOffset, rooms[k].area.basePosition.x + rooms[k].area.dimensions.x + this.data.Layers[j].overrideChunkXOffset, rooms[k].area.basePosition.y + rooms[k].area.dimensions.y + this.data.Layers[j].overrideChunkYOffset);
								spriteChunk.roomReference = rooms[k];
								string prototypeRoomName = rooms[k].area.PrototypeRoomName;
								this.Layers[j].CreateOverrideChunk(spriteChunk);
								BuilderUtil.CreateOverrideChunkData(spriteChunk, this, j, prototypeRoomName);
								SpriteChunk.s_roomChunks[this.data.Layers[j]].Add(spriteChunk);
							}
						}
					}
				}
			}
			IEnumerator BuildTracker = RenderMeshBuilder.Build(this, this._inEditMode, forceBuild);
			while (BuildTracker.MoveNext())
			{
				yield return null;
			}
			if (!this._inEditMode && this.isGungeonTilemap)
			{
				BuilderUtil.SpawnAnimatedTiles(this, forceBuild);
			}
			if (!this._inEditMode)
			{
				tk2dSpriteDefinition firstValidDefinition = this.SpriteCollectionInst.FirstValidDefinition;
				if (firstValidDefinition != null && firstValidDefinition.physicsEngine == tk2dSpriteDefinition.PhysicsEngine.Physics2D)
				{
					ColliderBuilder2D.Build(this, forceBuild);
				}
				else
				{
					ColliderBuilder3D.Build(this, forceBuild);
				}
				BuilderUtil.SpawnPrefabs(this, forceBuild);
			}
			foreach (Layer layer2 in this.layers)
			{
				layer2.ClearDirtyFlag();
			}
			if (this.colorChannel != null)
			{
				this.colorChannel.ClearDirtyFlag();
			}
			if (this.SpriteCollectionInst)
			{
				this.spriteCollectionKey = this.SpriteCollectionInst.buildKey;
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x060040C3 RID: 16579 RVA: 0x0014BE80 File Offset: 0x0014A080
	public bool GetTileAtPosition(Vector3 position, out int x, out int y)
	{
		float num;
		float num2;
		bool tileFracAtPosition = this.GetTileFracAtPosition(position, out num, out num2);
		x = (int)num;
		y = (int)num2;
		return tileFracAtPosition;
	}

	// Token: 0x060040C4 RID: 16580 RVA: 0x0014BEA4 File Offset: 0x0014A0A4
	public bool GetTileFracAtPosition(Vector3 position, out float x, out float y)
	{
		tk2dTileMapData.TileType tileType = this.data.tileType;
		if (tileType != tk2dTileMapData.TileType.Rectangular)
		{
			if (tileType == tk2dTileMapData.TileType.Isometric)
			{
				if (this.data.tileSize.x != 0f)
				{
					float num = Mathf.Atan2(this.data.tileSize.y, this.data.tileSize.x / 2f);
					Vector3 vector = base.transform.worldToLocalMatrix.MultiplyPoint(position);
					x = (vector.x - this.data.tileOrigin.x) / this.data.tileSize.x;
					y = (vector.y - this.data.tileOrigin.y) / this.data.tileSize.y;
					float num2 = y * 0.5f;
					int num3 = (int)num2;
					float num4 = num2 - (float)num3;
					float num5 = x % 1f;
					x = (float)((int)x);
					y = (float)(num3 * 2);
					if (num5 > 0.5f)
					{
						if (num4 > 0.5f && Mathf.Atan2(1f - num4, (num5 - 0.5f) * 2f) < num)
						{
							y += 1f;
						}
						else if (num4 < 0.5f && Mathf.Atan2(num4, (num5 - 0.5f) * 2f) < num)
						{
							y -= 1f;
						}
					}
					else if (num5 < 0.5f)
					{
						if (num4 > 0.5f && Mathf.Atan2(num4 - 0.5f, num5 * 2f) > num)
						{
							y += 1f;
							x -= 1f;
						}
						if (num4 < 0.5f && Mathf.Atan2(num4, (0.5f - num5) * 2f) < num)
						{
							y -= 1f;
							x -= 1f;
						}
					}
					return x >= 0f && x <= (float)this.width && y >= 0f && y <= (float)this.height;
				}
			}
			x = 0f;
			y = 0f;
			return false;
		}
		Vector3 vector2 = base.transform.worldToLocalMatrix.MultiplyPoint(position);
		x = (vector2.x - this.data.tileOrigin.x) / this.data.tileSize.x;
		y = (vector2.y - this.data.tileOrigin.y) / this.data.tileSize.y;
		return x >= 0f && x <= (float)this.width && y >= 0f && y <= (float)this.height;
	}

	// Token: 0x060040C5 RID: 16581 RVA: 0x0014C1A0 File Offset: 0x0014A3A0
	public Vector3 GetTilePosition(int x, int y)
	{
		tk2dTileMapData.TileType tileType = this.data.tileType;
		if (tileType == tk2dTileMapData.TileType.Rectangular || tileType != tk2dTileMapData.TileType.Isometric)
		{
			Vector3 vector = new Vector3((float)x * this.data.tileSize.x + this.data.tileOrigin.x, (float)y * this.data.tileSize.y + this.data.tileOrigin.y, 0f);
			return base.transform.localToWorldMatrix.MultiplyPoint(vector);
		}
		Vector3 vector2 = new Vector3(((float)x + (((y & 1) != 0) ? 0.5f : 0f)) * this.data.tileSize.x + this.data.tileOrigin.x, (float)y * this.data.tileSize.y + this.data.tileOrigin.y, 0f);
		return base.transform.localToWorldMatrix.MultiplyPoint(vector2);
	}

	// Token: 0x060040C6 RID: 16582 RVA: 0x0014C2B8 File Offset: 0x0014A4B8
	public int GetTileIdAtPosition(Vector3 position, int layer)
	{
		if (layer < 0 || layer >= this.layers.Length)
		{
			return -1;
		}
		int num;
		int num2;
		if (!this.GetTileAtPosition(position, out num, out num2))
		{
			return -1;
		}
		return this.layers[layer].GetTile(num, num2);
	}

	// Token: 0x060040C7 RID: 16583 RVA: 0x0014C300 File Offset: 0x0014A500
	public TileInfo GetTileInfoForTileId(int tileId)
	{
		return this.data.GetTileInfoForSprite(tileId);
	}

	// Token: 0x060040C8 RID: 16584 RVA: 0x0014C310 File Offset: 0x0014A510
	public Color GetInterpolatedColorAtPosition(Vector3 position)
	{
		Vector3 vector = base.transform.worldToLocalMatrix.MultiplyPoint(position);
		int num = (int)((vector.x - this.data.tileOrigin.x) / this.data.tileSize.x);
		int num2 = (int)((vector.y - this.data.tileOrigin.y) / this.data.tileSize.y);
		if (this.colorChannel == null || this.colorChannel.IsEmpty)
		{
			return Color.white;
		}
		if (num < 0 || num >= this.width || num2 < 0 || num2 >= this.height)
		{
			return this.colorChannel.clearColor;
		}
		int num3;
		ColorChunk colorChunk = this.colorChannel.FindChunkAndCoordinate(num, num2, out num3);
		if (colorChunk.Empty)
		{
			return this.colorChannel.clearColor;
		}
		int num4 = this.partitionSizeX + 1;
		Color color = colorChunk.colors[num3];
		Color color2 = colorChunk.colors[num3 + 1];
		Color color3 = colorChunk.colors[num3 + num4];
		Color color4 = colorChunk.colors[num3 + num4 + 1];
		float num5 = (float)num * this.data.tileSize.x + this.data.tileOrigin.x;
		float num6 = (float)num2 * this.data.tileSize.y + this.data.tileOrigin.y;
		float num7 = (vector.x - num5) / this.data.tileSize.x;
		float num8 = (vector.y - num6) / this.data.tileSize.y;
		Color color5 = Color.Lerp(color, color2, num7);
		Color color6 = Color.Lerp(color3, color4, num7);
		return Color.Lerp(color5, color6, num8);
	}

	// Token: 0x060040C9 RID: 16585 RVA: 0x0014C52C File Offset: 0x0014A72C
	public bool UsesSpriteCollection(tk2dSpriteCollectionData spriteCollection)
	{
		return this.spriteCollection != null && (spriteCollection == this.spriteCollection || spriteCollection == this.spriteCollection.inst);
	}

	// Token: 0x060040CA RID: 16586 RVA: 0x0014C568 File Offset: 0x0014A768
	public void EndEditMode()
	{
		this._inEditMode = false;
		this.SetPrefabsRootActive(true);
		this.Build(tk2dTileMap.BuildFlags.ForceBuild);
		if (this.prefabsRoot != null)
		{
			tk2dUtil.DestroyImmediate(this.prefabsRoot);
			this.prefabsRoot = null;
		}
	}

	// Token: 0x060040CB RID: 16587 RVA: 0x0014C5A4 File Offset: 0x0014A7A4
	public bool AreSpritesInitialized()
	{
		return this.layers != null;
	}

	// Token: 0x060040CC RID: 16588 RVA: 0x0014C5B4 File Offset: 0x0014A7B4
	public bool HasColorChannel()
	{
		return this.colorChannel != null && !this.colorChannel.IsEmpty;
	}

	// Token: 0x060040CD RID: 16589 RVA: 0x0014C5D4 File Offset: 0x0014A7D4
	public void CreateColorChannel()
	{
		this.colorChannel = new ColorChannel(this.width, this.height, this.partitionSizeX, this.partitionSizeY);
		this.colorChannel.Create();
	}

	// Token: 0x060040CE RID: 16590 RVA: 0x0014C604 File Offset: 0x0014A804
	public void DeleteColorChannel()
	{
		this.colorChannel.Delete();
	}

	// Token: 0x060040CF RID: 16591 RVA: 0x0014C614 File Offset: 0x0014A814
	public void DeleteSprites(int layerId, int x0, int y0, int x1, int y1)
	{
		x0 = Mathf.Clamp(x0, 0, this.width - 1);
		y0 = Mathf.Clamp(y0, 0, this.height - 1);
		x1 = Mathf.Clamp(x1, 0, this.width - 1);
		y1 = Mathf.Clamp(y1, 0, this.height - 1);
		int num = x1 - x0 + 1;
		int num2 = y1 - y0 + 1;
		Layer layer = this.layers[layerId];
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				layer.SetTile(x0 + j, y0 + i, -1);
			}
		}
		layer.OptimizeIncremental();
	}

	// Token: 0x060040D0 RID: 16592 RVA: 0x0014C6BC File Offset: 0x0014A8BC
	public void TouchMesh(Mesh mesh)
	{
	}

	// Token: 0x060040D1 RID: 16593 RVA: 0x0014C6C0 File Offset: 0x0014A8C0
	public void DestroyMesh(Mesh mesh)
	{
		tk2dUtil.DestroyImmediate(mesh);
	}

	// Token: 0x060040D2 RID: 16594 RVA: 0x0014C6C8 File Offset: 0x0014A8C8
	public int GetTilePrefabsListCount()
	{
		return this.tilePrefabsList.Count;
	}

	// Token: 0x170009D4 RID: 2516
	// (get) Token: 0x060040D3 RID: 16595 RVA: 0x0014C6D8 File Offset: 0x0014A8D8
	public List<tk2dTileMap.TilemapPrefabInstance> TilePrefabsList
	{
		get
		{
			return this.tilePrefabsList;
		}
	}

	// Token: 0x060040D4 RID: 16596 RVA: 0x0014C6E0 File Offset: 0x0014A8E0
	public void GetTilePrefabsListItem(int index, out int x, out int y, out int layer, out GameObject instance)
	{
		tk2dTileMap.TilemapPrefabInstance tilemapPrefabInstance = this.tilePrefabsList[index];
		x = tilemapPrefabInstance.x;
		y = tilemapPrefabInstance.y;
		layer = tilemapPrefabInstance.layer;
		instance = tilemapPrefabInstance.instance;
	}

	// Token: 0x060040D5 RID: 16597 RVA: 0x0014C71C File Offset: 0x0014A91C
	public void SetTilePrefabsList(List<int> xs, List<int> ys, List<int> layers, List<GameObject> instances)
	{
		int count = instances.Count;
		this.tilePrefabsList = new List<tk2dTileMap.TilemapPrefabInstance>(count);
		for (int i = 0; i < count; i++)
		{
			tk2dTileMap.TilemapPrefabInstance tilemapPrefabInstance = new tk2dTileMap.TilemapPrefabInstance();
			tilemapPrefabInstance.x = xs[i];
			tilemapPrefabInstance.y = ys[i];
			tilemapPrefabInstance.layer = layers[i];
			tilemapPrefabInstance.instance = instances[i];
			this.tilePrefabsList.Add(tilemapPrefabInstance);
		}
	}

	// Token: 0x170009D5 RID: 2517
	// (get) Token: 0x060040D6 RID: 16598 RVA: 0x0014C798 File Offset: 0x0014A998
	// (set) Token: 0x060040D7 RID: 16599 RVA: 0x0014C7A0 File Offset: 0x0014A9A0
	public Layer[] Layers
	{
		get
		{
			return this.layers;
		}
		set
		{
			this.layers = value;
		}
	}

	// Token: 0x170009D6 RID: 2518
	// (get) Token: 0x060040D8 RID: 16600 RVA: 0x0014C7AC File Offset: 0x0014A9AC
	// (set) Token: 0x060040D9 RID: 16601 RVA: 0x0014C7B4 File Offset: 0x0014A9B4
	public ColorChannel ColorChannel
	{
		get
		{
			return this.colorChannel;
		}
		set
		{
			this.colorChannel = value;
		}
	}

	// Token: 0x170009D7 RID: 2519
	// (get) Token: 0x060040DA RID: 16602 RVA: 0x0014C7C0 File Offset: 0x0014A9C0
	// (set) Token: 0x060040DB RID: 16603 RVA: 0x0014C7C8 File Offset: 0x0014A9C8
	public GameObject PrefabsRoot
	{
		get
		{
			return this.prefabsRoot;
		}
		set
		{
			this.prefabsRoot = value;
		}
	}

	// Token: 0x060040DC RID: 16604 RVA: 0x0014C7D4 File Offset: 0x0014A9D4
	public int GetTile(int x, int y, int layer)
	{
		if (layer < 0 || layer >= this.layers.Length)
		{
			return -1;
		}
		return this.layers[layer].GetTile(x, y);
	}

	// Token: 0x060040DD RID: 16605 RVA: 0x0014C7FC File Offset: 0x0014A9FC
	public tk2dTileFlags GetTileFlags(int x, int y, int layer)
	{
		if (layer < 0 || layer >= this.layers.Length)
		{
			return tk2dTileFlags.None;
		}
		return this.layers[layer].GetTileFlags(x, y);
	}

	// Token: 0x060040DE RID: 16606 RVA: 0x0014C824 File Offset: 0x0014AA24
	public void SetTile(int x, int y, int layer, int tile)
	{
		if (layer < 0 || layer >= this.layers.Length)
		{
			return;
		}
		this.layers[layer].SetTile(x, y, tile);
	}

	// Token: 0x060040DF RID: 16607 RVA: 0x0014C850 File Offset: 0x0014AA50
	public void SetTileFlags(int x, int y, int layer, tk2dTileFlags flags)
	{
		if (layer < 0 || layer >= this.layers.Length)
		{
			return;
		}
		this.layers[layer].SetTileFlags(x, y, flags);
	}

	// Token: 0x060040E0 RID: 16608 RVA: 0x0014C87C File Offset: 0x0014AA7C
	public void ClearTile(int x, int y, int layer)
	{
		if (layer < 0 || layer >= this.layers.Length)
		{
			return;
		}
		this.layers[layer].ClearTile(x, y);
	}

	// Token: 0x04003391 RID: 13201
	public string editorDataGUID = string.Empty;

	// Token: 0x04003392 RID: 13202
	public tk2dTileMapData data;

	// Token: 0x04003393 RID: 13203
	public GameObject renderData;

	// Token: 0x04003394 RID: 13204
	[SerializeField]
	private tk2dSpriteCollectionData spriteCollection;

	// Token: 0x04003395 RID: 13205
	[SerializeField]
	private int spriteCollectionKey;

	// Token: 0x04003396 RID: 13206
	public int width = 128;

	// Token: 0x04003397 RID: 13207
	public int height = 128;

	// Token: 0x04003398 RID: 13208
	public int partitionSizeX = 32;

	// Token: 0x04003399 RID: 13209
	public int partitionSizeY = 32;

	// Token: 0x0400339A RID: 13210
	public bool isGungeonTilemap = true;

	// Token: 0x0400339B RID: 13211
	[SerializeField]
	private Layer[] layers;

	// Token: 0x0400339C RID: 13212
	[SerializeField]
	private ColorChannel colorChannel;

	// Token: 0x0400339D RID: 13213
	[SerializeField]
	private GameObject prefabsRoot;

	// Token: 0x0400339E RID: 13214
	[SerializeField]
	private List<tk2dTileMap.TilemapPrefabInstance> tilePrefabsList = new List<tk2dTileMap.TilemapPrefabInstance>();

	// Token: 0x0400339F RID: 13215
	[SerializeField]
	private bool _inEditMode;

	// Token: 0x040033A0 RID: 13216
	public string serializedMeshPath;

	// Token: 0x02000BEB RID: 3051
	[Serializable]
	public class TilemapPrefabInstance
	{
		// Token: 0x040033A1 RID: 13217
		public int x;

		// Token: 0x040033A2 RID: 13218
		public int y;

		// Token: 0x040033A3 RID: 13219
		public int layer;

		// Token: 0x040033A4 RID: 13220
		public GameObject instance;
	}

	// Token: 0x02000BEC RID: 3052
	[Flags]
	public enum BuildFlags
	{
		// Token: 0x040033A6 RID: 13222
		Default = 0,
		// Token: 0x040033A7 RID: 13223
		EditMode = 1,
		// Token: 0x040033A8 RID: 13224
		ForceBuild = 2
	}
}
