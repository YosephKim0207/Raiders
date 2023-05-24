using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000996 RID: 2454
	[Tooltip("Get a texture from a material on a GameObject")]
	[ActionCategory(ActionCategory.Material)]
	public class GetMaterialTexture : ComponentAction<Renderer>
	{
		// Token: 0x06003547 RID: 13639 RVA: 0x00112D24 File Offset: 0x00110F24
		public override void Reset()
		{
			this.gameObject = null;
			this.materialIndex = 0;
			this.namedTexture = "_MainTex";
			this.storedTexture = null;
			this.getFromSharedMaterial = false;
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x00112D58 File Offset: 0x00110F58
		public override void OnEnter()
		{
			this.DoGetMaterialTexture();
			base.Finish();
		}

		// Token: 0x06003549 RID: 13641 RVA: 0x00112D68 File Offset: 0x00110F68
		private void DoGetMaterialTexture()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			string text = this.namedTexture.Value;
			if (text == string.Empty)
			{
				text = "_MainTex";
			}
			if (this.materialIndex.Value == 0 && !this.getFromSharedMaterial)
			{
				this.storedTexture.Value = base.renderer.material.GetTexture(text);
			}
			else if (this.materialIndex.Value == 0 && this.getFromSharedMaterial)
			{
				this.storedTexture.Value = base.renderer.sharedMaterial.GetTexture(text);
			}
			else if (base.renderer.materials.Length > this.materialIndex.Value && !this.getFromSharedMaterial)
			{
				Material[] materials = base.renderer.materials;
				this.storedTexture.Value = base.renderer.materials[this.materialIndex.Value].GetTexture(text);
				base.renderer.materials = materials;
			}
			else if (base.renderer.materials.Length > this.materialIndex.Value && this.getFromSharedMaterial)
			{
				Material[] sharedMaterials = base.renderer.sharedMaterials;
				this.storedTexture.Value = base.renderer.sharedMaterials[this.materialIndex.Value].GetTexture(text);
				base.renderer.materials = sharedMaterials;
			}
		}

		// Token: 0x040026A8 RID: 9896
		[Tooltip("The GameObject the Material is applied to.")]
		[RequiredField]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;

		// Token: 0x040026A9 RID: 9897
		[Tooltip("The index of the Material in the Materials array.")]
		public FsmInt materialIndex;

		// Token: 0x040026AA RID: 9898
		[UIHint(UIHint.NamedTexture)]
		[Tooltip("The texture to get. See Unity Shader docs for names.")]
		public FsmString namedTexture;

		// Token: 0x040026AB RID: 9899
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Title("StoreTexture")]
		[Tooltip("Store the texture in a variable.")]
		public FsmTexture storedTexture;

		// Token: 0x040026AC RID: 9900
		[Tooltip("Get the shared version of the texture.")]
		public bool getFromSharedMaterial;
	}
}
