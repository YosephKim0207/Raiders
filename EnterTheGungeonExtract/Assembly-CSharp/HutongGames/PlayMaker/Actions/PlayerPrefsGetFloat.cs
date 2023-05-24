using System;
using Brave;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A7C RID: 2684
	[ActionCategory("PlayerPrefs")]
	[Tooltip("Returns the value corresponding to key in the preference file if it exists.")]
	public class PlayerPrefsGetFloat : FsmStateAction
	{
		// Token: 0x0600390A RID: 14602 RVA: 0x00124840 File Offset: 0x00122A40
		public override void Reset()
		{
			this.keys = new FsmString[1];
			this.variables = new FsmFloat[1];
		}

		// Token: 0x0600390B RID: 14603 RVA: 0x0012485C File Offset: 0x00122A5C
		public override void OnEnter()
		{
			for (int i = 0; i < this.keys.Length; i++)
			{
				if (!this.keys[i].IsNone || !this.keys[i].Value.Equals(string.Empty))
				{
					this.variables[i].Value = PlayerPrefs.GetFloat(this.keys[i].Value, (!this.variables[i].IsNone) ? this.variables[i].Value : 0f);
				}
			}
			base.Finish();
		}

		// Token: 0x04002B5F RID: 11103
		[Tooltip("Case sensitive key.")]
		[CompoundArray("Count", "Key", "Variable")]
		public FsmString[] keys;

		// Token: 0x04002B60 RID: 11104
		[UIHint(UIHint.Variable)]
		public FsmFloat[] variables;
	}
}
