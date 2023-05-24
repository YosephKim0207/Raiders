using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CD9 RID: 3289
	[ActionCategory(".NPCs")]
	[Tooltip("Toss the current gun into the witches pot and (hopefully) get an upgrade.")]
	public class TossCurrentGunInPot : FsmStateAction
	{
		// Token: 0x060045D9 RID: 17881 RVA: 0x0016AB98 File Offset: 0x00168D98
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			this.m_cauldron = component.transform.parent.GetComponent<WitchCauldronController>();
			if (this.m_cauldron.TossPlayerEquippedGun(component.TalkingPlayer))
			{
				base.Fsm.Event(this.SuccessEvent);
			}
			base.Finish();
		}

		// Token: 0x060045DA RID: 17882 RVA: 0x0016ABF4 File Offset: 0x00168DF4
		public override void OnUpdate()
		{
			if (!this.m_cauldron.IsGunInPot)
			{
				base.Finish();
			}
		}

		// Token: 0x0400381E RID: 14366
		public FsmEvent SuccessEvent;

		// Token: 0x0400381F RID: 14367
		private WitchCauldronController m_cauldron;
	}
}
