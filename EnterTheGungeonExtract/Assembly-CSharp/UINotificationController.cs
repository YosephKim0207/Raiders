using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200180F RID: 6159
public class UINotificationController : MonoBehaviour
{
	// Token: 0x170015C4 RID: 5572
	// (get) Token: 0x06009133 RID: 37171 RVA: 0x003D5DC8 File Offset: 0x003D3FC8
	public dfPanel Panel
	{
		get
		{
			return this.m_panel;
		}
	}

	// Token: 0x170015C5 RID: 5573
	// (get) Token: 0x06009134 RID: 37172 RVA: 0x003D5DD0 File Offset: 0x003D3FD0
	public bool IsDoingNotification
	{
		get
		{
			return this.m_doingNotification;
		}
	}

	// Token: 0x06009135 RID: 37173 RVA: 0x003D5DD8 File Offset: 0x003D3FD8
	private void Start()
	{
		if (this.EnglishFont == null)
		{
			this.EnglishFont = this.DescriptionLabel.DefaultAssignedFont;
			this.OtherLanguageFont = this.DescriptionLabel.GUIManager.DefaultFont;
			this.NameBasePos = this.NameLabel.RelativePosition;
			this.DescBasePos = this.DescriptionLabel.RelativePosition;
		}
	}

	// Token: 0x06009136 RID: 37174 RVA: 0x003D5E40 File Offset: 0x003D4040
	public void Initialize()
	{
		this.m_panel = base.GetComponent<dfPanel>();
		GameUIRoot.Instance.AddControlToMotionGroups(this.m_panel, DungeonData.Direction.SOUTH, true);
		this.notificationObjectSprite.HeightOffGround = GameUIRoot.Instance.transform.position.y - 2f;
		this.notificationSynergySprite.HeightOffGround = GameUIRoot.Instance.transform.position.y - 1f;
		SpriteOutlineManager.AddOutlineToSprite(this.notificationObjectSprite, Color.white, -1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		this.outlineSprites = SpriteOutlineManager.GetOutlineSprites(this.notificationObjectSprite);
		for (int i = 0; i < this.outlineSprites.Length; i++)
		{
			tk2dSprite tk2dSprite = this.outlineSprites[i];
			if (tk2dSprite)
			{
				tk2dSprite.gameObject.layer = this.notificationObjectSprite.gameObject.layer;
				tk2dSprite.renderer.enabled = this.notificationObjectSprite.renderer.enabled;
				tk2dSprite.HeightOffGround = -0.25f;
			}
		}
		SpriteOutlineManager.AddOutlineToSprite(this.notificationSynergySprite, Color.white, -1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		this.synergyOutlineSprites = SpriteOutlineManager.GetOutlineSprites(this.notificationSynergySprite);
		for (int j = 0; j < this.synergyOutlineSprites.Length; j++)
		{
			tk2dSprite tk2dSprite2 = this.synergyOutlineSprites[j];
			if (tk2dSprite2)
			{
				tk2dSprite2.gameObject.layer = this.notificationSynergySprite.gameObject.layer;
				tk2dSprite2.renderer.enabled = this.notificationSynergySprite.renderer.enabled;
				tk2dSprite2.HeightOffGround = -0.25f;
			}
		}
		this.notificationObjectSprite.UpdateZDepth();
		this.notificationSynergySprite.UpdateZDepth();
		this.CheckLanguageFonts();
		base.StartCoroutine(this.BG_CoroutineProcessor());
	}

	// Token: 0x06009137 RID: 37175 RVA: 0x003D6030 File Offset: 0x003D4230
	public void ForceHide()
	{
		if (this.IsDoingNotification)
		{
			GameUIRoot.Instance.MoveNonCoreGroupOnscreen(this.m_panel, true);
			this.m_currentNotificationProcess = null;
			this.m_queuedNotifications.Clear();
		}
	}

	// Token: 0x06009138 RID: 37176 RVA: 0x003D6060 File Offset: 0x003D4260
	private IEnumerator BG_CoroutineProcessor()
	{
		for (;;)
		{
			if (this.m_queuedNotifications.Count != this.m_queuedNotificationParams.Count)
			{
				this.m_queuedNotificationParams.Clear();
				this.m_queuedNotifications.Clear();
			}
			if (this.m_currentNotificationProcess != null)
			{
				this.m_doingNotification = true;
				if (!this.m_currentNotificationProcess.MoveNext())
				{
					this.m_currentNotificationProcess = null;
				}
			}
			if (this.m_currentNotificationProcess == null)
			{
				if (this.m_queuedNotificationParams.Count > 0)
				{
					while (this.m_queuedNotificationParams.Count > 0 && this.m_queuedNotificationParams[0].OnlyIfSynergy && !this.m_queuedNotificationParams[0].HasAttachedSynergy)
					{
						this.m_queuedNotificationParams.RemoveAt(0);
						this.m_queuedNotifications.RemoveAt(0);
					}
				}
				if (this.m_queuedNotifications.Count > 0)
				{
					this.m_currentNotificationProcess = this.m_queuedNotifications[0];
					this.m_queuedNotifications.RemoveAt(0);
					if (this.m_queuedNotificationParams.Count > 0)
					{
						this.m_queuedNotificationParams.RemoveAt(0);
					}
				}
				else
				{
					if (this.m_panel.IsVisible)
					{
						this.DisableRenderers();
					}
					this.m_doingNotification = false;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06009139 RID: 37177 RVA: 0x003D607C File Offset: 0x003D427C
	private float ActualSign(float f)
	{
		if (Mathf.Abs(f) < 0.0001f)
		{
			return 0f;
		}
		if (f < 0f)
		{
			return -1f;
		}
		if (f > 0f)
		{
			return 1f;
		}
		return 0f;
	}

	// Token: 0x0600913A RID: 37178 RVA: 0x003D60BC File Offset: 0x003D42BC
	private void CheckLanguageFonts()
	{
		if (this.EnglishFont == null)
		{
			this.EnglishFont = this.DescriptionLabel.Font;
			this.OtherLanguageFont = this.DescriptionLabel.GUIManager.DefaultFont;
			this.NameBasePos = this.NameLabel.RelativePosition;
			this.DescBasePos = this.DescriptionLabel.RelativePosition;
		}
		if (StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
		{
			if (this.m_cachedLanguage != StringTableManager.GungeonSupportedLanguages.ENGLISH)
			{
				this.NameLabel.RelativePosition = this.NameBasePos;
				this.NameLabel.Font = this.EnglishFont;
				this.NameLabel.TextScale = 0.6f;
				this.DescriptionLabel.RelativePosition = this.DescBasePos;
				this.DescriptionLabel.Font = this.EnglishFont;
				this.DescriptionLabel.TextScale = 0.6f;
			}
		}
		else if (StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.JAPANESE)
		{
			if (this.m_cachedLanguage != StringTableManager.GungeonSupportedLanguages.JAPANESE)
			{
				this.NameLabel.RelativePosition = this.NameBasePos + new Vector3(0f, -9f, 0f);
				this.NameLabel.TextScale = 3f;
				this.DescriptionLabel.RelativePosition = this.DescBasePos + new Vector3(0f, -6f, 0f);
				this.DescriptionLabel.TextScale = 3f;
			}
		}
		else if (StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE)
		{
			if (StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE)
			{
				this.NameLabel.RelativePosition = this.NameBasePos + new Vector3(0f, -24f, 0f);
				this.NameLabel.TextScale = 3f;
				this.DescriptionLabel.RelativePosition = this.DescBasePos + new Vector3(0f, -12f, 0f);
				this.DescriptionLabel.TextScale = 3f;
			}
		}
		else if (StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.KOREAN)
		{
			if (this.m_cachedLanguage != StringTableManager.CurrentLanguage)
			{
				this.NameLabel.RelativePosition = this.NameBasePos + new Vector3(0f, -9f, 0f);
				this.DescriptionLabel.RelativePosition = this.DescBasePos + new Vector3(0f, -6f, 0f);
			}
		}
		else if (StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.RUSSIAN)
		{
			if (this.m_cachedLanguage != StringTableManager.CurrentLanguage)
			{
				this.NameLabel.RelativePosition = this.NameBasePos + new Vector3(0f, -12f, 0f);
				this.NameLabel.TextScale = 3f;
				this.DescriptionLabel.RelativePosition = this.DescBasePos + new Vector3(0f, -9f, 0f);
				this.DescriptionLabel.TextScale = 3f;
			}
		}
		else if (this.m_cachedLanguage != StringTableManager.CurrentLanguage)
		{
			this.NameLabel.RelativePosition = this.NameBasePos + new Vector3(0f, -12f, 0f);
			this.NameLabel.Font = this.OtherLanguageFont;
			this.NameLabel.TextScale = 3f;
			this.DescriptionLabel.RelativePosition = this.DescBasePos + new Vector3(0f, -9f, 0f);
			this.DescriptionLabel.Font = this.OtherLanguageFont;
			this.DescriptionLabel.TextScale = 3f;
		}
		this.m_cachedLanguage = StringTableManager.CurrentLanguage;
	}

	// Token: 0x0600913B RID: 37179 RVA: 0x003D647C File Offset: 0x003D467C
	private void SetWidths()
	{
		bool flag = GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.ENGLISH && GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.JAPANESE && GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.KOREAN && GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.CHINESE;
		if (flag && !this.m_isCurrentlyExpanded)
		{
			this.m_isCurrentlyExpanded = true;
			this.m_panel.Width = this.m_panel.Width + 126f;
			this.NameLabel.Width = this.NameLabel.Width + 126f;
			this.DescriptionLabel.Width = this.DescriptionLabel.Width + 126f;
			this.BoxSprite.Width = this.BoxSprite.Width + 42f;
			this.m_panel.PerformLayout();
			GameUIRoot.Instance.MoveNonCoreGroupImmediately(this.m_panel, false);
			GameUIRoot.Instance.UpdateControlMotionGroup(this.m_panel);
			GameUIRoot.Instance.MoveNonCoreGroupImmediately(this.m_panel, true);
		}
		else if (!flag && this.m_isCurrentlyExpanded)
		{
			this.m_isCurrentlyExpanded = false;
			this.NameLabel.Width = this.NameLabel.Width - 126f;
			this.DescriptionLabel.Width = this.DescriptionLabel.Width - 126f;
			this.BoxSprite.Width = this.BoxSprite.Width - 42f;
			this.m_panel.Width = this.m_panel.Width - 126f;
			this.m_panel.PerformLayout();
			GameUIRoot.Instance.MoveNonCoreGroupImmediately(this.m_panel, false);
			GameUIRoot.Instance.UpdateControlMotionGroup(this.m_panel);
			GameUIRoot.Instance.MoveNonCoreGroupImmediately(this.m_panel, true);
		}
	}

	// Token: 0x0600913C RID: 37180 RVA: 0x003D665C File Offset: 0x003D485C
	public void DoNotification(EncounterTrackable trackable, bool onlyIfSynergy = false)
	{
		if (!trackable)
		{
			return;
		}
		this.CheckLanguageFonts();
		this.SetWidths();
		tk2dBaseSprite component = trackable.GetComponent<tk2dBaseSprite>();
		NotificationParams notificationParams = new NotificationParams();
		notificationParams.EncounterGuid = trackable.EncounterGuid;
		PickupObject component2 = trackable.GetComponent<PickupObject>();
		if (component2)
		{
			notificationParams.pickupId = component2.PickupObjectId;
		}
		notificationParams.SpriteCollection = component.Collection;
		notificationParams.SpriteID = component.spriteId;
		notificationParams = this.SetupTexts(trackable, notificationParams);
		notificationParams.OnlyIfSynergy = onlyIfSynergy;
		if (StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.JAPANESE)
		{
			this.NameLabel.RelativePosition = this.NameBasePos + new Vector3(0f, -9f, 0f);
			this.NameLabel.TextScale = 3f;
			this.DescriptionLabel.RelativePosition = this.DescBasePos + new Vector3(0f, -6f, 0f);
			this.DescriptionLabel.TextScale = 3f;
		}
		this.DoNotificationInternal(notificationParams);
	}

	// Token: 0x0600913D RID: 37181 RVA: 0x003D6768 File Offset: 0x003D4968
	private void DoNotificationInternal(NotificationParams notifyParams)
	{
		this.m_queuedNotifications.Add(this.HandleNotification(notifyParams));
		this.m_queuedNotificationParams.Add(notifyParams);
		base.StartCoroutine(this.PruneQueuedNotifications());
	}

	// Token: 0x0600913E RID: 37182 RVA: 0x003D6798 File Offset: 0x003D4998
	private IEnumerator PruneQueuedNotifications()
	{
		yield return null;
		if (this.m_queuedNotifications.Count <= 1)
		{
			yield break;
		}
		if (this.m_queuedNotifications.Count == 2 && !this.IsDoingNotification)
		{
			yield break;
		}
		int startIndex = ((!this.IsDoingNotification) ? 1 : 0);
		if (startIndex >= this.m_queuedNotifications.Count - 1)
		{
			yield break;
		}
		for (int i = startIndex; i < this.m_queuedNotifications.Count - 1; i++)
		{
			NotificationParams notificationParams = this.m_queuedNotificationParams[i];
			if (!notificationParams.HasAttachedSynergy)
			{
				this.m_queuedNotificationParams.RemoveAt(i);
				this.m_queuedNotifications.RemoveAt(i);
				i--;
			}
		}
		yield break;
	}

	// Token: 0x0600913F RID: 37183 RVA: 0x003D67B4 File Offset: 0x003D49B4
	public void DoCustomNotification(string header, string description, tk2dSpriteCollectionData collection, int spriteId, UINotificationController.NotificationColor notifyColor = UINotificationController.NotificationColor.SILVER, bool allowQueueing = false, bool forceSingleLine = false)
	{
		this.CheckLanguageFonts();
		this.DoNotificationInternal(new NotificationParams
		{
			SpriteCollection = collection,
			SpriteID = spriteId,
			PrimaryTitleString = header.ToUpperInvariant(),
			SecondaryDescriptionString = description,
			isSingleLine = forceSingleLine,
			forcedColor = notifyColor
		});
	}

	// Token: 0x06009140 RID: 37184 RVA: 0x003D6808 File Offset: 0x003D4A08
	public void AttemptSynergyAttachment(AdvancedSynergyEntry e)
	{
		for (int i = this.m_queuedNotificationParams.Count - 1; i >= 0; i--)
		{
			NotificationParams notificationParams = this.m_queuedNotificationParams[i];
			if (!string.IsNullOrEmpty(notificationParams.EncounterGuid))
			{
				EncounterDatabaseEntry entry = EncounterDatabase.GetEntry(notificationParams.EncounterGuid);
				int num = ((entry == null) ? (-1) : entry.pickupObjectId);
				if (num >= 0 && e.ContainsPickup(num))
				{
					notificationParams.HasAttachedSynergy = true;
					notificationParams.AttachedSynergy = e;
					this.m_queuedNotificationParams[i] = notificationParams;
					break;
				}
			}
		}
	}

	// Token: 0x06009141 RID: 37185 RVA: 0x003D68A4 File Offset: 0x003D4AA4
	private NotificationParams SetupTexts(EncounterTrackable trackable, NotificationParams notifyParams)
	{
		string text = trackable.name;
		string text2 = "???";
		if (!string.IsNullOrEmpty(trackable.journalData.GetPrimaryDisplayName(false)))
		{
			text = trackable.journalData.GetPrimaryDisplayName(false);
			if (text.Contains("®"))
			{
				text = text.Replace('®', '@');
			}
		}
		else
		{
			PickupObject component = trackable.GetComponent<PickupObject>();
			if (component != null)
			{
				text = component.DisplayName;
			}
		}
		if (trackable.GetComponent<SpiceItem>() != null)
		{
			int num = GameManager.Instance.PrimaryPlayer.spiceCount;
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				num += GameManager.Instance.SecondaryPlayer.spiceCount;
			}
			text2 = trackable.journalData.GetCustomNotificationPanelDescription(Mathf.Min(num, 3));
		}
		else if (!string.IsNullOrEmpty(trackable.journalData.GetNotificationPanelDescription()))
		{
			text2 = trackable.journalData.GetNotificationPanelDescription();
		}
		notifyParams.PrimaryTitleString = text.ToUpperInvariant();
		notifyParams.SecondaryDescriptionString = text2;
		return notifyParams;
	}

	// Token: 0x06009142 RID: 37186 RVA: 0x003D69B0 File Offset: 0x003D4BB0
	private void ResetSynergySprite()
	{
		this.notificationSynergySprite.renderer.enabled = false;
		this.notificationSynergySprite.scale = GameUIUtility.GetCurrentTK2D_DFScale(this.m_panel.GetManager()) * Vector3.one;
		for (int i = 0; i < this.synergyOutlineSprites.Length; i++)
		{
			this.synergyOutlineSprites[i].renderer.enabled = false;
			this.synergyOutlineSprites[i].transform.localPosition = (new Vector3(this.ActualSign(this.synergyOutlineSprites[i].transform.localPosition.x) * 0.0625f, this.ActualSign(this.synergyOutlineSprites[i].transform.localPosition.y) * 0.0625f, 0f) * Pixelator.Instance.CurrentTileScale * 16f * GameUIRoot.Instance.PixelsToUnits()).WithZ(1f);
			this.synergyOutlineSprites[i].scale = this.notificationObjectSprite.scale;
		}
		Vector3 center = this.ObjectBoxSprite.GetCenter();
		this.notificationSynergySprite.PlaceAtPositionByAnchor(center, tk2dBaseSprite.Anchor.MiddleCenter);
	}

	// Token: 0x06009143 RID: 37187 RVA: 0x003D6AEC File Offset: 0x003D4CEC
	private void SetupSynergySprite(tk2dSpriteCollectionData collection, int spriteId)
	{
		this.notificationSynergySprite.SetSprite(collection, spriteId);
		Vector3 center = this.ObjectBoxSprite.GetCenter();
		this.notificationSynergySprite.PlaceAtPositionByAnchor(center, tk2dBaseSprite.Anchor.MiddleCenter);
		this.notificationSynergySprite.transform.localPosition = this.notificationSynergySprite.transform.localPosition.Quantize(this.BoxSprite.PixelsToUnits() * 3f);
	}

	// Token: 0x06009144 RID: 37188 RVA: 0x003D6B58 File Offset: 0x003D4D58
	private void SetupSprite(tk2dSpriteCollectionData collection, int spriteId)
	{
		this.ResetSynergySprite();
		if (collection == null || spriteId < 0)
		{
			this.notificationObjectSprite.renderer.enabled = false;
			for (int i = 0; i < this.outlineSprites.Length; i++)
			{
				this.outlineSprites[i].renderer.enabled = false;
			}
			return;
		}
		this.notificationObjectSprite.renderer.enabled = false;
		this.notificationObjectSprite.SetSprite(collection, spriteId);
		this.notificationObjectSprite.scale = GameUIUtility.GetCurrentTK2D_DFScale(this.m_panel.GetManager()) * Vector3.one;
		for (int j = 0; j < this.outlineSprites.Length; j++)
		{
			this.outlineSprites[j].renderer.enabled = false;
			this.outlineSprites[j].transform.localPosition = (new Vector3(this.ActualSign(this.outlineSprites[j].transform.localPosition.x) * 0.0625f, this.ActualSign(this.outlineSprites[j].transform.localPosition.y) * 0.0625f, 0f) * Pixelator.Instance.CurrentTileScale * 16f * GameUIRoot.Instance.PixelsToUnits()).WithZ(1f);
			this.outlineSprites[j].scale = this.notificationObjectSprite.scale;
		}
		Vector3 center = this.ObjectBoxSprite.GetCenter();
		this.notificationObjectSprite.PlaceAtPositionByAnchor(center, tk2dBaseSprite.Anchor.MiddleCenter);
		this.notificationObjectSprite.transform.localPosition = this.notificationObjectSprite.transform.localPosition.Quantize(this.BoxSprite.PixelsToUnits() * 3f);
	}

	// Token: 0x06009145 RID: 37189 RVA: 0x003D6D30 File Offset: 0x003D4F30
	private void ToggleSynergyStatus(bool synergy)
	{
		if (synergy)
		{
			this.CrosshairSprite.SpriteName = "crosshair_synergy";
			this.CrosshairSprite.Size = this.CrosshairSprite.SpriteInfo.sizeInPixels * 3f;
			this.BoxSprite.SpriteName = "notification_box_synergy";
			this.ObjectBoxSprite.IsVisible = false;
			this.StickerSprite.IsVisible = false;
		}
	}

	// Token: 0x06009146 RID: 37190 RVA: 0x003D6DA0 File Offset: 0x003D4FA0
	private void ToggleGoldStatus(bool gold)
	{
		this.CrosshairSprite.SpriteName = ((!gold) ? "crosshair" : "crosshair_gold");
		this.CrosshairSprite.Size = this.CrosshairSprite.SpriteInfo.sizeInPixels * 3f;
		this.BoxSprite.SpriteName = ((!gold) ? "notification_box" : "notification_box_gold_001");
		this.ObjectBoxSprite.IsVisible = true;
		this.ObjectBoxSprite.SpriteName = ((!gold) ? "object_box" : "object_box_gold_001");
		this.StickerSprite.IsVisible = gold;
	}

	// Token: 0x06009147 RID: 37191 RVA: 0x003D6E4C File Offset: 0x003D504C
	private void TogglePurpleStatus(bool purple)
	{
		if (purple)
		{
			this.CrosshairSprite.SpriteName = "crosshair_gold";
			this.CrosshairSprite.Size = this.CrosshairSprite.SpriteInfo.sizeInPixels * 3f;
			this.BoxSprite.SpriteName = "notification_box_purp_001";
			this.ObjectBoxSprite.IsVisible = true;
			this.ObjectBoxSprite.SpriteName = "object_box_purp_001";
			this.StickerSprite.IsVisible = false;
		}
	}

	// Token: 0x06009148 RID: 37192 RVA: 0x003D6ECC File Offset: 0x003D50CC
	private void DisableRenderers()
	{
		this.notificationObjectSprite.renderer.enabled = false;
		SpriteOutlineManager.ToggleOutlineRenderers(this.notificationObjectSprite, false);
		this.m_panel.IsVisible = false;
	}

	// Token: 0x06009149 RID: 37193 RVA: 0x003D6EF8 File Offset: 0x003D50F8
	public void ForceToFront()
	{
		if (this.m_panel)
		{
			this.m_panel.Parent.BringToFront();
		}
	}

	// Token: 0x0600914A RID: 37194 RVA: 0x003D6F1C File Offset: 0x003D511C
	private int GetIDOfOwnedSynergizingItem(int sourceID, AdvancedSynergyEntry syn)
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			for (int j = 0; j < playerController.inventory.AllGuns.Count; j++)
			{
				int pickupObjectId = playerController.inventory.AllGuns[j].PickupObjectId;
				if (pickupObjectId != sourceID && syn.ContainsPickup(pickupObjectId))
				{
					return pickupObjectId;
				}
			}
			for (int k = 0; k < playerController.activeItems.Count; k++)
			{
				int pickupObjectId2 = playerController.activeItems[k].PickupObjectId;
				if (pickupObjectId2 != sourceID && syn.ContainsPickup(pickupObjectId2))
				{
					return pickupObjectId2;
				}
			}
			for (int l = 0; l < playerController.passiveItems.Count; l++)
			{
				int pickupObjectId3 = playerController.passiveItems[l].PickupObjectId;
				if (pickupObjectId3 != sourceID && syn.ContainsPickup(pickupObjectId3))
				{
					return pickupObjectId3;
				}
			}
		}
		return -1;
	}

	// Token: 0x0600914B RID: 37195 RVA: 0x003D703C File Offset: 0x003D523C
	private IEnumerator HandleNotification(NotificationParams notifyParams)
	{
		yield return null;
		this.SetupSprite(notifyParams.SpriteCollection, notifyParams.SpriteID);
		this.DescriptionLabel.ProcessMarkup = true;
		this.DescriptionLabel.ColorizeSymbols = true;
		this.NameLabel.Text = notifyParams.PrimaryTitleString.ToUpperInvariant();
		this.DescriptionLabel.Text = notifyParams.SecondaryDescriptionString;
		this.CenterLabel.Opacity = 1f;
		this.NameLabel.Opacity = 1f;
		this.DescriptionLabel.Opacity = 1f;
		this.CenterLabel.IsVisible = false;
		this.NameLabel.IsVisible = true;
		this.DescriptionLabel.IsVisible = true;
		dfSpriteAnimation component = this.BoxSprite.GetComponent<dfSpriteAnimation>();
		component.Stop();
		dfSpriteAnimation component2 = this.CrosshairSprite.GetComponent<dfSpriteAnimation>();
		component2.Stop();
		dfSpriteAnimation component3 = this.ObjectBoxSprite.GetComponent<dfSpriteAnimation>();
		component3.Stop();
		UINotificationController.NotificationColor forcedColor = notifyParams.forcedColor;
		string trackableGuid = notifyParams.EncounterGuid;
		bool isGold = forcedColor == UINotificationController.NotificationColor.GOLD || (!string.IsNullOrEmpty(trackableGuid) && GameStatsManager.Instance.QueryEncounterable(trackableGuid) == 1);
		bool isPurple = forcedColor == UINotificationController.NotificationColor.PURPLE || (!string.IsNullOrEmpty(trackableGuid) && EncounterDatabase.GetEntry(trackableGuid).usesPurpleNotifications);
		this.ToggleGoldStatus(isGold);
		this.TogglePurpleStatus(isPurple);
		bool singleLineSansSprite = notifyParams.isSingleLine;
		if (singleLineSansSprite || notifyParams.SpriteCollection == null)
		{
			this.ObjectBoxSprite.IsVisible = false;
			this.StickerSprite.IsVisible = false;
		}
		if (singleLineSansSprite)
		{
			this.CenterLabel.IsVisible = true;
			this.NameLabel.IsVisible = false;
			this.DescriptionLabel.IsVisible = false;
			this.CenterLabel.Text = this.NameLabel.Text;
		}
		else
		{
			this.NameLabel.IsVisible = true;
			this.DescriptionLabel.IsVisible = true;
			this.CenterLabel.IsVisible = false;
		}
		this.m_doingNotification = true;
		this.m_panel.IsVisible = false;
		GameUIRoot.Instance.MoveNonCoreGroupOnscreen(this.m_panel, false);
		float elapsed = 0f;
		float duration = 5f;
		bool hasPlayedAnim = false;
		if (singleLineSansSprite)
		{
			this.notificationObjectSprite.renderer.enabled = false;
			SpriteOutlineManager.ToggleOutlineRenderers(this.notificationObjectSprite, false);
		}
		while (elapsed < ((!notifyParams.HasAttachedSynergy) ? duration : (duration - 2f)))
		{
			elapsed += BraveTime.DeltaTime;
			if (!hasPlayedAnim && elapsed > 0.75f)
			{
				this.BoxSprite.GetComponent<dfSpriteAnimation>().Clip = ((!isPurple) ? ((!isGold) ? this.SilverAnimClip : this.GoldAnimClip) : this.PurpleAnimClip);
				hasPlayedAnim = true;
				this.ObjectBoxSprite.Parent.GetComponent<dfSpriteAnimation>().Play();
			}
			yield return null;
			this.m_panel.IsVisible = true;
			if (!singleLineSansSprite && notifyParams.SpriteCollection != null)
			{
				this.notificationObjectSprite.renderer.enabled = true;
				SpriteOutlineManager.ToggleOutlineRenderers(this.notificationObjectSprite, true);
			}
		}
		if (notifyParams.HasAttachedSynergy)
		{
			AdvancedSynergyEntry pairedSynergy = notifyParams.AttachedSynergy;
			EncounterDatabaseEntry encounterSource = EncounterDatabase.GetEntry(trackableGuid);
			int pickupObjectId = ((encounterSource == null) ? (-1) : encounterSource.pickupObjectId);
			PickupObject puo = PickupObjectDatabase.GetById(pickupObjectId);
			if (puo)
			{
				int pID = this.GetIDOfOwnedSynergizingItem(puo.PickupObjectId, pairedSynergy);
				PickupObject puo2 = PickupObjectDatabase.GetById(pID);
				if (puo2 && puo2.sprite)
				{
					this.SetupSynergySprite(puo2.sprite.Collection, puo2.sprite.spriteId);
					elapsed = 0f;
					duration = 4f;
					this.notificationSynergySprite.renderer.enabled = true;
					SpriteOutlineManager.ToggleOutlineRenderers(this.notificationSynergySprite, true);
					dfSpriteAnimation boxSpriteAnimator = this.BoxSprite.GetComponent<dfSpriteAnimation>();
					boxSpriteAnimator.Clip = this.SynergyTransformClip;
					boxSpriteAnimator.Play();
					dfSpriteAnimation crosshairSpriteAnimator = this.CrosshairSprite.GetComponent<dfSpriteAnimation>();
					crosshairSpriteAnimator.Clip = this.SynergyCrosshairTransformClip;
					crosshairSpriteAnimator.Play();
					dfSpriteAnimation objectSpriteAnimator = this.ObjectBoxSprite.GetComponent<dfSpriteAnimation>();
					objectSpriteAnimator.Clip = this.SynergyBoxTransformClip;
					objectSpriteAnimator.Play();
					string synergyName = (string.IsNullOrEmpty(pairedSynergy.NameKey) ? string.Empty : StringTableManager.GetSynergyString(pairedSynergy.NameKey, -1));
					bool synergyHasName = !string.IsNullOrEmpty(synergyName);
					if (synergyHasName)
					{
						this.CenterLabel.IsVisible = true;
						this.CenterLabel.Text = synergyName;
					}
					while (elapsed < duration)
					{
						float baseSpriteLocalX = this.notificationObjectSprite.transform.localPosition.x;
						float synSpriteLocalX = this.notificationSynergySprite.transform.localPosition.x;
						this.CrosshairSprite.Size = this.CrosshairSprite.SpriteInfo.sizeInPixels * 3f;
						float p2u = this.BoxSprite.PixelsToUnits();
						Vector3 endPosition = this.ObjectBoxSprite.GetCenter();
						Vector3 startPosition = endPosition + new Vector3(0f, -120f * p2u, 0f);
						Vector3 startPosition2 = endPosition;
						Vector3 endPosition2 = endPosition + new Vector3(0f, 12f * p2u, 0f);
						endPosition -= new Vector3(0f, 21f * p2u, 0f);
						float t = elapsed / duration;
						float quickT = elapsed / 1f;
						float smoothT = Mathf.SmoothStep(0f, 1f, quickT);
						if (synergyHasName)
						{
							float num = Mathf.SmoothStep(0f, 1f, elapsed / 0.5f);
							float num2 = Mathf.SmoothStep(0f, 1f, (elapsed - 0.5f) / 0.5f);
							this.NameLabel.Opacity = 1f - num;
							this.DescriptionLabel.Opacity = 1f - num;
							this.CenterLabel.Opacity = num2;
						}
						Vector3 t2 = Vector3.Lerp(startPosition, endPosition, smoothT).Quantize(p2u * 3f).WithX(startPosition.x);
						Vector3 t3 = Vector3.Lerp(startPosition2, endPosition2, smoothT).Quantize(p2u * 3f).WithX(startPosition2.x);
						t3.y = Mathf.Max(startPosition2.y, t3.y);
						this.notificationSynergySprite.PlaceAtPositionByAnchor(t2, tk2dBaseSprite.Anchor.MiddleCenter);
						this.notificationSynergySprite.transform.position = this.notificationSynergySprite.transform.position + new Vector3(0f, 0f, -0.125f);
						this.notificationObjectSprite.PlaceAtPositionByAnchor(t3, tk2dBaseSprite.Anchor.MiddleCenter);
						this.notificationObjectSprite.transform.localPosition = this.notificationObjectSprite.transform.localPosition.WithX(baseSpriteLocalX);
						this.notificationSynergySprite.transform.localPosition = this.notificationSynergySprite.transform.localPosition.WithX(synSpriteLocalX);
						this.notificationSynergySprite.UpdateZDepth();
						this.notificationObjectSprite.UpdateZDepth();
						elapsed += BraveTime.DeltaTime;
						yield return null;
					}
				}
			}
		}
		GameUIRoot.Instance.MoveNonCoreGroupOnscreen(this.m_panel, true);
		elapsed = 0f;
		duration = 0.25f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		this.CenterLabel.Opacity = 1f;
		this.NameLabel.Opacity = 1f;
		this.DescriptionLabel.Opacity = 1f;
		this.CenterLabel.IsVisible = false;
		this.NameLabel.IsVisible = true;
		this.DescriptionLabel.IsVisible = true;
		this.DisableRenderers();
		this.m_doingNotification = false;
		yield break;
	}

	// Token: 0x04009954 RID: 39252
	public tk2dBaseSprite notificationObjectSprite;

	// Token: 0x04009955 RID: 39253
	public tk2dBaseSprite notificationSynergySprite;

	// Token: 0x04009956 RID: 39254
	public dfSprite ObjectBoxSprite;

	// Token: 0x04009957 RID: 39255
	public dfSprite CrosshairSprite;

	// Token: 0x04009958 RID: 39256
	public dfSprite StickerSprite;

	// Token: 0x04009959 RID: 39257
	public dfSprite BoxSprite;

	// Token: 0x0400995A RID: 39258
	public dfLabel NameLabel;

	// Token: 0x0400995B RID: 39259
	public dfLabel DescriptionLabel;

	// Token: 0x0400995C RID: 39260
	public dfLabel CenterLabel;

	// Token: 0x0400995D RID: 39261
	public dfAnimationClip SilverAnimClip;

	// Token: 0x0400995E RID: 39262
	public dfAnimationClip GoldAnimClip;

	// Token: 0x0400995F RID: 39263
	public dfAnimationClip PurpleAnimClip;

	// Token: 0x04009960 RID: 39264
	[Header("Synergues")]
	public dfAnimationClip SynergyTransformClip;

	// Token: 0x04009961 RID: 39265
	public dfAnimationClip SynergyBoxTransformClip;

	// Token: 0x04009962 RID: 39266
	public dfAnimationClip SynergyCrosshairTransformClip;

	// Token: 0x04009963 RID: 39267
	private tk2dSprite[] outlineSprites;

	// Token: 0x04009964 RID: 39268
	private tk2dSprite[] synergyOutlineSprites;

	// Token: 0x04009965 RID: 39269
	private dfPanel m_panel;

	// Token: 0x04009966 RID: 39270
	private List<IEnumerator> m_queuedNotifications = new List<IEnumerator>();

	// Token: 0x04009967 RID: 39271
	private List<NotificationParams> m_queuedNotificationParams = new List<NotificationParams>();

	// Token: 0x04009968 RID: 39272
	private IEnumerator m_currentNotificationProcess;

	// Token: 0x04009969 RID: 39273
	private bool m_doingNotification;

	// Token: 0x0400996A RID: 39274
	private dfFontBase EnglishFont;

	// Token: 0x0400996B RID: 39275
	private dfFontBase OtherLanguageFont;

	// Token: 0x0400996C RID: 39276
	private Vector3 NameBasePos;

	// Token: 0x0400996D RID: 39277
	private Vector3 DescBasePos;

	// Token: 0x0400996E RID: 39278
	private StringTableManager.GungeonSupportedLanguages m_cachedLanguage;

	// Token: 0x0400996F RID: 39279
	private bool m_isCurrentlyExpanded;

	// Token: 0x04009970 RID: 39280
	private bool m_textsLowered;

	// Token: 0x02001810 RID: 6160
	public enum NotificationColor
	{
		// Token: 0x04009972 RID: 39282
		SILVER,
		// Token: 0x04009973 RID: 39283
		GOLD,
		// Token: 0x04009974 RID: 39284
		PURPLE
	}
}
