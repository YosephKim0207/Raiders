using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001352 RID: 4946
public class BlackHoleDoer : SpawnObjectItem
{
	// Token: 0x0600702E RID: 28718 RVA: 0x002C7860 File Offset: 0x002C5A60
	private void Start()
	{
		this.m_distortMaterial = new Material(ShaderCache.Acquire("Brave/Internal/DistortionRadius"));
		this.m_distortMaterial.SetFloat("_Strength", this.distortStrength);
		this.m_distortMaterial.SetFloat("_TimePulse", this.distortTimeScale);
		this.m_distortMaterial.SetFloat("_RadiusFactor", this.distortRadiusFactor);
		this.m_distortMaterial.SetVector("_WaveCenter", this.GetCenterPointInScreenUV(base.sprite.WorldCenter));
		Pixelator.Instance.RegisterAdditionalRenderPass(this.m_distortMaterial);
		this.m_radiusSquared = this.radius * this.radius;
		this.m_currentPhase = 0;
		this.m_currentPhaseInitiated = false;
		this.m_currentPhaseTimer = -1000f;
		if (this.HasHellSynergy && this.SpawningPlayer.HasActiveBonusSynergy(this.HellSynergy, false))
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.HellSynergyVFX, base.transform.position + new Vector3(0f, -0.5f, 0.5f), Quaternion.Euler(45f, 0f, 0f), base.transform);
			MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
			this.radius *= 2f;
			this.damageRadius *= 4f;
			this.gravitationalForceActors *= 4f;
			this.damageToEnemiesPerSecond *= 3f;
			base.StartCoroutine(this.HoldPortalOpen(component));
		}
	}

	// Token: 0x0600702F RID: 28719 RVA: 0x002C79EC File Offset: 0x002C5BEC
	private IEnumerator HoldPortalOpen(MeshRenderer portal)
	{
		portal.material.SetFloat("_UVDistCutoff", 0f);
		yield return new WaitForSeconds(this.introDuration);
		float elapsed = 0f;
		float duration = this.coreDuration;
		float t = 0f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			t = Mathf.Clamp01(elapsed / 0.25f);
			portal.material.SetFloat("_UVDistCutoff", Mathf.Lerp(0f, 0.21f, t));
			yield return null;
		}
		yield break;
	}

	// Token: 0x06007030 RID: 28720 RVA: 0x002C7A10 File Offset: 0x002C5C10
	private Vector4 GetCenterPointInScreenUV(Vector2 centerPoint)
	{
		Vector3 vector = GameManager.Instance.MainCameraController.Camera.WorldToViewportPoint(centerPoint.ToVector3ZUp(0f));
		return new Vector4(vector.x, vector.y, 0f, 0f);
	}

	// Token: 0x06007031 RID: 28721 RVA: 0x002C7A5C File Offset: 0x002C5C5C
	private float GetDistanceToRigidbody(SpeculativeRigidbody other)
	{
		return Vector2.Distance(other.UnitCenter, base.specRigidbody.UnitCenter);
	}

	// Token: 0x06007032 RID: 28722 RVA: 0x002C7A74 File Offset: 0x002C5C74
	private Vector2 GetFrameAccelerationForRigidbody(Vector2 unitCenter, float currentDistance, float g)
	{
		Vector2 zero = Vector2.zero;
		float num = Mathf.Clamp01(1f - currentDistance / this.radius);
		float num2 = g * num * num;
		Vector2 normalized = (base.specRigidbody.UnitCenter - unitCenter).normalized;
		return normalized * num2;
	}

	// Token: 0x06007033 RID: 28723 RVA: 0x002C7AC8 File Offset: 0x002C5CC8
	private bool AdjustDebrisVelocity(DebrisObject debris)
	{
		if (debris.IsPickupObject)
		{
			return false;
		}
		if (debris.GetComponent<BlackHoleDoer>() != null)
		{
			return false;
		}
		Vector2 vector = debris.sprite.WorldCenter - base.specRigidbody.UnitCenter;
		float num = Vector2.SqrMagnitude(vector);
		if (num >= this.m_radiusSquared)
		{
			return false;
		}
		float num2 = this.gravitationalForceActors;
		float num3 = Mathf.Sqrt(num);
		if (num3 < this.damageRadius)
		{
			UnityEngine.Object.Destroy(debris.gameObject);
			return true;
		}
		Vector2 frameAccelerationForRigidbody = this.GetFrameAccelerationForRigidbody(debris.sprite.WorldCenter, num3, num2);
		float num4 = Mathf.Clamp(BraveTime.DeltaTime, 0f, 0.02f);
		if (debris.HasBeenTriggered)
		{
			debris.ApplyVelocity(frameAccelerationForRigidbody * num4);
		}
		else if (num3 < this.radius / 2f)
		{
			debris.Trigger(frameAccelerationForRigidbody * num4, 0.5f, 1f);
		}
		return true;
	}

	// Token: 0x06007034 RID: 28724 RVA: 0x002C7BC8 File Offset: 0x002C5DC8
	private bool AdjustRigidbodyVelocity(SpeculativeRigidbody other)
	{
		Vector2 vector = other.UnitCenter - base.specRigidbody.UnitCenter;
		float num = Vector2.SqrMagnitude(vector);
		if (num < this.m_radiusSquared)
		{
			float num2 = this.gravitationalForce;
			Vector2 velocity = other.Velocity;
			Projectile projectile = other.projectile;
			if (projectile)
			{
				projectile.collidesWithPlayer = false;
				if (other.GetComponent<BlackHoleDoer>() != null)
				{
					return false;
				}
				if (velocity == Vector2.zero)
				{
					return false;
				}
				if (num < 4f && (this.destroysBullets || this.m_cachedOuterLimitsSynergy))
				{
					if (projectile.GetComponent<BecomeOrbitProjectileModifier>())
					{
						this.m_planetsEaten++;
					}
					projectile.DieInAir(false, true, true, false);
					if (this.m_planetsEaten > 2)
					{
						if (this.OuterLimitsVFX)
						{
							GameObject gameObject = SpawnManager.SpawnVFX(this.OuterLimitsVFX, false);
							gameObject.GetComponent<tk2dBaseSprite>().PlaceAtPositionByAnchor(base.transform.position.XY(), tk2dBaseSprite.Anchor.MiddleCenter);
						}
						RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
						absoluteRoom.ApplyActionToNearbyEnemies(base.transform.position.XY(), 100f, new Action<AIActor, float>(this.OuterLimitsProcessEnemy));
						AkSoundEngine.PostEvent("Stop_SND_OBJ", base.gameObject);
						AkSoundEngine.PostEvent("Play_WPN_blackhole_impact_01", base.gameObject);
						AkSoundEngine.PostEvent("Play_OBJ_lightning_flash_01", base.gameObject);
						UnityEngine.Object.Destroy(base.gameObject);
					}
				}
				num2 = this.gravitationalForce;
			}
			else
			{
				if (!other.aiActor)
				{
					return false;
				}
				num2 = this.gravitationalForceActors;
				if (!other.aiActor.enabled)
				{
					return false;
				}
				if (!other.aiActor.HasBeenEngaged)
				{
					return false;
				}
				if (BraveMathCollege.DistToRectangle(base.specRigidbody.UnitCenter, other.UnitBottomLeft, other.UnitDimensions) < this.damageRadius)
				{
					other.healthHaver.ApplyDamage(this.damageToEnemiesPerSecond * BraveTime.DeltaTime, vector.normalized, string.Empty, CoreDamageTypes.None, DamageCategory.DamageOverTime, false, null, false);
				}
				if (other.healthHaver.IsBoss)
				{
					return false;
				}
			}
			Vector2 frameAccelerationForRigidbody = this.GetFrameAccelerationForRigidbody(other.UnitCenter, Mathf.Sqrt(num), num2);
			float num3 = Mathf.Clamp(BraveTime.DeltaTime, 0f, 0.02f);
			Vector2 vector2 = frameAccelerationForRigidbody * num3;
			Vector2 vector3 = velocity + vector2;
			if (BraveTime.DeltaTime > 0.02f)
			{
				vector3 *= 0.02f / BraveTime.DeltaTime;
			}
			other.Velocity = vector3;
			if (projectile != null)
			{
				projectile.collidesWithPlayer = false;
				if (projectile.IsBulletScript)
				{
					projectile.RemoveBulletScriptControl();
				}
				if (vector3 != Vector2.zero)
				{
					projectile.Direction = vector3.normalized;
					projectile.Speed = Mathf.Max(3f, vector3.magnitude);
					other.Velocity = projectile.Direction * projectile.Speed;
					if (projectile.shouldRotate && (vector3.x != 0f || vector3.y != 0f))
					{
						float num4 = BraveMathCollege.Atan2Degrees(projectile.Direction);
						if (!float.IsNaN(num4) && !float.IsInfinity(num4))
						{
							Quaternion quaternion = Quaternion.Euler(0f, 0f, num4);
							if (!float.IsNaN(quaternion.x) && !float.IsNaN(quaternion.y))
							{
								projectile.transform.rotation = quaternion;
							}
						}
					}
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06007035 RID: 28725 RVA: 0x002C7F8C File Offset: 0x002C618C
	public void OuterLimitsProcessEnemy(AIActor a, float b)
	{
		if (a && a.IsNormalEnemy && a.healthHaver && !a.IsGone)
		{
			a.healthHaver.ApplyDamage(100f, Vector2.zero, "projectile", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			if (this.OuterLimitsDamageVFX != null)
			{
				a.PlayEffectOnActor(this.OuterLimitsDamageVFX, Vector3.zero, false, true, false);
			}
		}
	}

	// Token: 0x06007036 RID: 28726 RVA: 0x002C8010 File Offset: 0x002C6210
	private void LateUpdate()
	{
		this.m_elapsed += BraveTime.DeltaTime;
		this.m_cachedOuterLimitsSynergy = GameManager.Instance.BestActivePlayer && GameManager.Instance.BestActivePlayer.HasActiveBonusSynergy(CustomSynergyType.OUTER_LIMITS, false);
		if (this && base.projectile && this.m_elapsed > 9f)
		{
			base.projectile.DieInAir(false, true, true, false);
			return;
		}
		if (this.m_distortMaterial != null)
		{
			this.m_distortMaterial.SetVector("_WaveCenter", this.GetCenterPointInScreenUV(base.sprite.WorldCenter));
		}
		switch (this.m_currentPhase)
		{
		case 0:
			this.LateUpdateIntro();
			break;
		case 1:
			this.LateUpdateCore();
			break;
		case 2:
			this.LateUpdateOutro();
			break;
		default:
			Debug.LogError("Invalid State in BlackHoleDoer: " + this.m_currentPhase.ToString());
			break;
		}
	}

	// Token: 0x06007037 RID: 28727 RVA: 0x002C8138 File Offset: 0x002C6338
	private void LateUpdateIntro()
	{
		if (this.introStyle == BlackHoleDoer.BlackHoleIntroStyle.Instant)
		{
			this.m_currentPhase = 1;
		}
		else if (this.introStyle == BlackHoleDoer.BlackHoleIntroStyle.Gradual)
		{
			if (!this.m_currentPhaseInitiated)
			{
				this.m_currentPhaseInitiated = true;
				this.m_currentPhaseTimer = this.introDuration;
			}
			else if (this.m_currentPhaseTimer > 0f)
			{
				this.m_currentPhaseTimer -= BraveTime.DeltaTime;
			}
			else
			{
				this.m_currentPhase = 1;
				this.m_currentPhaseInitiated = false;
				this.m_currentPhaseTimer = -1000f;
			}
		}
	}

	// Token: 0x06007038 RID: 28728 RVA: 0x002C81CC File Offset: 0x002C63CC
	private void LateUpdateCore()
	{
		if (!this.m_currentPhaseInitiated)
		{
			this.m_currentPhaseInitiated = true;
			this.m_currentPhaseTimer = this.coreDuration;
		}
		else if (this.m_currentPhaseTimer > 0f)
		{
			this.m_currentPhaseTimer -= BraveTime.DeltaTime;
			for (int i = 0; i < PhysicsEngine.Instance.AllRigidbodies.Count; i++)
			{
				if (PhysicsEngine.Instance.AllRigidbodies[i].gameObject.activeSelf)
				{
					if (PhysicsEngine.Instance.AllRigidbodies[i].enabled)
					{
						this.AdjustRigidbodyVelocity(PhysicsEngine.Instance.AllRigidbodies[i]);
					}
				}
			}
			for (int j = 0; j < StaticReferenceManager.AllDebris.Count; j++)
			{
				this.AdjustDebrisVelocity(StaticReferenceManager.AllDebris[j]);
			}
		}
		else
		{
			this.m_currentPhase = 2;
			this.m_currentPhaseInitiated = false;
			this.m_currentPhaseTimer = -1000f;
		}
	}

	// Token: 0x06007039 RID: 28729 RVA: 0x002C82E4 File Offset: 0x002C64E4
	private void LateUpdateOutro()
	{
		BlackHoleDoer.BlackHoleOutroStyle blackHoleOutroStyle = this.outroStyle;
		if (blackHoleOutroStyle != BlackHoleDoer.BlackHoleOutroStyle.FadeAway)
		{
			if (blackHoleOutroStyle == BlackHoleDoer.BlackHoleOutroStyle.Nova)
			{
				this.LateUpdateOutro_Nova();
			}
		}
		else
		{
			this.LateUpdateOutro_Fade();
		}
	}

	// Token: 0x0600703A RID: 28730 RVA: 0x002C8320 File Offset: 0x002C6520
	private void LateUpdateOutro_Fade()
	{
		if (!this.m_currentPhaseInitiated)
		{
			this.m_currentPhaseInitiated = true;
			this.m_currentPhaseTimer = this.outroDuration;
			this.m_fadeStartDistortStrength = this.m_distortMaterial.GetFloat("_Strength");
			tk2dSpriteAnimationClip clipByName = base.spriteAnimator.GetClipByName("black_hole_out_item_vfx");
			this.outroDuration = clipByName.BaseClipLength;
			base.spriteAnimator.PlayAndDestroyObject("black_hole_out_item_vfx", null);
		}
		else if (this.m_currentPhaseTimer > 0f)
		{
			this.m_currentPhaseTimer -= BraveTime.DeltaTime;
			float num = 1f - this.m_currentPhaseTimer / this.outroDuration;
			if (this.m_distortMaterial != null)
			{
				this.m_distortMaterial.SetFloat("_Strength", Mathf.Lerp(this.m_fadeStartDistortStrength, 0f, num));
			}
		}
	}

	// Token: 0x0600703B RID: 28731 RVA: 0x002C8404 File Offset: 0x002C6604
	private void LateUpdateOutro_Nova()
	{
		if (!this.m_currentPhaseInitiated)
		{
			this.m_currentPhaseInitiated = true;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600703C RID: 28732 RVA: 0x002C8428 File Offset: 0x002C6628
	protected override void OnDestroy()
	{
		if (Pixelator.Instance != null)
		{
			Pixelator.Instance.DeregisterAdditionalRenderPass(this.m_distortMaterial);
		}
		base.OnDestroy();
	}

	// Token: 0x04006F85 RID: 28549
	[Header("Intro Settings")]
	public BlackHoleDoer.BlackHoleIntroStyle introStyle;

	// Token: 0x04006F86 RID: 28550
	[ShowInInspectorIf("introStyle", 0, false)]
	public float introDuration = 0.5f;

	// Token: 0x04006F87 RID: 28551
	[Header("Core Settings")]
	public float coreDuration = 5f;

	// Token: 0x04006F88 RID: 28552
	public float damageRadius = 0.5f;

	// Token: 0x04006F89 RID: 28553
	public float radius = 15f;

	// Token: 0x04006F8A RID: 28554
	public float gravitationalForce = 10f;

	// Token: 0x04006F8B RID: 28555
	public float gravitationalForceActors = 50f;

	// Token: 0x04006F8C RID: 28556
	public bool affectsBullets = true;

	// Token: 0x04006F8D RID: 28557
	public bool destroysBullets = true;

	// Token: 0x04006F8E RID: 28558
	public bool affectsDebris = true;

	// Token: 0x04006F8F RID: 28559
	public bool destroysDebris = true;

	// Token: 0x04006F90 RID: 28560
	public bool affectsEnemies = true;

	// Token: 0x04006F91 RID: 28561
	public float damageToEnemiesPerSecond = 30f;

	// Token: 0x04006F92 RID: 28562
	public bool affectsPlayer;

	// Token: 0x04006F93 RID: 28563
	public float damageToPlayerPerSecond;

	// Token: 0x04006F94 RID: 28564
	[Header("Outro Settings")]
	public BlackHoleDoer.BlackHoleOutroStyle outroStyle;

	// Token: 0x04006F95 RID: 28565
	[ShowInInspectorIf("outroStyle", 0, false)]
	public float outroDuration = 0.5f;

	// Token: 0x04006F96 RID: 28566
	[ShowInInspectorIf("outroStyle", 1, false)]
	public float novaForce = 50f;

	// Token: 0x04006F97 RID: 28567
	public float distortStrength = 0.01f;

	// Token: 0x04006F98 RID: 28568
	public float distortTimeScale = 0.5f;

	// Token: 0x04006F99 RID: 28569
	public float distortRadiusFactor = 1f;

	// Token: 0x04006F9A RID: 28570
	private int m_currentPhase;

	// Token: 0x04006F9B RID: 28571
	private bool m_currentPhaseInitiated;

	// Token: 0x04006F9C RID: 28572
	private float m_currentPhaseTimer = -1000f;

	// Token: 0x04006F9D RID: 28573
	private float m_radiusSquared;

	// Token: 0x04006F9E RID: 28574
	[Header("Synergy Settings")]
	public bool HasHellSynergy;

	// Token: 0x04006F9F RID: 28575
	[LongNumericEnum]
	public CustomSynergyType HellSynergy;

	// Token: 0x04006FA0 RID: 28576
	public GameObject HellSynergyVFX;

	// Token: 0x04006FA1 RID: 28577
	public GameObject OuterLimitsVFX;

	// Token: 0x04006FA2 RID: 28578
	public GameObject OuterLimitsDamageVFX;

	// Token: 0x04006FA3 RID: 28579
	private bool m_cachedOuterLimitsSynergy;

	// Token: 0x04006FA4 RID: 28580
	private int m_planetsEaten;

	// Token: 0x04006FA5 RID: 28581
	private float m_elapsed;

	// Token: 0x04006FA6 RID: 28582
	private float m_fadeStartDistortStrength;

	// Token: 0x04006FA7 RID: 28583
	private Material m_distortMaterial;

	// Token: 0x02001353 RID: 4947
	public enum BlackHoleIntroStyle
	{
		// Token: 0x04006FA9 RID: 28585
		Gradual,
		// Token: 0x04006FAA RID: 28586
		Instant
	}

	// Token: 0x02001354 RID: 4948
	public enum BlackHoleOutroStyle
	{
		// Token: 0x04006FAC RID: 28588
		FadeAway,
		// Token: 0x04006FAD RID: 28589
		Nova
	}
}
