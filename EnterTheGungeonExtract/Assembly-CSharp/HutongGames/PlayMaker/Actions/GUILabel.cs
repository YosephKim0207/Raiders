using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009C8 RID: 2504
	[Tooltip("GUI Label.")]
	[ActionCategory(ActionCategory.GUI)]
	public class GUILabel : GUIContentAction
	{
		// Token: 0x06003619 RID: 13849 RVA: 0x00115280 File Offset: 0x00113480
		public override void OnGUI()
		{
			base.OnGUI();
			if (string.IsNullOrEmpty(this.style.Value))
			{
				GUI.Label(this.rect, this.content);
			}
			else
			{
				GUI.Label(this.rect, this.content, this.style.Value);
			}
		}
	}
}
