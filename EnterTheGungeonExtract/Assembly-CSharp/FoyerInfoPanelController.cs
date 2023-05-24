using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001774 RID: 6004
public class FoyerInfoPanelController : MonoBehaviour
{
	// Token: 0x06008BD7 RID: 35799 RVA: 0x003A3D10 File Offset: 0x003A1F10
	private void SetBadgeVisibility()
	{
	}

	// Token: 0x06008BD8 RID: 35800 RVA: 0x003A3D14 File Offset: 0x003A1F14
	private void ProcessSprite(dfSprite targetSprite, bool playerHas, bool anyHas)
	{
		if (playerHas)
		{
			targetSprite.IsVisible = true;
			targetSprite.Color = Color.white;
		}
		else if (anyHas)
		{
			targetSprite.IsVisible = true;
			targetSprite.Color = new Color(0.35f, 0f, 0f);
		}
		else
		{
			targetSprite.IsVisible = false;
		}
	}

	// Token: 0x06008BD9 RID: 35801 RVA: 0x003A3D7C File Offset: 0x003A1F7C
	private bool AnyPlayerElement(int elementIndex)
	{
		return false;
	}

	// Token: 0x06008BDA RID: 35802 RVA: 0x003A3D80 File Offset: 0x003A1F80
	private IEnumerator Start()
	{
		this.m_panel = base.GetComponent<dfPanel>();
		tk2dBaseSprite[] allSprites = base.GetComponentsInChildren<tk2dBaseSprite>();
		this.arrow.GetComponent<Renderer>().enabled = false;
		this.arrow.gameObject.layer = LayerMask.NameToLayer("BG_Critical");
		CharacterSelectFacecardIdleDoer componentInChildren = this.arrow.GetComponentInChildren<CharacterSelectFacecardIdleDoer>();
		componentInChildren.transform.localPosition = componentInChildren.transform.localPosition.WithY(0f);
		for (int i = 0; i < allSprites.Length; i++)
		{
			allSprites[i].ignoresTiltworldDepth = true;
			allSprites[i].transform.position = allSprites[i].transform.position.WithZ(0f);
		}
		for (int j = 0; j < this.scaledSprites.Length; j++)
		{
			this.scaledSprites[j].transform.localScale = GameUIUtility.GetCurrentTK2D_DFScale(this.m_panel.GetManager()) * Vector3.one;
		}
		base.transform.position = dfFollowObject.ConvertWorldSpaces(this.followTransform.position + this.offset + this.AdditionalDaveOffset, GameManager.Instance.MainCameraController.Camera, this.m_panel.GUIManager.RenderCamera).WithZ(0f);
		base.transform.position = base.transform.position.Quantize(3f * this.m_panel.PixelsToUnits());
		if (FoyerInfoPanelController.m_extantPanelController != null)
		{
			if (GameManager.Instance.IsSelectingCharacter)
			{
				yield return base.StartCoroutine(this.HandleTransition());
			}
			else
			{
				UnityEngine.Object.Destroy(FoyerInfoPanelController.m_extantPanelController.gameObject);
			}
		}
		FoyerInfoPanelController.m_extantPanelController = this;
		yield return null;
		base.StartCoroutine(this.HandleOpen());
		yield break;
	}

