using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020017F9 RID: 6137
public class TextBoxManager : MonoBehaviour
{
	// Token: 0x1700159E RID: 5534
	// (get) Token: 0x06009090 RID: 37008 RVA: 0x003D22B4 File Offset: 0x003D04B4
	private float TEXT_REVEAL_SPEED
	{
		get
		{
			switch (GameManager.Options.TextSpeed)
			{
			case GameOptions.GenericHighMedLowOption.LOW:
				return 27f;
			case GameOptions.GenericHighMedLowOption.MEDIUM:
				return 100f;
			case GameOptions.GenericHighMedLowOption.HIGH:
				return float.MaxValue;
			default:
				return 100f;
			}
		}
	}

	// Token: 0x1700159F RID: 5535
	// (get) Token: 0x06009091 RID: 37009 RVA: 0x003D2300 File Offset: 0x003D0500
	public static float ZombieBoxMultiplier
	{
		get
		{
			switch (GameManager.Options.TextSpeed)
			{
			case GameOptions.GenericHighMedLowOption.LOW:
				return 2.5f;
			case GameOptions.GenericHighMedLowOption.MEDIUM:
				return 1.5f;
			case GameOptions.GenericHighMedLowOption.HIGH:
				return 1f;
			default:
				return 1f;
			}
		}
	}

	// Token: 0x170015A0 RID: 5536
	// (get) Token: 0x06009092 RID: 37010 RVA: 0x003D2348 File Offset: 0x003D0548
	public bool IsRevealingText
	{
		get
		{
			return this.m_isRevealingText;
		}
	}

	// Token: 0x170015A1 RID: 5537
	// (get) Token: 0x06009093 RID: 37011 RVA: 0x003D2350 File Offset: 0x003D0550
	// (set) Token: 0x06009094 RID: 37012 RVA: 0x003D2358 File Offset: 0x003D0558
	public bool IsScalingUp { get; set; }

	// Token: 0x170015A2 RID: 5538
	// (get) Token: 0x06009095 RID: 37013 RVA: 0x003D2364 File Offset: 0x003D0564
	// (set) Token: 0x06009096 RID: 37014 RVA: 0x003D236C File Offset: 0x003D056C
	public bool IsScalingDown { get; set; }

	// Token: 0x170015A3 RID: 5539
	// (get) Token: 0x06009097 RID: 37015 RVA: 0x003D2378 File Offset: 0x003D0578
	public static int ExtantTextBoxCount
	{
		get
		{
			return TextBoxManager.extantTextBoxMap.Count;
		}
	}

