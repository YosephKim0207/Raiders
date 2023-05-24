using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CAF RID: 3247
	[ActionCategory(".NPCs")]
	[Tooltip("Sets various unusual player flags.")]
	public class SetExoticPlayerFlag : FsmStateAction
	{
		// Token: 0x0600454E RID: 17742 RVA: 0x00167290 File Offset: 0x00165490
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			if (component && component.TalkingPlayer)
			{
				if (this.SetGunGameTrue.Value)
				{
					SetExoticPlayerFlag.SetGunGame(true);
				}
				if (this.SetChallengeModTrue.Value)
				{
					ChallengeManager.ChallengeModeType = ChallengeModeType.ChallengeMode;
					component.TalkingPlayer.PlayEffectOnActor((GameObject)BraveResources.Load("Global VFX/VFX_DaisukeFavor", ".prefab"), new Vector3(0f, -0.625f, 0f), true, false, false);
				}
				else if (this.SetMegaChallengeModeTrue.Value)
				{
					ChallengeManager.ChallengeModeType = ChallengeModeType.ChallengeMegaMode;
					component.TalkingPlayer.PlayEffectOnActor((GameObject)BraveResources.Load("Global VFX/VFX_DaisukeFavor", ".prefab"), new Vector3(0f, -0.625f, 0f), true, false, false);
				}
				else if (this.ToggleRainbowRun.Value)
				{
					if (!GameStatsManager.Instance.rainbowRunToggled)
					{
						GameStatsManager.Instance.rainbowRunToggled = true;
						AkSoundEngine.PostEvent("Play_NPC_Blessing_Rainbow_Get_01", base.Owner.gameObject);
						GameUIRoot.Instance.notificationController.DoCustomNotification(GameUIRoot.Instance.FoyerAmmonomiconLabel.ForceGetLocalizedValue("#RAINBOW_POPUP_ACTIVE"), string.Empty, null, -1, UINotificationController.NotificationColor.SILVER, false, true);
						component.TalkingPlayer.PlayEffectOnActor((GameObject)BraveResources.Load("Global VFX/VFX_BowlerFavor", ".prefab"), new Vector3(0f, -0.625f, 0f), true, false, false);
					}
					else
					{
						GameStatsManager.Instance.rainbowRunToggled = false;
						AkSoundEngine.PostEvent("Play_NPC_Blessing_Rainbow_Remove_01", base.Owner.gameObject);
						GameUIRoot.Instance.notificationController.DoCustomNotification(GameUIRoot.Instance.FoyerAmmonomiconLabel.ForceGetLocalizedValue("#RAINBOW_POPUP_INACTIVE"), string.Empty, null, -1, UINotificationController.NotificationColor.SILVER, false, true);
					}
					GameOptions.Save();
				}
				else if (this.ToggleTurboMode.Value)
				{
					if (!GameStatsManager.Instance.isTurboMode)
					{
						GameStatsManager.Instance.isTurboMode = true;
						AkSoundEngine.PostEvent("Play_NPC_Blessing_Speed_Tonic_01", base.Owner.gameObject);
						if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
						{
							GameUIRoot.Instance.notificationController.DoCustomNotification("Game Speed: Turbo", string.Empty, null, -1, UINotificationController.NotificationColor.SILVER, false, true);
						}
						else
						{
							GameUIRoot.Instance.notificationController.DoCustomNotification(GameUIRoot.Instance.FoyerAmmonomiconLabel.ForceGetLocalizedValue("#OPTIONS_GAMESPEED"), GameUIRoot.Instance.FoyerAmmonomiconLabel.ForceGetLocalizedValue("#OPTIONS_GAMESPEED_TURBO"), null, -1, UINotificationController.NotificationColor.SILVER, false, false);
						}
					}
					else
					{
						GameStatsManager.Instance.isTurboMode = false;
						AkSoundEngine.PostEvent("Play_NPC_Blessing_Slow_Tonic_01", base.Owner.gameObject);
						if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
						{
							GameUIRoot.Instance.notificationController.DoCustomNotification("Game Speed: Classic", string.Empty, null, -1, UINotificationController.NotificationColor.SILVER, false, true);
						}
						else
						{
							GameUIRoot.Instance.notificationController.DoCustomNotification(GameUIRoot.Instance.FoyerAmmonomiconLabel.ForceGetLocalizedValue("#OPTIONS_GAMESPEED"), GameUIRoot.Instance.FoyerAmmonomiconLabel.ForceGetLocalizedValue("#OPTIONS_GAMESPEED_NORMAL"), null, -1, UINotificationController.NotificationColor.SILVER, false, false);
						}
					}
					GameOptions.Save();
				}
			}
			base.Finish();
		}

		// Token: 0x0600454F RID: 17743 RVA: 0x001675D0 File Offset: 0x001657D0
		public static void SetGunGame(bool doEffects)
		{
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[i];
				playerController.CharacterUsesRandomGuns = true;
				for (int j = 1; j < playerController.inventory.AllGuns.Count; j++)
				{
					Gun gun = playerController.inventory.AllGuns[j];
					playerController.inventory.RemoveGunFromInventory(gun);
					UnityEngine.Object.Destroy(gun.gameObject);
					j--;
				}
				if (doEffects)
				{
					playerController.PlayEffectOnActor((GameObject)BraveResources.Load("Global VFX/VFX_MagicFavor_Light", ".prefab"), new Vector3(0f, -0.625f, 0f), true, true, false);
				}
			}
		}

		// Token: 0x0400376B RID: 14187
		public FsmBool SetGunGameTrue;

		// Token: 0x0400376C RID: 14188
		public FsmBool SetChallengeModTrue;

		// Token: 0x0400376D RID: 14189
		public FsmBool SetMegaChallengeModeTrue;

		// Token: 0x0400376E RID: 14190
		public FsmBool ToggleTurboMode;

		// Token: 0x0400376F RID: 14191
		public FsmBool ToggleRainbowRun;
	}
}
