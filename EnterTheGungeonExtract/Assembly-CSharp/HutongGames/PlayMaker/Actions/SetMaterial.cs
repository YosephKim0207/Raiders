using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B02 RID: 2818
	[ActionCategory(ActionCategory.Material)]
	[Tooltip("Sets the material on a game object.")]
	public class SetMaterial : ComponentAction<Renderer>
	{
		// Token: 0x06003B79 RID: 15225 RVA: 0x0012C0A8 File Offset: 0x0012A2A8
		public override void Reset()
		{
			this.gameObject = null;
			this.material = null;
			this.materialIndex = 0;
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x0012C0C4 File Offset: 0x0012A2C4
		public override void OnEnter()
		{
			this.DoSetMaterial();
			base.Finish();
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x0012C0D4 File Offset: 0x0012A2D4
		private void DoSetMaterial()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			if (this.materialIndex.Value == 0)
			{
				base.renderer.material = this.material.Value;
			}
			else if (base.renderer.materials.Length > this.materialIndex.Value)
			{
				Material[] materials = base.renderer.materials;
				materials[this.materialIndex.Value] = this.material.Value;
				base.renderer.materials = materials;
			}
		}

		// Token: 0x04002D99 RID: 11673
		[CheckForComponent(typeof(Renderer))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D9A RID: 11674
		public FsmInt materialIndex;

		// Token: 0x04002D9B RID: 11675
		[RequiredField]
		public FsmMaterial material;
	}
}
