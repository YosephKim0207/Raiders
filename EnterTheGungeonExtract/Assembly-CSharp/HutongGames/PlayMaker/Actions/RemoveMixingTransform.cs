using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AA7 RID: 2727
	[Tooltip("Removes a mixing transform previously added with Add Mixing Transform. If transform has been added as recursive, then it will be removed as recursive. Once you remove all mixing transforms added to animation state all curves become animated again.")]
	[ActionCategory(ActionCategory.Animation)]
	public class RemoveMixingTransform : BaseAnimationAction
	{
		// Token: 0x060039DD RID: 14813 RVA: 0x00127388 File Offset: 0x00125588
		public override void Reset()
		{
			this.gameObject = null;
			this.animationName = string.Empty;
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x001273A4 File Offset: 0x001255A4
		public override void OnEnter()
		{
			this.DoRemoveMixingTransform();
			base.Finish();
		}

		// Token: 0x060039DF RID: 14815 RVA: 0x001273B4 File Offset: 0x001255B4
		private void DoRemoveMixingTransform()
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
			Transform transform = ownerDefaultTarget.transform.Find(this.transfrom.Value);
			animationState.AddMixingTransform(transform);
		}

		// Token: 0x04002C22 RID: 11298
		[Tooltip("The GameObject playing the animation.")]
		[CheckForComponent(typeof(Animation))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002C23 RID: 11299
		[RequiredField]
		[Tooltip("The name of the animation.")]
		public FsmString animationName;

		// Token: 0x04002C24 RID: 11300
		[Tooltip("The mixing transform to remove. E.g., root/upper_body/left_shoulder")]
		[RequiredField]
		public FsmString transfrom;
	}
}
