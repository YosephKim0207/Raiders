using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009E4 RID: 2532
	[Tooltip("GUILayout Label for simple text.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutTextLabel : GUILayoutAction
	{
		// Token: 0x0600366C RID: 13932 RVA: 0x00116630 File Offset: 0x00114830
		public override void Reset()
		{
			base.Reset();
			this.text = string.Empty;
			this.style = string.Empty;
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x00116658 File Offset: 0x00114858
		public override void OnGUI()
		{
			if (string.IsNullOrEmpty(this.style.Value))
			{
				GUILayout.Label(new GUIContent(this.text.Value), base.LayoutOptions);
			}
			else
			{
				GUILayout.Label(new GUIContent(this.text.Value), this.style.Value, base.LayoutOptions);
			}
		}

		// Token: 0x040027C3 RID: 10179
		[Tooltip("Text to display.")]
		public FsmString text;

		// Token: 0x040027C4 RID: 10180
		[Tooltip("Optional GUIStyle in the active GUISkin.")]
		public FsmString style;
	}
}
