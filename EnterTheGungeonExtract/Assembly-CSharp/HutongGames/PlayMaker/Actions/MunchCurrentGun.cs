using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CA6 RID: 3238
	[Tooltip("Toss the current gun into the gunper monper and (hopefully) get an upgrade.")]
	[ActionCategory(".NPCs")]
	public class MunchCurrentGun : FsmStateAction
	{
		// Token: 0x06004531 RID: 17713 RVA: 0x00166A00 File Offset: 0x00164C00
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			this.m_muncher = component.GetComponent<GunberMuncherController>();
			this.m_muncher.TossPlayerEquippedGun(component.TalkingPlayer);
		}

		// Token: 0x06004532 RID: 17714 RVA: 0x00166A38 File Offset: 0x00164C38
		public override void OnUpdate()
		{
			if (this.m_muncher.IsProcessing)
			{
				return;
			}
			if (this.m_muncher.ShouldGiveReward)
			{
				base.Fsm.Event(this.rewardGivenEvent);
			}
			else
			{
				base.Fsm.Event(this.noRewardEvent);
			}
		}

		// Token: 0x04003758 RID: 14168
		public FsmEvent rewardGivenEvent;

		// Token: 0x04003759 RID: 14169
		public FsmEvent noRewardEvent;

		// Token: 0x0400375A RID: 14170
		private GunberMuncherController m_muncher;
	}
}
