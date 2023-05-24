using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008CE RID: 2254
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Sets the layer's current weight")]
	public class SetAnimatorLayerWeight : FsmStateAction
	{
		// Token: 0x060031FB RID: 12795 RVA: 0x00107BC0 File Offset: 0x00105DC0
		public override void Reset()
		{
			this.gameObject = null;
			this.layerIndex = null;
			this.layerWeight = null;
			this.everyFrame = false;
		}

		// Token: 0x060031FC RID: 12796 RVA: 0x00107BE0 File Offset: 0x00105DE0
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
			this.DoLayerWeight();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x00107C50 File Offset: 0x00105E50
		public override void OnUpdate()
		{
			this.DoLayerWeight();
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x00107C58 File Offset: 0x00105E58
		private void DoLayerWeight()
		{
			if (this._animator == null)
			{
				return;
			}
			this._animator.SetLayerWeight(this.layerIndex.Value, this.layerWeight.Value);
		}

		// Token: 0x04002320 RID: 8992
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002321 RID: 8993
		[Tooltip("The layer's index")]
		[RequiredField]
		public FsmInt layerIndex;

		// Token: 0x04002322 RID: 8994
		[Tooltip("Sets the layer's current weight")]
		[RequiredField]
		public FsmFloat layerWeight;

		// Token: 0x04002323 RID: 8995
		[Tooltip("Repeat every frame. Useful for changing over time.")]
		public bool everyFrame;

		// Token: 0x04002324 RID: 8996
		private Animator _animator;
	}
}
