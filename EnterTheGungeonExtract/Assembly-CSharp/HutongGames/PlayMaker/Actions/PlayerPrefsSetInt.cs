using System;
using Brave;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A81 RID: 2689
	[ActionCategory("PlayerPrefs")]
	[Tooltip("Sets the value of the preference identified by key.")]
	public class PlayerPrefsSetInt : FsmStateAction
	{
		// Token: 0x06003919 RID: 14617 RVA: 0x00124BF0 File Offset: 0x00122DF0
		public override void Reset()
		{
			this.keys = new FsmString[1];
			this.values = new FsmInt[1];
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x00124C0C File Offset: 0x00122E0C
		public override void OnEnter()
		{
			for (int i = 0; i < this.keys.Length; i++)
			{
				if (!this.keys[i].IsNone || !this.keys[i].Value.Equals(string.Empty))
				{
					PlayerPrefs.SetInt(this.keys[i].Value, (!this.values[i].IsNone) ? this.values[i].Value : 0);
				}
			}
			base.Finish();
		}

		// Token: 0x04002B6B RID: 11115
		[Tooltip("Case sensitive key.")]
		[CompoundArray("Count", "Key", "Value")]
		public FsmString[] keys;

		// Token: 0x04002B6C RID: 11116
		public FsmInt[] values;
	}
}
