using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000914 RID: 2324
	[ActionCategory(ActionCategory.Convert)]
	[Tooltip("Converts a Bool value to a Color.")]
	public class ConvertBoolToColor : FsmStateAction
	{
		// Token: 0x0600332E RID: 13102 RVA: 0x0010C790 File Offset: 0x0010A990
		public override void Reset()
		{
			this.boolVariable = null;
			this.colorVariable = null;
			this.falseColor = Color.black;
			this.trueColor = Color.white;
			this.everyFrame = false;
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x0010C7C8 File Offset: 0x0010A9C8
		public override void OnEnter()
		{
			this.DoConvertBoolToColor();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003330 RID: 13104 RVA: 0x0010C7E4 File Offset: 0x0010A9E4
		public override void OnUpdate()
		{
			this.DoConvertBoolToColor();
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x0010C7EC File Offset: 0x0010A9EC
		private void DoConvertBoolToColor()
		{
			this.colorVariable.Value = ((!this.boolVariable.Value) ? this.falseColor.Value : this.trueColor.Value);
		}

		// Token: 0x0400245F RID: 9311
		[Tooltip("The Bool variable to test.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool boolVariable;

		// Token: 0x04002460 RID: 9312
		[Tooltip("The Color variable to set based on the bool variable value.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmColor colorVariable;

		// Token: 0x04002461 RID: 9313
		[Tooltip("Color if Bool variable is false.")]
		public FsmColor falseColor;

		// Token: 0x04002462 RID: 9314
		[Tooltip("Color if Bool variable is true.")]
		public FsmColor trueColor;

		// Token: 0x04002463 RID: 9315
		[Tooltip("Repeat every frame. Useful if the Bool variable is changing.")]
		public bool everyFrame;
	}
}
