using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AA8 RID: 2728
	[Tooltip("Resets the GUI matrix. Useful if you've rotated or scaled the GUI and now want to reset it.")]
	[ActionCategory(ActionCategory.GUI)]
	public class ResetGUIMatrix : FsmStateAction
	{
		// Token: 0x060039E1 RID: 14817 RVA: 0x0012742C File Offset: 0x0012562C
		public override void OnGUI()
		{
			Matrix4x4 identity = Matrix4x4.identity;
			GUI.matrix = identity;
			PlayMakerGUI.GUIMatrix = identity;
		}
	}
}
