using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C95 RID: 3221
	[Tooltip("Responds to chest events.")]
	[ActionCategory(".NPCs")]
	public class ChestEvent : FsmStateAction
	{
		// Token: 0x060044F6 RID: 17654 RVA: 0x001647A0 File Offset: 0x001629A0
		public override void Reset()
		{
			this.unlocked = null;
			this.locked = null;
			this.unsealed = null;
			this.Sealed = null;
			this.opened = null;
			this.destroyed = null;
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x001647CC File Offset: 0x001629CC
		public override void OnEnter()
		{
			RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(base.Owner.transform.position.IntXY(VectorConversions.Floor));
			List<Chest> componentsInRoom = roomFromPosition.GetComponentsInRoom<Chest>();
			if (componentsInRoom != null && componentsInRoom.Count > 0)
			{
				this.m_chest = componentsInRoom[0];
				if (componentsInRoom.Count > 1)
				{
					Debug.LogError("Too many chests!");
				}
				this.m_wasLocked = this.m_chest.IsLocked;
				this.m_wasSealed = this.m_chest.IsSealed;
				this.m_wasOpen = this.m_chest.IsOpen;
				this.m_wasDestroyed = this.m_chest.IsBroken;
			}
			else
			{
				Debug.LogError("No chests found!");
				base.Finish();
			}
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x00164894 File Offset: 0x00162A94
		public override void OnUpdate()
		{
			if (!this.m_chest)
			{
				base.Finish();
			}
			else
			{
				if (((this.unlocked != null) & this.m_wasLocked) && !this.m_chest.IsLocked)
				{
					base.Fsm.Event(this.unlocked);
				}
				if (((this.locked != null) & !this.m_wasLocked) && this.m_chest.IsLocked)
				{
					base.Fsm.Event(this.locked);
				}
				if (((this.unsealed != null) & this.m_wasSealed) && !this.m_chest.IsSealed)
				{
					base.Fsm.Event(this.unsealed);
				}
				if (((this.Sealed != null) & !this.m_wasSealed) && this.m_chest.IsSealed)
				{
					base.Fsm.Event(this.Sealed);
				}
				if (((this.opened != null) & !this.m_wasOpen) && this.m_chest.IsOpen)
				{
					base.Fsm.Event(this.opened);
				}
				if (((this.destroyed != null) & !this.m_wasDestroyed) && this.m_chest.IsBroken)
				{
					base.Fsm.Event(this.destroyed);
				}
				this.m_wasLocked = this.m_chest.IsLocked;
				this.m_wasSealed = this.m_chest.IsSealed;
				this.m_wasOpen = this.m_chest.IsOpen;
				this.m_wasDestroyed = this.m_chest.IsBroken;
			}
		}

		// Token: 0x04003703 RID: 14083
		[Tooltip("Event to play when the chest has been unlocked.")]
		public FsmEvent unlocked;

		// Token: 0x04003704 RID: 14084
		[Tooltip("Event to play when the chest has been locked.")]
		public FsmEvent locked;

		// Token: 0x04003705 RID: 14085
		[Tooltip("Event to play when the chest has been unsealed.")]
		public FsmEvent unsealed;

		// Token: 0x04003706 RID: 14086
		[Tooltip("Event to play when the chest has been sealed.")]
		public FsmEvent Sealed;

		// Token: 0x04003707 RID: 14087
		[Tooltip("Event to play when the chest has been opened.")]
		public FsmEvent opened;

		// Token: 0x04003708 RID: 14088
		[Tooltip("Event to play when the chest has been destroyed.")]
		public FsmEvent destroyed;

		// Token: 0x04003709 RID: 14089
		private Chest m_chest;

		// Token: 0x0400370A RID: 14090
		private bool m_wasLocked;

		// Token: 0x0400370B RID: 14091
		private bool m_wasSealed;

		// Token: 0x0400370C RID: 14092
		private bool m_wasOpen;

		// Token: 0x0400370D RID: 14093
		private bool m_wasDestroyed;
	}
}
