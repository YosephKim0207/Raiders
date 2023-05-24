using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B25 RID: 2853
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Stops location service updates. This could be useful for saving battery life.")]
	public class StopLocationServiceUpdates : FsmStateAction
	{
		// Token: 0x06003C18 RID: 15384 RVA: 0x0012EAF0 File Offset: 0x0012CCF0
		public override void Reset()
		{
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x0012EAF4 File Offset: 0x0012CCF4
		public override void OnEnter()
		{
			base.Finish();
		}
	}
}
