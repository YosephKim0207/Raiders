using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001210 RID: 4624
public class SimpleRoomActivation : BraveBehaviour, IPlaceConfigurable
{
	// Token: 0x0600676F RID: 26479 RVA: 0x00287D44 File Offset: 0x00285F44
	public void ConfigureOnPlacement(RoomHandler room)
	{
		room.BecameVisible += this.Activate;
	}

	// Token: 0x06006770 RID: 26480 RVA: 0x00287D58 File Offset: 0x00285F58
	protected void Activate(float delay)
	{
		if (this.m_active)
		{
			return;
		}
		this.m_active = true;
		for (int i = 0; i < this.objectsToActivate.Length; i++)
		{
			this.objectsToActivate[i].SetActive(true);
		}
	}

	// Token: 0x06006771 RID: 26481 RVA: 0x00287DA0 File Offset: 0x00285FA0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04006350 RID: 25424
	public GameObject[] objectsToActivate;

	// Token: 0x04006351 RID: 25425
	private bool m_active;
}
