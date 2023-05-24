using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AEA RID: 2794
	[Tooltip("Sets the Tinting Color for all background elements rendered by the GUI. By default only effects GUI rendered by this FSM, check Apply Globally to effect all GUI controls.")]
	[ActionCategory(ActionCategory.GUI)]
	public class SetGUIBackgroundColor : FsmStateAction
	{
		// Token: 0x06003B18 RID: 15128 RVA: 0x0012B5FC File Offset: 0x001297FC
		public override void Reset()
		{
			this.backgroundColor = Color.white;
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x0012B610 File Offset: 0x00129810
		public override void OnGUI()
		{
			GUI.backgroundColor = this.backgroundColor.Value;
			if (this.applyGlobally.Value)
			{
				PlayMakerGUI.GUIBackgroundColor = GUI.backgroundColor;
			}
		}

		// Token: 0x04002D62 RID: 11618
		[RequiredField]
		public FsmColor backgroundColor;

		// Token: 0x04002D63 RID: 11619
		public FsmBool applyGlobally;
	}
}
