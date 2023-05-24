using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008D7 RID: 2263
	[Tooltip("Quits the player application.")]
	[ActionCategory(ActionCategory.Application)]
	public class ApplicationQuit : FsmStateAction
	{
		// Token: 0x06003226 RID: 12838 RVA: 0x0010846C File Offset: 0x0010666C
		public override void Reset()
		{
		}

		// Token: 0x06003227 RID: 12839 RVA: 0x00108470 File Offset: 0x00106670
		public override void OnEnter()
		{
			Application.Quit();
			base.Finish();
		}
	}
}
