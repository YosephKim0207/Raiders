using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C34 RID: 3124
	[ActionCategory("2D Toolkit/Sprite")]
	[Tooltip("Set the scale of a sprite. \nNOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dAnimatedSprite)")]
	public class Tk2dSpriteSetScale : FsmStateAction
	{
		// Token: 0x06004352 RID: 17234 RVA: 0x0015CCC4 File Offset: 0x0015AEC4
		private void _getSprite()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this._sprite = ownerDefaultTarget.GetComponent<tk2dBaseSprite>();
		}

		// Token: 0x06004353 RID: 17235 RVA: 0x0015CCFC File Offset: 0x0015AEFC
		public override void Reset()
		{
			this.gameObject = null;
			this.scale = new Vector3(1f, 1f, 1f);
			this.everyframe = false;
		}

		// Token: 0x06004354 RID: 17236 RVA: 0x0015CD2C File Offset: 0x0015AF2C
		public override void OnEnter()
		{
			this._getSprite();
			this.DoSetSpriteScale();
			if (!this.everyframe)
			{
				base.Finish();
			}
		}

		// Token: 0x06004355 RID: 17237 RVA: 0x0015CD4C File Offset: 0x0015AF4C
		public override void OnUpdate()
		{
			this.DoSetSpriteScale();
		}

		// Token: 0x06004356 RID: 17238 RVA: 0x0015CD54 File Offset: 0x0015AF54
		private void DoSetSpriteScale()
		{
			if (this._sprite == null)
			{
				base.LogWarning("Missing tk2dBaseSprite component");
				return;
			}
			if (this._sprite.scale != this.scale.Value)
			{
				this._sprite.scale = this.scale.Value;
			}
		}

		// Token: 0x0400357A RID: 13690
		[CheckForComponent(typeof(tk2dBaseSprite))]
		[Tooltip("The Game Object to work with. NOTE: The Game Object must have a tk2dBaseSprite or derived component attached ( tk2dSprite, tk2dAnimatedSprite).")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400357B RID: 13691
		[Tooltip("The scale Id")]
		[UIHint(UIHint.FsmVector3)]
		public FsmVector3 scale;

		// Token: 0x0400357C RID: 13692
		[ActionSection("")]
		[Tooltip("Repeat every frame.")]
		public bool everyframe;

		// Token: 0x0400357D RID: 13693
		private tk2dBaseSprite _sprite;
	}
}
