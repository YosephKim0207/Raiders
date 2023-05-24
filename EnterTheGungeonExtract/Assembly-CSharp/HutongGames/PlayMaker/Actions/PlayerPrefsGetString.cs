using System;
using Brave;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A7E RID: 2686
	[Tooltip("Returns the value corresponding to key in the preference file if it exists.")]
	[ActionCategory("PlayerPrefs")]
	public class PlayerPrefsGetString : FsmStateAction
	{
		// Token: 0x06003910 RID: 14608 RVA: 0x001249CC File Offset: 0x00122BCC
		public override void Reset()
		{
			this.keys = new FsmString[1];
			this.variables = new FsmString[1];
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x001249E8 File Offset: 0x00122BE8
		public override void OnEnter()
		{
			for (int i = 0; i < this.keys.Length; i++)
			{
				if (!this.keys[i].IsNone || !this.keys[i].Value.Equals(string.Empty))
				{
					this.variables[i].Value = PlayerPrefs.GetString(this.keys[i].Value, (!this.variables[i].IsNone) ? this.variables[i].Value : string.Empty);
				}
			}
			base.Finish();
		}

		// Token: 0x04002B63 RID: 11107
		[CompoundArray("Count", "Key", "Variable")]
		[Tooltip("Case sensitive key.")]
		public FsmString[] keys;

		// Token: 0x04002B64 RID: 11108
		[UIHint(UIHint.Variable)]
		public FsmString[] variables;
	}
}
