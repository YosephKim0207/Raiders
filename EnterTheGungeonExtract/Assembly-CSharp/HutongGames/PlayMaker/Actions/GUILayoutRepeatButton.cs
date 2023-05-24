using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009E1 RID: 2529
	[Tooltip("GUILayout Repeat Button. Sends an Event while pressed. Optionally store the button state in a Bool Variable.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutRepeatButton : GUILayoutAction
	{
		// Token: 0x06003663 RID: 13923 RVA: 0x00116424 File Offset: 0x00114624
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

		// Token: 0x06003664 RID: 13924 RVA: 0x0011647C File Offset: 0x0011467C
		public override void OnGUI()
		{
			bool flag;
			if (string.IsNullOrEmpty(this.style.Value))
			{
				flag = GUILayout.RepeatButton(new GUIContent(this.text.Value, this.image.Value, this.tooltip.Value), base.LayoutOptions);
			}
			else
			{
				flag = GUILayout.RepeatButton(new GUIContent(this.text.Value, this.image.Value, this.tooltip.Value), this.style.Value, base.LayoutOptions);
			}
			if (flag)
			{
				base.Fsm.Event(this.sendEvent);
			}
			this.storeButtonState.Value = flag;
		}

		// Token: 0x040027B8 RID: 10168
		public FsmEvent sendEvent;

		// Token: 0x040027B9 RID: 10169
		[UIHint(UIHint.Variable)]
		public FsmBool storeButtonState;

		// Token: 0x040027BA RID: 10170
		public FsmTexture image;

		// Token: 0x040027BB RID: 10171
		public FsmString text;

		// Token: 0x040027BC RID: 10172
		public FsmString tooltip;

		// Token: 0x040027BD RID: 10173
		public FsmString style;
	}
}
