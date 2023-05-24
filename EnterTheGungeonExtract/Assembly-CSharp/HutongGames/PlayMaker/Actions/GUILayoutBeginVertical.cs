using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009CF RID: 2511
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("Begins a vertical control group. The group must be closed with GUILayoutEndVertical action.")]
	public class GUILayoutBeginVertical : GUILayoutAction
	{
		// Token: 0x0600362E RID: 13870 RVA: 0x00115920 File Offset: 0x00113B20
		public override void Reset()
		{
			base.Reset();
			this.text = string.Empty;
			this.image = null;
			this.tooltip = string.Empty;
			this.style = string.Empty;
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x00115960 File Offset: 0x00113B60
		public override void OnGUI()
		{
			GUILayout.BeginVertical(new GUIContent(this.text.Value, this.image.Value, this.tooltip.Value), this.style.Value, base.LayoutOptions);
		}

		// Token: 0x04002785 RID: 10117
		public FsmTexture image;

		// Token: 0x04002786 RID: 10118
		public FsmString text;

		// Token: 0x04002787 RID: 10119
		public FsmString tooltip;

		// Token: 0x04002788 RID: 10120
		public FsmString style;
	}
}
