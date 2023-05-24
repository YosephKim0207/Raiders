using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B58 RID: 2904
	[Tooltip("Normalizes a Vector2 Variable.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2Normalize : FsmStateAction
	{
		// Token: 0x06003CEA RID: 15594 RVA: 0x0013161C File Offset: 0x0012F81C
		public override void Reset()
		{
			this.vector2Variable = null;
			this.everyFrame = false;
		}

		// Token: 0x06003CEB RID: 15595 RVA: 0x0013162C File Offset: 0x0012F82C
		public override void OnEnter()
		{
			this.vector2Variable.Value = this.vector2Variable.Value.normalized;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x00131668 File Offset: 0x0012F868
		public override void OnUpdate()
		{
			this.vector2Variable.Value = this.vector2Variable.Value.normalized;
		}

		// Token: 0x04002F2E RID: 12078
		[Tooltip("The vector to normalize")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector2 vector2Variable;

		// Token: 0x04002F2F RID: 12079
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
	}
}
