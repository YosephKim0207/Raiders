using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000950 RID: 2384
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Divides one Float by another.")]
	public class FloatDivide : FsmStateAction
	{
		// Token: 0x0600341C RID: 13340 RVA: 0x0010F25C File Offset: 0x0010D45C
		public override void Reset()
		{
			this.floatVariable = null;
			this.divideBy = null;
			this.everyFrame = false;
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x0010F274 File Offset: 0x0010D474
		public override void OnEnter()
		{
			this.floatVariable.Value /= this.divideBy.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x0010F2A4 File Offset: 0x0010D4A4
		public override void OnUpdate()
		{
			this.floatVariable.Value /= this.divideBy.Value;
		}

		// Token: 0x04002538 RID: 9528
		[RequiredField]
		[Tooltip("The float variable to divide.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;

		// Token: 0x04002539 RID: 9529
		[Tooltip("Divide the float variable by this value.")]
		[RequiredField]
		public FsmFloat divideBy;

		// Token: 0x0400253A RID: 9530
		[Tooltip("Repeate every frame. Useful if the variables are changing.")]
		public bool everyFrame;
	}
}
