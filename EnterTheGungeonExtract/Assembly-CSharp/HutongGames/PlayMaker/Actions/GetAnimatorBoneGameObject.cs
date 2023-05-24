using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008A3 RID: 2211
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Gets the GameObject mapped to this human bone id")]
	public class GetAnimatorBoneGameObject : FsmStateAction
	{
		// Token: 0x0600312F RID: 12591 RVA: 0x00104CC0 File Offset: 0x00102EC0
		public override void Reset()
		{
			this.gameObject = null;
			this.bone = HumanBodyBones.Hips;
			this.boneGameObject = null;
		}

		// Token: 0x06003130 RID: 12592 RVA: 0x00104CE4 File Offset: 0x00102EE4
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
			this.GetBoneTransform();
			base.Finish();
		}

		// Token: 0x06003131 RID: 12593 RVA: 0x00104D48 File Offset: 0x00102F48
		private void GetBoneTransform()
		{
			this.boneGameObject.Value = this._animator.GetBoneTransform((HumanBodyBones)this.bone.Value).gameObject;
		}

		// Token: 0x0400223D RID: 8765
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400223E RID: 8766
		[ObjectType(typeof(HumanBodyBones))]
		[Tooltip("The bone reference")]
		public FsmEnum bone;

		// Token: 0x0400223F RID: 8767
		[Tooltip("The Bone's GameObject")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		public FsmGameObject boneGameObject;

		// Token: 0x04002240 RID: 8768
		private Animator _animator;
	}
}
