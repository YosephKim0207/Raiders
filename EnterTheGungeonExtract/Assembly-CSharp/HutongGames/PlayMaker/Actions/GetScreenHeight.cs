using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009AC RID: 2476
	[Tooltip("Gets the Height of the Screen in pixels.")]
	[ActionCategory(ActionCategory.Application)]
	public class GetScreenHeight : FsmStateAction
	{
		// Token: 0x060035A8 RID: 13736 RVA: 0x00113D18 File Offset: 0x00111F18
		public override void Reset()
		{
			this.storeScreenHeight = null;
		}

		// Token: 0x060035A9 RID: 13737 RVA: 0x00113D24 File Offset: 0x00111F24
		public override void OnEnter()
		{
			this.storeScreenHeight.Value = (float)Screen.height;
			base.Finish();
		}

		// Token: 0x040026F8 RID: 9976
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeScreenHeight;
	}
}
