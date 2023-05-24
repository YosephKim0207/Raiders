using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200098E RID: 2446
	[Tooltip("Sends an Event when a Key is pressed.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetKeyDown : FsmStateAction
	{
		// Token: 0x0600352A RID: 13610 RVA: 0x00112918 File Offset: 0x00110B18
		public override void Reset()
		{
			this.sendEvent = null;
			this.key = KeyCode.None;
			this.storeResult = null;
		}

		// Token: 0x0600352B RID: 13611 RVA: 0x00112930 File Offset: 0x00110B30
		public override void OnUpdate()
		{
			bool keyDown = Input.GetKeyDown(this.key);
			if (keyDown)
			{
				base.Fsm.Event(this.sendEvent);
			}
			this.storeResult.Value = keyDown;
		}

		// Token: 0x04002690 RID: 9872
		[RequiredField]
		public KeyCode key;

		// Token: 0x04002691 RID: 9873
		public FsmEvent sendEvent;

		// Token: 0x04002692 RID: 9874
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;
	}
}
