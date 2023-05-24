using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200098F RID: 2447
	[Tooltip("Sends an Event when a Key is released.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetKeyUp : FsmStateAction
	{
		// Token: 0x0600352D RID: 13613 RVA: 0x00112974 File Offset: 0x00110B74
		public override void Reset()
		{
			this.sendEvent = null;
			this.key = KeyCode.None;
			this.storeResult = null;
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x0011298C File Offset: 0x00110B8C
		public override void OnUpdate()
		{
			bool keyUp = Input.GetKeyUp(this.key);
			if (keyUp)
			{
				base.Fsm.Event(this.sendEvent);
			}
			this.storeResult.Value = keyUp;
		}

		// Token: 0x04002693 RID: 9875
		[RequiredField]
		public KeyCode key;

		// Token: 0x04002694 RID: 9876
		public FsmEvent sendEvent;

		// Token: 0x04002695 RID: 9877
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;
	}
}
