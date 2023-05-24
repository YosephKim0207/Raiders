using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CAC RID: 3244
	public class PrepareTakeSherpaPickup : FsmStateAction
	{
		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x06004545 RID: 17733 RVA: 0x00166DCC File Offset: 0x00164FCC
		public static bool IsOnSherpaMoneyStep
		{
			get
			{
				if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_READY_FOR_UNLOCKS))
				{
					if (!GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK1_COMPLETE))
					{
						return GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK1_ELEMENT1) && !GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK1_ELEMENT2);
					}
					if (!GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK2_COMPLETE))
					{
						return GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK2_ELEMENT1) && !GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK2_ELEMENT2);
					}
					if (!GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK3_COMPLETE))
					{
						return GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK3_ELEMENT1) && !GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK3_ELEMENT2);
					}
					if (!GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK4_COMPLETE))
					{
						return GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK4_ELEMENT1) && !GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK4_ELEMENT2);
					}
				}
				return false;
			}
		}

		// Token: 0x06004546 RID: 17734 RVA: 0x00166EE8 File Offset: 0x001650E8
		public override void OnEnter()
		{
			if (this.m_parentAction == null)
			{
				for (int i = 0; i < base.Fsm.PreviousActiveState.Actions.Length; i++)
				{
					if (base.Fsm.PreviousActiveState.Actions[i] is SherpaDetectItem)
					{
						this.m_parentAction = base.Fsm.PreviousActiveState.Actions[i] as SherpaDetectItem;
						break;
					}
				}
			}
			for (int j = 0; j < base.Fsm.ActiveState.Actions.Length; j++)
			{
				if (base.Fsm.ActiveState.Actions[j] is TakeSherpaPickup)
				{
					TakeSherpaPickup takeSherpaPickup = base.Fsm.ActiveState.Actions[j] as TakeSherpaPickup;
					takeSherpaPickup.parentAction = this.m_parentAction;
					break;
				}
			}
			if (this.CurrentPickupTargetIndex >= this.m_parentAction.AllValidTargets.Count)
			{
				this.CurrentPickupTargetIndex = -1;
			}
			this.CurrentPickupTargetIndex++;
			if (this.CurrentPickupTargetIndex >= this.m_parentAction.AllValidTargets.Count || this.CurrentPickupTargetIndex < 0)
			{
				base.Fsm.Event(this.OnOutOfItems);
				base.Finish();
				return;
			}
			PickupObject pickupObject = this.m_parentAction.AllValidTargets[this.CurrentPickupTargetIndex];
			FsmString fsmString = base.Fsm.Variables.GetFsmString("npcReplacementString");
			EncounterTrackable component = pickupObject.GetComponent<EncounterTrackable>();
			if (fsmString != null && component != null)
			{
				fsmString.Value = component.journalData.GetPrimaryDisplayName(false);
			}
			base.Finish();
		}

		// Token: 0x04003763 RID: 14179
		public FsmEvent OnOutOfItems;

		// Token: 0x04003764 RID: 14180
		[NonSerialized]
		public int CurrentPickupTargetIndex = -1;

		// Token: 0x04003765 RID: 14181
		private SherpaDetectItem m_parentAction;
	}
}
