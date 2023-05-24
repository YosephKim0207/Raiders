using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009D8 RID: 2520
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("Close a group started with BeginVertical.")]
	public class GUILayoutEndVertical : FsmStateAction
	{
		// Token: 0x06003648 RID: 13896 RVA: 0x00115DD0 File Offset: 0x00113FD0
		public override void Reset()
		{
		}

		// Token: 0x06003649 RID: 13897 RVA: 0x00115DD4 File Offset: 0x00113FD4
		public override void OnGUI()
		{
			GUILayout.EndVertical();
		}
	}
}
