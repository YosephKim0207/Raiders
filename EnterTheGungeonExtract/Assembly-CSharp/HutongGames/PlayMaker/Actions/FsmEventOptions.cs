using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200095B RID: 2395
	[Tooltip("Sets how subsequent events sent in this state are handled.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class FsmEventOptions : FsmStateAction
	{
		// Token: 0x06003448 RID: 13384 RVA: 0x0010FA30 File Offset: 0x0010DC30
		public override void Reset()
		{
			this.sendToFsmComponent = null;
			this.sendToGameObject = null;
			this.fsmName = string.Empty;
			this.sendToChildren = false;
			this.broadcastToAll = false;
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x0010FA68 File Offset: 0x0010DC68
		public override void OnUpdate()
		{
		}

		// Token: 0x0400256A RID: 9578
		public PlayMakerFSM sendToFsmComponent;

		// Token: 0x0400256B RID: 9579
		public FsmGameObject sendToGameObject;

		// Token: 0x0400256C RID: 9580
		public FsmString fsmName;

		// Token: 0x0400256D RID: 9581
		public FsmBool sendToChildren;

		// Token: 0x0400256E RID: 9582
		public FsmBool broadcastToAll;
	}
}
