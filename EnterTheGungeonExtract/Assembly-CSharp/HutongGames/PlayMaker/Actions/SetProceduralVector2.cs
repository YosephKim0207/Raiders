using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A8B RID: 2699
	[ActionCategory("Substance")]
	[Tooltip("Set a named Vector2 property in a Substance material. NOTE: Use Rebuild Textures after setting Substance properties.")]
	public class SetProceduralVector2 : FsmStateAction
	{
		// Token: 0x06003946 RID: 14662 RVA: 0x00125584 File Offset: 0x00123784
		public override void Reset()
		{
			this.substanceMaterial = null;
			this.vector2Property = null;
			this.vector2Value = null;
			this.everyFrame = false;
		}

		// Token: 0x06003947 RID: 14663 RVA: 0x001255A4 File Offset: 0x001237A4
		public override void OnEnter()
		{
			this.DoSetProceduralVector();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003948 RID: 14664 RVA: 0x001255C0 File Offset: 0x001237C0
		public override void OnUpdate()
		{
			this.DoSetProceduralVector();
		}

		// Token: 0x06003949 RID: 14665 RVA: 0x001255C8 File Offset: 0x001237C8
		private void DoSetProceduralVector()
		{
			ProceduralMaterial proceduralMaterial = this.substanceMaterial.Value as ProceduralMaterial;
			if (proceduralMaterial == null)
			{
				base.LogError("The Material is not a Substance Material!");
				return;
			}
			proceduralMaterial.SetProceduralVector(this.vector2Property.Value, this.vector2Value.Value);
		}

		// Token: 0x04002B93 RID: 11155
		[Tooltip("The Substance Material.")]
		[RequiredField]
		public FsmMaterial substanceMaterial;

		// Token: 0x04002B94 RID: 11156
		[RequiredField]
		[Tooltip("The named vector property in the material.")]
		public FsmString vector2Property;

		// Token: 0x04002B95 RID: 11157
		[Tooltip("The Vector3 value to set the property to.\nNOTE: Use Set Procedural Vector2 for Vector3 values.")]
		[RequiredField]
		public FsmVector2 vector2Value;

		// Token: 0x04002B96 RID: 11158
		[Tooltip("NOTE: Updating procedural materials every frame can be very slow!")]
		public bool everyFrame;
	}
}
