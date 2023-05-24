using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001781 RID: 6017
public class GameUIBossHealthController : MonoBehaviour
{
	// Token: 0x06008C44 RID: 35908 RVA: 0x003A9F14 File Offset: 0x003A8114
	private void Awake()
	{
		while (this.m_cachedPercentHealths.Count < this.barSprites.Count)
		{
			this.m_cachedPercentHealths.Add(1f);
		}
		while (this.m_barsActive.Count < this.barSprites.Count)
		{
			this.m_barsActive.Add(false);
		}
	}

	// Token: 0x06008C45 RID: 35909 RVA: 0x003A9F80 File Offset: 0x003A8180
	private void CheckLanguageFonts()
	{
		if (this.EnglishFont == null)
		{
			this.EnglishFont = this.bossNameLabel.Font;
			this.EnglishAtlas = this.bossNameLabel.Atlas;
			this.OtherLanguageFont = this.bossNameLabel.GUIManager.DefaultFont;
			this.OtherLanguageAtlas = this.bossNameLabel.GUIManager.DefaultAtlas;
		}
		if (StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
		{
			if (this.m_cachedLanguage != StringTableManager.GungeonSupportedLanguages.ENGLISH)
			{
				this.bossNameLabel.Atlas = this.EnglishAtlas;
				this.bossNameLabel.Font = this.EnglishFont;
			}
		}
		else if (StringTableManager.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.JAPANESE)
		{
			if (StringTableManager.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.CHINESE)
			{
				if (StringTableManager.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.KOREAN)
				{
					if (StringTableManager.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.RUSSIAN)
					{
						if (this.m_cachedLanguage != StringTableManager.CurrentLanguage)
						{
							this.bossNameLabel.Atlas = this.OtherLanguageAtlas;
							this.bossNameLabel.Font = this.OtherLanguageFont;
						}
					}
				}
			}
		}
		this.m_cachedLanguage = StringTableManager.CurrentLanguage;
	}

	// Token: 0x06008C46 RID: 35910 RVA: 0x003AA0A8 File Offset: 0x003A82A8
	public void LateUpdate()
	{
		if (this.m_defaultBossNameRelativePosition == null)
		{
			this.m_defaultBossNameRelativePosition = new Vector3?(this.bossNameLabel.RelativePosition);
		}
		this.m_vfxTimer -= BraveTime.DeltaTime;
		if (this.m_healthHavers.Count > 0)
		{
			for (int i = 0; i < this.m_healthHavers.Count; i++)
			{
				if (this.m_healthHavers[i])
				{
					this.UpdateBarSizes(i);
					this.UpdateBossHealth(this.barSprites[i], this.m_healthHavers[i].GetCurrentHealth() / this.m_healthHavers[i].GetMaxHealth() / (float)this.barSprites.Count);
				}
			}
		}
		else if (this.m_barsActive[0] && this.m_cachedPercentHealths[0] > 0f)
		{
			this.UpdateBossHealth(this.barSprites[0], 0f);
		}
		if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.KOREAN || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE)
		{
			this.bossNameLabel.RelativePosition = this.m_defaultBossNameRelativePosition.Value + new Vector3(0f, -12f, 0f);
		}
		else if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.JAPANESE || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.RUSSIAN)
		{
			this.bossNameLabel.RelativePosition = this.m_defaultBossNameRelativePosition.Value;
		}
		else
		{
			this.bossNameLabel.RelativePosition = this.m_defaultBossNameRelativePosition.Value + new Vector3(0f, -12f, 0f);
		}
		bool flag = false;
		float num = BraveCameraUtility.ASPECT / 1.7777778f;
		PlayerController[] allPlayers = GameManager.Instance.AllPlayers;
		for (int j = 0; j < allPlayers.Length; j++)
		{
			Vector2 unitCenter = allPlayers[j].specRigidbody.GetUnitCenter(ColliderType.HitBox);
			Vector2 vector = BraveUtility.WorldPointToViewport(unitCenter, ViewportType.Camera);
			float num2 = (vector.x - 0.5f) * num;
			float num3 = (1f - vector.x) * num;
			if (GameManager.Options.SmallUIEnabled)
			{
				if (this.IsVertical && num3 < 0.04f && num3 > -0.05f && vector.y > 0.32f && vector.y < 0.7f)
				{
					flag = true;
					break;
				}
				if (!this.IsVertical && num2 >= -0.115f && num2 <= 0.115f && vector.y > -0.05f && vector.y < 0.06f)
				{
					flag = true;
					break;
				}
			}
			else
			{
				if (this.IsVertical && num3 < 0.075f && num3 > -0.05f && vector.y > 0.14f && vector.y < 0.9f)
				{
					flag = true;
					break;
				}
				if (!this.IsVertical && num2 >= -0.23f && num2 <= 0.23f && vector.y > -0.05f && vector.y < 0.11f)
				{
					flag = true;
					break;
				}
			}
		}
		this.Opacity = Mathf.MoveTowards(this.Opacity, (!flag) ? 1f : this.MinOpacity, this.OpacityChangeSpeed * BraveTime.DeltaTime);
		for (int k = 0; k < this.barSprites.Count; k++)
		{
			this.barSprites[k].Opacity = this.Opacity;
		}
		this.tankSprite.Opacity = this.Opacity;
	}

	// Token: 0x06008C47 RID: 35911 RVA: 0x003AA4C4 File Offset: 0x003A86C4
	private void UpdateBarSizes(int barIndex)
	{
		this.barSprites[barIndex].RelativePosition = this.barSprites[barIndex].RelativePosition.WithX(this.barSprites[0].RelativePosition.x + this.barSprites[0].Size.x / (float)this.m_activeBarSprites * (float)barIndex);
	}

	// Token: 0x06008C48 RID: 35912 RVA: 0x003AA538 File Offset: 0x003A8738
	public void SetBossName(string bossName)
	{
		this.CheckLanguageFonts();
		if (GameManager.Instance.Dungeon)
		{
			this.bossNameLabel.Glitchy = GameManager.Instance.Dungeon.IsGlitchDungeon;
		}
		this.bossNameLabel.Text = bossName;
	}

	// Token: 0x06008C49 RID: 35913 RVA: 0x003AA588 File Offset: 0x003A8788
	public void RegisterBossHealthHaver(HealthHaver healthHaver, string bossName = null)
	{
		if (bossName != null)
		{
			this.SetBossName(bossName);
		}
		if (!this.m_healthHavers.Contains(healthHaver))
		{
			this.m_healthHavers.Add(healthHaver);
			if (this.m_healthHavers.Count > this.barSprites.Count)
			{
				dfSlicedSprite component = this.barSprites[0].Parent.AddPrefab(this.barSprites[0].gameObject).GetComponent<dfSlicedSprite>();
				component.RelativePosition = this.barSprites[0].RelativePosition;
				this.barSprites.Add(component);
				this.m_cachedPercentHealths.Add(1f);
				this.m_barsActive.Add(false);
			}
			else
			{
				int num = this.m_healthHavers.Count - 1;
				this.m_cachedPercentHealths[num] = 1f;
				this.m_barsActive[num] = false;
			}
		}
	}

	// Token: 0x06008C4A RID: 35914 RVA: 0x003AA678 File Offset: 0x003A8878
	public void DeregisterBossHealthHaver(HealthHaver healthHaver)
	{
		int num = this.m_healthHavers.IndexOf(healthHaver);
		if (num >= 0)
		{
			this.UpdateBossHealth(this.barSprites[num], 0f);
			this.m_healthHavers[num] = null;
		}
		for (int i = 0; i < this.m_healthHavers.Count; i++)
		{
			if (this.m_healthHavers[i] && this.m_healthHavers[i].IsAlive)
			{
				return;
			}
		}
		this.ClearExtraBarData();
		this.m_healthHavers.Clear();
	}

	// Token: 0x06008C4B RID: 35915 RVA: 0x003AA718 File Offset: 0x003A8918
	private void ClearExtraBarData()
	{
		for (int i = 1; i < this.barSprites.Count; i++)
		{
			UnityEngine.Object.Destroy(this.barSprites[i].gameObject);
			this.barSprites.RemoveAt(i);
			if (i < this.m_cachedPercentHealths.Count)
			{
				this.m_cachedPercentHealths.RemoveAt(i);
			}
			if (i < this.m_barsActive.Count)
			{
				this.m_barsActive.RemoveAt(i);
			}
			i--;
		}
	}

	// Token: 0x06008C4C RID: 35916 RVA: 0x003AA7A4 File Offset: 0x003A89A4
	public void ForceUpdateBossHealth(float currentBossHealth, float maxBossHealth, string bossName = null)
	{
		if (bossName != null)
		{
			this.SetBossName(bossName);
		}
		this.UpdateBossHealth(this.barSprites[0], currentBossHealth / maxBossHealth);
	}

	// Token: 0x06008C4D RID: 35917 RVA: 0x003AA7C8 File Offset: 0x003A89C8
	public void DisableBossHealth()
	{
		for (int i = this.barSprites.Count - 1; i >= 0; i--)
		{
			this.m_barsActive[i] = false;
			this.m_cachedPercentHealths[i] = 0f;
		}
		this.ClearExtraBarData();
		this.m_activeBarSprites = 0;
		this.m_healthHavers.Clear();
		this.tankSprite.IsVisible = false;
	}

	// Token: 0x06008C4E RID: 35918 RVA: 0x003AA838 File Offset: 0x003A8A38
	public IEnumerator TriggerBossPortraitCR(PortraitSlideSettings pss)
	{
		GameObject instantiatedBossCardPrefab = UnityEngine.Object.Instantiate<GameObject>(this.bossCardUIPrefab.gameObject, new Vector3(-100f, -100f, 0f), Quaternion.identity);
		BossCardUIController bosscard = instantiatedBossCardPrefab.GetComponent<BossCardUIController>();
		bosscard.InitializeTexts(pss);
		this.m_extantBosscard = bosscard;
		yield return base.StartCoroutine(bosscard.CoreSequence(pss));
		yield break;
	}

	// Token: 0x06008C4F RID: 35919 RVA: 0x003AA85C File Offset: 0x003A8A5C
	public void EndBossPortraitEarly()
	{
		if (this.m_extantBosscard)
		{
			this.m_extantBosscard.BreakSequence();
			UnityEngine.Object.Destroy(this.m_extantBosscard.gameObject);
		}
	}

	// Token: 0x06008C50 RID: 35920 RVA: 0x003AA88C File Offset: 0x003A8A8C
	private void UpdateBossHealth(dfSlicedSprite barSprite, float percent)
	{
		int num = this.barSprites.IndexOf(barSprite);
		if (percent <= 0f)
		{
			if (!this.m_barsActive[num])
			{
				Debug.LogError("uh... activating a boss health bar at 0 health. this seems dumb");
				return;
			}
			this.m_targetPercent = 0f;
			if (!this.m_isAnimating)
			{
				barSprite.FillAmount = percent;
			}
		}
		else if (!this.m_barsActive[num])
		{
			this.TriggerBossHealth(barSprite, percent);
		}
		else
		{
			if (percent > this.m_cachedPercentHealths[this.barSprites.IndexOf(barSprite)])
			{
				base.StartCoroutine(this.FillBossBar(barSprite));
			}
			this.m_targetPercent = percent;
			if (!this.m_isAnimating)
			{
				barSprite.FillAmount = percent;
			}
		}
		if (this.m_cachedPercentHealths[this.barSprites.IndexOf(barSprite)] > percent && this.damagedVFX != null && this.m_vfxTimer <= 0f)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.damagedVFX);
			dfSprite component = gameObject.GetComponent<dfSprite>();
			dfSpriteAnimation component2 = gameObject.GetComponent<dfSpriteAnimation>();
			component.BringToFront();
			component.Size = component.SpriteInfo.sizeInPixels * Pixelator.Instance.CurrentTileScale;
			barSprite.GetManager().AddControl(component);
			Bounds bounds = barSprite.GetBounds();
			if (this.IsVertical)
			{
				float num2 = (bounds.max.y - bounds.min.y) * barSprite.FillAmount + bounds.min.y;
				component.transform.position = new Vector3(bounds.center.x, num2, bounds.center.z);
			}
			else
			{
				float num3 = (bounds.max.x - bounds.min.x) * barSprite.FillAmount + bounds.min.x;
				component.transform.position = new Vector3(num3, bounds.center.y, bounds.center.z);
			}
			component.BringToFront();
			component.Opacity = this.Opacity;
			component2.Play();
			this.m_vfxTimer = 0.25f;
		}
		this.m_cachedPercentHealths[this.barSprites.IndexOf(barSprite)] = percent;
	}

	// Token: 0x06008C51 RID: 35921 RVA: 0x003AAB08 File Offset: 0x003A8D08
	private void TriggerBossHealth(dfSlicedSprite barSprite, float targetPercent)
	{
		int num = this.barSprites.IndexOf(barSprite);
		if (this.m_barsActive[num])
		{
			return;
		}
		this.m_barsActive[num] = true;
		this.m_activeBarSprites++;
		for (int i = 0; i < this.m_healthHavers.Count; i++)
		{
			this.UpdateBarSizes(i);
		}
		barSprite.FillAmount = 0f;
		this.tankSprite.IsVisible = true;
		this.tankSprite.Invalidate();
		barSprite.Invalidate();
		this.m_targetPercent = targetPercent;
		base.StartCoroutine(this.FillBossBar(barSprite));
	}

	// Token: 0x06008C52 RID: 35922 RVA: 0x003AABB0 File Offset: 0x003A8DB0
	private IEnumerator FillBossBar(dfSlicedSprite barSprite)
	{
		float elapsed = 0f;
		this.m_isAnimating = true;
		float startPercent = barSprite.FillAmount;
		while (elapsed < this.fillTime)
		{
			elapsed += BraveTime.DeltaTime;
			if (barSprite)
			{
				barSprite.FillAmount = Mathf.SmoothStep(startPercent, this.m_targetPercent, elapsed / this.fillTime);
			}
			yield return null;
		}
		this.m_isAnimating = false;
		yield break;
	}

	// Token: 0x170014FB RID: 5371
	// (get) Token: 0x06008C53 RID: 35923 RVA: 0x003AABD4 File Offset: 0x003A8DD4
	public bool IsActive
	{
		get
		{
			return this.m_barsActive[0];
		}
	}

	// Token: 0x04009388 RID: 37768
	private const float c_minTimeBetweenHitVfx = 0.25f;

	// Token: 0x04009389 RID: 37769
	public dfSlicedSprite tankSprite;

	// Token: 0x0400938A RID: 37770
	public List<dfSlicedSprite> barSprites;

	// Token: 0x0400938B RID: 37771
	public dfLabel bossNameLabel;

	// Token: 0x0400938C RID: 37772
	public float fillTime = 1.5f;

	// Token: 0x0400938D RID: 37773
	public GameObject damagedVFX;

	// Token: 0x0400938E RID: 37774
	public BossCardUIController bossCardUIPrefab;

	// Token: 0x0400938F RID: 37775
	public bool IsVertical;

	// Token: 0x04009390 RID: 37776
	[NonSerialized]
	public float Opacity = 1f;

	// Token: 0x04009391 RID: 37777
	[NonSerialized]
	public float OpacityChangeSpeed = 2f;

	// Token: 0x04009392 RID: 37778
	[NonSerialized]
	public float MinOpacity = 0.6f;

	// Token: 0x04009393 RID: 37779
	private Vector3? m_defaultBossNameRelativePosition;

	// Token: 0x04009394 RID: 37780
	private dfAtlas EnglishAtlas;

	// Token: 0x04009395 RID: 37781
	private dfFontBase EnglishFont;

	// Token: 0x04009396 RID: 37782
	private dfAtlas OtherLanguageAtlas;

	// Token: 0x04009397 RID: 37783
	private dfFontBase OtherLanguageFont;

	// Token: 0x04009398 RID: 37784
	private StringTableManager.GungeonSupportedLanguages m_cachedLanguage;

	// Token: 0x04009399 RID: 37785
	private List<bool> m_barsActive = new List<bool>();

	// Token: 0x0400939A RID: 37786
	private List<float> m_cachedPercentHealths = new List<float>();

	// Token: 0x0400939B RID: 37787
	private List<HealthHaver> m_healthHavers = new List<HealthHaver>();

	// Token: 0x0400939C RID: 37788
	private int m_activeBarSprites;

	// Token: 0x0400939D RID: 37789
	private bool m_isAnimating;

	// Token: 0x0400939E RID: 37790
	private float m_targetPercent;

	// Token: 0x0400939F RID: 37791
	private float m_vfxTimer;

	// Token: 0x040093A0 RID: 37792
	private BossCardUIController m_extantBosscard;
}
