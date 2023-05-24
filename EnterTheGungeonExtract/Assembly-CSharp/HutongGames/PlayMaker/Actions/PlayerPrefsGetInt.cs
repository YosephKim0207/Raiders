using System;
using Brave;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A7D RID: 2685
	[Tooltip("Returns the value corresponding to key in the preference file if it exists.")]
	[ActionCategory("PlayerPrefs")]
	public class PlayerPrefsGetInt : FsmStateAction
	{
		// Token: 0x0600390D RID: 14605 RVA: 0x00124908 File Offset: 0x00122B08
		public override void Reset()
		{
			this.keys = new FsmString[1];
			this.variables = new FsmInt[1];
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x00124924 File Offset: 0x00122B24
		public override void OnEnter()
		{
			for (int i = 0; i < this.keys.Length; i++)
			{
				if (!this.keys[i].IsNone || !this.keys[i].Value.Equals(string.Empty))
				{
					this.variables[i].Value = PlayerPrefs.GetInt(this.keys[i].Value, (!this.variables[i].IsNone) ? this.variables[i].Value : 0);
				}
			}
			base.Finish();
		}

		// Token: 0x04002B61 RID: 11105
		[Tooltip("Case sensitive key.")]
		[CompoundArray("Count", "Key", "Variable")]
		public FsmString[] keys;

		// Token: 0x04002B62 RID: 11106
		[UIHint(UIHint.Variable)]
		public FsmInt[] variables;
	}
}
