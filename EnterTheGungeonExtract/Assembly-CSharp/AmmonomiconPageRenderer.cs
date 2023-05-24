using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020014E5 RID: 5349
public class AmmonomiconPageRenderer : MonoBehaviour
{
	// Token: 0x170011EE RID: 4590
	// (get) Token: 0x060079AD RID: 31149 RVA: 0x0030AF38 File Offset: 0x00309138
	// (set) Token: 0x060079AE RID: 31150 RVA: 0x0030AF40 File Offset: 0x00309140
	public IAmmonomiconFocusable LastFocusTarget
	{
		get
		{
			return this.m_lastFocusTarget;
		}
		set
		{
			this.m_lastFocusTarget = value;
		}
	}

	// Token: 0x060079AF RID: 31151 RVA: 0x0030AF4C File Offset: 0x0030914C
	public void Awake()
	{
		if (this.pageType == AmmonomiconPageRenderer.PageType.EQUIPMENT_LEFT)
		{
			dfScrollPanel component = base.transform.parent.Find("Scroll Panel").GetComponent<dfScrollPanel>();
			Transform transform = component.transform.Find("Header");
			dfLabel[] componentsInChildren = transform.GetComponentsInChildren<dfLabel>();
			this.BaseAlagardFont = componentsInChildren[0].Font;
			this.OtherAlagardFont = (BraveResources.Load("Alternate Fonts/AlagardExtended22", ".prefab") as GameObject).GetComponent<dfFont>();
		}
		else if (this.pageType == AmmonomiconPageRenderer.PageType.EQUIPMENT_RIGHT)
		{
			dfScrollPanel component2 = base.transform.parent.Find("Scroll Panel").GetComponent<dfScrollPanel>();
			dfLabel component3 = component2.transform.Find("Scroll Panel").Find("Panel").Find("Label")
				.GetComponent<dfLabel>();
			if (component3)
			{
				this.EnglishFont = component3.Font;
				this.OtherLanguageFont = GameUIRoot.Instance.Manager.DefaultFont;
			}
		}
	}

	// Token: 0x060079B0 RID: 31152 RVA: 0x0030B04C File Offset: 0x0030924C
	public List<AmmonomiconPokedexEntry> GetPokedexEntries()
	{
		return this.m_pokedexEntries;
	}

	// Token: 0x060079B1 RID: 31153 RVA: 0x0030B054 File Offset: 0x00309254
	public AmmonomiconPokedexEntry GetPokedexEntry(EncounterTrackable targetTrackable)
	{
		for (int i = 0; i < this.m_pokedexEntries.Count; i++)
		{
			if (this.m_pokedexEntries[i].linkedEncounterTrackable.myGuid == targetTrackable.EncounterGuid)
			{
				return this.m_pokedexEntries[i];
			}
		}
		return null;
	}

	// Token: 0x060079B2 RID: 31154 RVA: 0x0030B0B4 File Offset: 0x003092B4
	protected void ToggleHeaderImage()
	{
		if (this.pageType == AmmonomiconPageRenderer.PageType.EQUIPMENT_LEFT || this.pageType == AmmonomiconPageRenderer.PageType.GUNS_LEFT || this.pageType == AmmonomiconPageRenderer.PageType.ITEMS_LEFT || this.pageType == AmmonomiconPageRenderer.PageType.ENEMIES_LEFT || this.pageType == AmmonomiconPageRenderer.PageType.BOSSES_LEFT)
		{
			if (GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.ENGLISH && this.HeaderBGSprite != null)
			{
				this.HeaderBGSprite.IsVisible = false;
			}
			else if (this.HeaderBGSprite != null)
			{
				this.HeaderBGSprite.IsVisible = true;
			}
		}
	}

	// Token: 0x060079B3 RID: 31155 RVA: 0x0030B14C File Offset: 0x0030934C
	public void ForceUpdateLanguageFonts()
	{
		AmmonomiconPageRenderer ammonomiconPageRenderer = ((!(AmmonomiconController.Instance.ImpendingRightPageRenderer != null)) ? AmmonomiconController.Instance.CurrentRightPageRenderer : AmmonomiconController.Instance.ImpendingRightPageRenderer);
		if (ammonomiconPageRenderer.pageType != AmmonomiconPageRenderer.PageType.DEATH_RIGHT)
		{
			dfScrollPanel component = ammonomiconPageRenderer.guiManager.transform.Find("Scroll Panel").GetComponent<dfScrollPanel>();
			dfLabel component2 = component.transform.Find("Scroll Panel").Find("Panel").Find("Label")
				.GetComponent<dfLabel>();
			this.CheckLanguageFonts(component2);
			component2.Localize();
		}
		this.ForceUpdateHeaderFonts();
	}

