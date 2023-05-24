using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008F9 RID: 2297
	public abstract class BaseLogAction : FsmStateAction
	{
		// Token: 0x060032A5 RID: 12965 RVA: 0x0010A164 File Offset: 0x00108364
		public override void Reset()
		{
			this.sendToUnityLog = false;
		}

		// Token: 0x040023CB RID: 9163
		public bool sendToUnityLog;
	}
}
