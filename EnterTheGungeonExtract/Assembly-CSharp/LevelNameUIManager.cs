using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x020017A4 RID: 6052
public class LevelNameUIManager : MonoBehaviour
{
	// Token: 0x06008DA6 RID: 36262 RVA: 0x003B98B4 File Offset: 0x003B7AB4
	public void ShowLevelName(Dungeon d)
	{
		base.StartCoroutine(this.ShowLevelName_CR(d));
	}

	// Token: 0x06008DA7 RID: 36263 RVA: 0x003B98C4 File Offset: 0x003B7AC4
	public IEnumerator ShowLevelName_CR(Dungeon d)
	{
		while (GameManager.Instance.IsLoadingLevel)
		{
			yield return null;
		}
		this.m_displaying = true;
		yield return null;
		while (GameManager.Instance.IsSelectingCharacter || GameManager.Instance.IsLoadingLevel)
		{
			yield return null;
		}
		yield return new WaitForSeconds(0.25f);
		if (!this.m_displaying)
		{
			yield break;
		}
		this.m_panel = base.GetComponent<dfPanel>();
		if (d.DungeonFloorLevelTextOverride == "no override")
		{
			int num = -1;
			GameLevelDefinition lastLoadedLevelDefinition = GameManager.Instance.GetLastLoadedLevelDefinition();
			if (lastLoadedLevelDefinition != null)
			{
				num = GameManager.Instance.dungeonFloors.IndexOf(lastLoadedLevelDefinition);
			}
			if (lastLoadedLevelDefinition != null && lastLoadedLevelDefinition.lastSelectedFlowEntry != null && lastLoadedLevelDefinition.lastSelectedFlowEntry.overridesLevelDetailText)
			{
				this.levelNumberLabel.Text = this.levelNumberLabel.ForceGetLocalizedValue(lastLoadedLevelDefinition.lastSelectedFlowEntry.overrideLevelDetailText);
			}
			else
			{
				this.levelNumberLabel.Text = this.levelNumberLabel.ForceGetLocalizedValue("#LEVEL") + ((num <= 0) ? "???" : num.ToString());
			}
			this.levelNumberShadow.Width = this.levelNumberLabel.GetAutosizeWidth() / 3f;
		}
		else if (string.IsNullOrEmpty(d.DungeonFloorLevelTextOverride))
		{
			this.levelNumberLabel.Text = string.Empty;
			this.levelNumberShadow.Width = 0f;
			this.levelNumberShadow.IsVisible = false;
		}
		else
		{
			this.levelNumberLabel.Text = this.levelNumberLabel.ForceGetLocalizedValue(d.DungeonFloorLevelTextOverride);
			this.levelNumberShadow.Width = this.levelNumberLabel.GetAutosizeWidth() / 3f;
		}
		this.levelNameLabel.Text = this.levelNameLabel.ForceGetLocalizedValue(d.DungeonFloorName);
		this.levelNameShadow.Width = this.levelNameLabel.GetAutosizeWidth() / 3f;
		this.hyphen.Width = (Mathf.Max(this.levelNameLabel.GetAutosizeWidth(), this.levelNumberLabel.GetAutosizeWidth()) + 88f) / 3f;
		this.levelNumberShadow.Width = Mathf.Max(40f, this.levelNumberShadow.Width + 6f);
		this.levelNameShadow.Width = Mathf.Max(40f, this.levelNameShadow.Width + 6f);
		if (string.IsNullOrEmpty(this.levelNumberLabel.Text))
		{
			this.levelNumberLabel.IsVisible = false;
			this.levelNumberShadow.IsVisible = false;
		}
		base.StartCoroutine(this.HandleLevelName());
		yield break;
	}

	// Token: 0x06008DA8 RID: 36264 RVA: 0x003B98E8 File Offset: 0x003B7AE8
	public void ShowCustomLevelName(string name, string levelNumText = "Level 0")
	{
		this.m_panel = base.GetComponent<dfPanel>();
		this.levelNameLabel.Text = name;
		this.levelNameShadow.Width = this.levelNameLabel.GetAutosizeWidth() / 3f;
		this.levelNumberLabel.Text = levelNumText;
		this.levelNumberShadow.Width = this.levelNumberLabel.GetAutosizeWidth() / 3f;
		this.hyphen.Width = (Mathf.Max(this.levelNameLabel.GetAutosizeWidth(), this.levelNumberLabel.GetAutosizeWidth()) + 88f) / 3f;
		this.m_displaying = true;
		base.StartCoroutine(this.HandleLevelName());
	}

	// Token: 0x06008DA9 RID: 36265 RVA: 0x003B9998 File Offset: 0x003B7B98
	public void BanishLevelNameText()
	{
		if (this.m_displaying)
		{
			this.m_displaying = false;
		}
	}

	// Token: 0x06008DAA RID: 36266 RVA: 0x003B99AC File Offset: 0x003B7BAC
	private IEnumerator HandleLevelName()
	{
		yield return new WaitForSeconds(this.initialDelay);
		this.m_panel.Opacity = 0f;
		this.m_panel.IsVisible = true;
		float elapsed = 0f;
		while (elapsed < this.fadeInTime && this.m_displaying)
		{
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.Clamp01(elapsed / this.fadeInTime);
			this.m_panel.Opacity = t;
			yield return null;
		}
		elapsed = 0f;
		while (elapsed < this.displayTime && this.m_displaying)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		elapsed = 0f;
		while (elapsed < this.fadeOutTime && this.m_displaying)
		{
			elapsed += BraveTime.DeltaTime;
			float t2 = Mathf.Clamp01(elapsed / this.fadeOutTime);
			this.m_panel.Opacity = 1f - t2;
			yield return null;
		}
		this.m_panel.IsVisible = false;
		yield break;
	}

	// Token: 0x0400955D RID: 38237
	public dfLabel levelNameLabel;

	// Token: 0x0400955E RID: 38238
	public dfLabel levelNumberLabel;

	// Token: 0x0400955F RID: 38239
	public dfSlicedSprite levelNameShadow;

	// Token: 0x04009560 RID: 38240
	public dfSlicedSprite levelNumberShadow;

	// Token: 0x04009561 RID: 38241
	public dfSlicedSprite hyphen;

	// Token: 0x04009562 RID: 38242
	public float initialDelay = 0.5f;

	// Token: 0x04009563 RID: 38243
	public float fadeInTime = 1f;

	// Token: 0x04009564 RID: 38244
	public float displayTime = 3f;

	// Token: 0x04009565 RID: 38245
	public float fadeOutTime = 1f;

	// Token: 0x04009566 RID: 38246
	private const float c_widthBoost = 6f;

	// Token: 0x04009567 RID: 38247
	private const float c_minWidth = 40f;

	// Token: 0x04009568 RID: 38248
	private dfPanel m_panel;

	// Token: 0x04009569 RID: 38249
	private bool m_displaying;
}
