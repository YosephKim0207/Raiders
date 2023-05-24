using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009DA RID: 2522
	[Tooltip("GUILayout Text Field to edit a Float Variable. Optionally send an event if the text has been edited.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutFloatField : GUILayoutAction
	{
		// Token: 0x0600364E RID: 13902 RVA: 0x00115DF8 File Offset: 0x00113FF8
		public override void Reset()
		{
			base.Reset();
			this.floatVariable = null;
			this.style = string.Empty;
			this.changedEvent = null;
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x00115E20 File Offset: 0x00114020
		public override void OnGUI()
		{
			bool changed = GUI.changed;
			GUI.changed = false;
			if (!string.IsNullOrEmpty(this.style.Value))
			{
				this.floatVariable.Value = float.Parse(GUILayout.TextField(this.floatVariable.Value.ToString(), this.style.Value, base.LayoutOptions));
			}
			else
			{
				this.floatVariable.Value = float.Parse(GUILayout.TextField(this.floatVariable.Value.ToString(), base.LayoutOptions));
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

		// Token: 0x0400279F RID: 10143
		[UIHint(UIHint.Variable)]
		[Tooltip("Float Variable to show in the edit field.")]
		public FsmFloat floatVariable;

		// Token: 0x040027A0 RID: 10144
		[Tooltip("Optional GUIStyle in the active GUISKin.")]
		public FsmString style;

		// Token: 0x040027A1 RID: 10145
		[Tooltip("Optional event to send when the value changes.")]
		public FsmEvent changedEvent;
	}
}
