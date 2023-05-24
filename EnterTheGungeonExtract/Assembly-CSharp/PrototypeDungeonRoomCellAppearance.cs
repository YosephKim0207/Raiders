using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000F53 RID: 3923
[Serializable]
public class PrototypeDungeonRoomCellAppearance
{
	// Token: 0x06005480 RID: 21632 RVA: 0x001FBB9C File Offset: 0x001F9D9C
	public PrototypeDungeonRoomCellAppearance()
	{
		this.globalOverrideIndices = new PrototypeIndexOverrideData();
		this.m_overrideIndices = new List<PrototypeIndexOverrideData>();
	}

	// Token: 0x06005481 RID: 21633 RVA: 0x001FBBC4 File Offset: 0x001F9DC4
	public bool HasChanges()
	{
		return this.overrideDungeonMaterialIndex != -1 || this.IsPhantomCarpet || this.ForceDisallowGoop || this.OverrideFloorType != CellVisualData.CellFloorType.Stone || this.globalOverrideIndices.indices.Count != 0 || this.m_overrideIndices.Count != 0;
	}

	// Token: 0x06005482 RID: 21634 RVA: 0x001FBC28 File Offset: 0x001F9E28
	public bool HasAnyOverride()
	{
		for (int i = 0; i < this.m_overrideIndices.Count; i++)
		{
			if (this.m_overrideIndices[i].indices != null && this.m_overrideIndices[i].indices.Count != 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005483 RID: 21635 RVA: 0x001FBC8C File Offset: 0x001F9E8C
	public List<int> GetOverridesForTilemap(PrototypeDungeonRoom sourceRoom, GlobalDungeonData.ValidTilesets tileset)
	{
		if ((sourceRoom.overriddenTilesets & tileset) == tileset)
		{
			int i = Mathf.RoundToInt(Mathf.Log((float)tileset, 2f));
			while (i >= this.m_overrideIndices.Count)
			{
				this.m_overrideIndices.Add(new PrototypeIndexOverrideData());
			}
			if (this.m_overrideIndices[i].indices.Count > 0)
			{
				return this.m_overrideIndices[i].indices;
			}
			if (this.globalOverrideIndices.indices.Count > 0)
			{
				return this.globalOverrideIndices.indices;
			}
		}
		return null;
	}

	// Token: 0x06005484 RID: 21636 RVA: 0x001FBD30 File Offset: 0x001F9F30
	public void SetAllOverridesForTilemap(GlobalDungeonData.ValidTilesets tileset, List<int> overrides)
	{
		int num = Mathf.RoundToInt(Mathf.Log((float)tileset, 2f));
		PrototypeIndexOverrideData prototypeIndexOverrideData = new PrototypeIndexOverrideData();
		prototypeIndexOverrideData.indices = overrides;
		if (num < this.m_overrideIndices.Count)
		{
			this.m_overrideIndices[num] = prototypeIndexOverrideData;
		}
		else
		{
			while (num != this.m_overrideIndices.Count)
			{
				this.m_overrideIndices.Add(new PrototypeIndexOverrideData());
			}
			this.m_overrideIndices.Add(prototypeIndexOverrideData);
		}
	}

	// Token: 0x06005485 RID: 21637 RVA: 0x001FBDB4 File Offset: 0x001F9FB4
	public void ClearOverrideForTilemap(GlobalDungeonData.ValidTilesets tileset)
	{
		int num = Mathf.RoundToInt(Mathf.Log((float)tileset, 2f));
		if (num < this.m_overrideIndices.Count)
		{
			this.m_overrideIndices[num] = new PrototypeIndexOverrideData();
		}
	}

	// Token: 0x06005486 RID: 21638 RVA: 0x001FBDF8 File Offset: 0x001F9FF8
	public void ClearAllOverrideData()
	{
		this.m_overrideIndices.Clear();
		this.globalOverrideIndices.indices.Clear();
	}

	// Token: 0x06005487 RID: 21639 RVA: 0x001FBE18 File Offset: 0x001FA018
	public void MirrorData(PrototypeDungeonRoomCellAppearance source)
	{
		this.overrideDungeonMaterialIndex = source.overrideDungeonMaterialIndex;
		this.IsPhantomCarpet = source.IsPhantomCarpet;
		this.ForceDisallowGoop = source.ForceDisallowGoop;
		this.OverrideFloorType = source.OverrideFloorType;
		this.globalOverrideIndices = new PrototypeIndexOverrideData();
		this.globalOverrideIndices.indices = new List<int>();
		if (source.globalOverrideIndices.indices != null)
		{
			for (int i = 0; i < source.globalOverrideIndices.indices.Count; i++)
			{
				this.globalOverrideIndices.indices.Add(source.globalOverrideIndices.indices[i]);
			}
		}
		this.m_overrideIndices = new List<PrototypeIndexOverrideData>();
		for (int j = 0; j < source.m_overrideIndices.Count; j++)
		{
			this.m_overrideIndices.Add(new PrototypeIndexOverrideData());
			this.m_overrideIndices[j].indices = new List<int>();
			if (source.m_overrideIndices[j].indices != null)
			{
				for (int k = 0; k < source.m_overrideIndices[j].indices.Count; k++)
				{
					this.m_overrideIndices[j].indices.Add(source.m_overrideIndices[j].indices[k]);
				}
			}
		}
	}

	// Token: 0x04004D72 RID: 19826
	[SerializeField]
	public int overrideDungeonMaterialIndex = -1;

	// Token: 0x04004D73 RID: 19827
	[SerializeField]
	public bool IsPhantomCarpet;

	// Token: 0x04004D74 RID: 19828
	[SerializeField]
	public bool ForceDisallowGoop;

	// Token: 0x04004D75 RID: 19829
	[SerializeField]
	public CellVisualData.CellFloorType OverrideFloorType;

	// Token: 0x04004D76 RID: 19830
	[SerializeField]
	public PrototypeIndexOverrideData globalOverrideIndices;

	// Token: 0x04004D77 RID: 19831
	[SerializeField]
	private List<PrototypeIndexOverrideData> m_overrideIndices;
}
