using System;
using Dungeonator;
using Pathfinding;
using UnityEngine;

// Token: 0x020015AE RID: 5550
public class OccupiedCells
{
	// Token: 0x06007F51 RID: 32593 RVA: 0x0033666C File Offset: 0x0033486C
	public OccupiedCells(SpeculativeRigidbody specRigidbody, RoomHandler room)
		: this(specRigidbody, specRigidbody.PrimaryPixelCollider, room)
	{
	}

	// Token: 0x06007F52 RID: 32594 RVA: 0x0033667C File Offset: 0x0033487C
	public OccupiedCells(SpeculativeRigidbody specRigidbody, PixelCollider pixelCollider, RoomHandler room)
	{
		this.m_specRigidbody = specRigidbody;
		this.m_pixelCollider = pixelCollider;
		this.m_cachedRoom = room;
		if (this.m_cachedRoom == null)
		{
			Debug.LogError("error on: " + this.m_specRigidbody.name + this.m_specRigidbody.transform.position);
		}
		Pathfinder.Instance.RegisterObstacle(this, this.m_cachedRoom);
	}

	// Token: 0x06007F53 RID: 32595 RVA: 0x003366F0 File Offset: 0x003348F0
	public OccupiedCells(IntVector2 basePosition, IntVector2 dimensions, RoomHandler room)
	{
		this.m_usesCustom = true;
		this.m_customBasePosition = basePosition;
		this.m_customDimensions = dimensions;
		this.m_cachedRoom = room;
		if (this.m_cachedRoom == null)
		{
			Debug.LogError("error on: " + this.m_specRigidbody.name + this.m_specRigidbody.transform.position);
		}
		Pathfinder.Instance.RegisterObstacle(this, this.m_cachedRoom);
	}

	// Token: 0x06007F54 RID: 32596 RVA: 0x0033676C File Offset: 0x0033496C
	public void Clear()
	{
		if (GameManager.HasInstance && !GameManager.Instance.IsLoadingLevel && PhysicsEngine.HasInstance && this.m_specRigidbody && GameManager.Instance.Dungeon)
		{
			if (this.m_usesCustom)
			{
				RoomHandler absoluteRoom = this.m_customBasePosition.ToVector3().GetAbsoluteRoom();
				if (absoluteRoom != null)
				{
					this.m_cachedRoom = absoluteRoom;
				}
			}
			else
			{
				RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(this.m_specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor));
				if (absoluteRoomFromPosition != null)
				{
					this.m_cachedRoom = absoluteRoomFromPosition;
				}
			}
		}
		if (Pathfinder.HasInstance && this.m_cachedRoom != null)
		{
			Pathfinder.Instance.DeregisterObstacle(this, this.m_cachedRoom);
		}
	}

	// Token: 0x06007F55 RID: 32597 RVA: 0x00336848 File Offset: 0x00334A48
	public void FlagCells()
	{
		if (!GameManager.HasInstance || GameManager.Instance.Dungeon == null || GameManager.Instance.Dungeon.data == null)
		{
			return;
		}
		DungeonData data = GameManager.Instance.Dungeon.data;
		if (this.m_usesCustom)
		{
			IntVector2 customBasePosition = this.m_customBasePosition;
			IntVector2 intVector = this.m_customBasePosition + this.m_customDimensions;
			for (int i = customBasePosition.x; i < intVector.x; i++)
			{
				for (int j = customBasePosition.y; j < intVector.y; j++)
				{
					CellData cellData = data.cellData[i][j];
					if (cellData != null)
					{
						cellData.isOccupied = true;
					}
				}
			}
		}
		else
		{
			IntVector2 intVector2 = this.m_pixelCollider.UnitBottomLeft.ToIntVector2(VectorConversions.Floor);
			IntVector2 intVector3 = this.m_pixelCollider.UnitTopRight.ToIntVector2(VectorConversions.Ceil);
			for (int k = intVector2.x; k < intVector3.x; k++)
			{
				for (int l = intVector2.y; l < intVector3.y; l++)
				{
					CellData cellData2 = data.cellData[k][l];
					if (cellData2 != null)
					{
						cellData2.isOccupied = true;
					}
				}
			}
		}
	}

	// Token: 0x06007F56 RID: 32598 RVA: 0x003369A8 File Offset: 0x00334BA8
	public void UpdateCells()
	{
		Pathfinder.Instance.FlagRoomAsDirty(this.m_cachedRoom);
	}

	// Token: 0x040081F7 RID: 33271
	protected RoomHandler m_cachedRoom;

	// Token: 0x040081F8 RID: 33272
	protected SpeculativeRigidbody m_specRigidbody;

	// Token: 0x040081F9 RID: 33273
	protected PixelCollider m_pixelCollider;

	// Token: 0x040081FA RID: 33274
	protected bool m_usesCustom;

	// Token: 0x040081FB RID: 33275
	protected IntVector2 m_customBasePosition;

	// Token: 0x040081FC RID: 33276
	protected IntVector2 m_customDimensions;
}
