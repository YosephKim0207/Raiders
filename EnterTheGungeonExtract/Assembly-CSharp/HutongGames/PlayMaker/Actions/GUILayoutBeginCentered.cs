using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009CC RID: 2508
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("Begin a centered GUILayout block. The block is centered inside a parent GUILayout Area. So to place the block in the center of the screen, first use a GULayout Area the size of the whole screen (the default setting). NOTE: Block must end with a corresponding GUILayoutEndCentered.")]
	public class GUILayoutBeginCentered : FsmStateAction
	{
		// Token: 0x06003625 RID: 13861 RVA: 0x00115748 File Offset: 0x00113948
		public override void Reset()
		{
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x0011574C File Offset: 0x0011394C
		public override void OnGUI()
		{
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical(new GUILayoutOption[0]);
		}
	}
}
