using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020011E2 RID: 4578
public class ResizableCollider : DungeonPlaceableBehaviour, IPlaceConfigurable, IDwarfDrawable
{
	// Token: 0x06006625 RID: 26149 RVA: 0x0027B0B4 File Offset: 0x002792B4
	public IntVector2 GetOverrideDwarfDimensions(PrototypePlacedObjectData objectData)
	{
		int num = (int)objectData.GetFieldValueByName("NumTiles");
		if (this.IsHorizontal)
		{
			return new IntVector2(num, 1);
		}
		return new IntVector2(1, num);
	}

	// Token: 0x06006626 RID: 26150 RVA: 0x0027B0E8 File Offset: 0x002792E8
	private IEnumerator FrameDelayedConfiguration()
	{
		yield return null;
		for (int i = 0; i < this.spriteSources.Length; i++)
		{
			if (this.IsHorizontal)
			{
				int num = Mathf.RoundToInt(this.spriteSources[i].dimensions.x % 16f);
				this.spriteSources[i].dimensions = this.spriteSources[i].dimensions.WithX(this.NumTiles * 16f + (float)num);
			}
			else
			{
				int num2 = Mathf.RoundToInt(this.spriteSources[i].dimensions.y % 16f);
				this.spriteSources[i].dimensions = this.spriteSources[i].dimensions.WithY(this.NumTiles * 16f + (float)num2);
			}
		}
		if (base.specRigidbody)
		{
			for (int j = 0; j < base.specRigidbody.PixelColliders.Count; j++)
			{
				if (this.IsHorizontal)
				{
					base.specRigidbody.PixelColliders[j].ManualWidth = (int)this.NumTiles * 16;
				}
				else
				{
					base.specRigidbody.PixelColliders[j].ManualHeight = (int)this.NumTiles * 16;
				}
				base.specRigidbody.PixelColliders[j].Regenerate(base.transform, false, false);
			}
			base.specRigidbody.Reinitialize();
			this.m_cells = new OccupiedCells(base.specRigidbody, base.GetAbsoluteParentRoom());
		}
		yield break;
	}

	// Token: 0x06006627 RID: 26151 RVA: 0x0027B104 File Offset: 0x00279304
	protected override void OnDestroy()
	{
		if (this.m_cells != null)
		{
			this.m_cells.Clear();
		}
		base.OnDestroy();
	}

	// Token: 0x06006628 RID: 26152 RVA: 0x0027B124 File Offset: 0x00279324
	public void ConfigureOnPlacement(RoomHandler room)
	{
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Round);
		int num = 0;
		while ((float)num < this.NumTiles)
		{
			IntVector2 intVector2 = intVector + new IntVector2(0, num);
			if (this.IsHorizontal)
			{
				intVector2 = intVector + new IntVector2(num, 0);
			}
			if (GameManager.Instance.Dungeon.data.CheckInBounds(intVector2))
			{
				CellData cellData = GameManager.Instance.Dungeon.data[intVector2];
				if (cellData != null)
				{
					cellData.isOccupied = true;
				}
			}
			num++;
		}
		base.StartCoroutine(this.FrameDelayedConfiguration());
	}

	// Token: 0x040061FA RID: 25082
	public bool IsHorizontal = true;

	// Token: 0x040061FB RID: 25083
	[DwarfConfigurable]
	public float NumTiles = 3f;

	// Token: 0x040061FC RID: 25084
	public tk2dSlicedSprite[] spriteSources;

	// Token: 0x040061FD RID: 25085
	private OccupiedCells m_cells;
}
