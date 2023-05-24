using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B28 RID: 2856
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Splits a string into substrings using separator characters.")]
	public class StringSplit : FsmStateAction
	{
		// Token: 0x06003C20 RID: 15392 RVA: 0x0012EBB4 File Offset: 0x0012CDB4
		public override void Reset()
		{
			this.stringToSplit = null;
			this.separators = null;
			this.trimStrings = false;
			this.trimChars = null;
			this.stringArray = null;
		}

		// Token: 0x06003C21 RID: 15393 RVA: 0x0012EBE0 File Offset: 0x0012CDE0
		public override void OnEnter()
		{
			char[] array = this.trimChars.Value.ToCharArray();
			if (!this.stringToSplit.IsNone && !this.stringArray.IsNone)
			{
				this.stringArray.Values = this.stringToSplit.Value.Split(this.separators.Value.ToCharArray());
				if (this.trimStrings.Value)
				{
					for (int i = 0; i < this.stringArray.Values.Length; i++)
					{
						string text = this.stringArray.Values[i] as string;
						if (text != null)
						{
							if (!this.trimChars.IsNone && array.Length > 0)
							{
								this.stringArray.Set(i, text.Trim(array));
							}
							else
							{
								this.stringArray.Set(i, text.Trim());
							}
						}
					}
				}
				this.stringArray.SaveChanges();
			}
			base.Finish();
		}

		// Token: 0x04002E4B RID: 11851
		[UIHint(UIHint.Variable)]
		[Tooltip("String to split.")]
		public FsmString stringToSplit;

		// Token: 0x04002E4C RID: 11852
		[Tooltip("Characters used to split the string.\nUse '\\n' for newline\nUse '\\t' for tab")]
		public FsmString separators;

		// Token: 0x04002E4D RID: 11853
		[Tooltip("Remove all leading and trailing white-space characters from each seperated string.")]
		public FsmBool trimStrings;

		// Token: 0x04002E4E RID: 11854
		[Tooltip("Optional characters used to trim each seperated string.")]
		public FsmString trimChars;

		// Token: 0x04002E4F RID: 11855
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.String, "", 0, 0, 65536)]
		[Tooltip("Store the split strings in a String Array.")]
		public FsmArray stringArray;
	}
}
