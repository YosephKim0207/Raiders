using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009E0 RID: 2528
	[Tooltip("GUILayout Password Field. Optionally send an event if the text has been edited.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutPasswordField : GUILayoutAction
	{
		// Token: 0x06003660 RID: 13920 RVA: 0x00116354 File Offset: 0x00114554
		public override void Reset()
		{
			this.text = null;
			this.maxLength = 25;
			this.style = "TextField";
			this.mask = "*";
			this.changedEvent = null;
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x00116394 File Offset: 0x00114594
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

		// Token: 0x040027B3 RID: 10163
		[Tooltip("The password Text")]
		[UIHint(UIHint.Variable)]
		public FsmString text;

		// Token: 0x040027B4 RID: 10164
		[Tooltip("The Maximum Length of the field")]
		public FsmInt maxLength;

		// Token: 0x040027B5 RID: 10165
		[Tooltip("The Style of the Field")]
		public FsmString style;

		// Token: 0x040027B6 RID: 10166
		[Tooltip("Event sent when field content changed")]
		public FsmEvent changedEvent;

		// Token: 0x040027B7 RID: 10167
		[Tooltip("Replacement character to hide the password")]
		public FsmString mask;
	}
}
