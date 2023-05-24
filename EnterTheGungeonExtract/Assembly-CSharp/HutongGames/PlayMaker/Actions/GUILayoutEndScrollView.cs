using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009D7 RID: 2519
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("Close a group started with GUILayout Begin ScrollView.")]
	public class GUILayoutEndScrollView : FsmStateAction
	{
		// Token: 0x06003646 RID: 13894 RVA: 0x00115DC0 File Offset: 0x00113FC0
		public override void OnGUI()
		{
			GUILayout.EndScrollView();
		}
	}
}
