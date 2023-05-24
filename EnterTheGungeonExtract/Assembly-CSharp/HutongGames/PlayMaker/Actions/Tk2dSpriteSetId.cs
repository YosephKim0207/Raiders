using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C32 RID: 3122
	[ActionCategory("2D Toolkit/Sprite")]
	[Tooltip("Set the sprite id of a sprite. Can use id or name. \nNOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dAnimatedSprite)")]
	public class Tk2dSpriteSetId : FsmStateAction
	{
		// Token: 0x06004347 RID: 17223 RVA: 0x0015CB00 File Offset: 0x0015AD00
		private void _getSprite()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._sprite = ownerDefaultTarget.GetComponent<tk2dBaseSprite>();
		}

		// Token: 0x06004348 RID: 17224 RVA: 0x0015CB38 File Offset: 0x0015AD38
		public override void Reset()
		{
			this.gameObject = null;
			this.spriteID = null;
			this.ORSpriteName = null;
		}

		// Token: 0x06004349 RID: 17225 RVA: 0x0015CB50 File Offset: 0x0015AD50
		public override void OnEnter()
		{
			this._getSprite();
			this.DoSetSpriteID();
			base.Finish();
		}

		// Token: 0x0600434A RID: 17226 RVA: 0x0015CB64 File Offset: 0x0015AD64
		private void DoSetSpriteID()
		{
			if (this._sprite == null)
			{
				base.LogWarning("Missing tk2dBaseSprite component: " + this._sprite.gameObject.name);
				return;
			}
			int num = this.spriteID.Value;
			if (this.ORSpriteName.Value != string.Empty)
			{
				num = this._sprite.GetSpriteIdByName(this.ORSpriteName.Value);
			}
			if (num != this._sprite.spriteId)
			{
				this._sprite.spriteId = num;
			}
		}

		// Token: 0x04003572 RID: 13682
		[CheckForComponent(typeof(tk2dBaseSprite))]
		[RequiredField]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dAnimatedSprite).")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04003573 RID: 13683
		[UIHint(UIHint.FsmInt)]
		[Tooltip("The sprite Id")]
		public FsmInt spriteID;

		// Token: 0x04003574 RID: 13684
		[Tooltip("OR The sprite name ")]
		[UIHint(UIHint.FsmString)]
		public FsmString ORSpriteName;

		// Token: 0x04003575 RID: 13685
		private tk2dBaseSprite _sprite;
	}
}
