using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CDD RID: 3293
	[Tooltip("Paths to near the player's current location.")]
	[ActionCategory(".NPCs")]
	public class WalkToPlayer : FsmStateAction
	{
		// Token: 0x060045E8 RID: 17896 RVA: 0x0016B2B0 File Offset: 0x001694B0
		public override string ErrorCheck()
		{
			return string.Empty;
		}

		// Token: 0x060045E9 RID: 17897 RVA: 0x0016B2C4 File Offset: 0x001694C4
		public override void OnEnter()
		{
			this.m_owner = base.Owner.GetComponent<TalkDoerLite>();
			this.m_lastPosition = this.m_owner.specRigidbody.UnitCenter;
			Vector2 vector = this.m_lastPosition;
			WalkToPlayer.TargetPathType targetPathType = this.pathDestinationType;
			if (targetPathType != WalkToPlayer.TargetPathType.PLAYER)
			{
				if (targetPathType == WalkToPlayer.TargetPathType.PLAYER_ROOM_CENTER)
				{
					vector = GameManager.Instance.BestActivePlayer.CurrentRoom.GetCenterCell().ToCenterVector3(0f);
				}
			}
			else
			{
				vector = GameManager.Instance.BestActivePlayer.CenterPosition;
			}
			this.m_owner.PathfindToPosition(vector, null, null);
		}

		// Token: 0x060045EA RID: 17898 RVA: 0x0016B374 File Offset: 0x00169574
		public override void OnUpdate()
		{
			base.OnUpdate();
			this.m_owner.specRigidbody.Velocity = this.m_owner.GetPathVelocityContribution(this.m_lastPosition, 32);
			if (this.m_owner.CurrentPath == null)
			{
				base.Finish();
			}
			this.m_lastPosition = this.m_owner.specRigidbody.UnitCenter;
		}

		// Token: 0x04003828 RID: 14376
		public WalkToPlayer.TargetPathType pathDestinationType;

		// Token: 0x04003829 RID: 14377
		private TalkDoerLite m_owner;

		// Token: 0x0400382A RID: 14378
		private Vector2 m_lastPosition;

		// Token: 0x02000CDE RID: 3294
		public enum TargetPathType
		{
			// Token: 0x0400382C RID: 14380
			PLAYER,
			// Token: 0x0400382D RID: 14381
			PLAYER_ROOM_CENTER
		}
	}
}
