using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C25 RID: 3109
	[ActionCategory("2D Toolkit/SpriteAnimator")]
	[Tooltip("Pause a sprite animation. Can work everyframe to pause resume animation on the fly. \nNOTE: The Game Object must have a tk2dSpriteAnimator attached.")]
	[HelpUrl("https://hutonggames.fogbugz.com/default.asp?W720")]
	public class Tk2dPauseAnimation : FsmStateAction
	{
		// Token: 0x060042FE RID: 17150 RVA: 0x0015BF9C File Offset: 0x0015A19C
		private void _getSprite()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._sprite = ownerDefaultTarget.GetComponent<tk2dSpriteAnimator>();
		}

		// Token: 0x060042FF RID: 17151 RVA: 0x0015BFD4 File Offset: 0x0015A1D4
		public override void Reset()
		{
			this.gameObject = null;
			this.pause = true;
			this.everyframe = false;
		}

		// Token: 0x06004300 RID: 17152 RVA: 0x0015BFF0 File Offset: 0x0015A1F0
		public override void OnEnter()
		{
			this._getSprite();
			this.DoPauseAnimation();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x06004301 RID: 17153 RVA: 0x0015C010 File Offset: 0x0015A210
		public override void OnUpdate()
		{
			this.DoPauseAnimation();
		}

		// Token: 0x06004302 RID: 17154 RVA: 0x0015C018 File Offset: 0x0015A218
		private void DoPauseAnimation()
		{
			if (this._sprite == null)
			{
				base.LogWarning("Missing tk2dSpriteAnimator component: " + this._sprite.gameObject.name);
				return;
			}
			if (this._sprite.Paused != this.pause.Value)
			{
				if (this.pause.Value)
				{
					this._sprite.Pause();
				}
				else
				{
					this._sprite.Resume();
				}
			}
		}

		// Token: 0x04003545 RID: 13637
		[CheckForComponent(typeof(tk2dSpriteAnimator))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dSpriteAnimator component attached.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04003546 RID: 13638
		[Tooltip("Pause flag")]
		public FsmBool pause;

		// Token: 0x04003547 RID: 13639
		[Tooltip("Repeat every frame.")]
		[ActionSection("")]
		public bool everyframe;

		// Token: 0x04003548 RID: 13640
		private tk2dSpriteAnimator _sprite;
	}
}
