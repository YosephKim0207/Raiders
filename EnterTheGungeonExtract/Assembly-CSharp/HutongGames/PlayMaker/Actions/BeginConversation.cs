using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C88 RID: 3208
	[Tooltip("Starts an NPC conversation (makes the NPC uninteractable).")]
	[ActionCategory(".NPCs")]
	public class BeginConversation : FsmStateAction
	{
		// Token: 0x060044C3 RID: 17603 RVA: 0x001635BC File Offset: 0x001617BC
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			if (component.TalkingPlayer == null)
			{
				component.TalkingPlayer = GameManager.Instance.PrimaryPlayer;
			}
			for (int i = 0; i < StaticReferenceManager.AllNpcs.Count; i++)
			{
				TalkDoerLite talkDoerLite = StaticReferenceManager.AllNpcs[i];
				if (talkDoerLite && talkDoerLite != component)
				{
					talkDoerLite.CloseTextBox(true);
				}
			}
			GameUIRoot.Instance.InitializeConversationPortrait(component.TalkingPlayer);
			GameUIRoot.Instance.levelNameUI.BanishLevelNameText();
			if (this.conversationType == BeginConversation.ConversationType.Normal)
			{
				component.State = TalkDoerLite.TalkingState.Conversation;
			}
			else if (this.conversationType == BeginConversation.ConversationType.Passive)
			{
				component.State = TalkDoerLite.TalkingState.Passive;
			}
			bool flag = this.locked == BeginConversation.LockedConversation.Locked;
			if (this.locked == BeginConversation.LockedConversation.Default)
			{
				flag = this.conversationType == BeginConversation.ConversationType.Normal;
			}
			if (flag && !component.HasPlayerLocked)
			{
				component.HasPlayerLocked = true;
				component.TalkingPlayer.SetInputOverride("conversation");
				Pixelator.Instance.LerpToLetterbox(0.35f, 0.25f);
				Pixelator.Instance.DoFinalNonFadedLayer = true;
				GameUIRoot.Instance.ToggleLowerPanels(false, false, "conversation");
				GameUIRoot.Instance.HideCoreUI("conversation");
				if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
				{
					GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().ForceHideMetaCurrencyPanel();
				}
				Minimap.Instance.TemporarilyPreventMinimap = true;
				Vector2 vector = component.speakPoint.transform.position.XY();
				Vector2 vector2 = ((!this.UsesCustomScreenBuffer) ? BeginConversation.NpcScreenBuffer : this.CustomScreenBuffer);
				CameraController mainCameraController = GameManager.Instance.MainCameraController;
				Vector2 vector3 = CameraController.CameraToWorld(vector2.x, vector2.y);
				Vector2 vector4 = CameraController.CameraToWorld(1f - vector2.x, 1f - vector2.y);
				Vector2 vector5 = vector4 - vector3;
				if (this.overrideNpcScreenHeight.Value >= 0f)
				{
					float y = CameraController.CameraToWorld(0.5f, this.overrideNpcScreenHeight.Value).y;
					vector3.y = y;
					vector4.y = y;
					vector5.y = 0f;
				}
				mainCameraController.SetManualControl(true, true);
				Rect rect = new Rect(vector3.x, vector3.y, vector5.x, vector5.y);
				if (rect.Contains(vector))
				{
					mainCameraController.OverridePosition = mainCameraController.transform.position;
				}
				else
				{
					Vector2 vector6 = BraveMathCollege.ClosestPointOnRectangle(vector, vector3, vector4 - vector3);
					mainCameraController.OverridePosition = mainCameraController.transform.position + (vector - vector6);
				}
			}
			base.Finish();
		}

		// Token: 0x040036CB RID: 14027
		public const float LetterBoxAmount = 0.35f;

		// Token: 0x040036CC RID: 14028
		public const float LetterBoxLerpTime = 0.25f;

		// Token: 0x040036CD RID: 14029
		public static Vector2 NpcScreenBuffer = new Vector2(0.3f, 0.3f);

		// Token: 0x040036CE RID: 14030
		[Tooltip("Normal: Full conversation, press 'action' to advance.\nPassive: Just a speec bubble over the NPC's head.")]
		public BeginConversation.ConversationType conversationType;

		// Token: 0x040036CF RID: 14031
		[Tooltip("Whether or not to take control away from the player during the conversation.\nDefault will lock normal conversations but not passive conversations.")]
		public BeginConversation.LockedConversation locked;

		// Token: 0x040036D0 RID: 14032
		[Tooltip("Whether or not to take control away from the player during the conversation.\nDefault will lock normal conversations but not passive conversations.")]
		public FsmFloat overrideNpcScreenHeight = -1f;

		// Token: 0x040036D1 RID: 14033
		public bool UsesCustomScreenBuffer;

		// Token: 0x040036D2 RID: 14034
		public Vector2 CustomScreenBuffer;

		// Token: 0x02000C89 RID: 3209
		public enum ConversationType
		{
			// Token: 0x040036D4 RID: 14036
			Normal,
			// Token: 0x040036D5 RID: 14037
			Passive
		}

		// Token: 0x02000C8A RID: 3210
		public enum LockedConversation
		{
			// Token: 0x040036D7 RID: 14039
			Default,
			// Token: 0x040036D8 RID: 14040
			Locked,
			// Token: 0x040036D9 RID: 14041
			Unlocked
		}
	}
}
