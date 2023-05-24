using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009D3 RID: 2515
	[Tooltip("GUILayout Password Field. Optionally send an event if the text has been edited.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutEmailField : GUILayoutAction
	{
		// Token: 0x0600363A RID: 13882 RVA: 0x00115CB4 File Offset: 0x00113EB4
		public override void Reset()
		{
			this.text = null;
			this.maxLength = 25;
			this.style = "TextField";
			this.valid = true;
			this.changedEvent = null;
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x00115CF0 File Offset: 0x00113EF0
		public override void OnGUI()
		{
			bool changed = GUI.changed;
			GUI.changed = false;
			this.text.Value = GUILayout.TextField(this.text.Value, this.style.Value, base.LayoutOptions);
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

		// Token: 0x0400279A RID: 10138
		[UIHint(UIHint.Variable)]
		[Tooltip("The email Text")]
		public FsmString text;

		// Token: 0x0400279B RID: 10139
		[Tooltip("The Maximum Length of the field")]
		public FsmInt maxLength;

		// Token: 0x0400279C RID: 10140
		[Tooltip("The Style of the Field")]
		public FsmString style;

		// Token: 0x0400279D RID: 10141
		[Tooltip("Event sent when field content changed")]
		public FsmEvent changedEvent;

		// Token: 0x0400279E RID: 10142
		[Tooltip("Email valid format flag")]
		public FsmBool valid;
	}
}
