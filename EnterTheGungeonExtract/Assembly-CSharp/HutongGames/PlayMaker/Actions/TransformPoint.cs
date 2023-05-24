using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B39 RID: 2873
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Transforms a Position from a Game Object's local space to world space.")]
	public class TransformPoint : FsmStateAction
	{
		// Token: 0x06003C5B RID: 15451 RVA: 0x0012FE20 File Offset: 0x0012E020
		public override void Reset()
		{
			this.gameObject = null;
			this.localPosition = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x06003C5C RID: 15452 RVA: 0x0012FE40 File Offset: 0x0012E040
		public override void OnEnter()
		{
			this.DoTransformPoint();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003C5D RID: 15453 RVA: 0x0012FE5C File Offset: 0x0012E05C
		public override void OnUpdate()
		{
			this.DoTransformPoint();
		}

		// Token: 0x06003C5E RID: 15454 RVA: 0x0012FE64 File Offset: 0x0012E064
		private void DoTransformPoint()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this.storeResult.Value = ownerDefaultTarget.transform.TransformPoint(this.localPosition.Value);
		}

		// Token: 0x04002EB0 RID: 11952
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002EB1 RID: 11953
		[RequiredField]
		public FsmVector3 localPosition;

		// Token: 0x04002EB2 RID: 11954
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 storeResult;

		// Token: 0x04002EB3 RID: 11955
		public bool everyFrame;
	}
}
