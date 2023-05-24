using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008C8 RID: 2248
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Controls culling of this Animator component.\nIf true, set to 'AlwaysAnimate': always animate the entire character. Object is animated even when offscreen.\nIf False, set to 'BasedOnRenderes' or CullUpdateTransforms ( On Unity 5) animation is disabled when renderers are not visible.")]
	public class SetAnimatorCullingMode : FsmStateAction
	{
		// Token: 0x060031DF RID: 12767 RVA: 0x001074C8 File Offset: 0x001056C8
		public override void Reset()
		{
			this.gameObject = null;
			this.alwaysAnimate = null;
		}

		// Token: 0x060031E0 RID: 12768 RVA: 0x001074D8 File Offset: 0x001056D8
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
			this.SetCullingMode();
			base.Finish();
		}

		// Token: 0x060031E1 RID: 12769 RVA: 0x0010753C File Offset: 0x0010573C
		private void SetCullingMode()
		{
			if (this._animator == null)
			{
				return;
			}
			this._animator.cullingMode = ((!this.alwaysAnimate.Value) ? AnimatorCullingMode.CullUpdateTransforms : AnimatorCullingMode.AlwaysAnimate);
		}

		// Token: 0x04002302 RID: 8962
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The Target. An Animator component is required")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002303 RID: 8963
		[Tooltip("If true, always animate the entire character, else animation is disabled when renderers are not visible")]
		public FsmBool alwaysAnimate;

		// Token: 0x04002304 RID: 8964
		private Animator _animator;
	}
}
