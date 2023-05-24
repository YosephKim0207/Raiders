using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009D4 RID: 2516
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("Close a GUILayout group started with BeginArea.")]
	public class GUILayoutEndArea : FsmStateAction
	{
		// Token: 0x0600363D RID: 13885 RVA: 0x00115D70 File Offset: 0x00113F70
		public override void Reset()
		{
		}

		// Token: 0x0600363E RID: 13886 RVA: 0x00115D74 File Offset: 0x00113F74
		public override void OnGUI()
		{
			GUILayout.EndArea();
		}
	}
}
