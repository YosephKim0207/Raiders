using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A84 RID: 2692
	[Tooltip("Plays a Random Animation on a Game Object. You can set the relative weight of each animation to control how often they are selected.")]
	[ActionCategory(ActionCategory.Animation)]
	public class PlayRandomAnimation : BaseAnimationAction
	{
		// Token: 0x06003922 RID: 14626 RVA: 0x00124DCC File Offset: 0x00122FCC
		public override void Reset()
		{
			this.gameObject = null;
			this.animations = new FsmString[0];
			this.weights = new FsmFloat[0];
			this.playMode = PlayMode.StopAll;
			this.blendTime = 0.3f;
			this.finishEvent = null;
			this.loopEvent = null;
			this.stopOnExit = false;
		}

		// Token: 0x06003923 RID: 14627 RVA: 0x00124E24 File Offset: 0x00123024
		public override void OnEnter()
		{
			this.DoPlayRandomAnimation();
		}

		// Token: 0x06003924 RID: 14628 RVA: 0x00124E2C File Offset: 0x0012302C
		private void DoPlayRandomAnimation()
		{
			if (this.animations.Length > 0)
			{
				int randomWeightedIndex = ActionHelpers.GetRandomWeightedIndex(this.weights);
				if (randomWeightedIndex != -1)
				{
					this.DoPlayAnimation(this.animations[randomWeightedIndex].Value);
				}
			}
		}

		// Token: 0x06003925 RID: 14629 RVA: 0x00124E70 File Offset: 0x00123070
		private void DoPlayAnimation(string animName)
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				base.Finish();
				return;
			}
			if (string.IsNullOrEmpty(animName))
			{
				base.LogWarning("Missing animName!");
				base.Finish();
				return;
			}
			this.anim = base.animation[animName];
			if (this.anim == null)
			{
				base.LogWarning("Missing animation: " + animName);
				base.Finish();
				return;
			}
			float value = this.blendTime.Value;
			if (value < 0.001f)
			{
				base.animation.Play(animName, this.playMode);
			}
			else
			{
				base.animation.CrossFade(animName, value, this.playMode);
			}
			this.prevAnimtTime = this.anim.time;
		}

		// Token: 0x06003926 RID: 14630 RVA: 0x00124F50 File Offset: 0x00123150
		public override void OnUpdate()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null || this.anim == null)
			{
				return;
			}
			if (!this.anim.enabled || (this.anim.wrapMode == WrapMode.ClampForever && this.anim.time > this.anim.length))
			{
				base.Fsm.Event(this.finishEvent);
				base.Finish();
			}
			if (this.anim.wrapMode != WrapMode.ClampForever && this.anim.time > this.anim.length && this.prevAnimtTime < this.anim.length)
			{
				base.Fsm.Event(this.loopEvent);
			}
		}

		// Token: 0x06003927 RID: 14631 RVA: 0x00125034 File Offset: 0x00123234
		public override void OnExit()
		{
			if (this.stopOnExit)
			{
				this.StopAnimation();
			}
		}

		// Token: 0x06003928 RID: 14632 RVA: 0x00125048 File Offset: 0x00123248
		private void StopAnimation()
		{
			if (base.animation != null)
			{
				base.animation.Stop(this.anim.name);
			}
		}

		// Token: 0x04002B71 RID: 11121
		[RequiredField]
		[CheckForComponent(typeof(Animation))]
		[Tooltip("Game Object to play the animation on.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002B72 RID: 11122
		[UIHint(UIHint.Animation)]
		[CompoundArray("Animations", "Animation", "Weight")]
		public FsmString[] animations;

		// Token: 0x04002B73 RID: 11123
		[HasFloatSlider(0f, 1f)]
		public FsmFloat[] weights;

		// Token: 0x04002B74 RID: 11124
		[Tooltip("How to treat previously playing animations.")]
		public PlayMode playMode;

		// Token: 0x04002B75 RID: 11125
		[Tooltip("Time taken to blend to this animation.")]
		[HasFloatSlider(0f, 5f)]
		public FsmFloat blendTime;

		// Token: 0x04002B76 RID: 11126
		[Tooltip("Event to send when the animation is finished playing. NOTE: Not sent with Loop or PingPong wrap modes!")]
		public FsmEvent finishEvent;

		// Token: 0x04002B77 RID: 11127
		[Tooltip("Event to send when the animation loops. If you want to send this event to another FSM use Set Event Target. NOTE: This event is only sent with Loop and PingPong wrap modes.")]
		public FsmEvent loopEvent;

		// Token: 0x04002B78 RID: 11128
		[Tooltip("Stop playing the animation when this state is exited.")]
		public bool stopOnExit;

		// Token: 0x04002B79 RID: 11129
		private AnimationState anim;

		// Token: 0x04002B7A RID: 11130
		private float prevAnimtTime;
	}
}
