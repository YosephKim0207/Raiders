using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001800 RID: 6144
public class TitleDioramaController : MonoBehaviour
{
	// Token: 0x060090D5 RID: 37077 RVA: 0x003D40B4 File Offset: 0x003D22B4
	private IEnumerator Start()
	{
		while (Foyer.DoIntroSequence)
		{
			yield return null;
		}
		if (this.m_fadeCamera != null)
		{
			BraveCameraUtility.MaintainCameraAspect(this.m_fadeCamera);
		}
		if (Foyer.DoMainMenu && this.PastIslandSprite == null)
		{
			if (this.ShouldUseLQ())
			{
				if (this.CloudsPrefab)
				{
					this.CloudsPrefab.SetActive(false);
				}
				if (this.BackupCloudsPrefab)
				{
					this.BackupCloudsPrefab.SetActive(true);
				}
			}
			else
			{
				if (this.CloudsPrefab)
				{
					this.CloudsPrefab.SetActive(true);
				}
				if (this.BackupCloudsPrefab)
				{
					this.BackupCloudsPrefab.SetActive(false);
				}
			}
			base.StartCoroutine(this.Core(true));
			base.StartCoroutine(this.HandleLichIdlePhase());
		}
		else if (this.FadeQuad)
		{
			this.FadeQuad.enabled = false;
		}
		yield break;
	}

	// Token: 0x060090D6 RID: 37078 RVA: 0x003D40D0 File Offset: 0x003D22D0
	private bool ShouldUseLQ()
	{
		return GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.VERY_LOW || GameManager.Options.GetDefaultRecommendedGraphicalQuality() != GameOptions.GenericHighMedLowOption.HIGH;
	}

	// Token: 0x060090D7 RID: 37079 RVA: 0x003D40F4 File Offset: 0x003D22F4
	public void CacheFrameToFadeBuffer(Camera cacheCamera)
	{
		this.FadeQuad.material.SetFloat("_UseAddlTex", 1f);
		this.FadeQuad.material.SetTexture("_AddlTex", Pixelator.Instance.GetCachedFrame());
	}

	// Token: 0x060090D8 RID: 37080 RVA: 0x003D4130 File Offset: 0x003D2330
	public bool IsRevealed(bool doReveal = false)
	{
		if (doReveal)
		{
			this.m_rushed = true;
		}
		return this.m_isRevealed;
	}

	// Token: 0x060090D9 RID: 37081 RVA: 0x003D4148 File Offset: 0x003D2348
	public void ManualTrigger()
	{
		this.m_manualTrigger = true;
		base.StartCoroutine(this.Core(false));
		base.StartCoroutine(this.HandleLichIdlePhase());
	}

	// Token: 0x060090DA RID: 37082 RVA: 0x003D416C File Offset: 0x003D236C
	private void Update()
	{
		if (this.m_fadeCamera != null)
		{
			BraveCameraUtility.MaintainCameraAspect(this.m_fadeCamera);
		}
		if (this.ShouldUseLQ() && this.CloudsPrefab && this.CloudsPrefab.activeSelf)
		{
			if (this.CloudsPrefab)
			{
				this.CloudsPrefab.SetActive(false);
			}
			if (this.BackupCloudsPrefab)
			{
				this.BackupCloudsPrefab.SetActive(true);
			}
		}
		else if (!this.ShouldUseLQ() && this.BackupCloudsPrefab && this.BackupCloudsPrefab.activeSelf)
		{
			if (this.CloudsPrefab)
			{
				this.CloudsPrefab.SetActive(true);
			}
			if (this.BackupCloudsPrefab)
			{
				this.BackupCloudsPrefab.SetActive(false);
			}
		}
		if (this.FadeQuad != null && this.FadeQuad.enabled && this.IsRevealed(false))
		{
			this.FadeQuad.enabled = false;
		}
	}

