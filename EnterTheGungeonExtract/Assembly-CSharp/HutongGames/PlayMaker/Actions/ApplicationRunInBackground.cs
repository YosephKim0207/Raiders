using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008D8 RID: 2264
	[Tooltip("Sets if the Application should play in the background. Useful for servers or testing network games on one machine.")]
	[ActionCategory(ActionCategory.Application)]
	public class ApplicationRunInBackground : FsmStateAction
	{
		// Token: 0x06003229 RID: 12841 RVA: 0x00108488 File Offset: 0x00106688
		public override void Reset()
		{
			this.runInBackground = true;
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x00108498 File Offset: 0x00106698
		public override void OnEnter()
		{
			Application.runInBackground = this.runInBackground.Value;
			base.Finish();
		}

		// Token: 0x04002348 RID: 9032
		public FsmBool runInBackground;
	}
}
