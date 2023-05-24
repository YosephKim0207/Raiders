using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200173B RID: 5947
public class AmmonomiconPokedexEntry : MonoBehaviour
{
	// Token: 0x1700149A RID: 5274
	// (get) Token: 0x06008A54 RID: 35412 RVA: 0x00399580 File Offset: 0x00397780
	public tk2dClippedSprite ChildSprite
	{
		get
		{
			return this.m_childSprite;
		}
	}

	// Token: 0x06008A55 RID: 35413 RVA: 0x00399588 File Offset: 0x00397788
	private void Awake()
	{
		this.m_button = base.GetComponent<dfButton>();
		this.m_bgSprite = base.GetComponentInChildren<dfSlicedSprite>();
		this.m_button.PrecludeUpdateCycle = true;
		this.m_bgSprite.PrecludeUpdateCycle = true;
		this.questionMarkSprite.PrecludeUpdateCycle = true;
		this.m_button.MouseHover += this.m_button_MouseHover;
		this.m_button.Click += this.m_button_Click;
		this.m_button.LostFocus += this.m_button_LostFocus;
		this.m_button.GotFocus += this.m_button_GotFocus;
		this.m_button.ControlClippingChanged += this.m_button_ControlClippingChanged;
	}

	// Token: 0x06008A56 RID: 35414 RVA: 0x00399644 File Offset: 0x00397844
	private void m_button_ControlClippingChanged(dfControl control, bool value)
	{
		if (this.encounterState != AmmonomiconPokedexEntry.EncounterState.UNKNOWN)
		{
			this.m_childSprite.renderer.enabled = !value;
			if (this.IsGunderfury)
			{
				string text = "gunderfury_LV" + (GunderfuryController.GetCurrentTier() + 1) + "0_idle_001";
				int spriteIdByName = this.m_childSprite.Collection.GetSpriteIdByName(text, -1);
				if (spriteIdByName != this.m_childSprite.spriteId)
				{
					this.m_childSprite.SetSprite(spriteIdByName);
					this.m_childSprite.PlaceAtLocalPositionByAnchor(Vector3.zero, tk2dBaseSprite.Anchor.MiddleCenter);
				}
			}
			this.UpdateClipping(this.m_childSprite);
			for (int i = 0; i < this.extantSynergyArrows.Count; i++)
			{
				this.UpdateClipping(this.extantSynergyArrows[i]);
			}
		}
	}

	// Token: 0x06008A57 RID: 35415 RVA: 0x00399714 File Offset: 0x00397914
	private void UpdateClipping(tk2dClippedSprite targetSprite)
	{
		if (!GameManager.Instance.IsLoadingLevel && this.m_button.IsVisible)
		{
			Vector3[] corners = this.m_button.Parent.Parent.Parent.GetCorners();
			float x = corners[0].x;
			float y = corners[0].y;
			float x2 = corners[3].x;
			float y2 = corners[3].y;
			Bounds bounds = targetSprite.GetUntrimmedBounds();
			bounds.center += targetSprite.transform.position;
			float num = Mathf.Clamp01((x - bounds.min.x) / bounds.size.x);
			float num2 = Mathf.Clamp01((y2 - bounds.min.y) / bounds.size.y);
			float num3 = Mathf.Clamp01((x2 - bounds.min.x) / bounds.size.x);
			float num4 = Mathf.Clamp01((y - bounds.min.y) / bounds.size.y);
			targetSprite.clipBottomLeft = new Vector2(num, num2);
			targetSprite.clipTopRight = new Vector2(num3, num4);
			if (SpriteOutlineManager.HasOutline(targetSprite))
			{
				tk2dClippedSprite[] outlineSprites = SpriteOutlineManager.GetOutlineSprites<tk2dClippedSprite>(targetSprite);
				for (int i = 0; i < outlineSprites.Length; i++)
				{
					bounds = outlineSprites[i].GetUntrimmedBounds();
					bounds.center += outlineSprites[i].transform.position;
					num = Mathf.Clamp01((x - bounds.min.x) / bounds.size.x);
					num2 = Mathf.Clamp01((y2 - bounds.min.y) / bounds.size.y);
					num3 = Mathf.Clamp01((x2 - bounds.min.x) / bounds.size.x);
					num4 = Mathf.Clamp01((y - bounds.min.y) / bounds.size.y);
					outlineSprites[i].clipBottomLeft = new Vector2(num, num2);
					outlineSprites[i].clipTopRight = new Vector2(num3, num4);
				}
			}
		}
	}

