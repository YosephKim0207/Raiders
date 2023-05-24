using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008D6 RID: 2262
	[Tooltip("Sends an Event when the user hits any Key or Mouse Button.")]
	[ActionCategory(ActionCategory.Input)]
	public class AnyKey : FsmStateAction
	{
		// Token: 0x06003223 RID: 12835 RVA: 0x00108438 File Offset: 0x00106638
		public override void Reset()
		{
			this.sendEvent = null;
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x00108444 File Offset: 0x00106644
		public override void OnUpdate()
		{
			if (Input.anyKeyDown)
			{
				base.Fsm.Event(this.sendEvent);
			}
		}

		// Token: 0x04002347 RID: 9031
		[Tooltip("Event to send when any Key or Mouse Button is pressed.")]
		[RequiredField]
		public FsmEvent sendEvent;
	}
}
