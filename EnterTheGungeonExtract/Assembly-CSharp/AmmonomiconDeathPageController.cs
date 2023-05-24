using System;
using System.Collections;
using InControl;
using UnityEngine;

// Token: 0x02001734 RID: 5940
public class AmmonomiconDeathPageController : MonoBehaviour, ILevelLoadedListener
{
	// Token: 0x06008A16 RID: 35350 RVA: 0x003972C0 File Offset: 0x003954C0
	public void DoInitialize()
	{
		this.m_doingSomething = false;
		if (this.isRightPage)
		{
			this.InitializeRightPage();
		}
		else
		{
			this.InitializeLeftPage();
		}
	}

	// Token: 0x06008A17 RID: 35351 RVA: 0x003972E8 File Offset: 0x003954E8
	private string GetDeathPortraitName()
	{
		switch (GameManager.Instance.PrimaryPlayer.characterIdentity)
		{
		case PlayableCharacters.Pilot:
			return "coop_page_death_rogue_001";
		case PlayableCharacters.Convict:
			return "coop_page_death_jailbird_001";
		case PlayableCharacters.Robot:
			return "coop_page_death_robot_001";
		case PlayableCharacters.Ninja:
			return "coop_page_death_ninja_001";
		case PlayableCharacters.Cosmonaut:
			return "coop_page_death_cultist_001";
		case PlayableCharacters.Soldier:
			return "coop_page_death_marine_001";
		case PlayableCharacters.Guide:
			return "coop_page_death_scholar_001";
		case PlayableCharacters.Bullet:
			return "coop_page_death_bullet_001";
		case PlayableCharacters.Eevee:
			return "coop_page_death_eevee_001";
		case PlayableCharacters.Gunslinger:
			return "coop_page_death_slinger_001";
		}
		return "coop_page_death_cultist_001";
	}

	// Token: 0x06008A18 RID: 35352 RVA: 0x00397380 File Offset: 0x00395580
	private string GetStringKeyForCharacter(PlayableCharacters identity)
	{
		switch (identity)
		{
		case PlayableCharacters.Pilot:
			return "#CHAR_ROGUE_SHORT";
		case PlayableCharacters.Convict:
			return "#CHAR_CONVICT_SHORT";
		case PlayableCharacters.Robot:
			return "#CHAR_ROBOT_SHORT";
		case PlayableCharacters.Soldier:
			return "#CHAR_MARINE_SHORT";
		case PlayableCharacters.Guide:
			return "#CHAR_GUIDE_SHORT";
		case PlayableCharacters.CoopCultist:
			return "#CHAR_CULTIST_SHORT";
		case PlayableCharacters.Bullet:
			return "#CHAR_BULLET_SHORT";
		case PlayableCharacters.Eevee:
			return "#CHAR_PARADOX_SHORT";
		case PlayableCharacters.Gunslinger:
			return "#CHAR_GUNSLINGER_SHORT";
		}
		return "#CHAR_CULTIST_SHORT";
	}

	// Token: 0x06008A19 RID: 35353 RVA: 0x00397400 File Offset: 0x00395600
	public void UpdateWidth(dfLabel target, int min = -1)
	{
		dfSlicedSprite componentInChildren = target.Parent.GetComponentInChildren<dfSlicedSprite>();
		if (componentInChildren)
		{
			componentInChildren.Width = (float)(Mathf.CeilToInt(target.Width / 4f) + 5);
			if (min > 0)
			{
				componentInChildren.Width = Mathf.Max((float)min, componentInChildren.Width);
			}
		}
	}

	// Token: 0x06008A1A RID: 35354 RVA: 0x00397458 File Offset: 0x00395658
	public void ToggleBG(dfLabel target)
	{
		if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
		{
			target.BackgroundSprite = string.Empty;
			target.Padding = new RectOffset(0, 0, 0, 0);
		}
		else
		{
			target.BackgroundSprite = "chamber_flash_small_001";
			target.Padding = new RectOffset(6, 6, 0, 0);
			target.BackgroundColor = new Color(0.1764706f, 0.19607843f, 0.30588236f);
		}
	}

