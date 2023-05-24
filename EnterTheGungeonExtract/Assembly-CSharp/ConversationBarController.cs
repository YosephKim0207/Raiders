using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001754 RID: 5972
public class ConversationBarController : MonoBehaviour
{
	// Token: 0x170014BB RID: 5307
	// (get) Token: 0x06008B03 RID: 35587 RVA: 0x0039E520 File Offset: 0x0039C720
	public bool IsActive
	{
		get
		{
			return this.m_isActive;
		}
	}

	// Token: 0x06008B04 RID: 35588 RVA: 0x0039E528 File Offset: 0x0039C728
	public void HideBar()
	{
		this.m_isActive = false;
		if (this.m_conversationBarSprite == null)
		{
			this.m_conversationBarSprite = base.GetComponent<dfSprite>();
		}
		if (this.m_motionGroup != null)
		{
			GameUIRoot.Instance.MoveNonCoreGroupOnscreen(this.m_conversationBarSprite, true);
		}
		base.StartCoroutine(this.DelayedHide());
	}

	// Token: 0x06008B05 RID: 35589 RVA: 0x0039E588 File Offset: 0x0039C788
	private IEnumerator DelayedHide()
	{
		yield return new WaitForSeconds(0.25f);
		if (!this.m_isActive)
		{
			this.m_conversationBarSprite.IsVisible = false;
			this.textOption1.IsVisible = false;
			this.textOption2.IsVisible = false;
		}
		yield break;
	}

	// Token: 0x06008B06 RID: 35590 RVA: 0x0039E5A4 File Offset: 0x0039C7A4
	public void SetSelectedResponse(int selectedResponse)
	{
		if (selectedResponse == 0)
		{
			this.textOption1.TextColor = this.selectedColor;
			this.textOption2.TextColor = this.unselectedColor;
			this.reticleLeft1.IsVisible = false;
			this.reticleRight1.IsVisible = false;
			this.reticleLeft2.IsVisible = false;
			this.reticleRight2.IsVisible = false;
		}
		else if (selectedResponse == 1)
		{
			this.textOption1.TextColor = this.unselectedColor;
			this.textOption2.TextColor = this.selectedColor;
			this.reticleLeft1.IsVisible = false;
			this.reticleRight1.IsVisible = false;
			this.reticleLeft2.IsVisible = false;
			this.reticleRight2.IsVisible = false;
		}
	}

	// Token: 0x06008B07 RID: 35591 RVA: 0x0039E67C File Offset: 0x0039C87C
	public void LateUpdate()
	{
		if (this.m_temporarilyHidden && !GameManager.Instance.IsPaused)
		{
			GameUIRoot.Instance.MoveNonCoreGroupOnscreen(this.m_conversationBarSprite, false);
			this.m_temporarilyHidden = false;
		}
		if (this.textOption1.IsVisible)
		{
			if (!this.m_temporarilyHidden && GameManager.Instance.IsPaused)
			{
				GameUIRoot.Instance.MoveNonCoreGroupOnscreen(this.m_conversationBarSprite, true);
				this.m_temporarilyHidden = true;
			}
			else if (BraveInput.GetInstanceForPlayer(this.m_lastAssignedPlayer.PlayerIDX).IsKeyboardAndMouse(false))
			{
				Vector2 vector = Input.mousePosition.WithY((float)Screen.height - Input.mousePosition.y).XY();
				if (this.textOption1.IsVisible && this.textOption1.GetScreenRect().Contains(vector))
				{
					this.HandleOptionHover(this.textOption1);
					if (Input.GetMouseButtonDown(0))
					{
						this.HandleOptionClick(this.textOption1);
						this.m_lastAssignedPlayer.SuppressThisClick = true;
					}
				}
				if (this.textOption2.IsVisible && this.textOption2.GetScreenRect().Contains(vector))
				{
					this.HandleOptionHover(this.textOption2);
					if (Input.GetMouseButtonDown(0))
					{
						this.HandleOptionClick(this.textOption2);
						this.m_lastAssignedPlayer.SuppressThisClick = true;
					}
				}
			}
		}
	}

