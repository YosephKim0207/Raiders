using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200129E RID: 4766
public static class GlobalSparksDoer
{
	// Token: 0x06006AA9 RID: 27305 RVA: 0x0029D0F0 File Offset: 0x0029B2F0
	public static RedMatterParticleController GetRedMatterController()
	{
		return (!GlobalSparksDoer.m_redMatterParticles) ? null : GlobalSparksDoer.m_redMatterParticles.GetComponent<RedMatterParticleController>();
	}

	// Token: 0x06006AAA RID: 27306 RVA: 0x0029D114 File Offset: 0x0029B314
	public static EmbersController GetEmbersController()
	{
		if (GlobalSparksDoer.EmberParticles == null)
		{
			GlobalSparksDoer.InitializeParticles(GlobalSparksDoer.SparksType.EMBERS_SWIRLING);
		}
		return GlobalSparksDoer.EmberParticles.GetComponent<EmbersController>();
	}

	// Token: 0x06006AAB RID: 27307 RVA: 0x0029D138 File Offset: 0x0029B338
	public static void DoSingleParticle(Vector3 position, Vector3 direction, float? startSize = null, float? startLifetime = null, Color? startColor = null, GlobalSparksDoer.SparksType systemType = GlobalSparksDoer.SparksType.SPARKS_ADDITIVE_DEFAULT)
	{
		ParticleSystem particleSystem = GlobalSparksDoer.m_particles;
		switch (systemType)
		{
		case GlobalSparksDoer.SparksType.SPARKS_ADDITIVE_DEFAULT:
			particleSystem = GlobalSparksDoer.m_particles;
			break;
		case GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE:
			particleSystem = GlobalSparksDoer.m_phantomParticles;
			break;
		case GlobalSparksDoer.SparksType.FLOATY_CHAFF:
			particleSystem = GlobalSparksDoer.m_chaffParticles;
			break;
		case GlobalSparksDoer.SparksType.SOLID_SPARKLES:
			particleSystem = GlobalSparksDoer.m_sparkleParticles;
			break;
		case GlobalSparksDoer.SparksType.EMBERS_SWIRLING:
			particleSystem = GlobalSparksDoer.EmberParticles;
			break;
		case GlobalSparksDoer.SparksType.STRAIGHT_UP_FIRE:
			particleSystem = GlobalSparksDoer.m_fireParticles;
			break;
		case GlobalSparksDoer.SparksType.DARK_MAGICKS:
			particleSystem = GlobalSparksDoer.m_darkMagicParticles;
			break;
		case GlobalSparksDoer.SparksType.BLOODY_BLOOD:
			particleSystem = GlobalSparksDoer.m_bloodParticles;
			break;
		case GlobalSparksDoer.SparksType.STRAIGHT_UP_GREEN_FIRE:
			particleSystem = GlobalSparksDoer.m_greenFireParticles;
			break;
		case GlobalSparksDoer.SparksType.RED_MATTER:
			particleSystem = GlobalSparksDoer.m_redMatterParticles;
			break;
		}
		if (particleSystem == null)
		{
			particleSystem = GlobalSparksDoer.InitializeParticles(systemType);
		}
		ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
		{
			position = position,
			velocity = direction,
			startSize = ((startSize == null) ? particleSystem.startSize : startSize.Value),
			startLifetime = ((startLifetime == null) ? particleSystem.startLifetime : startLifetime.Value),
			startColor = ((startColor == null) ? particleSystem.startColor : startColor.Value),
			randomSeed = (uint)UnityEngine.Random.Range(1, 1000)
		};
		particleSystem.Emit(emitParams, 1);
	}

