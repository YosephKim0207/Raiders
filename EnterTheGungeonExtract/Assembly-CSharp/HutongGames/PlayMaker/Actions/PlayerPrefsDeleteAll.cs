using System;
using Brave;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A7A RID: 2682
	[ActionCategory("PlayerPrefs")]
	[Tooltip("Removes all keys and values from the preferences. Use with caution.")]
	public class PlayerPrefsDeleteAll : FsmStateAction
	{
		// Token: 0x06003904 RID: 14596 RVA: 0x001247B8 File Offset: 0x001229B8
		public override void Reset()
		{
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x001247BC File Offset: 0x001229BC
		public override void OnEnter()
		{
			PlayerPrefs.DeleteAll();
			base.Finish();
		}
	}
}
