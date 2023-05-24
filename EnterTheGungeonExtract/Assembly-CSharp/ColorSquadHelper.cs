using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001123 RID: 4387
public class ColorSquadHelper : MonoBehaviour, IPlaceConfigurable
{
	// Token: 0x060060D7 RID: 24791 RVA: 0x002541DC File Offset: 0x002523DC
	private IEnumerator Start()
	{
		yield return null;
		List<FlippableCover> covers = this.m_room.GetComponentsInRoom<FlippableCover>();
		for (int i = 0; i < covers.Count; i++)
		{
			covers[i].Flip(DungeonData.GetDirectionFromVector2(BraveUtility.GetMajorAxis(covers[i].transform.position.XY() - base.transform.position.XY())));
		}
		yield break;
	}

	// Token: 0x060060D8 RID: 24792 RVA: 0x002541F8 File Offset: 0x002523F8
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.m_room = room;
	}

	// Token: 0x04005B79 RID: 23417
	private RoomHandler m_room;
}
