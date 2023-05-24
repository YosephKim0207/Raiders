using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C98 RID: 3224
	[ActionCategory(".NPCs")]
	[Tooltip("Opens a dialogue box and speaks one or more lines of dialogue. Also supports one set of player responses.\nOnly the first valid Dialogue Box action will be run for a given state.")]
	public class DialogueBox : FsmStateAction
	{
		// Token: 0x06004503 RID: 17667 RVA: 0x00164D70 File Offset: 0x00162F70
		public override void Reset()
		{
			this.condition = DialogueBox.Condition.All;
			this.sequence = DialogueBox.DialogueSequence.Default;
			this.persistentStringsToShow = 1;
			this.dialogue = new FsmString[]
			{
				new FsmString(string.Empty)
			};
			this.responses = null;
			this.events = null;
			this.skipWalkAwayEvent = false;
			this.forceCloseTime = 0f;
			this.zombieTime = 0f;
			this.SuppressDefaultAnims = false;
			this.OverrideTalkAnim = string.Empty;
			this.PlayBoxOnInteractingPlayer = false;
			this.AlternativeTalker = null;
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x00164E1C File Offset: 0x0016301C
		public override string ErrorCheck()
		{
			string text = string.Empty;
			AIAnimator component = base.Owner.GetComponent<AIAnimator>();
			if (!this.SuppressDefaultAnims.Value)
			{
				if (!component)
				{
					text += "Owner must have an AIAnimator to manage animations to use default animations.";
				}
				if (component && !component.HasDefaultAnimation)
				{
					text += "AIAnimator must have a default (base or idle) animation to use default animations.";
				}
				if (component && !component.HasDirectionalAnimation("talk"))
				{
					text += "AIAnimator must have a talk animation to use default animations.";
				}
			}
			if (this.sequence == DialogueBox.DialogueSequence.Mutliline && this.dialogue.Length != 1)
			{
				text += "Multiline only supports a single dialogue string.\n";
			}
			if (this.sequence == DialogueBox.DialogueSequence.Sequential && this.dialogue.Length != 1)
			{
				text += "Sequential only supports a single dialogue string.\n";
			}
			if (this.sequence == DialogueBox.DialogueSequence.SeqThenRepeatLast && this.dialogue.Length != 1)
			{
				text += "SeqThenRepeatLast only supports a single dialogue string.\n";
			}
			if (this.sequence == DialogueBox.DialogueSequence.SeqThenRemoveState && this.dialogue.Length != 1)
			{
				text += "SeqThenRemoveState only supports a single dialogue string.\n";
			}
			if (this.sequence == DialogueBox.DialogueSequence.PersistentSequential && this.dialogue.Length < 2)
			{
				text += "PersistentSequential needs at least one sequential dialogue string and one stopper string.\n";
			}
			if (this.dialogue != null && this.dialogue.Length == 0)
			{
				text += "Dialogue strings must contain at least one line of dialogue.\n";
			}
			if (this.forceCloseTime.Value > 0f && this.responses != null && this.responses.Length != 0)
			{
				text += "Force Close Timer will be ignored if there are dialogue responses.\n";
			}
			return text;
		}

		// Token: 0x06004505 RID: 17669 RVA: 0x00164FC8 File Offset: 0x001631C8
		public override void OnEnter()
		{
			this.m_talkDoer = base.Owner.GetComponent<TalkDoerLite>();
			if (this.ShouldSkip())
			{
				base.Finish();
				return;
			}
			this.m_dialogueState = DialogueBox.DialogueState.ShowNextDialogue;
			if (this.sequence != DialogueBox.DialogueSequence.PersistentSequential)
			{
				this.m_textIndex = 0;
			}
			this.m_forceCloseTimer = 0f;
			if (this.skipWalkAwayEvent.Value)
			{
				this.m_talkDoer.AllowWalkAways = false;
			}
			if (this.sequence == DialogueBox.DialogueSequence.Default)
			{
				this.m_numDialogues = this.dialogue.Length;
			}
			else if (this.sequence == DialogueBox.DialogueSequence.Mutliline)
			{
				this.m_numDialogues = StringTableManager.GetNumStrings(this.dialogue[0].Value);
			}
			else
			{
				this.m_numDialogues = 1;
			}
			this.m_rawResponses = new string[this.responses.Length];
			for (int i = 0; i < this.responses.Length; i++)
			{
				this.m_rawResponses[i] = StringTableManager.GetString(this.responses[i].Value);
				this.m_rawResponses[i] = this.NPCReplacementPostprocessString(this.m_rawResponses[i]);
			}
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x001650E4 File Offset: 0x001632E4
		public override void OnUpdate()
		{
			if (this.m_dialogueState == DialogueBox.DialogueState.ShowNextDialogue)
			{
				this.NextDialogue();
				this.m_dialogueState = DialogueBox.DialogueState.ShowingDialogue;
				if (!this.SuppressDefaultAnims.Value)
				{
					if (this.AlternativeTalker != null)
					{
						this.AlternativeTalker.aiAnimator.PlayUntilFinished(this.TalkAnimName, false, null, -1f, false);
					}
					else
					{
						this.m_talkDoer.aiAnimator.PlayUntilFinished(this.TalkAnimName, false, null, -1f, false);
					}
				}
			}
			else if (this.m_dialogueState == DialogueBox.DialogueState.ShowingDialogue)
			{
				bool flag = false;
				if (this.m_talkDoer.State == TalkDoerLite.TalkingState.Conversation)
				{
					BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(this.m_talkDoer.TalkingPlayer.PlayerIDX);
					bool flag2;
					flag = instanceForPlayer.WasAdvanceDialoguePressed(out flag2);
					if (flag2)
					{
						this.m_talkDoer.TalkingPlayer.SuppressThisClick = true;
					}
				}
				bool flag3 = false;
				if (this.m_forceCloseTimer > 0f)
				{
					this.m_forceCloseTimer -= BraveTime.DeltaTime;
					flag3 = this.m_forceCloseTimer <= 0f;
				}
				if (flag || flag3)
				{
					if (TextBoxManager.TextBoxCanBeAdvanced(this.m_talkDoer.speakPoint) && !flag3)
					{
						TextBoxManager.AdvanceTextBox(this.m_talkDoer.speakPoint);
					}
					else if (this.m_textIndex < this.m_numDialogues && this.sequence != DialogueBox.DialogueSequence.PersistentSequential)
					{
						if (this.m_talkDoer.echo1 != null)
						{
							this.m_talkDoer.echo1.IsDoingForcedSpeech = false;
						}
						if (this.m_talkDoer.echo2 != null)
						{
							this.m_talkDoer.echo2.IsDoingForcedSpeech = false;
						}
						this.m_dialogueState = DialogueBox.DialogueState.ShowNextDialogue;
					}
					else if (this.responses.Length > 0)
					{
						this.m_dialogueState = DialogueBox.DialogueState.ShowingResponses;
					}
					else
					{
						if (this.forceCloseTime.Value != 0f && this.zombieTime.Value != 0f)
						{
							float num = this.forceCloseTime.Value + this.zombieTime.Value;
							float num2 = 0.5f + TextBoxManager.GetEstimatedReadingTime(this.m_currentDialogueText) * TextBoxManager.ZombieBoxMultiplier;
							float num3 = Mathf.Max(num, num2);
							this.m_talkDoer.SetZombieBoxTimer(Mathf.Max(num3 - this.forceCloseTime.Value, 0.1f), this.TalkAnimName);
						}
						else if (!this.SuppressDefaultAnims.Value)
						{
							if (this.AlternativeTalker != null)
							{
								this.AlternativeTalker.aiAnimator.EndAnimationIf(this.TalkAnimName);
							}
							else
							{
								this.m_talkDoer.aiAnimator.EndAnimationIf(this.TalkAnimName);
							}
						}
						base.Finish();
					}
				}
				else if (this.responses.Length > 0 && this.m_textIndex == this.m_numDialogues && !TextBoxManager.TextBoxCanBeAdvanced(this.m_talkDoer.speakPoint))
				{
					this.m_dialogueState = DialogueBox.DialogueState.ShowingResponses;
				}
			}
			else if (this.m_dialogueState == DialogueBox.DialogueState.ShowingResponses)
			{
				this.ShowResponses();
				this.m_dialogueState = DialogueBox.DialogueState.WaitingForResponse;
			}
			else if (this.m_dialogueState == DialogueBox.DialogueState.WaitingForResponse)
			{
				int num4;
				if (!GameUIRoot.Instance.GetPlayerConversationResponse(out num4))
				{
					return;
				}
				this.m_talkDoer.TalkingPlayer.ClearInputOverride("dialogueResponse");
				this.m_talkDoer.CloseTextBox(true);
				base.Finish();
				if (!this.SuppressDefaultAnims.Value)
				{
					if (this.AlternativeTalker != null)
					{
						this.AlternativeTalker.aiAnimator.EndAnimationIf(this.TalkAnimName);
					}
					else
					{
						this.m_talkDoer.aiAnimator.EndAnimationIf(this.TalkAnimName);
					}
				}
				base.Fsm.Event(this.events[num4]);
			}
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x001654C8 File Offset: 0x001636C8
		public override void OnExit()
		{
			if (this.m_talkDoer)
			{
				this.m_talkDoer.CloseTextBox(false);
				if (this.skipWalkAwayEvent.Value)
				{
					this.m_talkDoer.AllowWalkAways = true;
				}
			}
		}

		// Token: 0x06004508 RID: 17672 RVA: 0x00165504 File Offset: 0x00163704
		private bool ShouldSkip()
		{
			if (this.condition == DialogueBox.Condition.FirstEncounterThisInstance)
			{
				if (this.m_talkDoer.NumTimesSpokenTo > 1)
				{
					return true;
				}
			}
			else if (this.condition == DialogueBox.Condition.FirstEverEncounter)
			{
				EncounterTrackable component = base.Owner.GetComponent<EncounterTrackable>();
				if (component == null)
				{
					return true;
				}
				if (GameStatsManager.Instance.QueryEncounterable(component) > 1)
				{
					return true;
				}
			}
			else if (this.condition == DialogueBox.Condition.KeyboardAndMouse)
			{
				if (!BraveInput.GetInstanceForPlayer(this.m_talkDoer.TalkingPlayer.PlayerIDX).IsKeyboardAndMouse(false))
				{
					return true;
				}
			}
			else if (this.condition == DialogueBox.Condition.Controller && BraveInput.GetInstanceForPlayer(this.m_talkDoer.TalkingPlayer.PlayerIDX).IsKeyboardAndMouse(false))
			{
				return true;
			}
			for (int i = 0; i < base.State.Actions.Length; i++)
			{
				if (base.State.Actions[i] == this)
				{
					break;
				}
				if (base.State.Actions[i] is DialogueBox && base.State.Actions[i].Active)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004509 RID: 17673 RVA: 0x00165640 File Offset: 0x00163840
		private string NPCReplacementPostprocessString(string input)
		{
			FsmString fsmString = base.Fsm.Variables.GetFsmString("npcReplacementString");
			if (fsmString != null && !string.IsNullOrEmpty(fsmString.Value))
			{
				input = input.Replace("%NPCREPLACEMENT", fsmString.Value);
			}
			string text = "%NPCNUMBER1";
			int num = 1;
			while (input.Contains(text))
			{
				FsmInt fsmInt = base.Fsm.Variables.GetFsmInt("npcNumber" + num.ToString());
				if (fsmInt != null)
				{
					input = input.Replace(text, fsmInt.Value.ToString());
				}
				num++;
				text = "%NPCNUMBER" + num.ToString();
			}
			return input;
		}

		// Token: 0x0600450A RID: 17674 RVA: 0x00165710 File Offset: 0x00163910
		private void NextDialogue()
		{
			if (this.m_textIndex > 0)
			{
			}
			bool flag = this.m_textIndex == this.m_numDialogues - 1;
			string text = "ERROR ERROR";
			if (this.m_textIndex < this.dialogue.Length && this.m_textIndex >= 0 && this.dialogue[this.m_textIndex].UsesVariable && !this.dialogue[this.m_textIndex].Value.StartsWith("#"))
			{
				text = this.dialogue[this.m_textIndex].Value;
			}
			else if (this.sequence == DialogueBox.DialogueSequence.Default)
			{
				text = StringTableManager.GetString(this.dialogue[this.m_textIndex].Value);
			}
			else if (this.sequence == DialogueBox.DialogueSequence.Mutliline)
			{
				text = StringTableManager.GetExactString(this.dialogue[0].Value, this.m_textIndex);
			}
			else if (this.sequence == DialogueBox.DialogueSequence.SeqThenRemoveState)
			{
				bool flag2;
				text = StringTableManager.GetStringSequential(this.dialogue[0].Value, ref this.m_sequentialStringLastIndex, out flag2, false);
				if (flag2)
				{
					BravePlayMakerUtility.DisconnectState(base.State);
				}
			}
			else if (this.sequence == DialogueBox.DialogueSequence.Sequential || this.sequence == DialogueBox.DialogueSequence.SeqThenRepeatLast)
			{
				bool flag3 = this.sequence == DialogueBox.DialogueSequence.SeqThenRepeatLast;
				text = StringTableManager.GetStringSequential(this.dialogue[0].Value, ref this.m_sequentialStringLastIndex, flag3);
			}
			else if (this.sequence == DialogueBox.DialogueSequence.PersistentSequential)
			{
				if (this.m_textIndex < this.dialogue.Length - 1)
				{
					text = StringTableManager.GetStringPersistentSequential(this.dialogue[this.m_textIndex].Value);
				}
				else
				{
					text = StringTableManager.GetString(this.dialogue[this.m_textIndex].Value);
					flag = true;
				}
			}
			if (text.Contains("$"))
			{
				string[] array = text.Split(new char[] { '$' });
				text = array[0];
				if (array.Length > 1)
				{
					int num = 1;
					while (num < array.Length && num - 1 < this.m_rawResponses.Length)
					{
						this.m_rawResponses[num - 1] = array[num];
						num++;
					}
				}
			}
			else if (text.Contains("&"))
			{
				string[] array2 = text.Split(new char[] { '&' });
				text = array2[0];
				if (this.m_talkDoer.echo1 != null)
				{
					this.m_talkDoer.echo1.ForceTimedSpeech(array2[1], 1f, 4f, TextBoxManager.BoxSlideOrientation.FORCE_RIGHT);
				}
				if (this.m_talkDoer.echo2 != null && array2.Length > 2)
				{
					this.m_talkDoer.echo2.ForceTimedSpeech(array2[2], 2f, 4f, TextBoxManager.BoxSlideOrientation.FORCE_LEFT);
				}
			}
			text = this.NPCReplacementPostprocessString(text);
			this.m_currentDialogueText = text;
			this.ClearAlternativeTalkerFromPrevious();
			if (this.AlternativeTalker != null)
			{
				this.AlternativeTalker.SuppressClear = true;
				TextBoxManager.ClearTextBox(this.m_talkDoer.speakPoint);
				TextBoxManager.ClearTextBox(this.m_talkDoer.TalkingPlayer.transform);
				TalkDoerLite talkDoer = this.m_talkDoer;
				Vector3 vector = this.AlternativeTalker.speakPoint.position + new Vector3(0f, 0f, -5f);
				Transform transform = this.AlternativeTalker.speakPoint;
				float num2 = -1f;
				string text2 = text;
				bool flag4 = false;
				bool flag5 = this.HasNextDialogue() && this.m_talkDoer.State == TalkDoerLite.TalkingState.Conversation;
				talkDoer.ShowText(vector, transform, num2, text2, flag4, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, flag5, this.IsThoughtBubble.Value, this.AlternativeTalker.audioCharacterSpeechTag);
			}
			else if (this.PlayBoxOnInteractingPlayer.Value)
			{
				TextBoxManager.ClearTextBox(this.m_talkDoer.speakPoint);
				TalkDoerLite talkDoer2 = this.m_talkDoer;
				Vector3 vector = this.m_talkDoer.TalkingPlayer.CenterPosition.ToVector3ZUp(this.m_talkDoer.TalkingPlayer.CenterPosition.y) + new Vector3(0f, 1f, -5f);
				Transform transform = this.m_talkDoer.TalkingPlayer.transform;
				float num2 = -1f;
				string text2 = text;
				bool flag5 = false;
				bool flag4 = this.HasNextDialogue() && this.m_talkDoer.State == TalkDoerLite.TalkingState.Conversation;
				talkDoer2.ShowText(vector, transform, num2, text2, flag5, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, flag4, this.IsThoughtBubble.Value, this.m_talkDoer.TalkingPlayer.characterAudioSpeechTag);
			}
			else
			{
				if (this.m_talkDoer.TalkingPlayer)
				{
					TextBoxManager.ClearTextBox(this.m_talkDoer.TalkingPlayer.transform);
				}
				TalkDoerLite talkDoer3 = this.m_talkDoer;
				Vector3 vector = this.m_talkDoer.speakPoint.position + new Vector3(0f, 0f, -5f);
				Transform transform = this.m_talkDoer.speakPoint;
				float num2 = -1f;
				string text2 = text;
				bool flag4 = false;
				bool flag5 = this.HasNextDialogue() && this.m_talkDoer.State == TalkDoerLite.TalkingState.Conversation;
				talkDoer3.ShowText(vector, transform, num2, text2, flag4, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, flag5, this.IsThoughtBubble.Value, null);
			}
			if (flag && this.forceCloseTime.Value > 0f)
			{
				this.m_forceCloseTimer = this.forceCloseTime.Value;
			}
			if (this.sequence == DialogueBox.DialogueSequence.PersistentSequential)
			{
				this.m_persistentIndex++;
				if (this.m_persistentIndex >= this.persistentStringsToShow.Value)
				{
					this.m_persistentIndex = 0;
					this.m_textIndex = Mathf.Min(this.m_textIndex + 1, this.dialogue.Length - 1);
				}
			}
			else
			{
				this.m_textIndex++;
			}
		}

		// Token: 0x0600450B RID: 17675 RVA: 0x00165CF4 File Offset: 0x00163EF4
		private void ClearAlternativeTalkerFromPrevious()
		{
			FsmState previousActiveState = base.Fsm.PreviousActiveState;
			if (previousActiveState != null && previousActiveState != base.State)
			{
				for (int i = 0; i < previousActiveState.Actions.Length; i++)
				{
					if (previousActiveState.Actions[i] is DialogueBox)
					{
						DialogueBox dialogueBox = previousActiveState.Actions[i] as DialogueBox;
						if (dialogueBox.AlternativeTalker != null)
						{
							dialogueBox.AlternativeTalker.SuppressClear = false;
							TextBoxManager.ClearTextBox(dialogueBox.AlternativeTalker.speakPoint);
						}
					}
				}
			}
		}

		// Token: 0x0600450C RID: 17676 RVA: 0x00165D88 File Offset: 0x00163F88
		private void ShowResponses()
		{
			if (this.m_talkDoer.echo1 != null)
			{
				this.m_talkDoer.echo1.IsDoingForcedSpeech = false;
			}
			if (this.m_talkDoer.echo2 != null)
			{
				this.m_talkDoer.echo2.IsDoingForcedSpeech = false;
			}
			if (this.responses.Length > 0)
			{
				this.m_talkDoer.TalkingPlayer.SetInputOverride("dialogueResponse");
				GameUIRoot.Instance.DisplayPlayerConversationOptions(this.m_talkDoer.TalkingPlayer, this.m_rawResponses);
			}
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x00165E24 File Offset: 0x00164024
		private bool HasNextDialogue()
		{
			if (this.sequence == DialogueBox.DialogueSequence.PersistentSequential)
			{
				return false;
			}
			if (this.m_textIndex < this.m_numDialogues - 1)
			{
				return true;
			}
			for (int i = 0; i < base.State.Transitions.Length; i++)
			{
				if (!string.IsNullOrEmpty(base.State.Transitions[i].ToState))
				{
					FsmState state = base.Fsm.GetState(base.State.Transitions[i].ToState);
					for (int j = 0; j < state.Actions.Length; j++)
					{
						FsmStateAction fsmStateAction = state.Actions[j];
						if (fsmStateAction is DialogueBox)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x0600450E RID: 17678 RVA: 0x00165EE4 File Offset: 0x001640E4
		private string TalkAnimName
		{
			get
			{
				return (!this.SuppressDefaultAnims.Value && !string.IsNullOrEmpty(this.OverrideTalkAnim.Value)) ? this.OverrideTalkAnim.Value : "talk";
			}
		}

		// Token: 0x04003715 RID: 14101
		[Tooltip("Only show this dialogue box if this condition is met")]
		public DialogueBox.Condition condition;

		// Token: 0x04003716 RID: 14102
		[Tooltip("Handles the dialogue sequence.")]
		[ActionSection("Text")]
		public DialogueBox.DialogueSequence sequence;

		// Token: 0x04003717 RID: 14103
		[Tooltip("The number of persistent strings to show for each key before progressing to the next one.")]
		public FsmInt persistentStringsToShow = 1;

		// Token: 0x04003718 RID: 14104
		[Tooltip("Dialogue strings for the NPC to say.")]
		public FsmString[] dialogue;

		// Token: 0x04003719 RID: 14105
		[CompoundArray("Responses", "Text", "Event")]
		public FsmString[] responses;

		// Token: 0x0400371A RID: 14106
		public FsmEvent[] events;

		// Token: 0x0400371B RID: 14107
		[Tooltip("If true, player distance will not cause the playerWalkedAway event to fire.")]
		[ActionSection("Advanced")]
		public FsmBool skipWalkAwayEvent;

		// Token: 0x0400371C RID: 14108
		[Tooltip("If set, after this amount of time (seconds) the dialogue box will force close.")]
		public FsmFloat forceCloseTime;

		// Token: 0x0400371D RID: 14109
		[Tooltip("If set, after the dialogue box closes it will remain up for this amount of time (seconds). Set to -1 to leave it up until something else overrides it.")]
		public FsmFloat zombieTime;

		// Token: 0x0400371E RID: 14110
		[Tooltip("If true, don't use the default talk and idle animations.")]
		public FsmBool SuppressDefaultAnims;

		// Token: 0x0400371F RID: 14111
		[Tooltip("If specified, use this animation instead of the default talk animation.")]
		public FsmString OverrideTalkAnim;

		// Token: 0x04003720 RID: 14112
		[Tooltip("If marked, play the textbox over the player. Only for Pasts!")]
		public FsmBool PlayBoxOnInteractingPlayer;

		// Token: 0x04003721 RID: 14113
		[Tooltip("Thot box")]
		public FsmBool IsThoughtBubble;

		// Token: 0x04003722 RID: 14114
		[Tooltip("If used, play the textbox over this talk doer instead.")]
		public TalkDoerLite AlternativeTalker;

		// Token: 0x04003723 RID: 14115
		private TalkDoerLite m_talkDoer;

		// Token: 0x04003724 RID: 14116
		private DialogueBox.DialogueState m_dialogueState;

		// Token: 0x04003725 RID: 14117
		private int m_numDialogues;

		// Token: 0x04003726 RID: 14118
		private int m_textIndex;

		// Token: 0x04003727 RID: 14119
		private int m_persistentIndex;

		// Token: 0x04003728 RID: 14120
		private float m_forceCloseTimer;

		// Token: 0x04003729 RID: 14121
		private string[] m_rawResponses;

		// Token: 0x0400372A RID: 14122
		private int m_sequentialStringLastIndex = -1;

		// Token: 0x0400372B RID: 14123
		private string m_currentDialogueText;

		// Token: 0x02000C99 RID: 3225
		private enum DialogueState
		{
			// Token: 0x0400372D RID: 14125
			ShowNextDialogue,
			// Token: 0x0400372E RID: 14126
			ShowingDialogue,
			// Token: 0x0400372F RID: 14127
			ShowingResponses,
			// Token: 0x04003730 RID: 14128
			WaitingForResponse
		}

		// Token: 0x02000C9A RID: 3226
		public enum DialogueSequence
		{
			// Token: 0x04003732 RID: 14130
			Default,
			// Token: 0x04003733 RID: 14131
			Sequential,
			// Token: 0x04003734 RID: 14132
			SeqThenRepeatLast,
			// Token: 0x04003735 RID: 14133
			SeqThenRemoveState,
			// Token: 0x04003736 RID: 14134
			Mutliline,
			// Token: 0x04003737 RID: 14135
			PersistentSequential
		}

		// Token: 0x02000C9B RID: 3227
		public enum Condition
		{
			// Token: 0x04003739 RID: 14137
			All,
			// Token: 0x0400373A RID: 14138
			FirstEncounterThisInstance,
			// Token: 0x0400373B RID: 14139
			FirstEverEncounter,
			// Token: 0x0400373C RID: 14140
			KeyboardAndMouse = 100,
			// Token: 0x0400373D RID: 14141
			Controller = 110
		}
	}
}
