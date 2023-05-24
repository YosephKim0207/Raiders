using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009DF RID: 2527
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("GUILayout Label.")]
	public class GUILayoutLabel : GUILayoutAction
	{
		// Token: 0x0600365D RID: 13917 RVA: 0x00116270 File Offset: 0x00114470
		public override void Reset()
		{
			base.Reset();
			this.text = string.Empty;
			this.image = null;
			this.tooltip = string.Empty;
			this.style = string.Empty;
		}

		// Token: 0x0600365E RID: 13918 RVA: 0x001162B0 File Offset: 0x001144B0
		public override void OnGUI()
		{
			if (string.IsNullOrEmpty(this.style.Value))
			{
				GUILayout.Label(new GUIContent(this.text.Value, this.image.Value, this.tooltip.Value), base.LayoutOptions);
			}
			else
			{
				GUILayout.Label(new GUIContent(this.text.Value, this.image.Value, this.tooltip.Value), this.style.Value, base.LayoutOptions);
			}
		}

		// Token: 0x040027AF RID: 10159
		public FsmTexture image;

		// Token: 0x040027B0 RID: 10160
		public FsmString text;

		// Token: 0x040027B1 RID: 10161
		public FsmString tooltip;

		// Token: 0x040027B2 RID: 10162
		public FsmString style;
	}
}
