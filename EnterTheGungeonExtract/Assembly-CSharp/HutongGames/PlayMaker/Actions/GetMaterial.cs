using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000995 RID: 2453
	[Tooltip("Get a material at index on a gameObject and store it in a variable")]
	[ActionCategory(ActionCategory.Material)]
	public class GetMaterial : ComponentAction<Renderer>
	{
		// Token: 0x06003543 RID: 13635 RVA: 0x00112B98 File Offset: 0x00110D98
		public override void Reset()
		{
			this.gameObject = null;
			this.material = null;
			this.materialIndex = 0;
			this.getSharedMaterial = false;
		}

		// Token: 0x06003544 RID: 13636 RVA: 0x00112BBC File Offset: 0x00110DBC
		public override void OnEnter()
		{
			this.DoGetMaterial();
			base.Finish();
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x00112BCC File Offset: 0x00110DCC
		private void DoGetMaterial()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			if (this.materialIndex.Value == 0 && !this.getSharedMaterial)
			{
				this.material.Value = base.renderer.material;
			}
			else if (this.materialIndex.Value == 0 && this.getSharedMaterial)
			{
				this.material.Value = base.renderer.sharedMaterial;
			}
			else if (base.renderer.materials.Length > this.materialIndex.Value && !this.getSharedMaterial)
			{
				Material[] materials = base.renderer.materials;
				this.material.Value = materials[this.materialIndex.Value];
				base.renderer.materials = materials;
			}
			else if (base.renderer.materials.Length > this.materialIndex.Value && this.getSharedMaterial)
			{
				Material[] sharedMaterials = base.renderer.sharedMaterials;
				this.material.Value = sharedMaterials[this.materialIndex.Value];
				base.renderer.sharedMaterials = sharedMaterials;
			}
		}

		// Token: 0x040026A4 RID: 9892
		[Tooltip("The GameObject the Material is applied to.")]
		[RequiredField]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;

		// Token: 0x040026A5 RID: 9893
		[Tooltip("The index of the Material in the Materials array.")]
		public FsmInt materialIndex;

		// Token: 0x040026A6 RID: 9894
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the material in a variable.")]
		public FsmMaterial material;

		// Token: 0x040026A7 RID: 9895
		[Tooltip("Get the shared material of this object. NOTE: Modifying the shared material will change the appearance of all objects using this material, and change material settings that are stored in the project too.")]
		public bool getSharedMaterial;
	}
}