	// Token: 0x06008A58 RID: 35416 RVA: 0x003999A8 File Offset: 0x00397BA8
	private void LateUpdate()
	{
		this.UpdateClipping(this.m_childSprite);
		for (int i = 0; i < this.extantSynergyArrows.Count; i++)
		{
			this.UpdateClipping(this.extantSynergyArrows[i]);
		}
	}

	// Token: 0x06008A59 RID: 35417 RVA: 0x003999F0 File Offset: 0x00397BF0
	public void UpdateEncounterState()
	{
		if (GameStatsManager.Instance.QueryEncounterable(this.linkedEncounterTrackable) == 0)
		{
			if (this.linkedEncounterTrackable.PrerequisitesMet() && !this.linkedEncounterTrackable.journalData.SuppressKnownState && !this.linkedEncounterTrackable.journalData.IsEnemy)
			{
				this.SetEncounterState(AmmonomiconPokedexEntry.EncounterState.KNOWN);
			}
			else
			{
				this.SetEncounterState(AmmonomiconPokedexEntry.EncounterState.UNKNOWN);
			}
		}
		else if (this.linkedEncounterTrackable.PrerequisitesMet())
		{
			this.SetEncounterState(AmmonomiconPokedexEntry.EncounterState.ENCOUNTERED);
		}
		else
		{
			this.SetEncounterState(AmmonomiconPokedexEntry.EncounterState.UNKNOWN);
		}
	}

	// Token: 0x06008A5A RID: 35418 RVA: 0x00399A88 File Offset: 0x00397C88
	public void ForceFocus()
	{
		this.m_button.Focus(true);
	}

	// Token: 0x06008A5B RID: 35419 RVA: 0x00399A98 File Offset: 0x00397C98
	public void SetEncounterState(AmmonomiconPokedexEntry.EncounterState st)
	{
		if (this.IsEquipmentPage)
		{
			return;
		}
		if (!this.ForceEncounterState)
		{
			this.encounterState = st;
		}
		switch (this.encounterState)
		{
		case AmmonomiconPokedexEntry.EncounterState.ENCOUNTERED:
			this.m_childSprite.usesOverrideMaterial = true;
			this.m_childSprite.renderer.material.shader = ShaderCache.Acquire("Brave/AmmonomiconSpriteListShader");
			this.m_childSprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
			this.m_childSprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");
			this.m_childSprite.renderer.material.SetFloat("_SpriteScale", this.m_childSprite.scale.x);
			this.m_childSprite.renderer.material.SetFloat("_Saturation", 1f);
			this.m_childSprite.renderer.material.SetColor("_OverrideColor", new Color(0.4f, 0.4f, 0.4f, 0f));
			this.m_childSprite.renderer.enabled = true;
			this.questionMarkSprite.IsVisible = false;
			break;
		case AmmonomiconPokedexEntry.EncounterState.KNOWN:
			this.m_childSprite.usesOverrideMaterial = true;
			this.m_childSprite.renderer.material.shader = ShaderCache.Acquire("Brave/AmmonomiconSpriteListShader");
			this.m_childSprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
			this.m_childSprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");
			this.m_childSprite.renderer.material.SetFloat("_SpriteScale", this.m_childSprite.scale.x);
			this.m_childSprite.renderer.material.SetFloat("_Saturation", 0f);
			this.m_childSprite.renderer.material.SetColor("_OverrideColor", new Color(0.4f, 0.4f, 0.4f, 0f));
			this.m_childSprite.renderer.enabled = true;
			this.questionMarkSprite.IsVisible = false;
			break;
		case AmmonomiconPokedexEntry.EncounterState.UNKNOWN:
			this.m_childSprite.renderer.enabled = false;
			this.questionMarkSprite.IsVisible = true;
			break;
		}
	}

