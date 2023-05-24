using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001314 RID: 4884
public class GoopPlaceable : DungeonPlaceableBehaviour, IPlaceConfigurable
{
	// Token: 0x06006E14 RID: 28180 RVA: 0x002B43D4 File Offset: 0x002B25D4
	protected override void OnDestroy()
	{
		if (this.m_room != null)
		{
			this.m_room.Entered -= this.PlayerEntered;
		}
		base.OnDestroy();
	}

	// Token: 0x06006E15 RID: 28181 RVA: 0x002B4400 File Offset: 0x002B2600
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.m_room = room;
		this.m_room.Entered += this.PlayerEntered;
	}

	// Token: 0x06006E16 RID: 28182 RVA: 0x002B4420 File Offset: 0x002B2620
	public void PlayerEntered(PlayerController playerController)
	{
		DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goop);
		goopManagerForGoopType.AddGoopCircle(base.transform.position.XY() + new Vector2(0.5f, 0.5f), this.radius, -1, false, -1);
		if (this.m_room != null)
		{
			this.m_room.Entered -= this.PlayerEntered;
		}
	}

	// Token: 0x04006B6E RID: 27502
	public GoopDefinition goop;

	// Token: 0x04006B6F RID: 27503
	[DwarfConfigurable]
	public float radius = 1f;

	// Token: 0x04006B70 RID: 27504
	private RoomHandler m_room;
}
