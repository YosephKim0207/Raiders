using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009CE RID: 2510
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("Begins a ScrollView. Use GUILayoutEndScrollView at the end of the block.")]
	public class GUILayoutBeginScrollView : GUILayoutAction
	{
		// Token: 0x0600362B RID: 13867 RVA: 0x0011581C File Offset: 0x00113A1C
		public override void Reset()
		{
			base.Reset();
			this.scrollPosition = null;
			this.horizontalScrollbar = null;
			this.verticalScrollbar = null;
			this.useCustomStyle = null;
			this.horizontalStyle = null;
			this.verticalStyle = null;
			this.backgroundStyle = null;
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x00115858 File Offset: 0x00113A58
		public override void OnGUI()
		{
			if (this.useCustomStyle.Value)
			{
				this.scrollPosition.Value = GUILayout.BeginScrollView(this.scrollPosition.Value, this.horizontalScrollbar.Value, this.verticalScrollbar.Value, this.horizontalStyle.Value, this.verticalStyle.Value, this.backgroundStyle.Value, base.LayoutOptions);
			}
			else
			{
				this.scrollPosition.Value = GUILayout.BeginScrollView(this.scrollPosition.Value, this.horizontalScrollbar.Value, this.verticalScrollbar.Value, base.LayoutOptions);
			}
		}

		// Token: 0x0400277E RID: 10110
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("Assign a Vector2 variable to store the scroll position of this view.")]
		public FsmVector2 scrollPosition;

		// Token: 0x0400277F RID: 10111
		[Tooltip("Always show the horizontal scrollbars.")]
		public FsmBool horizontalScrollbar;

		// Token: 0x04002780 RID: 10112
		[Tooltip("Always show the vertical scrollbars.")]
		public FsmBool verticalScrollbar;

		// Token: 0x04002781 RID: 10113
		[Tooltip("Define custom styles below. NOTE: You have to define all the styles if you check this option.")]
		public FsmBool useCustomStyle;

		// Token: 0x04002782 RID: 10114
		[Tooltip("Named style in the active GUISkin for the horizontal scrollbars.")]
		public FsmString horizontalStyle;

		// Token: 0x04002783 RID: 10115
		[Tooltip("Named style in the active GUISkin for the vertical scrollbars.")]
		public FsmString verticalStyle;

		// Token: 0x04002784 RID: 10116
		[Tooltip("Named style in the active GUISkin for the background.")]
		public FsmString backgroundStyle;
	}
}
