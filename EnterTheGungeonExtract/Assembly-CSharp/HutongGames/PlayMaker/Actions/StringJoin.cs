using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B27 RID: 2855
	[Tooltip("Join an array of strings into a single string.")]
	[ActionCategory(ActionCategory.String)]
	public class StringJoin : FsmStateAction
	{
		// Token: 0x06003C1E RID: 15390 RVA: 0x0012EB50 File Offset: 0x0012CD50
		public override void OnEnter()
		{
			if (!this.stringArray.IsNone && !this.storeResult.IsNone)
			{
				this.storeResult.Value = string.Join(this.separator.Value, this.stringArray.stringValues);
			}
			base.Finish();
		}

		// Token: 0x04002E48 RID: 11848
		[Tooltip("Array of string to join into a single string.")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.String, "", 0, 0, 65536)]
		public FsmArray stringArray;

		// Token: 0x04002E49 RID: 11849
		[Tooltip("Seperator to add between each string.")]
		public FsmString separator;

		// Token: 0x04002E4A RID: 11850
		[Tooltip("Store the joined string in string variable.")]
		[UIHint(UIHint.Variable)]
		public FsmString storeResult;
	}
}
