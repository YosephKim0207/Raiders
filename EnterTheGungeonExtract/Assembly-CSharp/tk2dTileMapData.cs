using System;
using System.Collections.Generic;
using tk2dRuntime.TileMap;
using UnityEngine;

// Token: 0x02000BFE RID: 3070
public class tk2dTileMapData : ScriptableObject
{
	// Token: 0x170009E6 RID: 2534
	// (get) Token: 0x06004152 RID: 16722 RVA: 0x00151630 File Offset: 0x0014F830
	public int NumLayers
	{
		get
		{
			if (this.tileMapLayers == null || this.tileMapLayers.Count == 0)
			{
				this.InitLayers();
			}
			return this.tileMapLayers.Count;
		}
	}

	// Token: 0x170009E7 RID: 2535
	// (get) Token: 0x06004153 RID: 16723 RVA: 0x00151660 File Offset: 0x0014F860
	public LayerInfo[] Layers
	{
		get
		{
			if (this.tileMapLayers == null || this.tileMapLayers.Count == 0)
			{
				this.InitLayers();
			}
			return this.tileMapLayers.ToArray();
		}
	}

	// Token: 0x06004154 RID: 16724 RVA: 0x00151690 File Offset: 0x0014F890
	public TileInfo GetTileInfoForSprite(int tileId)
	{
		if (this.tileInfo == null || tileId < 0 || tileId >= this.tileInfo.Length)
		{
			return null;
		}
		return this.tileInfo[tileId];
	}

	// Token: 0x06004155 RID: 16725 RVA: 0x001516BC File Offset: 0x0014F8BC
	public TileInfo[] GetOrCreateTileInfo(int numTiles)
	{
		bool flag = false;
		if (this.tileInfo == null)
		{
			this.tileInfo = new TileInfo[numTiles];
			flag = true;
		}
		else if (this.tileInfo.Length != numTiles)
		{
			Array.Resize<TileInfo>(ref this.tileInfo, numTiles);
			flag = true;
		}
		if (flag)
		{
			for (int i = 0; i < this.tileInfo.Length; i++)
			{
				if (this.tileInfo[i] == null)
				{
					this.tileInfo[i] = new TileInfo();
				}
			}
		}
		return this.tileInfo;
	}

	// Token: 0x06004156 RID: 16726 RVA: 0x00151744 File Offset: 0x0014F944
	public void GetTileOffset(out float x, out float y)
	{
		tk2dTileMapData.TileType tileType = this.tileType;
		if (tileType != tk2dTileMapData.TileType.Isometric)
		{
			if (tileType != tk2dTileMapData.TileType.Rectangular)
			{
			}
			x = 0f;
			y = 0f;
		}
		else
		{
			x = 0.5f;
			y = 0f;
		}
	}

	// Token: 0x06004157 RID: 16727 RVA: 0x00151790 File Offset: 0x0014F990
	private void InitLayers()
	{
		this.tileMapLayers = new List<LayerInfo>();
		LayerInfo layerInfo = new LayerInfo();
		layerInfo = new LayerInfo();
		layerInfo.name = "Layer 0";
		layerInfo.hash = 1892887448;
		layerInfo.z = 0f;
		this.tileMapLayers.Add(layerInfo);
	}

	// Token: 0x040033F9 RID: 13305
	public Vector3 tileSize;

	// Token: 0x040033FA RID: 13306
	public Vector3 tileOrigin;

	// Token: 0x040033FB RID: 13307
	public tk2dTileMapData.TileType tileType;

	// Token: 0x040033FC RID: 13308
	public tk2dTileMapData.SortMethod sortMethod;

	// Token: 0x040033FD RID: 13309
	public bool layersFixedZ;

	// Token: 0x040033FE RID: 13310
	public bool useSortingLayers;

	// Token: 0x040033FF RID: 13311
	public GameObject[] tilePrefabs = new GameObject[0];

	// Token: 0x04003400 RID: 13312
	[SerializeField]
	private TileInfo[] tileInfo = new TileInfo[0];

	// Token: 0x04003401 RID: 13313
	[SerializeField]
	public List<LayerInfo> tileMapLayers = new List<LayerInfo>();

	// Token: 0x02000BFF RID: 3071
	public enum SortMethod
	{
		// Token: 0x04003403 RID: 13315
		BottomLeft,
		// Token: 0x04003404 RID: 13316
		TopLeft,
		// Token: 0x04003405 RID: 13317
		BottomRight,
		// Token: 0x04003406 RID: 13318
		TopRight
	}

	// Token: 0x02000C00 RID: 3072
	public enum TileType
	{
		// Token: 0x04003408 RID: 13320
		Rectangular,
		// Token: 0x04003409 RID: 13321
		Isometric
	}
}
