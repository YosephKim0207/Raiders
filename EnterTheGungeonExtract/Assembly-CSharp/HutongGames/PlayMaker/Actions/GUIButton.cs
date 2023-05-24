using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009C4 RID: 2500
	[Tooltip("GUI button. Sends an Event when pressed. Optionally store the button state in a Bool Variable.")]
	[ActionCategory(ActionCategory.GUI)]
	public class GUIButton : GUIContentAction
	{
		// Token: 0x0600360B RID: 13835 RVA: 0x00114E38 File Offset: 0x00113038
		public override void Reset()
		{
			base.Reset();
			this.sendEvent = null;
			this.storeButtonState = null;
			this.style = "Button";
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x00114E60 File Offset: 0x00113060
		public override void OnGUI()
		{
			base.OnGUI();
			bool flag = false;
			if (GUI.Button(this.rect, this.content, this.style.Value))
			{
				base.Fsm.Event(this.sendEvent);
				flag = true;
			}
			if (this.storeButtonState != null)
			{
				this.storeButtonState.Value = flag;
			}
		}

		// Token: 0x04002752 RID: 10066
		public FsmEvent sendEvent;

		// Token: 0x04002753 RID: 10067
		[UIHint(UIHint.Variable)]
		public FsmBool storeButtonState;
	}
}
