using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C2D RID: 3117
	[Tooltip("Get the sprite id of a sprite. \nNOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dAnimatedSprite).")]
	[ActionCategory("2D Toolkit/Sprite")]
	public class Tk2dSpriteGetId : FsmStateAction
	{
		// Token: 0x0600432D RID: 17197 RVA: 0x0015C7B8 File Offset: 0x0015A9B8
		private void _getSprite()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._sprite = ownerDefaultTarget.GetComponent<tk2dBaseSprite>();
		}

		// Token: 0x0600432E RID: 17198 RVA: 0x0015C7F0 File Offset: 0x0015A9F0
		public override void Reset()
		{
			this.gameObject = null;
			this.spriteID = null;
			this.everyframe = false;
		}

		// Token: 0x0600432F RID: 17199 RVA: 0x0015C808 File Offset: 0x0015AA08
		public override void OnEnter()
		{
			this._getSprite();
			this.DoGetSpriteID();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x06004330 RID: 17200 RVA: 0x0015C828 File Offset: 0x0015AA28
		public override void OnUpdate()
		{
			this.DoGetSpriteID();
		}

		// Token: 0x06004331 RID: 17201 RVA: 0x0015C830 File Offset: 0x0015AA30
		private void DoGetSpriteID()
		{
			if (this._sprite == null)
			{
				base.LogWarning("Missing tk2dBaseSprite component");
				return;
			}
			if (this.spriteID.Value != this._sprite.spriteId)
			{
				this.spriteID.Value = this._sprite.spriteId;
			}
		}

		// Token: 0x04003562 RID: 13666
		[CheckForComponent(typeof(tk2dBaseSprite))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dAnimatedSprite)")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04003563 RID: 13667
		[UIHint(UIHint.FsmInt)]
		[Tooltip("The sprite Id")]
		public FsmInt spriteID;

		// Token: 0x04003564 RID: 13668
		[Tooltip("Repeat every frame.")]
		[ActionSection("")]
		public bool everyframe;

		// Token: 0x04003565 RID: 13669
		private tk2dBaseSprite _sprite;
	}
}
