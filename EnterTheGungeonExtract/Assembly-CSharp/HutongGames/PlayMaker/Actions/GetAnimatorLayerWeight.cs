using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008BA RID: 2234
	[Tooltip("Gets the layer's current weight")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorLayerWeight : FsmStateActionAnimatorBase
	{
		// Token: 0x06003199 RID: 12697 RVA: 0x001065A0 File Offset: 0x001047A0
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.layerIndex = null;
			this.layerWeight = null;
		}

		// Token: 0x0600319A RID: 12698 RVA: 0x001065C0 File Offset: 0x001047C0
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
			this.GetLayerWeight();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x00106630 File Offset: 0x00104830
		public override void OnActionUpdate()
		{
			this.GetLayerWeight();
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x00106638 File Offset: 0x00104838
		private void GetLayerWeight()
		{
			if (this._animator != null)
			{
				this.layerWeight.Value = this._animator.GetLayerWeight(this.layerIndex.Value);
			}
		}

		// Token: 0x040022BD RID: 8893
		[Tooltip("The target.")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022BE RID: 8894
		[Tooltip("The layer's index")]
		[RequiredField]
		public FsmInt layerIndex;

		// Token: 0x040022BF RID: 8895
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[ActionSection("Results")]
		[Tooltip("The layer's current weight")]
		public FsmFloat layerWeight;

		// Token: 0x040022C0 RID: 8896
		private Animator _animator;
	}
}
