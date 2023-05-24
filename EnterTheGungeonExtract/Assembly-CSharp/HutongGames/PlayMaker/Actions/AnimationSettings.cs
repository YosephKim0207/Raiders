using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000896 RID: 2198
	[Tooltip("Set the Wrap Mode, Blend Mode, Layer and Speed of an Animation.\nNOTE: Settings are applied once, on entering the state, NOT continuously. To dynamically control an animation's settings, use Set Animation Speede etc.")]
	[ActionCategory(ActionCategory.Animation)]
	public class AnimationSettings : BaseAnimationAction
	{
		// Token: 0x060030FF RID: 12543 RVA: 0x001041D8 File Offset: 0x001023D8
		public override void Reset()
		{
			this.gameObject = null;
			this.animName = null;
			this.wrapMode = WrapMode.Loop;
			this.blendMode = AnimationBlendMode.Blend;
			this.speed = 1f;
			this.layer = 0;
		}

		// Token: 0x06003100 RID: 12544 RVA: 0x00104214 File Offset: 0x00102414
		public override void OnEnter()
		{
			this.DoAnimationSettings();
			base.Finish();
		}

		// Token: 0x06003101 RID: 12545 RVA: 0x00104224 File Offset: 0x00102424
		private void DoAnimationSettings()
		{
			if (string.IsNullOrEmpty(this.animName.Value))
			{
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			AnimationState animationState = base.animation[this.animName.Value];
			if (animationState == null)
			{
				base.LogWarning("Missing animation: " + this.animName.Value);
				return;
			}
			animationState.wrapMode = this.wrapMode;
			animationState.blendMode = this.blendMode;
			if (!this.layer.IsNone)
			{
				animationState.layer = this.layer.Value;
			}
			if (!this.speed.IsNone)
			{
				animationState.speed = this.speed.Value;
			}
		}

		// Token: 0x04002203 RID: 8707
		[CheckForComponent(typeof(Animation))]
		[Tooltip("A GameObject with an Animation Component.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002204 RID: 8708
		[UIHint(UIHint.Animation)]
		[Tooltip("The name of the animation.")]
		[RequiredField]
		public FsmString animName;

		// Token: 0x04002205 RID: 8709
		[Tooltip("The behavior of the animation when it wraps.")]
		public WrapMode wrapMode;

		// Token: 0x04002206 RID: 8710
		[Tooltip("How the animation is blended with other animations on the Game Object.")]
		public AnimationBlendMode blendMode;

		// Token: 0x04002207 RID: 8711
		[Tooltip("The speed of the animation. 1 = normal; 2 = double speed...")]
		[HasFloatSlider(0f, 5f)]
		public FsmFloat speed;

		// Token: 0x04002208 RID: 8712
		[Tooltip("The animation layer")]
		public FsmInt layer;
	}
}
