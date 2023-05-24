using System;
using System.Collections;
using Dungeonator;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02001764 RID: 5988
public class FinalIntroSequenceManager : MonoBehaviour
{
	// Token: 0x06008B5C RID: 35676 RVA: 0x003A0810 File Offset: 0x0039EA10
	private void Awake()
	{
		if (!Foyer.DoIntroSequence)
		{
			return;
		}
		GameManager.Instance.IsSelectingCharacter = true;
		this.m_inFoyer = true;
		if (!this.m_inFoyer)
		{
			GameManager.PreventGameManagerExistence = true;
		}
		if (GameManager.Options == null)
		{
			GameOptions.Load();
		}
		Pixelator.DEBUG_LogSystemRenderingData();
	}

	// Token: 0x06008B5D RID: 35677 RVA: 0x003A0860 File Offset: 0x0039EA60
	public void TriggerSequence()
	{
		if (Foyer.DoIntroSequence)
		{
			GameManager.Instance.StartCoroutine(this.CoreSequence());
			base.StartCoroutine(this.HandleBackgroundSkipChecks());
		}
	}

	// Token: 0x06008B5E RID: 35678 RVA: 0x003A088C File Offset: 0x0039EA8C
	private void OnDestroy()
	{
		GameManager.PreventGameManagerExistence = false;
	}