	// Token: 0x170015A4 RID: 5540
	// (get) Token: 0x06009098 RID: 37016 RVA: 0x003D2384 File Offset: 0x003D0584
	public static bool ExtantTextBoxVisible
	{
		get
		{
			if (TextBoxManager.extantTextBoxMap == null || TextBoxManager.extantTextBoxMap.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < TextBoxManager.extantTextPointList.Count; i++)
			{
				if (!TextBoxManager.extantTextPointList[i])
				{
					TextBoxManager.extantTextPointList.RemoveAt(i);
					i--;
				}
				else if (GameManager.Instance.MainCameraController.PointIsVisible(TextBoxManager.extantTextPointList[i].position.XY()))
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x06009099 RID: 37017 RVA: 0x003D2424 File Offset: 0x003D0624
	public static void ClearPerLevelData()
	{
		TextBoxManager.extantTextBoxMap.Clear();
	}

	// Token: 0x0600909A RID: 37018 RVA: 0x003D2430 File Offset: 0x003D0630
	public static void ShowTextBox(Vector3 worldPosition, Transform parent, float duration, string text, string audioTag = "", bool instant = true, TextBoxManager.BoxSlideOrientation slideOrientation = TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, bool showContinueText = false, bool useAlienLanguage = false)
	{
		TextBoxManager.ShowBoxInternal(worldPosition, parent, duration, text, "TextBox", TextBoxManager.BOX_PADDING, audioTag, instant, slideOrientation, showContinueText, useAlienLanguage);
	}

	// Token: 0x0600909B RID: 37019 RVA: 0x003D245C File Offset: 0x003D065C
	public static void ShowInfoBox(Vector3 worldPosition, Transform parent, float duration, string text, bool instant = true, bool showContinueText = false)
	{
		TextBoxManager.ShowBoxInternal(worldPosition, parent, duration, text, "InfoBox", TextBoxManager.INFOBOX_PADDING, string.Empty, instant, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, showContinueText, false);
	}

	// Token: 0x0600909C RID: 37020 RVA: 0x003D2488 File Offset: 0x003D0688
	public static void ShowLetterBox(Vector3 worldPosition, Transform parent, float duration, string text, bool instant = true, bool showContinueText = false)
	{
		TextBoxManager.ShowBoxInternal(worldPosition, parent, duration, text, "LetterBox", TextBoxManager.BOX_PADDING, string.Empty, instant, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, showContinueText, false);
	}

	// Token: 0x0600909D RID: 37021 RVA: 0x003D24B4 File Offset: 0x003D06B4
	public static void ShowStoneTablet(Vector3 worldPosition, Transform parent, float duration, string text, bool instant = true, bool showContinueText = false)
	{
		TextBoxManager.ShowBoxInternal(worldPosition, parent, duration, text, "StoneTablet", TextBoxManager.BOX_PADDING, string.Empty, instant, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, showContinueText, false);
	}

	// Token: 0x0600909E RID: 37022 RVA: 0x003D24E0 File Offset: 0x003D06E0
	public static void ShowWoodPanel(Vector3 worldPosition, Transform parent, float duration, string text, bool instant = true, bool showContinueText = false)
	{
		TextBoxManager.ShowBoxInternal(worldPosition, parent, duration, text, "WoodPanel", TextBoxManager.BOX_PADDING, string.Empty, instant, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, showContinueText, false);
	}

	// Token: 0x0600909F RID: 37023 RVA: 0x003D250C File Offset: 0x003D070C
	public static void ShowThoughtBubble(Vector3 worldPosition, Transform parent, float duration, string text, bool instant = true, bool showContinueText = false, string overrideAudioTag = "")
	{
		TextBoxManager.ShowBoxInternal(worldPosition, parent, duration, text, "ThoughtBubble", TextBoxManager.BOX_PADDING, overrideAudioTag, instant, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, showContinueText, false);
	}

	// Token: 0x060090A0 RID: 37024 RVA: 0x003D2534 File Offset: 0x003D0734
	public static void ShowNote(Vector3 worldPosition, Transform parent, float duration, string text, bool instant = true, bool showContinueText = false)
	{
		TextBoxManager.ShowBoxInternal(worldPosition, parent, duration, text, "Note", TextBoxManager.BOX_PADDING, string.Empty, instant, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, showContinueText, false);
	}

	// Token: 0x060090A1 RID: 37025 RVA: 0x003D2560 File Offset: 0x003D0760
	public static bool TextBoxCanBeAdvanced(Transform parent)
	{
		if (TextBoxManager.extantTextBoxMap.ContainsKey(parent))
		{
			GameObject gameObject = TextBoxManager.extantTextBoxMap[parent];
			TextBoxManager component = gameObject.GetComponent<TextBoxManager>();
			return component.IsRevealingText;
		}
		return false;
	}

	// Token: 0x060090A2 RID: 37026 RVA: 0x003D2598 File Offset: 0x003D0798
	public static void AdvanceTextBox(Transform parent)
	{
		if (TextBoxManager.extantTextBoxMap.ContainsKey(parent))
		{
			GameObject gameObject = TextBoxManager.extantTextBoxMap[parent];
			TextBoxManager component = gameObject.GetComponent<TextBoxManager>();
			component.SkipTextReveal();
		}
	}

	// Token: 0x060090A3 RID: 37027 RVA: 0x003D25D0 File Offset: 0x003D07D0
	protected static void ShowBoxInternal(Vector3 worldPosition, Transform parent, float duration, string text, string prefabName, float padding, string audioTag, bool instant, TextBoxManager.BoxSlideOrientation slideOrientation, bool showContinueText, bool UseAlienLanguage = false)
	{
		Vector2 dimensions = new Vector2(-1f, -1f);
		if (parent != null && TextBoxManager.extantTextBoxMap.ContainsKey(parent))
		{
			dimensions = TextBoxManager.extantTextBoxMap[parent].GetComponent<TextBoxManager>().boxSprite.dimensions;
			UnityEngine.Object.Destroy(TextBoxManager.extantTextBoxMap[parent]);
			TextBoxManager.extantTextPointList.Remove(parent);
			TextBoxManager.extantTextBoxMap.Remove(parent);
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load(prefabName, ".prefab"));
		TextBoxManager component = gameObject.GetComponent<TextBoxManager>();
		component.boxPadding = padding;
		component.IsScalingUp = true;
		component.audioTag = audioTag;
		component.SetText(text, worldPosition, instant, slideOrientation, true, UseAlienLanguage, prefabName == "ThoughtBubble");
		if (parent != null)
		{
			component.transform.parent = parent;
			TextBoxManager.extantTextPointList.Add(parent);
			TextBoxManager.extantTextBoxMap.Add(parent, gameObject);
		}
		if (duration >= 0f)
		{
			component.HandleLifespan(gameObject, parent, duration);
		}
		if (showContinueText)
		{
			component.ShowContinueText();
		}
		component.StartCoroutine(component.HandleScaleUp(dimensions));
	}

	// Token: 0x170015A5 RID: 5541
	// (get) Token: 0x060090A4 RID: 37028 RVA: 0x003D26FC File Offset: 0x003D08FC
	private float ScaleFactor
	{
		get
		{
			return (float)Mathf.Max(1, Mathf.FloorToInt(1f / GameManager.Instance.MainCameraController.CurrentZoomScale));
		}
	}

	// Token: 0x060090A5 RID: 37029 RVA: 0x003D2720 File Offset: 0x003D0920
	private IEnumerator HandleScaleUp(Vector2 prevBoxSize)
	{
		this.IsScalingUp = true;
		if (prevBoxSize.x <= 0f || prevBoxSize.y <= 0f)
		{
			Transform targetTransform = base.transform;
			float elapsed = 0f;
			float duration = 0.06f;
			while (elapsed < duration)
			{
				elapsed += GameManager.INVARIANT_DELTA_TIME;
				targetTransform.localScale = Vector3.Lerp(new Vector3(0.01f, 0.01f, 1f), Vector3.one * this.ScaleFactor, Mathf.SmoothStep(0f, 1f, elapsed / duration));
				yield return null;
			}
		}
		else
		{
			Vector2 targetdimensions = this.boxSprite.dimensions;
			float elapsed2 = 0f;
			float durationModifier = Mathf.Clamp01(((targetdimensions - prevBoxSize).magnitude - 5f) / 10f);
			float duration2 = Mathf.Lerp(0.025f, 0.06f, durationModifier);
			while (elapsed2 < duration2)
			{
				elapsed2 += GameManager.INVARIANT_DELTA_TIME;
				this.boxSprite.dimensions = Vector2.Lerp(prevBoxSize, targetdimensions, Mathf.SmoothStep(0f, 1f, elapsed2 / duration2));
				yield return null;
			}
		}
		this.IsScalingUp = false;
		yield break;
	}

	// Token: 0x060090A6 RID: 37030 RVA: 0x003D2744 File Offset: 0x003D0944
	private IEnumerator HandleScaleDown()
	{
		this.IsScalingDown = true;
		Transform targetTransform = base.transform;
		float elapsed = 0f;
		float duration = 0.06f;
		Vector3 startScale = targetTransform.localScale;
		while (elapsed < duration)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			if (!targetTransform)
			{
				yield break;
			}
			targetTransform.localScale = Vector3.Lerp(startScale, new Vector3(0.01f, 0.01f, 1f), Mathf.SmoothStep(0f, 1f, elapsed / duration));
			yield return null;
		}
		if (!this || !base.gameObject)
		{
			yield break;
		}
		this.IsScalingDown = false;
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060090A7 RID: 37031 RVA: 0x003D2760 File Offset: 0x003D0960
	public static bool HasTextBox(Transform parent)
	{
		return TextBoxManager.extantTextBoxMap.ContainsKey(parent);
	}

	// Token: 0x060090A8 RID: 37032 RVA: 0x003D2770 File Offset: 0x003D0970
	public static void ClearTextBoxImmediate(Transform parent)
	{
		if (TextBoxManager.extantTextBoxMap.ContainsKey(parent))
		{
			TextBoxManager.extantTextPointList.Remove(parent);
			TextBoxManager.extantTextBoxMap.Remove(parent);
		}
		TextBoxManager componentInChildren = parent.GetComponentInChildren<TextBoxManager>();
		if (componentInChildren)
		{
			UnityEngine.Object.Destroy(componentInChildren.gameObject);
		}
	}

	// Token: 0x060090A9 RID: 37033 RVA: 0x003D27C4 File Offset: 0x003D09C4
	public static void ClearTextBox(Transform parent)
	{
		if (TextBoxManager.extantTextBoxMap.ContainsKey(parent))
		{
			TextBoxManager component = TextBoxManager.extantTextBoxMap[parent].GetComponent<TextBoxManager>();
			component.StartCoroutine(component.HandleScaleDown());
			TextBoxManager.extantTextPointList.Remove(parent);
			TextBoxManager.extantTextBoxMap.Remove(parent);
		}
	}

	// Token: 0x060090AA RID: 37034 RVA: 0x003D2818 File Offset: 0x003D0A18
	public void HandleLifespan(GameObject target, Transform parent, float lifespan)
	{
		base.StartCoroutine(this.TextBoxLifespanCR(target, parent, lifespan));
	}

	// Token: 0x060090AB RID: 37035 RVA: 0x003D282C File Offset: 0x003D0A2C
	public void ShowContinueText()
	{
		if (this.continueTextMesh)
		{
			base.StartCoroutine(this.ShowContinueTextCR());
		}
	}

	// Token: 0x060090AC RID: 37036 RVA: 0x003D284C File Offset: 0x003D0A4C
	public void SkipTextReveal()
	{
		this.skipTextReveal = true;
	}

	// Token: 0x060090AD RID: 37037 RVA: 0x003D2858 File Offset: 0x003D0A58
	private IEnumerator TextBoxLifespanCR(GameObject target, Transform parent, float lifespan)
	{
		yield return null;
		while (this.m_isRevealingText)
		{
			yield return null;
		}
		yield return new WaitForSeconds(lifespan);
		if (parent != null)
		{
			TextBoxManager.ClearTextBox(parent);
		}
		else
		{
			UnityEngine.Object.Destroy(target);
		}
		yield break;
	}

	// Token: 0x060090AE RID: 37038 RVA: 0x003D2888 File Offset: 0x003D0A88
	private IEnumerator ShowContinueTextCR()
	{
		float delay = 0.3f;
		for (;;)
		{
			this.continueTextMesh.text = ".";
			this.continueTextMesh.Commit();
			yield return new WaitForSeconds(delay);
			this.continueTextMesh.text = "..";
			this.continueTextMesh.Commit();
			yield return new WaitForSeconds(delay);
			this.continueTextMesh.text = "...";
			this.continueTextMesh.Commit();
			yield return new WaitForSeconds(delay * 2f);
			this.continueTextMesh.text = string.Empty;
			this.continueTextMesh.Commit();
			yield return new WaitForSeconds(delay);
		}
		yield break;
	}

	// Token: 0x060090AF RID: 37039 RVA: 0x003D28A4 File Offset: 0x003D0AA4
	private IEnumerator RevealTextCharacters(string strippedString)
	{
		this.m_isRevealingText = true;
		this.skipTextReveal = false;
		while (this.IsScalingUp)
		{
			yield return null;
		}
		float elapsed = 0f;
		float duration = (float)strippedString.Length / this.TEXT_REVEAL_SPEED;
		if (this.TEXT_REVEAL_SPEED > 10000f)
		{
			duration = 0f;
		}
		Renderer boxRenderer = this.boxSpriteTransform.GetComponent<Renderer>();
		this.textMesh.inlineStyling = true;
		this.textMesh.color = Color.black;
		this.textMesh.visibleCharacters = 0;
		int visibleCharacters = 0;
		if (duration > 0f)
		{
			while (elapsed < duration)
			{
				elapsed += ((!TextBoxManager.TIME_INVARIANT) ? BraveTime.DeltaTime : GameManager.INVARIANT_DELTA_TIME);
				if (this.skipTextReveal)
				{
					elapsed = duration;
				}
				float t = elapsed / duration;
				int numCharacters = Mathf.FloorToInt((float)strippedString.Length * t);
				if (numCharacters > 100000)
				{
					numCharacters = 0;
				}
				if (numCharacters > visibleCharacters && boxRenderer.isVisible)
				{
					visibleCharacters = numCharacters;
					if (!string.IsNullOrEmpty(this.audioTag))
					{
						AkSoundEngine.PostEvent("Play_CHR_" + this.audioTag + "_voice_01", base.gameObject);
					}
				}
				this.textMesh.visibleCharacters = visibleCharacters;
				this.textMesh.text = strippedString;
				this.textMesh.Commit();
				yield return null;
			}
		}
		this.textMesh.visibleCharacters = int.MaxValue;
		this.textMesh.text = strippedString;
		this.textMesh.Commit();
		this.m_isRevealingText = false;
		this.skipTextReveal = false;
		yield break;
	}

	// Token: 0x060090B0 RID: 37040 RVA: 0x003D28C8 File Offset: 0x003D0AC8
	private void LateUpdate()
	{
		TextBoxManager.UNPIXELATED_LAYER = ((!Pixelator.Instance.DoFinalNonFadedLayer) ? LayerMask.NameToLayer("Unoccluded") : LayerMask.NameToLayer("Unfaded"));
		if (TextBoxManager.PIXELATED_LAYER == -1)
		{
			TextBoxManager.PIXELATED_LAYER = LayerMask.NameToLayer("FG_Critical");
		}
		if (GameManager.Instance.IsPaused && base.gameObject.layer == TextBoxManager.UNPIXELATED_LAYER)
		{
			base.gameObject.SetLayerRecursively(TextBoxManager.PIXELATED_LAYER);
		}
		else if (!GameManager.Instance.IsPaused && base.gameObject.layer != TextBoxManager.UNPIXELATED_LAYER)
		{
			base.gameObject.SetLayerRecursively(TextBoxManager.UNPIXELATED_LAYER);
		}
		this.UpdateForCameraPosition();
	}

	// Token: 0x060090B1 RID: 37041 RVA: 0x003D2990 File Offset: 0x003D0B90
	public void UpdateForCameraPosition()
	{
		if (!this.fitToScreen)
		{
			return;
		}
		Vector3 vector = base.transform.position - this.m_basePosition;
		Vector2 vector2 = this.boxSprite.transform.position.XY() - vector.XY();
		Vector2 vector3 = this.boxSprite.transform.position.XY() + this.boxSprite.dimensions / 16f - vector.XY();
		Camera component = GameManager.Instance.MainCameraController.GetComponent<Camera>();
		Vector2 vector4 = component.WorldToViewportPoint(vector2.ToVector3ZUp(vector2.y));
		Vector2 vector5 = component.WorldToViewportPoint(vector3.ToVector3ZUp(vector3.y));
		float num = Mathf.Min(vector4.x, 0.1f) + Mathf.Max(vector5.x - 1f, -0.1f);
		float num2 = Mathf.Min(vector4.y, 0.1f) + Mathf.Max(vector5.y - 1f, -0.1f);
		float num3 = num * 480f / 16f;
		float num4 = num2 * 270f / 16f;
		base.transform.position = (this.m_basePosition + new Vector3(-num3, -num4, 0f)).Quantize(0.0625f);
		if (!this.IsScalingUp && !this.IsScalingDown)
		{
			base.transform.localScale = Vector3.one * this.ScaleFactor;
		}
	}

	// Token: 0x060090B2 RID: 37042 RVA: 0x003D2B38 File Offset: 0x003D0D38
	private string ToUpperExcludeSprites(string inputString)
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		foreach (char c in inputString)
		{
			if (c == '[' && !flag)
			{
				flag = true;
			}
			else if (c == ']' && flag)
			{
				flag = false;
			}
			else if (!flag)
			{
				c = char.ToUpperInvariant(c);
			}
			stringBuilder.Append(c);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060090B3 RID: 37043 RVA: 0x003D2BB4 File Offset: 0x003D0DB4
	public void SetText(string text, Vector3 worldPosition, bool instant = true, TextBoxManager.BoxSlideOrientation slideOrientation = TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, bool showContinueText = true, bool UseAlienLanguage = false, bool clampThoughtBubble = false)
	{
		if (this.boxSpriteTransform == null)
		{
			this.boxSpriteTransform = this.boxSprite.transform;
		}
		if (this.textMeshTransform == null)
		{
			this.textMeshTransform = this.textMesh.transform;
		}
		if (this.continueTextMeshTransform == null && this.continueTextMesh)
		{
			this.continueTextMeshTransform = this.continueTextMesh.transform;
		}
		if (text == string.Empty)
		{
			return;
		}
		text = text.Replace("\\n", Environment.NewLine);
		float x = this.boxSpriteTransform.localPosition.x;
		float num = -x / (this.boxSprite.dimensions.x / 16f);
		string text2 = this.textMesh.GetStrippedWoobleString(text);
		if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.CHINESE)
		{
			this.textMesh.LineSpacing = 0.125f;
		}
		else
		{
			this.textMesh.LineSpacing = 0f;
		}
		if (GameManager.Options.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.JAPANESE)
		{
			this.textMesh.wordWrapWidth = 350;
		}
		else if (text2.Length < 25)
		{
			this.textMesh.wordWrapWidth = 250;
		}
		else
		{
			this.textMesh.wordWrapWidth = 200 + (text2.Length - 25) / 4;
			if (!text2.EndsWith(" "))
			{
				text2 += " ";
			}
		}
		if (Application.isPlaying)
		{
			bool flag = false;
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				flag |= GameManager.Instance.AllPlayers[i].UnderstandsGleepGlorp;
			}
			if (UseAlienLanguage && !flag)
			{
				text2 = this.ToUpperExcludeSprites(text2);
				this.textMesh.font = GameManager.Instance.DefaultAlienConversationFont;
			}
			else
			{
				this.textMesh.font = GameManager.Instance.DefaultNormalConversationFont;
			}
		}
		this.textMesh.text = text2;
		this.textMesh.CheckFontsForLanguage();
		this.textMesh.ForceBuild();
		Bounds trueBounds = this.textMesh.GetTrueBounds();
		float num2 = Mathf.Ceil((trueBounds.size.x + this.boxPadding * 2f + this.additionalPaddingLeft + this.additionalPaddingRight) * 16f) / 16f;
		float num3 = Mathf.Ceil((trueBounds.size.y + this.boxPadding * 2f + this.additionalPaddingTop + this.additionalPaddingBottom) * 16f) / 16f;
		if (showContinueText && this.continueTextMesh)
		{
			num2 += this.continueTextMesh.GetEstimatedMeshBoundsForString("...").extents.x * 2f;
		}
		float num4 = num2 * 16f;
		float num5 = num3 * 16f;
		if (clampThoughtBubble)
		{
			float num6 = 47f + (Mathf.Max(47f, num4) - 47f).Quantize(23f, VectorConversions.Floor);
			float num7 = 57f + (Mathf.Max(57f, num4) - 57f).Quantize(23f, VectorConversions.Floor);
			if (num6 < num4)
			{
				num6 += 23f;
			}
			if (num7 < num4)
			{
				num7 += 23f;
			}
			float num8 = Mathf.Abs(num6 - num4);
			float num9 = Mathf.Abs(num7 - num4);
			num4 = ((num8 >= num9) ? num7 : num6);
		}
		Vector3 vector = new Vector3(0f, 0f);
		tk2dSpriteDefinition currentSpriteDef = this.boxSprite.GetCurrentSpriteDef();
		Vector3 boundsDataExtents = currentSpriteDef.boundsDataExtents;
		if (currentSpriteDef.texelSize.x != 0f && currentSpriteDef.texelSize.y != 0f && boundsDataExtents.x != 0f && boundsDataExtents.y != 0f)
		{
			vector = new Vector3(boundsDataExtents.x / currentSpriteDef.texelSize.x, boundsDataExtents.y / currentSpriteDef.texelSize.y, 1f);
		}
		vector = Vector3.Max(vector, Vector3.one);
		num4 = Mathf.Max(num4, (this.boxSprite.borderLeft + this.boxSprite.borderRight) * vector.x);
		num5 = Mathf.Max(num5, (this.boxSprite.borderTop + this.boxSprite.borderBottom) * vector.y);
		this.boxSprite.dimensions = new Vector2(num4, num5);
		if (this.boxSprite.dimensions.x < (this.boxSprite.borderLeft + this.boxSprite.borderRight) * vector.x || this.boxSprite.dimensions.y < (this.boxSprite.borderTop + this.boxSprite.borderBottom) * vector.y)
		{
			this.boxSprite.BorderOnly = true;
		}
		else
		{
			this.boxSprite.BorderOnly = false;
		}
		this.boxSprite.ForceBuild();
		this.textMesh.color = this.textColor;
		if (instant)
		{
			this.textMesh.text = this.textMesh.PreprocessWoobleSignifiers(text);
			if (UseAlienLanguage)
			{
				this.textMesh.text = this.ToUpperExcludeSprites(this.textMesh.text);
			}
			this.textMesh.Commit();
		}
		else
		{
			this.textMesh.text = string.Empty;
			this.textMesh.Commit();
			string text3 = this.textMesh.PreprocessWoobleSignifiers(text);
			if (UseAlienLanguage)
			{
				text3 = this.ToUpperExcludeSprites(text3);
			}
			base.StartCoroutine(this.RevealTextCharacters(text3));
		}
		float num10 = BraveMathCollege.QuantizeFloat(this.boxSprite.dimensions.y / 16f - this.boxPadding - this.additionalPaddingTop, 0.0625f);
		if (this.textMesh.anchor == TextAnchor.UpperLeft)
		{
			this.textMeshTransform.localPosition = new Vector3(this.boxPadding + this.additionalPaddingLeft, num10, -0.1f);
		}
		else if (this.textMesh.anchor == TextAnchor.UpperCenter)
		{
			this.textMeshTransform.localPosition = new Vector3(num2 / 2f, num10, -0.1f);
		}
		this.textMeshTransform.localPosition += new Vector3(0.0234375f, 0.0234375f, 0f);
		if (this.continueTextMesh)
		{
			if (showContinueText)
			{
				Bounds estimatedMeshBoundsForString = this.continueTextMesh.GetEstimatedMeshBoundsForString("...");
				this.continueTextMeshTransform.localPosition = new Vector3(num2 - this.continuePaddingRight - estimatedMeshBoundsForString.extents.x * 2f, this.continuePaddingBottom, -0.1f);
			}
			else
			{
				this.continueTextMesh.text = string.Empty;
				this.continueTextMesh.Commit();
			}
		}
		switch (slideOrientation)
		{
		case TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT:
			this.boxSpriteTransform.localPosition = this.boxSpriteTransform.localPosition.WithX(BraveMathCollege.QuantizeFloat(-1f * num * (this.boxSprite.dimensions.x / 16f), 0.0625f));
			break;
		case TextBoxManager.BoxSlideOrientation.FORCE_RIGHT:
			num = 0.1f;
			this.boxSpriteTransform.localPosition = this.boxSpriteTransform.localPosition.WithX(BraveMathCollege.QuantizeFloat(-1f * num * (this.boxSprite.dimensions.x / 16f), 0.0625f));
			break;
		case TextBoxManager.BoxSlideOrientation.FORCE_LEFT:
			num = 0.85f;
			this.boxSpriteTransform.localPosition = this.boxSpriteTransform.localPosition.WithX(BraveMathCollege.QuantizeFloat(-1f * num * (this.boxSprite.dimensions.x / 16f), 0.0625f));
			break;
		default:
			this.boxSpriteTransform.localPosition = this.boxSpriteTransform.localPosition.WithX(BraveMathCollege.QuantizeFloat(-1f * num * (this.boxSprite.dimensions.x / 16f), 0.0625f));
			break;
		}
		if (slideOrientation == TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT)
		{
			float num11 = ((!Application.isPlaying) ? 0f : GameManager.Instance.MainCameraController.transform.position.x);
			if (worldPosition.x > num11)
			{
				this.boxSpriteTransform.localPosition = this.boxSpriteTransform.localPosition.WithX(-1f * (1f - num) * (this.boxSprite.dimensions.x / 16f));
			}
		}
		base.transform.position = worldPosition;
		base.transform.localScale = Vector3.one * this.ScaleFactor;
		this.m_basePosition = worldPosition;
		this.UpdateForCameraPosition();
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unoccluded"));
	}

	// Token: 0x060090B4 RID: 37044 RVA: 0x003D3558 File Offset: 0x003D1758
	public static float GetEstimatedReadingTime(string text)
	{
		int num = 0;
		bool flag = false;
		foreach (char c in text)
		{
			if (c == '{' || c == '[')
			{
				flag = true;
			}
			else if (c == '}' || c == ']')
			{
				flag = false;
			}
			if (!flag && !char.IsWhiteSpace(c))
			{
				num++;
			}
		}
		int num2 = 987;
		switch (GameManager.Options.CurrentLanguage)
		{
		case StringTableManager.GungeonSupportedLanguages.ENGLISH:
			num2 = 987;
			break;
		case StringTableManager.GungeonSupportedLanguages.RUBEL_TEST:
			num2 = 1000;
			break;
		case StringTableManager.GungeonSupportedLanguages.FRENCH:
			num2 = 998;
			break;
		case StringTableManager.GungeonSupportedLanguages.SPANISH:
			num2 = 1025;
			break;
		case StringTableManager.GungeonSupportedLanguages.GERMAN:
			num2 = 920;
			break;
		case StringTableManager.GungeonSupportedLanguages.BRAZILIANPORTUGUESE:
			num2 = 913;
			break;
		case StringTableManager.GungeonSupportedLanguages.JAPANESE:
			num2 = 357;
			break;
		case StringTableManager.GungeonSupportedLanguages.KOREAN:
			num2 = 357;
			break;
		case StringTableManager.GungeonSupportedLanguages.RUSSIAN:
			num2 = 986;
			break;
		case StringTableManager.GungeonSupportedLanguages.POLISH:
			num2 = 916;
			break;
		case StringTableManager.GungeonSupportedLanguages.CHINESE:
			num2 = 357;
			break;
		}
		return (float)num / ((float)num2 / 60f);
	}

	// Token: 0x04009897 RID: 39063
	public static bool TIME_INVARIANT = false;

	// Token: 0x04009898 RID: 39064
	private const float TEXT_REVEAL_SPEED_INSTANT = 3.4028235E+38f;

	// Token: 0x04009899 RID: 39065
	private const float TEXT_REVEAL_SPEED_FAST = 100f;

	// Token: 0x0400989A RID: 39066
	private const float TEXT_REVEAL_SPEED_SLOW = 27f;

	// Token: 0x0400989B RID: 39067
	private const float SCALE_UP_TIME = 0.06f;

	// Token: 0x0400989C RID: 39068
	private const float SCALE_DOWN_TIME = 0.06f;

	// Token: 0x0400989D RID: 39069
	[SerializeField]
	private tk2dSlicedSprite boxSprite;

	// Token: 0x0400989E RID: 39070
	[SerializeField]
	private tk2dTextMesh textMesh;

	// Token: 0x0400989F RID: 39071
	[SerializeField]
	private tk2dTextMesh continueTextMesh;

	// Token: 0x040098A0 RID: 39072
	public float additionalPaddingLeft;

	// Token: 0x040098A1 RID: 39073
	public float additionalPaddingRight;

	// Token: 0x040098A2 RID: 39074
	public float additionalPaddingTop;

	// Token: 0x040098A3 RID: 39075
	public float additionalPaddingBottom;

	// Token: 0x040098A4 RID: 39076
	public float continuePaddingRight;

	// Token: 0x040098A5 RID: 39077
	public float continuePaddingBottom;

	// Token: 0x040098A6 RID: 39078
	public bool fitToScreen;

	// Token: 0x040098A7 RID: 39079
	public Color textColor = Color.black;

	// Token: 0x040098A8 RID: 39080
	private static float BOX_PADDING = 0.5f;

	// Token: 0x040098A9 RID: 39081
	private static float INFOBOX_PADDING = 0.25f;

	// Token: 0x040098AA RID: 39082
	private bool m_isRevealingText;

	// Token: 0x040098AB RID: 39083
	private bool skipTextReveal;

	// Token: 0x040098AE RID: 39086
	private string audioTag = string.Empty;

	// Token: 0x040098AF RID: 39087
	private float boxPadding;

	// Token: 0x040098B0 RID: 39088
	private Vector3 m_basePosition;

	// Token: 0x040098B1 RID: 39089
	private Transform boxSpriteTransform;

	// Token: 0x040098B2 RID: 39090
	private Transform textMeshTransform;

	// Token: 0x040098B3 RID: 39091
	private Transform continueTextMeshTransform;

	// Token: 0x040098B4 RID: 39092
	private static List<Transform> extantTextPointList = new List<Transform>();

	// Token: 0x040098B5 RID: 39093
	private static Dictionary<Transform, GameObject> extantTextBoxMap = new Dictionary<Transform, GameObject>();

	// Token: 0x040098B6 RID: 39094
	private static int UNPIXELATED_LAYER = -1;

	// Token: 0x040098B7 RID: 39095
	private static int PIXELATED_LAYER = -1;

	// Token: 0x020017FA RID: 6138
	public enum BoxSlideOrientation
	{
		// Token: 0x040098B9 RID: 39097
		NO_ADJUSTMENT,
		// Token: 0x040098BA RID: 39098
		FORCE_RIGHT,
		// Token: 0x040098BB RID: 39099
		FORCE_LEFT
	}
}
