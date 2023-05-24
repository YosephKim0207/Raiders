using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009AD RID: 2477
	[ActionCategory(ActionCategory.Application)]
	[Tooltip("Gets the Width of the Screen in pixels.")]
	public class GetScreenWidth : FsmStateAction
	{
		// Token: 0x060035AB RID: 13739 RVA: 0x00113D48 File Offset: 0x00111F48
		public override void Reset()
		{
			this.storeScreenWidth = null;
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x00113D54 File Offset: 0x00111F54
		public override void OnEnter()
		{
			this.storeScreenWidth.Value = (float)Screen.width;
			base.Finish();
		}

		// Token: 0x040026F9 RID: 9977
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeScreenWidth;
	}
}
