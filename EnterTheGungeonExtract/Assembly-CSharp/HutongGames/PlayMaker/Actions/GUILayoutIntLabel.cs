using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009DE RID: 2526
	[Tooltip("GUILayout Label for an Int Variable.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutIntLabel : GUILayoutAction
	{
		// Token: 0x0600365A RID: 13914 RVA: 0x001161A0 File Offset: 0x001143A0
		public override void Reset()
		{
			base.Reset();
			this.prefix = string.Empty;
			this.style = string.Empty;
			this.intVariable = null;
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x001161D0 File Offset: 0x001143D0
		public override void OnGUI()
		{
			if (string.IsNullOrEmpty(this.style.Value))
			{
				GUILayout.Label(new GUIContent(this.prefix.Value + this.intVariable.Value), base.LayoutOptions);
			}
			else
			{
				GUILayout.Label(new GUIContent(this.prefix.Value + this.intVariable.Value), this.style.Value, base.LayoutOptions);
			}
		}

		// Token: 0x040027AC RID: 10156
		[Tooltip("Text to put before the int variable.")]
		public FsmString prefix;

		// Token: 0x040027AD RID: 10157
		[Tooltip("Int variable to display.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt intVariable;

		// Token: 0x040027AE RID: 10158
		[Tooltip("Optional GUIStyle in the active GUISKin.")]
		public FsmString style;
	}
}
