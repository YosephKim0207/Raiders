using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200172D RID: 5933
public class AmmonomiconController : MonoBehaviour
{
	// Token: 0x060089C5 RID: 35269 RVA: 0x00394C68 File Offset: 0x00392E68
	public static bool GuiManagerIsPageRenderer(dfGUIManager manager)
	{
		return AmmonomiconController.m_instance != null && AmmonomiconController.m_instance.IsOpen && AmmonomiconController.m_instance.m_AmmonomiconInstance != null && AmmonomiconController.m_instance.m_AmmonomiconInstance.GetComponent<dfGUIManager>() == manager;
	}

	// Token: 0x17001472 RID: 5234
	// (get) Token: 0x060089C6 RID: 35270 RVA: 0x00394CC8 File Offset: 0x00392EC8
	// (set) Token: 0x060089C7 RID: 35271 RVA: 0x00394D50 File Offset: 0x00392F50
	public static AmmonomiconController Instance
	{
		get
		{
			if (BraveUtility.isLoadingLevel)
			{
				return null;
			}
			if (GameManager.Instance.Dungeon == null)
			{
				return null;
			}
			if (AmmonomiconController.m_instance == null)
			{
				AmmonomiconController ammonomiconController = UnityEngine.Object.FindObjectOfType<AmmonomiconController>();
				if (ammonomiconController == null)
				{
					Debug.LogError("INSTANTIATING AMMONOMICON ???");
					GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Ammonomicon Controller", ".prefab"));
					ammonomiconController = gameObject.GetComponent<AmmonomiconController>();
				}
				AmmonomiconController.m_instance = ammonomiconController;
			}
			return AmmonomiconController.m_instance;
		}
		set
		{
			AmmonomiconController.m_instance = value;
		}
	}

	// Token: 0x17001473 RID: 5235
	// (get) Token: 0x060089C8 RID: 35272 RVA: 0x00394D58 File Offset: 0x00392F58
	public static bool HasInstance
	{
		get
		{
			return AmmonomiconController.m_instance;
		}
	}

	// Token: 0x17001474 RID: 5236
	// (get) Token: 0x060089C9 RID: 35273 RVA: 0x00394D64 File Offset: 0x00392F64
	public static AmmonomiconController ForceInstance
	{
		get
		{
			if (AmmonomiconController.m_instance == null)
			{
				AmmonomiconController ammonomiconController = UnityEngine.Object.FindObjectOfType<AmmonomiconController>();
				if (ammonomiconController == null)
				{
					Debug.LogError("INSTANTIATING AMMONOMICON ???");
					GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Ammonomicon Controller", ".prefab"));
					ammonomiconController = gameObject.GetComponent<AmmonomiconController>();
				}
				AmmonomiconController.m_instance = ammonomiconController;
			}
			return AmmonomiconController.m_instance;
		}
	}

	// Token: 0x060089CA RID: 35274 RVA: 0x00394DCC File Offset: 0x00392FCC
	public static void EnsureExistence()
	{
		if (GameManager.Instance.Dungeon == null)
		{
			return;
		}
		if (AmmonomiconController.m_instance == null)
		{
			AmmonomiconController ammonomiconController = UnityEngine.Object.FindObjectOfType<AmmonomiconController>();
			if (ammonomiconController == null)
			{
				Debug.LogError("INSTANTIATING AMMONOMICON ???");
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Ammonomicon Controller", ".prefab"));
				ammonomiconController = gameObject.GetComponent<AmmonomiconController>();
			}
			AmmonomiconController.m_instance = ammonomiconController;
		}
	}

	// Token: 0x17001475 RID: 5237
	// (get) Token: 0x060089CB RID: 35275 RVA: 0x00394E44 File Offset: 0x00393044
	public bool IsOpening
	{
		get
		{
			return this.m_isOpening;
		}
	}

	// Token: 0x17001476 RID: 5238
	// (get) Token: 0x060089CC RID: 35276 RVA: 0x00394E4C File Offset: 0x0039304C
	public bool IsClosing
	{
		get
		{
			return this.m_isClosing;
		}
	}

	// Token: 0x17001477 RID: 5239
	// (get) Token: 0x060089CD RID: 35277 RVA: 0x00394E54 File Offset: 0x00393054
	public bool IsOpen
	{
		get
		{
			return this.m_isOpen;
		}
	}

	// Token: 0x17001478 RID: 5240
	// (get) Token: 0x060089CE RID: 35278 RVA: 0x00394E5C File Offset: 0x0039305C
	public bool BookmarkHasFocus
	{
		get
		{
			return this.m_isOpen && this.m_AmmonomiconInstance.BookmarkHasFocus;
		}
	}

	// Token: 0x060089CF RID: 35279 RVA: 0x00394E78 File Offset: 0x00393078
	public void ReturnFocusToBookmark()
	{
		this.m_AmmonomiconInstance.bookmarks[this.m_AmmonomiconInstance.CurrentlySelectedTabIndex].ForceFocus();
	}

	// Token: 0x17001479 RID: 5241
	// (get) Token: 0x060089D0 RID: 35280 RVA: 0x00394E98 File Offset: 0x00393098
	public AmmonomiconInstanceManager Ammonomicon
	{
		get
		{
			return this.m_AmmonomiconInstance;
		}
	}

	// Token: 0x1700147A RID: 5242
	// (get) Token: 0x060089D1 RID: 35281 RVA: 0x00394EA0 File Offset: 0x003930A0
	public AmmonomiconPageRenderer BestInteractingLeftPageRenderer
	{
		get
		{
			if (this.IsTurningPage && this.ImpendingLeftPageRenderer != null)
			{
				return this.ImpendingLeftPageRenderer;
			}
			return this.CurrentLeftPageRenderer;
		}
	}

	// Token: 0x1700147B RID: 5243
	// (get) Token: 0x060089D2 RID: 35282 RVA: 0x00394ECC File Offset: 0x003930CC
	public AmmonomiconPageRenderer BestInteractingRightPageRenderer
	{
		get
		{
			if (this.IsTurningPage && this.ImpendingRightPageRenderer != null)
			{
				return this.ImpendingRightPageRenderer;
			}
			return this.CurrentRightPageRenderer;
		}
	}

