using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009D6 RID: 2518
	[Tooltip("Close a group started with BeginHorizontal.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutEndHorizontal : FsmStateAction
	{
		// Token: 0x06003643 RID: 13891 RVA: 0x00115DAC File Offset: 0x00113FAC
		public override void Reset()
		{
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x00115DB0 File Offset: 0x00113FB0
		public override void OnGUI()
		{
			GUILayout.EndHorizontal();
		}
	}
}