	// Token: 0x06008BDB RID: 35803 RVA: 0x003A3D9C File Offset: 0x003A1F9C
	private IEnumerator HandleTransition()
	{
		FoyerInfoPanelController.IsTransitioning = true;
		this.arrow.gameObject.SetActive(false);
		dfPanel currentTextPanel = FoyerInfoPanelController.m_extantPanelController.m_panel;
		Vector3 initialPosition = FoyerInfoPanelController.m_extantPanelController.arrow.transform.position;
		Vector3 targetPosition = this.arrow.transform.position;
		float elapsed = 0f;
		float duration = 0.15f;
		currentTextPanel.IsVisible = false;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			Vector3 currentPosition = Vector3.Lerp(initialPosition, targetPosition, t);
			FoyerInfoPanelController.m_extantPanelController.arrow.transform.position = currentPosition;
			currentTextPanel.IsVisible = false;
			yield return null;
		}
		int targetReticleFrame = FoyerInfoPanelController.m_extantPanelController.arrow.GetComponent<tk2dSpriteAnimator>().CurrentFrame + 1;
		FoyerInfoPanelController.m_extantPanelController.arrow.gameObject.SetActive(false);
		FoyerInfoPanelController.m_extantPanelController.arrow.transform.position = initialPosition;
		currentTextPanel.IsVisible = false;
		this.arrow.gameObject.SetActive(true);
		this.arrow.GetComponent<tk2dSpriteAnimator>().Play();
		this.arrow.GetComponent<tk2dSpriteAnimator>().SetFrame(targetReticleFrame);
		UnityEngine.Object.Destroy(FoyerInfoPanelController.m_extantPanelController.gameObject);
		FoyerInfoPanelController.IsTransitioning = false;
		yield break;
	}

	// Token: 0x06008BDC RID: 35804 RVA: 0x003A3DB8 File Offset: 0x003A1FB8
	private void Update()
	{
		if (GameManager.Instance.IsPaused && this.arrow && this.arrow.transform.childCount > 0)
		{
			MeshRenderer component = this.arrow.transform.GetChild(0).GetComponent<MeshRenderer>();
			if (component)
			{
				component.enabled = false;
			}
		}
		for (int i = 0; i < this.scaledSprites.Length; i++)
		{
			this.scaledSprites[i].transform.localScale = GameUIUtility.GetCurrentTK2D_DFScale(this.m_panel.GetManager()) * Vector3.one;
		}
	}

	// Token: 0x06008BDD RID: 35805 RVA: 0x003A3E68 File Offset: 0x003A2068
	private void OnDestroy()
	{
		if (FoyerInfoPanelController.m_extantPanelController == this)
		{
			FoyerInfoPanelController.m_extantPanelController = null;
		}
	}

	// Token: 0x06008BDE RID: 35806 RVA: 0x003A3E80 File Offset: 0x003A2080
	private void LateUpdate()
	{
		base.transform.position = dfFollowObject.ConvertWorldSpaces(this.followTransform.position + this.offset + this.AdditionalDaveOffset, GameManager.Instance.MainCameraController.Camera, this.m_panel.GUIManager.RenderCamera).WithZ(0f);
		base.transform.position = base.transform.position.QuantizeFloor(this.m_panel.PixelsToUnits() / (Pixelator.Instance.ScaleTileScale / Pixelator.Instance.CurrentTileScale));
	}

	// Token: 0x06008BDF RID: 35807 RVA: 0x003A3F24 File Offset: 0x003A2124
	private IEnumerator HandleOpen()
	{
		float elapsed = 0f;
		float duration = 0.7f;
		this.textPanel.Width = 1f;
		this.textPanel.IsVisible = true;
		this.textPanel.ResolutionChangedPostLayout = null;
		dfPanel dfPanel = this.textPanel;
		dfPanel.ResolutionChangedPostLayout = (Action<dfControl, Vector3, Vector3>)Delegate.Combine(dfPanel.ResolutionChangedPostLayout, new Action<dfControl, Vector3, Vector3>(this.ResolutionChangedPanel));
		yield return new WaitForSeconds(0.45f);
		this.ResolutionChangedPanel(this.textPanel, Vector3.zero, Vector3.zero);
		if (this.characterIdentity == PlayableCharacters.Eevee)
		{
			dfLabel component = this.textPanel.transform.Find("PastKilledLabel").GetComponent<dfLabel>();
			component.ProcessMarkup = true;
			component.ColorizeSymbols = true;
			component.ModifyLocalizedText(component.Text + " (" + 5.ToString() + "[sprite \"hbux_text_icon\"])");
		}
		else if (this.characterIdentity == PlayableCharacters.Gunslinger)
		{
			dfLabel component2 = this.textPanel.transform.Find("PastKilledLabel").GetComponent<dfLabel>();
			component2.ProcessMarkup = true;
			component2.ColorizeSymbols = true;
			component2.ModifyLocalizedText(component2.Text + " (" + 7.ToString() + "[sprite \"hbux_text_icon\"])");
		}
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			this.textPanel.Width = (float)((int)Mathf.Lerp(1f, 850f, t));
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008BE0 RID: 35808 RVA: 0x003A3F40 File Offset: 0x003A2140
	private void ResolutionChangedPanel(dfControl newTextPanel, Vector3 previousRelativePosition, Vector3 newRelativePosition)
	{
		dfLabel component = newTextPanel.transform.Find("NameLabel").GetComponent<dfLabel>();
		dfLabel component2 = newTextPanel.transform.Find("DescLabel").GetComponent<dfLabel>();
		dfLabel component3 = newTextPanel.transform.Find("GunLabel").GetComponent<dfLabel>();
		dfLabel component4 = newTextPanel.transform.Find("PastKilledLabel").GetComponent<dfLabel>();
		if (this.characterIdentity == PlayableCharacters.Eevee || this.characterIdentity == PlayableCharacters.Gunslinger || GameStatsManager.Instance.TestPastBeaten(this.characterIdentity))
		{
			component4.IsVisible = true;
		}
		else
		{
			component4.IsVisible = false;
		}
		float currentTileScale = Pixelator.Instance.CurrentTileScale;
		int num = Mathf.FloorToInt(currentTileScale);
		tk2dBaseSprite sprite = newTextPanel.Parent.GetComponentsInChildren<CharacterSelectFacecardIdleDoer>(true)[0].sprite;
		newTextPanel.transform.position = sprite.transform.position + new Vector3(18f * currentTileScale * component.PixelsToUnits(), 41f * currentTileScale * component.PixelsToUnits(), 0f);
		if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
		{
			component.Padding = new RectOffset(2 * num, 2 * num, -2 * num, 0 * num);
			component2.Padding = new RectOffset(2 * num, 2 * num, -2 * num, 0 * num);
			component3.Padding = new RectOffset(2 * num, 2 * num, -2 * num, 0 * num);
			component4.Padding = new RectOffset(2 * num, 2 * num, -2 * num, 0 * num);
		}
		else
		{
			component.Padding = new RectOffset(2 * num, 2 * num, 0, 0);
			component2.Padding = new RectOffset(2 * num, 2 * num, 0, 0);
			component3.Padding = new RectOffset(2 * num, 2 * num, 0, 0);
			component4.Padding = new RectOffset(2 * num, 2 * num, 0, 0);
		}
		component.RelativePosition = new Vector3(currentTileScale * 2f, currentTileScale, 0f);
		component2.RelativePosition = new Vector3(0f, currentTileScale + component.Size.y, 0f) + component.RelativePosition;
		component3.RelativePosition = new Vector3(0f, currentTileScale + component2.Size.y, 0f) + component2.RelativePosition;
		component4.RelativePosition = new Vector3(0f, currentTileScale + component3.Size.y, 0f) + component3.RelativePosition;
		if (this.itemsPanel != null)
		{
			this.itemsPanel.RelativePosition = component2.RelativePosition;
			List<dfSprite> list = new List<dfSprite>();
			for (int i = 0; i < this.itemsPanel.Controls.Count; i++)
			{
				this.itemsPanel.Controls[i].RelativePosition = this.itemsPanel.Controls[i].RelativePosition.WithY(((this.itemsPanel.Height - this.itemsPanel.Controls[i].Height) / 2f).Quantize((float)num));
				if (list.Count == 0)
				{
					list.Add(this.itemsPanel.Controls[i] as dfSprite);
				}
				else
				{
					bool flag = false;
					for (int j = 0; j < list.Count; j++)
					{
						if (this.itemsPanel.Controls[i].RelativePosition.x < list[j].RelativePosition.x)
						{
							list.Insert(j, this.itemsPanel.Controls[i] as dfSprite);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						list.Add(this.itemsPanel.Controls[i] as dfSprite);
					}
				}
			}
			this.itemsPanel.CenterChildControls();
			float num2 = 0f;
			for (int k = 0; k < list.Count; k++)
			{
				if (k == 0)
				{
					list[k].RelativePosition = list[k].RelativePosition.WithX((float)(num * 4));
				}
				else
				{
					dfSprite dfSprite = list[k];
					dfSprite.RelativePosition = dfSprite.RelativePosition.WithX(list[k - 1].RelativePosition.x + list[k - 1].Size.x + (float)(num * 4));
				}
				list[k].RelativePosition = list[k].RelativePosition.Quantize((float)num);
				num2 = list[k].RelativePosition.x + list[k].Size.x + (float)(num * 4);
			}
			this.itemsPanel.Width = num2;
			component4.RelativePosition = component.RelativePosition + new Vector3(component.Width + (float)num, 0f, 0f);
			if (!component4.Text.StartsWith("("))
			{
				component4.Text = "(" + component4.Text + ")";
			}
			component4.Color = new Color(0.6f, 0.6f, 0.6f);
		}
	}

	// Token: 0x040092D9 RID: 37593
	public static bool IsTransitioning;

	// Token: 0x040092DA RID: 37594
	private static FoyerInfoPanelController m_extantPanelController;

	// Token: 0x040092DB RID: 37595
	public PlayableCharacters characterIdentity;

	// Token: 0x040092DC RID: 37596
	public tk2dSprite[] scaledSprites;

	// Token: 0x040092DD RID: 37597
	public tk2dSprite arrow;

	// Token: 0x040092DE RID: 37598
	public dfPanel textPanel;

	// Token: 0x040092DF RID: 37599
	public dfPanel itemsPanel;

	// Token: 0x040092E0 RID: 37600
	public Transform followTransform;

	// Token: 0x040092E1 RID: 37601
	public Vector3 offset;

	// Token: 0x040092E2 RID: 37602
	public Vector3 AdditionalDaveOffset;

	// Token: 0x040092E3 RID: 37603
	private dfPanel m_panel;
}
