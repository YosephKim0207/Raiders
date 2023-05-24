using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000898 RID: 2200
	[Tooltip("Interrupts the automatic target matching. CompleteMatch will make the gameobject match the target completely at the next frame.")]
	[ActionCategory(ActionCategory.Animator)]
	public class AnimatorInterruptMatchTarget : FsmStateAction
	{
		// Token: 0x06003106 RID: 12550 RVA: 0x00104424 File Offset: 0x00102624
		public override void Reset()
		{
			this.gameObject = null;
			this.completeMatch = true;
		}

		// Token: 0x06003107 RID: 12551 RVA: 0x0010443C File Offset: 0x0010263C
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
				component.InterruptMatchTarget(this.completeMatch.Value);
			}
			base.Finish();
		}

		// Token: 0x04002210 RID: 8720
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002211 RID: 8721
		[Tooltip("Will make the gameobject match the target completely at the next frame")]
		public FsmBool completeMatch;
	}
}
