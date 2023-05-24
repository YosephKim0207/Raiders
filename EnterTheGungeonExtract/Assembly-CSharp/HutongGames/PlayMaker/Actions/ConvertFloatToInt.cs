using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000919 RID: 2329
	[ActionCategory(ActionCategory.Convert)]
	[Tooltip("Converts a Float value to an Integer value.")]
	public class ConvertFloatToInt : FsmStateAction
	{
		// Token: 0x06003347 RID: 13127 RVA: 0x0010CA74 File Offset: 0x0010AC74
		public override void Reset()
		{
			this.floatVariable = null;
			this.intVariable = null;
			this.rounding = ConvertFloatToInt.FloatRounding.Nearest;
			this.everyFrame = false;
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x0010CA94 File Offset: 0x0010AC94
		public override void OnEnter()
		{
			this.DoConvertFloatToInt();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003349 RID: 13129 RVA: 0x0010CAB0 File Offset: 0x0010ACB0
		public override void OnUpdate()
		{
			this.DoConvertFloatToInt();
		}

		// Token: 0x0600334A RID: 13130 RVA: 0x0010CAB8 File Offset: 0x0010ACB8
		private void DoConvertFloatToInt()
		{
			ConvertFloatToInt.FloatRounding floatRounding = this.rounding;
			if (floatRounding != ConvertFloatToInt.FloatRounding.Nearest)
			{
				if (floatRounding != ConvertFloatToInt.FloatRounding.RoundDown)
				{
					if (floatRounding == ConvertFloatToInt.FloatRounding.RoundUp)
					{
						this.intVariable.Value = Mathf.CeilToInt(this.floatVariable.Value);
					}
				}
				else
				{
					this.intVariable.Value = Mathf.FloorToInt(this.floatVariable.Value);
				}
			}
			else
			{
				this.intVariable.Value = Mathf.RoundToInt(this.floatVariable.Value);
			}
		}

		// Token: 0x04002476 RID: 9334
		[Tooltip("The Float variable to convert to an integer.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		// Token: 0x04002477 RID: 9335
		[Tooltip("Store the result in an Integer variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt intVariable;

		// Token: 0x04002478 RID: 9336
		public ConvertFloatToInt.FloatRounding rounding;

		// Token: 0x04002479 RID: 9337
		public bool everyFrame;

		// Token: 0x0200091A RID: 2330
		public enum FloatRounding
		{
			// Token: 0x0400247B RID: 9339
			RoundDown,
			// Token: 0x0400247C RID: 9340
			RoundUp,
			// Token: 0x0400247D RID: 9341
			Nearest
		}
	}
}
