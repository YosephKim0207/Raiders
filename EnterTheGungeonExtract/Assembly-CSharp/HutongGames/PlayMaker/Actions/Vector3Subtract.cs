using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B6C RID: 2924
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Subtracts a Vector3 value from a Vector3 variable.")]
	public class Vector3Subtract : FsmStateAction
	{
		// Token: 0x06003D37 RID: 15671 RVA: 0x001327E0 File Offset: 0x001309E0
		public override void Reset()
		{
			this.vector3Variable = null;
			this.subtractVector = new FsmVector3
			{
				UseVariable = true
			};
			this.everyFrame = false;
		}

		// Token: 0x06003D38 RID: 15672 RVA: 0x00132810 File Offset: 0x00130A10
		public override void OnEnter()
		{
			this.vector3Variable.Value = this.vector3Variable.Value - this.subtractVector.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x0013284C File Offset: 0x00130A4C
		public override void OnUpdate()
		{
			this.vector3Variable.Value = this.vector3Variable.Value - this.subtractVector.Value;
		}

		// Token: 0x04002F8B RID: 12171
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector3Variable;

		// Token: 0x04002F8C RID: 12172
		[RequiredField]
		public FsmVector3 subtractVector;

		// Token: 0x04002F8D RID: 12173
		public bool everyFrame;
	}
}
