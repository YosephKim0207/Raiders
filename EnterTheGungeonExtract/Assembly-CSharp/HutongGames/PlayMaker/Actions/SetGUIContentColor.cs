using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AEC RID: 2796
	[Tooltip("Sets the Tinting Color for all text rendered by the GUI. By default only effects GUI rendered by this FSM, check Apply Globally to effect all GUI controls.")]
	[ActionCategory(ActionCategory.GUI)]
	public class SetGUIContentColor : FsmStateAction
	{
		// Token: 0x06003B1E RID: 15134 RVA: 0x0012B68C File Offset: 0x0012988C
		public override void Reset()
		{
			this.contentColor = Color.white;
		}

		// Token: 0x06003B1F RID: 15135 RVA: 0x0012B6A0 File Offset: 0x001298A0
		public override void OnGUI()
		{
			GUI.contentColor = this.contentColor.Value;
			if (this.applyGlobally.Value)
			{
				PlayMakerGUI.GUIContentColor = GUI.contentColor;
			}
		}

		// Token: 0x04002D66 RID: 11622
		[RequiredField]
		public FsmColor contentColor;

		// Token: 0x04002D67 RID: 11623
		public FsmBool applyGlobally;
	}
}
