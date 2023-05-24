using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009C3 RID: 2499
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("GUI Box.")]
	public class GUIBox : GUIContentAction
	{
		// Token: 0x06003609 RID: 13833 RVA: 0x00114DD0 File Offset: 0x00112FD0
		public override void OnGUI()
		{
			base.OnGUI();
			if (string.IsNullOrEmpty(this.style.Value))
			{
				GUI.Box(this.rect, this.content);
			}
			else
			{
				GUI.Box(this.rect, this.content, this.style.Value);
			}
		}
	}
}