	// Token: 0x060079B4 RID: 31156 RVA: 0x0030B1F0 File Offset: 0x003093F0
	private void ForceUpdateHeaderFonts()
	{
		AmmonomiconPageRenderer ammonomiconPageRenderer = ((!(AmmonomiconController.Instance.ImpendingLeftPageRenderer != null)) ? AmmonomiconController.Instance.CurrentLeftPageRenderer : AmmonomiconController.Instance.ImpendingLeftPageRenderer);
		if (this != ammonomiconPageRenderer)
		{
			return;
		}
		if (this.pageType == AmmonomiconPageRenderer.PageType.EQUIPMENT_LEFT)
		{
			this.ToggleHeaderImage();
		}
		dfScrollPanel component = ammonomiconPageRenderer.guiManager.transform.Find("Scroll Panel").GetComponent<dfScrollPanel>();
		Transform transform = component.transform.Find("Header");
		dfLabel[] componentsInChildren = transform.GetComponentsInChildren<dfLabel>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (this.OriginalHeaderRelativeY == null && componentsInChildren[i].transform.parent == transform)
			{
				this.OriginalHeaderRelativeY = new float?(componentsInChildren[i].RelativePosition.y);
			}
			this.CheckHeaderFonts(componentsInChildren[i], componentsInChildren[i].transform.parent == transform);
		}
	}

	// Token: 0x060079B5 RID: 31157 RVA: 0x0030B2FC File Offset: 0x003094FC
	public void UpdateOnBecameActive()
	{
		this.ForceUpdateLanguageFonts();
		if (AmmonomiconController.Instance.ImpendingLeftPageRenderer == null || AmmonomiconController.Instance.ImpendingLeftPageRenderer.LastFocusTarget == null)
		{
			switch (this.pageType)
			{
			case AmmonomiconPageRenderer.PageType.GUNS_RIGHT:
				this.SetFirstVisibleTexts();
				break;
			case AmmonomiconPageRenderer.PageType.ITEMS_RIGHT:
				this.SetFirstVisibleTexts();
				break;
			case AmmonomiconPageRenderer.PageType.ENEMIES_RIGHT:
				this.SetFirstVisibleTexts();
				break;
			case AmmonomiconPageRenderer.PageType.BOSSES_RIGHT:
				this.SetFirstVisibleTexts();
				break;
			}
		}
	}

	// Token: 0x060079B6 RID: 31158 RVA: 0x0030B39C File Offset: 0x0030959C
	private void SetFirstVisibleTexts()
	{
		if (AmmonomiconController.Instance.ImpendingLeftPageRenderer != null)
		{
			for (int i = 0; i < AmmonomiconController.Instance.ImpendingLeftPageRenderer.m_pokedexEntries.Count; i++)
			{
				AmmonomiconPokedexEntry ammonomiconPokedexEntry = AmmonomiconController.Instance.ImpendingLeftPageRenderer.m_pokedexEntries[i];
				if (ammonomiconPokedexEntry.encounterState == AmmonomiconPokedexEntry.EncounterState.ENCOUNTERED)
				{
					this.SetRightDataPageTexts(ammonomiconPokedexEntry.ChildSprite, ammonomiconPokedexEntry.linkedEncounterTrackable);
					if (AmmonomiconController.Instance.ImpendingLeftPageRenderer.LastFocusTarget == null)
					{
						AmmonomiconController.Instance.ImpendingLeftPageRenderer.LastFocusTarget = ammonomiconPokedexEntry.GetComponent<dfControl>();
					}
					return;
				}
				if (ammonomiconPokedexEntry.encounterState == AmmonomiconPokedexEntry.EncounterState.KNOWN)
				{
					this.SetPageDataUnknown(this);
					this.SetRightDataPageName(ammonomiconPokedexEntry.ChildSprite, ammonomiconPokedexEntry.linkedEncounterTrackable);
					if (AmmonomiconController.Instance.ImpendingLeftPageRenderer.LastFocusTarget == null)
					{
						AmmonomiconController.Instance.ImpendingLeftPageRenderer.LastFocusTarget = ammonomiconPokedexEntry.GetComponent<dfControl>();
					}
					return;
				}
			}
		}
		this.SetPageDataUnknown(this);
	}

	// Token: 0x060079B7 RID: 31159 RVA: 0x0030B498 File Offset: 0x00309698
	public void Initialize(MeshRenderer ts)
	{
		this.targetRenderer = ts;
		this.m_camera = base.GetComponent<Camera>();
		this.m_camera.aspect = 0.8888889f;
		this.guiManager = base.transform.parent.GetComponent<dfGUIManager>();
		this.guiManager.UIScale = 1f;
		Transform transform = this.guiManager.transform.Find("Scroll Panel");
		if (transform != null)
		{
			transform.GetComponent<dfScrollPanel>().LockScrollPanelToZero = true;
		}
		this.RebuildRenderData();
		this.topBezierPropID = Shader.PropertyToID("_TopBezier");
		this.leftBezierPropID = Shader.PropertyToID("_LeftBezier");
		this.rightBezierPropID = Shader.PropertyToID("_RightBezier");
		this.bottomBezierPropID = Shader.PropertyToID("_BottomBezier");
		Matrix4x4 matrix4x = default(Matrix4x4);
		matrix4x.SetRow(0, new Vector4(0f, 0f, 0f, 0f));
		matrix4x.SetRow(1, new Vector4(0f, 0f, 0f, 0f));
		matrix4x.SetRow(2, new Vector4(1f, 1f, 1f, 1f));
		matrix4x.SetRow(3, new Vector4(1f, 1f, 1f, 1f));
		this.SetMatrix(matrix4x);
		base.StartCoroutine(this.DelayedBuildPage());
	}

	// Token: 0x060079B8 RID: 31160 RVA: 0x0030B604 File Offset: 0x00309804
	private void RebuildRenderData()
	{
		if (this.m_renderBuffer != null)
		{
			RenderTexture.ReleaseTemporary(this.m_renderBuffer);
			this.m_renderBuffer = null;
		}
		Debug.LogWarning("Reacquiring Page Buffer 960x1080");
		this.m_renderBuffer = RenderTexture.GetTemporary(960, 1080, 0, RenderTextureFormat.Default);
		this.m_renderBuffer.name = "temporary ammonomicon render buffer";
		this.m_renderBuffer.filterMode = FilterMode.Point;
		this.m_renderBuffer.DiscardContents();
		this.m_camera.targetTexture = this.m_renderBuffer;
		this.renderMaterial = new Material(ShaderCache.Acquire("Custom/AmmonomiconPageShader"));
		this.renderMaterial.SetTexture("_MainTex", this.m_renderBuffer);
		this.targetRenderer.material = this.renderMaterial;
	}

	// Token: 0x060079B9 RID: 31161 RVA: 0x0030B6CC File Offset: 0x003098CC
	private IEnumerator DelayedBuildPage()
	{
		if (this.pageType == AmmonomiconPageRenderer.PageType.EQUIPMENT_LEFT)
		{
			while (GameManager.Instance.IsSelectingCharacter)
			{
				yield return null;
			}
		}
		switch (this.pageType)
		{
		case AmmonomiconPageRenderer.PageType.EQUIPMENT_LEFT:
			this.InitializeEquipmentPageLeft();
			break;
		case AmmonomiconPageRenderer.PageType.EQUIPMENT_RIGHT:
			this.InitializeEquipmentPageRight();
			break;
		case AmmonomiconPageRenderer.PageType.GUNS_LEFT:
			this.InitializeGunsPageLeft();
			break;
		case AmmonomiconPageRenderer.PageType.GUNS_RIGHT:
			this.SetPageDataUnknown(this);
			break;
		case AmmonomiconPageRenderer.PageType.ITEMS_LEFT:
			this.InitializeItemsPageLeft();
			break;
		case AmmonomiconPageRenderer.PageType.ITEMS_RIGHT:
			this.SetPageDataUnknown(this);
			break;
		case AmmonomiconPageRenderer.PageType.ENEMIES_LEFT:
			this.InitializeEnemiesPageLeft();
			break;
		case AmmonomiconPageRenderer.PageType.ENEMIES_RIGHT:
			this.SetPageDataUnknown(this);
			break;
		case AmmonomiconPageRenderer.PageType.BOSSES_LEFT:
			this.InitializeBossesPageLeft();
			break;
		case AmmonomiconPageRenderer.PageType.BOSSES_RIGHT:
			this.SetPageDataUnknown(this);
			break;
		case AmmonomiconPageRenderer.PageType.DEATH_LEFT:
			this.InitializeDeathPageLeft();
			break;
		case AmmonomiconPageRenderer.PageType.DEATH_RIGHT:
			this.InitializeDeathPageRight();
			break;
		}
		yield break;
	}

	// Token: 0x060079BA RID: 31162 RVA: 0x0030B6E8 File Offset: 0x003098E8
	private void InitializeDeathPageLeft()
	{
		AmmonomiconDeathPageController component = this.guiManager.GetComponent<AmmonomiconDeathPageController>();
		component.DoInitialize();
	}

	// Token: 0x060079BB RID: 31163 RVA: 0x0030B708 File Offset: 0x00309908
	private void InitializeDeathPageRight()
	{
		AmmonomiconDeathPageController component = this.guiManager.GetComponent<AmmonomiconDeathPageController>();
		component.DoInitialize();
		dfScrollPanel component2 = component.transform.Find("Scroll Panel").Find("Footer").Find("ScrollItemsPanel")
			.GetComponent<dfScrollPanel>();
		dfPanel component3 = component2.transform.Find("AllItemsPanel").GetComponent<dfPanel>();
		for (int i = 0; i < component3.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(component3.transform.GetChild(i).gameObject);
		}
		List<tk2dBaseSprite> list = new List<tk2dBaseSprite>();
		for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[j];
			for (int k = 0; k < playerController.inventory.AllGuns.Count; k++)
			{
				Gun gun = playerController.inventory.AllGuns[k];
				tk2dClippedSprite tk2dClippedSprite = this.AddSpriteToPage<tk2dClippedSprite>(gun.GetSprite().Collection, gun.DefaultSpriteID);
				SpriteOutlineManager.AddScaledOutlineToSprite<tk2dClippedSprite>(tk2dClippedSprite, Color.black, 0.1f, 0.01f);
				tk2dClippedSprite.transform.parent = component3.transform;
				tk2dClippedSprite.transform.position = component3.GetCenter();
				list.Add(tk2dClippedSprite);
			}
			for (int l = 0; l < playerController.activeItems.Count; l++)
			{
				tk2dClippedSprite tk2dClippedSprite2 = this.AddSpriteToPage<tk2dClippedSprite>(playerController.activeItems[l].sprite.Collection, playerController.activeItems[l].sprite.spriteId);
				SpriteOutlineManager.AddScaledOutlineToSprite<tk2dClippedSprite>(tk2dClippedSprite2, Color.black, 0.1f, 0.01f);
				tk2dClippedSprite2.transform.parent = component3.transform;
				tk2dClippedSprite2.transform.position = component3.GetCenter();
				list.Add(tk2dClippedSprite2);
			}
			for (int m = 0; m < playerController.passiveItems.Count; m++)
			{
				tk2dClippedSprite tk2dClippedSprite3 = this.AddSpriteToPage<tk2dClippedSprite>(playerController.passiveItems[m].sprite.Collection, playerController.passiveItems[m].sprite.spriteId);
				SpriteOutlineManager.AddScaledOutlineToSprite<tk2dClippedSprite>(tk2dClippedSprite3, Color.black, 0.1f, 0.01f);
				tk2dClippedSprite3.transform.parent = component3.transform;
				tk2dClippedSprite3.transform.position = component3.GetCenter();
				list.Add(tk2dClippedSprite3);
			}
		}
		list = list.ttOrderBy((tk2dBaseSprite a) => a.GetBounds().size.y);
		List<tk2dBaseSprite> list2 = new List<tk2dBaseSprite>();
		this.BoxArrangeItems(component3, list, new Vector2(0f, 6f), new Vector2(6f, 3f), ref list2);
		base.StartCoroutine(this.HandleDeathItemsClipping(component3, list));
	}

	// Token: 0x060079BC RID: 31164 RVA: 0x0030BA08 File Offset: 0x00309C08
	private IEnumerator HandleDeathItemsClipping(dfPanel parentPanel, List<tk2dBaseSprite> itemSprites)
	{
		while (!GameManager.Instance.IsLoadingLevel)
		{
			for (int i = 0; i < itemSprites.Count; i++)
			{
				tk2dClippedSprite tk2dClippedSprite = itemSprites[i] as tk2dClippedSprite;
				Vector3[] corners = parentPanel.Parent.GetCorners();
				float x = corners[0].x;
				float y = corners[0].y;
				float x2 = corners[3].x;
				float y2 = corners[3].y;
				Bounds untrimmedBounds = tk2dClippedSprite.GetUntrimmedBounds();
				untrimmedBounds.center += tk2dClippedSprite.transform.position;
				float num = Mathf.Clamp01((x - untrimmedBounds.min.x) / untrimmedBounds.size.x);
				float num2 = Mathf.Clamp01((y2 - untrimmedBounds.min.y) / untrimmedBounds.size.y);
				float num3 = Mathf.Clamp01((x2 - untrimmedBounds.min.x) / untrimmedBounds.size.x);
				float num4 = Mathf.Clamp01((y - untrimmedBounds.min.y) / untrimmedBounds.size.y);
				tk2dClippedSprite.clipBottomLeft = new Vector2(num, num2);
				tk2dClippedSprite.clipTopRight = new Vector2(num3, num4);
				if (SpriteOutlineManager.HasOutline(tk2dClippedSprite))
				{
					tk2dClippedSprite[] outlineSprites = SpriteOutlineManager.GetOutlineSprites<tk2dClippedSprite>(tk2dClippedSprite);
					for (int j = 0; j < outlineSprites.Length; j++)
					{
						outlineSprites[j].clipBottomLeft = tk2dClippedSprite.clipBottomLeft;
						outlineSprites[j].clipTopRight = tk2dClippedSprite.clipTopRight;
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060079BD RID: 31165 RVA: 0x0030BA2C File Offset: 0x00309C2C
	public void ReturnFocusToBookmarks()
	{
		this.LastFocusTarget = dfGUIManager.ActiveControl;
		for (int i = 0; i < AmmonomiconController.Instance.Ammonomicon.bookmarks.Length; i++)
		{
			if (AmmonomiconController.Instance.Ammonomicon.bookmarks[i].IsCurrentPage)
			{
				AmmonomiconController.Instance.Ammonomicon.bookmarks[i].ForceFocus();
				break;
			}
		}
	}

	// Token: 0x060079BE RID: 31166 RVA: 0x0030BA9C File Offset: 0x00309C9C
	public void LateUpdate()
	{
		if (this.m_camera.enabled && (!this.m_renderBuffer || this.m_renderBuffer == null || !this.m_renderBuffer.IsCreated()))
		{
			this.RebuildRenderData();
		}
		this.m_camera.transform.localPosition = new Vector3(0f, 0.001f, this.m_camera.transform.localPosition.z);
	}

	// Token: 0x060079BF RID: 31167 RVA: 0x0030BB28 File Offset: 0x00309D28
	public void DoRefreshData()
	{
		if (this.pageType == AmmonomiconPageRenderer.PageType.EQUIPMENT_LEFT)
		{
			for (int i = 0; i < this.m_pokedexEntries.Count; i++)
			{
				UnityEngine.Object.Destroy(this.m_pokedexEntries[i].gameObject);
			}
			this.LastFocusTarget = null;
			this.m_pokedexEntries.Clear();
			this.InitializeEquipmentPageLeft();
			if (this.m_pokedexEntries.Count > 0)
			{
				this.LastFocusTarget = this.m_pokedexEntries[0].GetComponent<dfButton>();
			}
			this.guiManager.UIScaleLegacyMode = true;
			this.guiManager.UIScaleLegacyMode = false;
		}
		else if (this.pageType == AmmonomiconPageRenderer.PageType.DEATH_LEFT)
		{
			this.InitializeDeathPageLeft();
		}
		else if (this.pageType == AmmonomiconPageRenderer.PageType.DEATH_RIGHT)
		{
			this.InitializeDeathPageRight();
		}
		else
		{
			for (int j = 0; j < this.m_pokedexEntries.Count; j++)
			{
				this.m_pokedexEntries[j].UpdateEncounterState();
			}
		}
	}

	// Token: 0x060079C0 RID: 31168 RVA: 0x0030BC30 File Offset: 0x00309E30
	public void BoxArrangeItems(dfPanel sourcePanel, List<tk2dBaseSprite> sourceElements, Vector2 panelPaddingPx, Vector2 elementPaddingPx, ref List<tk2dBaseSprite> previousLineSprites)
	{
		if (previousLineSprites == null)
		{
			previousLineSprites = new List<tk2dBaseSprite>();
		}
		List<tk2dBaseSprite> list = new List<tk2dBaseSprite>(sourceElements);
		float num = this.guiManager.PixelsToUnits();
		float num2 = (sourcePanel.Width - panelPaddingPx.x * 2f) * num;
		float num3 = 0f;
		for (int i = 0; i < list.Count; i++)
		{
			num3 = Mathf.Max(num3, list[i].GetBounds().size.y + 2f * elementPaddingPx.y * num);
		}
		float num4 = num2;
		float num5 = 0f;
		float num6 = -1f * panelPaddingPx.y * num;
		int num7 = 1;
		List<tk2dBaseSprite> list2 = new List<tk2dBaseSprite>();
		float num8 = panelPaddingPx.y * num;
		float num9 = 0f;
		while (list.Count > 0)
		{
			tk2dBaseSprite tk2dBaseSprite = list[0];
			list.RemoveAt(0);
			Bounds bounds = tk2dBaseSprite.GetBounds();
			Bounds untrimmedBounds = tk2dBaseSprite.GetUntrimmedBounds();
			Vector3 size = bounds.size;
			Vector3 size2 = untrimmedBounds.size;
			bool flag = size.x > num4;
			size.x = Mathf.Min(size.x, num2);
			size2.x = Mathf.Min(size2.x, num2);
			if (!flag)
			{
				num4 -= size.x + 2f * elementPaddingPx.x * num;
				float num10 = num6 - num3 + (num3 - size.y) / 2f;
				Vector3 vector = new Vector3(num5 + panelPaddingPx.x * num + elementPaddingPx.x * num, num10, 0f);
				tk2dBaseSprite.transform.parent = sourcePanel.transform;
				tk2dBaseSprite.PlaceAtLocalPositionByAnchor(vector, tk2dBaseSprite.Anchor.LowerLeft);
				num5 += size.x + 2f * elementPaddingPx.x * num;
				list2.Add(tk2dBaseSprite);
			}
			if (flag || list.Count == 0)
			{
				float num11 = num4;
				for (int j = 0; j < list2.Count; j++)
				{
					list2[j].transform.localPosition += new Vector3(num11 / 2f, 0f, 0f);
				}
				num8 += num3;
				if (previousLineSprites.Count > 0)
				{
					float num12 = 0f;
					for (int k = 0; k < list2.Count; k++)
					{
						num12 = Mathf.Max(num12, list2[k].GetBounds().size.y + 2f * elementPaddingPx.y * num);
					}
					float num13 = num3 - num12;
					if (list.Count == 0)
					{
						num13 = 0.5f * num9 + elementPaddingPx.y * num;
					}
					if (num13 > 0f)
					{
						for (int l = 0; l < list2.Count; l++)
						{
							list2[l].transform.localPosition = list2[l].transform.localPosition + new Vector3(0f, num13, 0f);
						}
						num6 += num13;
					}
					num8 -= num13;
					num9 = num13;
				}
				if (flag || list.Count != 0)
				{
					num6 -= num3;
					num5 = 0f;
					num4 = num2;
					num7++;
					list.Insert(0, tk2dBaseSprite);
					previousLineSprites = list2;
					list2 = new List<tk2dBaseSprite>();
				}
			}
		}
		previousLineSprites = list2;
		sourcePanel.Height = num8 / num + panelPaddingPx.y;
	}

	// Token: 0x060079C1 RID: 31169 RVA: 0x0030BFE8 File Offset: 0x0030A1E8
	private void SetPageDataUnknown(AmmonomiconPageRenderer rightPage)
	{
		if (rightPage == null)
		{
			return;
		}
		dfScrollPanel component = rightPage.guiManager.transform.Find("Scroll Panel").GetComponent<dfScrollPanel>();
		Transform transform = component.transform.Find("Header");
		if (transform)
		{
			dfLabel component2 = transform.Find("Label").GetComponent<dfLabel>();
			component2.Text = component2.ForceGetLocalizedValue("#AMMONOMICON_UNKNOWN");
			component2.PerformLayout();
			dfSprite component3 = transform.Find("Sprite").GetComponent<dfSprite>();
			if (component3)
			{
				component3.FillDirection = dfFillDirection.Vertical;
				component3.FillAmount = ((GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.ENGLISH) ? 0.8f : 1f);
				component3.InvertFill = true;
			}
		}
		dfLabel component4 = component.transform.Find("Tape Line One").Find("Label").GetComponent<dfLabel>();
		component4.Text = component4.ForceGetLocalizedValue("#AMMONOMICON_QUESTIONS");
		component4.PerformLayout();
		dfSlicedSprite componentInChildren = component.transform.Find("Tape Line One").GetComponentInChildren<dfSlicedSprite>();
		componentInChildren.Width = component4.GetAutosizeWidth() / 4f + 12f;
		dfLabel component5 = component.transform.Find("Tape Line Two").Find("Label").GetComponent<dfLabel>();
		component5.Text = component4.ForceGetLocalizedValue("#AMMONOMICON_QUESTIONS");
		component5.PerformLayout();
		dfSlicedSprite componentInChildren2 = component.transform.Find("Tape Line Two").GetComponentInChildren<dfSlicedSprite>();
		componentInChildren2.Width = component5.GetAutosizeWidth() / 4f + 12f;
		dfPanel component6 = component.transform.Find("ThePhoto").Find("Photo").Find("tk2dSpriteHolder")
			.GetComponent<dfPanel>();
		dfSprite component7 = component.transform.Find("ThePhoto").Find("Photo").Find("ItemShadow")
			.GetComponent<dfSprite>();
		component7.IsVisible = false;
		tk2dSprite componentInChildren3 = component6.GetComponentInChildren<tk2dSprite>();
		dfTextureSprite componentInChildren4 = component.transform.Find("ThePhoto").GetComponentInChildren<dfTextureSprite>();
		if (componentInChildren4 != null)
		{
			componentInChildren4.IsVisible = false;
		}
		if (!(componentInChildren3 == null))
		{
			if (SpriteOutlineManager.HasOutline(componentInChildren3))
			{
				SpriteOutlineManager.RemoveOutlineFromSprite(componentInChildren3, true);
			}
			componentInChildren3.renderer.enabled = false;
		}
		dfLabel component8 = component.transform.Find("Scroll Panel").Find("Panel").Find("Label")
			.GetComponent<dfLabel>();
		this.CheckLanguageFonts(component8);
		component8.Text = component8.ForceGetLocalizedValue("#AMMONOMICON_MYSTERIOUS");
		component8.transform.parent.GetComponent<dfPanel>().Height = component8.Height;
		component.transform.Find("Scroll Panel").GetComponent<dfScrollPanel>().ScrollPosition = Vector2.zero;
	}

	// Token: 0x060079C2 RID: 31170 RVA: 0x0030C2D0 File Offset: 0x0030A4D0
	public void SetRightDataPageUnknown(bool impending = false)
	{
		AmmonomiconPageRenderer ammonomiconPageRenderer = ((!impending) ? AmmonomiconController.Instance.CurrentRightPageRenderer : AmmonomiconController.Instance.ImpendingRightPageRenderer);
		this.SetPageDataUnknown(ammonomiconPageRenderer);
	}

	// Token: 0x060079C3 RID: 31171 RVA: 0x0030C304 File Offset: 0x0030A504
	private void CheckHeaderFonts(dfLabel headerLabel, bool isPrimaryLabel)
	{
		if (this.BaseAlagardFont == null)
		{
			this.BaseAlagardFont = headerLabel.Font;
			this.OtherAlagardFont = (BraveResources.Load("Alternate Fonts/AlagardExtended22", ".prefab") as GameObject).GetComponent<dfFont>();
		}
		if (isPrimaryLabel)
		{
			headerLabel.BringToFront();
		}
		if (StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
		{
			if (headerLabel.Font != this.BaseAlagardFont || headerLabel.TextScale != 2f)
			{
				headerLabel.Font = this.BaseAlagardFont;
				headerLabel.TextScale = 2f;
				headerLabel.PerformLayout();
			}
			if (isPrimaryLabel && headerLabel.RelativePosition.y != this.OriginalHeaderRelativeY.Value)
			{
				headerLabel.RelativePosition = headerLabel.RelativePosition.WithY(this.OriginalHeaderRelativeY.Value);
				headerLabel.PerformLayout();
			}
		}
		else if (StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.JAPANESE || StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.RUSSIAN || StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE)
		{
			if (isPrimaryLabel && headerLabel.RelativePosition.y != this.OriginalHeaderRelativeY.Value)
			{
				headerLabel.RelativePosition = headerLabel.RelativePosition.WithY(this.OriginalHeaderRelativeY.Value);
				headerLabel.PerformLayout();
			}
		}
		else if (StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.KOREAN)
		{
			if (isPrimaryLabel && headerLabel.RelativePosition.y != this.OriginalHeaderRelativeY.Value - 16f)
			{
				headerLabel.RelativePosition = headerLabel.RelativePosition.WithY(this.OriginalHeaderRelativeY.Value - 16f);
				headerLabel.PerformLayout();
			}
		}
		else
		{
			if (headerLabel != this.OtherAlagardFont || headerLabel.TextScale != 4f)
			{
				headerLabel.Font = this.OtherAlagardFont;
				headerLabel.TextScale = 4f;
				headerLabel.PerformLayout();
			}
			if (isPrimaryLabel && headerLabel.RelativePosition.y != this.OriginalHeaderRelativeY.Value - 24f)
			{
				headerLabel.RelativePosition = headerLabel.RelativePosition.WithY(this.OriginalHeaderRelativeY.Value - 24f);
				headerLabel.PerformLayout();
			}
		}
	}

	// Token: 0x060079C4 RID: 31172 RVA: 0x0030C554 File Offset: 0x0030A754
	private void AdjustForChinese()
	{
		if (AmmonomiconController.Instance != null && ((this.m_hasAdjustedForChinese && GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.CHINESE) || (!this.m_hasAdjustedForChinese && GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE)))
		{
			AmmonomiconPageRenderer ammonomiconPageRenderer = ((!(AmmonomiconController.Instance.ImpendingRightPageRenderer != null)) ? AmmonomiconController.Instance.CurrentRightPageRenderer : AmmonomiconController.Instance.ImpendingRightPageRenderer);
			if (ammonomiconPageRenderer != null)
			{
				dfScrollPanel component = ammonomiconPageRenderer.guiManager.transform.Find("Scroll Panel").GetComponent<dfScrollPanel>();
				dfLabel component2 = component.transform.Find("Tape Line One").Find("Label").GetComponent<dfLabel>();
				dfLabel component3 = component.transform.Find("Tape Line Two").Find("Label").GetComponent<dfLabel>();
				if (this.m_hasAdjustedForChinese && GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.CHINESE)
				{
					component2.RelativePosition += new Vector3(0f, -8f, 0f);
					component3.RelativePosition += new Vector3(0f, -8f, 0f);
					this.m_hasAdjustedForChinese = false;
				}
				else if (!this.m_hasAdjustedForChinese && GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE)
				{
					component2.RelativePosition += new Vector3(0f, 8f, 0f);
					component3.RelativePosition += new Vector3(0f, 8f, 0f);
					this.m_hasAdjustedForChinese = true;
				}
			}
		}
	}

	// Token: 0x060079C5 RID: 31173 RVA: 0x0030C720 File Offset: 0x0030A920
	private void CheckLanguageFonts(dfLabel mainText)
	{
		if (this.EnglishFont == null)
		{
			this.EnglishFont = mainText.Font;
			this.OtherLanguageFont = GameUIRoot.Instance.Manager.DefaultFont;
		}
		this.AdjustForChinese();
		if (this.m_cachedLanguage != GameManager.Options.CurrentLanguage)
		{
			this.m_cachedLanguage = GameManager.Options.CurrentLanguage;
			switch (this.pageType)
			{
			case AmmonomiconPageRenderer.PageType.GUNS_RIGHT:
				this.SetPageDataUnknown(this);
				break;
			case AmmonomiconPageRenderer.PageType.ITEMS_RIGHT:
				this.SetPageDataUnknown(this);
				break;
			case AmmonomiconPageRenderer.PageType.ENEMIES_RIGHT:
				this.SetPageDataUnknown(this);
				break;
			case AmmonomiconPageRenderer.PageType.BOSSES_RIGHT:
				this.SetPageDataUnknown(this);
				break;
			}
		}
		if (StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
		{
			if (mainText.Font != this.EnglishFont)
			{
				mainText.Atlas = this.guiManager.DefaultAtlas;
				mainText.Font = this.EnglishFont;
			}
		}
		else if (StringTableManager.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.JAPANESE && StringTableManager.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.KOREAN && StringTableManager.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.CHINESE && StringTableManager.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.RUSSIAN)
		{
			if (mainText.Font != this.OtherLanguageFont)
			{
				mainText.Atlas = GameUIRoot.Instance.Manager.DefaultAtlas;
				mainText.Font = this.OtherLanguageFont;
			}
		}
	}

	// Token: 0x060079C6 RID: 31174 RVA: 0x0030C898 File Offset: 0x0030AA98
	public void SetRightDataPageName(tk2dBaseSprite sourceSprite, EncounterDatabaseEntry linkedTrackable)
	{
		JournalEntry journalData = linkedTrackable.journalData;
		AmmonomiconPageRenderer ammonomiconPageRenderer = ((!(AmmonomiconController.Instance.ImpendingRightPageRenderer != null)) ? AmmonomiconController.Instance.CurrentRightPageRenderer : AmmonomiconController.Instance.ImpendingRightPageRenderer);
		dfScrollPanel component = ammonomiconPageRenderer.guiManager.transform.Find("Scroll Panel").GetComponent<dfScrollPanel>();
		Transform transform = component.transform.Find("Header");
		if (transform)
		{
			dfLabel component2 = transform.Find("Label").GetComponent<dfLabel>();
			component2.Text = journalData.GetPrimaryDisplayName(false);
			if (linkedTrackable.ForceEncounterState)
			{
				component2.Text = component2.ForceGetLocalizedValue("#AMMONOMICON_UNKNOWN");
			}
			component2.PerformLayout();
			dfSprite component3 = transform.Find("Sprite").GetComponent<dfSprite>();
			if (component3)
			{
				component3.FillDirection = dfFillDirection.Vertical;
				component3.FillAmount = ((GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.ENGLISH) ? 0.8f : 1f);
				component3.InvertFill = true;
			}
		}
	}

	// Token: 0x060079C7 RID: 31175 RVA: 0x0030C9AC File Offset: 0x0030ABAC
	public void SetRightDataPageTexts(tk2dBaseSprite sourceSprite, EncounterDatabaseEntry linkedTrackable)
	{
		JournalEntry journalData = linkedTrackable.journalData;
		AmmonomiconPageRenderer ammonomiconPageRenderer = ((!(AmmonomiconController.Instance.ImpendingRightPageRenderer != null)) ? AmmonomiconController.Instance.CurrentRightPageRenderer : AmmonomiconController.Instance.ImpendingRightPageRenderer);
		dfScrollPanel component = ammonomiconPageRenderer.guiManager.transform.Find("Scroll Panel").GetComponent<dfScrollPanel>();
		Transform transform = component.transform.Find("Header");
		if (transform)
		{
			dfLabel component2 = transform.Find("Label").GetComponent<dfLabel>();
			component2.Text = journalData.GetPrimaryDisplayName(false);
			if (linkedTrackable.ForceEncounterState)
			{
				component2.Text = component2.ForceGetLocalizedValue("#AMMONOMICON_UNKNOWN");
			}
			component2.PerformLayout();
			dfSprite component3 = transform.Find("Sprite").GetComponent<dfSprite>();
			if (component3)
			{
				component3.FillDirection = dfFillDirection.Vertical;
				component3.FillAmount = ((GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.ENGLISH) ? 0.8f : 1f);
				component3.InvertFill = true;
			}
		}
		dfLabel component4 = component.transform.Find("Tape Line One").Find("Label").GetComponent<dfLabel>();
		component4.Text = journalData.GetNotificationPanelDescription();
		component4.PerformLayout();
		dfSlicedSprite componentInChildren = component.transform.Find("Tape Line One").GetComponentInChildren<dfSlicedSprite>();
		componentInChildren.Width = component4.GetAutosizeWidth() / 4f + 12f;
		dfLabel component5 = component.transform.Find("Tape Line Two").Find("Label").GetComponent<dfLabel>();
		component5.Text = linkedTrackable.GetSecondTapeDescriptor();
		component5.PerformLayout();
		dfSlicedSprite componentInChildren2 = component.transform.Find("Tape Line Two").GetComponentInChildren<dfSlicedSprite>();
		componentInChildren2.Width = component5.GetAutosizeWidth() / 4f + 12f;
		dfPanel component6 = component.transform.Find("ThePhoto").Find("Photo").Find("tk2dSpriteHolder")
			.GetComponent<dfPanel>();
		dfSprite component7 = component.transform.Find("ThePhoto").Find("Photo").Find("ItemShadow")
			.GetComponent<dfSprite>();
		component7.IsVisible = !journalData.IsEnemy;
		tk2dSprite tk2dSprite = component6.GetComponentInChildren<tk2dSprite>();
		dfTextureSprite componentInChildren3 = component.transform.Find("ThePhoto").GetComponentInChildren<dfTextureSprite>();
		if (journalData.IsEnemy && journalData.enemyPortraitSprite != null)
		{
			if (tk2dSprite != null)
			{
				if (SpriteOutlineManager.HasOutline(tk2dSprite))
				{
					SpriteOutlineManager.RemoveOutlineFromSprite(tk2dSprite, true);
				}
				tk2dSprite.renderer.enabled = false;
			}
			componentInChildren3.IsVisible = true;
			componentInChildren3.Texture = journalData.enemyPortraitSprite;
		}
		else
		{
			if (componentInChildren3 != null)
			{
				componentInChildren3.IsVisible = false;
			}
			if (tk2dSprite == null)
			{
				tk2dSprite = this.AddSpriteToPage(sourceSprite);
				if (!journalData.IsEnemy)
				{
					tk2dSprite.scale *= 2f;
				}
				tk2dSprite.transform.parent = component6.transform;
			}
			else
			{
				tk2dSprite.renderer.enabled = true;
				tk2dSprite.SetSprite(sourceSprite.Collection, sourceSprite.spriteId);
			}
			if (SpriteOutlineManager.HasOutline(tk2dSprite))
			{
				SpriteOutlineManager.RemoveOutlineFromSprite(tk2dSprite, true);
			}
			SpriteOutlineManager.AddScaledOutlineToSprite<tk2dSprite>(tk2dSprite, Color.black, 0.1f, 0.05f);
			if (journalData.IsEnemy)
			{
				tk2dSprite.PlaceAtLocalPositionByAnchor(Vector3.zero, tk2dBaseSprite.Anchor.MiddleCenter);
			}
			else
			{
				tk2dSprite.PlaceAtLocalPositionByAnchor(Vector3.zero, tk2dBaseSprite.Anchor.LowerCenter);
			}
			if (Mathf.RoundToInt(sourceSprite.GetCurrentSpriteDef().GetBounds().size.x / 0.0625f) % 2 == 1)
			{
				tk2dSprite.transform.position = tk2dSprite.transform.position.WithX(tk2dSprite.transform.position.x - 0.03125f * tk2dSprite.scale.x);
			}
			tk2dSprite.usesOverrideMaterial = true;
			tk2dSprite.renderer.material.shader = ShaderCache.Acquire("tk2d/CutoutVertexColorTilted");
		}
		dfLabel component8 = component.transform.Find("Scroll Panel").Find("Panel").Find("Label")
			.GetComponent<dfLabel>();
		this.CheckLanguageFonts(component8);
		component8.Text = linkedTrackable.GetModifiedLongDescription();
		component8.transform.parent.GetComponent<dfPanel>().Height = component8.Height;
		component8.PerformLayout();
		component8.Update();
		dfScrollPanel component9 = component.transform.Find("Scroll Panel").GetComponent<dfScrollPanel>();
		component9.ScrollPosition = Vector2.zero;
		component.PerformLayout();
		component.Update();
	}

	// Token: 0x060079C8 RID: 31176 RVA: 0x0030CE84 File Offset: 0x0030B084
	private IEnumerator ConstructRectanglePageLayout(dfPanel sourcePanel, List<EncounterDatabaseEntry> journalEntries, Vector2 panelPaddingPx, Vector2 elementPaddingPx, bool hideButtons = false, List<AdvancedSynergyEntry> activeSynergies = null)
	{
		float boxyBox = (float)((!hideButtons) ? 20 : 8);
		float p2u = this.guiManager.PixelsToUnits();
		float panelWidthUnits = (sourcePanel.Width - panelPaddingPx.x * 2f) * p2u;
		float remainingLineWidth = panelWidthUnits;
		List<AmmonomiconPageRenderer.RectangleLineInfo> lineInfos = new List<AmmonomiconPageRenderer.RectangleLineInfo>();
		AmmonomiconPageRenderer.RectangleLineInfo currentLineInfo = default(AmmonomiconPageRenderer.RectangleLineInfo);
		float totalUnitHeight = 0f;
		tk2dSpriteCollectionData iconCollection = AmmonomiconController.ForceInstance.EncounterIconCollection;
		for (int i = 0; i < journalEntries.Count; i++)
		{
			if (journalEntries[i] != null)
			{
				string text = journalEntries[i].journalData.AmmonomiconSprite;
				if (text.StartsWith("gunderfury_LV"))
				{
					text = "gunderfury_LV" + (GunderfuryController.GetCurrentTier() + 1) + "0_idle_001";
				}
				tk2dSpriteDefinition tk2dSpriteDefinition = null;
				if (!string.IsNullOrEmpty(text))
				{
					tk2dSpriteDefinition = iconCollection.GetSpriteDefinition(text);
				}
				Bounds bounds;
				if (tk2dSpriteDefinition != null)
				{
					bounds = tk2dSpriteDefinition.GetBounds();
				}
				else
				{
					bounds = iconCollection.GetSpriteDefinition(AmmonomiconController.AmmonomiconErrorSprite).GetBounds();
				}
				Vector2 vector = bounds.size * 16f;
				Vector2 vector2 = (vector * 4f + elementPaddingPx * 2f) * p2u;
				if (remainingLineWidth < vector2.x)
				{
					totalUnitHeight += currentLineInfo.lineHeightUnits;
					if (this.pageType == AmmonomiconPageRenderer.PageType.EQUIPMENT_LEFT)
					{
						remainingLineWidth += elementPaddingPx.x * p2u * 2f + 4f * p2u;
					}
					currentLineInfo.initialXOffset = (remainingLineWidth / 2f).Quantize(p2u * 4f);
					lineInfos.Add(currentLineInfo);
					currentLineInfo = default(AmmonomiconPageRenderer.RectangleLineInfo);
					remainingLineWidth = panelWidthUnits;
				}
				currentLineInfo.numberOfElements++;
				currentLineInfo.lineHeightUnits = Mathf.Max(currentLineInfo.lineHeightUnits, vector2.y);
				remainingLineWidth -= vector2.x;
			}
		}
		if (this.pageType == AmmonomiconPageRenderer.PageType.EQUIPMENT_LEFT)
		{
			remainingLineWidth += elementPaddingPx.x * p2u * 2f + 4f * p2u;
		}
		totalUnitHeight += currentLineInfo.lineHeightUnits;
		currentLineInfo.initialXOffset = (remainingLineWidth / 2f).Quantize(p2u * 4f);
		lineInfos.Add(currentLineInfo);
		int accumulatedSpriteIndex = 0;
		float currentYLineTop = -(panelPaddingPx.y * p2u);
		dfButton prevButton = null;
		if (this.m_prevLineButtons == null)
		{
			this.m_prevLineButtons = new List<dfButton>();
		}
		GameObject pokedexBox = (GameObject)BraveResources.Load("Global Prefabs/Pokedex Box", ".prefab");
		for (int j = 0; j < lineInfos.Count; j++)
		{
			currentLineInfo = lineInfos[j];
			List<dfButton> list = new List<dfButton>();
			for (int k = 0; k < currentLineInfo.numberOfElements; k++)
			{
				EncounterDatabaseEntry encounterDatabaseEntry = journalEntries[accumulatedSpriteIndex];
				string text2 = encounterDatabaseEntry.journalData.AmmonomiconSprite;
				if (text2.StartsWith("gunderfury_LV"))
				{
					text2 = "gunderfury_LV60_idle_001";
				}
				int num = iconCollection.GetSpriteIdByName(text2, -1);
				if (num < 0)
				{
					Debug.LogWarning("Missing sprite " + text2 + "; add this to the Ammonomicon Icon Collection.");
					num = iconCollection.GetSpriteIdByName(AmmonomiconController.AmmonomiconErrorSprite);
				}
				dfButton dfButton = sourcePanel.AddPrefab(pokedexBox) as dfButton;
				dfButton.MakePixelPerfect();
				dfButton.PerformLayout();
				tk2dClippedSprite tk2dClippedSprite = this.AddSpriteToPage<tk2dClippedSprite>(iconCollection, num);
				if (journalEntries[accumulatedSpriteIndex].path.Contains("ResourcefulRatNote"))
				{
					tk2dClippedSprite.SetSprite("resourcefulrat_note_base_001");
				}
				float num2 = (tk2dClippedSprite.GetBounds().size * 16f * 4f * p2u).x / tk2dClippedSprite.scale.x;
				dfButton.Size = new Vector2(num2 / p2u + boxyBox * 2f, currentLineInfo.lineHeightUnits / p2u - (elementPaddingPx.y * 2f - boxyBox * 2f));
				if (text2.StartsWith("gunderfury_LV"))
				{
					text2 = "gunderfury_LV" + (GunderfuryController.GetCurrentTier() + 1) + "0_idle_001";
					num = iconCollection.GetSpriteIdByName(text2, -1);
					tk2dClippedSprite.SetSprite(num);
				}
				tk2dClippedSprite.transform.parent = dfButton.transform.Find("CenterPoint");
				tk2dClippedSprite.PlaceAtLocalPositionByAnchor(Vector3.zero, tk2dBaseSprite.Anchor.MiddleCenter);
				tk2dClippedSprite.transform.position = tk2dClippedSprite.transform.position.Quantize(4f * p2u);
				if (hideButtons)
				{
					SpriteOutlineManager.AddScaledOutlineToSprite<tk2dClippedSprite>(tk2dClippedSprite, Color.black, 0.1f, 0.05f);
				}
				if (j == 0 && k == 0)
				{
					dfButton.RelativePosition = new Vector3(currentLineInfo.initialXOffset / p2u + (elementPaddingPx.x - boxyBox), -currentYLineTop / p2u + (elementPaddingPx.y - boxyBox), 0f);
				}
				else if (k == 0)
				{
					dfButton.RelativePosition = new Vector3((currentLineInfo.initialXOffset / p2u).Quantize(4f), prevButton.RelativePosition.y + prevButton.Height + 4f, 0f);
				}
				if (k > 0)
				{
					dfButton.RelativePosition = prevButton.RelativePosition + new Vector3(prevButton.Width + 4f, 0f, 0f);
				}
				dfButton.RelativePosition = dfButton.RelativePosition.Quantize(4f);
				dfButton.PerformLayout();
				AmmonomiconPokedexEntry component = dfButton.GetComponent<AmmonomiconPokedexEntry>();
				component.IsEquipmentPage = hideButtons;
				component.IsGunderfury = text2.StartsWith("gunderfury_LV");
				component.AssignSprite(tk2dClippedSprite);
				component.linkedEncounterTrackable = journalEntries[accumulatedSpriteIndex];
				if (hideButtons)
				{
					component.pickupID = component.linkedEncounterTrackable.pickupObjectId;
					component.activeSynergies = activeSynergies;
				}
				if (journalEntries[accumulatedSpriteIndex].ForceEncounterState)
				{
					component.encounterState = AmmonomiconPokedexEntry.EncounterState.KNOWN;
					component.ForceEncounterState = true;
				}
				component.UpdateEncounterState();
				this.m_pokedexEntries.Add(component);
				UIKeyControls uikeyControls = component.gameObject.AddComponent<UIKeyControls>();
				uikeyControls.selectOnAction = true;
				if (k > 0)
				{
					uikeyControls.left = prevButton;
					prevButton.GetComponent<UIKeyControls>().right = dfButton;
				}
				else
				{
					UIKeyControls uikeyControls2 = uikeyControls;
					uikeyControls2.OnLeftDown = (Action)Delegate.Combine(uikeyControls2.OnLeftDown, new Action(this.ReturnFocusToBookmarks));
				}
				if (hideButtons)
				{
					dfButton.Opacity = 0.01f;
				}
				list.Add(dfButton);
				prevButton = dfButton;
				accumulatedSpriteIndex++;
			}
			if (j == 0)
			{
				Debug.Log(this.m_prevLineButtons + "|" + ((this.m_prevLineButtons != null) ? this.m_prevLineButtons.Count.ToString() : "null"));
			}
			if (this.m_prevLineButtons != null && this.m_prevLineButtons.Count > 0)
			{
				for (int l = 0; l < this.m_prevLineButtons.Count; l++)
				{
					int num3 = Mathf.RoundToInt((float)l / ((float)(this.m_prevLineButtons.Count - 1) * 1f) * (float)(list.Count - 1));
					num3 = Mathf.Clamp(num3, 0, list.Count - 1);
					UIKeyControls component2 = this.m_prevLineButtons[l].GetComponent<UIKeyControls>();
					if (component2 != null && num3 >= 0 && num3 < list.Count)
					{
						component2.down = list[num3];
					}
				}
				for (int m = 0; m < list.Count; m++)
				{
					int num4 = Mathf.RoundToInt((float)m / ((float)(list.Count - 1) * 1f) * (float)(this.m_prevLineButtons.Count - 1));
					num4 = Mathf.Clamp(num4, 0, this.m_prevLineButtons.Count - 1);
					UIKeyControls component3 = list[m].GetComponent<UIKeyControls>();
					if (component3 != null && num4 >= 0 && num4 < this.m_prevLineButtons.Count)
					{
						component3.up = this.m_prevLineButtons[num4];
					}
				}
			}
			this.m_prevLineButtons = list;
		}
		sourcePanel.Height = totalUnitHeight / p2u + 2f * panelPaddingPx.y + (float)(8 * lineInfos.Count);
		yield return null;
		if (!hideButtons)
		{
			this.SetRightDataPageUnknown(AmmonomiconController.Instance.IsTurningPage);
		}
		yield break;
	}

	// Token: 0x060079C9 RID: 31177 RVA: 0x0030CECC File Offset: 0x0030B0CC
	private void InternalInitializeEnemiesPage(bool isBosses)
	{
		Transform transform = this.guiManager.transform.Find("Scroll Panel").Find("Scroll Panel");
		dfPanel component = transform.Find("Guns Panel").GetComponent<dfPanel>();
		List<KeyValuePair<int, EncounterDatabaseEntry>> list = new List<KeyValuePair<int, EncounterDatabaseEntry>>();
		for (int i = 0; i < EnemyDatabase.Instance.Entries.Count; i++)
		{
			EnemyDatabaseEntry enemyDatabaseEntry = EnemyDatabase.Instance.Entries[i];
			if (enemyDatabaseEntry != null && enemyDatabaseEntry.isInBossTab == isBosses)
			{
				if (!string.IsNullOrEmpty(enemyDatabaseEntry.encounterGuid))
				{
					if (!EncounterDatabase.IsProxy(enemyDatabaseEntry.encounterGuid))
					{
						int num = ((enemyDatabaseEntry.ForcedPositionInAmmonomicon >= 0) ? enemyDatabaseEntry.ForcedPositionInAmmonomicon : 1000000000);
						list.Add(new KeyValuePair<int, EncounterDatabaseEntry>(num, EncounterDatabase.GetEntry(enemyDatabaseEntry.encounterGuid)));
					}
				}
			}
		}
		list = list.OrderBy((KeyValuePair<int, EncounterDatabaseEntry> e) => e.Key).ToList<KeyValuePair<int, EncounterDatabaseEntry>>();
		List<EncounterDatabaseEntry> list2 = new List<EncounterDatabaseEntry>();
		dfPanel component2 = component.transform.GetChild(0).GetComponent<dfPanel>();
		for (int j = 0; j < list.Count; j++)
		{
			KeyValuePair<int, EncounterDatabaseEntry> keyValuePair = list[j];
			if (keyValuePair.Value != null)
			{
				if (!keyValuePair.Value.journalData.SuppressInAmmonomicon)
				{
					list2.Add(keyValuePair.Value);
				}
			}
		}
		base.StartCoroutine(this.ConstructRectanglePageLayout(component2, list2, new Vector2(12f, 20f), new Vector2(20f, 20f), false, null));
		component2.Anchor = dfAnchorStyle.Top | dfAnchorStyle.Bottom | dfAnchorStyle.CenterHorizontal;
		component.Height = component2.Height;
		component2.Height = component.Height;
	}

	// Token: 0x060079CA RID: 31178 RVA: 0x0030D0B4 File Offset: 0x0030B2B4
	public void InitializeBossesPageLeft()
	{
		this.InternalInitializeEnemiesPage(true);
	}

	// Token: 0x060079CB RID: 31179 RVA: 0x0030D0C0 File Offset: 0x0030B2C0
	public void InitializeEnemiesPageLeft()
	{
		this.InternalInitializeEnemiesPage(false);
	}

	// Token: 0x060079CC RID: 31180 RVA: 0x0030D0CC File Offset: 0x0030B2CC
	public void InitializeItemsPageLeft()
	{
		Transform transform = this.guiManager.transform.Find("Scroll Panel").Find("Scroll Panel");
		dfPanel component = transform.Find("Guns Panel").GetComponent<dfPanel>();
		List<KeyValuePair<int, PickupObject>> list = new List<KeyValuePair<int, PickupObject>>();
		for (int i = 0; i < PickupObjectDatabase.Instance.Objects.Count; i++)
		{
			PickupObject pickupObject = PickupObjectDatabase.Instance.Objects[i];
			if (!(pickupObject is Gun) && pickupObject != null)
			{
				EncounterTrackable component2 = pickupObject.GetComponent<EncounterTrackable>();
				if (!(component2 == null))
				{
					if (string.IsNullOrEmpty(component2.ProxyEncounterGuid))
					{
						if (pickupObject.quality != PickupObject.ItemQuality.EXCLUDED)
						{
							int num = ((pickupObject.ForcedPositionInAmmonomicon >= 0) ? pickupObject.ForcedPositionInAmmonomicon : 1000000000);
							list.Add(new KeyValuePair<int, PickupObject>(num, pickupObject));
						}
					}
				}
			}
		}
		list = list.OrderBy((KeyValuePair<int, PickupObject> e) => e.Key).ToList<KeyValuePair<int, PickupObject>>();
		List<EncounterDatabaseEntry> list2 = new List<EncounterDatabaseEntry>();
		dfPanel component3 = component.transform.GetChild(0).GetComponent<dfPanel>();
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].Value.quality != PickupObject.ItemQuality.EXCLUDED)
			{
				EncounterTrackable component4 = list[j].Value.GetComponent<EncounterTrackable>();
				if (!component4.journalData.SuppressInAmmonomicon)
				{
					EncounterDatabaseEntry entry = EncounterDatabase.GetEntry(component4.EncounterGuid);
					if (list[j].Value is ContentTeaserItem || list[j].Value is ContentTeaserGun)
					{
						entry.ForceEncounterState = true;
					}
					if (entry != null)
					{
						list2.Add(entry);
					}
				}
			}
		}
		base.StartCoroutine(this.ConstructRectanglePageLayout(component3, list2, new Vector2(12f, 20f), new Vector2(20f, 20f), false, null));
		component3.Anchor = dfAnchorStyle.Top | dfAnchorStyle.Bottom | dfAnchorStyle.CenterHorizontal;
		component.Height = component3.Height;
		component3.Height = component.Height;
	}

	// Token: 0x060079CD RID: 31181 RVA: 0x0030D32C File Offset: 0x0030B52C
	public void InitializeGunsPageLeft()
	{
		Transform transform = this.guiManager.transform.Find("Scroll Panel").Find("Scroll Panel");
		dfPanel component = transform.Find("Guns Panel").GetComponent<dfPanel>();
		List<Gun> list = new List<Gun>();
		bool flag = GameStatsManager.HasInstance && GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_AMMONOMICON_COMPLETE);
		for (int i = 0; i < PickupObjectDatabase.Instance.Objects.Count; i++)
		{
			if (PickupObjectDatabase.Instance.Objects[i] is Gun)
			{
				Gun gun = PickupObjectDatabase.Instance.Objects[i] as Gun;
				EncounterTrackable component2 = gun.GetComponent<EncounterTrackable>();
				if (!(component2 == null))
				{
					if (string.IsNullOrEmpty(component2.ProxyEncounterGuid))
					{
						if (gun.quality != PickupObject.ItemQuality.EXCLUDED)
						{
							if (!flag || gun.PickupObjectId != GlobalItemIds.UnfinishedGun)
							{
								if (flag || gun.PickupObjectId != GlobalItemIds.FinishedGun)
								{
									list.Add(gun);
								}
							}
						}
					}
				}
			}
		}
		list = list.OrderBy((Gun g) => (g.ForcedPositionInAmmonomicon >= 0) ? g.ForcedPositionInAmmonomicon : 1000000000).ToList<Gun>();
		List<EncounterDatabaseEntry> list2 = new List<EncounterDatabaseEntry>();
		dfPanel component3 = component.transform.GetChild(0).GetComponent<dfPanel>();
		for (int j = 0; j < list.Count; j++)
		{
			EncounterTrackable component4 = list[j].GetComponent<EncounterTrackable>();
			if (!component4.journalData.SuppressInAmmonomicon)
			{
				EncounterDatabaseEntry entry = EncounterDatabase.GetEntry(component4.EncounterGuid);
				if (entry != null)
				{
					if (list[j] is ContentTeaserGun)
					{
						entry.ForceEncounterState = true;
					}
					list2.Add(entry);
				}
			}
		}
		base.StartCoroutine(this.ConstructRectanglePageLayout(component3, list2, new Vector2(12f, 20f), new Vector2(20f, 20f), false, null));
		component3.Anchor = dfAnchorStyle.Top | dfAnchorStyle.Bottom | dfAnchorStyle.CenterHorizontal;
		component.Height = component3.Height;
		component3.Height = component.Height;
	}

	// Token: 0x060079CE RID: 31182 RVA: 0x0030D57C File Offset: 0x0030B77C
	public void InitializeEquipmentPageRight()
	{
	}

	// Token: 0x060079CF RID: 31183 RVA: 0x0030D580 File Offset: 0x0030B780
	public void InitializeEquipmentPageLeft()
	{
		Debug.Log("INITIALIZING EQUIPMENT PAGE LEFT");
		Transform transform = this.guiManager.transform.Find("Scroll Panel").Find("Scroll Panel");
		this.PrimaryClipPanel = transform.GetComponent<dfControl>();
		dfPanel component = transform.Find("Guns Panel").GetComponent<dfPanel>();
		List<EncounterDatabaseEntry> list = new List<EncounterDatabaseEntry>();
		PlayerController playerController = null;
		bool flag = false;
		if (GameManager.Instance.IsSelectingCharacter)
		{
			flag = true;
			if (Foyer.Instance.CurrentSelectedCharacterFlag)
			{
				playerController = ((GameObject)BraveResources.Load(Foyer.Instance.CurrentSelectedCharacterFlag.CharacterPrefabPath, ".prefab")).GetComponent<PlayerController>();
			}
		}
		else
		{
			playerController = GameManager.Instance.PrimaryPlayer;
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				if (GameManager.Instance.AllPlayers[i].PlayerIDX == GameManager.Instance.LastPausingPlayerID)
				{
					playerController = GameManager.Instance.AllPlayers[i];
				}
			}
		}
		if (playerController != null)
		{
			List<AdvancedSynergyEntry> list2 = new List<AdvancedSynergyEntry>();
			if (flag)
			{
				for (int j = 0; j < playerController.startingGunIds.Count; j++)
				{
					Gun gun = PickupObjectDatabase.GetById(playerController.startingGunIds[j]) as Gun;
					EncounterTrackable component2 = gun.GetComponent<EncounterTrackable>();
					if (component2)
					{
						if (!string.IsNullOrEmpty(component2.EncounterGuid))
						{
							list.Add(EncounterDatabase.GetEntry(component2.EncounterGuid));
						}
					}
				}
			}
			else
			{
				for (int k = 0; k < playerController.ActiveExtraSynergies.Count; k++)
				{
					list2.Add(GameManager.Instance.SynergyManager.synergies[playerController.ActiveExtraSynergies[k]]);
				}
				if (playerController.inventory != null && playerController.inventory.AllGuns != null)
				{
					for (int l = 0; l < playerController.inventory.AllGuns.Count; l++)
					{
						Gun gun2 = playerController.inventory.AllGuns[l];
						if (gun2)
						{
							MimicGunController component3 = gun2.GetComponent<MimicGunController>();
							if (!component3)
							{
								EncounterTrackable component4 = gun2.GetComponent<EncounterTrackable>();
								if (component4 && !component4.SuppressInInventory)
								{
									if (!string.IsNullOrEmpty(component4.EncounterGuid))
									{
										list.Add(EncounterDatabase.GetEntry(component4.EncounterGuid));
									}
								}
							}
						}
					}
				}
			}
			dfPanel dfPanel = component.transform.GetChild(0).GetComponent<dfPanel>();
			base.StartCoroutine(this.ConstructRectanglePageLayout(dfPanel, list, new Vector2(12f, 16f), new Vector2(8f, 8f), true, list2));
			dfPanel.Anchor = dfAnchorStyle.Top | dfAnchorStyle.Bottom | dfAnchorStyle.CenterHorizontal;
			component.Height = Mathf.Max(100f, dfPanel.Height);
			dfPanel.Height = component.Height;
			List<PlayerItem> list3 = playerController.activeItems;
			if (flag)
			{
				list3 = new List<PlayerItem>(playerController.startingActiveItemIds.Count);
				for (int m = 0; m < playerController.startingActiveItemIds.Count; m++)
				{
					list3.Add(PickupObjectDatabase.GetById(playerController.startingActiveItemIds[m]) as PlayerItem);
				}
			}
			if (list3 != null && list3.Count > 0)
			{
				dfPanel component5 = transform.Find("Active Items Panel").GetComponent<dfPanel>();
				list.Clear();
				for (int n = 0; n < list3.Count; n++)
				{
					PlayerItem playerItem = list3[n];
					if (playerItem)
					{
						EncounterTrackable component6 = playerItem.GetComponent<EncounterTrackable>();
						if (component6 && !component6.SuppressInInventory)
						{
							list.Add(EncounterDatabase.GetEntry(component6.EncounterGuid));
						}
					}
				}
				dfPanel = component5.transform.GetChild(0).GetComponent<dfPanel>();
				base.StartCoroutine(this.ConstructRectanglePageLayout(dfPanel, list, new Vector2(12f, 16f), new Vector2(8f, 8f), true, list2));
				dfPanel.Anchor = dfAnchorStyle.Top | dfAnchorStyle.Bottom | dfAnchorStyle.CenterHorizontal;
				component5.Height = Mathf.Max(100f, dfPanel.Height);
				dfPanel.Height = component5.Height;
			}
			List<PassiveItem> list4 = playerController.passiveItems;
			if (flag)
			{
				list4 = new List<PassiveItem>(playerController.startingPassiveItemIds.Count);
				for (int num = 0; num < playerController.startingPassiveItemIds.Count; num++)
				{
					list4.Add(PickupObjectDatabase.GetById(playerController.startingPassiveItemIds[num]) as PassiveItem);
				}
			}
			if (list4 != null && list4.Count > 0)
			{
				dfPanel component7 = transform.Find("Passive Items Panel").GetComponent<dfPanel>();
				list.Clear();
				for (int num2 = 0; num2 < list4.Count; num2++)
				{
					PassiveItem passiveItem = list4[num2];
					if (passiveItem)
					{
						EncounterTrackable component8 = passiveItem.GetComponent<EncounterTrackable>();
						if (component8 && !component8.SuppressInInventory)
						{
							list.Add(EncounterDatabase.GetEntry(component8.EncounterGuid));
						}
					}
				}
				dfPanel = component7.transform.GetChild(0).GetComponent<dfPanel>();
				base.StartCoroutine(this.ConstructRectanglePageLayout(dfPanel, list, new Vector2(12f, 16f), new Vector2(8f, 8f), true, list2));
				dfPanel.Anchor = dfAnchorStyle.Top | dfAnchorStyle.Bottom | dfAnchorStyle.CenterHorizontal;
				component7.Height = Mathf.Max(100f, dfPanel.Height);
				dfPanel.Height = component7.Height;
			}
		}
	}

	// Token: 0x060079D0 RID: 31184 RVA: 0x0030DB7C File Offset: 0x0030BD7C
	public T AddSpriteToPage<T>(tk2dSpriteCollectionData collection, int spriteID) where T : tk2dBaseSprite
	{
		GameObject gameObject = new GameObject("ammonomicon tk2d sprite");
		gameObject.transform.parent = base.transform.parent;
		T t = tk2dBaseSprite.AddComponent<T>(gameObject, collection, spriteID);
		Bounds untrimmedBounds = t.GetUntrimmedBounds();
		Vector2 vector = GameUIUtility.TK2DtoDF(untrimmedBounds.size.XY(), this.guiManager.PixelsToUnits());
		t.scale = new Vector3(vector.x / untrimmedBounds.size.x, vector.y / untrimmedBounds.size.y, untrimmedBounds.size.z);
		t.ignoresTiltworldDepth = true;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.layer = this.guiManager.gameObject.layer;
		return t;
	}

	// Token: 0x060079D1 RID: 31185 RVA: 0x0030DC64 File Offset: 0x0030BE64
	public tk2dSprite AddSpriteToPage(tk2dBaseSprite sourceSprite)
	{
		return this.AddSpriteToPage<tk2dSprite>(sourceSprite.Collection, sourceSprite.spriteId);
	}

	// Token: 0x060079D2 RID: 31186 RVA: 0x0030DC78 File Offset: 0x0030BE78
	public void SetMatrix(Matrix4x4 matrix)
	{
		this.renderMaterial.SetVector(this.topBezierPropID, matrix.GetRow(0));
		this.renderMaterial.SetVector(this.leftBezierPropID, matrix.GetRow(1));
		this.renderMaterial.SetVector(this.bottomBezierPropID, matrix.GetRow(2));
		this.renderMaterial.SetVector(this.rightBezierPropID, matrix.GetRow(3));
	}

	// Token: 0x060079D3 RID: 31187 RVA: 0x0030DCEC File Offset: 0x0030BEEC
	public void EnableRendering()
	{
		if (this.m_renderBuffer != null)
		{
			this.m_renderBuffer.DiscardContents();
		}
		this.guiManager.gameObject.SetActive(true);
		this.m_camera.depth = this.m_camera.depth + 1f;
		this.m_camera.enabled = true;
		this.targetRenderer.enabled = true;
		this.guiManager.GetComponent<dfInputManager>().enabled = true;
		Color backgroundColor = this.m_camera.backgroundColor;
		CameraClearFlags clearFlags = this.m_camera.clearFlags;
		this.m_camera.clearFlags = CameraClearFlags.Color;
		this.m_camera.backgroundColor = Color.black;
		this.m_camera.Render();
		this.m_camera.clearFlags = clearFlags;
		this.m_camera.backgroundColor = backgroundColor;
	}

	// Token: 0x060079D4 RID: 31188 RVA: 0x0030DDC4 File Offset: 0x0030BFC4
	public void Disable(bool isPrecache = false)
	{
		this.m_camera.depth = this.m_camera.depth - 1f;
		this.m_camera.enabled = false;
		this.targetRenderer.enabled = false;
		this.guiManager.GetComponent<dfInputManager>().enabled = false;
		if (isPrecache)
		{
			base.StartCoroutine(this.HandleFrameDelayedInactivation());
		}
		else
		{
			this.guiManager.gameObject.SetActive(false);
		}
	}

	// Token: 0x060079D5 RID: 31189 RVA: 0x0030DE40 File Offset: 0x0030C040
	private IEnumerator HandleFrameDelayedInactivation()
	{
		yield return null;
		this.guiManager.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x060079D6 RID: 31190 RVA: 0x0030DE5C File Offset: 0x0030C05C
	public void Dispose()
	{
		if (this.m_camera)
		{
			this.m_camera.RemoveAllCommandBuffers();
		}
		if (this.m_renderBuffer != null)
		{
			RenderTexture.ReleaseTemporary(this.m_renderBuffer);
			this.m_renderBuffer = null;
		}
		if (this.targetRenderer)
		{
			UnityEngine.Object.Destroy(this.targetRenderer.gameObject);
		}
		if (this.guiManager)
		{
			UnityEngine.Object.Destroy(this.guiManager.gameObject);
		}
	}

	// Token: 0x060079D7 RID: 31191 RVA: 0x0030DEE8 File Offset: 0x0030C0E8
	private void OnDestroy()
	{
		this.Dispose();
	}

	// Token: 0x04007C2A RID: 31786
	public AmmonomiconPageRenderer.PageType pageType;

	// Token: 0x04007C2B RID: 31787
	public MeshRenderer targetRenderer;

	// Token: 0x04007C2C RID: 31788
	public dfGUIManager guiManager;

	// Token: 0x04007C2D RID: 31789
	public int offsetIndex = -1;

	// Token: 0x04007C2E RID: 31790
	public dfSprite HeaderBGSprite;

	// Token: 0x04007C2F RID: 31791
	private IAmmonomiconFocusable m_lastFocusTarget;

	// Token: 0x04007C30 RID: 31792
	[NonSerialized]
	public dfControl PrimaryClipPanel;

	// Token: 0x04007C31 RID: 31793
	private Camera m_camera;

	// Token: 0x04007C32 RID: 31794
	private Material renderMaterial;

	// Token: 0x04007C33 RID: 31795
	private int topBezierPropID;

	// Token: 0x04007C34 RID: 31796
	private int leftBezierPropID;

	// Token: 0x04007C35 RID: 31797
	private int rightBezierPropID;

	// Token: 0x04007C36 RID: 31798
	private int bottomBezierPropID;

	// Token: 0x04007C37 RID: 31799
	private List<AmmonomiconPokedexEntry> m_pokedexEntries = new List<AmmonomiconPokedexEntry>();

	// Token: 0x04007C38 RID: 31800
	private RenderTexture m_renderBuffer;

	// Token: 0x04007C39 RID: 31801
	private dfFontBase EnglishFont;

	// Token: 0x04007C3A RID: 31802
	private dfFontBase OtherLanguageFont;

	// Token: 0x04007C3B RID: 31803
	private dfFontBase BaseAlagardFont;

	// Token: 0x04007C3C RID: 31804
	private dfFontBase OtherAlagardFont;

	// Token: 0x04007C3D RID: 31805
	private float? OriginalHeaderRelativeY;

	// Token: 0x04007C3E RID: 31806
	private bool m_hasAdjustedForChinese;

	// Token: 0x04007C3F RID: 31807
	private StringTableManager.GungeonSupportedLanguages m_cachedLanguage;

	// Token: 0x04007C40 RID: 31808
	private List<dfButton> m_prevLineButtons;

	// Token: 0x020014E6 RID: 5350
	public enum PageType
	{
		// Token: 0x04007C46 RID: 31814
		NONE,
		// Token: 0x04007C47 RID: 31815
		EQUIPMENT_LEFT,
		// Token: 0x04007C48 RID: 31816
		EQUIPMENT_RIGHT,
		// Token: 0x04007C49 RID: 31817
		GUNS_LEFT,
		// Token: 0x04007C4A RID: 31818
		GUNS_RIGHT,
		// Token: 0x04007C4B RID: 31819
		ITEMS_LEFT,
		// Token: 0x04007C4C RID: 31820
		ITEMS_RIGHT,
		// Token: 0x04007C4D RID: 31821
		ENEMIES_LEFT,
		// Token: 0x04007C4E RID: 31822
		ENEMIES_RIGHT,
		// Token: 0x04007C4F RID: 31823
		BOSSES_LEFT,
		// Token: 0x04007C50 RID: 31824
		BOSSES_RIGHT,
		// Token: 0x04007C51 RID: 31825
		DEATH_LEFT,
		// Token: 0x04007C52 RID: 31826
		DEATH_RIGHT
	}

	// Token: 0x020014E7 RID: 5351
	private struct RectangleLineInfo
	{
		// Token: 0x04007C53 RID: 31827
		public int numberOfElements;

		// Token: 0x04007C54 RID: 31828
		public float lineHeightUnits;

		// Token: 0x04007C55 RID: 31829
		public float initialXOffset;
	}
}
