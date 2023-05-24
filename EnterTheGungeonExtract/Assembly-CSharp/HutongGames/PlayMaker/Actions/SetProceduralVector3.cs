using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A8C RID: 2700
	[ActionCategory("Substance")]
	[Tooltip("Set a named Vector3 property in a Substance material. NOTE: Use Rebuild Textures after setting Substance properties.")]
	public class SetProceduralVector3 : FsmStateAction
	{
		// Token: 0x0600394B RID: 14667 RVA: 0x00125628 File Offset: 0x00123828
		public override void Reset()
		{
			this.substanceMaterial = null;
			this.vector3Property = null;
			this.vector3Value = null;
			this.everyFrame = false;
		}

		// Token: 0x0600394C RID: 14668 RVA: 0x00125648 File Offset: 0x00123848
		public override void OnEnter()
		{
			this.DoSetProceduralVector();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600394D RID: 14669 RVA: 0x00125664 File Offset: 0x00123864
		public override void OnUpdate()
		{
			this.DoSetProceduralVector();
		}

		// Token: 0x0600394E RID: 14670 RVA: 0x0012566C File Offset: 0x0012386C
		private void DoSetProceduralVector()
		{
			ProceduralMaterial proceduralMaterial = this.substanceMaterial.Value as ProceduralMaterial;
			if (proceduralMaterial == null)
			{
				base.LogError("The Material is not a Substance Material!");
				return;
			}
			proceduralMaterial.SetProceduralVector(this.vector3Property.Value, this.vector3Value.Value);
		}

		// Token: 0x04002B97 RID: 11159
		[Tooltip("The Substance Material.")]
		[RequiredField]
		public FsmMaterial substanceMaterial;

		// Token: 0x04002B98 RID: 11160
		[Tooltip("The named vector property in the material.")]
		[RequiredField]
		public FsmString vector3Property;

		// Token: 0x04002B99 RID: 11161
		[Tooltip("The value to set the property to.\nNOTE: Use Set Procedural Vector3 for Vector3 values.")]
		[RequiredField]
		public FsmVector3 vector3Value;

		// Token: 0x04002B9A RID: 11162
		[Tooltip("NOTE: Updating procedural materials every frame can be very slow!")]
		public bool everyFrame;
	}
}