	// Token: 0x06008B08 RID: 35592 RVA: 0x0039E7F4 File Offset: 0x0039C9F4
	private void UpdateScaleAndPosition(dfControl c, float newScalar, bool doVerticalAdjustment = true)
	{
		if (c.transform.localScale.x != newScalar)
		{
			float x = c.transform.localScale.x;
			c.transform.localScale = new Vector3(newScalar, newScalar, 1f);
			c.RelativePosition = new Vector3(c.RelativePosition.x * (newScalar / x), (!doVerticalAdjustment) ? c.RelativePosition.y : (c.RelativePosition.y + ((x >= newScalar) ? (-c.Height) : c.Height)), c.RelativePosition.z);
		}
	}

	// Token: 0x06008B09 RID: 35593 RVA: 0x0039E8B4 File Offset: 0x0039CAB4
	public void ShowBar(PlayerController interactingPlayer, string[] responses)
	{
		GameUIRoot.Instance.notificationController.ForceHide();
		this.UpdateScaleAndPosition(this.reticleLeft1, 1f / GameUIRoot.GameUIScalar, true);
		this.UpdateScaleAndPosition(this.reticleLeft2, 1f / GameUIRoot.GameUIScalar, true);
		this.UpdateScaleAndPosition(this.portraitSprite, 1f / GameUIRoot.GameUIScalar, true);
		bool flag = false;
		if (!this.m_conversationBarSprite)
		{
			flag = true;
			this.m_conversationBarSprite = base.GetComponent<dfSprite>();
			this.m_motionGroup = GameUIRoot.Instance.AddControlToMotionGroups(this.m_conversationBarSprite, DungeonData.Direction.SOUTH, true);
		}
		if (this.m_conversationBarSprite.Parent.transform.localScale.x != 1f / GameUIRoot.GameUIScalar)
		{
			this.m_conversationBarSprite.Parent.transform.localScale = new Vector3(1f / GameUIRoot.GameUIScalar, 1f / GameUIRoot.GameUIScalar, 1f);
			if (flag)
			{
				this.m_conversationBarSprite.Parent.RelativePosition = this.m_conversationBarSprite.Parent.RelativePosition.WithY(this.m_conversationBarSprite.Parent.Height * 3f);
			}
		}
		if (interactingPlayer.characterIdentity == PlayableCharacters.Eevee)
		{
			Material material = UnityEngine.Object.Instantiate<Material>(this.portraitSprite.Atlas.Material);
			material.shader = Shader.Find("Brave/Internal/GlitchEevee");
			material.SetTexture("_EeveeTex", this.EeveeTex);
			material.SetFloat("_WaveIntensity", 0.1f);
			material.SetFloat("_ColorIntensity", 0.015f);
			this.portraitSprite.OverrideMaterial = material;
		}
		else
		{
			this.portraitSprite.OverrideMaterial = null;
		}
		this.m_isActive = true;
		this.m_lastAssignedPlayer = interactingPlayer;
		GameUIRoot.Instance.MoveNonCoreGroupOnscreen(this.m_conversationBarSprite, false);
		this.m_conversationBarSprite.BringToFront();
		if (interactingPlayer.characterIdentity == PlayableCharacters.Eevee)
		{
			switch (UnityEngine.Random.Range(0, 4))
			{
			case 0:
				this.portraitSprite.SpriteName = "talking_bar_character_window_rogue_003";
				break;
			case 1:
				this.portraitSprite.SpriteName = "talking_bar_character_window_marine_003";
				break;
			case 2:
				this.portraitSprite.SpriteName = "talking_bar_character_window_guide_003";
				break;
			case 3:
				this.portraitSprite.SpriteName = "talking_bar_character_window_convict_003";
				break;
			default:
				this.portraitSprite.SpriteName = "talking_bar_character_window_guide_003";
				break;
			}
		}
		else
		{
			this.portraitSprite.SpriteName = interactingPlayer.uiPortraitName;
		}
		if (GameManager.Options.SmallUIEnabled)
		{
			if (!this.m_portraitAdjustedForSmallUI)
			{
				this.portraitSprite.Size = this.portraitSprite.Size / 2f;
				this.portraitSprite.RelativePosition = this.portraitSprite.RelativePosition - new Vector3(0f, this.portraitSprite.Size.y * 2f, 0f);
				this.m_portraitAdjustedForSmallUI = true;
			}
		}
		else if (this.m_portraitAdjustedForSmallUI)
		{
			this.portraitSprite.RelativePosition = this.portraitSprite.RelativePosition + new Vector3(0f, this.portraitSprite.Size.y * 2f, 0f);
			this.portraitSprite.Size = this.portraitSprite.Size * 2f;
			this.m_portraitAdjustedForSmallUI = false;
		}
		this.m_conversationBarSprite.IsVisible = true;
		this.textOption1.IsVisible = true;
		this.textOption1.Text = responses[0];
		this.reticleRight1.RelativePosition = this.reticleLeft1.RelativePosition.WithX(this.reticleLeft1.RelativePosition.x + this.reticleLeft1.Width + this.textOption1.GetAutosizeWidth() + 24f);
		if (responses != null && responses.Length > 1)
		{
			this.textOption2.IsVisible = true;
			this.textOption2.Text = responses[1];
			this.reticleRight2.RelativePosition = this.reticleLeft2.RelativePosition.WithX(this.reticleLeft2.RelativePosition.x + this.reticleLeft2.Width + this.textOption2.GetAutosizeWidth() + 24f);
		}
		else
		{
			this.textOption2.IsVisible = false;
			this.textOption2.Text = string.Empty;
		}
	}

