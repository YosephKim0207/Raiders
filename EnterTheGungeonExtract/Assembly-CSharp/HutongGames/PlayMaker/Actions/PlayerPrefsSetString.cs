using System;
using Brave;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A82 RID: 2690
	[Tooltip("Sets the value of the preference identified by key.")]
	[ActionCategory("PlayerPrefs")]
	public class PlayerPrefsSetString : FsmStateAction
	{
		// Token: 0x0600391C RID: 14620 RVA: 0x00124CA8 File Offset: 0x00122EA8
		public override void Reset()
		{
			this.keys = new FsmString[1];
			this.values = new FsmString[1];
		}

		// Token: 0x0600391D RID: 14621 RVA: 0x00124CC4 File Offset: 0x00122EC4
		public override void OnEnter()
		{
			for (int i = 0; i < this.keys.Length; i++)
			{
				if (!this.keys[i].IsNone || !this.keys[i].Value.Equals(string.Empty))
				{
					PlayerPrefs.SetString(this.keys[i].Value, (!this.values[i].IsNone) ? this.values[i].Value : string.Empty);
				}
			}
			base.Finish();
		}

		// Token: 0x04002B6D RID: 11117
		[CompoundArray("Count", "Key", "Value")]
		[Tooltip("Case sensitive key.")]
		public FsmString[] keys;

		// Token: 0x04002B6E RID: 11118
		public FsmString[] values;
	}
}
