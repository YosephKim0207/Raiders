using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C84 RID: 3204
	[Tooltip("Paths to near the player's current location.")]
	[ActionCategory(".NPCs")]
	public class AncientPistol_WaitForRoomClear : FsmStateAction
	{
		// Token: 0x060044B0 RID: 17584 RVA: 0x00162FF0 File Offset: 0x001611F0
		public override void Awake()
		{
			base.Awake();
			this.m_pistolDoer = base.Owner.GetComponent<AncientPistolController>();
			this.m_currentTargetRoomIndex = 0;
		}

		// Token: 0x060044B1 RID: 17585 RVA: 0x00163010 File Offset: 0x00161210
		public override void OnEnter()
		{
			this.m_lastPosition = this.m_pistolDoer.specRigidbody.UnitCenter;
			Vector2 vector = Vector2.zero;
			RoomHandler roomHandler = this.m_pistolDoer.RoomSequence[this.m_currentTargetRoomIndex];
			RoomHandler roomHandler2 = this.m_pistolDoer.talkDoer.GetAbsoluteParentRoom();
			if (this.m_currentTargetRoomIndex > 0)
			{
				roomHandler2 = this.m_pistolDoer.RoomSequence[this.m_currentTargetRoomIndex - 1];
			}
			PrototypeRoomExit exitConnectedToRoom = roomHandler2.GetExitConnectedToRoom(roomHandler);
			if (exitConnectedToRoom != null)
			{
				vector = (exitConnectedToRoom.GetExitOrigin(0) - IntVector2.One + roomHandler2.area.basePosition + -5 * DungeonData.GetIntVector2FromDirection(exitConnectedToRoom.exitDirection)).ToVector2();
				vector = roomHandler2.GetBestRewardLocation(IntVector2.One, vector, false).ToVector2();
				this.m_pistolDoer.StartCoroutine(this.HandleDelayedPathing(vector));
			}
		}

		// Token: 0x060044B2 RID: 17586 RVA: 0x00163100 File Offset: 0x00161300
		private IEnumerator HandleDelayedPathing(Vector2 targetPosition)
		{
			yield return new WaitForSeconds(0.5f);
			this.m_pistolDoer.talkDoer.PathfindToPosition(targetPosition, null, null);
			yield break;
		}

		// Token: 0x060044B3 RID: 17587 RVA: 0x00163124 File Offset: 0x00161324
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (this.m_pistolDoer.talkDoer.CurrentPath != null)
			{
				if (!this.m_pistolDoer.talkDoer.CurrentPath.WillReachFinalGoal)
				{
					RoomHandler roomHandler = this.m_pistolDoer.RoomSequence[this.m_currentTargetRoomIndex];
					RoomHandler roomHandler2 = ((this.m_currentTargetRoomIndex != 0) ? this.m_pistolDoer.RoomSequence[this.m_currentTargetRoomIndex - 1] : this.m_pistolDoer.talkDoer.GetAbsoluteParentRoom());
					PrototypeRoomExit exitConnectedToRoom = roomHandler2.GetExitConnectedToRoom(roomHandler);
					if (exitConnectedToRoom != null)
					{
						IntVector2 intVector = exitConnectedToRoom.GetExitOrigin(0) - IntVector2.One + roomHandler2.area.basePosition + -5 * DungeonData.GetIntVector2FromDirection(exitConnectedToRoom.exitDirection);
						this.m_pistolDoer.transform.position = intVector.ToVector3();
						this.m_pistolDoer.specRigidbody.Reinitialize();
						PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(this.m_pistolDoer.specRigidbody, new int?(CollisionMask.LayerToMask(CollisionLayer.PlayerCollider)), false);
						this.m_pistolDoer.talkDoer.CurrentPath = null;
					}
				}
				else
				{
					this.m_pistolDoer.talkDoer.specRigidbody.Velocity = this.m_pistolDoer.talkDoer.GetPathVelocityContribution(this.m_lastPosition, 32);
					this.m_lastPosition = this.m_pistolDoer.talkDoer.specRigidbody.UnitCenter;
				}
			}
		}

		// Token: 0x060044B4 RID: 17588 RVA: 0x001632A4 File Offset: 0x001614A4
		public override void OnLateUpdate()
		{
			if (this.m_pistolDoer.talkDoer.CurrentPath != null)
			{
				return;
			}
			RoomHandler roomHandler = this.m_pistolDoer.RoomSequence[this.m_currentTargetRoomIndex];
			bool flag = GameManager.Instance.BestActivePlayer.CurrentRoom != roomHandler && this.m_currentTargetRoomIndex == this.m_pistolDoer.RoomSequence.Count - 1;
			if (!roomHandler.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear) && !flag)
			{
				this.m_currentTargetRoomIndex++;
				if (this.m_currentTargetRoomIndex >= this.m_pistolDoer.RoomSequence.Count)
				{
					base.Fsm.Event(this.NoMoreRooms);
				}
				else
				{
					base.Fsm.Event(this.MoreRooms);
				}
				base.Finish();
			}
		}

		// Token: 0x040036BF RID: 14015
		[Tooltip("Event sent if there are more rooms.")]
		public FsmEvent MoreRooms;

		// Token: 0x040036C0 RID: 14016
		[Tooltip("Event sent if there aren't.")]
		public FsmEvent NoMoreRooms;

		// Token: 0x040036C1 RID: 14017
		private AncientPistolController m_pistolDoer;

		// Token: 0x040036C2 RID: 14018
		private int m_currentTargetRoomIndex;

		// Token: 0x040036C3 RID: 14019
		private Vector2 m_lastPosition;
	}
}
