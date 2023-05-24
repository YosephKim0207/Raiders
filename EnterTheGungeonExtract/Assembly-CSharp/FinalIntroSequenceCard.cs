using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001761 RID: 5985
public class FinalIntroSequenceCard : MonoBehaviour
{
	// Token: 0x06008B46 RID: 35654 RVA: 0x0039FC0C File Offset: 0x0039DE0C
	public string[] GetTargetKeys(float cardElapsed)
	{
		string[] array = new string[this.AssociatedKeys.Length];
		for (int i = 0; i < this.AssociatedKeyTimes.Length; i++)
		{
			if (cardElapsed > this.AssociatedKeyTimes[i])
			{
				array[i] = this.AssociatedKeys[i];
			}
			else
			{
				array[i] = string.Empty;
			}
		}
		return array;
	}

	// Token: 0x170014C5 RID: 5317
	// (get) Token: 0x06008B47 RID: 35655 RVA: 0x0039FC68 File Offset: 0x0039DE68
	public float TotalTime
	{
		get
		{
			return this.StartHoldTime + this.PanTime + this.EndHoldTime;
		}
	}

	// Token: 0x06008B48 RID: 35656 RVA: 0x0039FC80 File Offset: 0x0039DE80
	private void Start()
	{
		this.blIntensities = new float[this.additionalBraveLights.Length];
		this.blRadii = new float[this.additionalBraveLights.Length];
		for (int i = 0; i < this.additionalBraveLights.Length; i++)
		{
			this.blIntensities[i] = this.additionalBraveLights[i].LightIntensity;
			this.blRadii[i] = this.additionalBraveLights[i].LightRadius;
		}
		for (int j = 0; j < this.SpriteRenderers.Length; j++)
		{
			this.SpriteRenderers[j].transform.localPosition = this.SpriteRenderers[j].transform.localPosition + CameraController.PLATFORM_CAMERA_OFFSET.WithZ(0f);
		}
	}

	// Token: 0x06008B49 RID: 35657 RVA: 0x0039FD48 File Offset: 0x0039DF48
	public void SetVisibility(float v)
	{
		if (this.BGRenderer.material.HasProperty("_AlphaMod"))
		{
			this.BGRenderer.material.SetFloat("_AlphaMod", v);
		}
		for (int i = 0; i < this.SpriteRenderers.Length; i++)
		{
			if (this.SpriteRenderers[i].GetComponent<tk2dSprite>())
			{
				this.SpriteRenderers[i].GetComponent<tk2dSprite>().usesOverrideMaterial = true;
			}
			this.SpriteRenderers[i].material.SetFloat("_AlphaMod", v);
		}
		if (v > 0.5f)
		{
			this.InitializeClockhands();
			base.StartCoroutine(this.HandleGunBurn());
		}
	}

	// Token: 0x06008B4A RID: 35658 RVA: 0x0039FE00 File Offset: 0x0039E000
	public void ToggleLighting(bool togglon)
	{
		base.StartCoroutine(this.ToggleLightingCR(togglon));
	}