	// Token: 0x06008B5F RID: 35679 RVA: 0x003A0894 File Offset: 0x0039EA94
	private bool QuickStartAvailable()
	{
		return GameStatsManager.Instance != null && GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.NUMBER_ATTEMPTS) >= 1f;
	}

	// Token: 0x06008B60 RID: 35680 RVA: 0x003A08B8 File Offset: 0x0039EAB8
	private IEnumerator MoveQuickstartOffscreen()
	{
		float elapsed = 0f;
		Vector3 startLocal = this.QuickStartObject.transform.localPosition;
		Vector3 offsetLocal = startLocal + new Vector3(0f, -3f, 0f);
		this.QuickStartController.HeightOffGround = 3f;
		while (elapsed < 0.5f)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / 0.5f;
			this.QuickStartObject.transform.localPosition = Vector3.Lerp(startLocal, offsetLocal, t);
			this.QuickStartController.UpdateZDepth();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008B61 RID: 35681 RVA: 0x003A08D4 File Offset: 0x0039EAD4
	private IEnumerator MoveQuickstartOnScreen()
	{
		float elapsed = 0f;
		Vector3 startLocal = this.QuickStartObject.transform.localPosition;
		Vector3 offsetLocal = startLocal + new Vector3(0f, 3f, 0f);
		this.QuickStartController.HeightOffGround = 3f;
		while (elapsed < 1f)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / 1f;
			this.QuickStartObject.transform.localPosition = Vector3.Lerp(startLocal, offsetLocal, t);
			this.QuickStartController.UpdateZDepth();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008B62 RID: 35682 RVA: 0x003A08F0 File Offset: 0x0039EAF0
	private IEnumerator Start()
	{
		if (!this.QuickStartAvailable() || !Foyer.DoIntroSequence)
		{
			this.QuickStartObject.SetActive(false);
		}
		else
		{
			this.QuickStartObject.SetActive(true);
			tk2dTextMesh componentInChildren = this.QuickStartObject.GetComponentInChildren<tk2dTextMesh>();
			componentInChildren.text = StringTableManager.GetString("#MAINMENU_NEW_QUICKSTART");
			base.StartCoroutine(this.MoveQuickstartOnScreen());
		}
		yield return null;
		if (Foyer.DoIntroSequence)
		{
			CameraController mainCameraController = GameManager.Instance.MainCameraController;
			mainCameraController.SetManualControl(true, false);
			mainCameraController.OverridePosition = base.transform.parent.position + new Vector3(16f, 3.5f, -5f);
			base.transform.parent.position += CameraController.PLATFORM_CAMERA_OFFSET;
			RenderSettings.ambientLight = Color.white;
		}
		if (this.QuickStartObject.activeSelf)
		{
			tk2dTextMesh componentInChildren2 = this.QuickStartObject.GetComponentInChildren<tk2dTextMesh>();
			componentInChildren2.text = StringTableManager.GetString("#MAINMENU_NEW_QUICKSTART");
		}
		yield break;
	}

	// Token: 0x06008B63 RID: 35683 RVA: 0x003A090C File Offset: 0x0039EB0C
	private IEnumerator HandleBackgroundSkipChecks()
	{
		yield return null;
		for (;;)
		{
			if (this.QuickStartObject.activeSelf)
			{
				if (!BraveInput.PlayerlessInstance.IsKeyboardAndMouse(false))
				{
					this.QuickStartController.gameObject.SetActive(true);
					this.QuickStartController.renderer.enabled = true;
					this.QuickStartKeyboard.gameObject.SetActive(false);
				}
				else
				{
					this.QuickStartKeyboard.gameObject.SetActive(true);
					this.QuickStartController.gameObject.SetActive(false);
				}
			}
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
			{
				this.m_skipCycle = true;
			}
			if (!this.m_isDoingQuickStart && !this.m_skipCycle)
			{
				if (this.QuickStartAvailable() && (BraveInput.PlayerlessInstance.ActiveActions.Device.Action4.WasPressed || Input.GetKeyDown(KeyCode.Q)))
				{
					this.m_skipCycle = true;
					this.m_isDoingQuickStart = true;
					base.StartCoroutine(this.DoQuickStart());
				}
				if (BraveInput.PlayerlessInstance.ActiveActions.Device.Action1.WasPressed || BraveInput.PlayerlessInstance.ActiveActions.Device.Action2.WasPressed || BraveInput.PlayerlessInstance.ActiveActions.Device.Action3.WasPressed || BraveInput.PlayerlessInstance.ActiveActions.Device.CommandWasPressed || BraveInput.PlayerlessInstance.ActiveActions.MenuSelectAction.WasPressed)
				{
					this.m_skipCycle = true;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008B64 RID: 35684 RVA: 0x003A0928 File Offset: 0x0039EB28
	private IEnumerator SkippableWait(float duration)
	{
		float elapsed = 0f;
		while (elapsed < duration)
		{
			if (this.m_skipCycle)
			{
				break;
			}
			elapsed += Time.deltaTime;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008B65 RID: 35685 RVA: 0x003A094C File Offset: 0x0039EB4C
	private IEnumerator CoreSequence()
	{
		this.IsDoingIntro = true;
		DebugTime.Log("FinalIntroSequenceManager.CoreSequence()", new object[0]);
		yield return new WaitForSeconds(0.5f);
		AkSoundEngine.PostEvent("Play_MUS_map_intro_01", GameManager.Instance.gameObject);
		if (!this.m_skipCycle)
		{
			yield return base.StartCoroutine(this.HandleDodgeRollLogo());
		}
		if (!this.m_skipCycle)
		{
			yield return base.StartCoroutine(this.SkippableWait(2.5f));
		}
		if (!this.m_skipCycle)
		{
			yield return base.StartCoroutine(this.FadeToBlack(0.5f, false, false));
		}
		if (!this.m_skipCycle)
		{
			yield return base.StartCoroutine(this.HandleDevolverLogo());
		}
		if (!this.m_skipCycle)
		{
			yield return base.StartCoroutine(this.SkippableWait(2.5f));
		}
		if (!this.m_skipCycle)
		{
			yield return base.StartCoroutine(this.FadeToBlack(0.5f, false, false));
		}
		if (this.m_skipCycle)
		{
			this.m_skipCycle = false;
			AkSoundEngine.PostEvent("Play_MUS_Intro_Beat_02", GameManager.Instance.gameObject);
			yield return base.StartCoroutine(this.FadeToBlack(0.5f, true, false));
			yield return base.StartCoroutine(this.SkippableWait(0.5f));
		}
		this.PT1_DodgeRoll_Guy.StopAndResetFrame();
		this.PT1_DodgeRoll_Logo.StopAndResetFrame();
		this.PT2_Devolver_Logo.StopAndResetFrame();
		this.PT1_DodgeRoll_Guy.renderer.enabled = false;
		this.PT1_DodgeRoll_Logo.renderer.enabled = false;
		this.PT2_Devolver_Logo.renderer.enabled = false;
		if (!this.m_isDoingQuickStart)
		{
			if (this.m_inFoyer)
			{
				TitleDioramaController tdc = UnityEngine.Object.FindObjectOfType<TitleDioramaController>();
				if (tdc && tdc.FadeQuad)
				{
					tdc.FadeQuad.enabled = false;
				}
				yield return base.StartCoroutine(this.MoveQuickstartOffscreen());
				RenderSettings.ambientLight = GameManager.Instance.Dungeon.decoSettings.ambientLightColor;
				yield return base.StartCoroutine(this.LegendCore());
				CameraController mainCameraController = GameManager.Instance.MainCameraController;
				mainCameraController.OnFinishedFrame = (Action)Delegate.Remove(mainCameraController.OnFinishedFrame, new Action(this.HandleOffsetUpdate));
				if (this.m_isDoingQuickStart)
				{
					yield break;
				}
				if (this.IntroTextMesh)
				{
				}
				AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
				this.IsDoingIntro = false;
				if (tdc)
				{
					while (!tdc.IsRevealed(false))
					{
						yield return null;
					}
					if (this.IntroTextMesh)
					{
						this.IntroTextMesh.gameObject.SetActive(false);
					}
				}
			}
			else
			{
				AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("tt_foyer");
				asyncOperation.allowSceneActivation = false;
				GameManager.PreventGameManagerExistence = false;
				asyncOperation.allowSceneActivation = true;
				this.IsDoingIntro = false;
			}
		}
		yield break;
	}

	// Token: 0x06008B66 RID: 35686 RVA: 0x003A0968 File Offset: 0x0039EB68
	private void SetIntroString(bool fadePrevious, bool resetToCenter, params string[] keys)
	{
		base.StartCoroutine(this.SetIntroStringCR(fadePrevious, resetToCenter, -1f, keys));
	}

	// Token: 0x06008B67 RID: 35687 RVA: 0x003A0980 File Offset: 0x0039EB80
	private void SetIntroString(bool fadePrevious, bool resetToCenter, float customDura, params string[] keys)
	{
		base.StartCoroutine(this.SetIntroStringCR(fadePrevious, resetToCenter, customDura, keys));
	}

	// Token: 0x06008B68 RID: 35688 RVA: 0x003A0994 File Offset: 0x0039EB94
	private IEnumerator SetIntroStringCR(bool fadePrevious, bool resetToCenter, float customDura, params string[] keys)
	{
		float ela = 0f;
		float dura = ((customDura <= 0f) ? 1f : customDura);
		if (this.customTextFadeOutTime > 0f)
		{
			dura = this.customTextFadeOutTime;
		}
		if (fadePrevious)
		{
			while (ela < dura)
			{
				ela += GameManager.INVARIANT_DELTA_TIME;
				float t = 1f - ela / dura;
				Color textColor = Color.Lerp(Color.black, Color.white, t);
				this.IntroTextMesh.color = textColor;
				if (this.m_skipLegend)
				{
					yield break;
				}
				yield return null;
			}
			this.IntroTextMesh.text = string.Empty;
			yield return new WaitForSeconds(1f);
		}
		if (resetToCenter)
		{
			this.IntroTextMesh.transform.localPosition = new Vector3(0f, -0.8125f, 10f) + new Vector3(0.015625f, 0.015625f, 0f) + CameraController.PLATFORM_CAMERA_OFFSET;
			this.m_currentIntroTextMeshLocalPosition = this.IntroTextMesh.transform.localPosition;
			this.IntroTextMesh.LineSpacing = -0.25f;
		}
		this.IntroTextMesh.color = Color.white;
		ela = 0f;
		dura = 1f;
		if (this.customTextFadeInTime > 0f)
		{
			dura = this.customTextFadeInTime;
		}
		while (ela < dura)
		{
			if (this.m_skipLegend)
			{
				yield break;
			}
			ela += GameManager.INVARIANT_DELTA_TIME;
			float t2 = ela / dura;
			Color textColor2 = Color.Lerp(Color.black, Color.white, t2);
			string colorTag = "^C" + BraveUtility.ColorToHexWithAlpha(textColor2);
			string introString = string.Empty;
			bool hasUsedColorTag = false;
			for (int i = 0; i < keys.Length; i++)
			{
				if (i > 0)
				{
					introString += "\n";
				}
				if (!hasUsedColorTag && (i == keys.Length - 1 || (keys[i + 1] == string.Empty && (keys.Length == 2 || (keys.Length == 3 && keys[2] == string.Empty)))))
				{
					hasUsedColorTag = true;
					introString += colorTag;
				}
				if (keys[i] != string.Empty)
				{
					introString += StringTableManager.GetIntroString(keys[i]);
				}
			}
			this.IntroTextMesh.text = introString;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008B69 RID: 35689 RVA: 0x003A09CC File Offset: 0x0039EBCC
	private void UpdateText(float totalElapsed, float cardElapsed, int currentCardIndex, ref int currentIndex)
	{
		if (currentCardIndex >= 0 && currentCardIndex < this.IntroCards.Length)
		{
			string[] targetKeys = this.IntroCards[currentCardIndex].GetTargetKeys(cardElapsed);
			bool flag = false;
			if (this.m_lastAssignedStrings == null || targetKeys.Length != this.m_lastAssignedStrings.Length)
			{
				flag = true;
			}
			else
			{
				for (int i = 0; i < targetKeys.Length; i++)
				{
					if (targetKeys[i] != this.m_lastAssignedStrings[i])
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				this.customTextFadeInTime = -1f;
				this.customTextFadeOutTime = -1f;
				bool flag2 = this.m_cachedLastFirstString != targetKeys[0];
				this.m_cachedLastFirstString = targetKeys[0];
				this.m_lastAssignedStrings = targetKeys;
				this.SetIntroString(flag2, false, targetKeys);
			}
		}
	}

	// Token: 0x06008B6A RID: 35690 RVA: 0x003A0A90 File Offset: 0x0039EC90
	private IEnumerator LegendSkippableWait(float dura)
	{
		float ela = 0f;
		while (ela < dura)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			this.UpdateSkipLegend();
			if (this.m_skipLegend)
			{
				this.customTextFadeInTime = -1f;
				this.customTextFadeOutTime = -1f;
				yield break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008B6B RID: 35691 RVA: 0x003A0AB4 File Offset: 0x0039ECB4
	private IEnumerator ContinueMovingPreviousCard(FinalIntroSequenceCard previousCard)
	{
		Vector3 previousCardVelocity = previousCard.EndCameraTransform.position - previousCard.StartCameraTransform.position;
		previousCardVelocity = previousCardVelocity.normalized * (previousCardVelocity.magnitude / previousCard.PanTime);
		float ela = 0f;
		float dura = 1f;
		while (ela < dura)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			Vector3 delta = previousCardVelocity * GameManager.INVARIANT_DELTA_TIME;
			previousCard.transform.position += delta * -1f;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008B6C RID: 35692 RVA: 0x003A0AD0 File Offset: 0x0039ECD0
	private void UpdateSkipLegend()
	{
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
		{
			this.m_skipLegend = true;
		}
		if (BraveInput.PlayerlessInstance.ActiveActions.IntroSkipActionPressed())
		{
			this.m_skipLegend = true;
		}
	}

	// Token: 0x06008B6D RID: 35693 RVA: 0x003A0B3C File Offset: 0x0039ED3C
	private void HandleOffsetUpdate()
	{
		this.IntroTextMesh.transform.parent = null;
		this.IntroTextMesh.transform.position = GameManager.Instance.MainCameraController.transform.position + this.m_currentIntroTextMeshLocalPosition;
	}

	// Token: 0x06008B6E RID: 35694 RVA: 0x003A0B8C File Offset: 0x0039ED8C
	private IEnumerator LegendCore()
	{
		if (this.FadeMaterial)
		{
			this.FadeMaterial.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
		}
		CameraController cc = GameManager.Instance.MainCameraController;
		Camera c = cc.Camera;
		yield return base.StartCoroutine(this.LegendSkippableWait(2f));
		if (!this.m_skipLegend)
		{
			int cardIndex = 0;
			FinalIntroSequenceCard currentCard = this.IntroCards[0];
			cc.SetManualControl(true, false);
			cc.OverridePosition = currentCard.StartCameraTransform.position.XY();
			Pixelator.Instance.DoFinalNonFadedLayer = true;
			Pixelator.Instance.CompositePixelatedUnfadedLayer = true;
			this.IntroTextMesh.transform.parent = c.transform;
			this.IntroTextMesh.transform.localPosition = new Vector3(0f, -0.8125f, 10f) + new Vector3(0.015625f, 0.015625f, 0f) + CameraController.PLATFORM_CAMERA_OFFSET;
			this.m_currentIntroTextMeshLocalPosition = this.IntroTextMesh.transform.localPosition;
			this.IntroTextMesh.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unfaded"));
			CameraController cameraController = cc;
			cameraController.OnFinishedFrame = (Action)Delegate.Combine(cameraController.OnFinishedFrame, new Action(this.HandleOffsetUpdate));
			this.customTextFadeInTime = this.FirstTextFadeInTime;
			this.SetIntroString(false, false, new string[]
			{
				this.introKeys[0],
				string.Empty
			});
			int currentStringIndex = 0;
			float additionalWaitTime = 1f;
			Pixelator.Instance.FadeToBlack(0.7f, true, this.FirstTextHoldTime + this.FirstTextFadeInTime + 1f + additionalWaitTime);
			Pixelator.Instance.SetVignettePower(2.25f);
			Pixelator.Instance.DoOcclusionLayer = false;
			yield return base.StartCoroutine(this.LegendSkippableWait(this.FirstTextFadeInTime + this.FirstTextHoldTime));
			if (!this.m_skipLegend)
			{
				this.customTextFadeInTime = currentCard.CustomTextFadeInTime;
				this.customTextFadeOutTime = currentCard.CustomTextFadeOutTime;
				this.SetIntroString(true, false, new string[] { string.Empty });
				yield return base.StartCoroutine(this.LegendSkippableWait(additionalWaitTime));
				if (!this.m_skipLegend)
				{
					this.UpdateText(0f, 0f, cardIndex, ref currentStringIndex);
					this.IntroTextMesh.transform.localPosition = new Vector3(0f, -7f, 10f) + new Vector3(0.015625f, 0.015625f, 0f) + CameraController.PLATFORM_CAMERA_OFFSET;
					this.m_currentIntroTextMeshLocalPosition = this.IntroTextMesh.transform.localPosition;
					yield return base.StartCoroutine(this.LegendSkippableWait(1f));
					if (!this.m_skipLegend)
					{
						bool continueDoing = true;
						float ela = -currentCard.StartHoldTime;
						float totalElapsed = 0f;
						for (int i = 0; i < this.IntroCards.Length; i++)
						{
							if (i != 0)
							{
								this.IntroCards[i].ToggleLighting(false);
							}
							this.IntroCards[i].SetVisibility(0f);
							if (i > 0)
							{
								this.IntroCards[i].transform.position = this.IntroCards[i - 1].EndCameraTransform.position - this.IntroCards[i].StartCameraTransform.localPosition;
								tk2dBaseSprite[] componentsInChildren = this.IntroCards[i].GetComponentsInChildren<tk2dBaseSprite>();
								for (int j = 0; j < componentsInChildren.Length; j++)
								{
									componentsInChildren[j].UpdateZDepth();
								}
							}
						}
						currentCard.SetVisibility(1f);
						FinalIntroSequenceCard previousCard = null;
						bool hasPrefaded = false;
						while (continueDoing)
						{
							if (ela > currentCard.PanTime + currentCard.EndHoldTime)
							{
								previousCard = currentCard;
								cardIndex++;
								if (cardIndex >= this.IntroCards.Length)
								{
									break;
								}
								hasPrefaded = false;
								currentCard = this.IntroCards[cardIndex];
								this.customTextFadeInTime = currentCard.CustomTextFadeInTime;
								this.customTextFadeOutTime = currentCard.CustomTextFadeOutTime;
								ela = -currentCard.StartHoldTime;
								previousCard.ToggleLighting(false);
								currentCard.ToggleLighting(true);
								if (previousCard.EndHoldTime == 0f)
								{
									base.StartCoroutine(this.ContinueMovingPreviousCard(previousCard));
								}
								if (currentCard.StartHoldTime > 0f)
								{
									float tempEla = 0f;
									while (tempEla < 1f)
									{
										tempEla += GameManager.INVARIANT_DELTA_TIME;
										previousCard.SetVisibility(1f - tempEla);
										currentCard.SetVisibility(tempEla);
										this.UpdateSkipLegend();
										if (this.m_skipLegend)
										{
											goto IL_C15;
										}
										yield return null;
									}
								}
							}
							else if (!hasPrefaded && ela > currentCard.PanTime + currentCard.EndHoldTime - 0.5f)
							{
								hasPrefaded = true;
								this.SetIntroString(true, false, 0.45f, new string[] { string.Empty });
							}
							yield return null;
							if (cardIndex > 0 && currentCard.StartHoldTime == 0f && ela < 1f)
							{
								previousCard.SetVisibility(1f - (ela + GameManager.INVARIANT_DELTA_TIME));
								currentCard.SetVisibility(ela + GameManager.INVARIANT_DELTA_TIME);
							}
							ela += GameManager.INVARIANT_DELTA_TIME;
							totalElapsed += GameManager.INVARIANT_DELTA_TIME;
							this.UpdateText(totalElapsed, ela + currentCard.StartHoldTime, cardIndex, ref currentStringIndex);
							float t = Mathf.SmoothStep(0f, 1f, ela / currentCard.PanTime);
							if (currentCard.StartHoldTime == 0f && currentCard.EndHoldTime == 0f)
							{
								t = Mathf.Clamp01(ela / currentCard.PanTime);
							}
							else if (currentCard.StartHoldTime == 0f)
							{
								t = BraveMathCollege.LinearToSmoothStepInterpolate(0f, 1f, Mathf.Clamp01(ela / currentCard.PanTime));
							}
							else if (currentCard.EndHoldTime == 0f)
							{
								t = BraveMathCollege.SmoothStepToLinearStepInterpolate(0f, 1f, Mathf.Clamp01(ela / currentCard.PanTime));
							}
							cc.OverridePosition = Vector2.Lerp(currentCard.StartCameraTransform.position.XY(), currentCard.EndCameraTransform.position.XY(), t) + CameraController.PLATFORM_CAMERA_OFFSET.XY();
							this.UpdateSkipLegend();
							continueDoing = Foyer.DoIntroSequence && !this.m_skipLegend;
						}
						if (!this.m_skipLegend)
						{
							if (!this.m_skipLegend && Foyer.DoIntroSequence)
							{
								Pixelator.Instance.FadeToBlack(1f, false, 0f);
								float finalEla = 0f;
								bool hasTriggeredFirstString = false;
								bool hasTriggeredSecondString = false;
								while (finalEla < this.LastTextHoldTime && !this.m_skipLegend)
								{
									finalEla += GameManager.INVARIANT_DELTA_TIME;
									totalElapsed += GameManager.INVARIANT_DELTA_TIME;
									if (!hasTriggeredFirstString)
									{
										hasTriggeredFirstString = true;
										this.SetIntroString(true, true, new string[]
										{
											"#INTRO_VIDEO_07",
											string.Empty,
											string.Empty
										});
									}
									else if (!hasTriggeredSecondString && finalEla > this.LastTextSecondStringTriggerTime)
									{
										hasTriggeredSecondString = true;
										this.SetIntroString(false, false, new string[]
										{
											"#INTRO_VIDEO_07",
											string.Empty,
											"#INTRO_VIDEO_08"
										});
									}
									this.UpdateSkipLegend();
									yield return null;
								}
							}
							if (!this.m_skipLegend)
							{
								Pixelator.Instance.CacheCurrentFrameToBuffer = true;
								yield return null;
								TitleDioramaController tdc = UnityEngine.Object.FindObjectOfType<TitleDioramaController>();
								tdc.CacheFrameToFadeBuffer(cc.Camera);
							}
							this.customTextFadeInTime = -1f;
							this.customTextFadeOutTime = this.LastTextFadeOutTime;
							yield return null;
						}
					}
				}
			}
		}
		IL_C15:
		if (this.m_isDoingQuickStart)
		{
			yield break;
		}
		if (this.m_skipLegend)
		{
			yield return null;
			if (this.m_isDoingQuickStart)
			{
				yield break;
			}
			this.m_skipLegend = false;
			this.customTextFadeOutTime = 0.5f;
			this.SetIntroString(true, false, new string[] { string.Empty });
			Pixelator.Instance.KillAllFades = true;
			yield return null;
			Pixelator.Instance.KillAllFades = false;
			Pixelator.Instance.FadeToBlack(0.25f, false, 0f);
			yield return new WaitForSeconds(0.25f);
			Pixelator.Instance.FadeToBlack(0.25f, true, 0.05f);
		}
		else
		{
			Pixelator.Instance.FadeToBlack(0.25f, true, 0.1f);
		}
		cc.OverrideZoomScale = 1f;
		cc.CurrentZoomScale = 1f;
		Pixelator.Instance.SetVignettePower(1f);
		Pixelator.Instance.DoOcclusionLayer = true;
		yield break;
	}

	// Token: 0x06008B6F RID: 35695 RVA: 0x003A0BA8 File Offset: 0x0039EDA8
	private IEnumerator FadeToBlack(float duration, bool startAtCurrent = false, bool force = false)
	{
		float elapsed = 0f;
		float startValue = 0f;
		if (startAtCurrent)
		{
			startValue = this.FadeMaterial.GetColor("_Color").a;
		}
		while (elapsed < duration)
		{
			if (!force && this.m_skipCycle)
			{
				yield break;
			}
			elapsed += Time.deltaTime;
			float t = elapsed / duration;
			this.FadeMaterial.SetColor("_Color", new Color(0f, 0f, 0f, Mathf.Lerp(startValue, 1f, t)));
			yield return null;
		}
		this.FadeMaterial.SetColor("_Color", new Color(0f, 0f, 0f, 1f));
		yield break;
	}

	// Token: 0x06008B70 RID: 35696 RVA: 0x003A0BD8 File Offset: 0x0039EDD8
	private IEnumerator HandleDevolverLogo()
	{
		this.FadeMaterial.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
		this.PT1_DodgeRoll_Logo.sprite.renderer.enabled = false;
		this.PT1_DodgeRoll_Guy.sprite.renderer.enabled = false;
		this.PT2_Devolver_Logo.sprite.renderer.enabled = true;
		this.PT2_Devolver_Logo.AudioBaseObject = GameManager.Instance.gameObject;
		this.PT2_Devolver_Logo.Play();
		while (this.PT2_Devolver_Logo.IsPlaying(this.PT2_Devolver_Logo.CurrentClip))
		{
			if (this.m_skipCycle)
			{
				yield break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008B71 RID: 35697 RVA: 0x003A0BF4 File Offset: 0x0039EDF4
	private IEnumerator HandleDodgeRollLogo()
	{
		this.FadeMaterial.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
		this.PT1_DodgeRoll_Logo.sprite.renderer.enabled = false;
		this.PT1_DodgeRoll_Guy.sprite.renderer.enabled = true;
		this.PT2_Devolver_Logo.sprite.renderer.enabled = false;
		this.PT1_DodgeRoll_Guy.Play();
		while (this.PT1_DodgeRoll_Guy.CurrentFrame < 9)
		{
			if (this.m_skipCycle)
			{
				yield break;
			}
			yield return null;
		}
		if (this.m_skipCycle)
		{
			yield break;
		}
		this.PT1_DodgeRoll_Logo.sprite.renderer.enabled = true;
		this.PT1_DodgeRoll_Logo.AudioBaseObject = GameManager.Instance.gameObject;
		this.PT1_DodgeRoll_Logo.Play();
		while (this.PT1_DodgeRoll_Logo.IsPlaying(this.PT1_DodgeRoll_Logo.CurrentClip) || this.PT1_DodgeRoll_Guy.IsPlaying(this.PT1_DodgeRoll_Guy.CurrentClip))
		{
			if (this.m_skipCycle)
			{
				yield break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008B72 RID: 35698 RVA: 0x003A0C10 File Offset: 0x0039EE10
	private IEnumerator DoQuickStart()
	{
		this.QuickStartObject.SetActive(false);
		base.StartCoroutine(this.FadeToBlack(0.1f, true, true));
		GameManager.PreventGameManagerExistence = false;
		GameManager.SKIP_FOYER = true;
		Foyer.DoMainMenu = false;
		if (!this.m_inFoyer)
		{
			uint num = 1U;
			DebugTime.RecordStartTime();
			AkSoundEngine.LoadBank("SFX.bnk", -1, out num);
			DebugTime.Log("FinalIntroSequenceManager.DoQuickStart.LoadBank()", new object[0]);
			GameManager.EnsureExistence();
		}
		AkSoundEngine.PostEvent("Play_UI_menu_confirm_01", base.gameObject);
		MidGameSaveData saveToContinue = null;
		if (GameManager.VerifyAndLoadMidgameSave(out saveToContinue, true))
		{
			yield return null;
			Dungeon.ShouldAttemptToLoadFromMidgameSave = true;
			GameManager.Instance.SetNextLevelIndex(GameManager.Instance.GetTargetLevelIndexFromSavedTileset(saveToContinue.levelSaved));
			GameManager.Instance.GeneratePlayersFromMidGameSave(saveToContinue);
			if (!this.m_inFoyer)
			{
				GameManager.Instance.FlushAudio();
			}
			GameManager.Instance.IsFoyer = false;
			Foyer.DoIntroSequence = false;
			Foyer.DoMainMenu = false;
			GameManager.Instance.IsSelectingCharacter = false;
			GameManager.Instance.DelayedLoadMidgameSave(0.25f, saveToContinue);
		}
		else
		{
			GameManager.PlayerPrefabForNewGame = (GameObject)BraveResources.Load(CharacterSelectController.GetCharacterPathFromQuickStart(), ".prefab");
			GameManager.Instance.GlobalInjectionData.PreprocessRun(false);
			yield return null;
			PlayerController playerController = GameManager.PlayerPrefabForNewGame.GetComponent<PlayerController>();
			GameStatsManager.Instance.BeginNewSession(playerController);
			GameObject instantiatedPlayer = UnityEngine.Object.Instantiate<GameObject>(GameManager.PlayerPrefabForNewGame, Vector3.zero, Quaternion.identity);
			GameManager.PlayerPrefabForNewGame = null;
			instantiatedPlayer.SetActive(true);
			PlayerController extantPlayer = instantiatedPlayer.GetComponent<PlayerController>();
			extantPlayer.PlayerIDX = 0;
			GameManager.Instance.PrimaryPlayer = extantPlayer;
			if (!this.m_inFoyer)
			{
				GameManager.Instance.FlushAudio();
			}
			GameManager.Instance.FlushMusicAudio();
			GameManager.Instance.SetNextLevelIndex(1);
			GameManager.Instance.IsSelectingCharacter = false;
			GameManager.Instance.IsFoyer = false;
			GameManager.Instance.DelayedLoadNextLevel(0.5f);
			yield return null;
			yield return null;
			yield return null;
			Foyer.Instance.OnDepartedFoyer();
		}
		yield break;
	}

	// Token: 0x04009244 RID: 37444
	public tk2dSpriteAnimator PT1_DodgeRoll_Guy;

	// Token: 0x04009245 RID: 37445
	public tk2dSpriteAnimator PT1_DodgeRoll_Logo;

	// Token: 0x04009246 RID: 37446
	public tk2dSpriteAnimator PT2_Devolver_Logo;

	// Token: 0x04009247 RID: 37447
	public Material FadeMaterial;

	// Token: 0x04009248 RID: 37448
	public GameObject QuickStartObject;

	// Token: 0x04009249 RID: 37449
	public tk2dTextMesh QuickStartKeyboard;

	// Token: 0x0400924A RID: 37450
	public tk2dSprite QuickStartController;

	// Token: 0x0400924B RID: 37451
	public tk2dTextMesh IntroTextMesh;

	// Token: 0x0400924C RID: 37452
	public FinalIntroSequenceCard[] IntroCards;

	// Token: 0x0400924D RID: 37453
	public bool IsDoingIntro;

	// Token: 0x0400924E RID: 37454
	private bool m_inFoyer;

	// Token: 0x0400924F RID: 37455
	private bool m_isDoingQuickStart;

	// Token: 0x04009250 RID: 37456
	private bool m_skipCycle;

	// Token: 0x04009251 RID: 37457
	private float customTextFadeInTime = -1f;

	// Token: 0x04009252 RID: 37458
	private float customTextFadeOutTime = -1f;

	// Token: 0x04009253 RID: 37459
	private string[] introKeys = new string[]
	{
		"#INTRO_VIDEO_01", "#INTRO_VIDEO_02a", "#INTRO_VIDEO_02b", "#INTRO_VIDEO_03", "#INTRO_VIDEO_04a", "#INTRO_VIDEO_04b", "#INTRO_VIDEO_05", "#INTRO_VIDEO_06a", "#INTRO_VIDEO_06b", "#INTRO_VIDEO_07",
		"#INTRO_VIDEO_08"
	};

	// Token: 0x04009254 RID: 37460
	private string m_cachedLastFirstString;

	// Token: 0x04009255 RID: 37461
	private string[] m_lastAssignedStrings;

	// Token: 0x04009256 RID: 37462
	public float FirstTextFadeInTime = 3f;

	// Token: 0x04009257 RID: 37463
	public float FirstTextHoldTime = 7f;

	// Token: 0x04009258 RID: 37464
	public float LastTextHoldTime = 7f;

	// Token: 0x04009259 RID: 37465
	public float LastTextFadeOutTime = 3f;

	// Token: 0x0400925A RID: 37466
	public float LastTextSecondStringTriggerTime = 5f;

	// Token: 0x0400925B RID: 37467
	private bool m_skipLegend;

	// Token: 0x0400925C RID: 37468
	private Vector3 m_currentIntroTextMeshLocalPosition;
}
