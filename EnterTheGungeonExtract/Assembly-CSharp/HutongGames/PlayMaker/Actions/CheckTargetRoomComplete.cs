using System;
using System.Collections.Generic;
using Dungeonator;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C94 RID: 3220
	public class CheckTargetRoomComplete : FsmStateAction
	{
		// Token: 0x060044F0 RID: 17648 RVA: 0x001644D8 File Offset: 0x001626D8
		public override void Awake()
		{
			base.Awake();
			this.m_talkDoer = base.Owner.GetComponent<TalkDoerLite>();
		}

		// Token: 0x060044F1 RID: 17649 RVA: 0x001644F4 File Offset: 0x001626F4
		public override void OnEnter()
		{
			bool flag = this.CheckRoom(true);
			if (!this.m_challengeInitialized)
			{
				this.ChooseRandomChallenge();
				this.m_challengeInitialized = true;
			}
			if (flag)
			{
				base.Fsm.Event(this.hasEnemies);
			}
			else
			{
				base.Fsm.Event(this.noEnemies);
			}
			base.Finish();
		}

		// Token: 0x060044F2 RID: 17650 RVA: 0x00164554 File Offset: 0x00162754
		private bool CheckRoom(bool canFallback)
		{
			RoomHandler absoluteParentRoom = this.m_talkDoer.GetAbsoluteParentRoom();
			RoomHandler injectionTarget = absoluteParentRoom.injectionTarget;
			if (injectionTarget.visibility == RoomHandler.VisibilityStatus.OBSCURED || injectionTarget.visibility == RoomHandler.VisibilityStatus.REOBSCURED || injectionTarget.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
			{
				return true;
			}
			if (!canFallback)
			{
				return false;
			}
			if (absoluteParentRoom.distanceFromEntrance > injectionTarget.distanceFromEntrance)
			{
				for (int i = 0; i < absoluteParentRoom.connectedRooms.Count; i++)
				{
					if (absoluteParentRoom.connectedRooms[i] != null && absoluteParentRoom.connectedRooms[i].distanceFromEntrance > absoluteParentRoom.distanceFromEntrance && absoluteParentRoom.connectedRooms[i].EverHadEnemies)
					{
						absoluteParentRoom.injectionTarget = absoluteParentRoom.connectedRooms[i];
						break;
					}
				}
			}
			return this.CheckRoom(false);
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x00164638 File Offset: 0x00162838
		private void ChooseRandomChallenge()
		{
			RoomHandler injectionTarget = this.m_talkDoer.GetAbsoluteParentRoom().injectionTarget;
			List<GunslingChallengeType> list = new List<GunslingChallengeType>((GunslingChallengeType[])Enum.GetValues(typeof(GunslingChallengeType)));
			if (GameManager.Instance.PrimaryPlayer != null && (GameManager.Instance.PrimaryPlayer.CharacterUsesRandomGuns || GameManager.Instance.PrimaryPlayer.IsGunLocked))
			{
				list.Remove(GunslingChallengeType.SPECIFIC_GUN);
			}
			if (!this.IsRoomTraversibleWithoutDodgeRolls(injectionTarget))
			{
				list.Remove(GunslingChallengeType.NO_DODGE_ROLL);
			}
			if (!GameStatsManager.Instance.GetFlag(GungeonFlags.DAISUKE_ACTIVE_IN_FOYER))
			{
				list.Remove(GunslingChallengeType.DAISUKE_CHALLENGES);
			}
			if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
			{
				list.Remove(GunslingChallengeType.DAISUKE_CHALLENGES);
			}
			this.ChallengeType = BraveUtility.RandomElement<GunslingChallengeType>(list);
			base.Fsm.Variables.GetFsmInt("ChallengeType").Value = (int)this.ChallengeType;
		}

		// Token: 0x060044F4 RID: 17652 RVA: 0x00164724 File Offset: 0x00162924
		private bool IsRoomTraversibleWithoutDodgeRolls(RoomHandler room)
		{
			DungeonData data = GameManager.Instance.Dungeon.data;
			for (int i = 0; i < room.Cells.Count; i++)
			{
				if (data.CheckInBoundsAndValid(room.Cells[i]))
				{
					CellData cellData = data[room.Cells[i]];
					if (cellData.type == CellType.PIT)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x040036FE RID: 14078
		public FsmEvent noEnemies;

		// Token: 0x040036FF RID: 14079
		public FsmEvent hasEnemies;

		// Token: 0x04003700 RID: 14080
		private GunslingChallengeType ChallengeType;

		// Token: 0x04003701 RID: 14081
		private TalkDoerLite m_talkDoer;

		// Token: 0x04003702 RID: 14082
		private bool m_challengeInitialized;
	}
}
