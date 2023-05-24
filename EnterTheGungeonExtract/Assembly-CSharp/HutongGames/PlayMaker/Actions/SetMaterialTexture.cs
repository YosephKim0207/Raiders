using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B06 RID: 2822
	[ActionCategory(ActionCategory.Material)]
	[Tooltip("Sets a named texture in a game object's material.")]
	public class SetMaterialTexture : ComponentAction<Renderer>
	{
		// Token: 0x06003B8B RID: 15243 RVA: 0x0012C63C File Offset: 0x0012A83C
		public override void Reset()
		{
			this.gameObject = null;
			this.materialIndex = 0;
			this.material = null;
			this.namedTexture = "_MainTex";
			this.texture = null;
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x0012C670 File Offset: 0x0012A870
		public override void OnEnter()
		{
			this.DoSetMaterialTexture();
			base.Finish();
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x0012C680 File Offset: 0x0012A880
		private void DoSetMaterialTexture()
		{
			string text = this.namedTexture.Value;
			if (text == string.Empty)
			{
				text = "_MainTex";
			}
			if (this.material.Value != null)
			{
				this.material.Value.SetTexture(text, this.texture.Value);
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			if (base.renderer.material == null)
			{
				base.LogError("Missing Material!");
				return;
			}
			if (this.materialIndex.Value == 0)
			{
				base.renderer.material.SetTexture(text, this.texture.Value);
			}
			else if (base.renderer.materials.Length > this.materialIndex.Value)
			{
				Material[] materials = base.renderer.materials;
				materials[this.materialIndex.Value].SetTexture(text, this.texture.Value);
				base.renderer.materials = materials;
			}
		}

		// Token: 0x04002DAD RID: 11693
		[CheckForComponent(typeof(Renderer))]
		[Tooltip("The GameObject that the material is applied to.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002DAE RID: 11694
		[Tooltip("GameObjects can have multiple materials. Specify an index to target a specific material.")]
		public FsmInt materialIndex;

		// Token: 0x04002DAF RID: 11695
		[Tooltip("Alternatively specify a Material instead of a GameObject and Index.")]
		public FsmMaterial material;

		// Token: 0x04002DB0 RID: 11696
		[Tooltip("A named parameter in the shader.")]
		[UIHint(UIHint.NamedTexture)]
		public FsmString namedTexture;

		// Token: 0x04002DB1 RID: 11697
		public FsmTexture texture;
	}
}
