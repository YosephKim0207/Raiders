using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001521 RID: 5409
public class ShadowSystem : BraveBehaviour
{
	// Token: 0x06007B5E RID: 31582 RVA: 0x0031671C File Offset: 0x0031491C
	public static void ForceAllLightsUpdate()
	{
		for (int i = 0; i < ShadowSystem.m_allLights.Count; i++)
		{
			ShadowSystem.m_allLights[i].IsDirty = true;
			ShadowSystem.m_allLights[i].renderer.enabled = true;
		}
	}

	// Token: 0x06007B5F RID: 31583 RVA: 0x0031676C File Offset: 0x0031496C
	public static void ForceRoomLightsUpdate(RoomHandler room, float duration)
	{
		for (int i = 0; i < ShadowSystem.m_allLights.Count; i++)
		{
			IntVector2 intVector = ShadowSystem.m_allLights[i].transform.position.IntXY(VectorConversions.Floor);
			if (GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector) == room)
			{
				ShadowSystem.m_allLights[i].TriggerTemporalUpdate(duration);
			}
		}
	}

	// Token: 0x06007B60 RID: 31584 RVA: 0x003167DC File Offset: 0x003149DC
	public static void ClearPerLevelData()
	{
		ShadowSystem.m_allLights.Clear();
	}

	// Token: 0x17001229 RID: 4649
	// (get) Token: 0x06007B61 RID: 31585 RVA: 0x003167E8 File Offset: 0x003149E8
	public static List<ShadowSystem> AllLights
	{
		get
		{
			return ShadowSystem.m_allLights;
		}
	}

	// Token: 0x1700122A RID: 4650
	// (get) Token: 0x06007B62 RID: 31586 RVA: 0x003167F0 File Offset: 0x003149F0
	private int ModifiedShadowMapSize
	{
		get
		{
			return this.shadowMapSize;
		}
	}

	// Token: 0x06007B63 RID: 31587 RVA: 0x003167F8 File Offset: 0x003149F8
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

	// Token: 0x06007B64 RID: 31588 RVA: 0x00316830 File Offset: 0x00314A30
	private void PreprocessAttachedUnityLight()
	{
		Light componentInChildren = base.transform.parent.GetComponentInChildren<Light>();
		if (componentInChildren != null)
		{
			this.uLightColor = componentInChildren.color;
			this.uLightIntensity = componentInChildren.intensity;
			this.uLightRange = componentInChildren.range;
			LightPulser component = componentInChildren.GetComponent<LightPulser>();
			if (component != null)
			{
				component.AssignShadowSystem(this);
			}
			UnityEngine.Object.Destroy(componentInChildren);
		}
	}

	// Token: 0x06007B65 RID: 31589 RVA: 0x003168A0 File Offset: 0x00314AA0
	private void Awake()
	{
		base.renderer.enabled = false;
		if (!ShadowSystem.m_allLights.Contains(this))
		{
			ShadowSystem.m_allLights.Add(this);
		}
	}

	// Token: 0x06007B66 RID: 31590 RVA: 0x003168CC File Offset: 0x00314ACC
	private void Start()
	{
		if (!this.ignoreUnityLight)
		{
			this.PreprocessAttachedUnityLight();
		}
		Material material = base.renderer.material;
		SceneLightManager component = base.GetComponent<SceneLightManager>();
		if (component != null)
		{
			Color color = component.validColors[UnityEngine.Random.Range(0, component.validColors.Length)];
			material.SetColor("_TintColor", color);
		}
		else
		{
			material.SetColor("_TintColor", Color.white);
		}
	}

	// Token: 0x06007B67 RID: 31591 RVA: 0x0031694C File Offset: 0x00314B4C
	private void CleanupLightsForLowLighting()
	{
		ShadowSystem.DisabledLightsRequireBoost = true;
		if (this._texTarget != null)
		{
			UnityEngine.Object.Destroy(this._texTarget);
		}
		this.ReleaseAllRenderTextures();
		base.renderer.enabled = false;
		base.transform.parent.gameObject.SetActive(false);
	}

	// Token: 0x06007B68 RID: 31592 RVA: 0x003169A4 File Offset: 0x00314BA4
	private void ReturnFromDead()
	{
		ShadowSystem.DisabledLightsRequireBoost = false;
		this.shadowMapSize = Mathf.NextPowerOfTwo(this.shadowMapSize);
		this.shadowMapSize = Mathf.Clamp(this.shadowMapSize, 8, 2048);
		if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf))
		{
			this._texTarget = new RenderTexture(this.ModifiedShadowMapSize, this.ModifiedShadowMapSize, 0, RenderTextureFormat.ARGBHalf);
		}
		else
		{
			this._texTarget = new RenderTexture(this.ModifiedShadowMapSize, this.ModifiedShadowMapSize, 0, RenderTextureFormat.Default);
		}
		this._texTarget.useMipMap = false;
		this._texTarget.autoGenerateMips = false;
		this.shadowCamera.rect = new Rect(0f, 0f, 1f, 1f);
		base.transform.localScale = Vector3.one * this.shadowCamera.orthographicSize / 5f;
		base.transform.localScale = base.transform.localScale.WithZ(base.transform.localScale.z * 1.414f);
		base.renderer.material.mainTexture = this._texTarget;
		this.IsDirty = true;
		base.renderer.enabled = true;
	}

	// Token: 0x06007B69 RID: 31593 RVA: 0x00316AE4 File Offset: 0x00314CE4
	private void OnEnable()
	{
		if (GameManager.Options.LightingQuality == GameOptions.GenericHighMedLowOption.LOW || this.ignoreCustomFloorLight || this.m_locallyDisabled)
		{
			if (this.ignoreCustomFloorLight)
			{
				this.PreprocessAttachedUnityLight();
			}
			this.CleanupLightsForLowLighting();
			return;
		}
		this.ReturnFromDead();
	}

	// Token: 0x06007B6A RID: 31594 RVA: 0x00316B34 File Offset: 0x00314D34
	protected override void OnDestroy()
	{
		if (ShadowSystem.m_allLights != null && ShadowSystem.m_allLights.Contains(this))
		{
			ShadowSystem.m_allLights.Remove(this);
		}
		foreach (KeyValuePair<Shader, Material> keyValuePair in this._shaderMap)
		{
			UnityEngine.Object.Destroy(keyValuePair.Value);
		}
		this._shaderMap.Clear();
		if (this._texTarget != null)
		{
			UnityEngine.Object.Destroy(this._texTarget);
		}
		this.ReleaseAllRenderTextures();
	}

	// Token: 0x06007B6B RID: 31595 RVA: 0x00316BE8 File Offset: 0x00314DE8
	private void TriggerTemporalUpdate(float duration)
	{
		base.StartCoroutine(this.HandleTemporalUpdate(duration));
	}

	// Token: 0x06007B6C RID: 31596 RVA: 0x00316BF8 File Offset: 0x00314DF8
	private IEnumerator HandleTemporalUpdate(float duration)
	{
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			this.RenderFullShadowMap();
			yield return null;
		}
		yield break;
	}

	// Token: 0x1700122B RID: 4651
	// (get) Token: 0x06007B6D RID: 31597 RVA: 0x00316C1C File Offset: 0x00314E1C
	private RenderTextureFormat IdealFormat
	{
		get
		{
			return (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf)) ? RenderTextureFormat.Default : RenderTextureFormat.ARGBHalf;
		}
	}

	// Token: 0x06007B6E RID: 31598 RVA: 0x00316C30 File Offset: 0x00314E30
	private bool RequiresCasterDepthBuffer()
	{
		if (StaticReferenceManager.AllShadowSystemDepthHavers.Count > 0)
		{
			Vector2 vector = base.transform.PositionVector2();
			float num = this.lightRadius * this.lightRadius;
			for (int i = 0; i < StaticReferenceManager.AllShadowSystemDepthHavers.Count; i++)
			{
				Transform transform = StaticReferenceManager.AllShadowSystemDepthHavers[i];
				float sqrMagnitude = (vector - transform.PositionVector2()).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06007B6F RID: 31599 RVA: 0x00316CB0 File Offset: 0x00314EB0
	private void RenderFullShadowMap()
	{
		if (GameManager.Options.LightingQuality == GameOptions.GenericHighMedLowOption.LOW || this.ignoreCustomFloorLight)
		{
			if (this.ignoreCustomFloorLight)
			{
				this.PreprocessAttachedUnityLight();
			}
			this.CleanupLightsForLowLighting();
			return;
		}
		if (!base.transform.parent.gameObject.activeSelf)
		{
			base.transform.parent.gameObject.SetActive(true);
			ShadowSystem.DisabledLightsRequireBoost = false;
			this.ReturnFromDead();
		}
		for (int i = 0; i < this.PersonalCookies.Count; i++)
		{
			this.PersonalCookies[i].enabled = true;
		}
		base.transform.position = base.transform.position.WithZ(base.transform.position.y - 2.5f);
		tk2dBaseSprite tk2dBaseSprite = null;
		int num = -1;
		int num2 = -1;
		if (GameManager.Instance.IsFoyer && GameManager.Instance.PrimaryPlayer != null)
		{
			tk2dBaseSprite = GameManager.Instance.PrimaryPlayer.sprite;
			num = tk2dBaseSprite.gameObject.layer;
			tk2dBaseSprite.gameObject.SetLayerRecursively(LayerMask.NameToLayer("PlayerAndProjectiles"));
			if (GameManager.Instance.SecondaryPlayer != null)
			{
				num2 = GameManager.Instance.SecondaryPlayer.sprite.gameObject.layer;
				GameManager.Instance.SecondaryPlayer.sprite.gameObject.SetLayerRecursively(LayerMask.NameToLayer("PlayerAndProjectiles"));
			}
		}
		int num3 = ((!this.RequiresCasterDepthBuffer()) ? 0 : 16);
		RenderTexture renderTexture = this.PushRenderTexture(this.ModifiedShadowMapSize, this.ModifiedShadowMapSize, num3, this.IdealFormat);
		renderTexture.filterMode = FilterMode.Point;
		renderTexture.wrapMode = TextureWrapMode.Clamp;
		this.shadowCamera.targetTexture = renderTexture;
		if (this.casterShader != null)
		{
			this.shadowCamera.RenderWithShader(this.casterShader, string.Empty);
		}
		else
		{
			this.shadowCamera.Render();
		}
		if (GameManager.Instance.IsFoyer && GameManager.Instance.PrimaryPlayer != null)
		{
			tk2dBaseSprite.gameObject.SetLayerRecursively(num);
			if (GameManager.Instance.SecondaryPlayer != null)
			{
				GameManager.Instance.SecondaryPlayer.sprite.gameObject.SetLayerRecursively(num2);
			}
		}
		Material material = this.GetMaterial(this.lightDistanceShader);
		material.SetFloat("_MinLuminance", this.minLuminance);
		material.SetFloat("_ShadowOffset", this.shadowBias);
		material.SetFloat("_Resolution", (float)this.ModifiedShadowMapSize);
		material.SetFloat("_LightRadius", this.lightRadius);
		RenderTexture renderTexture2 = this.PushRenderTexture(this.ModifiedShadowMapSize, 1, 0, this.IdealFormat);
		Graphics.Blit(renderTexture, renderTexture2, material, 0);
		Graphics.Blit(renderTexture2, this._texTarget, material, (!this.highQuality) ? 1 : 2);
		this.ReleaseAllRenderTextures();
		this.m_initialized = true;
		this.m_cachedPosition = base.transform.position;
		for (int j = 0; j < this.PersonalCookies.Count; j++)
		{
			this.PersonalCookies[j].enabled = false;
		}
		if (!base.renderer.enabled)
		{
			base.renderer.enabled = true;
		}
	}

	// Token: 0x06007B70 RID: 31600 RVA: 0x00317028 File Offset: 0x00315228
	private void Update()
	{
		ShadowSystem.m_numberLightsUpdatedThisFrame = 0;
	}

	// Token: 0x06007B71 RID: 31601 RVA: 0x00317030 File Offset: 0x00315230
	private void LateUpdate()
	{
		if (base.renderer.isVisible || true)
		{
			bool flag = !this._texTarget.IsCreated();
			bool flag2 = base.renderer.isVisible && this.IsDynamic;
			if (!this.m_initialized || this.IsDirty || base.transform.position != this.m_cachedPosition || flag2 || flag)
			{
				if (!flag2 && !flag)
				{
					if (ShadowSystem.m_numberLightsUpdatedThisFrame < 3)
					{
						this.IsDirty = false;
						ShadowSystem.m_numberLightsUpdatedThisFrame++;
						this.RenderFullShadowMap();
					}
				}
				else
				{
					this.IsDirty = false;
					this.RenderFullShadowMap();
				}
			}
		}
	}

	// Token: 0x06007B72 RID: 31602 RVA: 0x003170FC File Offset: 0x003152FC
	private RenderTexture PushRenderTexture(int width, int height, int depth = 0, RenderTextureFormat format = RenderTextureFormat.ARGBHalf)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(width, height, depth, format);
		temporary.filterMode = FilterMode.Point;
		temporary.wrapMode = TextureWrapMode.Clamp;
		this._tempRenderTextures.Add(temporary);
		return temporary;
	}

	// Token: 0x06007B73 RID: 31603 RVA: 0x00317130 File Offset: 0x00315330
	private void ReleaseAllRenderTextures()
	{
		if (this._tempRenderTextures == null || this._tempRenderTextures.Count == 0)
		{
			return;
		}
		foreach (RenderTexture renderTexture in this._tempRenderTextures)
		{
			RenderTexture.ReleaseTemporary(renderTexture);
		}
		this._tempRenderTextures.Clear();
	}

	// Token: 0x04007DE6 RID: 32230
	private static List<ShadowSystem> m_allLights = new List<ShadowSystem>();

	// Token: 0x04007DE7 RID: 32231
	public static bool DisabledLightsRequireBoost = false;

	// Token: 0x04007DE8 RID: 32232
	[NonSerialized]
	public bool IsDirty;

	// Token: 0x04007DE9 RID: 32233
	public bool IsDynamic;

	// Token: 0x04007DEA RID: 32234
	public float lightRadius = 10f;

	// Token: 0x04007DEB RID: 32235
	public bool ignoreUnityLight;

	// Token: 0x04007DEC RID: 32236
	public Color uLightColor;

	// Token: 0x04007DED RID: 32237
	public float uLightIntensity;

	// Token: 0x04007DEE RID: 32238
	public float uLightRange;

	// Token: 0x04007DEF RID: 32239
	public Texture2D uLightCookie;

	// Token: 0x04007DF0 RID: 32240
	public float uLightCookieAngle;

	// Token: 0x04007DF1 RID: 32241
	public bool ignoreCustomFloorLight;

	// Token: 0x04007DF2 RID: 32242
	[SerializeField]
	private float minLuminance = 0.01f;

	// Token: 0x04007DF3 RID: 32243
	[SerializeField]
	private float shadowBias = 0.001f;

	// Token: 0x04007DF4 RID: 32244
	[SerializeField]
	private Camera shadowCamera;

	// Token: 0x04007DF5 RID: 32245
	[SerializeField]
	private bool highQuality;

	// Token: 0x04007DF6 RID: 32246
	[SerializeField]
	private Shader lightDistanceShader;

	// Token: 0x04007DF7 RID: 32247
	[SerializeField]
	private Shader transparentShader;

	// Token: 0x04007DF8 RID: 32248
	[SerializeField]
	private Shader casterShader;

	// Token: 0x04007DF9 RID: 32249
	[SerializeField]
	private int shadowMapSize = 512;

	// Token: 0x04007DFA RID: 32250
	[SerializeField]
	public bool CoronalLight;

	// Token: 0x04007DFB RID: 32251
	[SerializeField]
	public List<Renderer> PersonalCookies = new List<Renderer>();

	// Token: 0x04007DFC RID: 32252
	private RenderTexture _texTarget;

	// Token: 0x04007DFD RID: 32253
	private Dictionary<Shader, Material> _shaderMap = new Dictionary<Shader, Material>();

	// Token: 0x04007DFE RID: 32254
	private List<RenderTexture> _tempRenderTextures = new List<RenderTexture>();

	// Token: 0x04007DFF RID: 32255
	private bool m_initialized;

	// Token: 0x04007E00 RID: 32256
	private Vector3 m_cachedPosition;

	// Token: 0x04007E01 RID: 32257
	private bool m_locallyDisabled;

	// Token: 0x04007E02 RID: 32258
	private static int m_numberLightsUpdatedThisFrame = 0;
}
