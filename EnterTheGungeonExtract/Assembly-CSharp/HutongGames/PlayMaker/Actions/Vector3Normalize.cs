using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B67 RID: 2919
	[Tooltip("Normalizes a Vector3 Variable.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class Vector3Normalize : FsmStateAction
	{
		// Token: 0x06003D27 RID: 15655 RVA: 0x001323D4 File Offset: 0x001305D4
		public override void Reset()
		{
			this.vector3Variable = null;
			this.everyFrame = false;
		}

		// Token: 0x06003D28 RID: 15656 RVA: 0x001323E4 File Offset: 0x001305E4
		public override void OnEnter()
		{
			this.vector3Variable.Value = this.vector3Variable.Value.normalized;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x00132420 File Offset: 0x00130620
		public override void OnUpdate()
		{
			this.vector3Variable.Value = this.vector3Variable.Value.normalized;
		}

		// Token: 0x04002F70 RID: 12144
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 vector3Variable;

		// Token: 0x04002F71 RID: 12145
		public bool everyFrame;
	}
}
