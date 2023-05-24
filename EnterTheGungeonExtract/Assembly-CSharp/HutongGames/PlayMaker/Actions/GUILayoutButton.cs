using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009D1 RID: 2513
	[Tooltip("GUILayout Button. Sends an Event when pressed. Optionally stores the button state in a Bool Variable.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutButton : GUILayoutAction
	{
		// Token: 0x06003634 RID: 13876 RVA: 0x00115A9C File Offset: 0x00113C9C
		public override void Reset()
		{
			base.Reset();
			this.sendEvent = null;
			this.storeButtonState = null;
			this.text = string.Empty;
			this.image = null;
			this.tooltip = string.Empty;
			this.style = string.Empty;
		}

		// Token: 0x06003635 RID: 13877 RVA: 0x00115AF4 File Offset: 0x00113CF4
		public override void OnGUI()
		{
			bool flag;
			if (string.IsNullOrEmpty(this.style.Value))
			{
				flag = GUILayout.Button(new GUIContent(this.text.Value, this.image.Value, this.tooltip.Value), base.LayoutOptions);
			}
			else
			{
				flag = GUILayout.Button(new GUIContent(this.text.Value, this.image.Value, this.tooltip.Value), this.style.Value, base.LayoutOptions);
			}
			if (flag)
			{
				base.Fsm.Event(this.sendEvent);
			}
			if (this.storeButtonState != null)
			{
				this.storeButtonState.Value = flag;
			}
		}

		// Token: 0x0400278D RID: 10125
		public FsmEvent sendEvent;

		// Token: 0x0400278E RID: 10126
		[UIHint(UIHint.Variable)]
		public FsmBool storeButtonState;

		// Token: 0x0400278F RID: 10127
		public FsmTexture image;

		// Token: 0x04002790 RID: 10128
		public FsmString text;

		// Token: 0x04002791 RID: 10129
		public FsmString tooltip;

		// Token: 0x04002792 RID: 10130
		public FsmString style;
	}
}
