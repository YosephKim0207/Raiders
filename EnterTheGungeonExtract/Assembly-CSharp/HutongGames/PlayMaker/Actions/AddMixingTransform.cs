using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200087F RID: 2175
	[Tooltip("Play an animation on a subset of the hierarchy. E.g., A waving animation on the upper body.")]
	[ActionCategory(ActionCategory.Animation)]
	public class AddMixingTransform : BaseAnimationAction
	{
		// Token: 0x06003079 RID: 12409 RVA: 0x000FF13C File Offset: 0x000FD33C
		public override void Reset()
		{
			this.gameObject = null;
			this.animationName = string.Empty;
			this.transform = string.Empty;
			this.recursive = true;
		}

		// Token: 0x0600307A RID: 12410 RVA: 0x000FF174 File Offset: 0x000FD374
		public override void OnEnter()
		{
			this.DoAddMixingTransform();
			base.Finish();
		}

		// Token: 0x0600307B RID: 12411 RVA: 0x000FF184 File Offset: 0x000FD384
		private void DoAddMixingTransform()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			AnimationState animationState = base.animation[this.animationName.Value];
			if (animationState == null)
			{
				return;
			}
			Transform transform = ownerDefaultTarget.transform.Find(this.transform.Value);
			animationState.AddMixingTransform(transform, this.recursive.Value);
		}

		// Token: 0x0400211D RID: 8477
		[Tooltip("The GameObject playing the animation.")]
		[RequiredField]
		[CheckForComponent(typeof(Animation))]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400211E RID: 8478
		[RequiredField]
		[Tooltip("The name of the animation to mix. NOTE: The animation should already be added to the Animation Component on the GameObject.")]
		public FsmString animationName;

		// Token: 0x0400211F RID: 8479
		[RequiredField]
		[Tooltip("The mixing transform. E.g., root/upper_body/left_shoulder")]
		public FsmString transform;

		// Token: 0x04002120 RID: 8480
		[Tooltip("If recursive is true all children of the mix transform will also be animated.")]
		public FsmBool recursive;
	}
}
