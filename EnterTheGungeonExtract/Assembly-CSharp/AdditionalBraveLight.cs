using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x02001511 RID: 5393
public class AdditionalBraveLight : BraveBehaviour
{
	// Token: 0x06007B18 RID: 31512 RVA: 0x00315760 File Offset: 0x00313960
	private void Awake()
	{
		if (this.TriggeredOnBulletBank)
		{
			this.LightIntensity = 0f;
		}
	}

	// Token: 0x06007B19 RID: 31513 RVA: 0x00315778 File Offset: 0x00313978
	public IEnumerator Start()
	{
		yield return null;
		this.Initialize();
		yield break;
	}

	// Token: 0x06007B1A RID: 31514 RVA: 0x00315794 File Offset: 0x00313994
	public void Initialize()
	{
		if (this.m_initialized)
		{
			return;
		}
		if (this.TriggeredOnBulletBank && this.RelevantBulletBank != null)
		{
			if (this.UseProjectileCreatedEvent)
			{
				AIBulletBank relevantBulletBank = this.RelevantBulletBank;
				relevantBulletBank.OnProjectileCreatedWithSource = (Action<string, Projectile>)Delegate.Combine(relevantBulletBank.OnProjectileCreatedWithSource, new Action<string, Projectile>(this.HandleProjectileCreated));
			}
			else
			{
				this.RelevantBulletBank.OnBulletSpawned += this.HandleBulletSpawned;
			}
		}
		Pixelator.Instance.AdditionalBraveLights.Add(this);
		if (this.FadeOnActorDeath)
		{
			if (!this.SpecifyActor)
			{
				this.SpecifyActor = base.aiActor;
			}
			this.SpecifyActor.healthHaver.OnPreDeath += this.OnPreDeath;
		}
		this.m_initialized = true;
	}

	// Token: 0x06007B1B RID: 31515 RVA: 0x00315874 File Offset: 0x00313A74
	private void HandleProjectileCreated(string arg1, Projectile arg2)
	{
		if (arg1 == this.BulletBankTransformName)
		{
			if (this.m_activeCoroutine != null)
			{
				base.StopCoroutine(this.m_activeCoroutine);
			}
			this.m_activeCoroutine = base.StartCoroutine(this.HandleBulletBankFade());
		}
	}

	// Token: 0x06007B1C RID: 31516 RVA: 0x003158B0 File Offset: 0x00313AB0
	public void ManuallyDoBulletSpawnedFade()
	{
		if (this.m_activeCoroutine != null)
		{
			base.StopCoroutine(this.m_activeCoroutine);
		}
		this.m_activeCoroutine = base.StartCoroutine(this.HandleBulletBankFade());
	}

	// Token: 0x06007B1D RID: 31517 RVA: 0x003158DC File Offset: 0x00313ADC
	public void EndEarly()
	{
		if (this.m_activeCoroutine != null)
		{
			this.isFading = true;
		}
	}

	// Token: 0x06007B1E RID: 31518 RVA: 0x003158F0 File Offset: 0x00313AF0
	private void HandleBulletSpawned(Bullet arg1, Projectile arg2)
	{
		if (arg1.RootTransform != null && this.BulletBankTransformName == arg1.RootTransform.name)
		{
			if (this.m_activeCoroutine != null)
			{
				base.StopCoroutine(this.m_activeCoroutine);
			}
			this.m_activeCoroutine = base.StartCoroutine(this.HandleBulletBankFade());
		}
	}

