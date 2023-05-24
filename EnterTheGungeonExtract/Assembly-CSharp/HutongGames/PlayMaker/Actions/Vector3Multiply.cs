using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B66 RID: 2918
	[Tooltip("Multiplies a Vector3 variable by a Float.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class Vector3Multiply : FsmStateAction
	{
		// Token: 0x06003D23 RID: 15651 RVA: 0x00132348 File Offset: 0x00130548
		public override void Reset()
		{
			this.vector3Variable = null;
			this.multiplyBy = 1f;
			this.everyFrame = false;
		}

		// Token: 0x06003D24 RID: 15652 RVA: 0x00132368 File Offset: 0x00130568
		public override void OnEnter()
		{
			this.vector3Variable.Value = this.vector3Variable.Value * this.multiplyBy.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003D25 RID: 15653 RVA: 0x001323A4 File Offset: 0x001305A4
		public override void OnUpdate()
		{
			this.vector3Variable.Value = this.vector3Variable.Value * this.multiplyBy.Value;
		}

		// Token: 0x04002F6D RID: 12141
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 vector3Variable;

		// Token: 0x04002F6E RID: 12142
		[RequiredField]
		public FsmFloat multiplyBy;

		// Token: 0x04002F6F RID: 12143
		public bool everyFrame;
	}
}
