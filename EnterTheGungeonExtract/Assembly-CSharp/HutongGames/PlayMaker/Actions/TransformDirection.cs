using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B36 RID: 2870
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Transforms a Direction from a Game Object's local space to world space.")]
	public class TransformDirection : FsmStateAction
	{
		// Token: 0x06003C53 RID: 15443 RVA: 0x0012FB70 File Offset: 0x0012DD70
		public override void Reset()
		{
			this.gameObject = null;
			this.localDirection = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x06003C54 RID: 15444 RVA: 0x0012FB90 File Offset: 0x0012DD90
		public override void OnEnter()
		{
			this.DoTransformDirection();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x0012FBAC File Offset: 0x0012DDAC
		public override void OnUpdate()
		{
			this.DoTransformDirection();
		}

		// Token: 0x06003C56 RID: 15446 RVA: 0x0012FBB4 File Offset: 0x0012DDB4
		private void DoTransformDirection()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this.storeResult.Value = ownerDefaultTarget.transform.TransformDirection(this.localDirection.Value);
		}

		// Token: 0x04002EA1 RID: 11937
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002EA2 RID: 11938
		[RequiredField]
		public FsmVector3 localDirection;

		// Token: 0x04002EA3 RID: 11939
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 storeResult;

		// Token: 0x04002EA4 RID: 11940
		public bool everyFrame;
	}
}
