using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B59 RID: 2905
	[Tooltip("Performs most possible operations on 2 Vector2: Dot product, Distance, Angle, Add, Subtract, Multiply, Divide, Min, Max")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2Operator : FsmStateAction
	{
		// Token: 0x06003CEE RID: 15598 RVA: 0x001316A4 File Offset: 0x0012F8A4
		public override void Reset()
		{
			this.vector1 = null;
			this.vector2 = null;
			this.operation = Vector2Operator.Vector2Operation.Add;
			this.storeVector2Result = null;
			this.storeFloatResult = null;
			this.everyFrame = false;
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x001316D0 File Offset: 0x0012F8D0
		public override void OnEnter()
		{
			this.DoVector2Operator();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x001316EC File Offset: 0x0012F8EC
		public override void OnUpdate()
		{
			this.DoVector2Operator();
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x001316F4 File Offset: 0x0012F8F4
		private void DoVector2Operator()
		{
			Vector2 value = this.vector1.Value;
			Vector2 value2 = this.vector2.Value;
			switch (this.operation)
			{
			case Vector2Operator.Vector2Operation.DotProduct:
				this.storeFloatResult.Value = Vector2.Dot(value, value2);
				break;
			case Vector2Operator.Vector2Operation.Distance:
				this.storeFloatResult.Value = Vector2.Distance(value, value2);
				break;
			case Vector2Operator.Vector2Operation.Angle:
				this.storeFloatResult.Value = Vector2.Angle(value, value2);
				break;
			case Vector2Operator.Vector2Operation.Add:
				this.storeVector2Result.Value = value + value2;
				break;
			case Vector2Operator.Vector2Operation.Subtract:
				this.storeVector2Result.Value = value - value2;
				break;
			case Vector2Operator.Vector2Operation.Multiply:
			{
				Vector2 zero = Vector2.zero;
				zero.x = value.x * value2.x;
				zero.y = value.y * value2.y;
				this.storeVector2Result.Value = zero;
				break;
			}
			case Vector2Operator.Vector2Operation.Divide:
			{
				Vector2 zero2 = Vector2.zero;
				zero2.x = value.x / value2.x;
				zero2.y = value.y / value2.y;
				this.storeVector2Result.Value = zero2;
				break;
			}
			case Vector2Operator.Vector2Operation.Min:
				this.storeVector2Result.Value = Vector2.Min(value, value2);
				break;
			case Vector2Operator.Vector2Operation.Max:
				this.storeVector2Result.Value = Vector2.Max(value, value2);
				break;
			}
		}

		// Token: 0x04002F30 RID: 12080
		[Tooltip("The first vector")]
		[RequiredField]
		public FsmVector2 vector1;

		// Token: 0x04002F31 RID: 12081
		[Tooltip("The second vector")]
		[RequiredField]
		public FsmVector2 vector2;

		// Token: 0x04002F32 RID: 12082
		[Tooltip("The operation")]
		public Vector2Operator.Vector2Operation operation = Vector2Operator.Vector2Operation.Add;

		// Token: 0x04002F33 RID: 12083
		[Tooltip("The Vector2 result when it applies.")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 storeVector2Result;

		// Token: 0x04002F34 RID: 12084
		[Tooltip("The float result when it applies")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeFloatResult;

		// Token: 0x04002F35 RID: 12085
		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		// Token: 0x02000B5A RID: 2906
		public enum Vector2Operation
		{
			// Token: 0x04002F37 RID: 12087
			DotProduct,
			// Token: 0x04002F38 RID: 12088
			Distance,
			// Token: 0x04002F39 RID: 12089
			Angle,
			// Token: 0x04002F3A RID: 12090
			Add,
			// Token: 0x04002F3B RID: 12091
			Subtract,
			// Token: 0x04002F3C RID: 12092
			Multiply,
			// Token: 0x04002F3D RID: 12093
			Divide,
			// Token: 0x04002F3E RID: 12094
			Min,
			// Token: 0x04002F3F RID: 12095
			Max
		}
	}
}
