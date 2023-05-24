using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C9C RID: 3228
	[Tooltip("Ends an NPC conversation (makes the NPC interactable).")]
	[ActionCategory(".NPCs")]
	public class EndConversation : FsmStateAction
	{
		// Token: 0x06004510 RID: 17680 RVA: 0x00165F28 File Offset: 0x00164128
		public override void Reset()
		{
			this.killZombieTextBoxes = false;
		}

		// Token: 0x06004511 RID: 17681 RVA: 0x00165F38 File Offset: 0x00164138
		public static void ForceEndConversation(TalkDoerLite talkDoer)
		{
			if (talkDoer.TalkingPlayer != null && talkDoer.State == TalkDoerLite.TalkingState.Conversation)
			{
				if (Vector2.Distance(talkDoer.TalkingPlayer.sprite.WorldCenter, talkDoer.sprite.WorldCenter) <= talkDoer.conversationBreakRadius)
				{
					talkDoer.CompletedTalkingPlayer = talkDoer.TalkingPlayer;
				}
				else
				{
					talkDoer.CompletedTalkingPlayer = null;
				}
			}
			if (talkDoer.HasPlayerLocked)
			{
				talkDoer.TalkingPlayer.ClearInputOverride("conversation");
				talkDoer.HasPlayerLocked = false;
				Pixelator.Instance.LerpToLetterbox(0.5f, 0.25f);
				Pixelator.Instance.DoFinalNonFadedLayer = false;
				GameUIRoot.Instance.ToggleLowerPanels(true, false, "conversation");
				GameUIRoot.Instance.ShowCoreUI("conversation");
				if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
				{
					GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().ForceRevealMetaCurrencyPanel();
				}
				Minimap.Instance.TemporarilyPreventMinimap = false;
				GameManager.Instance.MainCameraController.SetManualControl(false, true);
			}
			if (talkDoer.TalkingPlayer)
			{
				TextBoxManager.ClearTextBox(talkDoer.TalkingPlayer.transform);
			}
			talkDoer.IsTalking = false;
			talkDoer.TalkingPlayer = null;
			talkDoer.CloseTextBox(true);
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x0016607C File Offset: 0x0016427C
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			if (!component)
			{
				return;
			}
			if (component.TalkingPlayer != null && component.State == TalkDoerLite.TalkingState.Conversation)
			{
				if (Vector2.Distance(component.TalkingPlayer.sprite.WorldCenter, component.sprite.WorldCenter) <= component.conversationBreakRadius)
				{
					component.CompletedTalkingPlayer = component.TalkingPlayer;
				}
				else
				{
					component.CompletedTalkingPlayer = null;
				}
			}
			if (component.HasPlayerLocked)
			{
				component.TalkingPlayer.ClearInputOverride("conversation");
				component.HasPlayerLocked = false;
				Pixelator.Instance.LerpToLetterbox(0.5f, 0.25f);
				Pixelator.Instance.DoFinalNonFadedLayer = false;
				GameUIRoot.Instance.ToggleLowerPanels(true, false, "conversation");
				GameUIRoot.Instance.ShowCoreUI("conversation");
				if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
				{
					GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().ForceRevealMetaCurrencyPanel();
				}
				Minimap.Instance.TemporarilyPreventMinimap = false;
				if (!this.doNotLerpCamera.Value)
				{
					GameManager.Instance.MainCameraController.SetManualControl(false, true);
				}
			}
			if (this.suppressReinteractDelay.Value)
			{
				component.SuppressReinteractDelay = true;
			}
			if (component.TalkingPlayer)
			{
				TextBoxManager.ClearTextBox(component.TalkingPlayer.transform);
			}
			this.ClearAlternativeTalkerFromPrevious();
			component.IsTalking = false;
			component.TalkingPlayer = null;
			component.CloseTextBox(this.killZombieTextBoxes.Value);
			if (this.suppressReinteractDelay.Value)
			{
				component.SuppressReinteractDelay = false;
			}
			if (this.suppressFurtherInteraction.Value)
			{
				component.ForceNonInteractable = true;
			}
			base.Finish();
		}

		// Token: 0x06004513 RID: 17683 RVA: 0x00166244 File Offset: 0x00164444
		private void ClearAlternativeTalkerFromPrevious()
		{
			FsmState previousActiveState = base.Fsm.PreviousActiveState;
			if (previousActiveState != null)
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

		// Token: 0x0400373E RID: 14142
		[Tooltip("If true, force closes all text boxes, even zombie text boxes.")]
		public FsmBool killZombieTextBoxes;

		// Token: 0x0400373F RID: 14143
		public FsmBool doNotLerpCamera;

		// Token: 0x04003740 RID: 14144
		public FsmBool suppressReinteractDelay;

		// Token: 0x04003741 RID: 14145
		public FsmBool suppressFurtherInteraction;
	}
}