	// Token: 0x060090DB RID: 37083 RVA: 0x003D429C File Offset: 0x003D249C
	private IEnumerator Core(bool isFoyer = true)
	{
		GameManager.Instance.MainCameraController.SetManualControl(true, false);
		GameManager.Instance.MainCameraController.OverridePosition = base.transform.position + new Vector3(0.125f, 0.125f, 0f);
		if (GameManager.Options.CurrentPreferredScalingMode == GameOptions.PreferredScalingMode.UNIFORM_SCALING_FAST)
		{
			GameManager.Instance.MainCameraController.OverridePosition = base.transform.position;
		}
		GameManager.Instance.MainCameraController.OverrideZoomScale = 0.5f;
		GameManager.Instance.MainCameraController.CurrentZoomScale = 0.5f;
		Pixelator.Instance.DoOcclusionLayer = false;
		Pixelator.Instance.DoFinalNonFadedLayer = false;
		Pixelator.Instance.CompositePixelatedUnfadedLayer = false;
		if (isFoyer)
		{
			if (this.FadeQuad != null)
			{
				base.StartCoroutine(this.HandleReveal());
			}
		}
		else if (this.FadeQuad != null)
		{
			this.FadeQuad.material.SetFloat("_Threshold", 0f);
		}
		bool continueDoing = ((!isFoyer) ? this.m_manualTrigger : Foyer.DoMainMenu);
		while (continueDoing)
		{
			yield return null;
			if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
			{
				this.m_rushed = true;
			}
			if (BraveInput.PlayerlessInstance && BraveInput.PlayerlessInstance.ActiveActions != null && BraveInput.PlayerlessInstance.ActiveActions.IntroSkipActionPressed())
			{
				this.m_rushed = true;
			}
			continueDoing = ((!isFoyer) ? this.m_manualTrigger : Foyer.DoMainMenu);
		}
		Pixelator.Instance.DoOcclusionLayer = true;
		GameManager.Instance.MainCameraController.OverrideZoomScale = 1f;
		GameManager.Instance.MainCameraController.CurrentZoomScale = 1f;
		GameManager.Instance.MainCameraController.SetManualControl(false, false);
		if (this.FadeQuad != null)
		{
			this.FadeQuad.material.SetFloat("_Threshold", 0f);
			this.Release();
		}
		yield break;
	}

	// Token: 0x060090DC RID: 37084 RVA: 0x003D42C0 File Offset: 0x003D24C0
	private void OnDestroy()
	{
		this.Release();
	}

	// Token: 0x060090DD RID: 37085 RVA: 0x003D42C8 File Offset: 0x003D24C8
	private void Release()
	{
		if (this.m_cachedFadeBuffer != null)
		{
			RenderTexture.ReleaseTemporary(this.m_cachedFadeBuffer);
			this.m_cachedFadeBuffer = null;
		}
	}

