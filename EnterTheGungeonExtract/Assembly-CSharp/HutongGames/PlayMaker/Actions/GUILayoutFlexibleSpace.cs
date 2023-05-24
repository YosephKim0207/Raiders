using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009D9 RID: 2521
	[Tooltip("Inserts a flexible space element.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutFlexibleSpace : FsmStateAction
	{
		// Token: 0x0600364B RID: 13899 RVA: 0x00115DE4 File Offset: 0x00113FE4
		public override void Reset()
		{
		}

		// Token: 0x0600364C RID: 13900 RVA: 0x00115DE8 File Offset: 0x00113FE8
		public override void OnGUI()
		{
			GUILayout.FlexibleSpace();
		}
	}
}
