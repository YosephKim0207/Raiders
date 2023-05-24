using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C50 RID: 3152
	[Tooltip("Sends Events.")]
	[ActionCategory(".Brave")]
	public class BroadcastEventInRoom : BraveFsmStateAction
	{
		// Token: 0x060043E9 RID: 17385 RVA: 0x0015ECC0 File Offset: 0x0015CEC0
		public override void OnEnter()
		{
			base.OnEnter();
			GameManager.BroadcastRoomFsmEvent(this.eventString.Value, base.Owner.transform.position.GetAbsoluteRoom());
			base.Finish();
		}

		// Token: 0x0400360A RID: 13834
		public FsmString eventString;
	}
}
