using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001740 RID: 5952
public class BossCardUIController : TimeInvariantMonoBehaviour
{
	// Token: 0x06008A70 RID: 35440 RVA: 0x0039A8A0 File Offset: 0x00398AA0
	private void Initialize()
	{
		this.m_pix = this.uiCamera.GetComponent<Pixelator_Simple>();
		this.m_pix.Initialize();
		this.ToggleCoreVisiblity(false);
		this.ResetTextsToStart();
	}

	// Token: 0x06008A71 RID: 35441 RVA: 0x0039A8CC File Offset: 0x00398ACC
	private void InitializeTextsShared()
	{
		if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.KOREAN || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.JAPANESE || GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.RUSSIAN)
		{
			this.nameLabel.PerCharacterOffset = Vector3.zero;
			this.subtitleLabel.PerCharacterOffset = Vector3.zero;
			this.quoteLabel.PerCharacterOffset = Vector3.zero;
			this.nameLabel.transform.rotation = Quaternion.identity;
			this.subtitleLabel.transform.rotation = Quaternion.identity;
			this.quoteLabel.transform.rotation = Quaternion.identity;
			dfLabel dfLabel = this.nameLabel;
			bool flag = false;
			this.nameLabel.Outline = flag;
			dfLabel.Shadow = flag;
			dfLabel dfLabel2 = this.subtitleLabel;
			flag = false;
			this.subtitleLabel.Outline = flag;
			dfLabel2.Shadow = flag;
			dfLabel dfLabel3 = this.quoteLabel;
			flag = false;
			this.quoteLabel.Outline = flag;
			dfLabel3.Shadow = flag;
		}
	}

	// Token: 0x06008A72 RID: 35442 RVA: 0x0039A9D8 File Offset: 0x00398BD8
	public void InitializeTexts(PortraitSlideSettings pss)
	{
		this.m_topLeftTextPxOffset = pss.topLeftTextPxOffset;
		this.m_bottomRightTextPxOffset = pss.bottomRightTextPxOffset;
		if (GameManager.Instance.Dungeon)
		{
			this.nameLabel.Glitchy = GameManager.Instance.Dungeon.IsGlitchDungeon;
			this.subtitleLabel.Glitchy = GameManager.Instance.Dungeon.IsGlitchDungeon;
		}
		if (this.m_cachedNameLabelTextScale == -1f)
		{
			this.m_cachedNameLabelTextScale = this.nameLabel.TextScale;
		}
		this.nameLabel.Text = StringTableManager.GetEnemiesString(pss.bossNameString, -1);
		float autosizeWidth = this.nameLabel.GetAutosizeWidth();
		if (autosizeWidth > 800f)
		{
			this.nameLabel.PerCharacterOffset = new Vector3(0f, 2f, 0f);
			this.nameLabel.TextScale = 1000f / autosizeWidth * this.m_cachedNameLabelTextScale;
			this.m_topLeftTextPxOffset += new IntVector2(0, -6);
			this.m_topLeftTextPxOffset += new IntVector2(0, -6);
		}
		else
		{
			this.nameLabel.PerCharacterOffset = new Vector3(0f, 3f, 0f);
			this.nameLabel.TextScale = this.m_cachedNameLabelTextScale;
		}
		this.InitializeTextsShared();
		this.subtitleLabel.Text = StringTableManager.GetEnemiesString(pss.bossSubtitleString, -1);
		this.quoteLabel.Text = StringTableManager.GetEnemiesString(pss.bossQuoteString, -1);
		this.m_bossSprite = pss.bossArtSprite;
		this.m_bossSpritePxOffset = pss.bossSpritePxOffset;
	}

	// Token: 0x06008A73 RID: 35443 RVA: 0x0039AB80 File Offset: 0x00398D80
	private IEnumerator InvariantWaitForSeconds(float seconds)
	{
		float elapsed = 0f;
		while (elapsed < seconds)
		{
			elapsed += this.m_deltaTime;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008A74 RID: 35444 RVA: 0x0039ABA4 File Offset: 0x00398DA4
	public void TriggerSequence()
	{
		for (int i = 0; i < this.parallaxSprites.Count; i++)
		{
			if (!this.parallaxSprites[i])
			{
				this.parallaxSprites.RemoveAt(i);
				this.parallaxEnds.RemoveAt(i);
				this.parallaxStarts.RemoveAt(i);
				this.parallaxSpeeds.RemoveAt(i);
				i--;
			}
		}
		base.StartCoroutine(this.CoreSequence(null));
	}

	// Token: 0x06008A75 RID: 35445 RVA: 0x0039AC28 File Offset: 0x00398E28
	private void ToggleCoreVisiblity(bool visible)
	{
		for (int i = 0; i < this.parallaxSprites.Count; i++)
		{
			if (!this.parallaxSprites[i])
			{
				this.parallaxSprites.RemoveAt(i);
				this.parallaxEnds.RemoveAt(i);
				this.parallaxStarts.RemoveAt(i);
				this.parallaxSpeeds.RemoveAt(i);
				i--;
			}
		}
		if (!this.m_bossSprite)
		{
			this.bossSprite.IsVisible = false;
			this.playerSprite.IsVisible = false;
			this.coopSprite.IsVisible = false;
		}
		else
		{
			this.bossSprite.IsVisible = visible;
			this.bossSprite.Texture = this.m_bossSprite;
			this.playerSprite.IsVisible = !GameManager.Instance.PrimaryPlayer.healthHaver.IsDead;
			this.coopSprite.IsVisible = GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && !GameManager.Instance.SecondaryPlayer.healthHaver.IsDead;
			if (this.coopSprite.IsVisible)
			{
				this.coopSprite.ZOrder = this.bossSprite.ZOrder - 1;
			}
			PlayerController primaryPlayer = GameManager.Instance.PrimaryPlayer;
			this.playerSprite.Texture = primaryPlayer.BosscardSprites[0];
			if (primaryPlayer.characterIdentity == PlayableCharacters.Eevee)
			{
				Shader shader = Shader.Find("Brave/Internal/GlitchEevee");
				Material material = new Material(shader)
				{
					name = "Default Texture Shader",
					hideFlags = HideFlags.DontSave,
					mainTexture = primaryPlayer.BosscardSprites[0]
				};
				material.SetTexture("_EeveeTex", primaryPlayer.GetComponent<CharacterAnimationRandomizer>().CosmicTex);
				material.SetFloat("_WaveIntensity", 0.1f);
				material.SetFloat("_ColorIntensity", 0.015f);
				this.playerSprite.OverrideMaterial = material;
			}
			else if (this.playerSprite.OverrideMaterial != null)
			{
				this.playerSprite.OverrideMaterial = null;
			}
			BraveDFTextureAnimator component = this.playerSprite.GetComponent<BraveDFTextureAnimator>();
			component.timeless = true;
			component.textures = GameManager.Instance.PrimaryPlayer.BosscardSprites.ToArray();
			component.fps = GameManager.Instance.PrimaryPlayer.BosscardSpriteFPS;
			this.RecalculateScales();
		}
		this.topTriangle.IsVisible = visible;
		this.bottomTriangle.IsVisible = visible;
		if (!visible)
		{
			this.womboBG.IsVisible = false;
			this.womboBar.IsVisible = false;
			this.lightStreaksSprite.IsVisible = false;
			for (int j = 0; j < this.parallaxSprites.Count; j++)
			{
				this.parallaxSprites[j].IsVisible = false;
			}
		}
	}

	// Token: 0x06008A76 RID: 35446 RVA: 0x0039AF08 File Offset: 0x00399108
	private void RecalculateScales()
	{
		dfGUIManager manager = this.coopSprite.GetManager();
		Vector2 screenSize = manager.GetScreenSize();
		this.playerSprite.Size = manager.GetScreenSize();
		if (this.coopSprite != null)
		{
			float num = 1.7791667f;
			this.coopSprite.Size = new Vector2(screenSize.y * num, screenSize.y);
		}
		float num2 = (float)this.bossSprite.Texture.width / (float)this.bossSprite.Texture.height;
		this.bossSprite.Size = new Vector2(screenSize.y * num2, screenSize.y);
	}

	// Token: 0x06008A77 RID: 35447 RVA: 0x0039AFB4 File Offset: 0x003991B4
	private Vector3 GetCoopOffset()
	{
		Vector2 vector = this.playerTarget.localPosition.XY();
		if (this.playerSprite.IsVisible)
		{
			PerCharacterCoopPositionData coopBosscardOffset = GameManager.Instance.PrimaryPlayer.CoopBosscardOffset;
			if (coopBosscardOffset.flipCoopCultist && this.coopSprite.transform.localScale.x != -1f)
			{
				this.coopSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			Vector2 vector2 = new Vector2(coopBosscardOffset.percentOffset.x * -1f, coopBosscardOffset.percentOffset.y);
			Vector2 vector3 = Vector2.Scale(vector, vector2);
			return vector3.ToVector3ZUp(0f);
		}
		return Vector3.zero;
	}

	// Token: 0x06008A78 RID: 35448 RVA: 0x0039B084 File Offset: 0x00399284
	public void ToggleBoxing(bool enable)
	{
		if (!enable)
		{
			Pixelator.Instance.LerpToLetterbox(1f, 0f);
			Pixelator.Instance.SetWindowbox(1f);
		}
		else
		{
			float num = 1.7777778f;
			float aspect = BraveCameraUtility.ASPECT;
			if (num < aspect)
			{
				Pixelator.Instance.SetWindowbox(num / aspect * 0.5f);
			}
			else if (num > aspect)
			{
				Pixelator.Instance.LerpToLetterbox(num / aspect * 0.5f, 0f);
			}
		}
	}

	// Token: 0x06008A79 RID: 35449 RVA: 0x0039B10C File Offset: 0x0039930C
	protected override void Update()
	{
		base.Update();
		if (this.playerSprite && this.playerSprite.OverrideMaterial != null)
		{
			this.playerSprite.OverrideMaterial.SetFloat("_AdditionalTime", this.playerSprite.OverrideMaterial.GetFloat("_AdditionalTime") + GameManager.INVARIANT_DELTA_TIME / 4f);
		}
	}

	// Token: 0x06008A7A RID: 35450 RVA: 0x0039B17C File Offset: 0x0039937C
	public IEnumerator CoreSequence(PortraitSlideSettings pss)
	{
		if (this.m_isPlaying)
		{
			yield break;
		}
		this.m_isPlaying = true;
		this.Initialize();
		this.ToggleBoxing(true);
		GameUIRoot.Instance.HideCoreUI(string.Empty);
		GameUIRoot.Instance.ToggleUICamera(false);
		this.RecalculateScales();
		this.lightStreaksSprite.IsVisible = false;
		for (int i = 0; i < this.parallaxSprites.Count; i++)
		{
			this.parallaxSprites[i].IsVisible = false;
		}
		if (this.playerSprite)
		{
			this.playerSprite.IsVisible = false;
		}
		if (this.coopSprite)
		{
			this.coopSprite.IsVisible = false;
		}
		Material targetMaterial = this.m_pix.RenderMaterial;
		base.StartCoroutine(this.FlashWhiteToBlack(targetMaterial, false));
		BraveMemory.HandleBossCardFlashAnticipation();
		yield return base.StartCoroutine(this.InvariantWaitForSeconds(this.FLASH_DURATION));
		this.bossSprite.transform.position = this.bossStart.position;
		this.playerSprite.transform.position = this.playerStart.position;
		if (this.coopSprite.IsVisible)
		{
			this.coopSprite.transform.position = this.playerSprite.transform.position + this.GetCoopOffset();
		}
		this.ToggleCoreVisiblity(true);
		if (this.playerSprite)
		{
			this.playerSprite.IsVisible = false;
		}
		if (this.coopSprite)
		{
			this.coopSprite.IsVisible = false;
		}
		base.StartCoroutine(this.HandleLightStreaks());
		yield return base.StartCoroutine(this.InvariantWaitForSeconds(this.FLASH_DURATION));
		targetMaterial.SetColor("_OverrideColor", Color.clear);
		base.StartCoroutine(this.WomboCombo(pss));
		yield return base.StartCoroutine(this.InvariantWaitForSeconds(this.FLASHBAR_CROSS_DURATION));
		base.StartCoroutine(this.LerpTextsToTargets());
		float waitDuration = Mathf.Max(this.FLASHBAR_WAIT_DURATION + this.FLASHBAR_EXPAND_DURATION, this.TEXT_IN_DURATION);
		yield return base.StartCoroutine(this.InvariantWaitForSeconds(waitDuration));
		yield return base.StartCoroutine(this.InvariantWaitForSeconds(0.1f));
		yield return base.StartCoroutine(this.HandleCharacterSlides());
		base.StartCoroutine(this.FlashWhiteToBlack(targetMaterial, true));
		yield return base.StartCoroutine(this.InvariantWaitForSeconds(this.FLASH_DURATION));
		this.ToggleCoreVisiblity(false);
		this.m_doLightStreaks = false;
		this.ResetTextsToStart();
		GameUIRoot.Instance.ToggleUICamera(true);
		GameUIRoot.Instance.ShowCoreUI(string.Empty);
		this.ToggleBoxing(false);
		this.m_isPlaying = false;
		yield break;
	}

	// Token: 0x06008A7B RID: 35451 RVA: 0x0039B1A0 File Offset: 0x003993A0
	public void BreakSequence()
	{
		GameUIRoot.Instance.ToggleUICamera(true);
		GameUIRoot.Instance.ShowCoreUI(string.Empty);
		this.ToggleBoxing(false);
		this.m_isPlaying = false;
	}

	// Token: 0x06008A7C RID: 35452 RVA: 0x0039B1CC File Offset: 0x003993CC
	private IEnumerator HandleLightStreaks()
	{
		this.lightStreaksSprite.IsVisible = true;
		List<float> parallaxTs = new List<float>();
		for (int i = 0; i < this.parallaxSprites.Count; i++)
		{
			dfTextureSprite dfTextureSprite = this.parallaxSprites[i];
			dfTextureSprite.IsVisible = true;
			dfTextureSprite.transform.position = this.parallaxStarts[i].position;
			parallaxTs.Add(0f);
		}
		this.m_doLightStreaks = true;
		float elapsed = 0f;
		float individualSpriteDuration = 0.1f;
		int currentLightStreakSprite = 0;
		while (this.m_doLightStreaks)
		{
			elapsed += this.m_deltaTime;
			if (elapsed > individualSpriteDuration)
			{
				elapsed -= individualSpriteDuration;
				currentLightStreakSprite = (currentLightStreakSprite + 1) % this.lightStreakSpriteNames.Count;
				this.lightStreaksSprite.SpriteName = this.lightStreakSpriteNames[currentLightStreakSprite];
			}
			for (int j = 0; j < this.parallaxSprites.Count; j++)
			{
				parallaxTs[j] = (parallaxTs[j] + this.parallaxSpeeds[j] * this.m_deltaTime) % 1f;
				float num = parallaxTs[j].Quantize(this.PARALLAX_QUANTIZATION_STEP);
				this.parallaxSprites[j].transform.position = Vector3.Lerp(this.parallaxStarts[j].position, this.parallaxEnds[j].position, num);
			}
			yield return null;
		}
		this.lightStreaksSprite.IsVisible = false;
		for (int k = 0; k < this.parallaxSprites.Count; k++)
		{
			this.parallaxSprites[k].IsVisible = false;
		}
		yield break;
	}

	// Token: 0x06008A7D RID: 35453 RVA: 0x0039B1E8 File Offset: 0x003993E8
	private IEnumerator WomboCombo(PortraitSlideSettings pss)
	{
		this.womboBG.IsVisible = true;
		this.womboBG.Color = Color.black;
		this.womboBG.Opacity = 1f;
		this.womboBG.ZOrder = 1;
		this.womboBar.ZOrder = 2;
		this.lightStreaksSprite.ZOrder = 0;
		for (int i = 0; i < this.parallaxSprites.Count; i++)
		{
			this.parallaxSprites[i].ZOrder = 0;
		}
		this.womboBar.IsVisible = true;
		this.womboBar.Opacity = 1f;
		float crossDuration = this.FLASHBAR_CROSS_DURATION;
		float waitDuration = this.FLASHBAR_WAIT_DURATION;
		float expandDuration = this.FLASHBAR_EXPAND_DURATION;
		dfGUIManager manager = this.womboBar.GetManager();
		Vector2 screenSize = manager.GetScreenSize();
		float elapsed = 0f;
		while (elapsed < crossDuration)
		{
			elapsed += this.m_deltaTime;
			float t = elapsed / crossDuration;
			this.womboBar.Width = Mathf.Lerp(0f, screenSize.x * 1.5f, t);
			yield return null;
		}
		yield return base.StartCoroutine(this.InvariantWaitForSeconds(waitDuration));
		elapsed = 0f;
		while (elapsed < expandDuration)
		{
			elapsed += this.m_deltaTime;
			float t2 = elapsed / expandDuration;
			this.womboBar.Height = Mathf.Lerp(10f, screenSize.y * 1.5f, t2);
			float quadT = t2 * t2 * t2 * t2;
			this.womboBar.Opacity = 1f - quadT;
			this.womboBG.Opacity = Mathf.Lerp(1f, 0.8f, quadT);
			this.womboBG.Color = Color.Lerp(Color.black, (pss == null) ? Color.blue : pss.bgColor, quadT);
			yield return null;
		}
		this.womboBG.ZOrder = 0;
		this.lightStreaksSprite.ZOrder = 1;
		this.womboBar.IsVisible = false;
		this.womboBar.Size = new Vector2(0f, 10f);
		yield break;
	}

	// Token: 0x06008A7E RID: 35454 RVA: 0x0039B20C File Offset: 0x0039940C
	private IEnumerator HandleDelayedCoopCharacterSlide()
	{
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER)
		{
			yield break;
		}
		float initialMoveDuration = this.CHARACTER_INITIAL_MOVE_DURATION;
		float slideDuration = this.CHARACTER_SLIDE_DURATION - this.CHARACTER_INITIAL_MOVE_DURATION;
		float elapsed = 0f;
		Vector3 playerVec = this.playerTarget.position - this.playerStart.position;
		this.coopSprite.transform.position = this.playerStart.position + this.GetCoopOffset();
		float p2u = this.playerSprite.PixelsToUnits();
		while (elapsed < initialMoveDuration)
		{
			elapsed += this.m_deltaTime;
			float t = elapsed / initialMoveDuration;
			Vector3 calcedPlayerPos = Vector3.Lerp(this.playerStart.position, this.playerTarget.position, t);
			this.coopSprite.transform.position = calcedPlayerPos + this.GetCoopOffset();
			yield return null;
		}
		elapsed = 0f;
		Vector3 currentRealPlayerPosition = this.playerTarget.position;
		while (elapsed < slideDuration)
		{
			elapsed += this.m_deltaTime;
			currentRealPlayerPosition += playerVec.normalized * this.m_deltaTime * this.BOSS_SLIDE_SPEED;
			this.coopSprite.transform.position = currentRealPlayerPosition.Quantize(p2u) + this.GetCoopOffset();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008A7F RID: 35455 RVA: 0x0039B228 File Offset: 0x00399428
	private IEnumerator HandleCharacterSlides()
	{
		this.playerSprite.IsVisible = !GameManager.Instance.PrimaryPlayer.healthHaver.IsDead;
		this.coopSprite.IsVisible = GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && !GameManager.Instance.SecondaryPlayer.healthHaver.IsDead;
		float initialMoveDuration = this.CHARACTER_INITIAL_MOVE_DURATION;
		float slideDuration = this.CHARACTER_SLIDE_DURATION;
		float elapsed = 0f;
		Vector3 playerVec = this.playerTarget.position - this.playerStart.position;
		Vector3 bossVec = this.bossTarget.position - this.bossStart.position;
		this.bossSprite.transform.position = this.bossStart.position;
		this.playerSprite.transform.position = this.playerStart.position;
		float p2u = this.bossSprite.PixelsToUnits();
		Vector3 bossOffset = this.m_bossSpritePxOffset.ToVector3() * p2u;
		while (elapsed < initialMoveDuration)
		{
			elapsed += this.m_deltaTime;
			float t = elapsed / initialMoveDuration;
			this.bossSprite.transform.position = Vector3.Lerp(this.bossStart.position + bossOffset, this.bossTarget.position + bossOffset, t);
			this.playerSprite.transform.position = Vector3.Lerp(this.playerStart.position, this.playerTarget.position, t);
			yield return null;
		}
		base.StartCoroutine(this.HandleDelayedCoopCharacterSlide());
		elapsed = 0f;
		Vector3 currentRealBossPosition = this.bossSprite.transform.position;
		Vector3 currentRealPlayerPosition = this.playerSprite.transform.position;
		while (elapsed < slideDuration)
		{
			elapsed += this.m_deltaTime;
			currentRealBossPosition += bossVec.normalized * this.m_deltaTime * this.CHARACTER_SLIDE_SPEED;
			currentRealPlayerPosition += playerVec.normalized * this.m_deltaTime * this.BOSS_SLIDE_SPEED;
			this.bossSprite.transform.position = currentRealBossPosition.Quantize(p2u);
			this.playerSprite.transform.position = currentRealPlayerPosition.Quantize(p2u);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008A80 RID: 35456 RVA: 0x0039B244 File Offset: 0x00399444
	private IEnumerator LerpTextsToTargets()
	{
		float lerpDuration = this.TEXT_IN_DURATION;
		float elapsed = 0f;
		float p2u = this.bossSprite.PixelsToUnits();
		Vector3 topLeftTextOffset = this.m_topLeftTextPxOffset.ToVector3() * p2u;
		Vector3 bottomRightTextOffset = this.m_bottomRightTextPxOffset.ToVector3() * p2u;
		while (elapsed < lerpDuration)
		{
			elapsed += this.m_deltaTime;
			float t = Mathf.SmoothStep(0f, 1f, elapsed / lerpDuration);
			for (int i = 0; i < this.floatingTexts.Count; i++)
			{
				Vector3 vector = Vector3.zero;
				dfControl component = this.floatingTexts[i].GetComponent<dfControl>();
				if (component.Pivot == dfPivotPoint.BottomRight)
				{
					vector = component.Size.ToVector3ZUp(0f) * component.PixelsToUnits();
					vector.y *= -1f;
				}
				Vector3 vector2 = ((i != 1) ? topLeftTextOffset : bottomRightTextOffset);
				this.floatingTexts[i].position = Vector3.Lerp(this.floatingTextStarts[i].position + vector2, this.floatingTextTargets[i].position + vector2, t);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008A81 RID: 35457 RVA: 0x0039B260 File Offset: 0x00399460
	private void ResetTextsToStart()
	{
		for (int i = 0; i < this.floatingTexts.Count; i++)
		{
			this.floatingTexts[i].position = this.floatingTextStarts[i].position;
		}
		this.playerSprite.transform.position = this.playerStart.position;
		if (this.coopSprite.IsVisible)
		{
			this.coopSprite.transform.position = this.playerSprite.transform.position + this.GetCoopOffset();
		}
		this.bossSprite.transform.position = this.bossStart.position;
	}

	// Token: 0x06008A82 RID: 35458 RVA: 0x0039B31C File Offset: 0x0039951C
	private IEnumerator FlashColorToColor(Color startColor, Color targetColor, float fadeDuration, Material targetMaterial)
	{
		float elapsed = 0f;
		while (elapsed < fadeDuration)
		{
			elapsed += this.m_deltaTime;
			float t = elapsed / fadeDuration;
			Color c = Color.Lerp(startColor, targetColor, t);
			targetMaterial.SetColor("_OverrideColor", c);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008A83 RID: 35459 RVA: 0x0039B354 File Offset: 0x00399554
	private IEnumerator FlashWhiteToBlack(Material targetMaterial, bool backToClear)
	{
		float fadeDuration = this.FLASH_DURATION;
		PlatformInterface.SetAlienFXColor(new Color(1f, 1f, 1f, 1f), 0.5f);
		yield return base.StartCoroutine(this.FlashColorToColor(Color.clear, Color.white, fadeDuration, targetMaterial));
		yield return base.StartCoroutine(this.FlashColorToColor(Color.white, Color.black, fadeDuration, targetMaterial));
		if (backToClear)
		{
			yield return base.StartCoroutine(this.FlashColorToColor(Color.black, Color.clear, fadeDuration, targetMaterial));
		}
		yield break;
	}

	// Token: 0x06008A84 RID: 35460 RVA: 0x0039B380 File Offset: 0x00399580
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040090F3 RID: 37107
	[Header("Dave Stuff")]
	public float FLASH_DURATION = 0.1f;

	// Token: 0x040090F4 RID: 37108
	public float FLASHBAR_CROSS_DURATION = 0.2f;

	// Token: 0x040090F5 RID: 37109
	public float FLASHBAR_WAIT_DURATION = 0.1f;

	// Token: 0x040090F6 RID: 37110
	public float FLASHBAR_EXPAND_DURATION = 0.2f;

	// Token: 0x040090F7 RID: 37111
	public float TEXT_IN_DURATION = 0.5f;

	// Token: 0x040090F8 RID: 37112
	public float CHARACTER_INITIAL_MOVE_DURATION = 0.5f;

	// Token: 0x040090F9 RID: 37113
	public float CHARACTER_SLIDE_DURATION = 2f;

	// Token: 0x040090FA RID: 37114
	public float CHARACTER_SLIDE_SPEED = 0.05f;

	// Token: 0x040090FB RID: 37115
	public float BOSS_SLIDE_SPEED = 0.05f;

	// Token: 0x040090FC RID: 37116
	public float PARALLAX_QUANTIZATION_STEP = 0.1f;

	// Token: 0x040090FD RID: 37117
	[Header("Not for Daves")]
	public Camera uiCamera;

	// Token: 0x040090FE RID: 37118
	public dfTextureSprite topTriangle;

	// Token: 0x040090FF RID: 37119
	public dfTextureSprite bottomTriangle;

	// Token: 0x04009100 RID: 37120
	public dfTextureSprite womboBar;

	// Token: 0x04009101 RID: 37121
	public dfTextureSprite womboBG;

	// Token: 0x04009102 RID: 37122
	public dfTextureSprite bossSprite;

	// Token: 0x04009103 RID: 37123
	public Transform bossStart;

	// Token: 0x04009104 RID: 37124
	public Transform bossTarget;

	// Token: 0x04009105 RID: 37125
	public dfTextureSprite playerSprite;

	// Token: 0x04009106 RID: 37126
	public Transform playerStart;

	// Token: 0x04009107 RID: 37127
	public Transform playerTarget;

	// Token: 0x04009108 RID: 37128
	public dfTextureSprite coopSprite;

	// Token: 0x04009109 RID: 37129
	[Header("Parallax Bros")]
	public List<dfTextureSprite> parallaxSprites;

	// Token: 0x0400910A RID: 37130
	public List<float> parallaxSpeeds;

	// Token: 0x0400910B RID: 37131
	public List<Transform> parallaxStarts;

	// Token: 0x0400910C RID: 37132
	public List<Transform> parallaxEnds;

	// Token: 0x0400910D RID: 37133
	[Header("Light Streaks")]
	public dfSprite lightStreaksSprite;

	// Token: 0x0400910E RID: 37134
	public List<string> lightStreakSpriteNames;

	// Token: 0x0400910F RID: 37135
	[Header("Texts")]
	public List<Transform> floatingTexts;

	// Token: 0x04009110 RID: 37136
	public List<Transform> floatingTextStarts;

	// Token: 0x04009111 RID: 37137
	public List<Transform> floatingTextTargets;

	// Token: 0x04009112 RID: 37138
	public dfLabel nameLabel;

	// Token: 0x04009113 RID: 37139
	public dfLabel subtitleLabel;

	// Token: 0x04009114 RID: 37140
	public dfLabel quoteLabel;

	// Token: 0x04009115 RID: 37141
	private string m_charSpriteName;

	// Token: 0x04009116 RID: 37142
	private Texture m_bossSprite;

	// Token: 0x04009117 RID: 37143
	private Pixelator_Simple m_pix;

	// Token: 0x04009118 RID: 37144
	private IntVector2 m_bossSpritePxOffset;

	// Token: 0x04009119 RID: 37145
	private IntVector2 m_topLeftTextPxOffset;

	// Token: 0x0400911A RID: 37146
	private IntVector2 m_bottomRightTextPxOffset;

	// Token: 0x0400911B RID: 37147
	private bool m_isPlaying;

	// Token: 0x0400911C RID: 37148
	private float m_cachedNameLabelTextScale = -1f;

	// Token: 0x0400911D RID: 37149
	private bool m_doLightStreaks;
}
