using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200087B RID: 2171
	[Tooltip("Adds a named Animation Clip to a Game Object. Optionally trims the Animation.")]
	[ActionCategory(ActionCategory.Animation)]
	public class AddAnimationClip : FsmStateAction
	{
		// Token: 0x06003064 RID: 12388 RVA: 0x000FEC3C File Offset: 0x000FCE3C
		public override void Reset()
		{
			this.gameObject = null;
			this.animationClip = null;
			this.animationName = string.Empty;
			this.firstFrame = 0;
			this.lastFrame = 0;
			this.addLoopFrame = false;
		}

		// Token: 0x06003065 RID: 12389 RVA: 0x000FEC8C File Offset: 0x000FCE8C
		public override void OnEnter()
		{
			this.DoAddAnimationClip();
			base.Finish();
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x000FEC9C File Offset: 0x000FCE9C
		private void DoAddAnimationClip()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			AnimationClip animationClip = this.animationClip.Value as AnimationClip;
			if (animationClip == null)
			{
				return;
			}
			Animation component = ownerDefaultTarget.GetComponent<Animation>();
			if (this.firstFrame.Value == 0 && this.lastFrame.Value == 0)
			{
				component.AddClip(animationClip, this.animationName.Value);
			}
			else
			{
				component.AddClip(animationClip, this.animationName.Value, this.firstFrame.Value, this.lastFrame.Value, this.addLoopFrame.Value);
			}
		}

		// Token: 0x04002102 RID: 8450
		[Tooltip("The GameObject to add the Animation Clip to.")]
		[CheckForComponent(typeof(Animation))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002103 RID: 8451
		[Tooltip("The animation clip to add. NOTE: Make sure the clip is compatible with the object's hierarchy.")]
		[ObjectType(typeof(AnimationClip))]
		[RequiredField]
		public FsmObject animationClip;

		// Token: 0x04002104 RID: 8452
		[RequiredField]
		[Tooltip("Name the animation. Used by other actions to reference this animation.")]
		public FsmString animationName;

		// Token: 0x04002105 RID: 8453
		[Tooltip("Optionally trim the animation by specifying a first and last frame.")]
		public FsmInt firstFrame;

		// Token: 0x04002106 RID: 8454
		[Tooltip("Optionally trim the animation by specifying a first and last frame.")]
		public FsmInt lastFrame;

		// Token: 0x04002107 RID: 8455
		[Tooltip("Add an extra looping frame that matches the first frame.")]
		public FsmBool addLoopFrame;
	}
}
