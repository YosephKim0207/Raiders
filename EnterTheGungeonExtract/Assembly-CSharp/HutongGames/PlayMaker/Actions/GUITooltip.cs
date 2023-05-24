using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009E8 RID: 2536
	[Tooltip("Gets the Tooltip of the control the mouse is currently over and store it in a String Variable.")]
	[ActionCategory(ActionCategory.GUI)]
	public class GUITooltip : FsmStateAction
	{
		// Token: 0x0600367C RID: 13948 RVA: 0x00116B94 File Offset: 0x00114D94
		public override void Reset()
		{
			this.storeTooltip = null;
		}

		// Token: 0x0600367D RID: 13949 RVA: 0x00116BA0 File Offset: 0x00114DA0
		public override void OnGUI()
		{
			this.storeTooltip.Value = GUI.tooltip;
		}

		// Token: 0x040027D8 RID: 10200
		[UIHint(UIHint.Variable)]
		public FsmString storeTooltip;
	}
}
