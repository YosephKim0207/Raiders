using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C2B RID: 3115
	[Tooltip("Receive animation events and animation complete event of the current animation playing. \nNOTE: The Game Object must have a tk2dSpriteAnimator attached.")]
	[ActionCategory("2D Toolkit/SpriteAnimator")]
	public class Tk2dWatchAnimationEvents : FsmStateAction
	{
		// Token: 0x06004320 RID: 17184 RVA: 0x0015C530 File Offset: 0x0015A730
		private void _getSprite()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._sprite = ownerDefaultTarget.GetComponent<tk2dSpriteAnimator>();
		}

		// Token: 0x06004321 RID: 17185 RVA: 0x0015C568 File Offset: 0x0015A768
		public override void Reset()
		{
			this.gameObject = null;
			this.animationTriggerEvent = null;
			this.animationCompleteEvent = null;
		}

		// Token: 0x06004322 RID: 17186 RVA: 0x0015C580 File Offset: 0x0015A780
		public override void OnEnter()
		{
			this._getSprite();
			this.DoWatchAnimationWithEvents();
		}

		// Token: 0x06004323 RID: 17187 RVA: 0x0015C590 File Offset: 0x0015A790
		private void DoWatchAnimationWithEvents()
		{
			if (this._sprite == null)
			{
				base.LogWarning("Missing tk2dSpriteAnimator component");
				return;
			}
			if (this.animationTriggerEvent != null)
			{
				this._sprite.AnimationEventTriggered = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.AnimationEventDelegate);
			}
			if (this.animationCompleteEvent != null)
			{
				this._sprite.AnimationCompleted = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.AnimationCompleteDelegate);
			}
		}

		// Token: 0x06004324 RID: 17188 RVA: 0x0015C600 File Offset: 0x0015A800
		private void AnimationEventDelegate(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip, int frameNum)
		{
			tk2dSpriteAnimationFrame frame = clip.GetFrame(frameNum);
			Fsm.EventData.IntData = frame.eventInt;
			Fsm.EventData.StringData = frame.eventInfo;
			Fsm.EventData.FloatData = frame.eventFloat;
			base.Fsm.Event(this.animationTriggerEvent);
		}

		// Token: 0x06004325 RID: 17189 RVA: 0x0015C658 File Offset: 0x0015A858
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

		// Token: 0x0400355A RID: 13658
		[CheckForComponent(typeof(tk2dSpriteAnimator))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dSpriteAnimator component attached.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400355B RID: 13659
		[Tooltip("Trigger event defined in the clip. The event holds the following triggers infos: the eventInt, eventInfo and eventFloat properties")]
		public FsmEvent animationTriggerEvent;

		// Token: 0x0400355C RID: 13660
		[Tooltip("Animation complete event. The event holds the clipId reference")]
		public FsmEvent animationCompleteEvent;

		// Token: 0x0400355D RID: 13661
		private tk2dSpriteAnimator _sprite;
	}
}
