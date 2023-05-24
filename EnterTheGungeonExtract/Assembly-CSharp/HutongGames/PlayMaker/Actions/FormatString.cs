using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000958 RID: 2392
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Replaces each format item in a specified string with the text equivalent of variable's value. Stores the result in a string variable.")]
	public class FormatString : FsmStateAction
	{
		// Token: 0x0600343D RID: 13373 RVA: 0x0010F810 File Offset: 0x0010DA10
		public override void Reset()
		{
			this.format = null;
			this.variables = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x0600343E RID: 13374 RVA: 0x0010F830 File Offset: 0x0010DA30
		public override void OnEnter()
		{
			this.objectArray = new object[this.variables.Length];
			this.DoFormatString();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x0010F85C File Offset: 0x0010DA5C
		public override void OnUpdate()
		{
			this.DoFormatString();
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x0010F864 File Offset: 0x0010DA64
		private void DoFormatString()
		{
			for (int i = 0; i < this.variables.Length; i++)
			{
				this.variables[i].UpdateValue();
				this.objectArray[i] = this.variables[i].GetValue();
			}
			try
			{
				this.storeResult.Value = string.Format(this.format.Value, this.objectArray);
			}
			catch (FormatException ex)
			{
				base.LogError(ex.Message);
				base.Finish();
			}
		}

		// Token: 0x0400255F RID: 9567
		[RequiredField]
		[Tooltip("E.g. Hello {0} and {1}\nWith 2 variables that replace {0} and {1}\nSee C# string.Format docs.")]
		public FsmString format;

		// Token: 0x04002560 RID: 9568
		[Tooltip("Variables to use for each formatting item.")]
		public FsmVar[] variables;

		// Token: 0x04002561 RID: 9569
		[Tooltip("Store the formatted result in a string variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString storeResult;

		// Token: 0x04002562 RID: 9570
		[Tooltip("Repeat every frame. This is useful if the variables are changing.")]
		public bool everyFrame;

		// Token: 0x04002563 RID: 9571
		private object[] objectArray;
	}
}
