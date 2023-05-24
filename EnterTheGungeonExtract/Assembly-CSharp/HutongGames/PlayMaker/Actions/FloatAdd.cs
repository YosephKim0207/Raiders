using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200094B RID: 2379
	[Tooltip("Adds a value to a Float Variable.")]
	[ActionCategory(ActionCategory.Math)]
	public class FloatAdd : FsmStateAction
	{
		// Token: 0x06003403 RID: 13315 RVA: 0x0010EE8C File Offset: 0x0010D08C
		public override void Reset()
		{
			this.floatVariable = null;
			this.add = null;
			this.everyFrame = false;
			this.perSecond = false;
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x0010EEAC File Offset: 0x0010D0AC
		public override void OnEnter()
		{
			this.DoFloatAdd();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x0010EEC8 File Offset: 0x0010D0C8
		public override void OnUpdate()
		{
			this.DoFloatAdd();
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x0010EED0 File Offset: 0x0010D0D0
		private void DoFloatAdd()
		{
			if (!this.perSecond)
			{
				this.floatVariable.Value += this.add.Value;
			}
			else
			{
				this.floatVariable.Value += this.add.Value * Time.deltaTime;
			}
		}

		// Token: 0x04002522 RID: 9506
		[Tooltip("The Float variable to add to.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		// Token: 0x04002523 RID: 9507
		[RequiredField]
		[Tooltip("Amount to add.")]
		public FsmFloat add;

		// Token: 0x04002524 RID: 9508
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		// Token: 0x04002525 RID: 9509
		[Tooltip("Used with Every Frame. Adds the value over one second to make the operation frame rate independent.")]
		public bool perSecond;
	}
}
