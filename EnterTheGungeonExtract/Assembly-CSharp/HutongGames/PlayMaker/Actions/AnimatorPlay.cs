using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200089A RID: 2202
	[Tooltip("Plays a state. This could be used to synchronize your animation with audio or synchronize an Animator over the network.")]
	[ActionCategory(ActionCategory.Animator)]
	public class AnimatorPlay : FsmStateAction
	{
		// Token: 0x0600310E RID: 12558 RVA: 0x001046A4 File Offset: 0x001028A4
		public override void Reset()
		{
			this.gameObject = null;
			this.stateName = null;
			this.layer = new FsmInt
			{
				UseVariable = true
			};
			this.normalizedTime = new FsmFloat
			{
				UseVariable = true
			};
			this.everyFrame = false;
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x001046F0 File Offset: 0x001028F0
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				base.Finish();
				return;
			}
			this._animator = ownerDefaultTarget.GetComponent<Animator>();
			this.DoAnimatorPlay();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003110 RID: 12560 RVA: 0x00104748 File Offset: 0x00102948
		public override void OnUpdate()
		{
			this.DoAnimatorPlay();
		}

		// Token: 0x06003111 RID: 12561 RVA: 0x00104750 File Offset: 0x00102950
		private void DoAnimatorPlay()
		{
			if (this._animator != null)
			{
				int num = ((!this.layer.IsNone) ? this.layer.Value : (-1));
				float num2 = ((!this.normalizedTime.IsNone) ? this.normalizedTime.Value : float.NegativeInfinity);
				this._animator.Play(this.stateName.Value, num, num2);
			}
		}

		// Token: 0x0400221E RID: 8734
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400221F RID: 8735
		[Tooltip("The name of the state that will be played.")]
		public FsmString stateName;

		// Token: 0x04002220 RID: 8736
		[Tooltip("The layer where the state is.")]
		public FsmInt layer;

		// Token: 0x04002221 RID: 8737
		[Tooltip("The normalized time at which the state will play")]
		public FsmFloat normalizedTime;

		// Token: 0x04002222 RID: 8738
		[Tooltip("Repeat every frame. Useful when using normalizedTime to manually control the animation.")]
		public bool everyFrame;

		// Token: 0x04002223 RID: 8739
		private Animator _animator;
	}
}
