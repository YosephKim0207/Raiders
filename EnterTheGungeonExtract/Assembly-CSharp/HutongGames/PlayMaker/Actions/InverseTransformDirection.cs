using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009F3 RID: 2547
	[Tooltip("Transforms a Direction from world space to a Game Object's local space. The opposite of TransformDirection.")]
	[ActionCategory(ActionCategory.Transform)]
	public class InverseTransformDirection : FsmStateAction
	{
		// Token: 0x060036A8 RID: 13992 RVA: 0x00117358 File Offset: 0x00115558
		public override void Reset()
		{
			this.gameObject = null;
			this.worldDirection = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x00117378 File Offset: 0x00115578
		public override void OnEnter()
		{
			this.DoInverseTransformDirection();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x00117394 File Offset: 0x00115594
		public override void OnUpdate()
		{
			this.DoInverseTransformDirection();
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x0011739C File Offset: 0x0011559C
		private void DoInverseTransformDirection()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this.storeResult.Value = ownerDefaultTarget.transform.InverseTransformDirection(this.worldDirection.Value);
		}

		// Token: 0x04002807 RID: 10247
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002808 RID: 10248
		[RequiredField]
		public FsmVector3 worldDirection;

		// Token: 0x04002809 RID: 10249
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 storeResult;

		// Token: 0x0400280A RID: 10250
		public bool everyFrame;
	}
}
