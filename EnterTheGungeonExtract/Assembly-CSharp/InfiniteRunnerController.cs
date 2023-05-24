using System;
using System.Collections;
using Dungeonator;
using HutongGames.PlayMaker.Actions;

// Token: 0x0200119A RID: 4506
public class InfiniteRunnerController : BraveBehaviour, IPlaceConfigurable
{
	// Token: 0x06006437 RID: 25655 RVA: 0x0026D964 File Offset: 0x0026BB64
	public void ConfigureOnPlacement(RoomHandler room)
	{
		base.StartCoroutine(this.HandleDelayedInitialization(room));
	}

	// Token: 0x06006438 RID: 25656 RVA: 0x0026D974 File Offset: 0x0026BB74
	private IEnumerator HandleDelayedInitialization(RoomHandler room)
	{
		yield return null;
		room.TransferInteractableOwnershipToDungeon(base.talkDoer);
		this.TargetRoom = room.injectionFrameData[room.injectionFrameData.Count - 1];
		PlayMakerFSM dungeonFsm = base.GetDungeonFSM();
		for (int i = 0; i < dungeonFsm.FsmStates.Length; i++)
		{
			for (int j = 0; j < dungeonFsm.FsmStates[i].Actions.Length; j++)
			{
				if (dungeonFsm.FsmStates[i].Actions[j] is CheckRoomVisited)
				{
					CheckRoomVisited checkRoomVisited = dungeonFsm.FsmStates[i].Actions[j] as CheckRoomVisited;
					checkRoomVisited.targetRoom = this.TargetRoom;
				}
			}
		}
		yield break;
	}

	// Token: 0x06006439 RID: 25657 RVA: 0x0026D998 File Offset: 0x0026BB98
	public void StartQuest()
	{
		base.talkDoer.PathfindToPosition(this.TargetRoom.GetCenterCell().ToVector2(), null, null);
	}

	// Token: 0x0600643A RID: 25658 RVA: 0x0026D9D0 File Offset: 0x0026BBD0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005FD1 RID: 24529
	[NonSerialized]
	public RoomHandler TargetRoom;
}
