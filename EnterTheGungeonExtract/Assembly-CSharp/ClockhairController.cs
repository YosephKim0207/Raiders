using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200174C RID: 5964
public class ClockhairController : TimeInvariantMonoBehaviour
{
	// Token: 0x06008ACA RID: 35530 RVA: 0x0039D894 File Offset: 0x0039BA94
	private void Start()
	{
		this.Initialize();
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unfaded"));
	}

	// Token: 0x06008ACB RID: 35531 RVA: 0x0039D8B4 File Offset: 0x0039BAB4
	public void Initialize()
	{
		this.SetToTime(DateTime.Now.TimeOfDay);
	}

	// Token: 0x06008ACC RID: 35532 RVA: 0x0039D8D4 File Offset: 0x0039BAD4
	public void SetMotionType(float motionType)
	{
		if (this.IsSpinningWildly)
		{
			return;
		}
		if (this.HasMotionCoroutine)
		{
			this.m_motionType = motionType;
		}
		else
		{
			this.m_motionType = motionType;
			base.StartCoroutine(this.HandleSimpleMotion());
		}
	}

	// Token: 0x06008ACD RID: 35533 RVA: 0x0039D910 File Offset: 0x0039BB10
	public void UpdateDesat(bool shouldDesat, float desatRadiusUV)
	{
		this.m_desatRadius = desatRadiusUV;
		if (shouldDesat)
		{
			if (!this.m_shouldDesat)
			{
				this.m_shouldDesat = true;
				base.StartCoroutine(this.HandleDesat());
			}
		}
		else
		{
			this.m_shouldDesat = false;
		}
	}

	// Token: 0x06008ACE RID: 35534 RVA: 0x0039D94C File Offset: 0x0039BB4C
	private IEnumerator HandleDesat()
	{
		Material distMaterial = new Material(ShaderCache.Acquire("Brave/Internal/RadialDesaturateAndDarken"));
		Vector4 distortionSettings = this.GetCenterPointInScreenUV(base.sprite.WorldCenter, 1f, this.m_desatRadius);
		distMaterial.SetVector("_WaveCenter", distortionSettings);
		Pixelator.Instance.RegisterAdditionalRenderPass(distMaterial);
		float elapsed = 0f;
		while (this.m_shouldDesat)
		{
			elapsed += BraveTime.DeltaTime;
			distortionSettings = this.GetCenterPointInScreenUV(base.sprite.WorldCenter, 1f, this.m_desatRadius);
			distMaterial.SetVector("_WaveCenter", distortionSettings);
			yield return null;
		}
		Pixelator.Instance.DeregisterAdditionalRenderPass(distMaterial);
		UnityEngine.Object.Destroy(distMaterial);
		yield break;
	}

