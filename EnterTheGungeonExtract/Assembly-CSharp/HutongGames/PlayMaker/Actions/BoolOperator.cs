using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000901 RID: 2305
	[Tooltip("Performs boolean operations on 2 Bool Variables.")]
	[ActionCategory(ActionCategory.Math)]
	public class BoolOperator : FsmStateAction
	{
		// Token: 0x060032C7 RID: 12999 RVA: 0x0010A7A4 File Offset: 0x001089A4
		public override void Reset()
		{
			this.bool1 = false;
			this.bool2 = false;
			this.operation = BoolOperator.Operation.AND;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x060032C8 RID: 13000 RVA: 0x0010A7D4 File Offset: 0x001089D4
		public override void OnEnter()
		{
			this.DoBoolOperator();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060032C9 RID: 13001 RVA: 0x0010A7F0 File Offset: 0x001089F0
		public override void OnUpdate()
		{
			this.DoBoolOperator();
		}

		// Token: 0x060032CA RID: 13002 RVA: 0x0010A7F8 File Offset: 0x001089F8
		private void DoBoolOperator()
		{
			bool value = this.bool1.Value;
			bool value2 = this.bool2.Value;
			switch (this.operation)
			{
			case BoolOperator.Operation.AND:
				this.storeResult.Value = value && value2;
				break;
			case BoolOperator.Operation.NAND:
				this.storeResult.Value = !value || !value2;
				break;
			case BoolOperator.Operation.OR:
				this.storeResult.Value = value || value2;
				break;
			case BoolOperator.Operation.XOR:
				this.storeResult.Value = value ^ value2;
				break;
			}
		}

		// Token: 0x040023EC RID: 9196
		[Tooltip("The first Bool variable.")]
		[RequiredField]
		public FsmBool bool1;

		// Token: 0x040023ED RID: 9197
		[Tooltip("The second Bool variable.")]
		[RequiredField]
		public FsmBool bool2;

		// Token: 0x040023EE RID: 9198
		[Tooltip("Boolean Operation.")]
		public BoolOperator.Operation operation;

		// Token: 0x040023EF RID: 9199
		[Tooltip("Store the result in a Bool Variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool storeResult;

		// Token: 0x040023F0 RID: 9200
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		// Token: 0x02000902 RID: 2306
		public enum Operation
		{
			// Token: 0x040023F2 RID: 9202
			AND,
			// Token: 0x040023F3 RID: 9203
			NAND,
			// Token: 0x040023F4 RID: 9204
			OR,
			// Token: 0x040023F5 RID: 9205
			XOR
		}
	}
}
