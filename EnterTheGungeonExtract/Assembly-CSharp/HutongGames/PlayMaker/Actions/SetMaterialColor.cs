using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B03 RID: 2819
	[Tooltip("Sets a named color value in a game object's material.")]
	[ActionCategory(ActionCategory.Material)]
	public class SetMaterialColor : ComponentAction<Renderer>
	{
		// Token: 0x06003B7D RID: 15229 RVA: 0x0012C180 File Offset: 0x0012A380
		public override void Reset()
		{
			this.gameObject = null;
			this.materialIndex = 0;
			this.material = null;
			this.namedColor = "_Color";
			this.color = Color.black;
			this.everyFrame = false;
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x0012C1D0 File Offset: 0x0012A3D0
		public override void OnEnter()
		{
			this.DoSetMaterialColor();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x0012C1EC File Offset: 0x0012A3EC
		public override void OnUpdate()
		{
			this.DoSetMaterialColor();
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x0012C1F4 File Offset: 0x0012A3F4
		private void DoSetMaterialColor()
		{
			if (this.color.IsNone)
			{
				return;
			}
			string text = this.namedColor.Value;
			if (text == string.Empty)
			{
				text = "_Color";
			}
			if (this.material.Value != null)
			{
				this.material.Value.SetColor(text, this.color.Value);
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
				base.renderer.material.SetColor(text, this.color.Value);
			}
			else if (base.renderer.materials.Length > this.materialIndex.Value)
			{
				Material[] materials = base.renderer.materials;
				materials[this.materialIndex.Value].SetColor(text, this.color.Value);
				base.renderer.materials = materials;
			}
		}

		// Token: 0x04002D9C RID: 11676
		[CheckForComponent(typeof(Renderer))]
		[Tooltip("The GameObject that the material is applied to.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D9D RID: 11677
		[Tooltip("GameObjects can have multiple materials. Specify an index to target a specific material.")]
		public FsmInt materialIndex;

		// Token: 0x04002D9E RID: 11678
		[Tooltip("Alternatively specify a Material instead of a GameObject and Index.")]
		public FsmMaterial material;

		// Token: 0x04002D9F RID: 11679
		[Tooltip("A named color parameter in the shader.")]
		[UIHint(UIHint.NamedColor)]
		public FsmString namedColor;

		// Token: 0x04002DA0 RID: 11680
		[Tooltip("Set the parameter value.")]
		[RequiredField]
		public FsmColor color;

		// Token: 0x04002DA1 RID: 11681
		[Tooltip("Repeat every frame. Useful if the value is animated.")]
		public bool everyFrame;
	}
}
