using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C28 RID: 3112
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W721")]
	[Tooltip("Resume a sprite animation. Use Tk2dPauseAnimation for dynamic control. \nNOTE: The Game Object must have a tk2dSpriteAnimator attached.")]
	[ActionCategory("2D Toolkit/SpriteAnimator")]
	public class Tk2dResumeAnimation : FsmStateAction
	{
		// Token: 0x06004310 RID: 17168 RVA: 0x0015C330 File Offset: 0x0015A530
		private void _getSprite()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._sprite = ownerDefaultTarget.GetComponent<tk2dSpriteAnimator>();
		}

		// Token: 0x06004311 RID: 17169 RVA: 0x0015C368 File Offset: 0x0015A568
		public override void Reset()
		{
			this.gameObject = null;
		}

		// Token: 0x06004312 RID: 17170 RVA: 0x0015C374 File Offset: 0x0015A574
		public override void OnEnter()
		{
			this._getSprite();
			this.DoResumeAnimation();
			base.Finish();
		}

		// Token: 0x06004313 RID: 17171 RVA: 0x0015C388 File Offset: 0x0015A588
		private void DoResumeAnimation()
		{
			if (this._sprite == null)
			{
				base.LogWarning("Missing tk2dSpriteAnimator component");
				return;
			}
			if (this._sprite.Paused)
			{
				this._sprite.Resume();
			}
		}

		// Token: 0x04003552 RID: 13650
		[CheckForComponent(typeof(tk2dSpriteAnimator))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dSpriteAnimator component attached.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04003553 RID: 13651
		private tk2dSpriteAnimator _sprite;
	}
}
