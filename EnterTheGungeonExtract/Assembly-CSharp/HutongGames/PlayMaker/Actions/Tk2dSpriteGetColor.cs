using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C2C RID: 3116
	[Tooltip("Get the color of a sprite. \nNOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dAnimatedSprite)")]
	[ActionCategory("2D Toolkit/Sprite")]
	public class Tk2dSpriteGetColor : FsmStateAction
	{
		// Token: 0x06004327 RID: 17191 RVA: 0x0015C6D8 File Offset: 0x0015A8D8
		private void _getSprite()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._sprite = ownerDefaultTarget.GetComponent<tk2dBaseSprite>();
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x0015C710 File Offset: 0x0015A910
		public override void Reset()
		{
			this.gameObject = null;
			this.color = null;
			this.everyframe = false;
		}

		// Token: 0x06004329 RID: 17193 RVA: 0x0015C728 File Offset: 0x0015A928
		public override void OnEnter()
		{
			this._getSprite();
			this.DoGetSpriteColor();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x0600432A RID: 17194 RVA: 0x0015C748 File Offset: 0x0015A948
		public override void OnUpdate()
		{
			this.DoGetSpriteColor();
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x0015C750 File Offset: 0x0015A950
		private void DoGetSpriteColor()
		{
			if (this._sprite == null)
			{
				base.LogWarning("Missing tk2dBaseSprite component");
				return;
			}
			if (this._sprite.color != this.color.Value)
			{
				this.color.Value = this._sprite.color;
			}
		}

		// Token: 0x0400355E RID: 13662
		[CheckForComponent(typeof(tk2dBaseSprite))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dAnimatedSprite).")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400355F RID: 13663
		[UIHint(UIHint.Variable)]
		[Tooltip("The color")]
		public FsmColor color;

		// Token: 0x04003560 RID: 13664
		[Tooltip("Repeat every frame.")]
		[ActionSection("")]
		public bool everyframe;

		// Token: 0x04003561 RID: 13665
		private tk2dBaseSprite _sprite;
	}
}
