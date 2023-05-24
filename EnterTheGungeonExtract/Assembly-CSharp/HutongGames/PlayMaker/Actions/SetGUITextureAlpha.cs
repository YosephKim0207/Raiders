using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AF1 RID: 2801
	[ActionCategory(ActionCategory.GUIElement)]
	[Tooltip("Sets the Alpha of the GUITexture attached to a Game Object. Useful for fading GUI elements in/out.")]
	public class SetGUITextureAlpha : ComponentAction<GUITexture>
	{
		// Token: 0x06003B30 RID: 15152 RVA: 0x0012B85C File Offset: 0x00129A5C
		public override void Reset()
		{
			this.gameObject = null;
			this.alpha = 1f;
			this.everyFrame = false;
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x0012B87C File Offset: 0x00129A7C
		public override void OnEnter()
		{
			this.DoGUITextureAlpha();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x0012B898 File Offset: 0x00129A98
		public override void OnUpdate()
		{
			this.DoGUITextureAlpha();
		}

		// Token: 0x06003B33 RID: 15155 RVA: 0x0012B8A0 File Offset: 0x00129AA0
		private void DoGUITextureAlpha()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				Color color = base.guiTexture.color;
				base.guiTexture.color = new Color(color.r, color.g, color.b, this.alpha.Value);
			}
		}

		// Token: 0x04002D70 RID: 11632
		[CheckForComponent(typeof(GUITexture))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D71 RID: 11633
		[RequiredField]
		public FsmFloat alpha;

		// Token: 0x04002D72 RID: 11634
		public bool everyFrame;
	}
}
