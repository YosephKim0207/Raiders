using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B60 RID: 2912
	[Tooltip("Clamps the Magnitude of Vector3 Variable.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class Vector3ClampMagnitude : FsmStateAction
	{
		// Token: 0x06003D09 RID: 15625 RVA: 0x00131CD4 File Offset: 0x0012FED4
		public override void Reset()
		{
			this.vector3Variable = null;
			this.maxLength = null;
			this.everyFrame = false;
		}

		// Token: 0x06003D0A RID: 15626 RVA: 0x00131CEC File Offset: 0x0012FEEC
		public override void OnEnter()
		{
			this.DoVector3ClampMagnitude();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003D0B RID: 15627 RVA: 0x00131D08 File Offset: 0x0012FF08
		public override void OnUpdate()
		{
			this.DoVector3ClampMagnitude();
		}

		// Token: 0x06003D0C RID: 15628 RVA: 0x00131D10 File Offset: 0x0012FF10
		private void DoVector3ClampMagnitude()
		{
			this.vector3Variable.Value = Vector3.ClampMagnitude(this.vector3Variable.Value, this.maxLength.Value);
		}

		// Token: 0x04002F54 RID: 12116
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 vector3Variable;

		// Token: 0x04002F55 RID: 12117
		[RequiredField]
		public FsmFloat maxLength;

		// Token: 0x04002F56 RID: 12118
		public bool everyFrame;
	}
}