	// Token: 0x06008ACF RID: 35535 RVA: 0x0039D968 File Offset: 0x0039BB68
	public IEnumerator WipeoutDistortionAndFade(float duration)
	{
		this.m_shouldDesat = false;
		float startDistortValue = this.m_distortIntensity;
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			this.m_distortIntensity = Mathf.Lerp(startDistortValue, 0f, elapsed / duration);
			yield return null;
		}
		this.m_shouldDistort = false;
		yield break;
	}

	// Token: 0x06008AD0 RID: 35536 RVA: 0x0039D98C File Offset: 0x0039BB8C
	public void UpdateDistortion(float distortionPower, float distortRadius, float edgeRadius)
	{
		if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.LOW || GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.VERY_LOW)
		{
			return;
		}
		if (distortionPower != 0f)
		{
			this.m_distortIntensity = distortionPower;
			this.m_distortRadius = distortRadius;
			this.m_edgeRadius = edgeRadius;
			if (!this.m_shouldDistort)
			{
				this.m_shouldDistort = true;
				base.StartCoroutine(this.HandleDistortion());
			}
		}
		else
		{
			this.m_shouldDistort = false;
			this.m_distortIntensity = 0f;
			this.m_distortRadius = 25f;
		}
	}

	// Token: 0x06008AD1 RID: 35537 RVA: 0x0039DA1C File Offset: 0x0039BC1C
	private IEnumerator HandleDistortion()
	{
		Material distMaterial = new Material(ShaderCache.Acquire("Brave/Internal/DistortionWave"));
		Vector4 distortionSettings = this.GetCenterPointInScreenUV(base.sprite.WorldCenter, this.m_distortIntensity, this.m_distortRadius);
		distMaterial.SetVector("_WaveCenter", distortionSettings);
		Pixelator.Instance.RegisterAdditionalRenderPass(distMaterial);
		float elapsed = 0f;
		while (this.m_shouldDistort)
		{
			elapsed += BraveTime.DeltaTime;
			distortionSettings = this.GetCenterPointInScreenUV(base.sprite.WorldCenter, this.m_distortIntensity, this.m_distortRadius);
			distMaterial.SetVector("_WaveCenter", distortionSettings);
			distMaterial.SetFloat("_DistortProgress", Mathf.Clamp01(this.m_edgeRadius / 30f));
			yield return null;
		}
		Pixelator.Instance.DeregisterAdditionalRenderPass(distMaterial);
		UnityEngine.Object.Destroy(distMaterial);
		yield break;
	}

	// Token: 0x06008AD2 RID: 35538 RVA: 0x0039DA38 File Offset: 0x0039BC38
	public Vector4 GetCenterPointInScreenUV(Vector2 centerPoint, float dIntensity, float dRadius)
	{
		Vector3 vector = GameManager.Instance.MainCameraController.Camera.WorldToViewportPoint(centerPoint.ToVector3ZUp(0f));
		return new Vector4(vector.x, vector.y, dRadius, dIntensity);
	}

	// Token: 0x06008AD3 RID: 35539 RVA: 0x0039DA7C File Offset: 0x0039BC7C
	public void BeginSpinningWildly()
	{
		this.IsSpinningWildly = true;
		base.StartCoroutine(this.HandleSpinningWildly());
	}

	// Token: 0x06008AD4 RID: 35540 RVA: 0x0039DA94 File Offset: 0x0039BC94
	private IEnumerator HandleSpinningWildly()
	{
		TimeSpan currentTime = DateTime.Now.TimeOfDay;
		while (this.IsSpinningWildly)
		{
			currentTime = currentTime.Add(new TimeSpan((long)(BraveTime.DeltaTime * 10000000f * 60f)));
			this.SetToTime(currentTime);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008AD5 RID: 35541 RVA: 0x0039DAB0 File Offset: 0x0039BCB0
	private IEnumerator HandleSimpleMotion()
	{
		TimeSpan currentTime = DateTime.Now.TimeOfDay;
		this.HasMotionCoroutine = true;
		while (!this.IsSpinningWildly && this.HasMotionCoroutine)
		{
			currentTime = currentTime.Add(new TimeSpan((long)(BraveTime.DeltaTime * 10000000f * this.m_motionType)));
			this.SetToTime(currentTime);
			yield return null;
		}
		this.HasMotionCoroutine = false;
		yield break;
	}

	// Token: 0x06008AD6 RID: 35542 RVA: 0x0039DACC File Offset: 0x0039BCCC
	public void SpinToSessionStart(float duration)
	{
		float sessionStatValue = GameStatsManager.Instance.GetSessionStatValue(TrackedStats.TIME_PLAYED);
		TimeSpan timeSpan = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(0, 0, (int)sessionStatValue));
		base.StartCoroutine(this.SpinToTime(timeSpan, duration));
	}

	// Token: 0x06008AD7 RID: 35543 RVA: 0x0039DB14 File Offset: 0x0039BD14
	public void SetToTime(TimeSpan time)
	{
		int num = time.Hours % 12;
		int minutes = time.Minutes;
		int seconds = time.Seconds;
		float num2 = ((float)num / 12f + (float)minutes / 720f) * -360f;
		float num3 = ((float)minutes / 60f + (float)seconds / 3600f) * -360f;
		float num4 = (float)seconds / 60f * -360f;
		if (this.hourHandPivot != null)
		{
			this.hourHandPivot.transform.localRotation = Quaternion.Euler(0f, 0f, num2);
		}
		if (this.minuteHandPivot != null)
		{
			this.minuteHandPivot.transform.localRotation = Quaternion.Euler(0f, 0f, num3);
		}
		if (this.secondHandPivot != null)
		{
			this.secondHandPivot.transform.localRotation = Quaternion.Euler(0f, 0f, num4);
		}
	}

	// Token: 0x06008AD8 RID: 35544 RVA: 0x0039DC14 File Offset: 0x0039BE14
	private IEnumerator SpinToTime(TimeSpan targetTime, float duration = 5f)
	{
		TimeSpan currentTime = DateTime.Now.TimeOfDay;
		double secondDiff = currentTime.TotalSeconds - targetTime.TotalSeconds;
		int secondsToMovePerSecond = (int)(secondDiff / (double)duration);
		while (secondDiff > 0.0)
		{
			float adjSecondsToMove = Mathf.Lerp((float)(secondsToMovePerSecond / 10), (float)secondsToMovePerSecond, (float)secondDiff / (float)secondsToMovePerSecond);
			int secondsToMove = Mathf.CeilToInt(adjSecondsToMove * this.m_deltaTime);
			currentTime = currentTime.Subtract(new TimeSpan(0, 0, secondsToMove));
			this.SetToTime(currentTime);
			secondDiff = currentTime.TotalSeconds - targetTime.TotalSeconds;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008AD9 RID: 35545 RVA: 0x0039DC40 File Offset: 0x0039BE40
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04009190 RID: 37264
	public float ClockhairInDuration = 2f;

	// Token: 0x04009191 RID: 37265
	public float ClockhairSpinDuration = 1f;

	// Token: 0x04009192 RID: 37266
	public float ClockhairPauseBeforeShot = 0.5f;

	// Token: 0x04009193 RID: 37267
	public Transform hourHandPivot;

	// Token: 0x04009194 RID: 37268
	public Transform minuteHandPivot;

	// Token: 0x04009195 RID: 37269
	public Transform secondHandPivot;

	// Token: 0x04009196 RID: 37270
	public tk2dSpriteAnimator hourAnimator;

	// Token: 0x04009197 RID: 37271
	public tk2dSpriteAnimator minuteAnimator;

	// Token: 0x04009198 RID: 37272
	public tk2dSpriteAnimator secondAnimator;

	// Token: 0x04009199 RID: 37273
	private bool m_shouldDesat;

	// Token: 0x0400919A RID: 37274
	private float m_desatRadius;

	// Token: 0x0400919B RID: 37275
	private bool m_shouldDistort;

	// Token: 0x0400919C RID: 37276
	private float m_distortIntensity;

	// Token: 0x0400919D RID: 37277
	private float m_distortRadius;

	// Token: 0x0400919E RID: 37278
	private float m_edgeRadius = 20f;

	// Token: 0x0400919F RID: 37279
	public bool IsSpinningWildly;

	// Token: 0x040091A0 RID: 37280
	public bool HasMotionCoroutine;

	// Token: 0x040091A1 RID: 37281
	private float m_motionType;
}
