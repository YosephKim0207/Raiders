using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AEB RID: 2795
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("Sets the Tinting Color for the GUI. By default only effects GUI rendered by this FSM, check Apply Globally to effect all GUI controls.")]
	public class SetGUIColor : FsmStateAction
	{
		// Token: 0x06003B1B RID: 15131 RVA: 0x0012B644 File Offset: 0x00129844
		public override void Reset()
		{
			this.color = Color.white;
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x0012B658 File Offset: 0x00129858
		public override void OnGUI()
		{
			GUI.color = this.color.Value;
			if (this.applyGlobally.Value)
			{
				PlayMakerGUI.GUIColor = GUI.color;
			}
		}

		// Token: 0x04002D64 RID: 11620
		[RequiredField]
		public FsmColor color;

		// Token: 0x04002D65 RID: 11621
		public FsmBool applyGlobally;
	}
}
