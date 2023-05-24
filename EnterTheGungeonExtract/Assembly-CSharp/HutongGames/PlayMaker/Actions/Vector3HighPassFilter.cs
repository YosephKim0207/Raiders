using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B61 RID: 2913
	[Tooltip("Use a high pass filter to isolate sudden changes in a Vector3 Variable. Useful when working with Get Device Acceleration to remove the constant effect of gravity.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class Vector3HighPassFilter : FsmStateAction
	{
		// Token: 0x06003D0E RID: 15630 RVA: 0x00131D40 File Offset: 0x0012FF40
		public override void Reset()
		{
			this.vector3Variable = null;
			this.filteringFactor = 0.1f;
		}

		// Token: 0x06003D0F RID: 15631 RVA: 0x00131D5C File Offset: 0x0012FF5C
		public override void OnEnter()
		{
			this.filteredVector = new Vector3(this.vector3Variable.Value.x, this.vector3Variable.Value.y, this.vector3Variable.Value.z);
		}

		// Token: 0x06003D10 RID: 15632 RVA: 0x00131DB0 File Offset: 0x0012FFB0
		public override void OnUpdate()
		{
			this.filteredVector.x = this.vector3Variable.Value.x - (this.vector3Variable.Value.x * this.filteringFactor.Value + this.filteredVector.x * (1f - this.filteringFactor.Value));
			this.filteredVector.y = this.vector3Variable.Value.y - (this.vector3Variable.Value.y * this.filteringFactor.Value + this.filteredVector.y * (1f - this.filteringFactor.Value));
			this.filteredVector.z = this.vector3Variable.Value.z - (this.vector3Variable.Value.z * this.filteringFactor.Value + this.filteredVector.z * (1f - this.filteringFactor.Value));
			this.vector3Variable.Value = new Vector3(this.filteredVector.x, this.filteredVector.y, this.filteredVector.z);
		}

		// Token: 0x04002F57 RID: 12119
		[Tooltip("Vector3 Variable to filter. Should generally come from some constantly updated input, e.g., acceleration.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector3Variable;

		// Token: 0x04002F58 RID: 12120
		[Tooltip("Determines how much influence new changes have.")]
		public FsmFloat filteringFactor;

		// Token: 0x04002F59 RID: 12121
		private Vector3 filteredVector;
	}
}
