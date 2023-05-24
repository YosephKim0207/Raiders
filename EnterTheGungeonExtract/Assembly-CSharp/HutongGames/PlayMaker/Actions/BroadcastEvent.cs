using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000904 RID: 2308
	[Obsolete("This action is obsolete; use Send Event with Event Target instead.")]
	[Tooltip("Sends an Event to all FSMs in the scene or to all FSMs on a Game Object.\nNOTE: This action won't work on the very first frame of the game...")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class BroadcastEvent : FsmStateAction
	{
		// Token: 0x060032D0 RID: 13008 RVA: 0x0010A944 File Offset: 0x00108B44
		public override void Reset()
		{
			this.broadcastEvent = null;
			this.gameObject = null;
			this.sendToChildren = false;
			this.excludeSelf = false;
		}

		// Token: 0x060032D1 RID: 13009 RVA: 0x0010A96C File Offset: 0x00108B6C
		public override void OnEnter()
		{
			if (!string.IsNullOrEmpty(this.broadcastEvent.Value))
			{
				if (this.gameObject.Value != null)
				{
					base.Fsm.BroadcastEventToGameObject(this.gameObject.Value, this.broadcastEvent.Value, this.sendToChildren.Value, this.excludeSelf.Value);
				}
				else
				{
					base.Fsm.BroadcastEvent(this.broadcastEvent.Value, this.excludeSelf.Value);
				}
			}
			base.Finish();
		}

		// Token: 0x040023FA RID: 9210
		[RequiredField]
		public FsmString broadcastEvent;

		// Token: 0x040023FB RID: 9211
		[Tooltip("Optionally specify a game object to broadcast the event to all FSMs on that game object.")]
		public FsmGameObject gameObject;

		// Token: 0x040023FC RID: 9212
		[Tooltip("Broadcast to all FSMs on the game object's children.")]
		public FsmBool sendToChildren;

		// Token: 0x040023FD RID: 9213
		public FsmBool excludeSelf;
	}
}