	// Token: 0x06008B0A RID: 35594 RVA: 0x0039ED58 File Offset: 0x0039CF58
	private void HandleOptionHover(dfControl control)
	{
		if (control == this.textOption1)
		{
			GameUIRoot.Instance.SetConversationResponse(0);
		}
		if (control == this.textOption2)
		{
			GameUIRoot.Instance.SetConversationResponse(1);
		}
	}

	// Token: 0x06008B0B RID: 35595 RVA: 0x0039ED94 File Offset: 0x0039CF94
	private void HandleOptionClick(dfControl control)
	{
		if (control == this.textOption1)
		{
			GameUIRoot.Instance.SetConversationResponse(0);
		}
		if (control == this.textOption2)
		{
			GameUIRoot.Instance.SetConversationResponse(1);
		}
		GameUIRoot.Instance.SelectConversationResponse();
	}

	// Token: 0x040091D0 RID: 37328
	public Color selectedColor = Color.white;

	// Token: 0x040091D1 RID: 37329
	public Color unselectedColor = Color.gray;

	// Token: 0x040091D2 RID: 37330
	public dfButton textOption1;

	// Token: 0x040091D3 RID: 37331
	public dfButton textOption2;

	// Token: 0x040091D4 RID: 37332
	public dfSprite reticleLeft1;

	// Token: 0x040091D5 RID: 37333
	public dfSprite reticleRight1;

	// Token: 0x040091D6 RID: 37334
	public dfSprite reticleLeft2;

	// Token: 0x040091D7 RID: 37335
	public dfSprite reticleRight2;

	// Token: 0x040091D8 RID: 37336
	public dfSprite portraitSprite;

	// Token: 0x040091D9 RID: 37337
	public Texture2D EeveeTex;

	// Token: 0x040091DA RID: 37338
	private dfSprite m_conversationBarSprite;

	// Token: 0x040091DB RID: 37339
	private bool m_isActive;

	// Token: 0x040091DC RID: 37340
	private PlayerController m_lastAssignedPlayer;

	// Token: 0x040091DD RID: 37341
	private bool m_temporarilyHidden;

	// Token: 0x040091DE RID: 37342
	private dfPanel m_motionGroup;

	// Token: 0x040091DF RID: 37343
	private bool m_portraitAdjustedForSmallUI;
}
