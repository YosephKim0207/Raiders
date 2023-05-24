using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CDA RID: 3290
	[ActionCategory(".NPCs")]
	public class TriggerNextMonsterHunt : FsmStateAction
	{
		// Token: 0x060045DC RID: 17884 RVA: 0x0016AC20 File Offset: 0x00168E20
		public override void Reset()
		{
		}

		// Token: 0x060045DD RID: 17885 RVA: 0x0016AC24 File Offset: 0x00168E24
		public override string ErrorCheck()
		{
			return string.Empty;
		}

		// Token: 0x060045DE RID: 17886 RVA: 0x0016AC2C File Offset: 0x00168E2C
		public override void OnEnter()
		{
			this.m_talkDoer = base.Owner.GetComponent<TalkDoerLite>();
			if (!this.OnlySetText.Value)
			{
				int num = GameStatsManager.Instance.huntProgress.TriggerNextQuest();
				if (num > 0)
				{
					LootEngine.SpawnCurrency(this.m_talkDoer.sprite.WorldCenter, num, true, new Vector2?(Vector2.down * 1.75f), new float?(45f), 4f, 0.05f);
				}
			}
			FsmString fsmString = base.Fsm.Variables.GetFsmString("QuestIntroString");
			if (fsmString != null && GameStatsManager.Instance.huntProgress.ActiveQuest != null)
			{
				fsmString.Value = GameStatsManager.Instance.huntProgress.ActiveQuest.QuestIntroString;
				DialogueBox.DialogueSequence dialogueSequence = DialogueBox.DialogueSequence.Mutliline;
				if (fsmString.Value.Contains("_GENERIC"))
				{
					dialogueSequence = DialogueBox.DialogueSequence.Default;
				}
				if (base.State.Transitions.Length > 0)
				{
					FsmState state = base.Fsm.GetState(base.State.Transitions[0].ToState);
					for (int i = 0; i < state.Actions.Length; i++)
					{
						if (state.Actions[i] is DialogueBox)
						{
							(state.Actions[i] as DialogueBox).sequence = dialogueSequence;
						}
						FsmState state2 = base.Fsm.GetState(state.Transitions[0].ToState);
						if (state2.Actions[0] is DialogueBox)
						{
							if (dialogueSequence == DialogueBox.DialogueSequence.Default)
							{
								state2.Actions[0].Enabled = true;
							}
							else
							{
								state2.Actions[0].Enabled = false;
							}
						}
					}
				}
			}
			if (!string.IsNullOrEmpty(GameStatsManager.Instance.huntProgress.ActiveQuest.TargetStringKey))
			{
				Debug.Log("doing 1");
				FsmString fsmString2 = base.Fsm.Variables.GetFsmString("npcReplacementString");
				if (fsmString2 != null)
				{
					Debug.Log("doing 2: " + GameStatsManager.Instance.huntProgress.GetReplacementString());
					fsmString2.Value = GameStatsManager.Instance.huntProgress.GetReplacementString();
				}
			}
			FsmInt fsmInt = base.Fsm.Variables.GetFsmInt("npcNumber1");
			if (fsmInt != null)
			{
				fsmInt.Value = GameStatsManager.Instance.huntProgress.ActiveQuest.NumberKillsRequired - GameStatsManager.Instance.huntProgress.CurrentActiveMonsterHuntProgress;
			}
			base.Finish();
		}

		// Token: 0x04003820 RID: 14368
		public FsmBool OnlySetText = false;

		// Token: 0x04003821 RID: 14369
		private TalkDoerLite m_talkDoer;
	}
}
