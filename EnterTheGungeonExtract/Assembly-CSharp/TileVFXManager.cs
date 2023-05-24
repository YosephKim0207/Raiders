using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200123D RID: 4669
public class TileVFXManager : MonoBehaviour
{
	// Token: 0x17000F8A RID: 3978
	// (get) Token: 0x060068A5 RID: 26789 RVA: 0x0028F7F0 File Offset: 0x0028D9F0
	// (set) Token: 0x060068A6 RID: 26790 RVA: 0x0028F820 File Offset: 0x0028DA20
	public static TileVFXManager Instance
	{
		get
		{
			if (TileVFXManager.m_instance == null)
			{
				TileVFXManager.m_instance = GameManager.Instance.Dungeon.gameObject.GetOrAddComponent<TileVFXManager>();
			}
			return TileVFXManager.m_instance;
		}
		set
		{
			TileVFXManager.m_instance = value;
		}
	}

	// Token: 0x060068A7 RID: 26791 RVA: 0x0028F828 File Offset: 0x0028DA28
	public void RegisterCellVFX(IntVector2 cellPosition, TilesetIndexMetadata metadata)
	{
		if (this.m_registeredCells.Contains(cellPosition))
		{
			Debug.Log("registering a cell twice!!!!!!");
			return;
		}
		this.m_registeredCells.Add(cellPosition);
		this.m_registeredMetadata.Add(metadata);
		RuntimeTileVFXData runtimeTileVFXData = default(RuntimeTileVFXData);
		if (metadata.tileVFXPlaystyle == TilesetIndexMetadata.VFXPlaystyle.TIMED_REPEAT)
		{
			runtimeTileVFXData.cooldownRemaining = UnityEngine.Random.Range(0f, UnityEngine.Random.Range(metadata.tileVFXDelayTime - metadata.tileVFXDelayVariance, metadata.tileVFXDelayTime + metadata.tileVFXDelayVariance));
		}
		this.m_runtimeData.Add(runtimeTileVFXData);
	}

	// Token: 0x060068A8 RID: 26792 RVA: 0x0028F8BC File Offset: 0x0028DABC
	private void CreateVFX(IntVector2 cellPosition, TilesetIndexMetadata cellMetadata, RuntimeTileVFXData runtimeData, bool ignoreCulling = false)
	{
		Vector3 vector = (cellPosition.ToVector2() + cellMetadata.tileVFXOffset).ToVector3ZUp(0f);
		vector.z = vector.y;
		if (ignoreCulling)
		{
			SpawnManager.SpawnVFX(cellMetadata.tileVFXPrefab, vector, Quaternion.identity);
		}
		else
		{
			Vector2 vector2 = this.m_frameCameraPosition - vector.XY();
			vector2.y *= 1.7f;
			if (vector2.sqrMagnitude <= 420f)
			{
				SpawnManager.SpawnVFX(cellMetadata.tileVFXPrefab, vector, Quaternion.identity);
			}
		}
	}

	// Token: 0x060068A9 RID: 26793 RVA: 0x0028F960 File Offset: 0x0028DB60
	private void Update()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}
		this.m_frameCameraPosition = GameManager.Instance.MainCameraController.transform.PositionVector2();
		if (this.m_registeredCells.Count != this.m_registeredMetadata.Count || this.m_registeredCells.Count != this.m_runtimeData.Count)
		{
			Debug.LogError("MISMATCH IN TILE VFX MANAGER, THIS IS NOT GOOD.");
			return;
		}
		for (int i = 0; i < this.m_registeredCells.Count; i++)
		{
			IntVector2 intVector = this.m_registeredCells[i];
			TilesetIndexMetadata tilesetIndexMetadata = this.m_registeredMetadata[i];
			RuntimeTileVFXData runtimeTileVFXData = this.m_runtimeData[i];
			if (tilesetIndexMetadata.tileVFXPlaystyle == TilesetIndexMetadata.VFXPlaystyle.CONTINUOUS)
			{
				if (!runtimeTileVFXData.vfxHasEverBeenInstantiated)
				{
					this.CreateVFX(intVector, tilesetIndexMetadata, runtimeTileVFXData, true);
					runtimeTileVFXData.vfxHasEverBeenInstantiated = true;
				}
			}
			else if (tilesetIndexMetadata.tileVFXPlaystyle == TilesetIndexMetadata.VFXPlaystyle.TIMED_REPEAT)
			{
				runtimeTileVFXData.cooldownRemaining = Mathf.Max(0f, runtimeTileVFXData.cooldownRemaining - BraveTime.DeltaTime);
				if (runtimeTileVFXData.cooldownRemaining <= 0f)
				{
					this.CreateVFX(intVector, tilesetIndexMetadata, runtimeTileVFXData, false);
					runtimeTileVFXData.vfxHasEverBeenInstantiated = true;
					runtimeTileVFXData.cooldownRemaining = UnityEngine.Random.Range(tilesetIndexMetadata.tileVFXDelayTime - tilesetIndexMetadata.tileVFXDelayVariance, tilesetIndexMetadata.tileVFXDelayTime + tilesetIndexMetadata.tileVFXDelayVariance);
				}
			}
			else if (tilesetIndexMetadata.tileVFXPlaystyle == TilesetIndexMetadata.VFXPlaystyle.ON_ANIMATION_FRAME && TK2DTilemapChunkAnimator.PositionToAnimatorMap.ContainsKey(intVector))
			{
				for (int j = 0; j < TK2DTilemapChunkAnimator.PositionToAnimatorMap[intVector].Count; j++)
				{
					if (TK2DTilemapChunkAnimator.PositionToAnimatorMap[intVector][j].associatedMetadata == tilesetIndexMetadata)
					{
						TilemapAnimatorTileManager tilemapAnimatorTileManager = TK2DTilemapChunkAnimator.PositionToAnimatorMap[intVector][j];
						if (tilemapAnimatorTileManager.CurrentFrame == tilesetIndexMetadata.tileVFXAnimFrame)
						{
							if (!runtimeTileVFXData.vfxHasEverBeenInstantiated)
							{
								this.CreateVFX(intVector, tilesetIndexMetadata, runtimeTileVFXData, false);
								runtimeTileVFXData.vfxHasEverBeenInstantiated = true;
							}
						}
						else
						{
							runtimeTileVFXData.vfxHasEverBeenInstantiated = false;
						}
					}
				}
			}
			this.m_runtimeData[i] = runtimeTileVFXData;
		}
	}

	// Token: 0x040064EA RID: 25834
	private static TileVFXManager m_instance;

	// Token: 0x040064EB RID: 25835
	private List<IntVector2> m_registeredCells = new List<IntVector2>();

	// Token: 0x040064EC RID: 25836
	private List<TilesetIndexMetadata> m_registeredMetadata = new List<TilesetIndexMetadata>();

	// Token: 0x040064ED RID: 25837
	private List<RuntimeTileVFXData> m_runtimeData = new List<RuntimeTileVFXData>();

	// Token: 0x040064EE RID: 25838
	private Vector2 m_frameCameraPosition;
}
