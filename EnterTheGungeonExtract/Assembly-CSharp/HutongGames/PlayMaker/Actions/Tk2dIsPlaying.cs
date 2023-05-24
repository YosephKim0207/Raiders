using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C24 RID: 3108
	[Tooltip("Check if a sprite animation is playing. \nNOTE: The Game Object must have a tk2dSpriteAnimator attached.")]
	[ActionCategory("2D Toolkit/SpriteAnimator")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W720")]
	public class Tk2dIsPlaying : FsmStateAction
	{
		// Token: 0x060042F8 RID: 17144 RVA: 0x0015BE7C File Offset: 0x0015A07C
		private void _getSprite()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._sprite = ownerDefaultTarget.GetComponent<tk2dSpriteAnimator>();
		}

		// Token: 0x060042F9 RID: 17145 RVA: 0x0015BEB4 File Offset: 0x0015A0B4
		public override void Reset()
		{
			this.gameObject = null;
			this.clipName = null;
			this.everyframe = false;
			this.isPlayingEvent = null;
			this.isNotPlayingEvent = null;
		}

		// Token: 0x060042FA RID: 17146 RVA: 0x0015BEDC File Offset: 0x0015A0DC
		public override void OnEnter()
		{
			this._getSprite();
			this.DoIsPlaying();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x060042FB RID: 17147 RVA: 0x0015BEFC File Offset: 0x0015A0FC
		public override void OnUpdate()
		{
			this.DoIsPlaying();
		}

		// Token: 0x060042FC RID: 17148 RVA: 0x0015BF04 File Offset: 0x0015A104
		private void DoIsPlaying()
		{
			if (this._sprite == null)
			{
				base.LogWarning("Missing tk2dSpriteAnimator component: " + this._sprite.gameObject.name);
				return;
			}
			bool flag = this._sprite.IsPlaying(this.clipName.Value);
			this.isPlaying.Value = flag;
			if (flag)
			{
				base.Fsm.Event(this.isPlayingEvent);
			}
			else
			{
				base.Fsm.Event(this.isNotPlayingEvent);
			}
		}

		// Token: 0x0400353E RID: 13630
		[CheckForComponent(typeof(tk2dSpriteAnimator))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dSpriteAnimator component attached.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400353F RID: 13631
		[Tooltip("The clip name to play")]
		[RequiredField]
		public FsmString clipName;

		// Token: 0x04003540 RID: 13632
		[UIHint(UIHint.Variable)]
		[Tooltip("is the clip playing?")]
		public FsmBool isPlaying;

		// Token: 0x04003541 RID: 13633
		[Tooltip("EVvnt sent if clip is playing")]
		public FsmEvent isPlayingEvent;

		// Token: 0x04003542 RID: 13634
		[Tooltip("Event sent if clip is not playing")]
		public FsmEvent isNotPlayingEvent;

		// Token: 0x04003543 RID: 13635
		[Tooltip("Repeat every frame.")]
		public bool everyframe;

		// Token: 0x04003544 RID: 13636
		private tk2dSpriteAnimator _sprite;
	}
}
