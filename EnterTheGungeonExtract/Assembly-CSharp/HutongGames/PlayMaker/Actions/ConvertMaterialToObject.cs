using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200091E RID: 2334
	[ActionCategory(ActionCategory.Convert)]
	[Tooltip("Converts a Material variable to an Object variable. Useful if you want to use Set Property (which only works on Object variables).")]
	public class ConvertMaterialToObject : FsmStateAction
	{
		// Token: 0x0600335B RID: 13147 RVA: 0x0010CD50 File Offset: 0x0010AF50
		public override void Reset()
		{
			this.materialVariable = null;
			this.objectVariable = null;
			this.everyFrame = false;
		}

		// Token: 0x0600335C RID: 13148 RVA: 0x0010CD68 File Offset: 0x0010AF68
		public override void OnEnter()
		{
			this.DoConvertMaterialToObject();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600335D RID: 13149 RVA: 0x0010CD84 File Offset: 0x0010AF84
		public override void OnUpdate()
		{
			this.DoConvertMaterialToObject();
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x0010CD8C File Offset: 0x0010AF8C
		private void DoConvertMaterialToObject()
		{
			this.objectVariable.Value = this.materialVariable.Value;
		}

		// Token: 0x04002489 RID: 9353
		[Tooltip("The Material variable to convert to an Object.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmMaterial materialVariable;

		// Token: 0x0400248A RID: 9354
		[Tooltip("Store the result in an Object variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmObject objectVariable;

		// Token: 0x0400248B RID: 9355
		[Tooltip("Repeat every frame. Useful if the Material variable is changing.")]
		public bool everyFrame;
	}
}
