using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C33 RID: 3123
	[ActionCategory("2D Toolkit/Sprite")]
	[Tooltip("Set the pixel perfect flag of a sprite. \nNOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dAnimatedSprite)")]
	public class Tk2dSpriteSetPixelPerfect : FsmStateAction
	{
		// Token: 0x0600434C RID: 17228 RVA: 0x0015CC08 File Offset: 0x0015AE08
		private void _getSprite()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._sprite = ownerDefaultTarget.GetComponent<tk2dBaseSprite>();
		}

		// Token: 0x0600434D RID: 17229 RVA: 0x0015CC40 File Offset: 0x0015AE40
		public override void Reset()
		{
			this.gameObject = null;
			this.pixelPerfect = null;
			this.everyframe = false;
		}

		// Token: 0x0600434E RID: 17230 RVA: 0x0015CC58 File Offset: 0x0015AE58
		public override void OnEnter()
		{
			this._getSprite();
			this.DoSetSpritePixelPerfect();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x0600434F RID: 17231 RVA: 0x0015CC78 File Offset: 0x0015AE78
		public override void OnUpdate()
		{
			this.DoSetSpritePixelPerfect();
		}

		// Token: 0x06004350 RID: 17232 RVA: 0x0015CC80 File Offset: 0x0015AE80
		private void DoSetSpritePixelPerfect()
		{
			if (this._sprite == null)
			{
				base.LogWarning("Missing tk2dBaseSprite component");
				return;
			}
			if (this.pixelPerfect.Value)
			{
				this._sprite.MakePixelPerfect();
			}
		}

		// Token: 0x04003576 RID: 13686
		[CheckForComponent(typeof(tk2dBaseSprite))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dAnimatedSprite).")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04003577 RID: 13687
		[UIHint(UIHint.FsmBool)]
		[Tooltip("Does the sprite needs to be kept pixelPerfect? This is only necessary when using a perspective camera.")]
		public FsmBool pixelPerfect;

		// Token: 0x04003578 RID: 13688
		[Tooltip("Repeat every frame.")]
		[ActionSection("")]
		public bool everyframe;

		// Token: 0x04003579 RID: 13689
		private tk2dBaseSprite _sprite;
	}
}
