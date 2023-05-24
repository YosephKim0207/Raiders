using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C9E RID: 3230
	[Tooltip("Force teleport players to a certain area in the room if they're not already there.")]
	[ActionCategory(".NPCs")]
	public class EnsurePlayersAreInRoomArea : FsmStateAction
	{
		// Token: 0x06004517 RID: 17687 RVA: 0x0016638C File Offset: 0x0016458C
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			Vector2 vector = component.ParentRoom.area.UnitBottomLeft + this.lowerLeftRoomTile;
			Vector2 vector2 = component.ParentRoom.area.UnitBottomLeft + this.upperRightRoomTile;
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[i];
				if (!BraveMathCollege.AABBContains(vector, vector2, playerController.specRigidbody.GetUnitCenter(ColliderType.HitBox)))
				{
					Vector2 vector3 = new Vector2((vector.x + vector2.x) / 2f - 0.5f, vector.y + 1f);
					if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
					{
						vector3.x += 1.5f * (float)((i != 0) ? 1 : (-1));
					}
					playerController.WarpToPoint(vector3, true, false);
				}
			}
			base.Finish();
		}

		// Token: 0x04003743 RID: 14147
		public Vector2 lowerLeftRoomTile;

		// Token: 0x04003744 RID: 14148
		public Vector2 upperRightRoomTile;
	}
}
