using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009DD RID: 2525
	[Tooltip("GUILayout Text Field to edit an Int Variable. Optionally send an event if the text has been edited.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutIntField : GUILayoutAction
	{
		// Token: 0x06003657 RID: 13911 RVA: 0x00116098 File Offset: 0x00114298
		public override void Reset()
		{
			base.Reset();
			this.intVariable = null;
			this.style = string.Empty;
			this.changedEvent = null;
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x001160C0 File Offset: 0x001142C0
		public override void OnGUI()
		{
			bool changed = GUI.changed;
			GUI.changed = false;
			if (!string.IsNullOrEmpty(this.style.Value))
			{
				this.intVariable.Value = int.Parse(GUILayout.TextField(this.intVariable.Value.ToString(), this.style.Value, base.LayoutOptions));
			}
			else
			{
				this.intVariable.Value = int.Parse(GUILayout.TextField(this.intVariable.Value.ToString(), base.LayoutOptions));
			}
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

		// Token: 0x040027A9 RID: 10153
		[Tooltip("Int Variable to show in the edit field.")]
		[UIHint(UIHint.Variable)]
		public FsmInt intVariable;

		// Token: 0x040027AA RID: 10154
		[Tooltip("Optional GUIStyle in the active GUISKin.")]
		public FsmString style;

		// Token: 0x040027AB RID: 10155
		[Tooltip("Optional event to send when the value changes.")]
		public FsmEvent changedEvent;
	}
}
