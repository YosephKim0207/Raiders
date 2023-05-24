using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B0D RID: 2829
	[Tooltip("Sets a Game Object's material randomly from an array of Materials.")]
	[ActionCategory(ActionCategory.Material)]
	public class SetRandomMaterial : ComponentAction<Renderer>
	{
		// Token: 0x06003BA7 RID: 15271 RVA: 0x0012CBE4 File Offset: 0x0012ADE4
		public override void Reset()
		{
			this.gameObject = null;
			this.materialIndex = 0;
			this.materials = new FsmMaterial[3];
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x0012CC08 File Offset: 0x0012AE08
		public override void OnEnter()
		{
			this.DoSetRandomMaterial();
			base.Finish();
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x0012CC18 File Offset: 0x0012AE18
		private void DoSetRandomMaterial()
		{
			if (this.materials == null)
			{
				return;
			}
			if (this.materials.Length == 0)
			{
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
				base.renderer.material = this.materials[UnityEngine.Random.Range(0, this.materials.Length)].Value;
			}
			else if (base.renderer.materials.Length > this.materialIndex.Value)
			{
				Material[] array = base.renderer.materials;
				array[this.materialIndex.Value] = this.materials[UnityEngine.Random.Range(0, this.materials.Length)].Value;
				base.renderer.materials = array;
			}
		}

		// Token: 0x04002DC8 RID: 11720
		[RequiredField]
		[CheckForComponent(typeof(Renderer))]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002DC9 RID: 11721
		public FsmInt materialIndex;

		// Token: 0x04002DCA RID: 11722
		public FsmMaterial[] materials;
	}
}
