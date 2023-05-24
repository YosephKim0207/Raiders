using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009D5 RID: 2517
	[Tooltip("End a centered GUILayout block started with GUILayoutBeginCentered.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutEndCentered : FsmStateAction
	{
		// Token: 0x06003640 RID: 13888 RVA: 0x00115D84 File Offset: 0x00113F84
		public override void Reset()
		{
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x00115D88 File Offset: 0x00113F88
		public override void OnGUI()
		{
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
		}
	}
}
