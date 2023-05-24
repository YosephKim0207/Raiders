using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B19 RID: 2841
	[Tooltip("Sets the Scale of a named texture in a Game Object's Material. Useful for special effects.")]
	[ActionCategory(ActionCategory.Material)]
	public class SetTextureScale : ComponentAction<Renderer>
	{
		// Token: 0x06003BDF RID: 15327 RVA: 0x0012D870 File Offset: 0x0012BA70
		public override void Reset()
		{
			this.gameObject = null;
			this.materialIndex = 0;
			this.namedTexture = "_MainTex";
			this.scaleX = 1f;
			this.scaleY = 1f;
			this.everyFrame = false;
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x0012D8C8 File Offset: 0x0012BAC8
		public override void OnEnter()
		{
			this.DoSetTextureScale();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x0012D8E4 File Offset: 0x0012BAE4
		public override void OnUpdate()
		{
			this.DoSetTextureScale();
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x0012D8EC File Offset: 0x0012BAEC
		private void DoSetTextureScale()
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
				base.renderer.material.SetTextureScale(this.namedTexture.Value, new Vector2(this.scaleX.Value, this.scaleY.Value));
			}
			else if (base.renderer.materials.Length > this.materialIndex.Value)
			{
				Material[] materials = base.renderer.materials;
				materials[this.materialIndex.Value].SetTextureScale(this.namedTexture.Value, new Vector2(this.scaleX.Value, this.scaleY.Value));
				base.renderer.materials = materials;
			}
		}

		// Token: 0x04002DFC RID: 11772
		[CheckForComponent(typeof(Renderer))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002DFD RID: 11773
		public FsmInt materialIndex;

		// Token: 0x04002DFE RID: 11774
		[UIHint(UIHint.NamedColor)]
		public FsmString namedTexture;

		// Token: 0x04002DFF RID: 11775
		[RequiredField]
		public FsmFloat scaleX;

		// Token: 0x04002E00 RID: 11776
		[RequiredField]
		public FsmFloat scaleY;

		// Token: 0x04002E01 RID: 11777
		public bool everyFrame;
	}
}
