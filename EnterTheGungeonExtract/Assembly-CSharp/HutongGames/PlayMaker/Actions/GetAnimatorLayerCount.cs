using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008B7 RID: 2231
	[Tooltip("Returns the Animator controller layer count")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorLayerCount : FsmStateAction
	{
		// Token: 0x0600318D RID: 12685 RVA: 0x0010634C File Offset: 0x0010454C
		public override void Reset()
		{
			this.gameObject = null;
			this.layerCount = null;
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x0010635C File Offset: 0x0010455C
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				base.Finish();
				return;
			}
			this._animator = ownerDefaultTarget.GetComponent<Animator>();
			if (this._animator == null)
			{
				base.Finish();
				return;
			}
			this.DoGetLayerCount();
			base.Finish();
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x001063C0 File Offset: 0x001045C0
		private void DoGetLayerCount()
		{
			if (this._animator == null)
			{
				return;
			}
			this.layerCount.Value = this._animator.layerCount;
		}

		// Token: 0x040022B1 RID: 8881
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022B2 RID: 8882
		[RequiredField]
		[ActionSection("Results")]
		[Tooltip("The Animator controller layer count")]
		[UIHint(UIHint.Variable)]
		public FsmInt layerCount;

		// Token: 0x040022B3 RID: 8883
		private Animator _animator;
	}
}
