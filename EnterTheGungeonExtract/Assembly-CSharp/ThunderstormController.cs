using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001237 RID: 4663
public class ThunderstormController : MonoBehaviour
{
	// Token: 0x06006885 RID: 26757 RVA: 0x0028EE68 File Offset: 0x0028D068
	private void Start()
	{
		this.m_mainCameraTransform = GameManager.Instance.MainCameraController.transform;
		this.m_lastCameraPosition = this.m_mainCameraTransform.position;
		this.RainSystemTransform.position = this.m_mainCameraTransform.position + new Vector3(0f, 20f, 20f);
		this.m_lightningTimer = UnityEngine.Random.Range(this.MinTimeBetweenLightningStrikes, this.MaxTimeBetweenLightningStrikes);
		this.m_system = this.RainSystemTransform.GetComponent<ParticleSystem>();
		this.m_cachedEmissionRate = this.m_system.emission.rate.constant;
		if (this.m_particles == null)
		{
			this.m_particles = new ParticleSystem.Particle[this.m_system.maxParticles];
		}
	}

	// Token: 0x06006886 RID: 26758 RVA: 0x0028EF34 File Offset: 0x0028D134
	private void Update()
	{
		if (GameManager.Instance.IsLoadingLevel)
		{
			return;
		}
		if (this.TrackCamera)
		{
			Vector3 vector = this.m_mainCameraTransform.transform.position - this.m_lastCameraPosition;
			this.m_lastCameraPosition = this.m_mainCameraTransform.transform.position;
			this.RainSystemTransform.position += vector;
			this.RainSystemTransform.position = this.RainSystemTransform.position.WithZ(this.RainSystemTransform.position.y + this.ZOffset);
			if (this.DecayVertical)
			{
				float num = this.m_lastCameraPosition.y;
				if (this.DecayTrackPlayer)
				{
					num = GameManager.Instance.PrimaryPlayer.CenterPosition.y;
				}
				float num2 = Mathf.Lerp(1f, 0f, (num - this.DecayYRange.x) / (this.DecayYRange.y - this.DecayYRange.x));
				BraveUtility.SetEmissionRate(this.m_system, this.m_cachedEmissionRate * num2);
			}
		}
		if (this.m_system.emission.rate.constant > 0f && !TimeTubeCreditsController.IsTimeTubing && AmmonomiconController.Instance && !AmmonomiconController.Instance.IsOpen)
		{
			AkSoundEngine.PostEvent("Play_ENV_rain_loop_01", base.gameObject);
		}
		else
		{
			AkSoundEngine.PostEvent("Stop_ENV_rain_loop_01", base.gameObject);
		}
		if (this.DoLighting)
		{
			this.m_lightningTimer -= ((!GameManager.IsBossIntro) ? BraveTime.DeltaTime : GameManager.INVARIANT_DELTA_TIME);
			if (this.m_lightningTimer <= 0f)
			{
				if (!this.DecayVertical || this.m_lastCameraPosition.y < this.DecayYRange.y)
				{
					base.StartCoroutine(this.DoLightningStrike());
				}
				for (int i = 0; i < this.LightningRenderers.Length; i++)
				{
					base.StartCoroutine(this.ProcessLightningRenderer(this.LightningRenderers[i]));
				}
				if (this.ModifyAmbient)
				{
					base.StartCoroutine(this.HandleLightningAmbientBoost());
				}
				this.m_lightningTimer = UnityEngine.Random.Range(this.MinTimeBetweenLightningStrikes, this.MaxTimeBetweenLightningStrikes);
			}
		}
		this.ProcessParticles();
	}

	// Token: 0x06006887 RID: 26759 RVA: 0x0028F1B0 File Offset: 0x0028D3B0
	private void ProcessParticles()
	{
		int particles = this.m_system.GetParticles(this.m_particles);
		this.m_currentWindForce = new Vector3(Mathf.Sin(Time.timeSinceLevelLoad / 20f) * 7f, 0f, 0f);
		Vector3 vector = this.m_currentWindForce * ((!GameManager.IsBossIntro) ? BraveTime.DeltaTime : GameManager.INVARIANT_DELTA_TIME);
		for (int i = 0; i < particles; i++)
		{
			ParticleSystem.Particle[] particles2 = this.m_particles;
			int num = i;
			particles2[num].velocity = particles2[num].velocity + vector;
		}
		this.m_system.SetParticles(this.m_particles, particles);
	}

