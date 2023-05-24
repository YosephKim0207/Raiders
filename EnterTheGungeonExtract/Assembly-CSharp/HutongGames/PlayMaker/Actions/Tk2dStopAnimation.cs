using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C2A RID: 3114
	[ActionCategory("2D Toolkit/SpriteAnimator")]
	[Tooltip("Stops a sprite animation. \nNOTE: The Game Object must have a tk2dSpriteAnimator attached.")]
	public class Tk2dStopAnimation : FsmStateAction
	{
		// Token: 0x0600431B RID: 17179 RVA: 0x0015C490 File Offset: 0x0015A690
		private void _getSprite()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._sprite = ownerDefaultTarget.GetComponent<tk2dSpriteAnimator>();
		}

		// Token: 0x0600431C RID: 17180 RVA: 0x0015C4C8 File Offset: 0x0015A6C8
		public override void Reset()
		{
			this.gameObject = null;
		}

		// Token: 0x0600431D RID: 17181 RVA: 0x0015C4D4 File Offset: 0x0015A6D4
		public override void OnEnter()
		{
			this._getSprite();
			this.DoStopAnimation();
			base.Finish();
		}

		// Token: 0x0600431E RID: 17182 RVA: 0x0015C4E8 File Offset: 0x0015A6E8
		private void DoStopAnimation()
		{
			if (this._sprite == null)
			{
				base.LogWarning("Missing tk2dSpriteAnimator component: " + this._sprite.gameObject.name);
				return;
			}
			this._sprite.Stop();
		}

		// Token: 0x04003558 RID: 13656
		[CheckForComponent(typeof(tk2dSpriteAnimator))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dSpriteAnimator component attached.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04003559 RID: 13657
		private tk2dSpriteAnimator _sprite;
	}
}
