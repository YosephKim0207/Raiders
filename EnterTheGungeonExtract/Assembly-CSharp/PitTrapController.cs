using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001726 RID: 5926
public class PitTrapController : BasicTrapController
{
	// Token: 0x060089A1 RID: 35233 RVA: 0x0039400C File Offset: 0x0039220C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060089A2 RID: 35234 RVA: 0x00394014 File Offset: 0x00392214
	public override GameObject InstantiateObject(RoomHandler targetRoom, IntVector2 loc, bool deferConfiguration = false)
	{
		IntVector2 intVector = loc + targetRoom.area.basePosition;
		for (int i = intVector.x; i < intVector.x + this.placeableWidth; i++)
		{
			for (int j = intVector.y; j < intVector.y + this.placeableHeight; j++)
			{
				CellData cellData = GameManager.Instance.Dungeon.data.cellData[i][j];
				cellData.type = CellType.PIT;
				cellData.fallingPrevented = true;
			}
		}
		return base.InstantiateObject(targetRoom, loc, deferConfiguration);
	}

	// Token: 0x060089A3 RID: 35235 RVA: 0x003940B0 File Offset: 0x003922B0
	protected override void BeginState(BasicTrapController.State newState)
	{
		if (newState == BasicTrapController.State.Active)
		{
			for (int i = this.m_cachedPosition.x; i < this.m_cachedPosition.x + this.placeableWidth; i++)
			{
				for (int j = this.m_cachedPosition.y; j < this.m_cachedPosition.y + this.placeableHeight; j++)
				{
					GameManager.Instance.Dungeon.data.cellData[i][j].fallingPrevented = false;
				}
			}
			if (base.specRigidbody)
			{
				base.specRigidbody.enabled = false;
			}
		}
		else if (newState == BasicTrapController.State.Resetting)
		{
			for (int k = this.m_cachedPosition.x; k < this.m_cachedPosition.x + this.placeableWidth; k++)
			{
				for (int l = this.m_cachedPosition.y; l < this.m_cachedPosition.y + this.placeableHeight; l++)
				{
					GameManager.Instance.Dungeon.data.cellData[k][l].fallingPrevented = true;
				}
			}
			if (base.specRigidbody)
			{
				base.specRigidbody.enabled = true;
			}
		}
		base.BeginState(newState);
	}
}
