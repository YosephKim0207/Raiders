using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C5C RID: 3164
	[Tooltip("Checks if the owning game object has been encountered before.")]
	[ActionCategory(".Brave")]
	public class HasBeenEncountered : FsmStateAction
	{
		// Token: 0x06004427 RID: 17447 RVA: 0x001602CC File Offset: 0x0015E4CC
		public override void Reset()
		{
			this.GameObject = null;
			this.yes = null;
			this.no = null;
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x001602E4 File Offset: 0x0015E4E4
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.GameObject);
			this.m_encounterTrackable = ownerDefaultTarget.GetComponent<EncounterTrackable>();
			if (this.m_encounterTrackable != null)
			{
				if (GameStatsManager.Instance.QueryEncounterable(this.m_encounterTrackable) > 0)
				{
					if (this.yes != null)
					{
						base.Fsm.Event(this.yes);
					}
				}
				else if (this.no != null)
				{
					base.Fsm.Event(this.no);
				}
			}
			base.Finish();
		}

		// Token: 0x0400363A RID: 13882
		[CheckForComponent(typeof(EncounterTrackable))]
		public FsmOwnerDefault GameObject;

		// Token: 0x0400363B RID: 13883
		[Tooltip("Event to send when the mouse is released while over the GameObject.")]
		public FsmEvent yes;

		// Token: 0x0400363C RID: 13884
		[Tooltip("Event to send when the mouse moves off the GameObject.")]
		public FsmEvent no;

		// Token: 0x0400363D RID: 13885
		private EncounterTrackable m_encounterTrackable;
	}
}
