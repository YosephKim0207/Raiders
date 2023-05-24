using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dungeonator;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200152C RID: 5420
public class Pixelator : MonoBehaviour
{
	// Token: 0x17001239 RID: 4665
	// (get) Token: 0x06007BBC RID: 31676 RVA: 0x00318D84 File Offset: 0x00316F84
	// (set) Token: 0x06007BBD RID: 31677 RVA: 0x00318DB4 File Offset: 0x00316FB4
	public static Pixelator Instance
	{
		get
		{
			if (Pixelator.m_instance == null || !Pixelator.m_instance)
			{
				Pixelator.m_instance = UnityEngine.Object.FindObjectOfType<Pixelator>();
			}
			return Pixelator.m_instance;
		}
		set
		{
			Pixelator.m_instance = value;
		}
	}

	// Token: 0x1700123A RID: 4666
	// (get) Token: 0x06007BBE RID: 31678 RVA: 0x00318DBC File Offset: 0x00316FBC
	public static bool HasInstance
	{
		get
		{
			return Pixelator.m_instance;
		}
	}

	// Token: 0x1700123B RID: 4667
	// (get) Token: 0x06007BBF RID: 31679 RVA: 0x00318DC8 File Offset: 0x00316FC8
	public Vector3 CameraOrigin
	{
		get
		{
			return this.m_camera.ViewportToWorldPoint(Vector3.zero);
		}
	}

	// Token: 0x1700123C RID: 4668
	// (get) Token: 0x06007BC0 RID: 31680 RVA: 0x00318DDC File Offset: 0x00316FDC
	// (set) Token: 0x06007BC1 RID: 31681 RVA: 0x00318DF0 File Offset: 0x00316FF0
	public Color FadeColor
	{
		get
		{
			return this.m_fadeMaterial.GetColor("_FadeColor");
		}
		set
		{
			if (this.m_fadeMaterial != null)
			{
				this.m_fadeMaterial.SetColor("_FadeColor", value);
			}
		}
	}

	// Token: 0x1700123D RID: 4669
	// (get) Token: 0x06007BC2 RID: 31682 RVA: 0x00318E14 File Offset: 0x00317014
	public bool DoBloom
	{
		get
		{
			return this.ManualDoBloom && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW;
		}
	}

	// Token: 0x06007BC3 RID: 31683 RVA: 0x00318E44 File Offset: 0x00317044
	public void RegisterAdditionalRenderPass(Material pass)
	{
		if (this.AdditionalRenderPasses.Contains(pass))
		{
			return;
		}
		this.AdditionalRenderPasses.Add(pass);
		this.AdditionalRenderPassesInitialized.Add(false);
	}

	// Token: 0x06007BC4 RID: 31684 RVA: 0x00318E70 File Offset: 0x00317070
	public void DeregisterAdditionalRenderPass(Material pass)
	{
		if (this.AdditionalRenderPasses.Contains(pass))
		{
			int num = this.AdditionalRenderPasses.IndexOf(pass);
			if (num >= 0)
			{
				this.AdditionalRenderPassesInitialized.RemoveAt(num);
				this.AdditionalRenderPasses.RemoveAt(num);
			}
		}
	}

	// Token: 0x1700123E RID: 4670
	// (get) Token: 0x06007BC5 RID: 31685 RVA: 0x00318EBC File Offset: 0x003170BC
	public Material FadeMaterial
	{
		get
		{
			return this.m_fadeMaterial;
		}
	}

	// Token: 0x1700123F RID: 4671
	// (get) Token: 0x06007BC6 RID: 31686 RVA: 0x00318EC4 File Offset: 0x003170C4
	public static Texture2D SmallBlackTexture
	{
		get
		{
			if (Pixelator.m_smallBlackTexture == null)
			{
				Pixelator.m_smallBlackTexture = new Texture2D(1, 1);
				Pixelator.m_smallBlackTexture.SetPixel(0, 0, Color.black);
				Pixelator.m_smallBlackTexture.Apply();
			}
			return Pixelator.m_smallBlackTexture;
		}
	}

	// Token: 0x06007BC7 RID: 31687 RVA: 0x00318F04 File Offset: 0x00317104
	public void SetOcclusionDirty()
	{
		this.m_occlusionGridDirty = true;
		this.m_occlusionDirty = true;
	}

	// Token: 0x17001240 RID: 4672
	// (get) Token: 0x06007BC8 RID: 31688 RVA: 0x00318F14 File Offset: 0x00317114
	private float m_deltaTime
	{
		get
		{
			return GameManager.INVARIANT_DELTA_TIME;
		}
	}

	// Token: 0x17001241 RID: 4673
	// (get) Token: 0x06007BC9 RID: 31689 RVA: 0x00318F1C File Offset: 0x0031711C
	// (set) Token: 0x06007BCA RID: 31690 RVA: 0x00318F24 File Offset: 0x00317124
	public int CurrentMacroResolutionX
	{
		get
		{
			return this.m_currentMacroResolutionX;
		}
		set
		{
			this.m_currentMacroResolutionX = value;
		}
	}

	// Token: 0x17001242 RID: 4674
	// (get) Token: 0x06007BCB RID: 31691 RVA: 0x00318F30 File Offset: 0x00317130
	// (set) Token: 0x06007BCC RID: 31692 RVA: 0x00318F38 File Offset: 0x00317138
	public int CurrentMacroResolutionY
	{
		get
		{
			return this.m_currentMacroResolutionY;
		}
		set
		{
			this.m_currentMacroResolutionY = value;
		}
	}

	// Token: 0x17001243 RID: 4675
	// (get) Token: 0x06007BCD RID: 31693 RVA: 0x00318F44 File Offset: 0x00317144
	public Rect CurrentCameraRect
	{
		get
		{
			return this.m_camera.rect;
		}
	}

