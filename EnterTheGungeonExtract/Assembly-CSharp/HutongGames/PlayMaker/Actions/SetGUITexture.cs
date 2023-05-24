using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AF0 RID: 2800
	[ActionCategory(ActionCategory.GUIElement)]
	[Tooltip("Sets the Texture used by the GUITexture attached to a Game Object.")]
	public class SetGUITexture : ComponentAction<GUITexture>
	{
		// Token: 0x06003B2D RID: 15149 RVA: 0x0012B7FC File Offset: 0x001299FC
		public override void Reset()
		{
			this.gameObject = null;
			this.texture = null;
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x0012B80C File Offset: 0x00129A0C
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.guiTexture.texture = this.texture.Value;
			}
			base.Finish();
		}

		// Token: 0x04002D6E RID: 11630
		[RequiredField]
		[Tooltip("The GameObject that owns the GUITexture.")]
		[CheckForComponent(typeof(GUITexture))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D6F RID: 11631
		[Tooltip("Texture to apply.")]
		public FsmTexture texture;
	}
}
