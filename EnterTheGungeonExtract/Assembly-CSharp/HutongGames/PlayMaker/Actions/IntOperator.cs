using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009F0 RID: 2544
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Performs math operation on 2 Integers: Add, Subtract, Multiply, Divide, Min, Max.")]
	public class IntOperator : FsmStateAction
	{
		// Token: 0x0600369E RID: 13982 RVA: 0x0011717C File Offset: 0x0011537C
		public override void Reset()
		{
			this.integer1 = null;
			this.integer2 = null;
			this.operation = IntOperator.Operation.Add;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x001171A4 File Offset: 0x001153A4
		public override void OnEnter()
		{
			this.DoIntOperator();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060036A0 RID: 13984 RVA: 0x001171C0 File Offset: 0x001153C0
		public override void OnUpdate()
		{
			this.DoIntOperator();
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x001171C8 File Offset: 0x001153C8
		private void DoIntOperator()
		{
			int value = this.integer1.Value;
			int value2 = this.integer2.Value;
			switch (this.operation)
			{
			case IntOperator.Operation.Add:
				this.storeResult.Value = value + value2;
				break;
			case IntOperator.Operation.Subtract:
				this.storeResult.Value = value - value2;
				break;
			case IntOperator.Operation.Multiply:
				this.storeResult.Value = value * value2;
				break;
			case IntOperator.Operation.Divide:
				this.storeResult.Value = value / value2;
				break;
			case IntOperator.Operation.Min:
				this.storeResult.Value = Mathf.Min(value, value2);
				break;
			case IntOperator.Operation.Max:
				this.storeResult.Value = Mathf.Max(value, value2);
				break;
			}
		}

		// Token: 0x040027F7 RID: 10231
		[RequiredField]
		public FsmInt integer1;

		// Token: 0x040027F8 RID: 10232
		[RequiredField]
		public FsmInt integer2;

		// Token: 0x040027F9 RID: 10233
		public IntOperator.Operation operation;

		// Token: 0x040027FA RID: 10234
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt storeResult;

		// Token: 0x040027FB RID: 10235
		public bool everyFrame;

		// Token: 0x020009F1 RID: 2545
		public enum Operation
		{
			// Token: 0x040027FD RID: 10237
			Add,
			// Token: 0x040027FE RID: 10238
			Subtract,
			// Token: 0x040027FF RID: 10239
			Multiply,
			// Token: 0x04002800 RID: 10240
			Divide,
			// Token: 0x04002801 RID: 10241
			Min,
			// Token: 0x04002802 RID: 10242
			Max
		}
	}
}
