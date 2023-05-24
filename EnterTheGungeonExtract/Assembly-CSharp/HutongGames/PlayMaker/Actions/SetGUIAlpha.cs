using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AE9 RID: 2793
	[Tooltip("Sets the global Alpha for the GUI. Useful for fading GUI up/down. By default only effects GUI rendered by this FSM, check Apply Globally to effect all GUI controls.")]
	[ActionCategory(ActionCategory.GUI)]
	public class SetGUIAlpha : FsmStateAction
	{
		// Token: 0x06003B15 RID: 15125 RVA: 0x0012B57C File Offset: 0x0012977C
		public override void Reset()
		{
			this.alpha = 1f;
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x0012B590 File Offset: 0x00129790
		public override void OnGUI()
		{
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this.alpha.Value);
			if (this.applyGlobally.Value)
			{
				PlayMakerGUI.GUIColor = GUI.color;
			}
		}

		// Token: 0x04002D60 RID: 11616
		[RequiredField]
		public FsmFloat alpha;

		// Token: 0x04002D61 RID: 11617
		public FsmBool applyGlobally;
	}
}
