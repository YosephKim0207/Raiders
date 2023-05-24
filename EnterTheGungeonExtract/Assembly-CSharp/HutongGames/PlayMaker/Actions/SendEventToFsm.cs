using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000ABB RID: 2747
	[Tooltip("Sends an Event to another Fsm after an optional delay. Specify an Fsm Name or use the first Fsm on the object.")]
	[Obsolete("This action is obsolete; use Send Event with Event Target instead.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SendEventToFsm : FsmStateAction
	{
		// Token: 0x06003A45 RID: 14917 RVA: 0x001288A8 File Offset: 0x00126AA8
		public override void Reset()
		{
			this.gameObject = null;
			this.fsmName = null;
			this.sendEvent = null;
			this.delay = null;
			this.requireReceiver = false;
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x001288D0 File Offset: 0x00126AD0
		public override void OnEnter()
		{
			this.go = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (this.go == null)
			{
				base.Finish();
				return;
			}
			PlayMakerFSM gameObjectFsm = ActionHelpers.GetGameObjectFsm(this.go, this.fsmName.Value);
			if (gameObjectFsm == null)
			{
				if (this.requireReceiver)
				{
					base.LogError("GameObject doesn't have FsmComponent: " + this.go.name + " " + this.fsmName.Value);
				}
				return;
			}
			if ((double)this.delay.Value < 0.001)
			{
				gameObjectFsm.Fsm.Event(this.sendEvent.Value);
				base.Finish();
			}
			else
			{
				this.delayedEvent = gameObjectFsm.Fsm.DelayedEvent(FsmEvent.GetFsmEvent(this.sendEvent.Value), this.delay.Value);
			}
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x001289D0 File Offset: 0x00126BD0
		public override void OnUpdate()
		{
			if (DelayedEvent.WasSent(this.delayedEvent))
			{
				base.Finish();
			}
		}

		// Token: 0x04002C76 RID: 11382
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002C77 RID: 11383
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of Fsm on Game Object")]
		public FsmString fsmName;

		// Token: 0x04002C78 RID: 11384
		[RequiredField]
		[UIHint(UIHint.FsmEvent)]
		public FsmString sendEvent;

		// Token: 0x04002C79 RID: 11385
		[HasFloatSlider(0f, 10f)]
		public FsmFloat delay;

		// Token: 0x04002C7A RID: 11386
		private bool requireReceiver;

		// Token: 0x04002C7B RID: 11387
		private GameObject go;

		// Token: 0x04002C7C RID: 11388
		private DelayedEvent delayedEvent;
	}
}
