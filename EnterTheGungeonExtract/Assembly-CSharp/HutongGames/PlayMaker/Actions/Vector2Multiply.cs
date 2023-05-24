using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B57 RID: 2903
	[ActionCategory(ActionCategory.Vector2)]
	[Tooltip("Multiplies a Vector2 variable by a Float.")]
	public class Vector2Multiply : FsmStateAction
	{
		// Token: 0x06003CE6 RID: 15590 RVA: 0x00131590 File Offset: 0x0012F790
		public override void Reset()
		{
			this.vector2Variable = null;
			this.multiplyBy = 1f;
			this.everyFrame = false;
		}

		// Token: 0x06003CE7 RID: 15591 RVA: 0x001315B0 File Offset: 0x0012F7B0
		public override void OnEnter()
		{
			this.vector2Variable.Value = this.vector2Variable.Value * this.multiplyBy.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x001315EC File Offset: 0x0012F7EC
		public override void OnUpdate()
		{
			this.vector2Variable.Value = this.vector2Variable.Value * this.multiplyBy.Value;
		}

		// Token: 0x04002F2B RID: 12075
		[Tooltip("The vector to Multiply")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector2 vector2Variable;

		// Token: 0x04002F2C RID: 12076
		[Tooltip("The multiplication factor")]
		[RequiredField]
		public FsmFloat multiplyBy;

		// Token: 0x04002F2D RID: 12077
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
	}
}
