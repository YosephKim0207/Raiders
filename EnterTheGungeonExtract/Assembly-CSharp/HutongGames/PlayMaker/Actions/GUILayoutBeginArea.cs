using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009CA RID: 2506
	[Tooltip("Begin a GUILayout block of GUI controls in a fixed screen area. NOTE: Block must end with a corresponding GUILayoutEndArea.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutBeginArea : FsmStateAction
	{
		// Token: 0x0600361E RID: 13854 RVA: 0x00115360 File Offset: 0x00113560
		public override void Reset()
		{
			this.screenRect = null;
			this.left = 0f;
			this.top = 0f;
			this.width = 1f;
			this.height = 1f;
			this.normalized = true;
			this.style = string.Empty;
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x001153D0 File Offset: 0x001135D0
		public override void OnGUI()
		{
			this.rect = (this.screenRect.IsNone ? default(Rect) : this.screenRect.Value);
			if (!this.left.IsNone)
			{
				this.rect.x = this.left.Value;
			}
			if (!this.top.IsNone)
			{
				this.rect.y = this.top.Value;
			}
			if (!this.width.IsNone)
			{
				this.rect.width = this.width.Value;
			}
			if (!this.height.IsNone)
			{
				this.rect.height = this.height.Value;
			}
			if (this.normalized.Value)
			{
				this.rect.x = this.rect.x * (float)Screen.width;
				this.rect.width = this.rect.width * (float)Screen.width;
				this.rect.y = this.rect.y * (float)Screen.height;
				this.rect.height = this.rect.height * (float)Screen.height;
			}
			GUILayout.BeginArea(this.rect, GUIContent.none, this.style.Value);
		}

		// Token: 0x0400276B RID: 10091
		[UIHint(UIHint.Variable)]
		public FsmRect screenRect;

		// Token: 0x0400276C RID: 10092
		public FsmFloat left;

		// Token: 0x0400276D RID: 10093
		public FsmFloat top;

		// Token: 0x0400276E RID: 10094
		public FsmFloat width;

		// Token: 0x0400276F RID: 10095
		public FsmFloat height;

		// Token: 0x04002770 RID: 10096
		public FsmBool normalized;

		// Token: 0x04002771 RID: 10097
		public FsmString style;

		// Token: 0x04002772 RID: 10098
		private Rect rect;
	}
}
