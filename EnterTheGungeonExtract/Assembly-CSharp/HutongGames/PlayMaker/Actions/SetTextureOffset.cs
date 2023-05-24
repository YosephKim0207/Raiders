using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B18 RID: 2840
	[ActionCategory(ActionCategory.Material)]
	[Tooltip("Sets the Offset of a named texture in a Game Object's Material. Useful for scrolling texture effects.")]
	public class SetTextureOffset : ComponentAction<Renderer>
	{
		// Token: 0x06003BDA RID: 15322 RVA: 0x0012D6E4 File Offset: 0x0012B8E4
		public override void Reset()
		{
			this.gameObject = null;
			this.materialIndex = 0;
			this.namedTexture = "_MainTex";
			this.offsetX = 0f;
			this.offsetY = 0f;
			this.everyFrame = false;
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x0012D73C File Offset: 0x0012B93C
		public override void OnEnter()
		{
			this.DoSetTextureOffset();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003BDC RID: 15324 RVA: 0x0012D758 File Offset: 0x0012B958
		public override void OnUpdate()
		{
			this.DoSetTextureOffset();
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x0012D760 File Offset: 0x0012B960
		private void DoSetTextureOffset()
		{
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
				base.renderer.material.SetTextureOffset(this.namedTexture.Value, new Vector2(this.offsetX.Value, this.offsetY.Value));
			}
			else if (base.renderer.materials.Length > this.materialIndex.Value)
			{
				Material[] materials = base.renderer.materials;
				materials[this.materialIndex.Value].SetTextureOffset(this.namedTexture.Value, new Vector2(this.offsetX.Value, this.offsetY.Value));
				base.renderer.materials = materials;
			}
		}

		// Token: 0x04002DF6 RID: 11766
		[RequiredField]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002DF7 RID: 11767
		public FsmInt materialIndex;

		// Token: 0x04002DF8 RID: 11768
		[RequiredField]
		[UIHint(UIHint.NamedColor)]
		public FsmString namedTexture;

		// Token: 0x04002DF9 RID: 11769
		[RequiredField]
		public FsmFloat offsetX;

		// Token: 0x04002DFA RID: 11770
		[RequiredField]
		public FsmFloat offsetY;

		// Token: 0x04002DFB RID: 11771
		public bool everyFrame;
	}
}