	// Token: 0x06006AAC RID: 27308 RVA: 0x0029D2B0 File Offset: 0x0029B4B0
	public static void DoRandomParticleBurst(int num, Vector3 minPosition, Vector3 maxPosition, Vector3 direction, float angleVariance, float magnitudeVariance, float? startSize = null, float? startLifetime = null, Color? startColor = null, GlobalSparksDoer.SparksType systemType = GlobalSparksDoer.SparksType.SPARKS_ADDITIVE_DEFAULT)
	{
		for (int i = 0; i < num; i++)
		{
			Vector3 vector = new Vector3(UnityEngine.Random.Range(minPosition.x, maxPosition.x), UnityEngine.Random.Range(minPosition.y, maxPosition.y), UnityEngine.Random.Range(minPosition.z, maxPosition.z));
			Vector3 vector2 = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-angleVariance, angleVariance)) * (direction.normalized * UnityEngine.Random.Range(direction.magnitude - magnitudeVariance, direction.magnitude + magnitudeVariance));
			GlobalSparksDoer.DoSingleParticle(vector, vector2, startSize, startLifetime, startColor, systemType);
		}
	}

	// Token: 0x06006AAD RID: 27309 RVA: 0x0029D364 File Offset: 0x0029B564
	public static void DoLinearParticleBurst(int num, Vector3 minPosition, Vector3 maxPosition, float angleVariance, float baseMagnitude, float magnitudeVariance, float? startSize = null, float? startLifetime = null, Color? startColor = null, GlobalSparksDoer.SparksType systemType = GlobalSparksDoer.SparksType.SPARKS_ADDITIVE_DEFAULT)
	{
		for (int i = 0; i < num; i++)
		{
			Vector3 vector = Vector3.Lerp(minPosition, maxPosition, ((float)i + 1f) / (float)num);
			Vector3 vector2 = UnityEngine.Random.insideUnitCircle.normalized.ToVector3ZUp(0f);
			Vector3 vector3 = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-angleVariance, angleVariance)) * (vector2.normalized * UnityEngine.Random.Range(baseMagnitude - magnitudeVariance, vector2.magnitude + magnitudeVariance));
			GlobalSparksDoer.DoSingleParticle(vector, vector3, startSize, startLifetime, startColor, systemType);
		}
	}

	// Token: 0x06006AAE RID: 27310 RVA: 0x0029D3FC File Offset: 0x0029B5FC
	public static void DoRadialParticleBurst(int num, Vector3 minPosition, Vector3 maxPosition, float angleVariance, float baseMagnitude, float magnitudeVariance, float? startSize = null, float? startLifetime = null, Color? startColor = null, GlobalSparksDoer.SparksType systemType = GlobalSparksDoer.SparksType.SPARKS_ADDITIVE_DEFAULT)
	{
		for (int i = 0; i < num; i++)
		{
			Vector3 vector = new Vector3(UnityEngine.Random.Range(minPosition.x, maxPosition.x), UnityEngine.Random.Range(minPosition.y, maxPosition.y), UnityEngine.Random.Range(minPosition.z, maxPosition.z));
			Vector3 vector2 = vector - (maxPosition + minPosition) / 2f;
			Vector3 vector3 = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-angleVariance, angleVariance)) * (vector2.normalized * UnityEngine.Random.Range(baseMagnitude - magnitudeVariance, vector2.magnitude + magnitudeVariance));
			GlobalSparksDoer.DoSingleParticle(vector, vector3, startSize, startLifetime, startColor, systemType);
		}
	}

	// Token: 0x06006AAF RID: 27311 RVA: 0x0029D4C0 File Offset: 0x0029B6C0
	public static void EmitFromRegion(GlobalSparksDoer.EmitRegionStyle emitStyle, float numPerSecond, float duration, Vector3 minPosition, Vector3 maxPosition, Vector3 direction, float angleVariance, float magnitudeVariance, float? startSize = null, float? startLifetime = null, Color? startColor = null, GlobalSparksDoer.SparksType systemType = GlobalSparksDoer.SparksType.SPARKS_ADDITIVE_DEFAULT)
	{
		GameUIRoot.Instance.StartCoroutine(GlobalSparksDoer.HandleEmitFromRegion(emitStyle, numPerSecond, duration, minPosition, maxPosition, direction, angleVariance, magnitudeVariance, startSize, startLifetime, startColor, systemType));
	}

	// Token: 0x06006AB0 RID: 27312 RVA: 0x0029D4F4 File Offset: 0x0029B6F4
	private static IEnumerator HandleEmitFromRegion(GlobalSparksDoer.EmitRegionStyle emitStyle, float numPerSecond, float duration, Vector3 minPosition, Vector3 maxPosition, Vector3 direction, float angleVariance, float magnitudeVariance, float? startSize = null, float? startLifetime = null, Color? startColor = null, GlobalSparksDoer.SparksType systemType = GlobalSparksDoer.SparksType.SPARKS_ADDITIVE_DEFAULT)
	{
		float elapsed = 0f;
		float numReqToSpawn = 0f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			numReqToSpawn += numPerSecond * BraveTime.DeltaTime;
			if (numReqToSpawn > 1f)
			{
				int num = Mathf.FloorToInt(numReqToSpawn);
				if (emitStyle != GlobalSparksDoer.EmitRegionStyle.RANDOM)
				{
					if (emitStyle == GlobalSparksDoer.EmitRegionStyle.RADIAL)
					{
						GlobalSparksDoer.DoRadialParticleBurst(num, minPosition, maxPosition, angleVariance, direction.magnitude, magnitudeVariance, startSize, startLifetime, startColor, systemType);
					}
				}
				else
				{
					GlobalSparksDoer.DoRandomParticleBurst(num, minPosition, maxPosition, direction, angleVariance, magnitudeVariance, startSize, startLifetime, startColor, systemType);
				}
			}
			numReqToSpawn %= 1f;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006AB1 RID: 27313 RVA: 0x0029D564 File Offset: 0x0029B764
	private static ParticleSystem InitializeParticles(GlobalSparksDoer.SparksType targetType)
	{
		switch (targetType)
		{
		case GlobalSparksDoer.SparksType.SPARKS_ADDITIVE_DEFAULT:
			GlobalSparksDoer.m_particles = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/SparkSystem"), Vector3.zero, Quaternion.identity)).GetComponent<ParticleSystem>();
			return GlobalSparksDoer.m_particles;
		case GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE:
			GlobalSparksDoer.m_phantomParticles = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/PhantomSystem"), Vector3.zero, Quaternion.identity)).GetComponent<ParticleSystem>();
			return GlobalSparksDoer.m_phantomParticles;
		case GlobalSparksDoer.SparksType.FLOATY_CHAFF:
			GlobalSparksDoer.m_chaffParticles = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/ChaffSystem"), Vector3.zero, Quaternion.identity)).GetComponent<ParticleSystem>();
			return GlobalSparksDoer.m_chaffParticles;
		case GlobalSparksDoer.SparksType.SOLID_SPARKLES:
			GlobalSparksDoer.m_sparkleParticles = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/SparklesSystem"), Vector3.zero, Quaternion.identity)).GetComponent<ParticleSystem>();
			return GlobalSparksDoer.m_sparkleParticles;
		case GlobalSparksDoer.SparksType.EMBERS_SWIRLING:
			GlobalSparksDoer.EmberParticles = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/EmberSystem"), Vector3.zero, Quaternion.identity)).GetComponent<ParticleSystem>();
			return GlobalSparksDoer.EmberParticles;
		case GlobalSparksDoer.SparksType.STRAIGHT_UP_FIRE:
			GlobalSparksDoer.m_fireParticles = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/GlobalFireSystem"), Vector3.zero, Quaternion.identity)).GetComponent<ParticleSystem>();
			return GlobalSparksDoer.m_fireParticles;
		case GlobalSparksDoer.SparksType.DARK_MAGICKS:
			GlobalSparksDoer.m_darkMagicParticles = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/DarkMagicSystem"), Vector3.zero, Quaternion.identity)).GetComponent<ParticleSystem>();
			return GlobalSparksDoer.m_darkMagicParticles;
		case GlobalSparksDoer.SparksType.BLOODY_BLOOD:
			GlobalSparksDoer.m_bloodParticles = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/BloodSystem"), Vector3.zero, Quaternion.identity)).GetComponent<ParticleSystem>();
			return GlobalSparksDoer.m_bloodParticles;
		case GlobalSparksDoer.SparksType.STRAIGHT_UP_GREEN_FIRE:
			GlobalSparksDoer.m_greenFireParticles = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/GlobalGreenFireSystem"), Vector2.zero, Quaternion.identity)).GetComponent<ParticleSystem>();
			return GlobalSparksDoer.m_greenFireParticles;
		case GlobalSparksDoer.SparksType.RED_MATTER:
			GlobalSparksDoer.m_redMatterParticles = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/GlobalRedMatterSystem"), Vector2.zero, Quaternion.identity)).GetComponent<ParticleSystem>();
			return GlobalSparksDoer.m_redMatterParticles;
		default:
			return GlobalSparksDoer.m_particles;
		}
	}

	// Token: 0x06006AB2 RID: 27314 RVA: 0x0029D780 File Offset: 0x0029B980
	public static void CleanupOnSceneTransition()
	{
		GlobalSparksDoer.m_particles = null;
		GlobalSparksDoer.m_phantomParticles = null;
		GlobalSparksDoer.m_chaffParticles = null;
		GlobalSparksDoer.m_sparkleParticles = null;
		GlobalSparksDoer.m_fireParticles = null;
		GlobalSparksDoer.m_darkMagicParticles = null;
		GlobalSparksDoer.m_bloodParticles = null;
		GlobalSparksDoer.EmberParticles = null;
		GlobalSparksDoer.m_greenFireParticles = null;
		GlobalSparksDoer.m_redMatterParticles = null;
	}

	// Token: 0x04006730 RID: 26416
	private static ParticleSystem m_particles;

	// Token: 0x04006731 RID: 26417
	private static ParticleSystem m_phantomParticles;

	// Token: 0x04006732 RID: 26418
	private static ParticleSystem m_chaffParticles;

	// Token: 0x04006733 RID: 26419
	private static ParticleSystem m_sparkleParticles;

	// Token: 0x04006734 RID: 26420
	private static ParticleSystem m_fireParticles;

	// Token: 0x04006735 RID: 26421
	private static ParticleSystem m_darkMagicParticles;

	// Token: 0x04006736 RID: 26422
	public static ParticleSystem EmberParticles;

	// Token: 0x04006737 RID: 26423
	private static ParticleSystem m_bloodParticles;

	// Token: 0x04006738 RID: 26424
	private static ParticleSystem m_greenFireParticles;

	// Token: 0x04006739 RID: 26425
	private static ParticleSystem m_redMatterParticles;

	// Token: 0x0200129F RID: 4767
	public enum EmitRegionStyle
	{
		// Token: 0x0400673B RID: 26427
		RANDOM,
		// Token: 0x0400673C RID: 26428
		RADIAL
	}

	// Token: 0x020012A0 RID: 4768
	public enum SparksType
	{
		// Token: 0x0400673E RID: 26430
		SPARKS_ADDITIVE_DEFAULT,
		// Token: 0x0400673F RID: 26431
		BLACK_PHANTOM_SMOKE,
		// Token: 0x04006740 RID: 26432
		FLOATY_CHAFF,
		// Token: 0x04006741 RID: 26433
		SOLID_SPARKLES,
		// Token: 0x04006742 RID: 26434
		EMBERS_SWIRLING,
		// Token: 0x04006743 RID: 26435
		STRAIGHT_UP_FIRE,
		// Token: 0x04006744 RID: 26436
		DARK_MAGICKS,
		// Token: 0x04006745 RID: 26437
		BLOODY_BLOOD,
		// Token: 0x04006746 RID: 26438
		STRAIGHT_UP_GREEN_FIRE,
		// Token: 0x04006747 RID: 26439
		RED_MATTER
	}
}
