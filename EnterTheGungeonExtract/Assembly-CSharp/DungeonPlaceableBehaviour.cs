using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02000E74 RID: 3700
public class DungeonPlaceableBehaviour : BraveBehaviour, IHasDwarfConfigurables
{
	// Token: 0x17000B1E RID: 2846
	// (get) Token: 0x06004EB8 RID: 20152 RVA: 0x001B39A8 File Offset: 0x001B1BA8
	// (set) Token: 0x06004EB9 RID: 20153 RVA: 0x001B39B0 File Offset: 0x001B1BB0
	public IntVector2 PlacedPosition { get; set; }

	// Token: 0x06004EBA RID: 20154 RVA: 0x001B39BC File Offset: 0x001B1BBC
	public virtual GameObject InstantiateObjectDirectional(RoomHandler targetRoom, IntVector2 location, DungeonData.Direction direction)
	{
		BraveUtility.Log("Calling InstantiateDirectional on a DungeonPlaceableBehaviour that hasn't implemented it.", Color.yellow, BraveUtility.LogVerbosity.IMPORTANT);
		return DungeonPlaceableUtility.InstantiateDungeonPlaceable(base.gameObject, targetRoom, location, false, AIActor.AwakenAnimationType.Default, false);
	}

	// Token: 0x06004EBB RID: 20155 RVA: 0x001B39E0 File Offset: 0x001B1BE0
	public virtual GameObject InstantiateObject(RoomHandler targetRoom, IntVector2 location, bool deferConfiguration = false)
	{
		return DungeonPlaceableUtility.InstantiateDungeonPlaceable(base.gameObject, targetRoom, location, deferConfiguration, AIActor.AwakenAnimationType.Default, false);
	}

	// Token: 0x06004EBC RID: 20156 RVA: 0x001B39F4 File Offset: 0x001B1BF4
	public virtual GameObject InstantiateObjectOnlyActors(RoomHandler targetRoom, IntVector2 location, bool deferConfiguration = false)
	{
		return DungeonPlaceableUtility.InstantiateDungeonPlaceableOnlyActors(base.gameObject, targetRoom, location, deferConfiguration);
	}

	// Token: 0x06004EBD RID: 20157 RVA: 0x001B3A04 File Offset: 0x001B1C04
	public virtual int GetMinimumDifficulty()
	{
		return 0;
	}

	// Token: 0x06004EBE RID: 20158 RVA: 0x001B3A08 File Offset: 0x001B1C08
	public virtual int GetMaximumDifficulty()
	{
		return 0;
	}

	// Token: 0x06004EBF RID: 20159 RVA: 0x001B3A0C File Offset: 0x001B1C0C
	public virtual int GetWidth()
	{
		return this.placeableWidth;
	}

	// Token: 0x06004EC0 RID: 20160 RVA: 0x001B3A14 File Offset: 0x001B1C14
	public virtual int GetHeight()
	{
		return this.placeableHeight;
	}

	// Token: 0x06004EC1 RID: 20161 RVA: 0x001B3A1C File Offset: 0x001B1C1C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06004EC2 RID: 20162 RVA: 0x001B3A24 File Offset: 0x001B1C24
	public RoomHandler GetAbsoluteParentRoom()
	{
		return GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
	}

	// Token: 0x06004EC3 RID: 20163 RVA: 0x001B3A4C File Offset: 0x001B1C4C
	public void SetAreaPassable()
	{
		for (int i = this.PlacedPosition.x; i < this.PlacedPosition.x + this.placeableWidth; i++)
		{
			for (int j = this.PlacedPosition.y; j < this.PlacedPosition.y + this.placeableHeight; j++)
			{
				GameManager.Instance.Dungeon.data[i, j].isOccupied = false;
			}
		}
	}

	// Token: 0x04004580 RID: 17792
	[SerializeField]
	public int placeableWidth = 1;

	// Token: 0x04004581 RID: 17793
	[SerializeField]
	public int placeableHeight = 1;

	// Token: 0x04004582 RID: 17794
	[SerializeField]
	public DungeonPlaceableBehaviour.PlaceableDifficulty difficulty;

	// Token: 0x04004583 RID: 17795
	[SerializeField]
	public bool isPassable = true;

	// Token: 0x02000E75 RID: 3701
	public enum PlaceableDifficulty
	{
		// Token: 0x04004586 RID: 17798
		BASE,
		// Token: 0x04004587 RID: 17799
		HARD,
		// Token: 0x04004588 RID: 17800
		HARDER,
		// Token: 0x04004589 RID: 17801
		HARDEST
	}
}
