using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B68 RID: 2920
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Performs most possible operations on 2 Vector3: Dot product, Cross product, Distance, Angle, Project, Reflect, Add, Subtract, Multiply, Divide, Min, Max")]
	public class Vector3Operator : FsmStateAction
	{
		// Token: 0x06003D2B RID: 15659 RVA: 0x0013245C File Offset: 0x0013065C
		public override void Reset()
		{
			this.vector1 = null;
			this.vector2 = null;
			this.operation = Vector3Operator.Vector3Operation.Add;
			this.storeVector3Result = null;
			this.storeFloatResult = null;
			this.everyFrame = false;
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x00132488 File Offset: 0x00130688
		public override void OnEnter()
		{
			this.DoVector3Operator();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003D2D RID: 15661 RVA: 0x001324A4 File Offset: 0x001306A4
		public override void OnUpdate()
		{
			this.DoVector3Operator();
		}

		// Token: 0x06003D2E RID: 15662 RVA: 0x001324AC File Offset: 0x001306AC
		private void DoVector3Operator()
		{
			Vector3 value = this.vector1.Value;
			Vector3 value2 = this.vector2.Value;
			switch (this.operation)
			{
			case Vector3Operator.Vector3Operation.DotProduct:
				this.storeFloatResult.Value = Vector3.Dot(value, value2);
				break;
			case Vector3Operator.Vector3Operation.CrossProduct:
				this.storeVector3Result.Value = Vector3.Cross(value, value2);
				break;
			case Vector3Operator.Vector3Operation.Distance:
				this.storeFloatResult.Value = Vector3.Distance(value, value2);
				break;
			case Vector3Operator.Vector3Operation.Angle:
				this.storeFloatResult.Value = Vector3.Angle(value, value2);
				break;
			case Vector3Operator.Vector3Operation.Project:
				this.storeVector3Result.Value = Vector3.Project(value, value2);
				break;
			case Vector3Operator.Vector3Operation.Reflect:
				this.storeVector3Result.Value = Vector3.Reflect(value, value2);
				break;
			case Vector3Operator.Vector3Operation.Add:
				this.storeVector3Result.Value = value + value2;
				break;
			case Vector3Operator.Vector3Operation.Subtract:
				this.storeVector3Result.Value = value - value2;
				break;
			case Vector3Operator.Vector3Operation.Multiply:
			{
				Vector3 zero = Vector3.zero;
				zero.x = value.x * value2.x;
				zero.y = value.y * value2.y;
				zero.z = value.z * value2.z;
				this.storeVector3Result.Value = zero;
				break;
			}
			case Vector3Operator.Vector3Operation.Divide:
			{
				Vector3 zero2 = Vector3.zero;
				zero2.x = value.x / value2.x;
				zero2.y = value.y / value2.y;
				zero2.z = value.z / value2.z;
				this.storeVector3Result.Value = zero2;
				break;
			}
			case Vector3Operator.Vector3Operation.Min:
				this.storeVector3Result.Value = Vector3.Min(value, value2);
				break;
			case Vector3Operator.Vector3Operation.Max:
				this.storeVector3Result.Value = Vector3.Max(value, value2);
				break;
			}
		}

		// Token: 0x04002F72 RID: 12146
		[RequiredField]
		public FsmVector3 vector1;

		// Token: 0x04002F73 RID: 12147
		[RequiredField]
		public FsmVector3 vector2;

		// Token: 0x04002F74 RID: 12148
		public Vector3Operator.Vector3Operation operation = Vector3Operator.Vector3Operation.Add;

		// Token: 0x04002F75 RID: 12149
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeVector3Result;

		// Token: 0x04002F76 RID: 12150
		[UIHint(UIHint.Variable)]
		public FsmFloat storeFloatResult;

		// Token: 0x04002F77 RID: 12151
		public bool everyFrame;

		// Token: 0x02000B69 RID: 2921
		public enum Vector3Operation
		{
			// Token: 0x04002F79 RID: 12153
			DotProduct,
			// Token: 0x04002F7A RID: 12154
			CrossProduct,
			// Token: 0x04002F7B RID: 12155
			Distance,
			// Token: 0x04002F7C RID: 12156
			Angle,
			// Token: 0x04002F7D RID: 12157
			Project,
			// Token: 0x04002F7E RID: 12158
			Reflect,
			// Token: 0x04002F7F RID: 12159
			Add,
			// Token: 0x04002F80 RID: 12160
			Subtract,
			// Token: 0x04002F81 RID: 12161
			Multiply,
			// Token: 0x04002F82 RID: 12162
			Divide,
			// Token: 0x04002F83 RID: 12163
			Min,
			// Token: 0x04002F84 RID: 12164
			Max
		}
	}
}