	// Token: 0x06007BCE RID: 31694 RVA: 0x00318F54 File Offset: 0x00317154
	private void InitializePerPlatform()
	{
		this.PLATFORM_DEPTH = 24;
		bool flag = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.DefaultHDR);
		this.PLATFORM_RENDER_FORMAT = ((!flag) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR);
		if (!flag)
		{
			this.m_camera.hdr = false;
			base.GetComponent<SENaturalBloomAndDirtyLens>().enabled = false;
		}
	}

	// Token: 0x06007BCF RID: 31695 RVA: 0x00318FA4 File Offset: 0x003171A4
	private void Awake()
	{
		AkAudioListener[] components = base.GetComponents<AkAudioListener>();
		for (int i = 0; i < components.Length; i++)
		{
			if (components[i] != null)
			{
				UnityEngine.Object.Destroy(components[i]);
			}
		}
		this.m_gammaEffect = base.GetComponent<GenericFullscreenEffect>();
		this.m_reflMapID = Shader.PropertyToID("_ReflMapFromPixelator");
		this.m_reflFlipID = Shader.PropertyToID("_ReflectionYFactor");
		this.m_gammaID = Shader.PropertyToID("_GammaGamma");
		this.m_saturationID = Shader.PropertyToID("_Saturation");
		this.m_fadeID = Shader.PropertyToID("_Fade");
		this.m_fadeColorID = Shader.PropertyToID("_FadeColor");
		this.m_occlusionMapID = Shader.PropertyToID("_OcclusionMap");
		this.m_gBufferID = Shader.PropertyToID("_GBuffer");
		this.m_occlusionUVID = Shader.PropertyToID("_OcclusionUV");
		this.m_vignettePowerID = Shader.PropertyToID("_VignettePower");
		this.m_vignetteColorID = Shader.PropertyToID("_VignetteColor");
		this.m_damagedTexID = Shader.PropertyToID("_DamagedTex");
		this.m_cameraWSID = Shader.PropertyToID("_CameraWS");
		this.m_cameraOrthoSizeID = Shader.PropertyToID("_CameraOrthoSize");
		this.m_cameraOrthoSizeXID = Shader.PropertyToID("_CameraOrthoSizeX");
		this.m_lightPosID = Shader.PropertyToID("_LightPos");
		this.m_lightColorID = Shader.PropertyToID("_LightColor");
		this.m_lightRadiusID = Shader.PropertyToID("_LightRadius");
		this.m_lightIntensityID = Shader.PropertyToID("_LightIntensity");
		this.m_lightCookieID = Shader.PropertyToID("_LightCookie");
		this.m_lightCookieAngleID = Shader.PropertyToID("_LightCookieAngle");
		this.m_lightMaskTexID = Shader.PropertyToID("_LightMaskTex");
		this.m_preBackgroundTexID = Shader.PropertyToID("_PreBackgroundTex");
		this.m_camera = base.GetComponent<Camera>();
		this.m_simpleSpriteMaskShader = ShaderCache.Acquire("Brave/Internal/SimpleSpriteMask");
		this.m_simpleSpriteMaskUnpixelatedShader = ShaderCache.Acquire("Brave/Internal/SimpleSpriteMaskUnpixelated");
		this.InitializePerPlatform();
		BraveCameraUtility.MaintainCameraAspect(this.m_camera);
		if (Pixelator.m_smallBlackTexture == null)
		{
			Pixelator.m_smallBlackTexture = new Texture2D(1, 1);
			Pixelator.m_smallBlackTexture.SetPixel(0, 0, Color.black);
			Pixelator.m_smallBlackTexture.Apply();
		}
		this.m_smallWhiteTexture = new Texture2D(1, 1);
		this.m_smallWhiteTexture.SetPixel(0, 0, Color.white);
		this.m_smallWhiteTexture.Apply();
		this.m_bloomer = base.GetComponent<SENaturalBloomAndDirtyLens>();
		this.cm_occlusionPartition = 1 << LayerMask.NameToLayer("OcclusionRenderPartition");
		this.cm_core1 = 1 << LayerMask.NameToLayer("BG_Nonsense");
		this.cm_core2 = 1 << LayerMask.NameToLayer("BG_Critical");
		this.cm_core3 = (1 << LayerMask.NameToLayer("FG_Nonsense")) | (1 << LayerMask.NameToLayer("ShadowCaster")) | (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Water"));
		this.cm_refl = 1 << LayerMask.NameToLayer("FG_Reflection");
		this.cm_gbuffer = (1 << LayerMask.NameToLayer("FG_Nonsense")) | (1 << LayerMask.NameToLayer("ShadowCaster")) | (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("BG_Nonsense")) | (1 << LayerMask.NameToLayer("BG_Critical")) | (1 << LayerMask.NameToLayer("FG_Critical")) | (1 << LayerMask.NameToLayer("FG_Reflection")) | (1 << LayerMask.NameToLayer("Unpixelated")) | (1 << LayerMask.NameToLayer("Unfaded"));
		this.cm_gbufferSimple = (1 << LayerMask.NameToLayer("FG_Nonsense")) | (1 << LayerMask.NameToLayer("ShadowCaster")) | (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("BG_Nonsense")) | (1 << LayerMask.NameToLayer("BG_Critical")) | (1 << LayerMask.NameToLayer("FG_Critical")) | (1 << LayerMask.NameToLayer("FG_Reflection")) | (1 << LayerMask.NameToLayer("Unfaded"));
		this.cm_fg = (1 << LayerMask.NameToLayer("FG_Nonsense")) | (1 << LayerMask.NameToLayer("ShadowCaster")) | (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("FG_Reflection")) | (1 << LayerMask.NameToLayer("FG_Critical"));
		this.cm_fg_important = (1 << LayerMask.NameToLayer("ShadowCaster")) | (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("FG_Reflection")) | (1 << LayerMask.NameToLayer("FG_Critical"));
		this.cm_unoccluded = 1 << LayerMask.NameToLayer("Unoccluded");
		this.cm_unfaded = 1 << LayerMask.NameToLayer("Unfaded");
		if (GameManager.Options == null)
		{
			GameOptions.Load();
		}
		this.OnChangedMotionEnhancementMode(GameManager.Options.MotionEnhancementMode);
		this.OnChangedLightingQuality(GameManager.Options.LightingQuality);
	}

	// Token: 0x06007BD0 RID: 31696 RVA: 0x003194B0 File Offset: 0x003176B0
	public void OnChangedLightingQuality(GameOptions.GenericHighMedLowOption lightingQuality)
	{
		switch (lightingQuality)
		{
		case GameOptions.GenericHighMedLowOption.LOW:
		case GameOptions.GenericHighMedLowOption.VERY_LOW:
			this.m_gammaAdjustment = -0.1f;
			QualitySettings.pixelLightCount = 0;
			break;
		case GameOptions.GenericHighMedLowOption.MEDIUM:
			this.m_gammaAdjustment = 0f;
			QualitySettings.pixelLightCount = 4;
			break;
		case GameOptions.GenericHighMedLowOption.HIGH:
			this.m_gammaAdjustment = 0f;
			QualitySettings.pixelLightCount = 16;
			break;
		}
	}

	// Token: 0x06007BD1 RID: 31697 RVA: 0x00319520 File Offset: 0x00317720
	public void OnChangedMotionEnhancementMode(GameOptions.PixelatorMotionEnhancementMode newMode)
	{
		if (newMode == GameOptions.PixelatorMotionEnhancementMode.ENHANCED_EXPENSIVE)
		{
			this.cm_core4 = (1 << LayerMask.NameToLayer("FG_Critical")) | (1 << LayerMask.NameToLayer("FG_Reflection"));
			this.cm_unpixelated = 1 << LayerMask.NameToLayer("Unpixelated");
		}
		else if (newMode == GameOptions.PixelatorMotionEnhancementMode.UNENHANCED_CHEAP)
		{
			this.cm_core4 = (1 << LayerMask.NameToLayer("FG_Critical")) | (1 << LayerMask.NameToLayer("FG_Reflection")) | (1 << LayerMask.NameToLayer("Unpixelated"));
			this.cm_unpixelated = 0;
		}
		else
		{
			Debug.LogError("Unsupported MotionEnhancementMode in Pixelator. This should never, ever happen.");
		}
	}

	// Token: 0x06007BD2 RID: 31698 RVA: 0x003195C4 File Offset: 0x003177C4
	public static void DEBUG_LogSystemRenderingData()
	{
		Debug.Log("BRV::DeviceType = " + SystemInfo.deviceType.ToString());
		Debug.Log("BRV::GraphicsDeviceType = " + SystemInfo.graphicsDeviceName.ToString());
		Debug.Log("BRV::GraphicsDeviceType = " + SystemInfo.graphicsDeviceType.ToString());
		Debug.Log("BRV::GraphicsDeviceVendor = " + SystemInfo.graphicsDeviceVendor.ToString());
		Debug.Log("BRV::GraphicsDeviceVersion = " + SystemInfo.graphicsDeviceVersion.ToString());
		Debug.Log("BRV::GraphicsShaderLevel = " + SystemInfo.graphicsShaderLevel);
		Debug.Log("BRV::GraphicsMemorySize = " + SystemInfo.graphicsMemorySize);
		Debug.Log("BRV::MaxTextureSize = " + SystemInfo.maxTextureSize);
		Debug.Log("BRV::NPOTSupport = " + SystemInfo.npotSupport);
		Debug.Log("BRV::SupportedRenderTargetCount = " + SystemInfo.supportedRenderTargetCount);
		Debug.Log("BRV::SupportsImageEffects = " + SystemInfo.supportsImageEffects);
		Debug.Log("BRV::SupportsRenderTextures = " + SystemInfo.supportsRenderTextures);
		Debug.Log("BRV::SupportsStencil = " + SystemInfo.supportsStencil);
		Debug.Log("BRV::SupportsDefaultHDR = " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.DefaultHDR));
		Debug.Log("BRV::SupportsDepthFormat = " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth));
		Debug.Log("BRV::Iteration = 1");
	}

	// Token: 0x17001244 RID: 4676
	// (get) Token: 0x06007BD3 RID: 31699 RVA: 0x00319768 File Offset: 0x00317968
	private bool IsInIntro
	{
		get
		{
			return GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER && Foyer.DoIntroSequence;
		}
	}

	// Token: 0x17001245 RID: 4677
	// (get) Token: 0x06007BD4 RID: 31700 RVA: 0x00319788 File Offset: 0x00317988
	private bool IsInTitle
	{
		get
		{
			return GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER && Foyer.DoMainMenu;
		}
	}

	// Token: 0x17001246 RID: 4678
	// (get) Token: 0x06007BD5 RID: 31701 RVA: 0x003197A8 File Offset: 0x003179A8
	private bool IsInPunchout
	{
		get
		{
			return PunchoutController.IsActive;
		}
	}

	// Token: 0x06007BD6 RID: 31702 RVA: 0x003197B0 File Offset: 0x003179B0
	public void SetVignettePower(float tp)
	{
		if (this.m_fadeMaterial != null)
		{
			this.m_fadeMaterial.SetFloat(this.m_vignettePowerID, tp);
		}
		if (this.m_combinedVignetteFadeMaterial != null)
		{
			this.m_combinedVignetteFadeMaterial.SetFloat(this.m_vignettePowerID, tp);
		}
	}

	// Token: 0x06007BD7 RID: 31703 RVA: 0x00319804 File Offset: 0x00317A04
	private void Start()
	{
		if (!this.IsInIntro)
		{
			this.minimapCameraRef = Minimap.Instance.cameraRef;
		}
		if (GameManager.Instance.Dungeon != null && GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.WESTGEON)
		{
			this.UseTexturedOcclusion = true;
		}
		if (this.vignetteShader != null)
		{
			this.m_vignetteMaterial = new Material(this.vignetteShader);
			this.m_vignetteMaterial.SetColor("_OcclusionFallbackColor", this.occludedColor);
		}
		if (this.m_combinedVignetteFadeMaterial == null)
		{
			this.m_combinedVignetteFadeMaterial = new Material(ShaderCache.Acquire("Brave/CameraEffects/Pixelator_VignetteFade"));
			this.m_combinedVignetteFadeMaterial.SetColor("_OcclusionFallbackColor", this.occludedColor);
			this.m_combinedVignetteFadeMaterial.SetFloat(this.m_vignettePowerID, this.vignettePower);
			this.m_combinedVignetteFadeMaterial.SetColor(this.m_vignetteColorID, this.vignetteColor);
			this.m_combinedVignetteFadeMaterial.SetTexture(this.m_damagedTexID, this.ouchTexture);
			this.m_combinedVignetteFadeMaterial.SetVector("_LowlightColor", GameManager.Instance.BestGenerationDungeonPrefab.decoSettings.lowQualityCheapLightVector);
		}
		if (this.fadeShader != null)
		{
			this.m_fadeMaterial = new Material(this.fadeShader);
			this.m_fadeMaterial.SetFloat(this.m_vignettePowerID, this.vignettePower);
			this.m_fadeMaterial.SetColor(this.m_vignetteColorID, this.vignetteColor);
			this.m_fadeMaterial.SetTexture(this.m_damagedTexID, this.ouchTexture);
		}
		this.m_pointLightMaterial = new Material(ShaderCache.Acquire("Brave/Internal/GBuffer_LightRenderer"));
		this.m_pointLightMaterialFast = new Material(ShaderCache.Acquire("Brave/Internal/GBuffer_LightRenderer_Fast"));
		this.m_gbufferMaskMaterial = new Material(ShaderCache.Acquire("Brave/Internal/GBuffer_LightMask"));
		this.m_gbufferLightMaskCombinerMaterial = new Material(ShaderCache.Acquire("Brave/Internal/GBuffer_LightMaskCombiner"));
		this.m_partialCopyMaterial = new Material(ShaderCache.Acquire("Brave/Internal/PartialCopy"));
		this.occluder = new OcclusionLayer();
		this.occluder.SourceOcclusionTexture = this.sourceOcclusionTexture;
		this.occluder.occludedColor = this.occludedColor;
		this.overrideTileScale = 1;
		this.CheckSize();
		base.StartCoroutine(this.BackgroundCoroutineProcessor());
		if (GameManager.Instance.BestGenerationDungeonPrefab && GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON)
		{
			this.SetLumaGain(0.1f);
		}
		else
		{
			this.SetLumaGain(0f);
		}
	}

	// Token: 0x06007BD8 RID: 31704 RVA: 0x00319AA8 File Offset: 0x00317CA8
	private void OnDestroy()
	{
		if (this.m_reflectionTargetTexture != null)
		{
			UnityEngine.Object.Destroy(this.m_reflectionTargetTexture);
		}
	}

	// Token: 0x06007BD9 RID: 31705 RVA: 0x00319AC8 File Offset: 0x00317CC8
	public void MarkOcclusionDirty()
	{
		this.m_occlusionDirty = true;
	}

	// Token: 0x06007BDA RID: 31706 RVA: 0x00319AD4 File Offset: 0x00317CD4
	private bool IsExitDetailCell(CellData neighbor, CellData current)
	{
		return neighbor.isExitNonOccluder;
	}

	// Token: 0x06007BDB RID: 31707 RVA: 0x00319AEC File Offset: 0x00317CEC
	private IEnumerator BackgroundCoroutineProcessor()
	{
		for (;;)
		{
			bool isFoyer = GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER;
			float privateDeltaTime = this.m_deltaTime;
			if (this.m_occlusionGridDirty)
			{
				DungeonData data = GameManager.Instance.Dungeon.data;
				bool flag = false;
				for (int i = 0; i < this.m_modifiedRangeMins.Count; i++)
				{
					IntVector2 intVector = this.m_modifiedRangeMins[i];
					IntVector2 intVector2 = this.m_modifiedRangeMaxs[i];
					bool flag2 = false;
					for (int j = intVector.x; j <= intVector2.x; j++)
					{
						for (int k = intVector.y; k <= intVector2.y; k++)
						{
							if (data.CheckInBounds(j, k))
							{
								CellData cellData = data[j, k];
								if (cellData != null)
								{
									CellData cellData2 = cellData;
									if (cellData.IsAnyFaceWall() && data.CheckInBoundsAndValid(j, k - 2))
									{
										cellData2 = data[j, k - 2];
									}
									if (cellData.occlusionData.cellOcclusionDirty)
									{
										if (cellData.occlusionData.remainingDelay > 0f)
										{
											cellData.occlusionData.remainingDelay = Mathf.Max(0f, cellData.occlusionData.remainingDelay - privateDeltaTime);
											flag2 = true;
										}
										else
										{
											float num = 0.7f;
											if (cellData2.parentRoom != null && cellData2.occlusionData.occlusionParentDefintion != null)
											{
												if (cellData2.parentRoom.visibility == RoomHandler.VisibilityStatus.CURRENT || isFoyer)
												{
													num = cellData.occlusionData.cellVisibleTargetOcclusion;
												}
												else
												{
													num = cellData.occlusionData.cellVisitedTargetOcclusion;
												}
											}
											else if (cellData2.occlusionData.occlusionParentDefintion != null)
											{
												if ((cellData2.occlusionData.occlusionParentDefintion.downstreamRoom != null && cellData2.occlusionData.occlusionParentDefintion.downstreamRoom.IsSecretRoom) || (cellData2.occlusionData.occlusionParentDefintion.upstreamRoom != null && cellData2.occlusionData.occlusionParentDefintion.upstreamRoom.IsSecretRoom))
												{
													num = cellData.occlusionData.cellVisibleTargetOcclusion;
												}
												else if (cellData2.occlusionData.occlusionParentDefintion.Visibility == RoomHandler.VisibilityStatus.CURRENT || isFoyer)
												{
													num = cellData.occlusionData.cellVisibleTargetOcclusion;
												}
												else
												{
													num = cellData.occlusionData.cellVisitedTargetOcclusion;
												}
											}
											else if (cellData2.parentRoom != null || cellData2.cellVisualData.IsFeatureCell)
											{
												RoomHandler roomHandler = ((!cellData2.cellVisualData.IsFeatureCell) ? cellData2.parentRoom : cellData2.nearestRoom);
												if (roomHandler.visibility == RoomHandler.VisibilityStatus.CURRENT || isFoyer)
												{
													num = cellData.occlusionData.cellVisibleTargetOcclusion;
												}
												else
												{
													num = cellData.occlusionData.cellVisitedTargetOcclusion;
												}
											}
											else if (cellData.occlusionData.cellRoomVisiblityCount > 0)
											{
												num = cellData.occlusionData.cellVisibleTargetOcclusion;
											}
											else if (cellData.occlusionData.cellRoomVisitedCount > 0)
											{
												num = cellData.occlusionData.cellVisitedTargetOcclusion;
											}
											if (cellData.occlusionData.overrideOcclusion)
											{
												num = 0f;
											}
											float num2 = num - cellData.occlusionData.cellOcclusion;
											float num3 = Mathf.Sign(num2) * Mathf.Min(Mathf.Abs(num2), privateDeltaTime * this.occlusionTransitionFadeMultiplier);
											CellData cellData3 = cellData;
											cellData3.occlusionData.cellOcclusion = cellData3.occlusionData.cellOcclusion + num3;
											cellData.occlusionData.minCellOccluionHistory = Mathf.Min(cellData.occlusionData.minCellOccluionHistory, cellData.occlusionData.cellOcclusion);
											if (cellData.occlusionData.cellOcclusion == num)
											{
												cellData.occlusionData.cellOcclusionDirty = false;
											}
											else
											{
												flag2 = true;
											}
											if (cellData.occlusionData.overrideOcclusion)
											{
												cellData.occlusionData.cellOcclusion = 0f;
											}
										}
									}
								}
							}
						}
					}
					if (!flag2)
					{
						this.m_modifiedRangeMins.RemoveAt(i);
						this.m_modifiedRangeMaxs.RemoveAt(i);
						i--;
					}
					else
					{
						flag = true;
						this.MarkOcclusionDirty();
					}
				}
				if (!flag)
				{
					this.m_occlusionGridDirty = false;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06007BDC RID: 31708 RVA: 0x00319B08 File Offset: 0x00317D08
	public float ProcessOcclusionChange(IntVector2 startingPosition, float targetVisibility, RoomHandler source, bool useFloodFill = true)
	{
		return this.HandleRoomOcclusionChange(startingPosition, source, useFloodFill);
	}

	// Token: 0x06007BDD RID: 31709 RVA: 0x00319B24 File Offset: 0x00317D24
	public void ProcessRoomAdditionalExits(IntVector2 startingPosition, RoomHandler source, bool useFloodFill = true)
	{
		this.HandleRoomExitsCheck(startingPosition, source, useFloodFill);
	}

	// Token: 0x06007BDE RID: 31710 RVA: 0x00319B30 File Offset: 0x00317D30
	protected List<CellData> GetExitCellsToProcess(IntVector2 startingPosition, RoomHandler targetRoom, RoomHandler currentVisibleRoom, DungeonData data)
	{
		List<CellData> list = new List<CellData>();
		if (!targetRoom.area.IsProceduralRoom)
		{
			for (int i = 0; i < targetRoom.area.instanceUsedExits.Count; i++)
			{
				RuntimeRoomExitData runtimeRoomExitData = targetRoom.area.exitToLocalDataMap[targetRoom.area.instanceUsedExits[i]];
				RuntimeExitDefinition runtimeExitDefinition = targetRoom.exitDefinitionsByExit[runtimeRoomExitData];
				if (!runtimeExitDefinition.downstreamRoom.IsSecretRoom || targetRoom != runtimeExitDefinition.upstreamRoom)
				{
					if (!runtimeExitDefinition.upstreamRoom.IsSecretRoom || targetRoom != runtimeExitDefinition.downstreamRoom)
					{
						foreach (IntVector2 intVector in runtimeExitDefinition.GetCellsForRoom(targetRoom))
						{
							CellData cellData = data[intVector];
							if (cellData != null)
							{
								list.Add(cellData);
							}
							CellData cellData2 = data[cellData.position + IntVector2.Up];
							if (cellData2 != null)
							{
								list.Add(cellData2);
							}
							CellData cellData3 = data[cellData.position + IntVector2.Up * 2];
							if (cellData3 != null)
							{
								list.Add(cellData3);
							}
						}
						if (runtimeExitDefinition.upstreamExit != null && runtimeExitDefinition.upstreamExit.isWarpWingStart && runtimeExitDefinition.upstreamExit.warpWingPortal != null && runtimeExitDefinition.upstreamExit.warpWingPortal.failPortal != null && runtimeExitDefinition.upstreamExit.warpWingPortal.parentRoom == targetRoom)
						{
							RuntimeExitDefinition runtimeExitDefinition2 = runtimeExitDefinition.upstreamExit.warpWingPortal.failPortal.parentRoom.exitDefinitionsByExit[runtimeExitDefinition.upstreamExit.warpWingPortal.failPortal.parentExit];
							foreach (IntVector2 intVector2 in runtimeExitDefinition2.GetCellsForRoom(runtimeExitDefinition.upstreamExit.warpWingPortal.failPortal.parentRoom))
							{
								BraveUtility.DrawDebugSquare(intVector2.ToVector2(), Color.yellow, 1000f);
								CellData cellData4 = data[intVector2];
								if (cellData4 != null)
								{
									list.Add(cellData4);
								}
								CellData cellData5 = data[cellData4.position + IntVector2.Up];
								if (cellData5 != null)
								{
									list.Add(cellData5);
								}
								CellData cellData6 = data[cellData4.position + IntVector2.Up * 2];
								if (cellData6 != null)
								{
									list.Add(cellData6);
								}
							}
						}
						if (runtimeExitDefinition.downstreamExit != null && runtimeExitDefinition.downstreamExit.isWarpWingStart && runtimeExitDefinition.downstreamExit.warpWingPortal != null && runtimeExitDefinition.downstreamExit.warpWingPortal.failPortal != null && runtimeExitDefinition.downstreamExit.warpWingPortal.parentRoom == targetRoom)
						{
							RuntimeExitDefinition runtimeExitDefinition3 = runtimeExitDefinition.downstreamExit.warpWingPortal.failPortal.parentRoom.exitDefinitionsByExit[runtimeExitDefinition.downstreamExit.warpWingPortal.failPortal.parentExit];
							foreach (IntVector2 intVector3 in runtimeExitDefinition3.GetCellsForRoom(runtimeExitDefinition.downstreamExit.warpWingPortal.failPortal.parentRoom))
							{
								BraveUtility.DrawDebugSquare(intVector3.ToVector2(), Color.yellow, 1000f);
								CellData cellData7 = data[intVector3];
								list.Add(cellData7);
								CellData cellData8 = data[cellData7.position + IntVector2.Up];
								list.Add(cellData8);
								CellData cellData9 = data[cellData7.position + IntVector2.Up * 2];
								list.Add(cellData9);
							}
						}
						if (runtimeExitDefinition.IntermediaryCells != null)
						{
							foreach (IntVector2 intVector4 in runtimeExitDefinition.IntermediaryCells)
							{
								CellData cellData10 = data[intVector4];
								list.Add(cellData10);
								CellData cellData11 = data[cellData10.position + IntVector2.Up];
								list.Add(cellData11);
								CellData cellData12 = data[cellData10.position + IntVector2.Up * 2];
								list.Add(cellData12);
							}
						}
					}
				}
			}
		}
		else
		{
			for (int j = 0; j < targetRoom.connectedRooms.Count; j++)
			{
				RoomHandler roomHandler = targetRoom.connectedRooms[j];
				PrototypeRoomExit exitConnectedToRoom = roomHandler.GetExitConnectedToRoom(targetRoom);
				if (exitConnectedToRoom != null)
				{
					RuntimeExitDefinition runtimeExitDefinition4 = roomHandler.exitDefinitionsByExit[roomHandler.area.exitToLocalDataMap[exitConnectedToRoom]];
					HashSet<IntVector2> cellsForRoom = runtimeExitDefinition4.GetCellsForRoom(targetRoom);
					foreach (IntVector2 intVector5 in cellsForRoom)
					{
						CellData cellData13 = data[intVector5];
						if (cellData13 != null)
						{
							list.Add(cellData13);
						}
						CellData cellData14 = data[cellData13.position + IntVector2.Up];
						if (cellData14 != null)
						{
							list.Add(cellData14);
						}
						CellData cellData15 = data[cellData13.position + IntVector2.Up * 2];
						if (cellData15 != null)
						{
							list.Add(cellData15);
						}
					}
					if (runtimeExitDefinition4.IntermediaryCells != null)
					{
						foreach (IntVector2 intVector6 in runtimeExitDefinition4.IntermediaryCells)
						{
							CellData cellData16 = data[intVector6];
							if (cellData16 != null)
							{
								list.Add(cellData16);
							}
							CellData cellData17 = data[cellData16.position + IntVector2.Up];
							if (cellData17 != null)
							{
								list.Add(cellData17);
							}
							CellData cellData18 = data[cellData16.position + IntVector2.Up * 2];
							if (cellData18 != null)
							{
								list.Add(cellData18);
							}
						}
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06007BDF RID: 31711 RVA: 0x0031A224 File Offset: 0x00318424
	protected void HandleRoomExitsCheck(IntVector2 startingPosition, RoomHandler targetRoom, bool useFloodFill = true)
	{
		int num = ((targetRoom.visibility != RoomHandler.VisibilityStatus.CURRENT) ? (-1) : 1);
		int num2 = ((targetRoom.visibility != RoomHandler.VisibilityStatus.VISITED) ? 0 : 1);
		RoomHandler roomHandler = ((!(GameManager.Instance.PrimaryPlayer == null)) ? GameManager.Instance.PrimaryPlayer.CurrentRoom : GameManager.Instance.Dungeon.data.Entrance);
		DungeonData data = GameManager.Instance.Dungeon.data;
		List<CellData> exitCellsToProcess = this.GetExitCellsToProcess(startingPosition, targetRoom, roomHandler, data);
		this.m_occlusionGridDirty = true;
		IntVector2 intVector = IntVector2.MaxValue;
		IntVector2 intVector2 = IntVector2.MinValue;
		for (int i = 0; i < exitCellsToProcess.Count; i++)
		{
			CellData cellData = exitCellsToProcess[i];
			if (cellData != null)
			{
				float num3 = IntVector2.Distance(cellData.position, startingPosition);
				cellData.occlusionData.remainingDelay = ((!useFloodFill) ? 0f : (num3 / 35f));
				if (targetRoom.visibility == RoomHandler.VisibilityStatus.REOBSCURED)
				{
					cellData.occlusionData.cellRoomVisiblityCount = 0;
					cellData.occlusionData.cellRoomVisitedCount = 0;
					cellData.occlusionData.cellVisitedTargetOcclusion = 1f;
					cellData.occlusionData.minCellOccluionHistory = 1f;
				}
				else
				{
					cellData.occlusionData.cellRoomVisiblityCount = Mathf.RoundToInt(Mathf.Clamp01((float)num));
					cellData.occlusionData.cellRoomVisitedCount = Mathf.RoundToInt(Mathf.Clamp01((float)num2));
					cellData.occlusionData.cellVisibleTargetOcclusion = 0f;
					cellData.occlusionData.cellVisitedTargetOcclusion = 0.7f;
				}
				cellData.occlusionData.cellOcclusionDirty = true;
				intVector = IntVector2.Min(intVector, cellData.position);
				intVector2 = IntVector2.Max(intVector2, cellData.position);
			}
		}
		this.ProcessModifiedRanges(intVector + new IntVector2(-3, -3), intVector2 + new IntVector2(3, 3));
	}

	// Token: 0x06007BE0 RID: 31712 RVA: 0x0031A420 File Offset: 0x00318620
	public void ProcessModifiedRanges(IntVector2 newMin, IntVector2 newMax)
	{
		bool flag = false;
		for (int i = 0; i < this.m_modifiedRangeMins.Count; i++)
		{
			if (IntVector2.AABBOverlap(newMin, newMax - newMin, this.m_modifiedRangeMins[i], this.m_modifiedRangeMaxs[i] - this.m_modifiedRangeMins[i]))
			{
				this.m_modifiedRangeMins[i] = IntVector2.Min(this.m_modifiedRangeMins[i], newMin);
				this.m_modifiedRangeMaxs[i] = IntVector2.Max(this.m_modifiedRangeMaxs[i], newMax);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			this.m_modifiedRangeMins.Add(newMin);
			this.m_modifiedRangeMaxs.Add(newMax);
		}
	}

	// Token: 0x06007BE1 RID: 31713 RVA: 0x0031A4E8 File Offset: 0x003186E8
	protected float HandleRoomOcclusionChange(IntVector2 startingPosition, RoomHandler targetRoom, bool useFloodFill = true)
	{
		if (targetRoom.PreventRevealEver)
		{
			return 0f;
		}
		int num = ((targetRoom.visibility != RoomHandler.VisibilityStatus.CURRENT) ? (-1) : 1);
		int num2 = ((targetRoom.visibility != RoomHandler.VisibilityStatus.VISITED) ? 0 : 1);
		RoomHandler roomHandler = ((!(GameManager.Instance.PrimaryPlayer == null)) ? GameManager.Instance.PrimaryPlayer.CurrentRoom : GameManager.Instance.Dungeon.data.Entrance);
		DungeonData data = GameManager.Instance.Dungeon.data;
		HashSet<CellData> hashSet = new HashSet<CellData>();
		for (int i = 0; i < targetRoom.CellsWithoutExits.Count; i++)
		{
			CellData cellData = data[targetRoom.CellsWithoutExits[i]];
			if (cellData != null)
			{
				if (!cellData.isSecretRoomCell || targetRoom.IsSecretRoom)
				{
					hashSet.Add(cellData);
					if (cellData.position.y + 1 < data.Height)
					{
						CellData cellData2 = data[cellData.position + IntVector2.Up];
						if (cellData2 != null)
						{
							hashSet.Add(cellData2);
						}
					}
					if (cellData.position.y + 2 < data.Height)
					{
						CellData cellData3 = data[cellData.position + IntVector2.Up * 2];
						if (cellData3 != null)
						{
							hashSet.Add(cellData3);
						}
					}
					if (this.UseTexturedOcclusion)
					{
						for (int j = 0; j < IntVector2.Cardinals.Length; j++)
						{
							CellData cellData4 = data[cellData.position + IntVector2.Cardinals[j]];
							if (cellData4 != null)
							{
								hashSet.Add(cellData4);
							}
						}
					}
				}
			}
		}
		List<CellData> exitCellsToProcess = this.GetExitCellsToProcess(startingPosition, targetRoom, roomHandler, data);
		for (int k = 0; k < exitCellsToProcess.Count; k++)
		{
			if (exitCellsToProcess[k] != null)
			{
				hashSet.Add(exitCellsToProcess[k]);
			}
		}
		for (int l = 0; l < targetRoom.FeatureCells.Count; l++)
		{
			CellData cellData5 = data[targetRoom.FeatureCells[l]];
			if (cellData5 != null)
			{
				hashSet.Add(cellData5);
			}
		}
		this.m_occlusionGridDirty = true;
		IntVector2 intVector = IntVector2.MaxValue;
		IntVector2 intVector2 = IntVector2.MinValue;
		float num3 = 0f;
		if (this.occlusionRevealSpeed <= 0f)
		{
			useFloodFill = false;
		}
		if (useFloodFill)
		{
			foreach (CellData cellData6 in hashSet)
			{
				if (cellData6 != null)
				{
					float num4 = IntVector2.Distance(cellData6.position, startingPosition);
					cellData6.occlusionData.remainingDelay = num4 / this.occlusionRevealSpeed;
					num3 = Mathf.Max(num3, cellData6.occlusionData.remainingDelay);
					if (targetRoom.visibility == RoomHandler.VisibilityStatus.REOBSCURED)
					{
						cellData6.occlusionData.cellRoomVisiblityCount = 0;
						cellData6.occlusionData.cellRoomVisitedCount = 0;
						cellData6.occlusionData.cellVisibleTargetOcclusion = 1f;
						cellData6.occlusionData.cellVisitedTargetOcclusion = 1f;
						cellData6.occlusionData.minCellOccluionHistory = 1f;
					}
					else
					{
						cellData6.occlusionData.cellRoomVisiblityCount = Mathf.RoundToInt(Mathf.Clamp01((float)num));
						cellData6.occlusionData.cellRoomVisitedCount = Mathf.RoundToInt(Mathf.Clamp01((float)num2));
						cellData6.occlusionData.cellVisibleTargetOcclusion = 0f;
						cellData6.occlusionData.cellVisitedTargetOcclusion = 0.7f;
					}
					cellData6.occlusionData.cellOcclusionDirty = true;
					intVector = IntVector2.Min(intVector, cellData6.position);
					intVector2 = IntVector2.Max(intVector2, cellData6.position);
				}
			}
		}
		else
		{
			foreach (CellData cellData7 in hashSet)
			{
				if (cellData7 != null)
				{
					cellData7.occlusionData.remainingDelay = 0f;
					if (targetRoom.visibility == RoomHandler.VisibilityStatus.REOBSCURED)
					{
						cellData7.occlusionData.cellRoomVisiblityCount = 0;
						cellData7.occlusionData.cellRoomVisitedCount = 0;
						cellData7.occlusionData.cellVisibleTargetOcclusion = 1f;
						cellData7.occlusionData.cellVisitedTargetOcclusion = 1f;
						cellData7.occlusionData.minCellOccluionHistory = 1f;
					}
					else
					{
						cellData7.occlusionData.cellRoomVisiblityCount = Mathf.RoundToInt(Mathf.Clamp01((float)num));
						cellData7.occlusionData.cellRoomVisitedCount = Mathf.RoundToInt(Mathf.Clamp01((float)num2));
						cellData7.occlusionData.cellVisibleTargetOcclusion = 0f;
						cellData7.occlusionData.cellVisitedTargetOcclusion = 0.7f;
					}
					cellData7.occlusionData.cellOcclusionDirty = true;
					intVector = IntVector2.Min(intVector, cellData7.position);
					intVector2 = IntVector2.Max(intVector2, cellData7.position);
				}
			}
		}
		this.ProcessModifiedRanges(intVector + new IntVector2(-3, -3), intVector2 + new IntVector2(3, 3));
		return num3;
	}

	// Token: 0x06007BE2 RID: 31714 RVA: 0x0031AA7C File Offset: 0x00318C7C
	private void CheckSize()
	{
		if ((float)GameManager.Instance.Dungeon.Height > this.m_camera.farClipPlane)
		{
			this.m_camera.farClipPlane = (float)(GameManager.Instance.Dungeon.Height + 50);
		}
		this.CurrentMacroResolutionX = this.NUM_MACRO_PIXELS_HORIZONTAL;
		this.CurrentMacroResolutionY = this.NUM_MACRO_PIXELS_VERTICAL;
		this.CurrentTileScale = 3f;
		this.ScaleTileScale = Mathf.Max(1f, Mathf.Min(20f, (float)Screen.height * this.m_camera.rect.height / 270f));
		BraveCameraUtility.MaintainCameraAspect(this.m_camera);
		for (int i = 0; i < this.slavedCameras.Count; i++)
		{
			BraveCameraUtility.MaintainCameraAspect(this.slavedCameras[i]);
		}
		this.m_camera.orthographicSize = (float)this.NUM_MACRO_PIXELS_VERTICAL / 32f;
		if (!this.m_backgroundCamera)
		{
			this.m_backgroundCamera = BraveCameraUtility.GenerateBackgroundCamera(this.m_camera);
		}
		if (!this.IsInIntro)
		{
			GameUIRoot.Instance.UpdateScale();
		}
		if (GameManager.Options.CurrentPreferredFullscreenMode != GameOptions.PreferredFullscreenMode.BORDERLESS)
		{
			if (Screen.fullScreen && GameManager.Options.CurrentPreferredFullscreenMode != GameOptions.PreferredFullscreenMode.FULLSCREEN)
			{
				GameManager.Options.CurrentPreferredFullscreenMode = GameOptions.PreferredFullscreenMode.FULLSCREEN;
			}
			else if (!Screen.fullScreen && GameManager.Options.CurrentPreferredFullscreenMode == GameOptions.PreferredFullscreenMode.FULLSCREEN)
			{
				GameManager.Options.CurrentPreferredFullscreenMode = GameOptions.PreferredFullscreenMode.WINDOWED;
			}
		}
	}

	// Token: 0x17001247 RID: 4679
	// (get) Token: 0x06007BE3 RID: 31715 RVA: 0x0031AC0C File Offset: 0x00318E0C
	private int GBUFFER_DESCALE
	{
		get
		{
			switch (GameManager.Options.ShaderQuality)
			{
			case GameOptions.GenericHighMedLowOption.LOW:
				return 8;
			case GameOptions.GenericHighMedLowOption.MEDIUM:
				return 4;
			case GameOptions.GenericHighMedLowOption.HIGH:
				return 2;
			case GameOptions.GenericHighMedLowOption.VERY_LOW:
				return 8;
			default:
				return 8;
			}
		}
	}

	// Token: 0x06007BE4 RID: 31716 RVA: 0x0031AC48 File Offset: 0x00318E48
	private void RenderOptionalMaps()
	{
		if (this.UseTexturedOcclusion)
		{
			this.RenderOcclusionTexture(this.m_vignetteMaterial);
		}
		else if (this.m_texturedOcclusionTarget != null)
		{
			this.m_texturedOcclusionTarget.Release();
			this.m_texturedOcclusionTarget = null;
			this.m_vignetteMaterial.SetTexture("_TextureOcclusionTex", null);
		}
		if (GameManager.Options != null && GameManager.Options.RealtimeReflections)
		{
			this.RenderReflectionMap();
		}
		else if (this.m_reflectionTargetTexture != null)
		{
			this.m_reflectionTargetTexture.Release();
			this.m_reflectionTargetTexture = null;
		}
	}

	// Token: 0x06007BE5 RID: 31717 RVA: 0x0031ACEC File Offset: 0x00318EEC
	private void RenderOcclusionTexture(Material targetVignetteMaterial)
	{
		Pixelator.IsRenderingOcclusionTexture = true;
		if (this.m_texturedOcclusionTarget == null)
		{
			this.m_texturedOcclusionTarget = new RenderTexture(this.NUM_MACRO_PIXELS_HORIZONTAL, this.NUM_MACRO_PIXELS_VERTICAL, 0, RenderTextureFormat.Default);
			this.m_texturedOcclusionTarget.hideFlags = HideFlags.DontSave;
			this.m_texturedOcclusionTarget.filterMode = FilterMode.Point;
			targetVignetteMaterial.SetTexture("_TextureOcclusionTex", this.m_texturedOcclusionTarget);
		}
		Camera camera = this.slavedCameras[0];
		camera.CopyFrom(this.m_camera);
		camera.rect = new Rect(0f, 0f, 1f, 1f);
		camera.hdr = false;
		camera.targetTexture = this.m_texturedOcclusionTarget;
		camera.clearFlags = CameraClearFlags.Color;
		camera.backgroundColor = Color.clear;
		camera.cullingMask = this.cm_occlusionPartition;
		camera.Render();
		Pixelator.IsRenderingOcclusionTexture = false;
	}

	// Token: 0x06007BE6 RID: 31718 RVA: 0x0031ADC8 File Offset: 0x00318FC8
	public Vector2 GetCurrentSmoothCameraOffset()
	{
		Vector3 vector = ((!this.IsInIntro) ? CameraController.PLATFORM_CAMERA_OFFSET : Vector3.zero);
		Vector3 vector2 = this.m_camera.transform.position - vector;
		Vector3 vector3 = new Vector3(Mathf.Round(vector2.x * 16f), Mathf.Round(vector2.y * 16f), Mathf.Round(vector2.z * 16f)) / 16f;
		return (vector2 - vector3).XY();
	}

	// Token: 0x06007BE7 RID: 31719 RVA: 0x0031AE5C File Offset: 0x0031905C
	public IntVector2 GetCurrentMicropixelOffset()
	{
		Vector2 currentSmoothCameraOffset = this.GetCurrentSmoothCameraOffset();
		int num = Mathf.RoundToInt(currentSmoothCameraOffset.x / (0.0625f / this.ScaleTileScale));
		int num2 = Mathf.RoundToInt(currentSmoothCameraOffset.y / (0.0625f / this.ScaleTileScale));
		return new IntVector2(num, num2);
	}

	// Token: 0x06007BE8 RID: 31720 RVA: 0x0031AEAC File Offset: 0x003190AC
	private void RenderReflectionMap()
	{
		Pixelator.IsRenderingReflectionMap = true;
		Vector3 vector = ((!this.IsInIntro) ? CameraController.PLATFORM_CAMERA_OFFSET : Vector3.zero);
		Vector3 vector2 = this.m_camera.transform.position - vector;
		Vector3 vector3 = new Vector3(Mathf.Round(vector2.x * 16f) - 1f, Mathf.Round(vector2.y * 16f) - 1f, Mathf.Round(vector2.z * 16f)) / 16f;
		vector3 += vector;
		this.m_camera.transform.position = vector3;
		if (this.m_reflectionTargetTexture == null || this.m_reflectionTargetTexture.width != this.NUM_MACRO_PIXELS_HORIZONTAL || this.m_reflectionTargetTexture.height != this.NUM_MACRO_PIXELS_VERTICAL)
		{
			if (this.m_reflectionTargetTexture != null)
			{
				this.m_reflectionTargetTexture.Release();
			}
			this.m_reflectionTargetTexture = new RenderTexture(this.NUM_MACRO_PIXELS_HORIZONTAL, this.NUM_MACRO_PIXELS_VERTICAL, 0, RenderTextureFormat.Default);
			this.m_reflectionTargetTexture.hideFlags = HideFlags.DontSave;
			this.m_reflectionTargetTexture.filterMode = FilterMode.Bilinear;
			Shader.SetGlobalTexture(this.m_reflMapID, this.m_reflectionTargetTexture);
		}
		Shader.SetGlobalFloat(this.m_reflFlipID, 2f);
		Camera camera = this.slavedCameras[0];
		camera.CopyFrom(this.m_camera);
		camera.rect = new Rect(0f, 0f, 1f, 1f);
		camera.hdr = true;
		camera.targetTexture = this.m_reflectionTargetTexture;
		CameraClearFlags cameraClearFlags = CameraClearFlags.Color;
		camera.clearFlags = cameraClearFlags;
		camera.backgroundColor = Color.clear;
		camera.cullingMask = this.cm_refl;
		camera.Render();
		Shader.SetGlobalFloat(this.m_reflFlipID, 0f);
		this.m_camera.transform.position = vector2 + vector;
		Pixelator.IsRenderingReflectionMap = false;
	}

	// Token: 0x06007BE9 RID: 31721 RVA: 0x0031B0A8 File Offset: 0x003192A8
	public void SetLumaGain(float gain)
	{
		this.m_gammaEffect.ActiveMaterial.SetFloat("_Gain", gain);
	}

	// Token: 0x06007BEA RID: 31722 RVA: 0x0031B0C0 File Offset: 0x003192C0
	private void CalculateDepixelatedOffset(Vector3 cachedPosition, Vector3 quantizedPosition, int corePixelatedWidth, int corePixelatedHeight, RenderTexture referenceBufferA)
	{
		Vector2 vector = cachedPosition.XY() - quantizedPosition.XY();
		vector *= 16f;
		vector.x /= (float)referenceBufferA.width;
		vector.y /= (float)referenceBufferA.height;
		Vector4 vector2 = new Vector4(vector.x, vector.y, vector.x + (float)corePixelatedWidth / (float)referenceBufferA.width, vector.y + (float)corePixelatedHeight / (float)referenceBufferA.height);
		if (this.m_uvRangeID == -1)
		{
			this.m_uvRangeID = Shader.PropertyToID("_UVRange");
		}
		this.m_partialCopyMaterial.SetVector(this.m_uvRangeID, vector2);
		if (this.m_gbufferLightMaskCombinerMaterial != null)
		{
			this.m_gbufferLightMaskCombinerMaterial.SetVector(this.m_uvRangeID, vector2);
		}
	}

	// Token: 0x06007BEB RID: 31723 RVA: 0x0031B1A8 File Offset: 0x003193A8
	private void HandlePreDeathFramingLogic()
	{
		if (this.CacheCurrentFrameToBuffer && GameManager.Instance.AllPlayers != null)
		{
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				if (GameManager.Instance.AllPlayers[i])
				{
					GameManager.Instance.AllPlayers[i].ToggleHandRenderers(false, "death");
				}
			}
		}
	}

	// Token: 0x06007BEC RID: 31724 RVA: 0x0031B21C File Offset: 0x0031941C
	private void DoOcclusionUpdate(Material targetVignetteMaterial)
	{
		if (this.DoOcclusionLayer && !this.IsInIntro && !GameManager.Instance.IsSelectingCharacter)
		{
			Vector3 vector = this.m_camera.transform.position + new Vector3(BraveCameraUtility.ASPECT * -this.m_camera.orthographicSize, -this.m_camera.orthographicSize, 0f);
			Vector3 vector2 = vector + new Vector3((float)(this.CurrentMacroResolutionX / 16), (float)(this.CurrentMacroResolutionY / 16), 0f);
			IntVector2 intVector = vector.IntXY(VectorConversions.Round);
			if (this.generatedNewTexture && targetVignetteMaterial != null && this.occluder.SourceOcclusionTexture != null)
			{
				targetVignetteMaterial.SetTexture(this.m_occlusionMapID, this.occluder.SourceOcclusionTexture);
			}
			this.generatedNewTexture = false;
			if (this.localOcclusionTexture == null || this.occluder.cachedX != intVector.x - 2 || this.occluder.cachedY != intVector.y - 2 || this.m_occlusionDirty)
			{
				this.m_occlusionDirty = false;
				this.generatedNewTexture = true;
				this.occluder.GenerateOcclusionTexture(intVector.x - 2, intVector.y - 2, GameManager.Instance.Dungeon.data);
				this.localOcclusionTexture = this.occluder.SourceOcclusionTexture;
				if (targetVignetteMaterial != null && this.occluder.SourceOcclusionTexture != null)
				{
					targetVignetteMaterial.SetTexture(this.m_occlusionMapID, this.occluder.SourceOcclusionTexture);
				}
			}
			if (targetVignetteMaterial != null)
			{
				Vector2 vector3 = vector.XY() - intVector.ToVector2();
				Vector2 vector4 = vector2.XY() - (intVector + new IntVector2(this.CurrentMacroResolutionX / 16, this.CurrentMacroResolutionY / 16)).ToVector2();
				int num = this.CurrentMacroResolutionX / 16 + 4;
				int num2 = this.CurrentMacroResolutionY / 16 + 4;
				float num3 = 2f;
				float num4 = (num3 + vector3.x) / (float)num;
				float num5 = (num3 + vector3.y) / (float)num2;
				float num6 = 1f - (num3 - vector4.x) / (float)num;
				float num7 = 1f - (num3 - vector4.y) / (float)num2;
				Vector4 vector5 = new Vector4(num4, num5, num6, num7);
				if (targetVignetteMaterial != null)
				{
					targetVignetteMaterial.SetVector(this.m_occlusionUVID, vector5);
				}
			}
		}
		else
		{
			if (this.localOcclusionTexture == null || this.localOcclusionTexture.width > 1)
			{
				this.localOcclusionTexture = new Texture2D(1, 1);
				this.localOcclusionTexture.SetPixel(0, 0, new Color(0f, 1f, 1f, 1f));
				this.localOcclusionTexture.Apply();
			}
			if (targetVignetteMaterial != null && this.localOcclusionTexture != null)
			{
				targetVignetteMaterial.SetTexture(this.m_occlusionMapID, this.localOcclusionTexture);
			}
		}
	}

	// Token: 0x06007BED RID: 31725 RVA: 0x0031B554 File Offset: 0x00319754
	private bool ShouldOverrideMultiplexing()
	{
		return false;
	}

	// Token: 0x06007BEE RID: 31726 RVA: 0x0031B558 File Offset: 0x00319758
	private void OnRenderImage(RenderTexture source, RenderTexture target)
	{
		Dungeon dungeon = GameManager.Instance.Dungeon;
		if (dungeon == null || dungeon.data == null || dungeon.data.cellData == null)
		{
			bool flag = dungeon == null;
			bool flag2 = dungeon == null || dungeon.data == null;
			bool flag3 = dungeon == null || dungeon.data == null || dungeon.data.cellData == null;
			Debug.LogWarningFormat("No dungeon data found! {0} {1} {2}", new object[] { flag, flag2, flag3 });
			return;
		}
		if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.VERY_LOW && !this.IsInIntro && !this.IsInTitle && !this.IsInPunchout)
		{
			if (this.m_backupFadeMaterial == null && this.m_fadeMaterial != null)
			{
				this.m_backupFadeMaterial = this.m_fadeMaterial;
				this.m_fadeMaterial = this.m_combinedVignetteFadeMaterial;
			}
			if (this.m_combinedVignetteFadeMaterial)
			{
				if (GameManager.Options.LightingQuality == GameOptions.GenericHighMedLowOption.LOW && !this.m_combinedVignetteFadeMaterial.IsKeywordEnabled("LOWLIGHT_ON"))
				{
					this.m_combinedVignetteFadeMaterial.DisableKeyword("LOWLIGHT_OFF");
					this.m_combinedVignetteFadeMaterial.EnableKeyword("LOWLIGHT_ON");
				}
				else if (GameManager.Options.LightingQuality == GameOptions.GenericHighMedLowOption.HIGH && !this.m_combinedVignetteFadeMaterial.IsKeywordEnabled("LOWLIGHT_OFF"))
				{
					this.m_combinedVignetteFadeMaterial.DisableKeyword("LOWLIGHT_ON");
					this.m_combinedVignetteFadeMaterial.EnableKeyword("LOWLIGHT_OFF");
				}
			}
			if (this.m_gammaEffect && this.m_gammaEffect.ActiveMaterial)
			{
				if (this.m_gammaEffect.enabled)
				{
					this.m_combinedVignetteFadeMaterial.SetTexture(this.m_occlusionMapID, this.occluder.SourceOcclusionTexture);
					this.m_gammaEffect.enabled = false;
				}
				this.m_combinedVignetteFadeMaterial.SetFloat(this.m_gammaID, this.m_gammaEffect.ActiveMaterial.GetFloat(this.m_gammaID));
			}
			this.RenderGame_Combined(source, target, Pixelator.CoreRenderMode.LOW_QUALITY);
		}
		else
		{
			if (this.m_gammaEffect && !this.m_gammaEffect.enabled)
			{
				this.m_vignetteMaterial.SetTexture(this.m_occlusionMapID, this.occluder.SourceOcclusionTexture);
				this.m_gammaEffect.enabled = true;
			}
			GameOptions.PreferredScalingMode preferredScalingMode = GameManager.Options.CurrentPreferredScalingMode;
			if (this.IsInIntro && preferredScalingMode == GameOptions.PreferredScalingMode.UNIFORM_SCALING_FAST)
			{
				preferredScalingMode = GameOptions.PreferredScalingMode.UNIFORM_SCALING;
			}
			if ((preferredScalingMode == GameOptions.PreferredScalingMode.UNIFORM_SCALING_FAST || this.ShouldOverrideMultiplexing()) && !this.IsInPunchout)
			{
				if (this.m_backupFadeMaterial == null && this.m_fadeMaterial != null)
				{
					this.m_backupFadeMaterial = this.m_fadeMaterial;
					this.m_fadeMaterial = this.m_combinedVignetteFadeMaterial;
				}
				if (this.m_combinedVignetteFadeMaterial.IsKeywordEnabled("LOWLIGHT_ON"))
				{
					this.m_combinedVignetteFadeMaterial.DisableKeyword("LOWLIGHT_ON");
					this.m_combinedVignetteFadeMaterial.EnableKeyword("LOWLIGHT_OFF");
				}
				if (this.m_gammaEffect && this.m_gammaEffect.ActiveMaterial)
				{
					this.m_combinedVignetteFadeMaterial.SetTexture(this.m_occlusionMapID, this.occluder.SourceOcclusionTexture);
					if (!this.m_gammaEffect.enabled)
					{
						this.m_gammaEffect.enabled = true;
					}
					this.m_combinedVignetteFadeMaterial.SetFloat(this.m_gammaID, 1f);
				}
				this.RenderGame_Combined(source, target, Pixelator.CoreRenderMode.FAST_SCALING);
			}
			else
			{
				if (this.m_backupFadeMaterial != null)
				{
					this.m_fadeMaterial = this.m_backupFadeMaterial;
					this.m_backupFadeMaterial = null;
				}
				this.RenderGame_Pretty(source, target);
			}
		}
	}

	// Token: 0x06007BEF RID: 31727 RVA: 0x0031B94C File Offset: 0x00319B4C
	public RenderTexture GetCachedFrame_VeryLowSettings()
	{
		return this.m_cachedFrame_VeryLowSettings;
	}

	// Token: 0x06007BF0 RID: 31728 RVA: 0x0031B954 File Offset: 0x00319B54
	public void ClearCachedFrame_VeryLowSettings()
	{
		if (this.m_cachedFrame_VeryLowSettings != null)
		{
			RenderTexture.ReleaseTemporary(this.m_cachedFrame_VeryLowSettings);
		}
		this.m_cachedFrame_VeryLowSettings = null;
	}

	// Token: 0x06007BF1 RID: 31729 RVA: 0x0031B97C File Offset: 0x00319B7C
	private void RenderGame_Combined(RenderTexture source, RenderTexture target, Pixelator.CoreRenderMode renderMode)
	{
		this.HandlePreDeathFramingLogic();
		if (renderMode == Pixelator.CoreRenderMode.LOW_QUALITY)
		{
			if (this.m_bloomer && this.m_bloomer.enabled)
			{
				this.m_bloomer.enabled = false;
			}
		}
		else
		{
			this.RenderOptionalMaps();
			if ((this.m_bloomer && this.DoBloom && !this.m_bloomer.enabled) || (!this.DoBloom && this.m_bloomer.enabled))
			{
				this.m_bloomer.enabled = this.DoBloom;
			}
		}
		this.CheckSize();
		BraveCameraUtility.MaintainCameraAspect(this.m_camera);
		if (renderMode == Pixelator.CoreRenderMode.NORMAL)
		{
			this.DoOcclusionUpdate(this.m_vignetteMaterial);
		}
		else
		{
			this.DoOcclusionUpdate(this.m_combinedVignetteFadeMaterial);
		}
		int num = this.CurrentMacroResolutionX / this.overrideTileScale;
		int num2 = this.CurrentMacroResolutionY / this.overrideTileScale;
		Camera camera = this.slavedCameras[0];
		camera.CopyFrom(this.m_camera);
		camera.orthographicSize = this.m_camera.orthographicSize;
		camera.rect = new Rect(0f, 0f, 1f, 1f);
		camera.hdr = renderMode != Pixelator.CoreRenderMode.LOW_QUALITY && this.m_bloomer && this.m_bloomer.enabled;
		RenderTexture renderTexture = null;
		if (this.AdditionalPreBGCamera != null)
		{
			renderTexture = RenderTexture.GetTemporary(source.width, source.height, this.PLATFORM_DEPTH, this.PLATFORM_RENDER_FORMAT);
			renderTexture.filterMode = FilterMode.Point;
			this.AdditionalPreBGCamera.enabled = false;
			this.AdditionalPreBGCamera.clearFlags = CameraClearFlags.Color;
			this.AdditionalPreBGCamera.backgroundColor = Color.black;
			this.AdditionalPreBGCamera.targetTexture = renderTexture;
			this.AdditionalPreBGCamera.Render();
			Shader.SetGlobalTexture(this.m_preBackgroundTexID, renderTexture);
		}
		RenderTexture renderTexture2 = RenderTexture.GetTemporary(num, num2, this.PLATFORM_DEPTH, this.PLATFORM_RENDER_FORMAT);
		renderTexture2.filterMode = FilterMode.Point;
		camera.targetTexture = renderTexture2;
		CameraClearFlags cameraClearFlags = CameraClearFlags.Color;
		if (this.AdditionalBGCamera != null)
		{
			this.AdditionalBGCamera.enabled = false;
			this.AdditionalBGCamera.clearFlags = CameraClearFlags.Color;
			cameraClearFlags = CameraClearFlags.Nothing;
			this.AdditionalBGCamera.backgroundColor = Color.black;
			this.AdditionalBGCamera.targetTexture = renderTexture2;
			this.AdditionalBGCamera.Render();
		}
		camera.clearFlags = cameraClearFlags;
		camera.backgroundColor = Color.black;
		camera.cullingMask = this.cm_core1 | this.cm_core2;
		camera.Render();
		camera.clearFlags = CameraClearFlags.Depth;
		camera.backgroundColor = Color.clear;
		if (renderMode == Pixelator.CoreRenderMode.FAST_SCALING)
		{
			camera.cullingMask = this.cm_core3 | this.cm_core4 | (1 << LayerMask.NameToLayer("Unpixelated"));
		}
		else
		{
			camera.cullingMask = this.cm_core3 | this.cm_core4;
		}
		camera.Render();
		if (this.AdditionalCoreStackRenderPass != null)
		{
			if (TimeTubeCreditsController.IsTimeTubing || this.m_timetubedInstance)
			{
				this.m_timetubedInstance = true;
				Graphics.Blit(renderTexture2, renderTexture2, this.AdditionalCoreStackRenderPass);
			}
			else
			{
				RenderTexture temporary = RenderTexture.GetTemporary(renderTexture2.width, renderTexture2.height, renderTexture2.depth, renderTexture2.format);
				temporary.filterMode = FilterMode.Point;
				Graphics.Blit(renderTexture2, temporary, this.AdditionalCoreStackRenderPass);
				RenderTexture.ReleaseTemporary(renderTexture2);
				renderTexture2 = temporary;
			}
		}
		if (this.AdditionalPreBGCamera != null)
		{
			RenderTexture.ReleaseTemporary(renderTexture);
		}
		RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0, this.PLATFORM_RENDER_FORMAT);
		temporary2.filterMode = FilterMode.Point;
		Graphics.Blit(Pixelator.m_smallBlackTexture, temporary2);
		if (renderMode == Pixelator.CoreRenderMode.LOW_QUALITY)
		{
			this.RenderGBufferCheap(source, camera, renderTexture2.depthBuffer, temporary2, num, num2);
		}
		else if (renderMode == Pixelator.CoreRenderMode.FAST_SCALING)
		{
			this.RenderGBufferScaling(source, camera, renderTexture2.depthBuffer, temporary2);
		}
		if (this.AdditionalRenderPasses.Count > 0)
		{
			for (int i = 0; i < this.AdditionalRenderPasses.Count; i++)
			{
				if (this.AdditionalRenderPasses[i] == null)
				{
					this.AdditionalRenderPasses.RemoveAt(i);
					i--;
				}
				else
				{
					RenderTexture temporary3 = RenderTexture.GetTemporary(renderTexture2.width, renderTexture2.height, renderTexture2.depth, renderTexture2.format);
					Graphics.Blit(renderTexture2, temporary3, this.AdditionalRenderPasses[i], 0);
					if (this.AdditionalRenderPassesInitialized[i])
					{
						RenderTexture.ReleaseTemporary(renderTexture2);
						renderTexture2 = temporary3;
					}
					else
					{
						this.AdditionalRenderPassesInitialized[i] = true;
						RenderTexture.ReleaseTemporary(temporary3);
					}
				}
			}
		}
		else if (!this.m_hasInitializedAdditionalRenderTarget)
		{
			RenderTexture temporary4 = RenderTexture.GetTemporary(renderTexture2.width, renderTexture2.height, renderTexture2.depth, renderTexture2.format);
			Graphics.Blit(renderTexture2, temporary4);
			RenderTexture.ReleaseTemporary(renderTexture2);
			renderTexture2 = temporary4;
			this.m_hasInitializedAdditionalRenderTarget = true;
		}
		if (!this.m_gammaLocked)
		{
			this.m_gammaEffect.ActiveMaterial.SetFloat(this.m_gammaID, 2f - GameManager.Options.Gamma + this.m_gammaAdjustment);
		}
		if (this.m_combinedVignetteFadeMaterial != null)
		{
			this.m_combinedVignetteFadeMaterial.SetTexture(this.m_gBufferID, temporary2);
			this.m_combinedVignetteFadeMaterial.SetFloat(this.m_saturationID, this.saturation);
			this.m_combinedVignetteFadeMaterial.SetFloat(this.m_fadeID, this.fade);
		}
		if (this.DoFinalNonFadedLayer && this.m_combinedVignetteFadeMaterial != null)
		{
			Graphics.Blit(renderTexture2, target, this.m_combinedVignetteFadeMaterial);
			BraveCameraUtility.MaintainCameraAspect(camera);
			if (this.CompositePixelatedUnfadedLayer)
			{
				RenderTexture temporary5 = RenderTexture.GetTemporary(BraveCameraUtility.H_PIXELS, BraveCameraUtility.V_PIXELS);
				temporary5.filterMode = FilterMode.Point;
				camera.targetTexture = temporary5;
				camera.clearFlags = CameraClearFlags.Color;
				camera.backgroundColor = new Color(1f, 0f, 0f);
				camera.cullingMask = this.cm_unfaded;
				camera.Render();
				if (this.m_compositor == null)
				{
					this.m_compositor = new Material(ShaderCache.Acquire("Hidden/SimpleCompositor"));
				}
				this.m_compositor.SetTexture("_BaseTex", target);
				this.m_compositor.SetTexture("_LayerTex", temporary5);
				Graphics.Blit(temporary5, target, this.m_compositor);
				RenderTexture.ReleaseTemporary(temporary5);
			}
			else
			{
				camera.targetTexture = target;
				camera.clearFlags = CameraClearFlags.Depth;
				camera.cullingMask = this.cm_unfaded;
				camera.Render();
			}
		}
		else if (this.m_combinedVignetteFadeMaterial != null)
		{
			Graphics.Blit(renderTexture2, target, this.m_combinedVignetteFadeMaterial);
		}
		else
		{
			Graphics.Blit(renderTexture2, target);
		}
		BraveCameraUtility.MaintainCameraAspect(camera);
		camera.rect = new Rect(0f, 0f, 1f, 1f);
		camera.targetTexture = target;
		camera.clearFlags = CameraClearFlags.Depth;
		camera.cullingMask = this.cm_unoccluded;
		camera.Render();
		RenderTexture.ReleaseTemporary(temporary2);
		RenderTexture.ReleaseTemporary(renderTexture2);
		this.m_camera.cullingMask = 0;
		camera.cullingMask = 0;
		if (this.CacheCurrentFrameToBuffer)
		{
			this.ClearCachedFrame_VeryLowSettings();
			this.m_cachedFrame_VeryLowSettings = RenderTexture.GetTemporary(renderTexture2.width, renderTexture2.height, 0, renderTexture2.format);
			this.m_cachedFrame_VeryLowSettings.filterMode = FilterMode.Point;
			Graphics.Blit(renderTexture2, this.m_cachedFrame_VeryLowSettings);
			this.CacheCurrentFrameToBuffer = false;
		}
	}

	// Token: 0x06007BF2 RID: 31730 RVA: 0x0031C114 File Offset: 0x0031A314
	private void RenderGame_Pretty(RenderTexture source, RenderTexture target)
	{
		bool isCurrentlyZoomIntermediate = GameManager.Instance.MainCameraController.IsCurrentlyZoomIntermediate;
		this.HandlePreDeathFramingLogic();
		if (Pixelator.DebugGraphicsInfo)
		{
			Pixelator.DEBUG_LogSystemRenderingData();
		}
		this.RenderOptionalMaps();
		if ((this.m_bloomer && this.DoBloom && !this.m_bloomer.enabled) || (!this.DoBloom && this.m_bloomer.enabled))
		{
			this.m_bloomer.enabled = this.DoBloom;
		}
		this.CheckSize();
		BraveCameraUtility.MaintainCameraAspect(this.m_camera);
		this.DoOcclusionUpdate(this.m_vignetteMaterial);
		int num = this.CurrentMacroResolutionX / this.overrideTileScale;
		int num2 = this.CurrentMacroResolutionY / this.overrideTileScale;
		int num3 = num + this.extraPixels;
		int num4 = num2 + this.extraPixels;
		Vector3 vector = ((!this.IsInIntro) ? CameraController.PLATFORM_CAMERA_OFFSET : Vector3.zero);
		Vector3 vector2 = this.m_camera.transform.position - vector;
		Vector3 vector3 = new Vector3(Mathf.Round(vector2.x * 16f) - 1f, Mathf.Round(vector2.y * 16f) - 1f, Mathf.Round(vector2.z * 16f)) / 16f;
		vector3 += vector;
		this.m_camera.transform.position = vector3;
		Camera camera = this.slavedCameras[0];
		camera.CopyFrom(this.m_camera);
		camera.orthographicSize = this.m_camera.orthographicSize * ((float)num4 / ((float)this.CurrentMacroResolutionY / (float)this.overrideTileScale));
		camera.rect = new Rect(0f, 0f, 1f, 1f);
		camera.hdr = true;
		RenderTexture renderTexture = null;
		if (this.AdditionalPreBGCamera != null)
		{
			renderTexture = RenderTexture.GetTemporary(source.width, source.height, this.PLATFORM_DEPTH, this.PLATFORM_RENDER_FORMAT);
			renderTexture.filterMode = FilterMode.Point;
			this.AdditionalPreBGCamera.enabled = false;
			this.AdditionalPreBGCamera.clearFlags = CameraClearFlags.Color;
			this.AdditionalPreBGCamera.backgroundColor = Color.black;
			this.AdditionalPreBGCamera.targetTexture = renderTexture;
			this.AdditionalPreBGCamera.Render();
			Shader.SetGlobalTexture(this.m_preBackgroundTexID, renderTexture);
		}
		RenderTexture renderTexture2 = RenderTexture.GetTemporary(num3, num4, this.PLATFORM_DEPTH, this.PLATFORM_RENDER_FORMAT);
		renderTexture2.filterMode = FilterMode.Point;
		camera.targetTexture = renderTexture2;
		CameraClearFlags cameraClearFlags = CameraClearFlags.Color;
		if (this.AdditionalBGCamera != null)
		{
			this.AdditionalBGCamera.enabled = false;
			this.AdditionalBGCamera.clearFlags = CameraClearFlags.Color;
			cameraClearFlags = CameraClearFlags.Nothing;
			this.AdditionalBGCamera.backgroundColor = Color.black;
			this.AdditionalBGCamera.targetTexture = renderTexture2;
			this.AdditionalBGCamera.Render();
		}
		camera.clearFlags = cameraClearFlags;
		camera.backgroundColor = Color.black;
		camera.cullingMask = this.cm_core1 | this.cm_core2;
		camera.Render();
		camera.clearFlags = CameraClearFlags.Depth;
		camera.backgroundColor = Color.clear;
		camera.cullingMask = this.cm_core3 | this.cm_core4;
		camera.Render();
		camera.orthographicSize = this.m_camera.orthographicSize;
		this.m_camera.transform.position = vector2 + vector;
		RenderTexture renderTexture3 = null;
		if (this.AdditionalCoreStackRenderPass != null)
		{
			if (TimeTubeCreditsController.IsTimeTubing || this.m_timetubedInstance)
			{
				this.m_timetubedInstance = true;
				Graphics.Blit(renderTexture2, renderTexture2, this.AdditionalCoreStackRenderPass);
			}
			else
			{
				RenderTexture temporary = RenderTexture.GetTemporary(renderTexture2.width, renderTexture2.height, renderTexture2.depth, renderTexture2.format);
				temporary.filterMode = FilterMode.Point;
				Graphics.Blit(renderTexture2, temporary, this.AdditionalCoreStackRenderPass);
				renderTexture3 = renderTexture2;
				renderTexture2 = temporary;
			}
		}
		if (this.AdditionalPreBGCamera != null)
		{
			RenderTexture.ReleaseTemporary(renderTexture);
		}
		RenderTexture renderTexture4 = null;
		if (GameManager.Options.MotionEnhancementMode == GameOptions.PixelatorMotionEnhancementMode.ENHANCED_EXPENSIVE)
		{
			this.m_camera.transform.position = new Vector3(Mathf.Floor(vector2.x * (16f * this.ScaleTileScale)), Mathf.Floor(vector2.y * (16f * this.ScaleTileScale)), Mathf.Floor(vector2.z * (16f * this.ScaleTileScale))) / (16f * this.ScaleTileScale);
			this.m_camera.transform.position -= new Vector3(0.0625f, 0.0625f, 0f);
			camera.orthographicSize = this.m_camera.orthographicSize;
			renderTexture4 = RenderTexture.GetTemporary(source.width, source.height, this.PLATFORM_DEPTH, RenderTextureFormat.Depth);
			Graphics.Blit(Pixelator.m_smallBlackTexture, renderTexture4);
			camera.targetTexture = renderTexture4;
			camera.clearFlags = CameraClearFlags.Depth;
			camera.cullingMask = this.cm_fg_important;
			if (!this.PRECLUDE_DEPTH_RENDERING)
			{
				camera.Render();
			}
			camera.orthographicSize = this.m_camera.orthographicSize * ((float)num4 / ((float)this.CurrentMacroResolutionY / (float)this.overrideTileScale));
			this.m_camera.transform.position += new Vector3(0.0625f, 0.0625f, 0f);
			this.m_camera.transform.position = vector2 + vector;
		}
		this.CalculateDepixelatedOffset(vector2, vector3, num, num2, renderTexture2);
		RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0, this.PLATFORM_RENDER_FORMAT);
		temporary2.filterMode = FilterMode.Point;
		Graphics.Blit(Pixelator.m_smallBlackTexture, temporary2);
		this.m_camera.transform.position = vector3;
		camera.orthographicSize = this.m_camera.orthographicSize * ((float)num4 / ((float)this.CurrentMacroResolutionY / (float)this.overrideTileScale));
		this.RenderGBuffer(source, camera, (!renderTexture3) ? renderTexture2.depthBuffer : renderTexture3.depthBuffer, temporary2, vector2, vector3);
		camera.orthographicSize = this.m_camera.orthographicSize;
		this.m_camera.transform.position = vector2 + vector;
		if (renderTexture3 != null)
		{
			RenderTexture.ReleaseTemporary(renderTexture3);
		}
		RenderTexture temporary3 = RenderTexture.GetTemporary(source.width, source.height, this.PLATFORM_DEPTH, this.PLATFORM_RENDER_FORMAT);
		int num5 = Mathf.Max(Mathf.CeilToInt((float)source.width / (float)this.CurrentMacroResolutionX), Mathf.CeilToInt((float)source.height / (float)this.CurrentMacroResolutionY));
		if (this.CurrentMacroResolutionX * num5 == source.width && this.CurrentMacroResolutionY * num5 == source.height)
		{
			Graphics.Blit(renderTexture2, temporary3, this.m_partialCopyMaterial);
		}
		else
		{
			RenderTexture temporary4 = RenderTexture.GetTemporary(this.CurrentMacroResolutionX * num5, this.CurrentMacroResolutionY * num5, this.PLATFORM_DEPTH, this.PLATFORM_RENDER_FORMAT);
			Graphics.Blit(renderTexture2, temporary4);
			temporary4.filterMode = this.DownsamplingFilterMode;
			Graphics.Blit(temporary4, temporary3, this.m_partialCopyMaterial);
			RenderTexture.ReleaseTemporary(temporary4);
		}
		if (GameManager.Options.MotionEnhancementMode == GameOptions.PixelatorMotionEnhancementMode.ENHANCED_EXPENSIVE)
		{
			this.m_camera.transform.position = new Vector3(Mathf.Floor(vector2.x * (16f * this.ScaleTileScale)), Mathf.Floor(vector2.y * (16f * this.ScaleTileScale)), Mathf.Floor(vector2.z * (16f * this.ScaleTileScale))) / (16f * this.ScaleTileScale);
			this.m_camera.transform.position -= new Vector3(0.0625f, 0.0625f, 0f);
			camera.orthographicSize = this.m_camera.orthographicSize;
			camera.targetTexture = temporary3;
			camera.SetTargetBuffers(temporary3.colorBuffer, renderTexture4.depthBuffer);
			camera.clearFlags = CameraClearFlags.Nothing;
			camera.cullingMask = this.cm_unpixelated;
			camera.Render();
			camera.orthographicSize = this.m_camera.orthographicSize * ((float)num4 / ((float)this.CurrentMacroResolutionY / (float)this.overrideTileScale));
			this.m_camera.transform.position += new Vector3(0.0625f, 0.0625f, 0f);
			this.m_camera.transform.position = vector2 + vector;
		}
		RenderTexture renderTexture5 = RenderTexture.GetTemporary(source.width, source.height, this.PLATFORM_DEPTH, this.PLATFORM_RENDER_FORMAT);
		if (!this.m_gammaLocked)
		{
			this.m_gammaEffect.ActiveMaterial.SetFloat(this.m_gammaID, 2f - GameManager.Options.Gamma + this.m_gammaAdjustment);
		}
		if (this.m_vignetteMaterial != null)
		{
			this.m_vignetteMaterial.SetTexture(this.m_gBufferID, temporary2);
			Graphics.Blit(temporary3, renderTexture5, this.m_vignetteMaterial);
		}
		else
		{
			Graphics.Blit(temporary3, renderTexture5);
		}
		this.m_camera.transform.position = new Vector3(Mathf.Floor(vector2.x * (16f * this.ScaleTileScale)), Mathf.Floor(vector2.y * (16f * this.ScaleTileScale)), Mathf.Floor(vector2.z * (16f * this.ScaleTileScale))) / (16f * this.ScaleTileScale);
		this.m_camera.transform.position -= new Vector3(0.0625f, 0.0625f, 0f);
		camera.orthographicSize = this.m_camera.orthographicSize;
		camera.targetTexture = renderTexture5;
		camera.clearFlags = CameraClearFlags.Depth;
		camera.cullingMask = this.cm_unoccluded;
		camera.Render();
		camera.orthographicSize = this.m_camera.orthographicSize * ((float)num4 / ((float)this.CurrentMacroResolutionY / (float)this.overrideTileScale));
		this.m_camera.transform.position += new Vector3(0.0625f, 0.0625f, 0f);
		this.m_camera.transform.position = vector2 + vector;
		if (this.AdditionalRenderPasses.Count > 0)
		{
			for (int i = 0; i < this.AdditionalRenderPasses.Count; i++)
			{
				if (this.AdditionalRenderPasses[i] == null)
				{
					this.AdditionalRenderPasses.RemoveAt(i);
					i--;
				}
				else
				{
					RenderTexture temporary5 = RenderTexture.GetTemporary(renderTexture5.width, renderTexture5.height, renderTexture5.depth, renderTexture5.format);
					Graphics.Blit(renderTexture5, temporary5, this.AdditionalRenderPasses[i], 0);
					if (this.AdditionalRenderPassesInitialized[i])
					{
						RenderTexture.ReleaseTemporary(renderTexture5);
						renderTexture5 = temporary5;
					}
					else
					{
						this.AdditionalRenderPassesInitialized[i] = true;
						RenderTexture.ReleaseTemporary(temporary5);
					}
				}
			}
		}
		else if (!this.m_hasInitializedAdditionalRenderTarget)
		{
			RenderTexture temporary6 = RenderTexture.GetTemporary(renderTexture5.width, renderTexture5.height, renderTexture5.depth, renderTexture5.format);
			Graphics.Blit(renderTexture5, temporary6);
			RenderTexture.ReleaseTemporary(renderTexture5);
			renderTexture5 = temporary6;
			this.m_hasInitializedAdditionalRenderTarget = true;
		}
		if (this.m_fadeMaterial != null)
		{
			this.m_fadeMaterial.SetFloat(this.m_saturationID, this.saturation);
			this.m_fadeMaterial.SetFloat(this.m_fadeID, this.fade);
		}
		if (this.DoFinalNonFadedLayer && this.m_fadeMaterial != null)
		{
			if (this.CompositePixelatedUnfadedLayer && SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11)
			{
				RenderTexture temporary7 = RenderTexture.GetTemporary(source.width, source.height);
				Graphics.Blit(renderTexture5, temporary7, this.m_fadeMaterial);
				Graphics.Blit(temporary7, target);
				this.m_camera.transform.position -= new Vector3(0.0625f, 0.0625f, 0f);
				camera.orthographicSize = this.m_camera.orthographicSize;
				RenderTexture temporary8 = RenderTexture.GetTemporary(BraveCameraUtility.H_PIXELS, BraveCameraUtility.V_PIXELS);
				temporary8.filterMode = FilterMode.Point;
				camera.targetTexture = temporary8;
				camera.clearFlags = CameraClearFlags.Color;
				camera.backgroundColor = new Color(1f, 0f, 0f);
				camera.cullingMask = this.cm_unfaded;
				camera.Render();
				if (this.m_compositor == null)
				{
					this.m_compositor = new Material(ShaderCache.Acquire("Hidden/SimpleCompositor"));
				}
				this.m_compositor.SetTexture("_BaseTex", temporary7);
				this.m_compositor.SetTexture("_LayerTex", temporary8);
				Graphics.Blit(temporary8, target, this.m_compositor);
				RenderTexture.ReleaseTemporary(temporary8);
				RenderTexture.ReleaseTemporary(temporary7);
				camera.orthographicSize = this.m_camera.orthographicSize * ((float)num4 / ((float)this.CurrentMacroResolutionY / (float)this.overrideTileScale));
				this.m_camera.transform.position += new Vector3(0.0625f, 0.0625f, 0f);
			}
			else
			{
				RenderTexture temporary9 = RenderTexture.GetTemporary(source.width, source.height);
				Graphics.Blit(renderTexture5, temporary9, this.m_fadeMaterial);
				Graphics.Blit(temporary9, target);
				if (this.CompositePixelatedUnfadedLayer)
				{
					this.m_camera.transform.position -= new Vector3(0.0625f, 0.0625f, 0f);
					camera.orthographicSize = this.m_camera.orthographicSize;
					RenderTexture temporary10 = RenderTexture.GetTemporary(BraveCameraUtility.H_PIXELS, BraveCameraUtility.V_PIXELS);
					temporary10.filterMode = FilterMode.Point;
					camera.targetTexture = temporary10;
					camera.clearFlags = CameraClearFlags.Color;
					camera.backgroundColor = new Color(1f, 0f, 0f);
					camera.cullingMask = this.cm_unfaded;
					camera.Render();
					if (this.m_compositor == null)
					{
						this.m_compositor = new Material(ShaderCache.Acquire("Hidden/SimpleCompositor"));
					}
					this.m_compositor.SetTexture("_BaseTex", temporary9);
					this.m_compositor.SetTexture("_LayerTex", temporary10);
					Graphics.Blit(temporary10, target, this.m_compositor);
					RenderTexture.ReleaseTemporary(temporary10);
					camera.orthographicSize = this.m_camera.orthographicSize * ((float)num4 / ((float)this.CurrentMacroResolutionY / (float)this.overrideTileScale));
					this.m_camera.transform.position += new Vector3(0.0625f, 0.0625f, 0f);
				}
				else
				{
					this.m_camera.transform.position = new Vector3(Mathf.Floor(vector2.x * (16f * this.ScaleTileScale)), Mathf.Floor(vector2.y * (16f * this.ScaleTileScale)), Mathf.Floor(vector2.z * (16f * this.ScaleTileScale))) / (16f * this.ScaleTileScale);
					this.m_camera.transform.position -= new Vector3(0.0625f, 0.0625f, 0f);
					camera.orthographicSize = this.m_camera.orthographicSize;
					camera.targetTexture = target;
					camera.clearFlags = CameraClearFlags.Depth;
					camera.cullingMask = this.cm_unfaded;
					camera.Render();
					camera.orthographicSize = this.m_camera.orthographicSize * ((float)num4 / ((float)this.CurrentMacroResolutionY / (float)this.overrideTileScale));
					this.m_camera.transform.position += new Vector3(0.0625f, 0.0625f, 0f);
					this.m_camera.transform.position = vector2 + vector;
				}
				RenderTexture.ReleaseTemporary(temporary9);
			}
		}
		else if (this.m_fadeMaterial != null)
		{
			Graphics.Blit(renderTexture5, target, this.m_fadeMaterial);
		}
		else
		{
			Graphics.Blit(renderTexture5, target);
		}
		RenderTexture.ReleaseTemporary(temporary2);
		if (GameManager.Options.MotionEnhancementMode == GameOptions.PixelatorMotionEnhancementMode.ENHANCED_EXPENSIVE)
		{
			RenderTexture.ReleaseTemporary(renderTexture4);
		}
		if (isCurrentlyZoomIntermediate)
		{
			renderTexture2.Release();
		}
		else
		{
			RenderTexture.ReleaseTemporary(renderTexture2);
		}
		RenderTexture.ReleaseTemporary(temporary3);
		RenderTexture.ReleaseTemporary(renderTexture5);
		this.m_camera.cullingMask = 0;
		camera.cullingMask = 0;
	}

	// Token: 0x06007BF3 RID: 31731 RVA: 0x0031D22C File Offset: 0x0031B42C
	private void RenderEnemyProjectileMasks(Camera stackCamera, RenderTexture source)
	{
		int num = ((GameManager.Options.MotionEnhancementMode != GameOptions.PixelatorMotionEnhancementMode.ENHANCED_EXPENSIVE) ? (this.CurrentMacroResolutionX / this.overrideTileScale) : source.width);
		int num2 = ((GameManager.Options.MotionEnhancementMode != GameOptions.PixelatorMotionEnhancementMode.ENHANCED_EXPENSIVE) ? (this.CurrentMacroResolutionY / this.overrideTileScale) : source.height);
		this.m_UnblurredProjectileMaskTex = RenderTexture.GetTemporary(num, num2, this.PLATFORM_DEPTH, RenderTextureFormat.Default);
		this.m_UnblurredProjectileMaskTex.filterMode = FilterMode.Point;
		this.m_BlurredProjectileMaskTex = RenderTexture.GetTemporary(this.m_UnblurredProjectileMaskTex.width, this.m_UnblurredProjectileMaskTex.height, this.PLATFORM_DEPTH, RenderTextureFormat.Default);
		this.m_BlurredProjectileMaskTex.filterMode = FilterMode.Point;
		if (this.m_blurMaterial == null)
		{
			this.m_blurMaterial = new Material(base.GetComponent<SENaturalBloomAndDirtyLens>().shader);
			this.m_blurMaterial.hideFlags = HideFlags.HideAndDontSave;
		}
		int num3 = LayerMask.NameToLayer("PlayerAndProjectiles");
		ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
		for (int i = 0; i < allProjectiles.Count; i++)
		{
			if (!allProjectiles[i].neverMaskThis)
			{
				allProjectiles[i].CacheLayer(num3);
			}
		}
		stackCamera.clearFlags = CameraClearFlags.Color;
		stackCamera.cullingMask = 1 << num3;
		stackCamera.SetReplacementShader(this.m_simpleSpriteMaskShader, string.Empty);
		stackCamera.targetTexture = this.m_UnblurredProjectileMaskTex;
		stackCamera.Render();
		stackCamera.ResetReplacementShader();
		stackCamera.clearFlags = CameraClearFlags.Nothing;
		stackCamera.cullingMask = this.cm_fg & ~(1 << num3);
		stackCamera.SetReplacementShader(ShaderCache.Acquire("Brave/Internal/Black"), string.Empty);
		stackCamera.Render();
		stackCamera.ResetReplacementShader();
		for (int j = 0; j < 3; j++)
		{
			this.m_blurMaterial.SetFloat("_BlurSize", this.ProjectileMaskBlurSize * 0.5f + (float)j);
			RenderTexture renderTexture = RenderTexture.GetTemporary(this.m_UnblurredProjectileMaskTex.width, this.m_UnblurredProjectileMaskTex.height, 0, RenderTextureFormat.Default);
			renderTexture.filterMode = FilterMode.Point;
			Graphics.Blit((j != 0) ? this.m_BlurredProjectileMaskTex : this.m_UnblurredProjectileMaskTex, renderTexture, this.m_blurMaterial, 2);
			RenderTexture.ReleaseTemporary(this.m_BlurredProjectileMaskTex);
			this.m_BlurredProjectileMaskTex = renderTexture;
			renderTexture = RenderTexture.GetTemporary(this.m_UnblurredProjectileMaskTex.width, this.m_UnblurredProjectileMaskTex.height, 0, RenderTextureFormat.Default);
			renderTexture.filterMode = FilterMode.Point;
			Graphics.Blit(this.m_BlurredProjectileMaskTex, renderTexture, this.m_blurMaterial, 3);
			RenderTexture.ReleaseTemporary(this.m_BlurredProjectileMaskTex);
			this.m_BlurredProjectileMaskTex = renderTexture;
		}
		for (int k = 0; k < allProjectiles.Count; k++)
		{
			if (!allProjectiles[k].neverMaskThis)
			{
				allProjectiles[k].DecacheLayer();
			}
		}
	}

	// Token: 0x17001248 RID: 4680
	// (get) Token: 0x06007BF4 RID: 31732 RVA: 0x0031D500 File Offset: 0x0031B700
	protected float LightCullFactor
	{
		get
		{
			if (InfiniteMinecartZone.InInfiniteMinecartZone)
			{
				return 2f;
			}
			if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH)
			{
				return 1.5f;
			}
			return 1.25f;
		}
	}

	// Token: 0x17001249 RID: 4681
	// (get) Token: 0x06007BF5 RID: 31733 RVA: 0x0031D530 File Offset: 0x0031B730
	private bool ActuallyRenderGBuffer
	{
		get
		{
			return this.DoRenderGBuffer;
		}
	}

	// Token: 0x06007BF6 RID: 31734 RVA: 0x0031D538 File Offset: 0x0031B738
	private void RenderGBufferCheap(RenderTexture source, Camera stackCamera, RenderBuffer depthTarget, RenderTexture TempBuffer_Lighting, int coreBufferWidth, int coreBufferHeight)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(this.CurrentMacroResolutionX / (this.GBUFFER_DESCALE * this.overrideTileScale), this.CurrentMacroResolutionY / (this.GBUFFER_DESCALE * this.overrideTileScale), 0, this.PLATFORM_RENDER_FORMAT);
		Graphics.Blit(Pixelator.m_smallBlackTexture, temporary);
		Graphics.Blit(temporary, temporary, this.m_pointLightMaterial, 1);
		RenderTexture renderTexture = null;
		if (this.IsInIntro || GameManager.Options.LightingQuality != GameOptions.GenericHighMedLowOption.LOW)
		{
			renderTexture = RenderTexture.GetTemporary(coreBufferWidth, coreBufferHeight, this.PLATFORM_DEPTH, RenderTextureFormat.Default);
			renderTexture.filterMode = FilterMode.Point;
			Graphics.Blit(Pixelator.m_smallBlackTexture, renderTexture);
			int num = ((GameManager.Options.MotionEnhancementMode != GameOptions.PixelatorMotionEnhancementMode.ENHANCED_EXPENSIVE) ? this.cm_gbuffer : this.cm_gbufferSimple);
			stackCamera.clearFlags = CameraClearFlags.Nothing;
			stackCamera.cullingMask = num;
			stackCamera.targetTexture = renderTexture;
			stackCamera.SetTargetBuffers(renderTexture.colorBuffer, depthTarget);
			stackCamera.SetReplacementShader(this.m_simpleSpriteMaskShader, "UnlitTilted");
			stackCamera.Render();
			stackCamera.ResetReplacementShader();
		}
		for (int i = 0; i < this.AdditionalBraveLights.Count; i++)
		{
			if (this.AdditionalBraveLights[i] && this.AdditionalBraveLights[i].gameObject.activeSelf)
			{
				if (this.AdditionalBraveLights[i].UsesCustomMaterial)
				{
					AdditionalBraveLight additionalBraveLight = this.AdditionalBraveLights[i];
					float lightRadius = additionalBraveLight.LightRadius;
					float lightIntensity = additionalBraveLight.LightIntensity;
					if (lightIntensity != 0f)
					{
						Vector2 vector;
						if (additionalBraveLight.sprite)
						{
							vector = additionalBraveLight.sprite.WorldCenter;
						}
						else
						{
							vector = additionalBraveLight.transform.position;
						}
						Vector2 vector2 = stackCamera.transform.position.XY();
						if (!this.LightCulled(vector, vector2, lightRadius, stackCamera.orthographicSize, BraveCameraUtility.ASPECT))
						{
							Material customLightMaterial = additionalBraveLight.CustomLightMaterial;
							customLightMaterial.SetVector(this.m_cameraWSID, vector2.ToVector4());
							customLightMaterial.SetFloat(this.m_cameraOrthoSizeID, stackCamera.orthographicSize);
							customLightMaterial.SetFloat(this.m_cameraOrthoSizeXID, stackCamera.orthographicSize * stackCamera.aspect);
							customLightMaterial.SetVector(this.m_lightPosID, vector.ToVector4());
							customLightMaterial.SetColor(this.m_lightColorID, additionalBraveLight.LightColor);
							customLightMaterial.SetFloat(this.m_lightRadiusID, lightRadius);
							customLightMaterial.SetFloat(this.m_lightIntensityID, lightIntensity);
							Graphics.Blit(temporary, temporary, customLightMaterial, 0);
						}
					}
				}
			}
		}
		bool flag = false;
		if (flag && GameManager.Instance.Dungeon.PlayerIsLight && !GameManager.Instance.IsLoadingLevel && GameManager.Instance.PrimaryPlayer)
		{
			Vector2 vector3 = stackCamera.transform.position.XY();
			this.m_pointLightMaterialFast.SetVector(this.m_cameraWSID, vector3.ToVector4());
			this.m_pointLightMaterialFast.SetFloat(this.m_cameraOrthoSizeID, stackCamera.orthographicSize);
			this.m_pointLightMaterialFast.SetFloat(this.m_cameraOrthoSizeXID, stackCamera.orthographicSize * stackCamera.aspect);
			float playerLightRadius = GameManager.Instance.Dungeon.PlayerLightRadius;
			float num2 = GameManager.Instance.Dungeon.PlayerLightIntensity / 5f;
			Vector2 centerPosition = GameManager.Instance.PrimaryPlayer.CenterPosition;
			if (!this.LightCulled(centerPosition, vector3, playerLightRadius, stackCamera.orthographicSize, BraveCameraUtility.ASPECT))
			{
				this.m_pointLightMaterialFast.SetVector(this.m_lightPosID, centerPosition.ToVector4());
				this.m_pointLightMaterialFast.SetColor(this.m_lightColorID, GameManager.Instance.Dungeon.PlayerLightColor);
				this.m_pointLightMaterialFast.SetFloat(this.m_lightRadiusID, playerLightRadius);
				this.m_pointLightMaterialFast.SetFloat(this.m_lightIntensityID, num2);
				Graphics.Blit(temporary, temporary, this.m_pointLightMaterialFast, 0);
			}
		}
		if (renderTexture == null)
		{
			Graphics.Blit(temporary, TempBuffer_Lighting);
		}
		else
		{
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, this.PLATFORM_DEPTH, this.PLATFORM_RENDER_FORMAT);
			Graphics.Blit(renderTexture, temporary2);
			this.m_gbufferMaskMaterial.SetTexture(this.m_lightMaskTexID, temporary2);
			Graphics.Blit(temporary, TempBuffer_Lighting, this.m_gbufferMaskMaterial);
			RenderTexture.ReleaseTemporary(temporary2);
			RenderTexture.ReleaseTemporary(renderTexture);
		}
		RenderTexture.ReleaseTemporary(temporary);
	}

	// Token: 0x06007BF7 RID: 31735 RVA: 0x0031D9A4 File Offset: 0x0031BBA4
	private void RenderGBufferScaling(RenderTexture source, Camera stackCamera, RenderBuffer depthTarget, RenderTexture TempBuffer_Lighting)
	{
		int num = this.CurrentMacroResolutionX / this.overrideTileScale;
		int num2 = this.CurrentMacroResolutionY / this.overrideTileScale;
		if (this.ActuallyRenderGBuffer)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(this.CurrentMacroResolutionX / (this.GBUFFER_DESCALE * this.overrideTileScale), this.CurrentMacroResolutionY / (this.GBUFFER_DESCALE * this.overrideTileScale), 0, this.PLATFORM_RENDER_FORMAT);
			Graphics.Blit(Pixelator.m_smallBlackTexture, temporary);
			Graphics.Blit(temporary, temporary, this.m_pointLightMaterial, 1);
			RenderTexture renderTexture = null;
			RenderTexture renderTexture2 = null;
			if (this.IsInIntro || GameManager.Options.LightingQuality != GameOptions.GenericHighMedLowOption.LOW)
			{
				renderTexture = RenderTexture.GetTemporary(num, num2, this.PLATFORM_DEPTH, RenderTextureFormat.Default);
				renderTexture.filterMode = FilterMode.Point;
				Graphics.Blit(Pixelator.m_smallBlackTexture, renderTexture);
				int num3 = ((GameManager.Options.MotionEnhancementMode != GameOptions.PixelatorMotionEnhancementMode.ENHANCED_EXPENSIVE) ? this.cm_gbuffer : this.cm_gbufferSimple);
				stackCamera.clearFlags = CameraClearFlags.Nothing;
				stackCamera.cullingMask = num3;
				stackCamera.targetTexture = renderTexture;
				stackCamera.SetTargetBuffers(renderTexture.colorBuffer, depthTarget);
				stackCamera.SetReplacementShader(this.m_simpleSpriteMaskShader, "UnlitTilted");
				stackCamera.Render();
				stackCamera.ResetReplacementShader();
				Vector2 vector = stackCamera.transform.position.XY();
				this.m_pointLightMaterial.SetVector(this.m_cameraWSID, vector.ToVector4());
				this.m_pointLightMaterial.SetFloat(this.m_cameraOrthoSizeID, stackCamera.orthographicSize);
				this.m_pointLightMaterial.SetFloat(this.m_cameraOrthoSizeXID, stackCamera.orthographicSize * stackCamera.aspect);
				this.m_pointLightMaterialFast.SetVector(this.m_cameraWSID, vector.ToVector4());
				this.m_pointLightMaterialFast.SetFloat(this.m_cameraOrthoSizeID, stackCamera.orthographicSize);
				this.m_pointLightMaterialFast.SetFloat(this.m_cameraOrthoSizeXID, stackCamera.orthographicSize * stackCamera.aspect);
				for (int i = 0; i < ShadowSystem.AllLights.Count; i++)
				{
					ShadowSystem shadowSystem = ShadowSystem.AllLights[i];
					if (shadowSystem && shadowSystem.gameObject.activeSelf)
					{
						bool flag = shadowSystem.uLightCookie == null;
						Material material = ((!flag) ? this.m_pointLightMaterial : this.m_pointLightMaterialFast);
						Vector2 vector2 = shadowSystem.transform.position.XY();
						if (!this.LightCulled(vector2, vector, shadowSystem.uLightRange, stackCamera.orthographicSize, BraveCameraUtility.ASPECT))
						{
							material.SetVector(this.m_lightPosID, vector2.ToVector4());
							material.SetColor(this.m_lightColorID, shadowSystem.uLightColor);
							material.SetFloat(this.m_lightRadiusID, shadowSystem.uLightRange);
							material.SetFloat(this.m_lightIntensityID, shadowSystem.uLightIntensity * this.pointLightMultiplier);
							if (!flag)
							{
								material.SetTexture(this.m_lightCookieID, shadowSystem.uLightCookie);
								material.SetFloat(this.m_lightCookieAngleID, shadowSystem.uLightCookieAngle);
							}
							Graphics.Blit(temporary, temporary, material, 0);
						}
					}
				}
				for (int j = 0; j < this.AdditionalBraveLights.Count; j++)
				{
					if (this.AdditionalBraveLights[j] && this.AdditionalBraveLights[j].gameObject.activeSelf)
					{
						AdditionalBraveLight additionalBraveLight = this.AdditionalBraveLights[j];
						float lightRadius = additionalBraveLight.LightRadius;
						float lightIntensity = additionalBraveLight.LightIntensity;
						if (lightIntensity != 0f)
						{
							Vector2 vector3;
							if (additionalBraveLight.sprite)
							{
								vector3 = additionalBraveLight.sprite.WorldCenter;
							}
							else
							{
								vector3 = additionalBraveLight.transform.position;
							}
							if (!this.LightCulled(vector3, vector, lightRadius, stackCamera.orthographicSize, BraveCameraUtility.ASPECT))
							{
								Material material2 = this.m_pointLightMaterialFast;
								if (additionalBraveLight.UsesCustomMaterial)
								{
									material2 = additionalBraveLight.CustomLightMaterial;
									material2.SetVector(this.m_cameraWSID, vector.ToVector4());
									material2.SetFloat(this.m_cameraOrthoSizeID, stackCamera.orthographicSize);
									material2.SetFloat(this.m_cameraOrthoSizeXID, stackCamera.orthographicSize * stackCamera.aspect);
								}
								else if (additionalBraveLight.UsesCone)
								{
									material2 = this.m_pointLightMaterial;
									material2.SetFloat("_LightAngle", additionalBraveLight.LightAngle);
									material2.SetFloat("_LightOrient", additionalBraveLight.LightOrient);
								}
								material2.SetVector(this.m_lightPosID, vector3.ToVector4());
								material2.SetColor(this.m_lightColorID, additionalBraveLight.LightColor);
								material2.SetFloat(this.m_lightRadiusID, lightRadius);
								material2.SetFloat(this.m_lightIntensityID, lightIntensity);
								Graphics.Blit(temporary, temporary, material2, 0);
							}
						}
					}
				}
				if (GameManager.Instance.Dungeon.PlayerIsLight && !GameManager.Instance.IsLoadingLevel && GameManager.Instance.PrimaryPlayer)
				{
					float playerLightRadius = GameManager.Instance.Dungeon.PlayerLightRadius;
					float playerLightIntensity = GameManager.Instance.Dungeon.PlayerLightIntensity;
					Vector2 centerPosition = GameManager.Instance.PrimaryPlayer.CenterPosition;
					if (!this.LightCulled(centerPosition, vector, playerLightRadius, stackCamera.orthographicSize, BraveCameraUtility.ASPECT))
					{
						this.m_pointLightMaterialFast.SetVector(this.m_lightPosID, centerPosition.ToVector4());
						this.m_pointLightMaterialFast.SetColor(this.m_lightColorID, GameManager.Instance.Dungeon.PlayerLightColor);
						this.m_pointLightMaterialFast.SetFloat(this.m_lightRadiusID, playerLightRadius);
						this.m_pointLightMaterialFast.SetFloat(this.m_lightIntensityID, playerLightIntensity);
						Graphics.Blit(temporary, temporary, this.m_pointLightMaterialFast, 0);
					}
				}
			}
			if (renderTexture == null)
			{
				Graphics.Blit(temporary, TempBuffer_Lighting);
			}
			else
			{
				RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, this.PLATFORM_DEPTH, this.PLATFORM_RENDER_FORMAT);
				Graphics.Blit(renderTexture, temporary2);
				this.m_gbufferMaskMaterial.SetTexture(this.m_lightMaskTexID, renderTexture);
				Graphics.Blit(temporary, TempBuffer_Lighting, this.m_gbufferMaskMaterial);
				RenderTexture.ReleaseTemporary(temporary2);
				RenderTexture.ReleaseTemporary(renderTexture);
				if (renderTexture2 != null)
				{
					RenderTexture.ReleaseTemporary(renderTexture2);
				}
			}
			RenderTexture.ReleaseTemporary(temporary);
		}
		else
		{
			Graphics.Blit(Pixelator.m_smallBlackTexture, TempBuffer_Lighting);
			RenderSettings.ambientMode = AmbientMode.Flat;
			RenderSettings.ambientLight = Color.white;
			Graphics.Blit(TempBuffer_Lighting, TempBuffer_Lighting, this.m_pointLightMaterial, 1);
		}
	}

	// Token: 0x06007BF8 RID: 31736 RVA: 0x0031E024 File Offset: 0x0031C224
	private bool LightCulled(Vector2 lightPosition, Vector2 cameraPosition, float lightRange, float orthoSize, float aspect)
	{
		return Vector2.Distance(lightPosition, cameraPosition) > lightRange + orthoSize * this.LightCullFactor * aspect;
	}

	// Token: 0x06007BF9 RID: 31737 RVA: 0x0031E040 File Offset: 0x0031C240
	private void RenderGBuffer(RenderTexture source, Camera stackCamera, RenderBuffer depthTarget, RenderTexture TempBuffer_Lighting, Vector3 cachedPosition, Vector3 quantizedPosition)
	{
		int num = this.CurrentMacroResolutionX / this.overrideTileScale + this.extraPixels;
		int num2 = this.CurrentMacroResolutionY / this.overrideTileScale + this.extraPixels;
		Vector3 vector = ((!this.IsInIntro) ? CameraController.PLATFORM_CAMERA_OFFSET : Vector3.zero);
		if (this.ActuallyRenderGBuffer)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(this.CurrentMacroResolutionX / (this.GBUFFER_DESCALE * this.overrideTileScale), this.CurrentMacroResolutionY / (this.GBUFFER_DESCALE * this.overrideTileScale), 0, this.PLATFORM_RENDER_FORMAT);
			Graphics.Blit(Pixelator.m_smallBlackTexture, temporary);
			Graphics.Blit(temporary, temporary, this.m_pointLightMaterial, 1);
			RenderTexture renderTexture = null;
			RenderTexture renderTexture2 = null;
			if (this.IsInIntro || GameManager.Options.LightingQuality != GameOptions.GenericHighMedLowOption.LOW)
			{
				renderTexture = RenderTexture.GetTemporary(num, num2, this.PLATFORM_DEPTH, RenderTextureFormat.Default);
				renderTexture.filterMode = FilterMode.Point;
				Graphics.Blit(Pixelator.m_smallBlackTexture, renderTexture);
				int num3 = ((GameManager.Options.MotionEnhancementMode != GameOptions.PixelatorMotionEnhancementMode.ENHANCED_EXPENSIVE) ? this.cm_gbuffer : this.cm_gbufferSimple);
				stackCamera.clearFlags = CameraClearFlags.Nothing;
				stackCamera.cullingMask = num3;
				stackCamera.targetTexture = renderTexture;
				stackCamera.SetTargetBuffers(renderTexture.colorBuffer, depthTarget);
				stackCamera.SetReplacementShader(this.m_simpleSpriteMaskShader, "UnlitTilted");
				stackCamera.Render();
				if (GameManager.Options.MotionEnhancementMode == GameOptions.PixelatorMotionEnhancementMode.ENHANCED_EXPENSIVE)
				{
					int num4 = this.cm_unpixelated;
					renderTexture2 = RenderTexture.GetTemporary(source.width, source.height, this.PLATFORM_DEPTH, RenderTextureFormat.Default);
					Graphics.Blit(Pixelator.m_smallBlackTexture, renderTexture2);
					this.m_camera.transform.position = new Vector3(Mathf.Floor(cachedPosition.x * (16f * this.ScaleTileScale)), Mathf.Floor(cachedPosition.y * (16f * this.ScaleTileScale)), Mathf.Floor(cachedPosition.z * (16f * this.ScaleTileScale))) / (16f * this.ScaleTileScale);
					this.m_camera.transform.position -= new Vector3(0.0625f, 0.0625f, 0f);
					stackCamera.orthographicSize = this.m_camera.orthographicSize;
					stackCamera.cullingMask = num4;
					stackCamera.targetTexture = renderTexture2;
					stackCamera.SetReplacementShader(this.m_simpleSpriteMaskUnpixelatedShader, "UnlitTilted");
					stackCamera.Render();
					stackCamera.orthographicSize = this.m_camera.orthographicSize * ((float)num2 / ((float)this.CurrentMacroResolutionY / (float)this.overrideTileScale));
					this.m_camera.transform.position = cachedPosition + vector;
				}
				stackCamera.ResetReplacementShader();
				Vector2 vector2 = stackCamera.transform.position.XY();
				this.m_pointLightMaterial.SetVector(this.m_cameraWSID, vector2.ToVector4());
				this.m_pointLightMaterial.SetFloat(this.m_cameraOrthoSizeID, stackCamera.orthographicSize);
				this.m_pointLightMaterial.SetFloat(this.m_cameraOrthoSizeXID, stackCamera.orthographicSize * stackCamera.aspect);
				this.m_pointLightMaterialFast.SetVector(this.m_cameraWSID, vector2.ToVector4());
				this.m_pointLightMaterialFast.SetFloat(this.m_cameraOrthoSizeID, stackCamera.orthographicSize);
				this.m_pointLightMaterialFast.SetFloat(this.m_cameraOrthoSizeXID, stackCamera.orthographicSize * stackCamera.aspect);
				for (int i = 0; i < ShadowSystem.AllLights.Count; i++)
				{
					ShadowSystem shadowSystem = ShadowSystem.AllLights[i];
					if (shadowSystem && shadowSystem.gameObject.activeSelf)
					{
						bool flag = shadowSystem.uLightCookie == null;
						Material material = ((!flag) ? this.m_pointLightMaterial : this.m_pointLightMaterialFast);
						Vector2 vector3 = shadowSystem.transform.position.XY();
						if (!this.LightCulled(vector3, vector2, shadowSystem.uLightRange, stackCamera.orthographicSize, BraveCameraUtility.ASPECT))
						{
							material.SetVector(this.m_lightPosID, vector3.ToVector4());
							material.SetColor(this.m_lightColorID, shadowSystem.uLightColor);
							material.SetFloat(this.m_lightRadiusID, shadowSystem.uLightRange);
							material.SetFloat(this.m_lightIntensityID, shadowSystem.uLightIntensity * this.pointLightMultiplier);
							if (!flag)
							{
								material.SetTexture(this.m_lightCookieID, shadowSystem.uLightCookie);
								material.SetFloat(this.m_lightCookieAngleID, shadowSystem.uLightCookieAngle);
							}
							Graphics.Blit(temporary, temporary, material, 0);
						}
					}
				}
				for (int j = 0; j < this.AdditionalBraveLights.Count; j++)
				{
					if (this.AdditionalBraveLights[j] && this.AdditionalBraveLights[j].gameObject.activeSelf)
					{
						AdditionalBraveLight additionalBraveLight = this.AdditionalBraveLights[j];
						float lightRadius = additionalBraveLight.LightRadius;
						float lightIntensity = additionalBraveLight.LightIntensity;
						if (lightIntensity != 0f)
						{
							Vector2 vector4;
							if (additionalBraveLight.sprite)
							{
								vector4 = additionalBraveLight.sprite.WorldCenter;
							}
							else
							{
								vector4 = additionalBraveLight.transform.position;
							}
							if (!this.LightCulled(vector4, vector2, lightRadius, stackCamera.orthographicSize, BraveCameraUtility.ASPECT))
							{
								Material material2 = this.m_pointLightMaterialFast;
								if (additionalBraveLight.UsesCustomMaterial)
								{
									material2 = additionalBraveLight.CustomLightMaterial;
									material2.SetVector(this.m_cameraWSID, vector2.ToVector4());
									material2.SetFloat(this.m_cameraOrthoSizeID, stackCamera.orthographicSize);
									material2.SetFloat(this.m_cameraOrthoSizeXID, stackCamera.orthographicSize * stackCamera.aspect);
								}
								else if (additionalBraveLight.UsesCone)
								{
									material2 = this.m_pointLightMaterial;
									material2.SetFloat("_LightAngle", additionalBraveLight.LightAngle);
									material2.SetFloat("_LightOrient", additionalBraveLight.LightOrient);
								}
								material2.SetVector(this.m_lightPosID, vector4.ToVector4());
								material2.SetColor(this.m_lightColorID, additionalBraveLight.LightColor);
								material2.SetFloat(this.m_lightRadiusID, lightRadius);
								material2.SetFloat(this.m_lightIntensityID, lightIntensity);
								Graphics.Blit(temporary, temporary, material2, 0);
							}
						}
					}
				}
				if (GameManager.Instance.Dungeon.PlayerIsLight && !GameManager.Instance.IsLoadingLevel && GameManager.Instance.PrimaryPlayer)
				{
					float playerLightRadius = GameManager.Instance.Dungeon.PlayerLightRadius;
					float playerLightIntensity = GameManager.Instance.Dungeon.PlayerLightIntensity;
					Vector2 centerPosition = GameManager.Instance.PrimaryPlayer.CenterPosition;
					if (!this.LightCulled(centerPosition, vector2, playerLightRadius, stackCamera.orthographicSize, BraveCameraUtility.ASPECT))
					{
						this.m_pointLightMaterialFast.SetVector(this.m_lightPosID, centerPosition.ToVector4());
						this.m_pointLightMaterialFast.SetColor(this.m_lightColorID, GameManager.Instance.Dungeon.PlayerLightColor);
						this.m_pointLightMaterialFast.SetFloat(this.m_lightRadiusID, playerLightRadius);
						this.m_pointLightMaterialFast.SetFloat(this.m_lightIntensityID, playerLightIntensity);
						Graphics.Blit(temporary, temporary, this.m_pointLightMaterialFast, 0);
					}
				}
			}
			else
			{
				for (int k = 0; k < this.AdditionalBraveLights.Count; k++)
				{
					if (this.AdditionalBraveLights[k] && this.AdditionalBraveLights[k].gameObject.activeSelf)
					{
						if (this.AdditionalBraveLights[k].UsesCustomMaterial)
						{
							AdditionalBraveLight additionalBraveLight2 = this.AdditionalBraveLights[k];
							Vector2 vector5 = stackCamera.transform.position.XY();
							float lightRadius2 = additionalBraveLight2.LightRadius;
							float lightIntensity2 = additionalBraveLight2.LightIntensity;
							if (lightIntensity2 != 0f)
							{
								Vector2 vector6;
								if (additionalBraveLight2.sprite)
								{
									vector6 = additionalBraveLight2.sprite.WorldCenter;
								}
								else
								{
									vector6 = additionalBraveLight2.transform.position;
								}
								if (!this.LightCulled(vector6, vector5, lightRadius2, stackCamera.orthographicSize, BraveCameraUtility.ASPECT))
								{
									Material material3 = this.m_pointLightMaterialFast;
									if (additionalBraveLight2.UsesCustomMaterial)
									{
										material3 = additionalBraveLight2.CustomLightMaterial;
										material3.SetVector(this.m_cameraWSID, vector5.ToVector4());
										material3.SetFloat(this.m_cameraOrthoSizeID, stackCamera.orthographicSize);
										material3.SetFloat(this.m_cameraOrthoSizeXID, stackCamera.orthographicSize * stackCamera.aspect);
									}
									material3.SetVector(this.m_lightPosID, vector6.ToVector4());
									material3.SetColor(this.m_lightColorID, additionalBraveLight2.LightColor);
									material3.SetFloat(this.m_lightRadiusID, lightRadius2);
									material3.SetFloat(this.m_lightIntensityID, lightIntensity2);
									Graphics.Blit(temporary, temporary, material3, 0);
								}
							}
						}
					}
				}
			}
			if (renderTexture == null)
			{
				Graphics.Blit(temporary, TempBuffer_Lighting);
			}
			else
			{
				RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, this.PLATFORM_DEPTH, this.PLATFORM_RENDER_FORMAT);
				int num5 = Mathf.Max(Mathf.CeilToInt((float)source.width / (float)this.CurrentMacroResolutionX), Mathf.CeilToInt((float)source.height / (float)this.CurrentMacroResolutionY));
				if ((this.CurrentMacroResolutionX * num5 != source.width || this.CurrentMacroResolutionY * num5 != source.height) && (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.MEDIUM || GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH || GameManager.Options.LightingQuality != GameOptions.GenericHighMedLowOption.LOW))
				{
					RenderTexture temporary3 = RenderTexture.GetTemporary(this.CurrentMacroResolutionX * num5, this.CurrentMacroResolutionY * num5, this.PLATFORM_DEPTH, this.PLATFORM_RENDER_FORMAT);
					Graphics.Blit(renderTexture, temporary3);
					temporary3.filterMode = this.DownsamplingFilterMode;
					if (renderTexture2 != null)
					{
						this.m_gbufferLightMaskCombinerMaterial.SetTexture("_MainTex", temporary3);
						this.m_gbufferLightMaskCombinerMaterial.SetTexture("_LightTex", renderTexture2);
						Graphics.Blit(temporary3, temporary2, this.m_gbufferLightMaskCombinerMaterial);
					}
					else
					{
						Graphics.Blit(temporary3, temporary2, this.m_partialCopyMaterial);
					}
					RenderTexture.ReleaseTemporary(temporary3);
				}
				else if (renderTexture2 != null)
				{
					this.m_gbufferLightMaskCombinerMaterial.SetTexture("_MainTex", renderTexture);
					this.m_gbufferLightMaskCombinerMaterial.SetTexture("_LightTex", renderTexture2);
					Graphics.Blit(renderTexture, temporary2, this.m_gbufferLightMaskCombinerMaterial);
				}
				else
				{
					Graphics.Blit(renderTexture, temporary2, this.m_partialCopyMaterial);
				}
				this.m_gbufferMaskMaterial.SetTexture(this.m_lightMaskTexID, temporary2);
				Graphics.Blit(temporary, TempBuffer_Lighting, this.m_gbufferMaskMaterial);
				RenderTexture.ReleaseTemporary(temporary2);
				RenderTexture.ReleaseTemporary(renderTexture);
				if (renderTexture2 != null)
				{
					RenderTexture.ReleaseTemporary(renderTexture2);
				}
			}
			RenderTexture.ReleaseTemporary(temporary);
		}
		else
		{
			Graphics.Blit(Pixelator.m_smallBlackTexture, TempBuffer_Lighting);
			RenderSettings.ambientMode = AmbientMode.Flat;
			RenderSettings.ambientLight = Color.white;
			Graphics.Blit(TempBuffer_Lighting, TempBuffer_Lighting, this.m_pointLightMaterial, 1);
		}
	}

	// Token: 0x06007BFA RID: 31738 RVA: 0x0031EB78 File Offset: 0x0031CD78
	public void CustomFade(float duration, float holdTime, Color startColor, Color endColor, float startScreenBrightness, float endScreenBrightness)
	{
		base.StartCoroutine(this.CustomFade_CR(duration, holdTime, startColor, endColor, startScreenBrightness, endScreenBrightness));
	}

	// Token: 0x06007BFB RID: 31739 RVA: 0x0031EB90 File Offset: 0x0031CD90
	private IEnumerator CustomFade_CR(float duration, float holdTime, Color startColor, Color endColor, float startScreenBrightness, float endScreenBrightness)
	{
		if (holdTime > 0f)
		{
			this.m_fadeMaterial.SetColor(this.m_fadeColorID, startColor);
			this.fade = startScreenBrightness;
			while (holdTime > 0f)
			{
				holdTime -= this.m_deltaTime;
				yield return null;
			}
		}
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += this.m_deltaTime;
			float t = elapsed / duration;
			this.m_fadeMaterial.SetColor(this.m_fadeColorID, Color.Lerp(startColor, endColor, t));
			this.fade = Mathf.Lerp(startScreenBrightness, endScreenBrightness, t);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06007BFC RID: 31740 RVA: 0x0031EBD8 File Offset: 0x0031CDD8
	public void TriggerPastFadeIn()
	{
		base.StartCoroutine(this.HandlePastFadeIn());
	}

	// Token: 0x06007BFD RID: 31741 RVA: 0x0031EBE8 File Offset: 0x0031CDE8
	private IEnumerator HandlePastFadeIn()
	{
		this.m_gammaLocked = true;
		float elapsed = -3f;
		float duration = 3f;
		float startGamma = Mathf.Max(0.05f, 2f - GameManager.Options.Gamma - 0.5f);
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / duration;
			t = Mathf.Clamp01(t);
			this.saturation = t;
			this.m_gammaEffect.ActiveMaterial.SetFloat(this.m_gammaID, Mathf.Lerp(startGamma, 2f - GameManager.Options.Gamma, t));
			yield return null;
		}
		this.m_gammaLocked = false;
		this.saturation = 1f;
		yield break;
	}

	// Token: 0x06007BFE RID: 31742 RVA: 0x0031EC04 File Offset: 0x0031CE04
	public void SetFadeColor(Color c)
	{
		if (this.m_fadeMaterial != null)
		{
			this.m_fadeMaterial.SetColor(this.m_fadeColorID, c);
		}
		this.fade = 1f - c.a;
	}

	// Token: 0x06007BFF RID: 31743 RVA: 0x0031EC3C File Offset: 0x0031CE3C
	public void FreezeFrame()
	{
		base.StartCoroutine(this.HandleFreezeFrame());
	}

	// Token: 0x06007C00 RID: 31744 RVA: 0x0031EC4C File Offset: 0x0031CE4C
	private IEnumerator HandleFreezeFrame()
	{
		float ela = 0f;
		float dura = 0.6f;
		while (ela < dura)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			float t = ela / dura;
			this.SetFreezeFramePower(t, false);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06007C01 RID: 31745 RVA: 0x0031EC68 File Offset: 0x0031CE68
	private IEnumerator HandleTimedFreezeFrame(float duration, float holdDuration)
	{
		float ela = 0f;
		this.SetFreezeFramePower(1f, true);
		while (ela < holdDuration)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		ela = 0f;
		while (ela < duration)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			float t = ela / duration;
			this.SetFreezeFramePower(1f - t, true);
			yield return null;
		}
		this.ClearFreezeFrame();
		yield break;
	}

	// Token: 0x06007C02 RID: 31746 RVA: 0x0031EC94 File Offset: 0x0031CE94
	public void TimedFreezeFrame(float duration, float holdDuration)
	{
		base.StartCoroutine(this.HandleTimedFreezeFrame(duration, holdDuration));
	}

	// Token: 0x06007C03 RID: 31747 RVA: 0x0031ECA8 File Offset: 0x0031CEA8
	public void SetSaturationColorPower(Color satColor, float t)
	{
		this.m_gammaAdjustment = Mathf.Lerp((GameManager.Options.LightingQuality != GameOptions.GenericHighMedLowOption.LOW) ? 0f : (-0.1f), -0.35f, t);
		this.m_fadeMaterial.SetColor("_SaturationColor", Color.Lerp(new Color(1f, 1f, 1f), satColor, t));
		this.saturation = Mathf.Lerp(1f, 0f, t);
		this.m_fadeMaterial.SetFloat(this.m_saturationID, this.saturation);
	}

	// Token: 0x06007C04 RID: 31748 RVA: 0x0031ED3C File Offset: 0x0031CF3C
	public void SetFreezeFramePower(float t, bool isCameraEffect = false)
	{
		this.m_gammaAdjustment = Mathf.Lerp((GameManager.Options.LightingQuality != GameOptions.GenericHighMedLowOption.LOW) ? 0f : (-0.1f), -0.35f, t);
		this.m_fadeMaterial.SetColor("_SaturationColor", Color.Lerp(new Color(1f, 1f, 1f), new Color(0.825f, 0.7f, 0.3f), t));
		this.saturation = Mathf.Lerp(1f, 0f, t);
		if (isCameraEffect)
		{
			this.m_gammaAdjustment = Mathf.Lerp((GameManager.Options.LightingQuality != GameOptions.GenericHighMedLowOption.LOW) ? 0f : (-0.1f), -0.6f, t);
		}
		this.m_fadeMaterial.SetFloat(this.m_saturationID, this.saturation);
	}

	// Token: 0x06007C05 RID: 31749 RVA: 0x0031EE18 File Offset: 0x0031D018
	public void ClearFreezeFrame()
	{
		this.OnChangedLightingQuality(GameManager.Options.LightingQuality);
		this.m_fadeMaterial.SetColor("_SaturationColor", new Color(1f, 1f, 1f));
		this.saturation = 1f;
		this.m_fadeMaterial.SetFloat(this.m_saturationID, 1f);
	}

	// Token: 0x06007C06 RID: 31750 RVA: 0x0031EE7C File Offset: 0x0031D07C
	public void FadeToColor(float duration, Color c, bool reverse = false, float holdTime = 0f)
	{
		if (this.m_fadeLocked)
		{
			return;
		}
		base.StartCoroutine(this.FadeToColor_CR(duration, c, reverse, holdTime));
	}

	// Token: 0x06007C07 RID: 31751 RVA: 0x0031EE9C File Offset: 0x0031D09C
	public void FadeToBlack(float duration, bool reverse = false, float holdTime = 0f)
	{
		if (!reverse && this.fade == 0f)
		{
			return;
		}
		this.m_fadeLocked = true;
		base.StartCoroutine(this.FadeToColor_CR(duration, Color.black, reverse, holdTime));
	}

	// Token: 0x06007C08 RID: 31752 RVA: 0x0031EED4 File Offset: 0x0031D0D4
	private IEnumerator FadeToColor_CR(float duration, Color targetColor, bool reverse = false, float hold = 0f)
	{
		float elapsed = 0f;
		float minFade = 1f - targetColor.a;
		if (hold > 0f)
		{
			this.m_fadeMaterial.SetColor(this.m_fadeColorID, targetColor);
			this.fade = ((!reverse) ? 1f : minFade);
			while (hold > 0f)
			{
				hold -= this.m_deltaTime;
				if (this.KillAllFades)
				{
					this.m_fadeLocked = false;
					yield break;
				}
				yield return null;
			}
		}
		while (elapsed < duration)
		{
			elapsed += this.m_deltaTime;
			float t = elapsed / duration;
			this.m_fadeMaterial.SetColor(this.m_fadeColorID, targetColor);
			float fadeFrac = ((!reverse) ? (1f - t) : t);
			this.fade = Mathf.Lerp(minFade, 1f, fadeFrac);
			if (this.KillAllFades)
			{
				this.m_fadeLocked = false;
				yield break;
			}
			yield return null;
		}
		this.fade = (float)((!reverse) ? 0 : 1);
		this.m_fadeLocked = false;
		yield break;
	}

	// Token: 0x06007C09 RID: 31753 RVA: 0x0031EF0C File Offset: 0x0031D10C
	public void HandleDamagedVignette(Vector2 damageDirection)
	{
		base.StartCoroutine(this.HandleDamagedVignette_CR());
	}

	// Token: 0x06007C0A RID: 31754 RVA: 0x0031EF1C File Offset: 0x0031D11C
	private IEnumerator HandleDamagedVignette_CR()
	{
		float elapsed = 0f;
		float inDuration = 0.04f;
		float outDuration = 0.5f;
		while (elapsed < inDuration + outDuration)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t = 0f;
			if (elapsed < inDuration)
			{
				t = Mathf.SmoothStep(0f, 1f, elapsed / inDuration);
			}
			else
			{
				t = 1f - Mathf.SmoothStep(0f, 1f, (elapsed - inDuration) / outDuration);
			}
			this.m_fadeMaterial.SetFloat("_DamagedPower", t);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06007C0B RID: 31755 RVA: 0x0031EF38 File Offset: 0x0031D138
	public void SetWindowbox(float targetFraction)
	{
		this.m_fadeMaterial.SetFloat("_WindowboxFrac", targetFraction);
	}

	// Token: 0x06007C0C RID: 31756 RVA: 0x0031EF4C File Offset: 0x0031D14C
	public void LerpToLetterbox(float targetFraction, float duration)
	{
		if (duration <= 0f)
		{
			this.m_fadeMaterial.SetFloat("_LetterboxFrac", targetFraction);
		}
		else
		{
			base.StartCoroutine(this.LerpToLetterbox_CR(targetFraction, duration));
		}
	}

	// Token: 0x06007C0D RID: 31757 RVA: 0x0031EF80 File Offset: 0x0031D180
	private IEnumerator LerpToLetterbox_CR(float targetFraction, float duration)
	{
		float elapsed = 0f;
		float startFraction = this.m_fadeMaterial.GetFloat("_LetterboxFrac");
		while (elapsed < duration)
		{
			elapsed += this.m_deltaTime;
			float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			this.m_fadeMaterial.SetFloat("_LetterboxFrac", Mathf.Lerp(startFraction, targetFraction, t));
			yield return null;
		}
		yield break;
	}

	// Token: 0x1700124A RID: 4682
	// (get) Token: 0x06007C0E RID: 31758 RVA: 0x0031EFAC File Offset: 0x0031D1AC
	// (set) Token: 0x06007C0F RID: 31759 RVA: 0x0031EFD8 File Offset: 0x0031D1D8
	public bool CacheCurrentFrameToBuffer
	{
		get
		{
			if (this.m_gammaPass == null)
			{
				this.m_gammaPass = base.GetComponent<GenericFullscreenEffect>();
			}
			return this.m_gammaPass.CacheCurrentFrameToBuffer;
		}
		set
		{
			if (this.m_gammaPass == null)
			{
				this.m_gammaPass = base.GetComponent<GenericFullscreenEffect>();
			}
			this.m_gammaPass.CacheCurrentFrameToBuffer = value;
		}
	}

	// Token: 0x06007C10 RID: 31760 RVA: 0x0031F004 File Offset: 0x0031D204
	public void CacheScreenSpacePositionsForDeathFrame(Vector2 playerPosition, Vector2 enemyPosition)
	{
		this.CachedPlayerViewportPoint = this.m_camera.WorldToViewportPoint(playerPosition.ToVector3ZUp(0f));
		if (enemyPosition != Vector2.zero)
		{
			this.CachedEnemyViewportPoint = this.m_camera.WorldToViewportPoint(enemyPosition.ToVector3ZUp(0f));
		}
		else
		{
			this.CachedEnemyViewportPoint = new Vector3(-1f, -1f, 0f);
		}
	}

	// Token: 0x06007C11 RID: 31761 RVA: 0x0031F078 File Offset: 0x0031D278
	public RenderTexture GetCachedFrame()
	{
		if ((GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.VERY_LOW || GameManager.Options.CurrentPreferredScalingMode == GameOptions.PreferredScalingMode.UNIFORM_SCALING_FAST) && !this.IsInIntro)
		{
			return this.GetCachedFrame_VeryLowSettings();
		}
		return base.GetComponent<GenericFullscreenEffect>().GetCachedFrame();
	}

	// Token: 0x06007C12 RID: 31762 RVA: 0x0031F0B8 File Offset: 0x0031D2B8
	public void ClearCachedFrame()
	{
		this.ClearCachedFrame_VeryLowSettings();
		base.GetComponent<GenericFullscreenEffect>().ClearCachedFrame();
	}

	// Token: 0x06007C13 RID: 31763 RVA: 0x0031F0CC File Offset: 0x0031D2CC
	private Material GetMaterial(Shader shader)
	{
		Material material;
		if (this._shaderMap.TryGetValue(shader, out material))
		{
			return material;
		}
		material = new Material(shader);
		this._shaderMap.Add(shader, material);
		return material;
	}

	// Token: 0x06007C14 RID: 31764 RVA: 0x0031F104 File Offset: 0x0031D304
	public static bool IsValidReflectionObject(tk2dBaseSprite source)
	{
		return source.gameActor != null;
	}

	// Token: 0x04007E5E RID: 32350
	public Texture2D localOcclusionTexture;

	// Token: 0x04007E5F RID: 32351
	private static Pixelator m_instance;

	// Token: 0x04007E60 RID: 32352
	public float occlusionRevealSpeed = 35f;

	// Token: 0x04007E61 RID: 32353
	public float occlusionTransitionFadeMultiplier = 4f;

	// Token: 0x04007E62 RID: 32354
	[NonSerialized]
	public float pointLightMultiplier = 1f;

	// Token: 0x04007E63 RID: 32355
	public Color occludedColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x04007E64 RID: 32356
	public AnimationCurve occlusionPerimeterCurve;

	// Token: 0x04007E65 RID: 32357
	public int perimeterTileWidth = 5;

	// Token: 0x04007E66 RID: 32358
	[Header("Vignette Settings")]
	public float vignettePower;

	// Token: 0x04007E67 RID: 32359
	public float damagedVignettePower = 0.5f;

	// Token: 0x04007E68 RID: 32360
	public Color vignetteColor = Color.black;

	// Token: 0x04007E69 RID: 32361
	public Color damagedVignetteColor = Color.red;

	// Token: 0x04007E6A RID: 32362
	public Shader vignetteShader;

	// Token: 0x04007E6B RID: 32363
	public Shader fadeShader;

	// Token: 0x04007E6C RID: 32364
	public Shader utilityShader;

	// Token: 0x04007E6D RID: 32365
	public bool UseTexturedOcclusion;

	// Token: 0x04007E6E RID: 32366
	public Texture2D ouchTexture;

	// Token: 0x04007E6F RID: 32367
	public Camera minimapCameraRef;

	// Token: 0x04007E70 RID: 32368
	public Texture2D sourceOcclusionTexture;

	// Token: 0x04007E71 RID: 32369
	public float saturation = 1f;

	// Token: 0x04007E72 RID: 32370
	public float fade = 1f;

	// Token: 0x04007E73 RID: 32371
	public bool DoMinimap = true;

	// Token: 0x04007E74 RID: 32372
	public bool DoRenderGBuffer = true;

	// Token: 0x04007E75 RID: 32373
	public bool DoOcclusionLayer = true;

	// Token: 0x04007E76 RID: 32374
	[NonSerialized]
	public bool ManualDoBloom = true;

	// Token: 0x04007E77 RID: 32375
	public bool PRECLUDE_DEPTH_RENDERING;

	// Token: 0x04007E78 RID: 32376
	[NonSerialized]
	public bool DoFinalNonFadedLayer;

	// Token: 0x04007E79 RID: 32377
	[NonSerialized]
	public bool CompositePixelatedUnfadedLayer;

	// Token: 0x04007E7A RID: 32378
	private List<bool> AdditionalRenderPassesInitialized = new List<bool>();

	// Token: 0x04007E7B RID: 32379
	private List<Material> AdditionalRenderPasses = new List<Material>();

	// Token: 0x04007E7C RID: 32380
	private bool m_hasInitializedAdditionalRenderTarget;

	// Token: 0x04007E7D RID: 32381
	public Material AdditionalCoreStackRenderPass;

	// Token: 0x04007E7E RID: 32382
	public int overrideTileScale = 1;

	// Token: 0x04007E7F RID: 32383
	public List<Camera> slavedCameras;

	// Token: 0x04007E80 RID: 32384
	private Camera m_camera;

	// Token: 0x04007E81 RID: 32385
	private Camera m_backgroundCamera;

	// Token: 0x04007E82 RID: 32386
	[SerializeField]
	private Material m_vignetteMaterial;

	// Token: 0x04007E83 RID: 32387
	[SerializeField]
	private Material m_combinedVignetteFadeMaterial;

	// Token: 0x04007E84 RID: 32388
	[SerializeField]
	private Material m_fadeMaterial;

	// Token: 0x04007E85 RID: 32389
	[NonSerialized]
	private Material m_backupFadeMaterial;

	// Token: 0x04007E86 RID: 32390
	[NonSerialized]
	private Material m_compositor;

	// Token: 0x04007E87 RID: 32391
	[NonSerialized]
	private Material m_pointLightMaterial;

	// Token: 0x04007E88 RID: 32392
	[NonSerialized]
	private Material m_pointLightMaterialFast;

	// Token: 0x04007E89 RID: 32393
	[NonSerialized]
	private Material m_coronalLightMaterial;

	// Token: 0x04007E8A RID: 32394
	[NonSerialized]
	private Material m_gbufferMaskMaterial;

	// Token: 0x04007E8B RID: 32395
	[SerializeField]
	private Material m_gbufferLightMaskCombinerMaterial;

	// Token: 0x04007E8C RID: 32396
	[SerializeField]
	private Material m_partialCopyMaterial;

	// Token: 0x04007E8D RID: 32397
	private static Texture2D m_smallBlackTexture;

	// Token: 0x04007E8E RID: 32398
	private Texture2D m_smallWhiteTexture;

	// Token: 0x04007E8F RID: 32399
	private RenderTexture m_texturedOcclusionTarget;

	// Token: 0x04007E90 RID: 32400
	private RenderTexture m_reflectionTargetTexture;

	// Token: 0x04007E91 RID: 32401
	private SENaturalBloomAndDirtyLens m_bloomer;

	// Token: 0x04007E92 RID: 32402
	public Camera AdditionalPreBGCamera;

	// Token: 0x04007E93 RID: 32403
	public Camera AdditionalBGCamera;

	// Token: 0x04007E94 RID: 32404
	public int NewTileScale = 3;

	// Token: 0x04007E95 RID: 32405
	[NonSerialized]
	public float CurrentTileScale = 3f;

	// Token: 0x04007E96 RID: 32406
	[NonSerialized]
	public float ScaleTileScale;

	// Token: 0x04007E97 RID: 32407
	private bool m_occlusionDirty;

	// Token: 0x04007E98 RID: 32408
	private OcclusionLayer occluder;

	// Token: 0x04007E99 RID: 32409
	private Transform m_gameQuadTransform;

	// Token: 0x04007E9A RID: 32410
	private int m_currentMacroResolutionX = 480;

	// Token: 0x04007E9B RID: 32411
	private int m_currentMacroResolutionY = 270;

	// Token: 0x04007E9C RID: 32412
	private int cm_occlusionPartition;

	// Token: 0x04007E9D RID: 32413
	private int cm_core1;

	// Token: 0x04007E9E RID: 32414
	private int cm_core2;

	// Token: 0x04007E9F RID: 32415
	private int cm_core3;

	// Token: 0x04007EA0 RID: 32416
	private int cm_core4;

	// Token: 0x04007EA1 RID: 32417
	private int cm_refl;

	// Token: 0x04007EA2 RID: 32418
	private int cm_gbuffer;

	// Token: 0x04007EA3 RID: 32419
	private int cm_gbufferSimple;

	// Token: 0x04007EA4 RID: 32420
	private int cm_fg;

	// Token: 0x04007EA5 RID: 32421
	private int cm_fg_important;

	// Token: 0x04007EA6 RID: 32422
	private int cm_unoccluded;

	// Token: 0x04007EA7 RID: 32423
	private int cm_unpixelated;

	// Token: 0x04007EA8 RID: 32424
	private int cm_unfaded;

	// Token: 0x04007EA9 RID: 32425
	private int PLATFORM_DEPTH;

	// Token: 0x04007EAA RID: 32426
	private RenderTextureFormat PLATFORM_RENDER_FORMAT;

	// Token: 0x04007EAB RID: 32427
	private Shader m_simpleSpriteMaskShader;

	// Token: 0x04007EAC RID: 32428
	private Shader m_simpleSpriteMaskUnpixelatedShader;

	// Token: 0x04007EAD RID: 32429
	public static bool DebugGraphicsInfo;

	// Token: 0x04007EAE RID: 32430
	private int m_gBufferID;

	// Token: 0x04007EAF RID: 32431
	private int m_saturationID;

	// Token: 0x04007EB0 RID: 32432
	private int m_fadeID;

	// Token: 0x04007EB1 RID: 32433
	private int m_fadeColorID;

	// Token: 0x04007EB2 RID: 32434
	private int m_occlusionMapID;

	// Token: 0x04007EB3 RID: 32435
	private int m_occlusionUVID;

	// Token: 0x04007EB4 RID: 32436
	private int m_reflMapID;

	// Token: 0x04007EB5 RID: 32437
	private int m_reflFlipID;

	// Token: 0x04007EB6 RID: 32438
	private int m_gammaID;

	// Token: 0x04007EB7 RID: 32439
	private int m_vignettePowerID;

	// Token: 0x04007EB8 RID: 32440
	private int m_vignetteColorID;

	// Token: 0x04007EB9 RID: 32441
	private int m_damagedTexID;

	// Token: 0x04007EBA RID: 32442
	private int m_cameraWSID;

	// Token: 0x04007EBB RID: 32443
	private int m_cameraOrthoSizeID;

	// Token: 0x04007EBC RID: 32444
	private int m_cameraOrthoSizeXID;

	// Token: 0x04007EBD RID: 32445
	private int m_lightPosID;

	// Token: 0x04007EBE RID: 32446
	private int m_lightColorID;

	// Token: 0x04007EBF RID: 32447
	private int m_lightRadiusID;

	// Token: 0x04007EC0 RID: 32448
	private int m_lightIntensityID;

	// Token: 0x04007EC1 RID: 32449
	private int m_lightCookieID;

	// Token: 0x04007EC2 RID: 32450
	private int m_lightCookieAngleID;

	// Token: 0x04007EC3 RID: 32451
	private int m_lightMaskTexID;

	// Token: 0x04007EC4 RID: 32452
	private int m_preBackgroundTexID;

	// Token: 0x04007EC5 RID: 32453
	private GenericFullscreenEffect m_gammaEffect;

	// Token: 0x04007EC6 RID: 32454
	private float m_gammaAdjustment;

	// Token: 0x04007EC7 RID: 32455
	public static bool AllowPS4MotionEnhancement;

	// Token: 0x04007EC8 RID: 32456
	protected Dictionary<RoomHandler, IEnumerator> RoomOcclusionCoroutineMap = new Dictionary<RoomHandler, IEnumerator>();

	// Token: 0x04007EC9 RID: 32457
	protected List<RoomHandler> ActiveOcclusionCoroutines = new List<RoomHandler>();

	// Token: 0x04007ECA RID: 32458
	private bool m_occlusionGridDirty;

	// Token: 0x04007ECB RID: 32459
	private List<IntVector2> m_modifiedRangeMins = new List<IntVector2>();

	// Token: 0x04007ECC RID: 32460
	private List<IntVector2> m_modifiedRangeMaxs = new List<IntVector2>();

	// Token: 0x04007ECD RID: 32461
	public int NUM_MACRO_PIXELS_HORIZONTAL = 480;

	// Token: 0x04007ECE RID: 32462
	public int NUM_MACRO_PIXELS_VERTICAL = 270;

	// Token: 0x04007ECF RID: 32463
	private bool generatedNewTexture;

	// Token: 0x04007ED0 RID: 32464
	private IntVector2 oldBaseTile;

	// Token: 0x04007ED1 RID: 32465
	[NonSerialized]
	public static bool IsRenderingOcclusionTexture;

	// Token: 0x04007ED2 RID: 32466
	[NonSerialized]
	public static bool IsRenderingReflectionMap;

	// Token: 0x04007ED3 RID: 32467
	private int m_uvRangeID = -1;

	// Token: 0x04007ED4 RID: 32468
	public FilterMode DownsamplingFilterMode = FilterMode.Bilinear;

	// Token: 0x04007ED5 RID: 32469
	private RenderTexture m_cachedFrame_VeryLowSettings;

	// Token: 0x04007ED6 RID: 32470
	[NonSerialized]
	private bool m_timetubedInstance;

	// Token: 0x04007ED7 RID: 32471
	private int extraPixels = 2;

	// Token: 0x04007ED8 RID: 32472
	[NonSerialized]
	private RenderTexture m_UnblurredProjectileMaskTex;

	// Token: 0x04007ED9 RID: 32473
	[NonSerialized]
	private RenderTexture m_BlurredProjectileMaskTex;

	// Token: 0x04007EDA RID: 32474
	public float ProjectileMaskBlurSize = 0.05f;

	// Token: 0x04007EDB RID: 32475
	private Material m_blurMaterial;

	// Token: 0x04007EDC RID: 32476
	public List<AdditionalBraveLight> AdditionalBraveLights = new List<AdditionalBraveLight>();

	// Token: 0x04007EDD RID: 32477
	private bool m_gammaLocked;

	// Token: 0x04007EDE RID: 32478
	private bool m_fadeLocked;

	// Token: 0x04007EDF RID: 32479
	[NonSerialized]
	public bool KillAllFades;

	// Token: 0x04007EE0 RID: 32480
	private GenericFullscreenEffect m_gammaPass;

	// Token: 0x04007EE1 RID: 32481
	public Vector3 CachedPlayerViewportPoint;

	// Token: 0x04007EE2 RID: 32482
	public Vector3 CachedEnemyViewportPoint;

	// Token: 0x04007EE3 RID: 32483
	public const int OCCLUSION_BUFFER = 2;

	// Token: 0x04007EE4 RID: 32484
	private Dictionary<Shader, Material> _shaderMap = new Dictionary<Shader, Material>();

	// Token: 0x0200152D RID: 5421
	internal class OcclusionCellData
	{
		// Token: 0x06007C16 RID: 31766 RVA: 0x0031F120 File Offset: 0x0031D320
		public OcclusionCellData(CellData c, float dist)
		{
			this.cell = c;
			this.distance = dist;
		}

		// Token: 0x04007EE5 RID: 32485
		public CellData cell;

		// Token: 0x04007EE6 RID: 32486
		public float distance;

		// Token: 0x04007EE7 RID: 32487
		public float changePercentModifier = 1f;
	}

	// Token: 0x0200152E RID: 5422
	private enum CoreRenderMode
	{
		// Token: 0x04007EE9 RID: 32489
		NORMAL,
		// Token: 0x04007EEA RID: 32490
		LOW_QUALITY,
		// Token: 0x04007EEB RID: 32491
		FAST_SCALING
	}
}