	// Token: 0x06007B1F RID: 31519 RVA: 0x00315954 File Offset: 0x00313B54
	private IEnumerator HandleBulletBankFade()
	{
		float elapsed = 0f;
		this.isFading = false;
		if (this.BulletBankHoldTime > 0f)
		{
			while (elapsed < this.BulletBankHoldTime && !this.isFading)
			{
				elapsed += BraveTime.DeltaTime;
				this.LightIntensity = this.BulletBankIntensity;
				yield return null;
			}
		}
		this.isFading = true;
		elapsed = 0f;
		while (elapsed < this.BulletBankFadeTime)
		{
			elapsed += BraveTime.DeltaTime;
			this.LightIntensity = Mathf.Lerp(this.BulletBankIntensity, 0f, elapsed / this.BulletBankFadeTime);
			yield return null;
		}
		this.isFading = false;
		this.LightIntensity = 0f;
		this.m_activeCoroutine = null;
		yield break;
	}

	// Token: 0x06007B20 RID: 31520 RVA: 0x00315970 File Offset: 0x00313B70
	protected override void OnDestroy()
	{
		if (Pixelator.HasInstance)
		{
			Pixelator.Instance.AdditionalBraveLights.Remove(this);
		}
		if (this.FadeOnActorDeath)
		{
			this.SpecifyActor.healthHaver.OnPreDeath -= this.OnPreDeath;
		}
		base.OnDestroy();
	}

	// Token: 0x06007B21 RID: 31521 RVA: 0x003159C8 File Offset: 0x00313BC8
	private void OnPreDeath(Vector2 deathDir)
	{
		tk2dSpriteAnimationClip deathClip = this.SpecifyActor.healthHaver.GetDeathClip(deathDir.ToAngle());
		base.StartCoroutine(this.FadeLight(deathClip.BaseClipLength));
	}

	// Token: 0x06007B22 RID: 31522 RVA: 0x00315A00 File Offset: 0x00313C00
	private IEnumerator FadeLight(float fadeTime)
	{
		float timer = 0f;
		float startRadius = this.LightRadius;
		float startIntensity = this.LightIntensity;
		while (timer < fadeTime)
		{
			yield return null;
			this.LightRadius = Mathf.Lerp(startRadius, 0f, timer / fadeTime);
			this.LightRadius = Mathf.Lerp(startIntensity, 1f, timer / fadeTime);
			timer += BraveTime.DeltaTime;
		}
		yield break;
	}

	// Token: 0x04007D90 RID: 32144
	public Color LightColor = Color.white;

	// Token: 0x04007D91 RID: 32145
	public float LightIntensity = 3f;

	// Token: 0x04007D92 RID: 32146
	public float LightRadius = 5f;

	// Token: 0x04007D93 RID: 32147
	public bool FadeOnActorDeath;

	// Token: 0x04007D94 RID: 32148
	[ShowInInspectorIf("FadeOnActorDeath", true)]
	public AIActor SpecifyActor;

	// Token: 0x04007D95 RID: 32149
	public bool TriggeredOnBulletBank;

	// Token: 0x04007D96 RID: 32150
	public bool UseProjectileCreatedEvent;

	// Token: 0x04007D97 RID: 32151
	public AIBulletBank RelevantBulletBank;

	// Token: 0x04007D98 RID: 32152
	public float BulletBankHoldTime;

	// Token: 0x04007D99 RID: 32153
	public float BulletBankFadeTime = 0.5f;

	// Token: 0x04007D9A RID: 32154
	public string BulletBankTransformName;

	// Token: 0x04007D9B RID: 32155
	public float BulletBankIntensity = 3f;

	// Token: 0x04007D9C RID: 32156
	public bool UsesCone;

	// Token: 0x04007D9D RID: 32157
	public float LightAngle = 180f;

	// Token: 0x04007D9E RID: 32158
	public float LightOrient;

	// Token: 0x04007D9F RID: 32159
	public bool UsesCustomMaterial;

	// Token: 0x04007DA0 RID: 32160
	public Material CustomLightMaterial;

	// Token: 0x04007DA1 RID: 32161
	private bool m_initialized;

	// Token: 0x04007DA2 RID: 32162
	private Coroutine m_activeCoroutine;

	// Token: 0x04007DA3 RID: 32163
	private bool isFading;
}
