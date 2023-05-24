using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001240 RID: 4672
public class TK2DTilemapChunkAnimator : MonoBehaviour
{
	// Token: 0x060068B4 RID: 26804 RVA: 0x00290248 File Offset: 0x0028E448
	public void Initialize(List<TilemapAnimatorTileManager> tiles, Mesh refMesh, tk2dTileMap refTilemap)
	{
		this.m_tiles = tiles;
		for (int i = 0; i < this.m_tiles.Count; i++)
		{
			this.m_tiles[i].animator = this;
		}
		this.m_refMesh = refMesh;
		this.m_refTilemap = refTilemap;
		this.m_currentUVs = this.m_refMesh.uv;
	}

	// Token: 0x060068B5 RID: 26805 RVA: 0x002902AC File Offset: 0x0028E4AC
	private void Update()
	{
		bool flag = false;
		for (int i = 0; i < this.m_tiles.Count; i++)
		{
			if (this.m_tiles[i].UpdateRelevantSection(ref this.m_currentUVs, this.m_refMesh, this.m_refTilemap, BraveTime.DeltaTime))
			{
				flag = true;
			}
		}
		if (flag)
		{
			this.m_refMesh.uv = this.m_currentUVs;
		}
	}

	// Token: 0x04006505 RID: 25861
	public static Dictionary<IntVector2, List<TilemapAnimatorTileManager>> PositionToAnimatorMap = new Dictionary<IntVector2, List<TilemapAnimatorTileManager>>(new IntVector2EqualityComparer());

	// Token: 0x04006506 RID: 25862
	private List<TilemapAnimatorTileManager> m_tiles;

	// Token: 0x04006507 RID: 25863
	private Mesh m_refMesh;

	// Token: 0x04006508 RID: 25864
	private tk2dTileMap m_refTilemap;

	// Token: 0x04006509 RID: 25865
	private Vector2[] m_currentUVs;
}
