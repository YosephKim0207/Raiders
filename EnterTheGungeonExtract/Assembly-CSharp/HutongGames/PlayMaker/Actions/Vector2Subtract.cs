using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B5D RID: 2909
	[Tooltip("Subtracts a Vector2 value from a Vector2 variable.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2Subtract : FsmStateAction
	{
		// Token: 0x06003CFB RID: 15611 RVA: 0x00131A60 File Offset: 0x0012FC60
		public override void Reset()
		{
			this.vector2Variable = null;
			this.subtractVector = new FsmVector2
			{
				UseVariable = true
			};
			this.everyFrame = false;
		}

		// Token: 0x06003CFC RID: 15612 RVA: 0x00131A90 File Offset: 0x0012FC90
		public override void OnEnter()
		{
			this.vector2Variable.Value = this.vector2Variable.Value - this.subtractVector.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003CFD RID: 15613 RVA: 0x00131ACC File Offset: 0x0012FCCC
		public override void OnUpdate()
		{
			this.vector2Variable.Value = this.vector2Variable.Value - this.subtractVector.Value;
		}

		// Token: 0x04002F47 RID: 12103
		[Tooltip("The Vector2 operand")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector2 vector2Variable;

		// Token: 0x04002F48 RID: 12104
		[Tooltip("The vector2 to substract with")]
		[RequiredField]
		public FsmVector2 subtractVector;

		// Token: 0x04002F49 RID: 12105
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
	}
}
