using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C30 RID: 3120
	[Tooltip("Make a sprite pixelPerfect. \nNOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dAnimatedSprite)")]
	[ActionCategory("2D Toolkit/Sprite")]
	public class Tk2dSpriteMakePixelPerfect : FsmStateAction
	{
		// Token: 0x0600433C RID: 17212 RVA: 0x0015C994 File Offset: 0x0015AB94
		private void _getSprite()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._sprite = ownerDefaultTarget.GetComponent<tk2dBaseSprite>();
		}

		// Token: 0x0600433D RID: 17213 RVA: 0x0015C9CC File Offset: 0x0015ABCC
		public override void Reset()
		{
			this.gameObject = null;
		}

		// Token: 0x0600433E RID: 17214 RVA: 0x0015C9D8 File Offset: 0x0015ABD8
		public override void OnEnter()
		{
			this._getSprite();
			this.MakePixelPerfect();
			base.Finish();
		}

		// Token: 0x0600433F RID: 17215 RVA: 0x0015C9EC File Offset: 0x0015ABEC
		private void MakePixelPerfect()
		{
			if (this._sprite == null)
			{
				base.LogWarning("Missing tk2dBaseSprite component: ");
				return;
			}
			this._sprite.MakePixelPerfect();
		}

		// Token: 0x0400356C RID: 13676
		[RequiredField]
		[CheckForComponent(typeof(tk2dBaseSprite))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dAnimatedSprite)")]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400356D RID: 13677
		private tk2dBaseSprite _sprite;
	}
}