	// Token: 0x1700147C RID: 5244
	// (get) Token: 0x060089D3 RID: 35283 RVA: 0x00394EF8 File Offset: 0x003930F8
	public AmmonomiconPageRenderer CurrentLeftPageRenderer
	{
		get
		{
			return this.m_CurrentLeftPageManager;
		}
	}

	// Token: 0x1700147D RID: 5245
	// (get) Token: 0x060089D4 RID: 35284 RVA: 0x00394F00 File Offset: 0x00393100
	public AmmonomiconPageRenderer CurrentRightPageRenderer
	{
		get
		{
			return this.m_CurrentRightPageManager;
		}
	}

	// Token: 0x1700147E RID: 5246
	// (get) Token: 0x060089D5 RID: 35285 RVA: 0x00394F08 File Offset: 0x00393108
	public AmmonomiconPageRenderer ImpendingLeftPageRenderer
	{
		get
		{
			return this.m_ImpendingLeftPageManager;
		}
	}

	// Token: 0x1700147F RID: 5247
	// (get) Token: 0x060089D6 RID: 35286 RVA: 0x00394F10 File Offset: 0x00393110
	public AmmonomiconPageRenderer ImpendingRightPageRenderer
	{
		get
		{
			return this.m_ImpendingRightPageManager;
		}
	}

	// Token: 0x17001480 RID: 5248
	// (get) Token: 0x060089D7 RID: 35287 RVA: 0x00394F18 File Offset: 0x00393118
	public bool IsTurningPage
	{
		get
		{
			return this.m_isPageTransitioning;
		}
	}

	// Token: 0x060089D8 RID: 35288 RVA: 0x00394F20 File Offset: 0x00393120
	private void Awake()
	{
		for (int i = 0; i < 12; i++)
		{
			this.m_offsetInUse.Add(false);
			this.m_offsets.Add(new Vector3((float)(-200 + -20 * i), (float)(-200 + -20 * i), 0f));
		}
	}

	// Token: 0x060089D9 RID: 35289 RVA: 0x00394F78 File Offset: 0x00393178
	private void Start()
	{
		this.PrecacheAllData();
	}

	// Token: 0x060089DA RID: 35290 RVA: 0x00394F80 File Offset: 0x00393180
	public void PrecacheAllData()
	{
		this.m_AmmonomiconBase = UnityEngine.Object.Instantiate<GameObject>(this.AmmonomiconBasePrefab, new Vector3(-500f, -500f, 0f), Quaternion.identity);
		this.m_AmmonomiconInstance = this.m_AmmonomiconBase.GetComponent<AmmonomiconInstanceManager>();
		Transform transform = this.m_AmmonomiconBase.transform.Find("Core");
		this.m_AmmonomiconLowerImage = transform.Find("Ammonomicon Bottom").GetComponent<dfTextureSprite>();
		this.m_AmmonomiconUpperImage = transform.Find("Ammonomicon Top").GetComponent<dfTextureSprite>();
		this.m_AmmonomiconOptionalThirdImage = transform.Find("Ammonomicon Toppest").GetComponent<dfTextureSprite>();
		this.m_AmmonomiconOptionalThirdImage.Material = new Material(ShaderCache.Acquire("Daikon Forge/Default UI Shader Highest Queue"));
		this.m_AmmonomiconOptionalThirdImage.IsVisible = false;
		this.m_AmmonomiconUpperImage.Material = new Material(ShaderCache.Acquire("Daikon Forge/Default UI Shader High Queue"));
		this.m_LowerRenderTargetPrefab = transform.Find("Ammonomicon Page Renderer Lower").GetComponent<MeshRenderer>();
		this.m_LowerRenderTargetPrefab.enabled = false;
		this.m_UpperRenderTargetPrefab = transform.Find("Ammonomicon Page Renderer Upper").GetComponent<MeshRenderer>();
		this.m_UpperRenderTargetPrefab.enabled = false;
		this.m_AmmonomiconInstance.GuiManager.RenderCamera.enabled = false;
		this.m_AmmonomiconInstance.GuiManager.enabled = false;
		AmmonomiconInstanceManager component = this.AmmonomiconBasePrefab.GetComponent<AmmonomiconInstanceManager>();
		Transform transform2 = new GameObject("_Ammonomicon").transform;
		this.m_AmmonomiconBase.transform.parent = transform2;
		base.transform.parent = transform2;
		for (int i = 0; i < component.bookmarks.Length - 1; i++)
		{
			AmmonomiconPageRenderer ammonomiconPageRenderer = this.LoadPageUIAtPath(component.bookmarks[i].TargetNewPageLeft, component.bookmarks[i].LeftPageType, true, false);
			AmmonomiconPageRenderer ammonomiconPageRenderer2 = this.LoadPageUIAtPath(component.bookmarks[i].TargetNewPageRight, component.bookmarks[i].RightPageType, true, false);
			ammonomiconPageRenderer.transform.parent.parent = transform2;
			ammonomiconPageRenderer2.transform.parent.parent = transform2;
		}
		UnityEngine.Object.DontDestroyOnLoad(transform2.gameObject);
	}

