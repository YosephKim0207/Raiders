using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AF2 RID: 2802
	[ActionCategory(ActionCategory.GUIElement)]
	[Tooltip("Sets the Color of the GUITexture attached to a Game Object.")]
	public class SetGUITextureColor : ComponentAction<GUITexture>
	{
		// Token: 0x06003B35 RID: 15157 RVA: 0x0012B910 File Offset: 0x00129B10
		public override void Reset()
		{
			this.gameObject = null;
			this.color = Color.white;
			this.everyFrame = false;
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x0012B930 File Offset: 0x00129B30
		public override void OnEnter()
		{
			this.DoSetGUITextureColor();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x0012B94C File Offset: 0x00129B4C
		public override void OnUpdate()
		{
			this.DoSetGUITextureColor();
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x0012B954 File Offset: 0x00129B54
		private void DoSetGUITextureColor()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.guiTexture.color = this.color.Value;
			}
		}

		// Token: 0x04002D73 RID: 11635
		[RequiredField]
		[CheckForComponent(typeof(GUITexture))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D74 RID: 11636
		[RequiredField]
		public FsmColor color;

		// Token: 0x04002D75 RID: 11637
		public bool everyFrame;
	}
}
