using System;
using Dungeonator;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C91 RID: 3217
	public class CheckEntireFloorVisited : FsmStateAction
	{
		// Token: 0x060044E5 RID: 17637 RVA: 0x00164188 File Offset: 0x00162388
		public override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060044E6 RID: 17638 RVA: 0x00164190 File Offset: 0x00162390
		public override void OnEnter()
		{
			bool flag = this.TestCompletion();
			if (flag)
			{
				if (CheckEntireFloorVisited.IsQuestCallbackActive)
				{
					Dungeon dungeon = GameManager.Instance.Dungeon;
					dungeon.OnAnyRoomVisited = (Action)Delegate.Remove(dungeon.OnAnyRoomVisited, new Action(this.NotifyComplete));
					CheckEntireFloorVisited.IsQuestCallbackActive = false;
				}
				base.Fsm.Event(this.HasVisited);
			}
			else
			{
				if (!CheckEntireFloorVisited.IsQuestCallbackActive)
				{
					Dungeon dungeon2 = GameManager.Instance.Dungeon;
					dungeon2.OnAnyRoomVisited = (Action)Delegate.Combine(dungeon2.OnAnyRoomVisited, new Action(this.NotifyComplete));
					CheckEntireFloorVisited.IsQuestCallbackActive = true;
				}
				base.Fsm.Event(this.HasNotVisited);
			}
			base.Finish();
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x00164250 File Offset: 0x00162450
		private bool TestCompletion()
		{
			bool flag = GameManager.Instance.Dungeon.AllRoomsVisited;
			if (!this.IncludeSecretRooms.Value || !this.IncludeWarpRooms.Value || this.OnlyIncludeStandardRooms.Value)
			{
				flag = true;
				for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
				{
					RoomHandler roomHandler = GameManager.Instance.Dungeon.data.rooms[i];
					bool isSecretRoom = roomHandler.IsSecretRoom;
					bool isStartOfWarpWing = roomHandler.IsStartOfWarpWing;
					bool flag2 = roomHandler.visibility == RoomHandler.VisibilityStatus.OBSCURED;
					bool flag3 = roomHandler.IsStandardRoom || roomHandler.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SPECIAL || roomHandler.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.REWARD;
					if (!roomHandler.OverrideTilemap)
					{
						if (flag2)
						{
							if (this.IncludeSecretRooms.Value || !isSecretRoom)
							{
								if (this.IncludeWarpRooms.Value || !isStartOfWarpWing)
								{
									if (!this.OnlyIncludeStandardRooms.Value || flag3)
									{
										if (!roomHandler.RevealedOnMap)
										{
											flag = false;
											break;
										}
									}
								}
							}
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x060044E8 RID: 17640 RVA: 0x001643B4 File Offset: 0x001625B4
		private void NotifyComplete()
		{
			bool flag = this.TestCompletion();
			if (flag)
			{
				CheckEntireFloorVisited.IsQuestCallbackActive = false;
				Dungeon dungeon = GameManager.Instance.Dungeon;
				dungeon.OnAnyRoomVisited = (Action)Delegate.Remove(dungeon.OnAnyRoomVisited, new Action(this.NotifyComplete));
				GameUIRoot.Instance.notificationController.DoCustomNotification(StringTableManager.GetString("#LOSTADVENTURER_NOTIFICATION_HEADER"), StringTableManager.GetString("#LOSTADVENTURER_NOTIFICATION_BODY"), base.Owner.GetComponent<TalkDoerLite>().OptionalCustomNotificationSprite.Collection, base.Owner.GetComponent<TalkDoerLite>().OptionalCustomNotificationSprite.spriteId, UINotificationController.NotificationColor.GOLD, false, false);
			}
		}

		// Token: 0x040036F0 RID: 14064
		public static bool IsQuestCallbackActive;

		// Token: 0x040036F1 RID: 14065
		[Tooltip("Event sent if there are.")]
		public FsmEvent HasVisited;

		// Token: 0x040036F2 RID: 14066
		[Tooltip("Event sent if there aren't.")]
		public FsmEvent HasNotVisited;

		// Token: 0x040036F3 RID: 14067
		public FsmBool IncludeSecretRooms = false;

		// Token: 0x040036F4 RID: 14068
		public FsmBool IncludeWarpRooms = false;

		// Token: 0x040036F5 RID: 14069
		public FsmBool OnlyIncludeStandardRooms = true;
	}
}