	// Token: 0x060090DE RID: 37086 RVA: 0x003D42F0 File Offset: 0x003D24F0
	private IEnumerator LerpFadeValue(float startValue, float endValue, float duration, bool linearStep = false)
	{
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.Lerp(startValue, endValue, elapsed / duration);
			if (linearStep)
			{
				t = Mathf.Lerp(startValue, endValue, BraveMathCollege.HermiteInterpolation(elapsed / duration));
			}
			if (this.FadeQuad)
			{
				this.FadeQuad.material.SetFloat("_Threshold", t);
			}
			if (this.m_rushed && !linearStep)
			{
				yield break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060090DF RID: 37087 RVA: 0x003D4328 File Offset: 0x003D2528
	private IEnumerator HandleFinalEyeholeEmission()
	{
		yield return base.StartCoroutine(this.LerpEyeholeEmission(1800f, 2400f, 0.2f, true));
		yield return base.StartCoroutine(this.LerpEyeholeEmission(2400f, 200f, 0.5f, true));
		while (Foyer.DoMainMenu)
		{
			yield return base.StartCoroutine(this.LerpEyeholeEmission(200f, 300f, 1f, true));
			yield return base.StartCoroutine(this.LerpEyeholeEmission(300f, 200f, 1f, true));
		}
		yield break;
	}

	// Token: 0x060090E0 RID: 37088 RVA: 0x003D4344 File Offset: 0x003D2544
	private IEnumerator LerpEyeholeEmissionColorPower(float startValue, float endValue, float duration, bool really = false)
	{
		float elapsed = 0f;
		this.Eyeholes.usesOverrideMaterial = true;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			this.Eyeholes.renderer.material.SetFloat("_EmissiveColorPower", Mathf.Lerp(startValue, endValue, elapsed / duration));
			if (this.m_rushed && !really)
			{
				yield break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060090E1 RID: 37089 RVA: 0x003D437C File Offset: 0x003D257C
	private IEnumerator LerpEyeholeEmission(float startValue, float endValue, float duration, bool really = false)
	{
		float elapsed = 0f;
		this.Eyeholes.usesOverrideMaterial = true;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			this.Eyeholes.renderer.material.SetFloat("_EmissivePower", Mathf.Lerp(startValue, endValue, elapsed / duration));
			if (this.m_rushed && !really)
			{
				yield break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060090E2 RID: 37090 RVA: 0x003D43B4 File Offset: 0x003D25B4
	public void ForceHideFadeQuad()
	{
		if (this.FadeQuad)
		{
			this.FadeQuad.material.SetFloat("_Threshold", 0f);
			this.FadeQuad.enabled = false;
		}
	}

	// Token: 0x060090E3 RID: 37091 RVA: 0x003D43EC File Offset: 0x003D25EC
	private IEnumerator HandleReveal()
	{
		if (this.FadeQuad)
		{
			this.FadeQuad.enabled = true;
			this.FadeQuad.material.SetFloat("_Threshold", 1.25f);
		}
		float elapsed = 0f;
		while (elapsed < 0.25f && !this.m_rushed)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		if (!this.m_rushed)
		{
			yield return base.StartCoroutine(this.LerpFadeValue(1.25f, 1f, 1.75f, false));
		}
		elapsed = 0f;
		while (elapsed < 0.75f && !this.m_rushed)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		if (!this.m_rushed)
		{
			base.StartCoroutine(this.LerpFadeValue(1f, 1.05f, 1.25f, false));
		}
		if (!this.m_rushed)
		{
			yield return base.StartCoroutine(this.LerpEyeholeEmission(200f, 1800f, 1.1f, false));
		}
		if (this.m_rushed)
		{
			base.StartCoroutine(this.HandleFinalEyeholeEmission());
			yield return base.StartCoroutine(this.LerpFadeValue(1.05f, 0f, 0.3f, true));
		}
		else
		{
			base.StartCoroutine(this.HandleFinalEyeholeEmission());
			yield return base.StartCoroutine(this.LerpFadeValue(1.05f, 0f, 0.6f, true));
		}
		this.m_isRevealed = true;
		yield break;
	}

	// Token: 0x060090E4 RID: 37092 RVA: 0x003D4408 File Offset: 0x003D2608
	private IEnumerator HandleLichIdlePhase()
	{
		if (!this.LichArmAnimator)
		{
			yield break;
		}
		float duration = UnityEngine.Random.Range(1f, 3f);
		this.LichArmAnimator.Play("lich_arm_idle");
		this.LichBodyAnimator.Play("lich_body_idle");
		this.LichCapeAnimator.Play("lich_cape_idle");
		yield return new WaitForSeconds(duration);
		if (UnityEngine.Random.value < 0.5f)
		{
			base.StartCoroutine(this.HandleLichFiddlePhase());
		}
		else
		{
			base.StartCoroutine(this.HandleLichCapePhase());
		}
		yield break;
	}

	// Token: 0x060090E5 RID: 37093 RVA: 0x003D4424 File Offset: 0x003D2624
	private IEnumerator HandleLichFiddlePhase()
	{
		int numFiddles = UnityEngine.Random.Range(1, 5);
		this.LichBodyAnimator.Play("lich_body_idle");
		this.LichCapeAnimator.Play("lich_cape_idle");
		for (int i = 0; i < numFiddles; i++)
		{
			this.LichArmAnimator.Play("lich_arm_fiddle");
			while (this.LichArmAnimator.IsPlaying("lich_arm_fiddle"))
			{
				yield return null;
			}
		}
		base.StartCoroutine(this.HandleLichIdlePhase());
		yield break;
	}

	// Token: 0x060090E6 RID: 37094 RVA: 0x003D4440 File Offset: 0x003D2640
	private IEnumerator HandleLichCapePhase()
	{
		float duration = UnityEngine.Random.Range(8f, 16f);
		this.LichArmAnimator.Play("lich_arm_windy");
		this.LichBodyAnimator.Play("lich_body_windy");
		this.LichCapeAnimator.Play("lich_cape_in");
		yield return new WaitForSeconds(duration);
		this.LichCapeAnimator.Play("lich_cape_out");
		while (this.LichCapeAnimator.IsPlaying("lich_cape_out"))
		{
			yield return null;
		}
		base.StartCoroutine(this.HandleLichIdlePhase());
		yield break;
	}

	// Token: 0x040098E7 RID: 39143
	public tk2dSpriteAnimator LichArmAnimator;

	// Token: 0x040098E8 RID: 39144
	public tk2dSpriteAnimator LichBodyAnimator;

	// Token: 0x040098E9 RID: 39145
	public tk2dSpriteAnimator LichCapeAnimator;

	// Token: 0x040098EA RID: 39146
	public tk2dSprite BeaconBeams;

	// Token: 0x040098EB RID: 39147
	public tk2dSprite Eyeholes;

	// Token: 0x040098EC RID: 39148
	public MeshRenderer FadeQuad;

	// Token: 0x040098ED RID: 39149
	public tk2dSprite PastIslandSprite;

	// Token: 0x040098EE RID: 39150
	public MeshRenderer SkyRenderer;

	// Token: 0x040098EF RID: 39151
	public Camera m_fadeCamera;

	// Token: 0x040098F0 RID: 39152
	public GameObject VFX_BulletImpact;

	// Token: 0x040098F1 RID: 39153
	public GameObject VFX_Splash;

	// Token: 0x040098F2 RID: 39154
	public GameObject VFX_TrailParticles;

	// Token: 0x040098F3 RID: 39155
	public GameObject CloudsPrefab;

	// Token: 0x040098F4 RID: 39156
	public GameObject BackupCloudsPrefab;

	// Token: 0x040098F5 RID: 39157
	private bool m_manualTrigger;

	// Token: 0x040098F6 RID: 39158
	private bool m_isRevealed;

	// Token: 0x040098F7 RID: 39159
	private RenderTexture m_cachedFadeBuffer;

	// Token: 0x040098F8 RID: 39160
	private bool m_rushed;
}
