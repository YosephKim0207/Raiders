using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C27 RID: 3111
	[ActionCategory("2D Toolkit/SpriteAnimator")]
	[Tooltip("Plays a sprite animation. \nCan receive animation events and animation complete event. \nNOTE: The Game Object must have a tk2dSpriteAnimator attached.")]
	public class Tk2dPlayAnimationWithEvents : FsmStateAction
	{
		// Token: 0x06004309 RID: 17161 RVA: 0x0015C16C File Offset: 0x0015A36C
		private void _getSprite()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._sprite = ownerDefaultTarget.GetComponent<tk2dSpriteAnimator>();
		}

		// Token: 0x0600430A RID: 17162 RVA: 0x0015C1A4 File Offset: 0x0015A3A4
		public override void Reset()
		{
			this.gameObject = null;
			this.clipName = null;
			this.animationTriggerEvent = null;
			this.animationCompleteEvent = null;
		}

		// Token: 0x0600430B RID: 17163 RVA: 0x0015C1C4 File Offset: 0x0015A3C4
		public override void OnEnter()
		{
			this._getSprite();
			this.DoPlayAnimationWithEvents();
		}

		// Token: 0x0600430C RID: 17164 RVA: 0x0015C1D4 File Offset: 0x0015A3D4
		private void DoPlayAnimationWithEvents()
		{
			if (this._sprite == null)
			{
				base.LogWarning("Missing tk2dSpriteAnimator component");
				return;
			}
			this._sprite.Play(this.clipName.Value);
			if (this.animationTriggerEvent != null)
			{
				this._sprite.AnimationEventTriggered = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventDelegate);
			}
			if (this.animationCompleteEvent != null)
			{
				this._sprite.AnimationCompleted = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.AnimationCompleteDelegate);
			}
		}

		// Token: 0x0600430D RID: 17165 RVA: 0x0015C258 File Offset: 0x0015A458
		private void AnimationEventDelegate(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip, int frameNum)
		{
			tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNum);
			Fsm.EventData.IntData = frame.eventInt;
			Fsm.EventData.StringData = frame.eventInfo;
			Fsm.EventData.FloatData = frame.eventFloat;
			base.Fsm.Event(this.animationTriggerEvent);
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x0015C2B0 File Offset: 0x0015A4B0
		private void AnimationCompleteDelegate(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip)
		{
			int num = -1;
			tk2dSpriteAnimationClip[] array = ((!(sprite.Library != null)) ? null : sprite.Library.clips);
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == clip)
					{
						num = i;
						break;
					}
				}
			}
			Fsm.EventData.IntData = num;
			base.Fsm.Event(this.animationCompleteEvent);
		}

		// Token: 0x0400354D RID: 13645
		[CheckForComponent(typeof(tk2dSpriteAnimator))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dSpriteAnimator component attached.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400354E RID: 13646
		[Tooltip("The clip name to play")]
		[RequiredField]
		public FsmString clipName;

		// Token: 0x0400354F RID: 13647
		[Tooltip("Trigger event defined in the clip. The event holds the following triggers infos: the eventInt, eventInfo and eventFloat properties")]
		public FsmEvent animationTriggerEvent;

		// Token: 0x04003550 RID: 13648
		[Tooltip("Animation complete event. The event holds the clipId reference")]
		public FsmEvent animationCompleteEvent;

		// Token: 0x04003551 RID: 13649
		private tk2dSpriteAnimator _sprite;
	}
}
