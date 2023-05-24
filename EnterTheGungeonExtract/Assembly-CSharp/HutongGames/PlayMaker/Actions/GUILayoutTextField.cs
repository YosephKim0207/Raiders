using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009E3 RID: 2531
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("GUILayout Text Field. Optionally send an event if the text has been edited.")]
	public class GUILayoutTextField : GUILayoutAction
	{
		// Token: 0x06003669 RID: 13929 RVA: 0x00116574 File Offset: 0x00114774
		public override void Reset()
		{
			base.Reset();
			this.text = null;
			this.maxLength = 25;
			this.style = "TextField";
			this.changedEvent = null;
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x001165A8 File Offset: 0x001147A8
		public override void OnGUI()
		{
			bool changed = GUI.changed;
			GUI.changed = false;
			this.text.Value = GUILayout.TextField(this.text.Value, this.maxLength.Value, this.style.Value, base.LayoutOptions);
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

		// Token: 0x040027BF RID: 10175
		[UIHint(UIHint.Variable)]
		public FsmString text;

		// Token: 0x040027C0 RID: 10176
		public FsmInt maxLength;

		// Token: 0x040027C1 RID: 10177
		public FsmString style;

		// Token: 0x040027C2 RID: 10178
		public FsmEvent changedEvent;
	}
}