	// Token: 0x06008A5C RID: 35420 RVA: 0x00399D08 File Offset: 0x00397F08
	private void m_button_GotFocus(dfControl control, dfFocusEventArgs args)
	{
		AkSoundEngine.PostEvent("Play_UI_menu_select_01", base.gameObject);
		if (SpriteOutlineManager.HasOutline(this.m_childSprite))
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(this.m_childSprite, true);
			SpriteOutlineManager.AddScaledOutlineToSprite<tk2dClippedSprite>(this.m_childSprite, Color.white, 0.1f, 0f);
		}
		this.m_bgSprite.SpriteName = "big_box_page_raised_selected_001";
		AmmonomiconController.Instance.BestInteractingLeftPageRenderer.LastFocusTarget = this.m_button;
		if (this.encounterState == AmmonomiconPokedexEntry.EncounterState.ENCOUNTERED)
		{
			AmmonomiconController.Instance.BestInteractingRightPageRenderer.SetRightDataPageTexts(this.m_childSprite, this.linkedEncounterTrackable);
		}
		else if (this.encounterState == AmmonomiconPokedexEntry.EncounterState.KNOWN)
		{
			AmmonomiconController.Instance.BestInteractingRightPageRenderer.SetRightDataPageUnknown(false);
			AmmonomiconController.Instance.BestInteractingRightPageRenderer.SetRightDataPageName(this.m_childSprite, this.linkedEncounterTrackable);
		}
		else
		{
			AmmonomiconController.Instance.BestInteractingRightPageRenderer.SetRightDataPageUnknown(false);
		}
		if (AmmonomiconController.Instance.BestInteractingLeftPageRenderer.pageType == AmmonomiconPageRenderer.PageType.EQUIPMENT_LEFT)
		{
			this.UpdateSynergyHighlights();
		}
	}

	// Token: 0x06008A5D RID: 35421 RVA: 0x00399E14 File Offset: 0x00398014
	private void UpdateSynergyHighlights()
	{
		if (GameManager.Instance.IsSelectingCharacter)
		{
			return;
		}
		List<AmmonomiconPokedexEntry> pokedexEntries = AmmonomiconController.Instance.BestInteractingLeftPageRenderer.GetPokedexEntries();
		List<AmmonomiconPokedexEntry> list = new List<AmmonomiconPokedexEntry>();
		if (this.activeSynergies != null)
		{
			for (int i = 0; i < this.activeSynergies.Count; i++)
			{
				PlayerController playerController = GameManager.Instance.BestActivePlayer;
				for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
				{
					if (GameManager.Instance.AllPlayers[j].PlayerIDX == GameManager.Instance.LastPausingPlayerID)
					{
						playerController = GameManager.Instance.AllPlayers[j];
					}
				}
				AdvancedSynergyEntry advancedSynergyEntry = this.activeSynergies[i];
				if (advancedSynergyEntry.ContainsPickup(this.pickupID) && pokedexEntries != null)
				{
					for (int k = 0; k < pokedexEntries.Count; k++)
					{
						if (pokedexEntries[k].pickupID >= 0 && pokedexEntries[k].pickupID != this.pickupID)
						{
							if (advancedSynergyEntry.ContainsPickup(pokedexEntries[k].pickupID))
							{
								tk2dClippedSprite tk2dClippedSprite = AmmonomiconController.Instance.CurrentLeftPageRenderer.AddSpriteToPage<tk2dClippedSprite>(AmmonomiconController.Instance.EncounterIconCollection, AmmonomiconController.Instance.EncounterIconCollection.GetSpriteIdByName("synergy_ammonomicon_arrow_001"));
								tk2dClippedSprite.SetSprite("synergy_ammonomicon_arrow_001");
								Bounds bounds = pokedexEntries[k].m_childSprite.GetBounds();
								Bounds untrimmedBounds = pokedexEntries[k].m_childSprite.GetUntrimmedBounds();
								Vector3 size = bounds.size;
								tk2dClippedSprite.transform.position = (pokedexEntries[k].m_childSprite.WorldCenter.ToVector3ZisY(0f) + new Vector3(-8f * pokedexEntries[k].m_bgSprite.PixelsToUnits(), size.y / 2f + 32f * pokedexEntries[k].m_bgSprite.PixelsToUnits(), 0f)).WithZ(-0.65f);
								tk2dClippedSprite.transform.parent = this.m_childSprite.transform.parent;
								this.extantSynergyArrows.Add(tk2dClippedSprite);
								pokedexEntries[k].ChangeOutlineColor(SynergyDatabase.SynergyBlue);
								list.Add(pokedexEntries[k]);
							}
						}
					}
				}
			}
			if (pokedexEntries != null)
			{
				for (int l = 0; l < pokedexEntries.Count; l++)
				{
					if (pokedexEntries[l] != this && !list.Contains(pokedexEntries[l]) && SpriteOutlineManager.HasOutline(pokedexEntries[l].m_childSprite))
					{
						SpriteOutlineManager.RemoveOutlineFromSprite(pokedexEntries[l].m_childSprite, true);
						SpriteOutlineManager.AddScaledOutlineToSprite<tk2dClippedSprite>(pokedexEntries[l].m_childSprite, Color.black, 0.1f, 0.05f);
					}
				}
			}
		}
	}

	// Token: 0x06008A5E RID: 35422 RVA: 0x0039A11C File Offset: 0x0039831C
	private void m_button_LostFocus(dfControl control, dfFocusEventArgs args)
	{
		for (int i = 0; i < this.extantSynergyArrows.Count; i++)
		{
			UnityEngine.Object.Destroy(this.extantSynergyArrows[i].gameObject);
		}
		this.extantSynergyArrows.Clear();
		if (SpriteOutlineManager.HasOutline(this.m_childSprite))
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(this.m_childSprite, true);
			SpriteOutlineManager.AddScaledOutlineToSprite<tk2dClippedSprite>(this.m_childSprite, Color.black, 0.1f, 0.05f);
		}
		this.m_bgSprite.SpriteName = "big_box_page_flat_001";
	}

	// Token: 0x06008A5F RID: 35423 RVA: 0x0039A1AC File Offset: 0x003983AC
	private void m_button_Click(dfControl control, dfMouseEventArgs mouseEvent)
	{
		this.m_button.Focus(true);
	}

	// Token: 0x06008A60 RID: 35424 RVA: 0x0039A1BC File Offset: 0x003983BC
	private void m_button_MouseHover(dfControl control, dfMouseEventArgs mouseEvent)
	{
	}

	// Token: 0x06008A61 RID: 35425 RVA: 0x0039A1C0 File Offset: 0x003983C0
	public void ChangeOutlineColor(Color targetColor)
	{
		SpriteOutlineManager.RemoveOutlineFromSprite(this.m_childSprite, true);
		SpriteOutlineManager.AddScaledOutlineToSprite<tk2dClippedSprite>(this.m_childSprite, targetColor, 0.1f, 0f);
	}

	// Token: 0x06008A62 RID: 35426 RVA: 0x0039A1E4 File Offset: 0x003983E4
	public void AssignSprite(tk2dClippedSprite sprit)
	{
		this.m_childSprite = sprit;
		this.m_childSprite.ignoresTiltworldDepth = true;
		this.m_childSprite.transform.position += new Vector3(0f, 0f, -0.5f);
		if (this.encounterState != AmmonomiconPokedexEntry.EncounterState.UNKNOWN)
		{
			this.m_childSprite.renderer.enabled = !this.m_button.IsControlClipped;
		}
	}

	// Token: 0x040090CD RID: 37069
	public bool IsEquipmentPage;

	// Token: 0x040090CE RID: 37070
	public bool ForceEncounterState;

	// Token: 0x040090CF RID: 37071
	public AmmonomiconPokedexEntry.EncounterState encounterState;

	// Token: 0x040090D0 RID: 37072
	public EncounterDatabaseEntry linkedEncounterTrackable;

	// Token: 0x040090D1 RID: 37073
	public int pickupID = -1;

	// Token: 0x040090D2 RID: 37074
	public dfSprite questionMarkSprite;

	// Token: 0x040090D3 RID: 37075
	public List<AdvancedSynergyEntry> activeSynergies;

	// Token: 0x040090D4 RID: 37076
	private tk2dClippedSprite m_childSprite;

	// Token: 0x040090D5 RID: 37077
	private dfButton m_button;

	// Token: 0x040090D6 RID: 37078
	private dfSlicedSprite m_bgSprite;

	// Token: 0x040090D7 RID: 37079
	[NonSerialized]
	public bool IsGunderfury;

	// Token: 0x040090D8 RID: 37080
	private const string c_flatSprite = "big_box_page_flat_001";

	// Token: 0x040090D9 RID: 37081
	private const string c_raisedSprite = "big_box_page_raised_001";

	// Token: 0x040090DA RID: 37082
	private const string c_raisedSelectedSprite = "big_box_page_raised_selected_001";

	// Token: 0x040090DB RID: 37083
	private dfInputManager m_inputAdapter;

	// Token: 0x040090DC RID: 37084
	private List<tk2dClippedSprite> extantSynergyArrows = new List<tk2dClippedSprite>();

	// Token: 0x0200173C RID: 5948
	public enum EncounterState
	{
		// Token: 0x040090DE RID: 37086
		ENCOUNTERED,
		// Token: 0x040090DF RID: 37087
		KNOWN,
		// Token: 0x040090E0 RID: 37088
		UNKNOWN
	}
}
