using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009D0 RID: 2512
	[Tooltip("GUILayout Box.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutBox : GUILayoutAction
	{
		// Token: 0x06003631 RID: 13873 RVA: 0x001159B8 File Offset: 0x00113BB8
		public override void Reset()
		{
			base.Reset();
			this.text = string.Empty;
			this.image = null;
			this.tooltip = string.Empty;
			this.style = string.Empty;
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x001159F8 File Offset: 0x00113BF8
		public override void OnGUI()
		{
			if (string.IsNullOrEmpty(this.style.Value))
			{
				GUILayout.Box(new GUIContent(this.text.Value, this.image.Value, this.tooltip.Value), base.LayoutOptions);
			}
			else
			{
				GUILayout.Box(new GUIContent(this.text.Value, this.image.Value, this.tooltip.Value), this.style.Value, base.LayoutOptions);
			}
		}

		// Token: 0x04002789 RID: 10121
		[Tooltip("Image to display in the Box.")]
		public FsmTexture image;

		// Token: 0x0400278A RID: 10122
		[Tooltip("Text to display in the Box.")]
		public FsmString text;

		// Token: 0x0400278B RID: 10123
		[Tooltip("Optional Tooltip string.")]
		public FsmString tooltip;

		// Token: 0x0400278C RID: 10124
		[Tooltip("Optional GUIStyle in the active GUISkin.")]
		public FsmString style;
	}
}