	// Token: 0x060089DB RID: 35291 RVA: 0x00395198 File Offset: 0x00393398
	private void OpenInternal(bool isDeath, bool isVictory, EncounterTrackable targetTrackable = null)
	{
		this.m_isOpening = true;
		while (dfGUIManager.GetModalControl() != null)
		{
			Debug.LogError(dfGUIManager.GetModalControl().name + " was modal, popping...");
			dfGUIManager.PopModal();
		}
		this.m_isPageTransitioning = true;
		this.m_AmmonomiconInstance.GuiManager.enabled = true;
		this.m_AmmonomiconInstance.GuiManager.RenderCamera.enabled = true;
		int num = ((!isDeath) ? 0 : (this.m_AmmonomiconInstance.bookmarks.Length - 1));
		this.m_CurrentLeftPageManager = this.LoadPageUIAtPath(this.m_AmmonomiconInstance.bookmarks[num].TargetNewPageLeft, (!isDeath) ? AmmonomiconPageRenderer.PageType.EQUIPMENT_LEFT : AmmonomiconPageRenderer.PageType.DEATH_LEFT, false, isVictory);
		this.m_CurrentRightPageManager = this.LoadPageUIAtPath(this.m_AmmonomiconInstance.bookmarks[num].TargetNewPageRight, (!isDeath) ? AmmonomiconPageRenderer.PageType.EQUIPMENT_RIGHT : AmmonomiconPageRenderer.PageType.DEATH_RIGHT, false, isVictory);
		this.m_CurrentLeftPageManager.ForceUpdateLanguageFonts();
		this.m_CurrentRightPageManager.ForceUpdateLanguageFonts();
		if (this.m_CurrentRightPageManager.pageType == AmmonomiconPageRenderer.PageType.EQUIPMENT_RIGHT && this.m_CurrentLeftPageManager.LastFocusTarget != null)
		{
			AmmonomiconPokedexEntry component = (this.m_CurrentLeftPageManager.LastFocusTarget as dfButton).GetComponent<AmmonomiconPokedexEntry>();
			this.m_CurrentRightPageManager.SetRightDataPageTexts(component.ChildSprite, component.linkedEncounterTrackable);
		}
		else if (this.m_CurrentRightPageManager.pageType == AmmonomiconPageRenderer.PageType.EQUIPMENT_RIGHT)
		{
			this.m_CurrentRightPageManager.SetRightDataPageUnknown(false);
		}
		this.m_CurrentRightPageManager.targetRenderer.sharedMaterial.shader = ShaderCache.Acquire("Custom/AmmonomiconPageShader");
		base.StartCoroutine(this.HandleOpenAmmonomicon(isDeath, GameManager.Options.HasEverSeenAmmonomicon, targetTrackable));
		GameManager.Options.HasEverSeenAmmonomicon = true;
	}

	// Token: 0x060089DC RID: 35292 RVA: 0x00395350 File Offset: 0x00393550
	public void OpenAmmonomiconToTrackable(EncounterTrackable targetTrackable)
	{
		if (this.m_isOpen || this.m_isOpening)
		{
			return;
		}
		this.m_isOpen = true;
		this.OpenInternal(false, false, targetTrackable);
	}

	// Token: 0x060089DD RID: 35293 RVA: 0x0039537C File Offset: 0x0039357C
	public void OpenAmmonomicon(bool isDeath, bool isVictory)
	{
		if (this.m_isOpen || this.m_isOpening)
		{
			return;
		}
		this.m_isOpen = true;
		this.OpenInternal(isDeath, isVictory, null);
	}

	// Token: 0x060089DE RID: 35294 RVA: 0x003953A8 File Offset: 0x003935A8
	private void LateUpdate()
	{
		if (Pixelator.Instance == null)
		{
			return;
		}
		if (this.m_AmmonomiconBase != null && !GameManager.Instance.IsLoadingLevel)
		{
			dfGUIManager component = this.m_AmmonomiconBase.GetComponent<dfGUIManager>();
			component.UIScale = Pixelator.Instance.ScaleTileScale / 3f;
			Vector2 screenSize = component.GetScreenSize();
			Vector2 vector = new Vector2(screenSize.x / 1920f, screenSize.y / 1080f);
			float num = Pixelator.Instance.ScaleTileScale / 3f;
			if (this.m_CurrentLeftPageManager != null)
			{
				this.m_CurrentLeftPageManager.targetRenderer.transform.localScale = new Vector3(1.7777778f * vector.x, 2f * vector.x, 1f) * num;
				this.m_CurrentLeftPageManager.targetRenderer.transform.localPosition = new Vector3(-0.5f * this.m_CurrentLeftPageManager.targetRenderer.transform.localScale.x, 0f, -0.5f);
				if (this.m_currentFrameDefinition != null)
				{
					this.m_CurrentLeftPageManager.targetRenderer.transform.localPosition += Vector3.Scale(this.m_currentFrameDefinition.CurrentLeftOffset, this.m_CurrentLeftPageManager.targetRenderer.transform.localScale);
				}
			}
			if (this.m_CurrentRightPageManager != null)
			{
				this.m_CurrentRightPageManager.targetRenderer.transform.localScale = new Vector3(1.7777778f * vector.x, 2f * vector.x, 1f) * num;
				this.m_CurrentRightPageManager.targetRenderer.transform.localPosition = new Vector3(0.5f * this.m_CurrentRightPageManager.targetRenderer.transform.localScale.x, 0f, -0.5f);
				if (this.m_currentFrameDefinition != null)
				{
					this.m_CurrentRightPageManager.targetRenderer.transform.localPosition += Vector3.Scale(this.m_currentFrameDefinition.CurrentRightOffset, this.m_CurrentRightPageManager.targetRenderer.transform.localScale);
				}
			}
			if (this.m_ImpendingLeftPageManager != null)
			{
				this.m_ImpendingLeftPageManager.targetRenderer.transform.localScale = new Vector3(1.7777778f * vector.x, 2f * vector.x, 1f) * num;
				this.m_ImpendingLeftPageManager.targetRenderer.transform.localPosition = new Vector3(-0.5f * this.m_ImpendingLeftPageManager.targetRenderer.transform.localScale.x, 0f, -0.5f);
				if (this.m_currentFrameDefinition != null)
				{
					this.m_ImpendingLeftPageManager.targetRenderer.transform.localPosition += Vector3.Scale(this.m_currentFrameDefinition.ImpendingLeftOffset, this.m_ImpendingLeftPageManager.targetRenderer.transform.localScale);
				}
			}
			if (this.m_ImpendingRightPageManager != null)
			{
				this.m_ImpendingRightPageManager.targetRenderer.transform.localScale = new Vector3(1.7777778f * vector.x, 2f * vector.x, 1f) * num;
				this.m_ImpendingRightPageManager.targetRenderer.transform.localPosition = new Vector3(0.5f * this.m_ImpendingRightPageManager.targetRenderer.transform.localScale.x, 0f, -0.5f);
				if (this.m_currentFrameDefinition != null)
				{
					this.m_ImpendingRightPageManager.targetRenderer.transform.localPosition += Vector3.Scale(this.m_currentFrameDefinition.ImpendingRightOffset, this.m_ImpendingRightPageManager.targetRenderer.transform.localScale);
				}
			}
			if (this.m_CurrentLeftPageManager != null && this.m_CurrentRightPageManager != null)
			{
				if (Input.mousePosition.x > (float)Screen.width / 2f)
				{
					if (this.m_CurrentRightPageManager.guiManager.RenderCamera.depth <= this.m_CurrentLeftPageManager.guiManager.RenderCamera.depth)
					{
						this.m_CurrentRightPageManager.guiManager.RenderCamera.depth = 4f;
					}
				}
				else if (this.m_CurrentLeftPageManager.guiManager.RenderCamera.depth <= this.m_CurrentRightPageManager.guiManager.RenderCamera.depth)
				{
					this.m_CurrentRightPageManager.guiManager.RenderCamera.depth = 1f;
				}
			}
		}
	}

