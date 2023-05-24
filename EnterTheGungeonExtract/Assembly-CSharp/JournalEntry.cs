using System;
using Dungeonator;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02001500 RID: 5376
[Serializable]
public class JournalEntry
{
	// Token: 0x17001211 RID: 4625
	// (get) Token: 0x06007AA8 RID: 31400 RVA: 0x00312CB8 File Offset: 0x00310EB8
	// (set) Token: 0x06007AA9 RID: 31401 RVA: 0x00312CC0 File Offset: 0x00310EC0
	public static int ReloadDataSemaphore { get; set; }

	// Token: 0x06007AAA RID: 31402 RVA: 0x00312CC8 File Offset: 0x00310EC8
	private void CheckSemaphore()
	{
		if (this.PrivateSemaphoreValue < JournalEntry.ReloadDataSemaphore)
		{
			this.m_cachedPrimaryDisplayName = null;
			this.m_cachedNotificationPanelDescription = null;
			this.m_cachedAmmonomiconFullEntry = null;
			this.PrivateSemaphoreValue = JournalEntry.ReloadDataSemaphore;
		}
	}

	// Token: 0x06007AAB RID: 31403 RVA: 0x00312CFC File Offset: 0x00310EFC
	public string GetAmmonomiconFullEntry(bool isInfiniteAmmoGun, bool doesntDamageSecretWalls)
	{
		this.CheckSemaphore();
		if (string.IsNullOrEmpty(this.m_cachedAmmonomiconFullEntry))
		{
			if (this.SpecialIdentifier == JournalEntry.CustomJournalEntryType.RESOURCEFUL_RAT)
			{
				string text = "#RESOURCEFULRAT_AGD_LONGDESC_PREKILL";
				if (Application.isPlaying && GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_RESOURCEFULRAT))
				{
					text = "#RESOURCEFULRAT_AGD_LONGDESC_POSTKILL";
				}
				string text2 = this.HandleLongDescSuffix();
				return StringTableManager.GetEnemiesLongDescription(text) + text2;
			}
			if (string.IsNullOrEmpty(this.AmmonomiconFullEntry))
			{
				return string.Empty;
			}
			if (this.IsEnemy)
			{
				this.m_cachedAmmonomiconFullEntry = StringTableManager.GetEnemiesLongDescription(this.AmmonomiconFullEntry);
			}
			else
			{
				string text3 = this.AmmonomiconFullEntry;
				if (this.AmmonomiconFullEntry == "#PIGITEM1_LONGDESC" && Application.isPlaying && GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_HERO_PIG))
				{
					text3 = "#PIGITEM2_LONGDESC";
				}
				if (this.AmmonomiconFullEntry == "#BRACELETRED_LONGDESC" && Application.isPlaying && GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_RECEIVED_RUBY_BRACELET))
				{
					text3 = "#BRACELETRED_LONGDESC_V2";
				}
				string text4 = string.Empty;
				if (isInfiniteAmmoGun)
				{
					text4 = text4 + StringTableManager.GetItemsString("#INFINITEAMMO_TEXT", -1) + " ";
				}
				if (doesntDamageSecretWalls)
				{
					text4 = text4 + StringTableManager.GetItemsString("#NOSECRETS_TEXT", -1) + " ";
				}
				string text5 = this.HandleLongDescSuffix();
				this.m_cachedAmmonomiconFullEntry = text4 + StringTableManager.GetItemsLongDescription(text3) + text5;
			}
		}
		return this.m_cachedAmmonomiconFullEntry;
	}

	// Token: 0x06007AAC RID: 31404 RVA: 0x00312E7C File Offset: 0x0031107C
	private string HandleLongDescSuffix()
	{
		string text = string.Empty;
		if (this.SpecialIdentifier != JournalEntry.CustomJournalEntryType.NONE && Application.isPlaying)
		{
			DungeonData.Direction[] resourcefulRatSolution = GameManager.GetResourcefulRatSolution();
			if (this.SpecialIdentifier == JournalEntry.CustomJournalEntryType.RAT_NOTE_01 && Application.isPlaying)
			{
				text = this.GetRatSpriteFromDirection(resourcefulRatSolution[0]);
			}
			else if (this.SpecialIdentifier == JournalEntry.CustomJournalEntryType.RAT_NOTE_02 && Application.isPlaying)
			{
				text = this.GetRatSpriteFromDirection(resourcefulRatSolution[1]);
			}
			else if (this.SpecialIdentifier == JournalEntry.CustomJournalEntryType.RAT_NOTE_03 && Application.isPlaying)
			{
				text = this.GetRatSpriteFromDirection(resourcefulRatSolution[2]);
			}
			else if (this.SpecialIdentifier == JournalEntry.CustomJournalEntryType.RAT_NOTE_04 && Application.isPlaying)
			{
				text = this.GetRatSpriteFromDirection(resourcefulRatSolution[3]);
			}
			else if (this.SpecialIdentifier == JournalEntry.CustomJournalEntryType.RAT_NOTE_05 && Application.isPlaying)
			{
				text = this.GetRatSpriteFromDirection(resourcefulRatSolution[4]);
			}
			else if (this.SpecialIdentifier == JournalEntry.CustomJournalEntryType.RAT_NOTE_06 && Application.isPlaying)
			{
				text = this.GetRatSpriteFromDirection(resourcefulRatSolution[5]);
			}
		}
		return text;
	}

	// Token: 0x06007AAD RID: 31405 RVA: 0x00312F8C File Offset: 0x0031118C
	private string GetRatSpriteFromDirection(DungeonData.Direction dir)
	{
		switch (dir)
		{
		case DungeonData.Direction.NORTH:
			return "[sprite \"resourcefulrat_text_note_001\"]";
		case DungeonData.Direction.EAST:
			return "[sprite \"resourcefulrat_text_note_002\"]";
		case DungeonData.Direction.SOUTH:
			return "[sprite \"resourcefulrat_text_note_003\"]";
		case DungeonData.Direction.WEST:
			return "[sprite \"resourcefulrat_text_note_004\"]";
		}
		return string.Empty;
	}

	// Token: 0x06007AAE RID: 31406 RVA: 0x00312FE0 File Offset: 0x003111E0
	public string GetPrimaryDisplayName(bool duringStartup = false)
	{
		this.CheckSemaphore();
		if (string.IsNullOrEmpty(this.m_cachedPrimaryDisplayName))
		{
			if (this.IsEnemy)
			{
				this.m_cachedPrimaryDisplayName = StringTableManager.GetEnemiesString(this.PrimaryDisplayName, 0);
			}
			else
			{
				string text = this.PrimaryDisplayName;
				if (Application.isPlaying && !duringStartup && this.PrimaryDisplayName == "#PIGITEM1_ENCNAME" && GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_HERO_PIG))
				{
					text = "#PIGITEM2_ENCNAME";
				}
				this.m_cachedPrimaryDisplayName = StringTableManager.GetItemsString(text, 0);
			}
		}
		return this.m_cachedPrimaryDisplayName;
	}

	// Token: 0x06007AAF RID: 31407 RVA: 0x00313080 File Offset: 0x00311280
	public string GetNotificationPanelDescription()
	{
		this.CheckSemaphore();
		if (string.IsNullOrEmpty(this.m_cachedNotificationPanelDescription))
		{
			if (this.IsEnemy)
			{
				this.m_cachedNotificationPanelDescription = StringTableManager.GetEnemiesString(this.NotificationPanelDescription, 0);
			}
			else
			{
				string text = this.NotificationPanelDescription;
				if (this.NotificationPanelDescription == "#PIGITEM1_SHORTDESC" && Application.isPlaying && GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_HERO_PIG))
				{
					text = "#PIGITEM2_SHORTDESC";
				}
				if (this.NotificationPanelDescription == "#BRACELETRED_SHORTDESC" && Application.isPlaying && GameStatsManager.Instance.GetFlag(GungeonFlags.BLACKSMITH_RECEIVED_RUBY_BRACELET))
				{
					text = "#BRACELETRED_SHORTDESC_V2";
				}
				if (this.NotificationPanelDescription == "#JUNK_SHORTDESC" && SackKnightController.HasJunkan())
				{
					text = "#JUNKSHRINE_SHORTDESC";
				}
				this.m_cachedNotificationPanelDescription = StringTableManager.GetItemsString(text, 0);
			}
		}
		return this.m_cachedNotificationPanelDescription;
	}

	// Token: 0x06007AB0 RID: 31408 RVA: 0x00313178 File Offset: 0x00311378
	public string GetCustomNotificationPanelDescription(int index)
	{
		this.CheckSemaphore();
		if (this.IsEnemy)
		{
			return StringTableManager.GetEnemiesString(this.NotificationPanelDescription, index);
		}
		return StringTableManager.GetItemsString(this.NotificationPanelDescription, index);
	}

	// Token: 0x06007AB1 RID: 31409 RVA: 0x003131A4 File Offset: 0x003113A4
	protected bool Equals(JournalEntry other)
	{
		return this.SuppressInAmmonomicon.Equals(other.SuppressInAmmonomicon) && this.DisplayOnLoadingScreen.Equals(other.DisplayOnLoadingScreen) && this.RequiresLightBackgroundInLoadingScreen.Equals(other.RequiresLightBackgroundInLoadingScreen) && string.Equals(this.PrimaryDisplayName, other.PrimaryDisplayName) && string.Equals(this.NotificationPanelDescription, other.NotificationPanelDescription) && string.Equals(this.AmmonomiconFullEntry, other.AmmonomiconFullEntry) && string.Equals(this.AmmonomiconSprite, other.AmmonomiconSprite) && this.IsEnemy.Equals(other.IsEnemy) && this.SuppressKnownState.Equals(other.SuppressKnownState) && object.Equals(this.enemyPortraitSprite, other.enemyPortraitSprite);
	}

	// Token: 0x06007AB2 RID: 31410 RVA: 0x0031328C File Offset: 0x0031148C
	public override bool Equals(object obj)
	{
		return !object.ReferenceEquals(null, obj) && (object.ReferenceEquals(this, obj) || (obj.GetType() == base.GetType() && this.Equals((JournalEntry)obj)));
	}

	// Token: 0x06007AB3 RID: 31411 RVA: 0x003132CC File Offset: 0x003114CC
	public override int GetHashCode()
	{
		int num = this.SuppressInAmmonomicon.GetHashCode();
		num = (num * 397) ^ this.DisplayOnLoadingScreen.GetHashCode();
		num = (num * 397) ^ this.RequiresLightBackgroundInLoadingScreen.GetHashCode();
		num = (num * 397) ^ ((this.PrimaryDisplayName == null) ? 0 : this.PrimaryDisplayName.GetHashCode());
		num = (num * 397) ^ ((this.NotificationPanelDescription == null) ? 0 : this.NotificationPanelDescription.GetHashCode());
		num = (num * 397) ^ ((this.AmmonomiconFullEntry == null) ? 0 : this.AmmonomiconFullEntry.GetHashCode());
		num = (num * 397) ^ ((this.AmmonomiconSprite == null) ? 0 : this.AmmonomiconSprite.GetHashCode());
		num = (num * 397) ^ this.IsEnemy.GetHashCode();
		return (num * 397) ^ ((!(this.enemyPortraitSprite != null)) ? 0 : this.enemyPortraitSprite.GetHashCode());
	}

	// Token: 0x06007AB4 RID: 31412 RVA: 0x003133FC File Offset: 0x003115FC
	public JournalEntry Clone()
	{
		return new JournalEntry
		{
			SuppressInAmmonomicon = this.SuppressInAmmonomicon,
			DisplayOnLoadingScreen = this.DisplayOnLoadingScreen,
			RequiresLightBackgroundInLoadingScreen = this.RequiresLightBackgroundInLoadingScreen,
			PrimaryDisplayName = this.PrimaryDisplayName,
			NotificationPanelDescription = this.NotificationPanelDescription,
			AmmonomiconFullEntry = this.AmmonomiconFullEntry,
			AmmonomiconSprite = this.AmmonomiconSprite,
			IsEnemy = this.IsEnemy,
			enemyPortraitSprite = this.enemyPortraitSprite,
			SuppressKnownState = this.SuppressKnownState,
			SpecialIdentifier = this.SpecialIdentifier
		};
	}

	// Token: 0x06007AB5 RID: 31413 RVA: 0x00313494 File Offset: 0x00311694
	public void ClearCache()
	{
		this.m_cachedPrimaryDisplayName = null;
		this.m_cachedNotificationPanelDescription = null;
		this.m_cachedAmmonomiconFullEntry = null;
	}

	// Token: 0x04007D34 RID: 32052
	public bool SuppressInAmmonomicon;

	// Token: 0x04007D35 RID: 32053
	public bool SuppressKnownState;

	// Token: 0x04007D36 RID: 32054
	public bool DisplayOnLoadingScreen;

	// Token: 0x04007D37 RID: 32055
	public bool RequiresLightBackgroundInLoadingScreen;

	// Token: 0x04007D38 RID: 32056
	[StringTableString("items")]
	public string PrimaryDisplayName;

	// Token: 0x04007D39 RID: 32057
	[StringTableString("items")]
	public string NotificationPanelDescription;

	// Token: 0x04007D3A RID: 32058
	[StringTableString("items")]
	public string AmmonomiconFullEntry;

	// Token: 0x04007D3B RID: 32059
	[FormerlySerializedAs("AlternateAmmonomiconButtonSpriteName")]
	public string AmmonomiconSprite = string.Empty;

	// Token: 0x04007D3C RID: 32060
	public bool IsEnemy;

	// Token: 0x04007D3D RID: 32061
	public Texture2D enemyPortraitSprite;

	// Token: 0x04007D3F RID: 32063
	public JournalEntry.CustomJournalEntryType SpecialIdentifier;

	// Token: 0x04007D40 RID: 32064
	[NonSerialized]
	private string m_cachedPrimaryDisplayName;

	// Token: 0x04007D41 RID: 32065
	[NonSerialized]
	private string m_cachedNotificationPanelDescription;

	// Token: 0x04007D42 RID: 32066
	[NonSerialized]
	private string m_cachedAmmonomiconFullEntry;

	// Token: 0x04007D43 RID: 32067
	[NonSerialized]
	private int PrivateSemaphoreValue;

	// Token: 0x02001501 RID: 5377
	public enum CustomJournalEntryType
	{
		// Token: 0x04007D45 RID: 32069
		NONE,
		// Token: 0x04007D46 RID: 32070
		RAT_NOTE_01 = 101,
		// Token: 0x04007D47 RID: 32071
		RAT_NOTE_02,
		// Token: 0x04007D48 RID: 32072
		RAT_NOTE_03,
		// Token: 0x04007D49 RID: 32073
		RAT_NOTE_04,
		// Token: 0x04007D4A RID: 32074
		RAT_NOTE_05,
		// Token: 0x04007D4B RID: 32075
		RAT_NOTE_06,
		// Token: 0x04007D4C RID: 32076
		RESOURCEFUL_RAT
	}
}
