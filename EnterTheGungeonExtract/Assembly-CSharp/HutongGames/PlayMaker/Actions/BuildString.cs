using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000905 RID: 2309
	[Tooltip("Builds a String from other Strings.")]
	[ActionCategory(ActionCategory.String)]
	public class BuildString : FsmStateAction
	{
		// Token: 0x060032D3 RID: 13011 RVA: 0x0010AA10 File Offset: 0x00108C10
		public override void Reset()
		{
			this.stringParts = new FsmString[3];
			this.separator = null;
			this.addToEnd = true;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x060032D4 RID: 13012 RVA: 0x0010AA40 File Offset: 0x00108C40
		public override void OnEnter()
		{
			this.DoBuildString();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060032D5 RID: 13013 RVA: 0x0010AA5C File Offset: 0x00108C5C
		public override void OnUpdate()
		{
			this.DoBuildString();
		}

		// Token: 0x060032D6 RID: 13014 RVA: 0x0010AA64 File Offset: 0x00108C64
		private void DoBuildString()
		{
			if (this.storeResult == null)
			{
				return;
			}
			this.result = string.Empty;
			for (int i = 0; i < this.stringParts.Length - 1; i++)
			{
				this.result += this.stringParts[i];
				this.result += this.separator.Value;
			}
			this.result += this.stringParts[this.stringParts.Length - 1];
			if (this.addToEnd.Value)
			{
				this.result += this.separator.Value;
			}
			this.storeResult.Value = this.result;
		}

		// Token: 0x040023FE RID: 9214
		[RequiredField]
		[Tooltip("Array of Strings to combine.")]
		public FsmString[] stringParts;

		// Token: 0x040023FF RID: 9215
		[Tooltip("Separator to insert between each String. E.g. space character.")]
		public FsmString separator;

		// Token: 0x04002400 RID: 9216
		[Tooltip("Add Separator to end of built string.")]
		public FsmBool addToEnd;

		// Token: 0x04002401 RID: 9217
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the final String in a variable.")]
		[RequiredField]
		public FsmString storeResult;

		// Token: 0x04002402 RID: 9218
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		// Token: 0x04002403 RID: 9219
		private string result;
	}
}
