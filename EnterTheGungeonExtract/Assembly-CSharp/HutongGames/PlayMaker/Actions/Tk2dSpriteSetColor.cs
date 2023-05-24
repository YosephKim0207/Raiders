using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C31 RID: 3121
	[ActionCategory("2D Toolkit/Sprite")]
	[Tooltip("Set the color of a sprite. \nNOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dAnimatedSprite)")]
	public class Tk2dSpriteSetColor : FsmStateAction
	{
		// Token: 0x06004341 RID: 17217 RVA: 0x0015CA20 File Offset: 0x0015AC20
		private void _getSprite()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._sprite = ownerDefaultTarget.GetComponent<tk2dBaseSprite>();
		}

		// Token: 0x06004342 RID: 17218 RVA: 0x0015CA58 File Offset: 0x0015AC58
		public override void Reset()
		{
			this.gameObject = null;
			this.color = null;
			this.everyframe = false;
		}

		// Token: 0x06004343 RID: 17219 RVA: 0x0015CA70 File Offset: 0x0015AC70
		public override void OnEnter()
		{
			this._getSprite();
			this.DoSetSpriteColor();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x06004344 RID: 17220 RVA: 0x0015CA90 File Offset: 0x0015AC90
		public override void OnUpdate()
		{
			this.DoSetSpriteColor();
		}

		// Token: 0x06004345 RID: 17221 RVA: 0x0015CA98 File Offset: 0x0015AC98
		private void DoSetSpriteColor()
		{
			if (this._sprite == null)
			{
				base.LogWarning("Missing tk2dBaseSprite component");
				return;
			}
			if (this._sprite.color != this.color.Value)
			{
				this._sprite.color = this.color.Value;
			}
		}

		// Token: 0x0400356E RID: 13678
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dAnimatedSprite).")]
		[RequiredField]
		[CheckForComponent(typeof(tk2dBaseSprite))]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400356F RID: 13679
		[Tooltip("The color")]
		[UIHint(UIHint.FsmColor)]
		public FsmColor color;

		// Token: 0x04003570 RID: 13680
		[ActionSection("")]
		[Tooltip("Repeat every frame.")]
		public bool everyframe;

		// Token: 0x04003571 RID: 13681
		private tk2dBaseSprite _sprite;
	}
}
