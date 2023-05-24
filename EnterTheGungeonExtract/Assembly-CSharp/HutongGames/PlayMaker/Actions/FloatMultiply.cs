using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000952 RID: 2386
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Multiplies one Float by another.")]
	public class FloatMultiply : FsmStateAction
	{
		// Token: 0x06003424 RID: 13348 RVA: 0x0010F454 File Offset: 0x0010D654
		public override void Reset()
		{
			this.floatVariable = null;
			this.multiplyBy = null;
			this.everyFrame = false;
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x0010F46C File Offset: 0x0010D66C
		public override void OnEnter()
		{
			this.floatVariable.Value *= this.multiplyBy.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003426 RID: 13350 RVA: 0x0010F49C File Offset: 0x0010D69C
		public override void OnUpdate()
		{
			this.floatVariable.Value *= this.multiplyBy.Value;
		}

		// Token: 0x04002544 RID: 9540
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The float variable to multiply.")]
		public FsmFloat floatVariable;

		// Token: 0x04002545 RID: 9541
		[Tooltip("Multiply the float variable by this value.")]
		[RequiredField]
		public FsmFloat multiplyBy;

		// Token: 0x04002546 RID: 9542
		[Tooltip("Repeat every frame. Useful if the variables are changing.")]
		public bool everyFrame;
	}
}
