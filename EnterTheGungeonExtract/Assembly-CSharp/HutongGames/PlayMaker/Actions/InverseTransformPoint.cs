using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009F4 RID: 2548
	[Tooltip("Transforms position from world space to a Game Object's local space. The opposite of TransformPoint.")]
	[ActionCategory(ActionCategory.Transform)]
	public class InverseTransformPoint : FsmStateAction
	{
		// Token: 0x060036AD RID: 13997 RVA: 0x001173F4 File Offset: 0x001155F4
		public override void Reset()
		{
			this.gameObject = null;
			this.worldPosition = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x00117414 File Offset: 0x00115614
		public override void OnEnter()
		{
			this.DoInverseTransformPoint();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x00117430 File Offset: 0x00115630
		public override void OnUpdate()
		{
			this.DoInverseTransformPoint();
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x00117438 File Offset: 0x00115638
		private void DoInverseTransformPoint()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this.storeResult.Value = ownerDefaultTarget.transform.InverseTransformPoint(this.worldPosition.Value);
		}

		// Token: 0x0400280B RID: 10251
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400280C RID: 10252
		[RequiredField]
		public FsmVector3 worldPosition;

		// Token: 0x0400280D RID: 10253
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 storeResult;

		// Token: 0x0400280E RID: 10254
		public bool everyFrame;
	}
}
