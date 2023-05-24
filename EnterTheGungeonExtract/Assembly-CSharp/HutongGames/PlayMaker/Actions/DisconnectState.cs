using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C57 RID: 3159
	[Tooltip("Removes all transitions to this state.")]
	[ActionCategory(".Brave")]
	public class DisconnectState : FsmStateAction
	{
		// Token: 0x0600440B RID: 17419 RVA: 0x0015F894 File Offset: 0x0015DA94
		public override void OnEnter()
		{
			BravePlayMakerUtility.DisconnectState(base.State);
			base.Finish();
		}
	}
}
