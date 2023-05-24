using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009B8 RID: 2488
	[Tooltip("Gets the number of Touches.")]
	[ActionCategory(ActionCategory.Device)]
	public class GetTouchCount : FsmStateAction
	{
		// Token: 0x060035D8 RID: 13784 RVA: 0x001143AC File Offset: 0x001125AC
		public override void Reset()
		{
			this.storeCount = null;
			this.everyFrame = false;
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x001143BC File Offset: 0x001125BC
		public override void OnEnter()
		{
			this.DoGetTouchCount();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x001143D8 File Offset: 0x001125D8
		public override void OnUpdate()
		{
			this.DoGetTouchCount();
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x001143E0 File Offset: 0x001125E0
		private void DoGetTouchCount()
		{
			this.storeCount.Value = Input.touchCount;
		}

		// Token: 0x04002721 RID: 10017
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt storeCount;

		// Token: 0x04002722 RID: 10018
		public bool everyFrame;
	}
}
