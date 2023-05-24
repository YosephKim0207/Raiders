using System;
using Brave;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A7B RID: 2683
	[ActionCategory("PlayerPrefs")]
	[Tooltip("Removes key and its corresponding value from the preferences.")]
	public class PlayerPrefsDeleteKey : FsmStateAction
	{
		// Token: 0x06003907 RID: 14599 RVA: 0x001247D4 File Offset: 0x001229D4
		public override void Reset()
		{
			this.key = string.Empty;
		}

		// Token: 0x06003908 RID: 14600 RVA: 0x001247E8 File Offset: 0x001229E8
		public override void OnEnter()
		{
			if (!this.key.IsNone && !this.key.Value.Equals(string.Empty))
			{
				PlayerPrefs.DeleteKey(this.key.Value);
			}
			base.Finish();
		}

		// Token: 0x04002B5E RID: 11102
		public FsmString key;
	}
}