	// Token: 0x06008A1B RID: 35355 RVA: 0x003974CC File Offset: 0x003956CC
	private void InitializeLeftPage()
	{
		if (this.isVictoryPage)
		{
			this.youDiedLabel.Text = this.youDiedLabel.ForceGetLocalizedValue("#DEATH_YOUWON");
			dfLabel[] componentsInChildren = this.youDiedLabel.GetComponentsInChildren<dfLabel>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Text = this.youDiedLabel.ForceGetLocalizedValue("#DEATH_YOUWON");
			}
		}
		else
		{
			this.youDiedLabel.Text = this.youDiedLabel.ForceGetLocalizedValue("#DEATH_YOUDIED");
			dfLabel[] componentsInChildren2 = this.youDiedLabel.GetComponentsInChildren<dfLabel>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].Text = this.youDiedLabel.ForceGetLocalizedValue("#DEATH_YOUDIED");
			}
		}
		string text = this.hauntingLabel.ForceGetLocalizedValue("#DEATH_PASTKILLED");
		string text2 = this.hauntingLabel.ForceGetLocalizedValue("#DEATH_PASTHAUNTS");
		string text3 = this.hauntingLabel.ForceGetLocalizedValue("#DEATH_HELLCLEARED");
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_LICH))
		{
			this.hauntingLabel.Text = text3;
		}
		else if (GameStatsManager.Instance.GetCharacterSpecificFlag(GameManager.Instance.PrimaryPlayer.characterIdentity, CharacterSpecificGungeonFlags.KILLED_PAST))
		{
			this.hauntingLabel.Text = text;
		}
		else
		{
			this.hauntingLabel.Text = text2;
		}
		this.UpdateWidth(this.hauntingLabel, 142);
		this.hauntingLabel.PerformLayout();
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			this.deathNumberLabel.Parent.Parent.IsVisible = false;
			this.coopDeathNumberLabel.Parent.Parent.IsVisible = true;
			this.coopDeathNumberLabel.Text = GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.NUMBER_DEATHS).ToString();
			this.coopIndividualDeathsNumberLabel.Text = string.Concat(new string[]
			{
				"[sprite \"",
				this.GetDeathPortraitName(),
				"\"] x",
				GameManager.Instance.PrimaryPlayer.DeathsThisRun.ToString(),
				" [sprite \"coop_page_death_cultist_001\"] x",
				GameManager.Instance.SecondaryPlayer.DeathsThisRun.ToString()
			});
		}
		else
		{
			this.deathNumberLabel.Parent.Parent.IsVisible = true;
			this.coopDeathNumberLabel.Parent.Parent.IsVisible = false;
			this.deathNumberLabel.Text = GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.NUMBER_DEATHS).ToString();
		}
		this.UpdateWidth(this.gungeoneerTitle, -1);
		this.UpdateWidth(this.killsTitle, -1);
		this.UpdateWidth(this.areaTitle, -1);
		this.UpdateWidth(this.timeTitle, -1);
		this.UpdateWidth(this.moneyTitle, -1);
		this.gungeoneerLabel.Text = this.gungeoneerLabel.ForceGetLocalizedValue(this.GetStringKeyForCharacter(GameManager.Instance.PrimaryPlayer.characterIdentity));
		this.areaLabel.Text = this.areaLabel.ForceGetLocalizedValue(GameManager.Instance.Dungeon.DungeonShortName);
		int num = Mathf.FloorToInt(GameStatsManager.Instance.GetSessionStatValue(TrackedStats.TIME_PLAYED));
		if (GameManager.Options.SpeedrunMode)
		{
			int num2 = Mathf.FloorToInt(1000f * (GameStatsManager.Instance.GetSessionStatValue(TrackedStats.TIME_PLAYED) % 1f));
			TimeSpan timeSpan = new TimeSpan(0, 0, 0, num, num2);
			string text4 = string.Format("{0:0}:{1:00}:{2:00}.{3:000}", new object[] { timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds });
			this.timeLabel.Text = text4;
		}
		else
		{
			TimeSpan timeSpan2 = new TimeSpan(0, 0, num);
			string text5 = string.Format("{0:0}:{1:00}:{2:00}", timeSpan2.Hours, timeSpan2.Minutes, timeSpan2.Seconds);
			this.timeLabel.Text = text5;
		}
		this.moneyLabel.Text = GameStatsManager.Instance.GetSessionStatValue(TrackedStats.TOTAL_MONEY_COLLECTED).ToString();
		this.killsLabel.Text = string.Empty;
		this.killsLabel.Parent.Width = 30f;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			this.killsLabel.ProcessMarkup = true;
			this.killsLabel.ColorizeSymbols = true;
			bool flag = GameManager.Instance.PrimaryPlayer.KillsThisRun > 99 && GameManager.Instance.SecondaryPlayer.KillsThisRun > 99;
			if (flag)
			{
				this.killsLabel.TabSize = 2;
			}
			else
			{
				this.killsLabel.TabSize = 4;
			}
			this.killsLabel.Text = string.Concat(new string[]
			{
				"[sprite \"",
				this.GetDeathPortraitName(),
				"\"]\t",
				GameManager.Instance.PrimaryPlayer.KillsThisRun.ToString(),
				"\t[sprite \"coop_page_death_cultist_001\"]\t",
				GameManager.Instance.SecondaryPlayer.KillsThisRun.ToString()
			});
		}
		else
		{
			this.killsLabel.Text = GameStatsManager.Instance.GetSessionStatValue(TrackedStats.ENEMIES_KILLED).ToString();
		}
		this.UpdateTapeLabel(this.gungeoneerLabel, -1f);
		this.UpdateTapeLabel(this.areaLabel, -1f);
		this.UpdateTapeLabel(this.timeLabel, -1f);
		this.UpdateTapeLabel(this.moneyLabel, -1f);
		this.killsLabel.PerformLayout();
		this.UpdateTapeLabel(this.killsLabel, this.killsLabel.GetAutosizeWidth());
		string text6 = this.quickRestartButton.ForceGetLocalizedValue("#DEATH_QUICKRESTART");
		string text7 = this.mainMenuButton.ForceGetLocalizedValue("#DEATH_MAINMENU");
		if (!text6.StartsWith(" "))
		{
			text6 = " " + text6;
		}
		if (!text7.StartsWith(" "))
		{
			text7 = " " + text7;
		}
		if (BraveInput.PrimaryPlayerInstance.IsKeyboardAndMouse(false))
		{
			text6 = "[sprite \"space_bar_up_001\"" + text6;
			text7 = "[sprite \"esc_up_001\"" + text7;
		}
		else
		{
			text6 = UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.Action1, BraveInput.PlayerOneCurrentSymbology, null) + text6;
			text7 = UIControllerButtonHelper.GetUnifiedControllerButtonTag(InputControlType.Action2, BraveInput.PlayerOneCurrentSymbology, null) + text7;
		}
		this.quickRestartButton.Text = text6;
		this.mainMenuButton.Text = text7;
		this.quickRestartButton.Click += this.DoQuickRestart;
		this.mainMenuButton.Click += this.DoMainMenu;
		this.quickRestartButton.Focus(true);
		if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.SUPERBOSSRUSH)
		{
			this.quickRestartButton.IsVisible = false;
			this.mainMenuButton.Focus(true);
		}
		else
		{
			this.quickRestartButton.IsVisible = true;
			this.quickRestartButton.Focus(true);
		}
	}

	// Token: 0x06008A1C RID: 35356 RVA: 0x00397C20 File Offset: 0x00395E20
	private void OnDestroy()
	{
		if (this.m_temporaryPhotoTex)
		{
			RenderTexture.ReleaseTemporary(this.m_temporaryPhotoTex);
			this.m_temporaryPhotoTex = null;
		}
	}

	// Token: 0x06008A1D RID: 35357 RVA: 0x00397C44 File Offset: 0x00395E44
	private void InitializeRightPage()
	{
		if (this.ChallengeModeRibbon)
		{
			this.ChallengeModeRibbon.IsVisible = false;
		}
		if (this.isVictoryPage)
		{
			if (this.ChallengeModeRibbon && ChallengeManager.CHALLENGE_MODE_ACTIVE)
			{
				this.ChallengeModeRibbon.IsVisible = true;
				this.ChallengeModeRibbon.RelativePosition += new Vector3(-200f, -68f, 0f);
			}
			this.killedByLabel.Glitchy = false;
			this.killedByLabel.Text = this.killedByLabel.ForceGetLocalizedValue("#DEATH_NOBODY");
			this.UpdateTapeLabel(this.killedByLabel, -1f);
			this.UpdateWidth(this.killedByHeaderLabel, -1);
			this.SetWinPic();
		}
		else
		{
			string text = ((!AmmonomiconDeathPageController.LastKilledPlayerPrimary) ? GameManager.Instance.SecondaryPlayer.healthHaver.lastIncurredDamageSource : GameManager.Instance.PrimaryPlayer.healthHaver.lastIncurredDamageSource);
			if (string.IsNullOrEmpty(text))
			{
				text = StringTableManager.GetEnemiesString("#KILLEDBYDEFAULT", -1);
			}
			this.killedByLabel.Text = string.Empty;
			this.killedByLabel.Parent.Width = 30f;
			this.killedByLabel.Glitchy = false;
			if (GameManager.Instance.Dungeon.IsGlitchDungeon)
			{
				this.killedByLabel.Glitchy = true;
			}
			if (text == "primaryplayer" || text == "secondaryplayer")
			{
				text = StringTableManager.GetEnemiesString("#KILLEDBYDEFAULT", -1);
			}
			if (this.RatDeathDrawings)
			{
				this.RatDeathDrawings.IsVisible = false;
			}
			if (text == StringTableManager.GetEnemiesString("#RESOURCEFULRAT_ENCNAME", -1) || text == StringTableManager.GetEnemiesString("#METALGEARRAT_ENCNAME", -1))
			{
				if (this.RatDeathDrawings)
				{
					this.RatDeathDrawings.IsVisible = true;
				}
				text = StringTableManager.GetEnemiesString("#KILLEDBY_RESOURCEFULRAT", -1);
			}
			this.killedByLabel.Text = text;
			this.killedByLabel.PerformLayout();
			this.UpdateTapeLabel(this.killedByLabel, this.killedByLabel.GetAutosizeWidth());
			this.UpdateWidth(this.killedByHeaderLabel, -1);
			if (this.photoSprite != null)
			{
				float num = this.photoSprite.Width / this.photoSprite.Height;
				float num2 = 1.7777778f;
				if (this.isVictoryPage)
				{
					Texture texture = BraveResources.Load("Win_Pic_Gun_Get_001", ".prefab") as Texture;
					this.photoSprite.Texture = texture;
				}
				else
				{
					RenderTexture renderTexture = Pixelator.Instance.GetCachedFrame();
					if (!Mathf.Approximately(1.7777778f, BraveCameraUtility.ASPECT))
					{
						int height = renderTexture.height;
						int num3 = Mathf.RoundToInt((float)height * 1.7777778f);
						RenderTextureDescriptor renderTextureDescriptor = new RenderTextureDescriptor(num3, height, renderTexture.format, renderTexture.depth);
						RenderTexture temporary = RenderTexture.GetTemporary(renderTextureDescriptor);
						temporary.filterMode = FilterMode.Point;
						float num4 = (float)renderTexture.width / ((float)num3 * 1f);
						float num5 = (float)(renderTexture.width - num3) / 2f / (float)renderTexture.width;
						Graphics.Blit(renderTexture, temporary, new Vector2(1f / num4, 1f), new Vector2(num5, 0f));
						this.m_temporaryPhotoTex = temporary;
						renderTexture = temporary;
					}
					renderTexture.filterMode = FilterMode.Point;
					this.photoSprite.Texture = renderTexture;
					this.m_cachedUVRescale = new Vector4(0f, 0f, 1f, 1f);
					Vector3 cachedPlayerViewportPoint = Pixelator.Instance.CachedPlayerViewportPoint;
					bool flag = cachedPlayerViewportPoint.x > 0f && cachedPlayerViewportPoint.x < 1f && cachedPlayerViewportPoint.y > 0f && cachedPlayerViewportPoint.y < 1f;
					if (flag)
					{
						float num6 = num / num2;
						int num7 = Mathf.RoundToInt(this.photoSprite.Height / 4f);
						float num8 = (float)num7 / 270f;
						float num9 = num8;
						float num10 = num8;
						if (num > num2)
						{
							num10 = num9 / num6;
						}
						else if (num < num2)
						{
							num9 = num10 * num6;
						}
						Vector2 vector = new Vector2(cachedPlayerViewportPoint.x - num9 / 2f, cachedPlayerViewportPoint.y - num10 / 2f);
						Vector2 vector2 = new Vector2(cachedPlayerViewportPoint.x + num9 / 2f, cachedPlayerViewportPoint.y + num10 / 2f);
						if (vector.x < 0f)
						{
							vector2.x += -1f * vector.x;
							vector.x = 0f;
						}
						if (vector2.x > 1f)
						{
							vector.x -= vector2.x - 1f;
							vector2.x = 1f;
						}
						if (vector.y < 0f)
						{
							vector2.y += -1f * vector.y;
							vector.y = 0f;
						}
						if (vector2.y > 1f)
						{
							vector.y -= vector2.y - 1f;
							vector2.y = 1f;
						}
						this.m_cachedUVRescale = new Vector4(vector.x, vector.y, vector2.x, vector2.y);
					}
					else
					{
						this.m_cachedUVRescale = new Vector4(0f, 0f, 1f, 1f / num);
					}
				}
			}
		}
	}

	// Token: 0x06008A1E RID: 35358 RVA: 0x00398210 File Offset: 0x00396410
	private bool ShouldUseJunkPic()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			if (playerController)
			{
				for (int j = 0; j < playerController.passiveItems.Count; j++)
				{
					if (playerController.passiveItems[j] is CompanionItem)
					{
						CompanionItem companionItem = playerController.passiveItems[j] as CompanionItem;
						if (companionItem.ExtantCompanion && companionItem.ExtantCompanion.GetComponent<SackKnightController>() && companionItem.ExtantCompanion.GetComponent<SackKnightController>().CurrentForm == SackKnightController.SackKnightPhase.ANGELIC_KNIGHT)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06008A1F RID: 35359 RVA: 0x003982D4 File Offset: 0x003964D4
	private void SetWinPic()
	{
		if (this.ShouldUseJunkPic() && GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.FINALGEON)
		{
			switch (GameManager.Instance.PrimaryPlayer.characterIdentity)
			{
			case PlayableCharacters.Pilot:
				this.photoSprite.Texture = BraveResources.Load("win_pic_junkan_pilot_001", ".png") as Texture;
				goto IL_1B4;
			case PlayableCharacters.Convict:
				this.photoSprite.Texture = BraveResources.Load("win_pic_junkan_convict_001", ".png") as Texture;
				goto IL_1B4;
			case PlayableCharacters.Robot:
				this.photoSprite.Texture = BraveResources.Load("win_pic_junkan_robot_001", ".png") as Texture;
				goto IL_1B4;
			case PlayableCharacters.Soldier:
				this.photoSprite.Texture = BraveResources.Load("win_pic_junkan_marine_001", ".png") as Texture;
				goto IL_1B4;
			case PlayableCharacters.Guide:
				this.photoSprite.Texture = BraveResources.Load("win_pic_junkan_hunter_001", ".png") as Texture;
				goto IL_1B4;
			case PlayableCharacters.Bullet:
				this.photoSprite.Texture = BraveResources.Load("win_pic_junkan_bullet_001", ".png") as Texture;
				goto IL_1B4;
			case PlayableCharacters.Eevee:
				this.photoSprite.Texture = BraveResources.Load("win_pic_junkan_eevee_001", ".png") as Texture;
				goto IL_1B4;
			case PlayableCharacters.Gunslinger:
				this.photoSprite.Texture = BraveResources.Load("win_pic_junkan_slinger_001", ".png") as Texture;
				goto IL_1B4;
			}
			this.photoSprite.Texture = BraveResources.Load("Win_Pic_Gun_Get_001", ".png") as Texture;
			IL_1B4:;
		}
		else if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
		{
			this.photoSprite.Texture = BraveResources.Load("Win_Pic_BossRush_001", ".png") as Texture;
		}
		else
		{
			GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
			if (tilesetId != GlobalDungeonData.ValidTilesets.FORGEGEON)
			{
				if (tilesetId != GlobalDungeonData.ValidTilesets.HELLGEON)
				{
					if (tilesetId != GlobalDungeonData.ValidTilesets.FINALGEON)
					{
						this.photoSprite.Texture = BraveResources.Load("Win_Pic_Gun_Get_001", ".png") as Texture;
					}
					else
					{
						switch (GameManager.Instance.PrimaryPlayer.characterIdentity)
						{
						case PlayableCharacters.Pilot:
							this.photoSprite.Texture = BraveResources.Load("Win_Pic_Pilot_001", ".png") as Texture;
							goto IL_384;
						case PlayableCharacters.Convict:
							this.photoSprite.Texture = BraveResources.Load("Win_Pic_Convict_001", ".png") as Texture;
							goto IL_384;
						case PlayableCharacters.Robot:
							this.photoSprite.Texture = BraveResources.Load("Win_Pic_Robot_001", ".png") as Texture;
							goto IL_384;
						case PlayableCharacters.Soldier:
							this.photoSprite.Texture = BraveResources.Load("Win_Pic_Marine_001", ".png") as Texture;
							goto IL_384;
						case PlayableCharacters.Guide:
							this.photoSprite.Texture = BraveResources.Load("Win_Pic_Hunter_001", ".png") as Texture;
							goto IL_384;
						case PlayableCharacters.Bullet:
							this.photoSprite.Texture = BraveResources.Load("Win_Pic_Bullet_001", ".png") as Texture;
							goto IL_384;
						}
						this.photoSprite.Texture = BraveResources.Load("Win_Pic_Gun_Get_001", ".png") as Texture;
						IL_384:;
					}
				}
				else if (GameManager.IsGunslingerPast)
				{
					this.photoSprite.Texture = BraveResources.Load("Win_Pic_Slinger_001", ".png") as Texture;
				}
				else
				{
					this.photoSprite.Texture = BraveResources.Load("Win_Pic_Lich_Kill_001", ".png") as Texture;
				}
			}
			else
			{
				this.photoSprite.Texture = BraveResources.Load("Win_Pic_Gun_Get_001", ".png") as Texture;
			}
		}
	}

	// Token: 0x06008A20 RID: 35360 RVA: 0x003986E0 File Offset: 0x003968E0
	public void BraveOnLevelWasLoaded()
	{
		this.m_doingSomething = false;
	}

	// Token: 0x06008A21 RID: 35361 RVA: 0x003986EC File Offset: 0x003968EC
	private void UpdateTapeLabel(dfLabel targetLabel, float overrideWidth = -1f)
	{
		dfPanel dfPanel = targetLabel.Parent as dfPanel;
		if (overrideWidth > 0f)
		{
			dfPanel.Width = overrideWidth.Quantize(4f) + 32f;
		}
		else
		{
			dfPanel.Width = targetLabel.Width + 32f;
		}
	}

	// Token: 0x06008A22 RID: 35362 RVA: 0x00398740 File Offset: 0x00396940
	private void Update()
	{
		if (this.isRightPage)
		{
			if (!this.isVictoryPage && this.photoSprite.RenderMaterial != null)
			{
				this.photoSprite.RenderMaterial.SetVector("_UVRescale", this.m_cachedUVRescale);
			}
		}
		else
		{
			if (!this.quickRestartButton.HasFocus && !this.mainMenuButton.HasFocus)
			{
				this.quickRestartButton.Focus(true);
			}
			if (AmmonomiconController.Instance && !AmmonomiconController.Instance.HandlingQueuedUnlocks && !AmmonomiconController.Instance.IsOpening && !AmmonomiconController.Instance.IsClosing)
			{
				if (Input.GetKeyDown(KeyCode.Escape) || (!BraveInput.PrimaryPlayerInstance.IsKeyboardAndMouse(false) && BraveInput.WasCancelPressed(null)))
				{
					this.DoMainMenu(null, null);
				}
				else if (Input.GetKeyDown(KeyCode.Space) || (!BraveInput.PrimaryPlayerInstance.IsKeyboardAndMouse(false) && BraveInput.WasSelectPressed(null)))
				{
					this.DoQuickRestart(null, null);
				}
			}
		}
	}

	// Token: 0x06008A23 RID: 35363 RVA: 0x00398864 File Offset: 0x00396A64
	private void DoMainMenu(dfControl control, dfMouseEventArgs mouseEvent)
	{
		if (AmmonomiconController.Instance.IsOpening || AmmonomiconController.Instance.IsClosing)
		{
			return;
		}
		SaveManager.DeleteCurrentSlotMidGameSave(null);
		GameManager.Instance.StartCoroutine(this.HandleMainMenu());
	}

	// Token: 0x06008A24 RID: 35364 RVA: 0x003988B0 File Offset: 0x00396AB0
	private IEnumerator HandleMainMenu()
	{
		if (this.m_doingSomething)
		{
			yield break;
		}
		this.m_doingSomething = true;
		if (BraveInput.PrimaryPlayerInstance.IsKeyboardAndMouse(false))
		{
			this.mainMenuButton.Text = "[sprite \"esc_up_002\"" + this.mainMenuButton.ForceGetLocalizedValue("#DEATH_MAINMENU");
		}
		Pixelator.Instance.ClearCachedFrame();
		if (this.m_temporaryPhotoTex)
		{
			RenderTexture.ReleaseTemporary(this.m_temporaryPhotoTex);
			this.m_temporaryPhotoTex = null;
		}
		Pixelator.Instance.DoFinalNonFadedLayer = false;
		GameUIRoot.Instance.ToggleUICamera(false);
		Pixelator.Instance.FadeToBlack(0.4f, false, 0f);
		AmmonomiconController.Instance.CloseAmmonomicon(false);
		AkSoundEngine.PostEvent("Play_UI_menu_cancel_01", base.gameObject);
		while (AmmonomiconController.Instance.IsOpen)
		{
			yield return null;
		}
		if (AmmonomiconController.Instance.CurrentLeftPageRenderer != null)
		{
			AmmonomiconController.Instance.CurrentLeftPageRenderer.Disable(false);
			AmmonomiconController.Instance.CurrentLeftPageRenderer.Dispose();
		}
		if (AmmonomiconController.Instance.CurrentRightPageRenderer != null)
		{
			AmmonomiconController.Instance.CurrentRightPageRenderer.Disable(false);
			AmmonomiconController.Instance.CurrentRightPageRenderer.Dispose();
		}
		yield return null;
		GameManager.Instance.LoadCharacterSelect(true, false);
		yield break;
	}

	// Token: 0x06008A25 RID: 35365 RVA: 0x003988CC File Offset: 0x00396ACC
	private void DoQuickRestart(dfControl control, dfMouseEventArgs mouseEvent)
	{
		if (AmmonomiconController.Instance.IsOpening || AmmonomiconController.Instance.IsClosing)
		{
			return;
		}
		if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.SUPERBOSSRUSH)
		{
			return;
		}
		SaveManager.DeleteCurrentSlotMidGameSave(null);
		GameManager.Instance.StartCoroutine(this.HandleQuickRestart());
	}

	// Token: 0x06008A26 RID: 35366 RVA: 0x00398928 File Offset: 0x00396B28
	public static QuickRestartOptions GetNumMetasToQuickRestart()
	{
		QuickRestartOptions quickRestartOptions = default(QuickRestartOptions);
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			if (GameManager.Instance.AllPlayers[i] && GameManager.Instance.AllPlayers[i].CharacterUsesRandomGuns)
			{
				quickRestartOptions.GunGame = true;
				quickRestartOptions.NumMetas += 6;
				break;
			}
			if (GameManager.Instance.AllPlayers[i].characterIdentity == PlayableCharacters.Eevee)
			{
				quickRestartOptions.NumMetas += 5;
			}
			else if (GameManager.Instance.AllPlayers[i].characterIdentity == PlayableCharacters.Gunslinger)
			{
				if (!GameStatsManager.Instance.GetFlag(GungeonFlags.GUNSLINGER_UNLOCKED))
				{
					quickRestartOptions.NumMetas += 5;
				}
				else
				{
					quickRestartOptions.NumMetas += 7;
				}
			}
		}
		if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.BOSSRUSH)
		{
			quickRestartOptions.BossRush = true;
			quickRestartOptions.NumMetas += 3;
		}
		if (ChallengeManager.CHALLENGE_MODE_ACTIVE)
		{
			quickRestartOptions.ChallengeMode = ChallengeManager.ChallengeModeType;
			if (ChallengeManager.ChallengeModeType == ChallengeModeType.ChallengeMode)
			{
				if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.CHALLENGE_MODE_ATTEMPTS) >= 30f)
				{
					quickRestartOptions.NumMetas++;
				}
				else
				{
					quickRestartOptions.NumMetas += 6;
				}
			}
			else if (ChallengeManager.ChallengeModeType == ChallengeModeType.ChallengeMegaMode)
			{
				if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.CHALLENGE_MODE_ATTEMPTS) >= 30f)
				{
					quickRestartOptions.NumMetas += 2;
				}
				else
				{
					quickRestartOptions.NumMetas += 12;
				}
			}
		}
		return quickRestartOptions;
	}

	// Token: 0x06008A27 RID: 35367 RVA: 0x00398AEC File Offset: 0x00396CEC
	private IEnumerator HandleQuickRestart()
	{
		if (GameManager.Instance.IsLoadingLevel || this.m_doingSomething)
		{
			yield break;
		}
		this.m_doingSomething = true;
		if (BraveInput.PrimaryPlayerInstance.IsKeyboardAndMouse(false))
		{
			this.quickRestartButton.Text = "[sprite \"space_bar_down_001\"" + this.quickRestartButton.ForceGetLocalizedValue("#DEATH_QUICKRESTART");
		}
		AkSoundEngine.PostEvent("Play_UI_menu_characterselect_01", base.gameObject);
		Pixelator.Instance.ClearCachedFrame();
		if (this.m_temporaryPhotoTex)
		{
			RenderTexture.ReleaseTemporary(this.m_temporaryPhotoTex);
			this.m_temporaryPhotoTex = null;
		}
		AmmonomiconController.Instance.CloseAmmonomicon(false);
		while (AmmonomiconController.Instance.IsOpen)
		{
			if (!AmmonomiconController.Instance.IsClosing)
			{
				AmmonomiconController.Instance.CloseAmmonomicon(false);
			}
			yield return null;
		}
		if (AmmonomiconController.Instance.CurrentLeftPageRenderer != null)
		{
			AmmonomiconController.Instance.CurrentLeftPageRenderer.Disable(false);
			AmmonomiconController.Instance.CurrentLeftPageRenderer.Dispose();
		}
		if (AmmonomiconController.Instance.CurrentRightPageRenderer != null)
		{
			AmmonomiconController.Instance.CurrentRightPageRenderer.Disable(false);
			AmmonomiconController.Instance.CurrentRightPageRenderer.Dispose();
		}
		yield return null;
		if (GameManager.LastUsedPlayerPrefab && GameManager.LastUsedPlayerPrefab.GetComponent<PlayerController>().characterIdentity == PlayableCharacters.Gunslinger && !GameStatsManager.Instance.GetFlag(GungeonFlags.GUNSLINGER_UNLOCKED))
		{
			GameManager.LastUsedPlayerPrefab = (GameObject)ResourceCache.Acquire("PlayerEevee");
		}
		QuickRestartOptions qrOptions = AmmonomiconDeathPageController.GetNumMetasToQuickRestart();
		if (qrOptions.NumMetas > 0)
		{
			GameUIRoot.Instance.CheckKeepModifiersQuickRestart(qrOptions.NumMetas);
			while (!GameUIRoot.Instance.HasSelectedAreYouSureOption())
			{
				yield return null;
			}
			if (!GameUIRoot.Instance.GetAreYouSureOption())
			{
				qrOptions = default(QuickRestartOptions);
				if (GameManager.LastUsedPlayerPrefab && (GameManager.LastUsedPlayerPrefab.GetComponent<PlayerController>().characterIdentity == PlayableCharacters.Eevee || GameManager.LastUsedPlayerPrefab.GetComponent<PlayerController>().characterIdentity == PlayableCharacters.Gunslinger))
				{
					GameManager.LastUsedPlayerPrefab = (GameObject)ResourceCache.Acquire(CharacterSelectController.GetCharacterPathFromQuickStart());
				}
			}
		}
		GameUIRoot.Instance.ToggleUICamera(false);
		Pixelator.Instance.DoFinalNonFadedLayer = false;
		Pixelator.Instance.FadeToBlack(0.4f, false, 0f);
		GameManager.Instance.DelayedQuickRestart(0.5f, qrOptions);
		yield break;
	}

	// Token: 0x04009096 RID: 37014
	public static bool LastKilledPlayerPrimary = true;

	// Token: 0x04009097 RID: 37015
	public bool isRightPage;

	// Token: 0x04009098 RID: 37016
	public bool isVictoryPage;

	// Token: 0x04009099 RID: 37017
	[HideInInspectorIf("isRightPage", false)]
	public dfLabel youDiedLabel;

	// Token: 0x0400909A RID: 37018
	[HideInInspectorIf("isRightPage", false)]
	public dfLabel gungeoneerTitle;

	// Token: 0x0400909B RID: 37019
	[HideInInspectorIf("isRightPage", false)]
	public dfLabel areaTitle;

	// Token: 0x0400909C RID: 37020
	[HideInInspectorIf("isRightPage", false)]
	public dfLabel timeTitle;

	// Token: 0x0400909D RID: 37021
	[HideInInspectorIf("isRightPage", false)]
	public dfLabel moneyTitle;

	// Token: 0x0400909E RID: 37022
	[HideInInspectorIf("isRightPage", false)]
	public dfLabel killsTitle;

	// Token: 0x0400909F RID: 37023
	[HideInInspectorIf("isRightPage", false)]
	public dfLabel gungeoneerLabel;

	// Token: 0x040090A0 RID: 37024
	[HideInInspectorIf("isRightPage", false)]
	public dfLabel areaLabel;

	// Token: 0x040090A1 RID: 37025
	[HideInInspectorIf("isRightPage", false)]
	public dfLabel timeLabel;

	// Token: 0x040090A2 RID: 37026
	[HideInInspectorIf("isRightPage", false)]
	public dfLabel moneyLabel;

	// Token: 0x040090A3 RID: 37027
	[HideInInspectorIf("isRightPage", false)]
	public dfLabel killsLabel;

	// Token: 0x040090A4 RID: 37028
	[HideInInspectorIf("isRightPage", false)]
	public dfLabel deathNumberLabel;

	// Token: 0x040090A5 RID: 37029
	[HideInInspectorIf("isRightPage", false)]
	public dfLabel coopDeathNumberLabel;

	// Token: 0x040090A6 RID: 37030
	[HideInInspectorIf("isRightPage", false)]
	public dfLabel coopIndividualDeathsNumberLabel;

	// Token: 0x040090A7 RID: 37031
	[HideInInspectorIf("isRightPage", false)]
	public dfLabel hauntingLabel;

	// Token: 0x040090A8 RID: 37032
	[HideInInspectorIf("isRightPage", false)]
	public dfButton quickRestartButton;

	// Token: 0x040090A9 RID: 37033
	[HideInInspectorIf("isRightPage", false)]
	public dfButton mainMenuButton;

	// Token: 0x040090AA RID: 37034
	[ShowInInspectorIf("isRightPage", false)]
	public dfLabel killedByHeaderLabel;

	// Token: 0x040090AB RID: 37035
	[ShowInInspectorIf("isRightPage", false)]
	public dfLabel killedByLabel;

	// Token: 0x040090AC RID: 37036
	[ShowInInspectorIf("isRightPage", false)]
	public dfTextureSprite photoSprite;

	// Token: 0x040090AD RID: 37037
	[ShowInInspectorIf("isRightPage", false)]
	public dfSprite ChallengeModeRibbon;

	// Token: 0x040090AE RID: 37038
	[ShowInInspectorIf("isRightPage", false)]
	public dfSprite RatDeathDrawings;

	// Token: 0x040090AF RID: 37039
	private Vector4 m_cachedUVRescale;

	// Token: 0x040090B0 RID: 37040
	private RenderTexture m_temporaryPhotoTex;

	// Token: 0x040090B1 RID: 37041
	private bool m_doingSomething;
}
