using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C2F RID: 3119
	[Tooltip("Get the scale of a sprite. \nNOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dAnimatedSprite)")]
	[ActionCategory("2D Toolkit/Sprite")]
	public class Tk2dSpriteGetScale : FsmStateAction
	{
		// Token: 0x06004336 RID: 17206 RVA: 0x0015C8B4 File Offset: 0x0015AAB4
		private void _getSprite()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._sprite = ownerDefaultTarget.GetComponent<tk2dBaseSprite>();
		}

		// Token: 0x06004337 RID: 17207 RVA: 0x0015C8EC File Offset: 0x0015AAEC
		public override void Reset()
		{
			this.gameObject = null;
			this.scale = null;
			this.everyframe = false;
		}

		// Token: 0x06004338 RID: 17208 RVA: 0x0015C904 File Offset: 0x0015AB04
		public override void OnEnter()
		{
			this._getSprite();
			this.DoGetSpriteScale();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x06004339 RID: 17209 RVA: 0x0015C924 File Offset: 0x0015AB24
		public override void OnUpdate()
		{
			this.DoGetSpriteScale();
		}

		// Token: 0x0600433A RID: 17210 RVA: 0x0015C92C File Offset: 0x0015AB2C
		private void DoGetSpriteScale()
		{
			if (this._sprite == null)
			{
				base.LogWarning("Missing tk2dBaseSprite component");
				return;
			}
			if (this._sprite.scale != this.scale.Value)
			{
				this.scale.Value = this._sprite.scale;
			}
		}

		// Token: 0x04003568 RID: 13672
		[CheckForComponent(typeof(tk2dBaseSprite))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dAnimatedSprite).")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04003569 RID: 13673
		[UIHint(UIHint.Variable)]
		[Tooltip("The scale Id")]
		public FsmVector3 scale;

		// Token: 0x0400356A RID: 13674
		[Tooltip("Repeat every frame.")]
		[ActionSection("")]
		public bool everyframe;

		// Token: 0x0400356B RID: 13675
		private tk2dBaseSprite _sprite;
	}
}
