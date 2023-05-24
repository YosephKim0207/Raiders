using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009E5 RID: 2533
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("Makes an on/off Toggle Button and stores the button state in a Bool Variable.")]
	public class GUILayoutToggle : GUILayoutAction
	{
		// Token: 0x0600366F RID: 13935 RVA: 0x001166D0 File Offset: 0x001148D0
		public override void Reset()
		{
			base.Reset();
			this.storeButtonState = null;
			this.text = string.Empty;
			this.image = null;
			this.tooltip = string.Empty;
			this.style = "Toggle";
			this.changedEvent = null;
		}

		// Token: 0x06003670 RID: 13936 RVA: 0x00116728 File Offset: 0x00114928
		public override void OnGUI()
		{
			bool changed = GUI.changed;
			GUI.changed = false;
			this.storeButtonState.Value = GUILayout.Toggle(this.storeButtonState.Value, new GUIContent(this.text.Value, this.image.Value, this.tooltip.Value), this.style.Value, base.LayoutOptions);
			if (GUI.changed)
			{
				base.Fsm.Event(this.changedEvent);
				GUIUtility.ExitGUI();
			}
			else
			{
				GUI.changed = changed;
			}
		}

		// Token: 0x040027C5 RID: 10181
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmBool storeButtonState;

		// Token: 0x040027C6 RID: 10182
		public FsmTexture image;

		// Token: 0x040027C7 RID: 10183
		public FsmString text;

		// Token: 0x040027C8 RID: 10184
		public FsmString tooltip;

		// Token: 0x040027C9 RID: 10185
		public FsmString style;

		// Token: 0x040027CA RID: 10186
		public FsmEvent changedEvent;
	}
}
