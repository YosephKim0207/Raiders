using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B4C RID: 2892
	[Tooltip("Sets the value of a Vector2 Variable.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class SetVector2Value : FsmStateAction
	{
		// Token: 0x06003CB5 RID: 15541 RVA: 0x00130C08 File Offset: 0x0012EE08
		public override void Reset()
		{
			this.vector2Variable = null;
			this.vector2Value = null;
			this.everyFrame = false;
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x00130C20 File Offset: 0x0012EE20
		public override void OnEnter()
		{
			this.vector2Variable.Value = this.vector2Value.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003CB7 RID: 15543 RVA: 0x00130C4C File Offset: 0x0012EE4C
		public override void OnUpdate()
		{
			this.vector2Variable.Value = this.vector2Value.Value;
		}

		// Token: 0x04002EFC RID: 12028
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The vector2 target")]
		public FsmVector2 vector2Variable;

		// Token: 0x04002EFD RID: 12029
		[Tooltip("The vector2 source")]
		[RequiredField]
		public FsmVector2 vector2Value;

		// Token: 0x04002EFE RID: 12030
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
