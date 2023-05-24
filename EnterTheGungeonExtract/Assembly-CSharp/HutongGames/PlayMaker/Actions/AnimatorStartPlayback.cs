using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200089B RID: 2203
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Sets the animator in playback mode.")]
	public class AnimatorStartPlayback : FsmStateAction
	{
		// Token: 0x06003113 RID: 12563 RVA: 0x001047D8 File Offset: 0x001029D8
		public override void Reset()
		{
			this.gameObject = null;
		}

		// Token: 0x06003114 RID: 12564 RVA: 0x001047E4 File Offset: 0x001029E4
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				base.Finish();
				return;
			}
			Animator component = ownerDefaultTarget.GetComponent<Animator>();
			if (component != null)
			{
				component.StartPlayback();
			}
			base.Finish();
		}

		// Token: 0x04002224 RID: 8740
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;
	}
}
