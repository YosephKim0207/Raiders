using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009CD RID: 2509
	[Tooltip("GUILayout BeginHorizontal.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutBeginHorizontal : GUILayoutAction
	{
		// Token: 0x06003628 RID: 13864 RVA: 0x00115784 File Offset: 0x00113984
		public override void Reset()
		{
			base.Reset();
			this.text = string.Empty;
			this.image = null;
			this.tooltip = string.Empty;
			this.style = string.Empty;
		}

		// Token: 0x06003629 RID: 13865 RVA: 0x001157C4 File Offset: 0x001139C4
		public override void OnGUI()
		{
			GUILayout.BeginHorizontal(new GUIContent(this.text.Value, this.image.Value, this.tooltip.Value), this.style.Value, base.LayoutOptions);
		}

		// Token: 0x0400277A RID: 10106
		public FsmTexture image;

		// Token: 0x0400277B RID: 10107
		public FsmString text;

		// Token: 0x0400277C RID: 10108
		public FsmString tooltip;

		// Token: 0x0400277D RID: 10109
		public FsmString style;
	}
}
