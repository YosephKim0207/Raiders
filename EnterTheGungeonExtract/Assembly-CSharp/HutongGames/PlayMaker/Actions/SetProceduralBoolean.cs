using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A88 RID: 2696
	[ActionCategory("Substance")]
	[Tooltip("Set a named bool property in a Substance material. NOTE: Use Rebuild Textures after setting Substance properties.")]
	public class SetProceduralBoolean : FsmStateAction
	{
		// Token: 0x06003937 RID: 14647 RVA: 0x00125378 File Offset: 0x00123578
		public override void Reset()
		{
			this.substanceMaterial = null;
			this.boolProperty = string.Empty;
			this.boolValue = false;
			this.everyFrame = false;
		}

		// Token: 0x06003938 RID: 14648 RVA: 0x001253A4 File Offset: 0x001235A4
		public override void OnEnter()
		{
			this.DoSetProceduralFloat();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003939 RID: 14649 RVA: 0x001253C0 File Offset: 0x001235C0
		public override void OnUpdate()
		{
			this.DoSetProceduralFloat();
		}

		// Token: 0x0600393A RID: 14650 RVA: 0x001253C8 File Offset: 0x001235C8
		private void DoSetProceduralFloat()
		{
			ProceduralMaterial proceduralMaterial = this.substanceMaterial.Value as ProceduralMaterial;
			if (proceduralMaterial == null)
			{
				base.LogError("The Material is not a Substance Material!");
				return;
			}
			proceduralMaterial.SetProceduralBoolean(this.boolProperty.Value, this.boolValue.Value);
		}

		// Token: 0x04002B87 RID: 11143
		[RequiredField]
		[Tooltip("The Substance Material.")]
		public FsmMaterial substanceMaterial;

		// Token: 0x04002B88 RID: 11144
		[Tooltip("The named bool property in the material.")]
		[RequiredField]
		public FsmString boolProperty;

		// Token: 0x04002B89 RID: 11145
		[RequiredField]
		[Tooltip("The value to set the property to.")]
		public FsmBool boolValue;

		// Token: 0x04002B8A RID: 11146
		[Tooltip("NOTE: Updating procedural materials every frame can be very slow!")]
		public bool everyFrame;
	}
}
