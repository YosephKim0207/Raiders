using System;
using Dungeonator;
using UnityEngine;

// Token: 0x0200172A RID: 5930
public class TrapController : DungeonPlaceableBehaviour
{
	// Token: 0x060089B0 RID: 35248 RVA: 0x00394470 File Offset: 0x00392670
	public virtual void Start()
	{
		if (!string.IsNullOrEmpty(this.TrapSwitchState))
		{
			AkSoundEngine.SetSwitch("ENV_Trap", this.TrapSwitchState, base.gameObject);
		}
	}

	// Token: 0x060089B1 RID: 35249 RVA: 0x0039449C File Offset: 0x0039269C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060089B2 RID: 35250 RVA: 0x003944A4 File Offset: 0x003926A4
	public override GameObject InstantiateObject(RoomHandler targetRoom, IntVector2 loc, bool deferConfiguration = false)
	{
		IntVector2 intVector = loc + targetRoom.area.basePosition;
		for (int i = intVector.x; i < intVector.x + this.placeableWidth; i++)
		{
			for (int j = intVector.y; j < intVector.y + this.placeableHeight; j++)
			{
				if (this.m_markCellOccupied)
				{
					GameManager.Instance.Dungeon.data.cellData[i][j].isOccupied = true;
				}
				GameManager.Instance.Dungeon.data.cellData[i][j].containsTrap = true;
			}
		}
		return base.InstantiateObject(targetRoom, loc, deferConfiguration);
	}

	// Token: 0x04009013 RID: 36883
	public string TrapSwitchState;

	// Token: 0x04009014 RID: 36884
	protected bool m_markCellOccupied = true;
}
