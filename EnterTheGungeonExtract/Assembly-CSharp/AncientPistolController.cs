using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020010E9 RID: 4329
public class AncientPistolController : BraveBehaviour, IPlaceConfigurable
{
	// Token: 0x06005F57 RID: 24407 RVA: 0x0024AA20 File Offset: 0x00248C20
	public void ConfigureOnPlacement(RoomHandler room)
	{
		base.StartCoroutine(this.HandleDelayedInitialization(room));
	}

	// Token: 0x06005F58 RID: 24408 RVA: 0x0024AA30 File Offset: 0x00248C30
	private IEnumerator HandleDelayedInitialization(RoomHandler room)
	{
		yield return null;
		room.TransferInteractableOwnershipToDungeon(base.talkDoer);
		this.RoomSequence = new List<RoomHandler>();
		this.RoomSequenceEnemies = new List<bool>();
		for (int i = 1; i < room.injectionFrameData.Count; i++)
		{
			this.RoomSequence.Add(room.injectionFrameData[i]);
			this.RoomSequenceEnemies.Add(i < room.injectionFrameData.Count - 1);
		}
		if (this.RoomSequence.Count < 1)
		{
			Debug.LogError("Failed to initialize Ancient Pistol1");
		}
		yield break;
	}

	// Token: 0x06005F59 RID: 24409 RVA: 0x0024AA54 File Offset: 0x00248C54
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040059C1 RID: 22977
	[NonSerialized]
	public List<RoomHandler> RoomSequence;

	// Token: 0x040059C2 RID: 22978
	public List<bool> RoomSequenceEnemies;
}
