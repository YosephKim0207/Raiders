using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009C5 RID: 2501
	[Tooltip("GUI base action - don't use!")]
	public abstract class GUIContentAction : GUIAction
	{
		// Token: 0x0600360E RID: 13838 RVA: 0x00114ED0 File Offset: 0x001130D0
		public override void Reset()
		{
			base.Reset();
			this.image = null;
			this.text = string.Empty;
			this.tooltip = string.Empty;
			this.style = string.Empty;
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x00114F10 File Offset: 0x00113110
		public override void OnGUI()
		{
			base.OnGUI();
			this.content = new GUIContent(this.text.Value, this.image.Value, this.tooltip.Value);
		}

		// Token: 0x04002754 RID: 10068
		public FsmTexture image;

		// Token: 0x04002755 RID: 10069
		public FsmString text;

		// Token: 0x04002756 RID: 10070
		public FsmString tooltip;

		// Token: 0x04002757 RID: 10071
		public FsmString style;

		// Token: 0x04002758 RID: 10072
		internal GUIContent content;
	}
}
