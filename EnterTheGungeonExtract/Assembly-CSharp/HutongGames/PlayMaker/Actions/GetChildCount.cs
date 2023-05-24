using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200096E RID: 2414
	[Tooltip("Gets the number of children that a GameObject has.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetChildCount : FsmStateAction
	{
		// Token: 0x06003497 RID: 13463 RVA: 0x00110A38 File Offset: 0x0010EC38
		public override void Reset()
		{
			this.gameObject = null;
			this.storeResult = null;
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x00110A48 File Offset: 0x0010EC48
		public override void OnEnter()
		{
			this.DoGetChildCount();
			base.Finish();
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x00110A58 File Offset: 0x0010EC58
		private void DoGetChildCount()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this.storeResult.Value = ownerDefaultTarget.transform.childCount;
		}

		// Token: 0x040025C9 RID: 9673
		[RequiredField]
		[Tooltip("The GameObject to test.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x040025CA RID: 9674
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the number of children in an int variable.")]
		public FsmInt storeResult;
	}
}
