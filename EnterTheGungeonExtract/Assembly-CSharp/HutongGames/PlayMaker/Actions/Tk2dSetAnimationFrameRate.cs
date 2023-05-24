using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C29 RID: 3113
	[ActionCategory("2D Toolkit/SpriteAnimator")]
	[Tooltip("Set the current clip frames per seconds on a animated sprite. \nNOTE: The Game Object must have a tk2dSpriteAnimator attached.")]
	public class Tk2dSetAnimationFrameRate : FsmStateAction
	{
		// Token: 0x06004315 RID: 17173 RVA: 0x0015C3CC File Offset: 0x0015A5CC
		private void _getSprite()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._sprite = ownerDefaultTarget.GetComponent<tk2dSpriteAnimator>();
		}

		// Token: 0x06004316 RID: 17174 RVA: 0x0015C404 File Offset: 0x0015A604
		public override void Reset()
		{
			this.gameObject = null;
			this.framePerSeconds = 30f;
			this.everyFrame = false;
		}

		// Token: 0x06004317 RID: 17175 RVA: 0x0015C424 File Offset: 0x0015A624
		public override void OnEnter()
		{
			this._getSprite();
			this.DoSetAnimationFPS();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06004318 RID: 17176 RVA: 0x0015C444 File Offset: 0x0015A644
		public override void OnUpdate()
		{
			this.DoSetAnimationFPS();
		}

		// Token: 0x06004319 RID: 17177 RVA: 0x0015C44C File Offset: 0x0015A64C
		private void DoSetAnimationFPS()
		{
			if (this._sprite == null)
			{
				base.LogWarning("Missing tk2dSpriteAnimator component");
				return;
			}
			this._sprite.CurrentClip.fps = this.framePerSeconds.Value;
		}

		// Token: 0x04003554 RID: 13652
		[CheckForComponent(typeof(tk2dSpriteAnimator))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dSpriteAnimator component attached.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04003555 RID: 13653
		[Tooltip("The frame per seconds of the current clip")]
		[RequiredField]
		public FsmFloat framePerSeconds;

		// Token: 0x04003556 RID: 13654
		[Tooltip("Repeat every Frame")]
		public bool everyFrame;

		// Token: 0x04003557 RID: 13655
		private tk2dSpriteAnimator _sprite;
	}
}
