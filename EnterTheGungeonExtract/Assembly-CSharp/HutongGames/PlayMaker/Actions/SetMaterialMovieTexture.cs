using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B05 RID: 2821
	[Tooltip("Sets a named texture in a game object's material to a movie texture.")]
	[ActionCategory(ActionCategory.Material)]
	public class SetMaterialMovieTexture : ComponentAction<Renderer>
	{
		// Token: 0x06003B87 RID: 15239 RVA: 0x0012C4D4 File Offset: 0x0012A6D4
		public override void Reset()
		{
			this.gameObject = null;
			this.materialIndex = 0;
			this.material = null;
			this.namedTexture = "_MainTex";
			this.movieTexture = null;
		}

		// Token: 0x06003B88 RID: 15240 RVA: 0x0012C508 File Offset: 0x0012A708
		public override void OnEnter()
		{
			this.DoSetMaterialTexture();
			base.Finish();
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x0012C518 File Offset: 0x0012A718
		private void DoSetMaterialTexture()
		{
			MovieTexture movieTexture = this.movieTexture.Value as MovieTexture;
			string text = this.namedTexture.Value;
			if (text == string.Empty)
			{
				text = "_MainTex";
			}
			if (this.material.Value != null)
			{
				this.material.Value.SetTexture(text, movieTexture);
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
				base.renderer.material.SetTexture(text, movieTexture);
			}
			else if (base.renderer.materials.Length > this.materialIndex.Value)
			{
				Material[] materials = base.renderer.materials;
				materials[this.materialIndex.Value].SetTexture(text, movieTexture);
				base.renderer.materials = materials;
			}
		}

		// Token: 0x04002DA8 RID: 11688
		[CheckForComponent(typeof(Renderer))]
		[Tooltip("The GameObject that the material is applied to.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002DA9 RID: 11689
		[Tooltip("GameObjects can have multiple materials. Specify an index to target a specific material.")]
		public FsmInt materialIndex;

		// Token: 0x04002DAA RID: 11690
		[Tooltip("Alternatively specify a Material instead of a GameObject and Index.")]
		public FsmMaterial material;

		// Token: 0x04002DAB RID: 11691
		[Tooltip("A named texture in the shader.")]
		[UIHint(UIHint.NamedTexture)]
		public FsmString namedTexture;

		// Token: 0x04002DAC RID: 11692
		[ObjectType(typeof(MovieTexture))]
		[RequiredField]
		public FsmObject movieTexture;
	}
}
