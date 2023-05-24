using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001121 RID: 4385
public class ChanceToLookNoncriticalRoom : MonoBehaviour, IPlaceConfigurable
{
	// Token: 0x060060CC RID: 24780 RVA: 0x00253DE4 File Offset: 0x00251FE4
	public void ConfigureOnPlacement(RoomHandler room)
	{
		if (room.connectedRooms.Count == 1)
		{
			room.ShouldAttemptProceduralLock = true;
			room.AttemptProceduralLockChance = UnityEngine.Random.Range(this.chanceMin, this.chanceMax);
		}
	}

	// Token: 0x04005B6E RID: 23406
	public float chanceMin = 0.3f;

	// Token: 0x04005B6F RID: 23407
	public float chanceMax = 0.5f;
}
