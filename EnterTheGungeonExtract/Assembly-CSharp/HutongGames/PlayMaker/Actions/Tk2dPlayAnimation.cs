using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C26 RID: 3110
	[ActionCategory("2D Toolkit/SpriteAnimator")]
	[Tooltip("Plays a sprite animation. \nNOTE: The Game Object must have a tk2dSpriteAnimator attached.")]
	public class Tk2dPlayAnimation : FsmStateAction
	{
		// Token: 0x06004304 RID: 17156 RVA: 0x0015C0A8 File Offset: 0x0015A2A8
		private void _getSprite()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._sprite = ownerDefaultTarget.GetComponent<tk2dSpriteAnimator>();
		}

		// Token: 0x06004305 RID: 17157 RVA: 0x0015C0E0 File Offset: 0x0015A2E0
		public override void Reset()
		{
			this.gameObject = null;
			this.animLibName = null;
			this.clipName = null;
		}

		// Token: 0x06004306 RID: 17158 RVA: 0x0015C0F8 File Offset: 0x0015A2F8
		public override void OnEnter()
		{
			this._getSprite();
			this.DoPlayAnimation();
		}

		// Token: 0x06004307 RID: 17159 RVA: 0x0015C108 File Offset: 0x0015A308
		private void DoPlayAnimation()
		{
			if (this._sprite == null)
			{
				base.LogWarning("Missing tk2dSpriteAnimator component");
				return;
			}
			if (!this.animLibName.Value.Equals(string.Empty))
			{
			}
			this._sprite.Play(this.clipName.Value);
		}

		// Token: 0x04003549 RID: 13641
		[CheckForComponent(typeof(tk2dSpriteAnimator))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dSpriteAnimator component attached.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400354A RID: 13642
		[Tooltip("The anim Lib name. Leave empty to use the one current selected")]
		public FsmString animLibName;

		// Token: 0x0400354B RID: 13643
		[Tooltip("The clip name to play")]
		[RequiredField]
		public FsmString clipName;

		// Token: 0x0400354C RID: 13644
		private tk2dSpriteAnimator _sprite;
	}
}
