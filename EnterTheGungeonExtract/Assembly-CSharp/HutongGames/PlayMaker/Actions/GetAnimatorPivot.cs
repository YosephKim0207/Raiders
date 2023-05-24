using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008BD RID: 2237
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Returns the pivot weight and/or position. The pivot is the most stable point between the avatar's left and right foot.\n For a weight value of 0, the left foot is the most stable point For a value of 1, the right foot is the most stable point")]
	public class GetAnimatorPivot : FsmStateActionAnimatorBase
	{
		// Token: 0x060031A8 RID: 12712 RVA: 0x0010697C File Offset: 0x00104B7C
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.pivotWeight = null;
			this.pivotPosition = null;
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x0010699C File Offset: 0x00104B9C
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
			this.DoCheckPivot();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x00106A0C File Offset: 0x00104C0C
		public override void OnActionUpdate()
		{
			this.DoCheckPivot();
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x00106A14 File Offset: 0x00104C14
		private void DoCheckPivot()
		{
			if (this._animator == null)
			{
				return;
			}
			if (!this.pivotWeight.IsNone)
			{
				this.pivotWeight.Value = this._animator.pivotWeight;
			}
			if (!this.pivotPosition.IsNone)
			{
				this.pivotPosition.Value = this._animator.pivotPosition;
			}
		}

		// Token: 0x040022D0 RID: 8912
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022D1 RID: 8913
		[Tooltip("The pivot is the most stable point between the avatar's left and right foot.\n For a value of 0, the left foot is the most stable point For a value of 1, the right foot is the most stable point")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		public FsmFloat pivotWeight;

		// Token: 0x040022D2 RID: 8914
		[UIHint(UIHint.Variable)]
		[Tooltip("The pivot is the most stable point between the avatar's left and right foot.\n For a value of 0, the left foot is the most stable point For a value of 1, the right foot is the most stable point")]
		public FsmVector3 pivotPosition;

		// Token: 0x040022D3 RID: 8915
		private Animator _animator;
	}
}
