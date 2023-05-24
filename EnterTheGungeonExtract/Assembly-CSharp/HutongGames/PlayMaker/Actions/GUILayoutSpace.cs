using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009E2 RID: 2530
	[Tooltip("Inserts a space in the current layout group.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutSpace : FsmStateAction
	{
		// Token: 0x06003666 RID: 13926 RVA: 0x00116544 File Offset: 0x00114744
		public override void Reset()
		{
			this.space = 10f;
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x00116558 File Offset: 0x00114758
		public override void OnGUI()
		{
			GUILayout.Space(this.space.Value);
		}

		// Token: 0x040027BE RID: 10174
		public FsmFloat space;
	}
}
