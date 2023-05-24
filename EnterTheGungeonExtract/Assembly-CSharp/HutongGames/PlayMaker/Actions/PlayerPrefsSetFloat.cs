using System;
using Brave;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A80 RID: 2688
	[ActionCategory("PlayerPrefs")]
	[Tooltip("Sets the value of the preference identified by key.")]
	public class PlayerPrefsSetFloat : FsmStateAction
	{
		// Token: 0x06003916 RID: 14614 RVA: 0x00124B34 File Offset: 0x00122D34
		public override void Reset()
		{
			this.keys = new FsmString[1];
			this.values = new FsmFloat[1];
		}

		// Token: 0x06003917 RID: 14615 RVA: 0x00124B50 File Offset: 0x00122D50
		public override void OnEnter()
		{
			for (int i = 0; i < this.keys.Length; i++)
			{
				if (!this.keys[i].IsNone || !this.keys[i].Value.Equals(string.Empty))
				{
					PlayerPrefs.SetFloat(this.keys[i].Value, (!this.values[i].IsNone) ? this.values[i].Value : 0f);
				}
			}
			base.Finish();
		}

		// Token: 0x04002B69 RID: 11113
		[CompoundArray("Count", "Key", "Value")]
		[Tooltip("Case sensitive key.")]
		public FsmString[] keys;

		// Token: 0x04002B6A RID: 11114
		public FsmFloat[] values;
	}
}
