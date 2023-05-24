using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B55 RID: 2901
	[Tooltip("Use a low pass filter to reduce the influence of sudden changes in a Vector2 Variable.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2LowPassFilter : FsmStateAction
	{
		// Token: 0x06003CDE RID: 15582 RVA: 0x00131390 File Offset: 0x0012F590
		public override void Reset()
		{
			this.vector2Variable = null;
			this.filteringFactor = 0.1f;
		}

		// Token: 0x06003CDF RID: 15583 RVA: 0x001313AC File Offset: 0x0012F5AC
		public override void OnEnter()
		{
			this.filteredVector = new Vector2(this.vector2Variable.Value.x, this.vector2Variable.Value.y);
		}

		// Token: 0x06003CE0 RID: 15584 RVA: 0x001313EC File Offset: 0x0012F5EC
		public override void OnUpdate()
		{
			this.filteredVector.x = this.vector2Variable.Value.x * this.filteringFactor.Value + this.filteredVector.x * (1f - this.filteringFactor.Value);
			this.filteredVector.y = this.vector2Variable.Value.y * this.filteringFactor.Value + this.filteredVector.y * (1f - this.filteringFactor.Value);
			this.vector2Variable.Value = new Vector2(this.filteredVector.x, this.filteredVector.y);
		}

		// Token: 0x04002F23 RID: 12067
		[Tooltip("Vector2 Variable to filter. Should generally come from some constantly updated input")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector2 vector2Variable;

		// Token: 0x04002F24 RID: 12068
		[Tooltip("Determines how much influence new changes have. E.g., 0.1 keeps 10 percent of the unfiltered vector and 90 percent of the previously filtered value")]
		public FsmFloat filteringFactor;

		// Token: 0x04002F25 RID: 12069
		private Vector2 filteredVector;
	}
}
