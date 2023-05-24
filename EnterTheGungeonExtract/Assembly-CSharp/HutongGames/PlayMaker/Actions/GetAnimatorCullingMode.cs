using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008A5 RID: 2213
	[Tooltip("Returns the culling of this Animator component. Optionnaly sends events.\nIf true ('AlwaysAnimate'): always animate the entire character. Object is animated even when offscreen.\nIf False ('BasedOnRenderers') animation is disabled when renderers are not visible.")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorCullingMode : FsmStateAction
	{
		// Token: 0x06003138 RID: 12600 RVA: 0x00104E64 File Offset: 0x00103064
		public override void Reset()
		{
			this.gameObject = null;
			this.alwaysAnimate = null;
			this.alwaysAnimateEvent = null;
			this.basedOnRenderersEvent = null;
		}

		// Token: 0x06003139 RID: 12601 RVA: 0x00104E84 File Offset: 0x00103084
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
			this.DoCheckCulling();
			base.Finish();
		}

		// Token: 0x0600313A RID: 12602 RVA: 0x00104EE8 File Offset: 0x001030E8
		private void DoCheckCulling()
		{
			if (this._animator == null)
			{
				return;
			}
			bool flag = this._animator.cullingMode == AnimatorCullingMode.AlwaysAnimate;
			this.alwaysAnimate.Value = flag;
			if (flag)
			{
				base.Fsm.Event(this.alwaysAnimateEvent);
			}
			else
			{
				base.Fsm.Event(this.basedOnRenderersEvent);
			}
		}

		// Token: 0x04002246 RID: 8774
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002247 RID: 8775
		[UIHint(UIHint.Variable)]
		[Tooltip("If true, always animate the entire character, else animation is disabled when renderers are not visible")]
		[ActionSection("Results")]
		[RequiredField]
		public FsmBool alwaysAnimate;

		// Token: 0x04002248 RID: 8776
		[Tooltip("Event send if culling mode is 'AlwaysAnimate'")]
		public FsmEvent alwaysAnimateEvent;

		// Token: 0x04002249 RID: 8777
		[Tooltip("Event send if culling mode is 'BasedOnRenders'")]
		public FsmEvent basedOnRenderersEvent;

		// Token: 0x0400224A RID: 8778
		private Animator _animator;
	}
}
