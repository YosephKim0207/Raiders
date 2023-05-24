using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B04 RID: 2820
	[Tooltip("Sets a named float in a game object's material.")]
	[ActionCategory(ActionCategory.Material)]
	public class SetMaterialFloat : ComponentAction<Renderer>
	{
		// Token: 0x06003B82 RID: 15234 RVA: 0x0012C334 File Offset: 0x0012A534
		public override void Reset()
		{
			this.gameObject = null;
			this.materialIndex = 0;
			this.material = null;
			this.namedFloat = string.Empty;
			this.floatValue = 0f;
			this.everyFrame = false;
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x0012C384 File Offset: 0x0012A584
		public override void OnEnter()
		{
			this.DoSetMaterialFloat();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x0012C3A0 File Offset: 0x0012A5A0
		public override void OnUpdate()
		{
			this.DoSetMaterialFloat();
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x0012C3A8 File Offset: 0x0012A5A8
		private void DoSetMaterialFloat()
		{
			if (this.material.Value != null)
			{
				this.material.Value.SetFloat(this.namedFloat.Value, this.floatValue.Value);
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
				base.renderer.material.SetFloat(this.namedFloat.Value, this.floatValue.Value);
			}
			else if (base.renderer.materials.Length > this.materialIndex.Value)
			{
				Material[] materials = base.renderer.materials;
				materials[this.materialIndex.Value].SetFloat(this.namedFloat.Value, this.floatValue.Value);
				base.renderer.materials = materials;
			}
		}

		// Token: 0x04002DA2 RID: 11682
		[CheckForComponent(typeof(Renderer))]
		[Tooltip("The GameObject that the material is applied to.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002DA3 RID: 11683
		[Tooltip("GameObjects can have multiple materials. Specify an index to target a specific material.")]
		public FsmInt materialIndex;

		// Token: 0x04002DA4 RID: 11684
		[Tooltip("Alternatively specify a Material instead of a GameObject and Index.")]
		public FsmMaterial material;

		// Token: 0x04002DA5 RID: 11685
		[Tooltip("A named float parameter in the shader.")]
		[RequiredField]
		public FsmString namedFloat;

		// Token: 0x04002DA6 RID: 11686
		[Tooltip("Set the parameter value.")]
		[RequiredField]
		public FsmFloat floatValue;

		// Token: 0x04002DA7 RID: 11687
		[Tooltip("Repeat every frame. Useful if the value is animated.")]
		public bool everyFrame;
	}
}
