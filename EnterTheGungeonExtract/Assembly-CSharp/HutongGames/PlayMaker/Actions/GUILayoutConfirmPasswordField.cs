using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009D2 RID: 2514
	[Tooltip("GUILayout Password Field. Optionally send an event if the text has been edited.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutConfirmPasswordField : GUILayoutAction
	{
		// Token: 0x06003637 RID: 13879 RVA: 0x00115BC8 File Offset: 0x00113DC8
		public override void Reset()
		{
			this.text = null;
			this.maxLength = 25;
			this.style = "TextField";
			this.mask = "*";
			this.changedEvent = null;
			this.confirm = false;
			this.password = null;
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x00115C24 File Offset: 0x00113E24
		public override void OnGUI()
		{
			bool changed = GUI.changed;
			GUI.changed = false;
			this.text.Value = GUILayout.PasswordField(this.text.Value, this.mask.Value[0], this.style.Value, base.LayoutOptions);
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

		// Token: 0x04002793 RID: 10131
		[UIHint(UIHint.Variable)]
		[Tooltip("The password Text")]
		public FsmString text;

		// Token: 0x04002794 RID: 10132
		[Tooltip("The Maximum Length of the field")]
		public FsmInt maxLength;

		// Token: 0x04002795 RID: 10133
		[Tooltip("The Style of the Field")]
		public FsmString style;

		// Token: 0x04002796 RID: 10134
		[Tooltip("Event sent when field content changed")]
		public FsmEvent changedEvent;

		// Token: 0x04002797 RID: 10135
		[Tooltip("Replacement character to hide the password")]
		public FsmString mask;

		// Token: 0x04002798 RID: 10136
		[Tooltip("GUILayout Password Field. Optionally send an event if the text has been edited.")]
		public FsmBool confirm;

		// Token: 0x04002799 RID: 10137
		[Tooltip("Confirmation content")]
		public FsmString password;
	}
}
