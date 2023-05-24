using System;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C4F RID: 3151
	public class BlacksmithDetectItem : FsmStateAction
	{
		// Token: 0x060043E0 RID: 17376 RVA: 0x0015E7EC File Offset: 0x0015C9EC
		public DesiredItem GetCurrentDesire()
		{
			return this.desires[this.m_currentDesireIndex];
		}

		// Token: 0x060043E1 RID: 17377 RVA: 0x0015E7FC File Offset: 0x0015C9FC
		public PickupObject GetTargetPickupObject()
		{
			return this.m_currentTarget;
		}

		// Token: 0x060043E2 RID: 17378 RVA: 0x0015E804 File Offset: 0x0015CA04
		public override void Reset()
		{
		}

		// Token: 0x060043E3 RID: 17379 RVA: 0x0015E808 File Offset: 0x0015CA08
		public override string ErrorCheck()
		{
			return string.Empty;
		}

		// Token: 0x060043E4 RID: 17380 RVA: 0x0015E81C File Offset: 0x0015CA1C
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			this.talkingPlayer = component.TalkingPlayer;
			this.m_hasNonItemTarget = false;
			this.DoCheck();
			base.Finish();
		}

		// Token: 0x060043E5 RID: 17381 RVA: 0x0015E854 File Offset: 0x0015CA54
		private void CheckPlayerForDesire(DesiredItem desire)
		{
			this.m_currentTargets = new List<PickupObject>();
			if (desire.type == DesiredItem.DetectType.SPECIFIC_ITEM)
			{
				PickupObject byId = PickupObjectDatabase.GetById(desire.specificItemId);
				if (byId is Gun)
				{
					for (int i = 0; i < this.talkingPlayer.inventory.AllGuns.Count; i++)
					{
						if (this.talkingPlayer.inventory.AllGuns[i].PickupObjectId == byId.PickupObjectId)
						{
							this.m_currentTargets.Add(byId);
						}
					}
				}
				else if (byId is PlayerItem)
				{
					for (int j = 0; j < this.talkingPlayer.activeItems.Count; j++)
					{
						if (this.talkingPlayer.activeItems[j].PickupObjectId == byId.PickupObjectId)
						{
							this.m_currentTargets.Add(byId);
						}
					}
				}
				else if (byId is PassiveItem)
				{
					for (int k = 0; k < GameManager.Instance.PrimaryPlayer.passiveItems.Count; k++)
					{
						if (this.talkingPlayer.passiveItems[k].PickupObjectId == byId.PickupObjectId)
						{
							this.m_currentTargets.Add(byId);
						}
					}
				}
			}
			else if (desire.type == DesiredItem.DetectType.CURRENCY)
			{
				if (this.talkingPlayer.carriedConsumables.Currency >= desire.amount)
				{
					this.m_hasNonItemTarget = true;
				}
			}
			else if (desire.type == DesiredItem.DetectType.META_CURRENCY)
			{
				int num = Mathf.RoundToInt(GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.META_CURRENCY));
				if (num >= desire.amount)
				{
					this.m_hasNonItemTarget = true;
				}
			}
			else if (desire.type == DesiredItem.DetectType.KEYS && this.talkingPlayer.carriedConsumables.KeyBullets >= desire.amount)
			{
				this.m_hasNonItemTarget = true;
			}
		}

		// Token: 0x060043E6 RID: 17382 RVA: 0x0015EA44 File Offset: 0x0015CC44
		private void NextDesire()
		{
			if (this.m_currentTarget != null)
			{
				return;
			}
			this.m_currentTargets = null;
			this.m_currentTargetIndex = 0;
			this.m_currentDesireIndex++;
			base.Fsm.Event(this.NextDesireEvent);
		}

		// Token: 0x060043E7 RID: 17383 RVA: 0x0015EA90 File Offset: 0x0015CC90
		private void DoCheck()
		{
			this.m_currentTarget = null;
			this.m_hasNonItemTarget = false;
			if (this.m_currentDesireIndex >= this.desires.Length)
			{
				this.m_currentDesireIndex = 0;
				this.m_currentTargets = null;
				this.m_currentTargetIndex = 0;
				base.Fsm.Event(this.OutOfDesiresEvent);
				return;
			}
			DesiredItem desiredItem = this.desires[this.m_currentDesireIndex];
			if (GameStatsManager.Instance.GetFlag(desiredItem.flagToSet))
			{
				this.NextDesire();
				return;
			}
			if (this.m_currentTargets == null)
			{
				this.m_currentTargetIndex = 0;
				this.CheckPlayerForDesire(desiredItem);
			}
			if (this.m_currentTargetIndex >= this.m_currentTargets.Count && !this.m_hasNonItemTarget)
			{
				this.NextDesire();
				return;
			}
			if (this.m_currentTargets.Count > 0)
			{
				PickupObject pickupObject = this.m_currentTargets[this.m_currentTargetIndex];
				this.m_currentTarget = pickupObject;
				this.m_currentTargetIndex++;
				FsmString fsmString = base.Fsm.Variables.GetFsmString("npcReplacementString");
				EncounterTrackable component = pickupObject.GetComponent<EncounterTrackable>();
				if (fsmString != null && component != null)
				{
					fsmString.Value = component.GetModifiedDisplayName();
				}
			}
			DialogueBox dialogueBox = null;
			for (int i = 0; i < base.State.Actions.Length; i++)
			{
				if (base.State.Actions[i] is DialogueBox)
				{
					dialogueBox = base.State.Actions[i] as DialogueBox;
				}
			}
			switch (desiredItem.type)
			{
			case DesiredItem.DetectType.SPECIFIC_ITEM:
				dialogueBox.dialogue[0].Value = "#BLACKSMITH_ASK_FOR_SPECIFIC";
				break;
			case DesiredItem.DetectType.CURRENCY:
				dialogueBox.dialogue[0].Value = "#BLACKSMITH_ASK_FOR_AMOUNT_OF_COINS";
				break;
			case DesiredItem.DetectType.META_CURRENCY:
				dialogueBox.dialogue[0].Value = "#BLACKSMITH_ASK_FOR_AMOUNT_OF_META_CURRENCY";
				break;
			case DesiredItem.DetectType.KEYS:
				dialogueBox.dialogue[0].Value = "#BLACKSMITH_ASK_FOR_AMOUNT_OF_KEYS";
				break;
			default:
				dialogueBox.dialogue[0].Value = "#BLACKSMITH_ASK_FOR_SPECIFIC";
				break;
			}
		}

		// Token: 0x04003601 RID: 13825
		public DesiredItem[] desires;

		// Token: 0x04003602 RID: 13826
		public FsmEvent NextDesireEvent;

		// Token: 0x04003603 RID: 13827
		public FsmEvent OutOfDesiresEvent;

		// Token: 0x04003604 RID: 13828
		private int m_currentDesireIndex;

		// Token: 0x04003605 RID: 13829
		private List<PickupObject> m_currentTargets;

		// Token: 0x04003606 RID: 13830
		private int m_currentTargetIndex;

		// Token: 0x04003607 RID: 13831
		private bool m_hasNonItemTarget;

		// Token: 0x04003608 RID: 13832
		private PlayerController talkingPlayer;

		// Token: 0x04003609 RID: 13833
		private PickupObject m_currentTarget;
	}
}
