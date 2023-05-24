using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A89 RID: 2697
	[Tooltip("Set a named color property in a Substance material. NOTE: Use Rebuild Textures after setting Substance properties.")]
	[ActionCategory("Substance")]
	public class SetProceduralColor : FsmStateAction
	{
		// Token: 0x0600393C RID: 14652 RVA: 0x00125424 File Offset: 0x00123624
		public override void Reset()
		{
			this.substanceMaterial = null;
			this.colorProperty = string.Empty;
			this.colorValue = Color.white;
			this.everyFrame = false;
		}

		// Token: 0x0600393D RID: 14653 RVA: 0x00125454 File Offset: 0x00123654
		public override void OnEnter()
		{
			this.DoSetProceduralFloat();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600393E RID: 14654 RVA: 0x00125470 File Offset: 0x00123670
		public override void OnUpdate()
		{
			this.DoSetProceduralFloat();
		}

		// Token: 0x0600393F RID: 14655 RVA: 0x00125478 File Offset: 0x00123678
		private void DoSetProceduralFloat()
		{
			ProceduralMaterial proceduralMaterial = this.substanceMaterial.Value as ProceduralMaterial;
			if (proceduralMaterial == null)
			{
				base.LogError("The Material is not a Substance Material!");
				return;
			}
			proceduralMaterial.SetProceduralColor(this.colorProperty.Value, this.colorValue.Value);
		}

		// Token: 0x04002B8B RID: 11147
		[Tooltip("The Substance Material.")]
		[RequiredField]
		public FsmMaterial substanceMaterial;

		// Token: 0x04002B8C RID: 11148
		[RequiredField]
		[Tooltip("The named color property in the material.")]
		public FsmString colorProperty;

		// Token: 0x04002B8D RID: 11149
		[Tooltip("The value to set the property to.")]
		[RequiredField]
		public FsmColor colorValue;

		// Token: 0x04002B8E RID: 11150
		[Tooltip("NOTE: Updating procedural materials every frame can be very slow!")]
		public bool everyFrame;
	}
}
