using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B31 RID: 2865
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Sends events based on Touch Phases. Optionally filter by a fingerID.")]
	public class TouchEvent : FsmStateAction
	{
		// Token: 0x06003C43 RID: 15427 RVA: 0x0012F498 File Offset: 0x0012D698
		public override void Reset()
		{
			this.fingerId = new FsmInt
			{
				UseVariable = true
			};
			this.storeFingerId = null;
		}

		// Token: 0x06003C44 RID: 15428 RVA: 0x0012F4C0 File Offset: 0x0012D6C0
		public override void OnUpdate()
		{
			if (Input.touchCount > 0)
			{
				foreach (Touch touch in Input.touches)
				{
					if ((this.fingerId.IsNone || touch.fingerId == this.fingerId.Value) && touch.phase == this.touchPhase)
					{
						this.storeFingerId.Value = touch.fingerId;
						base.Fsm.Event(this.sendEvent);
					}
				}
			}
		}

		// Token: 0x04002E7D RID: 11901
		public FsmInt fingerId;

		// Token: 0x04002E7E RID: 11902
		public TouchPhase touchPhase;

		// Token: 0x04002E7F RID: 11903
		public FsmEvent sendEvent;

		// Token: 0x04002E80 RID: 11904
		[UIHint(UIHint.Variable)]
		public FsmInt storeFingerId;
	}
}
