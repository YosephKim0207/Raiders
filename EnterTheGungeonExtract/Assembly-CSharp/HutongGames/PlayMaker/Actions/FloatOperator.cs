using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000953 RID: 2387
	[Tooltip("Performs math operations on 2 Floats: Add, Subtract, Multiply, Divide, Min, Max.")]
	[ActionCategory(ActionCategory.Math)]
	public class FloatOperator : FsmStateAction
	{
		// Token: 0x06003428 RID: 13352 RVA: 0x0010F4C4 File Offset: 0x0010D6C4
		public override void Reset()
		{
			this.float1 = null;
			this.float2 = null;
			this.operation = FloatOperator.Operation.Add;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x0010F4EC File Offset: 0x0010D6EC
		public override void OnEnter()
		{
			this.DoFloatOperator();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x0010F508 File Offset: 0x0010D708
		public override void OnUpdate()
		{
			this.DoFloatOperator();
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x0010F510 File Offset: 0x0010D710
		private void DoFloatOperator()
		{
			float value = this.float1.Value;
			float value2 = this.float2.Value;
			switch (this.operation)
			{
			case FloatOperator.Operation.Add:
				this.storeResult.Value = value + value2;
				break;
			case FloatOperator.Operation.Subtract:
				this.storeResult.Value = value - value2;
				break;
			case FloatOperator.Operation.Multiply:
				this.storeResult.Value = value * value2;
				break;
			case FloatOperator.Operation.Divide:
				this.storeResult.Value = value / value2;
				break;
			case FloatOperator.Operation.Min:
				this.storeResult.Value = Mathf.Min(value, value2);
				break;
			case FloatOperator.Operation.Max:
				this.storeResult.Value = Mathf.Max(value, value2);
				break;
			}
		}

		// Token: 0x04002547 RID: 9543
		[Tooltip("The first float.")]
		[RequiredField]
		public FsmFloat float1;

		// Token: 0x04002548 RID: 9544
		[Tooltip("The second float.")]
		[RequiredField]
		public FsmFloat float2;

		// Token: 0x04002549 RID: 9545
		[Tooltip("The math operation to perform on the floats.")]
		public FloatOperator.Operation operation;

		// Token: 0x0400254A RID: 9546
		[Tooltip("Store the result of the operation in a float variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat storeResult;

		// Token: 0x0400254B RID: 9547
		[Tooltip("Repeat every frame. Useful if the variables are changing.")]
		public bool everyFrame;

		// Token: 0x02000954 RID: 2388
		public enum Operation
		{
			// Token: 0x0400254D RID: 9549
			Add,
			// Token: 0x0400254E RID: 9550
			Subtract,
			// Token: 0x0400254F RID: 9551
			Multiply,
			// Token: 0x04002550 RID: 9552
			Divide,
			// Token: 0x04002551 RID: 9553
			Min,
			// Token: 0x04002552 RID: 9554
			Max
		}
	}
}
