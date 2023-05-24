using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AA9 RID: 2729
	[Tooltip("Resets all Input. After ResetInputAxes all axes return to 0 and all buttons return to 0 for one frame")]
	[ActionCategory(ActionCategory.Input)]
	public class ResetInputAxes : FsmStateAction
	{
		// Token: 0x060039E3 RID: 14819 RVA: 0x00127454 File Offset: 0x00125654
		public override void Reset()
		{
		}

		// Token: 0x060039E4 RID: 14820 RVA: 0x00127458 File Offset: 0x00125658
		public override void OnEnter()
		{
			Input.ResetInputAxes();
			base.Finish();
		}
	}
}
