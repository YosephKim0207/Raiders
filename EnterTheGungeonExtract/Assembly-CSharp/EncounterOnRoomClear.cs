using System;
using Dungeonator;

// Token: 0x02001090 RID: 4240
public class EncounterOnRoomClear : BraveBehaviour
{
	// Token: 0x06005D4B RID: 23883 RVA: 0x0023C4C4 File Offset: 0x0023A6C4
	public void Start()
	{
		RoomHandler parentRoom = base.aiActor.ParentRoom;
		parentRoom.OnEnemiesCleared = (Action)Delegate.Combine(parentRoom.OnEnemiesCleared, new Action(this.RoomCleared));
	}

	// Token: 0x06005D4C RID: 23884 RVA: 0x0023C4F4 File Offset: 0x0023A6F4
	protected override void OnDestroy()
	{
		if (base.aiActor && base.aiActor.ParentRoom != null)
		{
			RoomHandler parentRoom = base.aiActor.ParentRoom;
			parentRoom.OnEnemiesCleared = (Action)Delegate.Remove(parentRoom.OnEnemiesCleared, new Action(this.RoomCleared));
		}
		base.OnDestroy();
	}

	// Token: 0x06005D4D RID: 23885 RVA: 0x0023C554 File Offset: 0x0023A754
	private void RoomCleared()
	{
		if (base.encounterTrackable)
		{
			GameStatsManager.Instance.HandleEncounteredObject(base.encounterTrackable);
		}
	}
}
