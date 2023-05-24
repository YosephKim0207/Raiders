using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009DB RID: 2523
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("GUILayout Label for a Float Variable.")]
	public class GUILayoutFloatLabel : GUILayoutAction
	{
		// Token: 0x06003651 RID: 13905 RVA: 0x00115F00 File Offset: 0x00114100
		public override void Reset()
		{
			base.Reset();
			this.prefix = string.Empty;
			this.style = string.Empty;
			this.floatVariable = null;
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x00115F30 File Offset: 0x00114130
		public override void OnGUI()
		{
			if (string.IsNullOrEmpty(this.style.Value))
			{
				GUILayout.Label(new GUIContent(this.prefix.Value + this.floatVariable.Value), base.LayoutOptions);
			}
			else
			{
				GUILayout.Label(new GUIContent(this.prefix.Value + this.floatVariable.Value), this.style.Value, base.LayoutOptions);
			}
		}

		// Token: 0x040027A2 RID: 10146
		[Tooltip("Text to put before the float variable.")]
		public FsmString prefix;

		// Token: 0x040027A3 RID: 10147
		[Tooltip("Float variable to display.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		// Token: 0x040027A4 RID: 10148
		[Tooltip("Optional GUIStyle in the active GUISKin.")]
		public FsmString style;
	}
}
