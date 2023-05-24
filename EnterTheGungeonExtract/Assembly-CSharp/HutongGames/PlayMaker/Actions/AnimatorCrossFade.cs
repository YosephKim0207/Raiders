using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000897 RID: 2199
	[Tooltip("Create a dynamic transition between the current state and the destination state.Both state as to be on the same layer. note: You cannot change the current state on a synchronized layer, you need to change it on the referenced layer.")]
	[ActionCategory(ActionCategory.Animator)]
	public class AnimatorCrossFade : FsmStateAction
	{
		// Token: 0x06003103 RID: 12547 RVA: 0x00104308 File Offset: 0x00102508
		public override void Reset()
		{
			this.gameObject = null;
			this.stateName = null;
			this.transitionDuration = 1f;
			this.layer = new FsmInt
			{
				UseVariable = true
			};
			this.normalizedTime = new FsmFloat
			{
				UseVariable = true
			};
		}

		// Token: 0x06003104 RID: 12548 RVA: 0x0010435C File Offset: 0x0010255C
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				base.Finish();
				return;
			}
			this._animator = ownerDefaultTarget.GetComponent<Animator>();
			if (this._animator != null)
			{
				int num = ((!this.layer.IsNone) ? this.layer.Value : (-1));
				float num2 = ((!this.normalizedTime.IsNone) ? this.normalizedTime.Value : float.NegativeInfinity);
				this._animator.CrossFade(this.stateName.Value, this.transitionDuration.Value, num, num2);
			}
			base.Finish();
		}

		// Token: 0x04002209 RID: 8713
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component is required")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400220A RID: 8714
		[Tooltip("The name of the state that will be played.")]
		public FsmString stateName;

		// Token: 0x0400220B RID: 8715
		[Tooltip("The duration of the transition. Value is in source state normalized time.")]
		public FsmFloat transitionDuration;

		// Token: 0x0400220C RID: 8716
		[Tooltip("Layer index containing the destination state. Leave to none to ignore")]
		public FsmInt layer;

		// Token: 0x0400220D RID: 8717
		[Tooltip("Start time of the current destination state. Value is in source state normalized time, should be between 0 and 1.")]
		public FsmFloat normalizedTime;

		// Token: 0x0400220E RID: 8718
		private Animator _animator;

		// Token: 0x0400220F RID: 8719
		private int _paramID;
	}
}
