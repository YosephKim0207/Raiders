using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B51 RID: 2897
	[ActionCategory(ActionCategory.Vector2)]
	[Tooltip("Use a high pass filter to isolate sudden changes in a Vector2 Variable.")]
	public class Vector2HighPassFilter : FsmStateAction
	{
		// Token: 0x06003CCD RID: 15565 RVA: 0x00130F94 File Offset: 0x0012F194
		public override void Reset()
		{
			this.vector2Variable = null;
			this.filteringFactor = 0.1f;
		}

		// Token: 0x06003CCE RID: 15566 RVA: 0x00130FB0 File Offset: 0x0012F1B0
		public override void OnEnter()
		{
			this.filteredVector = new Vector2(this.vector2Variable.Value.x, this.vector2Variable.Value.y);
		}

		// Token: 0x06003CCF RID: 15567 RVA: 0x00130FF0 File Offset: 0x0012F1F0
		public override void OnUpdate()
		{
			this.filteredVector.x = this.vector2Variable.Value.x - (this.vector2Variable.Value.x * this.filteringFactor.Value + this.filteredVector.x * (1f - this.filteringFactor.Value));
			this.filteredVector.y = this.vector2Variable.Value.y - (this.vector2Variable.Value.y * this.filteringFactor.Value + this.filteredVector.y * (1f - this.filteringFactor.Value));
			this.vector2Variable.Value = new Vector2(this.filteredVector.x, this.filteredVector.y);
		}

		// Token: 0x04002F10 RID: 12048
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("Vector2 Variable to filter. Should generally come from some constantly updated input.")]
		public FsmVector2 vector2Variable;

		// Token: 0x04002F11 RID: 12049
		[Tooltip("Determines how much influence new changes have.")]
		public FsmFloat filteringFactor;

		// Token: 0x04002F12 RID: 12050
		private Vector2 filteredVector;
	}
}