	// Token: 0x06008B4B RID: 35659 RVA: 0x0039FE10 File Offset: 0x0039E010
	private IEnumerator ToggleLightingCR(bool togglon)
	{
		if (togglon)
		{
			this.m_hasLightingBeenEnabled = true;
		}
		float ela = 0f;
		if (!togglon)
		{
			yield return new WaitForSeconds(1f);
		}
		float dura = ((!togglon) ? this.LightingFadeOutDuration : this.LightingFadeInDuration);
		Color[] lightSourceColors = new Color[this.additionalBraveLights.Length];
		Vector3[] lightLocalPositions = new Vector3[this.additionalBraveLights.Length];
		for (int i = 0; i < this.additionalBraveLights.Length; i++)
		{
			lightSourceColors[i] = this.additionalBraveLights[i].LightColor;
			lightLocalPositions[i] = this.additionalBraveLights[i].transform.localPosition;
		}
		while (ela < dura)
		{
			ela += GameManager.INVARIANT_DELTA_TIME;
			float t = ((!togglon) ? (1f - ela / dura) : (ela / dura));
			t = Mathf.Clamp01(t);
			for (int j = 0; j < this.additionalBraveLights.Length; j++)
			{
				if (this.LightingReturnToNeutralGray && !togglon && this.m_hasLightingBeenEnabled)
				{
					this.additionalBraveLights[j].LightColor = Color.Lerp(lightSourceColors[j], new Color(0.9f, 0.7f, 0.2f, 1f), 1f - t);
					this.additionalBraveLights[j].transform.localPosition = Vector3.Lerp(lightLocalPositions[j], lightLocalPositions[j] + new Vector3(0f, 8.5f, 0f), 1f - t);
				}
				this.additionalBraveLights[j].LightIntensity = Mathf.Lerp(0f, this.blIntensities[j], t);
				this.additionalBraveLights[j].LightRadius = Mathf.Lerp(0f, this.blRadii[j], t);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008B4C RID: 35660 RVA: 0x0039FE34 File Offset: 0x0039E034
	private IEnumerator HandleGunBurn()
	{
		if (this.m_gunBurn)
		{
			yield break;
		}
		if (this.GunRenderer == null)
		{
			yield break;
		}
		this.GunRenderer.material.SetFloat("_RadialFade", 2f);
		this.m_gunBurn = true;
		ParticleSystem gunParticles = this.GunRenderer.GetComponentInChildren<ParticleSystem>(true);
		if (gunParticles)
		{
			gunParticles.gameObject.SetActive(true);
		}
		float ela = 0f;
		float dura = this.GunFadeTime;
		yield return new WaitForSeconds(this.GunFadeDelay);
		tk2dSprite gunSprite = this.GunRenderer.GetComponent<tk2dSprite>();
		float gunParticlesPerSecond = 30f;
		float m_elapsedParticles = 0f;
		while (ela < dura)
		{
			ela += BraveTime.DeltaTime;
			m_elapsedParticles += BraveTime.DeltaTime * gunParticlesPerSecond;
			float t = ela / dura;
			this.GunRenderer.material.SetFloat("_RadialFade", 2f - t * 2f);
			this.GunRenderer.material.SetFloat("_Emission", Mathf.Lerp(10f, 4f, t));
			if (gunParticles && m_elapsedParticles > 1f)
			{
				Vector2 normalized = (gunSprite.WorldTopLeft - gunSprite.WorldBottomRight).normalized;
				int num = Mathf.FloorToInt(m_elapsedParticles);
				m_elapsedParticles -= (float)num;
				float num2 = 2f - t * 2f;
				if (num2 < 1.55f && (double)num2 > 0.35)
				{
					float num3 = 1f - Mathf.Abs(0.95f - num2) / 0.6f;
					Vector2 vector = Vector2.Lerp(gunSprite.WorldTopRight, gunSprite.WorldBottomLeft, Mathf.InverseLerp(1.55f, 0.35f, num2)) + new Vector2(0f, 0.0625f);
					for (int i = 0; i < num; i++)
					{
						Vector2 vector2 = vector + UnityEngine.Random.Range(-1f, 1f) * normalized * num3 * 0.875f;
						ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
						{
							position = vector2.ToVector3ZUp(gunParticles.transform.position.z),
							velocity = gunParticles.startSpeed * gunParticles.transform.forward,
							startSize = gunParticles.startSize,
							startLifetime = gunParticles.startLifetime,
							startColor = gunParticles.startColor
						};
						gunParticles.Emit(emitParams, 1);
					}
				}
			}
			yield return null;
		}
		ela = 0f;
		dura = 4f;
		while (ela < dura)
		{
			ela += BraveTime.DeltaTime;
			float t2 = ela / dura;
			this.GunRenderer.material.SetFloat("_Emission", Mathf.Lerp(4f, 0f, t2));
			yield return null;
		}
		yield break;
	}

	// Token: 0x06008B4D RID: 35661 RVA: 0x0039FE50 File Offset: 0x0039E050
	private void InitializeClockhands()
	{
		if (this.m_clockhandsInitialized)
		{
			return;
		}
		this.m_clockhandsInitialized = true;
		if (this.clockhand1 && this.clockhand2)
		{
			this.clockhand1.GetComponent<SimpleSpriteRotator>().enabled = true;
			this.clockhand2.GetComponent<SimpleSpriteRotator>().enabled = true;
			this.clockhand1.transform.localRotation = Quaternion.Euler(0f, 0f, 135f);
			this.clockhand2.transform.localRotation = Quaternion.Euler(0f, 0f, 315f);
		}
	}

	// Token: 0x06008B4E RID: 35662 RVA: 0x0039FEFC File Offset: 0x0039E0FC
	private void Update()
	{
		this.m_elapsed += GameManager.INVARIANT_DELTA_TIME;
		for (int i = 0; i < this.additionalBraveLights.Length; i++)
		{
		}
	}

	// Token: 0x04009213 RID: 37395
	public Transform StartCameraTransform;

	// Token: 0x04009214 RID: 37396
	public Transform EndCameraTransform;

	// Token: 0x04009215 RID: 37397
	public Renderer BGRenderer;

	// Token: 0x04009216 RID: 37398
	public Renderer[] SpriteRenderers;

	// Token: 0x04009217 RID: 37399
	public Renderer GunRenderer;

	// Token: 0x04009218 RID: 37400
	public float StartHoldTime = 3f;

	// Token: 0x04009219 RID: 37401
	public float PanTime = 5f;

	// Token: 0x0400921A RID: 37402
	public float EndHoldTime = 3f;

	// Token: 0x0400921B RID: 37403
	public float GunFadeDelay = 3f;

	// Token: 0x0400921C RID: 37404
	public float GunFadeTime = 2.5f;

	// Token: 0x0400921D RID: 37405
	public float CustomTextFadeInTime = -1f;

	// Token: 0x0400921E RID: 37406
	public float CustomTextFadeOutTime = -1f;

	// Token: 0x0400921F RID: 37407
	public string[] AssociatedKeys;

	// Token: 0x04009220 RID: 37408
	public float[] AssociatedKeyTimes;

	// Token: 0x04009221 RID: 37409
	public tk2dBaseSprite borderSprite;

	// Token: 0x04009222 RID: 37410
	public AdditionalBraveLight[] additionalBraveLights;

	// Token: 0x04009223 RID: 37411
	private float[] blIntensities;

	// Token: 0x04009224 RID: 37412
	private float[] blRadii;

	// Token: 0x04009225 RID: 37413
	private float m_elapsed;

	// Token: 0x04009226 RID: 37414
	public float LightingFadeInDuration = 6f;

	// Token: 0x04009227 RID: 37415
	public float LightingFadeOutDuration = 6f;

	// Token: 0x04009228 RID: 37416
	public bool LightingReturnToNeutralGray;

	// Token: 0x04009229 RID: 37417
	private bool m_hasLightingBeenEnabled;

	// Token: 0x0400922A RID: 37418
	private bool m_gunBurn;

	// Token: 0x0400922B RID: 37419
	public Transform clockhand1;

	// Token: 0x0400922C RID: 37420
	public Transform clockhand2;

	// Token: 0x0400922D RID: 37421
	private bool m_clockhandsInitialized;
}
