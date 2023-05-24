using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A79 RID: 2681
	[Tooltip("Plays an Animation on a Game Object. You can add named animation clips to the object in the Unity editor, or with the Add Animation Clip action.")]
	[ActionCategory(ActionCategory.Animation)]
	public class PlayAnimation : BaseAnimationAction
	{
		// Token: 0x060038FD RID: 14589 RVA: 0x00124538 File Offset: 0x00122738
		public override void Reset()
		{
			this.gameObject = null;
			this.animName = null;
			this.playMode = PlayMode.StopAll;
			this.blendTime = 0.3f;
			this.finishEvent = null;
			this.loopEvent = null;
			this.stopOnExit = false;
		}

		// Token: 0x060038FE RID: 14590 RVA: 0x00124574 File Offset: 0x00122774
		public override void OnEnter()
		{
			this.DoPlayAnimation();
		}

		// Token: 0x060038FF RID: 14591 RVA: 0x0012457C File Offset: 0x0012277C
		private void DoPlayAnimation()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				base.Finish();
				return;
			}
			if (string.IsNullOrEmpty(this.animName.Value))
			{
				base.LogWarning("Missing animName!");
				base.Finish();
				return;
			}
			this.anim = base.animation[this.animName.Value];
			if (this.anim == null)
			{
				base.LogWarning("Missing animation: " + this.animName.Value);
				base.Finish();
				return;
			}
			float value = this.blendTime.Value;
			if (value < 0.001f)
			{
				base.animation.Play(this.animName.Value, this.playMode);
			}
			else
			{
				base.animation.CrossFade(this.animName.Value, value, this.playMode);
			}
			this.prevAnimtTime = this.anim.time;
		}

		// Token: 0x06003900 RID: 14592 RVA: 0x0012468C File Offset: 0x0012288C
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

		// Token: 0x06003901 RID: 14593 RVA: 0x00124770 File Offset: 0x00122970
		public override void OnExit()
		{
			if (this.stopOnExit)
			{
				this.StopAnimation();
			}
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x00124784 File Offset: 0x00122984
		private void StopAnimation()
		{
			if (base.animation != null)
			{
				base.animation.Stop(this.animName.Value);
			}
		}

		// Token: 0x04002B55 RID: 11093
		[Tooltip("Game Object to play the animation on.")]
		[CheckForComponent(typeof(Animation))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002B56 RID: 11094
		[Tooltip("The name of the animation to play.")]
		[UIHint(UIHint.Animation)]
		public FsmString animName;

		// Token: 0x04002B57 RID: 11095
		[Tooltip("How to treat previously playing animations.")]
		public PlayMode playMode;

		// Token: 0x04002B58 RID: 11096
		[HasFloatSlider(0f, 5f)]
		[Tooltip("Time taken to blend to this animation.")]
		public FsmFloat blendTime;

		// Token: 0x04002B59 RID: 11097
		[Tooltip("Event to send when the animation is finished playing. NOTE: Not sent with Loop or PingPong wrap modes!")]
		public FsmEvent finishEvent;

		// Token: 0x04002B5A RID: 11098
		[Tooltip("Event to send when the animation loops. If you want to send this event to another FSM use Set Event Target. NOTE: This event is only sent with Loop and PingPong wrap modes.")]
		public FsmEvent loopEvent;

		// Token: 0x04002B5B RID: 11099
		[Tooltip("Stop playing the animation when this state is exited.")]
		public bool stopOnExit;

		// Token: 0x04002B5C RID: 11100
		private AnimationState anim;

		// Token: 0x04002B5D RID: 11101
		private float prevAnimtTime;
	}
}
