using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C60 RID: 3168
	[ActionCategory(".Brave")]
	[Tooltip("Plays an animation on the specified object.")]
	public class PlayBraveAnimation : FsmStateAction
	{
		// Token: 0x06004431 RID: 17457 RVA: 0x00160484 File Offset: 0x0015E684
		public override void Reset()
		{
			this.GameObject = null;
			this.animName = string.Empty;
			this.mode = PlayBraveAnimation.PlayMode.UntilCancelled;
			this.nextAnimName = string.Empty;
			this.waitTime = 0f;
		}

		// Token: 0x06004432 RID: 17458 RVA: 0x001604C4 File Offset: 0x0015E6C4
		public override string ErrorCheck()
		{
			string text = string.Empty;
			GameObject gameObject = ((this.GameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.GameObject.GameObject.Value : base.Owner);
			if (gameObject)
			{
				tk2dSpriteAnimator component = gameObject.GetComponent<tk2dSpriteAnimator>();
				AIAnimator component2 = gameObject.GetComponent<AIAnimator>();
				if (!component && !component2)
				{
					return "Requires a 2D Toolkit animator or an AI Animator.\n";
				}
				if (component2)
				{
					if (!component2.HasDirectionalAnimation(this.animName.Value))
					{
						text = text + "Unknown animation " + this.animName.Value + ".\n";
					}
					if (this.UsesNextAnim && !component2.HasDirectionalAnimation(this.nextAnimName.Value))
					{
						text = text + "Unknown animation " + this.nextAnimName.Value + ".\n";
					}
				}
				else if (component)
				{
					if (component.GetClipByName(this.animName.Value) == null)
					{
						text = text + "Unknown animation " + this.animName.Value + ".\n";
					}
					if (this.UsesNextAnim && component.GetClipByName(this.nextAnimName.Value) == null)
					{
						text = text + "Unknown animation " + this.nextAnimName.Value + ".\n";
					}
				}
			}
			else if (!this.GameObject.GameObject.UseVariable)
			{
				return "No object specified";
			}
			return text;
		}

		// Token: 0x06004433 RID: 17459 RVA: 0x00160654 File Offset: 0x0015E854
		public override void OnEnter()
		{
			GameObject gameObject = base.Fsm.GetOwnerDefaultTarget(this.GameObject);
			if (this.playOnOtherTalkDoerInRoom.Value)
			{
				TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
				for (int i = 0; i < StaticReferenceManager.AllNpcs.Count; i++)
				{
					if (StaticReferenceManager.AllNpcs[i].ParentRoom == component.ParentRoom && StaticReferenceManager.AllNpcs[i] != component)
					{
						gameObject = StaticReferenceManager.AllNpcs[i].gameObject;
						break;
					}
				}
			}
			tk2dSpriteAnimator component2 = gameObject.GetComponent<tk2dSpriteAnimator>();
			AIAnimator component3 = gameObject.GetComponent<AIAnimator>();
			string text = ((!(component2 != null) || component2.CurrentClip == null) ? string.Empty : component2.CurrentClip.name);
			if (!this.dontPlayIfPlaying.Value || !(text == this.animName.Value))
			{
				if (this.mode == PlayBraveAnimation.PlayMode.UntilCancelled)
				{
					if (component3)
					{
						bool flag = true;
						if (component3.talkDoer && this.animName.Value == "idle" && component3.talkDoer.IsPlayingZombieAnimation)
						{
							flag = false;
						}
						if (flag)
						{
							component3.PlayUntilCancelled(this.animName.Value, false, null, -1f, false);
						}
					}
					else
					{
						component2.Play(this.animName.Value);
					}
				}
				else if (this.mode == PlayBraveAnimation.PlayMode.Duration)
				{
					if (component3)
					{
						component3.PlayForDuration(this.animName.Value, this.duration.Value, false, null, -1f, false);
					}
					else if (this.next == PlayBraveAnimation.NextMode.ReturnToPrevious)
					{
						component2.PlayForDuration(this.animName.Value, this.duration.Value);
					}
					else if (this.next == PlayBraveAnimation.NextMode.NewAnimation)
					{
						component2.PlayForDuration(this.animName.Value, this.duration.Value, this.nextAnimName.Value, false);
					}
				}
				else if (this.mode == PlayBraveAnimation.PlayMode.UntilFinished)
				{
					if (component3)
					{
						component3.PlayUntilFinished(this.animName.Value, false, null, -1f, false);
					}
					else if (this.next == PlayBraveAnimation.NextMode.ReturnToPrevious)
					{
						component2.PlayForDuration(this.animName.Value, -1f);
					}
					else if (this.next == PlayBraveAnimation.NextMode.NewAnimation)
					{
						component2.PlayForDuration(this.animName.Value, -1f, this.nextAnimName.Value, false);
					}
				}
			}
			if (this.waitTime.Value > 0f)
			{
				this.m_timer = this.waitTime.Value;
			}
			else
			{
				base.Finish();
			}
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x00160958 File Offset: 0x0015EB58
		public override void OnUpdate()
		{
			if (this.m_timer > 0f)
			{
				this.m_timer -= BraveTime.DeltaTime;
				if (this.m_timer <= 0f)
				{
					base.Finish();
				}
			}
		}

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x06004435 RID: 17461 RVA: 0x00160994 File Offset: 0x0015EB94
		private bool UsesNextAnim
		{
			get
			{
				return this.next == PlayBraveAnimation.NextMode.NewAnimation;
			}
		}

		// Token: 0x04003640 RID: 13888
		public FsmOwnerDefault GameObject;

		// Token: 0x04003641 RID: 13889
		[Tooltip("Name of the animation to play.")]
		public FsmString animName;

		// Token: 0x04003642 RID: 13890
		[Tooltip("How to play the animation.")]
		public PlayBraveAnimation.PlayMode mode;

		// Token: 0x04003643 RID: 13891
		[Tooltip("How long to play the animation for.")]
		public FsmFloat duration;

		// Token: 0x04003644 RID: 13892
		[Tooltip("If the animation is already playing, don't trigger it again.")]
		public FsmBool dontPlayIfPlaying;

		// Token: 0x04003645 RID: 13893
		[Tooltip("What animation to play next.")]
		public PlayBraveAnimation.NextMode next;

		// Token: 0x04003646 RID: 13894
		[Tooltip("The next animation to play (used only for UntilFinishedThenNext).")]
		public FsmString nextAnimName;

		// Token: 0x04003647 RID: 13895
		[Tooltip("Time to wait after the animation before continuing to the next action; 0 continues immediately.")]
		public FsmFloat waitTime;

		// Token: 0x04003648 RID: 13896
		public FsmBool playOnOtherTalkDoerInRoom;

		// Token: 0x04003649 RID: 13897
		private float m_timer;

		// Token: 0x02000C61 RID: 3169
		public enum PlayMode
		{
			// Token: 0x0400364B RID: 13899
			UntilCancelled,
			// Token: 0x0400364C RID: 13900
			Duration,
			// Token: 0x0400364D RID: 13901
			UntilFinished
		}

		// Token: 0x02000C62 RID: 3170
		public enum NextMode
		{
			// Token: 0x0400364F RID: 13903
			ReturnToPrevious,
			// Token: 0x04003650 RID: 13904
			NewAnimation
		}
	}
}
