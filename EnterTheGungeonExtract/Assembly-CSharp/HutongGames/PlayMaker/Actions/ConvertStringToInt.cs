using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000920 RID: 2336
	[ActionCategory(ActionCategory.Convert)]
	[Tooltip("Converts an String value to an Int value.")]
	public class ConvertStringToInt : FsmStateAction
	{
		// Token: 0x06003365 RID: 13157 RVA: 0x0010CF04 File Offset: 0x0010B104
		public override void Reset()
		{
			this.intVariable = null;
			this.stringVariable = null;
			this.everyFrame = false;
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x0010CF1C File Offset: 0x0010B11C
		public override void OnEnter()
		{
			this.DoConvertStringToInt();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x0010CF38 File Offset: 0x0010B138
		public override void OnUpdate()
		{
			this.DoConvertStringToInt();
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x0010CF40 File Offset: 0x0010B140
		private void DoConvertStringToInt()
		{
			this.intVariable.Value = int.Parse(this.stringVariable.Value);
		}

		// Token: 0x04002490 RID: 9360
		[Tooltip("The String variable to convert to an integer.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		// Token: 0x04002491 RID: 9361
		[Tooltip("Store the result in an Int variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt intVariable;

		// Token: 0x04002492 RID: 9362
		[Tooltip("Repeat every frame. Useful if the String variable is changing.")]
		public bool everyFrame;
	}
}
