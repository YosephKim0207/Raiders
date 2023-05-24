using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008B8 RID: 2232
	[Tooltip("Returns the name of a layer from its index")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorLayerName : FsmStateAction
	{
		// Token: 0x06003191 RID: 12689 RVA: 0x001063F4 File Offset: 0x001045F4
		public override void Reset()
		{
			this.gameObject = null;
			this.layerIndex = null;
			this.layerName = null;
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x0010640C File Offset: 0x0010460C
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
			this.DoGetLayerName();
			base.Finish();
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x00106470 File Offset: 0x00104670
		private void DoGetLayerName()
		{
			if (this._animator == null)
			{
				return;
			}
			this.layerName.Value = this._animator.GetLayerName(this.layerIndex.Value);
		}

		// Token: 0x040022B4 RID: 8884
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022B5 RID: 8885
		[Tooltip("The layer index")]
		[RequiredField]
		public FsmInt layerIndex;

		// Token: 0x040022B6 RID: 8886
		[ActionSection("Results")]
		[Tooltip("The layer name")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString layerName;

		// Token: 0x040022B7 RID: 8887
		private Animator _animator;
	}
}
