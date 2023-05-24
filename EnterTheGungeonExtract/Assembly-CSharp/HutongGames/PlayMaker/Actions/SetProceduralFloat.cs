using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A8A RID: 2698
	[ActionCategory("Substance")]
	[Tooltip("Set a named float property in a Substance material. NOTE: Use Rebuild Textures after setting Substance properties.")]
	public class SetProceduralFloat : FsmStateAction
	{
		// Token: 0x06003941 RID: 14657 RVA: 0x001254D4 File Offset: 0x001236D4
		public override void Reset()
		{
			this.substanceMaterial = null;
			this.floatProperty = string.Empty;
			this.floatValue = 0f;
			this.everyFrame = false;
		}

		// Token: 0x06003942 RID: 14658 RVA: 0x00125504 File Offset: 0x00123704
		public override void OnEnter()
		{
			this.DoSetProceduralFloat();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003943 RID: 14659 RVA: 0x00125520 File Offset: 0x00123720
		public override void OnUpdate()
		{
			this.DoSetProceduralFloat();
		}

		// Token: 0x06003944 RID: 14660 RVA: 0x00125528 File Offset: 0x00123728
		private void DoSetProceduralFloat()
		{
			ProceduralMaterial proceduralMaterial = this.substanceMaterial.Value as ProceduralMaterial;
			if (proceduralMaterial == null)
			{
				base.LogError("The Material is not a Substance Material!");
				return;
			}
			proceduralMaterial.SetProceduralFloat(this.floatProperty.Value, this.floatValue.Value);
		}

		// Token: 0x04002B8F RID: 11151
		[Tooltip("The Substance Material.")]
		[RequiredField]
		public FsmMaterial substanceMaterial;

		// Token: 0x04002B90 RID: 11152
		[RequiredField]
		[Tooltip("The named float property in the material.")]
		public FsmString floatProperty;

		// Token: 0x04002B91 RID: 11153
		[RequiredField]
		[Tooltip("The value to set the property to.")]
		public FsmFloat floatValue;

		// Token: 0x04002B92 RID: 11154
		[Tooltip("NOTE: Updating procedural materials every frame can be very slow!")]
		public bool everyFrame;
	}
}