	// Token: 0x060089DF RID: 35295 RVA: 0x003958BC File Offset: 0x00393ABC
	public void OnApplicationFocus(bool focusStatus)
	{
		this.m_applicationFocus = focusStatus;
	}

	// Token: 0x060089E0 RID: 35296 RVA: 0x003958C8 File Offset: 0x00393AC8
	private IEnumerator HandleOpenAmmonomicon(bool isDeath, bool isShortAnimation, EncounterTrackable targetTrackable = null)
	{
		List<AmmonomiconFrameDefinition> TargetAnimationFrames = this.OpenAnimationFrames;
		if (isShortAnimation)
		{
			AkSoundEngine.PostEvent("Play_UI_ammonomicon_open_01", base.gameObject);
			TargetAnimationFrames = new List<AmmonomiconFrameDefinition>();
			for (int i = 0; i < 9; i++)
			{
				TargetAnimationFrames.Add(this.OpenAnimationFrames[i]);
			}
			for (int j = 23; j < this.OpenAnimationFrames.Count; j++)
			{
				TargetAnimationFrames.Add(this.OpenAnimationFrames[j]);
			}
		}
		else
		{
			AkSoundEngine.PostEvent("Play_UI_ammonomicon_intro_01", base.gameObject);
		}
		float animationTime = this.GetAnimationLength(TargetAnimationFrames);
		float elapsed = 0f;
		int currentFrameIndex = 0;
		float nextFrameTime = TargetAnimationFrames[0].frameTime * this.GLOBAL_ANIMATION_SCALE;
		this.SetFrame(TargetAnimationFrames[0]);
		while (elapsed < animationTime)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			if (elapsed >= animationTime)
			{
				break;
			}
			if (elapsed >= nextFrameTime)
			{
				currentFrameIndex++;
				nextFrameTime += TargetAnimationFrames[currentFrameIndex].frameTime * this.GLOBAL_ANIMATION_SCALE;
				this.SetFrame(TargetAnimationFrames[currentFrameIndex]);
			}
			while (!this.m_applicationFocus)
			{
				yield return null;
			}
			yield return null;
		}
		this.SetFrame(TargetAnimationFrames[TargetAnimationFrames.Count - 1]);
		if (isDeath)
		{
			this.m_AmmonomiconInstance.OpenDeath();
		}
		else
		{
			this.m_AmmonomiconInstance.Open();
		}
		if (targetTrackable != null)
		{
			AmmonomiconPokedexEntry pokedexEntry = this.CurrentLeftPageRenderer.GetPokedexEntry(targetTrackable);
			if (pokedexEntry != null)
			{
				Debug.Log("GET INFO SUCCESS");
				pokedexEntry.ForceFocus();
			}
		}
		this.m_isPageTransitioning = false;
		this.HandleQueuedUnlocks();
		yield break;
	}

	// Token: 0x060089E1 RID: 35297 RVA: 0x003958F8 File Offset: 0x00393AF8
	private void HandleQueuedUnlocks()
	{
		List<EncounterDatabaseEntry> queuedTrackables = GameManager.Instance.GetQueuedTrackables();
		if (queuedTrackables.Count > 0)
		{
			base.StartCoroutine(this.HandleQueuedUnlocks_CR(queuedTrackables));
		}
		else
		{
			this.m_isOpening = false;
		}
	}

	// Token: 0x060089E2 RID: 35298 RVA: 0x00395938 File Offset: 0x00393B38
	private IEnumerator HandleQueuedUnlocks_CR(List<EncounterDatabaseEntry> trackableData)
	{
		this.HandlingQueuedUnlocks = true;
		int i = 0;
		IL_1E1:
		while (i < trackableData.Count)
		{
			yield return null;
			EncounterDatabaseEntry trackable = trackableData[i];
			GameObject hasAppearedInstance = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Global Prefabs/AppearedInTheGungeonRoot", ".prefab"), new Vector3(-1200f, 300f, 0f), Quaternion.identity);
			dfPanel hasAppearedPanel = hasAppearedInstance.GetComponentInChildren<dfPanel>();
			hasAppearedPanel.BringToFront();
			AppearedInTheGungeonController apparator = hasAppearedPanel.GetComponent<AppearedInTheGungeonController>();
			apparator.Appear(trackable);
			while (!(BraveInput.PrimaryPlayerInstance != null) || !BraveInput.PrimaryPlayerInstance.ActiveActions.AnyActionPressed())
			{
				if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && BraveInput.SecondaryPlayerInstance != null && BraveInput.SecondaryPlayerInstance.ActiveActions.AnyActionPressed())
				{
					IL_16B:
					apparator.ShwoopClosed();
					GameManager.Instance.AcknowledgeKnownTrackable(trackable);
					float ela = 0f;
					while (ela < 0.2f)
					{
						ela += GameManager.INVARIANT_DELTA_TIME;
						yield return null;
					}
					i++;
					goto IL_1E1;
				}
				yield return null;
			}
			goto IL_16B;
		}
		this.HandlingQueuedUnlocks = false;
		this.m_isOpening = false;
		yield break;
	}

	// Token: 0x060089E3 RID: 35299 RVA: 0x0039595C File Offset: 0x00393B5C
	private void SetFrame(AmmonomiconFrameDefinition def)
	{
		this.m_currentFrameDefinition = def;
		this.m_AmmonomiconLowerImage.IsVisible = def.AmmonomiconBottomLayerTexture != null;
		if (this.m_AmmonomiconLowerImage.IsVisible)
		{
			this.m_AmmonomiconLowerImage.Texture = def.AmmonomiconBottomLayerTexture;
		}
		this.m_AmmonomiconUpperImage.IsVisible = def.AmmonomiconTopLayerTexture != null;
		if (this.m_AmmonomiconUpperImage.IsVisible)
		{
			this.m_AmmonomiconUpperImage.Texture = def.AmmonomiconTopLayerTexture;
		}
		if (def.AmmonomiconToppestLayerTexture != null)
		{
			this.m_AmmonomiconOptionalThirdImage.IsVisible = true;
			this.m_AmmonomiconOptionalThirdImage.Texture = def.AmmonomiconToppestLayerTexture;
		}
		else
		{
			this.m_AmmonomiconOptionalThirdImage.IsVisible = false;
		}
		if (this.m_CurrentLeftPageManager != null)
		{
			this.m_CurrentLeftPageManager.targetRenderer.transform.localPosition = new Vector3(-0.5f * this.m_CurrentLeftPageManager.targetRenderer.transform.localScale.x, 0f, -0.5f);
			this.m_CurrentLeftPageManager.targetRenderer.transform.localPosition += Vector3.Scale(def.CurrentLeftOffset, this.m_CurrentLeftPageManager.targetRenderer.transform.localScale);
			this.m_CurrentLeftPageManager.targetRenderer.enabled = def.CurrentLeftVisible;
			if (def.CurrentLeftVisible)
			{
				this.m_CurrentLeftPageManager.SetMatrix(def.CurrentLeftMatrix);
			}
		}
		if (this.m_CurrentRightPageManager != null)
		{
			this.m_CurrentRightPageManager.targetRenderer.transform.localPosition = new Vector3(0.5f * this.m_CurrentRightPageManager.targetRenderer.transform.localScale.x, 0f, -0.5f);
			this.m_CurrentRightPageManager.targetRenderer.transform.localPosition += Vector3.Scale(def.CurrentRightOffset, this.m_CurrentRightPageManager.targetRenderer.transform.localScale);
			this.m_CurrentRightPageManager.targetRenderer.enabled = def.CurrentRightVisible;
			if (def.CurrentRightVisible)
			{
				this.m_CurrentRightPageManager.SetMatrix(def.CurrentRightMatrix);
			}
		}
		if (this.m_ImpendingLeftPageManager != null)
		{
			this.m_ImpendingLeftPageManager.targetRenderer.transform.localPosition = new Vector3(-0.5f * this.m_ImpendingLeftPageManager.targetRenderer.transform.localScale.x, 0f, -0.5f);
			this.m_ImpendingLeftPageManager.targetRenderer.transform.localPosition += Vector3.Scale(def.ImpendingLeftOffset, this.m_ImpendingLeftPageManager.targetRenderer.transform.localScale);
			this.m_ImpendingLeftPageManager.targetRenderer.enabled = def.ImpendingLeftVisible;
			if (def.ImpendingLeftVisible)
			{
				this.m_ImpendingLeftPageManager.SetMatrix(def.ImpendingLeftMatrix);
			}
		}
		if (this.m_ImpendingRightPageManager != null)
		{
			this.m_ImpendingRightPageManager.targetRenderer.transform.localPosition = new Vector3(0.5f * this.m_ImpendingRightPageManager.targetRenderer.transform.localScale.x, 0f, -0.5f);
			this.m_ImpendingRightPageManager.targetRenderer.transform.localPosition += Vector3.Scale(def.ImpendingRightOffset, this.m_ImpendingRightPageManager.targetRenderer.transform.localScale);
			this.m_ImpendingRightPageManager.targetRenderer.enabled = def.ImpendingRightVisible;
			if (def.ImpendingRightVisible)
			{
				this.m_ImpendingRightPageManager.SetMatrix(def.ImpendingRightMatrix);
			}
		}
	}

	// Token: 0x060089E4 RID: 35300 RVA: 0x00395D48 File Offset: 0x00393F48
	public void CloseAmmonomicon(bool doDestroy = false)
	{
		if (this.m_isClosing || this.m_isOpening)
		{
			return;
		}
		AkSoundEngine.PostEvent("Stop_UI_ammonomicon_open_01", base.gameObject);
		AkSoundEngine.PostEvent("Play_UI_menu_back_01", base.gameObject);
		this.m_isClosing = true;
		this.m_isPageTransitioning = true;
		base.StartCoroutine(this.HandleCloseAmmonomicon(doDestroy));
	}

	// Token: 0x060089E5 RID: 35301 RVA: 0x00395DAC File Offset: 0x00393FAC
	private void ForceTerminateClosing()
	{
		this.m_isClosing = false;
	}

	// Token: 0x060089E6 RID: 35302 RVA: 0x00395DB8 File Offset: 0x00393FB8
	private IEnumerator HandleCloseMotion()
	{
		float elapsed = 0f;
		dfPanel targetPanel = this.m_AmmonomiconBase.transform.Find("Core").GetComponent<dfPanel>();
		if (this.m_cachedCorePanelY == -1f)
		{
			this.m_cachedCorePanelY = targetPanel.RelativePosition.y;
		}
		targetPanel.RelativePosition = targetPanel.RelativePosition.WithY(this.m_cachedCorePanelY);
		float startRelativeY = targetPanel.RelativePosition.y;
		while (elapsed < this.TotalDepartureTime && this.m_isClosing)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t = elapsed / this.TotalDepartureTime;
			float currentY = startRelativeY + this.DepartureYCurve.Evaluate(t) * this.DepartureYTotalDistance;
			targetPanel.RelativePosition = targetPanel.RelativePosition.WithY(currentY);
			yield return null;
		}
		while (this.m_isOpen && this.m_isClosing)
		{
			yield return null;
		}
		targetPanel.RelativePosition = targetPanel.RelativePosition.WithY(startRelativeY);
		yield break;
	}

	// Token: 0x060089E7 RID: 35303 RVA: 0x00395DD4 File Offset: 0x00393FD4
	private IEnumerator HandleCloseAmmonomicon(bool doDestroy = false)
	{
		List<AmmonomiconFrameDefinition> TargetAnimationFrames = this.CloseAnimationFrames;
		float animationTime = this.GetAnimationLength(TargetAnimationFrames);
		float elapsed = 0f;
		this.SetFrame(TargetAnimationFrames[0]);
		base.StartCoroutine(this.HandleCloseMotion());
		while (elapsed < animationTime && this.m_isClosing)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			if (elapsed >= animationTime)
			{
				break;
			}
			yield return null;
		}
		if (this.m_CurrentLeftPageManager != null)
		{
			this.m_CurrentLeftPageManager.Disable(false);
		}
		if (this.m_CurrentRightPageManager != null)
		{
			this.m_CurrentRightPageManager.Disable(false);
		}
		if (this.m_ImpendingLeftPageManager != null)
		{
			this.m_ImpendingLeftPageManager.Disable(false);
		}
		if (this.m_ImpendingRightPageManager != null)
		{
			this.m_ImpendingRightPageManager.Disable(false);
		}
		this.m_CurrentLeftPageManager = null;
		this.m_CurrentRightPageManager = null;
		this.m_AmmonomiconInstance.Close();
		this.m_AmmonomiconInstance.GuiManager.RenderCamera.enabled = false;
		this.m_AmmonomiconInstance.GuiManager.enabled = false;
		this.m_isPageTransitioning = false;
		this.m_isClosing = false;
		this.m_isOpen = false;
		yield break;
	}

	// Token: 0x060089E8 RID: 35304 RVA: 0x00395DF0 File Offset: 0x00393FF0
	private float GetAnimationLength(List<AmmonomiconFrameDefinition> frames)
	{
		float num = 0f;
		for (int i = 0; i < frames.Count; i++)
		{
			num += frames[i].frameTime * this.GLOBAL_ANIMATION_SCALE;
		}
		return num;
	}

	// Token: 0x060089E9 RID: 35305 RVA: 0x00395E34 File Offset: 0x00394034
	private AmmonomiconPageRenderer LoadPageUIAtPath(string path, AmmonomiconPageRenderer.PageType pageType, bool isPreCache = false, bool isVictory = false)
	{
		AmmonomiconPageRenderer ammonomiconPageRenderer;
		if (this.m_extantPageMap.ContainsKey(pageType))
		{
			ammonomiconPageRenderer = this.m_extantPageMap[pageType];
			if (pageType == AmmonomiconPageRenderer.PageType.DEATH_LEFT || pageType == AmmonomiconPageRenderer.PageType.DEATH_RIGHT)
			{
				AmmonomiconDeathPageController component = ammonomiconPageRenderer.transform.parent.GetComponent<AmmonomiconDeathPageController>();
				component.isVictoryPage = isVictory;
			}
			ammonomiconPageRenderer.EnableRendering();
			ammonomiconPageRenderer.DoRefreshData();
		}
		else
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load(path, ".prefab"));
			ammonomiconPageRenderer = gameObject.GetComponentInChildren<AmmonomiconPageRenderer>();
			dfGUIManager component2 = this.m_AmmonomiconBase.GetComponent<dfGUIManager>();
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.m_LowerRenderTargetPrefab.gameObject);
			gameObject2.transform.parent = component2.transform.Find("Core");
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.layer = LayerMask.NameToLayer("SecondaryGUI");
			MeshRenderer component3 = gameObject2.GetComponent<MeshRenderer>();
			if (isVictory)
			{
				AmmonomiconDeathPageController component4 = ammonomiconPageRenderer.transform.parent.GetComponent<AmmonomiconDeathPageController>();
				component4.isVictoryPage = true;
			}
			ammonomiconPageRenderer.Initialize(component3);
			ammonomiconPageRenderer.EnableRendering();
			for (int i = 0; i < this.m_offsets.Count; i++)
			{
				if (!this.m_offsetInUse[i])
				{
					this.m_offsetInUse[i] = true;
					gameObject.transform.position = this.m_offsets[i];
					ammonomiconPageRenderer.offsetIndex = i;
					break;
				}
			}
			this.m_extantPageMap.Add(pageType, ammonomiconPageRenderer);
			if (isPreCache)
			{
				ammonomiconPageRenderer.Disable(isPreCache);
			}
			else
			{
				ammonomiconPageRenderer.transform.parent.parent = this.m_AmmonomiconBase.transform.parent;
			}
		}
		return ammonomiconPageRenderer;
	}

	// Token: 0x060089EA RID: 35306 RVA: 0x00395FF4 File Offset: 0x003941F4
	private void MakeImpendingCurrent()
	{
		if (!this.m_isOpen)
		{
			return;
		}
		this.m_CurrentLeftPageManager.Disable(false);
		this.m_CurrentRightPageManager.Disable(false);
		this.m_CurrentLeftPageManager = this.m_ImpendingLeftPageManager;
		this.m_CurrentRightPageManager = this.m_ImpendingRightPageManager;
		this.m_CurrentLeftPageManager.targetRenderer.sharedMaterial.shader = ShaderCache.Acquire("Custom/AmmonomiconPageShader");
		this.m_CurrentRightPageManager.targetRenderer.sharedMaterial.shader = ShaderCache.Acquire("Custom/AmmonomiconPageShader");
		this.m_ImpendingLeftPageManager = null;
		this.m_ImpendingRightPageManager = null;
	}

	// Token: 0x060089EB RID: 35307 RVA: 0x0039608C File Offset: 0x0039428C
	public void TurnToPreviousPage(string pathToNextLeftPage, AmmonomiconPageRenderer.PageType leftPageType, string pathToNextRightPage, AmmonomiconPageRenderer.PageType rightPageType)
	{
		if (this.m_isPageTransitioning)
		{
			this.SetQueuedTransition(false, pathToNextLeftPage, leftPageType, pathToNextRightPage, rightPageType);
			return;
		}
		this.m_isPageTransitioning = true;
		this.m_ImpendingLeftPageManager = this.LoadPageUIAtPath(pathToNextLeftPage, leftPageType, false, false);
		this.m_ImpendingRightPageManager = this.LoadPageUIAtPath(pathToNextRightPage, rightPageType, false, false);
		this.m_ImpendingLeftPageManager.UpdateOnBecameActive();
		this.m_ImpendingRightPageManager.UpdateOnBecameActive();
		this.m_ImpendingRightPageManager.targetRenderer.sharedMaterial.shader = ShaderCache.Acquire("Custom/AmmonomiconTransitionPageShader");
		this.m_CurrentLeftPageManager.targetRenderer.sharedMaterial.shader = ShaderCache.Acquire("Custom/AmmonomiconTransitionPageShader");
		this.m_CurrentRightPageManager.targetRenderer.sharedMaterial.shader = ShaderCache.Acquire("Custom/AmmonomiconPageShader");
		this.m_ImpendingLeftPageManager.targetRenderer.sharedMaterial.shader = ShaderCache.Acquire("Custom/AmmonomiconPageShader");
		base.StartCoroutine(this.HandleTurnToPreviousPage());
	}

	// Token: 0x060089EC RID: 35308 RVA: 0x00396178 File Offset: 0x00394378
	private IEnumerator HandleTurnToPreviousPage()
	{
		AkSoundEngine.PostEvent("Play_UI_page_turn_01", base.gameObject);
		float animationTime = this.GetAnimationLength(this.TurnPageLeftAnimationFrames);
		float elapsed = 0f;
		int currentFrameIndex = 0;
		float nextFrameTime = this.TurnPageLeftAnimationFrames[0].frameTime * this.GLOBAL_ANIMATION_SCALE;
		this.SetFrame(this.TurnPageLeftAnimationFrames[0]);
		while (elapsed < animationTime)
		{
			if (!this.m_isOpen)
			{
				yield break;
			}
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			if (elapsed >= animationTime)
			{
				break;
			}
			if (elapsed >= nextFrameTime)
			{
				currentFrameIndex++;
				nextFrameTime += this.TurnPageLeftAnimationFrames[currentFrameIndex].frameTime * this.GLOBAL_ANIMATION_SCALE;
				this.SetFrame(this.TurnPageLeftAnimationFrames[currentFrameIndex]);
			}
			yield return null;
		}
		if (!this.m_isOpen)
		{
			yield break;
		}
		this.MakeImpendingCurrent();
		this.SetFrame(this.OpenAnimationFrames[this.OpenAnimationFrames.Count - 1]);
		this.m_isPageTransitioning = false;
		if (this.m_transitionIsQueued)
		{
			if (this.m_queuedNextPage)
			{
				this.TurnToNextPage(this.m_queuedLeftPath, this.m_queuedLeftType, this.m_queuedRightPath, this.m_queuedRightType);
			}
			else
			{
				this.TurnToPreviousPage(this.m_queuedLeftPath, this.m_queuedLeftType, this.m_queuedRightPath, this.m_queuedRightType);
			}
			this.m_transitionIsQueued = false;
		}
		yield break;
	}

	// Token: 0x060089ED RID: 35309 RVA: 0x00396194 File Offset: 0x00394394
	private void SetQueuedTransition(bool nextPage, string pathToNextLeftPage, AmmonomiconPageRenderer.PageType leftPageType, string pathToNextRightPage, AmmonomiconPageRenderer.PageType rightPageType)
	{
		if (this.m_isClosing)
		{
			return;
		}
		if (this.m_isPageTransitioning && this.ImpendingLeftPageRenderer.pageType == leftPageType)
		{
			this.m_transitionIsQueued = false;
			return;
		}
		this.m_transitionIsQueued = true;
		this.m_queuedLeftPath = pathToNextLeftPage;
		this.m_queuedLeftType = leftPageType;
		this.m_queuedRightPath = pathToNextRightPage;
		this.m_queuedRightType = rightPageType;
		this.m_queuedNextPage = nextPage;
	}

	// Token: 0x060089EE RID: 35310 RVA: 0x00396200 File Offset: 0x00394400
	public void TurnToNextPage(string pathToNextLeftPage, AmmonomiconPageRenderer.PageType leftPageType, string pathToNextRightPage, AmmonomiconPageRenderer.PageType rightPageType)
	{
		if (this.m_isPageTransitioning)
		{
			this.SetQueuedTransition(true, pathToNextLeftPage, leftPageType, pathToNextRightPage, rightPageType);
			return;
		}
		this.m_isPageTransitioning = true;
		this.m_ImpendingLeftPageManager = this.LoadPageUIAtPath(pathToNextLeftPage, leftPageType, false, false);
		this.m_ImpendingRightPageManager = this.LoadPageUIAtPath(pathToNextRightPage, rightPageType, false, false);
		this.m_ImpendingLeftPageManager.UpdateOnBecameActive();
		this.m_ImpendingRightPageManager.UpdateOnBecameActive();
		this.m_ImpendingLeftPageManager.targetRenderer.sharedMaterial.shader = ShaderCache.Acquire("Custom/AmmonomiconTransitionPageShader");
		this.m_CurrentRightPageManager.targetRenderer.sharedMaterial.shader = ShaderCache.Acquire("Custom/AmmonomiconTransitionPageShader");
		this.m_CurrentLeftPageManager.targetRenderer.sharedMaterial.shader = ShaderCache.Acquire("Custom/AmmonomiconPageShader");
		this.m_ImpendingRightPageManager.targetRenderer.sharedMaterial.shader = ShaderCache.Acquire("Custom/AmmonomiconPageShader");
		base.StartCoroutine(this.HandleTurnToNextPage());
	}

	// Token: 0x060089EF RID: 35311 RVA: 0x003962EC File Offset: 0x003944EC
	private IEnumerator HandleTurnToNextPage()
	{
		AkSoundEngine.PostEvent("Play_UI_page_turn_01", base.gameObject);
		float animationTime = this.GetAnimationLength(this.TurnPageRightAnimationFrames);
		float elapsed = 0f;
		int currentFrameIndex = 0;
		float nextFrameTime = this.TurnPageRightAnimationFrames[0].frameTime * this.GLOBAL_ANIMATION_SCALE;
		this.SetFrame(this.TurnPageRightAnimationFrames[0]);
		while (elapsed < animationTime)
		{
			if (!this.m_isOpen)
			{
				yield break;
			}
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			if (elapsed >= animationTime)
			{
				break;
			}
			if (elapsed >= nextFrameTime)
			{
				currentFrameIndex++;
				nextFrameTime += this.TurnPageRightAnimationFrames[currentFrameIndex].frameTime * this.GLOBAL_ANIMATION_SCALE;
				this.SetFrame(this.TurnPageRightAnimationFrames[currentFrameIndex]);
			}
			yield return null;
		}
		if (!this.m_isOpen)
		{
			yield break;
		}
		this.MakeImpendingCurrent();
		this.SetFrame(this.OpenAnimationFrames[this.OpenAnimationFrames.Count - 1]);
		this.m_isPageTransitioning = false;
		if (this.m_transitionIsQueued)
		{
			if (this.m_queuedNextPage)
			{
				this.TurnToNextPage(this.m_queuedLeftPath, this.m_queuedLeftType, this.m_queuedRightPath, this.m_queuedRightType);
			}
			else
			{
				this.TurnToPreviousPage(this.m_queuedLeftPath, this.m_queuedLeftType, this.m_queuedRightPath, this.m_queuedRightType);
			}
			this.m_transitionIsQueued = false;
		}
		yield break;
	}

	// Token: 0x04009032 RID: 36914
	public static string AmmonomiconErrorSprite = "zombullet_idle_front_001";

	// Token: 0x04009033 RID: 36915
	private static AmmonomiconController m_instance;

	// Token: 0x04009034 RID: 36916
	public string AmmonomiconEquipmentLeftPagePath;

	// Token: 0x04009035 RID: 36917
	public string AmmonomiconEquipmentRightPagePath;

	// Token: 0x04009036 RID: 36918
	public List<AmmonomiconFrameDefinition> OpenAnimationFrames;

	// Token: 0x04009037 RID: 36919
	public List<AmmonomiconFrameDefinition> TurnPageRightAnimationFrames;

	// Token: 0x04009038 RID: 36920
	public List<AmmonomiconFrameDefinition> TurnPageLeftAnimationFrames;

	// Token: 0x04009039 RID: 36921
	public List<AmmonomiconFrameDefinition> CloseAnimationFrames;

	// Token: 0x0400903A RID: 36922
	public AnimationCurve DepartureYCurve;

	// Token: 0x0400903B RID: 36923
	public float TotalDepartureTime = 0.5f;

	// Token: 0x0400903C RID: 36924
	public float DepartureYTotalDistance = -5f;

	// Token: 0x0400903D RID: 36925
	public tk2dSpriteCollectionData EncounterIconCollection;

	// Token: 0x0400903E RID: 36926
	private AmmonomiconFrameDefinition m_currentFrameDefinition;

	// Token: 0x0400903F RID: 36927
	public GameObject AmmonomiconBasePrefab;

	// Token: 0x04009040 RID: 36928
	[SerializeField]
	private float GLOBAL_ANIMATION_SCALE = 1f;

	// Token: 0x04009041 RID: 36929
	private GameObject m_AmmonomiconBase;

	// Token: 0x04009042 RID: 36930
	private AmmonomiconInstanceManager m_AmmonomiconInstance;

	// Token: 0x04009043 RID: 36931
	private MeshRenderer m_LowerRenderTargetPrefab;

	// Token: 0x04009044 RID: 36932
	private MeshRenderer m_UpperRenderTargetPrefab;

	// Token: 0x04009045 RID: 36933
	private dfTextureSprite m_AmmonomiconLowerImage;

	// Token: 0x04009046 RID: 36934
	private dfTextureSprite m_AmmonomiconUpperImage;

	// Token: 0x04009047 RID: 36935
	private dfTextureSprite m_AmmonomiconOptionalThirdImage;

	// Token: 0x04009048 RID: 36936
	private dfTextureSprite m_CurrentLeft_RenderTarget;

	// Token: 0x04009049 RID: 36937
	private dfTextureSprite m_CurrentRight_RenderTarget;

	// Token: 0x0400904A RID: 36938
	private AmmonomiconPageRenderer m_CurrentLeftPageManager;

	// Token: 0x0400904B RID: 36939
	private AmmonomiconPageRenderer m_CurrentRightPageManager;

	// Token: 0x0400904C RID: 36940
	private AmmonomiconPageRenderer m_ImpendingLeftPageManager;

	// Token: 0x0400904D RID: 36941
	private AmmonomiconPageRenderer m_ImpendingRightPageManager;

	// Token: 0x0400904E RID: 36942
	private bool m_isOpening;

	// Token: 0x0400904F RID: 36943
	private bool m_isOpen;

	// Token: 0x04009050 RID: 36944
	private bool m_isPageTransitioning;

	// Token: 0x04009051 RID: 36945
	private List<bool> m_offsetInUse = new List<bool>();

	// Token: 0x04009052 RID: 36946
	private List<Vector3> m_offsets = new List<Vector3>();

	// Token: 0x04009053 RID: 36947
	private const float m_PAGE_DEPTH = -0.5f;

	// Token: 0x04009054 RID: 36948
	private bool m_applicationFocus = true;

	// Token: 0x04009055 RID: 36949
	public bool HandlingQueuedUnlocks;

	// Token: 0x04009056 RID: 36950
	private float m_cachedCorePanelY = -1f;

	// Token: 0x04009057 RID: 36951
	private Dictionary<AmmonomiconPageRenderer.PageType, AmmonomiconPageRenderer> m_extantPageMap = new Dictionary<AmmonomiconPageRenderer.PageType, AmmonomiconPageRenderer>();

	// Token: 0x04009058 RID: 36952
	private bool m_transitionIsQueued;

	// Token: 0x04009059 RID: 36953
	private string m_queuedLeftPath;

	// Token: 0x0400905A RID: 36954
	private AmmonomiconPageRenderer.PageType m_queuedLeftType;

	// Token: 0x0400905B RID: 36955
	private string m_queuedRightPath;

	// Token: 0x0400905C RID: 36956
	private AmmonomiconPageRenderer.PageType m_queuedRightType;

	// Token: 0x0400905D RID: 36957
	private bool m_queuedNextPage;

	// Token: 0x0400905E RID: 36958
	private bool m_isClosing;
}