	// Token: 0x06006888 RID: 26760 RVA: 0x0028F260 File Offset: 0x0028D460
	protected IEnumerator InvariantWait(float duration)
	{
		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006889 RID: 26761 RVA: 0x0028F27C File Offset: 0x0028D47C
	protected IEnumerator HandleLightningAmbientBoost()
	{
		Color cachedAmbient = RenderSettings.ambientLight;
		Color modAmbient = new Color(cachedAmbient.r + this.AmbientBoost, cachedAmbient.g + this.AmbientBoost, cachedAmbient.b + this.AmbientBoost);
		GameManager.Instance.Dungeon.OverrideAmbientLight = true;
		for (int i = 0; i < 2; i++)
		{
			float elapsed = 0f;
			float duration = 0.15f * (float)(i + 1);
			while (elapsed < duration)
			{
				elapsed += GameManager.INVARIANT_DELTA_TIME;
				float t = elapsed / duration;
				GameManager.Instance.Dungeon.OverrideAmbientColor = Color.Lerp(modAmbient, cachedAmbient, t);
				yield return null;
			}
		}
		GameManager.Instance.Dungeon.OverrideAmbientLight = false;
		yield break;
	}

	// Token: 0x0600688A RID: 26762 RVA: 0x0028F298 File Offset: 0x0028D498
	protected IEnumerator ProcessLightningRenderer(Renderer target)
	{
		target.enabled = true;
		yield return base.StartCoroutine(this.InvariantWait(0.05f));
		target.enabled = false;
		yield return base.StartCoroutine(this.InvariantWait(0.1f));
		target.enabled = true;
		yield return base.StartCoroutine(this.InvariantWait(0.1f));
		target.enabled = false;
		yield break;
	}

	// Token: 0x0600688B RID: 26763 RVA: 0x0028F2BC File Offset: 0x0028D4BC
	protected IEnumerator DoLightningStrike()
	{
		AkSoundEngine.PostEvent("Play_ENV_thunder_flash_01", base.gameObject);
		PlatformInterface.SetAlienFXColor(new Color(1f, 1f, 1f, 1f), 0.25f);
		Pixelator.Instance.FadeToColor(0.1f, Color.white, true, 0.05f);
		yield return new WaitForSeconds(0.15f);
		Pixelator.Instance.FadeToColor(0.1f, Color.white, true, 0.05f);
		yield return new WaitForSeconds(0.1f);
		GameManager.Instance.MainCameraController.DoScreenShake(this.ThunderShake, null, false);
		yield break;
	}

	// Token: 0x040064BC RID: 25788
	public bool DoLighting = true;

	// Token: 0x040064BD RID: 25789
	public float MinTimeBetweenLightningStrikes = 5f;

	// Token: 0x040064BE RID: 25790
	public float MaxTimeBetweenLightningStrikes = 10f;

	// Token: 0x040064BF RID: 25791
	public Transform RainSystemTransform;

	// Token: 0x040064C0 RID: 25792
	public ScreenShakeSettings ThunderShake;

	// Token: 0x040064C1 RID: 25793
	public bool TrackCamera = true;

	// Token: 0x040064C2 RID: 25794
	public bool DecayVertical;

	// Token: 0x040064C3 RID: 25795
	public Vector2 DecayYRange;

	// Token: 0x040064C4 RID: 25796
	public bool DecayTrackPlayer;

	// Token: 0x040064C5 RID: 25797
	public Renderer[] LightningRenderers;

	// Token: 0x040064C6 RID: 25798
	public bool ModifyAmbient;

	// Token: 0x040064C7 RID: 25799
	public float AmbientBoost = 0.25f;

	// Token: 0x040064C8 RID: 25800
	public float ZOffset = -20f;

	// Token: 0x040064C9 RID: 25801
	private Transform m_mainCameraTransform;

	// Token: 0x040064CA RID: 25802
	private Vector3 m_lastCameraPosition;

	// Token: 0x040064CB RID: 25803
	private ParticleSystem m_system;

	// Token: 0x040064CC RID: 25804
	private ParticleSystem.Particle[] m_particles;

	// Token: 0x040064CD RID: 25805
	private float m_cachedEmissionRate;

	// Token: 0x040064CE RID: 25806
	private Vector3 m_currentWindForce = Vector3.zero;

	// Token: 0x040064CF RID: 25807
	private float m_lightningTimer;
}
