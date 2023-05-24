using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B53 RID: 2899
	[Tooltip("Reverses the direction of a Vector2 Variable. Same as multiplying by -1.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2Invert : FsmStateAction
	{
		// Token: 0x06003CD5 RID: 15573 RVA: 0x0013127C File Offset: 0x0012F47C
		public override void Reset()
		{
			this.vector2Variable = null;
			this.everyFrame = false;
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x0013128C File Offset: 0x0012F48C
		public override void OnEnter()
		{
			this.vector2Variable.Value = this.vector2Variable.Value * -1f;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x001312C0 File Offset: 0x0012F4C0
		public override void OnUpdate()
		{
			this.vector2Variable.Value = this.vector2Variable.Value * -1f;
		}

		// Token: 0x04002F1C RID: 12060
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The vector to invert")]
		public FsmVector2 vector2Variable;

		// Token: 0x04002F1D RID: 12061
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
	}
}
