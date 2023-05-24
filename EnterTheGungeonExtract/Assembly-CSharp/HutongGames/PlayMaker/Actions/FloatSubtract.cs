using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000956 RID: 2390
	[Tooltip("Subtracts a value from a Float Variable.")]
	[ActionCategory(ActionCategory.Math)]
	public class FloatSubtract : FsmStateAction
	{
		// Token: 0x06003433 RID: 13363 RVA: 0x0010F6A8 File Offset: 0x0010D8A8
		public override void Reset()
		{
			this.floatVariable = null;
			this.subtract = null;
			this.everyFrame = false;
			this.perSecond = false;
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x0010F6C8 File Offset: 0x0010D8C8
		public override void OnEnter()
		{
			this.DoFloatSubtract();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x0010F6E4 File Offset: 0x0010D8E4
		public override void OnUpdate()
		{
			this.DoFloatSubtract();
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x0010F6EC File Offset: 0x0010D8EC
		private void DoFloatSubtract()
		{
			if (!this.perSecond)
			{
				this.floatVariable.Value -= this.subtract.Value;
			}
			else
			{
				this.floatVariable.Value -= this.subtract.Value * Time.deltaTime;
			}
		}

		// Token: 0x04002557 RID: 9559
		[Tooltip("The float variable to subtract from.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		// Token: 0x04002558 RID: 9560
		[Tooltip("Value to subtract from the float variable.")]
		[RequiredField]
		public FsmFloat subtract;

		// Token: 0x04002559 RID: 9561
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		// Token: 0x0400255A RID: 9562
		[Tooltip("Used with Every Frame. Adds the value over one second to make the operation frame rate independent.")]
		public bool perSecond;
	}
}
